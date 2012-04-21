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

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System.Runtime.Remoting;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class AppDomainNameFormatterFixture: MarshalByRefObject
    {
        [TestMethod]
        public void WillFormatNameWithAppDomainNamePrefix()
        {
            AppDomainNameFormatter nameFormatter = new AppDomainNameFormatter();

			string createdName = nameFormatter.CreateName("MyInstance");
			Assert.IsTrue(createdName.EndsWith(" - MyInstance"));
			Assert.IsTrue(createdName.Length <= 128);
        }
    }
}
