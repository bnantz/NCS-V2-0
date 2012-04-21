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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// Base class for <see cref="TraceListeners"/> that wrap other trace listeners and 
	/// use a <see cref="ILogFormatter"/> to format the trace information.
	/// </summary>
	public abstract class FormattedTraceListenerWrapperBase : FormattedTraceListenerBase
	{
		private TraceListener slaveListener;

		/// <summary>
		/// Initializes a <see cref="FormattedTraceListenerWrapperBase"/>.
		/// </summary>
		public FormattedTraceListenerWrapperBase()
		{
		}

		/// <summary>
		/// Initializes a <see cref="FormattedTraceListenerWrapperBase"/> with a slave <see cref="TraceListener"/>.
		/// </summary>
		/// <param name="slaveListener">The wrapped listener.</param>
		public FormattedTraceListenerWrapperBase(TraceListener slaveListener)
		{
			this.slaveListener = slaveListener;
		}

		/// <summary>
		/// Initializes a <see cref="FormattedTraceListenerWrapperBase"/> with a slave <see cref="TraceListener"/> 
		/// and a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="slaveListener">The wrapped listener.</param>
		/// <param name="formater">The formatter.</param>
		public FormattedTraceListenerWrapperBase(TraceListener slaveListener, ILogFormatter formater) 
			: base(formater)
		{
			this.slaveListener = slaveListener;
		}

		/// <summary>
		/// Forwards the trace request to the wrapped listener.
		/// </summary>
		/// <param name="eventCache">The context information.</param>
		/// <param name="source">The trace source.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="id">The event id.</param>
		/// <param name="data">The objects to trace.</param>
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data)
		{
			this.slaveListener.TraceData(eventCache, source, severity, id, data);
		}

		/// <summary>
		/// Formats the object to trace and forward the trace request to the wrapped listener with the formatted result.
		/// </summary>
		/// <remarks>
		/// Formatting is only performed if the object to trace is a <see cref="LogEntry"/> and the formatter is set.
		/// </remarks>
		/// <param name="eventCache">The context information.</param>
		/// <param name="source">The trace source.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="id">The event id.</param>
		/// <param name="data">The object to trace.</param>
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data)
		{
			if (data is LogEntry)
			{
				if (this.Formatter != null)
				{
					this.slaveListener.TraceData(eventCache, source, severity, id, this.Formatter.Format(data as LogEntry));
				}
				else
				{
					this.slaveListener.TraceData(eventCache, source, severity, id, data);
				}

				InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
			}
			else
			{
				this.slaveListener.TraceData(eventCache, source, severity, id, data);
			}
		}

		/// <summary>
		/// Forwards the trace request to the wrapped listener.
		/// </summary>
		/// <param name="eventCache">The context information.</param>
		/// <param name="source">The trace source.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="id">The event id.</param>
		/// <param name="message">The message to trace.</param>
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
		{
			this.slaveListener.TraceEvent(eventCache, source, severity, id, message);
		}

		/// <summary>
		/// Forwards the trace request to the wrapped listener.
		/// </summary>
		/// <param name="eventCache">The context information.</param>
		/// <param name="source">The trace source.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="id">The event id.</param>
		/// <param name="format">The format to use.</param>
		/// <param name="args">The objects to trace.</param>
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
		{
			this.slaveListener.TraceEvent(eventCache, source, severity, id, format, args);
		}

		/// <summary>
		/// Forwards the trace request to the wrapped listener.
		/// </summary>
		/// <param name="message">The message to trace.</param>
		public override void Write(string message)
		{
			this.slaveListener.Write(message);
		}

		/// <summary>
		/// Forwards the tracing to the wrapped listener.
		/// </summary>
		/// <param name="message">The message to trace.</param>
		public override void WriteLine(string message)
		{
			this.slaveListener.WriteLine(message);
		}

		internal TraceListener SlaveListener
		{
			get { return this.slaveListener; }	
		}

		/// <summary>
		/// Deal with resources.
		/// </summary>
		/// <param name="disposing">true if called from a Dispose message.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.slaveListener.Dispose();
			}
		}
	}
}
