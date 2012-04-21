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

using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: ConfigurationDesignManager(typeof(InstrumentationConfigurationDesignManager))]
[assembly: ConfigurationDesignManager(typeof(ConfigurationSourceConfigurationDesignManager))]

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess=true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]
[assembly: FileIOPermission(SecurityAction.RequestMinimum)]

[assembly: AssemblyTitle("Enterprise Library Configuration Application Block Design")]
[assembly: AssemblyDescription("Enterprise Library Configuration Application Block Design")]
[assembly: AssemblyVersion("2.0.0.0")]

[assembly: ComVisible(false)]

[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests")]