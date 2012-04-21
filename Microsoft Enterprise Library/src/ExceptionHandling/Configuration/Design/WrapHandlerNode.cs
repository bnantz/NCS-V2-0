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
	/// Represents a design time representation of a <see cref="WrapHandlerData"/> configuration element.
	/// </summary>
    public sealed class WrapHandlerNode : ExceptionHandlerNode
    {        
		private string message;
		private Type type;

		/// <summary>
		/// Initialize a new instance of the <see cref="WrapHandlerNode"/> class.
		/// </summary>
        public WrapHandlerNode()
            : this(new WrapHandlerData(Resources.DefaultWrapHandlerNodeName, string.Empty, null))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="WrapHandlerNode"/> class with a <see cref="WrapHandlerData"/> instance.
		/// </summary>
		/// <param name="wrapHandlerData">A	<see cref="WrapHandlerData"/> instance</param>
        public WrapHandlerNode(WrapHandlerData wrapHandlerData) 
        {
			if (null == wrapHandlerData) throw new ArgumentNullException("wrapHandlerData");

			Rename(wrapHandlerData.Name);
			this.message = wrapHandlerData.ExceptionMessage;
			this.type = wrapHandlerData.WrapExceptionType;
        }

		/// <summary>
		/// Gets or sets the exception message to use.
		/// </summary>
		/// <value>
		/// The exception message to use.
		/// </value>
        [SRDescription("WrapHandlerNodeMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ExceptionMessage
        {
			get { return message; ; }
            set { message = value; }
        }

		/// <summary>
		/// Gets or sets the <see cref="Type"/> of <see cref="Exception"/> to use for wrapping.
		/// </summary>
		/// <value>
		/// The <see cref="Type"/> of <see cref="Exception"/> to use for wrapping.
		/// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Exception), TypeSelectorIncludes.BaseType)]
        [SRDescription("ExceptionWrapTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public Type WrapExceptionType
        {
            get { return type; }
            set { type = value; }
        }

		/// <summary>
		/// Gets the <see cref="WrapHandlerData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="WrapHandlerData"/> this node represents.
		/// </value>
		public override ExceptionHandlerData ExceptionHandlerData
		{
			get { return new WrapHandlerData(Name, message, type); }
		}
    }
}