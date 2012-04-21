//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	/// <summary>
	/// Represents a design time representation of a <see cref="ReplaceHandlerData"/> configuration element.
	/// </summary>
    public sealed class ReplaceHandlerNode : ExceptionHandlerNode
    {
		private string message;
		private Type type;		

		/// <summary>
		/// Initialize a new instance of the <see cref="ReplaceHandlerNode"/> class.
		/// </summary>
        public ReplaceHandlerNode()
            : this(new ReplaceHandlerData(Resources.DefaultReplaceHandlerNodeName, string.Empty, null))
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="ReplaceHandlerNode"/> class with a <see cref="ReplaceHandlerData"/> instance.
        /// </summary>
		/// <param name="replaceHandlerData">A <see cref="ReplaceHandlerData"/> instance</param>
        public ReplaceHandlerNode(ReplaceHandlerData replaceHandlerData) 
        {
			if (null == replaceHandlerData) throw new ArgumentNullException("replaceHandlerData");
			Rename(replaceHandlerData.Name);
			this.message = replaceHandlerData.ExceptionMessage;
			this.type = replaceHandlerData.ReplaceExceptionType;
        }

        /// <summary>
        /// Gets or sets the exception message to use.
        /// </summary>
		/// <value>
		/// The exception message to use.
		/// </value>
        [SRDescription("ExceptionReplaceMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ExceptionMessage
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of <see cref="Exception"/> to use for replacement.
        /// </summary>
		/// <value>
		/// The <see cref="Type"/> of <see cref="Exception"/> to use for replacement.
		/// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType)]
        [SRDescription("ExceptionReplaceTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public Type ReplaceExceptionType
        {
            get { return type; }
            set { type = value; }
        }

		/// <summary>
		/// Gets the <see cref="ReplaceHandlerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="ReplaceHandlerData"/> this node represents.
		/// </value>
		public override ExceptionHandlerData ExceptionHandlerData
		{
			get { return new ReplaceHandlerData(Name, message, type); }
		}
    }
}