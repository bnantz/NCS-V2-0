''' <summary>
''' Contains InterOp method calls used by the application.
''' </summary>
Public Module NativeMethods

    Public Declare Auto Function SetForegroundWindow Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean

    Public Declare Auto Function ShowWindowAsync Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean

    Public Declare Auto Function IsIconic Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean

    Public SW_RESTORE As Integer = 9

End Module
