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
    public class DataAccessTestsFixture
    {
        private Database db;
		private DataSet dataSet;
		private string sqlQuery = "SELECT * FROM Region";
		private DbCommand command;

		public DataAccessTestsFixture(Database db)
		{
			this.db = db;
			this.dataSet = new DataSet();
			this.command = db.GetSqlStringCommand(sqlQuery);
		}

        public void CanGetNonEmptyResultSet()
        {
            this.db.LoadDataSet(this.command, this.dataSet, "Foo");
            Assert.AreEqual(4, this.dataSet.Tables["Foo"].Rows.Count);
        }

        public void CanGetTablePositionally()
        {
            this.db.LoadDataSet(this.command, this.dataSet, "Foo");
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        public void CanGetNonEmptyResultSetUsingTransaction()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, this.dataSet, "Foo", transaction);
					transaction.Commit();
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        
        public void CanGetNonEmptyResultSetUsingTransactionWithNullTableName()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    db.LoadDataSet(this.command, this.dataSet, "Foo", transaction);
					transaction.Commit();
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        
        public void ExecuteDataSetWithCommand()
        {
            this.db.LoadDataSet(this.command, this.dataSet, "Foo");
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        
        public void ExecuteDataSetWithDbTransaction()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, this.dataSet, "Foo", transaction);
					transaction.Commit();
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        
        public void CannotLoadDataSetWithEmptyTableName()
        {
            db.LoadDataSet(command, dataSet, "");
            Assert.Fail("Cannot call LoadDataSet with empty SourceTable name");
        }

        
        public void ExecuteNullCommand()
        {
            this.db.LoadDataSet(null, null, (string)null);
        }

        public void ExecuteCommandNullTransaction()
        {
            this.db.LoadDataSet(this.command, this.dataSet, "Foo", null);
        }

        public void ExecuteCommandNullDataset()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, null, "Foo", transaction);
					transaction.Commit();
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        public void ExecuteCommandNullCommand()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(null, this.dataSet, "Foo", transaction);
					transaction.Commit();
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        public void ExecuteCommandNullTableName()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, this.dataSet, (string)null, transaction);
					transaction.Commit();
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }
    }
}

