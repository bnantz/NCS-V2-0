//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Instrumentation
{
	[TestClass]
	public class InstrumentationListenerFixture
	{
		[TestMethod]
		public void CreatesMultipleInstancesGivenAnInstanceNameAtCreation()
		{
			TestInstrumentationListener listener = new TestInstrumentationListener("Foo");
			Assert.AreEqual(2, listener.savedInstanceNames.Length);
		}
		
		[TestMethod]
		public void CreatesOnlySinglePerfCounterInstanceGivenListenerWithNoInstanceName()
		{
			NoNameInstrumentationListener listener = new NoNameInstrumentationListener();
			Assert.AreEqual(1, listener.savedInstanceNames.Length);
		}

		[TestMethod]
		public void CreatesOnlySinglePerfCounterInstanceGivenListenerWithNoDefaultInstanceName()
		{
			NoDefaultNameInstrumentationListener listener = new NoDefaultNameInstrumentationListener("Foo");
			Assert.AreEqual(1, listener.savedInstanceNames.Length);
			Assert.AreEqual("Foo", listener.savedInstanceNames[0]);
		}

		private class TestInstrumentationListener : InstrumentationListener
		{
			public string[] savedInstanceNames;

			public TestInstrumentationListener(string instanceName)
				: base(instanceName, true, true, true, new NoPrefixNameFormatter())
			{
			}

			protected override void CreatePerformanceCounters(string[] instanceNames)
			{
				savedInstanceNames = instanceNames;
			}
		}
		
		private class NoNameInstrumentationListener : InstrumentationListener
		{
			public string[] savedInstanceNames;
			
			public NoNameInstrumentationListener()
			: base(true, true, true, new NoPrefixNameFormatter())
			{
			}

			protected override void CreatePerformanceCounters(string[] instanceNames)
			{
				savedInstanceNames = instanceNames;
			}
		}

		private class NoDefaultNameInstrumentationListener : InstrumentationListener
		{
			public string[] savedInstanceNames;

			public NoDefaultNameInstrumentationListener(string instanceName)
				: base(new string[] { instanceName }, true, true, true, new NoPrefixNameFormatter())
			{
			}

			protected override void CreatePerformanceCounters(string[] instanceNames)
			{
				savedInstanceNames = instanceNames;
			}
		}
	}
}
