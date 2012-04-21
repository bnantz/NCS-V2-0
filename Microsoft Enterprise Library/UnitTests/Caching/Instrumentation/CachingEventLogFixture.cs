//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation.Tests
{
	[TestClass]
	public class CachingEventLogFixture
	{
		private const string instanceName = "test";
		private const string exceptionMessage = "exception message";
		private const string errorMessage = "error message";
		private const string key = "key";

		private NoPrefixNameFormatter formatter;

		[TestInitialize]
		public void SetUp()
		{
			formatter = new NoPrefixNameFormatter();
		}

		[TestMethod]
		public void CacheFailureWithInstrumentationDisabledDoesNotWriteToEventLog()
		{
			CachingInstrumentationListener listener
				= new CachingInstrumentationListener(instanceName, false, false, false, formatter);
			Exception exception = new Exception(exceptionMessage);

			CacheFailureEventArgs args = new CacheFailureEventArgs(errorMessage, exception);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				listener.CacheFailed(null, args);

				Assert.AreEqual(eventCount, eventLog.Entries.Count);
			}
		}

		[TestMethod]
		public void CacheFailureWithInstrumentationEnabledDoesWriteToEventLog()
		{
			CachingInstrumentationListener listener
				= new CachingInstrumentationListener(instanceName, false, true, false, formatter);
			Exception exception = new Exception(exceptionMessage);

			CacheFailureEventArgs args = new CacheFailureEventArgs(errorMessage, exception);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				listener.CacheFailed(null, args);

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
				Assert.IsTrue(eventLog.Entries[eventCount].Message.IndexOf(exceptionMessage) > -1);
			}
		}

		[TestMethod]
		public void CacheCallbackFailureWithInstrumentationDisabledDoesNotWriteToEventLog()
		{
			CachingInstrumentationListener listener
				= new CachingInstrumentationListener(instanceName, false, false, false, formatter);
			Exception exception = new Exception(exceptionMessage);

			CacheCallbackFailureEventArgs args = new CacheCallbackFailureEventArgs(errorMessage, exception);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				listener.CacheCallbackFailed(null, args);

				Assert.AreEqual(eventCount, eventLog.Entries.Count);
			}
		}

		[TestMethod]
		public void CacheCallbackFailureWithInstrumentationEnabledDoesWriteToEventLog()
		{
			CachingInstrumentationListener listener
				= new CachingInstrumentationListener(instanceName, false, true, false, formatter);
			Exception exception = new Exception(exceptionMessage);

			CacheCallbackFailureEventArgs args = new CacheCallbackFailureEventArgs(key, exception);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				listener.CacheCallbackFailed(null, args);

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
				Assert.IsTrue(eventLog.Entries[eventCount].Message.IndexOf(exceptionMessage) > -1);
			}
		}

		private static EventLog GetEventLog()
		{
			return new EventLog("Application", ".", CachingInstrumentationListener.EventLogSourceName);
		}
	}
}
