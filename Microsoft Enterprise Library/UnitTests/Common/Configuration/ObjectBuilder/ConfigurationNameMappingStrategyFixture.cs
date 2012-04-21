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
	public class ConfigurationNameMappingStrategyFixture
	{
		IBuilder<BuilderStage> builder;

		[TestInitialize]
		public void SetUp()
		{
			builder = new BuilderBase<BuilderStage>();
			builder.Strategies.AddNew<ConfigurationNameMappingStrategy>(BuilderStage.PreCreation);

			MockNameMapper.invoked = false;
		}

		[TestMethod]
		public void ChainForNullIdOnTypeWithoutNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp<TypeWithoutNameMappingAttribute>(null, null, null);

			Assert.IsFalse(MockNameMapper.invoked);
		}

		[TestMethod]
		public void ChainForNullIdOnTypeWithNameMappingDoesInvokeMappper()
		{
			builder.BuildUp<TypeWithNameMappingAttribute>(null, null, null);

			Assert.IsTrue(MockNameMapper.invoked);
		}

		[TestMethod]
		public void ChainForNonNullIdOnTypeWithoutNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp<TypeWithoutNameMappingAttribute>(null, "id", null);

			Assert.IsFalse(MockNameMapper.invoked);
		}

		[TestMethod]
		public void ChainForNonNullIdOnTypeWithNameMappingDoesNotInvokeMappper()
		{
			builder.BuildUp<TypeWithNameMappingAttribute>(null, "id", null);

			Assert.IsFalse(MockNameMapper.invoked);
		}
	}


	internal class TypeWithoutNameMappingAttribute
	{
	}

	[ConfigurationNameMapper(typeof(MockNameMapper))]
	internal class TypeWithNameMappingAttribute
	{
	}

	internal class MockNameMapper : IConfigurationNameMapper
	{
		internal static bool invoked;

		public string MapName(string name, IConfigurationSource configSource)
		{
			invoked = true;
			return name;
		}
	}
}
