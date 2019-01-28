Imports System.Runtime.InteropServices
Module CapMen
  
    Public Const WS_BORDER As Integer = &H800000
    Public Const WS_CAPTION As Integer = &HC00000
    Public Const WS_SYSMENU As Integer = &H80000
    Public Const WS_CHILD As Integer = &H40000000
    Public Const WS_VISIBLE As Integer = &H10000000
    Public Const WS_OVERLAPPED As Integer = &H0
    Public Const WS_MINIMIZEBOX As Integer = &H20000
    Public Const WS_MAXIMIZEBOX As Integer = &H10000
    Public Const WS_THICKFRAME As Integer = &H40000
    Public Const WS_OVERLAPPEDWINDOW As Boolean = (WS_OVERLAPPED Or WS_CAPTION Or WS_SYSMENU Or WS_THICKFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX)
    Public Const SWP_NOMOVE As Short = &H2S
    Public Const SWP_NOSIZE As Short = 1
    Public Const SWP_NOZORDER As Short = &H4S
    Public Const HWND_BOTTOM As Short = 1
    Public Const HWND_TOPMOST As Short = -1
    Public Const HWND_NOTOPMOST As Short = -2
    Public Const SM_CYCAPTION As Short = 4
    Public Const SM_CXFRAME As Short = 32
    Public Const SM_CYFRAME As Short = 33
    Public Const WS_EX_TRANSPARENT As Integer = &H20
    Public Const GWL_STYLE As Short = (-16)
    Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal hWnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer


    Declare Function lStrCpy Lib "kernel32" Alias "lstrcpyA" (ByVal lpString1 As Integer, ByVal lpString2 As Integer) As Integer

    Declare Function lStrCpyn Lib "kernel32" Alias "lstrcpynA" (ByVal lpString1 As Object, ByVal lpString2 As Integer, ByVal iMaxLength As Integer) As Integer
    Declare Sub RtlMoveMemory Lib "kernel32" (ByRef hpvDest As Object, ByVal hpvSource As Integer, ByVal cbCopy As Integer)
   
    Declare Sub hmemcpy Lib "kernel32" (ByRef hpvDest As Object, ByRef hpvSource As Object, ByVal cbCopy As Integer)

    Declare Function SetWindowPos Lib "user32" (ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
    Declare Function DestroyWindow Lib "user32" (ByVal hndw As Integer) As Boolean
    Declare Function GetSystemMetrics Lib "user32" (ByVal nIndex As Integer) As Integer
    Declare Function SetWindowText Lib "user32" Alias "SetWindowTextA" (ByVal hWnd As Integer, ByVal lpString As String) As Integer

    Public lwndC As Integer

    Function MyFrameCallback(ByVal lwnd As Integer, ByVal lpVHdr As Integer) As Integer

        Debug.Print("FrameCallBack")

        Dim VideoHeader As VIDEOHDR
        Dim VideoData() As Byte

        Dim ptr As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(VideoHeader))
        Marshal.StructureToPtr(VideoHeader, ptr, False)
        RtlMoveMemory(ptr, lpVHdr, Len(VideoHeader))
        VideoHeader = DirectCast(Marshal.PtrToStructure(ptr, GetType(VIDEOHDR)), VIDEOHDR)
        Marshal.FreeCoTaskMem(ptr)

        ReDim VideoData(VideoHeader.dwBytesUsed)

        RtlMoveMemory(VideoData(0), VideoHeader.lpData, VideoHeader.dwBytesUsed)

        Debug.Print(VideoHeader.dwBytesUsed)
        Debug.Print(VideoData.ToString())

    End Function

    Function MyYieldCallback(ByRef lwnd As Integer) As Integer

        Debug.Print("Yield")

    End Function

    Function MyErrorCallback(ByVal lwnd As Integer, ByVal iID As Integer, ByVal ipstrStatusText As Integer) As Integer

        If iID = 0 Then Exit Function

        Dim sStatusText As String
        Dim usStatusText As String

        sStatusText = New String(Chr(0), 255)

        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(sStatusText))
        ptr = Marshal.StringToBSTR(sStatusText)
        lStrCpy(ptr, ipstrStatusText)
        sStatusText = Marshal.PtrToStringAnsi(ptr)
        Marshal.FreeCoTaskMem(ptr)
        sStatusText = Left(sStatusText, InStr(sStatusText, Chr(0)) - 1)

    End Function

    Function MyStatusCallback(ByVal lwnd As Integer, ByVal iID As Integer, ByVal ipstrStatusText As Integer) As Integer

        If iID = 0 Then Exit Function

        Dim sStatusText As String
        Dim usStatusText As String

        sStatusText = New String(Chr(0), 255)
        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(sStatusText))
        ptr = Marshal.StringToBSTR(sStatusText)
        lStrCpy(ptr, ipstrStatusText)
        sStatusText = Marshal.PtrToStringAnsi(ptr)
        sStatusText = Left(sStatusText, InStr(sStatusText, Chr(0)) - 1)

        Marshal.FreeCoTaskMem(ptr)

        Select Case iID '


        End Select


    End Function

    Sub ResizeCaptureWindow(ByVal lwnd As Integer)

        Dim CAPSTATUS_Renamed As CAPSTATUS
        Dim lCaptionHeight As Integer
        Dim lX_Border As Integer
        Dim lY_Border As Integer


        lCaptionHeight = GetSystemMetrics(SM_CYCAPTION)
        lX_Border = GetSystemMetrics(SM_CXFRAME)
        lY_Border = GetSystemMetrics(SM_CYFRAME)

        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(CAPSTATUS_Renamed))
        Marshal.StructureToPtr(CAPSTATUS_Renamed, ptr, False)

        If capGetStatus(lwnd, ptr, Len(CAPSTATUS_Renamed)) Then
            CAPSTATUS_Renamed = DirectCast(Marshal.PtrToStructure(ptr, GetType(CAPSTATUS)), CAPSTATUS)

            SetWindowPos(lwnd, HWND_BOTTOM, 0, 0, CAPSTATUS_Renamed.uiImageWidth + (lX_Border * 2), CAPSTATUS_Renamed.uiImageHeight + lCaptionHeight + (lY_Border * 2), SWP_NOMOVE Or SWP_NOZORDER)
        End If
        Marshal.FreeCoTaskMem(ptr)
        
    End Sub

    Function MyVideoStreamCallback(ByRef lwnd As Integer, ByRef lpVHdr As Integer) As Integer
        Beep()
    End Function

    Function MyWaveStreamCallback(ByRef lwnd As Integer, ByRef lpVHdr As Integer) As Integer

    End Function

    Sub LogError(ByRef txtError As String, ByRef lID As Integer)

    End Sub
End Module
