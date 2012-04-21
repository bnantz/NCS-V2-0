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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	[TestClass]
	public class ExceptionHandlerInstrumentationListenerFixture
	{
		private const string policyName = "policy";
		private const string exceptionMessage = "exception message";

		private ExceptionPolicyImpl exceptionPolicy;

		[TestInitialize]
		public void SetUp()
		{
			IExceptionHandler[] handlers = new IExceptionHandler[] { new MockThrowingExceptionHandler() };
			ExceptionPolicyEntry policyEntry = new ExceptionPolicyEntry(PostHandlingAction.None, handlers);
			Dictionary<Type, ExceptionPolicyEntry> policyEntries = new Dictionary<Type,ExceptionPolicyEntry>();
			policyEntries.Add(typeof(ArgumentException), policyEntry);

			exceptionPolicy = new ExceptionPolicyImpl(policyName, policyEntries);

			ReflectionInstrumentationAttacher attacher
				= new ReflectionInstrumentationAttacher(
					exceptionPolicy.GetInstrumentationEventProvider(),
					typeof(ExceptionHandlingInstrumentationListener),
					new object[] { policyName, true, true, true });
			attacher.BindInstrumentation();
		}

		[TestMethod]
		public void FailureHandlingExceptionWritesToEventLog()
		{
			ArgumentException exception = new ArgumentException(exceptionMessage);

			using (EventLog eventLog = GetEventLog())
			{
				int eventCount = eventLog.Entries.Count;

				try
				{
					exceptionPolicy.HandleException(exception);
				}
				catch (ExceptionHandlingException)
				{ }

				Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
				Assert.IsTrue(eventLog.Entries[eventCount].Message.IndexOf(exceptionMessage) > -1);
			}
		}

		private static EventLog GetEventLog()
		{
			return new EventLog("Application", ".", "Enterprise Library ExceptionHandling");
		}
	}
}
