//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
	/// Static factory class used to get instances of a specified CacheManager
	/// </summary>
	public static class CacheFactory
	{
		private static CacheManagerFactory factory = new CacheManagerFactory(ConfigurationSourceFactory.Create());
		private static object lockObject = new object();


		/// <summary>
		/// Returns the default CacheManager instance. The same instance should be returned each time this method
		/// is called. The name of the instance to treat as the default CacheManager is defined in the configuration file.
		/// Guaranteed to return an intialized CacheManager if no exception thrown
		/// </summary>
		/// <returns>Default cache manager instance.</returns>
		/// <exception cref="ConfigurationException">Unable to create default CacheManager</exception>
		public static CacheManager GetCacheManager()
		{
			try
			{
				lock (lockObject)
				{
					return factory.CreateDefault();
				}
			}
			catch (ConfigurationErrorsException configurationException)
			{
				TryLogConfigurationError(configurationException, "default");

				throw;
			}
		}

		/// <summary>
		/// Returns the named CacheManager instance. Guaranteed to return an initialized CacheManager if no exception thrown.
		/// </summary>
		/// <param name="cacheManagerName">Name defined in configuration for the cache manager to instantiate</param>
		/// <returns>The requested CacheManager instance.</returns>
		/// <exception cref="ArgumentNullException">cacheManagerName is null</exception>
		/// <exception cref="ArgumentException">cacheManagerName is empty</exception>
		/// <exception cref="ConfigurationException">Could not find instance specified in cacheManagerName</exception>
		/// <exception cref="InvalidOperationException">Error processing configuration information defined in application configuration file.</exception>
		public static CacheManager GetCacheManager(string cacheManagerName)
		{
			try
			{
				lock (lockObject)
				{
					return factory.Create(cacheManagerName);
				}
			}
			catch (ConfigurationErrorsException configurationException)
			{
				TryLogConfigurationError(configurationException, cacheManagerName);

				throw;
			}
		}

		private static void TryLogConfigurationError(ConfigurationErrorsException configurationException, string instanceName)
		{
			try
			{
				DefaultCachingEventLogger eventLogger = EnterpriseLibraryFactory.BuildUp<DefaultCachingEventLogger>();
				if (eventLogger != null)
				{
					eventLogger.LogConfigurationError(instanceName, configurationException);
				}
			}
			catch { }
		}
	}
}