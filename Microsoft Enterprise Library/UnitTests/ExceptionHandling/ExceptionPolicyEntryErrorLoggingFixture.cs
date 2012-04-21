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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

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
	public class ExceptionPolicyEntryErrorLoggingFixture
	{
		bool errorCalledBack;
		ExceptionHandlingInstrumentationProvider instrumentationProvider;
		
		[TestInitialize]
		public void SetUp()
		{
			errorCalledBack = false;
		}
		
		[TestMethod]
		public void HandlerThatReturnsNullAtEndOfChainWillCauseErrorEventToBeRaised()
		{
			ExceptionPolicyEntry policyEntry = CreatePolicyEntry(new MockReturnNullExceptionHandler(), PostHandlingAction.ThrowNewException);

			try
			{
				policyEntry.Handle(new ArgumentException());
			}
			catch(ExceptionHandlingException)
			{
			}

			Assert.IsTrue(errorCalledBack);
		}
		
		[TestMethod]
		public void HandlerThatThrowsExceptionCausesErrorEventToBeRaised()
		{
			ExceptionPolicyEntry policyEntry = CreatePolicyEntry(new MockThrowingExceptionHandler(), PostHandlingAction.None);

			try
			{
				policyEntry.Handle(new ArgumentException());
			}
			catch (ExceptionHandlingException)
			{
			}

			Assert.IsTrue(errorCalledBack);
		}

		private ExceptionPolicyEntry CreatePolicyEntry(IExceptionHandler exceptionHandler, PostHandlingAction postHandlingAction)
		{
			List<IExceptionHandler> handlerList = new List<IExceptionHandler>();
			handlerList.Add(exceptionHandler);
			ExceptionPolicyEntry policyEntry = new ExceptionPolicyEntry(postHandlingAction, handlerList);

			instrumentationProvider = new ExceptionHandlingInstrumentationProvider();
			instrumentationProvider.exceptionHandlingErrorOccurred += new EventHandler<ExceptionHandlingErrorEventArgs>(ErrorCallback);

			policyEntry.SetInstrumentationProvider(instrumentationProvider);

			return policyEntry;
		}

		private void ErrorCallback(object sender, ExceptionHandlingErrorEventArgs e)
		{
			errorCalledBack = true;
		}
		
		


	}
}
