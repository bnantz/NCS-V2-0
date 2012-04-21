//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.Tests.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.Tests
{
    [TestClass]
    public class DataCacheStorageNodeFixture 
    {
		[TestMethod]
		public void EnsureDataCacheStorageNodePropertyCatoriesAndDescriptions()
		{
			Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(DataCacheStorageNode), "DatabaseInstance", Resources.DatabaseNameDescription));
			Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(DataCacheStorageNode), "DatabaseInstance"));

			Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(DataCacheStorageNode), "PartitionName", Resources.DatabasePartitionNameDesciption));
			Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(DataCacheStorageNode), "PartitionName"));
		}		

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructinWithNullDataThrows()
		{
			new DataCacheStorageNode(null);
		}
	}
}