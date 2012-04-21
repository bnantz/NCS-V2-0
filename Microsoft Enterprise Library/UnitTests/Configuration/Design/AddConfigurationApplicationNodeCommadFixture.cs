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
using System.ComponentModel.Design;

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
    public class AddConfigurationApplicationNodeCommadFixture  : ConfigurationDesignHost
    {
		private AddConfigurationApplicationNodeCommand addConfigurationApplicationNodeCommand;
        private bool hierarchyAdded;
        private ConfigurationNode nodeAdded;


		protected override void InitializeCore()
        {			
			addConfigurationApplicationNodeCommand = new AddConfigurationApplicationNodeCommand(ServiceProvider);
			HiearchyService.HierarchyAdded += new EventHandler<HierarchyAddedEventArgs>(OnHierarchyAdded);
        }
		        
        protected override void CleanupCore()
        {
            hierarchyAdded = false;
            addConfigurationApplicationNodeCommand.Dispose();						
        }

		[TestMethod]
		public void CreateAConfigurationApplicationNodeCreatesAHierarchyAndAddsNode()
		{
			addConfigurationApplicationNodeCommand.Execute(null);

			Assert.IsTrue(hierarchyAdded);
			Assert.AreEqual(typeof(ConfigurationApplicationNode), nodeAdded.GetType());
		}

        private void OnHierarchyAdded(object sender, HierarchyAddedEventArgs args)
        {
            hierarchyAdded = true;
            nodeAdded = args.UIHierarchy.RootNode;
        }
    }
}