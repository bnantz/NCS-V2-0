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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
	[TestClass]
	public class InstrumentationAttacherFactoryFixture
	{
		[TestMethod]
		public void NoBinderInstanceReturnedIfNoAttributeOnSourceClass()
		{
			NoListenerSource source = new NoListenerSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);
			
			Assert.AreSame(typeof(NoBindingInstrumentationAttacher), createdAttacher.GetType());
		}
		
		[TestMethod]
		public void ReflectionBinderInstanceReturnedIfSingleArgumentAttributeOnSourceClass()
		{
			ReflectionBindingSource source = new ReflectionBindingSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);
			
			Assert.AreSame(typeof (ReflectionInstrumentationAttacher), createdAttacher.GetType());
		}
		
		[TestMethod]
		public void ExplicitBinderInstanceReturnedIfTwoArgumentAttributeOnSourceClass()
		{
			ExplicitBindingSource source = new ExplicitBindingSource();
			InstrumentationAttacherFactory factory = new InstrumentationAttacherFactory();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

			IInstrumentationAttacher createdAttacher = factory.CreateBinder(source, new object[0], reflectionCache);
			
			Assert.AreSame(typeof (ExplicitInstrumentationAttacher), createdAttacher.GetType());
		}
		
		public class NoListenerSource
		{
		}
		
		[InstrumentationListener(typeof(FooListener))]
		public class ReflectionBindingSource
		{
		}
		
		[InstrumentationListener(typeof(FooListener), typeof(FooBinder))]
		public class ExplicitBindingSource
		{
		}
		
		public class FooListener {}
		public class FooBinder {}
	}
}
