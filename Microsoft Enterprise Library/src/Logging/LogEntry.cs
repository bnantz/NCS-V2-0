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
using System.Globalization;
using System.Management.Instrumentation;
using System.Text;
using System.Security.Permissions;
using System.Threading;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Represents a log message.  Contains the common properties that are required for all log messages.
	/// </summary>
	[XmlRoot("logEntry")]
	[Serializable]
	[InstrumentationClass(InstrumentationType.Event)]
	[ManagedName("LogEntryV20")]
	public class LogEntry : ICloneable
	{
		private static readonly TextFormatter toStringFormatter = new TextFormatter();

		private string message = string.Empty;
		private string title = string.Empty;
		private ICollection<string> categories = new List<string>(0);
		private int priority = -1;
		private int eventId = 0;
		private Guid activityId;

		private TraceEventType severity = TraceEventType.Information;

		private string machineName = string.Empty;
		private DateTime timeStamp = DateTime.MaxValue;

		private StringBuilder errorMessages;
		private IDictionary<string, object> extendedProperties;

		private string appDomainName;
		private string processId;
		private string processName;
		private string threadName;
		private string win32ThreadId;

		/// <summary>
		/// Initialize a new instance of a <see cref="LogEntry"/> class.
		/// </summary>
		public LogEntry()
		{
			CollectIntrinsicProperties();
		}

		/// <summary>
		/// Create a new instance of <see cref="LogEntry"/> with a full set of constructor parameters
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more trace listeners.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		/// <param name="title">Additional description of the log entry message.</param>
		/// <param name="properties">Dictionary of key/value pairs to record.</param>
		public LogEntry(object message, string category, int priority, int eventId,
						TraceEventType severity, string title, IDictionary<string, object> properties)
			: this(message, BuildCategoriesCollection(category), priority, eventId, severity, title, properties)
		{
		}

		/// <summary>
		/// Create a new instance of <see cref="LogEntry"/> with a full set of constructor parameters
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="categories">Collection of category names used to route the log entry to a one or more sinks.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		/// <param name="title">Additional description of the log entry message.</param>
		/// <param name="properties">Dictionary of key/value pairs to record.</param>
		public LogEntry(object message, ICollection<string> categories, int priority, int eventId,
						TraceEventType severity, string title, IDictionary<string, object> properties)
		{
			if (message == null)
				throw new ArgumentNullException("message");
			if (categories == null)
				throw new ArgumentNullException("categories");

			this.Message = message.ToString();
			this.Priority = priority;
			this.Categories = categories;
			this.EventId = eventId;
			this.Severity = severity;
			this.Title = title;
			this.ExtendedProperties = properties;

			CollectIntrinsicProperties();
		}

		/// <summary>
		/// Message body to log.  Value from ToString() method from message object.
		/// </summary>
		public string Message
		{
			get { return this.message; }
			set { this.message = value; }
		}

		/// <summary>
		/// Category name used to route the log entry to a one or more trace listeners.
		/// </summary>
		[IgnoreMember]
		public ICollection<string> Categories
		{
			get { return categories; }
			set { this.categories = value; }
		}

		/// <summary>
		/// Importance of the log message.  Only messages whose priority is between the minimum and maximum priorities (inclusive)
		/// will be processed.
		/// </summary>
		public int Priority
		{
			get { return this.priority; }
			set { this.priority = value; }
		}

		/// <summary>
		/// Event number or identifier.
		/// </summary>
		public int EventId
		{
			get { return this.eventId; }
			set { this.eventId = value; }
		}

		/// <summary>
		/// Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).
		/// </summary>
		[IgnoreMember]
		public TraceEventType Severity
		{
			get { return this.severity; }
			set { this.severity = value; }
		}

		/// <summary>
		/// <para>Gets the string representation of the <see cref="Severity"/> enumeration.</para>
		/// </summary>
		/// <value>
		/// <para>The string value of the <see cref="Severity"/> enumeration.</para>
		/// </value>
		public string LoggedSeverity
		{
			get { return severity.ToString(); }
		}

		/// <summary>
		/// Additional description of the log entry message.
		/// </summary>
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		/// <summary>
		/// Date and time of the log entry message.
		/// </summary>
		public DateTime TimeStamp
		{
			get { return this.timeStamp; }
			set { this.timeStamp = value; }
		}

		/// <summary>
		/// Name of the computer.
		/// </summary>
		public string MachineName
		{
			get { return this.machineName; }
			set { this.machineName = value; }
		}

		/// <summary>
		/// The <see cref="AppDomain"/> in which the program is running
		/// </summary>
		public string AppDomainName
		{
			get { return this.appDomainName; }
			set { this.appDomainName = value; }
		}

		/// <summary>
		/// The Win32 process ID for the current running process.
		/// </summary>
		public string ProcessId
		{
			get { return processId; }
			set { processId = value; }
		}

		/// <summary>
		/// The name of the current running process.
		/// </summary>
		public string ProcessName
		{
			get { return processName; }
			set { processName = value; }
		}

		/// <summary>
		/// The name of the .NET thread.
		/// </summary>
		///  <seealso cref="Win32ThreadId"/>
		public string ManagedThreadName
		{
			get { return threadName; }
			set { threadName = value; }
		}

		/// <summary>
		/// The Win32 Thread ID for the current thread.
		/// </summary>
		public string Win32ThreadId
		{
			get { return win32ThreadId; }
			set { win32ThreadId = value; }
		}

		/// <summary>
		/// Dictionary of key/value pairs to record.
		/// </summary>
		[IgnoreMember]
		public IDictionary<string, object> ExtendedProperties
		{
			get
			{
				if (extendedProperties == null)
				{
					extendedProperties = new Dictionary<string, object>();
				}
				return this.extendedProperties;
			}
			set { this.extendedProperties = value; }
		}

		/// <summary>
		/// Read-only property that returns the timeStamp formatted using the current culture.
		/// </summary>
		public string TimeStampString
		{
			get { return TimeStamp.ToString(CultureInfo.CurrentCulture); }
		}

		/// <summary>
		/// Tracing activity id
		/// </summary>
		[IgnoreMember]
		public Guid ActivityId
		{
			get { return this.activityId; }
			set { this.activityId = value; }
		}

		/// <summary>
		/// Creates a new <see cref="LogEntry"/> that is a copy of the current instance.
		/// </summary>
		/// <remarks>
		/// If the dictionary contained in <see cref="ExtendedProperties"/> implements <see cref="ICloneable"/>, the resulting
		/// <see cref="LogEntry"/> will have its ExtendedProperties set by calling <c>Clone()</c>. Otherwise the resulting
		/// <see cref="LogEntry"/> will have its ExtendedProperties set to <see langword="null"/>.
		/// </remarks>
		/// <implements>ICloneable.Clone</implements>
		/// <returns>A new <c>LogEntry</c> that is a copy of the current instance.</returns>
		public object Clone()
		{
			LogEntry result = new LogEntry();

			result.Message = this.Message;
			result.EventId = this.EventId;
			result.Title = this.Title;
			result.Severity = this.Severity;
			result.Priority = this.Priority;

			result.TimeStamp = this.TimeStamp;
			result.MachineName = this.MachineName;
			result.AppDomainName = this.AppDomainName;
			result.ProcessId = this.ProcessId;
			result.ProcessName = this.ProcessName;
			result.ManagedThreadName = this.ManagedThreadName;
			result.ActivityId = this.ActivityId;

			// clone categories
			result.Categories = new List<string>(this.Categories);

			// clone extended properties
			if (this.extendedProperties != null)
				result.ExtendedProperties = new Dictionary<string, object>(this.extendedProperties);

			// clone error messages
			if (this.errorMessages != null)
			{
				result.errorMessages = new StringBuilder(this.errorMessages.ToString());
			}

			return result;
		}

		/// <summary>
		/// Add an error or warning message to the start of the messages string builder.
		/// </summary>
		/// <param name="message">Message to be added to this instance</param>
		public virtual void AddErrorMessage(string message)
		{
			if (errorMessages == null)
			{
				errorMessages = new StringBuilder();
			}
			errorMessages.Insert(0, Environment.NewLine);
			errorMessages.Insert(0, Environment.NewLine);
			errorMessages.Insert(0, message);
		}

		/// <summary>
		/// Gets the error message with the <see cref="LogEntry"></see>
		/// </summary>
		public string ErrorMessages
		{
			get
			{
				if (errorMessages == null)
					return null;
				else
					return errorMessages.ToString();
			}
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents the current <see cref="LogEntry"/>, 
		/// using a default formatting template.
		/// </summary>
		/// <returns>A <see cref="String"/> that represents the current <see cref="LogEntry"/>.</returns>
		public override string ToString()
		{
			return toStringFormatter.Format(this);
		}

		/// <summary>
		/// Set the intrinsic properties such as MachineName and UserIdentity.
		/// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private void CollectIntrinsicProperties()
		{
			this.TimeStamp = DateTime.UtcNow;
			this.ActivityId = Trace.CorrelationManager.ActivityId;

			try
			{
				MachineName = Environment.MachineName;
			}
			catch (Exception e)
			{
				this.MachineName = String.Format(Properties.Resources.Culture, Properties.Resources.IntrinsicPropertyError, e.Message);
			}

			try
			{                                 
				appDomainName = AppDomain.CurrentDomain.FriendlyName;
			}
			catch (Exception e)
			{
				appDomainName = String.Format(Properties.Resources.Culture, Properties.Resources.IntrinsicPropertyError, e.Message);
			}

			try
			{
				processId = NativeMethods.GetCurrentProcessId().ToString(NumberFormatInfo.InvariantInfo);
			}
			catch (Exception e)
			{
				processId = String.Format(Properties.Resources.Culture, Properties.Resources.IntrinsicPropertyError, e.Message);
			}

			try
			{
				processName = GetProcessName();
			}
			catch (Exception e)
			{
				processName = String.Format(Properties.Resources.Culture, Properties.Resources.IntrinsicPropertyError, e.Message);
			}

			try
			{
				threadName = Thread.CurrentThread.Name;
			}
			catch (Exception e)
			{
				threadName = String.Format(Properties.Resources.Culture, Properties.Resources.IntrinsicPropertyError, e.Message);
			}

			try
			{
				win32ThreadId = NativeMethods.GetCurrentThreadId().ToString(NumberFormatInfo.InvariantInfo);
			}
			catch (Exception e)
			{
				win32ThreadId = String.Format(Properties.Resources.Culture, Properties.Resources.IntrinsicPropertyError, e.Message);
			}
		}

		/// <summary>
		/// Gets the current process name.
		/// </summary>
		/// <returns>The process name.</returns>
		public static string GetProcessName()
		{
			StringBuilder buffer = new StringBuilder(1024);
			int length = NativeMethods.GetModuleFileName(NativeMethods.GetModuleHandle(null), buffer, buffer.Capacity);
			return buffer.ToString();
		}

		private static ICollection<string> BuildCategoriesCollection(string category)
		{
			if (string.IsNullOrEmpty(category))
				throw new ArgumentNullException("category");

			return new string[] { category };
		}

		/// <summary>
		/// Tracing activity id as a string to support WMI Queries
		/// </summary>
		public string ActivityIdString
		{
			get { return this.ActivityId.ToString(); }
		}

		/// <summary>
		/// Category names used to route the log entry to a one or more trace listeners.
		/// This readonly property is available to support WMI queries
		/// </summary>
		public string[] CategoriesStrings
		{
			get
			{
				string[] categoriesStrings = new string[Categories.Count];
				this.Categories.CopyTo(categoriesStrings, 0);
				return categoriesStrings;
			}
		}
	}
}
