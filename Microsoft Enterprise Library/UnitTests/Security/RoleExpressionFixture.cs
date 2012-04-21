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
    public class RoleExpressionFixture
    {
		private IPrincipal principal;

        [TestInitialize]
		public void TestInitialize()
		{
			GenericIdentity identity = new GenericIdentity("foo");
			string[] roles = new string[] { "Manager" };
			principal = new GenericPrincipal(identity, roles);
		}


		[TestMethod]
        public void TrueTest()
        {
            RoleExpression expression = new RoleExpression("Manager");
            Assert.IsTrue(expression.Evaluate(principal));
        }

		[TestMethod]
        public void FalseTest()
        {
            RoleExpression expression = new RoleExpression("Admin");
            Assert.IsFalse(expression.Evaluate(principal));
        }

		[TestMethod]
        public void AnyTest()
        {
            RoleExpression expression = new RoleExpression("*");
            Assert.AreEqual(typeof(AnyExpression), expression.Word.GetType());
        }

		[TestMethod]
        public void WordTest()
        {
            RoleExpression expression = new RoleExpression("Role1");
            Assert.AreEqual(typeof(WordExpression), expression.Word.GetType());
        }
    }
}

