//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design
{
	sealed class SecurityCacheCachingStoreCommandRegistrar : CommandRegistrar
    {
        public SecurityCacheCachingStoreCommandRegistrar(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
        }

        public override void Register()
        {
            AddCachingStoreProviderNodeCommand();
            AddDefaultCommands(typeof(CachingStoreProviderNode));
        
        }

        private void AddCachingStoreProviderNodeCommand()
        {
            ConfigurationUICommand item = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                Resources.SecurityInstance,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.SecurityInstance),
				new AddCachingStoreProviderNodeCommand(ServiceProvider),
				typeof(SecurityCacheProviderNode));

            AddUICommand(item, typeof(SecurityCacheProviderCollectionNode));
        }
    }
}
