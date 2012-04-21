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
    public class EmailTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInEmailTraceListenerNodeThrows()
        {
            new EmailTraceListenerNode(null);
        }

        [TestMethod]
        public void EmailTraceListenerNodeDefaults()
        {
            EmailTraceListenerNode emailListener = new EmailTraceListenerNode();

            Assert.AreEqual("Email TraceListener", emailListener.Name);
            Assert.AreEqual(string.Empty, emailListener.SubjectLineEnder);
            Assert.AreEqual(string.Empty, emailListener.SubjectLineStarter);
            Assert.AreEqual(DefaultValues.EmailListenerToAddress, emailListener.ToAddress);
            Assert.AreEqual(DefaultValues.EmailListenerFromAddress, emailListener.FromAddress);
            Assert.AreEqual(DefaultValues.EmailListenerSmtpPort, emailListener.SmtpPort);
            Assert.AreEqual(DefaultValues.EmailListenerSmtpAddress, emailListener.SmtpServer);
        }

        [TestMethod]
        public void EmailTraceListenerNodeTest()
        {
            string name = "some name";
            string subjectSuffix = "subject suffix";
            string subjectPrefix = "subject prefix";
            string toAddress = "some to address";
            string fromAddress = "some from address";
            string smtpServer = "some server";
            int smtpPort = 123;

            EmailTraceListenerNode emailTraceListenerNode = new EmailTraceListenerNode();
            emailTraceListenerNode.Name = name;
            emailTraceListenerNode.SubjectLineStarter = subjectPrefix;
            emailTraceListenerNode.SubjectLineEnder = subjectSuffix;
            emailTraceListenerNode.ToAddress = toAddress;
            emailTraceListenerNode.FromAddress = fromAddress;
            emailTraceListenerNode.SmtpServer = smtpServer;
            emailTraceListenerNode.SmtpPort = smtpPort;

            ApplicationNode.AddNode(emailTraceListenerNode);

            EmailTraceListenerData nodeData = (EmailTraceListenerData)emailTraceListenerNode.TraceListenerData;


            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(subjectSuffix, nodeData.SubjectLineEnder);
            Assert.AreEqual(subjectPrefix, nodeData.SubjectLineStarter);
            Assert.AreEqual(toAddress, nodeData.ToAddress);
            Assert.AreEqual(fromAddress, nodeData.FromAddress);
            Assert.AreEqual(smtpPort, nodeData.SmtpPort);
            Assert.AreEqual(smtpServer, nodeData.SmtpServer);
        }

        [TestMethod]
        public void EmailTraceListenerNodeDataTest()
        {
            string name = "some name";
            string subjectSuffix = "subject suffix";
            string subjectPrefix = "subject prefix";
            string toAddress = "some to address";
            string fromAddress = "some from address";
            string smtpServer = "some server";
            int smtpPort = 123;

            EmailTraceListenerData emailTraceListenerData = new EmailTraceListenerData();
            emailTraceListenerData.Name = name;
            emailTraceListenerData.ToAddress = toAddress;
            emailTraceListenerData.FromAddress = fromAddress;
            emailTraceListenerData.SmtpServer = smtpServer;
            emailTraceListenerData.SmtpPort = smtpPort;
            emailTraceListenerData.SubjectLineEnder = subjectSuffix;
            emailTraceListenerData.SubjectLineStarter = subjectPrefix;

            EmailTraceListenerNode emailTraceListenerNode = new EmailTraceListenerNode(emailTraceListenerData);
            ApplicationNode.AddNode(emailTraceListenerNode);

            Assert.AreEqual(name, emailTraceListenerNode.Name);
            Assert.AreEqual(subjectSuffix, emailTraceListenerNode.SubjectLineEnder);
            Assert.AreEqual(subjectPrefix, emailTraceListenerNode.SubjectLineStarter);
            Assert.AreEqual(toAddress, emailTraceListenerNode.ToAddress);
            Assert.AreEqual(fromAddress, emailTraceListenerNode.FromAddress);
            Assert.AreEqual(smtpPort, emailTraceListenerNode.SmtpPort);
            Assert.AreEqual(smtpServer, emailTraceListenerNode.SmtpServer);

        }
    }
}
