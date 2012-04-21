//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
	class CachingNodeMapRegistrar : NodeMapRegistrar
	{
		public CachingNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
			AddSingleNodeMap(Resources.IsolatedStorageUICommandText,
				typeof(IsolatedStorageCacheStorageNode),
				typeof(IsolatedStorageCacheStorageData));

			AddSingleNodeMap(Resources.CustomStorageUICommandText,
				typeof(CustomCacheStorageNode),
				typeof(CustomCacheStorageData));
		}        
	}
}
