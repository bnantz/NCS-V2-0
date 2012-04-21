//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: ConfigurationDesignManager(typeof(LoggingConfigurationDesignManager))]

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: ComVisible(false)]

[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests")]

[assembly: AssemblyTitle("Enterprise Library Logging Application Block Design")]
[assembly: AssemblyDescription("Enterprise Library Logging Application Block Design")]
[assembly: AssemblyVersion("2.0.0.0")]

