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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class AddCryptographySettingsNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void AddCryptographySettingsCommandAddsHashProviderCollection()
        {
            AddCryptographySettingsNodeCommand cmd = new AddCryptographySettingsNodeCommand(ServiceProvider);
            cmd.Execute(ApplicationNode);

            HashProviderCollectionNode hashProviderCollectionNode = (HashProviderCollectionNode)Hierarchy.FindNodeByType(typeof(HashProviderCollectionNode));

            Assert.IsNotNull(hashProviderCollectionNode);
        }

        [TestMethod]
        public void AddCryptographySettingsCommandAddsSymmetricCryptoProviderCollection()
        {
            AddCryptographySettingsNodeCommand cmd = new AddCryptographySettingsNodeCommand(ServiceProvider);
            cmd.Execute(ApplicationNode);

            SymmetricCryptoProviderCollectionNode symmetricCryptographyCollectionNode = (SymmetricCryptoProviderCollectionNode)Hierarchy.FindNodeByType(typeof(SymmetricCryptoProviderCollectionNode));

            Assert.IsNotNull(symmetricCryptographyCollectionNode);
        }
    }
}
