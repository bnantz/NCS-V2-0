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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests
{
	[TestClass]
	public class ConfigurationDesignManagerFixture : ConfigurationDesignHost
	{
		[TestMethod]
		public void OpenAndSaveConfiguration()
		{
			ApplicationNode.Hierarchy.Load();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
			ApplicationNode.Hierarchy.Open();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

			DatabaseSectionNode rootNode = (DatabaseSectionNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(DatabaseSectionNode));
			Assert.IsNotNull(rootNode);
			Assert.AreEqual("Service_Dflt", rootNode.DefaultDatabase.Name);

			Assert.AreEqual(6, ApplicationNode.Hierarchy.FindNodesByType(typeof(ConnectionStringSettingsNode)).Count);
			Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(ProviderMappingNode)).Count);
			Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(OraclePackageElementNode)).Count);

			ApplicationNode.Hierarchy.Load();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
			ApplicationNode.Hierarchy.Open();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

			ConnectionStringsSectionNode csNode = (ConnectionStringsSectionNode)ApplicationNode.Hierarchy.FindNodeByType(typeof(ConnectionStringsSectionNode));
			ConnectionStringSettingsNode myNode = new ConnectionStringSettingsNode(new ConnectionStringSettings("foo", ""));
			myNode.AddNode(new ParameterNode("foo", "bar"));
			csNode.AddNode(myNode);

			ApplicationNode.Hierarchy.Save();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

			Assert.AreEqual(7, ApplicationNode.Hierarchy.FindNodesByType(typeof(ConnectionStringSettingsNode)).Count);
			Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(ProviderMappingNode)).Count);
			Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(OraclePackageElementNode)).Count);

			myNode.Remove();
			ApplicationNode.Hierarchy.Save();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

			Assert.AreEqual(6, ApplicationNode.Hierarchy.FindNodesByType(typeof(ConnectionStringSettingsNode)).Count);
			Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(ProviderMappingNode)).Count);
			Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(OraclePackageElementNode)).Count);
		}
	}
}
