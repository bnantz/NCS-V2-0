//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class SaveFileConfigurationFixture
    {
        private string file;

        [TestInitialize]
        public void TestInitialize()
        {
			file = CreateFile();
        }		

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(file)) File.Delete(file);
        }

        [TestMethod]
        public void CanSaveConfigurationSectionToFile()
        {          
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save(file, InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());

            ValidateConfiguration(file);
        }

		private string CreateFile()
		{
			string tempFile = Path.GetTempPath() + @"app.config";
			XmlDocument doc = new XmlDocument();
			XmlElement elem = doc.CreateElement("configuration");
			doc.AppendChild(elem);
			doc.Save(tempFile);
			return tempFile; ;
		}
	
        private void ValidateConfiguration(string configFile)
        {
            InstrumentationConfigurationSection section = GetSection(configFile);

            Assert.IsTrue(section.PerformanceCountersEnabled);
            Assert.IsTrue(section.WmiEnabled);
            Assert.IsTrue(section.EventLoggingEnabled);
        }

        private InstrumentationConfigurationSection GetSection(string configFile)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            InstrumentationConfigurationSection section = (InstrumentationConfigurationSection)config.GetSection(InstrumentationConfigurationSection.SectionName);
            return section;
        }

       
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToSaveWithNullOrEmptyFileNameThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save((string)null, InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
        }

        [TestMethod]
        public void TryToSaveWithAFileConfigurationSaveParameter()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Add(new FileConfigurationParameter(file), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());

            ValidateConfiguration(file);
        }


		[TestMethod]
		public void TryToSaveWithConfigurationMultipleTimes()
		{
			string tempFile = CreateFile();
			try
			{
				IConfigurationSource source = new FileConfigurationSource(tempFile);
				source.Add(new FileConfigurationParameter(tempFile), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
				ValidateConfiguration(tempFile);
				source.Add(new FileConfigurationParameter(tempFile), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
				ValidateConfiguration(tempFile);
				source.Add(new FileConfigurationParameter(tempFile), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
				ValidateConfiguration(tempFile);
			}			
			finally
			{
				if (File.Exists(tempFile)) File.Delete(tempFile);
			}
		}

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToSaveWithTheWrongConfigurationSaveParameterTypeThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Add(new WrongConfigurationSaveParameter(), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToSaveWithNullOrEmptySectionNameThrows()
        {           
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save(file, null, CreateInstrumentationSection());            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryToSaveWithNullSectionThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save(file, InstrumentationConfigurationSection.SectionName, null);            
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void SaveConfigurationSectionWithNoConfigurationFileThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save("foo.exe.cofig", InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
        }       

        private InstrumentationConfigurationSection CreateInstrumentationSection()
        {
            return new InstrumentationConfigurationSection(true, true, true);
        }

        class WrongConfigurationSaveParameter : IConfigurationParameter
        {
        }
    }
}
