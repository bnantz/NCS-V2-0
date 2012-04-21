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
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;

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
    public class CacheExceptionHandlingFixture: ICacheScavenger
    {
		[TestMethod]
        public void ExceptionThrownDuringAddResultsInObjectBeingRemovedFromCacheCompletely()
        {
            MockBackingStore backingStore = new MockBackingStore();
            CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(10);
			CachingInstrumentationProvider instrumentationProvider = new CachingInstrumentationProvider();

			Cache cache = new Cache(backingStore, scavengingPolicy, instrumentationProvider);
            cache.Initialize(this);

            try
            {
                cache.Add("foo", "bar");
                Assert.Fail("Should have thrown exception thrown internally to Cache.Add");
            }
            catch (Exception)
            {
                Assert.IsFalse(cache.Contains("foo"));
                Assert.AreEqual(1, backingStore.removalCount);
            }
        }

		[TestMethod]
        public void ExceptionThrownDuringAddIntoIsolatedStorageAllowsItemToBeReaddedLater()
        {            
            using (IsolatedStorageBackingStore backingStore = new IsolatedStorageBackingStore("foo", null))
            {
                CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(10);
				CachingInstrumentationProvider instrumentationProvider = new CachingInstrumentationProvider();

                Cache cache = new Cache(backingStore, scavengingPolicy, instrumentationProvider);
                cache.Initialize(this);

                try
                {
                    cache.Add("my", new NonSerializableClass());
                    Assert.Fail("Should have thrown exception internally to Cache.Add");
                }
                catch (Exception)
                {
                    cache.Add("my", new SerializableClass());
                    Assert.IsTrue(cache.Contains("my"));
                }
            }
        }

		[TestMethod]
        public void ItemAddedPreviousToFailedAddIsRemovedCompletelyIfSecondAddFails()
        {            
            using (IsolatedStorageBackingStore backingStore = new IsolatedStorageBackingStore("foo", null))
            {
                CacheCapacityScavengingPolicy scavengingPolicy = new CacheCapacityScavengingPolicy(10);
				CachingInstrumentationProvider instrumentationProvider = new CachingInstrumentationProvider();

                Cache cache = new Cache(backingStore, scavengingPolicy, instrumentationProvider);
                cache.Initialize(this);

                cache.Add("my", new SerializableClass());

                try
                {
                    cache.Add("my", new NonSerializableClass());
                    Assert.Fail("Should have thrown exception internally to Cache.Add");
                }
                catch (Exception)
                {
                    Assert.IsFalse(cache.Contains("my"));
                    Assert.AreEqual(0, backingStore.Count);

                    Hashtable isolatedStorageContents = backingStore.Load();
                    Assert.AreEqual(0, isolatedStorageContents.Count);
                }
            }
        }

        private class NonSerializableClass
        {
        }

        [Serializable]
        private class SerializableClass
        {
        }

        private class MockBackingStore : IBackingStore
        {
            public int removalCount = 0;

            public string CurrentCacheManager
            {
                get { return string.Empty; }
                set {  }
            }

            public int Count
            {
                get
                {
                    return 0;
                }
            }

            public void Add(CacheItem newCacheItem)
            {
                throw new Exception();
            }

            public void Remove(string key)
            {
                this.removalCount++;
            }

            public void UpdateLastAccessedTime(string key, DateTime timestamp)
            {
            }

            public void Flush()
            {
            }

            public Hashtable Load()
            {
                return new Hashtable();
            }            

            public void Dispose()
            {
            }
        }

        
        void ICacheScavenger.StartScavenging()
        {
        }
    }
}

