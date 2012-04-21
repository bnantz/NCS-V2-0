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
    public class WrapHandlerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInWrapHandlerNodeThrows()
        {
            new WrapHandlerNode(null);
        }

        [TestMethod]
        public void WrapHandlerNodeDefaults()
        {
            WrapHandlerNode wrapHandler = new WrapHandlerNode();

            Assert.AreEqual(string.Empty, wrapHandler.ExceptionMessage);
            Assert.AreEqual(null, wrapHandler.WrapExceptionType);
            Assert.AreEqual("Wrap Handler", wrapHandler.Name);
        }

        [TestMethod]
        public void WrapHandlerDataTest()
        {
            string name = "some name";
            string message = "some message";
            Type exceptionType = typeof(AppDomainUnloadedException);
            Type wrapExceptionType = typeof(ApplicationException);

            WrapHandlerData data = new WrapHandlerData();
            data.Name = name;
            data.ExceptionMessage = message;
            data.Type = exceptionType;
            data.WrapExceptionType = wrapExceptionType;


            WrapHandlerNode node = new WrapHandlerNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(message, node.ExceptionMessage);
            Assert.AreEqual(wrapExceptionType, node.WrapExceptionType);
        }

        [TestMethod]
        public void WrapHandlerNodeDataTest()
        {
            string name = "some name";
            string message = "some message";
            Type wrapExceptionType = typeof(ApplicationException);

            WrapHandlerData wrapHandlerData = new WrapHandlerData();
            wrapHandlerData.Name = name;
            wrapHandlerData.ExceptionMessage = message;
            wrapHandlerData.WrapExceptionType = wrapExceptionType;

            WrapHandlerNode wrapHandlerNode = new WrapHandlerNode(wrapHandlerData);

            WrapHandlerData nodeData = (WrapHandlerData)wrapHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(message, nodeData.ExceptionMessage);
            Assert.AreEqual(wrapExceptionType, nodeData.WrapExceptionType);
        }

    }
}
