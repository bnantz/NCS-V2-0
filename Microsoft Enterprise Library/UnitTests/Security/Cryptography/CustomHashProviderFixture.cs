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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
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
	public class CustomHashProviderFixture
	{
		[TestMethod]
		public void CanBuildCustomHashProviderFromGivenConfiguration()
		{
			CustomHashProviderData customData
				= new CustomHashProviderData("custom", typeof(MockCustomHashProvider));
			customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
			CryptographySettings settings = new CryptographySettings();
			settings.HashProviders.Add(customData);
			DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(CryptographyConfigurationView.SectionName, settings);

			IHashProvider custom
				= EnterpriseLibraryFactory.BuildUp<IHashProvider>("custom", configurationSource);

			Assert.IsNotNull(custom);
			Assert.AreSame(typeof(MockCustomHashProvider), custom.GetType());
			Assert.AreEqual("value1", ((MockCustomHashProvider)custom).customValue);
		}

		[TestMethod]
		public void CanBuildCustomHashProviderFromSavedConfiguration()
		{
			CustomHashProviderData customData
				= new CustomHashProviderData("custom", typeof(MockCustomHashProvider));
			customData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
			CryptographySettings settings = new CryptographySettings();
			settings.HashProviders.Add(customData);

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>(1);
			sections[CryptographyConfigurationView.SectionName] = settings;
			IConfigurationSource configurationSource 
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			IHashProvider custom
				= EnterpriseLibraryFactory.BuildUp<IHashProvider>("custom", configurationSource);

			Assert.IsNotNull(custom);
			Assert.AreSame(typeof(MockCustomHashProvider), custom.GetType());
			Assert.AreEqual("value1", ((MockCustomHashProvider)custom).customValue);
		}
	}
}
