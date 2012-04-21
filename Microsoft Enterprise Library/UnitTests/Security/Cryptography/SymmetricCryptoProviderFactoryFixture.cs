//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
	[TestClass]
    public class SymmetricCryptoProviderFactoryFixture
    {
		private const string providerName = "dpapiSymmetric1";
		private const string symmetricAlgorithm = "symmetricAlgorithm1";

		[TestMethod]
		public void CreateByNameTest()
		{
			SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(CreateSource(providerName));
			ISymmetricCryptoProvider provider = factory.Create(providerName);

			Assert.AreEqual(typeof(DpapiSymmetricCryptoProvider), provider.GetType());
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void LookupInvalidProviderThrows()
		{
			SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(CreateSource(providerName));
			factory.Create("provider3");
		}

		[TestMethod]
		public void CreateDefaultProvider()
		{
			SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(CreateSource(providerName));
			ISymmetricCryptoProvider provider = factory.CreateDefault();
			
			Assert.AreEqual(typeof(DpapiSymmetricCryptoProvider), provider.GetType());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CreateDefaultProviderWithNoneDefinedThrows()
		{
			SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(CreateSource(string.Empty));
			ISymmetricCryptoProvider provider = factory.CreateDefault();
			Assert.IsNotNull(provider);			
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CreateProviderWithNullNameThrows()
		{
			SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(CreateSource(providerName));
			factory.Create(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CreateProviderWithEmptyNameThrows()
		{
			SymmetricCryptoProviderFactory factory = new SymmetricCryptoProviderFactory(CreateSource(providerName));
			factory.Create(string.Empty);
		}

		private IConfigurationSource CreateSource(string defaultName)
		{
			DictionaryConfigurationSource sections = new DictionaryConfigurationSource();
			CryptographySettings settings = new CryptographySettings();
			settings.DefaultSymmetricCryptoProviderName = defaultName;
			settings.SymmetricCryptoProviders.Add(new DpapiSymmetricCryptoProviderData(providerName, DataProtectionScope.CurrentUser));
			settings.SymmetricCryptoProviders.Add(new SymmetricAlgorithmProviderData(symmetricAlgorithm, typeof(RijndaelManaged), "ProtectedKey.file", DataProtectionScope.CurrentUser));
			sections.Add(CryptographyConfigurationView.SectionName, settings);			
			return sections;
		}

	}
}

