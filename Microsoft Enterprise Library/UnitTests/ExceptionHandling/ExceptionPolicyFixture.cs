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
using SysConfig = System.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using System.Diagnostics;
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
    public class ExceptionPolicyFixture
    {
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HandleExceptionWithNullPolicyThrows()
		{
			ExceptionPolicy.HandleException(new UnauthorizedAccessException(), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HandleExceptionWithNullExceptionThrows()
		{
			ExceptionPolicy.HandleException(null, "Wrap Policy");
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void UndefinedPolicyRequestedThrows()
		{
			ExceptionPolicy.HandleException(new MockException(), "Undefined Policy");
		}		

		[TestMethod]
		public void WrapHandlerTest()
		{
			Exception originalException = new ArgumentNullException();
			Exception wrappedException = null;

			try
			{
				ExceptionPolicy.HandleException(originalException, "Wrap Policy");
			}
			catch (Exception ex)
			{
				wrappedException = ex;
			}

			Assert.IsNotNull(wrappedException);
			Assert.AreEqual("Test Message", wrappedException.Message);
			Assert.AreEqual(typeof(ApplicationException), wrappedException.GetType());
			Assert.IsNotNull(wrappedException.InnerException);
		}

		[TestMethod]
		public void ReplaceHandlerTest()
		{
			Exception originalException = new ArgumentNullException();
			Exception replacedException = null;

			try
			{
				ExceptionPolicy.HandleException(originalException, "Replace Policy");
			}
			catch (Exception ex)
			{
				replacedException = ex;
			}

			Assert.IsNotNull(replacedException);
			Assert.AreEqual("Test Message", replacedException.Message);
			Assert.AreEqual(typeof(ApplicationException), replacedException.GetType());
			Assert.IsNull(replacedException.InnerException);
		}

		[TestMethod]
		public void CustomHandlerTest()
		{
			MockExceptionHandler.Clear();
			Exception originalException = new ArgumentNullException();
			Exception customException = null;

			try
			{
				ExceptionPolicy.HandleException(originalException, "Custom Policy");
			}
			catch (Exception ex)
			{
				customException = ex;
			}

			Assert.IsNotNull(customException);
			Assert.AreEqual(2, MockExceptionHandler.attributes.Count);
			Assert.AreEqual("32", MockExceptionHandler.attributes["Age"]);
			Assert.AreEqual(typeof(ArgumentNullException), customException.GetType());			

			MockExceptionHandler.Clear();
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void InvalidExceptionTypeInConfigurationTest()
		{
			try
			{
				throw new System.Security.SecurityException("ExceptionType in Config File is not available");
			}
			catch (Exception ex)
			{

				bool rethrow = ExceptionPolicy.HandleException(ex, "InvalidExceptionTypeInConfiguration");
				if (rethrow)
				{
					throw;
				}
			}
		}

        [TestMethod]
        public void WmiEventFiredWhenCreatingUndefinedPolicy()
        {
            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                try
                {
                    ExceptionPolicy.HandleException(new Exception(), "ThisIsAnUnknownKey");
                }
                catch (ConfigurationErrorsException)
                {
                    eventListener.WaitForEvents();
                    Assert.AreEqual(1, eventListener.EventsReceived.Count);
                    Assert.AreEqual("ExceptionHandlingConfigurationFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                    Assert.AreEqual("ThisIsAnUnknownKey", eventListener.EventsReceived[0].GetPropertyValue("PolicyName"));
                    string exceptionMessage = (string)eventListener.EventsReceived[0].GetPropertyValue("ExceptionMessage");

                    Assert.IsFalse(-1 == exceptionMessage.IndexOf("ThisIsAnUnknownKey"));

                }
            }
        }

        [TestMethod]
        public void EventLogWrittenWhenCreatingUndefinedPolicy()
        {
            try
            {
                ExceptionPolicy.HandleException(new Exception(), "ThisIsAnUnknownKey");
            }
            catch (ConfigurationErrorsException)
            {
                using (EventLog applicationLog = new EventLog("Application"))
                {
                    EventLogEntry lastEntry = applicationLog.Entries[applicationLog.Entries.Count - 1];

                    Assert.AreEqual("Enterprise Library ExceptionHandling", lastEntry.Source);
                    Assert.IsTrue(lastEntry.Message.Contains("ThisIsAnUnknownKey"));
                }
            }
        }
    }
}

