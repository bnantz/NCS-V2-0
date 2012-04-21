//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Used by <see cref="LogSource"/>s to keep the knowledge of which <see cref="TraceListeners"/> have been 
	/// already traced through. 
	/// </summary>
	public class TraceListenerFilter
	{
		private Dictionary<TraceListener, object> viewedTraceListeners;

		/// <summary>
		/// Initializes a new instance of the <see cref="TraceListenerFilter"/> class.
		/// </summary>
		public TraceListenerFilter()
		{
			viewedTraceListeners = new Dictionary<TraceListener, object>();
		}

		internal IEnumerable<TraceListener> GetAvailableTraceListeners(IList traceListeners)
		{
			IList<TraceListener> filteredTraceListeners = new List<TraceListener>(traceListeners.Count);
			foreach (TraceListener listener in traceListeners)
			{
				if (!viewedTraceListeners.ContainsKey(listener))
				{
					viewedTraceListeners.Add(listener, listener);
					filteredTraceListeners.Add(listener);
				}
			}

			return filteredTraceListeners;
		}
	}
}

