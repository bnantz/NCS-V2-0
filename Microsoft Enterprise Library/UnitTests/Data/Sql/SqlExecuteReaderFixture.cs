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
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
	[TestClass]
	public class SqlExecuteReaderFixture
	{
		private const string insertString = "Insert into Region values (99, 'Midwest')";
		private const string queryString = "Select * from Region";
		private Database db;
		private ExecuteReaderFixture baseFixture;

		[TestInitialize]
		public void TestInitialize()
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
			db = factory.CreateDefault();

			DbCommand insertCommand = db.GetSqlStringCommand(insertString);
			DbCommand queryCommand = db.GetSqlStringCommand(queryString);

			baseFixture = new ExecuteReaderFixture(db, insertString, insertCommand, queryString, queryCommand);
		}

		[TestMethod]
		public void CanExecuteReaderWithStoredProcInTransaction()
		{
			using (DbConnection connection = db.CreateConnection())
			{
				connection.Open();
				using (DbTransaction trans = connection.BeginTransaction())
				{
					using (db.ExecuteReader(trans, "Ten Most Expensive Products"))
					{
					}
					trans.Commit();
				}
			}
		}

		[TestMethod]
		public void CanExecuteReaderWithCommandText()
		{
			baseFixture.CanExecuteReaderWithCommandText();
		}

		[TestMethod]
		public void CanExecuteReaderFromDbCommand()
		{
			baseFixture.CanExecuteReaderFromDbCommand();
		}

		[TestMethod]
		[ExpectedException(typeof(SqlException))]
		public void ExecuteReaderWithBadCommandThrows()
		{
			DbCommand badCommand = db.GetSqlStringCommand("select * from foobar");
			db.ExecuteReader(badCommand);
		}


		[TestMethod]
		public void WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute()
		{
			baseFixture.WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute();
		}

		[TestMethod]
		public void CanExecuteQueryThroughDataReaderUsingTransaction()
		{
			baseFixture.CanExecuteQueryThroughDataReaderUsingTransaction();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CanExecuteQueryThroughDataReaderUsingNullCommand()
		{
			baseFixture.CanExecuteQueryThroughDataReaderUsingNullCommand();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CanExecuteQueryThroughDataReaderUsingNullCommandAndNullTransaction()
		{
			baseFixture.CanExecuteQueryThroughDataReaderUsingNullCommandAndNullTransaction();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CanExecuteQueryThroughDataReaderUsingNullTransaction()
		{
			baseFixture.CanExecuteQueryThroughDataReaderUsingNullTransaction();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ExecuteReaderWithNullCommand()
		{
			baseFixture.ExecuteReaderWithNullCommand();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void NullQueryStringTest()
		{
			baseFixture.NullQueryStringTest();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyQueryStringTest()
		{
			baseFixture.EmptyQueryStringTest();
		}
	}
}

