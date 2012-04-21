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
	public class WrapHandlerFixture	
	{
		private const string message = "message";

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void HandlerThrowsWhenNotWrappingAnException()
		{
			WrapHandler handler = new WrapHandler(message, typeof(object));
			handler.HandleException(new ApplicationException(), Guid.NewGuid());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructingWithNullExceptionTypeThrows()
		{
			WrapHandler handler = new WrapHandler(message, null);
		}

		[TestMethod]
		public void CanWrapException()
		{
			WrapHandler handler = new WrapHandler(message, typeof(ApplicationException));
			Exception ex = handler.HandleException(new InvalidOperationException(), Guid.NewGuid());

			Assert.AreEqual(typeof(ApplicationException), ex.GetType());
			Assert.AreEqual(typeof(ApplicationException), handler.WrapExceptionType);
			Assert.AreEqual(message, ex.Message);
			Assert.AreEqual(typeof(InvalidOperationException), ex.InnerException.GetType());
		}
	}
}
