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
using System.Data.SqlClient;
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
    public class InstrumentationEventBroadcastFixture
    {
        const string connectionString = @"server=(local)\sqlexpress;database=northwind;integrated security=true;";
        Database db;
        MockListener listener;

        [TestInitialize]
        public void SetUp()
        {
            db = new SqlDatabase(connectionString);
            listener = new MockListener();
            ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
            binder.Bind(db.GetInstrumentationEventProvider(), listener);
        }

        [TestMethod]
        public void ConnectionOpenedEventBroadcast()
        {
            db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");

            Assert.IsNotNull(listener.connectionOpenedArgs);
        }

        [TestMethod]
        public void ConnectionFailedEventBroadcast()
        {
			db = new SqlDatabase("invalid;");
			ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
			binder.Bind(db.GetInstrumentationEventProvider(), listener);

            try
            {
                db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");
            }
            catch (ArgumentException)
            {
            }

			Assert.AreEqual("invalid;", listener.connectionDataArgs.ConnectionString);
        }

        [TestMethod]
        public void ConnectionExecutedEventBroadcast()
        {
            db.ExecuteNonQuery(CommandType.Text, "Select count(*) from Region");

            AssertDateIsWithinBounds(DateTime.Now, listener.commandExecutedArgs.StartTime, 2);
        }
        
        [TestMethod]
        public void CommandFailedEventBroadcast()
        {
            try
            {
                db.ExecuteNonQuery(CommandType.StoredProcedure, "NonExistentStoredProcedure");
            }
            catch (SqlException)
            {
            }
            Assert.AreEqual(connectionString, listener.commandFailedArgs.ConnectionString);
            Assert.AreEqual("NonExistentStoredProcedure", listener.commandFailedArgs.CommandText);
        }

        private void AssertDateIsWithinBounds(DateTime expectedTime, DateTime actualTime, int maxDifference)
        {
            int diff = (expectedTime - actualTime).Seconds;
            Assert.IsTrue(diff <= maxDifference);
        }

        public class MockListener
        {
            public ConnectionFailedEventArgs connectionDataArgs;
            public CommandExecutedEventArgs commandExecutedArgs;
            public CommandFailedEventArgs commandFailedArgs;
			public EventArgs connectionOpenedArgs;

            [InstrumentationConsumer("ConnectionOpened")]
			public void ConnectionOpenedHandler(object sender, EventArgs e)
			{
				connectionOpenedArgs = e;
			}

            [InstrumentationConsumer("ConnectionFailed")]
            public void ConnectionFailedHandler(object sender, ConnectionFailedEventArgs e)
            {
                connectionDataArgs = e;
            }

            [InstrumentationConsumer("CommandExecuted")]
            public void CommandExecutedHandler(object sender, CommandExecutedEventArgs e)
            {
                commandExecutedArgs = e;
            }
            
            [InstrumentationConsumer("CommandFailed")]
            public void CommandFailedHandler(object sender, CommandFailedEventArgs e)
            {
                commandFailedArgs = e;
            }
        }
    }
}
