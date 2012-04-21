//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using System.Text;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	[TestClass]
    public class ExceptionUtilityFixture
    {
		private const string EventLogName = "Application";
		private static readonly string EventLogSource = "Enterprise Library Exception Handling";
		private const string idMessage = "ID : {handlingInstanceID}";
		private const string exceptionMessage = "Unable to handle exception";
		private const string policy = "policy";

		[TestMethod]
		public void FormatTokenInMessage()
		{			
			Guid id = Guid.NewGuid();
			string formattedMessage = ExceptionUtility.FormatExceptionMessage(idMessage, id);

			Assert.AreEqual("ID : " + id.ToString(), formattedMessage);
		}

		[TestMethod]
        public void LogHandlingError()
        {
			if (!EventLog.SourceExists(EventLogSource)) return;

            Exception ex = new Exception(exceptionMessage);
            ExceptionUtility.FormatExceptionHandlingExceptionMessage(policy, null, null, ex);

            StringBuilder message = new StringBuilder();
            StringWriter writer = null;
            try
            {
                writer = new StringWriter(message);
                TextExceptionFormatter formatter = new TextExceptionFormatter(writer, ex);
                formatter.Format();
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            using (EventLog log = new EventLog(EventLogName))
            {
                EventLogEntry entry = log.Entries[log.Entries.Count - 1];

                Assert.AreEqual(EventLogEntryType.Error, entry.EntryType);
                Assert.AreEqual(EventLogSource, entry.Source);
            }
        }
    }
}

