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

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
	[TestClass]
	public class ConfigurationDesignManagerProxyFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void EnsureConstructingWithBadTypeThrows()
		{
			new ConfigurationDesignManagerProxy(typeof(Int32));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EnsureConstructinWithNullTypeThrow()
		{
			new ConfigurationDesignManagerProxy(null);
		}
	}
}
