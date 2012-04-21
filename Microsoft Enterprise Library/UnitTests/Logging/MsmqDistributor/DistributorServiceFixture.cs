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

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
	[TestClass]
	public class DistributorServiceFixture
	{
		private DistributorServiceTestFacade distributorServiceTestFacade;
		private MockMsmqListener mockListener;

		private const int sleepTimer = 300;

		[TestInitialize]
		public void Setup()
		{
			this.distributorServiceTestFacade = new DistributorServiceTestFacade();
			this.mockListener = new MockMsmqListener(this.distributorServiceTestFacade, 1000, CommonUtil.MessageQueuePath);

			this.distributorServiceTestFacade.QueueListener = mockListener;
		}

		[TestCleanup]
		public void Teardown()
		{
			// Reset distribution strategy to in process
			//CommonUtil.SetDistributionStrategy("InProc");
		}

		[TestMethod]
		public void Initialization()
		{
			DistributorServiceTestFacade distributor = new DistributorServiceTestFacade();
			distributor.InitializeComponent();
			Assert.IsNotNull(distributor);
			Assert.AreEqual(ServiceStatus.OK, distributor.Status);

            MsmqDistributorSettings settings = MsmqDistributorSettings.GetSettings(new SystemConfigurationSource());

            Assert.AreEqual(settings.ServiceName, distributor.ApplicationName);

			// force log entry
            distributor.EventLogger.LogServiceFailure(string.Empty, new Exception("simulated exception - forced event logger flush"), TraceEventType.Error);

			Assert.IsTrue(CommonUtil.LogEntryExists(Resources.InitializeComponentStarted), "init begin");
			Assert.IsTrue(CommonUtil.LogEntryExists(Resources.InitializeComponentCompleted), "init end");
		}

		[TestMethod]
		public void StartAndStopService()
		{
			this.distributorServiceTestFacade.OnStart();

			Assert.IsTrue(mockListener.StartCalled, "mock start called");
			Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status, "status");
			Assert.IsTrue(CommonUtil.LogEntryExists(Resources.ValidationStarted), "validate start");
			Assert.IsTrue(CommonUtil.LogEntryExists(Resources.ValidationComplete), "validate complete");
			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStartComplete, distributorServiceTestFacade.ApplicationName)), "start complete");

			this.distributorServiceTestFacade.OnStop();
			Assert.IsTrue(mockListener.StopCalled, "mock stop called");
			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStopComplete, distributorServiceTestFacade.ApplicationName)), "stop complete");
			Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status);
		}

		[TestMethod]
		public void StartServiceWithError()
		{
			mockListener.ExceptionOnStart = true;
			this.distributorServiceTestFacade.OnStart();

			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStartError, distributorServiceTestFacade.ApplicationName)), "start error");
			Assert.IsTrue(CommonUtil.LogEntryExists(Resources.ValidationError), "validate error");

			Assert.AreEqual(ServiceStatus.Shutdown, this.distributorServiceTestFacade.Status);
		}

		[TestMethod]
		public void StopServiceWithError()
		{
			mockListener.ExceptionOnStop = true;

			this.distributorServiceTestFacade.OnStart();
			this.distributorServiceTestFacade.OnStop();

			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStopError, distributorServiceTestFacade.ApplicationName)), "stop error");
			Assert.AreEqual(ServiceStatus.Shutdown, this.distributorServiceTestFacade.Status);
		}

		[TestMethod]
		public void StopServiceWithWarning()
		{
			mockListener.StopReturnsFalse = true;

			this.distributorServiceTestFacade.OnStart();
			this.distributorServiceTestFacade.OnStop();

			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStopWarning, distributorServiceTestFacade.ApplicationName)), "stop warning");
		}

		[TestMethod]
		public void PauseAndContinueService()
		{
			this.distributorServiceTestFacade.OnStart();

			this.distributorServiceTestFacade.OnPause();
			Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status, "status");
			Assert.IsTrue(mockListener.StopCalled, "mock stop called");
			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServicePausedSuccess, distributorServiceTestFacade.ApplicationName)), "start complete");

			this.distributorServiceTestFacade.OnContinue();
			Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status);
			Assert.IsTrue(mockListener.StartCalled, "mock start called");
			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceResumeComplete, distributorServiceTestFacade.ApplicationName)), "stop complete");
		}

		[TestMethod]
		public void PauseServiceWithWarning()
		{
			mockListener.StopReturnsFalse = true;

			this.distributorServiceTestFacade.OnStart();
			this.distributorServiceTestFacade.OnPause();

			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServicePauseWarning, distributorServiceTestFacade.ApplicationName)), "stop warning");
			Assert.AreEqual(ServiceStatus.OK, this.distributorServiceTestFacade.Status);
		}

		[TestMethod]
		public void PauseServiceWithError()
		{
			mockListener.ExceptionOnStop = true;

			this.distributorServiceTestFacade.OnStart();
			this.distributorServiceTestFacade.OnPause();

			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServicePauseError, distributorServiceTestFacade.ApplicationName)), "stop warning");
			Assert.AreEqual(ServiceStatus.Shutdown, this.distributorServiceTestFacade.Status);
		}

		[TestMethod]
		public void ContinueServiceWithError()
		{
			this.distributorServiceTestFacade.OnStart();
			this.distributorServiceTestFacade.OnPause();
			mockListener.ExceptionOnStart = true;
			this.distributorServiceTestFacade.OnContinue();

			Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceResumeError, distributorServiceTestFacade.ApplicationName)), "continue error");
			Assert.AreEqual(ServiceStatus.Shutdown, this.distributorServiceTestFacade.Status);
		}
	}
}
