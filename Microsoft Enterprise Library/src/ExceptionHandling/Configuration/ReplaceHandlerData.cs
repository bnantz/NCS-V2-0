//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
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
    /// Represents the configuration data for a <see cref="ReplaceHandler"/>.
    /// </summary>		
	[Assembler(typeof(ReplaceHandlerAssembler))]
	public class ReplaceHandlerData : ExceptionHandlerData
    {
		private const string exceptionMessageProperty = "exceptionMessage";
		private const string replaceExceptionTypeProperty = "replaceExceptionType";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceHandlerData"/> class.
        /// </summary>
		public ReplaceHandlerData() 
        {
        }		

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceHandlerData"/> class with a name, exception message, and replace exception type name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="ReplaceHandlerData"/>.
        /// </param>
        /// <param name="exceptionMessage">
        /// The exception message replacement.
        /// </param>
        /// <param name="replaceExceptionType">
        /// The fully qualified assembly name the type of the replacement exception.
        /// </param>
        public ReplaceHandlerData(string name, string exceptionMessage, Type replaceExceptionType) 
			: base(name, typeof(ReplaceHandler))
        {
            this.ExceptionMessage = exceptionMessage;
            this.ReplaceExceptionType = replaceExceptionType;			
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
		[ConfigurationProperty(replaceExceptionTypeProperty, IsRequired= true)]
		[TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
		[SubclassTypeValidator(typeof(Exception))]
		public Type ReplaceExceptionType
        {
			get { return (Type)this[replaceExceptionTypeProperty]; }
			set { this[replaceExceptionTypeProperty] = value; }
        }				
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="ReplaceHandler"/> described by a <see cref="ReplaceHandlerData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="ReplaceHandlerData"/> type and it is used by the <see cref="ExceptionHandlerCustomFactory"/> 
    /// to build the specific <see cref="IExceptionHandler"/> object represented by the configuration object.
    /// </remarks>
    public class ReplaceHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
	{
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="ReplaceHandler"/> based on an instance of <see cref="ReplaceHandlerData"/>.
        /// </summary>
        /// <seealso cref="ExceptionHandlerCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="ReplaceHandlerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="ReplaceHandler"/>.</returns>
        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ReplaceHandlerData castedObjectConfiguration
				= (ReplaceHandlerData)objectConfiguration;

			ReplaceHandler createdObject
				= new ReplaceHandler(castedObjectConfiguration.ExceptionMessage, castedObjectConfiguration.ReplaceExceptionType);

			return createdObject;
		}
	}
}