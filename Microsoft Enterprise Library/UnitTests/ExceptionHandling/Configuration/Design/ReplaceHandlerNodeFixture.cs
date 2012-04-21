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

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ReplaceHandlerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInReplaceHandlerNodeThrows()
        {
            new ReplaceHandlerNode(null);
        }

        [TestMethod]
        public void ReplaceHandlerNodeDefaults()
        {
            ReplaceHandlerNode replaceHandler = new ReplaceHandlerNode();

            Assert.AreEqual(string.Empty, replaceHandler.ExceptionMessage);
            Assert.AreEqual(null, replaceHandler.ReplaceExceptionType);
            Assert.AreEqual("Replace Handler", replaceHandler.Name);
        }

        [TestMethod]
        public void ReplaceHandlerDataTest()
        {
            string name = "some name";
            string message = "some message";
            Type replaceExceptionType = typeof(ApplicationException);

            ReplaceHandlerData data = new ReplaceHandlerData();
            data.Name = name;
            data.ExceptionMessage = message;
            data.ReplaceExceptionType = replaceExceptionType;


            ReplaceHandlerNode node = new ReplaceHandlerNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(message, node.ExceptionMessage);
            Assert.AreEqual(replaceExceptionType, node.ReplaceExceptionType);
        }		

        [TestMethod]
        public void ReplaceHandlerNodeDataTest()
        {
            string name = "some name";
            string message = "some message";
            Type replaceExceptionType = typeof(ApplicationException);

            ReplaceHandlerData replaceHandlerData = new ReplaceHandlerData();
            replaceHandlerData.Name = name;
            replaceHandlerData.ExceptionMessage = message;
            replaceHandlerData.ReplaceExceptionType = replaceExceptionType;

            ReplaceHandlerNode replaceHandlerNode = new ReplaceHandlerNode(replaceHandlerData);

            ReplaceHandlerData nodeData = (ReplaceHandlerData) replaceHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(message, nodeData.ExceptionMessage);
            Assert.AreEqual(replaceExceptionType, nodeData.ReplaceExceptionType);
        }
    }
}
