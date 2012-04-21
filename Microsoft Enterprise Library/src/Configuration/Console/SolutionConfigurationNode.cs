//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright � Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright � Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    [Image(typeof(SolutionConfigurationNode))]
    class SolutionConfigurationNode : ConfigurationNode
    {
        public SolutionConfigurationNode() : base(Resources.DefaultSolutionNodeName)
        {
        }

        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return base.Name;
            }            
        }
    }
}
