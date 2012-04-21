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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class SystemDiagnosticsTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInSystemDiagnosticsTraceListenerNodeThrows()
        {
            new SystemDiagnosticsTraceListenerNode(null);
        }

        [TestMethod]
        public void SystemDiagnosticsTraceListenerNodeDefaults()
        {
            SystemDiagnosticsTraceListenerNode systemDiagnosticsListener = new SystemDiagnosticsTraceListenerNode();

            Assert.AreEqual("System.Diagnostics TraceListener", systemDiagnosticsListener.Name);
            Assert.AreEqual(string.Empty, systemDiagnosticsListener.InitData);
            Assert.IsNull(systemDiagnosticsListener.Type);
        }

        [TestMethod]
        public void SystemDiagnosticsTraceListenerNodeTest()
        {
            string name = "some name";
            string initData = "some initData";
            Type type = typeof(ConsoleTraceListener);

            SystemDiagnosticsTraceListenerNode systemDiagnosticsTraceListenerNode = new SystemDiagnosticsTraceListenerNode();
            systemDiagnosticsTraceListenerNode.Name = name;
            systemDiagnosticsTraceListenerNode.Type = type;
            systemDiagnosticsTraceListenerNode.InitData = initData;

            ApplicationNode.AddNode(systemDiagnosticsTraceListenerNode);

            SystemDiagnosticsTraceListenerData nodeData = (SystemDiagnosticsTraceListenerData)systemDiagnosticsTraceListenerNode.TraceListenerData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(initData, nodeData.InitData);
        }

        [TestMethod]
        public void SystemDiagnosticsTraceListenerNodeDataTest()
        {
            string name = "some name";
            string initData = "some initData";
            Type type = typeof(ConsoleTraceListener);

            SystemDiagnosticsTraceListenerData systemDiagnosticsTraceListenerData = new SystemDiagnosticsTraceListenerData();
            systemDiagnosticsTraceListenerData.Name = name;
            systemDiagnosticsTraceListenerData.Type = type;
            systemDiagnosticsTraceListenerData.InitData = initData;

            SystemDiagnosticsTraceListenerNode systemDiagnosticsTraceListenerNode = new SystemDiagnosticsTraceListenerNode(systemDiagnosticsTraceListenerData);
            ApplicationNode.AddNode(systemDiagnosticsTraceListenerNode);

            Assert.AreEqual(name, systemDiagnosticsTraceListenerNode.Name);
            Assert.AreEqual(type, systemDiagnosticsTraceListenerNode.Type);
            Assert.AreEqual(initData, systemDiagnosticsTraceListenerNode.InitData);
        }
    }
}
