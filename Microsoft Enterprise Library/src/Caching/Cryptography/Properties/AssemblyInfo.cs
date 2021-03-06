//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;
using System.Runtime.ConstrainedExecution;

[assembly: ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess = true)]
[assembly: ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]

[assembly: AssemblyTitle("Enterprise Library Caching Cryptography Provider")]
[assembly: AssemblyDescription("Enterprise Library Caching Cryptography Provider")]
[assembly: AssemblyVersion("2.0.0.0")]

