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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration.Tests
{
	[TestClass]
	public class MsmqDistributorSettingsFixture
	{
		[TestMethod]
		public void CanDeserializeSerializedSettings()
		{
		}


		[TestMethod]
		public void CanReadSettingsFromConfigurationFile()
		{
			MsmqDistributorSettings settings = MsmqDistributorSettings.GetSettings(new SystemConfigurationSource());

			Assert.IsNotNull(settings);
			Assert.AreEqual(CommonUtil.MessageQueuePath, settings.MsmqPath);
			Assert.AreEqual(1000, settings.QueueTimerInterval);
			Assert.AreEqual("Msmq Distributor", settings.ServiceName);
		}
	}
}
