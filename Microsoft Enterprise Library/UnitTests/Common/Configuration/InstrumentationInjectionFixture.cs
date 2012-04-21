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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
	[TestClass]
	public class InstrumentationInjectionFixture
	{
		public static bool wasCalled;
		private DictionaryConfigurationSource configSource;

		[TestInitialize]
		public void SetUp()
		{
			wasCalled = false;
			configSource = new DictionaryConfigurationSource();
			configSource.Add(InstrumentationConfigurationSection.SectionName,
			                 new InstrumentationConfigurationSection(true, true, true));
		}

		[TestMethod]
		public void InstrumentationIsWiredUpCorrectlyWhenConfigurationSectionIsPresent()
		{
			EventSource source
				= EnterpriseLibraryFactory.BuildUp<EventSource>("ignore", configSource);

			Assert.IsTrue(source.IsWired);
		}
		
		[TestMethod]
		public void InstrumentationAvoidsTryingToWireUpToObjectsWithNoListenerDefined()
		{
			NoListenerEventSource source
				= EnterpriseLibraryFactory.BuildUp<NoListenerEventSource>("ignore", configSource);

			Assert.IsFalse(source.IsWired);
		}

		[TestMethod]
		public void InstrumentationIsAttachedWhenInstrumentedAttributeIsInBaseClass()
		{
			DerivedEventSource source = EnterpriseLibraryFactory.BuildUp<DerivedEventSource>("ignore", configSource);
			Assert.IsTrue(source.IsWired);
		}

		[TestMethod]
		public void InstrumentationNotWiredWhenConfigurationSectionNotPresent()
		{
			EventSource source
				= EnterpriseLibraryFactory.BuildUp<EventSource>("ignore", new DictionaryConfigurationSource());

			Assert.IsFalse(source.IsWired);
		}

		[TestMethod]
		public void InstrumentationNotWiredWhenConfigurationValuesAllFalse()
		{
			DictionaryConfigurationSource section = new DictionaryConfigurationSource();
			section.Add(InstrumentationConfigurationSection.SectionName,
				new InstrumentationConfigurationSection(false, false, false));
			
			EventSource source
				= EnterpriseLibraryFactory.BuildUp<EventSource>("ignore", new DictionaryConfigurationSource());

			Assert.IsFalse(source.IsWired);
		}

		[CustomFactory(typeof(MockCustomFactory<DerivedEventSource>))]
		private class DerivedEventSource : EventSource
		{
			public DerivedEventSource() : base()
			{
			}
		}

		[CustomFactory(typeof(MockCustomFactory<NoListenerEventSource>))]
		private class NoListenerEventSource
		{
			public NoListenerEventSource()
			{ }

			[InstrumentationProvider("MySubject")]
			public event EventHandler<EventArgs> TestEvent;

			public bool IsWired { get { return TestEvent != null; } }
		}

		[CustomFactory(typeof(MockCustomFactory<EventSource>))]
		[InstrumentationListener(typeof(EventListener))]
		private class EventSource
		{
			public EventSource()
			{ }

			[InstrumentationProvider("MySubject")]
			public event EventHandler<EventArgs> TestEvent;

			public bool IsWired { get { return TestEvent != null; } }
		}

		public class EventListener
		{
			public EventListener(string instanceName, bool a, bool b, bool c)
			{
			}

			[InstrumentationConsumer("MySubject")]
			public void CallMe(object sender, EventArgs e) { wasCalled = true; }
		}

		public class MockCustomFactory<T> : ICustomFactory
			where T : new()
		{
			public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
			{
				return new T();
			}
		}
	}
}
