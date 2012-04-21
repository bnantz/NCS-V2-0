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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class ConfigurationSourceSectionFixture
    {
        private const string configFileName = "test.exe.config";
        private const string filePath = "myfile.config";
        private const string fileSourceName = "fileSource";

        [TestMethod]
        public void CanReadAndWriteConfigurationSourceSectionInformation()
        {
            RemoveSection();
            SaveSection();
            System.Configuration.Configuration config = OpenFileConfig();            
            ConfigurationSourceSection section = (ConfigurationSourceSection)config.GetSection(ConfigurationSourceSection.SectionName);
            FileConfigurationSourceElement elem = (FileConfigurationSourceElement)section.Sources.Get(fileSourceName);

            Assert.AreEqual(typeof(FileConfigurationSource), elem.Type);
            Assert.AreEqual(filePath, elem.FilePath);

            RemoveSection();
        }

        private void SaveSection()
        {
            System.Configuration.Configuration config = OpenFileConfig();
            config.Sections.Add(ConfigurationSourceSection.SectionName, CreateConfigurationSourceSection());
            config.Save();            
        }

        private void RemoveSection()
        {
            System.Configuration.Configuration config = OpenFileConfig();
            config.Sections.Remove(ConfigurationSourceSection.SectionName);
            config.Save();
        }

        private static System.Configuration.Configuration OpenFileConfig()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFileName;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return config;
        }        

        private ConfigurationSourceSection CreateConfigurationSourceSection()
        {
            ConfigurationSourceSection section = new ConfigurationSourceSection();
            section.Sources.Add(new FileConfigurationSourceElement(fileSourceName, filePath));
            return section;
        }

		[TestMethod]
		public void CanDeserializeSerializedConfiguration()
		{
			string sourceName1 = "source1";
			string sourceName2 = "source2";

			string sourceFile1 = "file 1";

			ConfigurationSourceSection settings = new ConfigurationSourceSection();

			ConfigurationSourceElement data1 = new FileConfigurationSourceElement(sourceName1, sourceFile1);
			ConfigurationSourceElement data2 = new SystemConfigurationSourceElement(sourceName2);

			settings.Sources.Add(data1);
			settings.Sources.Add(data2);
			settings.SelectedSource = sourceName1;

			IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
			sections[ConfigurationSourceSection.SectionName] = settings;
			IConfigurationSource configurationSource
				= ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

			ConfigurationSourceSection roSettigs = (ConfigurationSourceSection)configurationSource.GetSection(ConfigurationSourceSection.SectionName);

			Assert.IsNotNull(roSettigs);
			Assert.AreEqual(2, roSettigs.Sources.Count);
			Assert.AreEqual(sourceName1, roSettigs.SelectedSource);

			Assert.IsNotNull(roSettigs.Sources.Get(sourceName1));
			Assert.AreSame(typeof(FileConfigurationSourceElement), roSettigs.Sources.Get(sourceName1).GetType());
			Assert.AreEqual(sourceFile1, ((FileConfigurationSourceElement)roSettigs.Sources.Get(sourceName1)).FilePath);

			Assert.IsNotNull(roSettigs.Sources.Get(sourceName2));
			Assert.AreSame(typeof(SystemConfigurationSourceElement), roSettigs.Sources.Get(sourceName2).GetType());
		}
    }
}
