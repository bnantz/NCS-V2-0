//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source Quick Start
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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Design.Tests
{
    [TestClass]
    public class SqlConfigurationSourceRegistrarFixture
    {
		private MockUIComandService cmdService;
		private ServiceContainer services;
			

		[TestInitialize]
		public void TestInitialize()
		{
			cmdService = new MockUIComandService();
			services = new ServiceContainer();
			services.AddService(typeof(IUICommandService), cmdService);
		}

		[TestMethod]
		public void VerifyCommandRegistration()
		{			
			SqlConfigurationSourceCommandRegistrar registrar = new SqlConfigurationSourceCommandRegistrar(services);
			registrar.Register();

			Assert.AreEqual(1, cmdService.List[typeof(ConfigurationSourceSectionNode)].Count);
            Assert.AreEqual(2, cmdService.List[typeof(SqlConfigurationSourceElementNode)].Count);
        }		
	}
}
