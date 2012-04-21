//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using System.Configuration;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
	[TestClass]
	public class ConfigurationSourceFactoryFixture
	{
		[TestMethod]
		public void CanCreateAConfigurationSourceThatExistsInConfig()
		{
			IConfigurationSource source = ConfigurationSourceFactory.Create("fileSource");

			Assert.AreEqual(typeof(FileConfigurationSource), source.GetType());
		}

		[TestMethod]
		public void DefaultConfigurationSourceIsSystemSource()
		{
			IConfigurationSource defaultSource = ConfigurationSourceFactory.Create();

			Assert.AreEqual(typeof(SystemConfigurationSource), defaultSource.GetType());

		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RequestForNullNameThrows()
		{
			ConfigurationSourceFactory.Create(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void RequestForNonExistentNameThrows()
		{
			ConfigurationSourceFactory.Create("invalid");
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void RequestForNameWithoutSectionThrows()
		{
			System.Configuration.Configuration configuration = ConfigurationTestHelper.GetConfigurationForCustomFile("test.exe.config");
			configuration.Sections.Remove(ConfigurationSourceSection.SectionName);
			configuration.Save();

			AppDomainSetup setupInfo = new AppDomainSetup();
			setupInfo.ConfigurationFile = configuration.FilePath;
			setupInfo.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			AppDomain newDomain = AppDomain.CreateDomain("test", null, setupInfo);

			try
			{
				ConfigurationSourceFactoryFixtureHelper helper =
					(ConfigurationSourceFactoryFixtureHelper)newDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ConfigurationSourceFactoryFixtureHelper).FullName);

				helper.RequestForNameWithoutSectionThrows();
			}
			finally
			{
				AppDomain.Unload(newDomain);
			}
		}

	}

	public class ConfigurationSourceFactoryFixtureHelper : MarshalByRefObject
	{
		public void RequestForNameWithoutSectionThrows()
		{
			ConfigurationSourceFactory.Create("name");
		}
	}
}
