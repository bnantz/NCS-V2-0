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
	/// <summary>
	/// Summary description for FormattingListenerFixture
	/// </summary>
	[TestClass]
	public class FormattedTextWriterTraceListenerFixture
	{
		[TestMethod]
		public void FormattedListenerWillUseFormatterIfExists()
		{

			StringWriter writer = new StringWriter();
			FormattedTextWriterTraceListener listener = new FormattedTextWriterTraceListener(writer, new TextFormatter("DUMMY{newline}DUMMY"));

			// need to go through the source to get a TraceEventCache
			LogSource source = new LogSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));

			Assert.AreEqual("DUMMY" + Environment.NewLine + "DUMMY", writer.ToString());
		}

		[TestMethod]
		public void FormattedListenerWillFallbackToTraceEntryToStringIfFormatterDoesNotExists()
		{
			LogEntry testEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
			StringWriter writer = new StringWriter();
			FormattedTextWriterTraceListener listener = new FormattedTextWriterTraceListener(writer);

			// need to go through the source to get a TraceEventCache
			LogSource source = new LogSource("notfromconfig", SourceLevels.All);
			source.Listeners.Add(listener);
			source.TraceData(TraceEventType.Error, 0, testEntry);

			string writtenData = writer.ToString();
			string testEntryToString = testEntry.ToString();

			Assert.IsTrue(-1 != writtenData.IndexOf(testEntryToString));
		}
	}
}
