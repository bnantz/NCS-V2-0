//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Collections.Generic;

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
    public class PriorityFilterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInPriorityFilterNodeThrows()
        {
            new PriorityFilterNode(null);
        }

        [TestMethod]
        public void PriorityFilterNodeDefaults()
        {
            PriorityFilterNode filter = new PriorityFilterNode();

            Assert.AreEqual("Priority Filter", filter.Name);
            Assert.IsNull(filter.MinimumPriority);            
        }        

        [TestMethod]
        public void PriorityFilterDataTest()
        {
            string name = "some name";
            int minimumPriority = 123;
            int maximumPriority = 234;

            PriorityFilterData data = new PriorityFilterData();
            data.Name = name;
            data.MinimumPriority = minimumPriority;
            data.MaximumPriority = maximumPriority;

            PriorityFilterNode node = new PriorityFilterNode(data);

            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(minimumPriority, node.MinimumPriority);
            Assert.AreEqual(maximumPriority, node.MaximumPriority);

        }

        [TestMethod]
        public void PriorityFilterNodeTest()
        {
            string name = "some name";
            int minimumPriority = 123;
            int maximumPriority = Int32.MaxValue;

            PriorityFilterNode node = new PriorityFilterNode();
            node.Name = name;
            node.MaximumPriority = maximumPriority;
            node.MinimumPriority = minimumPriority;

            PriorityFilterData nodeData = (PriorityFilterData) node.LogFilterData;

            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(minimumPriority, nodeData.MinimumPriority);
            Assert.AreEqual(Int32.MaxValue, nodeData.MaximumPriority);
        }

        [TestMethod]
        public void MinimumPriotityGreaterThanMaximumPriorityResultsInValidationError()
        {
            PriorityFilterMaximumPriorityValidationAttribute maxPrioValidation = new PriorityFilterMaximumPriorityValidationAttribute();
            PriorityFilterNode prioFilterNode = new PriorityFilterNode();

            prioFilterNode.MaximumPriority = 4;
            prioFilterNode.MinimumPriority = 8;
            List<ValidationError> validationErrors = new List<ValidationError>();
            maxPrioValidation.Validate(prioFilterNode, typeof(PriorityFilterNode).GetProperty("MaximumPriority"), validationErrors, ServiceProvider);

            Assert.AreEqual(1, validationErrors.Count);

        }
    }
}
