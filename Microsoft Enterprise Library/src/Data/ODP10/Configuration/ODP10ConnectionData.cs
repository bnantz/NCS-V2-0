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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration
{
	/// <summary>
	/// Oracle-specific connection information.
	/// </summary>
	public class ODP10ConnectionData : NamedConfigurationElement
	{
		private const string packagesProperty = "packages";

		/// <summary>
		/// Initializes a new instance of the <see cref="ODP10ConnectionData"/> class with default values.
		/// </summary>
        public ODP10ConnectionData()
		{
		}

		/// <summary>
        /// Gets a collection of <see cref="ODP10PackageData"/> objects.
		/// </summary>
		/// <value>
        /// A collection of <see cref="ODP10PackageData"/> objects.
		/// </value>
		[ConfigurationProperty(packagesProperty, IsRequired = true)]
        public NamedElementCollection<ODP10PackageData> Packages
		{
			get
			{
                return (NamedElementCollection<ODP10PackageData>)base[packagesProperty];
			}
		}
	}
}
