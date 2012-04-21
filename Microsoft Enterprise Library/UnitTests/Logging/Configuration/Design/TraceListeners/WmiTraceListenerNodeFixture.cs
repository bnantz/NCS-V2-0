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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class WmiTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInWmiTraceListenerNodeThrows()
        {
            new WmiTraceListenerNode(null);
        }

        [TestMethod]
        public void WmiTraceListenerNodeDefaults()
        {
            WmiTraceListenerNode wmiListener = new WmiTraceListenerNode();

            Assert.AreEqual("WMI TraceListener", wmiListener.Name);
        }

        [TestMethod]
        public void WmiTraceListenerNodeTest()
        {
            string name = "some name";

            WmiTraceListenerNode wmiTraceListenerNode = new WmiTraceListenerNode();
            wmiTraceListenerNode.Name = name;

            ApplicationNode.AddNode(wmiTraceListenerNode);

            WmiTraceListenerData nodeData = (WmiTraceListenerData)wmiTraceListenerNode.TraceListenerData;

            Assert.AreEqual(name, nodeData.Name);
        }

        [TestMethod]
        public void WmiTraceListenerNodeDataTest()
        {
            string name = "some name";

            WmiTraceListenerData wmiTraceListenerData = new WmiTraceListenerData();
            wmiTraceListenerData.Name = name;

            WmiTraceListenerNode wmiTraceListenerNode = new WmiTraceListenerNode(wmiTraceListenerData);
            ApplicationNode.AddNode(wmiTraceListenerNode);

            Assert.AreEqual(name, wmiTraceListenerNode.Name);
        }
    }
}
