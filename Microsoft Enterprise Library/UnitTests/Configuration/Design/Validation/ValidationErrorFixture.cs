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

using System.Reflection;
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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests
{
    [TestClass]
    public class ValidationErrorFixture
    {
        private ValidationError error;
        private ConfigurationNode node;
        private string message;
		private PropertyInfo propertyInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            node = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
            message = "Test";
			propertyInfo = node.GetType().GetProperty("ConfigurationFile");
            error = new ValidationError(node, propertyInfo.Name, message);
        }

        [TestMethod]
        public void InvalidItemIsSetCorrectly()
        {
            Assert.AreSame(node, error.InvalidItem);
        }

        [TestMethod]
        public void MessageIsSetCorrectly()
        {
            Assert.AreEqual(message, error.Message);
        }

		[TestMethod]
		public void PropertyNameSetCorrectly()
		{
			Assert.AreEqual(propertyInfo.Name, error.PropertyName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructWithNullItemThrows()		
		{
			new ValidationError(null, "Test", "Test");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructWithNullPropertyThrows()
		{
			new ValidationError(node, null, "Test");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructWithEmptyPropertyThrows()
		{
			new ValidationError(node, string.Empty, "Test");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructWithNullMessageThrows()
		{
			new ValidationError(node, "Test", null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructWithEmptyMessageThrows()
		{
			new ValidationError(node, "Test", string.Empty);
		}		
    }
}