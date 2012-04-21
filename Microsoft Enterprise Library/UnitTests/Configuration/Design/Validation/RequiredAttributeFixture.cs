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

using System.Collections.Generic;
using System;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class RequiredAttributeFixture : ConfigurationDesignHost
    {
        private MyRequiredTestNode requiredNode;
        private PropertyInfo valueInfo1;
        private PropertyInfo valueInfo2;
		private IConfigurationUIHierarchy hierarchy;
		private IServiceProvider serviceProvider;
		
		protected override void InitializeCore()
        {
			requiredNode = new MyRequiredTestNode();
			ApplicationNode.AddNode(requiredNode);
            valueInfo1 = requiredNode.GetType().GetProperty("Value1");
            valueInfo2 = requiredNode.GetType().GetProperty("Value2");
			serviceProvider = ServiceBuilder.Build();
			hierarchy = new ConfigurationUIHierarchy(ApplicationNode, serviceProvider);
        }

		protected override void CleanupCore()
		{
			hierarchy.Dispose();
		}

        [TestMethod]
        public void RequiredNotThereTest()
        {
            RequiredAttribute attribute = new RequiredAttribute();
			List<ValidationError> errors = new List<ValidationError>();
			attribute.Validate(requiredNode, valueInfo1, errors, ServiceProvider);
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void RequiredValueTest()
        {
            RequiredAttribute attribute = new RequiredAttribute();
            requiredNode.Value2 = "MyTest";
			List<ValidationError> errors = new List<ValidationError>();
			attribute.Validate(requiredNode, valueInfo2, errors, ServiceProvider);
            Assert.AreEqual(0, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        [TestMethod]
        public void RequiredNotThereTestWithVistor()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            Assert.IsNotNull(requiredNode.Site);
            cmd.Execute(requiredNode);
			Assert.AreEqual(2, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        [TestMethod]
        public void RequiredValueTestWithVistor()
        {
            ValidateNodeCommand cmd = new ValidateNodeCommand(serviceProvider);
            Assert.IsNotNull(requiredNode.Site);
            requiredNode.Value1 = "aaa";
            requiredNode.Value2 = "aaaaaa";
            cmd.Execute(requiredNode);
			Assert.AreEqual(0, ValidationAttributeHelper.GetValidationErrorsCount(serviceProvider));
        }

        private class MyRequiredTestNode : ConfigurationNode
        {
            private string value1;
            private string value2;

            public MyRequiredTestNode() : base("Test")
            {				
            }

            [Required]
            public string Value1
            {
                get { return value1; }
                set { value1 = value; }
            }

            [Required]
            public string Value2
            {
                get { return value2; }
                set { value2 = value; }
            }
        }
    }
}