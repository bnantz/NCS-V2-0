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
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	public class MockFactoryStrategy : BuilderStrategy
	{
		private ICustomFactory factory;
		private IConfigurationSource configurationSource;
		private ConfigurationReflectionCache reflectionCache;

		public MockFactoryStrategy(ICustomFactory factory, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			this.factory = factory;
			this.configurationSource = configurationSource;
			this.reflectionCache = reflectionCache;
		}

		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
		{
			existing = factory.CreateObject(context, idToBuild, configurationSource, reflectionCache);
			return base.BuildUp(context, typeToBuild, existing, idToBuild);
		}
	}
}