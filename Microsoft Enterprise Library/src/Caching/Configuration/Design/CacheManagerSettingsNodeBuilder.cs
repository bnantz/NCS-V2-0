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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{	
	class CacheManagerSettingsNodeBuilder : NodeBuilder
	{
		private CacheManagerSettings cacheManagerSettings;						
		private CacheManagerNode defaultNode;		

		public CacheManagerSettingsNodeBuilder(IServiceProvider serviceProvider, CacheManagerSettings cacheManagerSettings) : base(serviceProvider)
		{
			this.cacheManagerSettings = cacheManagerSettings;			
		}

		public CacheManagerSettingsNode Build()
		{
			CacheManagerSettingsNode rootNode = new CacheManagerSettingsNode();
			CacheManagerCollectionNode node = new CacheManagerCollectionNode();
			foreach (CacheManagerData data in cacheManagerSettings.CacheManagers)
			{
				CreateCacheManagerNode(node, data);
			}			
			rootNode.AddNode(node);
			rootNode.DefaultCacheManager = defaultNode;
			return rootNode;
		}

		private void CreateCacheManagerNode(CacheManagerCollectionNode node, CacheManagerData cacheManagerData)
		{
			CacheManagerNode cacheManagerNode = new CacheManagerNode(cacheManagerData);
			node.AddNode(cacheManagerNode);
			if (cacheManagerNode.Name == cacheManagerSettings.DefaultCacheManager) defaultNode = cacheManagerNode;
			CreateStorageNode(cacheManagerNode, cacheManagerData.CacheStorage);
		}

		private void CreateStorageNode(CacheManagerNode cacheManagerNode,string cacheStorageName)
		{
			if (string.IsNullOrEmpty(cacheStorageName)) return;

			CacheStorageData cacheStorageData = cacheManagerSettings.BackingStores.Get(cacheStorageName);
			if (null == cacheStorageData) 
			{
				LogError(cacheManagerNode, string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionNoStorageProviderDefined, cacheStorageName));
				return;
			}
			if (cacheStorageData.Type == typeof(NullBackingStore)) return; // special case
			ConfigurationNode storageNode = NodeCreationService.CreateNodeByDataType(cacheStorageData.GetType(), new object[] { cacheStorageData });
			if (null == storageNode)
			{
				LogNodeMapError(cacheManagerNode, cacheStorageData.GetType());
				return;
			}
			cacheManagerNode.AddNode(storageNode);
			CreateEncryptionNode(storageNode, cacheStorageData.StorageEncryption);						
		}

		private void CreateEncryptionNode(ConfigurationNode storageNode, string storageEncryption)
		{
			if (string.IsNullOrEmpty(storageEncryption)) return;

			StorageEncryptionProviderData encryptionProviderData = cacheManagerSettings.EncryptionProviders.Get(storageEncryption);
			if (null == encryptionProviderData)
			{
				LogError(storageNode, string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionNoEncrypitonProviderDefined, storageEncryption));
				return;
			}

			ConfigurationNode encyrptionNode = NodeCreationService.CreateNodeByDataType(encryptionProviderData.GetType(), new object[] { encryptionProviderData });
			if (null == encyrptionNode)
			{
				LogNodeMapError(storageNode, encryptionProviderData.GetType());
				return;
			}
			storageNode.AddNode(encyrptionNode);
		}
	}
}
