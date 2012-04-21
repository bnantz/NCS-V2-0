//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
	public abstract class ConfigurationDesignHost
	{		
		private IConfigurationUIHierarchy hierarchy;
		private IServiceProvider serviceProvider;
		private ConfigurationApplicationNode appNode;		
		
		[TestInitialize]
		public void TestInitialize()
		{
			BeforeSetup();

			appNode = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
			serviceProvider = ServiceBuilder.Build();
			hierarchy = new ConfigurationUIHierarchy(appNode, serviceProvider);
            ServiceHelper.GetUIHierarchyService(ServiceProvider).SelectedHierarchy = Hierarchy;
			hierarchy.Load();
			InitializeCore();

			AfterSetup();
		}

		protected virtual void InitializeCore()
		{

		}

		[TestCleanup]
		public void TestCleanup()
		{
			BeforeCleanup();

			CleanupCore();
			hierarchy.Dispose();
			IDisposable disposableServiceProvider = serviceProvider as IDisposable;
			if (null != disposableServiceProvider) disposableServiceProvider.Dispose();

			AfterCleanup();
		}

		public virtual void BeforeSetup() { }
		public virtual void AfterSetup() { }
		public virtual void BeforeCleanup() { }
		public virtual void AfterCleanup() {}

		protected virtual void CleanupCore()
		{

		}

		protected IServiceProvider ServiceProvider
		{
			get { return serviceProvider; }
		}

		
		protected IConfigurationUIHierarchy Hierarchy
		{
			get { return hierarchy;  }
		}

		protected ConfigurationApplicationNode ApplicationNode 
		{
			get { return appNode;  }
		}

		protected IConfigurationUIHierarchyService HiearchyService
		{
			get
			{
				return (IConfigurationUIHierarchyService)serviceProvider.GetService(typeof(IConfigurationUIHierarchyService));
			}
		}

		protected INodeCreationService NodeCreationService
		{
			get
			{
				return (INodeCreationService)serviceProvider.GetService(typeof(INodeCreationService));
			}
		}

		protected IErrorLogService ErrorLogService
		{
			get
			{
				return (IErrorLogService)serviceProvider.GetService(typeof(IErrorLogService));
			}
		}

		protected IUIService UIService 
		{
			get 
			{
				return (IUIService)serviceProvider.GetService(typeof(IUIService));
			}
		}

		protected IUICommandService UICommandService 
		{
			get 
			{
				return (IUICommandService)serviceProvider.GetService(typeof(IUICommandService));
			}
		}

        protected void SetDictionaryConfigurationSource(DictionaryConfigurationSource configurationSource)
        {
            ConfigurationSourceSectionNode configurationSourceSection = new ConfigurationSourceSectionNode();
            DictionarySourceElementNode configurationSourceNode = new DictionarySourceElementNode(configurationSource);
            ApplicationNode.AddNode(configurationSourceSection);

            configurationSourceSection.AddNode(configurationSourceNode);
            configurationSourceSection.SelectedSource = configurationSourceNode;
        }

        private class DictionarySourceElementNode : ConfigurationSourceElementNode
        {
            DictionaryConfigurationSource source;

            public DictionarySourceElementNode(DictionaryConfigurationSource configurationSource) : base("dictionary source")
            {
                source = configurationSource;
            }

            public override Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource ConfigurationSource
            {
                get { return source; }
            }

            public override Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationParameter ConfigurationParameter
            {
                get { return null; }
            }

			public override ConfigurationSourceElement ConfigurationSourceElement
			{
				get { return null; }
			}
		}
	}	
}
