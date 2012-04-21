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
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Messaging;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    /// <summary>
    /// Constants and utility functions to support all programmer tests.
    /// </summary>
    public class CommonUtil
    {
        // Sink constants
        public const string MessageQueuePath = @".\Private$\entlib";
        public const string FileName = "FlatFileTestOutput\\EntLibLog.txt";
        public const string EventLogName = "Application";
        public const string EventLogNameCustom = "EntLib Tests";
        public const string EventLogSourceName = "Enterprise Library Unit Tests";

        // Log entry property constants
        public const string MsgBody = "My message body";
        public const string MsgCategory = "foo";
        public const string MsgTitle = "=== Header ===";
        public const int MsgEventID = 1;
        public static readonly DateTime MsgDateTime = DateTime.MaxValue;

        // Formatted message constants
        public static readonly string FormattedMessage =
            "Timestamp: 12/31/9999 11:59:59 PM\r\nMessage: My message body\r\nCategory: foo\r\nPriority: 100\r\nEventId: 1\r\nSeverity: Unspecified\r\nTitle:=== Header ===\r\nMachine: machine" +
                "\r\nApp Domain: " + AppDomain.CurrentDomain.FriendlyName + "\r\nProcessId: " + NativeMethods.GetCurrentProcessId().ToString() +
				"\r\nProcess Name: " + GetProcessName() + "\r\nThread Name: " + Thread.CurrentThread.Name + "\r\nWin32 ThreadId:" + NativeMethods.GetCurrentThreadId().ToString() + "\r\nExtended Properties: ";

        public static readonly string MessageWithDictionaryXml =
            "<EntLibMessage><Category></Category><Priority>-1</Priority><Header></Header>" +
                "<EventID>0</EventID><Body></Body><DateTime>12/31/9999 11:59:59 PM</DateTime><Severity>Unspecified</Severity><MachineName>machine</MachineName>" +
                "<ExtendedProperties>key1[===]value1[|||]key3[===]value3[|||]key2[===]value2</ExtendedProperties></EntLibMessage>";

        public static readonly string FormattedMessageWithDictionary =
            "Timestamp: 12/31/9999 11:59:59 PM\r\nTitle: === Header ===\r\n\r\nMessage: My message body\r\n\r\nExtended Properties:\r\nKey: key1\t\tValue: value1\r\nKey: key3\t\tValue: value3\r\nKey: key2\t\tValue: value2\r\n";

        public static string GetIntrinsicInfo()
        {
            string intrinsicInfo = "<AssemblyInformation>" + Assembly.GetExecutingAssembly().FullName + "</AssemblyInformation>\r\n  " +
                "<ApplicationInformation>" + AppDomain.CurrentDomain.FriendlyName + "</ApplicationInformation>\r\n  " +
                "<UserIdentity>" + WindowsIdentity.GetCurrent().Name + "</UserIdentity>\r\n  " +
                "<ThreadIdentity>" + Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture) + "</ThreadIdentity>\r\n";

            return intrinsicInfo;
        }

        public static string GetTimeZone()
        {
            int hours = TimeZone.CurrentTimeZone.GetUtcOffset(new DateTime(1999, 1, 1)).Hours;
            return hours.ToString("00") + ":00";
        }

        public static void CreatePrivateTestQ()
        {
            string path = CommonUtil.MessageQueuePath;
            if (MessageQueue.Exists(path))
            {
                DeletePrivateTestQ();
            }
            MessageQueue.Create(path, false);
        }

        public static int GetNumberOfMessagesOnQueue()
        {
            using (MessageQueue queue = new MessageQueue(CommonUtil.MessageQueuePath))
            {
                Message[] messages = queue.GetAllMessages();
                return messages.Length;
            }
        }

		public static string GetProcessName()
		{
			StringBuilder buffer = new StringBuilder(1024);
			int length = NativeMethods.GetModuleFileName(NativeMethods.GetModuleHandle(null), buffer, buffer.Capacity);
			Debug.Assert(length > 0);
			return buffer.ToString();
		}
		
		public static void CreateTransactionalPrivateTestQ()
        {
            string path = CommonUtil.MessageQueuePath;
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path, true);
            }
        }

        public static void DeletePrivateTestQ()
        {
            string path = CommonUtil.MessageQueuePath;
            if (MessageQueue.Exists(path))
            {
                MessageQueue.Delete(path);
            }
        }

		///// <summary>
		///// Build a formatted xml string
		///// </summary>
		//public static string GetMessage(string body, string header, int eventID,
		//                                string category, int categoryID, Severity severity)
		//{
		//    //  build a message
		//    string msg = string.Format("<Message>" +
		//        "<Header>{0}</Header>" +
		//        "<Body>{1}</Body>" +
		//        "<Severity>{2}</Severity>" +
		//        "<LogCategory>{3}</LogCategory>" +
		//        "<EventID>{4}</EventID></Message>",
		//                               header, body, (int)severity, category, eventID);

		//    return msg;
		//}

		public static LogEntry GetDefaultLogEntry()
		{
			LogEntry entry = new LogEntry(
				CommonUtil.MsgBody,
				CommonUtil.MsgCategory,
				-1,
				CommonUtil.MsgEventID,
				TraceEventType.Information,
				CommonUtil.MsgTitle,
				null
				);

			entry.Priority = 100;

			entry.TimeStamp = DateTime.MaxValue;
			entry.MachineName = "machine";

			return entry;
		}

        private static int eventLogEntryCounter = 0;

        public static void ResetEventLogCounter()
        {
            using (EventLog log = new EventLog(EventLogName))
            {
                eventLogEntryCounter = log.Entries.Count;
            }
        }

        private static int eventLogEntryCounterCustom = 0;

        public static void ResetEventLogCounterCustom()
        {
            eventLogEntryCounterCustom = GetCustomEventLog().Entries.Count;
        }

        public static int EventLogEntryCount()
        {
            using (EventLog log = new EventLog(EventLogName))
            {
                return log.Entries.Count - eventLogEntryCounter;
            }
        }

        public static int EventLogEntryCountCustom()
        {
            return GetCustomEventLog().Entries.Count - eventLogEntryCounterCustom;
        }

        public static string GetLastEventLogEntry()
        {
            using (EventLog log = new EventLog(EventLogName))
            {
                return log.Entries[log.Entries.Count - 1].Message;
            }
        }

        public static string GetLastEventLogEntryCustom()
        {
        	EventLog log = GetCustomEventLog();
        	return log.Entries[log.Entries.Count - 1].Message;
        }

        public static string ReadEventLogEntryBody()
        {
            using (EventLog log = new EventLog(EventLogName))
            {
                return log.Entries[log.Entries.Count - 1].Message;
            }
        }

        public static EventLog GetCustomEventLog()
        {
            if (!EventLog.Exists(CommonUtil.EventLogNameCustom))
            {
                using (EventLog log = new EventLog(EventLogNameCustom, ".", EventLogNameCustom + " Source"))
                {
                    log.WriteEntry("Event Log Created");
                }
            }
            return new EventLog(CommonUtil.EventLogNameCustom);
        }

		//public static Dictionary<string, object> GetSinkParams(string name, string value)
		//{
		//    Dictionary<string, object> parameters = new Dictionary<string, object>();
		//    parameters[name] = value;
		//    return parameters;
		//}

		//public static void SendTestMessage(ILogSink sink)
		//{
		//    LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
		//    sink.SendMessage(logEntry);
		//}

		//public static void SetDistributionStrategy(string type)
		//{
		//    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		//    LoggingSettings loggingSettings = (LoggingSettings)config.GetSection(LoggingSettings.SectionName);

		//    loggingSettings.LogWriter.DistributionStrategy = type;
		//    loggingSettings.LoggingEnabled = true;

		//    config.Save();
		//    System.Configuration.ConfigurationManager.RefreshSection(LoggingSettings.SectionName);
		//    Logger.Reset();
		//}

		public static bool LogEntryExists(string message)
		{
			// confirm listener started begin message written
			using (EventLog log = new EventLog(EventLogName))
			{
				string expected = message;
				string entry = log.Entries[log.Entries.Count - 1].Message;
				return (entry.IndexOf(expected) > -1);
			}
		}

		public static string ExecuteProcess(string command, string cmdarg)
		{
			ProcessStartInfo info = new ProcessStartInfo(command, cmdarg);

			info.RedirectStandardOutput = true;
			info.UseShellExecute = false;
			info.CreateNoWindow = true;
			using (Process p = Process.Start(info))
			{
				string result = p.StandardOutput.ReadToEnd();

				p.WaitForExit();

				return result;
			}
		}

		public static Dictionary<string, object> GetPropertiesDictionary()
		{
			Dictionary<string, object> hash = new Dictionary<string, object>();
			hash["key1"] = "value1";
			hash["key2"] = "value2";
			hash["key3"] = "value3";

			return hash;
		}

		public static IConfigurationSource SaveSectionsAndGetConfigurationSource(LoggingSettings loggingSettings)
		{
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = "test.exe.config";
			System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			rwConfiguration.Sections.Remove(LoggingSettings.SectionName);
			rwConfiguration.Sections.Add(LoggingSettings.SectionName, loggingSettings);

			File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
			rwConfiguration.Save();
			rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

			FileConfigurationSource.ResetImplementation(fileMap.ExeConfigFilename, false);
            return new FileConfigurationSource(fileMap.ExeConfigFilename);
		}

        public static long GetPerformanceCounterValue(string categoryName, string instanceName, string counterName)
        {
            if (PerformanceCounterCategory.InstanceExists(instanceName, categoryName))
            {
                return GetPerformanceCounterSample(categoryName, instanceName, counterName).RawValue;
            }
            return 0;
        }

        public static CounterSample GetPerformanceCounterSample(string categoryName, string instanceName, string counterName)
        {
            using (PerformanceCounter counter = new PerformanceCounter())
            {
                counter.CategoryName = categoryName;
                counter.CounterName = counterName;
                counter.InstanceName = instanceName;
                return counter.NextSample();
            }
        }

		//public static TextFormatter CreateTextFormatter(string template)
		//{
		//    return new TextFormatter(template);
		//}

		//public static string GetDefaultLogEntryFormattedString()
		//{
		//    return "12/31/9999 11:59:59 PM: === Header ===\r\n\r\nMy message body";
		//}

    }
}

