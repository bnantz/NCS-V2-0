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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
	sealed class AddLoggingExceptionHandlerCommand : AddChildNodeCommand
	{		
		public AddLoggingExceptionHandlerCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(LoggingExceptionHandlerNode))
		{

		}
		
		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			LoggingExceptionHandlerNode node = ChildNode as LoggingExceptionHandlerNode;
			if (null == node) return;

			if (null == CurrentHierarchy.FindNodeByType(typeof(LoggingSettingsNode)))
			{
				ConfigurationApplicationNode applicationNode = (ConfigurationApplicationNode)CurrentHierarchy.FindNodeByType(typeof(ConfigurationApplicationNode));
				new AddLoggingSettingsNodeCommand(ServiceProvider).Execute(applicationNode);
			}
		}
	}
}
