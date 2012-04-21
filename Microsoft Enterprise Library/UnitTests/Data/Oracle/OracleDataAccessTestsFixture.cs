//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
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
	[TestClass]
	public class OracleDataAccessTestsFixture
	{
		Database db;
		DataAccessTestsFixture baseFixture;

		[TestInitialize]
		public void TestInitialize()
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
			db = factory.Create("OracleTest");
			baseFixture = new DataAccessTestsFixture(db);
		}

		[TestMethod]
		public void CanGetResultSetBackWithParamaterizedQuery()
		{
			string sqlCommand = "SELECT RegionDescription FROM Region where regionId = :ID";
			DataSet ds = new DataSet();
			DbCommand cmd = db.GetSqlStringCommand(sqlCommand);
			db.AddInParameter(cmd, ":ID", DbType.Int32, 4);

			using (DbConnection connection = db.CreateConnection())
			{
				connection.Open();
				using (DbTransaction transaction = connection.BeginTransaction())
				{
					db.LoadDataSet(cmd, ds, "Foo", transaction);
					Assert.AreEqual(1, ds.Tables[0].Rows.Count);
				}
			}
		}

		// ********************************

		[TestMethod]
		public void CanGetNonEmptyResultSet()
		{
			baseFixture.CanGetNonEmptyResultSet();
		}

		[TestMethod]
		public void CanGetTablePositionally()
		{
			baseFixture.CanGetTablePositionally();
		}

		[TestMethod]
		public void CanGetNonEmptyResultSetUsingTransaction()
		{
			baseFixture.CanGetNonEmptyResultSetUsingTransaction();
		}

		[TestMethod]
		public void CanGetNonEmptyResultSetUsingTransactionWithNullTableName()
		{
			baseFixture.CanGetNonEmptyResultSetUsingTransactionWithNullTableName();
		}

		[TestMethod]
		public void ExecuteDataSetWithCommand()
		{
			baseFixture.ExecuteDataSetWithCommand();
		}

		[TestMethod]
		public void ExecuteDataSetWithDbTransaction()
		{
			baseFixture.ExecuteDataSetWithDbTransaction();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CannotLoadDataSetWithEmptyTableName()
		{
			baseFixture.CannotLoadDataSetWithEmptyTableName();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ExecuteNullCommand()
		{
			baseFixture.ExecuteNullCommand();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ExecuteCommandNullTransaction()
		{
			baseFixture.ExecuteCommandNullTransaction();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ExecuteCommandNullDataset()
		{
			baseFixture.ExecuteCommandNullDataset();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ExecuteCommandNullCommand()
		{
			baseFixture.ExecuteCommandNullCommand();
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ExecuteCommandNullTableName()
		{
			baseFixture.ExecuteCommandNullTableName();
		}
	}
}

