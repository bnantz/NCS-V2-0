//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Tests
{
	[TestClass]
	public class CacheManagerDataFixture
	{
		private const string name1 = "name1";
		private const string name2 = "name2";
		private const string storageName = "cache storage";

		private const int pollFrequency1 = 11;
		private const int itemsBeforeScavenge1 = 12;
		private const int itemsToScavenge1 = 13;

		private const int pollFrequency2 = 21;
		private const int itemsBeforeScavenge2 = 22;
		private const int itemsToScavenge2 = 23;

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			CacheManagerData data1 = new CacheManagerData(name1, pollFrequency1, itemsBeforeScavenge1, itemsToScavenge1, storageName);
			CacheManagerData data2 = new CacheManagerData(name2, pollFrequency2, itemsBeforeScavenge2, itemsToScavenge2, storageName);

			CacheManagerSettings settings = new CacheManagerSettings();
			settings.DefaultCacheManager = name1;

			settings.CacheManagers.Add(data1);
			settings.CacheManagers.Add(data2);

			// needed to save configuration
			settings.BackingStores.Add(new CustomCacheStorageData("foo", typeof(MockCustomStorageBackingStore)));

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[CacheManagerSettings.SectionName] = settings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			CacheManagerSettings roSettigs = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);

			Assert.IsNotNull(roSettigs);
			Assert.AreEqual(2, roSettigs.CacheManagers.Count);
			Assert.AreEqual(name1, roSettigs.DefaultCacheManager);

			Assert.IsNotNull(roSettigs.CacheManagers.Get(name1));
			Assert.AreEqual(name1, roSettigs.CacheManagers.Get(name1).Name);
			Assert.AreEqual(pollFrequency1, roSettigs.CacheManagers.Get(name1).ExpirationPollFrequencyInSeconds);
			Assert.AreEqual(itemsBeforeScavenge1, roSettigs.CacheManagers.Get(name1).MaximumElementsInCacheBeforeScavenging);
			Assert.AreEqual(itemsToScavenge1, roSettigs.CacheManagers.Get(name1).NumberToRemoveWhenScavenging);
			Assert.AreEqual(storageName, roSettigs.CacheManagers.Get(name1).CacheStorage);

			Assert.IsNotNull(roSettigs.CacheManagers.Get(name2));
			Assert.AreEqual(name2, roSettigs.CacheManagers.Get(name2).Name);
			Assert.AreEqual(pollFrequency2, roSettigs.CacheManagers.Get(name2).ExpirationPollFrequencyInSeconds);
			Assert.AreEqual(itemsBeforeScavenge2, roSettigs.CacheManagers.Get(name2).MaximumElementsInCacheBeforeScavenging);
			Assert.AreEqual(itemsToScavenge2, roSettigs.CacheManagers.Get(name2).NumberToRemoveWhenScavenging);
			Assert.AreEqual(storageName, roSettigs.CacheManagers.Get(name2).CacheStorage);
		}
	}
}
