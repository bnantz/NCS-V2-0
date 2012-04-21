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
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class TextFormatterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInTextFormatterNodeThrows()
        {
            new TextFormatterNode(null);
        }

        [TestMethod]
        public void TextFormatterNodeDefaults()
        {
            TextFormatterNode textFormatter = new TextFormatterNode();

            Assert.AreEqual("Text Formatter", textFormatter.Name);
            Assert.AreEqual(DefaultValues.TextFormatterFormat, textFormatter.Template.Text);
        }

        [TestMethod]
        public void TextFormatterDataTest()
        {
            string name = "some name";
            string template = "template";

            TextFormatterData data = new TextFormatterData();
            data.Name = name;
            data.Template = template;

            TextFormatterNode node = new TextFormatterNode(data);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(template, node.Template.Text);
        }

        [TestMethod]
        public void TextFormatterNodeTest()
        {
            string name = "some name";
            string template = "template";

            TextFormatterNode formatterNode = new TextFormatterNode();
            formatterNode.Name = name;
            formatterNode.Template = new Template(template);

            TextFormatterData nodeData = (TextFormatterData) formatterNode.FormatterData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(template, nodeData.Template);
        }
    }
}
