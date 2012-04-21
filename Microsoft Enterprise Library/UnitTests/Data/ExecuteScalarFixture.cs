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
    public class ExecuteScalarFixture
    {
		private Database db;
		private DbCommand command;

		public ExecuteScalarFixture(Database db, DbCommand command)
		{
			this.db = db;
			this.command = command;
		}

        public void ExecuteScalarWithIDbCommand()
        {
            int count = Convert.ToInt32(this.db.ExecuteScalar(this.command));

            Assert.AreEqual(4, count);
        }

        public void ExecuteScalarWithIDbTransaction()
        {
            int count = -1;
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    count = Convert.ToInt32(this.db.ExecuteScalar(this.command, transaction));
					transaction.Commit();
                }
            }

            Assert.AreEqual(4, count);
        }

		public void ExecuteScalarWithCommandTextAndTypeInTransaction()
		{
			int count = -1;
			using (DbConnection connection = this.db.CreateConnection())
			{
				connection.Open();
				using (DbTransaction transaction = connection.BeginTransaction())
				{
					count = Convert.ToInt32(this.db.ExecuteScalar(transaction, this.command.CommandType, this.command.CommandText));
					transaction.Commit();
				}
			}

			Assert.AreEqual(4, count);
		}

        public void CanExecuteScalarDoAnInsertion()
        {
            string insertCommand = "Insert into Region values (99, 'Midwest')";
            DbCommand command = this.db.GetSqlStringCommand(insertCommand);
			using (DbConnection connection = this.db.CreateConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    this.db.ExecuteScalar(command, transaction.Transaction);

                    DbCommand rowCountCommand = this.db.GetSqlStringCommand("select count(*) from Region");
                    int count = Convert.ToInt32(this.db.ExecuteScalar(rowCountCommand, transaction.Transaction));
                    Assert.AreEqual(5, count);
                }
            }
        }

        public void ExecuteScalarWithNullIDbCommand()
        {
            int count = Convert.ToInt32(this.db.ExecuteScalar((DbCommand)null));
        }

        public void ExecuteScalarWithNullIDbTransaction()
        {
            int count = Convert.ToInt32(this.db.ExecuteScalar(this.command, null));
        }

        public void ExecuteScalarWithNullIDbCommandAndNullTransaction()
        {
            int count = Convert.ToInt32(this.db.ExecuteScalar(null, (string)null));
        }
    }
}

