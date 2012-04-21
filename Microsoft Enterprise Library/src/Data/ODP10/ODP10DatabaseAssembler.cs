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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.ODP10;
using Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.ODP10
{
    internal class ODP10DatabaseAssembler : IDatabaseAssembler
	{
		public Database Assemble(string name, ConnectionStringSettings connectionStringSettings, IConfigurationSource configurationSource)
		{
            ODP10ConnectionSettings oracleConnectionSettings = ODP10ConnectionSettings.GetSettings(configurationSource);
			if (oracleConnectionSettings != null)
			{
				ODP10ConnectionData oracleConnectionData = oracleConnectionSettings.ODP10ConnectionsData.Get(name);
				if (oracleConnectionData != null)
				{
                    IODP10Package[] packages = new IODP10Package[oracleConnectionData.Packages.Count];
					int i = 0;
                    foreach (IODP10Package package in oracleConnectionData.Packages)
					{
						packages[i++] = package;
					}

					return new ODP10Database(connectionStringSettings.ConnectionString, packages);
				}
			}

            return new ODP10Database(connectionStringSettings.ConnectionString);
		}
	}
}
