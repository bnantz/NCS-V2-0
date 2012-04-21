//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Data;
using System.Data.Common;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
	public class ParameterDiscoveryFixture
    {
		private DbCommand storedProcedure;

		public ParameterDiscoveryFixture(DbCommand storedProcedure)
		{
			this.storedProcedure = storedProcedure;
		}

        public void CanCreateStoredProcedureCommand()
        {
            Assert.AreEqual(storedProcedure.CommandType, CommandType.StoredProcedure);
        }

		public class TestCache : ParameterCache
		{
			public bool CacheUsed = false;

			protected override void AddParametersFromCache(DbCommand command, Database database)
			{
				CacheUsed = true;
				base.AddParametersFromCache(command, database);
			}
		}		
    }
}

