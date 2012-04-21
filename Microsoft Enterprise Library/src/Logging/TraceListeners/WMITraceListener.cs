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
using System.Diagnostics;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// WmiTraceListener is a <see cref="TraceListener"/> that send a WMI event
	/// </summary>
	public class WmiTraceListener : TraceListener, IInstrumentationEventProvider
	{
		private LoggingInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Initializes a new instance of <see cref="WmiTraceListener"/> 
		/// </summary>
		public WmiTraceListener()
			: base()
		{
			instrumentationProvider = new LoggingInstrumentationProvider();
		}

		/// <summary>
		/// Sends an event given a predefined string
		/// </summary>
		/// <param name="message">The string to write as the event</param>
		public override void Write(string message)
		{
			LogEntry logEntry = new LogEntry();
			logEntry.Message = message;
			ManagementInstrumentation.Fire(logEntry);
		}

		/// <summary>
		/// Sends an email message given a predefined string
		/// </summary>
		/// <param name="message">The string to write as the email message</param>
		public override void WriteLine(string message)
		{
			Write(message);
		}

		/// <summary>
		/// Delivers the trace data as an event.
		/// </summary>
		/// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
		/// <param name="source">The name of the trace source that delivered the trace data.</param>
		/// <param name="eventType">The type of event.</param>
		/// <param name="id">The id of the event.</param>
		/// <param name="data">The data to trace.</param>
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			if (data is LogEntry)
			{
				ManagementInstrumentation.Fire(data as LogEntry);
				instrumentationProvider.FireTraceListenerEntryWrittenEvent();
			}
			else if (data is string)
			{
				Write(data);
			}
			else
			{
				base.TraceData(eventCache, source, eventType, id, data);
			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns the object that provides instrumentation services for the trace listener.
		/// </summary>
		/// <see cref="IInstrumentationEventProvider.GetInstrumentationEventProvider()"/>
		/// <returns>The object that providers intrumentation services.</returns>
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}
	}
}