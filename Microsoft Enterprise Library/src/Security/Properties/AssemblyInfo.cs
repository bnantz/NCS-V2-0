//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;

[assembly : SecurityPermission(SecurityAction.RequestMinimum, Flags=SecurityPermissionFlag.SerializationFormatter | SecurityPermissionFlag.ControlThread)]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess=true)]

[assembly : AssemblyTitle("Enterprise Library Security Application Block")]
[assembly : AssemblyDescription("Enterprise Library Security Application Block")]
[assembly : AssemblyVersion("2.0.0.0")]

[assembly: Instrumented(@"root\EnterpriseLibrary")]

[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Tests")]
[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Security.Tests")]
[assembly: InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests")]
