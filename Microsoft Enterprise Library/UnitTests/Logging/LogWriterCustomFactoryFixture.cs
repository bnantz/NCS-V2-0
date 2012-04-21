//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
	[TestClass]
	public class LogWriterCustomFactoryFixture
	{
		private object createdObject1 = null;
		private object createdObject2 = null;

		[TestCleanup]
		public void TearDown()
		{
			if (createdObject1 != null && createdObject1 is IDisposable)
				(createdObject1 as IDisposable).Dispose();
			if (createdObject2 != null && createdObject2 is IDisposable)
				(createdObject2 as IDisposable).Dispose();
		}

		[TestMethod]
		public void CanBuildLogWriterFromConfiguration()
		{
			MockBuilderContext context = CreateContext(null, new SystemConfigurationSource());

			createdObject1 = context.HeadOfChain.BuildUp(context, typeof(LogWriter), null, null);

			Assert.IsNotNull(createdObject1);
			Assert.AreSame(typeof(LogWriter), createdObject1.GetType());
		}

		[TestMethod]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void RequestForLogWriterWithoutSettingsThrows()
		{
			MockBuilderContext context = CreateContext(null, new DictionaryConfigurationSource());

			context.HeadOfChain.BuildUp(context, typeof(LogWriter), null, null);
		}

		[TestMethod]
		public void RequestForLogWriterWithLocatorAddsSingleton()
		{
			Locator locator = new Locator();
			LifetimeContainer container = new LifetimeContainer();
			locator.Add(typeof(ILifetimeContainer), container);

			MockBuilderContext context = CreateContext(locator, new SystemConfigurationSource());


			createdObject1
				= context.HeadOfChain.BuildUp(context, typeof(LogWriter), null, null);
			createdObject2
				= context.HeadOfChain.BuildUp(context, typeof(LogWriter), null, null);

			Assert.IsNotNull(createdObject1);
			Assert.AreSame(typeof(LogWriter), createdObject1.GetType());
			Assert.IsTrue(object.ReferenceEquals(createdObject1, createdObject2));
		}

		[TestMethod]
		public void RequestForLogWriterWithoutLocatorDoesNotAddSingleton()
		{
			MockBuilderContext context = CreateContext(null, new SystemConfigurationSource());

			createdObject1
				= context.HeadOfChain.BuildUp(context, typeof(LogWriter), null, null);
			createdObject2
				= context.HeadOfChain.BuildUp(context, typeof(LogWriter), null, null);

			Assert.IsNotNull(createdObject1);
			Assert.AreSame(typeof(LogWriter), createdObject1.GetType());
			Assert.IsFalse(object.ReferenceEquals(createdObject1, createdObject2));
		}

		[TestMethod]
		public void SameLogWriterFactoryReturnsDifferentInstances()
		{
			LogWriterFactory factory = new LogWriterFactory();

			createdObject1 = factory.Create();
			createdObject2 = factory.Create();

			Assert.IsNotNull(createdObject1);
			Assert.IsFalse(object.ReferenceEquals(createdObject1, createdObject2));
		}

		[TestMethod]
		public void DifferentLogWriterFactoryReturnsDifferentInstance()
		{
			LogWriterFactory factory = new LogWriterFactory();
			createdObject1 = factory.Create();

			factory = new LogWriterFactory();
			createdObject2 = factory.Create();

			Assert.IsNotNull(createdObject1);
			Assert.IsFalse(object.ReferenceEquals(createdObject1, createdObject2));
		}

		private MockBuilderContext CreateContext(IReadWriteLocator locator, IConfigurationSource configurationSource)
		{
			MockBuilderContext context = new MockBuilderContext(locator);
			context.InnerChain.Add(new SingletonStrategy());
			context.InnerChain.Add(
				new MockFactoryStrategy(
					new LogWriterCustomFactory(),
					configurationSource,
					new ConfigurationReflectionCache()));

			return context;
		}
	}
}
