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
    public class TypeValidationAttributeFixture : ConfigurationDesignHost
    {
        private MyTypeTestNode typeNode;
        private PropertyInfo valueInfo1;
		private IConfigurationUIHierarchy hierarchy;
		private IServiceProvider serviceProvider;

		protected override void InitializeCore()
        {
			ConfigurationApplicationNode appNode = new ConfigurationApplicationNode();
            typeNode = new MyTypeTestNode();
            valueInfo1 = typeNode.GetType().GetProperty("TypeName");
			serviceProvider = ServiceBuilder.Build();
			appNode.AddNode(typeNode);
			hierarchy = new ConfigurationUIHierarchy(appNode, serviceProvider);
        }

		protected override void CleanupCore()
		{
			hierarchy.Dispose();
		}

        [TestMethod]
        public void ValidTypeProducesNoErrors()
        {
            TypeValidationAttribute attribute = new TypeValidationAttribute();
            List<ValidationError> errors = new List<ValidationError>();
            typeNode.TypeName = GetType().AssemblyQualifiedName;
			attribute.Validate(typeNode, valueInfo1, errors, ServiceProvider);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        public void InvalidTypeProducesAnError()
        {
            TypeValidationAttribute attribute = new TypeValidationAttribute();
            typeNode.TypeName = "MyTest";
            List<ValidationError> errors = new List<ValidationError>();
			attribute.Validate(typeNode, valueInfo1, errors, ServiceProvider);
            Assert.AreEqual(1, errors.Count);
        }

        private class MyTypeTestNode : ConfigurationNode
        {
            private string value1;

            public MyTypeTestNode() : base()
            {
            }

            [TypeValidation]
            public string TypeName
            {
                get { return value1; }
                set { value1 = value; }
            }
        }
    }
}