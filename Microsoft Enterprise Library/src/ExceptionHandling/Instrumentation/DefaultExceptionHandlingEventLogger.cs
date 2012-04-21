//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// Is the instrumentation gateway when no instances of the objects from the block are involved.
    /// </summary>
    [EventLogDefinition("Application", "Enterprise Library ExceptionHandling")]
    [CustomFactory(typeof(DefaultExceptionHandlingEventLoggerCustomFactory))]
    public class DefaultExceptionHandlingEventLogger : InstrumentationListener
    {
		private IEventLogEntryFormatter eventLogEntryFormatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExceptionHandlingEventLogger"/> class, specifying whether 
        /// logging to the event log and firing WMI events is allowed.
        /// </summary>
        /// <param name="eventLoggingEnabled"><code>true</code> if writing to the event log is allowed, <code>false</code> otherwise.</param>
        /// <param name="wmiEnabled"><code>true</code> if firing WMI events is allowed, <code>false</code> otherwise.</param>
        public DefaultExceptionHandlingEventLogger(bool eventLoggingEnabled, bool wmiEnabled)
			: base((string)null, false, eventLoggingEnabled, wmiEnabled, null)
        {
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}

        /// <summary>
        /// Logs the occurrence of a configuration error for the Enterprise Library Exception Handling Application Block through the 
        /// available instrumentation mechanisms.
        /// </summary>
        /// <param name="exception">The exception raised for the configuration error.</param>
        /// <param name="policyName">The name of the Exception policy in which the configuration error was detected.</param>
        public void LogConfigurationError(Exception exception, string policyName)
        {
            if (WmiEnabled) ManagementInstrumentation.Fire(new ExceptionHandlingConfigurationFailureEvent(policyName, exception.Message));
            if (EventLoggingEnabled)
            {
				string eventLogMessage
					= string.Format(
						Resources.Culture,
						Resources.ConfigurationFailureCreatingPolicy,
						policyName);
				string entryText = eventLogEntryFormatter.GetEntryText(eventLogMessage, exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
        }

        /// <summary>
        /// Logs the occurrence of an internal error for the Enterprise Library Exception Handling Application Block through the 
        /// available instrumentation mechanisms.
        /// </summary>
        /// <param name="policyName">The name of the Exception policy in which the error was occurred.</param>
        /// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
        public void LogInternalError(string policyName, string exceptionMessage)
        {
            if (WmiEnabled) ManagementInstrumentation.Fire(new ExceptionHandlingFailureEvent(policyName, exceptionMessage));
            if (EventLoggingEnabled)
            {
				string entryText = eventLogEntryFormatter.GetEntryText(exceptionMessage);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }
    }
}
