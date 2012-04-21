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
using System.Text;
using System.Collections.Generic;
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Instrumentation
{
	/// <summary>
	/// Summary description for ReflectionInstallerForInternalClassesFixture
	/// </summary>
	[TestClass]
	public class ReflectionInstallerForInternalClassesFixture
	{
		internal class InternalListener
		{
			public bool callbackHappened;

			[InstrumentationConsumer("TestMessage")]
			public void CallMe(object sender, EventArgs e)
			{
				callbackHappened = true;
			}
		}

		public class EventSource
		{
			[InstrumentationProvider("TestMessage")]
			public event EventHandler<EventArgs> myEvent;

			public void Fire()
			{
				myEvent(this, new EventArgs());
			}
		}

		[TestMethod]
		public void InstallerWillWireUpSubjectToPublicMethodInInternalListenerClass()
		{
			InternalListener listener = new InternalListener();
			EventSource source = new EventSource();
			ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
			binder.Bind(source, listener);

			source.Fire();

			Assert.IsTrue(listener.callbackHappened);
		}
	}
}
