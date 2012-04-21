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
    public class SecuritySettingsNodeFixture : ConfigurationDesignHost
    {       

        [TestMethod]
        public void SecuritySettingsNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(SecuritySettingsNode), "Name"));
        }

        [TestMethod]
        public void SecuritySettingsNodeDefaultDataTest()
        {
            SecuritySettingsNode securitySettings = new SecuritySettingsNode();
            ApplicationNode.AddNode(securitySettings);

            Assert.IsNull(securitySettings.DefaultAuthorizationInstance);
            Assert.IsNull(securitySettings.DefaultSecurityCacheInstance);
            Assert.AreEqual("Security Application Block", securitySettings.Name);
        }
    }
}
