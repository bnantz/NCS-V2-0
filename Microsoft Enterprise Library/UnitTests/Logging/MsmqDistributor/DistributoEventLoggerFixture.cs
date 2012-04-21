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

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
	[TestClass]
	public class DistributoEventLoggerFixture
	{
		public const string TestEventLogName = "Application"; //"Test Event Log";
		public const string TestApplicationName = "Test Log Distributor";
		public const string message = "message";


		[TestMethod]
		public void CanCreateDistributorEventLogger()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);
		}

		[TestMethod]
		public void ServiceStartedWritesToEventLog()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				logger.LogServiceStarted();

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
			}
		}

		[TestMethod]
		public void ServiceStartedFiresWmiEvent()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
			{
				logger.LogServiceStarted();

				eventListener.WaitForEvents();
				Assert.AreEqual(1, eventListener.EventsReceived.Count);
				Assert.AreEqual("DistributorServiceLifecycleEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
				Assert.AreEqual(true, eventListener.EventsReceived[0].GetPropertyValue("Started"));
			}
		}

		[TestMethod]
		public void ServiceStoppedWritesToEventLog()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				logger.LogServiceStopped();

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
			}
		}

		[TestMethod]
		public void ServiceStoppedFiresWmiEvent()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
			{
				logger.LogServiceStopped();

				eventListener.WaitForEvents();
				Assert.AreEqual(1, eventListener.EventsReceived.Count);
				Assert.AreEqual("DistributorServiceLifecycleEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
				Assert.AreEqual(false, eventListener.EventsReceived[0].GetPropertyValue("Started"));
			}
		}

		[TestMethod]
		public void ServicePausedWritesToEventLog()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				logger.LogServicePaused();

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
			}
		}

		[TestMethod]
		public void ServicePausedFiresWmiEvent()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
			{
				logger.LogServicePaused();

				eventListener.WaitForEvents();
				Assert.AreEqual(1, eventListener.EventsReceived.Count);
				Assert.AreEqual("DistributorServiceLifecycleEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
				Assert.AreEqual(false, eventListener.EventsReceived[0].GetPropertyValue("Started"));
			}
		}

		[TestMethod]
		public void ServiceResumedWritesToEventLog()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				logger.LogServiceResumed();

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
			}
		}

		[TestMethod]
		public void ServiceResumedFiresWmiEvent()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
			{
				logger.LogServiceResumed();

				eventListener.WaitForEvents();
				Assert.AreEqual(1, eventListener.EventsReceived.Count);
				Assert.AreEqual("DistributorServiceLifecycleEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
				Assert.AreEqual(true, eventListener.EventsReceived[0].GetPropertyValue("Started"));
			}
		}

		[TestMethod]
		public void ServiceFailureWithoutExceptionWritesToEventLog()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				logger.LogServiceFailure(message, null, TraceEventType.Error);

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
			}
		}

		[TestMethod]
		public void ServiceFailureWithExceptionWritesToEventLog()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				logger.LogServiceFailure(message, GetException(), TraceEventType.Error);

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
			}
		}

		[TestMethod]
		public void ServiceFailureWithoutExceptionFiresWmiEvent()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
			{
				logger.LogServiceFailure(message, null, TraceEventType.Error);

				eventListener.WaitForEvents();
				Assert.AreEqual(1, eventListener.EventsReceived.Count);
				Assert.AreEqual("DistributorServiceFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
				Assert.IsTrue(((string)eventListener.EventsReceived[0].GetPropertyValue("FailureMessage")).StartsWith(message));
			}
		}

		[TestMethod]
		public void ServiceFailureWithExceptionFiresWmiEvent()
		{
			DistributorEventLogger logger = new DistributorEventLogger(TestApplicationName);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
			{
				logger.LogServiceFailure(message, GetException(), TraceEventType.Error);

				eventListener.WaitForEvents();
				Assert.AreEqual(1, eventListener.EventsReceived.Count);
				Assert.AreEqual("DistributorServiceFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
				Assert.IsTrue(((string)eventListener.EventsReceived[0].GetPropertyValue("FailureMessage")).StartsWith(message));
			}
		}

		private static EventLog GetEventLog()
		{
			if (!EventLog.Exists(TestEventLogName))
			{
				using (EventLog log = new EventLog(TestEventLogName, ".", TestApplicationName))
				{
					log.WriteEntry("Event Log Created");
				}
			}
			return new EventLog(TestEventLogName);
		}

		private Exception GetException()
		{
			Exception exception = null;
			try
			{
				throw new ArgumentException("argument");
			}
			catch (Exception e)
			{
				exception = e;
			}
			return exception;
		}
	}
}
