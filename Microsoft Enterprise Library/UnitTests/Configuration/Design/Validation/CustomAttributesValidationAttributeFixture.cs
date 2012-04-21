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
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests 
{
    [TestClass]
    public class CustomAttributesValidationAttributeFixture: ConfigurationDesignHost
    {
        private readonly PropertyInfo attributeProperty = typeof(NodeWithAttributes).GetProperty("Attributes");

        [TestMethod]
        public void AttributeWithNullKeyCausesError()
        {
            NodeWithAttributes node = new NodeWithAttributes();
            List<ValidationError> errors = new List<ValidationError>();

            node.Attributes.Add(new EditableKeyValue(null, "value"));

            CustomAttributesValidationAttribute validationAttribute = new CustomAttributesValidationAttribute();
            validationAttribute.Validate(node, attributeProperty, errors, ServiceProvider);

            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void DuplicateAttributeKeysCauseError()
        {
            NodeWithAttributes node = new NodeWithAttributes();
            List<ValidationError> errors = new List<ValidationError>();

            node.Attributes.Add(new EditableKeyValue("key", "value"));
            node.Attributes.Add(new EditableKeyValue("key", "value"));

            CustomAttributesValidationAttribute validationAttribute = new CustomAttributesValidationAttribute();
            validationAttribute.Validate(node, attributeProperty, errors, ServiceProvider);

            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public void EmptyCollectionPassesValidation()
        {
            NodeWithAttributes node = new NodeWithAttributes();
            List<ValidationError> errors = new List<ValidationError>();

            CustomAttributesValidationAttribute validationAttribute = new CustomAttributesValidationAttribute();
            validationAttribute.Validate(node, attributeProperty, errors, ServiceProvider);

            Assert.AreEqual(0, errors.Count);
        }

        private class NodeWithAttributes
        {
            private List<EditableKeyValue> _attributes = new List<EditableKeyValue>();

            [CustomAttributesValidation]
            public List<EditableKeyValue> Attributes
            {
                get { return _attributes; }
            }
        }
    }
}
