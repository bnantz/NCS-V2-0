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
using System.Management;
using System.Management.Instrumentation;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{    
    [TestClass]
    public class WMIListenerFixture
    {
        private bool wmiLogged;
        private string wmiResult;
        private string wmiPath = @"\root\EnterpriseLibrary";
        private ManagementBaseObject wmiLogEntry;
        
        private void BlockUntilWMIEventArrives()
        {
            // loop and poll the event watcher
            bool loop = true;
            int count = 0;
            int timeoutCount = 100;
            while (loop)
            {
                // keep looping until the event handler gets called or we reach our timeout
                loop = !this.wmiLogged && (count++ < timeoutCount);
                Thread.Sleep(100);
            }
        }

        private void watcher_EventArrived(object sender, EventArrivedEventArgs args)
        {
            wmiLogged = true;
            wmiResult = args.NewEvent.GetText(TextFormat.Mof);
            wmiLogEntry = args.NewEvent;
        }

        private void SendLogEntry(WmiTraceListener listener, LogEntry logEntry)
        {
            ManagementScope scope = new ManagementScope(@"\\." + this.wmiPath);
            scope.Options.EnablePrivileges = true;

            StringBuilder sb = new StringBuilder("SELECT * FROM ");
            sb.Append("LogEntryV20");
            string query = sb.ToString();
            EventQuery eq = new EventQuery(query);

            using (ManagementEventWatcher watcher = new ManagementEventWatcher(scope, eq))
            {
                watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
                watcher.Start();

                LogSource source = new LogSource("notfromconfig", SourceLevels.All);
                source.Listeners.Add(listener);
                source.TraceData(TraceEventType.Error, 1, logEntry);

                BlockUntilWMIEventArrives();

                watcher.Stop();
            }
        }

        [TestInitialize]
        public void ResetLogEntryInfo()
        {
            wmiLogged = false;
            wmiResult = string.Empty;
            wmiPath = @"\root\EnterpriseLibrary";
            wmiLogEntry = null;
        }

        [TestMethod]
        public void TestWMIEventOccurred()
        {
            WmiTraceListener listener = new WmiTraceListener();

            LogEntry logEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            SendLogEntry(listener, logEntry);

            Assert.IsTrue(this.wmiLogged);
            Assert.IsTrue(this.wmiResult.IndexOf("message") > -1);
        }


        [TestMethod]
         public void AttributeLookupWillNotHaveAnEffect()
         {
             WmiTraceListener listener = new WmiTraceListener();
             listener.Attributes.Add("formatter", "nonexistent");
         }
        
        [TestMethod]
        public void TestCanGetActivityIdStringWithWMI()
        {
            WmiTraceListener listener = new WmiTraceListener();

            LogEntry logEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            Guid logEntryGuid = Guid.NewGuid();
            logEntry.ActivityId = logEntryGuid;
            SendLogEntry(listener, logEntry);

            Assert.IsTrue(this.wmiLogged);
            Assert.AreEqual(this.wmiLogEntry.GetPropertyValue("ActivityIdString"), logEntryGuid.ToString());
        }

        [TestMethod]
        public void TestCanGetcategoriesStringsWithWMI()
        {
            WmiTraceListener listener = new WmiTraceListener();

            LogEntry logEntry = new LogEntry("message", new string[] { "cat1", "cat2", "cat3" }, 0, 0, TraceEventType.Error, "title", null);
            SendLogEntry(listener, logEntry);

            Assert.IsTrue(this.wmiLogged);
            string[] categoriesStrings = (string[])this.wmiLogEntry.GetPropertyValue("CategoriesStrings");
            Assert.AreEqual(categoriesStrings.Length, logEntry.Categories.Count);
        }

        [TestMethod]
        public void TestLoggingACustomLogEntry()
        {
            WmiTraceListener listener = new WmiTraceListener();

            MyCustomLogEntry logEntry = new MyCustomLogEntry();
            logEntry.MyName = "Enterprise Library Tester";
            SendLogEntry(listener, logEntry);

            Assert.IsTrue(this.wmiLogged);
            Assert.AreEqual(this.wmiLogEntry.GetPropertyValue("MyName"), logEntry.MyName);
        }
    }
}