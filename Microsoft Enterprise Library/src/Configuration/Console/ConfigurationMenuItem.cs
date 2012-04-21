//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    [System.ComponentModel.DesignerCategory("Code")]
    class ConfigurationMenuItem : MenuItem
    {        
		private ConfigurationUICommand command;
		private ConfigurationNode node;

        private ConfigurationMenuItem()
        {
        }
        
        public ConfigurationMenuItem(ConfigurationNode node, ConfigurationUICommand command)
        {
			Debug.Assert(node != null);
            this.command = command;            
			Shortcut = command.Shortcut;
			Text = command.Text;
			this.node = node;
            this.Enabled = (command.GetCommandState(node) == CommandState.Enabled);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (command != null)
                {
                    command.Dispose();
                }
            }
            base.Dispose(disposing);
        }		
       
        public string LongText
        {
            get { return command.LongText; }            
        }

        public InsertionPoint InsertionPoint
        {
            get { return command.InsertionPoint; }            
        } 
        

        public override MenuItem CloneMenu()
        {
            ConfigurationMenuItem item = new ConfigurationMenuItem();
            item.CloneMenu(this);
			item.command = command;
			item.node = node;
            return item;
        }

        protected override void OnClick(EventArgs e)
        {
            using (new WaitCursor())
            {
                base.OnClick(e);
                command.Execute(node);
            }
        }

    }
}