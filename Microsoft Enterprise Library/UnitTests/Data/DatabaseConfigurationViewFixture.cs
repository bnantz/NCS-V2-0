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
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
	/// <summary>
	/// Summary description for DatabaseConfigurationViewFixture
	/// </summary>
	[TestClass]
	public class DatabaseConfigurationViewFixture
	{
		private const string connectionName = "Service_Dflt";
		private const string unknownConnectionName = "not in config";
		private const string connectionNameWithoutProvider = "no provider";
		private const string connectionString = @"server=(local)\SQLEXPRESS;database=Northwind;Integrated Security=true";
		private const string providerName = "System.Data.SqlClient";
		private const string instanceName = "Database Instance";

		private ConnectionStringsSection GetConnectionStringsSection()
		{
			ConnectionStringsSection section = new ConnectionStringsSection();
			ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(instanceName, connectionString, providerName);
			section.ConnectionStrings.Add(connectionStringSettings);
			return section;
		}

		private DbProviderMapping GetProviderMapping()
		{
			DbProviderMapping providerMapping = new DbProviderMapping(typeof(SqlClientFactory).AssemblyQualifiedName, typeof(SqlDatabase));
			return providerMapping;
		}

		[TestMethod]
		public void CanGetConnectionStringsIfConnectionStringExists()
		{
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			DatabaseConfigurationView view = new DatabaseConfigurationView(configurationSource);

			ConnectionStringSettings data = view.GetConnectionStringSettings(connectionName);

			Assert.IsNotNull(data);
			Assert.AreEqual(connectionString, data.ConnectionString);
		}

		[ExpectedException(typeof(ConfigurationErrorsException))]
		[TestMethod]
		public void FailsGetConnectionStringIfConnectionStringDoesNotExist()
		{
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			DatabaseConfigurationView view = new DatabaseConfigurationView(configurationSource);

			view.GetConnectionStringSettings(unknownConnectionName);
		}

		[TestMethod]
		public void CreateNamedDatabaseInstanceWithDictSource()
		{
			DictionaryConfigurationSource source = new DictionaryConfigurationSource();
			DatabaseSettings settings = new DatabaseSettings();
			ConnectionStringsSection connSection = GetConnectionStringsSection();
			DbProviderMapping providerMapping = GetProviderMapping();
			settings.ProviderMappings.Add(providerMapping);
			source.Add("dataConfiguration", settings);
			source.Add("connectionStrings", connSection);
			DatabaseProviderFactory factory = new DatabaseProviderFactory(source);
			Database dbIns = factory.Create(instanceName);
			Assert.IsNotNull(dbIns);

		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RequestForNullNameThrows()
		{
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			DatabaseConfigurationView view = new DatabaseConfigurationView(configurationSource);

			view.GetConnectionStringSettings(null);

		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RequestForEmptyNameThrows()
		{
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			DatabaseConfigurationView view = new DatabaseConfigurationView(configurationSource);

			view.GetConnectionStringSettings("");

		}
	}
}
