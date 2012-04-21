//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	[TestClass]
	public class AssemblerBasedObjectFactoryFixture
	{
		private MockAssembledObjectFactory factory;
		private IBuilderContext context;
		private DictionaryConfigurationSource configurationSource;
		private ConfigurationReflectionCache reflectionCache;

		[TestInitialize]
		public void SetUp()
		{
			factory = new MockAssembledObjectFactory();
			context = new MockBuilderContext();
			configurationSource = new DictionaryConfigurationSource();
			reflectionCache = new ConfigurationReflectionCache();

			MockAssembler.ConstructorCalls = 0;
		}

		[TestCleanup]
		public void TearDown()
		{
			MockAssembler.ConstructorCalls = 0;
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CreationFromConfigurationWithoutAssemblerAttributeThrows()
		{
			MockConfigurationObjectBase configurationObject
				= new MockConfigurationObjectWithoutAssemblerAttribute();

			factory.Create(context, configurationObject, configurationSource, reflectionCache);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CreationFromConfigurationWithInvalidAssemblerThrows()
		{
			MockConfigurationObjectBase configurationObject
				= new MockConfigurationObjectWithAssemblerAttributeForInvalidAssembler();

			factory.Create(context, configurationObject, configurationSource, reflectionCache);
		}

		[TestMethod]
		public void CreationFromConfigurationWithAssemblerAttributeSucceds()
		{
			MockConfigurationObjectBase configurationObject
				= new MockConfigurationObjectWithAssemblerAttribute();

			MockAssembledObject createdObject
				= factory.Create(context, configurationObject, configurationSource, reflectionCache);

			Assert.IsNotNull(createdObject);
		}

		[TestMethod]
		public void SecondCreationFromConfigurationWithAssemblerAttributeSuccedsUsingCachedAssembler()
		{
			MockConfigurationObjectBase configurationObject
				= new MockConfigurationObjectWithAssemblerAttribute();

			MockAssembledObject createdObject1
				= factory.Create(context, configurationObject, configurationSource, reflectionCache);
			MockAssembledObject createdObject2
				= factory.Create(context, configurationObject, configurationSource, reflectionCache);

			Assert.IsNotNull(createdObject1);
			Assert.IsNotNull(createdObject2);

			Assert.AreEqual(1, MockAssembler.ConstructorCalls);
		}
	}


	internal class MockAssembledObject
	{
	}

	internal class MockAssembledObjectFactory : AssemblerBasedCustomFactory<MockAssembledObject, MockConfigurationObjectBase>
	{
		protected override MockConfigurationObjectBase GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			if ("existing name".Equals(name))
			{
				return new MockConfigurationObjectWithAssemblerAttribute();
			}

			return null;
		}
	}

	internal class MockConfigurationObjectBase
	{
	}

	internal class MockConfigurationObjectWithoutAssemblerAttribute : MockConfigurationObjectBase
	{
	}

	[Assembler(typeof(MockAssembler))]
	internal class MockConfigurationObjectWithAssemblerAttribute : MockConfigurationObjectBase
	{
	}

	[Assembler(typeof(MockInvalidAssembler))]
	internal class MockConfigurationObjectWithAssemblerAttributeForInvalidAssembler : MockConfigurationObjectBase
	{
	}

	internal class MockAssembler : IAssembler<MockAssembledObject, MockConfigurationObjectBase>
	{
		public static int ConstructorCalls = 0;

		public MockAssembler()
		{
			ConstructorCalls++;
		}

		public MockAssembledObject Assemble(IBuilderContext context, MockConfigurationObjectBase objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return new MockAssembledObject();
		}
	}

	internal class MockInvalidAssembler : IAssembler<MockAssembledObject, object>
	{
		public MockAssembledObject Assemble(IBuilderContext context, object objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return null;
		}
	}
}
