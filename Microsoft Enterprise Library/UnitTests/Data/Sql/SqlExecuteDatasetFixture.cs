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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Sql
{
	[TestClass]
	public class SqlExecuteDatasetFixture
	{
		private const string queryString = "Select * from Region";
		private const string storedProc = "Ten Most Expensive Products";
		private Database db;
		
		[TestInitialize]
		public void TestInitialize()
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
			db = factory.CreateDefault();			
		}

		[TestMethod]
		public void CanRetriveDataSetFromSqlString()
		{
			DataSet dataSet = db.ExecuteDataSet(CommandType.Text, queryString);
			Assert.AreEqual(1, dataSet.Tables.Count);
		}

		[TestMethod]
		public void CanRetiveDataSetFromStoredProcedure()
		{
			DataSet dataSet = db.ExecuteDataSet(storedProc);
			Assert.AreEqual(1, dataSet.Tables.Count);
		}

		[TestMethod]
		public void CanRetriveDataSetFromSqlStringWithTransaction()
		{
			using (DbConnection connection = db.CreateConnection())
			{
				connection.Open();
				using (DbTransaction trans = connection.BeginTransaction())
				{
					DataSet dataSet = db.ExecuteDataSet(trans, CommandType.Text, queryString);
					Assert.AreEqual(1, dataSet.Tables.Count);
				}
			}
		}

		[TestMethod]
		public void CanRetiveDataSetFromStoredProcedureWithTransaction()
		{
			using (DbConnection connection = db.CreateConnection())
			{
				connection.Open();
				using (DbTransaction trans = connection.BeginTransaction())
				{

					DataSet dataSet = db.ExecuteDataSet(trans, storedProc);
					Assert.AreEqual(1, dataSet.Tables.Count);
				}
			}
		}
	}
}
