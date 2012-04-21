//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Provides tracing services through a set of <see cref="TraceListener"/>s.
	/// </summary>
	public class LogSource : IInstrumentationEventProvider, IDisposable
	{
		private LoggingInstrumentationProvider instrumentationProvider;
		private SourceLevels level;
		private string name;
		private List<TraceListener> traceListeners;
		private TraceEventCache manager;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogSource"/> class with a name.
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public LogSource(string name)
			: this(name, new List<TraceListener>(new TraceListener[] { new DefaultTraceListener() }), SourceLevels.All)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogSource"/> class with a name and a level.
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		/// <param name="level">The <see cref="SourceLevels"/> value.</param>
		public LogSource(string name, SourceLevels level)
			: this(name, new List<TraceListener>(new TraceListener[] { new DefaultTraceListener() }), level)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogSource"/> class with a name, a collection of <see cref="TraceListener"/>s and a level.
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		/// <param name="traceListeners">The collection of <see cref="TraceListener"/>s.</param>
		/// <param name="level">The <see cref="SourceLevels"/> value.</param>
		public LogSource(string name, List<TraceListener> traceListeners, SourceLevels level)
		{
			this.name = name;
			this.traceListeners = traceListeners;
			this.level = level;
			this.manager = new TraceEventCache();
			this.instrumentationProvider = new LoggingInstrumentationProvider();

		}

		/// <summary>
		/// Gets the name for the <see cref="LogSource"/> instance.
		/// </summary>
		public string Name
		{
			get { return name; }
		}

		/// <summary>
		/// Gets the collection of trace listeners for the <see cref="LogSource"/> instance.
		/// </summary>
		public List<TraceListener> Listeners
		{
			get { return traceListeners; }
		}

		/// <summary>
		/// Gets the <see cref="SourceLevels"/> values at which to trace for the <see cref="LogSource"/> instance.
		/// </summary>
		public SourceLevels Level
		{
			get { return level; }
		}

		/// <summary>
		/// Writes trace data to the trace listeners in the <see cref="LogSource.Listeners"/> collection using the specified 
		/// event type, event identifier, and trace data. 
		/// </summary>
		/// <param name="eventType">The value that specifies the type of event that caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		/// <param name="logEntry">The <see cref="LogEntry"/> to trace.</param>
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void TraceData(TraceEventType eventType, int id, LogEntry logEntry)
		{
			TraceData(eventType, id, logEntry, new TraceListenerFilter());
		}

		/// <summary>
		/// Writes trace data to the trace listeners in the <see cref="LogSource.Listeners"/> collection that have not already been
		/// written to for tracing using the specified event type, event identifier, and trace data.
		/// </summary>
		/// <remarks>
		/// The <paramref name="traceListenerFilter"/> will be updated to reflect the trace listeners that were written to by the 
		/// <see cref="LogSource"/>.
		/// </remarks>
		/// <param name="eventType">The value that specifies the type of event that caused the trace.</param>
		/// <param name="id">A numeric identifier for the event.</param>
		/// <param name="logEntry">The <see cref="LogEntry"/> to trace.</param>
		/// <param name="traceListenerFilter">The filter for already written to trace listeners.</param>
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void TraceData(TraceEventType eventType, int id, LogEntry logEntry, TraceListenerFilter traceListenerFilter)
		{
			if (!ShouldTrace(eventType)) return;

			foreach (TraceListener listener in traceListenerFilter.GetAvailableTraceListeners(traceListeners))
			{
				try
				{
					if (!listener.IsThreadSafe) Monitor.Enter(listener);

					listener.TraceData(manager, Name, eventType, id, logEntry);
					instrumentationProvider.FireTraceListenerEntryWrittenEvent();

					listener.Flush();
				}
				finally
				{
					if (!listener.IsThreadSafe) Monitor.Exit(listener);
				}
			}
		}

		/// <summary>
		/// Returns the object that provides instrumentation services for the <see cref="LogSource"/>.
		/// </summary>
		/// <see cref="IInstrumentationEventProvider.GetInstrumentationEventProvider()"/>
		/// <returns>The object that providers intrumentation services. This object may be null if instrumentation services are not created for this instance.</returns>
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}

		/// <summary>
		/// Releases the resources used by the <see cref="LogSource"/>.
		/// </summary>
		public void Dispose()
		{
			foreach (TraceListener listener in traceListeners)
			{
				listener.Dispose();
			}
		}

		private bool ShouldTrace(TraceEventType eventType)
		{
			return ((((TraceEventType)level) & eventType) != (TraceEventType)0);
		}
	}
}
