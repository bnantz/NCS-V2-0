//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Tests
{
    [TestClass]
    public class AuthorizationRuleNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInAuthorizationRuleNodeThrows()
        {
            new AuthorizationRuleNode(null);
        }

        [TestMethod]
        public void AuthorizationRuleNodeDefaults()
        {
            AuthorizationRuleNode ruleNode = new AuthorizationRuleNode();
            Assert.AreEqual(string.Empty, ruleNode.Expression);
            Assert.AreEqual("Rule", ruleNode.Name);
        }

        [TestMethod]
        public void AuthorizationRuleDataTest()
        {
            string name = "some name";
            string expression = "some expression";

            AuthorizationRuleData data = new AuthorizationRuleData();
            data.Name = name;
            data.Expression = expression;

            AuthorizationRuleNode node = new AuthorizationRuleNode(data);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(expression, node.Expression);
        }

        [TestMethod]
        public void AuthorizationRuleNodeTest()
        {
            string name = "some name";
            string expression = "some expression";

            AuthorizationRuleNode authorizationNode = new AuthorizationRuleNode();
            authorizationNode.Name = name;
            authorizationNode.Expression = expression;

            AuthorizationRuleData nodeData = (AuthorizationRuleData)authorizationNode.AuthorizationRuleData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(expression, nodeData.Expression);
        }
    }
}
