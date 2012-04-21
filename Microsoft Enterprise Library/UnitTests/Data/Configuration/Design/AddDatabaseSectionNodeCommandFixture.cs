//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Collections.Generic;


namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
	[TestClass]
	public class AddDatabaseSectionNodeCommandFixture : ConfigurationDesignHost
	{
		[TestMethod]
		public void ExectueAddsDefaultNodes()
		{
			AddDatabaseSectionNodeCommand cmd = new AddDatabaseSectionNodeCommand(ServiceProvider);
			cmd.Execute(ApplicationNode);

			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

			ConfigurationNode node = Hierarchy.FindNodeByType(typeof(DatabaseSectionNode));
			Assert.IsNotNull(node);

			node = Hierarchy.FindNodeByType(typeof(ConnectionStringSettingsNode));
			Assert.IsNotNull(node);

			IList<ConfigurationNode> nodes = Hierarchy.FindNodesByType(typeof(ParameterNode));
			Assert.AreEqual(3, nodes.Count);

			node = Hierarchy.FindNodeByType(typeof(ProviderMappingsNode));
			Assert.IsNotNull(node);
		}
	}
}
