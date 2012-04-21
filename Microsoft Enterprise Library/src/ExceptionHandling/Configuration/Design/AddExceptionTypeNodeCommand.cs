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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// Intercepts creation of the ExceptionTypeDesignNode
    /// </summary>
    class AddExceptionTypeNodeCommand : AddChildNodeCommand
    {
        public AddExceptionTypeNodeCommand(IServiceProvider serviceProvider, Type childType) : base(serviceProvider, childType)
        {
        }

        protected override void ExecuteCore(ConfigurationNode node)
        {
			Type selectedType = SelectedType;
			if (null == selectedType) return;
			base.ExecuteCore(node);
			ExceptionTypeNode typeNode = (ExceptionTypeNode)ChildNode;
			typeNode.PostHandlingAction = PostHandlingAction.NotifyRethrow;			
			try
			{
				typeNode.Type = selectedType;
			}
			catch (InvalidOperationException)
			{
				typeNode.Remove();
				UIService.ShowError(string.Format(Resources.Culture, Resources.DuplicateExceptionTypeErrorMessage, selectedType != null ? selectedType.Name : string.Empty));
			}		
        }

		protected virtual Type SelectedType
		{
			get
			{
				TypeSelectorUI selector = new TypeSelectorUI(
					typeof(Exception),
					typeof(Exception),
					TypeSelectorIncludes.BaseType |
						TypeSelectorIncludes.AbstractTypes);
				DialogResult result = selector.ShowDialog();
				if (result == DialogResult.OK)
				{
					return selector.SelectedType;
				}
				return null;
			}
		}
    }
}