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

using System;using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{	
	sealed class ExceptionHandlingSettingsNodeBuilder : NodeBuilder
	{
		private ExceptionHandlingSettings settings;		
		
		public ExceptionHandlingSettingsNodeBuilder(IServiceProvider serviceProvider, ExceptionHandlingSettings settings)
			: base(serviceProvider)
		{
			this.settings = settings;			
		}

		public ExceptionHandlingSettingsNode Build()
		{
			ExceptionHandlingSettingsNode node = new ExceptionHandlingSettingsNode();			
			foreach (ExceptionPolicyData policyData in settings.ExceptionPolicies)
			{
				BuildExceptionPolicyNode(node, policyData);
			}			
			return node;
		}		

		private void BuildExceptionPolicyNode(ExceptionHandlingSettingsNode node, ExceptionPolicyData policyData)
		{
			ExceptionPolicyNode policyNode = new ExceptionPolicyNode(policyData);
			node.AddNode(policyNode);
			foreach (ExceptionTypeData exceptionTypeData in policyData.ExceptionTypes)
			{
				BuildExceptionTypeNode(policyNode, exceptionTypeData);
			}			
		}

		private void BuildExceptionTypeNode(ExceptionPolicyNode policyNode, ExceptionTypeData exceptionTypeData)
		{
			ExceptionTypeNode exceptionTypeNode = new ExceptionTypeNode(exceptionTypeData);
			policyNode.AddNode(exceptionTypeNode);				
			foreach (ExceptionHandlerData exceptionHandlerData in exceptionTypeData.ExceptionHandlers)
			{
				BuildExceptionHandlerNode(exceptionTypeNode, exceptionHandlerData);
			}			
		}

		private void BuildExceptionHandlerNode(ExceptionTypeNode exceptionTypeNode, ExceptionHandlerData exceptionHandlerData)
		{
			ConfigurationNode exceptionHandlerNode = NodeCreationService.CreateNodeByDataType(exceptionHandlerData.GetType(), new object[] { exceptionHandlerData });
			exceptionTypeNode.AddNode(exceptionHandlerNode);
		}
	}
}
