//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// Represents the <see cref="ODP10ConnectionElementNode"/> configuration section.
    /// </summary>    
    [Image(typeof(ODP10ConnectionElementNode))]
	public sealed class ODP10ConnectionElementNode : ConfigurationNode
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="OracleConnectionElementNode"/> class.
		/// </summary>
        public ODP10ConnectionElementNode()
			: base(Resources.OraclePackagesNodeName)
		{
			
		}

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		/// <remarks>
		/// Overridden to make readonly for the design tool. 
		/// </remarks>
		[ReadOnly(true)]
		public override string Name
		{
			get
			{
				return base.Name;
			}			
		}		
	}
}