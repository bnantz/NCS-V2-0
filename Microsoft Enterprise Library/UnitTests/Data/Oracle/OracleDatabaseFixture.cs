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
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
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
    public class OracleDatabaseFixture
    {
		IConfigurationSource configurationSource;
		DatabaseConfigurationView view;
		IDatabaseAssembler assembler;

		[TestInitialize]
		public void SetUp()
		{
			configurationSource = new SystemConfigurationSource();
			view = new DatabaseConfigurationView(configurationSource);
			assembler = new DatabaseCustomFactory().GetAssembler(typeof(OracleDatabase), "", new ConfigurationReflectionCache());
		}

		[TestMethod]
		public void CanConnectToOracleAndExecuteAReader()
		{
			ConnectionStringSettings data = view.GetConnectionStringSettings("OracleTest");
			OracleDatabase oracleDatabase = (OracleDatabase)assembler.Assemble(data.Name, data, configurationSource);

			DbConnection connection = oracleDatabase.CreateConnection();
			Assert.IsNotNull(connection);
			Assert.IsTrue(connection is OracleConnection);
			connection.Open();
			DbCommand cmd = oracleDatabase.GetSqlStringCommand("Select * from Region");
			cmd.CommandTimeout = 0;
		}

		[TestMethod]
		public void CanExecuteCommandWithEmptyPackages()
		{
			ConnectionStringSettings data = view.GetConnectionStringSettings("OracleTest");

			OracleDatabase oracleDatabase = new OracleDatabase(data.ConnectionString);
			DbConnection connection = oracleDatabase.CreateConnection();
			Assert.IsNotNull(connection);
			Assert.IsTrue(connection is OracleConnection);
			connection.Open();
			DbCommand cmd = oracleDatabase.GetSqlStringCommand("Select * from Region");
			cmd.CommandTimeout = 0;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructingAnOracleDatabaseWithNullPackageListThrows()
		{
			ConnectionStringSettings data = view.GetConnectionStringSettings("OracleTest");

			new OracleDatabase(data.ConnectionString, null);
		}
    }
}

