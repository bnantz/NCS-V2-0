//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// UI to support exporting a <see cref="ProtectedKey"/>.
    /// </summary>
    public partial class ExportKeyUI : Form
    {
        private ProtectedKey key;

        /// <summary>
        /// Initialize a new instance of the <see cref="ExportKeyUI"/> class.
        /// </summary>
        public ExportKeyUI(ProtectedKey key)
        {
            this.key = key;
            InitializeComponent();

            btnCancel.Text = Resources.ExportKeyUICancelButton;
            btnOk.Text = Resources.ExportKeyUIOkButton;
            Text = Resources.ExportKeyDialogTitle;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (exportKeyControl1.ValidateControl())
            {
                try
                {
                    using (Stream fileOut = File.OpenWrite(exportKeyControl1.Filename))
                    {
                        KeyManager.ArchiveKey(fileOut, key, exportKeyControl1.Password);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.ErrorExportingKey, Resources.ExportDialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
			else
			{
				this.DialogResult = DialogResult.None;
			}
        }
    }
}