//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class MinimumLengthAttributeFixture : ConfigurationDesignHost
    {
        private MyMinLengthTestNode minLengthNode;
        private PropertyInfo valueInfo1;
        private PropertyInfo valueInfo2;
		private IConfigurationUIHierarchy hierarchy;
		private IServiceProvider serviceProvider;
 
		protected override void InitializeCore()
        {
			ConfigurationApplicationNode appNode = new ConfigurationApplicationNode();
            minLengthNode = new MyMinLengthTestNode();
			appNode.AddNode(minLengthNode);
            valueInfo1 = minLengthNode.GetType().GetProperty("Value1");
            valueInfo2 = minLengthNode.GetType().GetProperty("Value2");
			serviceProvider = ServiceBuilder.Build();
			hierarchy = new ConfigurationUIHierarchy(appNode, serviceProvider);   
        }

		protected override void CleanupCore()
		{
			hierarchy.Dispose();
		}

        [TestMethod]
        public void MinLengthViolationWithNull()
        {
            MinimumLengthAttribute attribute = new MinimumLengthAttribute(8);
			List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(minLengthNode, valueInfo1, errors, ServiceProvider);

            Assert.AreEqual(1, errors.Count);
        }

		[TestMethod]
		public void MinLengthViolationWithEmptyString()
		{
			MinimumLengthAttribute attribute = new MinimumLengthAttribute(8);
			List<ValidationError> errors = new List<ValidationError>();
			minLengthNode.Value1 = string.Empty;
			attribute.Validate(minLengthNode, valueInfo1, errors, ServiceProvider);

			Assert.AreEqual(1, errors.Count);
		}

        [TestMethod]
        public void MinLengthTest()
        {
            MinimumLengthAttribute attribute = new MinimumLengthAttribute(8);
            minLengthNode.Value2 = "MyTestPassword";
			List<ValidationError> errors = new List<ValidationError>();
            attribute.Validate(minLengthNode, valueInfo2, errors, ServiceProvider);

            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void MinLengthViolationTestWithValidateCommand()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);            
            cmd.Execute(minLengthNode);

            Assert.AreEqual(2, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        [TestMethod]
		public void MinLengthTestWithValidateCommand()
        {
			ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            minLengthNode.Value1 = "MyTest";
            minLengthNode.Value2 = "MyTestPassword";
            cmd.Execute(minLengthNode);

			Assert.AreEqual(0, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        private class MyMinLengthTestNode : ConfigurationNode
        {
            private string value1;
            private string value2;

            public MyMinLengthTestNode() : base("Test")
            {				
            }

            [MinimumLength(3)]
            public string Value1
            {
                get { return value1; }
                set { value1 = value; }
            }

            [MinimumLength(8)]
            public string Value2
            {
                get { return value2; }
                set { value2 = value; }
            }
        }
    }
}
