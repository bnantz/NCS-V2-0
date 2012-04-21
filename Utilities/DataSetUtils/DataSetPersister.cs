using System.Data;
using System.IO;
using System.Text;

namespace NCS.DataSetUtils
{
	/// <summary>
	/// Summary description for DataSetPersister.
	/// </summary>
	public class DataSetPersister
	{
      /// <summary>
      /// Returns the XML representation of the Dataset as a String
      /// </summary>
      /// <param name="ds">The input DataSet</param>
      /// <returns></returns>
      public static string ViewDataSet(DataSet ds)
      {
         StringWriter sw = null;
         StringBuilder sb = new StringBuilder();

         try
         {
            sw = new StringWriter(sb);
            ds.WriteXml(sw, XmlWriteMode.DiffGram);
         }
         catch{}
         finally
         {
            if(sw != null)
               sw.Close();
         }

         return sb.ToString();
      }

      /// <summary>
      /// Writes out the DataSet as an XML file with changes
      /// </summary>
      /// <param name="ds">The DataSource to write out</param>
      /// <param name="filename">The name of the file to write to.</param>
      public static void SaveDataSet(DataSet ds, string filename)
      {
         try
         {
            ds.WriteXml(filename, XmlWriteMode.DiffGram);
         }
         catch{}
      }

      /// <summary>
      /// Loads a DataSet from a XML file
      /// </summary>
      /// <param name="filename">The name of the file to load from.</param>
      /// <returns></returns>
      public static DataSet LoadDataSet(string filename)
      {
         DataSet ds = new DataSet();

         try
         {
            ds.ReadXml(filename, XmlReadMode.DiffGram);
         }
         catch{}

         return ds;
      }

      /// <summary>
      /// Returns the XML representation of the DataTable as a String 
      /// </summary>
      /// <param name="dt">The input DataTable</param>
      /// <returns></returns>
      public static string ViewDataTable(DataTable dt)
      {
         DataSet ds = new DataSet();

         try
         {
            ds.Tables.Add(dt);
         }
         catch{}

         return ViewDataSet(ds);
      }

      /// <summary>
      /// Writes out the DataTable as an XML file with changes
      /// </summary>
      /// <param name="dt">The DataTable to save</param>
      /// <param name="filename">The filename to save to.</param>
      public static void SaveDataTable(DataTable dt, string filename)
      {
         DataSet ds = new DataSet();

         try
         {
            ds.Tables.Add(dt);
            SaveDataSet(ds, filename);
         }
         catch{}
      }

      /// <summary>
      /// Loads a DataTable from a XML file
      /// </summary>
      /// <param name="filename">The name of the file to load from.</param>
      /// <returns></returns>
      public static DataTable LoadDataTable(string filename)
      {
         DataSet ds = new DataSet();
         DataTable dt = new DataTable();

         try
         {
            ds = LoadDataSet(filename);
            dt = ds.Tables[0];
         }
         catch{}

         return dt;
      }

      /// <summary>
      /// Returns the XML representation of the DataView as a String
      /// </summary>
      /// <param name="dv">The input DataView</param>
      /// <returns></returns>
      public static string ViewDataView(DataView dv)
      {
         return ViewDataTable(dv.Table);
      }

      /// <summary>
      /// Writes out the DataView as an XML file with changes
      /// </summary>
      /// <param name="dv">The DataView to save</param>
      /// <param name="filename">The filename to save to.</param>
      public static void SaveDataView(DataView dv, string filename)
      {
         SaveDataTable(dv.Table,  filename);
      }

      /// <summary>
      /// Loads a DataView from a XML file
      /// </summary>
      /// <param name="filename">The name of the file to load from.</param>
      /// <returns></returns>
      public static DataView LoadDataView(string filename)
      {
         DataSet ds = new DataSet();
         DataTable dt = new DataTable();
         DataView dv = new DataView();

         try
         {
            ds = LoadDataSet(filename);
            dt = ds.Tables[0];
            dv.Table = dt;
         }
         catch{}

         return dv;
      }

	}
}
