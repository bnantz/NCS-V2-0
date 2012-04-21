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

using System;
using System.Configuration;
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
    public class CacheManagerFactoryFixture
    {
		private static CacheManagerFactory factory;

		
		[TestInitialize]
		public void CreateFactory()
        {
			factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
        }

		[TestMethod]
        public void CreateNamedCacheInstance()
        {
            CacheManager cache = factory.Create("InMemoryPersistence");
            Assert.IsNotNull(cache, "Should have created caching instance through factory");
        }

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
        public void WillThrowExceptionIfCannotFindCacheInstance()
        {
            factory.Create("ThisIsABadName");
            
            Assert.Fail("Should have thrown ConfigurationErrorsException");
        }

		[TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WillThrowExceptionIfInstanceNameIsNull()
        {
			factory.Create(null);
        }

		[TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WillThrowExceptionIfInstanceNameIsEmptyString()
        {
			factory.Create("");
        }

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
        public void WillThrowExceptionIfNullCacheStorage()
        {
			factory.Create("CacheManagerWithBadCacheStorageInstance");
        }

		[TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void WillThrowExceptionIfCannotCreateNamedStorageType()
        {
			factory.Create("CacheManagerWithBadStoreType");
        }

		[TestMethod]
        public void CallingSameFactoryTwiceReturnsSameInstance()
        {
			CacheManager cacheOne = factory.Create("InMemoryPersistence");
			CacheManager cacheTwo = factory.Create("InMemoryPersistence");
            Assert.AreSame(cacheOne, cacheTwo, "CacheManagerFactory should always return the same instance when using the same instance name");
        }

		[TestMethod]
        public void CallingDifferentFactoryTwiceReturnsDifferentInstances()
        {
			CacheManager cacheOne = factory.Create("InMemoryPersistence");

            CacheManagerFactory secondFactory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
			CacheManager cacheTwo = secondFactory.Create("InMemoryPersistence");

            Assert.IsFalse(object.ReferenceEquals(cacheOne, cacheTwo), "Different factories should always return different instances for same instance name");
        }

		[TestMethod]
        public void CanCreateDefaultCacheManager()
        {
			CacheManager cacheManager = factory.CreateDefault();
            Assert.IsNotNull(cacheManager);
        }

		[TestMethod]
        public void DefaultCacheManagerAndNamedDefaultInstanceAreSameObject()
        {
			CacheManager defaultInstance = factory.CreateDefault();
			CacheManager namedInstance = factory.Create("ShortInMemoryPersistence");

            Assert.AreSame(defaultInstance, namedInstance);
        }
    }
}