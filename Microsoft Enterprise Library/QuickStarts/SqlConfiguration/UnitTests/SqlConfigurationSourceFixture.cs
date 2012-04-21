//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QUickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration;
using System.Configuration;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif


namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Tests
{
    [TestClass]
    public class SqlConfigurationSourceFixture
    {
        private const string localSection = "dummy.local";
        private const string localSectionSource = "";
        private const string nonExistingSection = "dummy.nonexisting";

        private SqlConfigurationData data1 = null;
        private SqlConfigurationData data2 = null;

        private void UpdateSection(string sectionName, string sectionType, object sectionValue)
        {
            using (SqlConnection myConnection = new SqlConnection(data1.ConnectionString))
            {
                // Create Instance of Connection and Command Object
                SqlCommand myCommand = new SqlCommand("EntLib_SetConfig", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("@section_name", sectionName);
                myCommand.Parameters.AddWithValue("@section_type", sectionType);
                if (sectionValue == null)
                    sectionValue = String.Empty;
                myCommand.Parameters.AddWithValue("@section_value", sectionValue);


                // Execute the command
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            string connectString1 = @"server=(local)\SQLExpress;database=Northwind;Integrated Security=true";
            string connectString2 = @"server=(local)\SQLExpress;database=Northwind;";
            string getSproc = @"EntLib_GetConfig";
            string setStoredProc = @"EntLib_SetConfig";
            string refreshStoredProc = @"UpdateSectionDate";
            string removeStoredProc = @"EntLib_RemoveSection";
            
            data1 = new SqlConfigurationData(connectString1, getSproc, setStoredProc, refreshStoredProc, removeStoredProc);
            data2 = new SqlConfigurationData(connectString2, getSproc, setStoredProc, refreshStoredProc, removeStoredProc);
        }
        
        [TestMethod]
        public void DifferentConfigationInfoDoesNotShareImplementation()
        {
            SqlConfigurationSourceImplementation configSourceImpl1 = SqlConfigurationSource.GetImplementation(data1);
            SqlConfigurationSourceImplementation configSourceImpl2 = SqlConfigurationSource.GetImplementation(data2);

            Assert.IsFalse(object.ReferenceEquals(configSourceImpl1, configSourceImpl2));
        }

        [TestMethod]
        public void SharedConfigurationFilesCanHaveDifferentCasing()
        {
            SqlConfigurationSourceImplementation configSourceImpl1 = SqlConfigurationSource.GetImplementation(data1);

            SqlConfigurationData tempData = new SqlConfigurationData(
                                                    data1.ConnectionString.ToUpper(), 
                                                    data1.GetStoredProcedure.ToUpper(), 
                                                    data1.SetStoredProcedure.ToUpper(), 
                                                    data1.RefreshStoredProcedure.ToUpper(),
                                                    data1.RemoveStoredProcedure.ToUpper());
            SqlConfigurationSourceImplementation configSourceImpl2 = SqlConfigurationSource.GetImplementation(tempData);

            Assert.AreSame(configSourceImpl1, configSourceImpl2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatingSqlConfigurationSourceWithNullArgumentThrows()
        {
            SqlConfigurationSource source = new SqlConfigurationSource(null, null, null, null, null);
        }


        [TestMethod]
        public void DifferentSqlConfigurationSourcesDoNotShareEvents()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(50);

            SqlConfigurationSource.ResetImplementation(data1, true);
            SystemConfigurationSource.ResetImplementation(true);


            bool sysSourceChanged = false;
            bool otherSourceChanged = false;

            SystemConfigurationSource systemSource = new SystemConfigurationSource();
            DummySection sysDummySerializableSection = systemSource.GetSection(localSection) as DummySection;

            SqlConfigurationSource          otherSource =   new SqlConfigurationSource(
                                                                data1.ConnectionString,  
                                                                data1.GetStoredProcedure,  
                                                                data1.SetStoredProcedure, 
                                                                data1.RefreshStoredProcedure, 
                                                                data1.RemoveStoredProcedure);
            SqlConfigurationParameter   parameter =     new SqlConfigurationParameter(
                                                                data1.ConnectionString,  
                                                                data1.GetStoredProcedure,  
                                                                data1.SetStoredProcedure, 
                                                                data1.RefreshStoredProcedure, 
                                                                data1.RemoveStoredProcedure);
            otherSource.Add(parameter,localSection, sysDummySerializableSection);
            
            DummySection    otherDummySerializableSection = otherSource.GetSection(localSection) as DummySection;

            Assert.IsTrue(sysDummySerializableSection != null);
            Assert.IsTrue(otherDummySerializableSection != null);

            systemSource.AddSectionChangeHandler(localSection, delegate(object o, ConfigurationChangedEventArgs args)
                {
                    sysSourceChanged = true;
                });

            otherSource.AddSectionChangeHandler(localSection, delegate(object o, ConfigurationChangedEventArgs args)
                {
                    Assert.AreEqual(12, ((DummySection)otherSource.GetSection(localSection)).Value);
                    otherSourceChanged = true;
                });

            DummySection rwSection = new DummySection();
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMachineConfiguration();
            rwConfiguration.Sections.Remove(localSection);
            rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
            rwSection.Name = localSection;
            rwSection.Value = 12;
            rwSection.SectionInformation.ConfigSource = data1.ConnectionString;

            SqlConfigurationManager.SaveSection(rwSection.Name, rwSection, data1);
            
            Thread.Sleep(200);

            Assert.AreEqual(false, sysSourceChanged);
            Assert.AreEqual(true, otherSourceChanged);

        }
    
        [TestMethod]
        public void GetsNullIfSectionDoesNotExist()
        {
            SqlConfigurationSource source = new SqlConfigurationSource(
                                                                data1.ConnectionString,
                                                                data1.GetStoredProcedure,
                                                                data1.SetStoredProcedure,
                                                                data1.RefreshStoredProcedure,
                                                                data1.RemoveStoredProcedure);
            object section = source.GetSection(nonExistingSection);

            Assert.IsNull(section);
        }

        [TestMethod]
        public void GetsNullForSectionWithEmptyValue()
        {
            UpdateSection(localSection, typeof(DummySection).FullName, null);
            SqlConfigurationSource source = new SqlConfigurationSource(
                                                                data1.ConnectionString,
                                                                data1.GetStoredProcedure,
                                                                data1.SetStoredProcedure,
                                                                data1.RefreshStoredProcedure,
                                                                data1.RemoveStoredProcedure); 
            object section = source.GetSection(localSection);

            Assert.IsNull(section);
        }

    }
}
