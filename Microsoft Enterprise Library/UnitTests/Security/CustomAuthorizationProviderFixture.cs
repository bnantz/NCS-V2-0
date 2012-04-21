//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
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
	public class CustomAuthorizationProviderFixture
	{
		[TestMethod]
		public void CanBuildCustomAuthorizationProviderFromGivenConfiguration()
		{
			CustomAuthorizationProviderData customData
				= new CustomAuthorizationProviderData("custom", typeof(MockCustomAuthorizationProvider));
			customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
			SecuritySettings settings = new SecuritySettings();
			settings.AuthorizationProviders.Add(customData);
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(SecuritySettings.SectionName, settings);

			IAuthorizationProvider custom
				= EnterpriseLibraryFactory.BuildUp<IAuthorizationProvider>("custom", configurationSource);

			Assert.IsNotNull(custom);
			Assert.AreSame(typeof(MockCustomAuthorizationProvider), custom.GetType());
			Assert.AreEqual("value1", ((MockCustomAuthorizationProvider)custom).customValue);
		}

		[TestMethod]
		public void CanBuildCustomAuthorizationProviderFromSavedConfiguration()
		{
			CustomAuthorizationProviderData customData
				= new CustomAuthorizationProviderData("custom", typeof(MockCustomAuthorizationProvider));
			customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
			SecuritySettings settings = new SecuritySettings();
			settings.AuthorizationProviders.Add(customData);

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>(1);
			sections[SecuritySettings.SectionName] = settings;
			IConfigurationSource configurationSource 
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			IAuthorizationProvider custom
				= EnterpriseLibraryFactory.BuildUp<IAuthorizationProvider>("custom", configurationSource);

			Assert.IsNotNull(custom);
			Assert.AreSame(typeof(MockCustomAuthorizationProvider), custom.GetType());
			Assert.AreEqual("value1", ((MockCustomAuthorizationProvider)custom).customValue);
		}

		[TestMethod]
		public void CanBuildCustomAuthorizationProviderFromSystemConfiguration()
		{
			IConfigurationSource configurationSource
				= new SystemConfigurationSource();

			IAuthorizationProvider custom
				= EnterpriseLibraryFactory.BuildUp<IAuthorizationProvider>("custom", configurationSource);

			Assert.IsNotNull(custom);
			Assert.AreSame(typeof(MockCustomAuthorizationProvider), custom.GetType());
			Assert.AreEqual("value1", ((MockCustomAuthorizationProvider)custom).customValue);
		}
	}
}
