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
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
	public class MockTraceListener : TraceListener
	{
		public object tracedData = null;
		public string tracedSource = null;
		public TraceEventType tracedEventType = TraceEventType.Information;

		private static object traceRequestMonitor = new object();
		private static int processedTraceRequests = 0;
		private static List<LogEntry> entries = new List<LogEntry>();
		private static List<MockTraceListener> instances = new List<MockTraceListener>();

		public MockTraceListener()
			: this("")
		{
		}

		public MockTraceListener(string name)
		{
			this.Name = name;
		}

		public static void Reset()
		{
			lock (traceRequestMonitor)
			{
				entries.Clear();
				instances.Clear();
				processedTraceRequests = 0;
			}
		}

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			lock (traceRequestMonitor)
			{
				tracedData = data;
				tracedSource = source;
				tracedEventType = eventType;
				MockTraceListener.Entries.Add(data as LogEntry);
				MockTraceListener.Instances.Add(this);
				processedTraceRequests++;
			}
		}

		public override void Write(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void WriteLine(string message)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public static List<LogEntry> Entries
		{
			get { return entries; }
		}

		public static List<MockTraceListener> Instances
		{
			get { return instances; }
		}

		public static int ProcessedTraceRequests
		{
			get { lock (traceRequestMonitor) { return processedTraceRequests; } }
		}

		public static LogEntry LastEntry
		{
			get { return entries.Count > 0 ? entries[entries.Count - 1] : null; }
		}

	}
}
