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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif


namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests.TraceListeners
{
    [TestClass]
    public class TraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void TraceListenerNodeDefaults()
        {
            TraceListenerNode listener = new TraceListenerNodeImpl();

            Assert.AreEqual(TraceOptions.None, listener.TraceOutputOptions);
        }

        [TestMethod]
        public void TraceListenerNodeTest()
        {
            TraceOptions options = TraceOptions.DateTime;

            TraceListenerNode listenerNode = new TraceListenerNodeImpl();
            listenerNode.TraceOutputOptions = options;

            TraceListenerData nodeData = listenerNode.TraceListenerData;

            Assert.AreEqual(options, nodeData.TraceOutputOptions);
        }

        [TestMethod]
        public void TraceListenerNodeDataTest()
        {
            TraceOptions options = TraceOptions.DateTime;

            TraceListenerData traceListenerData = new TraceListenerData();
            traceListenerData.TraceOutputOptions = options;

            TraceListenerNode traceListenerNode = new TraceListenerNodeImpl(traceListenerData);

            Assert.AreEqual(options, traceListenerNode.TraceOutputOptions);
        }

        private class TraceListenerNodeImpl : TraceListenerNode
        {			

			public TraceListenerNodeImpl() : this(new TraceListenerData())
			{

			}

            public TraceListenerNodeImpl(TraceListenerData data)
            {
				Rename(data.Name);
				TraceOutputOptions = data.TraceOutputOptions;
            }


			public override TraceListenerData TraceListenerData
			{
				get 
				{ 
					TraceListenerData data =  new TraceListenerData();
					data.Name = Name;
					data.TraceOutputOptions = TraceOutputOptions;
					return data;
				}
			}
		}
    }
}

