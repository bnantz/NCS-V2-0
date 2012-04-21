//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
    public class AuthorizationRuleProviderNodeFixture: ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInAuthorizationRuleProviderNodeThrows()
        {
            new AuthorizationRuleProviderNode(null);
        }

        [TestMethod]
        public void CanCreateAuthorizationRuleProviderNodeWithData()
        {
            Assert.IsNotNull(ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(AuthorizationRuleProviderData), new object[] { new AuthorizationRuleProviderData() }));
        }

        [TestMethod]
        public void AuthorizationRuleProviderDefaults()
        {
            AuthorizationRuleProviderNode authRuleProviderNode = new AuthorizationRuleProviderNode();
            Assert.AreEqual(0, authRuleProviderNode.Nodes.Count);
            Assert.AreEqual("RuleProvider", authRuleProviderNode.Name);
        }
    }
}
