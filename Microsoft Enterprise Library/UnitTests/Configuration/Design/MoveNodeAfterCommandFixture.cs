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
    public class MoveNodeAfterCommandFixture 
    {
        private MyTestNode nodeA;
        private MyTestNode nodeB;
        private MyTestNode nodeC;
		private ConfigurationApplicationNode node;
		private IConfigurationUIHierarchy hierarchy;
		private IServiceProvider serviceProvider;

        [TestInitialize]
        public void TestInitialize()
        {
			node = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
			serviceProvider = ServiceBuilder.Build();
			hierarchy = new ConfigurationUIHierarchy(node, serviceProvider);
            nodeA = new MyTestNode();
            nodeB = new MyTestNode();
            nodeC = new MyTestNode();
            node.AddNode(nodeA);
			node.AddNode(nodeB);
			node.AddNode(nodeC);
        }

		[TestCleanup]
		public void TestCleanup()
		{
			hierarchy.Dispose();
		}

        [TestMethod]
        public void MoveNodeDownFromTopMovesNodeToNextSibling()
        {
			MoveNodeAfterCommand cmd = new MoveNodeAfterCommand(serviceProvider);
            cmd.Execute(nodeA);

            Assert.AreSame(node.FirstNode, nodeB);
            Assert.AreSame(node.FirstNode.NextSibling, nodeA);
        }

        [TestMethod]
        public void MoveNodeDownFromBottomDoesNotMoveNode()
        {
			MoveNodeAfterCommand cmd = new MoveNodeAfterCommand(serviceProvider);
            cmd.Execute(nodeC);

            Assert.AreSame(node.LastNode, nodeC);
        }

        [TestMethod]
        public void MoveNodeDownFomMiddlePutsNodeAtBottomAndShitsBottomNodeUp()
        {
			MoveNodeAfterCommand cmd = new MoveNodeAfterCommand(serviceProvider);
            cmd.Execute(nodeB);

            Assert.AreSame(node.LastNode, nodeB);
            Assert.AreSame(node.LastNode.PreviousSibling, nodeC);
        }

        private class MyTestNode : ConfigurationNode
        {
        }
    }
}