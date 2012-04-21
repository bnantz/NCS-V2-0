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
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	class AddConfigurationSourceSectionNodeCommand : AddChildNodeCommand
	{
		public AddConfigurationSourceSectionNodeCommand(IServiceProvider serviceProvider) : base(serviceProvider, typeof(ConfigurationSourceSectionNode))
		{

		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			ConfigurationSourceSectionNode node = ChildNode as ConfigurationSourceSectionNode;
			Debug.Assert(node != null);

			SystemConfigurationSourceElementNode sourceNode = new SystemConfigurationSourceElementNode();
			node.AddNode(sourceNode);
			node.SelectedSource = sourceNode;
		}
	}
}
