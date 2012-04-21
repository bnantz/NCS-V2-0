//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
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
	public class CustomLogFormatterFixture
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
		public void CanBuildCustomLogFormatterFromGivenConfiguration()
		{
			CustomFormatterData customData
				= new CustomFormatterData("formatter", typeof(MockCustomLogFormatter));
			customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
			LoggingSettings settings = new LoggingSettings();
			settings.Formatters.Add(customData);
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(LoggingSettings.SectionName, settings);

			ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, "formatter", configurationSource, reflectionCache);

			Assert.IsNotNull(formatter);
			Assert.AreSame(typeof(MockCustomLogFormatter), formatter.GetType());
			Assert.AreEqual("value1", ((MockCustomLogFormatter)formatter).customValue);
		}

		[TestMethod]
		public void CanBuildCustomLogFormatterFromSavedConfiguration()
		{
			CustomFormatterData customData
				= new CustomFormatterData("formatter", typeof(MockCustomLogFormatter));
			customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
			LoggingSettings settings = new LoggingSettings();
			settings.Formatters.Add(customData);

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>(1);
			sections[LoggingSettings.SectionName] = settings;
			IConfigurationSource configurationSource 
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, "formatter", configurationSource, reflectionCache);

			Assert.IsNotNull(formatter);
			Assert.AreSame(typeof(MockCustomLogFormatter), formatter.GetType());
			Assert.AreEqual("value1", ((MockCustomLogFormatter)formatter).customValue);
		}

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			LoggingSettings rwLoggingSettings = new LoggingSettings();
			rwLoggingSettings.Formatters.Add(new CustomFormatterData("formatter1", typeof(MockCustomLogFormatter)));

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[LoggingSettings.SectionName] = rwLoggingSettings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

			Assert.AreEqual(1, roLoggingSettings.Formatters.Count);
			Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter1"));
			Assert.AreSame(typeof(CustomFormatterData), roLoggingSettings.Formatters.Get("formatter1").GetType());
			Assert.AreSame(typeof(MockCustomLogFormatter), roLoggingSettings.Formatters.Get("formatter1").Type);
		}
	}
}
