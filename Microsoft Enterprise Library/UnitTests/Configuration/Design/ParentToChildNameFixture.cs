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
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
	[TestClass]
	public class ParentToChildNameFixture : ConfigurationDesignHost
	{
		[TestMethod]
		public void StrangeBehaviour()
		{
			ConfigurationNode parentNode = new NodeImpl("parent");
			ConfigurationNode childNode = new NodeImpl("child");
			parentNode.AddNode(childNode);
			
			ApplicationNode.AddNode(parentNode);
			
			Assert.AreEqual("child", childNode.Name);
			Assert.AreEqual("parent", parentNode.Name);
		}

		[TestMethod]
		public void RenameNodeChangesIndex()
		{
			ConfigurationNode childNode = new NodeImpl("Test");
			ApplicationNode.AddNode(childNode);
			childNode.Name = "test3";

			ConfigurationNode childNode2 = new NodeImpl("Test2");
			ApplicationNode.AddNode(childNode2);

			childNode2.Name = ServiceHelper.GetNameCreationService(ServiceProvider).GetUniqueName("Test", childNode, childNode.Parent);
		}

		private class NodeImpl : ConfigurationNode
		{
			public NodeImpl(string name) : base(name) { }
		}
	}
}
