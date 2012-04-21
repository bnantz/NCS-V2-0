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

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
	[TestClass]
	public class EventLogEntryFormatterFixture
	{
		private const string applicationName = "Application.exe";
		private const string blockName = "Test Block";
		private const string exceptionMessage = "Exception message";
		private const string errorMessage = "Error Message";
		private const string extraInformation1 = "extra 1";
		private const string extraInformation2 = "extra 2";

		[TestMethod]
		public void CanGetFormattedEntryWithMessage()
		{
			EventLogEntryFormatter formatter = new EventLogEntryFormatter(applicationName, blockName);

			string entryText = formatter.GetEntryText(errorMessage);

			Assert.IsNotNull(entryText);
			Assert.IsTrue(entryText.IndexOf(applicationName) > 0);
			Assert.IsTrue(entryText.IndexOf(blockName) > 0);
			Assert.IsTrue(entryText.IndexOf(errorMessage) > 0);
		}

		[TestMethod]
		public void CanGetFormattedEntryWithMessageAndExtraInformation()
		{
			EventLogEntryFormatter formatter = new EventLogEntryFormatter(applicationName, blockName);
			string[] extraInformation = new string[] { extraInformation1, extraInformation2 };

			string entryText = formatter.GetEntryText(errorMessage, extraInformation);

			Assert.IsNotNull(entryText);
			Assert.IsTrue(entryText.IndexOf(applicationName) > 0);
			Assert.IsTrue(entryText.IndexOf(blockName) > 0);
			Assert.IsTrue(entryText.IndexOf(errorMessage) > 0);
			Assert.IsTrue(entryText.IndexOf(extraInformation1) > 0);
			Assert.IsTrue(entryText.IndexOf(extraInformation2) > 0);
		}

		[TestMethod]
		public void CanGetFormattedEntryWithMessageAndExceptionAndExtraInformation()
		{
			EventLogEntryFormatter formatter = new EventLogEntryFormatter(applicationName, blockName);
			Exception ex = null;
			string[] extraInformation = new string[] { extraInformation1, extraInformation2 };

			try
			{
				throw new Exception(exceptionMessage);
			}
			catch (Exception e)
			{
				ex = e;
			}

			string entryText = formatter.GetEntryText(errorMessage, ex, extraInformation);

			Assert.IsNotNull(entryText);
			Assert.IsTrue(entryText.IndexOf(errorMessage) > 0);
			Assert.IsTrue(entryText.IndexOf(applicationName) > 0);
			Assert.IsTrue(entryText.IndexOf(blockName) > 0);
			Assert.IsTrue(entryText.IndexOf(exceptionMessage) > 0);
			Assert.IsTrue(entryText.IndexOf(extraInformation1) > 0);
			Assert.IsTrue(entryText.IndexOf(extraInformation2) > 0);
		}
	}
}