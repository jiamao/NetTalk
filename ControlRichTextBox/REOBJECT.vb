Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports System.IO

<StructLayout(LayoutKind.Sequential)> _
Public Class REOBJECT
    Public cbStruct As Integer
    Public cp As Integer
    Public clsid As Guid
    Public poleobj As IntPtr
    Public pstg As IStorage
    Public polesite As IOleClientSite
    Public sizel As Size
    Public dvAspect As UInt32
    Public dwFlags As UInt32
    Public dwUser As UInt32
End Class

<ComVisible(False)> _
Public Enum CLIPFORMAT
    ' Fields
    CF_BITMAP = 2
    CF_DIB = 8
    CF_DIF = 5
    CF_DSPBITMAP = 130
    CF_DSPENHMETAFILE = &H8E
    CF_DSPMETAFILEPICT = &H83
    CF_DSPTEXT = &H81
    CF_ENHMETAFILE = 14
    CF_HDROP = 15
    CF_LOCALE = &H10
    CF_MAX = &H11
    CF_METAFILEPICT = 3
    CF_OEMTEXT = 7
    CF_OWNERDISPLAY = &H80
    CF_PALETTE = 9
    CF_PENDATA = 10
    CF_RIFF = 11
    CF_SYLK = 4
    CF_TEXT = 1
    CF_TIFF = 6
    CF_UNICODETEXT = 13
    CF_WAVE = 12
End Enum




