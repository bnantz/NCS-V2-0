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


namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
	[TestClass]
    public class IsolatedStoragePersistentCacheFactoryFixture 
    {
		private CacheManagerFactory factory;
		private CacheManager cacheManager;	


		[TestInitialize]
		public void CreateCacheManager()
        {
			factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
            cacheManager = factory.Create("InIsoStorePersistence");
            cacheManager.Flush();
        }

		[TestCleanup]
		public void ReleaseCacheManager()
        {
            cacheManager.Flush();
            cacheManager.Dispose();
        }

		[TestMethod]
        public void CanCreateIsolatedStorageCacheManager()
        {
            cacheManager.Add("bab", "foo");
            Assert.AreEqual(1, cacheManager.Count);

			CacheManagerFactory differentFactory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
            CacheManager differentCacheManager = differentFactory.Create("InIsoStorePersistence");
            
            int count = differentCacheManager.Count;
            differentCacheManager.Dispose();

            Assert.AreEqual(1, count, "If we actually persisted added item, different cache manager should see item, too.");
        }
    }
}

