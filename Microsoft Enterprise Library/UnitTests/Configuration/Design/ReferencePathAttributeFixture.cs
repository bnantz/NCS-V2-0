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

using System;
using System.Reflection;
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
    public class ReferencePathAttributeFixture
    {
        private PathTest test;

        [TestInitialize]
        public void Init()
        {
            test = new PathTest();
        }

        [TestMethod]
        public void ReflectionTest()
        {
            Type t = test.GetType();
            PropertyInfo property = t.GetProperty("Path");
            ReferenceTypeAttribute[] attributes = (ReferenceTypeAttribute[])property.GetCustomAttributes(typeof(ReferenceTypeAttribute), true);

            Assert.AreEqual(1, attributes.Length);
            Assert.AreEqual(typeof(ConfigurationApplicationNode), attributes[0].ReferenceType);
        }

        private class PathTest
        {
            private string path = "My path";

			[ReferenceType(typeof(ConfigurationApplicationNode))]
            public string Path
            {
                get { return path; }
            }
        }
    }
}