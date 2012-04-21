//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class ExceptionHandlingLoggingConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void OpenAndSaveTest()
        {
            Hierarchy.Load();
            Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Hierarchy.Open();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlingSettingsNode)).Count);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionPolicyNode)).Count);
            Assert.AreEqual(2, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionTypeNode)).Count);
            Assert.AreEqual(3, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlerNode)).Count);

            Hierarchy.Load();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
            Hierarchy.Open();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlingSettingsNode)).Count);
            Assert.AreEqual(1, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionPolicyNode)).Count);
            Assert.AreEqual(2, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionTypeNode)).Count);
            Assert.AreEqual(3, Hierarchy.FindNodesByType(ApplicationNode, typeof(ExceptionHandlerNode)).Count);

			ExceptionTypeNode node = (ExceptionTypeNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(ExceptionTypeNode));
			AddLoggingExceptionHandlerCommand cmd = new AddLoggingExceptionHandlerCommand(ServiceProvider);
			cmd.Execute(node);
			Assert.IsNotNull(Hierarchy.FindNodeByType(typeof(LoggingSettingsNode)));
			LoggingExceptionHandlerNode logehNode = (LoggingExceptionHandlerNode)Hierarchy.FindNodeByType(typeof(LoggingExceptionHandlerNode));
			logehNode.FormatterType = typeof(XmlExceptionFormatter);
			logehNode.LogCategory = (CategoryTraceSourceNode)Hierarchy.FindNodeByType(typeof(CategoryTraceSourceNode));
			Hierarchy.Save();



			Hierarchy.Load();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);
			Hierarchy.Open();
			Assert.AreEqual(0, ErrorLogService.ConfigurationErrorCount);

			logehNode = (LoggingExceptionHandlerNode)Hierarchy.FindNodeByType(typeof(LoggingExceptionHandlerNode));
			logehNode.Remove();

			Hierarchy.Save();
        }
    }
}
