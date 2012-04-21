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
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Provides methods to maintain a key/value dictionary that is stored in the <see cref="CallContext"/>.
    /// </summary>
    /// <remarks>
    /// A context item represents a key/value that needs to be logged with each message
    /// on the same CallContext.
    /// </remarks>
	public class ContextItems
    {
		/// <summary>
        /// The name of the data slot in the <see cref="CallContext"/> used by the application block.
		/// </summary>
        public const string CallContextSlotName = "EntLibLoggerContextItems";

        /// <summary>
        /// Creates a new instance of a <see cref="ContextItems"/> class.
        /// </summary>
        public ContextItems()
        {
        }

        /// <summary>
        /// Adds a key/value pair to a dictionary in the <see cref="CallContext"/>.  
        /// Each context item is recorded with every log entry.
        /// </summary>
        /// <param name="key">Hashtable key.</param>
        /// <param name="value">Value of the context item.  Byte arrays will be base64 encoded.</param>
        /// <example>The following example demonstrates use of the AddContextItem method.
        /// <code>Logger.SetContextItem("SessionID", myComponent.SessionId);</code></example>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]        
        public void SetContextItem(object key, object value)
        {
            Hashtable contextItems = (Hashtable)CallContext.GetData(CallContextSlotName);
            if (contextItems == null)
            {
                contextItems = new Hashtable();
            }

            contextItems[key] = value;

            CallContext.SetData(CallContextSlotName, contextItems);
        }

        /// <summary>
        /// Empties the context items dictionary.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public void FlushContextItems()
        {
            CallContext.FreeNamedDataSlot(CallContextSlotName);
        }

        /// <summary>
        /// Merges each key/value pair from the context items dictionary with the ExtendedProperties
        /// dictionary of the <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="log"><see cref="LogEntry"/> object that is being logged.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public void ProcessContextItems(LogEntry log)
        {
            Hashtable contextItems = (Hashtable)CallContext.GetData(CallContextSlotName);
            if (contextItems == null || contextItems.Count == 0)
            {
                return;
            }

            foreach (DictionaryEntry entry in contextItems)
            {
                string itemValue = GetContextItemValue(entry.Value);
                log.ExtendedProperties.Add(entry.Key.ToString(), itemValue);
            }
        }

        private string GetContextItemValue(object contextData)
        {
            string value = string.Empty;
            try
            {
                // convert to base 64 string if data type is byte array
                if (contextData.GetType() == typeof(byte[]))
                {
                    value = Convert.ToBase64String((byte[])contextData);
                }
                else
                {
                    value = contextData.ToString();
                }
            }
            catch
            { /* ignore exceptions */
            }

            return value;
        }
    }
}