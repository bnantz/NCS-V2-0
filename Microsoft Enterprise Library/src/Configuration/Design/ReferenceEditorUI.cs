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
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// The UI for the reference type selector.
    /// </devdoc>    
    internal class ReferenceEditorUI : UserControl
    {
        private Container components = null;
        private ConfigurationNode rootNode;
        private Type referenceType;
        private ConfigurationNode currentNode;
        private IWindowsFormsEditorService service;
        private ListBox referencesListBox;
        private string noneText;
		private bool isRequired;

        public ReferenceEditorUI()
        {
            InitializeComponent();
			noneText = Resources.None;
            referencesListBox.Click += new EventHandler(OnListBoxClick);
        }

		public ReferenceEditorUI(ConfigurationNode rootNode, Type referenceType, ConfigurationNode currentReference, IWindowsFormsEditorService service, bool isRequired)
			: this()
        {
            this.rootNode = rootNode;
            this.referenceType = referenceType;
            this.currentNode = currentReference;
            this.service = service;
			this.isRequired = isRequired;
            DisplayList();
        }

        public ConfigurationNode SelectedNode
        {
            get
            {
				ConfigurationNode selectedNode = referencesListBox.SelectedItem as ConfigurationNode;
                if (selectedNode == null)
                {
                    return null;
                }
                return selectedNode;
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void OnListBoxClick(object sender, EventArgs args)
        {
            service.CloseDropDown();
        }

        private void DisplayList()
        {			
            referencesListBox.Items.Clear();
			if (!isRequired)
			{
				referencesListBox.Items.Add(noneText);
			}		
			List<ConfigurationNode> nodes = new List<ConfigurationNode>(rootNode.Hierarchy.FindNodesByType(referenceType));
            if (nodes != null)
            {
				nodes.Sort();                
                foreach (ConfigurationNode node in nodes)
                {
                    referencesListBox.Items.Add(node);
                    if ((currentNode != null) && (currentNode == node))
                    {
                        referencesListBox.SelectedItem = node;
                    }
                }
            }

        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager resources = new ResourceManager(typeof(ReferenceEditorUI));
            this.referencesListBox = new ListBox();
            this.SuspendLayout();
            // 
            // referencesListBox
            // 
            this.referencesListBox.AccessibleDescription = resources.GetString("referencesListBox.AccessibleDescription");
            this.referencesListBox.AccessibleName = resources.GetString("referencesListBox.AccessibleName");
            this.referencesListBox.Anchor = ((AnchorStyles)(resources.GetObject("referencesListBox.Anchor")));
            this.referencesListBox.BackgroundImage = ((Image)(resources.GetObject("referencesListBox.BackgroundImage")));
            this.referencesListBox.ColumnWidth = ((int)(resources.GetObject("referencesListBox.ColumnWidth")));
            this.referencesListBox.Dock = ((DockStyle)(resources.GetObject("referencesListBox.Dock")));
            this.referencesListBox.Enabled = ((bool)(resources.GetObject("referencesListBox.Enabled")));
            this.referencesListBox.Font = ((Font)(resources.GetObject("referencesListBox.Font")));
            this.referencesListBox.HorizontalExtent = ((int)(resources.GetObject("referencesListBox.HorizontalExtent")));
            this.referencesListBox.HorizontalScrollbar = ((bool)(resources.GetObject("referencesListBox.HorizontalScrollbar")));
            this.referencesListBox.ImeMode = ((ImeMode)(resources.GetObject("referencesListBox.ImeMode")));
            this.referencesListBox.IntegralHeight = ((bool)(resources.GetObject("referencesListBox.IntegralHeight")));
            this.referencesListBox.ItemHeight = ((int)(resources.GetObject("referencesListBox.ItemHeight")));
            this.referencesListBox.Location = ((Point)(resources.GetObject("referencesListBox.Location")));
            this.referencesListBox.Name = "referencesListBox";
            this.referencesListBox.RightToLeft = ((RightToLeft)(resources.GetObject("referencesListBox.RightToLeft")));
            this.referencesListBox.ScrollAlwaysVisible = ((bool)(resources.GetObject("referencesListBox.ScrollAlwaysVisible")));
            this.referencesListBox.Size = ((Size)(resources.GetObject("referencesListBox.Size")));
            this.referencesListBox.TabIndex = ((int)(resources.GetObject("referencesListBox.TabIndex")));
            this.referencesListBox.Visible = ((bool)(resources.GetObject("referencesListBox.Visible")));
            // 
            // ReferenceEditorUI
            // 
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.referencesListBox);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((Font)(resources.GetObject("$this.Font")));
            this.ImeMode = ((ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((Point)(resources.GetObject("$this.Location")));
            this.Name = "ReferenceEditorUI";
            this.RightToLeft = ((RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.Size = ((Size)(resources.GetObject("$this.Size")));
            this.ResumeLayout(false);

        }
        #endregion
    }
}