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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder
{
	[TestClass]
	public class ExplicitInstrumentationAttacherFixture
	{
		[TestInitialize]
		public void ClearState()
		{
			ExplicitBinder.cachedSource = null;
			ExplicitBinder.cachedListener = null;
		}
		
		[TestMethod]
		public void ExplicitAttacherWillBeUsedToAttachInstrumentationInSourceAndListener()
		{
			ExplicitlyBoundSource source = new ExplicitlyBoundSource();
			object[] listenerConstructorArgs = new object[0];
			
			ExplicitInstrumentationAttacher attacher = 
				new ExplicitInstrumentationAttacher(source, 
				                                  typeof(ListenerType), listenerConstructorArgs, 
				                                  typeof(ExplicitBinder));
			attacher.BindInstrumentation();
			
			Assert.AreSame(source, ExplicitBinder.cachedSource);
			Assert.IsNotNull(ExplicitBinder.cachedListener);
		}
		
		[InstrumentationListener(typeof(ListenerType), typeof(ExplicitBinder))]
		public class ExplicitlyBoundSource {}
		
		public class ListenerType
		{
		}
		
		public class ExplicitBinder : IExplicitInstrumentationBinder
		{
			public static object cachedSource;
			public static object cachedListener;
			
			public void Bind(object source, object listener)
			{
				cachedSource = source;
				cachedListener = listener;
			}
		}
	}
}
