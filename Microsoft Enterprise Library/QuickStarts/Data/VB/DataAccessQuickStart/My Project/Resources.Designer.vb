﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.7
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("DataAccessQuickStart.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} rows were affected.
        '''</summary>
        Friend ReadOnly Property AffectedRowsMessage() As String
            Get
                Return ResourceManager.GetString("AffectedRowsMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Application Error.
        '''</summary>
        Friend ReadOnly Property ApplicationErrorMessage() As String
            Get
                Return ResourceManager.GetString("ApplicationErrorMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to This test requires database modifications. Please make sure the database has been initialized using the SetUpQuickStartsDB.bat database script, or from the Install Quickstart option on the Start menu..
        '''</summary>
        Friend ReadOnly Property DbRequirementsMessage() As String
            Get
                Return ResourceManager.GetString("DbRequirementsMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Unable to open {0} : {1}.
        '''</summary>
        Friend ReadOnly Property FileOpenError() As String
            Get
                Return ResourceManager.GetString("FileOpenError", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to There are some problems while trying to use the Data Access Quick Start, please check the following error messages: 
        '''{0}
        '''.
        '''</summary>
        Friend ReadOnly Property GeneralExceptionMessage() As String
            Get
                Return ResourceManager.GetString("GeneralExceptionMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Completed {0} iterations..
        '''</summary>
        Friend ReadOnly Property ProgressMessage() As String
            Get
                Return ResourceManager.GetString("ProgressMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Transfer Completed.
        '''</summary>
        Friend ReadOnly Property TransferCompletedMessage() As String
            Get
                Return ResourceManager.GetString("TransferCompletedMessage", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Transfer Failed.
        '''</summary>
        Friend ReadOnly Property TransferFailedMessage() As String
            Get
                Return ResourceManager.GetString("TransferFailedMessage", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
