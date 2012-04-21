//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    internal class App
    {
        private MainForm form;

        private App(string [] files)
        {
            this.form = new MainForm(files);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string [] args)
        {
            App app = new App(args);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(app.CurrentDomain_AssemblyResolve);
            AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(app.CurrentDomain_TypeResolve);
            
            app.Run();

            
        }

        public Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            if (form != null)
            {
                string errorMessage = string.Format(Resources.Culture, Resources.TypeResolveFailed, args.Name, AppDomain.CurrentDomain.BaseDirectory);
                form.ShowError(errorMessage, Resources.CaptionError);
				form.HierarchyService.RemoveHierarchy(form.CurrentHierarchy);
            }
            return null;
        }

        public Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (form != null)
            {
                string errorMessage = string.Format(Resources.Culture, Resources.AssemblyResolveFailed, args.Name, AppDomain.CurrentDomain.BaseDirectory);
                form.ShowError(errorMessage, Resources.CaptionError);
				form.HierarchyService.RemoveHierarchy(form.CurrentHierarchy);
            }
            return null;
        }

        private void Run()
        {
			//Application.EnableVisualStyles();
            Application.Run(this.form);
        }
    }
}