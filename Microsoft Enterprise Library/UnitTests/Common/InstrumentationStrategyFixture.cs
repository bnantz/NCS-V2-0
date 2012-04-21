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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
	[TestClass]
	public class InstrumentationStrategyFixture
	{
		[TestMethod]
		public void InstancesWithNamesWillBeAttachedToTheirInstrumentationListeners()
		{
			DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
			configSource.Add(InstrumentationConfigurationSection.SectionName,
							 new InstrumentationConfigurationSection(true, true, true));
			MockBuilderContext context = new MockBuilderContext();
			context.Policies.AddPolicies(GetPolicies(configSource));
			InstrumentationStrategy strategy = new InstrumentationStrategy();
			NamedSource namedSource = new NamedSource();
			NamedSource createdObject = strategy.BuildUp<NamedSource>(context, namedSource, "Foo");

			Assert.IsTrue(createdObject.IsWired);
		}

		#region NamedMocks
		[InstrumentationListener(typeof(NameExpectingListener))]
		public class NamedSource
		{
			[InstrumentationProvider("A")]
			public event EventHandler<EventArgs> myA;

			public bool IsWired { get { return myA != null; } }
		}

		public class NameExpectingListener : InstrumentationListener
		{
			public NameExpectingListener(string instanceName, bool a, bool b, bool c)
				: base(instanceName, a, b, c, new NoPrefixNameFormatter())
			{
			}

			[InstrumentationConsumer("A")]
			public void A(object sender, EventArgs e)
			{
			}
		}
		#endregion
		
		[TestMethod]
		public void InstancesWithMadeUpNameWillBeAttachedToTheirInstrumentationListeners()
		{
			DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
			configSource.Add(InstrumentationConfigurationSection.SectionName,
							 new InstrumentationConfigurationSection(true, true, true));
			MockBuilderContext context = new MockBuilderContext();
			context.Policies.AddPolicies(GetPolicies(configSource));
			InstrumentationStrategy strategy = new InstrumentationStrategy();
			MadeUpNamedSource namedSource = new MadeUpNamedSource();
			MadeUpNamedSource createdObject = strategy.BuildUp<MadeUpNamedSource>(context, namedSource, ConfigurationNameProvider.MakeUpName());

			Assert.IsTrue(createdObject.IsWired);
		}

		[TestMethod]
		public void InstancesWithNoNameWillBeAttachedToTheirInstrumentationListeners()
		{
			DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
			configSource.Add(InstrumentationConfigurationSection.SectionName,
							 new InstrumentationConfigurationSection(true, true, true));
			MockBuilderContext context = new MockBuilderContext();
			context.Policies.AddPolicies(GetPolicies(configSource));
			InstrumentationStrategy strategy = new InstrumentationStrategy();
			
			UnnamedSource source = new UnnamedSource();
			UnnamedSource createdObject = strategy.BuildUp<UnnamedSource>(context, source, null);

			Assert.IsTrue(createdObject.IsWired);
		}

		#region NoNameMocks
		[InstrumentationListener(typeof(UnnamedListener))]
		public class MadeUpNamedSource
		{
			[InstrumentationProvider("A")]
			public event EventHandler<EventArgs> myA;

			public bool IsWired { get { return myA != null; } }
		}

		[InstrumentationListener(typeof(UnnamedListener))]
		public class UnnamedSource
		{
			[InstrumentationProvider("A")]
			public event EventHandler<EventArgs> myA;

			public bool IsWired { get { return myA != null; } }
		}
		
		public class UnnamedListener : InstrumentationListener
		{
			public UnnamedListener(bool a, bool b, bool c)
				: base(a, b, c, new NoPrefixNameFormatter())
			{
			}

			[InstrumentationConsumer("A")]
			public void A(object sender, EventArgs e)
			{
			}
		}
		#endregion

		private static PolicyList GetPolicies(IConfigurationSource configurationSource)
		{
			PolicyList policyList = new PolicyList();
			policyList.Set<IConfigurationObjectPolicy>(new ConfigurationObjectPolicy(configurationSource), typeof(IConfigurationSource), null);

			return policyList;
		}
	}
}
