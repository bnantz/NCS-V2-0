//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Tests
{
	[TestClass]
	public class FormattedDatabaseTraceListenerFixture
	{
		public const string connectionString = @"server=(local)\SQLEXPRESS;database=Logging;Integrated Security=true";
		public const string wrongConnectionString = @"server=(local)\SQLEXPRESS;database=Northwind;Integrated Security=true";

		private void ClearLogs()
		{
			//clear the log entries from the database
			DatabaseProviderFactory factory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
			Data.Database db = factory.CreateDefault();
			DbCommand command = db.GetStoredProcCommand("ClearLogs");
			db.ExecuteNonQuery(command);
		}

		private string GetLastLogMessage(string databaseName)
		{
			DatabaseProviderFactory factory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
			Data.Database db = ((databaseName == null) || (databaseName.Length == 0)) ? factory.CreateDefault() : factory.Create(databaseName);
			DbCommand command = db.GetSqlStringCommand("SELECT TOP 1 FormattedMessage FROM Log ORDER BY TimeStamp DESC");
			string messageContents = Convert.ToString(db.ExecuteScalar(command));
			return messageContents;
		}

        private int GetNumberOfLogMessage(string databaseName)
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
            Data.Database db = ((databaseName == null) || (databaseName.Length == 0)) ? factory.CreateDefault() : factory.Create(databaseName);
            DbCommand command = db.GetSqlStringCommand("SELECT COUNT(*) FROM Log");
            int numMessages = Convert.ToInt32(db.ExecuteScalar(command));
            return numMessages;
        }
        
	    
	    [TestInitialize]
		public void SetUp()
		{
			ClearLogs();
		}

		[TestCleanup]
		public void Teardown()
		{
			ClearLogs();
		}

		[TestMethod]
		public void FormatterListenerWithStoredProcsAndDbInstance()
		{
			FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(connectionString), "WriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));

			// need to go through the source to get a TraceEventCache
			TraceSource source = new TraceSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));

			string messageContents = GetLastLogMessage("LoggingDb");

			Assert.AreEqual("TEST" + Environment.NewLine + "TEST", messageContents);
		}

		[TestMethod]
		[ExpectedException(typeof(SqlException))]
		public void FormatterListenerWithStoredProcsAndWrongDbInstance()
		{
			FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(wrongConnectionString), "WriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));
			TraceSource source = new TraceSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
		}

		[TestMethod]
		[ExpectedException(typeof(SqlException))]
		public void FormatterListenerWithWrongStoredProcs()
		{
			FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(connectionString), "WrongWriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));
			TraceSource source = new TraceSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
		}

		[TestMethod]
		public void LogWithMultipleCategories()
		{
			FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(connectionString), "WriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));

			// need to go through the source to get a TraceEventCache
			TraceSource source = new TraceSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			LogEntry logEntry = new LogEntry();
			logEntry.Message = "message";
			logEntry.Categories.Add("FormattedCategory");
			logEntry.Categories.Add("DictionaryCategory");
			logEntry.EventId = 123;
			logEntry.Priority = 11;
			logEntry.Severity = TraceEventType.Error;
			logEntry.Title = "title";
			source.TraceData(TraceEventType.Error, 0, logEntry);

			DatabaseProviderFactory factory = new DatabaseProviderFactory(ConfigurationSourceFactory.Create());
			Data.Database db = factory.CreateDefault();
			DbCommand command = db.GetSqlStringCommand("SELECT Count(*) FROM Category");
			int categoryCount = Convert.ToInt32(db.ExecuteScalar(command));

			Assert.AreEqual(2, categoryCount);
		}

		[TestMethod]
		public void FormatterListenerWriteWithStoredProcsAndDbInstance()
		{
			FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(connectionString), "WriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));
			listener.Write("Test Message");

			string messageContents = GetLastLogMessage("LoggingDb");

			Assert.AreEqual("Test Message", messageContents);
		}

		[TestMethod]
		public void FormatterListenerFiresInstrumentation()
		{
			FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(connectionString), "WriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));
			MockLoggingInstrumentationListener instrumentationListener = new MockLoggingInstrumentationListener();
			ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
			binder.Bind(listener.GetInstrumentationEventProvider(), instrumentationListener);

			TraceEventCache eventCache = new TraceEventCache();
			listener.TraceData(eventCache, "", TraceEventType.Error, 0, new LogEntry("message", "category", 0, 0, TraceEventType.Error, "title", null));

			Assert.AreEqual(1, instrumentationListener.calls);
		}

		[TestMethod]
		public void FormatterListenerDoesNotFireInstrumentationWhenTracingString()
		{
			FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(connectionString), "WriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));
			MockLoggingInstrumentationListener instrumentationListener = new MockLoggingInstrumentationListener();
			ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
			binder.Bind(listener.GetInstrumentationEventProvider(), instrumentationListener);

			TraceEventCache eventCache = new TraceEventCache();
			listener.TraceData(eventCache, "", TraceEventType.Error, 0, "message");


			Assert.AreEqual(0, instrumentationListener.calls);
		}

		[TestMethod]
		public void FormatterListenerAsString()
		{
			FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(connectionString), "WriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));

			// need to go through the source to get a TraceEventCache
			TraceSource source = new TraceSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			source.TraceData(TraceEventType.Error, 0, "test message");

			string messageContents = GetLastLogMessage("LoggingDb");

			Assert.AreEqual("test message", messageContents);
		}

        [TestMethod]
        public void LogToDatabaseUsingDirectObjectOnlyResultsInOneMessage()
        {
            FormattedDatabaseTraceListener listener = new FormattedDatabaseTraceListener(new SqlDatabase(connectionString), "WriteLog", "AddCategory", new TextFormatter("TEST{newline}TEST"));

            TraceSource source = new TraceSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            int numMessages = GetNumberOfLogMessage("LoggingDb");

            source.TraceData(TraceEventType.Error, 1, new TestCustomObject());
            source.Close();

            int newNumMessages = GetNumberOfLogMessage("LoggingDb");

            Assert.AreEqual(numMessages, newNumMessages - 1);
        }

		private class MockLoggingInstrumentationListener
		{
			public int calls = 0;

			[InstrumentationConsumer("TraceListenerEntryWritten")]
			public void TraceListenerEntryWritten(object sender, EventArgs e)
			{
				calls++;
			}

		}
	}
}
