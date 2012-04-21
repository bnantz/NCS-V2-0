//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Diagnostics;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[assembly: ConfigurationDesignManager(typeof(ExceptionHandlingConfigurationDesignManager))]

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: ComVisible(false)]

[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests")]

[assembly: AssemblyTitle("Enterprise Library Exception Handling Application Block Design")]
[assembly: AssemblyDescription("Enterprise Library Exception Handling Application Block Design")]
[assembly: AssemblyVersion("2.0.0.0")]

