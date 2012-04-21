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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
	[TestClass]
	public class CustomLogFilterFixture
	{
		[TestMethod]
		public void CanBuildCustomLogFilterFromGivenConfiguration()
		{
			CustomLogFilterData filterData
				= new CustomLogFilterData("custom", typeof(MockCustomLogFilter));
			filterData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");

			MockLogObjectsHelper helper = new MockLogObjectsHelper();
			ILogFilter filter = LogFilterCustomFactory.Instance.Create(new MockBuilderContext(), filterData, helper.configurationSource, new ConfigurationReflectionCache());

			Assert.IsNotNull(filter);
			Assert.AreSame(typeof(MockCustomLogFilter), filter.GetType());
			Assert.AreEqual("value1", ((MockCustomLogFilter)filter).customValue);
		}

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			LoggingSettings rwLoggingSettings = new LoggingSettings();
			rwLoggingSettings.LogFilters.Add(new CustomLogFilterData("filter1", typeof(MockCustomLogFilter)));

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[LoggingSettings.SectionName] = rwLoggingSettings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

			Assert.AreEqual(1, roLoggingSettings.LogFilters.Count);
			Assert.IsNotNull(roLoggingSettings.LogFilters.Get("filter1"));
			Assert.AreSame(typeof(CustomLogFilterData), roLoggingSettings.LogFilters.Get("filter1").GetType());
			Assert.AreSame(typeof(MockCustomLogFilter), roLoggingSettings.LogFilters.Get("filter1").Type);
		}
	}
}
