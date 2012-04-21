//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block QuickStart
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Web.Security;

namespace SecurityQuickStart
{
	/// <summary>
	/// Summary description for UserRoleForm.
	/// </summary>
	public class AddUserRoleForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox usersComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox rolesComboBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string userName;
		private string role;

		public AddUserRoleForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			ResetDataControls();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AddUserRoleForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.usersComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rolesComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = resources.GetString("groupBox1.AccessibleDescription");
            this.groupBox1.AccessibleName = resources.GetString("groupBox1.AccessibleName");
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("groupBox1.Anchor")));
            this.groupBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groupBox1.BackgroundImage")));
            this.groupBox1.Controls.Add(this.cancelButton);
            this.groupBox1.Controls.Add(this.okButton);
            this.groupBox1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("groupBox1.Dock")));
            this.groupBox1.Enabled = ((bool)(resources.GetObject("groupBox1.Enabled")));
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = ((System.Drawing.Font)(resources.GetObject("groupBox1.Font")));
            this.groupBox1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("groupBox1.ImeMode")));
            this.groupBox1.Location = ((System.Drawing.Point)(resources.GetObject("groupBox1.Location")));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("groupBox1.RightToLeft")));
            this.groupBox1.Size = ((System.Drawing.Size)(resources.GetObject("groupBox1.Size")));
            this.groupBox1.TabIndex = ((int)(resources.GetObject("groupBox1.TabIndex")));
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = resources.GetString("groupBox1.Text");
            this.groupBox1.Visible = ((bool)(resources.GetObject("groupBox1.Visible")));
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = resources.GetString("cancelButton.AccessibleDescription");
            this.cancelButton.AccessibleName = resources.GetString("cancelButton.AccessibleName");
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cancelButton.Anchor")));
            this.cancelButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cancelButton.BackgroundImage")));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cancelButton.Dock")));
            this.cancelButton.Enabled = ((bool)(resources.GetObject("cancelButton.Enabled")));
            this.cancelButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("cancelButton.FlatStyle")));
            this.cancelButton.Font = ((System.Drawing.Font)(resources.GetObject("cancelButton.Font")));
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.ImageAlign")));
            this.cancelButton.ImageIndex = ((int)(resources.GetObject("cancelButton.ImageIndex")));
            this.cancelButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cancelButton.ImeMode")));
            this.cancelButton.Location = ((System.Drawing.Point)(resources.GetObject("cancelButton.Location")));
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cancelButton.RightToLeft")));
            this.cancelButton.Size = ((System.Drawing.Size)(resources.GetObject("cancelButton.Size")));
            this.cancelButton.TabIndex = ((int)(resources.GetObject("cancelButton.TabIndex")));
            this.cancelButton.Text = resources.GetString("cancelButton.Text");
            this.cancelButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("cancelButton.TextAlign")));
            this.cancelButton.Visible = ((bool)(resources.GetObject("cancelButton.Visible")));
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = resources.GetString("okButton.AccessibleDescription");
            this.okButton.AccessibleName = resources.GetString("okButton.AccessibleName");
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("okButton.Anchor")));
            this.okButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("okButton.BackgroundImage")));
            this.okButton.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("okButton.Dock")));
            this.okButton.Enabled = ((bool)(resources.GetObject("okButton.Enabled")));
            this.okButton.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("okButton.FlatStyle")));
            this.okButton.Font = ((System.Drawing.Font)(resources.GetObject("okButton.Font")));
            this.okButton.Image = ((System.Drawing.Image)(resources.GetObject("okButton.Image")));
            this.okButton.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.ImageAlign")));
            this.okButton.ImageIndex = ((int)(resources.GetObject("okButton.ImageIndex")));
            this.okButton.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("okButton.ImeMode")));
            this.okButton.Location = ((System.Drawing.Point)(resources.GetObject("okButton.Location")));
            this.okButton.Name = "okButton";
            this.okButton.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("okButton.RightToLeft")));
            this.okButton.Size = ((System.Drawing.Size)(resources.GetObject("okButton.Size")));
            this.okButton.TabIndex = ((int)(resources.GetObject("okButton.TabIndex")));
            this.okButton.Text = resources.GetString("okButton.Text");
            this.okButton.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("okButton.TextAlign")));
            this.okButton.Visible = ((bool)(resources.GetObject("okButton.Visible")));
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = resources.GetString("label1.AccessibleDescription");
            this.label1.AccessibleName = resources.GetString("label1.AccessibleName");
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
            this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
            this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
            this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
            this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
            this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
            this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
            this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
            this.label1.Name = "label1";
            this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
            this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
            this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
            this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
            // 
            // usersComboBox
            // 
            this.usersComboBox.AccessibleDescription = resources.GetString("usersComboBox.AccessibleDescription");
            this.usersComboBox.AccessibleName = resources.GetString("usersComboBox.AccessibleName");
            this.usersComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("usersComboBox.Anchor")));
            this.usersComboBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("usersComboBox.BackgroundImage")));
            this.usersComboBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("usersComboBox.Dock")));
            this.usersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.usersComboBox.Enabled = ((bool)(resources.GetObject("usersComboBox.Enabled")));
            this.usersComboBox.Font = ((System.Drawing.Font)(resources.GetObject("usersComboBox.Font")));
            this.usersComboBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("usersComboBox.ImeMode")));
            this.usersComboBox.IntegralHeight = ((bool)(resources.GetObject("usersComboBox.IntegralHeight")));
            this.usersComboBox.ItemHeight = ((int)(resources.GetObject("usersComboBox.ItemHeight")));
            this.usersComboBox.Location = ((System.Drawing.Point)(resources.GetObject("usersComboBox.Location")));
            this.usersComboBox.MaxDropDownItems = ((int)(resources.GetObject("usersComboBox.MaxDropDownItems")));
            this.usersComboBox.MaxLength = ((int)(resources.GetObject("usersComboBox.MaxLength")));
            this.usersComboBox.Name = "usersComboBox";
            this.usersComboBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("usersComboBox.RightToLeft")));
            this.usersComboBox.Size = ((System.Drawing.Size)(resources.GetObject("usersComboBox.Size")));
            this.usersComboBox.TabIndex = ((int)(resources.GetObject("usersComboBox.TabIndex")));
            this.usersComboBox.Text = resources.GetString("usersComboBox.Text");
            this.usersComboBox.Visible = ((bool)(resources.GetObject("usersComboBox.Visible")));
            // 
            // label2
            // 
            this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
            this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
            this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
            this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
            this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
            this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
            this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
            this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
            this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
            this.label2.Name = "label2";
            this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
            this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
            this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
            this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
            // 
            // rolesComboBox
            // 
            this.rolesComboBox.AccessibleDescription = resources.GetString("rolesComboBox.AccessibleDescription");
            this.rolesComboBox.AccessibleName = resources.GetString("rolesComboBox.AccessibleName");
            this.rolesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("rolesComboBox.Anchor")));
            this.rolesComboBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rolesComboBox.BackgroundImage")));
            this.rolesComboBox.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("rolesComboBox.Dock")));
            this.rolesComboBox.Enabled = ((bool)(resources.GetObject("rolesComboBox.Enabled")));
            this.rolesComboBox.Font = ((System.Drawing.Font)(resources.GetObject("rolesComboBox.Font")));
            this.rolesComboBox.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("rolesComboBox.ImeMode")));
            this.rolesComboBox.IntegralHeight = ((bool)(resources.GetObject("rolesComboBox.IntegralHeight")));
            this.rolesComboBox.ItemHeight = ((int)(resources.GetObject("rolesComboBox.ItemHeight")));
            this.rolesComboBox.Location = ((System.Drawing.Point)(resources.GetObject("rolesComboBox.Location")));
            this.rolesComboBox.MaxDropDownItems = ((int)(resources.GetObject("rolesComboBox.MaxDropDownItems")));
            this.rolesComboBox.MaxLength = ((int)(resources.GetObject("rolesComboBox.MaxLength")));
            this.rolesComboBox.Name = "rolesComboBox";
            this.rolesComboBox.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("rolesComboBox.RightToLeft")));
            this.rolesComboBox.Size = ((System.Drawing.Size)(resources.GetObject("rolesComboBox.Size")));
            this.rolesComboBox.TabIndex = ((int)(resources.GetObject("rolesComboBox.TabIndex")));
            this.rolesComboBox.Text = resources.GetString("rolesComboBox.Text");
            this.rolesComboBox.Visible = ((bool)(resources.GetObject("rolesComboBox.Visible")));
            // 
            // AddUserRoleForm
            // 
            this.AcceptButton = this.okButton;
            this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
            this.AccessibleName = resources.GetString("$this.AccessibleName");
            this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
            this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
            this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
            this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.Add(this.rolesComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.usersComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
            this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
            this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
            this.MaximizeBox = false;
            this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
            this.MinimizeBox = false;
            this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
            this.Name = "AddUserRoleForm";
            this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
            this.ShowInTaskbar = false;
            this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
            this.Text = resources.GetString("$this.Text");
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		public string UserName
		{
			get { return this.userName; }
		}

		public string Role
		{
			get { return this.role; }
		}

		public void ResetDataControls()
		{
			this.usersComboBox.Items.Clear();
			this.rolesComboBox.Items.Clear();

            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                this.usersComboBox.Items.Add(user.UserName);
            }
			
            this.rolesComboBox.Items.AddRange(Roles.GetAllRoles());

			this.rolesComboBox.Text = "";
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			this.userName = (string)this.usersComboBox.Items[this.usersComboBox.SelectedIndex];
			if (this.rolesComboBox.SelectedIndex >= 0)
			{
				this.role = (string)this.rolesComboBox.Items[this.rolesComboBox.SelectedIndex];
			}
			else
			{
				this.role = this.rolesComboBox.Text;
			}

			if (this.userName.CompareTo("")==0 || this.role.CompareTo("")==0)
			{
				MessageBox.Show(Properties.Resources.AddUserRoleErrorMessage, Properties.Resources.ErrorMessage,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
