//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Reflection;
using System.Security.Permissions;

[assembly : SecurityPermission(SecurityAction.RequestMinimum, Flags=
    SecurityPermissionFlag.SerializationFormatter |
        SecurityPermissionFlag.ControlThread |
        SecurityPermissionFlag.UnmanagedCode
    )]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, MemberAccess=true)]

[assembly: AssemblyTitle("Enterprise Library Cryptography Application Block Tests")]
[assembly: AssemblyDescription("Enterprise Library Cryptography Application Block Tests")]
[assembly : AssemblyVersion("2.0.0.0")]

