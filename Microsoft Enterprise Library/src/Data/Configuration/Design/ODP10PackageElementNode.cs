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

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.ODP10.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Represents a <see cref="ODP10PackageData"/> configuration element.
	/// </summary>
    [Image(typeof(ODP10PackageElementNode))]
	public sealed class ODP10PackageElementNode : ConfigurationNode
	{
		private string prefix;

		/// <summary>
		/// Initialize a new instance of the <see cref="OraclePackageElementNode"/> class.
		/// </summary>
		public ODP10PackageElementNode()
			: this(new ODP10PackageData(Resources.OracleConnectionElementNodeName, string.Empty))
		{
		}


		/// <summary>
        /// Initialize a new instance of the <see cref="ODP10PackageElementNode"/> class with a <see cref="ODP10PackageData"/> instance.
		/// </summary>
        /// <param name="odp10PackageElement">A <see cref="ODP10PackageData"/> instance.</param>
        public ODP10PackageElementNode(ODP10PackageData odp10PackageElement)
			: base(null == odp10PackageElement ? string.Empty : odp10PackageElement.Name)
		{
			if (null == odp10PackageElement) throw new ArgumentNullException("odp10PackageElement");
			this.prefix = odp10PackageElement.Prefix;
		}

		/// <summary>
		/// Gets or sets the prefix for the oracle package.
		/// </summary>
		/// <value>
		/// The prefix for the oracle package.
		/// </value>
		[Required]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("PrefixDescription", typeof(Resources))]
		public string Prefix
		{
			get { return prefix; }
			set { prefix = value; }
		}

		/// <summary>
        /// Gets the <see cref="ODP10PackageData"/> this node represents.
		/// </summary>
		/// <value>
        /// The <see cref="ODP10PackageData"/> this node represents.
		/// </value>
		[Browsable(false)]
		public ODP10PackageData ODP10PackageElement
		{
			get { return new ODP10PackageData(Name, prefix); }
		}
	}
}