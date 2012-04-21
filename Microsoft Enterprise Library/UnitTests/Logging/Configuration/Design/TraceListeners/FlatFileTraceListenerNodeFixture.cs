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
    public class FlatFileTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInFlatFileTraceListenerNodeThrows()
        {
            new FlatFileTraceListenerNode(null);
        }

        [TestMethod]
        public void FlatFileTraceListenerNodeDefaults()
        {
            FlatFileTraceListenerNode flatFileListener = new FlatFileTraceListenerNode();

            Assert.AreEqual("FlatFile TraceListener", flatFileListener.Name);
            Assert.AreEqual(DefaultValues.FlatFileListenerFileName, flatFileListener.Filename);
            Assert.AreEqual(DefaultValues.FlatFileListenerHeader, flatFileListener.Header);
            Assert.AreEqual(DefaultValues.FlatFileListenerFooter, flatFileListener.Footer);
        }

        [TestMethod]
        public void FlatFileTraceListenerNodeTest()
        {
            string name = "some name";
            string fileName = "some filename";
            string header = "some header";
            string footer = "some footer";

            FlatFileTraceListenerNode flatFileTraceListenerNode = new FlatFileTraceListenerNode();
            flatFileTraceListenerNode.Name = name;
            flatFileTraceListenerNode.Footer = footer;
            flatFileTraceListenerNode.Header = header;
            flatFileTraceListenerNode.Filename = fileName;

            ApplicationNode.AddNode(flatFileTraceListenerNode);

            FlatFileTraceListenerData nodeData = (FlatFileTraceListenerData)flatFileTraceListenerNode.TraceListenerData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(header, nodeData.Header);
            Assert.AreEqual(footer, nodeData.Footer);
            Assert.AreEqual(fileName, nodeData.FileName);
        }

        [TestMethod]
        public void FlatFileTraceListenerNodeDataTest()
        {
            string name = "some name";
            string fileName = "some filename";
            string header = "some header";
            string footer = "some footer";

            FlatFileTraceListenerData flatFileTraceListenerData = new FlatFileTraceListenerData();
            flatFileTraceListenerData.Name = name;
            flatFileTraceListenerData.FileName = fileName;
            flatFileTraceListenerData.Header = header;
            flatFileTraceListenerData.Footer = footer;

            FlatFileTraceListenerNode flatFileTraceListenerNode = new FlatFileTraceListenerNode(flatFileTraceListenerData);
            ApplicationNode.AddNode(flatFileTraceListenerNode);

            Assert.AreEqual(name, flatFileTraceListenerNode.Name);
            Assert.AreEqual(fileName, flatFileTraceListenerNode.Filename);
            Assert.AreEqual(header, flatFileTraceListenerNode.Header);
            Assert.AreEqual(footer, flatFileTraceListenerNode.Footer);
        }
    }
}
