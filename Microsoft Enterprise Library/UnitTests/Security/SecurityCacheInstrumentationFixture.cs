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
	public class SecurityCacheInstrumentationFixture
	{
		private SecurityCacheProviderInstrumentationProvider instrumentationProvider;
		private SecurityCacheProviderInstrumentationListener disabledInstrumentationListener;
		private SecurityCacheProviderInstrumentationListener enabledInstrumentationListener;
		private AppDomainNameFormatter formatter;
		private const string instanceName = "testInstance";
		private string formattedInstanceName;
		private const int numberOfEvents = 50;

		[TestInitialize]
		public void SetUp()
		{
			formatter = new AppDomainNameFormatter();
			formattedInstanceName = formatter.CreateName(instanceName);
			instrumentationProvider = new SecurityCacheProviderInstrumentationProvider();
			enabledInstrumentationListener = new SecurityCacheProviderInstrumentationListener(instanceName, true, true, true, formatter);
			disabledInstrumentationListener = new SecurityCacheProviderInstrumentationListener(instanceName, false, false, false, formatter);
		}

		[TestMethod]
		public void SecurityCacheCheckDoesNotifyWmiIfEnabled()
		{

			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
			{
				FireSecurityCacheReadPerformed();
				eventListener.WaitForEvents();

				Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
			}
		}

		[TestMethod]
		public void SecurityCacheCheckDoesNotNotifyWmiIfDisabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

			using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
			{
				FireSecurityCacheReadPerformed();
				eventListener.WaitForEvents();

				Assert.AreEqual(0, eventListener.EventsReceived.Count);
			}
		}

		[TestMethod]
		public void SecurityCacheCheckDoesUpdatePerformanceCountersIfEnabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, enabledInstrumentationListener);

			EnterpriseLibraryPerformanceCounter performanceCounter
				= CreatePerformanceCounter(SecurityCacheProviderInstrumentationListener.SecurityCacheReadPerformedCounterName);
				performanceCounter.Clear();
				Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

				FireSecurityCacheReadPerformed();

				// Timing dependant
				Assert.IsFalse(performanceCounter.GetValueFor(formattedInstanceName) == 0);
		}

		[TestMethod]
		public void SecurityCacheCheckDoesNotUpdatePerformanceCountersIfDisabled()
		{
			new ReflectionInstrumentationBinder().Bind(instrumentationProvider, disabledInstrumentationListener);

			EnterpriseLibraryPerformanceCounter performanceCounter
				= CreatePerformanceCounter(SecurityCacheProviderInstrumentationListener.SecurityCacheReadPerformedCounterName);
				performanceCounter.Clear();
				Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);

				FireSecurityCacheReadPerformed();

				// Timing dependant
				Assert.IsTrue(performanceCounter.GetValueFor(formattedInstanceName) == 0);
		}

		private EnterpriseLibraryPerformanceCounter CreatePerformanceCounter(string counterName)
		{
			return new EnterpriseLibraryPerformanceCounter(
									SecurityCacheProviderInstrumentationListener.PerfomanceCountersCategoryName,
									counterName,
									formattedInstanceName);
		}

		private void FireSecurityCacheReadPerformed()
		{
			for (int i = 0; i < numberOfEvents; i++)
			{
				try
				{
					instrumentationProvider.FireSecurityCacheReadPerformed(SecurityEntityType.Identity, null);
				}
				catch (Exception)
				{ }
			}
		}

	}
}
