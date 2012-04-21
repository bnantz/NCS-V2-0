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
using System.Management;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests
{
    public class WmiEventWatcher : IDisposable
    {
        private ManagementEventWatcher eventWatcher;
        private int numberOfEventsToWatchFor;
        private List<ManagementBaseObject> eventsReceived = new List<ManagementBaseObject>();
		private object eventsCollectionLock = new object();

        public WmiEventWatcher(int numberOfEventsToWatchFor)
        {
            this.numberOfEventsToWatchFor = numberOfEventsToWatchFor;

            WqlEventQuery eventQuery = new WqlEventQuery("BaseWmiEvent");
            ManagementScope scope = new ManagementScope(@"\\.\root\EnterpriseLibrary");

            eventWatcher = new ManagementEventWatcher(scope, eventQuery);
            eventWatcher.EventArrived += new EventArrivedEventHandler(delegate_EventArrived);

            eventWatcher.Start();
        }

        public List<ManagementBaseObject> EventsReceived { get { return eventsReceived; } }

        public void WaitForEvents()
        {
            for (int i = 0; i < numberOfEventsToWatchFor * 2; i++)
            {
                Thread.Sleep(50);
				lock (eventsCollectionLock)
				{
					if (eventsReceived.Count == numberOfEventsToWatchFor) break;
				}
            }
        }

        public void delegate_EventArrived(object sender, EventArrivedEventArgs e)
        {
			lock (eventsCollectionLock)
			{
				eventsReceived.Add(e.NewEvent);
			}
        }

        public void Dispose()
        {
            eventWatcher.Stop();
            eventWatcher.Dispose();
		}
    }
}
