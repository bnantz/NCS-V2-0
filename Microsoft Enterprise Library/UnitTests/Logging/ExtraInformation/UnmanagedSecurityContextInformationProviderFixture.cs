//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Properties;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Tests
{
	[TestClass]
    public class UnmanagedSecurityContextInformationProviderFixture
    {
		private Dictionary<string, object> dictionary;
		private UnmanagedSecurityContextInformationProvider provider;

		[TestInitialize]
		public void SetUp()
        {
            dictionary = new Dictionary<string, object>();
        }

		[TestMethod]
        public void PopulateDictionaryFilledCorrectly()
        {
            provider = new UnmanagedSecurityContextInformationProvider();
            provider.PopulateDictionary(dictionary);

            Assert.AreEqual(2, dictionary.Count);
			AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.UnmanagedSecurity_CurrentUser] as string, string.Format(Resources.ExtendedPropertyError, ""), "CurrentUser");
			AssertUtilities.AssertStringDoesNotContain(dictionary[Resources.UnmanagedSecurity_ProcessAccountName] as string, string.Format(Resources.ExtendedPropertyError, ""), "ProcessAccountName");
        }
    }
}

