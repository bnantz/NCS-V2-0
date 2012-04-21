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
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration;
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the oracle connection configuration section.
	/// </summary>    
    public sealed class ODP10ConnectionConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="OracleConnectionConfigurationDesignManager"/> class.
        /// </summary>
        public ODP10ConnectionConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
            ODP10ConnectionCommandRegistrar registrar = new ODP10ConnectionCommandRegistrar(serviceProvider);
			registrar.Register();
        }

		/// <summary>
		/// Opens the oracle connection configuration section, builds the design time nodes and adds them to the application node.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
			if (null != section)
			{
                ODP10ConnectionNodeBuilder builder = new ODP10ConnectionNodeBuilder(serviceProvider, (ODP10ConnectionSettings)section);
				builder.Build();
			}
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the oracle connection configuration section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the oracle connection configuration section.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            ODP10ConnectionSettings odp10ConnectionSection = null;
			IList<ConfigurationNode> connections = rootNode.Hierarchy.FindNodesByType(typeof(OracleConnectionElementNode));
			if (connections.Count == 0)
			{
				odp10ConnectionSection = null;
			}
			else
			{
                ODP10ConnectionSettingsBuilder builder = new ODP10ConnectionSettingsBuilder(serviceProvider);
                odp10ConnectionSection = builder.Build();
			}
            return new ConfigurationSectionInfo(rootNode, odp10ConnectionSection, ODP10ConnectionSettings.SectionName);
		}		
	}
}