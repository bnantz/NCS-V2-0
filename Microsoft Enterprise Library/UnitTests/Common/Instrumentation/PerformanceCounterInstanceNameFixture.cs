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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Instrumentation
{
	[TestClass]
	public class PerformanceCounterInstanceNameFixture
	{
		[TestMethod]
		public void SingleCharacterPrefixAndSuffixCreateValidName()
		{
			PerformanceCounterInstanceName instanceName = new PerformanceCounterInstanceName("a", "b");
			Assert.AreEqual("a - b", instanceName.ToString());
		}
		
		[TestMethod]
		public void EmptyPrefixPrintsJustSuffixForName()
		{
			PerformanceCounterInstanceName instanceName = new PerformanceCounterInstanceName(String.Empty, "b");
			Assert.AreEqual("b", instanceName.ToString());
		}
		
		[TestMethod]
		public void OverlyLongPrefixIsTruncatedToMaxLengthInGeneratedName()
		{
			PerformanceCounterInstanceName instanceName = new PerformanceCounterInstanceName("01234", "b", 3, 15);
			Assert.AreEqual("012 - b", instanceName.ToString());
		}
		
		[TestMethod]
		public void OverlyLongSuffixIsTruncatedToMaxLengthInGeneratedName()
		{
			PerformanceCounterInstanceName instanceName = new PerformanceCounterInstanceName("a", "01234", 15, 3);
			Assert.AreEqual("a - 012", instanceName.ToString());
		}
	}
}
