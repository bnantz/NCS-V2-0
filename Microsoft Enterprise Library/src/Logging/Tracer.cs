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
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using System.Collections.Generic;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Represents a performance tracing class to log method entry/exit and duration.
	/// </summary>
	/// <remarks>
	/// <para>Lifetime of the Tracer object will determine the beginning and the end of
	/// the trace.  The trace message will include, method being traced, start time, end time 
	/// and duration.</para>
	/// <para>Since Tracer uses the logging block to log the trace message, you can include application
	/// data as part of your trace message. Configured items from call context will be logged as
	/// part of the message.</para>
	/// <para>Trace message will be logged to the log category with the same name as the tracer operation name.
	/// You must configure the operation categories, or the catch-all categories, with desired log sinks to log 
	/// the trace messages.</para>
	/// </remarks>
	public class Tracer : IDisposable
	{
		/// <summary>
		/// Priority value for Trace messages
		/// </summary>
		public const int priority = 5;

		/// <summary>
		/// Event id for Trace messages
		/// </summary>
		public const int eventId = 1;

		/// <summary>
		/// Title for operation start Trace messages
		/// </summary>
		public const string startTitle = "TracerEnter";

		/// <summary>
		/// Title for operation end Trace messages
		/// </summary>
		public const string endTitle = "TracerExit";

		/// <summary>
		/// Name of the entry in the ExtendedProperties having the activity id
		/// </summary>
		public const string ActivityIdPropertyKey = "TracerActivityId";

        private TracerInstrumentationListener instrumentationListener;

		private Stopwatch stopwatch;
		private long tracingStartTicks;
		private bool tracerDisposed = false;

        private LogWriter writer;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
		/// </summary>
		/// <remarks>
		/// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
		/// </remarks>
		/// <param name="operation">The operation for the <see cref="Tracer"/></param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public Tracer(string operation)
        {
            if (Trace.CorrelationManager.ActivityId.Equals(Guid.Empty))
            {
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            Initialize(operation, ConfigurationSourceFactory.Create());
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
		/// </summary>
		/// <remarks>
		/// The activity id will override a previous activity id
		/// </remarks>
		/// <param name="operation">The operation for the <see cref="Tracer"/></param>
		/// <param name="activityId">The activity id</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Tracer(string operation, Guid activityId)
        {
            Trace.CorrelationManager.ActivityId = activityId;
            
            Initialize(operation, ConfigurationSourceFactory.Create());
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationConfiguration">configuration source that is used to determine instrumentation should be enabled</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public Tracer(string operation, LogWriter writer, IConfigurationSource instrumentationConfiguration)
        {
            if (writer == null) throw new ArgumentNullException("writer", Resources.ExceptionWriterShouldNotBeNull);

            this.writer = writer;

            if (Trace.CorrelationManager.ActivityId.Equals(Guid.Empty))
            {
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            Initialize(operation, instrumentationConfiguration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        /// <param name="instrumentationConfiguration">configuration source that is used to determine instrumentation should be enabled</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public Tracer(string operation, Guid activityId, LogWriter writer, IConfigurationSource instrumentationConfiguration)
        {
            if (writer == null) throw new ArgumentNullException("writer", Resources.ExceptionWriterShouldNotBeNull); 
            
            Trace.CorrelationManager.ActivityId = activityId;

            this.writer = writer;

            Initialize(operation, instrumentationConfiguration);
        }

		/// <summary>
		/// <para>Releases unmanaged resources and performs other cleanup operations before the <see cref="Tracer"/> is 
		/// reclaimed by garbage collection</para>
		/// </summary>
		~Tracer()
		{
			Dispose(false);
		}

		/// <summary>
		/// Causes the <see cref="Tracer"/> to output its closing message.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// <para>Releases the unmanaged resources used by the <see cref="Tracer"/> and optionally releases 
		/// the managed resources.</para>
		/// </summary>
		/// <param name="disposing">
		/// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> 
		/// to release only unmanaged resources.</para>
		/// </param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !tracerDisposed)
			{
                if (IsTracingEnabled()) WriteTraceEndMessage(endTitle);
				Trace.CorrelationManager.StopLogicalOperation();
				this.tracerDisposed = true;
			}
		}

		/// <summary>
		/// Answers whether tracing is enabled
		/// </summary>
		/// <returns>true if tracing is enabled</returns>
        public bool IsTracingEnabled()
        {
            LogWriter writer = GetWriter();
            return writer.IsTracingEnabled();
        }

        private void Initialize(string operation, IConfigurationSource configurationSource)
		{
            if (configurationSource != null)
            {
                instrumentationListener = EnterpriseLibraryFactory.BuildUp<TracerInstrumentationListener>(configurationSource);
            }
            else
            {
                instrumentationListener = new TracerInstrumentationListener(false);
            }

            Trace.CorrelationManager.StartLogicalOperation(operation);
            if (IsTracingEnabled())
            {
                instrumentationListener.TracerOperationStarted(Trace.CorrelationManager.LogicalOperationStack.Peek() as string);
                
                stopwatch = Stopwatch.StartNew();
                tracingStartTicks = Stopwatch.GetTimestamp();

                WriteTraceStartMessage(startTitle);
            }
		}

		private void WriteTraceStartMessage(string entryTitle)
		{
			string methodName = GetExecutingMethodName();
			string message = string.Format(Resources.Culture, Resources.Tracer_StartMessageFormat, Trace.CorrelationManager.ActivityId, methodName, tracingStartTicks);
            
			WriteTraceMessage(message, entryTitle, TraceEventType.Start);
		}

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private void WriteTraceEndMessage(string entryTitle)
		{
			long tracingEndTicks = Stopwatch.GetTimestamp();
			decimal secondsElapsed = GetSecondsElapsed(stopwatch.ElapsedMilliseconds);

			string methodName = GetExecutingMethodName();
			string message = string.Format(Resources.Culture, Resources.Tracer_EndMessageFormat, Trace.CorrelationManager.ActivityId, methodName, tracingEndTicks, secondsElapsed);
			WriteTraceMessage(message, entryTitle, TraceEventType.Stop);

            instrumentationListener.TracerOperationEnded(Trace.CorrelationManager.LogicalOperationStack.Peek() as string, stopwatch.ElapsedMilliseconds);
		}

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private void WriteTraceMessage(string message, string entryTitle, TraceEventType eventType)
		{
			Dictionary<string, object> extendedProperties = new Dictionary<string, object>();
			LogEntry entry = new LogEntry(message, Trace.CorrelationManager.LogicalOperationStack.Peek() as string, priority, eventId, eventType, entryTitle, extendedProperties);

            LogWriter writer = GetWriter();
            writer.Write(entry);
		}

		private string GetExecutingMethodName()
		{
			string result = "Unknown";
			StackTrace trace = new StackTrace(false);

			for (int index = 0; index < trace.FrameCount; ++index)
			{
				StackFrame frame = trace.GetFrame(index);
				MethodBase method = frame.GetMethod();
				if (method.DeclaringType != GetType())
				{
					result = method.Name;
					break;
				}
			}

			return result;
		}

		private decimal GetSecondsElapsed(long milliseconds)
		{
			decimal result = Convert.ToDecimal(milliseconds) / 1000m;
			return Math.Round(result, 6);
		}

        private LogWriter GetWriter()
        {
            return (writer != null) ? writer : Logger.Writer;
        }
	}
}
