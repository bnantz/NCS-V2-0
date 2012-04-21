//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Diagnostics;
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class LoggingExceptionHandlerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInLoggingExceptionHandlerNodehrows()
        {
            new LoggingExceptionHandlerNode(null);
        }

        [TestMethod]
        public void LoggingExceptionHandlerNodeDefaults()
        {
            LoggingExceptionHandlerNode policyNode = new LoggingExceptionHandlerNode();
            Assert.AreEqual("Logging Handler", policyNode.Name);
        }

        [TestMethod]
        public void CanCreateLoggingHandlerNodeFromData()
        {
            ConfigurationNode createdNode = ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(LoggingExceptionHandlerData), new object[] { new LoggingExceptionHandlerData() });
            Assert.IsNotNull(createdNode);
        }

        [TestMethod]
        public void LoggingHandlerNodeDataTest()
        {
            string name = "some name";
            int eventId = 3234;
            int priority = -1;
            Type formatterType = typeof(Guid);
            TraceEventType severity = TraceEventType.Resume;

            LoggingExceptionHandlerData data = new LoggingExceptionHandlerData();
            data.Name = name;
            data.EventId = eventId;
            data.FormatterType = formatterType;
            data.Priority = priority;
            data.Severity = severity;
			data.Title = "Title";
			data.LogCategory = "Category1";

            LoggingExceptionHandlerNode node = new LoggingExceptionHandlerNode(data);
			node.LogCategory = new CategoryTraceSourceNode();
			node.LogCategory.Name = "Category1";
            Assert.AreEqual(data.Name, node.Name);
            Assert.AreEqual(data.EventId, node.EventId);
            Assert.AreEqual(formatterType, node.FormatterType);
            Assert.AreEqual(data.Priority, node.Priority);
            Assert.AreEqual(data.Severity, node.Severity);
			Assert.AreEqual(data.Title, node.Title);
			Assert.AreEqual(data.LogCategory, node.LogCategory.Name);

        }

        [TestMethod]
        public void LoggingHandlerNodeTest()
        {
            string name = "some name";
            int eventId = 3234;
            int priority = -1;
            Type formatterType = typeof(Guid);
            TraceEventType severity = TraceEventType.Resume;


            LoggingExceptionHandlerData loggingHandlerData = new LoggingExceptionHandlerData();
            loggingHandlerData.Name = name;
            loggingHandlerData.EventId = eventId;
            loggingHandlerData.Priority = priority;
            loggingHandlerData.FormatterType = formatterType;
            loggingHandlerData.Severity = severity;
			
            LoggingExceptionHandlerNode exceptionHandlerNode = new LoggingExceptionHandlerNode(loggingHandlerData);
            ApplicationNode.AddNode(exceptionHandlerNode);

            LoggingExceptionHandlerData nodeData = (LoggingExceptionHandlerData) exceptionHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(eventId, nodeData.EventId);
            Assert.AreEqual(formatterType, nodeData.FormatterType);
            Assert.AreEqual(priority, nodeData.Priority);
            Assert.AreEqual(severity, nodeData.Severity);
        }

    }
}
