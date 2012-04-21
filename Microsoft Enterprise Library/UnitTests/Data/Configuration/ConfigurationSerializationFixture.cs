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

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Tests
{
	[TestClass]
	public class ConfigurationSerializationFixture
	{
		const string providerName1 = "provider 1";
		const string providerName2 = "provider 2";

		const string databaseName1 = "database 1";

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			DatabaseSettings settings = new DatabaseSettings();

			DbProviderMapping mappingData1 = new DbProviderMapping(providerName1, typeof(OracleDatabase));
			DbProviderMapping mappingData2 = new DbProviderMapping(providerName2, typeof(SqlDatabase));

			settings.DefaultDatabase = databaseName1;
			settings.ProviderMappings.Add(mappingData1);
			settings.ProviderMappings.Add(mappingData2);

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[DatabaseSettings.SectionName] = settings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			DatabaseSettings roSettigs = (DatabaseSettings)configurationSource.GetSection(DatabaseSettings.SectionName);

			Assert.IsNotNull(roSettigs);
			Assert.AreEqual(2, roSettigs.ProviderMappings.Count);
			Assert.AreEqual(databaseName1, roSettigs.DefaultDatabase);

			Assert.IsNotNull(roSettigs.ProviderMappings.Get(providerName1));
			Assert.AreSame(typeof(OracleDatabase), roSettigs.ProviderMappings.Get(providerName1).DatabaseType);
			Assert.AreEqual(providerName1, roSettigs.ProviderMappings.Get(providerName1).DbProviderName);
		}
	}
}
