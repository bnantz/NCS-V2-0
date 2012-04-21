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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class ODP10ConnectionSettingsBuilder 
	{
		private IConfigurationUIHierarchy hierarchy;
		private ODP10ConnectionSettings odp10ConnectionSettings;

        public ODP10ConnectionSettingsBuilder(IServiceProvider serviceProvider) 
		{
			this.hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
		}

        public ODP10ConnectionSettings Build()
		{
            odp10ConnectionSettings = new ODP10ConnectionSettings();
			IList<ConfigurationNode> connections = hierarchy.FindNodesByType(typeof(OracleConnectionElementNode));
			for (int index = 0; index < connections.Count; ++index)
			{
                ODP10ConnectionData data = new ODP10ConnectionData();
				data.Name = connections[index].Parent.Name;
                foreach (ODP10PackageElementNode node in connections[index].Nodes)
				{
                    data.Packages.Add(node.ODP10PackageElement);
				}
                odp10ConnectionSettings.ODP10ConnectionsData.Add(data);
			}
            return odp10ConnectionSettings;
		}
	}
}
