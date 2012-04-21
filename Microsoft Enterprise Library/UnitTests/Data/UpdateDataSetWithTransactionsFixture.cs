//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Data;
using System.Data.Common;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class UpdateDataSetWithTransactionsFixture : UpdateDataSetStoredProcedureBase
    {
        protected DbTransaction transaction;
        protected bool rollback = true;

        public void ModifyRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, transaction);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }

        public void AttemptToInsertBadRowInsideOfATransaction()
        {
            AddRowsWithErrorsToDataTable(startingData.Tables[0]);
            try
            {
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, transaction);
            }
            catch
            {
                transaction.Rollback();
                rollback = false;

                DataSet results = GetDataSetFromTableWithoutTransaction();

                Assert.AreEqual(8, results.Tables[0].Rows.Count);

                return;
            }

            Assert.Fail();
        }

        protected virtual DataSet GetDataSetFromTableWithoutTransaction()
        {
            DbCommand selectCommand = db.GetStoredProcCommand("RegionSelect");
            return db.ExecuteDataSet(selectCommand);
        }

        protected override void PrepareDatabaseSetup()
        {
			DbConnection connection = db.CreateConnection();
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        protected override DataSet GetDataSetFromTable()
        {
            DbCommand selectCommand = db.GetStoredProcCommand("RegionSelect");
            return db.ExecuteDataSet(selectCommand, transaction);
        }

        protected override void ResetDatabase()
        {
            if (rollback)
            {
                transaction.Rollback();
            }
            RestoreRegionTable();
        }

        private void RestoreRegionTable()
        {
            string sql = "delete from Region where RegionID >= 99";
            DbCommand cleanupCommand = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(cleanupCommand);
        }
    }
}

