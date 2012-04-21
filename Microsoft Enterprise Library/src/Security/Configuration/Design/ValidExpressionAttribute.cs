//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited=true)]
    sealed class ValidExpressionAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public ValidExpressionAttribute()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="errors"></param>
        protected override void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
            string expression = (string)propertyInfo.GetValue(instance, null);
            if (expression != null
                && expression.Length != 0)
            {
                try
                {
                    Parser parser = new Parser();
                    parser.Parse(expression);
                }
                catch (SyntaxException e)
                {
                    errors.Add(new ValidationError(instance, propertyInfo.Name, e.Message));
                }
            }
        }
    }
}