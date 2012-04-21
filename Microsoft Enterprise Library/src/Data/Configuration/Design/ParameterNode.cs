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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// Represents a parameter for a connection string.
    /// </summary>
    [Image(typeof(ParameterNode))]
    public sealed class ParameterNode : ConfigurationNode
    {        
		private string parameterValue;

		/// <overloads>
		/// Initialize a new instance of the <see cref="ParameterNode"/> class.
		/// </overloads>
 		/// <summary>
		/// Initialize a new instance of the <see cref="ParameterNode"/> class.
		/// </summary>
       public ParameterNode() : this(Resources.DefaultParameterNodeName, string.Empty)
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="ParameterNode"/> class with a name and a parameter value.
		/// </summary>
		/// <param name="name">The name of the parameter.</param>
		/// <param name="parameterValue">The parameter value.</param>
        public ParameterNode(string name, string parameterValue) : base(name)
        {			
            this.parameterValue = parameterValue;
        }

		/// <summary>
		/// Gets or sets the parameter value.
		/// </summary>        
		/// <value>
		/// The parameter value.
		/// </value>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("ParameterValueDescription", typeof(Resources))]
        public string Value
        {
            get { return parameterValue; }
            set { parameterValue = value; }
        }
    }
}