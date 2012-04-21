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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Tests
{
	[TestClass]
	public class SymmetricStorageEncryptionProviderDataFixture
	{
		private const string name1 = "name1";
		private const string symmetric1 = "symmetic";

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			CacheManagerSettings settings = new CacheManagerSettings();

			SymmetricStorageEncryptionProviderData data1 = new SymmetricStorageEncryptionProviderData(name1, symmetric1);
			settings.EncryptionProviders.Add(data1);

			// needed to save configuration
			settings.CacheManagers.Add(new CacheManagerData("foo", 0, 0, 0, "storage"));
			settings.BackingStores.Add(new CustomCacheStorageData("foo", typeof(MockCustomStorageBackingStore)));

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[CacheManagerSettings.SectionName] = settings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			CacheManagerSettings roSettigs = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);

			Assert.IsNotNull(roSettigs);
			Assert.AreEqual(1, roSettigs.EncryptionProviders.Count);

			Assert.IsNotNull(roSettigs.EncryptionProviders.Get(name1));
			Assert.AreSame(typeof(SymmetricStorageEncryptionProviderData), roSettigs.EncryptionProviders.Get(name1).GetType());
			Assert.AreEqual(name1, roSettigs.EncryptionProviders.Get(name1).Name);
			Assert.AreEqual(symmetric1, ((SymmetricStorageEncryptionProviderData)roSettigs.EncryptionProviders.Get(name1)).SymmetricInstance);
		}
	}
}
