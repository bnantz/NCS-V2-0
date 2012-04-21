//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using CachingQuickStart.Properties;

namespace CachingQuickStart
{
    /// <summary>
    /// UserDB allows the user interface to communicate
    /// with the DataProvider object. It also uses a 
    /// cache in order to prevent retrieving data from the
    /// DataProvider as much as possible.
    /// </summary>
    public class ProductData
    {
			private CacheManager cache;
			private DataProvider dataProvider;
			public string dataSourceMessage;

			public ProductData()
			{
				cache = CacheFactory.GetCacheManager("Loading Scenario Cache Manager");
				dataProvider = new DataProvider();
			}

			/// <summary>
			/// Retrieve an item -in this case, one product, by using an unique object identifier
			/// as key.
			/// </summary>
			/// <param name="anID">Unique identification for an object</param>
			/// <returns>Product object associated to the given ID</returns>
			public Product ReadProductByID(string productID)
			{
				Product product = (Product)cache.GetData(productID);

				// Does our cache already have the requested object?
				if (product == null)
				{
					// Requested object is not cached, so let's retrieve it from
					// data provider and cache it for further requests.
					product = this.dataProvider.GetProductByID(productID);

					if (product != null)
					{
						cache.Add(productID, product);
						dataSourceMessage = string.Format(Resources.Culture, Resources.MasterSourceMessage, product.ProductID, product.ProductName, product.ProductPrice) + "\r\n";
					}
					else
					{
						dataSourceMessage = Resources.ItemNotAvailableMessage + "\r\n";
					}
				}
				else
				{
					dataSourceMessage = string.Format(Resources.Culture, Resources.CacheSourceMessage, product.ProductID, product.ProductName, product.ProductPrice) + "\r\n";
				}
				return product;
			}

		/// <summary>
		/// Reads all available items, adding them to the cache.
		/// </summary>
		public void LoadAllProducts()
		{
            List<Product> list = this.dataProvider.GetProductList();

			for (int i = 0; i < list.Count; i++)
			{
				Product product = list[i];
				cache.Add(product.ProductID, product);
			}
		}

		/// <summary>
		/// Removes all items from the cache..
		/// </summary>
        public void FlushCache()
        {
            cache.Flush();
        }
                
    }
}