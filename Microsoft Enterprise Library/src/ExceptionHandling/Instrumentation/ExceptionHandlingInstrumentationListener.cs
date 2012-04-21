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
using System.Diagnostics;
using System.Text;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// Provides the concrete instrumentation for the logical events raised by a <see cref="ExceptionHandlingInstrumentationProvider"/> object.
    /// </summary>
	[HasInstallableResourcesAttribute]
	[PerformanceCountersDefinition(counterCategoryName, "ExceptionHandlingHelpResourceName")]
	[EventLogDefinition("Application", "Enterprise Library ExceptionHandling")]
	internal class ExceptionHandlingInstrumentationListener : InstrumentationListener
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();

		[PerformanceCounter("Exceptions Handled/sec", "ExceptionHandledHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter exceptionHandledCounter;

		[PerformanceCounter("Exception Handlers Executed/sec", "ExceptionHandlerExecutedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter exceptionHandlerExecutedCounter;

		private const string counterCategoryName = "Enterprise Library Exception Handling Counters";

		private string instanceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingInstrumentationListener"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="ExceptionPolicy"/> this instrumentation listener is bound to.</param>
        /// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        public ExceptionHandlingInstrumentationListener(string instanceName,
                                           bool performanceCountersEnabled,
                                           bool eventLoggingEnabled,
                                           bool wmiEnabled)
        : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter())
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingInstrumentationListener"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="ExceptionPolicy"/> this instrumentation listener is bound to.</param>
        /// <param name="performanceCountersEnabled"><code>true</code> if performance counters should be updated.</param>
        /// <param name="eventLoggingEnabled"><code>true</code> if event log entries should be written.</param>
        /// <param name="wmiEnabled"><code>true</code> if WMI events should be fired.</param>
        /// <param name="nameFormatter">The <see cref="IPerformanceCounterNameFormatter"/> that is used to creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
		public ExceptionHandlingInstrumentationListener(string instanceName,
                                           bool performanceCountersEnabled,
                                           bool eventLoggingEnabled,
                                           bool wmiEnabled,
                                           IPerformanceCounterNameFormatter nameFormatter)
		: base(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter)
        {
			this.instanceName = instanceName;
        }

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="ExceptionHandlingInstrumentationProvider.exceptionHandled"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
        [InstrumentationConsumer("ExceptionHandled")]
		public void ExceptionHandled(object sender, EventArgs e)
		{
			if (PerformanceCountersEnabled) exceptionHandledCounter.Increment();
		}

        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="ExceptionHandlingInstrumentationProvider.exceptionHandlerExecuted"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("ExceptionHandlerExecuted")]
		public void ExceptionHandlerExecuted(object sender, EventArgs e)
		{
			if (PerformanceCountersEnabled) exceptionHandlerExecutedCounter.Increment();
		}
		
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Handler for the <see cref="ExceptionHandlingInstrumentationProvider.exceptionHandlingErrorOccurred"/> event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Data for the event.</param>
		[InstrumentationConsumer("ExceptionHandlingErrorOccurred")]
		public void ExceptionHandlingErrorOccurred(object sender, ExceptionHandlingErrorEventArgs e)
		{
			if (EventLoggingEnabled)
			{
				string errorMessage
					= string.Format(
						Resources.Culture,
						Resources.ErrorHandlingExceptionMessage,
						instanceName);
				string entryText = new EventLogEntryFormatter(Resources.BlockName).GetEntryText(errorMessage, e.Message);

				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
            if (WmiEnabled) ManagementInstrumentation.Fire(new ExceptionHandlingFailureEvent(instanceName, e.Message));
		}

        /// <summary>
        /// Creates the performance counters to instrument the logging events to the instance names.
        /// </summary>
        /// <param name="instanceNames">The instance names for the performance counters.</param>
        protected override void CreatePerformanceCounters(string[] instanceNames)
		{
            exceptionHandledCounter = factory.CreateCounter(counterCategoryName, "Exceptions Handled/sec", instanceNames);
            exceptionHandlerExecutedCounter = factory.CreateCounter(counterCategoryName, "Exception Handlers Executed/sec", instanceNames);
		}
	}
}
