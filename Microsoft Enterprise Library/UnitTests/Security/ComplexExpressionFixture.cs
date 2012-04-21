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
    public class ComplexExpressionFixture
    {
      	[TestMethod]
        public void ComplexExpressionTest()
        {
            AndOperator expression = new AndOperator();
            expression.Left = new RoleExpression("Managers");
            expression.Right = new NotOperator(new IdentityExpression("Bob"));

            GenericIdentity identity = new GenericIdentity("Bob");
            string[] roles = new string[] {"Managers"};
            GenericPrincipal principal = new GenericPrincipal(identity, roles);

            bool result = expression.Evaluate(principal);
            Assert.IsFalse(result);
        }
    }
}

