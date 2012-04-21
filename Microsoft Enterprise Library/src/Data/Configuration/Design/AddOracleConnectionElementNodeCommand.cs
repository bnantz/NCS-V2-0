//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class AddOracleConnectionElementNodeCommand : AddChildNodeCommand
	{
		public AddOracleConnectionElementNodeCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(OracleConnectionElementNode))
        {
        }

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);
			OracleConnectionElementNode node = ChildNode as OracleConnectionElementNode;
			Debug.Assert(null != node, "Expected OracleConnectionElementNode");
			
			node.AddNode(new OraclePackageElementNode());			
		}
	}
}
