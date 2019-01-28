Module AviCap
   
    Public Const WM_USER As Short = &H400S
    Structure POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Short, ByVal lParam As Integer) As Integer
    Declare Function SendMessageS Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Short, ByVal lParam As String) As Integer

    Public Const WM_CAP_START As Short = WM_USER

    Public Const WM_CAP_GET_CAPSTREAMPTR As Integer = WM_CAP_START + 1

    Public Const WM_CAP_SET_CALLBACK_ERROR As Integer = WM_CAP_START + 2
    Public Const WM_CAP_SET_CALLBACK_STATUS As Integer = WM_CAP_START + 3
    Public Const WM_CAP_SET_CALLBACK_YIELD As Integer = WM_CAP_START + 4
    Public Const WM_CAP_SET_CALLBACK_FRAME As Integer = WM_CAP_START + 5
    Public Const WM_CAP_SET_CALLBACK_VIDEOSTREAM As Integer = WM_CAP_START + 6
    Public Const WM_CAP_SET_CALLBACK_WAVESTREAM As Integer = WM_CAP_START + 7
    Public Const WM_CAP_GET_USER_DATA As Integer = WM_CAP_START + 8
    Public Const WM_CAP_SET_USER_DATA As Integer = WM_CAP_START + 9

    Public Const WM_CAP_DRIVER_CONNECT As Integer = WM_CAP_START + 10
    Public Const WM_CAP_DRIVER_DISCONNECT As Integer = WM_CAP_START + 11
    Public Const WM_CAP_DRIVER_GET_NAME As Integer = WM_CAP_START + 12
    Public Const WM_CAP_DRIVER_GET_VERSION As Integer = WM_CAP_START + 13
    Public Const WM_CAP_DRIVER_GET_CAPS As Integer = WM_CAP_START + 14

    Public Const WM_CAP_FILE_SET_CAPTURE_FILE As Integer = WM_CAP_START + 20
    Public Const WM_CAP_FILE_GET_CAPTURE_FILE As Integer = WM_CAP_START + 21
    Public Const WM_CAP_FILE_ALLOCATE As Integer = WM_CAP_START + 22
    Public Const WM_CAP_FILE_SAVEAS As Integer = WM_CAP_START + 23
    Public Const WM_CAP_FILE_SET_INFOCHUNK As Integer = WM_CAP_START + 24
    Public Const WM_CAP_FILE_SAVEDIB As Integer = WM_CAP_START + 25

    Public Const WM_CAP_EDIT_COPY As Integer = WM_CAP_START + 30

    Public Const WM_CAP_SET_AUDIOFORMAT As Integer = WM_CAP_START + 35
    Public Const WM_CAP_GET_AUDIOFORMAT As Integer = WM_CAP_START + 36

    Public Const WM_CAP_DLG_VIDEOFORMAT As Integer = WM_CAP_START + 41
    Public Const WM_CAP_DLG_VIDEOSOURCE As Integer = WM_CAP_START + 42
    Public Const WM_CAP_DLG_VIDEODISPLAY As Integer = WM_CAP_START + 43
    Public Const WM_CAP_GET_VIDEOFORMAT As Integer = WM_CAP_START + 44
    Public Const WM_CAP_SET_VIDEOFORMAT As Integer = WM_CAP_START + 45
    Public Const WM_CAP_DLG_VIDEOCOMPRESSION As Integer = WM_CAP_START + 46

    Public Const WM_CAP_SET_PREVIEW As Integer = WM_CAP_START + 50
    Public Const WM_CAP_SET_OVERLAY As Integer = WM_CAP_START + 51
    Public Const WM_CAP_SET_PREVIEWRATE As Integer = WM_CAP_START + 52
    Public Const WM_CAP_SET_SCALE As Integer = WM_CAP_START + 53
    Public Const WM_CAP_GET_STATUS As Integer = WM_CAP_START + 54
    Public Const WM_CAP_SET_SCROLL As Integer = WM_CAP_START + 55

    Public Const WM_CAP_GRAB_FRAME As Integer = WM_CAP_START + 60
    Public Const WM_CAP_GRAB_FRAME_NOSTOP As Integer = WM_CAP_START + 61

    Public Const WM_CAP_SEQUENCE As Integer = WM_CAP_START + 62
    Public Const WM_CAP_SEQUENCE_NOFILE As Integer = WM_CAP_START + 63
    Public Const WM_CAP_SET_SEQUENCE_SETUP As Integer = WM_CAP_START + 64
    Public Const WM_CAP_GET_SEQUENCE_SETUP As Integer = WM_CAP_START + 65
    Public Const WM_CAP_SET_MCI_DEVICE As Integer = WM_CAP_START + 66
    Public Const WM_CAP_GET_MCI_DEVICE As Integer = WM_CAP_START + 67
    Public Const WM_CAP_STOP As Integer = WM_CAP_START + 68
    Public Const WM_CAP_ABORT As Integer = WM_CAP_START + 69

    Public Const WM_CAP_SINGLE_FRAME_OPEN As Integer = WM_CAP_START + 70
    Public Const WM_CAP_SINGLE_FRAME_CLOSE As Integer = WM_CAP_START + 71
    Public Const WM_CAP_SINGLE_FRAME As Integer = WM_CAP_START + 72

    Public Const WM_CAP_PAL_OPEN As Integer = WM_CAP_START + 80
    Public Const WM_CAP_PAL_SAVE As Integer = WM_CAP_START + 81
    Public Const WM_CAP_PAL_PASTE As Integer = WM_CAP_START + 82
    Public Const WM_CAP_PAL_AUTOCREATE As Integer = WM_CAP_START + 83
    Public Const WM_CAP_PAL_MANUALCREATE As Integer = WM_CAP_START + 84

    Public Const WM_CAP_SET_CALLBACK_CAPCONTROL As Integer = WM_CAP_START + 85

    Public Const WM_CAP_END As Short = WM_CAP_SET_CALLBACK_CAPCONTROL

    Structure CAPDRIVERCAPS
        Dim wDeviceIndex As Integer '               // Driver index in system.ini
        Dim fHasOverlay As Integer '                // Can device overlay?
        Dim fHasDlgVideoSource As Integer '         // Has Video source dlg?
        Dim fHasDlgVideoFormat As Integer '         // Has Format dlg?
        Dim fHasDlgVideoDisplay As Integer '        // Has External out dlg?
        Dim fCaptureInitialized As Integer '        // Driver ready to capture?
        Dim fDriverSuppliesPalettes As Integer '    // Can driver make palettes?
        Dim hVideoIn As Integer '                   // Driver In channel
        Dim hVideoOut As Integer '                  // Driver Out channel
        Dim hVideoExtIn As Integer '                // Driver Ext In channel
        Dim hVideoExtOut As Integer '               // Driver Ext Out channel
    End Structure

    Structure CAPSTATUS
        Dim uiImageWidth As Integer '// Width of the image
        Dim uiImageHeight As Integer '// Height of the image
        Dim fLiveWindow As Integer '// Now Previewing video?
        Dim fOverlayWindow As Integer '// Now Overlaying video?
        Dim fScale As Integer '// Scale image to client?
        Dim ptScroll As POINTAPI '// Scroll position
        Dim fUsingDefaultPalette As Integer '// Using default driver palette?
        Dim fAudioHardware As Integer '// Audio hardware present?
        Dim fCapFileExists As Integer '// Does capture file exist?
        Dim dwCurrentVideoFrame As Integer '// # of video frames cap'td
        Dim dwCurrentVideoFramesDropped As Integer '// # of video frames dropped
        Dim dwCurrentWaveSamples As Integer '// # of wave samples cap'td
        Dim dwCurrentTimeElapsedMS As Integer '// Elapsed capture duration
        Dim hPalCurrent As Integer '// Current palette in use
        Dim fCapturingNow As Integer '// Capture in progress?
        Dim dwReturn As Integer '// Error value after any operation
        Dim wNumVideoAllocated As Integer '// Actual number of video buffers
        Dim wNumAudioAllocated As Integer '// Actual number of audio buffers
    End Structure

    Structure CAPTUREPARMS
        Dim dwRequestMicroSecPerFrame As Integer '// Requested capture rate
        Dim fMakeUserHitOKToCapture As Integer '// Show "Hit OK to cap" dlg?
        Dim wPercentDropForError As Integer '// Give error msg if > (10%)
        Dim fYield As Integer '// Capture via background task?
        Dim dwIndexSize As Integer '// Max index size in frames (32K)
        Dim wChunkGranularity As Integer '// Junk chunk granularity (2K)
        Dim fUsingDOSMemory As Integer '// Use DOS buffers?
        Dim wNumVideoRequested As Integer '// # video buffers, If 0, autocalc
        Dim fCaptureAudio As Integer '// Capture audio?
        Dim wNumAudioRequested As Integer '// # audio buffers, If 0, autocalc
        Dim vKeyAbort As Integer '// Virtual key causing abort
        Dim fAbortLeftMouse As Integer '// Abort on left mouse?
        Dim fAbortRightMouse As Integer '// Abort on right mouse?
        Dim fLimitEnabled As Integer '// Use wTimeLimit?
        Dim wTimeLimit As Integer '// Seconds to capture
        Dim fMCIControl As Integer '// Use MCI video source?
        Dim fStepMCIDevice As Integer '// Step MCI device?
        Dim dwMCIStartTime As Integer '// Time to start in MS
        Dim dwMCIStopTime As Integer '// Time to stop in MS
        Dim fStepCaptureAt2x As Integer '// Perform spatial averaging 2x
        Dim wStepCaptureAverageFrames As Integer '// Temporal average n Frames
        Dim dwAudioBufferSize As Integer '// Size of audio bufs (0 = default)
        Dim fDisableWriteCache As Integer '// Attempt to disable write cache
    End Structure

    Structure CAPINFOCHUNK
        Dim fccInfoID As Integer '// Chunk ID, "ICOP" for copyright
        Dim lpData As Integer '// pointer to data
        Dim cbData As Integer '// size of lpData
    End Structure

    Structure VIDEOHDR
        Dim lpData As Integer '// address of video buffer
        Dim dwBufferLength As Integer '// size, in bytes, of the Data buffer
        Dim dwBytesUsed As Integer '// see below
        Dim dwTimeCaptured As Integer '// see below
        Dim dwUser As Integer '// user-specific data
        Dim dwFlags As Integer '// see below
        <VBFixedArray(3)> Dim dwReserved() As Integer '// reserved; do not use}
        Public Sub Initialize()
            ReDim dwReserved(3)
        End Sub
    End Structure


    Declare Function capCreateCaptureWindowA Lib "avicap32.dll" (ByVal lpszWindowName As String, ByVal dwStyle As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Short, ByVal hWndParent As Integer, ByVal nID As Integer) As Integer
    Declare Function capGetDriverDescriptionA Lib "avicap32.dll" (ByVal wDriver As Short, ByVal lpszName As String, ByVal cbName As Integer, ByVal lpszVer As String, ByVal cbVer As Integer) As Boolean

    Public Const IDS_CAP_BEGIN As Short = 300 '/* "Capture Start" */
    Public Const IDS_CAP_END As Short = 301 '/* "Capture End" */

    Public Const IDS_CAP_INFO As Short = 401 '/* "%s" */
    Public Const IDS_CAP_OUTOFMEM As Short = 402 '/* "Out of memory" */
    Public Const IDS_CAP_FILEEXISTS As Short = 403 '/* "File '%s' exists -- overwrite it?" */
    Public Const IDS_CAP_ERRORPALOPEN As Short = 404 '/* "Error opening palette '%s'" */
    Public Const IDS_CAP_ERRORPALSAVE As Short = 405 '/* "Error saving palette '%s'" */
    Public Const IDS_CAP_ERRORDIBSAVE As Short = 406 '/* "Error saving frame '%s'" */
    Public Const IDS_CAP_DEFAVIEXT As Short = 407 '/* "avi" */
    Public Const IDS_CAP_DEFPALEXT As Short = 408 '/* "pal" */
    Public Const IDS_CAP_CANTOPEN As Short = 409 '/* "Cannot open '%s'" */
    Public Const IDS_CAP_SEQ_MSGSTART As Short = 410 '/* "Select OK to start capture\nof video sequence\nto %s." */
    Public Const IDS_CAP_SEQ_MSGSTOP As Short = 411 '/* "Hit ESCAPE or click to end capture" */

    Public Const IDS_CAP_VIDEDITERR As Short = 412 '/* "An error occurred while trying to run VidEdit." */
    Public Const IDS_CAP_READONLYFILE As Short = 413 '/* "The file '%s' is a read-only file." */
    Public Const IDS_CAP_WRITEERROR As Short = 414 '/* "Unable to write to file '%s'.\nDisk may be full." */
    Public Const IDS_CAP_NODISKSPACE As Short = 415 '/* "There is no space to create a capture file on the specified device." */
    Public Const IDS_CAP_SETFILESIZE As Short = 416 '/* "Set File Size" */
    Public Const IDS_CAP_SAVEASPERCENT As Short = 417 '/* "SaveAs: %2ld%%  Hit Escape to abort." */

    Public Const IDS_CAP_DRIVER_ERROR As Short = 418 '/* Driver specific error message */

    Public Const IDS_CAP_WAVE_OPEN_ERROR As Short = 419 '/* "Error: Cannot open the wave input device.\nCheck sample size, frequency, and channels." */
    Public Const IDS_CAP_WAVE_ALLOC_ERROR As Short = 420 '/* "Error: Out of memory for wave buffers." */
    Public Const IDS_CAP_WAVE_PREPARE_ERROR As Short = 421 '/* "Error: Cannot prepare wave buffers." */
    Public Const IDS_CAP_WAVE_ADD_ERROR As Short = 422 '/* "Error: Cannot add wave buffers." */
    Public Const IDS_CAP_WAVE_SIZE_ERROR As Short = 423 '/* "Error: Bad wave size." */

    Public Const IDS_CAP_VIDEO_OPEN_ERROR As Short = 424 '/* "Error: Cannot open the video input device." */
    Public Const IDS_CAP_VIDEO_ALLOC_ERROR As Short = 425 '/* "Error: Out of memory for video buffers." */
    Public Const IDS_CAP_VIDEO_PREPARE_ERROR As Short = 426 '/* "Error: Cannot prepare video buffers." */
    Public Const IDS_CAP_VIDEO_ADD_ERROR As Short = 427 '/* "Error: Cannot add video buffers." */
    Public Const IDS_CAP_VIDEO_SIZE_ERROR As Short = 428 '/* "Error: Bad video size." */

    Public Const IDS_CAP_FILE_OPEN_ERROR As Short = 429 '/* "Error: Cannot open capture file." */
    Public Const IDS_CAP_FILE_WRITE_ERROR As Short = 430 '/* "Error: Cannot write to capture file.  Disk may be full." */
    Public Const IDS_CAP_RECORDING_ERROR As Short = 431 '/* "Error: Cannot write to capture file.  Data rate too high or disk full." */
    Public Const IDS_CAP_RECORDING_ERROR2 As Short = 432 '/* "Error while recording" */
    Public Const IDS_CAP_AVI_INIT_ERROR As Short = 433 '/* "Error: Unable to initialize for capture." */
    Public Const IDS_CAP_NO_FRAME_CAP_ERROR As Short = 434 '/* "Warning: No frames captured.\nConfirm that vertical sync interrupts\nare configured and enabled." */
    Public Const IDS_CAP_NO_PALETTE_WARN As Short = 435 '/* "Warning: Using default palette." */
    Public Const IDS_CAP_MCI_CONTROL_ERROR As Short = 436 '/* "Error: Unable to access MCI device." */
    Public Const IDS_CAP_MCI_CANT_STEP_ERROR As Short = 437 '/* "Error: Unable to step MCI device." */
    Public Const IDS_CAP_NO_AUDIO_CAP_ERROR As Short = 438 '/* "Error: No audio data captured.\nCheck audio card settings." */
    Public Const IDS_CAP_AVI_DRAWDIB_ERROR As Short = 439 '/* "Error: Unable to draw this data format." */
    Public Const IDS_CAP_COMPRESSOR_ERROR As Short = 440 '/* "Error: Unable to initialize compressor." */
    Public Const IDS_CAP_AUDIO_DROP_ERROR As Short = 441 '/* "Error: Audio data was lost during capture, reduce capture rate." */

    '/* status string IDs */
    Public Const IDS_CAP_STAT_LIVE_MODE As Short = 500 '/* "Live window" */
    Public Const IDS_CAP_STAT_OVERLAY_MODE As Short = 501 '/* "Overlay window" */
    Public Const IDS_CAP_STAT_CAP_INIT As Short = 502 '/* "Setting up for capture - Please wait" */
    Public Const IDS_CAP_STAT_CAP_FINI As Short = 503 '/* "Finished capture, now writing frame %ld" */
    Public Const IDS_CAP_STAT_PALETTE_BUILD As Short = 504 '/* "Building palette map" */
    Public Const IDS_CAP_STAT_OPTPAL_BUILD As Short = 505 '/* "Computing optimal palette" */
    Public Const IDS_CAP_STAT_I_FRAMES As Short = 506 '/* "%d frames" */
    Public Const IDS_CAP_STAT_L_FRAMES As Short = 507 '/* "%ld frames" */
    Public Const IDS_CAP_STAT_CAP_L_FRAMES As Short = 508 '/* "Captured %ld frames" */
    Public Const IDS_CAP_STAT_CAP_AUDIO As Short = 509 '/* "Capturing audio" */
    Public Const IDS_CAP_STAT_VIDEOCURRENT As Short = 510 '/* "Captured %ld frames (%ld dropped) %d.%03d sec." */
    Public Const IDS_CAP_STAT_VIDEOAUDIO As Short = 511 '/* "Captured %d.%03d sec.  %ld frames (%ld dropped) (%d.%03d fps).  %ld audio bytes (%d,%03d sps)" */
    Public Const IDS_CAP_STAT_VIDEOONLY As Short = 512 '/* "Captured %d.%03d sec.  %ld frames (%ld dropped) (%d.%03d fps)" */
    Function capSetCallbackOnError(ByVal lwnd As Integer, ByVal lpProc As Integer) As Boolean
        capSetCallbackOnError = SendMessage(lwnd, WM_CAP_SET_CALLBACK_ERROR, 0, lpProc)
    End Function
    Function capSetCallbackOnStatus(ByVal lwnd As Integer, ByVal lpProc As Integer) As Boolean
        capSetCallbackOnStatus = SendMessage(lwnd, WM_CAP_SET_CALLBACK_STATUS, 0, lpProc)
    End Function
    Function capSetCallbackOnYield(ByVal lwnd As Integer, ByVal lpProc As Integer) As Boolean
        capSetCallbackOnYield = SendMessage(lwnd, WM_CAP_SET_CALLBACK_YIELD, 0, lpProc)
    End Function
    Function capSetCallbackOnFrame(ByVal lwnd As Integer, ByVal lpProc As Integer) As Boolean
        capSetCallbackOnFrame = SendMessage(lwnd, WM_CAP_SET_CALLBACK_FRAME, 0, lpProc)
    End Function
    Function capSetCallbackOnVideoStream(ByVal lwnd As Integer, ByVal lpProc As Integer) As Boolean
        capSetCallbackOnVideoStream = SendMessage(lwnd, WM_CAP_SET_CALLBACK_VIDEOSTREAM, 0, lpProc)
    End Function
    Function capSetCallbackOnWaveStream(ByVal lwnd As Integer, ByVal lpProc As Integer) As Boolean
        capSetCallbackOnWaveStream = SendMessage(lwnd, WM_CAP_SET_CALLBACK_WAVESTREAM, 0, lpProc)
    End Function
    Function capSetCallbackOnCapControl(ByVal lwnd As Integer, ByVal lpProc As Integer) As Boolean
        capSetCallbackOnCapControl = SendMessage(lwnd, WM_CAP_SET_CALLBACK_CAPCONTROL, 0, lpProc)
    End Function
    Function capSetUserData(ByVal lwnd As Integer, ByVal lUser As Integer) As Boolean
        capSetUserData = SendMessage(lwnd, WM_CAP_SET_USER_DATA, 0, lUser)
    End Function
    Function capGetUserData(ByVal lwnd As Integer) As Integer
        capGetUserData = SendMessage(lwnd, WM_CAP_GET_USER_DATA, 0, 0)
    End Function
    Function capDriverConnect(ByVal lwnd As Integer, ByVal i As Short) As Boolean
        capDriverConnect = SendMessage(lwnd, WM_CAP_DRIVER_CONNECT, i, 0)
    End Function
    Function capDriverDisconnect(ByVal lwnd As Integer) As Boolean
        capDriverDisconnect = SendMessage(lwnd, WM_CAP_DRIVER_DISCONNECT, 0, 0)
    End Function
    Function capDriverGetName(ByVal lwnd As Integer, ByVal szName As Integer, ByVal wSize As Short) As Boolean
        Dim YOURCONSTANTMESSAGE As Object
        capDriverGetName = SendMessage(lwnd, YOURCONSTANTMESSAGE, wSize, szName)
    End Function
    Function capDriverGetVersion(ByVal lwnd As Integer, ByVal szVer As Integer, ByVal wSize As Short) As Boolean
        capDriverGetVersion = SendMessage(lwnd, WM_CAP_DRIVER_GET_VERSION, wSize, szVer)
    End Function
    Function capDriverGetCaps(ByVal lwnd As Integer, ByVal s As Integer, ByVal wSize As Short) As Boolean
        capDriverGetCaps = SendMessage(lwnd, WM_CAP_DRIVER_GET_CAPS, wSize, s)
    End Function
    Function capFileSetCaptureFile(ByVal lwnd As Integer, ByRef szName As String) As Boolean
        capFileSetCaptureFile = SendMessageS(lwnd, WM_CAP_FILE_SET_CAPTURE_FILE, 0, szName)
    End Function
    Function capFileGetCaptureFile(ByVal lwnd As Integer, ByVal szName As Integer, ByRef wSize As String) As Boolean
        capFileGetCaptureFile = SendMessageS(lwnd, WM_CAP_FILE_SET_CAPTURE_FILE, CShort(wSize), CStr(szName))
    End Function
    Function capFileAlloc(ByVal lwnd As Integer, ByVal dwSize As Integer) As Boolean
        capFileAlloc = SendMessage(lwnd, WM_CAP_FILE_ALLOCATE, 0, dwSize)
    End Function
    Function capFileSaveAs(ByVal lwnd As Integer, ByRef szName As String) As Boolean
        capFileSaveAs = SendMessageS(lwnd, WM_CAP_FILE_SAVEAS, 0, szName)
    End Function
    Function capFileSetInfoChunk(ByVal lwnd As Integer, ByVal lpInfoChunk As Integer) As Boolean
        capFileSetInfoChunk = SendMessage(lwnd, WM_CAP_FILE_SET_INFOCHUNK, 0, lpInfoChunk)
    End Function
    Function capFileSaveDIB(ByVal lwnd As Integer, ByVal szName As Integer) As Boolean
        capFileSaveDIB = SendMessage(lwnd, WM_CAP_FILE_SAVEDIB, 0, szName)
    End Function
    Function capEditCopy(ByVal lwnd As Integer) As Boolean
        capEditCopy = SendMessage(lwnd, WM_CAP_EDIT_COPY, 0, 0)
    End Function
    Function capSetAudioFormat(ByVal lwnd As Integer, ByVal s As Integer, ByVal wSize As Short) As Boolean
        capSetAudioFormat = SendMessage(lwnd, WM_CAP_SET_AUDIOFORMAT, wSize, s)
    End Function
    Function capGetAudioFormat(ByVal lwnd As Integer, ByVal s As Integer, ByVal wSize As Short) As Integer
        capGetAudioFormat = SendMessage(lwnd, WM_CAP_GET_AUDIOFORMAT, wSize, s)
    End Function
    Function capGetAudioFormatSize(ByVal lwnd As Integer) As Integer
        capGetAudioFormatSize = SendMessage(lwnd, WM_CAP_GET_AUDIOFORMAT, 0, 0)
    End Function
    Function capDlgVideoFormat(ByVal lwnd As Integer) As Boolean
        capDlgVideoFormat = SendMessage(lwnd, WM_CAP_DLG_VIDEOFORMAT, 0, 0)
    End Function
    Function capDlgVideoSource(ByVal lwnd As Integer) As Boolean
        capDlgVideoSource = SendMessage(lwnd, WM_CAP_DLG_VIDEOSOURCE, 0, 0)
    End Function
    Function capDlgVideoDisplay(ByVal lwnd As Integer) As Boolean
        capDlgVideoDisplay = SendMessage(lwnd, WM_CAP_DLG_VIDEODISPLAY, 0, 0)
    End Function
    Function capDlgVideoCompression(ByVal lwnd As Integer) As Boolean
        capDlgVideoCompression = SendMessage(lwnd, WM_CAP_DLG_VIDEOCOMPRESSION, 0, 0)
    End Function
    Function capGetVideoFormat(ByVal lwnd As Integer, ByVal s As Integer, ByVal wSize As Short) As Integer
        capGetVideoFormat = SendMessage(lwnd, WM_CAP_GET_VIDEOFORMAT, wSize, s)
    End Function
    Function capGetVideoFormatSize(ByVal lwnd As Integer) As Integer
        capGetVideoFormatSize = SendMessage(lwnd, WM_CAP_GET_VIDEOFORMAT, 0, 0)
    End Function
    Function capSetVideoFormat(ByVal lwnd As Integer, ByVal s As Integer, ByVal wSize As Short) As Boolean
        capSetVideoFormat = SendMessage(lwnd, WM_CAP_SET_VIDEOFORMAT, wSize, s)
    End Function
    Function capPreview(ByVal lwnd As Integer, ByVal f As Boolean) As Boolean
        capPreview = SendMessage(lwnd, WM_CAP_SET_PREVIEW, f, 0)
    End Function
    Function capPreviewRate(ByVal lwnd As Integer, ByVal wMS As Short) As Boolean
        capPreviewRate = SendMessage(lwnd, WM_CAP_SET_PREVIEWRATE, wMS, 0)
    End Function
    Function capOverlay(ByVal lwnd As Integer, ByVal f As Boolean) As Boolean
        capOverlay = SendMessage(lwnd, WM_CAP_SET_OVERLAY, f, 0)
    End Function
    Function capPreviewScale(ByVal lwnd As Integer, ByVal f As Boolean) As Boolean
        capPreviewScale = SendMessage(lwnd, WM_CAP_SET_SCALE, f, 0)
    End Function
    Function capGetStatus(ByVal lwnd As Integer, ByVal s As Integer, ByVal wSize As Short) As Boolean
        capGetStatus = SendMessage(lwnd, WM_CAP_GET_STATUS, wSize, s)
    End Function
    Function capSetScrollPos(ByVal lwnd As Integer, ByVal lpP As Integer) As Boolean
        capSetScrollPos = SendMessage(lwnd, WM_CAP_SET_SCROLL, 0, lpP)
    End Function
    Function capGrabFrame(ByVal lwnd As Integer) As Boolean
        capGrabFrame = SendMessage(lwnd, WM_CAP_GRAB_FRAME, 0, 0)
    End Function
    Function capGrabFrameNoStop(ByVal lwnd As Integer) As Boolean
        capGrabFrameNoStop = SendMessage(lwnd, WM_CAP_GRAB_FRAME_NOSTOP, 0, 0)
    End Function
    Function capCaptureSequence(ByVal lwnd As Integer) As Boolean
        capCaptureSequence = SendMessage(lwnd, WM_CAP_SEQUENCE, 0, 0)
    End Function
    Function capCaptureSequenceNoFile(ByVal lwnd As Integer) As Boolean
        capCaptureSequenceNoFile = SendMessage(lwnd, WM_CAP_SEQUENCE_NOFILE, 0, 0)
    End Function
    Function capCaptureStop(ByVal lwnd As Integer) As Boolean
        capCaptureStop = SendMessage(lwnd, WM_CAP_STOP, 0, 0)
    End Function
    Function capCaptureAbort(ByVal lwnd As Integer) As Boolean
        capCaptureAbort = SendMessage(lwnd, WM_CAP_ABORT, 0, 0)
    End Function
    Function capCaptureSingleFrameOpen(ByVal lwnd As Integer) As Boolean
        capCaptureSingleFrameOpen = SendMessage(lwnd, WM_CAP_SINGLE_FRAME_OPEN, 0, 0)
    End Function
    Function capCaptureSingleFrameClose(ByVal lwnd As Integer) As Boolean
        capCaptureSingleFrameClose = SendMessage(lwnd, WM_CAP_SINGLE_FRAME_CLOSE, 0, 0)
    End Function
    Function capCaptureSingleFrame(ByVal lwnd As Integer) As Boolean
        capCaptureSingleFrame = SendMessage(lwnd, WM_CAP_SINGLE_FRAME, 0, 0)
    End Function
    Function capCaptureGetSetup(ByVal lwnd As Integer, ByVal s As Integer, ByVal wSize As Short) As Boolean
        capCaptureGetSetup = SendMessage(lwnd, WM_CAP_GET_SEQUENCE_SETUP, wSize, s)
    End Function
    Function capCaptureSetSetup(ByVal lwnd As Integer, ByVal s As Integer, ByVal wSize As Short) As Boolean
        capCaptureSetSetup = SendMessage(lwnd, WM_CAP_SET_SEQUENCE_SETUP, wSize, s)
    End Function
    Function capSetMCIDeviceName(ByVal lwnd As Integer, ByVal szName As Integer) As Boolean
        capSetMCIDeviceName = SendMessage(lwnd, WM_CAP_SET_MCI_DEVICE, 0, szName)
    End Function
    Function capGetMCIDeviceName(ByVal lwnd As Integer, ByVal szName As Integer, ByVal wSize As Short) As Boolean
        capGetMCIDeviceName = SendMessage(lwnd, WM_CAP_GET_MCI_DEVICE, wSize, szName)
    End Function
    Function capPaletteOpen(ByVal lwnd As Integer, ByVal szName As Integer) As Boolean
        capPaletteOpen = SendMessage(lwnd, WM_CAP_PAL_OPEN, 0, szName)
    End Function
    Function capPaletteSave(ByVal lwnd As Integer, ByVal szName As Integer) As Boolean
        capPaletteSave = SendMessage(lwnd, WM_CAP_PAL_SAVE, 0, szName)
    End Function
    Function capPalettePaste(ByVal lwnd As Integer) As Boolean
        capPalettePaste = SendMessage(lwnd, WM_CAP_PAL_PASTE, 0, 0)
    End Function
    Function capPaletteAuto(ByVal lwnd As Integer, ByVal iFrames As Short, ByVal iColor As Integer) As Boolean
        Dim iColors As Object
        capPaletteAuto = SendMessage(lwnd, WM_CAP_PAL_AUTOCREATE, iFrames, iColors)
    End Function
    Function capPaletteManual(ByVal lwnd As Integer, ByVal fGrab As Boolean, ByVal iColors As Integer) As Boolean
        capPaletteManual = SendMessage(lwnd, WM_CAP_PAL_MANUALCREATE, fGrab, iColors)
    End Function
End Module
