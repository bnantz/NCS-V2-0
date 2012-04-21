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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
	[TestClass]
	public class TraceSourceFactoryFixture
	{
		private IBuilderContext context;
		private ConfigurationReflectionCache reflectionCache;
		private TraceListenerCustomFactory.TraceListenerCache traceListenerCache;

		[TestInitialize]
		public void SetUp()
		{
			context = new MockBuilderContext();
			reflectionCache = new ConfigurationReflectionCache();
			traceListenerCache = TraceListenerCustomFactory.CreateTraceListenerCache(3);
		}

		[TestMethod]
		public void CreatedTraceSourceWithNoListenersFromConfigurationIsEmpty()
		{
			MockLogObjectsHelper helper = new MockLogObjectsHelper();
			
			TraceSourceData sourceData = new TraceSourceData("notfromconfiguration", SourceLevels.All);

			LogSource traceSource
				= LogSourceCustomFactory.Instance.Create(context, sourceData, helper.configurationSource, reflectionCache, traceListenerCache);

			Assert.IsNotNull(traceSource);
			Assert.AreEqual("notfromconfiguration", traceSource.Name);
			Assert.AreEqual(SourceLevels.All, traceSource.Level);
			Assert.AreEqual(0, traceSource.Listeners.Count);
		}

		[TestMethod]
		public void CreatedTraceSourceWithListenersFromConfigurationHasCorrectCountOfListeners()
		{
			MockLogObjectsHelper helper = new MockLogObjectsHelper();

			helper.loggingSettings.TraceListeners.Add(new MockTraceListenerData("mock1"));
			helper.loggingSettings.TraceListeners.Add(new MockTraceListenerData("mock2"));

			TraceSourceData sourceData = new TraceSourceData("notfromconfiguration", SourceLevels.All);
			sourceData.TraceListeners.Add(new TraceListenerReferenceData("mock1"));
			sourceData.TraceListeners.Add(new TraceListenerReferenceData("mock2"));

			LogSource traceSource
				= LogSourceCustomFactory.Instance.Create(context, sourceData, helper.configurationSource, reflectionCache, traceListenerCache);

			Assert.IsNotNull(traceSource);
			Assert.AreEqual("notfromconfiguration", traceSource.Name);
			Assert.AreEqual(SourceLevels.All, traceSource.Level);
			Assert.AreEqual(2, traceSource.Listeners.Count);
			Assert.AreSame(typeof(MockTraceListener), traceSource.Listeners[0].GetType());
			Assert.AreEqual("mock1", traceSource.Listeners[0].Name);
			Assert.AreSame(typeof(MockTraceListener), traceSource.Listeners[1].GetType());
			Assert.AreEqual("mock2", traceSource.Listeners[1].Name);
		}

		[TestMethod]
		public void ManyTraceSourcesWithReferenceToSameTraceListenerGetSameInstanceIfSharingCache()
		{
			MockLogObjectsHelper helper = new MockLogObjectsHelper();

			helper.loggingSettings.TraceListeners.Add(new MockTraceListenerData("mock1"));
			helper.loggingSettings.TraceListeners.Add(new MockTraceListenerData("mock2"));

			TraceSourceData sourceData1 = new TraceSourceData("notfromconfiguration1", SourceLevels.All);
			sourceData1.TraceListeners.Add(new TraceListenerReferenceData("mock1"));
			sourceData1.TraceListeners.Add(new TraceListenerReferenceData("mock2"));

			TraceSourceData sourceData2 = new TraceSourceData("notfromconfiguration2", SourceLevels.All);
			sourceData2.TraceListeners.Add(new TraceListenerReferenceData("mock1"));

			LogSource traceSource1
				= LogSourceCustomFactory.Instance.Create(context, sourceData1, helper.configurationSource, reflectionCache, traceListenerCache);
			LogSource traceSource2
				= LogSourceCustomFactory.Instance.Create(context, sourceData2, helper.configurationSource, reflectionCache, traceListenerCache);

			Assert.IsNotNull(traceSource1);
			Assert.IsNotNull(traceSource2);
			Assert.IsFalse(traceSource1 == traceSource2);
			Assert.AreEqual("mock1", traceSource1.Listeners[0].Name);
			Assert.AreEqual("mock1", traceSource2.Listeners[0].Name);
			Assert.AreSame(traceSource1.Listeners[0], traceSource2.Listeners[0]);
		}
	}
}
