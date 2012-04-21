Public Module NativeMethods

    Public Declare Auto _
    Function SetForegroundWindow Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean

    Public Declare Auto _
    Function ShowWindowAsync Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean

    Public Declare Auto _
    Function IsIconic Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean

    Public SW_RESTORE As Integer = 9

End Module
