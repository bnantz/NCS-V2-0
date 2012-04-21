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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

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
    public class NodeCreationServiceFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void CreateNodeTest()
        {
            Type t = typeof(InstrumentationNode);
			NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(
				new AddChildNodeCommand(ServiceProvider, t), 
				t, typeof(InstrumentationConfigurationSection), "Instrumentation");
            NodeCreationService.AddNodeCreationEntry(entry);

			InstrumentationNode node = NodeCreationService.CreateNodeByDataType(
				typeof(InstrumentationConfigurationSection)) as InstrumentationNode;
            Assert.IsNotNull(node);
        }		
    }
}
