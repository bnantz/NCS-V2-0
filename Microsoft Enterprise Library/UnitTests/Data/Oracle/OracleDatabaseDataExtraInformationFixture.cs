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

using System.Configuration;
using System.Data.Common;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using SysConf = System.Configuration;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;

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
	/// Summary description for OracleDatabaseDataExtraInformationFixture
	/// </summary>
	[TestClass]
	public class OracleDatabaseDataExtraInformationFixture
	{
		private const string OracleTestStoredProcedureInPackageWithTranslation = "pref1GETCUSTOMERDETAILS";
		private const string OracleTestTranslatedStoredProcedureInPackageWithTranslation = "package1.pref1GETCUSTOMERDETAILS";
		private const string name = "name";

		private IDatabaseAssembler assembler;
		private DictionaryConfigurationSource configurationSource;
		private DatabaseConfigurationView configurationView;

		[TestInitialize]
		public void SetUp()
		{
			assembler = new DatabaseCustomFactory().GetAssembler(typeof(OracleDatabase), "", new ConfigurationReflectionCache());
			configurationSource = new DictionaryConfigurationSource();
			configurationView = new DatabaseConfigurationView(configurationSource);
		}

		[TestMethod]
		public void CanGetExtraInformation()
		{
			ConnectionStringSettings data 
				= new ConnectionStringSettings(name, "connection string;");

			OracleConnectionData oracleConnectionData = new OracleConnectionData();
			oracleConnectionData.Name = name;
			oracleConnectionData.Packages.Add(new OraclePackageData("package1", "pref1"));
			oracleConnectionData.Packages.Add(new OraclePackageData("package2", "pref2"));

			OracleConnectionSettings oracleConnectionSettings = new OracleConnectionSettings();
			oracleConnectionSettings.OracleConnectionsData.Add(oracleConnectionData);

			configurationSource.Add(OracleConnectionSettings.SectionName, oracleConnectionSettings);

			OracleDatabase database = (OracleDatabase)assembler.Assemble(data.Name, data, configurationSource);

			Assert.IsNotNull(database);
			Assert.AreSame(typeof(OracleDatabase), database.GetType());

			// can't access the packages - must resort to side effect
			DbCommand dBCommand = database.GetStoredProcCommand(OracleTestStoredProcedureInPackageWithTranslation);
			Assert.AreEqual((object)OracleTestTranslatedStoredProcedureInPackageWithTranslation, dBCommand.CommandText);
		}

		[TestMethod]
		public void WillNotFailIfOracleSectionDoesNotExist()
		{
			ConnectionStringSettings data
				= new ConnectionStringSettings(name, "connection string;");

			OracleDatabase database = (OracleDatabase)assembler.Assemble(data.Name, data, configurationSource);

			Assert.IsNotNull(database);
			Assert.AreSame(typeof(OracleDatabase), database.GetType());
		}

		[TestMethod]
		public void WillNotFailIfOracleConnectionDataEntryDesNotExistForName()
		{
			ConnectionStringSettings data
				= new ConnectionStringSettings(name, "connection string;");

			OracleConnectionSettings oracleConnectionSettings = new OracleConnectionSettings();

			OracleDatabase database = (OracleDatabase)assembler.Assemble(data.Name, data, configurationSource);

			Assert.IsNotNull(database);
			Assert.AreSame(typeof(OracleDatabase), database.GetType());
		}

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			OracleConnectionSettings rwSettings = new OracleConnectionSettings();

			OracleConnectionData rwOracleConnectionData = new OracleConnectionData();
			rwOracleConnectionData.Name = "name0";
			rwOracleConnectionData.Packages.Add(new OraclePackageData("package1", "pref1"));
			rwOracleConnectionData.Packages.Add(new OraclePackageData("package2", "pref2"));
			rwSettings.OracleConnectionsData.Add(rwOracleConnectionData);
			rwOracleConnectionData = new OracleConnectionData();
			rwOracleConnectionData.Name = "name1";
			rwOracleConnectionData.Packages.Add(new OraclePackageData("package3", "pref3"));
			rwOracleConnectionData.Packages.Add(new OraclePackageData("package4", "pref4"));
			rwSettings.OracleConnectionsData.Add(rwOracleConnectionData);

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[OracleConnectionSettings.SectionName] = rwSettings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			OracleConnectionSettings roSettings = (OracleConnectionSettings)configurationSource.GetSection(OracleConnectionSettings.SectionName);
			Assert.AreEqual(2, roSettings.OracleConnectionsData.Count);
			Assert.AreEqual("name0", roSettings.OracleConnectionsData.Get(0).Name);
			Assert.AreEqual(2, roSettings.OracleConnectionsData.Get(0).Packages.Count);
		}
	}
}
