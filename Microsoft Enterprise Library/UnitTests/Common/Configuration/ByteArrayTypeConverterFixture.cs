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

using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using SysConfig = System.Configuration;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
	[TestClass]
	public class ByteArrayConverterFixture
	{

		private const string sectionName = "byteArrayConverter";		

		[TestInitialize]
		public void TestInitialize()
		{
			SysConfig.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.Sections.Remove(sectionName);
			config.Save();
		}

		[TestMethod]		
		public void SerializeAndDeserializeAByteArray()
		{
			SysConfig.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			ConverterSection section = new ConverterSection();
			section.ByteArray = new byte[] { 1, 2, 3, 4 };
			config.Sections.Add(sectionName, section);
			config.Save();

			
			config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			section = config.Sections[sectionName] as ConverterSection;
			Assert.IsNotNull(section);
			byte actual = 1;
			Assert.AreEqual(section.ByteArray[0], actual);
		}

		[TestMethod]
		public void SerializeAndDeserialzieANullByteArray()
		{
			SysConfig.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			ConverterSection section = new ConverterSection();
			section.ByteArray = null;
			config.Sections.Add(sectionName, section);
			config.Save();


			config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			section = config.Sections[sectionName] as ConverterSection;
			Assert.IsNotNull(section);
			Assert.IsNull(section.ByteArray);
		}
		
		public class ConverterSection : SerializableConfigurationSection
		{
			private const string propertyName = "property";

			[ConfigurationProperty(propertyName)]
			[TypeConverter(typeof(ByteArrayTypeConverter))]
			public byte[] ByteArray
			{
				get { return (byte[])base[propertyName]; }
				set { base[propertyName] = value; }
			}
		}
		
	}
}
