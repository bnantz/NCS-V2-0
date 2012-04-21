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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
#if !NUNIT
	using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
	[TestClass]
	public class InstrumentationAttachmentStrategyFixture
	{
		private DictionaryConfigurationSource instrumentationConfigurationSource;
		private InstrumentationAttachmentStrategy strategy;
		private ConfigurationReflectionCache reflectionCache;

		[TestInitialize]
		public void CreateConfigurationSource()
		{
			instrumentationConfigurationSource = new DictionaryConfigurationSource();
			instrumentationConfigurationSource.Add(InstrumentationConfigurationSection.SectionName,
			                                       new InstrumentationConfigurationSection(true, true, true));
			strategy = new InstrumentationAttachmentStrategy();
			reflectionCache = new ConfigurationReflectionCache();
		}

		[TestMethod]
		public void InstrumentationCanBeAttachedWhenNoInstanceNameIsPresent()
		{
			UnnamedApplicationClass applicationObject = new UnnamedApplicationClass();
			strategy.AttachInstrumentation(applicationObject, instrumentationConfigurationSource, reflectionCache);

			Assert.IsTrue(applicationObject.IsWired);
		}

		[TestMethod]
		public void InstrumentationCanBeAttachedWhenInstanceNameIsPresent()
		{
			NamedApplicationClass applicationObject = new NamedApplicationClass();
			strategy.AttachInstrumentation("InstanceName", applicationObject, instrumentationConfigurationSource, reflectionCache);

			Assert.IsTrue(applicationObject.IsWired);
		}

		[TestMethod]
		public void InstrumentationCanBeAttachedToInstrumentationProviderWhenInstanceNameIsPresent()
		{
			ApplicationClass applicationObject = new ApplicationClass();
			strategy.AttachInstrumentation("InstanceName", applicationObject, instrumentationConfigurationSource, reflectionCache);

			Assert.IsTrue(((InstrumentationProvider) (applicationObject.GetInstrumentationEventProvider())).IsWired);
		}

		internal class UnnamedInstrumentationListener
		{
			public UnnamedInstrumentationListener(bool a, bool b, bool c) {}

			[InstrumentationConsumer("TestSubject")]
			public void TestSubjectMethod(object sender, EventArgs e) {}
		}

		internal class NamedInstrumentationListener
		{
			public NamedInstrumentationListener(string instanceName, bool a, bool b, bool c) {}

			[InstrumentationConsumer("TestSubject")]
			public void TestSubjectMethod(object sender, EventArgs e) {}
		}

		[InstrumentationListener(typeof (NamedInstrumentationListener))]
		public class NamedApplicationClass
		{
			[InstrumentationProvider("TestSubject")]
			public event EventHandler<EventArgs> testEvent;

			public bool IsWired
			{
				get { return testEvent != null; }
			}
		}

		[InstrumentationListener(typeof (UnnamedInstrumentationListener))]
		public class UnnamedApplicationClass
		{
			[InstrumentationProvider("TestSubject")]
			public event EventHandler<EventArgs> testEvent;

			public bool IsWired
			{
				get { return testEvent != null; }
			}
		}


		public class ApplicationClass : IInstrumentationEventProvider
		{
			private InstrumentationProvider instrumentationProvider = new InstrumentationProvider();

			public object GetInstrumentationEventProvider()
			{
				return instrumentationProvider;
			}
		}

		[InstrumentationListener(typeof (NamedInstrumentationListener))]
		public class InstrumentationProvider
		{
			[InstrumentationProvider("TestSubject")]
			public event EventHandler<EventArgs> testEvent;

			public bool IsWired
			{
				get { return testEvent != null; }
			}
		}
	}
}