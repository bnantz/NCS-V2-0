using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace NCS.DataSetUtils
{
	/// <summary>
	/// Largely based upon MS knowledgebase article found at:
	/// http://support.microsoft.com/default.aspx?scid=kb;en-us;326145
	/// </summary>
   public class DataSetHelper
   {
		

      private DataSet _ds;
      private ArrayList _FieldInfo;
      private string _FieldList;
      private ArrayList _GroupByFieldInfo;
      private string _GroupByFieldList;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="DataSet">The Dataset to perform opperations on</param>
      public DataSetHelper(ref DataSet DataSet)
      {
         this._ds = DataSet;
      }


      /// <summary>
      /// Default Constructor
      /// </summary>
      public DataSetHelper()
      {
         this._ds = null;
      }


      #region internal methods

      private void ParseFieldList(string FieldList, bool AllowRelation)
      {
         /*
          * This code parses FieldList into FieldInfo objects  and then
          * adds them to the this._FieldInfo private member
          *
          * FieldList systax:  [relationname.]fieldname[;alias], ...
         */
         if (this._FieldList == FieldList)
         {
            return;
         }

         this._FieldInfo = new ArrayList();
         this._FieldList = FieldList;

         FieldInfo Field;
         string[] FieldParts;
         string[] Fields = FieldList.Split(',');

         int i;
         for (i = 0; i <= Fields.Length - 1; i++)
         {
            Field = new FieldInfo();
            //parse FieldAlias
            FieldParts = Fields[i].Trim().Split(';');
            switch (FieldParts.Length)
            {
               case 1:
                  //to be set at the end of the loop
                  break;
               case 2:
                  Field.FieldAlias = FieldParts[1];
                  break;
               default:
                  throw new Exception("Too many semicolons in field definition: '" + Fields[i] + "'.");
            }
            //parse FieldName and RelationName
            FieldParts = FieldParts[0].Split('.');
            switch (FieldParts.Length)
            {
               case 1:
                  Field.FieldName = FieldParts[0];
                  break;
               case 2:
                  if (AllowRelation == false)
                     throw new Exception("Relation specifiers not permitted in field list: '" + Fields[i] + "'.");
                  Field.RelationName = FieldParts[0].Trim();
                  Field.FieldName = FieldParts[1].Trim();
                  break;
               default:
                  throw new Exception("Invalid field definition: " + Fields[i] + "'.");
            }
            if (Field.FieldAlias == null)
               Field.FieldAlias = Field.FieldName;
            this._FieldInfo.Add(Field);
         }
      }

      private void ParseGroupByFieldList(string FieldList)
      {
         /*
         * Parses FieldList into FieldInfo objects and adds them to the this._GroupByFieldInfo private member
         *
         * FieldList syntax: fieldname[;alias]|operatorname(fieldname)[;alias],...
         *
         * Supported Operators: count,sum,max,min,first,last
         */
         if (this._GroupByFieldList == FieldList) return;
         this._GroupByFieldInfo = new ArrayList();
         FieldInfo Field;
         string[] FieldParts;
         string[] Fields = FieldList.Split(',');
         for (int i = 0; i <= Fields.Length - 1; i++)
         {
            Field = new FieldInfo();
            //Parse FieldAlias
            FieldParts = Fields[i].Trim().Split(';');
            switch (FieldParts.Length)
            {
               case 1:
                  //to be set at the end of the loop
                  break;
               case 2:
                  Field.FieldAlias = FieldParts[1];
                  break;
               default:
                  throw new ArgumentException("Too many semicolons in field definition: '" + Fields[i] + "'.");
            }
            //Parse FieldName and Aggregate

            // note that having parentheses in a field name _might_ mean that it
            // names an aggregate function but it isn't necessarily an aggregate function;
            // it may just be a datacolumn name with parentheses.  This logic checks
            // whether the suspect aggregate function is supported.  If it isn't then
            // it takes the entire field string as the field name
            string[] possibleAggregateFieldParts = FieldParts[0].Split('(');

            if (possibleAggregateFieldParts.Length == 1)
            {
               Field.FieldName = possibleAggregateFieldParts[0];
            }
            else if (possibleAggregateFieldParts.Length == 2)
            {
               if (isSupportedOperator(possibleAggregateFieldParts[0]))
               {
                  Field.Aggregate = possibleAggregateFieldParts[0].Trim().ToLower(); //we're doing a case-sensitive comparison later
                  Field.FieldName = possibleAggregateFieldParts[1].Trim(' ', ')');						
               }
               else
               {
                  Field.FieldName = FieldParts[0]; // take the entire original string as the column name
               }
            }
            else
            {
               throw new ArgumentException("Invalid field definition: '" + Fields[i] + "'.");					
            }

            if (Field.FieldAlias == null)
            {
               if (Field.Aggregate == null)
                  Field.FieldAlias = Field.FieldName;
               else
                  Field.FieldAlias = Field.Aggregate + "of" + Field.FieldName;
            }
            this._GroupByFieldInfo.Add(Field);
         }
         this._GroupByFieldList = FieldList;
      }

      private bool isSupportedOperator(string aggregateOperator)
      {
         string spiffedOperator = aggregateOperator.Trim().ToLower();

         return	(		("count".Equals(spiffedOperator))
            ||	("sum".Equals(spiffedOperator))
            ||	("max".Equals(spiffedOperator))
            ||	("min".Equals(spiffedOperator))
            ||	("first".Equals(spiffedOperator))
            ||	("last".Equals(spiffedOperator))
            );
      }

      private bool ColumnEqual(object a, object b)
      {
         /*
          * Compares two values to see if they are equal. Also compares DBNULL.Value.
          *
          * Note: If your DataTable contains object fields, you must extend this
          * function to handle them in a meaningful way if you intend to group on them.
         */
         if ((a is DBNull) && (b is DBNull))
            return true; //both are null
         if ((a is DBNull) || (b is DBNull))
            return false; //only one is null

         bool retStatus = (a.Equals(b));
         return retStatus; //value type standard comparison
      }

      private object Min(object a, object b)
      {
         //Returns MIN of two values - DBNull is less than all others
         if ((a is DBNull) || (b is DBNull))
            return DBNull.Value;
         if (((IComparable) a).CompareTo(b) == -1)
            return a;
         else
            return b;
      }

      private object Max(object a, object b)
      {
         //Returns Max of two values - DBNull is less than all others
         if (a is DBNull)
            return b;
         if (b is DBNull)
            return a;
         if (((IComparable) a).CompareTo(b) == 1)
            return a;
         else
            return b;
      }

      private object Add(object a, object b)
      {
         bool aIsValid = false;
         bool bIsValid = false;

         Double doubleA = 0.0;
         Double doubleB = 0.0;

         if (!(a is DBNull))
         {
            try
            {
               doubleA = Convert.ToDouble(a);
               aIsValid = true;
            }
            catch (FormatException)
            {
               aIsValid = false;
            }
         }

         if (!(b is DBNull))
         {
            try
            {
               doubleB = Convert.ToDouble(b);
               bIsValid = true;
            }
            catch (FormatException)
            {
               bIsValid = false;
            }
         }
         //Adds two values - if one is DBNull, then returns the other
         if ((!aIsValid) && (bIsValid))
         {
            return b;
         }
         if ((aIsValid) && (!bIsValid))
         {
            return a;
         }

         return (doubleA + doubleB);
      }

      #endregion

      #region child columns have same value

      /// <summary>
      /// Answers whether the data in the column named columnName in the child table all have the same value.
      /// </summary>
      /// <param name="ds">The Dataset</param>
      /// <param name="relationName">The index of the parent row which is being considered for this check.</param>
      /// <param name="parentRowIndex">The name of the relation between the parent and child table.  </param>
      /// <param name="columnName">The DataColumn to look for</param>
      /// <returns></returns>
      public static bool columnInChildRowsHaveSameValue(DataSet ds, string relationName, int parentRowIndex,
         string columnName)
      {
         // by default, don't ignore null cells
         return columnInChildRowsHaveSameValue(ds, relationName, parentRowIndex, columnName, false);			
      }
		
      /// <summary>
      /// Answers whether the data in the column named columnName in the child table all have the same value.
      /// </summary>
      /// <param name="ds">The Dataset</param>
      /// <param name="relationName">The index of the parent row which is being considered for this check.</param>
      /// <param name="parentRowIndex">The name of the relation between the parent and child table.  </param>
      /// <param name="columnName">The DataColumn to look for</param>
      /// <param name="ignoreNullCells">Should we ignore NULL cells</param>
      /// <returns></returns>
      public static bool columnInChildRowsHaveSameValue(DataSet ds, string relationName, int parentRowIndex,
         string columnName, bool ignoreNullCells)
      {
         bool haveSameValue = true;

         DataRelation dataRelation = ds.Relations[relationName];

         // get the child rows which correspond to the parent row

         DataRow[] childRowsArray = dataRelation.ParentTable.Rows[parentRowIndex].GetChildRows(relationName);

         IEnumerator childRowsCollection = childRowsArray.GetEnumerator();

         if (childRowsArray.Length < 1)
         {
            // special case
            // (from bug 267)
            // it appears that if one tries to group by one or more columns and all child rows happen to have DBNull in the corresponding
            // columns that GetChildRows will not find any matching child rows (when it seems that it should have returned all rows)
            // Under these circumstances we'll try to do a select distinct on the grouping columns and, if we get exactly one data item
            // back, we'll set the childRowsArray equal to all of the rows in the child table.
            // a better solution would be to somehow involve the GetChildRows call directly but I didn't find anything in my
            // web searching which talked about this situation.

            DataSetHelper dsh = new DataSetHelper();

            DataTable distinctValuesTable = dsh.SelectDistinct("distinctChildValues", dataRelation.ChildTable, columnName);

            if (distinctValuesTable.Rows.Count > 0)
            {
               childRowsCollection = dataRelation.ChildTable.Rows.GetEnumerator();  // select everything	
            }
            else
            {
               throw new ApplicationException("No child rows for relation name [" + relationName + "] parent row index [" +
                  parentRowIndex + "]");
            }
         }

         // finally check for matching value of the column within all the child rows.

         object firstCellValue = null;
         bool firstCellValueWasSet = false;

         DataRow childRow;

         while (childRowsCollection.MoveNext())
         {
            childRow = (DataRow) childRowsCollection.Current;

            if (!firstCellValueWasSet)
            {
               if (		( (isNull(childRow[columnName])) && (!ignoreNullCells) )
                  ||	(!isNull(childRow[columnName])))
               {
                  firstCellValue = childRow[columnName];
                  firstCellValueWasSet = true;
               }
            }
            else
            {
               if (isNull(childRow[columnName]))
               {
                  if (!ignoreNullCells)
                  {
                     if (!isNull(firstCellValue))
                     {
                        haveSameValue = false;
                        break;
                     }
                  }
               }
               else
               {
                  if (!firstCellValue.Equals(childRow[columnName]))
                  {
                     haveSameValue = false;
                     break;
                  }
               }
            }
         }

         return haveSameValue;
      }

      private static bool isNull(object o)
      {
         return ( (o == null) || (o.Equals(DBNull.Value)));
      }
      #endregion

      /// <summary>
      /// Creates a table based on aggregates of fields of another table
      /// </summary>
      /// <param name="TableName">The name of the DataTable</param>
      /// <param name="SourceTable">The source DataTable</param>
      /// <param name="FieldList">FieldList syntax: fieldname[;alias]|aggregatefunction(fieldname)[;alias], ...</param>
      /// <returns></returns>
      public DataTable CreateGroupByTable(string TableName, DataTable SourceTable, string FieldList)
      {
         return CreateGroupByTable(TableName, SourceTable.Columns, FieldList);
      }

      /// <summary>
      /// Creates a table based on aggregates of fields of another table
      /// </summary>
      /// <param name="TableName">The name of the DataTable</param>
      /// <param name="dataColumns">Collection of DataColumns</param>
      /// <param name="FieldList">FieldList syntax: fieldname[;alias]|aggregatefunction(fieldname)[;alias], ...</param>
      /// <returns></returns>
      public DataTable CreateGroupByTable(string TableName, DataColumnCollection dataColumns, string FieldList)
      {
         /*
          * Creates a table based on aggregates of fields of another table
          *
          * RowFilter affects rows before GroupBy operation. No "Having" support
          * though this can be emulated by subsequent filtering of the table that results
          *
          *  FieldList syntax: fieldname[;alias]|aggregatefunction(fieldname)[;alias], ...
         */
         if (FieldList == null)
         {
            throw new ArgumentException("You must specify at least one field in the field list.");
            //return CreateTable(TableName, SourceTable);
         }
         else
         {
            DataTable dt = new DataTable(TableName);
            ParseGroupByFieldList(FieldList);
            foreach (FieldInfo Field in this._GroupByFieldInfo)
            {
               DataColumn dc = dataColumns[Field.FieldName];
               checkDataColumnAndFieldReferences(Field, dc);
               if (Field.Aggregate == null)
               {
                  try
                  {
                     dt.Columns.Add(Field.FieldAlias, dc.DataType, dc.Expression);
                  }
                  catch (EvaluateException ee)
                  {
                     Debug.WriteLine(ee.ToString());
                     Debug.WriteLine("Data column [" + dc.ColumnName + "] expression [" + dc.Expression +
                        "] cannot be evaluated; creating a duplicate of it without the expression.");
                     dt.Columns.Add(Field.FieldAlias, dc.DataType);
                  }
               }
               else
                  dt.Columns.Add(Field.FieldAlias, dc.DataType);
            }
            if (this._ds != null)
               this._ds.Tables.Add(dt);
            return dt;
         }
      }


      private void checkDataColumnAndFieldReferences(FieldInfo Field, DataColumn dc)
      {
         if (Field == null)
         {
            throw new ApplicationException("Field reference is null");
         }
         if (Field.FieldName == null)
         {
            throw new ApplicationException("Field name is null");
         }
         if (Field.FieldAlias == null)
         {
            throw new ApplicationException("Field alias is null");
         }
         if (dc == null)
         {
            throw new ApplicationException("Data column references is null for field name [" + Field.FieldName + "].  A column with this name probably doesn't exist in the source table.");
         }
         if (dc.DataType == null)
         {
            throw new ApplicationException("Data column datatype is null.");
         }
         if ( (Field.Aggregate != null) && (dc.Expression == null) )
         {
            throw new ApplicationException("Field aggregate is not null but datacolumn expression is null.");				
         }
      }

      /// <summary>
      /// Copies the selected rows and columns from SourceTable and inserts them into DestTable FieldList has same format as CreateGroupByTable
      /// </summary>
      /// <param name="DestTable">The destination DataTable</param>
      /// <param name="SourceDataView">The Source DataView</param>
      /// <param name="FieldList">FieldList syntax: fieldname[;alias]|aggregatefunction(fieldname)[;alias], ...</param>
      /// <param name="GroupBy">Group by parameter</param>
      public void InsertGroupByInto(DataTable DestTable, DataView SourceDataView, string FieldList,
         string GroupBy)
      {
         InsertGroupByInto(DestTable, SourceDataView.Table, FieldList, SourceDataView.RowFilter, GroupBy);
      }
      /// <summary>
      /// Copies the selected rows and columns from SourceTable and inserts them into DestTable FieldList has same format as CreateGroupByTable
      /// </summary>
      /// <param name="DestTable">The destination DataTable</param>
      /// <param name="SourceTable">The Source DataTable</param>
      /// <param name="FieldList">FieldList syntax: fieldname[;alias]|aggregatefunction(fieldname)[;alias], ...</param>
      /// <param name="RowFilter">The DataRow filter</param>
      /// <param name="GroupBy">Group by parameter</param>
      public void InsertGroupByInto(DataTable DestTable, DataTable SourceTable, string FieldList,
         string RowFilter, string GroupBy)
      {
         /*
          * Copies the selected rows and columns from SourceTable and inserts them into DestTable
          * FieldList has same format as CreateGroupByTable
         */
         if (FieldList == null)
            throw new ArgumentException("You must specify at least one field in the field list.");
         ParseGroupByFieldList(FieldList); //parse field list
         ParseFieldList(GroupBy, false); //parse field names to Group By into an arraylist
         DataRow[] Rows = SourceTable.Select(RowFilter, GroupBy);
         DataRow LastSourceRow = null, DestRow = null;
         bool SameRow;
         int RowCount = 0;
         foreach (DataRow SourceRow in Rows)
         {
            SameRow = false;
            if (LastSourceRow != null)
            {
               SameRow = true;
               foreach (FieldInfo Field in this._FieldInfo)
               {
                  if (!ColumnEqual(LastSourceRow[Field.FieldName], SourceRow[Field.FieldName]))
                  {
                     SameRow = false;
                     break;
                  }
               }
               if (!SameRow)
                  DestTable.Rows.Add(DestRow);
            }
            if (!SameRow)
            {
               DestRow = DestTable.NewRow();
               RowCount = 0;
            }
            RowCount += 1;
            foreach (FieldInfo Field in this._GroupByFieldInfo)
            {
               switch (Field.Aggregate) //this test is case-sensitive
               {
                  case null: //implicit last
                  case "": //implicit last
                  case "last":
                     DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
                     break;
                  case "first":
                     if (RowCount == 1)
                        DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
                     break;
                  case "count":
                     DestRow[Field.FieldAlias] = RowCount;
                     break;
                  case "sum":
                     DestRow[Field.FieldAlias] = Add(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
                     break;
                  case "max":
                     DestRow[Field.FieldAlias] = Max(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
                     break;
                  case "min":
                     if (RowCount == 1)
                        DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
                     else
                        DestRow[Field.FieldAlias] = Min(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
                     break;
               }
            }
            LastSourceRow = SourceRow;
         }
         if (DestRow != null)
            DestTable.Rows.Add(DestRow);
      }


      /// <summary>
      /// Implement a select distinct against a passed datatable.
      /// </summary>
      /// <param name="TableName">The name of the DataTable</param>
      /// <param name="SourceTable">The Source DataTable</param>
      /// <param name="fieldList">FieldList syntax: fieldname[;alias]|aggregatefunction(fieldname)[;alias], ...</param>
      /// <returns></returns>
      public DataTable SelectDistinct(string TableName, DataTable SourceTable, string fieldList)
      {
			
         DataTable distinctValuesTable = CreateGroupByTable("distinctValuesTable", SourceTable, fieldList);

         InsertGroupByInto(distinctValuesTable, SourceTable, fieldList, "", fieldList);

         if (this._ds != null)
         {
            this._ds.Tables.Add(distinctValuesTable);
         }

         return distinctValuesTable;
      }

      /// <summary>
      /// Selects unique values from the Dataview
      /// </summary>
      /// <param name="p_dv">The Source DataView</param>
      /// <param name="p_sCol">The Columns to use</param>
      /// <returns></returns>
      public DataTable SelectDistinct(DataView p_dv, string p_sCol)
      {
         object oLastVal = DBNull.Value;
         DataTable dt = null;
         DataRow row;

         try
         {
            p_dv.Sort = p_sCol;
            dt = new DataTable();
            dt.Columns.Add(p_sCol, p_dv.Table.Columns[p_sCol].DataType);

            foreach (DataRowView oItem in p_dv)
            {
               if (		(oLastVal == null)
                  ||	(DBNull.Value.Equals(oLastVal))
                  ||	(!oLastVal.Equals(oItem[p_sCol]))
                  )
               {
                  oLastVal = oItem[p_sCol];
                  row = dt.NewRow();
                  row[0] = oLastVal;
                  dt.Rows.Add(row);						
               }
            }

            return dt;
         }
         catch (Exception x)
         {
            Debug.WriteLine(x.ToString());
            throw x;
         }
         finally
         {
            if (dt != null)
            {
               dt.Dispose();
               row = null;
            }
         }
      }
		
      /// <summary>
      /// Selects unique values from the Dataview
      /// </summary>
      /// <param name="TableName">The DataTable name</param>
      /// <param name="SourceDataView">The Source DataView</param>
      /// <param name="fieldList">FieldList syntax: fieldname[;alias]|aggregatefunction(fieldname)[;alias], ...</param>
      /// <returns></returns>
      public DataTable SelectDistinct(string TableName, DataView SourceDataView, string fieldList)
      {
         DataTable distinctValuesTable = CreateGroupByTable("distinctValuesTable", SourceDataView.Table.Columns, fieldList);

         InsertGroupByInto(distinctValuesTable, SourceDataView.Table, fieldList, SourceDataView.RowFilter, fieldList);

         if (this._ds != null)
         {
            this._ds.Tables.Add(distinctValuesTable);
         }

         return distinctValuesTable;
      }


      /// <summary>
      /// Selects data from one DataTable to another and performs various aggregate functions along the way. See InsertGroupByInto and ParseGroupByFieldList for supported aggregate functions.
      /// </summary>
      /// <param name="TableName">The name of the DataTable</param>
      /// <param name="SourceTable">The source DataTable</param>
      /// <param name="FieldList">FieldList syntax: fieldname[;alias]|aggregatefunction(fieldname)[;alias], ...</param>
      /// <param name="RowFilter">The Filter expression</param>
      /// <param name="GroupBy">The group by parameter</param>
      /// <returns></returns>
      public DataTable SelectGroupByInto(string TableName, DataTable SourceTable, string FieldList,
         string RowFilter, string GroupBy)
      {
         /*
          * Selects data from one DataTable to another and performs various aggregate functions
          * along the way. See InsertGroupByInto and ParseGroupByFieldList for supported aggregate functions.
          */
         DataTable dt = CreateGroupByTable(TableName, SourceTable, FieldList);
         InsertGroupByInto(dt, SourceTable, FieldList, RowFilter, GroupBy);
         return dt;
      }

		
      private class FieldInfo
      {
         public string RelationName;
         public string FieldName; //source table field name
         public string FieldAlias; //destination table field name
         public string Aggregate;
      }

      /// <summary>
      /// merges two data tables
      /// </summary>
      /// <param name="_srcDT">The source DataTable</param>
      /// <param name="_destDS">The destination DataTable</param>
      /// <param name="destTableName">The Destination Table name</param>
      /// <param name="mergeOnCols">The DataColumns to merge on</param>
      /// <returns>The unmatched rows from the src data table</returns>
      public static DataTable MergeDataSets(DataTable _srcDT, DataSet _destDS, String destTableName, Hashtable mergeOnCols)
      {
         DataColumn[] srcMergeOnCol = new DataColumn[mergeOnCols.Count];
         DataColumn[] destMergeOnCol = new DataColumn[mergeOnCols.Count];

         DataSet src = new DataSet();
         DataTable srcDT = _srcDT.Copy();
         srcDT.TableName = "src";
         src.Tables.Add(srcDT);
         DataTable destDT = _destDS.Tables[destTableName];
         //String origTableName = destDT.TableName;
         //destDT.TableName = "dest";
			

         /*
          * We need to create the columns that will be merged into
          */
         DataSet tmpSrc = new DataSet();
         DataTable tmpSrcDT = srcDT.Clone();
         tmpSrc.Tables.Add(tmpSrcDT);

         foreach ( String srcName in mergeOnCols.Keys )
         {
            tmpSrcDT.Columns.Remove(srcName);	
         }

         foreach ( DataColumn col in destDT.Columns)
         {
            if ( tmpSrcDT.Columns.Contains(col.ColumnName) )
            {
               tmpSrcDT.Columns.Remove(col.ColumnName);
            }
				
         }
			
         tmpSrc.Tables[0].TableName = destDT.TableName;
         _destDS.Merge(tmpSrc,false,MissingSchemaAction.Add);

         _destDS.Merge(srcDT);


         int i = 0;
         foreach (String key in mergeOnCols.Keys)
         {
            srcMergeOnCol[i] = _destDS.Tables[srcDT.TableName].Columns[key];
            destMergeOnCol[i] = destDT.Columns[mergeOnCols[key].ToString()];
            i++;
         }

         DataRelation dataRel = new DataRelation("locRel", destMergeOnCol, srcMergeOnCol, false);
         _destDS.Relations.Add(dataRel);

         foreach (DataRow row in destDT.Rows)
         {
            if ( row.RowState == DataRowState.Deleted)
            {
               continue;
            }
            foreach (DataRow crow in row.GetChildRows(dataRel))
            {
               foreach (DataColumn ccol in crow.Table.Columns)
               {
                  if ( !mergeOnCols.ContainsKey(ccol.ColumnName))
                  {
                     row[ccol.ColumnName] = crow[ccol];
                  }
               }
               crow.Delete();
            }

         }
			
         DataTable ret = _destDS.Tables[srcDT.TableName];
         ret.AcceptChanges();

         _destDS.Relations.Remove(dataRel);
         _destDS.Tables.Remove(srcDT.TableName);
         return ret;
      }


      /// <summary>
      /// Create a valid SQL Select string based on the type of the Keys
      /// </summary>
      /// <param name="row">The SataRow to Select on</param>
      /// <param name="inputKeys">A Hashtable of keys to select</param>
      /// <returns></returns>
      public static string createSelectString(DataRow row, Hashtable inputKeys)
      {
         StringBuilder sb = new StringBuilder("");

         foreach(string key in inputKeys.Keys)
         {
            object o = row[key];
            if(o is Int32 || o is decimal)
               sb.AppendFormat("[" + key + "]" + "=" + row[key].ToString());
            else if(o is string)
            {
               if( row[key].ToString().IndexOf("'") > 0 )
               {
                  string escapedString = row[key].ToString();
                  escapedString = escapedString.Replace("'", @"''");

                  sb.AppendFormat("[" + key + "]" + "='" + escapedString + "'");
               }
               else
               {
                  sb.AppendFormat("[" + key + "]" + "='" + row[key].ToString() + "'");
               }
            }

            sb.AppendFormat(" and ");
         }

         string s = sb.ToString();

         s = s.TrimEnd(new char []{' ', 'a', 'n', 'd', ' '});
         
         return s;
      }
      
      /// <summary>
      /// Duplicate a table within passed dataset into a standalone data table.
      /// </summary>
      /// <param name="ds">The input DataSet</param>
      /// <param name="dataTableName">The name of the DataTable to copy</param>
      /// <returns></returns>
      public static DataTable copyToStandaloneTable(DataSet ds, string dataTableName)
      {
         DataTable srcTable = ds.Tables[dataTableName];

         DataTable detachedTable = ds.Tables[dataTableName].Clone();

         foreach (DataRow srcRow in srcTable.Rows)
         {
            detachedTable.ImportRow(srcRow);	
         }

         return detachedTable;
      }

      /// <summary>
      /// Converts a dataview's rows to a datatable holding those rows.  Preserves filtering
      /// and sorting that was set in the view.
      /// </summary>
      /// <param name="dataView"></param>
      /// <returns></returns>
      public static DataTable viewToTable(DataView dataView)
      {
         if (null == dataView)
         {
            throw new ArgumentNullException
               ("DataView", "Invalid DataView object specified");
         }

         DataTable dataTable = dataView.Table.Clone();
         int idx = 0;
         string [] strColNames = new string[dataTable.Columns.Count];
         foreach (DataColumn col in dataTable.Columns)
         {
            strColNames[idx++] = col.ColumnName;
         }

         IEnumerator viewEnumerator = dataView.GetEnumerator();
         while (viewEnumerator.MoveNext())
         {
            DataRowView drv = (DataRowView)viewEnumerator.Current;
            DataRow dr = dataTable.NewRow();
					
            foreach (string strName in strColNames)
            {
               dr[strName] = drv[strName];
            }

            dataTable.Rows.Add(dr);
         }

         return dataTable;
      }			

      /// <summary>
      /// Transforms a collection of rows to a dataset.
      /// </summary>
      /// <param name="rows"></param>
      /// <returns></returns>
      public static DataSet rowsToDataSet(DataRow [] rows)
      {
         DataSet ds = new DataSet();

         if(rows.Length > 0)
         {
            ds.Tables.Add(rows[0].Table.TableName);
            foreach(DataColumn col in rows[0].Table.Columns)
            {
               DataColumn theCol = new DataColumn();
               theCol.ColumnName = col.ColumnName;
               theCol.DataType = col.DataType;
               theCol.DefaultValue = col.DefaultValue;
               ds.Tables[0].Columns.Add(theCol);
            }

            foreach(DataRow row in rows)
            {
               ds.Tables[0].ImportRow(row);
            }
         }

         return ds;
      }
   }
}

