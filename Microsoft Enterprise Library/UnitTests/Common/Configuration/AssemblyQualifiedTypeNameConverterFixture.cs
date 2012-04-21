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
	public class AssemblyQualifiedTypeNameConverterFixture
	{

		private const string sectionName = "assemblyConverter";
		private const string badSectionName = "badAssemblyConverter";

		[TestInitialize]
		public void TestInitialize()
		{
			SysConfig.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.Sections.Remove(sectionName);
			config.Save();
		}

		[TestMethod]		
		public void SerializeAndDeserializeAType()
		{
			SysConfig.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			ConverterSection section = new ConverterSection();
			section.Type = typeof(Exception);
			config.Sections.Add(sectionName, section);
			config.Save();

			
			config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			section = config.Sections[sectionName] as ConverterSection;
			Assert.IsNotNull(section);
			Assert.AreEqual(section.Type, typeof(Exception));
		}

		[TestMethod]		
		[ExpectedException(typeof(ConfigurationErrorsException))]		
		public void SerializeABadTypeThrows()
		{
			SysConfig.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			BadConverterSection section = new BadConverterSection();
			section.BadConverter = new Exception();
			config.Sections.Add(sectionName, section);
			config.Save();
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void DeserialzieABadTypeThrows()
		{
			ConfigurationManager.RefreshSection(badSectionName);
			BadNameSection section = (BadNameSection)ConfigurationManager.GetSection(badSectionName);
			Type t = section.Type;
		}

		public class ConverterSection : SerializableConfigurationSection
		{
			private const string typeProperty = "type";

			[ConfigurationProperty(typeProperty)]
			[TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
			public Type Type
			{
				get { return (Type)base[typeProperty]; }
				set { base[typeProperty] = value; }
			}
		}	

		public class BadConverterSection : SerializableConfigurationSection
		{
			private const string typeProperty = "type";

			[ConfigurationProperty(typeProperty)]
			[TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
			public Exception BadConverter
			{
				get { return (Exception)base[typeProperty]; }
				set { base[typeProperty] = value; }
			}	
		}		
	}

	public class BadNameSection : SerializableConfigurationSection
	{
		private const string typeProperty = "type";

		[ConfigurationProperty(typeProperty, IsRequired= true)]
		[TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
		public Type Type
		{
			get { return (Type)base[typeProperty]; }
			set { base[typeProperty] = value; }
		}
	}
}
