Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports System.IO

Public Class ExtRichTextBox
    Inherits RichTextBox
    ' Methods
    Public Sub New()
        Me.DetectUrls = True
        Using graphics As Graphics = MyBase.CreateGraphics
            Me.xDpi = graphics.DpiX
            Me.yDpi = graphics.DpiY
        End Using
    End Sub

    <DllImport("gdiplus.dll")> _
    Private Shared Function GdipEmfToWmfBits(ByVal _hEmf As IntPtr, ByVal _bufferSize As UInt32, ByVal _buffer As Byte(), ByVal _mappingMode As Integer, ByVal _flags As EmfToWmfBitsFlags) As UInt32
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)> _
    Friend Shared Function GetClientRect(ByVal hWnd As IntPtr, <[In](), Out()> ByRef rect As Rectangle) As Boolean
    End Function

    Private Function GetImagePrefix(ByVal _image As Image) As String
        Dim builder As New StringBuilder
        Dim num As Integer = CInt(Math.Round(CDbl(((CSng(_image.Width) / Me.xDpi) * 2540.0!))))
        Dim num2 As Integer = CInt(Math.Round(CDbl(((CSng(_image.Height) / Me.yDpi) * 2540.0!))))
        Dim num3 As Integer = CInt(Math.Round(CDbl(((CSng(_image.Width) / Me.xDpi) * 1440.0!))))
        Dim num4 As Integer = CInt(Math.Round(CDbl(((CSng(_image.Height) / Me.yDpi) * 1440.0!))))
        builder.Append("{\pict\wmetafile8")
        builder.Append("\picw")
        builder.Append(num)
        builder.Append("\pich")
        builder.Append(num2)
        builder.Append("\picwgoal")
        builder.Append(num3)
        builder.Append("\pichgoal")
        builder.Append(num4)
        builder.Append(" ")
        Return builder.ToString
    End Function

    Private Function GetNeiMa(ByVal [text] As String) As String
        Dim str As String = ""
        Dim i As Integer
        For i = 0 To [text].Length - 1
            Dim num2 As Integer = AscW([text].Chars(i))
            If (num2 > 160) Then
                Dim ch As Char = [text].Chars(i)
                Dim bytes As Byte() = Encoding.GetEncoding(&H3A8).GetBytes(ch.ToString)
                Dim str2 As String = str
                str = String.Concat(New String() {str2, "\'", bytes(0).ToString("x2"), "\'", bytes(1).ToString("x2")})
            Else
                str = (str & [text].Chars(i))
            End If
        Next i
        Return str
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)> _
    Friend Shared Function GetParent(ByVal hWnd As IntPtr) As IntPtr
    End Function

    Private Function GetRtfImage(ByVal _image As Image) As String
        Dim builder As StringBuilder = Nothing
        Dim stream As MemoryStream = Nothing
        Dim mygraphics As Graphics = Nothing
        Dim myimage As Metafile = Nothing
        Dim str As String
        Dim hdc As IntPtr
        Try
            builder = New StringBuilder
            stream = New MemoryStream()
            '_image.Save(stream, ImageFormat.Gif)

            mygraphics = Me.CreateGraphics()
            hdc = mygraphics.GetHdc

            myimage = New Metafile(stream, hdc)
            mygraphics.ReleaseHdc(hdc)

            'mygraphics.Dispose()

            'mygraphics = 
            Graphics.FromImage(myimage).DrawImage(_image, New Rectangle(0, 0, _image.Width, _image.Height))

            Dim henhmetafile As IntPtr = myimage.GetHenhmetafile()

            Dim num As UInt32 = ExtRichTextBox.GdipEmfToWmfBits(henhmetafile, 0, Nothing, 8, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault)
            Dim buffer As Byte() = New Byte(num - 1) {}
            ExtRichTextBox.GdipEmfToWmfBits(henhmetafile, num, buffer, 8, EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault)
            Dim i As Integer
            For i = 0 To buffer.Length - 1
                builder.Append(String.Format("{0:X2}", buffer(i)))
            Next i
            str = builder.ToString
        Finally
            If (Not mygraphics Is Nothing) Then
                mygraphics.Dispose()
            End If
            If (Not myimage Is Nothing) Then
                myimage.Dispose()
            End If
            If (Not stream Is Nothing) Then
                stream.Close()
            End If
        End Try
        Return str
    End Function

    Public Function GetSelectionLink() As Integer
        Return Me.GetSelectionStyle(&H20, &H20)
    End Function

    Private Function GetSelectionStyle(ByVal mask As UInt32, ByVal effect As UInt32) As Integer
        Dim num As Integer
        Dim [structure] As New CHARFORMAT2_STRUCT
        [structure].cbSize = DirectCast(UInteger.Parse(Marshal.SizeOf([structure])), UInt32)
        [structure].szFaceName = New Char(&H20 - 1) {}
        Dim wParam As New IntPtr(1)
        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf([structure]))
        Marshal.StructureToPtr([structure], ptr, False)
        ExtRichTextBox.SendMessage(MyBase.Handle, &H43A, wParam, ptr)
        [structure] = DirectCast(Marshal.PtrToStructure(ptr, GetType(CHARFORMAT2_STRUCT)), CHARFORMAT2_STRUCT)
        If (([structure].dwMask And mask) = mask) Then
            If (([structure].dwEffects And effect) = effect) Then
                num = 1
            Else
                num = 0
            End If
        Else
            num = -1
        End If
        Marshal.FreeCoTaskMem(ptr)
        Return num
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)> _
    Friend Shared Function GetWindowRect(ByVal hWnd As IntPtr, <[In](), Out()> ByRef rect As Rectangle) As Boolean
    End Function

    'Public Sub InsertImage(ByVal _image As Image)
    '    Dim builder As New StringBuilder
    '    builder.Append("{\rtf1\ansi\ansicpg1252\deff0\deflang1033")
    '    builder.Append(Me.GetImagePrefix(_image))
    '    builder.Append(Me.GetRtfImage(_image))
    '    builder.Append(Me.RTF_IMAGE_POST)
    '    MyBase.SelectedRtf = builder.ToString
    'End Sub

    'Public Sub InsertImage(ByVal imageFile As String)
    '    Dim image As Image = Image.FromFile(imageFile)
    '    Me.InsertImage(image)
    'End Sub

    Public Sub InsertLink(ByVal [text] As String)
        Me.InsertLink([text], MyBase.SelectionStart)
    End Sub

    Public Sub InsertLink(ByVal [text] As String, ByVal position As Integer)
        If ((position < 0) OrElse (position > Me.Text.Length)) Then
            Throw New ArgumentOutOfRangeException("position")
        End If
        MyBase.SelectionStart = position
        Me.SelectedText = [text]

        MyBase.Select(position, [text].Length)
        Me.SetSelectionLink(True)
        MyBase.Select((position + [text].Length), 0)

    End Sub

    Public Sub InsertLink(ByVal [text] As String, ByVal hyperlink As String)
        Me.InsertLink([text], hyperlink, MyBase.SelectionStart)
    End Sub

    Public Sub InsertLink(ByVal [text] As String, ByVal hyperlink As String, ByVal position As Integer)
        If ((position < 0) OrElse (position > Me.Text.Length)) Then
            Throw New ArgumentOutOfRangeException("position")
        End If
        MyBase.SelectionStart = position
        MyBase.SelectedRtf = String.Concat(New String() {"{\rtf1\ansicpg936 ", Me.GetNeiMa([text]), "\v #", Me.GetNeiMa(hyperlink), "\v0\par}"})

        MyBase.Select(position, (([text].Length + hyperlink.Length) + 1))
        Me.SetSelectionLink(True)
        MyBase.Select((((position + [text].Length) + hyperlink.Length) + 1), 0)
    End Sub

    <DllImport("ole32.dll")> _
    Private Shared Function OleCreateFromData(ByVal pSrcDataObj As IDataObject, <[In]()> ByRef riid As Guid, ByVal renderopt As UInt32, ByRef pFormatEtc As FORMATETC, ByVal pClientSite As IOleClientSite, ByVal pStg As IStorage, <Out(), MarshalAs(UnmanagedType.IUnknown)> ByRef ppvObj As Object) As Integer
    End Function

    <DllImport("ole32.dll")> _
    Private Shared Function OleCreateFromFile(<[In]()> ByRef rclsid As Guid, <MarshalAs(UnmanagedType.LPWStr)> ByVal lpszFileName As String, <[In]()> ByRef riid As Guid, ByVal renderopt As UInt32, ByRef pFormatEtc As FORMATETC, ByVal pClientSite As IOleClientSite, ByVal pStg As IStorage, <Out(), MarshalAs(UnmanagedType.IUnknown)> ByRef ppvObj As Object) As Integer
    End Function

    <DllImport("ole32.dll")> _
    Private Shared Function OleCreateLinkFromData(<MarshalAs(UnmanagedType.Interface)> ByVal pSrcDataObj As IDataObject, <[In]()> ByRef riid As Guid, ByVal renderopt As UInt32, ByRef pFormatEtc As FORMATETC, ByVal pClientSite As IOleClientSite, ByVal pStg As IStorage, <Out(), MarshalAs(UnmanagedType.IUnknown)> ByRef ppvObj As Object) As Integer
    End Function

    <DllImport("ole32.dll")> _
    Private Shared Function OleCreateStaticFromData(<MarshalAs(UnmanagedType.Interface)> ByVal pSrcDataObj As IDataObject, <[In]()> ByRef riid As Guid, ByVal renderopt As UInt32, ByRef pFormatEtc As FORMATETC, ByVal pClientSite As IOleClientSite, ByVal pStg As IStorage, <Out(), MarshalAs(UnmanagedType.IUnknown)> ByRef ppvObj As Object) As Integer
    End Function

    <DllImport("ole32.dll")> _
    Private Shared Function OleLoadPicturePath(<MarshalAs(UnmanagedType.LPWStr)> ByVal lpszPicturePath As String, <[In](), MarshalAs(UnmanagedType.IUnknown)> ByVal pIUnknown As Object, ByVal dwReserved As UInt32, ByVal clrReserved As UInt32, ByRef riid As Guid, <Out(), MarshalAs(UnmanagedType.IUnknown)> ByRef ppvObj As Object) As Integer
    End Function

    <DllImport("ole32.dll")> _
    Private Shared Function OleSetContainedObject(<MarshalAs(UnmanagedType.IUnknown)> ByVal pUnk As Object, ByVal fContained As Boolean) As Integer
    End Function

    Public Sub ScrollToEnd()
        MyBase.SelectionStart = MyBase.Text.Length
        Dim m As Message = Message.Create(MyBase.Handle, &H115, Me.SB_BOTTOM, IntPtr.Zero)
        MyBase.WndProc((m))
    End Sub

    <DllImport("User32.dll", CharSet:=CharSet.Auto, PreserveSig:=False)> _
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal message As Integer, ByVal wParam As Integer) As IRichEditOle
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32", CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As HandleRef, ByVal msg As Integer, ByVal wParam As Integer, ByRef lp As PARAFORMAT) As Integer
    End Function

    <DllImport("user32", CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As HandleRef, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    <DllImport("user32", CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As HandleRef, ByVal msg As Integer, ByVal wParam As Integer, ByRef lp As CHARFORMAT) As Integer
    End Function

    Public Sub SetSelectionLink(ByVal link As Boolean)
        Me.SetSelectionStyle(&H20, IIf(link, &H20, 0))
    End Sub

    Private Sub SetSelectionStyle(ByVal mask As UInt32, ByVal effect As UInt32)
        Dim [structure] As New CHARFORMAT2_STRUCT
        [structure].cbSize = CType(Marshal.SizeOf([structure]), UInt32)
        [structure].dwMask = mask
        [structure].dwEffects = effect
        Dim wParam As New IntPtr(1)
        Dim ptr As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf([structure]))
        Marshal.StructureToPtr([structure], ptr, False)
        ExtRichTextBox.SendMessage(MyBase.Handle, &H444, wParam, ptr)
        Marshal.FreeCoTaskMem(ptr)
    End Sub


    ' Properties
    '<DefaultValue(False)> _
    Public Property DetectUrls() As Boolean
        Get
            Return MyBase.DetectUrls
        End Get
        Set(ByVal value As Boolean)
            MyBase.DetectUrls = value
        End Set
    End Property


    ' Fields
    Private Const CFE_AUTOCOLOR As UInt32 = &H40000000
    Private Const CFE_BOLD As UInt32 = 1
    Private Const CFE_ITALIC As UInt32 = 2
    Private Const CFE_LINK As UInt32 = &H20
    Private Const CFE_PROTECTED As UInt32 = &H10
    Private Const CFE_STRIKEOUT As UInt32 = 8
    Private Const CFE_SUBSCRIPT As UInt32 = &H10000
    Private Const CFE_SUPERSCRIPT As UInt32 = &H20000
    Private Const CFE_UNDERLINE As UInt32 = 4
    Private Const CFM_ALLCAPS As Integer = &H80
    Private Const CFM_ANIMATION As Integer = &H40000
    Private Const CFM_BACKCOLOR As Integer = &H4000000
    Private Const CFM_BOLD As UInt32 = 1
    Private Const CFM_CHARSET As UInt32 = &H8000000
    Private Const CFM_COLOR As UInt32 = &H40000000
    Private Const CFM_DISABLED As Integer = &H2000
    Private Const CFM_EMBOSS As Integer = &H800
    Private Const CFM_FACE As UInt32 = &H20000000
    Private Const CFM_HIDDEN As Integer = &H100
    Private Const CFM_IMPRINT As Integer = &H1000
    Private Const CFM_ITALIC As UInt32 = 2
    Private Const CFM_KERNING As Integer = &H100000
    Private Const CFM_LCID As Integer = &H2000000
    Private Const CFM_LINK As UInt32 = &H20
    Private Const CFM_OFFSET As UInt32 = &H10000000
    Private Const CFM_OUTLINE As Integer = &H200
    Private Const CFM_PROTECTED As UInt32 = &H10
    Private Const CFM_REVAUTHOR As Integer = &H8000
    Private Const CFM_REVISED As Integer = &H4000
    Private Const CFM_SHADOW As Integer = &H400
    Private Const CFM_SIZE As Integer = &H80000000
    Private Const CFM_SMALLCAPS As Integer = &H40
    Private Const CFM_SPACING As Integer = &H200000
    Private Const CFM_STRIKEOUT As UInt32 = 8
    Private Const CFM_STYLE As Integer = &H80000
    Private Const CFM_SUBSCRIPT As UInt32 = &H30000
    Private Const CFM_SUPERSCRIPT As UInt32 = &H30000
    Private Const CFM_UNDERLINE As UInt32 = 4
    Private Const CFM_UNDERLINETYPE As Integer = &H800000
    Private Const CFM_WEIGHT As Integer = &H400000
    Private Const CFU_UNDERLINE As Byte = 1
    Private Const CFU_UNDERLINEDASH As Byte = 5
    Private Const CFU_UNDERLINEDASHDOT As Byte = 6
    Private Const CFU_UNDERLINEDASHDOTDOT As Byte = 7
    Private Const CFU_UNDERLINEDOTTED As Byte = 4
    Private Const CFU_UNDERLINEDOUBLE As Byte = 3
    Private Const CFU_UNDERLINEHAIRLINE As Byte = 10
    Private Const CFU_UNDERLINENONE As Byte = 0
    Private Const CFU_UNDERLINETHICK As Byte = 9
    Private Const CFU_UNDERLINEWAVE As Byte = 8
    Private Const CFU_UNDERLINEWORD As Byte = 2
    Private Const EM_GETCHARFORMAT As Integer = &H43A
    Private Const EM_SETCHARFORMAT As Integer = &H444
    Private Const EM_SETEVENTMASK As Integer = &H431
    Private Const HMM_PER_INCH As Integer = &H9EC
    Private Const MM_ANISOTROPIC As Integer = 8
    Private Const RTF_HEADER As String = "{\rtf1\ansi\ansicpg1252\deff0\deflang1033"
    Private RTF_IMAGE_POST As String = "}"
    Private ReadOnly SB_BOTTOM As IntPtr = CType(7, IntPtr)
    Private Const SCF_ALL As Integer = 4
    Private Const SCF_SELECTION As Integer = 1
    Private Const SCF_WORD As Integer = 2
    Private Const TWIPS_PER_INCH As Integer = &H5A0
    Private Const WM_SETREDRAW As Integer = 11
    Private Const WM_USER As Integer = &H400
    Private Const WM_VSCROLL As Integer = &H115
    Private xDpi As Single
    Private yDpi As Single

    ' Nested Types
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure CHARFORMAT
        Public cbSize As Integer
        Public dwMask As UInt32
        Public dwEffects As UInt32
        Public yHeight As Integer
        Public yOffset As Integer
        Public crTextColor As Integer
        Public bCharSet As Byte
        Public bPitchAndFamily As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=&H20)> _
        Public szFaceName As Char()
        Public wWeight As Short
        Public sSpacing As Short
        Public crBackColor As Integer
        Public lcid As UInt32
        Public dwReserved As UInt32
        Public sStyle As Short
        Public wKerning As Short
        Public bUnderlineType As Byte
        Public bAnimation As Byte
        Public bRevAuthor As Byte
        Public bReserved1 As Byte
    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Private Structure CHARFORMAT2_STRUCT
        Public cbSize As UInt32
        Public dwMask As UInt32
        Public dwEffects As UInt32
        Public yHeight As Integer
        Public yOffset As Integer
        Public crTextColor As Integer
        Public bCharSet As Byte
        Public bPitchAndFamily As Byte
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=&H20)> _
        Public szFaceName As Char()
        Public wWeight As UInt16
        Public sSpacing As UInt16
        Public crBackColor As Integer
        Public lcid As Integer
        Public dwReserved As Integer
        Public sStyle As Short
        Public wKerning As Short
        Public bUnderlineType As Byte
        Public bAnimation As Byte
        Public bRevAuthor As Byte
        Public bReserved1 As Byte
    End Structure

    Private Enum EmfToWmfBitsFlags
        ' Fields
        EmfToWmfBitsFlagsDefault = 0
        EmfToWmfBitsFlagsEmbedEmf = 1
        EmfToWmfBitsFlagsIncludePlaceable = 2
        EmfToWmfBitsFlagsNoXORClip = 4
    End Enum

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure PARAFORMAT
        Public cbSize As Integer
        Public dwMask As UInt32
        Public wNumbering As Short
        Public wReserved As Short
        Public dxStartIndent As Integer
        Public dxRightIndent As Integer
        Public dxOffset As Integer
        Public wAlignment As Short
        Public cTabCount As Short
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=&H20)> _
        Public rgxTabs As Integer()
        Public dySpaceBefore As Integer
        Public dySpaceAfter As Integer
        Public dyLineSpacing As Integer
        Public sStyle As Short
        Public bLineSpacingRule As Byte
        Public bOutlineLevel As Byte
        Public wShadingWeight As Short
        Public wShadingStyle As Short
        Public wNumberingStart As Short
        Public wNumberingStyle As Short
        Public wNumberingTab As Short
        Public wBorderSpace As Short
        Public wBorderWidth As Short
        Public wBorders As Short
    End Structure
End Class

<Guid("00000118-0000-0000-C000-000000000046"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IOleClientSite
    <PreserveSig()> _
    Function SaveObject() As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetMoniker(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwAssign As Integer, <[In](), MarshalAs(UnmanagedType.U4)> ByVal dwWhichMoniker As Integer, <Out(), MarshalAs(UnmanagedType.Interface)> ByRef ppmk As Object) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetContainer(<Out(), MarshalAs(UnmanagedType.Interface)> ByRef container As IOleContainer) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function ShowObject() As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function OnShowWindow(<[In](), MarshalAs(UnmanagedType.I4)> ByVal fShow As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function RequestNewObjectLayout() As <MarshalAs(UnmanagedType.I4)> Integer
End Interface
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(True), Guid("0000011B-0000-0000-C000-000000000046")> _
Public Interface IOleContainer
    Sub ParseDisplayName(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pbc As Object, <[In](), MarshalAs(UnmanagedType.BStr)> ByVal pszDisplayName As String, <Out(), MarshalAs(UnmanagedType.LPArray)> ByVal pchEaten As Integer(), <Out(), MarshalAs(UnmanagedType.LPArray)> ByVal ppmkOut As Object())
    Sub EnumObjects(<[In](), MarshalAs(UnmanagedType.U4)> ByVal grfFlags As Integer, <Out(), MarshalAs(UnmanagedType.LPArray)> ByVal ppenum As Object())
    Sub LockContainer(<[In](), MarshalAs(UnmanagedType.I4)> ByVal fLock As Integer)
End Interface
<ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000b-0000-0000-C000-000000000046")> _
Public Interface IStorage
    Function CreateStream(ByVal pwcsName As String, ByVal grfMode As UInt32, ByVal reserved1 As UInt32, ByVal reserved2 As UInt32, <Out()> ByRef ppstm As IStream) As Integer
    Function OpenStream(ByVal pwcsName As String, ByVal reserved1 As IntPtr, ByVal grfMode As UInt32, ByVal reserved2 As UInt32, <Out()> ByRef ppstm As IStream) As Integer
    Function CreateStorage(ByVal pwcsName As String, ByVal grfMode As UInt32, ByVal reserved1 As UInt32, ByVal reserved2 As UInt32, <Out()> ByRef ppstg As IStorage) As Integer
    Function OpenStorage(ByVal pwcsName As String, ByVal pstgPriority As IStorage, ByVal grfMode As UInt32, ByVal snbExclude As IntPtr, ByVal reserved As UInt32, <Out()> ByRef ppstg As IStorage) As Integer
    Function CopyTo(ByVal ciidExclude As UInt32, ByVal rgiidExclude As Guid, ByVal snbExclude As IntPtr, ByVal pstgDest As IStorage) As Integer
    Function MoveElementTo(ByVal pwcsName As String, ByVal pstgDest As IStorage, ByVal pwcsNewName As String, ByVal grfFlags As UInt32) As Integer
    Function Commit(ByVal grfCommitFlags As UInt32) As Integer
    Function Revert() As Integer
    Function EnumElements(ByVal reserved1 As UInt32, ByVal reserved2 As IntPtr, ByVal reserved3 As UInt32, <Out()> ByRef ppenum As IEnumSTATSTG) As Integer
    Function DestroyElement(ByVal pwcsName As String) As Integer
    Function RenameElement(ByVal pwcsOldName As String, ByVal pwcsNewName As String) As Integer
    Function SetElementTimes(ByVal pwcsName As String, ByVal pctime As FILETIME, ByVal patime As FILETIME, ByVal pmtime As FILETIME) As Integer
    Function SetClass(ByVal clsid As Guid) As Integer
    Function SetStateBits(ByVal grfStateBits As UInt32, ByVal grfMask As UInt32) As Integer
    Function Stat(<Out()> ByRef pstatstg As STATSTG, ByVal grfStatFlag As UInt32) As Integer
End Interface
<ComImport(), Guid("00020D00-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IRichEditOle
    <PreserveSig()> _
    Function GetClientSite(<Out()> ByRef site As IOleClientSite) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetObjectCount() As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetLinkCount() As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetObject(ByVal iob As Integer, <[In](), Out()> ByVal lpreobject As REOBJECT, <MarshalAs(UnmanagedType.U4)> ByVal flags As GETOBJECTOPTIONS) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function InsertObject(ByVal lpreobject As REOBJECT) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function ConvertObject(ByVal iob As Integer, ByVal rclsidNew As Guid, ByVal lpstrUserTypeNew As String) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function ActivateAs(ByVal rclsid As Guid, ByVal rclsidAs As Guid) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function SetHostNames(ByVal lpstrContainerApp As String, ByVal lpstrContainerObj As String) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function SetLinkAvailable(ByVal iob As Integer, ByVal fAvailable As Boolean) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function SetDvaspect(ByVal iob As Integer, ByVal dvaspect As UInt32) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function HandsOffStorage(ByVal iob As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function SaveCompleted(ByVal iob As Integer, ByVal lpstg As IStorage) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function InPlaceDeactivate() As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function ContextSensitiveHelp(ByVal fEnterMode As Boolean) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetClipboardData(<[In](), Out()> ByRef lpchrg As CHARRANGE, <MarshalAs(UnmanagedType.U4)> ByVal reco As GETCLIPBOARDDATAFLAGS, <Out()> ByRef lplpdataobj As IDataObject) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function ImportDataObject(ByVal lpdataobj As IDataObject, ByVal cf As Integer, ByVal hMetaPict As IntPtr) As <MarshalAs(UnmanagedType.I4)> Integer
End Interface
Public Enum GETOBJECTOPTIONS
    ' Fields
    REO_GETOBJ_ALL_INTERFACES = 7
    REO_GETOBJ_NO_INTERFACES = 0
    REO_GETOBJ_POLEOBJ = 1
    REO_GETOBJ_POLESITE = 4
    REO_GETOBJ_PSTG = 2
End Enum
<StructLayout(LayoutKind.Sequential)> _
Public Structure CHARRANGE
    Public cpMin As Integer
    Public cpMax As Integer
End Structure
Public Enum GETCLIPBOARDDATAFLAGS
    ' Fields
    RECO_COPY = 2
    RECO_CUT = 3
    RECO_DRAG = 4
    RECO_DROP = 1
    RECO_PASTE = 0
End Enum

<ComImport(), Guid("0000000c-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IStream
    Inherits ISequentialStream
    Function Seek(ByVal dlibMove As UInt64, ByVal dwOrigin As UInt32, <Out()> ByRef plibNewPosition As UInt64) As Integer
    Function SetSize(ByVal libNewSize As UInt64) As Integer
    Function CopyTo(<[In]()> ByVal pstm As IStream, ByVal cb As UInt64, <Out()> ByRef pcbRead As UInt64, <Out()> ByRef pcbWritten As UInt64) As Integer
    Function Commit(ByVal grfCommitFlags As UInt32) As Integer
    Function Revert() As Integer
    Function LockRegion(ByVal libOffset As UInt64, ByVal cb As UInt64, ByVal dwLockType As UInt32) As Integer
    Function UnlockRegion(ByVal libOffset As UInt64, ByVal cb As UInt64, ByVal dwLockType As UInt32) As Integer
    Function Stat(<Out()> ByRef pstatstg As STATSTG, ByVal grfStatFlag As UInt32) As Integer
    Function Clone(<Out()> ByRef ppstm As IStream) As Integer
End Interface




<Guid("0c733a30-2a1c-11ce-ade5-00aa0044773d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface ISequentialStream
    Function Read(ByVal pv As IntPtr, ByVal cb As UInt32, <Out()> ByRef pcbRead As UInt32) As Integer
    Function Write(ByVal pv As IntPtr, ByVal cb As UInt32, <Out()> ByRef pcbWritten As UInt32) As Integer
End Interface


Public Interface IProgressCallback
    ' Methods
    Sub Begin()
    Sub Begin(ByVal minimum As Integer, ByVal maximum As Integer)
    Sub [End]()
    Sub Increment(ByVal val As Integer)
    Sub SetRange(ByVal minimum As Integer, ByVal maximum As Integer)
    Sub SetText(ByVal [text] As String)
    Sub StepTo(ByVal val As Integer)

    ' Properties
    ReadOnly Property IsAborting() As Boolean
End Interface




<Guid("00000109-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IPersistStream
    Inherits IPersist
    Sub GetClassID(<Out()> ByRef pClassID As Guid)
    <PreserveSig()> _
    Function IsDirty() As Integer
    Sub Load(<[In]()> ByVal pStm As UCOMIStream)
    Sub Save(<[In]()> ByVal pStm As UCOMIStream, <[In](), MarshalAs(UnmanagedType.Bool)> ByVal fClearDirty As Boolean)
    Sub GetSizeMax(<Out()> ByRef pcbSize As Long)
End Interface




<Guid("0000010c-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IPersist
    Sub GetClassID(<Out()> ByRef pClassID As Guid)
End Interface




<ComImport(), Guid("00000112-0000-0000-C000-000000000046"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IOleObject
    <PreserveSig()> _
    Function SetClientSite(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pClientSite As IOleClientSite) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetClientSite(<Out()> ByRef site As IOleClientSite) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function SetHostNames(<[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal szContainerApp As String, <[In](), MarshalAs(UnmanagedType.LPWStr)> ByVal szContainerObj As String) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function Close(<[In](), MarshalAs(UnmanagedType.I4)> ByVal dwSaveOption As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function SetMoniker(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwWhichMoniker As Integer, <[In](), MarshalAs(UnmanagedType.Interface)> ByVal pmk As Object) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetMoniker(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwAssign As Integer, <[In](), MarshalAs(UnmanagedType.U4)> ByVal dwWhichMoniker As Integer, <Out()> ByRef moniker As Object) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function InitFromData(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pDataObject As IDataObject, <[In](), MarshalAs(UnmanagedType.I4)> ByVal fCreation As Integer, <[In](), MarshalAs(UnmanagedType.U4)> ByVal dwReserved As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    Function GetClipboardData(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwReserved As Integer, <Out()> ByRef data As IDataObject) As Integer
    <PreserveSig()> _
    Function DoVerb(<[In](), MarshalAs(UnmanagedType.I4)> ByVal iVerb As Integer, <[In]()> ByVal lpmsg As IntPtr, <[In](), MarshalAs(UnmanagedType.Interface)> ByVal pActiveSite As IOleClientSite, <[In](), MarshalAs(UnmanagedType.I4)> ByVal lindex As Integer, <[In]()> ByVal hwndParent As IntPtr, <[In]()> ByVal lprcPosRect As COMRECT) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function EnumVerbs(<Out()> ByRef e As IEnumOLEVERB) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function Update() As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function IsUpToDate() As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetUserClassID(<[In](), Out()> ByRef pClsid As Guid) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetUserType(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwFormOfType As Integer, <Out(), MarshalAs(UnmanagedType.LPWStr)> ByRef userType As String) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function SetExtent(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwDrawAspect As Integer, <[In]()> ByVal pSizel As Size) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetExtent(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwDrawAspect As Integer, <Out()> ByVal pSizel As Size) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function Advise(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pAdvSink As IAdviseSink, <Out()> ByRef cookie As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function Unadvise(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwConnection As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function EnumAdvise(<Out()> ByRef e As IEnumSTATDATA) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function GetMiscStatus(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwAspect As Integer, <Out()> ByRef misc As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function SetColorScheme(<[In]()> ByVal pLogpal As tagLOGPALETTE) As <MarshalAs(UnmanagedType.I4)> Integer
End Interface




<ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000000a-0000-0000-C000-000000000046")> _
Public Interface ILockBytes
    Function ReadAt(ByVal ulOffset As UInt64, ByVal pv As IntPtr, ByVal cb As UInt32, <Out()> ByRef pcbRead As IntPtr) As Integer
    Function WriteAt(ByVal ulOffset As UInt64, ByVal pv As IntPtr, ByVal cb As UInt32, <Out()> ByRef pcbWritten As IntPtr) As Integer
    Function Flush() As Integer
    Function SetSize(ByVal cb As UInt64) As Integer
    Function LockRegion(ByVal libOffset As UInt64, ByVal cb As UInt64, ByVal dwLockType As UInt32) As Integer
    Function UnlockRegion(ByVal libOffset As UInt64, ByVal cb As UInt64, ByVal dwLockType As UInt32) As Integer
    Function Stat(<Out()> ByRef pstatstg As STATSTG, ByVal grfStatFlag As UInt32) As Integer
End Interface


<StructLayout(LayoutKind.Sequential)> _
Public NotInheritable Class tagLOGPALETTE
    <MarshalAs(UnmanagedType.U2)> _
    Public palVersion As Short
    <MarshalAs(UnmanagedType.U2)> _
    Public palNumEntries As Short
End Class






<ComImport(), Guid("0000000d-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IEnumSTATSTG
    <PreserveSig()> _
    Function [Next](ByVal celt As UInt32, <Out(), MarshalAs(UnmanagedType.LPArray)> ByVal rgelt As STATSTG(), <Out()> ByRef pceltFetched As UInt32) As UInt32
    Sub Skip(ByVal celt As UInt32)
    Sub Reset()
    Function Clone() As <MarshalAs(UnmanagedType.Interface)> IEnumSTATSTG
End Interface




<InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(True), Guid("00000105-0000-0000-C000-000000000046")> _
Public Interface IEnumSTATDATA
    Sub [Next](<[In](), MarshalAs(UnmanagedType.U4)> ByVal celt As Integer, <Out()> ByVal rgelt As STATDATA, <Out(), MarshalAs(UnmanagedType.LPArray)> ByVal pceltFetched As Integer())
    Sub Skip(<[In](), MarshalAs(UnmanagedType.U4)> ByVal celt As Integer)
    Sub Reset()
    Sub Clone(<Out(), MarshalAs(UnmanagedType.LPArray)> ByVal ppenum As IEnumSTATDATA())
End Interface




<ComImport(), Guid("00000104-0000-0000-C000-000000000046"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IEnumOLEVERB
    <PreserveSig()> _
    Function [Next](<MarshalAs(UnmanagedType.U4)> ByVal celt As Integer, <Out()> ByVal rgelt As tagOLEVERB, <Out(), MarshalAs(UnmanagedType.LPArray)> ByVal pceltFetched As Integer()) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function Skip(<[In](), MarshalAs(UnmanagedType.U4)> ByVal celt As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    Sub Reset()
    Sub Clone(<Out()> ByRef ppenum As IEnumOLEVERB)
End Interface

<StructLayout(LayoutKind.Sequential)> _
Public Structure STATDATA
    Public formatetc As FORMATETC
    Public advf As ADVF
    Public advSink As IAdviseSink
    Public connection As Integer
End Structure

<Flags()> _
Public Enum ADVF
    ' Fields
    ADVF_DATAONSTOP = &H40
    ADVF_NODATA = 1
    ADVF_ONLYONCE = 4
    ADVF_PRIMEFIRST = 2
    ADVFCACHE_FORCEBUILTIN = &H10
    ADVFCACHE_NOHANDLER = 8
    ADVFCACHE_ONSAVE = &H20
End Enum



<StructLayout(LayoutKind.Sequential)> _
Public Class COMRECT
    Public left As Integer
    Public top As Integer
    Public right As Integer
    Public bottom As Integer
    Public Sub New()
    End Sub

    Public Sub New(ByVal r As Rectangle)
        Me.left = r.X
        Me.top = r.Y
        Me.right = r.Right
        Me.bottom = r.Bottom
    End Sub

    Public Sub New(ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
        Me.left = left
        Me.top = top
        Me.right = right
        Me.bottom = bottom
    End Sub

    Public Shared Function FromXYWH(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer) As COMRECT
        Return New COMRECT(x, y, (x + width), (y + height))
    End Function

    Public Overrides Function ToString() As String
        Return String.Concat(New Object() {"Left = ", Me.left, " Top ", Me.top, " Right = ", Me.right, " Bottom = ", Me.bottom})
    End Function
End Class








<ComImport(), Guid("00000103-0000-0000-C000-000000000046"), ComVisible(True), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IEnumFORMATETC
    <PreserveSig()> _
    Function [Next](<[In](), MarshalAs(UnmanagedType.U4)> ByVal celt As Integer, <Out()> ByVal rgelt As FORMATETC, <[In](), Out(), MarshalAs(UnmanagedType.LPArray)> ByVal pceltFetched As Integer()) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function Skip(<[In](), MarshalAs(UnmanagedType.U4)> ByVal celt As Integer) As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function Reset() As <MarshalAs(UnmanagedType.I4)> Integer
    <PreserveSig()> _
    Function Clone(<Out(), MarshalAs(UnmanagedType.LPArray)> ByVal ppenum As IEnumFORMATETC()) As <MarshalAs(UnmanagedType.I4)> Integer
End Interface

<StructLayout(LayoutKind.Sequential)> _
Public NotInheritable Class tagOLEVERB
    Public lVerb As Integer
    <MarshalAs(UnmanagedType.LPWStr)> _
    Public lpszVerbName As String
    <MarshalAs(UnmanagedType.U4)> _
    Public fuFlags As Integer
    <MarshalAs(UnmanagedType.U4)> _
    Public grfAttribs As Integer
End Class





<ComVisible(True), Guid("0000010F-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IAdviseSink
    Sub OnDataChange(<[In]()> ByVal pFormatetc As FORMATETC, <[In]()> ByVal pStgmed As STGMEDIUM)
    Sub OnViewChange(<[In](), MarshalAs(UnmanagedType.U4)> ByVal dwAspect As Integer, <[In](), MarshalAs(UnmanagedType.I4)> ByVal lindex As Integer)
    Sub OnRename(<[In](), MarshalAs(UnmanagedType.Interface)> ByVal pmk As Object)
    Sub OnSave()
    Sub OnClose()
End Interface


<StructLayout(LayoutKind.Sequential)> _
Public Structure STGMEDIUM
    Public tymed As TYMED
    Public unionmember As IntPtr
    <MarshalAs(UnmanagedType.IUnknown)> _
    Public pUnkForRelease As Object
End Structure








<StructLayout(LayoutKind.Sequential), ComVisible(False)> _
Public Structure FORMATETC
    Public cfFormat As CLIPFORMAT
    Public ptd As IntPtr
    Public dwAspect As DVASPECT
    Public lindex As Integer
    Public tymed As TYMED
End Structure
<Flags()> _
Public Enum DVASPECT
    ' Fields
    DVASPECT_CONTENT = 1
    DVASPECT_DOCPRINT = 8
    DVASPECT_ICON = 4
    DVASPECT_THUMBNAIL = 2
End Enum
<Flags()> _
Public Enum TYMED
    ' Fields
    TYMED_ENHMF = &H40
    TYMED_FILE = 2
    TYMED_GDI = &H10
    TYMED_HGLOBAL = 1
    TYMED_ISTORAGE = 8
    TYMED_ISTREAM = 4
    TYMED_MFPICT = &H20
    TYMED_NULL = 0
End Enum







































