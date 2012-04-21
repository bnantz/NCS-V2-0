//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
	public class ExceptionPolicyImplFixture
	{
		private ExceptionPolicyData PolicyData
		{
			get
			{
				ExceptionPolicyData data = new ExceptionPolicyData("Policy");
				data.ExceptionTypes.Add(new ExceptionTypeData("Exception", typeof(Exception), PostHandlingAction.ThrowNewException));
				return data;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ExceptionHandlingException))]
		public void HandlerInChainThrowsExceptionWhenProduceError()
		{
			ExceptionPolicyData policyData = PolicyData;
			Dictionary<Type, ExceptionPolicyEntry> entries = GetEntries(policyData);
			ExceptionPolicyImpl policyIml = new ExceptionPolicyImpl(policyData.Name, entries);
			policyIml.HandleException(new ArgumentException());
		}		

		[TestMethod]
		public void HandleExceptionThatHasNoEntryReturnsTrue()
		{
			ExceptionPolicyData policyData = PolicyData;
			Dictionary<Type, ExceptionPolicyEntry> entries = GetEntries(policyData);
			ExceptionPolicyImpl policyIml = new ExceptionPolicyImpl(policyData.Name, entries);
			bool handled = policyIml.HandleException(new InvalidCastException());
			Assert.IsTrue(handled);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HandleExceptionWithNullExceptionThrows()
		{
			ExceptionPolicyData policyData = PolicyData;
			Dictionary<Type, ExceptionPolicyEntry> entries = GetEntries(policyData);
			ExceptionPolicyImpl policyIml = new ExceptionPolicyImpl(policyData.Name, entries);
			policyIml.HandleException(null);			
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructWithNullNameThrows()
		{
			ExceptionPolicyData policyData = PolicyData;
			new ExceptionPolicyImpl(null, GetEntries(policyData));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructWithNullEntriesThrows()
		{
			ExceptionPolicyData policyData = PolicyData;
			new ExceptionPolicyImpl(policyData.Name, null);
		}
	    
		private static Dictionary<Type, ExceptionPolicyEntry> GetEntries(ExceptionPolicyData policyData)
		{
			Dictionary<Type, ExceptionPolicyEntry> entries = new Dictionary<Type, ExceptionPolicyEntry>();
			List<IExceptionHandler> handlers = new List<IExceptionHandler>();
			handlers.Add(new MockThrowingExceptionHandler());
			handlers.Add(new MockExceptionHandler(new NameValueCollection()));
			foreach (ExceptionTypeData typeData in policyData.ExceptionTypes)
			{
				entries.Add(typeof(ArgumentException), new ExceptionPolicyEntry(typeData.PostHandlingAction,
					handlers));
			}
			return entries;
		}
	}
}
