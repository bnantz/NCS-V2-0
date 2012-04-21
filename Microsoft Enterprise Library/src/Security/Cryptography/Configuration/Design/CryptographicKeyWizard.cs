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
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// UI wizard that allows an user to manipulate a <see cref="ProtectedKeySettings"/> class, from within the configuration console.
    /// </summary>
    public partial class CryptographicKeyWizard : Form
    {
        private ProtectedKeySettings protectedKeySettings = new ProtectedKeySettings();
        private UserControl currentControl = null;
        private CryptographicKeyWizardStep currentWizardStep = CryptographicKeyWizardStep.SupplyKey;
        private Stack<CryptographicKeyWizardStep> previousWizardSteps = new Stack<CryptographicKeyWizardStep>();
        private Dictionary<CryptographicKeyWizardStep, UserControl> controlByCryptographicKeyWizardStep = new Dictionary<CryptographicKeyWizardStep, UserControl>();


        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicKeyWizard"/> class with a <see cref="IKeyCreator"/>.
        /// </summary>
        /// <param name="keyCreator">The <see cref="IKeyCreator"/> that should be used to generate and validate an input key.</param>
        public CryptographicKeyWizard(IKeyCreator keyCreator)
            :this(CryptographicKeyWizardStep.SupplyKey, keyCreator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptographicKeyWizard"/> class with a <see cref="IKeyCreator"/> and 
        /// for a specific <see cref="CryptographicKeyWizardStep"/>.
        /// </summary>
        /// <param name="keyCreator">The <see cref="IKeyCreator"/> that should be used to generate and validate an input key.</param>
        /// <param name="step">The <see cref="CryptographicKeyWizardStep"/> which should be shown within the wizard.</param>
        public CryptographicKeyWizard(CryptographicKeyWizardStep step, IKeyCreator keyCreator)
        {
            InitializeComponent();

            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.SupplyKey, supplyKeyControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.CreateNewKey, createNewKeyControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.OpenExistingKeyFile, openExistingKeyFileControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.ImportArchivedKey, importArchivedKeyControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.ChooseProtectionScope, chooseDpapiScopeControl);
            controlByCryptographicKeyWizardStep.Add(CryptographicKeyWizardStep.ChooseKeyFile, chooseProtectionScopeControl);

            btnCancel.Text = Resources.CryptographicKeyWizardCancelButton;
            btnPrevious.Text = Resources.CryptographicKeyWizardPreviousButton;
            btnNext.Text = Resources.CryptographicKeyWizardNextButton;
            btnFinish.Text = Resources.CryptographicKeyWizardFinishButton;
            Text = Resources.CryptographicKeyWizardTitle;


            createNewKeyControl.KeyCreator = keyCreator;
            
            currentWizardStep = step;
            RefreshWizardControls();
        }

        /// <summary>
        /// Gets or sets the <see cref="ProtectedKeySettings"/> for this wizard.
        /// </summary>
        public ProtectedKeySettings KeySettings
        {
            get{return protectedKeySettings;}
            set
            {
                createNewKeyControl.Key = value.ProtectedKey.DecryptedKey;
                chooseDpapiScopeControl.Scope = value.Scope;
                chooseProtectionScopeControl.Filepath = value.Filename;

            }
        }

        /// <summary>
        /// Sets an unencrypted key to be used within the wizard.    
        /// </summary>
        public Byte[] Key
        {
            set{createNewKeyControl.Key = value;}
        }

        /// <summary>
        /// Sets an <see cref="IKeyCreator"/> instance, that should be used to validate and generate keys.
        /// </summary>
        public IKeyCreator KeyCreator
        {
            set { createNewKeyControl.KeyCreator = value; }
        }

        private void UpdateState()
        {
            switch(currentWizardStep)
            {
                case CryptographicKeyWizardStep.ChooseKeyFile:
                    protectedKeySettings.Filename = chooseProtectionScopeControl.Filepath;
                    break;

                case CryptographicKeyWizardStep.OpenExistingKeyFile:
                    protectedKeySettings.Filename = openExistingKeyFileControl.Filepath;
                    break;

                case CryptographicKeyWizardStep.ChooseProtectionScope:
                    protectedKeySettings.Scope = chooseDpapiScopeControl.Scope;
                    break;
            }
        }

        private CryptographicKeyWizardStep NextStep
        {
            get 
            {
                switch(currentWizardStep)
                {
                    case CryptographicKeyWizardStep.SupplyKey:
                        switch(supplyKeyControl.Method)
                        {
                            case SupplyKeyMethod.CreateNew:
                                return CryptographicKeyWizardStep.CreateNewKey;

                            case SupplyKeyMethod.ImportKey:
                                return CryptographicKeyWizardStep.ImportArchivedKey;

                            case SupplyKeyMethod.OpenExisting:
                                return CryptographicKeyWizardStep.OpenExistingKeyFile;
                        }
                        
                        return CryptographicKeyWizardStep.CreateNewKey;

                    case CryptographicKeyWizardStep.CreateNewKey:
                        return CryptographicKeyWizardStep.ChooseKeyFile;

                    case CryptographicKeyWizardStep.ChooseKeyFile:
                        return CryptographicKeyWizardStep.ChooseProtectionScope;

                    case CryptographicKeyWizardStep.OpenExistingKeyFile:
                        return CryptographicKeyWizardStep.ChooseProtectionScope;
                    
                    case CryptographicKeyWizardStep.ImportArchivedKey:
                        return CryptographicKeyWizardStep.ChooseKeyFile;

                    default:
                        return CryptographicKeyWizardStep.Finished;
                }
            }
        }

        private void RefreshWizardControls()
        {
            btnNext.Visible =(NextStep != CryptographicKeyWizardStep.Finished);
            btnFinish.Visible = (NextStep == CryptographicKeyWizardStep.Finished);
            btnPrevious.Visible = (previousWizardSteps.Count != 0);

            if (currentControl != null)
            {
                currentControl.Visible = false;
            }

            currentControl = controlByCryptographicKeyWizardStep[currentWizardStep];
            currentControl.Visible = true;
            currentControl.Dock = DockStyle.Fill;
        }

        private void DoProceed()
        {
            IWizardValidationTarget validationTarget = currentControl as IWizardValidationTarget;
            if (validationTarget != null && !validationTarget.ValidateControl())
            {
                return;
            }

            UpdateState();

            previousWizardSteps.Push(currentWizardStep);
            currentWizardStep = NextStep;

            RefreshWizardControls();
        }

        private void DoGoBack()
        {
            currentWizardStep = previousWizardSteps.Pop();

            RefreshWizardControls();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentControl != null)
            {
                DoProceed();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            DoGoBack();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            switch(supplyKeyControl.Method)
            {
                case SupplyKeyMethod.OpenExisting:
                    using (Stream importedKeyReader = File.OpenRead(openExistingKeyFileControl.Filepath))
                    {
                        try
                        {
                            protectedKeySettings.ProtectedKey = KeyManager.Read(importedKeyReader, chooseDpapiScopeControl.Scope);
							protectedKeySettings.Scope = chooseDpapiScopeControl.Scope;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(Resources.ErrorImportingKey, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    break;

                case SupplyKeyMethod.CreateNew:
                    protectedKeySettings.ProtectedKey = ProtectedKey.CreateFromPlaintextKey(createNewKeyControl.Key, chooseDpapiScopeControl.Scope);
					protectedKeySettings.Scope = chooseDpapiScopeControl.Scope;
					break;

                case SupplyKeyMethod.ImportKey:
                    using (Stream archivedKeyReader = File.OpenRead(importArchivedKeyControl.Filename))
                    {
						try
						{
							protectedKeySettings.ProtectedKey = KeyManager.RestoreKey(archivedKeyReader, importArchivedKeyControl.Passphrase, chooseDpapiScopeControl.Scope);
							protectedKeySettings.Scope = chooseDpapiScopeControl.Scope;
						}
						catch (CryptographicException)
						{
							MessageBox.Show(Resources.KeyCouldNotBeRead, Resources.CryptoKeyWizardErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							this.DialogResult = DialogResult.None;
						}
                    }
                    break;
            }
        }
    }
}