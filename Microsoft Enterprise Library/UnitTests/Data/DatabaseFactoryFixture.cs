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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.ObjectBuilder;
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
	[TestClass]
	public class DatabaseFactoryFixture
	{
		private DatabaseCustomFactory factory;
		private ConfigurationReflectionCache reflectionCache;

		[TestInitialize]
		public void SetUp()
		{
			factory = new DatabaseCustomFactory();
			reflectionCache = new ConfigurationReflectionCache();
		}

		[TestMethod]
		public void CanCreateDatabaseForValidName()
		{
			MockBuilderContext context
				= new MockBuilderContext();
			context.InnerChain.Add(
				new MockFactoryStrategy(
					new DatabaseCustomFactory(),
					new SystemConfigurationSource(),
					new ConfigurationReflectionCache()));

			object database
				= context.HeadOfChain.BuildUp(context, null, null, "Service_Dflt");

			Assert.IsNotNull(database);
			Assert.AreSame(typeof(SqlDatabase), database.GetType());
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void CreateDatabaseForInvalidNameThrows()
		{
			MockBuilderContext context
				= new MockBuilderContext();
			context.InnerChain.Add(
				new MockFactoryStrategy(
					new DatabaseCustomFactory(),
					new SystemConfigurationSource(),
					new ConfigurationReflectionCache()));

			object database
				= context.HeadOfChain.BuildUp(context, null, null, "a bad name");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void RequestAssemblerForDatabaseWithoutAssemblerAttributeThrows()
		{
			new DatabaseCustomFactory().GetAssembler(typeof(InvalidDatabase), "", new ConfigurationReflectionCache());
		}
	}

	internal class InvalidDatabase : Database
	{
		internal InvalidDatabase()
			: base("", SqlClientFactory.Instance)
		{
		}

		protected override void DeriveParameters(System.Data.Common.DbCommand discoveryCommand)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

}
