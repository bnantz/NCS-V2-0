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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    internal class NullSubjectSource
    {
        [InstrumentationProvider(null)]
        public event EventHandler<EventArgs> Foo;

        public void NeverCalled()
        {
            Foo(this, new EventArgs());
        }
    }

    internal class BaseWithAttributeToOverride
    {
        [InstrumentationConsumer("NeverMatchThis")]
        public virtual void Handle(object sender, EventArgs e)
        {
        }
    }

    internal class DerivedListenerWithOverridingAttribute : BaseWithAttributeToOverride
    {
        public bool eventWasRaised;

        [InstrumentationConsumer("Single")]
        public override void Handle(object sender, EventArgs e)
        {
            eventWasRaised = true;
        }
    }

    internal class DerivedListenerWithOverriddenNoAttributeListenerMethod : SingleEventListener
    {
        public override void TestHandler(object sender, EventArgs e)
        {
        }
    }

    internal class DerivedSingleEventListener : SingleEventListener
    {
    }

    internal class SingleEventSource
    {
        [InstrumentationProvider("Single")]
        public event EventHandler<EventArgs> TestEvent;

        public void Raise() { if (TestEvent != null) TestEvent(this, new EventArgs()); }
    }

    internal class SingleEventSourceWithOtherEvents
    {
        public event EventHandler<EventArgs> NeverUsedEvent;

        [InstrumentationProvider("Single")]
        public event EventHandler<EventArgs> TestEvent;

        public void Raise() { TestEvent(this, new EventArgs()); }

        public void NeverCalled()
        {
            NeverUsedEvent(this, new EventArgs());
        }
    }

    internal class TwoOfSameEventSource
    {
        [InstrumentationProvider("foo")]
        public event EventHandler<EventArgs> FooEvent;

        [InstrumentationProvider("foo")]
        public event EventHandler<EventArgs> BarEvent;

        public void Raise()
        {
            FooEvent(this, new EventArgs());
            BarEvent(this, new EventArgs());
        }
    }
    
    internal class TwoEventSource
    {
        [InstrumentationProvider("Subject1")]
        public event EventHandler<EventArgs> Subject1Event;

        [InstrumentationProvider("Subject2")]
        public event EventHandler<EventArgs> Subject2Event;
        
        public void Raise()
        {
            Subject1Event(this, new EventArgs());
            Subject2Event(this, new EventArgs());
        }
    }

    internal class BaseEventSource
    {
        [InstrumentationProvider("Single")]
        public event EventHandler<EventArgs> BaseEvent;

        public void Raise()
        {
            BaseEvent(this, new EventArgs());
        }
    }

    internal class DerivedEventSource : BaseEventSource
    {
    }
    
    internal class SingleEventListener
    {
        public bool eventWasRaised = false;

        [InstrumentationConsumer("Single")]
        public virtual void TestHandler(object sender, EventArgs e)
        {
            eventWasRaised = true;
        }
    }

    internal class TwoEventListener
    {
        public string methodCalled = "";

        [InstrumentationConsumer("UnknownListener")]
        public void DoNotCallThis(object sender, EventArgs e)
        {
            methodCalled = "DoNotCallThis";
        }

        [InstrumentationConsumer("Single")]
        public void CallThis(object sender, EventArgs e)
        {
            methodCalled = "CallThis";
        }
    }

    internal class CountingEventListener
    {
        public int count = 0;

        [InstrumentationConsumer("foo")]
        public void FooHandler(object sender, EventArgs e)
        {
            count++;
        }
    }
    
    internal class DualAttributedListener
    {
        public int count = 0; 
        
        [InstrumentationConsumer("Subject1")]
        [InstrumentationConsumer("Subject2")]
        public void Handler(object sender, EventArgs e)
        {
            count++;
        }   
    }

    internal class EmptyEventListener
    {
    }
}
