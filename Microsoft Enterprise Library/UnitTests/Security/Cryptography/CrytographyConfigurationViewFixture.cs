//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
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
	public class CrytographyConfigurationViewFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructingWithANullConfigurationSourceThrows()
		{
			new CryptographyConfigurationView(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void SectionIsNotDefinedThrows()
		{			
			CryptographyConfigurationView view = new CryptographyConfigurationView(new NullConfigurationSource());
			CryptographySettings settings = view.CryptographySettings;
		}
	}
}
