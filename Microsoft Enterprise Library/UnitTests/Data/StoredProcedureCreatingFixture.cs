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

using System;
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
    /// <summary>
    /// Use the Data block to execute a create a stored procedure script using ExecNonQuery.
    /// </summary>    
    public class StoredProcedureCreatingFixture
    {
		private Database db;

		public StoredProcedureCreatingFixture(Database db)
		{
			this.db = db;
		}

        public void CanGetOutputValueFromStoredProcedure()
        {
            DbCommand storedProcedure = this.db.GetStoredProcCommand("TestProc", null, "ALFKI");
            this.db.ExecuteNonQuery(storedProcedure);

            int resultCount = Convert.ToInt32(this.db.GetParameterValue(storedProcedure, "vCount"));
            Assert.AreEqual(6, resultCount);
        }

        public void CanGetOutputValueFromStoredProcedureWithCachedParameters()
        {
            DbCommand storedProcedure = this.db.GetStoredProcCommand("TestProc", null, "ALFKI");
            this.db.ExecuteNonQuery(storedProcedure);

            DbCommand duplicateStoredProcedure = this.db.GetStoredProcCommand("TestProc", null, "CHOPS");
            this.db.ExecuteNonQuery(duplicateStoredProcedure);

            int resultCount = Convert.ToInt32(this.db.GetParameterValue(duplicateStoredProcedure, "vCount"));
            Assert.AreEqual(8, resultCount);
        }

        public void ArgumentExceptionWhenThereAreTooFewParameters()
        {
            DbCommand storedProcedure = this.db.GetStoredProcCommand("TestProc", "ALFKI");
            this.db.ExecuteNonQuery(storedProcedure);
        }

        public void ArgumentExceptionWhenThereAreTooManyParameters()
        {
            DbCommand invalidCommand = this.db.GetStoredProcCommand("TestProc", "ALFKI", "EIEIO", "Hello");
            this.db.ExecuteNonQuery(invalidCommand);
        }

        public void ExceptionThrownWhenReadingParametersFromCacheWithTooFewParameterValues()
        {
            DbCommand storedProcedure = this.db.GetStoredProcCommand("TestProc", null, "ALFKI");
            this.db.ExecuteNonQuery(storedProcedure);

            DbCommand duplicateStoredProcedure = this.db.GetStoredProcCommand("TestProc", "CHOPS");
            this.db.ExecuteNonQuery(duplicateStoredProcedure);
        }
    }
}

