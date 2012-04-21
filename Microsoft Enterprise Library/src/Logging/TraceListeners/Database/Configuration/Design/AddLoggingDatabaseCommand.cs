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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
	sealed class AddLoggingDatabaseCommand : AddChildNodeCommand
	{	
		public AddLoggingDatabaseCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(LoggingDatabaseNode))
		{

		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			LoggingDatabaseNode node = ChildNode as LoggingDatabaseNode;
			if (null == node) return;

			if (null == CurrentHierarchy.FindNodeByType(typeof(LoggingSettingsNode)))
			{				
				new AddLoggingSettingsNodeCommand(ServiceProvider).Execute(CurrentHierarchy.RootNode);
			}

			if (null == CurrentHierarchy.FindNodeByType(typeof(DatabaseSectionNode)))
			{				
				new AddDatabaseSectionNodeCommand(ServiceProvider).Execute(CurrentHierarchy.RootNode);
			}
		}
	}
}
