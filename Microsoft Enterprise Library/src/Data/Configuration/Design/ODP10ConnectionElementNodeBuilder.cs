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

using System;using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{	
	sealed class ODP10ConnectionNodeBuilder : NodeBuilder
	{
		private IConfigurationUIHierarchy hierarchy;
		private ODP10ConnectionSettings odp10ConnectionSettings;

        public ODP10ConnectionNodeBuilder(IServiceProvider serviceProvider, ODP10ConnectionSettings odp10ConnectionSettings)
			: base(serviceProvider)
		{
			this.hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            this.odp10ConnectionSettings = odp10ConnectionSettings;
		}

		public void Build()
		{			
			ConnectionStringsSectionNode node = hierarchy.FindNodeByType(typeof(ConnectionStringsSectionNode)) as ConnectionStringsSectionNode;			
			if (null == node)
			{
				LogError(hierarchy.RootNode, Resources.ExceptionMissingConnectionStrings);
				return;
			}

			for (int index = 0; index < odp10ConnectionSettings.ODP10ConnectionsData.Count; ++index)
			{
				ODP10ConnectionData odp10Connection = odp10ConnectionSettings.ODP10ConnectionsData.Get(index);
				ConnectionStringSettingsNode connectionStringNode = hierarchy.FindNodeByName(node, odp10Connection.Name) as ConnectionStringSettingsNode;
				if (null == connectionStringNode) 
				{
					LogError(node, string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionConnectionStringMissing, odp10Connection.Name));
					continue;
				}
				ODP10ConnectionElementNode odp10ElementNode = new ODP10ConnectionElementNode();				
				foreach (ODP10PackageData packageData in odp10Connection.Packages)
				{
					odp10ElementNode.AddNode(new ODP10PackageElementNode(packageData));
				}
				connectionStringNode.AddNode(odp10ElementNode);
			}				
		}		
	}
}
