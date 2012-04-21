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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationErrorFixture
    {
        private ConfigurationError error;
        private ConfigurationNode node;
        private string message;

        [TestInitialize]
        public void TestInitialize()
        {
            node = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
            message = "Test";
            error = new ConfigurationError(node, message);
        }

        [TestMethod]
        public void NodeIsSetCorrectly()
        {
            Assert.AreSame(node, error.ConfigurationNode);
        }

        [TestMethod]
        public void MessageIsSetCorrectly()
        {
            Assert.AreEqual(message, error.Message);
        }
		
		[TestMethod]
		public void ToStringContainsMessage()
		{
			Assert.IsTrue(error.ToString().Contains(error.Message));
		}
    }
}