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
using System.Collections.Generic;
using System.Text;
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	[TestClass]
	public class ConfigurationNameProviderFixture
	{
		[TestMethod]
		public void NormalNamesAreNotConsideredMadeUp()
		{
			Assert.IsFalse(ConfigurationNameProvider.IsMadeUpName("foo"));
		}
		
		[TestMethod]
		public void MadeUpNamesAreConsideredMadeUp()
		{
			Assert.IsTrue(ConfigurationNameProvider.IsMadeUpName(ConfigurationNameProvider.MakeUpName()));
		}
		
		[TestMethod]
		public void EmptyStringIsNotConsideredMadeUp()
		{
			Assert.IsFalse(ConfigurationNameProvider.IsMadeUpName(String.Empty));
		}
		
		[TestMethod]
		public void NullNamesAreNotConsideredMadeUp()
		{
			Assert.IsFalse(ConfigurationNameProvider.IsMadeUpName(null));
		}
	}
}
