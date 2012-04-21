//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
	[TestClass]
	public class AuthorizationInstrumentationFixture
	{
		private AuthorizationProviderInstrumentationProvider instrumentationProvider;
		private AuthorizationProviderInstrumentationListener disabledInstrumentationListener;
		private AuthorizationProviderInstrumentationListener enabledInstrumentationListener;
		private AppDomainNameFormatter formatter;
		private const string instanceName = "testInstance";
		private string formattedInstanceName;
		private const string identity = "identity";
		private const string taskName = "testTask";
		private const int numberOfEvents = 50;

		[TestInitialize]
		public void SetUp()
		{
			formatter = new AppDomainNameFormatter();
			formattedInstanceName = formatter.CreateName(instanceName);
			instrumentationProvider = new AuthorizationProviderInstrumentationProvider();
			enabledInstrumentationListener = new AuthorizationProviderInstrumentationListener(instanceName, true, true, true, formatter);
			disabledInstrumentationListener = new AuthorizationProviderInstrumentationListener(instanceName, false, false, false, formatter);
		}

		[TestMethod]
		public void AuthorizationCheckDoesNotifyWmiIfEnabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
			{
				FireAuthorizationCheckPerformed();
				eventListener.WaitForEvents();

				Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
			}
		}

		[TestMethod]
		public void AuthorizationCheckDoesNotNotifyWmiIfDisabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
			{
				FireAuthorizationCheckPerformed();
				eventListener.WaitForEvents();

				Assert.AreEqual(0, eventListener.EventsReceived.Count);
			}
		}

		[TestMethod]
		public void AuthorizationCheckDoesUpdatePerformanceCountersIfEnabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

			EnterpriseLibraryPerformanceCounter performanceCounter
				= CreatePerformanceCounter(AuthorizationProviderInstrumentationListener.AuthorizationCheckPerformedCounterName);
			performanceCounter.Clear();
			Assert.AreEqual(0L, performanceCounter.GetValueFor(formattedInstanceName));

			FireAuthorizationCheckPerformed();

			// Timing dependant
			Assert.AreEqual(50L, performanceCounter.GetValueFor(formattedInstanceName));
		}

		[TestMethod]
		public void AuthorizationCheckDoesNotUpdatePerformanceCountersIfDisabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

			EnterpriseLibraryPerformanceCounter performanceCounter
				= CreatePerformanceCounter(AuthorizationProviderInstrumentationListener.AuthorizationCheckPerformedCounterName);
			performanceCounter.Clear();
			Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

			FireAuthorizationCheckPerformed();

			// Timing dependant
			Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
		}

		[TestMethod]
		public void AuthorizationFailureDoesNotifyWmiIfEnabled()
		{

			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
			{
				FireAuthorizationCheckFailed();
				eventListener.WaitForEvents();

				Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
			}
		}

		[TestMethod]
		public void AuthorizationFailureDoesNotNotifyWmiIfDisabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
			{
				FireAuthorizationCheckFailed();
				eventListener.WaitForEvents();

				Assert.AreEqual(0, eventListener.EventsReceived.Count);
			}
		}

		[TestMethod]
		public void AuthorizationFailureDoesUpdatePerformanceCountersIfEnabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

			EnterpriseLibraryPerformanceCounter performanceCounter
				= CreatePerformanceCounter(AuthorizationProviderInstrumentationListener.AuthorizationCheckFailedCounterName);
			performanceCounter.Clear();
			Assert.AreEqual(0L, performanceCounter.GetValueFor(formattedInstanceName));

			FireAuthorizationCheckFailed();

			// Timing dependant
			Assert.AreEqual(50L, performanceCounter.GetValueFor(formattedInstanceName));
		}

		[TestMethod]
		public void AuthorizationFailureDoesNotUpdatePerformanceCountersIfDisabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

			EnterpriseLibraryPerformanceCounter performanceCounter
				= CreatePerformanceCounter(AuthorizationProviderInstrumentationListener.AuthorizationCheckFailedCounterName);
			performanceCounter.Clear();
			Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

			FireAuthorizationCheckFailed();

			// Timing dependant
			Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
		}

		private EnterpriseLibraryPerformanceCounter CreatePerformanceCounter(string counterName)
		{
			return new EnterpriseLibraryPerformanceCounter(
									AuthorizationProviderInstrumentationListener.PerformanceCountersCategoryName,
									counterName,
									formattedInstanceName);
		}

		private void FireAuthorizationCheckPerformed()
		{
			for (int i = 0; i < numberOfEvents; i++)
			{
				try
				{
					instrumentationProvider.FireAuthorizationCheckPerformed(identity, taskName);
				}
				catch (Exception)
				{ }
			}
		}

		private void FireAuthorizationCheckFailed()
		{
			for (int i = 0; i < numberOfEvents; i++)
			{
				try
				{
					instrumentationProvider.FireAuthorizationCheckFailed(identity, taskName);
				}
				catch (Exception)
				{ }
			}
		}
	}
}
