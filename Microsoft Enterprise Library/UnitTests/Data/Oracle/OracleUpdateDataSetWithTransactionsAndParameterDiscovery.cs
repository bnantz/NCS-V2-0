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
	public class OracleUpdateDataSetWithTransactionsAndParameterDiscovery : UpdateDataSetWithTransactionsAndParameterDiscovery
	{
		[TestInitialize]
		public void Initialize()
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
			db = factory.Create("OracleTest");
			try
			{
				DeleteStoredProcedures();
			}
			catch
			{
			}
			CreateStoredProcedures();
			base.SetUp();
		}

		[TestCleanup]
		public void Dispose()
		{
			base.TearDown();
			DeleteStoredProcedures();
		}

		[TestMethod]
		public void OracleModifyRowWithStoredProcedure()
		{
			base.ModifyRowWithStoredProcedure();
		}

		protected override void CreateStoredProcedures()
		{
			OracleDataSetHelper.CreateStoredProcedures(db);
		}

		protected override void DeleteStoredProcedures()
		{
			OracleDataSetHelper.DeleteStoredProcedures(db);
		}

		protected override void CreateDataAdapterCommands()
		{
			OracleDataSetHelper.CreateDataAdapterCommandsDynamically(db, ref insertCommand, ref updateCommand, ref deleteCommand);
		}

		protected override void AddTestData()
		{
			OracleDataSetHelper.AddTestData(db);
		}
	}
}

