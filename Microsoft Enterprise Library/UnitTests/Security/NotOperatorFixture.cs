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

using System.Security.Principal;
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
    public class NotOperatorFixture
    {
		private IPrincipal principal;
        
		[TestInitialize]
		public void TestInitialize()
		{
			GenericIdentity identity = new GenericIdentity("foo");
			principal = new GenericPrincipal(identity, null);
		}

		[TestMethod]
        public void FalseTest()
        {
            MockExpression expression = new MockExpression(true);
            NotOperator notExpression = new NotOperator(expression);
            Assert.IsFalse(notExpression.Evaluate(principal));
        }

		[TestMethod]
        public void TrueTest()
        {
            MockExpression expression = new MockExpression(false);
            NotOperator notExpression = new NotOperator(expression);
            Assert.IsTrue(notExpression.Evaluate(principal));
        }
    }
}

