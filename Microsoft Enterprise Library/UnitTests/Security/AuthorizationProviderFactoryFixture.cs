//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
	[TestClass]
    public class AuthorizationProviderFactoryFixture
    {
		private DictionaryConfigurationSource GetConfigurationSource()
		{
			DictionaryConfigurationSource sections = new DictionaryConfigurationSource();

			SecuritySettings securityConfig = new SecuritySettings();
			securityConfig.DefaultAuthorizationProviderName = "provider1";
			securityConfig.AuthorizationProviders.Add(new MockAuthorizationProviderData("provider1"));
			securityConfig.AuthorizationProviders.Add(new MockAuthorizationProvider2Data("provider2"));
			sections.Add(SecuritySettings.SectionName, securityConfig);

			return sections;
		}

		[TestMethod]
		public void GetDefaultAuthorizationProviderTest()
        {
			AuthorizationProviderFactory factory = new AuthorizationProviderFactory(GetConfigurationSource());
            IAuthorizationProvider provider = factory.CreateDefault();
            Assert.IsNotNull(provider);
            MockAuthorizationProvider mockProvider = provider as MockAuthorizationProvider;
            Assert.IsNotNull(mockProvider);
            Assert.IsTrue(mockProvider.Initialized);
        }

		[TestMethod]
		public void GetAuthorizationProviderByNameTest()
        {
			AuthorizationProviderFactory factory = new AuthorizationProviderFactory(GetConfigurationSource());
			IAuthorizationProvider provider = factory.Create("provider1");
            Assert.IsNotNull(provider);
			MockAuthorizationProvider mockProvider = provider as MockAuthorizationProvider;
			Assert.IsNotNull(mockProvider);
        }

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void AuthorizationProviderNotFoundTest()
        {
			AuthorizationProviderFactory factory = new AuthorizationProviderFactory(GetConfigurationSource());
			factory.Create("provider3");
        }
    }
}

