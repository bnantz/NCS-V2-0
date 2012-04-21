//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
	[TestClass]
	public class TraceListenerFactoryFixture
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
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void CreationWithNullLoggingSettingsThrows()
		{
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

			TraceListenerCustomFactory.Instance.Create(context, "listener", configurationSource, reflectionCache);
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void CreationWithMissingTraceListenerDataThrows()
		{
			MockLogObjectsHelper helper = new MockLogObjectsHelper();

			TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);
		}

		[TestMethod]
		public void CreationWithExistingTraceListenerDataSucceeeds()
		{
			MockLogObjectsHelper helper = new MockLogObjectsHelper();
			helper.loggingSettings.TraceListeners.Add(new MockTraceListenerData("listener"));

			TraceListener listener 
				= TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

			Assert.IsNotNull(listener);
			Assert.AreSame(typeof(MockTraceListener), listener.GetType());
		}
	}
}
