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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    [TestClass]
    public class EventBinderFixture
    {
        [TestMethod]
        public void AttachesEventToSubject()
        {
            Publisher publisher = new Publisher();
            EventInfo eventInfo= GetMemberInfo<EventInfo>(publisher, "FooEvent");

            Subscriber subscriber = new Subscriber();
            MethodInfo methodInfo = GetMemberInfo<MethodInfo>(subscriber, "HookMeUp");

            EventBinder binder = new EventBinder(publisher, subscriber);
            binder.Bind(eventInfo, methodInfo);

            publisher.Raise();

            Assert.IsTrue(subscriber.EventRaised);
        }

        private TMemberInfo GetMemberInfo<TMemberInfo>(object targetObject, string name) where TMemberInfo:MemberInfo
        {
            Type type = targetObject.GetType();
            MemberInfo memberInfo = type.GetMember(name)[0];

            return (TMemberInfo)memberInfo;
        }

        private class Publisher
        {
            public delegate void FooDelegate();
            public event FooDelegate FooEvent;
            public void Raise() { FooEvent(); }
        }

        private class Subscriber
        {
            public bool EventRaised = false;
            public void HookMeUp() { EventRaised = true; }
        }
    }
}
