//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Threading;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif


namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
	[TestClass]
    public class BackgroundSchedulerFixture
    {
		[TestMethod]
        public void SchedulerCanBeStoppedWhenRequested()
        {
            BackgroundScheduler scheduler = new BackgroundScheduler(null, null, null);
            scheduler.Start();
            Thread.Sleep(2500);

            Assert.IsTrue(scheduler.IsActive);

            scheduler.Stop();
            Thread.Sleep(10000);

            Assert.IsFalse(scheduler.IsActive);
        }
    }
}


