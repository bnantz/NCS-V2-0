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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	sealed class ODP10ConnectionCommandRegistrar : CommandRegistrar
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
        public ODP10ConnectionCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}		
		
		/// <summary>
		/// 
		/// </summary>
		public override void Register()
		{
            AddODP10ConnectionElementCommand();
			AddDefaultCommands(typeof(ODP10ConnectionElementNode));
            AddODP10PacakgeElementCommand();
            AddDefaultCommands(typeof(ODP10PackageElementNode));
		}

        private void AddODP10ConnectionElementCommand()
		{
			ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
				Resources.OracleConnectionUICommandText,
				Resources.OracleConnectionUICommandLongText,
                new AddODP10ConnectionElementNodeCommand(ServiceProvider),
                typeof(ODP10ConnectionElementNode));
			AddUICommand(item, typeof(ConnectionStringSettingsNode));			
		}

        private void AddODP10PacakgeElementCommand()
		{
            AddMultipleChildNodeCommand(Resources.ODP10PackageUICommandText,
				Resources.OraclePackageUICommandLongText,
                typeof(ODP10PackageElementNode),
                typeof(ODP10ConnectionElementNode));
		}   
	}
}
