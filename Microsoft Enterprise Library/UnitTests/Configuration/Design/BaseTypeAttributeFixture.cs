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

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class BaseTypeAttributeFixture
    {
        [TestMethod]
        public void EnsureValuesAreCorrectlySet()
        {
			BaseTypeAttribute baseTypeAttribute = new BaseTypeAttribute(typeof(Array));
            Assert.AreEqual(typeof(Array), baseTypeAttribute.BaseType);
			Assert.AreEqual(TypeSelectorIncludes.None, baseTypeAttribute.TypeSelectorIncludes);			
        }

		[TestMethod]
		public void EnsureValuesAreCorrectlySetWhenUsingConfigurationType()
		{
			BaseTypeAttribute baseTypeAttribute = new BaseTypeAttribute(typeof(Array), null);
			Assert.AreEqual(typeof(Array), baseTypeAttribute.BaseType);
			Assert.AreEqual(TypeSelectorIncludes.None, baseTypeAttribute.TypeSelectorIncludes);
			Assert.IsNull(baseTypeAttribute.ConfigurationType);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructingBaseTypeAttributeWithNullTypeThrows()
		{
			BaseTypeAttribute baseTypeAttribute = new BaseTypeAttribute(null);
		}


    }
}
