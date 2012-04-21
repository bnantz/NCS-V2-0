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
    public class ExecuteReaderFixture
    {
        private Database db;
		private string queryString;
		private string insertString;
		private DbCommand queryCommand;
		private DbCommand insertCommand;

		public ExecuteReaderFixture(Database db, string insertString, DbCommand insertCommand, string queryString, DbCommand queryCommand)
		{
			this.db = db;
			this.insertString = insertString;
			this.queryString = queryString;
			this.insertCommand = insertCommand;
			this.queryCommand = queryCommand;
		}

		public void CanExecuteReaderWithCommandText()
		{
			IDataReader reader = this.db.ExecuteReader(CommandType.Text, queryString);
			string accumulator = "";
			while (reader.Read())
			{
				accumulator += ((string)reader["RegionDescription"]).Trim();
			}
			reader.Close();

			Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);			
		}

        public void CanExecuteReaderFromDbCommand()
        {
            IDataReader reader = this.db.ExecuteReader(this.queryCommand);
            string accumulator = "";
            while (reader.Read())
            {
                accumulator += ((string)reader["RegionDescription"]).Trim();
            }
            reader.Close();

            Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
            Assert.AreEqual(ConnectionState.Closed, this.queryCommand.Connection.State);
        }

        public void WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute()
        {
            int count = -1;
            IDataReader reader = null;
            try
            {
                reader = this.db.ExecuteReader(this.insertCommand);
                count = reader.RecordsAffected;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                string deleteString = "Delete from Region where RegionId = 99";
                DbCommand cleanupCommand = this.db.GetSqlStringCommand(deleteString);
                this.db.ExecuteNonQuery(cleanupCommand);
            }

            Assert.AreEqual(1, count);
        }
		
        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    IDataReader reader = this.db.ExecuteReader(this.insertCommand, transaction.Transaction);
                    Assert.AreEqual(1, reader.RecordsAffected);
                    reader.Close();
                }

                Assert.AreEqual(ConnectionState.Open, connection.State);
            }
        }

        public void CanExecuteQueryThroughDataReaderUsingNullCommand()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                this.insertCommand = null;
                IDataReader reader = this.db.ExecuteReader(this.insertCommand, null);
            }
        }

        public void CanExecuteQueryThroughDataReaderUsingNullCommandAndNullTransaction()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                IDataReader reader = this.db.ExecuteReader(null, (string)null);
            }
        }

        public void CanExecuteQueryThroughDataReaderUsingNullTransaction()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                IDataReader reader = this.db.ExecuteReader(this.insertCommand, null);
            }
        }

        public void ExecuteReaderWithNullCommand()
        {
            IDataReader reader = this.db.ExecuteReader((DbCommand)null);
            Assert.AreEqual(null, this.insertCommand);
        }

        public void NullQueryStringTest()
        {
            DbCommand myCommand = this.db.GetSqlStringCommand(null);
            IDataReader reader = this.db.ExecuteReader(myCommand);
        }

        public void EmptyQueryStringTest()
        {
            DbCommand myCommand = this.db.GetSqlStringCommand(string.Empty);
            IDataReader reader = this.db.ExecuteReader(myCommand);
        }
    }
}

