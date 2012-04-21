//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================


using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class CryptographyDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void OpenAndSaveConfiguration()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            CryptographySettingsNode rootNode = (CryptographySettingsNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(CryptographySettingsNode));
            Assert.IsNotNull(rootNode);
            Assert.AreEqual("dpapiSymmetric1", rootNode.DefaultSymmetricCryptoProvider.Name);
            Assert.AreEqual("hashAlgorithm1", rootNode.DefaultHashProvider.Name);

            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CryptographySettingsNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(HashAlgorithmProviderNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(DpapiSymmetricCryptoProviderNode)).Count);


            ApplicationNode.Hierarchy.Save();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CryptographySettingsNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(HashAlgorithmProviderNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(DpapiSymmetricCryptoProviderNode)).Count);
        }

        [TestMethod]
        public void BuildContextTest()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            IConfigurationSource source = ApplicationNode.Hierarchy.BuildConfigurationSource();
            Assert.IsNotNull(source.GetSection(CryptographyConfigurationView.SectionName));
        }
    }
}
