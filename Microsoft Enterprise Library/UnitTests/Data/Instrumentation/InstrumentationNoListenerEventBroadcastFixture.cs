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
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Instrumentation
{
    [TestClass]
    public class InstrumentationNoListenerEventBroadcastFixture
    {
        [TestMethod]
        public void NoEventBroadcastIfNoEventRegistered()
        {
            string connectionString = @"server=(local)\sqlexpress;database=northwind;integrated security=true;";
            SqlDatabase db = new SqlDatabase(connectionString);

            db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
        }

        [TestMethod]
        public void NoConnectionFailedEventBroadcastWithNoListener()
        {
            string connectionString = @"null;";
            SqlDatabase db = new SqlDatabase(connectionString);
            try
            {
                db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
            }
            catch (ArgumentException)
            {
            }
        }
    }
}
