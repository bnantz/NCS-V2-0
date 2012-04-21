//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Oracle.DataAccess.Client;

namespace Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration
{
    /// <summary>
    /// <para>Represents the package information to use when calling a stored procedure for Oracle.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// A package name can be appended to the stored procedure name of a command if the prefix of the stored procedure
    /// matchs the prefix defined. This allows the caller of the stored procedure to use stored procedures
    /// in a more database independent fashion.
    /// </para>
    /// </remarks>
    public class ODP10PackageData : NamedConfigurationElement, IODP10Package
    {
		private const string prefixProperty = "prefix";

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ODP10PackageData"/> class.</para>
        /// </summary>
        public ODP10PackageData() : base()
        {
            this.Prefix = string.Empty;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ODP10PackageData"/> class, given the prefix to search for and the name of the package.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the package to append to any found procedure that has the <paramref name="prefix"/>.</para>
        /// </param>
        /// <param name="prefix">
        /// <para>The prefix of the stored procedures used in this package.</para>
        /// </param>
        public ODP10PackageData(string name, string prefix)
            : base(name)
        {
            this.Prefix = prefix;
        }

        /// <summary>
        /// <para>Gets or sets the prefix of the stored procedures that are in the package in Oracle.</para>
        /// </summary>
        /// <value>
        /// <para>The prefix of the stored procedures that are in the package in Oracle.</para>
        /// </value>
		[ConfigurationProperty(prefixProperty, IsRequired= true)]
		public string Prefix
		{
			get
			{
				return (string)this[prefixProperty];
			}
			set
			{
				this[prefixProperty] = value;
			}
		}
    }
}