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
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

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
    public class CustomLogFilterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCustomLogFilterNodeThrows()
        {
            new CustomLogFilterNode(null);
        }

        [TestMethod]
        public void CustomLogFilterNodeDefaults()
        {
            CustomLogFilterNode customLogFilter = new CustomLogFilterNode();

            Assert.AreEqual("Custom Filter", customLogFilter.Name);
            Assert.IsNull(customLogFilter.Type);
        }

        [TestMethod]
        public void CustomLogFilterDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(LogEnabledFilter);

            CustomLogFilterData data = new CustomLogFilterData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);

            CustomLogFilterNode node = new CustomLogFilterNode(data);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type, node.Type);
            Assert.AreEqual(attributeKey, node.Attributes[0].Key);
            Assert.AreEqual(attributeValue, node.Attributes[0].Value);
        }

        [TestMethod]
        public void CustomLogFilterNodeTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(LogEnabledFilter);

            CustomLogFilterNode node = new CustomLogFilterNode();
            node.Name = name;
            node.Attributes.Add(new EditableKeyValue(attributeKey, attributeValue));
            node.Type = type;

            CustomLogFilterData nodeData = (CustomLogFilterData)node.LogFilterData;

            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(name, nodeData.Name); 
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
    }
}
