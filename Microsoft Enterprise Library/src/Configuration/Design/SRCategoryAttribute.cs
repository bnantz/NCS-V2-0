//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a localized <see cref="CategoryAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SRCategoryAttribute : CategoryAttribute
    {
		private readonly Type resourceType;

		/// <summary>
		/// Initialize a new instance of the <see cref="SRCategoryAttribute"/> class with the <see cref="Type"/> containing the resources and the resource name.
		/// </summary>
		/// <param name="category">The resources string name.</param>
		/// <param name="resourceType">The <see cref="Type"/> containing the resource strings.</param>
        public SRCategoryAttribute(string category, Type resourceType) : base(category)
        {
            this.resourceType = resourceType;
        }

		/// <summary>
		/// Gets the type that contains the resources.
		/// </summary>
		/// <value>
		/// The type that contains the resources.
		/// </value>
		public Type ResourceType
		{
			get { return resourceType; }
		}

		/// <summary>
		/// Gets the localized string based on the key.
		/// </summary>
		/// <param name="value">The key to the string resources.</param>
		/// <returns>The localized string.</returns>
        protected override string GetLocalizedString(string value)
        {
            return ResourceStringLoader.LoadString(resourceType.FullName, value, resourceType.Assembly);
        }
    }
}