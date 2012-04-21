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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using System.Collections.Generic;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class AddLoggingSettingsNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ExecutingAddLoggingSettingsAddsDefaults()
        {
            AddLoggingSettingsNodeCommand addLoggingSettingsCommand = new AddLoggingSettingsNodeCommand(ServiceProvider);

            addLoggingSettingsCommand.Execute(ApplicationNode);

            LoggingSettingsNode loggingSettingsNode = (LoggingSettingsNode) Hierarchy.FindNodeByType(ApplicationNode, typeof(LoggingSettingsNode));
            FormattedEventLogTraceListenerNode defaultEventLogListenerNode = (FormattedEventLogTraceListenerNode) Hierarchy.FindNodeByType(ApplicationNode, typeof(FormattedEventLogTraceListenerNode));
            TextFormatterNode defaultFormatterNode = (TextFormatterNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(TextFormatterNode));
            ErrorsTraceSourceNode errorTraceSourceNode = (ErrorsTraceSourceNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(ErrorsTraceSourceNode));
            CategoryTraceSourceCollectionNode categoryCollectionNode = (CategoryTraceSourceCollectionNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(CategoryTraceSourceCollectionNode));
            TraceListenerReferenceNode defaultErrorListenerReferenceNode = (TraceListenerReferenceNode) Hierarchy.FindNodeByType(errorTraceSourceNode, typeof(TraceListenerReferenceNode));

            Assert.AreEqual(1, categoryCollectionNode.Nodes.Count);
            ConfigurationNode generalCategoryNode = categoryCollectionNode.Nodes[0];
            TraceListenerReferenceNode defaultGeneralCategoryListenerRefenceNode = (TraceListenerReferenceNode) Hierarchy.FindNodeByType(generalCategoryNode, typeof(TraceListenerReferenceNode));

            Assert.AreEqual("General", generalCategoryNode.Name);
            Assert.IsNotNull(defaultErrorListenerReferenceNode);
            Assert.IsNotNull(defaultGeneralCategoryListenerRefenceNode);
            Assert.IsNotNull(defaultFormatterNode);
            Assert.IsNotNull(defaultEventLogListenerNode);
            Assert.AreEqual(defaultFormatterNode, defaultEventLogListenerNode.Formatter);
            Assert.AreEqual(defaultEventLogListenerNode, defaultGeneralCategoryListenerRefenceNode.ReferencedTraceListener);
            Assert.AreEqual(defaultEventLogListenerNode, defaultErrorListenerReferenceNode.ReferencedTraceListener);
            Assert.AreEqual(loggingSettingsNode.DefaultCategory, generalCategoryNode);

        }
    }
}
