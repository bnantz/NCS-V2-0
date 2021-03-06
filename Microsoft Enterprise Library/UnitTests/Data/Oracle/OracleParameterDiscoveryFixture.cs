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
    public class OracleParameterDiscoveryFixture
    {
		private ParameterCache cache;
		private Database db;
		private DbConnection connection;
		private ParameterDiscoveryFixture baseFixture;
		private DbCommand storedProcedure;

		[TestInitialize]
		public void TestInitialize()
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
			db = factory.Create("OracleTest");
			storedProcedure = db.GetStoredProcCommand("CustOrdersOrders");
			connection = db.CreateConnection();
			connection.Open();
			storedProcedure.Connection = connection;
			cache = new ParameterCache();

			baseFixture = new ParameterDiscoveryFixture(storedProcedure);
		}

		[TestCleanup]
		public void TestCleanup()
		{
			if (connection != null)
			{
				connection.Dispose();
			}
		}


		[TestMethod]
        public void CanGetParametersForStoredProcedure()
        {
            cache.SetParameters(storedProcedure, db);
            Assert.AreEqual(2, storedProcedure.Parameters.Count);
            Assert.AreEqual("CUR_OUT", ((IDataParameter)storedProcedure.Parameters["CUR_OUT"]).ParameterName);
            Assert.AreEqual("VCUSTOMERID", ((IDataParameter)storedProcedure.Parameters["VCUSTOMERID"]).ParameterName);
        }

		[TestMethod]
        public void IsCacheUsed()
        {
			ParameterDiscoveryFixture.TestCache testCache = new ParameterDiscoveryFixture.TestCache();
            testCache.SetParameters(storedProcedure, db);

            DbCommand storedProcDuplicate = db.GetStoredProcCommand("CustOrdersOrders");
            storedProcDuplicate.Connection = connection;
            testCache.SetParameters(storedProcDuplicate, db);

            Assert.IsTrue(testCache.CacheUsed, "Cache is not used");
        }

		[TestMethod]
        public void CanDiscoverFeaturesWhileInsideTransaction()
        {
			using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
				DbCommand storedProcedure = db.GetStoredProcCommand("CustOrdersOrders");
                storedProcedure.Connection = transaction.Connection;
                storedProcedure.Transaction = transaction;

                db.DiscoverParameters(storedProcedure);

                Assert.AreEqual(2, storedProcedure.Parameters.Count);
            }
        }


		[TestMethod]
		public void CanCreateStoredProcedureCommand()
		{
			baseFixture.CanCreateStoredProcedureCommand();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetParametersWithNullCommandThrows()
		{
			cache.SetParameters(null, db);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetParametersWithNullDatabaseThrows()
		{			
			cache.SetParameters(storedProcedure, null);
		}		
    }
}

