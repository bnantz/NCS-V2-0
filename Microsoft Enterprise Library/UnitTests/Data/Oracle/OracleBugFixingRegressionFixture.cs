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
	public class OracleBugFixingRegressionFixture
	{
		private const string OracleTestStoredProcedureInPackageWithTranslation = "TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
		private const string OracleTestTranslatedStoredProcedureInPackageWithTranslation = "TESTPACKAGE.TESTPACKAGETOTRANSLATEGETCUSTOMERDETAILS";
		private const string OracleTestStoredProcedureInPackageWithoutTranslation = "TESTPACKAGETOKEEPGETCUSTOMERDETAILS";
		private const string OracleTestPackage1Prefix = "TESTPACKAGETOTRANSLATE";
		private const string OracleTestPackage1Name = "TESTPACKAGE";
		private const string OracleTestPackage2Prefix = "TESTPACKAGETOTRANSLATE2";
		private const string OracleTestPackage2Name = "TESTPACKAGE2";

		private Guid referenceGuid = new Guid("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
		private Database db;

		[TestInitialize]
		public void SetUp()
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(TestConfigurationSource.CreateConfigurationSource());
			db = factory.Create("OracleTest");
			CreateTableWithGuidAndBinary();
		}

		[TestCleanup]
		public void TearDown()
		{
			DropTableWithGuidAndBinary();
		}

		[TestMethod]
		public void CommandTextWithConfiguredPackageTranslationsShouldBeTranslatedToTheCorrectPackageBug1572()
		{
			DbCommand dBCommand = db.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithTranslation);

			Assert.AreEqual((object)OracleTestTranslatedStoredProcedureInPackageWithTranslation, dBCommand.CommandText);
		}

		[TestMethod]
		public void CommandTextWithoutConfiguredPackageTranslationsShouldNotBeTranslatedBug1572()
		{
			DbCommand dBCommand = db.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithoutTranslation);

			Assert.AreEqual((object)OracleTestStoredProcedureInPackageWithoutTranslation, dBCommand.CommandText);
		}

		[TestMethod]
		public void CanGetGuidFromReader()
		{
			using (IDataReader reader = db.ExecuteReader(CommandType.Text, "SELECT * FROM GUID_BINARY_TABLE"))
			{
				Assert.IsNotNull(reader);
				Assert.IsTrue(reader.Read());
				Guid guidValue = reader.GetGuid(0);
				Assert.IsNotNull(guidValue);
				Assert.AreEqual(referenceGuid, guidValue);
				bool boolValue = reader.GetBoolean(1);
				Assert.IsTrue(boolValue);
				Assert.IsFalse(reader.Read());
			}
		}

		private void CreateTableWithGuidAndBinary()
		{
			string commandText = null;
			string guidText = referenceGuid.ToString("N");

			commandText = @"DROP TABLE GUID_BINARY_TABLE";
			try { db.ExecuteNonQuery(CommandType.Text, commandText); }
			catch { }

			commandText = @"CREATE TABLE GUID_BINARY_TABLE(GUID_COL RAW(16), BOOL_COL VARCHAR2(10))";
			db.ExecuteNonQuery(CommandType.Text, commandText);

			commandText = @"INSERT INTO GUID_BINARY_TABLE(GUID_COL, BOOL_COL) VALUES ('" + guidText + "', 'true')";
			db.ExecuteNonQuery(CommandType.Text, commandText);
		}

		private void DropTableWithGuidAndBinary()
		{
			string commandText = null;

			commandText = @"DROP TABLE GUID_BINARY_TABLE";
			try { db.ExecuteNonQuery(commandText); }
			catch { }
		}

	}
}
