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

using System;
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
    public abstract class UpdateDataSetBehaviorsFixture : UpdateDataSetStoredProcedureBase
    {
        public void UpdateWithTransactionalBehavior()
        {
            AddRowsToDataTable(startingData.Tables[0]);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Transactional);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual(502, Convert.ToInt32(resultTable.Rows[6]["RegionID"]));
            Assert.AreEqual("Washington", resultTable.Rows[6]["RegionDescription"].ToString().Trim());
        }

        public void UpdateWithStandardBehavior()
        {
            AddRowsToDataTable(startingData.Tables[0]);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Standard);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual(502, Convert.ToInt32(resultTable.Rows[6]["RegionID"]));
            Assert.AreEqual("Washington", resultTable.Rows[6]["RegionDescription"].ToString().Trim());
        }

        protected override void PrepareDatabaseSetup()
        {
        }

        protected override DataSet GetDataSetFromTable()
        {
            DbCommand selectCommand = db.GetStoredProcCommand("RegionSelect");
            return db.ExecuteDataSet(selectCommand);
        }

        protected override void ResetDatabase()
        {
            RestoreRegionTable();
        }

        private void RestoreRegionTable()
        {
            string sql = "delete from Region where RegionID >= 99";
            DbCommand cleanupCommand = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(cleanupCommand);
        }

        protected override void AddTestData()
        {
        }
    }
}

