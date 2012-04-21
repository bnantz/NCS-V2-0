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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
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
	public class LogFormatterFixture
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
		public void ApplyTextFormat()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = FormatEntry("{timestamp}: {title} - {message}", entry);
			string expected = entry.TimeStampString + ": " + entry.Title + " - " + entry.Message;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ApplyTextXmlFormat()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			string template = "<EntLibLog><message>{message}</message><timestamp>{timestamp}</timestamp><title>{title}</title></EntLibLog>";
			string actual = FormatEntry(template, entry);

			string expected = "<EntLibLog><message>My message body</message><timestamp>" + DateTime.Parse("12/31/9999 11:59:59 PM").ToString() + "</timestamp><title>=== Header ===</title></EntLibLog>";
			Assert.AreEqual(expected, actual);
		}

		//[TestMethod]
		//public void WriteFormattedEntry()
		//{
		//    LogEntry entry = CommonUtil.GetDefaultLogEntry();
		//    entry.Categories = new string[] { "FormattedCategory" };
		//    entry.Severity = TraceEventType.Error;

		//    Logger.Write(entry);

		//    string actual = CommonUtil.GetLastEventLogEntryCustom();
		//    string expected = "12/31/9999 11:59:59 PM: === Header ===\r\n\r\nMy message body";
		//    Assert.AreEqual(expected, actual);
		//}

		[TestMethod]
		public void TextFormatExtendedProperties()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "DictionaryCategory" };
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "{timestamp}: {title} - {message}{newline}{dictionary({key} = {value}{newline})}";
			string actual = FormatEntry(template, entry);

			string expected = DateTime.Parse("12/31/9999 11:59:59 PM").ToString() + ": === Header === - My message body";
			Assert.IsTrue(actual.IndexOf(expected) > -1);
			Assert.IsTrue(actual.IndexOf("key1") > -1);
			Assert.IsTrue(actual.IndexOf("value1") > -1);
			Assert.IsTrue(actual.IndexOf("key2") > -1);
			Assert.IsTrue(actual.IndexOf("value2") > -1);
			Assert.IsTrue(actual.IndexOf("key3") > -1);
			Assert.IsTrue(actual.IndexOf("value3") > -1);
		}

		[TestMethod]
		public void TextFormatMutipleExtendedProperties()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "DictionaryCategory" };
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "{timestamp}: {title} - {message}{newline}" +
				"Dictionary 1:{newline}{dictionary(((({key} =-= {value}{newline}))))}{newline}{newline}" +
				"Dictionary 1 reformatted:{newline}{dictionary([[{key} === {value}{newline}]])}";
			string actual = FormatEntry(template, entry);

			Assert.IsTrue(actual.IndexOf("Dictionary 1:") > -1);
			Assert.IsTrue(actual.IndexOf("(((key1 =-= value1\r\n)))") > -1);
			Assert.IsTrue(actual.IndexOf("Dictionary 1 reformatted:") > -1);
			Assert.IsTrue(actual.IndexOf("[[key1 === value1\r\n]]") > -1);
		}

		[TestMethod]
		public void KeyValuePairExtendedPropertiesFormat()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "DictionaryCategory" };
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "{timestamp}: {title} - {message}{newline}" +
				"{dictionary({key} = {value}{newline})}{newline}" +
				"== KeyValue Pair Format Function =={newline}" +
				"Key1 = {keyvalue(key1)}{newline}" +
				"Key2 = {keyvalue(key2)}{newline}" +
				"Key3 = {keyvalue(key3)}";
			string actual = FormatEntry(template, entry);

			Assert.IsTrue(actual.IndexOf("== KeyValue Pair Format Function ==") > -1);
		}

		[TestMethod]
		public void TimestampFormat()
		{
			string template = "Time is: {timestamp(D)}";
			string actual = FormatEntry(template, CommonUtil.GetDefaultLogEntry());

			string expected = "Time is: Friday, December 31, 9999";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimestampTokenWithEmptyTemplate()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string template = "Time is: {timestamp()}";
			string actual = FormatEntry(template, entry);

			string expected = "Time is: " + DateTime.Parse("12/31/9999 11:59:59 PM").ToString();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DictionaryTokenWithEmptyTemplate()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "Dictionary: {dictionary()} value";
			string actual = FormatEntry(template, entry);

			string expected = "Dictionary:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void KeyValueTokenWithEmptyTemplate()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "Key is: {keyvalue()} value";
			string actual = FormatEntry(template, entry);

			string expected = "Key is:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void KeyValueTokenWithInvalidTemplate()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "Key is: {keyvalue(INVALIDKEY)} value";
			string actual = FormatEntry(template, entry);

			string expected = "Key is:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void KeyValueTokenWithMissingDictionary()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			// do not set extended properties field

			string template = "Key is: {keyvalue(key1)} value";
			string actual = FormatEntry(template, entry);

			string expected = "Key is:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DictionaryTokenWithMissingDictionary()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			// do not set extended properties field

			string template = "Key is: {dictionary({key} - {value}\n)} value";
			string actual = FormatEntry(template, entry);

			string expected = "Key is:  value";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimestampCustomFormat()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string template = "Time is: {timestamp(MM - dd - yyyy @ hh:mm:ss)}";
			string actual = FormatEntry(template, entry);

			string expected = "Time is: 12 - 31 - 9999 @ 11:59:59";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MultipleTimestampCustomFormats()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string template = "Month: {timestamp(MM )}, Day: {timestamp(dd )}, Year: {timestamp(yyyy )}";
			string actual = FormatEntry(template, entry);

			string expected = "Month: 12 , Day: 31 , Year: 9999 ";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MultipleCustomFormats()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

			string template = "Key1 = \"{keyvalue(key1)}\", Month: {timestamp(MM )}, Day: {timestamp(dd )}, Year: {timestamp(yyyy )}";
			string actual = FormatEntry(template, entry);
			string expected = "Key1 = \"value1\", Month: 12 , Day: 31 , Year: 9999 ";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TimestampFormatWithInvalidFormatString()
		{
			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			string actual = FormatEntry("Time is: {timestamp(INVALIDFORMAT)}", entry);

			string expected = "Time is: INVALID";
			Assert.IsTrue(actual.IndexOf(expected) > -1);
		}

		[TestMethod]
		public void TextFormatterWillNotAddErrorMessagesIfTokenIsNotPresent()
		{
			string errorMessage = "ERROR MESSAGE";
			string template = "Message: {message}";

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.AddErrorMessage(errorMessage);
			string actual = FormatEntry(template, entry);

			Assert.IsFalse(actual.IndexOf(errorMessage) > -1);
		}

		[TestMethod]
		public void TextFormatterWillNotAddErrorMessagesIfTokenIsPresentButEntryHasNoMessages()
		{
			string errorMessage = "ERROR MESSAGE";
			string template = "Message: {message}{newline}Errors: {errorMessages}";

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			string actual = FormatEntry(template, entry);

			Assert.IsFalse(actual.IndexOf(errorMessage) > -1);
		}

		[TestMethod]
		public void TextFormatterWillAddErrorMessagesIfTokenIsPresentAndEntryMessages()
		{
			string errorMessage = "ERROR MESSAGE";
			string template = "Message: {message}{newline}Errors: {errorMessages}";

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.AddErrorMessage(errorMessage);
			string actual = FormatEntry(template, entry);

			Assert.IsTrue(actual.IndexOf(errorMessage) > -1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TokenFunctionConstructorArgs1()
		{
			new ExceptionTokenFunction();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TokenFunctionConstructorArgs2()
		{
			new ExceptionTokenFunction(1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TokenFunctionConstructorArgs3()
		{
			new ExceptionTokenFunction("1");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TokenFunctionConstructorArgs4()
		{
			new ExceptionTokenFunction((long)1);
		}


		[TestMethod]
		public void TextFromatterWithoutTemplateUsesDefaultTemplate()
		{
			TextFormatter formatter = new TextFormatter();

            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Title = Guid.NewGuid().ToString();
            entry.AppDomainName = Guid.NewGuid().ToString();
            entry.MachineName = Guid.NewGuid().ToString();
            entry.ManagedThreadName = Guid.NewGuid().ToString();
            entry.Message = Guid.NewGuid().ToString();
            string category = Guid.NewGuid().ToString();
            entry.Categories = new string[] { category };
            entry.ProcessName = Guid.NewGuid().ToString();

            string formattedMessage = formatter.Format(entry);

            Assert.IsTrue(formattedMessage.IndexOf(entry.AppDomainName) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.Title) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.MachineName) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.ManagedThreadName) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.Message) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.Title) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(category) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.ProcessName) != -1);
		}

		[TestMethod]
		public void TextFromatterWithEmptyTemplateUsesDefaultTemplate()
		{
			TextFormatter formatter = new TextFormatter("");

            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Title = Guid.NewGuid().ToString();
            entry.AppDomainName = Guid.NewGuid().ToString();
            entry.MachineName = Guid.NewGuid().ToString();
            entry.ManagedThreadName = Guid.NewGuid().ToString();
            entry.Message = Guid.NewGuid().ToString();
            string category = Guid.NewGuid().ToString();
            entry.Categories = new string[] { category };
            entry.ProcessName = Guid.NewGuid().ToString();

            string formattedMessage = formatter.Format(entry);

            Assert.IsTrue(formattedMessage.IndexOf(entry.AppDomainName) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.Title) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.MachineName) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.ManagedThreadName) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.Message) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.Title) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(category) != -1);
            Assert.IsTrue(formattedMessage.IndexOf(entry.ProcessName) != -1);
		}

		public class ExceptionTokenFunction : TokenFunction
		{
			public ExceptionTokenFunction()
				: base(null)
			{
			}

			public ExceptionTokenFunction(int i)
				: base(null, null)
			{
			}

			public ExceptionTokenFunction(string s)
				: base("")
			{
			}

			public ExceptionTokenFunction(long l)
				: base("", "")
			{
			}

			public override string FormatToken(string tokenTemplate, LogEntry log)
			{
				return null;
			}
		}

		private string FormatEntry(string template, LogEntry entry)
		{
			TextFormatter formatter = new TextFormatter(template);
			return formatter.Format(entry);
		}

		[TestMethod]
		public void FormatCustomTokenFunction()
		{
			CustomLogEntry entry = new CustomLogEntry();

			ILogFormatter formatter = new CustomTextFormatter("Acme custom token template: [[AcmeDBLookup{value1}]]");
			string actual = formatter.Format(entry);

			string expected = "Acme custom token template: 1234";
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FormatterIsReusableBug1769()
		{
			ILogFormatter formatter = new TextFormatter("{message}");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();

			entry.Message = "message1";
			Assert.AreEqual(entry.Message, formatter.Format(entry));

			entry.Message = "message2";
			Assert.AreEqual(entry.Message, formatter.Format(entry));

			entry.Message = "message3";
			Assert.AreEqual(entry.Message, formatter.Format(entry));
		}

		[TestMethod]
		public void TextFormatterHandlesCategoriesForSingleCategoryBug1816()
		{
			ILogFormatter formatter = new TextFormatter("{category}");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "category1" };

			Assert.AreEqual("category1", formatter.Format(entry));
		}

		[TestMethod]
		public void TextFormatterHandlesCategoriesForTwoCategoriesBug1816()
		{
			ILogFormatter formatter = new TextFormatter("{category}");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "category1", "category2" };

			Assert.AreEqual("category1, category2", formatter.Format(entry));
		}

		[TestMethod]
		public void TextFormatterHandlesCategoriesForManyTwoCategoriesBug1816()
		{
			ILogFormatter formatter = new TextFormatter("{category}");

			LogEntry entry = CommonUtil.GetDefaultLogEntry();
			entry.Categories = new string[] { "category1", "category2", "category3", "category4" };

			Assert.AreEqual("category1, category2, category3, category4", formatter.Format(entry));
		}

		[TestMethod]
		public void CanCreateFormatterFromFactory()
		{
			FormatterData data = new TextFormatterData("ignore", "template");
			LoggingSettings settings = new LoggingSettings();
			settings.Formatters.Add(data);
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(LoggingSettings.SectionName, settings);

			ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, "ignore", configurationSource, reflectionCache);

			Assert.IsNotNull(formatter);
			Assert.AreEqual(formatter.GetType(), typeof(TextFormatter));
			Assert.AreEqual("template", ((TextFormatter)formatter).Template);
		}

		[TestMethod]
		public void CanCreateTextFormatterFromUnnamedDataBug1883()
		{
			string template = "{message}";
			TextFormatterData data = new TextFormatterData(template);

			ILogFormatter formatter = LogFormatterCustomFactory.Instance.Create(context, data, null, reflectionCache);

			Assert.AreSame(typeof(TextFormatter), formatter.GetType());
			Assert.AreEqual(template, ((TextFormatter)formatter).Template);
		}

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			LoggingSettings rwLoggingSettings = new LoggingSettings();
			rwLoggingSettings.Formatters.Add(new TextFormatterData("formatter1", "template1"));
			rwLoggingSettings.Formatters.Add(new TextFormatterData("formatter2", "template2"));

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[LoggingSettings.SectionName] = rwLoggingSettings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

			Assert.AreEqual(2, roLoggingSettings.Formatters.Count);
			Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter1"));
			Assert.AreSame(typeof(TextFormatterData), roLoggingSettings.Formatters.Get("formatter1").GetType());
			Assert.AreSame(typeof(TextFormatter), roLoggingSettings.Formatters.Get("formatter1").Type);
			Assert.AreEqual("template1", ((TextFormatterData)roLoggingSettings.Formatters.Get("formatter1")).Template);
			Assert.IsNotNull(roLoggingSettings.Formatters.Get("formatter2"));
			Assert.AreSame(typeof(TextFormatterData), roLoggingSettings.Formatters.Get("formatter2").GetType());
			Assert.AreSame(typeof(TextFormatter), roLoggingSettings.Formatters.Get("formatter2").Type);
			Assert.AreEqual("template2", ((TextFormatterData)roLoggingSettings.Formatters.Get("formatter2")).Template);
		}
	}
}

