//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using System.Diagnostics;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class TraceSourceNodeFixture : ConfigurationDesignHost
    {
        protected override void InitializeCore()
        {
            base.InitializeCore();
        }

        [TestMethod]
        public void TraceSourceNodeDefaults()
        {
            TraceSourceNode node = new TraceSourceNodeImpl(new TraceSourceData());
            Assert.AreEqual(SourceLevels.All, node.SourceLevels);
        }        

        private class TraceSourceNodeImpl : TraceSourceNode
        {
            public TraceSourceNodeImpl(TraceSourceData data)                
            {
				Rename(data.Name);
				SourceLevels = data.DefaultLevel;	
            }
        }
    }
}
