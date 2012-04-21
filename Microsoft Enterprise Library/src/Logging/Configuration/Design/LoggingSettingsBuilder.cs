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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
	sealed class LoggingSettingsBuilder 
	{
		private LoggingSettingsNode loggingSettingsNode;
		private IConfigurationUIHierarchy hierarchy;
		private LoggingSettings loggingSettings;

		public LoggingSettingsBuilder(IServiceProvider serviceProvider, LoggingSettingsNode loggingSettingsNode) 
		{
			this.loggingSettingsNode = loggingSettingsNode;
			hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}
		
		public LoggingSettings Build()
		{
			loggingSettings = new LoggingSettings(loggingSettingsNode.Name, 
				loggingSettingsNode.TracingEnabled, 
				loggingSettingsNode.DefaultCategory == null ? string.Empty : loggingSettingsNode.DefaultCategory.Name);
			loggingSettings.LogWarningWhenNoCategoriesMatch = loggingSettingsNode.LogWarningWhenNoCategoriesMatch;
			BuilFormatters();
			BuildLogFilters();
			BuildTraceListeners();
			BuildTraceSources();
			BuildSpecialTraceSources();

			return loggingSettings;
		}

		private void BuildSpecialTraceSources()
		{
			SpecialTraceSourcesNode specialSourcesNode = (SpecialTraceSourcesNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(SpecialTraceSourcesNode));
			if (specialSourcesNode != null)
			{
				ErrorsTraceSourceNode errorSourceNode = (ErrorsTraceSourceNode)hierarchy.FindNodeByType(specialSourcesNode, typeof(ErrorsTraceSourceNode));
				if (errorSourceNode != null)
				{
					loggingSettings.SpecialTraceSources.ErrorsTraceSource = errorSourceNode.TraceSourceData;
					BuildTraceListenerReferences(loggingSettings.SpecialTraceSources.ErrorsTraceSource, errorSourceNode);
				}
				AllTraceSourceNode allTraceSourceNodes = (AllTraceSourceNode)hierarchy.FindNodeByType(specialSourcesNode, typeof(AllTraceSourceNode));
				if (allTraceSourceNodes != null)
				{
					loggingSettings.SpecialTraceSources.AllEventsTraceSource = allTraceSourceNodes.TraceSourceData;
					BuildTraceListenerReferences(loggingSettings.SpecialTraceSources.AllEventsTraceSource, allTraceSourceNodes);
				}
				NotProcessedTraceSourceNode notProcessedSourceNode = (NotProcessedTraceSourceNode)hierarchy.FindNodeByType(specialSourcesNode, typeof(NotProcessedTraceSourceNode));
				if (notProcessedSourceNode != null)
				{
					loggingSettings.SpecialTraceSources.NotProcessedTraceSource = notProcessedSourceNode.TraceSourceData;
					BuildTraceListenerReferences(loggingSettings.SpecialTraceSources.NotProcessedTraceSource, notProcessedSourceNode);
				}
			}
		}

		private static void BuildTraceListenerReferences(TraceSourceData data, TraceSourceNode node)
		{
			foreach (TraceListenerReferenceNode refNode in node.Nodes)
			{
				data.TraceListeners.Add(refNode.TraceListenerReferenceData);
			}
		}

		private void BuildTraceSources()
		{
			CategoryTraceSourceCollectionNode categoryTracesourceCollectioNode = (CategoryTraceSourceCollectionNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(CategoryTraceSourceCollectionNode));
			if (categoryTracesourceCollectioNode != null)
			{
				foreach (CategoryTraceSourceNode traceSourceNode in categoryTracesourceCollectioNode.Nodes)
				{
                    TraceSourceData traceSourceData =traceSourceNode.TraceSourceData;
                    BuildTraceListenerReferences(traceSourceData, traceSourceNode);
					loggingSettings.TraceSources.Add(traceSourceData);
				}
			}
		}

		private void BuildTraceListeners()
		{
			TraceListenerCollectionNode traceListenerCollectionNode = (TraceListenerCollectionNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(TraceListenerCollectionNode));
			if (traceListenerCollectionNode != null)
			{
				foreach (TraceListenerNode listenerNode in traceListenerCollectionNode.Nodes)
				{
					loggingSettings.TraceListeners.Add(listenerNode.TraceListenerData);
				}
			}
		}

		private void BuildLogFilters()
		{
			LogFilterCollectionNode logFilterCollectionNode = (LogFilterCollectionNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(LogFilterCollectionNode));
			if (logFilterCollectionNode != null)
			{
				foreach (LogFilterNode filterNode in logFilterCollectionNode.Nodes)
				{
					loggingSettings.LogFilters.Add(filterNode.LogFilterData);
				}
			}
		}

		private void BuilFormatters()
		{
			FormatterCollectionNode formatterCollectionNode = (FormatterCollectionNode)hierarchy.FindNodeByType(loggingSettingsNode, typeof(FormatterCollectionNode));
			if (formatterCollectionNode != null)
			{
				foreach (FormatterNode formatterNode in formatterCollectionNode.Nodes)
				{
					loggingSettings.Formatters.Add(formatterNode.FormatterData);
				}
			}
		}
	}	
}
