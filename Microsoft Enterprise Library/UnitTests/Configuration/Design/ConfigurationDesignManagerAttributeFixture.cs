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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

[assembly: ConfigurationDesignManager(typeof(ConfigurationDesignManagerAttributeFixture.MyConfigManager))]

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationDesignManagerAttributeFixture
    {
        [TestMethod]
        public void CanGetCustomAttribute()
        {
            ConfigurationDesignManagerAttribute attribute = FindManagerTypeAttribute(typeof(MyConfigManager));
            Assert.IsNotNull(attribute);
        }

        
        private ConfigurationDesignManagerAttribute FindManagerTypeAttribute(Type t)
        {
            object[] attributes = this.GetType().Assembly.GetCustomAttributes(typeof(ConfigurationDesignManagerAttribute), false);
            Assert.IsTrue(attributes.Length > 0);
            foreach (ConfigurationDesignManagerAttribute attribute in attributes)
            {
                if (attribute.ConfigurationDesignManager == t)
                {
                    return attribute;
                }
            }
            return null;
        }

        public class MyConfigManager : IConfigurationDesignManager
        {
            public void Register(IServiceProvider serviceProvider)
            {
            }

            public void Save(IServiceProvider serviceProvider)
            {
            }

            public void Open(IServiceProvider serviceProvider)
            {
            }

            public void BuildConfigurationSource(IServiceProvider serviceProvider, DictionaryConfigurationSource dictionaryConfigurationSource)
            {                
            }
        }
    }
}