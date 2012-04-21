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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
	/// <summary>
	/// Test the ExecuteReader method on the Database class
	/// </summary>
	[TestClass]
	public class OracleExecuteReaderFixture
	{
		private const string insertString = "Insert into Region values (99, 'Midwest')";
		private const string queryString = "Select * from Region";
		private Database db;
		private ExecuteReaderFixture baseFixture;

		[TestInitialize]
		public void SetUp()
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
			db = factory.Create("OracleTest");

			DbCommand insertCommand = db.GetSqlStringCommand(insertString);
			DbCommand queryCommand = db.GetSqlStringCommand(queryString);

			baseFixture = new ExecuteReaderFixture(db, insertString, insertCommand, queryString, queryCommand);
		}

		[TestMethod]
		public void CanExecuteReaderWithCommandText()
		{
			baseFixture.CanExecuteReaderWithCommandText();
		}

		[TestMethod]
		public void Bug869Test()
		{
			object[] paramarray = new object[2];
			paramarray[0] = "BLAUS";
			paramarray[1] = null;

			using (IDataReader dataReader = db.ExecuteReader("GetCustomersTest", paramarray))
			{
				while (dataReader.Read())
				{
					// Get the value of the 'Name' column in the DataReader
					Assert.IsNotNull(dataReader["ContactName"]);
				}
				dataReader.Close();

			}
		}

		[TestMethod]
		public void CanExecuteReaderFromDbCommand()
		{
			baseFixture.CanExecuteReaderFromDbCommand();
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

