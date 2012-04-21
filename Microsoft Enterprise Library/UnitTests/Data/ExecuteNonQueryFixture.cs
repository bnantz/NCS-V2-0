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
    public class ExecuteNonQueryFixture
    {
        private Database db;
		private string insertString;
		private string countQuery;
		private DbCommand insertionCommand;
		private DbCommand countCommand;

		public ExecuteNonQueryFixture(Database db, string insertString, string countQuery, DbCommand insertionCommand, DbCommand countCommand)
		{
			this.db = db;
			this.insertString = insertString;
			this.countQuery = countQuery;
			this.insertionCommand = insertionCommand;
			this.countCommand = countCommand;
		}

        public void CanExecuteNonQueryWithDbCommand()
        {
            this.db.ExecuteNonQuery(this.insertionCommand);

            int count = Convert.ToInt32(this.db.ExecuteScalar(this.countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DbCommand cleanupCommand = this.db.GetSqlStringCommand(cleanupString);
            int rowsAffected = this.db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
			Assert.AreEqual(1, rowsAffected);
        }

		public void CanExecuteNonQueryWithCommandTextWithDefinedTypeAndTransaction()
		{
			using (DbConnection connection = db.CreateConnection())
			{
				connection.Open();
				using (DbTransaction trans = connection.BeginTransaction())
				{
					this.db.ExecuteNonQuery(trans, CommandType.Text, insertionCommand.CommandText);
					trans.Commit();
				}
			}

			int count = Convert.ToInt32(this.db.ExecuteScalar(this.countCommand));

			string cleanupString = "delete from Region where RegionId = 77";
			DbCommand cleanupCommand = this.db.GetSqlStringCommand(cleanupString);
			int rowsAffected = this.db.ExecuteNonQuery(cleanupCommand);

			Assert.AreEqual(5, count);
			Assert.AreEqual(1, rowsAffected);
				
		}

		public void CanExecuteNonQueryWithCommandTextAndTransaction()
		{
			using (DbConnection connection = db.CreateConnection())
			{
				connection.Open();
				using (DbTransaction trans = connection.BeginTransaction())
				{
					this.db.ExecuteNonQuery(trans, insertionCommand.CommandText);
					trans.Commit();
				}
			}

			int count = Convert.ToInt32(this.db.ExecuteScalar(this.countCommand));

			string cleanupString = "delete from Region where RegionId = 77";
			DbCommand cleanupCommand = this.db.GetSqlStringCommand(cleanupString);
			int rowsAffected = this.db.ExecuteNonQuery(cleanupCommand);

			Assert.AreEqual(5, count);
			Assert.AreEqual(1, rowsAffected);

		}

		

		public void CanExecuteNonQueryWithCommandTextAsStoredProcAndTransaction()
		{
			using (DbConnection connection = db.CreateConnection())
			{
				connection.Open();
				using (DbTransaction trans = connection.BeginTransaction())
				{
					connection.Open();
					this.db.ExecuteNonQuery(trans, insertionCommand.CommandText);
					trans.Commit();
				}
			}

			int count = Convert.ToInt32(this.db.ExecuteScalar(this.countCommand));

			string cleanupString = "delete from Region where RegionId = 77";
			DbCommand cleanupCommand = this.db.GetSqlStringCommand(cleanupString);
			int rowsAffected = this.db.ExecuteNonQuery(cleanupCommand);

			Assert.AreEqual(5, count);
			Assert.AreEqual(1, rowsAffected);				
		}

        public void CanExecuteNonQueryThroughTransaction()
        {
			using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    int rowsAffected = this.db.ExecuteNonQuery(this.insertionCommand, transaction.Transaction);

                    int count = Convert.ToInt32(this.db.ExecuteScalar(this.countCommand, transaction.Transaction));
                    Assert.AreEqual(5, count);
                    Assert.AreEqual(1, rowsAffected);
                }
            }
        }

        public void TransactionActuallyRollsBack()
        {
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    this.db.ExecuteNonQuery(this.insertionCommand, transaction.Transaction);
                }
            }

            DbCommand wrapper = this.db.GetSqlStringCommand(this.countQuery);
            int count = Convert.ToInt32(this.db.ExecuteScalar(wrapper));
            Assert.AreEqual(4, count);
        }

        public void ExecuteNonQueryWithNullDbTransaction()
        {
            this.db.ExecuteNonQuery(this.insertionCommand, null);
        }

        public void ExecuteNonQueryWithNullDbCommandAndTransaction()
        {
            this.db.ExecuteNonQuery(null, (string)null);
        }
    }
}

