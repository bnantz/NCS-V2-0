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
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests
{
	[TestClass]
    public class DatabaseCacheExceptionBehavior 
    {
		private CacheManager cache;

		[TestInitialize]
		public void TestInitialize()
		{			
			CacheManagerFactory factory = new CacheManagerFactory(TestConfigurationSource.GenerateConfiguration());
			cache = factory.Create("InDatabasePersistence");			
		}

		[TestCleanup]
		public void TestCleanup()
		{
			cache.Flush();
			cache.Dispose();			
		}

		[TestMethod]
        public void ItemRemovedFromCacheCompletelyIfAddFails()
        {
            cache.Add("foo", new SerializableClass());

            try
            {
                cache.Add("foo", new NonSerializableClass());
                Assert.Fail("should have thrown exception in Cache.Add");
            }
            catch (Exception)
            {
                Assert.IsFalse(cache.Contains("foo"));

                string isItInDatabaseQuery = "select count(*) from CacheData";
				Data.Database db = new SqlDatabase(@"server=(local)\SQLEXPRESS;database=Caching;Integrated Security=true");
                DbCommand wrapper = db.GetSqlStringCommand(isItInDatabaseQuery);
                int count = (int)db.ExecuteScalar(wrapper);

                Assert.AreEqual(0, count);
            }
        }

        private class NonSerializableClass
        {
        }

        [Serializable]
        private class SerializableClass
        {
        }
    }
}

