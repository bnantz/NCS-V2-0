//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	[TestClass]	
	public class ExceptionHandlingConfigurationViewFixture
	{
		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void WhenReadingUndefinedPolicyNameThrows()
		{
			ExceptionHandlingConfigurationView view = new ExceptionHandlingConfigurationView(new SystemConfigurationSource());
			view.GetExceptionPolicyData("Foo");
		}

		[TestMethod]
		public void GetExceptionHandlingSettingsFromConfiguration()
		{
			ExceptionHandlingConfigurationView view = new ExceptionHandlingConfigurationView(new SystemConfigurationSource());
			ExceptionHandlingSettings settings = view.ExceptionHandlingSettings;

			Assert.IsNotNull(settings);
			Assert.AreEqual(3, settings.ExceptionPolicies.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetPolicyWithNullNameThrows()
		{
			ExceptionHandlingConfigurationView view = new ExceptionHandlingConfigurationView(new SystemConfigurationSource());
			view.GetExceptionPolicyData(null);
		}

		[TestMethod]
		public void CanGetPolicyDataFromConfiguration()
		{
			const string policyName = "Wrap Policy";
			ExceptionHandlingConfigurationView view = new ExceptionHandlingConfigurationView(new SystemConfigurationSource());
			ExceptionPolicyData data = view.GetExceptionPolicyData(policyName);
			
			Assert.IsNotNull(data);
			Assert.AreEqual(policyName, data.Name);			
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructingWithANullConfigurationSourceThrows()
		{
			new ExceptionHandlingConfigurationView(null);
		}
	}
}
