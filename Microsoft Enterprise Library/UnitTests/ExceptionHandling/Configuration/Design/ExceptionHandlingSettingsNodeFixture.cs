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

namespace Microsoft.Practices.EnterpriseLibrary.ExcpetionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionHandlingSettingsNodeFixture : ConfigurationDesignHost
    {       

        [TestMethod]
        public void ExceptionHandlingSettingsNodeDefaults()
        {
            ExceptionHandlingSettingsNode settingsNode = new ExceptionHandlingSettingsNode();
            Assert.AreEqual("Exception Handling Application Block", settingsNode.Name);
        }

        [TestMethod]
        public void ExceptionHandlingSettingsNodeHasReadonlyName()
        {
            Assert.IsTrue(CommonUtil.IsPropertyReadOnly(typeof(ExceptionHandlingSettingsNode), "Name"));
        }
    }
}
