//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright � Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright � Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Diagnostics;
using System.Reflection;
using System.Security.Permissions;
using System.Runtime.ConstrainedExecution;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;

[assembly: SecurityPermission(SecurityAction.RequestMinimum)]
[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]

[assembly: AssemblyTitle("Enterprise Library Exception Handling Application Block")]
[assembly: AssemblyDescription("Enterprise Library Exception Handling Application Block")]
[assembly: AssemblyVersion("2.0.0.0")]

[assembly: Instrumented(@"root\EnterpriseLibrary")]

// Assemblies needing visibilty into our code for testing purposes
[assembly:InternalsVisibleTo("Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests")]
