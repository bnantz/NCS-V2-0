//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
	internal class SecurityCacheDataRetriever : IConfigurationNameMapper
	{
		public string MapName(string name, IConfigurationSource configurationSource)
		{
			if (name != null)
				return name;

			return new SecurityConfigurationView(configurationSource).GetDefaultSecurityCacheProviderName();
		}
	}
}
