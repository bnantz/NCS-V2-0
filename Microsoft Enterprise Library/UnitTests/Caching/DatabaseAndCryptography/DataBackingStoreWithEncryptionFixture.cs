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
using System.Runtime.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System.IO;
using System.Security.Cryptography;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif


namespace Microsoft.Practices.EnterpriseLibrary.Caching.DatabaseAndCryptography.Tests
{
	[TestClass]	
    public class DataBackingStoreWithEncryptionFixture 
    {
		private static DataBackingStore unencryptedBackingStore;
		private static Data.Database db;
        private const string CacheManagerName = "EncryptedCacheInDatabase";

		[TestInitialize]
		public void SetUp()
        {
            DatabaseProviderFactory dbFactory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
            db = dbFactory.CreateDefault();
            unencryptedBackingStore = new DataBackingStore(db, "encryptionTests", null);
            unencryptedBackingStore.Flush();

			ProtectedKey key = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged), DataProtectionScope.CurrentUser);
			using (FileStream stream = new FileStream("ProtectedKey.file", FileMode.Create))
			{
				KeyManager.Write(stream, key);
			}
		}

		[TestCleanup]
		public void TestCleanup()
		{
			unencryptedBackingStore.Flush();
			File.Delete("ProtectedKey.file");
		}

		[TestMethod]
        public void DataCanBeRetrievedWithNoEncryptionProvider()
        {
            unencryptedBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));

            Hashtable dataInCache = unencryptedBackingStore.Load();

            CacheItem retrievedItem = (CacheItem)dataInCache["key"];
            Assert.AreEqual("key", retrievedItem.Key);
            Assert.AreEqual("value", retrievedItem.Value);
            Assert.AreEqual(CacheItemPriority.Normal, retrievedItem.ScavengingPriority);
            Assert.AreEqual(typeof(MockRefreshAction), retrievedItem.RefreshAction.GetType());
			Assert.AreEqual(typeof(AlwaysExpired), retrievedItem.GetExpirations()[0].GetType());
        }

		[TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void AttemptingToReadEncryptedDataWithoutDecryptingThrowsException()
        {
            IStorageEncryptionProvider encryptionProvider = null;
			encryptionProvider = EnterpriseLibraryFactory.BuildUp<IStorageEncryptionProvider>("Fred");

            DataBackingStore encryptingBackingStore = new DataBackingStore(db, "encryptionTests", encryptionProvider);

            encryptingBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));
            Hashtable dataInCache = unencryptedBackingStore.Load();
        }

		[TestMethod]
        public void DecryptedDataCanBeReadBackFromDatabase()
        {
            IStorageEncryptionProvider encryptionProvider = null;
			encryptionProvider = EnterpriseLibraryFactory.BuildUp<IStorageEncryptionProvider>("Fred");

            DataBackingStore encryptingBackingStore = new DataBackingStore(db, "encryptionTests", encryptionProvider);

            encryptingBackingStore.Add(new CacheItem("key", "value", CacheItemPriority.Normal, new MockRefreshAction(), new AlwaysExpired()));
            Hashtable dataInCache = encryptingBackingStore.Load();

            CacheItem retrievedItem = (CacheItem)dataInCache["key"];
            Assert.AreEqual("key", retrievedItem.Key);
            Assert.AreEqual("value", retrievedItem.Value);
            Assert.AreEqual(CacheItemPriority.Normal, retrievedItem.ScavengingPriority);
            Assert.AreEqual(typeof(MockRefreshAction), retrievedItem.RefreshAction.GetType());
			Assert.AreEqual(typeof(AlwaysExpired), retrievedItem.GetExpirations()[0].GetType());
        }

        [Serializable]
        private class MockRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
            {
            }            
        }

    }
}