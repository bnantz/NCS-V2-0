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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Visual control that allows an user to select the <see cref="DataProtectionScope"/> wich is used to store an cryptographic key.
    /// </summary>
    public partial class ChooseDataProtectionScopeControl : UserControl
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ChooseDataProtectionScopeControl"/> class.
        /// </summary>
        public ChooseDataProtectionScopeControl()
        {
            InitializeComponent();

            rbDpapiScopeLocalMachine.Text = Resources.DataProtectionScopeLocalMachine;
            rbDpapiScopeCurrentUser.Text = Resources.DataProtectionScopeCurrentUser;

            lblChooseDpapiScopeMessage.Text = Resources.ChooseDpapiScopeMessage;
        }
        /// <summary>
        /// Gets or sets the <see cref="DataProtectionScope"/> which should be used to encrypt a key file.
        /// </summary>
        public DataProtectionScope Scope
        {
            get
            {
                return (rbDpapiScopeCurrentUser.Checked)
                    ? DataProtectionScope.CurrentUser
                    : DataProtectionScope.LocalMachine;
            }
            set
            {
                if (value == DataProtectionScope.CurrentUser)
                {
                    rbDpapiScopeCurrentUser.Checked = true;
                }
                else
                {
                    rbDpapiScopeLocalMachine.Checked = true;
                }
            }
        }
    }
}
