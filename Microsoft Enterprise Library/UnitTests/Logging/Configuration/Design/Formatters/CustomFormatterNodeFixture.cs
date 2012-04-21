//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
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
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

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
    public class CustomFormatterNodeFixture : ConfigurationDesignHost
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCustomFormatterNodeThrows()
        {
            new CustomFormatterNode(null);
        }

        [TestMethod]
        public void CustomFormatterNodeDefaults()
        {
            CustomFormatterNode customFormatter = new CustomFormatterNode();

            Assert.AreEqual("Custom Formatter", customFormatter.Name);
            Assert.IsNull(customFormatter.Type);
        }

        [TestMethod]
        public void CustomFormatterDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(BinaryLogFormatter);

            CustomFormatterData data = new CustomFormatterData();
            data.Name = name;
            data.Type = type;

            data.Attributes.Add(attributeKey, attributeValue);

            CustomFormatterNode node = new CustomFormatterNode(data);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type, node.Type);
            Assert.AreEqual(attributeKey, node.Attributes[0].Key);
            Assert.AreEqual(attributeValue, node.Attributes[0].Value);
        }

        [TestMethod]
        public void CustomFormatterNodeTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(BinaryLogFormatter);

            CustomFormatterNode formatterNode = new CustomFormatterNode();
            formatterNode.Attributes.Add(new EditableKeyValue(attributeKey, attributeValue));
            formatterNode.Name = name;
            formatterNode.Type = type;

            CustomFormatterData nodeData = (CustomFormatterData) formatterNode.FormatterData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
    }
}
