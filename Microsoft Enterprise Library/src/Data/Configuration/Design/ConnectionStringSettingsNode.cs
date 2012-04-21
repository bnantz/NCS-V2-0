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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System.Drawing.Design;
using System.Text;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// Represents a <see cref="ConnectionStringSettings"/> configuration element. 
    /// </summary>
    [Image(typeof(ConnectionStringSettingsNode))]
	public sealed class ConnectionStringSettingsNode : ConfigurationNode, IDatabaseProviderName
    {
        private string providerName;       

		/// <overloads>
	    /// Initialize a new instance of the <see cref="ConnectionStringSettingsNode"/> class.
		/// </overloads>
        /// <summary>
        /// Initialize a new instance of the <see cref="ConnectionStringSettingsNode"/> class.
        /// </summary>
        public ConnectionStringSettingsNode() : this(new ConnectionStringSettings(Resources.ConnectionStringNodeDefaultName, string.Empty))
        {
        }
        
		/// <summary>
		/// Initialize a new instance of the <see cref="ConnectionStringSettingsNode"/> class with a <see cref="ConnectionStringSettings"/> instance.
		/// </summary>
		/// <param name="connectionString">A <see cref="ConnectionStringSettings"/> instance.</param>
        public ConnectionStringSettingsNode(ConnectionStringSettings connectionString) : base()
        {
            if (null == connectionString)
            {
                throw new ArgumentNullException("connectionString");
            }
			this.providerName = connectionString.ProviderName;
			Rename(connectionString.Name);
        }

		/// <summary>
		/// Gets or sets the provider name to use for this connection.
		/// </summary>
		/// <value>
		/// The provider name to use for this connection
		/// </value>
		[Required]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("ProviderNameDescription", typeof(Resources))]
		[Editor(typeof(ProviderEditor), typeof(UITypeEditor))]
		public string ProviderName
		{
			get 
			
			{
				if (string.IsNullOrEmpty(providerName)) return "System.Data.SqlClient";
				return providerName;  
			}
			set { providerName = value;  }
		}

		/// <summary>
		/// Gets if children added to the node are sorted. 
		/// </summary>
		/// <value>
		/// Returns <c>false</c> so children are not sorted.
		/// </value>
		[Browsable(false)]
		public override bool SortChildren
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the database provider name to use with a connection.
		/// </summary>
		/// <value>
		/// The database provider name to use with a connection.
		/// </value>
		[Browsable(false)]
		string IDatabaseProviderName.DatabaseProviderName
		{
			get { return ProviderName;  }			
		}
    }
}