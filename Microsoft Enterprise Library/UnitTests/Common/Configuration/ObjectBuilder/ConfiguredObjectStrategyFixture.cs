//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.ObjectBuilder;
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	[TestClass]
	public class ConfiguredObjectStrategyFixture
	{
		private const string name = "name";

		MockBuilderContext context;
		ConfiguredObjectStrategy configuredObjectStrategy;
		MockBuilderStrategy mockStrategy;

		DictionaryConfigurationSource source; 

		[TestInitialize]
		public void SetUp()
		{
			context = new MockBuilderContext();
			configuredObjectStrategy = new ConfiguredObjectStrategy();
			mockStrategy = new MockBuilderStrategy();

			context.InnerChain.Add(configuredObjectStrategy);
			context.InnerChain.Add(mockStrategy);

			source = new DictionaryConfigurationSource();

			context.Policies.Set<IConfigurationObjectPolicy>(new ConfigurationObjectPolicy(source), typeof(IConfigurationSource), null);
		}

		[TestMethod]
		public void ConfiguredObjectStrategyCallsCustomFactoryIfFactoryAttributeIsPresent()
		{
			Assert.AreEqual(1, context.Policies.Count);
			
			object createdObject
				= context.HeadOfChain.BuildUp(context, typeof(MockObjectWithFactory), null, name);

			Assert.IsNotNull(createdObject);
			Assert.AreSame(MockFactory.MockObject, createdObject);
			Assert.AreEqual(1, context.Policies.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ConfiguredObjectStrategyThrowsIfFactoryAttributesIsNotPresent()
		{
			Assert.AreEqual(1, context.Policies.Count);

			context.HeadOfChain.BuildUp(context, typeof(object), null, name);
		}
	}

	internal class MockBuilderStrategy : BuilderStrategy
	{
		public object existing;

		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			this.existing = existing;
			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}
	}

	[CustomFactory(typeof(MockFactory))]
	internal class MockObjectWithFactory
	{
	}

	internal class MockFactory : ICustomFactory
	{
		public static object MockObject = new object();

		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return MockObject;
		}
	}
}
