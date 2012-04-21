//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: ConfigurationDesignManager(typeof(LoggingDatabaseConfigurationDesignManager), typeof(LoggingConfigurationDesignManager))]
[assembly: ConfigurationDesignManager(typeof(LoggingDatabaseConfigurationDesignManager), typeof(ConnectionStringsConfigurationDesignManager))]

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

[assembly: ComVisible(false)]

[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.Tests")]

[assembly : AssemblyTitle("Enterprise Library Logging and Instrumentation Database Provider Design")]
[assembly : AssemblyDescription("Enterprise Library Logging and Instrumentation Database Provider Design")]
[assembly : AssemblyVersion("2.0.0.0")]

