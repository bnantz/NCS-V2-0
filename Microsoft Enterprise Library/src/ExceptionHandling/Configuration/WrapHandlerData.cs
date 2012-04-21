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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration data for a <see cref="WrapHandler"/>.
    /// </summary>	
	[Assembler(typeof(WrapHandlerAssembler))]
	public class WrapHandlerData : ExceptionHandlerData
    {
		private const string exceptionMessageProperty = "exceptionMessage";
		private const string wrapExceptionTypeProperty = "wrapExceptionType";

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapHandlerData"/> class.
        /// </summary>
        public WrapHandlerData()
        {
        }		

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapHandlerData"/> class with a name, an exception message, and the fully qualified assembly name of the type of the wrapping exception.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="WrapHandlerData"/>.
        /// </param>
        /// <param name="exceptionMessage">
        /// The exception message replacement.
        /// </param>
        /// <param name="wrapExceptionType">
        /// The fully qualified assembly name of type of the wrapping exception
        /// </param>
        public WrapHandlerData(string name, string exceptionMessage, Type wrapExceptionType) : base(name, typeof(WrapHandler))
		{
			this.ExceptionMessage = exceptionMessage;
			this.WrapExceptionType = wrapExceptionType;			
        }

        /// <summary>
        /// Gets or sets the message for the replacement exception.
        /// </summary>
		[ConfigurationProperty(exceptionMessageProperty, IsRequired= true)]		
		public string ExceptionMessage
		{
			get { return (string)this[exceptionMessageProperty]; }
			set { this[exceptionMessageProperty] = value; }
		}

        /// <summary>
        /// Gets or sets the fully qualified type name of the replacement exception.
        /// </summary>
		[ConfigurationProperty(wrapExceptionTypeProperty, IsRequired= true)]
		[TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
		[SubclassTypeValidator(typeof(Exception))]
		public Type WrapExceptionType
		{
			get { return (Type)this[wrapExceptionTypeProperty]; }
			set { this[wrapExceptionTypeProperty] = value; }
		}		
	}

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="WrapHandler"/> described by a <see cref="WrapHandlerData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="WrapHandlerData"/> type and it is used by the <see cref="ExceptionHandlerCustomFactory"/> 
    /// to build the specific <see cref="IExceptionHandler"/> object represented by the configuration object.
    /// </remarks>
    public class WrapHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
	{
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="WrapHandler"/> based on an instance of <see cref="WrapHandlerData"/>.
        /// </summary>
        /// <seealso cref="ExceptionHandlerCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="WrapHandlerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="WrapHandler"/>.</returns>
        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			WrapHandlerData castedObjectConfiguration
				= (WrapHandlerData)objectConfiguration;

			WrapHandler createdObject
				= new WrapHandler(castedObjectConfiguration.ExceptionMessage, castedObjectConfiguration.WrapExceptionType);

			return createdObject;
		}
	}
}