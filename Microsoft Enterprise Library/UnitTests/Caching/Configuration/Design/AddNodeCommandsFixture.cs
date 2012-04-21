//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
	[TestClass]
	public class AddNodeCommandsFixture : ConfigurationDesignHost
	{
		[TestMethod]
		public void EnsureExcutingAddCacheManagerSettingsSetsDefaults()
		{
			AddCacheManagerSettingsNodeCommand cmd = new AddCacheManagerSettingsNodeCommand(ServiceProvider);
			cmd.Execute(ApplicationNode);

			CacheManagerSettingsNode node = (CacheManagerSettingsNode)Hierarchy.FindNodeByType(typeof(CacheManagerSettingsNode));

			Assert.AreEqual(1, node.Nodes.Count);

		}
	}
}
