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
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
	[TestClass]
    public class FlatFileTraceListenerFixture
	{
        [TestInitialize]
	    public void Initialize()
        {
            //Trace.AutoFlush = false;
        }
	    
	    [TestMethod]
		public void ListenerWillUseFormatterIfExists()
		{
			File.Delete("trace.log");

			FlatFileTraceListener listener = new FlatFileTraceListener("trace.log",new TextFormatter("DUMMY{newline}DUMMY"));

			// need to go through the source to get a TraceEventCache
			LogSource source = new LogSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
            source.Dispose();

            string strFileContents = GetFileContents("trace.log");

            Assert.AreEqual("DUMMY" + Environment.NewLine + "DUMMY" + Environment.NewLine, strFileContents);
		}

        [TestMethod]
        public void ListenerWithHeaderAndFooterWillUseFormatterIfExists()
        {
			File.Delete("tracewithheaderandfooter.log");  
        	
            FlatFileTraceListener listener = new FlatFileTraceListener("tracewithheaderandfooter.log", "--------header------", "=======footer===========", new TextFormatter("DUMMY{newline}DUMMY"));

            // need to go through the source to get a TraceEventCache
            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
            source.Dispose();

            string strFileContents = GetFileContents("tracewithheaderandfooter.log");

            Assert.AreEqual("--------header------" + Environment.NewLine + "DUMMY" + Environment.NewLine + "DUMMY" + Environment.NewLine + "=======footer===========" + Environment.NewLine, strFileContents);
        }
                
        [TestMethod]
		public void ListenerWillFallbackToTraceEntryToStringIfFormatterDoesNotExists()
		{
			LogEntry testLogEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            StreamWriter writer = new StreamWriter("trace.log");
            FlatFileTraceListener listener = new FlatFileTraceListener(writer);

			// need to go through the source to get a TraceEventCache
			LogSource source = new LogSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			source.TraceData(TraceEventType.Error, 0, testLogEntry);
            source.Dispose();

            string strFileContents = GetFileContents("trace.log");

			string testLogEntryAsString = testLogEntry.ToString();

			Assert.IsTrue(-1 != strFileContents.IndexOf(testLogEntryAsString));
		}
        	    
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void FlatFileListenerWillFallbackIfNotPriviledgesToWrite()
        {
            string fileName = @"trace.log";
            string fullPath = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), fileName);

            File.Delete(fileName);

            FileIOPermission fileIOPerm1 = new FileIOPermission(PermissionState.None);
            fileIOPerm1.SetPathList(FileIOPermissionAccess.Read, fullPath);
            fileIOPerm1.PermitOnly();

            try
            {
                FlatFileTraceListener listener = new FlatFileTraceListener(fileName,"---header---","***footer***",new TextFormatter("DUMMY{newline}DUMMY"));

                // need to go through the source to get a TraceEventCache
                LogSource source = new LogSource("notfromconfig", SourceLevels.All);
                source.Listeners.Add(listener);
                source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
                source.Dispose();
            }
            catch (SecurityException)
            {
                FileIOPermission.RevertAll();
                throw;
            }
        }
        
	    [TestMethod]
	    public void FlatFileTraceListenerMultipleWrites()
	    {
	        File.Delete("tracewithheaderandfooter.log");
	        string header = "--------header------";
	        int numberOfWrites = 4;

	        FlatFileTraceListener listener = new FlatFileTraceListener("tracewithheaderandfooter.log", header, "=======footer===========", new TextFormatter("DUMMY{newline}DUMMY"));
            
	        // need to go through the source to get a TraceEventCache
	        LogSource source = new LogSource("notfromconfig", SourceLevels.All);
	        source.Listeners.Add(listener);
	        for (int writeLoop = 0; writeLoop < numberOfWrites; writeLoop++)
	            source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
            source.Dispose();

	        StreamReader reader = new StreamReader("tracewithheaderandfooter.log");
            int headersFound = NumberOfItems("tracewithheaderandfooter.log", header);

	        Assert.AreEqual(numberOfWrites,headersFound);
	    }

        private string GetFileContents(string fileName)
        {
            string strFileContents = String.Empty;
            using (System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    strFileContents = reader.ReadToEnd();
                    reader.Close();
                }
            }
            return strFileContents;
        }

	    private int NumberOfItems(string fileName, string item)
        {
            int itemsFound = 0;
            using (System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    string strFileContents;
                    while ((strFileContents = reader.ReadLine()) != null)
                    {
                        if (strFileContents.Equals(item))
                            itemsFound++;
                    }
                    reader.Close();
                }
            }
            return itemsFound;
        }
	
	}
}
