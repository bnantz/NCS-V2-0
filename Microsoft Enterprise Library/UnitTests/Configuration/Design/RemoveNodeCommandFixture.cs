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
    public class RemoveNodeCommandFixture 
    {
		private ConfigurationApplicationNode appNode;
		private IConfigurationUIHierarchy hierarchy;
		private IServiceProvider serviceProvider;
		private int removeEventCount;

        [TestInitialize]
        public void TestInitialize()
        {
			appNode = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
			serviceProvider = ServiceBuilder.Build();
			hierarchy = new ConfigurationUIHierarchy(appNode, serviceProvider);
            appNode.AddNode(new MyTestNode());
			removeEventCount = 0;
        }

		[TestCleanup]
		public void TestCleanup()
		{
			hierarchy.Dispose();
		}

        [TestMethod]
        public void RemoveNodeTest()
        {
            MyTestNode node = appNode.Nodes[0] as MyTestNode;
            Assert.IsNotNull(node);
            node.Remove();

            Assert.AreEqual(0, appNode.Nodes.Count);
        }

		[TestMethod]
		public void RemoveNodeFiresRemoveEvent()
		{
			MyTestNode node = appNode.Nodes[0] as MyTestNode;
			Assert.IsNotNull(node);
			node.Removed += new EventHandler<ConfigurationNodeChangedEventArgs>(OnNodeRemoved);
			node.Remove();

			Assert.AreEqual(1, removeEventCount);
		}

		void OnNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
		{
			removeEventCount++;
		}

        private class MyTestNode : ConfigurationNode
        {
        }
    }
}