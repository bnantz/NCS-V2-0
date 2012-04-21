//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.Tests
{
    [TestClass]
    public class SymmetricStorageEncryptionProviderNodeFixture : ConfigurationDesignHost
    {

        [TestMethod]
        public void CanCreateSymmetricStorageEncryptionProviderNodeByData()
        {
            INodeCreationService nodeCreationService = ServiceHelper.GetNodeCreationService(ServiceProvider);
            SymmetricStorageEncryptionProviderNode node = nodeCreationService.CreateNodeByDataType(typeof(SymmetricStorageEncryptionProviderData)) as SymmetricStorageEncryptionProviderNode;

            Assert.IsNotNull(node);
        }

        [TestMethod]
        public void SymmetricStorageEncryptionProviderNodeName()
        {
            SymmetricStorageEncryptionProviderNode node = new SymmetricStorageEncryptionProviderNode();

            Assert.AreEqual("Symmetric Storage Encryption", node.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullInSymmetricStorageEncryptionProviderNodeThrows()
        {
            new SymmetricStorageEncryptionProviderNode(null);
        }
	}
}