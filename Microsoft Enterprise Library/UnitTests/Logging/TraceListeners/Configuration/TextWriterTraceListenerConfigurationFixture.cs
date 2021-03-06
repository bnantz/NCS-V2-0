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

using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
	[TestClass]
	public class TextWriterTraceListenerConfigurationFixture
	{
		private IBuilderContext context;
		private ConfigurationReflectionCache reflectionCache;

		[TestInitialize]
		public void SetUp()
		{
			context = new MockBuilderContext();
			reflectionCache = new ConfigurationReflectionCache();
		}

		[TestMethod]
		public void CanCreateInstanceFromGivenName()
		{
			SystemDiagnosticsTraceListenerData listenerData 
				= new SystemDiagnosticsTraceListenerData("listener", typeof(TextWriterTraceListener), "log.txt");
			listenerData.TraceOutputOptions = TraceOptions.Callstack;

			MockLogObjectsHelper helper = new MockLogObjectsHelper();
			helper.loggingSettings.TraceListeners.Add(listenerData);

			TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

			Assert.IsNotNull(listener);
			Assert.AreEqual(listener.GetType(), typeof(TextWriterTraceListener));
			Assert.AreEqual("listener", listener.Name);
			Assert.AreEqual(TraceOptions.Callstack, listener.TraceOutputOptions);
		}

		[TestMethod]
		public void CanCreateInstanceFromGivenConfiguration()
		{
			SystemDiagnosticsTraceListenerData listenerData
				= new SystemDiagnosticsTraceListenerData("listener", typeof(TextWriterTraceListener), "log.txt");
			listenerData.TraceOutputOptions = TraceOptions.Callstack;

			MockLogObjectsHelper helper = new MockLogObjectsHelper();

			TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, listenerData, helper.configurationSource,reflectionCache);

			Assert.IsNotNull(listener);
			Assert.AreEqual(listener.GetType(), typeof(TextWriterTraceListener));
			Assert.AreEqual("listener", listener.Name);
			Assert.AreEqual(TraceOptions.Callstack, listener.TraceOutputOptions);
		}

		[TestMethod]
		public void CanCreateInstanceFromConfigurationFile()
		{
			SystemDiagnosticsTraceListenerData listenerData 
				= new SystemDiagnosticsTraceListenerData("listener", typeof(TextWriterTraceListener), "log.txt");
			listenerData.TraceOutputOptions = TraceOptions.Callstack;

			LoggingSettings loggingSettings = new LoggingSettings();
			loggingSettings.TraceListeners.Add(listenerData);

			TraceListener listener
				= TraceListenerCustomFactory.Instance.Create(context, "listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings), reflectionCache);

			Assert.IsNotNull(listener);
			Assert.AreEqual(listener.GetType(), typeof(TextWriterTraceListener));
			Assert.AreEqual(TraceOptions.Callstack, listener.TraceOutputOptions);
		}
	}
}
