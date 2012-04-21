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
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Provides a user interface for selecting a file name.
    /// </summary>
    /// <seealso cref="FileNameEditor"/>
    public class FilteredFileNameEditor : FileNameEditor
    {
        private FilteredFileNameEditorAttribute editorAttribute;
        private string file;

        /// <summary>
        /// Initialize a new instance of the <see cref="FilteredFileNameEditor"/> class.
        /// </summary>
        public FilteredFileNameEditor()
        {
        }

        /// <summary>
        /// Edits the specified object using the editor style provided by the GetEditStyle method.
        /// </summary>
        /// <param name="context">
        /// An ITypeDescriptorContext that can be used to gain additional context information.
        /// </param>
		/// <param name="provider">
        /// A service provider object through which editing services may be obtained.
        /// </param>
        /// <param name="value">
        /// An instance of the value being edited.
        /// </param>
        /// <returns>
        /// The new value of the object. If the value of the object hasn't changed, this should return the same object it was passed.
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
			file = value as string;

            if (null == context) return base.EditValue(context, provider, value);
			if (null == provider) return base.EditValue(context, provider, value);

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
                {
                    editorAttribute = attribute as FilteredFileNameEditorAttribute;
                    if (editorAttribute != null)
                    {
                        break;
                    }
                }
            }			
            return base.EditValue(context, provider, value);
        }

        /// <summary>
        /// Initializes the open file dialog when it is created.
        /// </summary>
        /// <param name="openFileDialog">
        /// The <see cref="OpenFileDialog"/> to use to select a file name. 
        /// </param>
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            if (this.editorAttribute != null)
            {
                openFileDialog.Filter = this.editorAttribute.Filter;
                if (file != null)
                {
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(file));
                }
            }
        }
    }
}