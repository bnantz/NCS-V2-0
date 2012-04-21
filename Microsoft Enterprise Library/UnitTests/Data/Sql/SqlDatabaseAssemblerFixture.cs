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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
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
	public class SqlDatabaseAssemblerFixture
	{
		private ConfigurationReflectionCache reflectionCache;
		private IConfigurationSource configurationSource;

		[TestInitialize]
		public void SetUp()
		{
			reflectionCache = new ConfigurationReflectionCache();
			configurationSource = new SystemConfigurationSource();
		}

		[TestMethod]
		public void CanGetAssemblerForSqlDatabase()
		{
			IDatabaseAssembler assembler
				= new DatabaseCustomFactory().GetAssembler(typeof(SqlDatabase), "ignore", reflectionCache);

			Assert.IsNotNull(assembler);
		}

		[TestMethod]
		public void AssemblerCanAssembleSqlDatabase()
		{
			ConnectionStringSettings settings 
				= new ConnectionStringSettings("name", "test;", DbProviderMapping.DefaultSqlProviderName);

			IDatabaseAssembler assembler
				= new DatabaseCustomFactory().GetAssembler(typeof(SqlDatabase), settings.Name, reflectionCache);
			Database database = assembler.Assemble(settings.Name, settings, configurationSource);

			Assert.IsNotNull(database);
			Assert.AreSame(typeof(SqlDatabase), database.GetType());
			Assert.AreEqual(settings.ConnectionString, database.ConnectionStringWithoutCredentials);
		}

	}
}
