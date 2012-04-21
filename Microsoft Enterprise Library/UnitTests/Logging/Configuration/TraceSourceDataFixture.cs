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

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
	[TestClass]
	public class TraceSourceDataFixture
	{
		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			string name = "name";

			TraceSourceData data = new TraceSourceData(name, SourceLevels.Critical);
			data.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
			data.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

			LoggingSettings settings = new LoggingSettings();
			settings.TraceSources.Add(data);

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[LoggingSettings.SectionName] = settings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			LoggingSettings roSettigs = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

			Assert.AreEqual(1, roSettigs.TraceSources.Count);
			Assert.IsNotNull(roSettigs.TraceSources.Get(name));
			Assert.AreEqual(SourceLevels.Critical, roSettigs.TraceSources.Get(name).DefaultLevel);
			Assert.AreEqual(2, roSettigs.TraceSources.Get(name).TraceListeners.Count);
			Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener1"));
			Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener2"));
		}
	}

}

