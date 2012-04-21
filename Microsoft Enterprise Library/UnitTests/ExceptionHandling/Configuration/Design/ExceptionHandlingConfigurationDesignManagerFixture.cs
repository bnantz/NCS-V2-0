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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
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
    public class ExceptionHandlingConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void OpenAndSaveTest()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlingSettingsNode)).Count);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionPolicyNode)).Count);
            Assert.AreEqual(2, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionTypeNode)).Count);
            Assert.AreEqual(3, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlerNode)).Count);

            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlingSettingsNode)).Count);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionPolicyNode)).Count);
            Assert.AreEqual(2, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionTypeNode)).Count);
            Assert.AreEqual(3, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlerNode)).Count);


        }

        [TestMethod]
        public void BuildContextTest()
        {
            ExceptionHandlingConfigurationDesignManager designManager = new ExceptionHandlingConfigurationDesignManager();
            designManager.Register(ServiceProvider);
            designManager.Open(ServiceProvider);

            DictionaryConfigurationSource dictionarySource = new DictionaryConfigurationSource();
            designManager.BuildConfigurationSource(ServiceProvider, dictionarySource);
            Assert.IsTrue(dictionarySource.Contains("exceptionHandling"));
        }
    }
}
