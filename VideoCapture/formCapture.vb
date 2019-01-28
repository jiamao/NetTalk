Imports System.Runtime.InteropServices

Public Class formCapture

    Private Sub formCapture_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initcapture()
    End Sub
    Private Sub initcapture()
        Dim lpszName As String = New String(" ")
        Dim lpszVer As String = New String(" ")
        Dim Caps As CAPDRIVERCAPS
        capGetDriverDescriptionA(0, lpszName, 1, lpszVer, 1) '// Retrieves driver info
        lwndC = capCreateCaptureWindowA(lpszName, WS_CAPTION Or WS_THICKFRAME Or WS_VISIBLE Or WS_CHILD, pbwin.Left, pbwin.Top, pbwin.Width, pbwin.Height, pbwin.Handle, 0)

        SetWindowText(lwndC, lpszName)

        If capDriverConnect(lwndC, 0) Then

            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(Caps))
            Marshal.StructureToPtr(Caps, ptr, False)
            capDriverGetCaps(lwndC, ptr, Len(Caps))
            Caps = DirectCast(Marshal.PtrToStructure(ptr, GetType(CAPDRIVERCAPS)), CAPDRIVERCAPS)
            Marshal.FreeCoTaskMem(ptr)

            capPreviewScale(lwndC, True)

            capPreviewRate(lwndC, 66)

            capPreview(lwndC, True)

            ResizeCaptureWindow(lwndC)

        End If

    End Sub
    Private Sub formCapture_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        capSetCallbackOnError(lwndC, VariantType.Null)
        capSetCallbackOnStatus(lwndC, VariantType.Null)
        capSetCallbackOnYield(lwndC, VariantType.Null)
        capSetCallbackOnFrame(lwndC, VariantType.Null)
        capSetCallbackOnVideoStream(lwndC, VariantType.Null)
        capSetCallbackOnWaveStream(lwndC, VariantType.Null)
        capSetCallbackOnCapControl(lwndC, VariantType.Null)
    End Sub

    Private Sub btnrecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnrecord.Click
        Dim sfd As System.Windows.Forms.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        sfd.DefaultExt = "AVI|*.avi"

        If sfd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim filename As String = sfd.FileName
            'Dim lSize As Integer = 1000000
            'capFileSetCaptureFile(lwndC, filename)
            'capFileAlloc(lwndC, lSize)
            Dim CAP_PARAMS As CAPTUREPARMS
            Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(CAP_PARAMS))
            Marshal.StructureToPtr(CAP_PARAMS, ptr, False)
            capCaptureGetSetup(lwndC, ptr, Len(CAP_PARAMS))
            CAP_PARAMS = DirectCast(Marshal.PtrToStructure(ptr, GetType(CAPTUREPARMS)), CAPTUREPARMS)

            CAP_PARAMS.dwRequestMicroSecPerFrame = (1 * (10 ^ 6)) / 30
            CAP_PARAMS.fMakeUserHitOKToCapture = True
            CAP_PARAMS.fCaptureAudio = False

            capCaptureSetSetup(lwndC, ptr, Len(CAP_PARAMS))
            CAP_PARAMS = DirectCast(Marshal.PtrToStructure(ptr, GetType(CAPTUREPARMS)), CAPTUREPARMS)
            Marshal.FreeCoTaskMem(ptr)
            capCaptureSequence(lwndC)
            capFileSaveAs(lwndC, filename)
        End If
    End Sub

   
    Private Sub formCapture_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        'initcapture()
    End Sub
End Class