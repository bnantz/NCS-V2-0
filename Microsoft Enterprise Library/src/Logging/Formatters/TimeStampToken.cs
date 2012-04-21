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

using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    /// <summary>
    /// Formats a timestamp token with a custom date time format string.
    /// </summary>
    public class TimeStampToken : TokenFunction
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="TimeStampToken"/>.
        /// </summary>
        public TimeStampToken() : base("{timestamp(")
        {
        }

        /// <summary>
        /// Formats the timestamp property with the specified date time format string.
        /// </summary>
        /// <param name="tokenTemplate">Date time format string.</param>
        /// <param name="log">Log entry containing the timestamp.</param>
        /// <returns>Returns the formatted time stamp.</returns>
        public override string FormatToken(string tokenTemplate, LogEntry log)
        {
            return log.TimeStamp.ToString(tokenTemplate, CultureInfo.CurrentCulture);
        }
    }
}