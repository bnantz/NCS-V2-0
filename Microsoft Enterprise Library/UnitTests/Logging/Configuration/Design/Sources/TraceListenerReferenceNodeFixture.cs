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
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class TraceListenerReferenceNodeFixture: ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TraceListenerWithNullDataThrows()
        {
            new TraceListenerReferenceNode(null);
        }

        [TestMethod]
        public void TraceListenerNodeDefaultValues()
        {
            TraceListenerReferenceNode traceListenerReference = new TraceListenerReferenceNode();
            
            Assert.AreEqual("TraceListener Reference", traceListenerReference.Name);
            Assert.IsNull(traceListenerReference.ReferencedTraceListener);
        }

        [TestMethod]
        public void TraceListenerNameEqualsReferencedTraceListenerName()
        {
            string traceListenerName = "a traceListener";

            TraceListenerReferenceNode traceListenerReference = new TraceListenerReferenceNode();

            TraceListenerCollectionNode traceListenerCollection = new TraceListenerCollectionNode();
            TraceListenerNode aTraceListener = new WmiTraceListenerNode(new WmiTraceListenerData(traceListenerName));

            ApplicationNode.AddNode(traceListenerReference);
            ApplicationNode.AddNode(traceListenerCollection);
            traceListenerCollection.AddNode(aTraceListener);

            traceListenerReference.ReferencedTraceListener = aTraceListener;

            Assert.AreEqual(traceListenerName, traceListenerReference.Name);
        }

        [TestMethod]
        public void TraceListenerReferenceNameWillUpdateOnChange()
        {
            string traceListenerName = "a traceListener";

            TraceListenerReferenceNode traceListenerReference = new TraceListenerReferenceNode();

            TraceListenerCollectionNode traceListenerCollection = new TraceListenerCollectionNode();
            TraceListenerNode aTraceListener = new WmiTraceListenerNode(new WmiTraceListenerData(traceListenerName));

            ApplicationNode.AddNode(traceListenerReference);
            ApplicationNode.AddNode(traceListenerCollection);
            traceListenerCollection.AddNode(aTraceListener);

            traceListenerReference.ReferencedTraceListener = aTraceListener;
            Assert.AreEqual(aTraceListener.Name, traceListenerReference.Name);

            aTraceListener.Name = traceListenerName;
            Assert.AreEqual(traceListenerName, traceListenerReference.Name);
        }

        [TestMethod]
        public void TraceListenerReferencePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(TraceListenerReferenceNode), "Name"));
        }
    }
}
