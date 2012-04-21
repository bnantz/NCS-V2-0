//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{	
	sealed class ConfigurationSourceSectionNodeBuilder : NodeBuilder
	{
		private ConfigurationSourceSection configurationSourceSection;						
		private ConfigurationSourceElementNode defaultNode;

		public ConfigurationSourceSectionNodeBuilder(IServiceProvider serviceProvider, ConfigurationSourceSection configurationSourceSection)
			: base(serviceProvider)
		{
			this.configurationSourceSection = configurationSourceSection;			
		}

		public ConfigurationSourceSectionNode Build()
		{
			ConfigurationSourceSectionNode rootNode = new ConfigurationSourceSectionNode();
			foreach (ConfigurationSourceElement configurationSourceElement in configurationSourceSection.Sources)
			{
				CreateConfigurationSourceElement(rootNode, configurationSourceElement);
			}			
			rootNode.SelectedSource = defaultNode;
			return rootNode;
		}

		private void CreateConfigurationSourceElement(ConfigurationSourceSectionNode node, ConfigurationSourceElement configurationSourceElement)
		{
			ConfigurationNode sourceNode = NodeCreationService.CreateNodeByDataType(configurationSourceElement.GetType(), new object[] { configurationSourceElement });
			if (null == sourceNode)
			{
				LogNodeMapError(node, configurationSourceElement.GetType());
				return;
			}
			if (configurationSourceSection.SelectedSource == configurationSourceElement.Name) defaultNode = (ConfigurationSourceElementNode)sourceNode;
			node.AddNode(sourceNode);
		}
	}
}
