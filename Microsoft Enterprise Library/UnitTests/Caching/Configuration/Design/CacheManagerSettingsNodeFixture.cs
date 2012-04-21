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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestClass]
    public class CacheManagerSettingsNodeFixture : ConfigurationDesignHost
    {
        private DictionaryConfigurationSource configurationSource;

        protected override void InitializeCore()
        {
            configurationSource = new DictionaryConfigurationSource();
            SetDictionaryConfigurationSource(configurationSource);
        }

		[TestMethod]
		public void EnsureCacheManagerSettingsNodePropertyCatoriesAndDescriptions()
		{
			Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(CacheManagerSettingsNode), "DefaultCacheManager", Resources.DefaultCacheManagerDescription));
			Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(CacheManagerSettingsNode), "DefaultCacheManager"));
		}		

        [TestMethod]
        public void CacheManagerSettingsNodeTest()
        {
            CacheManagerNode defaultCacheManager = new CacheManagerNode();
            ApplicationNode.AddNode(defaultCacheManager);
            defaultCacheManager.Name = "testName";

            CacheManagerSettingsNode node = new CacheManagerSettingsNode();
            ApplicationNode.AddNode(node);
            Assert.AreEqual("Caching Application Block", node.Name);
            
            node.DefaultCacheManager = defaultCacheManager;
            Assert.AreEqual(defaultCacheManager, node.DefaultCacheManager);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MakeSureThatYouCanOnlyAddOneCacheManagerCollectionNode()
        {
            CacheManagerNode defaultCacheManager = new CacheManagerNode();
            ApplicationNode.AddNode(defaultCacheManager);
            defaultCacheManager.Name = "testName";

            CacheManagerSettingsNode node = new CacheManagerSettingsNode();
            ApplicationNode.AddNode(node);
            CacheManagerCollectionNode collectionNode = new CacheManagerCollectionNode();
            node.AddNode(collectionNode);
			node.AddNode(new CacheManagerCollectionNode());
        }
    }
}
