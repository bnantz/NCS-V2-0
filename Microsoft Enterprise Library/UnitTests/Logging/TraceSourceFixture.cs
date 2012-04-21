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

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

using System.Diagnostics;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
	[TestClass]
	public class TraceSourceFixture
	{
		[TestMethod]
		public void LogSourceCallsFlushRegardlessOfAutoflushValue()
		{
			MockFlushSensingTraceListener traceListener = new MockFlushSensingTraceListener();
			List<TraceListener> traceListeners = new List<TraceListener>(1);
			traceListeners.Add(traceListener);
			LogSource logSource = new LogSource("name", traceListeners, SourceLevels.All);

			bool currentAutoFlush = Trace.AutoFlush;

			try
			{
				Trace.AutoFlush = false;
				logSource.TraceData(TraceEventType.Critical, 0, CommonUtil.GetDefaultLogEntry());

				Trace.AutoFlush = true;
				logSource.TraceData(TraceEventType.Critical, 0, CommonUtil.GetDefaultLogEntry());

				Assert.AreEqual(2, traceListener.flushCalls);
			}
			finally
			{
				Trace.AutoFlush = currentAutoFlush;
			}
		}

		public class MockFlushSensingTraceListener : TraceListener
		{
			public int flushCalls = 0;

			public override void Flush()
			{
				flushCalls++;
			}

			public override void Write(string message)
			{ }

			public override void WriteLine(string message)
			{ }
		}

	}
}
