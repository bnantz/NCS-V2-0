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
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.Tests 
{
    [TestClass]
    public class ApplicationConfigurationFileValidationFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void ValidatingInvalidPathCharactersFails()
        {
            ApplicationConfigurationFileNode appConfigurationFileNode = new ApplicationConfigurationFileNode("def|abc");
            DoValidation(appConfigurationFileNode, 1);
        }

        private void DoValidation(ApplicationConfigurationFileNode fileNode, int expectedErrors)
        {
            PropertyInfo fileNameInfo = typeof(ApplicationConfigurationFileNode).GetProperty("FileName");
            ApplicationConfigurationFileValidationAttribute attr = new ApplicationConfigurationFileValidationAttribute();
            List<ValidationError> errors = new List<ValidationError>();
            attr.Validate(fileNode, fileNameInfo, errors, ServiceProvider);
            Assert.AreEqual(expectedErrors, errors.Count);
        }

        private class ApplicationConfigurationFileNode
        {
            private string fileName;

            public ApplicationConfigurationFileNode(string fileName)
            {
                this.fileName = fileName;
            }

            [ApplicationConfigurationFileValidation]
            public string FileName
            {
                get { return fileName; }
            }
        }
    }
}
