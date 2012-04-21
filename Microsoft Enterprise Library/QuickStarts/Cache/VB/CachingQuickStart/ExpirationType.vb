'===============================================================================
' Microsoft patterns & practices Enterprise Library
' Caching Application Block
'===============================================================================
' Copyright © 2004 Microsoft Corporation.  All rights reserved.
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
' OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
' LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
' FITNESS FOR A PARTICULAR PURPOSE.
'===============================================================================


' The list of expiration options the user is allowed to select
Public Enum ExpirationType
    AbsoluteTime = 0
    FileDependency
    SlidingTime
    ExtendedFormat
End Enum

