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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
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


namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Formatters
{
	[TestClass]
	public class BinaryLogFormatterFixture
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
		public void CanDeserializeSerializedConfiguration()
		{
			LoggingSettings rwLoggingSettings = new LoggingSettings();
			rwLoggingSettings.Formatters.Add(new BinaryLogFormatterData("formatter1"));
			rwLoggingSettings.Formatters.Add(new BinaryLogFormatterData("formatter2"));

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[LoggingSettings.SectionName] = rwLoggingSettings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

			Assert.AreEqual(2, roLoggingSettings.Formatters.Count);
			Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter1"));
			Assert.AreSame(typeof(BinaryLogFormatterData), roLoggingSettings.Formatters.Get("formatter1").GetType());
			Assert.AreSame(typeof(BinaryLogFormatter), roLoggingSettings.Formatters.Get("formatter1").Type);
			Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter2"));
			Assert.AreSame(typeof(BinaryLogFormatterData), roLoggingSettings.Formatters.Get("formatter2").GetType());
			Assert.AreSame(typeof(BinaryLogFormatter), roLoggingSettings.Formatters.Get("formatter2").Type);
		}

		[TestMethod]
		public void CanCreateFormatterFromFactoryFromGivenName()
		{
			FormatterData data = new BinaryLogFormatterData("ignore");
			LoggingSettings settings = new LoggingSettings();
			settings.Formatters.Add(data);
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(LoggingSettings.SectionName, settings);

			ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, "ignore", configurationSource, reflectionCache);

			Assert.IsNotNull(formatter);
			Assert.AreEqual(formatter.GetType(), typeof(BinaryLogFormatter));
		}

		[TestMethod]
		public void CanCreateFormatterFromFactoryFromGivenConfiguration()
		{
			FormatterData data = new BinaryLogFormatterData("ignore");
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

			ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, data, configurationSource, reflectionCache);

			Assert.IsNotNull(formatter);
			Assert.AreEqual(formatter.GetType(), typeof(BinaryLogFormatter));
		}

		[TestMethod]
		public void CanDeserializeFormattedEntry()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Message = "message";
			entry.Title = "title";
			entry.Categories = new List<string>(new string[] { "cat1", "cat2", "cat3" });

			string serializedLogEntryText = new BinaryLogFormatter().Format(entry);
			LogEntry deserializedEntry = BinaryLogFormatter.Deserialize(serializedLogEntryText);

			Assert.IsNotNull(deserializedEntry);
			Assert.IsFalse(object.ReferenceEquals(entry, deserializedEntry));
			Assert.AreEqual(entry.Categories.Count, deserializedEntry.Categories.Count);
			foreach (string category in entry.Categories)
			{
				Assert.IsTrue(deserializedEntry.Categories.Contains(category));
			}
			Assert.AreEqual(entry.Message, deserializedEntry.Message);
			Assert.AreEqual(entry.Title, deserializedEntry.Title);
		}

		[TestMethod]
		public void CanDeserializeFormattedCustomEntry()
		{
			CustomLogEntry entry = new CustomLogEntry();
			entry.TimeStamp = DateTime.MaxValue;
			entry.Title = "My custom message title";
			entry.Message = "My custom message body";
			entry.Categories = new List<string>(new string[] { "CustomFormattedCategory", "OtherCategory" });
			entry.AcmeCoField1 = "apple";
			entry.AcmeCoField2 = "orange";
			entry.AcmeCoField3 = "lemon";

			string serializedLogEntryText = new BinaryLogFormatter().Format(entry);
			CustomLogEntry deserializedEntry =
				(CustomLogEntry)BinaryLogFormatter.Deserialize(serializedLogEntryText);

			Assert.IsNotNull(deserializedEntry);
			Assert.IsFalse(object.ReferenceEquals(entry, deserializedEntry));
			Assert.AreEqual(entry.Categories.Count, deserializedEntry.Categories.Count);
			foreach (string category in entry.Categories)
			{
				Assert.IsTrue(deserializedEntry.Categories.Contains(category));
			}
			Assert.AreEqual(entry.Message, deserializedEntry.Message);
			Assert.AreEqual(entry.Title, deserializedEntry.Title);
			Assert.AreEqual(entry.AcmeCoField1, deserializedEntry.AcmeCoField1);
			Assert.AreEqual(entry.AcmeCoField2, deserializedEntry.AcmeCoField2);
			Assert.AreEqual(entry.AcmeCoField3, deserializedEntry.AcmeCoField3);
		}
	}
}
