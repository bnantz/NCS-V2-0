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

using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    static class DefaultValues
    {
		
        public const bool TracingEnabled = false;
		
        public const string MsmqQueuePath = @".\Private$\myQueue";
		
        public const string EmailListenerToAddress = "to@example.com";
		
        public const string EmailListenerFromAddress = "from@example.com";
		
        public const string EmailListenerSmtpAddress = "127.0.0.1";
		
        public const int EmailListenerSmtpPort = 25;
		
        public const bool ClientTracingEnabled = false;
		
        public const bool ClientLoggingEnabled = true;
		
        public const string ClientDistributionStrategy = "In Process";
		
        public const int ClientMinimumPriority = 0;
		
        public const CategoryFilterMode ClientCategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
		
        public static readonly string DistributorDefaultCategory = Resources.DefaultCategory;
		
        public static readonly string DistributorDefaultFormatter = Resources.DefaultFormatter;
		
        public const string DistributorServiceName = "Enterprise Library Logging Distributor Service";
		
        public const string DistributorMsmqPath = @".\Private$\myQueue";
		
        public const int DistributorQueueTimerInterval = 1000;
		
        public const string FlatFileListenerFileName = "trace.log";
		
        public const string FlatFileListenerHeader = "----------------------------------------";
		
        public const string FlatFileListenerFooter = "----------------------------------------";
		
        public const string EventLogListenerLogName = "Application";
		
        public const string EventLogListenerEventSource = "Enterprise Library Logging";
		
        public static readonly string TextFormatterFormat = Resources.DefaultTextFormat;
    }
}