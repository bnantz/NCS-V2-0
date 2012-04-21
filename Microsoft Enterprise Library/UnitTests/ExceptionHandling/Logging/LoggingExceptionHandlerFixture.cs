//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests
{
	[TestClass]
	public class LoggingExceptionHandlerFixture
	{
		[TestMethod]
		public void ExceptionHandledThroughLoggingBlock()
		{
			MockTraceListener.Reset();
			Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));

			Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
			Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains("TestCat"));
			Assert.AreEqual(5, MockTraceListener.LastEntry.EventId);
			Assert.AreEqual(TraceEventType.Error, MockTraceListener.LastEntry.Severity);
			Assert.AreEqual("TestTitle", MockTraceListener.LastEntry.Title);
		}

		[TestMethod]
		[ExpectedException(typeof(ExceptionHandlingException))]
		public void BadFormatterThrowsExceptionWhenHandlingExceptionWithLoggingBlock()
		{
			ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Bad Formatter Logging Policy");
		}

		[TestMethod]
		public void MultipleRequestsUseSameLogWriterInstance()
		{
			MockTraceListener.Reset();

			Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));
			Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));
			Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));

			Assert.AreEqual(3, MockTraceListener.Entries.Count);
			Assert.AreEqual(3, MockTraceListener.Instances.Count);
			Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[1]);
			Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[2]);
		}

		[TestMethod]
		public void LoggedExceptionCopiesExceptionDataForStringKeys()
		{
			MockTraceListener.Reset();

			Exception exception = new Exception("TEST EXCEPTION");
			object value = new object();
			object key4 = new object();
			exception.Data.Add("key1", value);
			exception.Data.Add("key2", "value");
			exception.Data.Add("key3", 3);
			exception.Data.Add(key4, "value");

			Assert.IsFalse(ExceptionPolicy.HandleException(exception, "Logging Policy"));
			Assert.AreEqual(1, MockTraceListener.Entries.Count);
			Assert.AreEqual(3, MockTraceListener.Entries[0].ExtendedProperties.Count);
			Assert.AreEqual(value, MockTraceListener.Entries[0].ExtendedProperties["key1"]);
			Assert.AreEqual("value", MockTraceListener.Entries[0].ExtendedProperties["key2"]);
			Assert.AreEqual(3, MockTraceListener.Entries[0].ExtendedProperties["key3"]);
		}

		[TestMethod]
		public void LoggedExceptionWithoutExceptionDataWorks()
		{
			MockTraceListener.Reset();

			Exception exception = new Exception("TEST EXCEPTION");
			Assert.IsFalse(ExceptionPolicy.HandleException(exception, "Logging Policy"));
			Assert.AreEqual(1, MockTraceListener.Entries.Count);
			Assert.AreEqual(0, MockTraceListener.Entries[0].ExtendedProperties.Count);
		}
	}
}