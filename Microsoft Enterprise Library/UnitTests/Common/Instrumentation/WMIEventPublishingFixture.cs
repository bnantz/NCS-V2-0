//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Management.Instrumentation;
using System.Management;
using System.Threading;
using System.Collections.Generic;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class WMIEventExplorationFixture
    {
        [TestMethod]
        public void FireSimpleEvent()
        {
            using (WmiEventWatcher watcher = new WmiEventWatcher(1))
            {
                BaseWmiEvent myEvent = new TestEvent("Hello, World");
                System.Management.Instrumentation.Instrumentation.Fire(myEvent);

                watcher.WaitForEvents();
            }
        }

        [TestMethod]
        public void Send100Events()
        {
            using (WmiEventWatcher watcher = new WmiEventWatcher(100))
            {
                for (int i = 0; i < 100; i++)
                {
                    BaseWmiEvent myEvent = new TestEvent("" + i);
                    System.Management.Instrumentation.Instrumentation.Fire(myEvent);
                }

                watcher.WaitForEvents();

                Assert.AreEqual(100, watcher.EventsReceived.Count);
                Assert.AreEqual("50", watcher.EventsReceived[50].Properties["Text"].Value);
            }
        }
    }
}
