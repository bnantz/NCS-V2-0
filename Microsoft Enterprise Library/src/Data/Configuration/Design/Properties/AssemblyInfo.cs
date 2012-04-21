//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: ConfigurationDesignManager(typeof(DataConfigurationDesignManager))]
[assembly: ConfigurationDesignManager(typeof(ConnectionStringsConfigurationDesignManager), typeof(DataConfigurationDesignManager))]
[assembly: ConfigurationDesignManager(typeof(OracleConnectionConfigurationDesignManager), typeof(ConnectionStringsConfigurationDesignManager))]

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]


[assembly: AssemblyTitle("Enterprise Library Data Access Application Block Design")]
[assembly: AssemblyDescription("Enterprise Library Data Access Application Block Design")]
[assembly: AssemblyVersion("2.0.0.0")]

[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Tests")]

[assembly: ComVisible(false)]