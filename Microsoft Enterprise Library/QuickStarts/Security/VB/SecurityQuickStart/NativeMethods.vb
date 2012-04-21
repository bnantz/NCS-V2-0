'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Security Application Block
'===============================================================================
' Copyright � 2004 Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================

Imports System
Imports System.Runtime.InteropServices
Imports System.Security


Public Module NativeMethods

    Public Declare Auto _
    Function SetForegroundWindow Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean

    Public Declare Auto _
    Function ShowWindowAsync Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean

    Public Declare Auto _
    Function IsIconic Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean

    Public SW_RESTORE As Integer = 9

End Module

