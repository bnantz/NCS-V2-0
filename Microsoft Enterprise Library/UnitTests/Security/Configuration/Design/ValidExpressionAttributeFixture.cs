//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Tests
{
    [TestClass]
    public class ValidExpressionAttributeFixture : ConfigurationDesignHost
    {
        private ExpressionTestNode expressionTestNode;

        protected override void InitializeCore()
        {
            expressionTestNode = new ExpressionTestNode(string.Empty);
            ApplicationNode.AddNode(expressionTestNode);
        }

        [TestMethod]
        public void NonExpressionFailsValidation()
        {
            string nonExpression = "hello, this doesnt make sense";
            expressionTestNode.Expression = nonExpression;

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider);
            cmd.Execute(expressionTestNode);

            Assert.AreEqual(1, ValidationAttributeHelper.GetValidationErrorsCount(ServiceProvider));
        }

        [TestMethod]
        public void NullExpressionPassesValidation()
        {
            expressionTestNode.Expression = null;

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider);
            cmd.Execute(expressionTestNode);

            Assert.AreEqual(0, ValidationAttributeHelper.GetValidationErrorsCount(ServiceProvider));
        }

        [TestMethod]
        public void ExpressionPassesValidation()
        {
            expressionTestNode.Expression = "NOT I:?";

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider);
            cmd.Execute(expressionTestNode);

            Assert.AreEqual(0, ValidationAttributeHelper.GetValidationErrorsCount(ServiceProvider));
        }


        private class ExpressionTestNode : ConfigurationNode
        {
            private string expression;

            public ExpressionTestNode(string expression)
            {
                this.expression = expression;
            }

            [ValidExpression]
            public string Expression
            {
                get { return expression; }
                set { expression = value; }
            }
        }
    }
}
