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

using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using System;

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
	public class ConfigurationChangeWatcherFixture
	{
		private int notifications;

		[TestInitialize]
		public void SetUp()
		{
			notifications = 0;
		}

		[TestMethod]
		public void RunningWatcherKeepsOnlyOnePollingThread()
		{
			ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(50);
			TestConfigurationChangeWatcher watcher = new TestConfigurationChangeWatcher();
			try
			{
				watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);

				for (int i = 0; i < 20; i++)
				{
					watcher.StopWatching();
					watcher.StartWatching();
				}

				// ramp up
				Thread.Sleep(50);

				watcher.DoNotification();

				// wait for notification
				Thread.Sleep(150);

				Assert.AreEqual(1, notifications);
			}
			finally
			{
				watcher.StopWatching();
				ConfigurationChangeWatcher.ResetDefaultPollDelay();
			}

		}

		private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
		{
			lock (this)
			{
				notifications++;
			}
		}

	}


	class TestConfigurationChangeWatcher : ConfigurationChangeWatcher
	{
		[ThreadStatic]
		static bool notified;

		private bool notify = false;
		private DateTime lastWriteTime = DateTime.Now;

		public override string SectionName
		{
			get { return "section"; }
		}

		protected override System.DateTime GetCurrentLastWriteTime()
		{
			if (notify && !notified)
			{
				TestConfigurationChangeWatcher.notified = true;
				lastWriteTime = DateTime.Now;
			}

			return lastWriteTime;
		}

		protected override string BuildThreadName()
		{
			return "Test Watcher";
		}

		protected override ConfigurationChangedEventArgs BuildEventData()
		{
			return new ConfigurationChangedEventArgs(SectionName);
		}

		protected override string GetEventSourceName()
		{
			throw new System.Exception("The method or operation is not implemented.");
		}

		internal void DoNotification()
		{
			notify = true;
		}
	}
}
