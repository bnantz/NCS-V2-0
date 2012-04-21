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
    public class GuidTokenFixture
    {
		[TestMethod]
        public void DefaultTest()
        {
            GuidToken token = new GuidToken();
            Assert.IsTrue(token.Value.Length == 36);
        }

		[TestMethod]
        public void DefinedTest()
        {
            Guid guid = Guid.NewGuid();
            GuidToken token = new GuidToken(guid);
            Assert.AreEqual(guid.ToString(), token.Value);
        }

		[TestMethod]
        public void EmptyTest()
        {
            GuidToken token = new GuidToken(Guid.Empty);
            Assert.IsNotNull(token.Value.Length == 0);
        }
    }
}

