//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
	[TestClass]
	public class LogFormatterFactoryFixture
	{
		private IBuilderContext context;
		private ConfigurationReflectionCache reflectionCache;

		[TestInitialize]
		public void SetUp()
		{
			context = new MockBuilderContext();
			reflectionCache = new ConfigurationReflectionCache();
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void CreationWithNullLoggingSettingsThrows()
		{
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

			LogFormatterCustomFactory.Instance.Create(context, "formatter", configurationSource, reflectionCache);
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void CreationWithMissingFormatterDataThrows()
		{
			MockLogObjectsHelper helper = new MockLogObjectsHelper();

			LogFormatterCustomFactory.Instance.Create(context, "formatter", helper.configurationSource, reflectionCache);
		}
	}
}
