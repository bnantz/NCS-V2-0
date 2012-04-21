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

using Microsoft.Practices.EnterpriseLibrary.Caching;
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
	public class CacheFactoryFixture
	{
		[TestMethod]
		public void GetDefaultCacheManagerTest()
		{
			CacheManager cacheManager = CacheFactory.GetCacheManager();
			Assert.IsNotNull(cacheManager);
		}
		
		[TestMethod]
		public void GetCacheManagerTest()
		{
			CacheManager cacheManager = CacheFactory.GetCacheManager("InIsoStorePersistenceWithNullEncryption");
			Assert.IsNotNull(cacheManager);
		}

	}


}
