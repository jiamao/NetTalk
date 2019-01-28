using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Drawing.Imaging;

namespace RichTextBox
{
    #region "OLE definitions"
    // STGM
    [Flags(), ComVisible(false)]
    public enum STGM : int
    {
        STGM_DIRECT = 0x0,
        STGM_TRANSACTED = 0x10000,
        STGM_SIMPLE = 0x8000000,
        STGM_READ = 0x0,
        STGM_WRITE = 0x1,
        STGM_READWRITE = 0x2,
        STGM_SHARE_DENY_NONE = 0x40,
        STGM_SHARE_DENY_READ = 0x30,
        STGM_SHARE_DENY_WRITE = 0x20,
        STGM_SHARE_EXCLUSIVE = 0x10,
        STGM_PRIORITY = 0x40000,
        STGM_DELETEONRELEASE = 0x4000000,
        STGM_NOSCRATCH = 0x100000,
        STGM_CREATE = 0x1000,
        STGM_CONVERT = 0x20000,
        STGM_FAILIFTHERE = 0x0,
        STGM_NOSNAPSHOT = 0x200000,
    }

    // DVASPECT
    [Flags(), ComVisible(false)]
    public enum DVASPECT : int
    {
        DVASPECT_CONTENT = 1,
        DVASPECT_THUMBNAIL = 2,
        DVASPECT_ICON = 4,
        DVASPECT_DOCPRINT = 8,
        DVASPECT_OPAQUE = 16,
        DVASPECT_TRANSPARENT = 32,
    }

    // CLIPFORMAT
    [ComVisible(false)]
    public enum CLIPFORMAT : int
    {
        CF_TEXT = 1,
        CF_BITMAP = 2,
        CF_METAFILEPICT = 3,
        CF_SYLK = 4,
        CF_DIF = 5,
        CF_TIFF = 6,
        CF_OEMTEXT = 7,
        CF_DIB = 8,
        CF_PALETTE = 9,
        CF_PENDATA = 10,
        CF_RIFF = 11,
        CF_WAVE = 12,
        CF_UNICODETEXT = 13,
        CF_ENHMETAFILE = 14,
        CF_HDROP = 15,
        CF_LOCALE = 16,
        CF_MAX = 17,
        CF_OWNERDISPLAY = 0x80,
        CF_DSPTEXT = 0x81,
        CF_DSPBITMAP = 0x82,
        CF_DSPMETAFILEPICT = 0x83,
        CF_DSPENHMETAFILE = 0x8E,
    }

    // Object flags
    [Flags(), ComVisible(false)]
    public enum REOOBJECTFLAGS : uint
    {
        REO_NULL = 0x00000000,	// No flags
        REO_READWRITEMASK = 0x0000003F,	// Mask out RO bits
        REO_DONTNEEDPALETTE = 0x00000020,	// Object doesn't need palette
        REO_BLANK = 0x00000010,	// Object is blank
        REO_DYNAMICSIZE = 0x00000008,	// Object defines size always
        REO_INVERTEDSELECT = 0x00000004,	// Object drawn all inverted if sel
        REO_BELOWBASELINE = 0x00000002,	// Object sits below the baseline
        REO_RESIZABLE = 0x00000001,	// Object may be resized
        REO_LINK = 0x80000000,	// Object is a link (RO)
        REO_STATIC = 0x40000000,	// Object is static (RO)
        REO_SELECTED = 0x08000000,	// Object selected (RO)
        REO_OPEN = 0x04000000,	// Object open in its server (RO)
        REO_INPLACEACTIVE = 0x02000000,	// Object in place active (RO)
        REO_HILITED = 0x01000000,	// Object is to be hilited (RO)
        REO_LINKAVAILABLE = 0x00800000,	// Link believed available (RO)
        REO_GETMETAFILE = 0x00400000	// Object requires metafile (RO)
    }

    // OLERENDER
    [ComVisible(false)]
    public enum OLERENDER : int
    {
        OLERENDER_NONE = 0,
        OLERENDER_DRAW = 1,
        OLERENDER_FORMAT = 2,
        OLERENDER_ASIS = 3,
    }

    // TYMED
    [Flags(), ComVisible(false)]
    public enum TYMED : int
    {
        TYMED_NULL = 0,
        TYMED_HGLOBAL = 1,
        TYMED_FILE = 2,
        TYMED_ISTREAM = 4,
        TYMED_ISTORAGE = 8,
        TYMED_GDI = 16,
        TYMED_MFPICT = 32,
        TYMED_ENHMF = 64,
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public struct FORMATETC
    {
        public CLIPFORMAT cfFormat;
        public IntPtr ptd;
        public DVASPECT dwAspect;
        public int lindex;
        public TYMED tymed;
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public struct STGMEDIUM
    {
        //[MarshalAs(UnmanagedType.I4)]
        public int tymed;
        public IntPtr unionmember;
        public IntPtr pUnkForRelease;
    }

    [ComVisible(true),
    ComImport(),
    Guid("00000103-0000-0000-C000-000000000046"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumFORMATETC
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Next(
            [In, MarshalAs(UnmanagedType.U4)]
			int celt,
            [Out]
			FORMATETC rgelt,
            [In, Out, MarshalAs(UnmanagedType.LPArray)]
			int[] pceltFetched);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Skip(
            [In, MarshalAs(UnmanagedType.U4)]
			int celt);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Reset();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Clone(
            [Out, MarshalAs(UnmanagedType.LPArray)]
			IEnumFORMATETC[] ppenum);
    }

    [ComVisible(true), StructLayout(LayoutKind.Sequential)]
    public class COMRECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public COMRECT()
        {
        }

        public COMRECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public static COMRECT FromXYWH(int x, int y, int width, int height)
        {
            return new COMRECT(x, y, x + width, y + height);
        }
    }

    public enum GETOBJECTOPTIONS
    {
        REO_GETOBJ_NO_INTERFACES = 0x00000000,
        REO_GETOBJ_POLEOBJ = 0x00000001,
        REO_GETOBJ_PSTG = 0x00000002,
        REO_GETOBJ_POLESITE = 0x00000004,
        REO_GETOBJ_ALL_INTERFACES = 0x00000007,
    }

    public enum GETCLIPBOARDDATAFLAGS
    {
        RECO_PASTE = 0,
        RECO_DROP = 1,
        RECO_COPY = 2,
        RECO_CUT = 3,
        RECO_DRAG = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CHARRANGE
    {
        public int cpMin;
        public int cpMax;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class REOBJECT
    {
        public int cbStruct = Marshal.SizeOf(typeof(REOBJECT));	// Size of structure
        public int cp;											// Character position of object
        public Guid clsid;										// Class ID of object
        public IntPtr poleobj;							    	// OLE object interface
        public IStorage pstg;									// Associated storage interface
        public IOleClientSite polesite;							// Associated client site interface
        public Size sizel;										// Size of object (may be 0,0)
        public uint dvAspect;									// Display aspect to use
        public uint dwFlags;									// Object status flags
        public uint dwUser;										// Dword for user's use
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst =3)]
        //public int fileName;                                 //object ID
    }



    [ComVisible(true), Guid("0000010F-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAdviseSink
    {

        //C#r: UNDONE (Field in interface) public static readonly    Guid iid;
        void OnDataChange(
            [In]
			FORMATETC pFormatetc,
            [In]
			STGMEDIUM pStgmed);

        void OnViewChange(
            [In, MarshalAs(UnmanagedType.U4)]
			int dwAspect,
            [In, MarshalAs(UnmanagedType.I4)]
			int lindex);

        void OnRename(
            [In, MarshalAs(UnmanagedType.Interface)]
			object pmk);

        void OnSave();


        void OnClose();
    }

    [ComVisible(false), StructLayout(LayoutKind.Sequential)]
    public sealed class STATDATA
    {

        [MarshalAs(UnmanagedType.U4)]
        public int advf;
        [MarshalAs(UnmanagedType.U4)]
        public int dwConnection;

    }

    [ComVisible(false), StructLayout(LayoutKind.Sequential)]
    public sealed class tagOLEVERB
    {
        [MarshalAs(UnmanagedType.I4)]
        public int lVerb;

        [MarshalAs(UnmanagedType.LPWStr)]
        public String lpszVerbName;

        [MarshalAs(UnmanagedType.U4)]
        public int fuFlags;

        [MarshalAs(UnmanagedType.U4)]
        public int grfAttribs;

    }

    [ComVisible(true), ComImport(), Guid("00000104-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumOLEVERB
    {

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Next(
            [MarshalAs(UnmanagedType.U4)]
			int celt,
            [Out]
			tagOLEVERB rgelt,
            [Out, MarshalAs(UnmanagedType.LPArray)]
			int[] pceltFetched);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Skip(
            [In, MarshalAs(UnmanagedType.U4)]
			int celt);

        void Reset();


        void Clone(
            out IEnumOLEVERB ppenum);


    }

    [ComVisible(true), Guid("00000105-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumSTATDATA
    {

        //C#r: UNDONE (Field in interface) public static readonly    Guid iid;

        void Next(
            [In, MarshalAs(UnmanagedType.U4)]
			int celt,
            [Out]
			STATDATA rgelt,
            [Out, MarshalAs(UnmanagedType.LPArray)]
			int[] pceltFetched);


        void Skip(
            [In, MarshalAs(UnmanagedType.U4)]
			int celt);


        void Reset();


        void Clone(
            [Out, MarshalAs(UnmanagedType.LPArray)]
			IEnumSTATDATA[] ppenum);


    }

    [ComVisible(true), Guid("0000011B-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleContainer
    {


        void ParseDisplayName(
            [In, MarshalAs(UnmanagedType.Interface)] object pbc,
            [In, MarshalAs(UnmanagedType.BStr)]      string pszDisplayName,
            [Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten,
            [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);


        void EnumObjects(
            [In, MarshalAs(UnmanagedType.U4)]        int grfFlags,
            [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppenum);


        void LockContainer(
            [In, MarshalAs(UnmanagedType.I4)] int fLock);
    }

    [ComVisible(true),
    ComImport(),
    Guid("0000010E-0000-0000-C000-000000000046"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDataObject
    {
        [PreserveSig()]
        uint GetData(
            ref FORMATETC a,
            ref STGMEDIUM b);

        [PreserveSig()]
        uint GetDataHere(
            ref FORMATETC pFormatetc,
            out STGMEDIUM pMedium);

        [PreserveSig()]
        uint QueryGetData(
            ref FORMATETC pFormatetc);

        [PreserveSig()]
        uint GetCanonicalFormatEtc(
            ref FORMATETC pformatectIn,
            out	FORMATETC pformatetcOut);

        [PreserveSig()]
        uint SetData(
            ref FORMATETC pFormatectIn,
            ref STGMEDIUM pmedium,
            [In, MarshalAs(UnmanagedType.Bool)]
			bool fRelease);

        [PreserveSig()]
        uint EnumFormatEtc(
            uint dwDirection, IEnumFORMATETC penum);

        [PreserveSig()]
        uint DAdvise(
            ref FORMATETC pFormatetc,
            int advf,
            [In, MarshalAs(UnmanagedType.Interface)]
			IAdviseSink pAdvSink,
            out uint pdwConnection);

        [PreserveSig()]
        uint DUnadvise(
            uint dwConnection);

        [PreserveSig()]
        uint EnumDAdvise(
            [Out, MarshalAs(UnmanagedType.Interface)]
			out IEnumSTATDATA ppenumAdvise);
    }

    [ComVisible(true), Guid("00000118-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleClientSite
    {

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SaveObject();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetMoniker(
            [In, MarshalAs(UnmanagedType.U4)]          int dwAssign,
            [In, MarshalAs(UnmanagedType.U4)]          int dwWhichMoniker,
            [Out, MarshalAs(UnmanagedType.Interface)] out object ppmk);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetContainer([MarshalAs(UnmanagedType.Interface)] out IOleContainer container);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ShowObject();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int OnShowWindow(
            [In, MarshalAs(UnmanagedType.I4)] int fShow);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int RequestNewObjectLayout();
    }

    [ComVisible(false), StructLayout(LayoutKind.Sequential)]
    public sealed class tagLOGPALETTE
    {
        [MarshalAs(UnmanagedType.U2)/*leftover(offset=0, palVersion)*/]
        public short palVersion;

        [MarshalAs(UnmanagedType.U2)/*leftover(offset=2, palNumEntries)*/]
        public short palNumEntries;

        // UNMAPPABLE: palPalEntry: Cannot be used as a structure field.
        //   /** @com.structmap(UNMAPPABLE palPalEntry) */
        //  public UNMAPPABLE palPalEntry;
    }

    [ComVisible(true), ComImport(), Guid("00000112-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleObject
    {

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetClientSite(
            [In, MarshalAs(UnmanagedType.Interface)]
			IOleClientSite pClientSite);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetClientSite(out IOleClientSite site);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetHostNames(
            [In, MarshalAs(UnmanagedType.LPWStr)]
			string szContainerApp,
            [In, MarshalAs(UnmanagedType.LPWStr)]
			string szContainerObj);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Close(
            [In, MarshalAs(UnmanagedType.I4)]
			int dwSaveOption);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetMoniker(
            [In, MarshalAs(UnmanagedType.U4)]
			int dwWhichMoniker,
            [In, MarshalAs(UnmanagedType.Interface)]
			object pmk);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetMoniker(
            [In, MarshalAs(UnmanagedType.U4)]
			int dwAssign,
            [In, MarshalAs(UnmanagedType.U4)]
			int dwWhichMoniker,
            out object moniker);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int InitFromData(
            [In, MarshalAs(UnmanagedType.Interface)]
			IDataObject pDataObject,
            [In, MarshalAs(UnmanagedType.I4)]
			int fCreation,
            [In, MarshalAs(UnmanagedType.U4)]
			int dwReserved);

        int GetClipboardData(
            [In, MarshalAs(UnmanagedType.U4)]
			int dwReserved,
            out IDataObject data);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int DoVerb(
            [In, MarshalAs(UnmanagedType.I4)]
			int iVerb,
            [In]
			IntPtr lpmsg,
            [In, MarshalAs(UnmanagedType.Interface)]
			IOleClientSite pActiveSite,
            [In, MarshalAs(UnmanagedType.I4)]
			int lindex,
            [In]
			IntPtr hwndParent,
            [In]
			COMRECT lprcPosRect);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int EnumVerbs(out IEnumOLEVERB e);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Update();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int IsUpToDate();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetUserClassID(
            [In, Out]
			ref Guid pClsid);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetUserType(
            [In, MarshalAs(UnmanagedType.U4)]
			int dwFormOfType,
            [Out, MarshalAs(UnmanagedType.LPWStr)]
			out string userType);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetExtent(
            [In, MarshalAs(UnmanagedType.U4)]
			int dwDrawAspect,
            [In]
			Size pSizel);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetExtent(
            [In, MarshalAs(UnmanagedType.U4)]
			int dwDrawAspect,
            [Out]
			Size pSizel);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Advise([In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink, out int cookie);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Unadvise([In, MarshalAs(UnmanagedType.U4)] int dwConnection);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int EnumAdvise(out IEnumSTATDATA e);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetMiscStatus([In, MarshalAs(UnmanagedType.U4)] int dwAspect, out int misc);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetColorScheme([In] tagLOGPALETTE pLogpal);
    }

    [ComImport]
    [Guid("0000000d-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumSTATSTG
    {
        // The user needs to allocate an STATSTG array whose size is celt.
        [PreserveSig]
        uint
            Next(
            uint celt,
            [MarshalAs(UnmanagedType.LPArray), Out]
			STATSTG[] rgelt,
            out uint pceltFetched
            );

        void Skip(uint celt);

        void Reset();

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATSTG Clone();
    }

    [ComImport]
    [Guid("0000000b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStorage
    {
        int CreateStream(
            /* [string][in] */ string pwcsName,
            /* [in] */ uint grfMode,
            /* [in] */ uint reserved1,
            /* [in] */ uint reserved2,
            /* [out] */ out IStream ppstm);

        int OpenStream(
            /* [string][in] */ string pwcsName,
            /* [unique][in] */ IntPtr reserved1,
            /* [in] */ uint grfMode,
            /* [in] */ uint reserved2,
            /* [out] */ out IStream ppstm);

        int CreateStorage(
            /* [string][in] */ string pwcsName,
            /* [in] */ uint grfMode,
            /* [in] */ uint reserved1,
            /* [in] */ uint reserved2,
            /* [out] */ out IStorage ppstg);

        int OpenStorage(
            /* [string][unique][in] */ string pwcsName,
            /* [unique][in] */ IStorage pstgPriority,
            /* [in] */ uint grfMode,
            /* [unique][in] */ IntPtr snbExclude,
            /* [in] */ uint reserved,
            /* [out] */ out IStorage ppstg);

        int CopyTo(
            /* [in] */ uint ciidExclude,
            /* [size_is][unique][in] */ Guid rgiidExclude,
            /* [unique][in] */ IntPtr snbExclude,
            /* [unique][in] */ IStorage pstgDest);

        int MoveElementTo(
            /* [string][in] */ string pwcsName,
            /* [unique][in] */ IStorage pstgDest,
            /* [string][in] */ string pwcsNewName,
            /* [in] */ uint grfFlags);

        int Commit(
            /* [in] */ uint grfCommitFlags);

        int Revert();

        int EnumElements(
            /* [in] */ uint reserved1,
            /* [size_is][unique][in] */ IntPtr reserved2,
            /* [in] */ uint reserved3,
            /* [out] */ out IEnumSTATSTG ppenum);

        int DestroyElement(
            /* [string][in] */ string pwcsName);

        int RenameElement(
            /* [string][in] */ string pwcsOldName,
            /* [string][in] */ string pwcsNewName);

        int SetElementTimes(
            /* [string][unique][in] */ string pwcsName,
            /* [unique][in] */ FILETIME pctime,
            /* [unique][in] */ FILETIME patime,
            /* [unique][in] */ FILETIME pmtime);

        int SetClass(
            /* [in] */ Guid clsid);

        int SetStateBits(
            /* [in] */ uint grfStateBits,
            /* [in] */ uint grfMask);

        int Stat(
            /* [out] */ out STATSTG pstatstg,
            /* [in] */ uint grfStatFlag);

    }

    [ComImport]
    [Guid("0000000a-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ILockBytes
    {
        int ReadAt(
            /* [in] */ uint ulOffset,
            /* [unique][out] */ IntPtr pv,
            /* [in] */ uint cb,
            /* [out] */ out IntPtr pcbRead);

        int WriteAt(
            /* [in] */ uint ulOffset,
            /* [size_is][in] */ IntPtr pv,
            /* [in] */ uint cb,
            /* [out] */ out IntPtr pcbWritten);

        int Flush();

        int SetSize(
            /* [in] */ uint cb);

        int LockRegion(
            /* [in] */ uint libOffset,
            /* [in] */ uint cb,
            /* [in] */ uint dwLockType);

        int UnlockRegion(
            /* [in] */ uint libOffset,
            /* [in] */ uint cb,
            /* [in] */ uint dwLockType);

        int Stat(
            /* [out] */ out STATSTG pstatstg,
            /* [in] */ uint grfStatFlag);

    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0c733a30-2a1c-11ce-ade5-00aa0044773d")]
    public interface ISequentialStream
    {
        int Read(
            /* [length_is][size_is][out] */ IntPtr pv,
            /* [in] */ uint cb,
            /* [out] */ out uint pcbRead);

        int Write(
            /* [size_is][in] */ IntPtr pv,
            /* [in] */ uint cb,
            /* [out] */ out uint pcbWritten);

    };

    [ComImport]
    [Guid("0000000c-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStream : ISequentialStream
    {
        int Seek(
            /* [in] */ ulong dlibMove,
            /* [in] */ uint dwOrigin,
            /* [out] */ out uint plibNewPosition);

        int SetSize(
            /* [in] */ uint libNewSize);

        int CopyTo(
            /* [unique][in] */ [In] IStream pstm,
            /* [in] */ uint cb,
            /* [out] */ out uint pcbRead,
            /* [out] */ out uint pcbWritten);

        int Commit(
            /* [in] */ uint grfCommitFlags);

        int Revert();

        int LockRegion(
            /* [in] */ uint libOffset,
            /* [in] */ uint cb,
            /* [in] */ uint dwLockType);

        int UnlockRegion(
            /* [in] */ uint libOffset,
            /* [in] */ uint cb,
            /* [in] */ uint dwLockType);

        int Stat(
            /* [out] */ out STATSTG pstatstg,
            /* [in] */ uint grfStatFlag);

        int Clone(
            /* [out] */ out IStream ppstm);

    };

    /// <summary>
    /// Definition for interface IPersist.
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000010c-0000-0000-C000-000000000046")]
    public interface IPersist
    {
        /// <summary>
        /// getClassID
        /// </summary>
        /// <param name="pClassID"></param>
        void GetClassID( /* [out] */ out Guid pClassID);
    }

    /// <summary>
    /// Definition for interface IPersistStream.
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000109-0000-0000-C000-000000000046")]
    public interface IPersistStream : IPersist
    {
        /// <summary>
        /// GetClassID
        /// </summary>
        /// <param name="pClassID"></param>
        new void GetClassID(out Guid pClassID);
        /// <summary>
        /// isDirty
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        int IsDirty();
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="pStm"></param>
        void Load([In] UCOMIStream pStm);
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="pStm"></param>
        /// <param name="fClearDirty"></param>
        void Save([In] UCOMIStream pStm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
        /// <summary>
        /// GetSizeMax
        /// </summary>
        /// <param name="pcbSize"></param>
        void GetSizeMax(out int pcbSize);
    }

    [ComImport(), Guid("00020D00-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IRichEditOle
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetClientSite(out IOleClientSite site);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetObjectCount();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetLinkCount();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetObject(int iob, [In, Out] REOBJECT lpreobject, [MarshalAs(UnmanagedType.U4)]GETOBJECTOPTIONS flags);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int InsertObject(REOBJECT lpreobject);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ConvertObject(int iob, Guid rclsidNew, string lpstrUserTypeNew);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ActivateAs(Guid rclsid, Guid rclsidAs);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetHostNames(string lpstrContainerApp, string lpstrContainerObj);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetLinkAvailable(int iob, bool fAvailable);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetDvaspect(int iob, uint dvaspect);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int HandsOffStorage(int iob);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SaveCompleted(int iob, IStorage lpstg);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int InPlaceDeactivate();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ContextSensitiveHelp(bool fEnterMode);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetClipboardData([In, Out] ref CHARRANGE lpchrg, [MarshalAs(UnmanagedType.U4)] GETCLIPBOARDDATAFLAGS reco, out IDataObject lplpdataobj);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ImportDataObject(IDataObject lpdataobj, int cf, IntPtr hMetaPict);
    }
    #endregion
    #region Public Enums

    // Enum for possible RTF colors
    public enum RtfColor
    {
        Black, Maroon, Green, Olive, Navy, Purple, Teal, Gray, Silver,
        Red, Lime, Yellow, Blue, Fuchsia, Aqua, White
    }

    #endregion

    public class myDataObject : IDataObject
    {
        private Bitmap mBitmap;
        public FORMATETC mpFormatetc;

        #region IDataObject Members

        private const uint S_OK = 0;
        private const uint E_POINTER = 0x80004003;
        private const uint E_NOTIMPL = 0x80004001;
        private const uint E_FAIL = 0x80004005;

        public uint GetData(ref FORMATETC pFormatetc, ref STGMEDIUM pMedium)
        {
            IntPtr hDst = mBitmap.GetHbitmap();

            pMedium.tymed = (int)TYMED.TYMED_GDI;
            pMedium.unionmember = hDst;
            pMedium.pUnkForRelease = IntPtr.Zero;

            return (uint)S_OK;
        }

        public uint GetDataHere(ref FORMATETC pFormatetc, out STGMEDIUM pMedium)
        {
            Trace.WriteLine("GetDataHere");

            pMedium = new STGMEDIUM();

            return (uint)E_NOTIMPL;
        }

        public uint QueryGetData(ref FORMATETC pFormatetc)
        {
            Trace.WriteLine("QueryGetData");

            return (uint)E_NOTIMPL;
        }

        public uint GetCanonicalFormatEtc(ref FORMATETC pFormatetcIn, out FORMATETC pFormatetcOut)
        {
            Trace.WriteLine("GetCanonicalFormatEtc");

            pFormatetcOut = new FORMATETC();

            return (uint)E_NOTIMPL;
        }

        public uint SetData(ref FORMATETC a, ref STGMEDIUM b, bool fRelease)
        {
            //mpFormatetc = pFormatectIn;
            //mpmedium = pmedium;

            Trace.WriteLine("SetData");

            return (int)S_OK;
        }

        public uint EnumFormatEtc(uint dwDirection, IEnumFORMATETC penum)
        {
            Trace.WriteLine("EnumFormatEtc");

            return (int)S_OK;
        }

        public uint DAdvise(ref FORMATETC a, int advf, IAdviseSink pAdvSink, out uint pdwConnection)
        {
            Trace.WriteLine("DAdvise");

            pdwConnection = 0;

            return (uint)E_NOTIMPL;
        }

        public uint DUnadvise(uint dwConnection)
        {
            Trace.WriteLine("DUnadvise");

            return (uint)E_NOTIMPL;
        }

        public uint EnumDAdvise(out IEnumSTATDATA ppenumAdvise)
        {
            Trace.WriteLine("EnumDAdvise");

            ppenumAdvise = null;

            return (uint)E_NOTIMPL;
        }

        #endregion

        public myDataObject()
        {
            mBitmap = new Bitmap(16, 16);
            mpFormatetc = new FORMATETC();
        }

        public void SetImage(string strFilename)
        {
            try
            {
                mBitmap = (Bitmap)Bitmap.FromFile(strFilename, true);

                mpFormatetc.cfFormat = CLIPFORMAT.CF_BITMAP;				// Clipboard format = CF_BITMAP
                mpFormatetc.ptd = IntPtr.Zero;							// Target Device = Screen
                mpFormatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;			// Level of detail = Full content
                mpFormatetc.lindex = -1;							// Index = Not applicaple
                mpFormatetc.tymed = TYMED.TYMED_GDI;					// Storage medium = HBITMAP handle
            }
            catch
            {
            }
        }

        public void SetImage(Image image)
        {
            try
            {
                mBitmap = new Bitmap(image);

                mpFormatetc.cfFormat = CLIPFORMAT.CF_BITMAP;				// Clipboard format = CF_BITMAP
                mpFormatetc.ptd = IntPtr.Zero;							// Target Device = Screen
                mpFormatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;			// Level of detail = Full content
                mpFormatetc.lindex = -1;							// Index = Not applicaple
                mpFormatetc.tymed = TYMED.TYMED_GDI;					// Storage medium = HBITMAP handle
            }
            catch
            {
            }
        }
    }

    public class MyExtRichTextBox : System.Windows.Forms.RichTextBox, IDisposable
    {        

        #region RichTextBoxPlus Members
        protected IRichEditOle IRichEditOleValue = null;
        protected IntPtr IRichEditOlePtr = IntPtr.Zero;
        public IRichEditOle GetRichEditOleInterface()
        {
            if (this.IRichEditOleValue == null)
            {
                //REOBJECT reObject = new REOBJECT();
                //reObject.cp = 0;
                //reObject.dwFlags = GetObjectOptions.REO_GETOBJ_POLEOBJ;
                //IntPtr ptr = Marshal.AllocCoTaskMem(reObject.cbStruct);
                //Marshal.StructureToPtr(reObject, ptr, false);

                // Allocate the ptr that EM_GETOLEINTERFACE will fill in.
                IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)));	// Alloc the ptr.
                Marshal.WriteIntPtr(ptr, IntPtr.Zero);	// Clear it.
                try
                {
                    if (0 != API.SendMessage(this.Handle, Messages.EM_GETOLEINTERFACE, IntPtr.Zero, ptr))
                    {
                        // Read the returned pointer.
                        IntPtr pRichEdit = Marshal.ReadIntPtr(ptr);
                        try
                        {
                            if (pRichEdit != IntPtr.Zero)
                            {
                                // Query for the IRichEditOle interface.
                                Guid guid = new Guid("00020D00-0000-0000-c000-000000000046");
                                Marshal.QueryInterface(pRichEdit, ref guid, out this.IRichEditOlePtr);

                                // Wrap it in the C# interface for IRichEditOle.
                                this.IRichEditOleValue = (IRichEditOle)Marshal.GetTypedObjectForIUnknown(this.IRichEditOlePtr, typeof(IRichEditOle));
                                if (this.IRichEditOleValue == null)
                                {
                                    throw new Exception("Failed to get the object wrapper for the interface.");
                                }
                            }
                            else
                            {
                                throw new Exception("Failed to get the pointer.");
                            }
                        }
                        finally
                        {
                            Marshal.Release(pRichEdit);
                        }
                    }
                    else
                    {
                        throw new Exception("EM_GETOLEINTERFACE failed.");
                    }
                }
                catch (Exception err)
                {
                    Trace.WriteLine(err.ToString());
                    this.ReleaseRichEditOleInterface();
                }
                finally
                {
                    // Free the ptr memory.
                    Marshal.FreeCoTaskMem(ptr);
                    //Marshal.DestroyStructure(ptr, typeof(REOBJECT));
                }
            }
            return this.IRichEditOleValue;
        }

        /// <summary>
        /// Releases the IRichEditOle interface if it hasn't been already.
        /// </summary>
        /// <remarks>This is automatically called in Dispose if needed.</remarks>
        public void ReleaseRichEditOleInterface()
        {
            if (this.IRichEditOlePtr != IntPtr.Zero)
            {
                Marshal.Release(this.IRichEditOlePtr);
            }

            this.IRichEditOlePtr = IntPtr.Zero;
            this.IRichEditOleValue = null;
        }

        #endregion

        #region IDisposable Members

        //		public void Dispose()
        //		{
        //			this.ReleaseRichEditOleInterface();
        //		}

        #endregion


        #region Imports and structs

        // It makes no difference if we use PARAFORMAT or
        // PARAFORMAT2 here, so I have opted for PARAFORMAT2.
        [StructLayout(LayoutKind.Sequential)]
        public struct PARAFORMAT
        {
            public int cbSize;
            public uint dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] rgxTabs;

            // PARAFORMAT2 from here onwards.
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public short sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public short wShadingWeight;
            public short wShadingStyle;
            public short wNumberingStart;
            public short wNumberingStyle;
            public short wNumberingTab;
            public short wBorderSpace;
            public short wBorderWidth;
            public short wBorders;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CHARFORMAT
        {
            public int cbSize;
            public UInt32 dwMask;
            public UInt32 dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public Int32 crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;

            // CHARFORMAT2 from here onwards.
            public short wWeight;
            public short sSpacing;
            public Int32 crBackColor;
            public uint lcid;
            public uint dwReserved;
            public short sStyle;
            public short wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
            int msg,
            int wParam,
            int lParam);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
            int msg,
            int wParam,
            ref PARAFORMAT lp);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
            int msg,
            int wParam,
            ref CHARFORMAT lp);

        private const int EM_SETEVENTMASK = 1073;
        private const int WM_SETREDRAW = 11;

        [DllImport("User32.dll", CharSet = CharSet.Auto, PreserveSig = false)]
        public static extern IRichEditOle SendMessage(IntPtr hWnd, int message, int wParam);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool GetClientRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("ole32.dll")]
        static extern int OleSetContainedObject([MarshalAs(UnmanagedType.IUnknown)]
			object pUnk, bool fContained);

        [DllImport("ole32.dll")]
        static extern int OleLoadPicturePath(
            [MarshalAs(UnmanagedType.LPWStr)] string lpszPicturePath,
            [MarshalAs(UnmanagedType.IUnknown)][In] object pIUnknown,
            uint dwReserved,
            uint clrReserved,
            ref Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        static extern int OleCreateFromFile([In] ref Guid rclsid,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszFileName, [In] ref Guid riid,
            uint renderopt, ref FORMATETC pFormatEtc, IOleClientSite pClientSite,
            IStorage pStg, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        static extern int OleCreateFromData(IDataObject pSrcDataObj,
            [In] ref Guid riid, uint renderopt, ref FORMATETC pFormatEtc,
            IOleClientSite pClientSite, IStorage pStg,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        static extern int OleCreateStaticFromData([MarshalAs(UnmanagedType.Interface)]IDataObject pSrcDataObj,
            [In] ref Guid riid, uint renderopt, ref FORMATETC pFormatEtc,
            IOleClientSite pClientSite, IStorage pStg,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        static extern int OleCreateLinkFromData([MarshalAs(UnmanagedType.Interface)]IDataObject pSrcDataObj,
            [In] ref Guid riid, uint renderopt, ref FORMATETC pFormatEtc,
            IOleClientSite pClientSite, IStorage pStg,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        #endregion

        #region MyExtRichTextBox Members
        public void InsertOleObject(IOleObject oleObj)
        {
            RichEditOle ole = new RichEditOle(this);
            ole.InsertOleObject(oleObj);
        }

        public void InsertMyControl(Control control)
        {
            RichEditOle ole = new RichEditOle(this);
            ole.InsertControl(control);

        }

        public void InsertMyDataObject(myDataObject mdo)
        {
            RichEditOle ole = new RichEditOle(this);
            ole.InsertMyDataObject(mdo);
        }

        public void UpdateObjects()
        {
            RichEditOle ole = new RichEditOle(this);
            ole.UpdateObjects();
        }

        public void InsertMyImage(Image image)
        {
            myDataObject mdo = new myDataObject();

            mdo.SetImage(image);

            this.InsertMyDataObject(mdo);
        }

        public void InsertMyImage(string imageFile)
        {
            myDataObject mdo = new myDataObject();

            mdo.SetImage(imageFile);

            this.InsertMyDataObject(mdo);
        }

        public void InsertMyImageFromFile(string strFilename)
        {
            RichEditOle ole = new RichEditOle(this);
            ole.InsertImageFromFile(strFilename);
        }

        public void InsertActiveX(string strProgID)
        {
            Type t = Type.GetTypeFromProgID(strProgID);
            if (t == null)
                return;

            object o = System.Activator.CreateInstance(t);

            bool b = (o is IOleObject);

            if (b)
                this.InsertOleObject((IOleObject)o);
        }

        // RichEditOle wrapper and helper
        class RichEditOle
        {
            public const int WM_USER = 0x0400;
            public const int EM_GETOLEINTERFACE = WM_USER + 60;

            private MyExtRichTextBox _richEdit;
            private IRichEditOle _RichEditOle;

            public RichEditOle(MyExtRichTextBox richEdit)
            {
                this._richEdit = richEdit;
            }

            private IRichEditOle IRichEditOle
            {
                get
                {
                    if (this._RichEditOle == null)
                    {
                        this._RichEditOle = SendMessage(this._richEdit.Handle, EM_GETOLEINTERFACE, 0);
                    }

                    return this._RichEditOle;
                }
            }



            [DllImport("ole32.dll", PreserveSig = false)]
            internal static extern int CreateILockBytesOnHGlobal(int hGlobal, bool fDeleteOnRelease, [Out] out ILockBytes ppLkbyt);

            [DllImport("ole32.dll")]
            static extern int StgCreateDocfileOnILockBytes(ILockBytes plkbyt, uint grfMode,
                uint reserved, out IStorage ppstgOpen);

            public void InsertControl(Control control)
            {
                if (control == null)
                    return;

                Guid guid = Marshal.GenerateGuidForType(control.GetType());

                //-----------------------
                ILockBytes pLockBytes=null;
                try
                {
                    CreateILockBytesOnHGlobal(0, true, out pLockBytes);
                }
                catch
                { }
                IStorage pStorage;
                StgCreateDocfileOnILockBytes(pLockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out pStorage);

                IOleClientSite pOleClientSite;
                this.IRichEditOle.GetClientSite(out pOleClientSite);
                //-----------------------

                //-----------------------
                REOBJECT reoObject = new REOBJECT();


                reoObject.cp = this._richEdit.SelectionStart;//this._richEdit.TextLength;
                reoObject.clsid = guid;
                reoObject.pstg = pStorage;
                reoObject.poleobj = Marshal.GetIUnknownForObject(control);
                reoObject.polesite = pOleClientSite;
                reoObject.dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT);
                reoObject.dwFlags = (uint)(REOOBJECTFLAGS.REO_BELOWBASELINE);
                try
                {
                    reoObject.dwUser = Convert.ToUInt32((control as MyPicture).Tag);
                }
                catch { }
                this.IRichEditOle.InsertObject(reoObject);

                //-----------------------
                //-----------------------

                Marshal.ReleaseComObject(pLockBytes);
                Marshal.ReleaseComObject(pOleClientSite);
                Marshal.ReleaseComObject(pStorage);
                //-----------------------
            }

            public bool InsertImageFromFile(string strFilename)
            {
                //-----------------------
                ILockBytes pLockBytes;
                CreateILockBytesOnHGlobal(0, true, out pLockBytes);

                IStorage pStorage;
                StgCreateDocfileOnILockBytes(pLockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out pStorage);

                IOleClientSite pOleClientSite;
                this.IRichEditOle.GetClientSite(out pOleClientSite);
                //-----------------------


                //-----------------------
                FORMATETC formatEtc = new FORMATETC();

                formatEtc.cfFormat = 0;
                formatEtc.ptd = IntPtr.Zero;
                formatEtc.dwAspect = DVASPECT.DVASPECT_CONTENT;
                formatEtc.lindex = -1;
                formatEtc.tymed = TYMED.TYMED_NULL;

                Guid IID_IOleObject = new Guid("{00000112-0000-0000-C000-000000000046}");
                Guid CLSID_NULL = new Guid("{00000000-0000-0000-0000-000000000000}");

                object pOleObjectOut;

                // I don't sure, but it appears that this function only loads from bitmap
                // You can also try OleCreateFromData, OleLoadPictureIndirect, etc.
                int hr = OleCreateFromFile(ref CLSID_NULL, strFilename, ref IID_IOleObject, (uint)OLERENDER.OLERENDER_DRAW, ref formatEtc, pOleClientSite, pStorage, out pOleObjectOut);

                if (pOleObjectOut == null)
                {
                    Marshal.ReleaseComObject(pLockBytes);
                    Marshal.ReleaseComObject(pOleClientSite);
                    Marshal.ReleaseComObject(pStorage);

                    return false;
                }

                IOleObject pOleObject = (IOleObject)pOleObjectOut;
                //-----------------------


                //-----------------------
                Guid guid = new Guid();

                //guid = Marshal.GenerateGuidForType(pOleObject.GetType());
                pOleObject.GetUserClassID(ref guid);
                //-----------------------

                //-----------------------
                OleSetContainedObject(pOleObject, true);

                REOBJECT reoObject = new REOBJECT();

                reoObject.cp = this._richEdit.TextLength;

                reoObject.clsid = guid;
                reoObject.pstg = pStorage;
                reoObject.poleobj = Marshal.GetIUnknownForObject(pOleObject);
                reoObject.polesite = pOleClientSite;
                reoObject.dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT);
                reoObject.dwFlags = (uint)(REOOBJECTFLAGS.REO_BELOWBASELINE);
                reoObject.dwUser = 0;

                this.IRichEditOle.InsertObject(reoObject);
                //-----------------------

                //-----------------------
                Marshal.ReleaseComObject(pLockBytes);
                Marshal.ReleaseComObject(pOleClientSite);
                Marshal.ReleaseComObject(pStorage);
                Marshal.ReleaseComObject(pOleObject);
                //-----------------------

                return true;
            }

            public void InsertMyDataObject(myDataObject mdo)
            {
                if (mdo == null)
                    return;

                //-----------------------
                ILockBytes pLockBytes;
                int sc = CreateILockBytesOnHGlobal(0, true, out pLockBytes);

                IStorage pStorage;
                sc = StgCreateDocfileOnILockBytes(pLockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out pStorage);

                IOleClientSite pOleClientSite;
                this.IRichEditOle.GetClientSite(out pOleClientSite);
                //-----------------------

                Guid guid = Marshal.GenerateGuidForType(mdo.GetType());

                Guid IID_IOleObject = new Guid("{00000112-0000-0000-C000-000000000046}");
                Guid IID_IDataObject = new Guid("{0000010e-0000-0000-C000-000000000046}");
                Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");

                object pOleObject;

                int hr = OleCreateStaticFromData(mdo, ref IID_IOleObject, (uint)OLERENDER.OLERENDER_FORMAT, ref mdo.mpFormatetc, pOleClientSite, pStorage, out pOleObject);

                if (pOleObject == null)
                    return;
                //-----------------------


                //-----------------------
                OleSetContainedObject(pOleObject, true);

                REOBJECT reoObject = new REOBJECT();

                reoObject.cp = this._richEdit.TextLength;

                reoObject.clsid = guid;
                reoObject.pstg = pStorage;
                reoObject.poleobj = Marshal.GetIUnknownForObject(pOleObject);
                reoObject.polesite = pOleClientSite;
                reoObject.dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT);
                reoObject.dwFlags = (uint)(REOOBJECTFLAGS.REO_BELOWBASELINE);
                reoObject.dwUser = 0;

                this.IRichEditOle.InsertObject(reoObject);
                //-----------------------

                //-----------------------
                Marshal.ReleaseComObject(pLockBytes);
                Marshal.ReleaseComObject(pOleClientSite);
                Marshal.ReleaseComObject(pStorage);
                Marshal.ReleaseComObject(pOleObject);
                //-----------------------
            }

            public void InsertOleObject(IOleObject oleObject)
            {
                if (oleObject == null)
                    return;

                //-----------------------
                ILockBytes pLockBytes;
                CreateILockBytesOnHGlobal(0, true, out pLockBytes);

                IStorage pStorage;
                StgCreateDocfileOnILockBytes(pLockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out pStorage);

                IOleClientSite pOleClientSite;
                this.IRichEditOle.GetClientSite(out pOleClientSite);
                //-----------------------

                //-----------------------
                Guid guid = new Guid();

                oleObject.GetUserClassID(ref guid);
                //-----------------------

                //-----------------------
                OleSetContainedObject(oleObject, true);

                REOBJECT reoObject = new REOBJECT();

                reoObject.cp = this._richEdit.TextLength;

                reoObject.clsid = guid;
                reoObject.pstg = pStorage;
                reoObject.poleobj = Marshal.GetIUnknownForObject(oleObject);
                reoObject.polesite = pOleClientSite;
                reoObject.dvAspect = (uint)DVASPECT.DVASPECT_CONTENT;
                reoObject.dwFlags = (uint)REOOBJECTFLAGS.REO_BELOWBASELINE;

                this.IRichEditOle.InsertObject(reoObject);
                //-----------------------

                //-----------------------
                Marshal.ReleaseComObject(pLockBytes);
                Marshal.ReleaseComObject(pOleClientSite);
                Marshal.ReleaseComObject(pStorage);
                //-----------------------
            }


            public void UpdateObjects()
            {
                int k = this.IRichEditOle.GetObjectCount();

                for (int i = 0; i < k; i++)
                {
                    REOBJECT reoObject = new REOBJECT();

                    this.IRichEditOle.GetObject(i, reoObject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);

                    if (reoObject.dwUser == 1)
                    {
                        Point pt = this._richEdit.GetPositionFromCharIndex(reoObject.cp);
                        //						MessageBox.Show(reoObject.fileName );
                        Rectangle rect = new Rectangle(pt, reoObject.sizel);

                        this._richEdit.Invalidate(rect, false); // repaint
                    }
                }
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region My Enums

        // Specifies the flags/options for the unmanaged call to the GDI+ method
        // Metafile.EmfToWmfBits().
        private enum EmfToWmfBitsFlags
        {

            // Use the default conversion
            EmfToWmfBitsFlagsDefault = 0x00000000,

            // Embedded the source of the EMF metafiel within the resulting WMF
            // metafile
            EmfToWmfBitsFlagsEmbedEmf = 0x00000001,

            // Place a 22-byte header in the resulting WMF file.  The header is
            // required for the metafile to be considered placeable.
            EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,

            // Don't simulate clipping by using the XOR operator.
            EmfToWmfBitsFlagsNoXORClip = 0x00000004
        };

        #endregion

        #region My Structs

        // Definitions for colors in an RTF document
        private struct RtfColorDef
        {
            public const string Black = @"\red0\green0\blue0";
            public const string Maroon = @"\red128\green0\blue0";
            public const string Green = @"\red0\green128\blue0";
            public const string Olive = @"\red128\green128\blue0";
            public const string Navy = @"\red0\green0\blue128";
            public const string Purple = @"\red128\green0\blue128";
            public const string Teal = @"\red0\green128\blue128";
            public const string Gray = @"\red128\green128\blue128";
            public const string Silver = @"\red192\green192\blue192";
            public const string Red = @"\red255\green0\blue0";
            public const string Lime = @"\red0\green255\blue0";
            public const string Yellow = @"\red255\green255\blue0";
            public const string Blue = @"\red0\green0\blue255";
            public const string Fuchsia = @"\red255\green0\blue255";
            public const string Aqua = @"\red0\green255\blue255";
            public const string White = @"\red255\green255\blue255";
        }

        // Control words for RTF font families
        private struct RtfFontFamilyDef
        {
            public const string Unknown = @"\fnil";
            public const string Roman = @"\froman";
            public const string Swiss = @"\fswiss";
            public const string Modern = @"\fmodern";
            public const string Script = @"\fscript";
            public const string Decor = @"\fdecor";
            public const string Technical = @"\ftech";
            public const string BiDirect = @"\fbidi";
        }

        #endregion

        #region My Constants

        // Not used in this application.  Descriptions can be found with documentation
        // of Windows GDI function SetMapMode
        private const int MM_TEXT = 1;
        private const int MM_LOMETRIC = 2;
        private const int MM_HIMETRIC = 3;
        private const int MM_LOENGLISH = 4;
        private const int MM_HIENGLISH = 5;
        private const int MM_TWIPS = 6;

        // Ensures that the metafile maintains a 1:1 aspect ratio
        private const int MM_ISOTROPIC = 7;

        // Allows the x-coordinates and y-coordinates of the metafile to be adjusted
        // independently
        private const int MM_ANISOTROPIC = 8;

        // Represents an unknown font family
        private const string FF_UNKNOWN = "UNKNOWN";

        // The number of hundredths of millimeters (0.01 mm) in an inch
        // For more information, see GetImagePrefix() method.
        private const int HMM_PER_INCH = 2540;

        // The number of twips in an inch
        // For more information, see GetImagePrefix() method.
        private const int TWIPS_PER_INCH = 1440;

        #endregion

        #region My Privates

        // The default text color
        private RtfColor textColor;

        // The default text background color
        private RtfColor highlightColor;

        // Dictionary that maps color enums to RTF color codes
        private HybridDictionary rtfColor;

        // Dictionary that mapas Framework font families to RTF font families
        private HybridDictionary rtfFontFamily;

        // The horizontal resolution at which the control is being displayed
        private float xDpi;

        // The vertical resolution at which the control is being displayed
        private float yDpi;

        #endregion

        #region Elements required to create an RTF document

        /* RTF HEADER
		 * ----------
		 * 
		 * \rtf[N]		- For text to be considered to be RTF, it must be enclosed in this tag.
		 *				  rtf1 is used because the RichTextBox conforms to RTF Specification
		 *				  version 1.
		 * \ansi		- The character set.
		 * \ansicpg[N]	- Specifies that unicode characters might be embedded. ansicpg1252
		 *				  is the default used by Windows.
		 * \deff[N]		- The default font. \deff0 means the default font is the first font
		 *				  found.
		 * \deflang[N]	- The default language. \deflang1033 specifies US English.
		 * */
        private const string RTF_HEADER = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033";

        /* RTF DOCUMENT AREA
         * -----------------
         * 
         * \viewkind[N]	- The type of view or zoom level.  \viewkind4 specifies normal view.
         * \uc[N]		- The number of bytes corresponding to a Unicode character.
         * \pard		- Resets to default paragraph properties
         * \cf[N]		- Foreground color.  \cf1 refers to the color at index 1 in
         *				  the color table
         * \f[N]		- Font number. \f0 refers to the font at index 0 in the font
         *				  table.
         * \fs[N]		- Font size in half-points.
         * */
        private const string RTF_DOCUMENT_PRE = @"\viewkind4\uc1\pard\cf1\f0\fs20";
        private const string RTF_DOCUMENT_POST = @"\cf0\fs17}";
        private string RTF_IMAGE_POST = @"}";

        #endregion

        #region Accessors

        // TODO: This can be ommitted along with RemoveBadCharacters
        // Overrides the default implementation of RTF.  This is done because the control
        // was originally developed to run in an instant messenger that uses the
        // Jabber XML-based protocol.  The framework would throw an exception when the
        // XML contained the null character, so I filtered out.
        public new string Rtf
        {
            get { return RemoveBadChars(base.Rtf); }
            set { base.Rtf = value; }
        }

        // The color of the text
        public RtfColor TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        // The color of the highlight
        public RtfColor HiglightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the text colors, creates dictionaries for RTF colors and
        /// font families, and stores the horizontal and vertical resolution of
        /// the RichTextBox's graphics context.
        /// </summary>
        public MyExtRichTextBox()
            : base()
        {

            // Initialize default text and background colors
            textColor = RtfColor.Black;
            highlightColor = RtfColor.White;

            // Initialize the dictionary mapping color codes to definitions
            rtfColor = new HybridDictionary();
            rtfColor.Add(RtfColor.Aqua, RtfColorDef.Aqua);
            rtfColor.Add(RtfColor.Black, RtfColorDef.Black);
            rtfColor.Add(RtfColor.Blue, RtfColorDef.Blue);
            rtfColor.Add(RtfColor.Fuchsia, RtfColorDef.Fuchsia);
            rtfColor.Add(RtfColor.Gray, RtfColorDef.Gray);
            rtfColor.Add(RtfColor.Green, RtfColorDef.Green);
            rtfColor.Add(RtfColor.Lime, RtfColorDef.Lime);
            rtfColor.Add(RtfColor.Maroon, RtfColorDef.Maroon);
            rtfColor.Add(RtfColor.Navy, RtfColorDef.Navy);
            rtfColor.Add(RtfColor.Olive, RtfColorDef.Olive);
            rtfColor.Add(RtfColor.Purple, RtfColorDef.Purple);
            rtfColor.Add(RtfColor.Red, RtfColorDef.Red);
            rtfColor.Add(RtfColor.Silver, RtfColorDef.Silver);
            rtfColor.Add(RtfColor.Teal, RtfColorDef.Teal);
            rtfColor.Add(RtfColor.White, RtfColorDef.White);
            rtfColor.Add(RtfColor.Yellow, RtfColorDef.Yellow);

            // Initialize the dictionary mapping default Framework font families to
            // RTF font families
            rtfFontFamily = new HybridDictionary();
            rtfFontFamily.Add(FontFamily.GenericMonospace.Name, RtfFontFamilyDef.Modern);
            rtfFontFamily.Add(FontFamily.GenericSansSerif, RtfFontFamilyDef.Swiss);
            rtfFontFamily.Add(FontFamily.GenericSerif, RtfFontFamilyDef.Roman);
            rtfFontFamily.Add(FF_UNKNOWN, RtfFontFamilyDef.Unknown);

            // Get the horizontal and vertical resolutions at which the object is
            // being displayed
            using (Graphics _graphics = this.CreateGraphics())
            {
                xDpi = _graphics.DpiX;
                yDpi = _graphics.DpiY;
            }
        }

        /// <summary>
        /// Calls the default constructor then sets the text color.
        /// </summary>
        /// <param name="_textColor"></param>
        public MyExtRichTextBox(RtfColor _textColor)
            : this()
        {
            textColor = _textColor;
        }

        /// <summary>
        /// Calls the default constructor then sets te text and highlight colors.
        /// </summary>
        /// <param name="_textColor"></param>
        /// <param name="_highlightColor"></param>
        public MyExtRichTextBox(RtfColor _textColor, RtfColor _highlightColor)
            : this()
        {
            textColor = _textColor;
            highlightColor = _highlightColor;
        }

        #endregion

        #region Append RTF or Text to RichTextBox Contents

        /// <summary>
        /// Assumes the string passed as a paramter is valid RTF text and attempts
        /// to append it as RTF to the content of the control.
        /// </summary>
        /// <param name="_rtf"></param>
        public void AppendRtf(string _rtf)
        {

            // Move caret to the end of the text
            this.Select(this.TextLength, 0);

            // Since SelectedRtf is null, this will append the string to the
            // end of the existing RTF
            this.SelectedRtf = _rtf;
        }

        /// <summary>
        /// Assumes that the string passed as a parameter is valid RTF text and
        /// attempts to insert it as RTF into the content of the control.
        /// </summary>
        /// <remarks>
        /// NOTE: The text is inserted wherever the caret is at the time of the call,
        /// and if any text is selected, that text is replaced.
        /// </remarks>
        /// <param name="_rtf"></param>
        public void InsertRtf(string _rtf)
        {
            this.SelectedRtf = _rtf;
        }

        /// <summary>
        /// Appends the text using the current font, text, and highlight colors.
        /// </summary>
        /// <param name="_text"></param>
        public void AppendTextAsRtf(string _text)
        {
            AppendTextAsRtf(_text, this.Font);
        }


        /// <summary>
        /// Appends the text using the given font, and current text and highlight
        /// colors.
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_font"></param>
        public void AppendTextAsRtf(string _text, Font _font)
        {
            AppendTextAsRtf(_text, _font, textColor);
        }

        /// <summary>
        /// Appends the text using the given font and text color, and the current
        /// highlight color.
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_font"></param>
        /// <param name="_color"></param>
        public void AppendTextAsRtf(string _text, Font _font, RtfColor _textColor)
        {
            AppendTextAsRtf(_text, _font, _textColor, highlightColor);
        }

        /// <summary>
        /// Appends the text using the given font, text, and highlight colors.  Simply
        /// moves the caret to the end of the RichTextBox's text and makes a call to
        /// insert.
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_font"></param>
        /// <param name="_textColor"></param>
        /// <param name="_backColor"></param>
        public void AppendTextAsRtf(string _text, Font _font, RtfColor _textColor, RtfColor _backColor)
        {
            // Move carret to the end of the text
            this.Select(this.TextLength, 0);

            InsertTextAsRtf(_text, _font, _textColor, _backColor);
        }

        #endregion

        #region Insert Plain Text

        /// <summary>
        /// Inserts the text using the current font, text, and highlight colors.
        /// </summary>
        /// <param name="_text"></param>
        public void InsertTextAsRtf(string _text)
        {
            InsertTextAsRtf(_text, this.Font);
        }


        /// <summary>
        /// Inserts the text using the given font, and current text and highlight
        /// colors.
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_font"></param>
        public void InsertTextAsRtf(string _text, Font _font)
        {
            InsertTextAsRtf(_text, _font, textColor);
        }

        /// <summary>
        /// Inserts the text using the given font and text color, and the current
        /// highlight color.
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_font"></param>
        /// <param name="_color"></param>
        public void InsertTextAsRtf(string _text, Font _font, RtfColor _textColor)
        {
            InsertTextAsRtf(_text, _font, _textColor, highlightColor);
        }

        /// <summary>
        /// Inserts the text using the given font, text, and highlight colors.  The
        /// text is wrapped in RTF codes so that the specified formatting is kept.
        /// You can only assign valid RTF to the RichTextBox.Rtf property, else
        /// an exception is thrown.  The RTF string should follow this format ...
        /// 
        /// {\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{[FONTS]}{\colortbl ;[COLORS]}}
        /// \viewkind4\uc1\pard\cf1\f0\fs20 [DOCUMENT AREA] }
        /// 
        /// </summary>
        /// <remarks>
        /// NOTE: The text is inserted wherever the caret is at the time of the call,
        /// and if any text is selected, that text is replaced.
        /// </remarks>
        /// <param name="_text"></param>
        /// <param name="_font"></param>
        /// <param name="_color"></param>
        /// <param name="_color"></param>
        public void InsertTextAsRtf(string _text, Font _font, RtfColor _textColor, RtfColor _backColor)
        {

            StringBuilder _rtf = new StringBuilder();

            // Append the RTF header
            _rtf.Append(RTF_HEADER);

            // Create the font table from the font passed in and append it to the
            // RTF string
            _rtf.Append(GetFontTable(_font));

            // Create the color table from the colors passed in and append it to the
            // RTF string
            _rtf.Append(GetColorTable(_textColor, _backColor));

            // Create the document area from the text to be added as RTF and append
            // it to the RTF string.
            _rtf.Append(GetDocumentArea(_text, _font));

            this.SelectedRtf = _rtf.ToString();
        }

        /// <summary>
        /// Creates the Document Area of the RTF being inserted. The document area
        /// (in this case) consists of the text being added as RTF and all the
        /// formatting specified in the Font object passed in. This should have the
        /// form ...
        /// 
        /// \viewkind4\uc1\pard\cf1\f0\fs20 [DOCUMENT AREA] }
        ///
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_font"></param>
        /// <returns>
        /// The document area as a string.
        /// </returns>
        private string GetDocumentArea(string _text, Font _font)
        {

            StringBuilder _doc = new StringBuilder();

            // Append the standard RTF document area control string
            _doc.Append(RTF_DOCUMENT_PRE);

            // Set the highlight color (the color behind the text) to the
            // third color in the color table.  See GetColorTable for more details.
            _doc.Append(@"\highlight2");

            // If the font is bold, attach corresponding tag
            if (_font.Bold)
                _doc.Append(@"\b");

            // If the font is italic, attach corresponding tag
            if (_font.Italic)
                _doc.Append(@"\i");

            // If the font is strikeout, attach corresponding tag
            if (_font.Strikeout)
                _doc.Append(@"\strike");

            // If the font is underlined, attach corresponding tag
            if (_font.Underline)
                _doc.Append(@"\ul");

            // Set the font to the first font in the font table.
            // See GetFontTable for more details.
            _doc.Append(@"\f0");

            // Set the size of the font.  In RTF, font size is measured in
            // half-points, so the font size is twice the value obtained from
            // Font.SizeInPoints
            _doc.Append(@"\fs");
            _doc.Append((int)Math.Round((2 * _font.SizeInPoints)));

            // Apppend a space before starting actual text (for clarity)
            _doc.Append(@" ");

            // Append actual text, however, replace newlines with RTF \par.
            // Any other special text should be handled here (e.g.) tabs, etc.
            _doc.Append(_text.Replace("\n", @"\par "));

            // RTF isn't strict when it comes to closing control words, but what the
            // heck ...

            // Remove the highlight
            _doc.Append(@"\highlight0");

            // If font is bold, close tag
            if (_font.Bold)
                _doc.Append(@"\b0");

            // If font is italic, close tag
            if (_font.Italic)
                _doc.Append(@"\i0");

            // If font is strikeout, close tag
            if (_font.Strikeout)
                _doc.Append(@"\strike0");

            // If font is underlined, cloes tag
            if (_font.Underline)
                _doc.Append(@"\ulnone");

            // Revert back to default font and size
            _doc.Append(@"\f0");
            _doc.Append(@"\fs20");

            // Close the document area control string
            _doc.Append(RTF_DOCUMENT_POST);

            return _doc.ToString();
        }

        #endregion

        #region Insert Image

        /// <summary>
        /// Inserts an image into the RichTextBox.  The image is wrapped in a Windows
        /// Format Metafile, because although Microsoft discourages the use of a WMF,
        /// the RichTextBox (and even MS Word), wraps an image in a WMF before inserting
        /// the image into a document.  The WMF is attached in HEX format (a string of
        /// HEX numbers).
        /// 
        /// The RTF Specification v1.6 says that you should be able to insert bitmaps,
        /// .jpegs, .gifs, .pngs, and Enhanced Metafiles (.emf) directly into an RTF
        /// document without the WMF wrapper. This works fine with MS Word,
        /// however, when you don't wrap images in a WMF, WordPad and
        /// RichTextBoxes simply ignore them.  Both use the riched20.dll or msfted.dll.
        /// </summary>
        /// <remarks>
        /// NOTE: The image is inserted wherever the caret is at the time of the call,
        /// and if any text is selected, that text is replaced.
        /// </remarks>
        /// <param name="_image"></param>
        public void InsertImage(Image _image)
        {

            StringBuilder _rtf = new StringBuilder();

            // Append the RTF header
            _rtf.Append(RTF_HEADER);

            // Create the font table using the RichTextBox's current font and append
            // it to the RTF string
            _rtf.Append(GetFontTable(this.Font));

            // Create the image control string and append it to the RTF string
            _rtf.Append(GetImagePrefix(_image));

            // Create the Windows Metafile and append its bytes in HEX format
            _rtf.Append(GetRtfImage(_image));

            // Close the RTF image control string
            _rtf.Append(RTF_IMAGE_POST);

            this.SelectedRtf = _rtf.ToString();
        }

        /// <summary>
        /// Creates the RTF control string that describes the image being inserted.
        /// This description (in this case) specifies that the image is an
        /// MM_ANISOTROPIC metafile, meaning that both X and Y axes can be scaled
        /// independently.  The control string also gives the images current dimensions,
        /// and its target dimensions, so if you want to control the size of the
        /// image being inserted, this would be the place to do it. The prefix should
        /// have the form ...
        /// 
        /// {\pict\wmetafile8\picw[A]\pich[B]\picwgoal[C]\pichgoal[D]
        /// 
        /// where ...
        /// 
        /// A	= current width of the metafile in hundredths of millimeters (0.01mm)
        ///		= Image Width in Inches * Number of (0.01mm) per inch
        ///		= (Image Width in Pixels / Graphics Context's Horizontal Resolution) * 2540
        ///		= (Image Width in Pixels / Graphics.DpiX) * 2540
        /// 
        /// B	= current height of the metafile in hundredths of millimeters (0.01mm)
        ///		= Image Height in Inches * Number of (0.01mm) per inch
        ///		= (Image Height in Pixels / Graphics Context's Vertical Resolution) * 2540
        ///		= (Image Height in Pixels / Graphics.DpiX) * 2540
        /// 
        /// C	= target width of the metafile in twips
        ///		= Image Width in Inches * Number of twips per inch
        ///		= (Image Width in Pixels / Graphics Context's Horizontal Resolution) * 1440
        ///		= (Image Width in Pixels / Graphics.DpiX) * 1440
        /// 
        /// D	= target height of the metafile in twips
        ///		= Image Height in Inches * Number of twips per inch
        ///		= (Image Height in Pixels / Graphics Context's Horizontal Resolution) * 1440
        ///		= (Image Height in Pixels / Graphics.DpiX) * 1440
        ///	
        /// </summary>
        /// <remarks>
        /// The Graphics Context's resolution is simply the current resolution at which
        /// windows is being displayed.  Normally it's 96 dpi, but instead of assuming
        /// I just added the code.
        /// 
        /// According to Ken Howe at pbdr.com, "Twips are screen-independent units
        /// used to ensure that the placement and proportion of screen elements in
        /// your screen application are the same on all display systems."
        /// 
        /// Units Used
        /// ----------
        /// 1 Twip = 1/20 Point
        /// 1 Point = 1/72 Inch
        /// 1 Twip = 1/1440 Inch
        /// 
        /// 1 Inch = 2.54 cm
        /// 1 Inch = 25.4 mm
        /// 1 Inch = 2540 (0.01)mm
        /// </remarks>
        /// <param name="_image"></param>
        /// <returns></returns>
        private string GetImagePrefix(Image _image)
        {

            StringBuilder _rtf = new StringBuilder();

            // Calculate the current width of the image in (0.01)mm
            int picw = (int)Math.Round((_image.Width / xDpi) * HMM_PER_INCH);

            // Calculate the current height of the image in (0.01)mm
            int pich = (int)Math.Round((_image.Height / yDpi) * HMM_PER_INCH);

            // Calculate the target width of the image in twips
            int picwgoal = (int)Math.Round((_image.Width / xDpi) * TWIPS_PER_INCH);

            // Calculate the target height of the image in twips
            int pichgoal = (int)Math.Round((_image.Height / yDpi) * TWIPS_PER_INCH);

            // Append values to RTF string
            _rtf.Append(@"{\pict\wmetafile8");
            _rtf.Append(@"\picw");
            _rtf.Append(picw);
            _rtf.Append(@"\pich");
            _rtf.Append(pich);
            _rtf.Append(@"\picwgoal");
            _rtf.Append(picwgoal);
            _rtf.Append(@"\pichgoal");
            _rtf.Append(pichgoal);
            _rtf.Append(" ");

            return _rtf.ToString();
        }

        /// <summary>
        /// Use the EmfToWmfBits function in the GDI+ specification to convert a 
        /// Enhanced Metafile to a Windows Metafile
        /// </summary>
        /// <param name="_hEmf">
        /// A handle to the Enhanced Metafile to be converted
        /// </param>
        /// <param name="_bufferSize">
        /// The size of the buffer used to store the Windows Metafile bits returned
        /// </param>
        /// <param name="_buffer">
        /// An array of bytes used to hold the Windows Metafile bits returned
        /// </param>
        /// <param name="_mappingMode">
        /// The mapping mode of the image.  This control uses MM_ANISOTROPIC.
        /// </param>
        /// <param name="_flags">
        /// Flags used to specify the format of the Windows Metafile returned
        /// </param>
        [DllImportAttribute("gdiplus.dll")]
        private static extern uint GdipEmfToWmfBits(IntPtr _hEmf, uint _bufferSize,
            byte[] _buffer, int _mappingMode, EmfToWmfBitsFlags _flags);


        /// <summary>
        /// Wraps the image in an Enhanced Metafile by drawing the image onto the
        /// graphics context, then converts the Enhanced Metafile to a Windows
        /// Metafile, and finally appends the bits of the Windows Metafile in HEX
        /// to a string and returns the string.
        /// </summary>
        /// <param name="_image"></param>
        /// <returns>
        /// A string containing the bits of a Windows Metafile in HEX
        /// </returns>
        private string GetRtfImage(Image _image)
        {

            StringBuilder _rtf = null;

            // Used to store the enhanced metafile
            MemoryStream _stream = null;

            // Used to create the metafile and draw the image
            Graphics _graphics = null;

            // The enhanced metafile
            Metafile _metaFile = null;

            // Handle to the device context used to create the metafile
            IntPtr _hdc;

            try
            {
                _rtf = new StringBuilder();
                _stream = new MemoryStream();

                // Get a graphics context from the RichTextBox
                using (_graphics = this.CreateGraphics())
                {

                    // Get the device context from the graphics context
                    _hdc = _graphics.GetHdc();

                    // Create a new Enhanced Metafile from the device context
                    _metaFile = new Metafile(_stream, _hdc);

                    // Release the device context
                    _graphics.ReleaseHdc(_hdc);
                }

                // Get a graphics context from the Enhanced Metafile
                using (_graphics = Graphics.FromImage(_metaFile))
                {

                    // Draw the image on the Enhanced Metafile
                    _graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height));

                }

                // Get the handle of the Enhanced Metafile
                IntPtr _hEmf = _metaFile.GetHenhmetafile();

                // A call to EmfToWmfBits with a null buffer return the size of the
                // buffer need to store the WMF bits.  Use this to get the buffer
                // size.
                uint _bufferSize = GdipEmfToWmfBits(_hEmf, 0, null, MM_ANISOTROPIC,
                    EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

                // Create an array to hold the bits
                byte[] _buffer = new byte[_bufferSize];

                // A call to EmfToWmfBits with a valid buffer copies the bits into the
                // buffer an returns the number of bits in the WMF.  
                uint _convertedSize = GdipEmfToWmfBits(_hEmf, _bufferSize, _buffer, MM_ANISOTROPIC,
                    EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);

                // Append the bits to the RTF string
                for (int i = 0; i < _buffer.Length; ++i)
                {
                    _rtf.Append(String.Format("{0:X2}", _buffer[i]));
                }

                return _rtf.ToString();
            }
            finally
            {
                if (_graphics != null)
                    _graphics.Dispose();
                if (_metaFile != null)
                    _metaFile.Dispose();
                if (_stream != null)
                    _stream.Close();
            }
        }

        #endregion

        #region ²åÈëLink

        #region Interop-Defines
        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2_STRUCT
        {
            public UInt32 cbSize;
            public UInt32 dwMask;
            public UInt32 dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public Int32 crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;
            public UInt16 wWeight;
            public UInt16 sSpacing;
            public int crBackColor; // Color.ToArgb() -> int
            public int lcid;
            public int dwReserved;
            public Int16 sStyle;
            public Int16 wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_USER = 0x0400;
        private const int EM_GETCHARFORMAT = WM_USER + 58;
        private const int EM_SETCHARFORMAT = WM_USER + 68;

        private const int SCF_SELECTION = 0x0001;
        private const int SCF_WORD = 0x0002;
        private const int SCF_ALL = 0x0004;

        #region CHARFORMAT2 Flags
        private const UInt32 CFE_BOLD = 0x0001;
        private const UInt32 CFE_ITALIC = 0x0002;
        private const UInt32 CFE_UNDERLINE = 0x0004;
        private const UInt32 CFE_STRIKEOUT = 0x0008;
        private const UInt32 CFE_PROTECTED = 0x0010;
        private const UInt32 CFE_LINK = 0x0020;
        private const UInt32 CFE_AUTOCOLOR = 0x40000000;
        private const UInt32 CFE_SUBSCRIPT = 0x00010000;		/* Superscript and subscript are */
        private const UInt32 CFE_SUPERSCRIPT = 0x00020000;		/*  mutually exclusive			 */

        private const int CFM_SMALLCAPS = 0x0040;			/* (*)	*/
        private const int CFM_ALLCAPS = 0x0080;			/* Displayed by 3.0	*/
        private const int CFM_HIDDEN = 0x0100;			/* Hidden by 3.0 */
        private const int CFM_OUTLINE = 0x0200;			/* (*)	*/
        private const int CFM_SHADOW = 0x0400;			/* (*)	*/
        private const int CFM_EMBOSS = 0x0800;			/* (*)	*/
        private const int CFM_IMPRINT = 0x1000;			/* (*)	*/
        private const int CFM_DISABLED = 0x2000;
        private const int CFM_REVISED = 0x4000;

        private const int CFM_BACKCOLOR = 0x04000000;
        private const int CFM_LCID = 0x02000000;
        private const int CFM_UNDERLINETYPE = 0x00800000;		/* Many displayed by 3.0 */
        private const int CFM_WEIGHT = 0x00400000;
        private const int CFM_SPACING = 0x00200000;		/* Displayed by 3.0	*/
        private const int CFM_KERNING = 0x00100000;		/* (*)	*/
        private const int CFM_STYLE = 0x00080000;		/* (*)	*/
        private const int CFM_ANIMATION = 0x00040000;		/* (*)	*/
        private const int CFM_REVAUTHOR = 0x00008000;


        private const UInt32 CFM_BOLD = 0x00000001;
        private const UInt32 CFM_ITALIC = 0x00000002;
        private const UInt32 CFM_UNDERLINE = 0x00000004;
        private const UInt32 CFM_STRIKEOUT = 0x00000008;
        private const UInt32 CFM_PROTECTED = 0x00000010;
        private const UInt32 CFM_LINK = 0x00000020;
        private const UInt32 CFM_SIZE = 0x80000000;
        private const UInt32 CFM_COLOR = 0x40000000;
        private const UInt32 CFM_FACE = 0x20000000;
        private const UInt32 CFM_OFFSET = 0x10000000;
        private const UInt32 CFM_CHARSET = 0x08000000;
        private const UInt32 CFM_SUBSCRIPT = CFE_SUBSCRIPT | CFE_SUPERSCRIPT;
        private const UInt32 CFM_SUPERSCRIPT = CFM_SUBSCRIPT;

        private const byte CFU_UNDERLINENONE = 0x00000000;
        private const byte CFU_UNDERLINE = 0x00000001;
        private const byte CFU_UNDERLINEWORD = 0x00000002; /* (*) displayed as ordinary underline	*/
        private const byte CFU_UNDERLINEDOUBLE = 0x00000003; /* (*) displayed as ordinary underline	*/
        private const byte CFU_UNDERLINEDOTTED = 0x00000004;
        private const byte CFU_UNDERLINEDASH = 0x00000005;
        private const byte CFU_UNDERLINEDASHDOT = 0x00000006;
        private const byte CFU_UNDERLINEDASHDOTDOT = 0x00000007;
        private const byte CFU_UNDERLINEWAVE = 0x00000008;
        private const byte CFU_UNDERLINETHICK = 0x00000009;
        private const byte CFU_UNDERLINEHAIRLINE = 0x0000000A; /* (*) displayed as ordinary underline	*/

        #endregion

        #endregion


        /// <summary>
        /// Insert a given text as a link into the RichTextBox at the current insert position.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        public void InsertLink(string text)
        {
            InsertLink(text, this.SelectionStart);
        }
        /// <summary>
        /// Insert a given text at a given position as a link. 
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="position">Insert position</param>
        public void InsertLink(string text, int position)
        {
            if (position < 0 || position > this.Text.Length)
                throw new ArgumentOutOfRangeException("position");

            this.SelectionStart = position;
            this.SelectedText = text;
            this.Select(position, text.Length);
            this.SetSelectionLink(true);
            this.Select(position + text.Length, 0);
        }

        /// <summary>
        /// Insert a given text at at the current input position as a link.
        /// The link text is followed by a hash (#) and the given hyperlink text, both of
        /// them invisible.
        /// When clicked on, the whole link text and hyperlink string are given in the
        /// LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        public void InsertLink(string text, string hyperlink)
        {
            InsertLink(text, hyperlink, this.SelectionStart);
        }

        /// <summary>
        /// Insert a given text at a given position as a link. The link text is followed by
        /// a hash (#) and the given hyperlink text, both of them invisible.
        /// When clicked on, the whole link text and hyperlink string are given in the
        /// LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        /// <param name="position">Insert position</param>
        public void InsertLink(string text, string hyperlink, int position)
        {
            if (position < 0 || position > this.Text.Length)
                throw new ArgumentOutOfRangeException("position");

            this.SelectionStart = position;
            this.SelectedRtf = @"{\rtf1\ansi\ansicpg936 " + getNeiMa(text) + @"\v #" + getNeiMa(hyperlink) + @"\v0\par}";
            //@"par\qj\kerning2\f0\fs21 bb{\field{\*\fldinst{HYPERLINK "0001"}";

            this.Select(position, text.Length + hyperlink.Length + 1);
            this.SetSelectionLink(true);
            this.Select(position + text.Length + hyperlink.Length + 1, 0);
        }
        private string getNeiMa(string strtext)
        {
            string str = "";
            for (int i = 0; i < strtext.Length; i++)
            {
                int num2 = Convert.ToInt32(strtext[i]);
                if (num2 > 160)
                {
                    char ch = strtext[i];
                    byte[] bs = Encoding.GetEncoding("gb2312").GetBytes(ch.ToString());
                    string str2 = str;
                    str = string.Concat(new string[] { str2, "\\'", bs[0].ToString("x2"), "\\'", bs[1].ToString("x2") });
                }
                else
                {
                    str += strtext[i].ToString();
                }
            }
            return str;
        }
        
        /// <summary>
        /// Set the current selection's link style
        /// </summary>
        /// <param name="link">true: set link style, false: clear link style</param>
        public void SetSelectionLink(bool link)
        {
            SetSelectionStyle(CFM_LINK, link ? CFE_LINK : 0);
        }
        /// <summary>
        /// Get the link style for the current selection
        /// </summary>
        /// <returns>0: link style not set, 1: link style set, -1: mixed</returns>
        public int GetSelectionLink()
        {
            return GetSelectionStyle(CFM_LINK, CFE_LINK);
        }
        private void SetSelectionStyle(UInt32 mask, UInt32 effect)
        {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (UInt32)Marshal.SizeOf(cf);
            cf.dwMask = mask;
            cf.dwEffects = effect;

            IntPtr wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            IntPtr res = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar);

            Marshal.FreeCoTaskMem(lpar);
        }

        private int GetSelectionStyle(UInt32 mask, UInt32 effect)
        {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (UInt32)Marshal.SizeOf(cf);
            cf.szFaceName = new char[32];

            IntPtr wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            IntPtr res = SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar);

            cf = (CHARFORMAT2_STRUCT)Marshal.PtrToStructure(lpar, typeof(CHARFORMAT2_STRUCT));

            int state;
            // dwMask holds the information which properties are consistent throughout the selection:
            if ((cf.dwMask & mask) == mask)
            {
                if ((cf.dwEffects & effect) == effect)
                    state = 1;
                else
                    state = 0;
            }
            else
            {
                state = -1;
            }

            Marshal.FreeCoTaskMem(lpar);
            return state;
        }
        #endregion
        #region RTF Helpers

        /// <summary>
        /// Creates a font table from a font object.  When an Insert or Append 
        /// operation is performed a font is either specified or the default font
        /// is used.  In any case, on any Insert or Append, only one font is used,
        /// thus the font table will always contain a single font.  The font table
        /// should have the form ...
        /// 
        /// {\fonttbl{\f0\[FAMILY]\fcharset0 [FONT_NAME];}
        /// </summary>
        /// <param name="_font"></param>
        /// <returns></returns>
        private string GetFontTable(Font _font)
        {

            StringBuilder _fontTable = new StringBuilder();

            // Append table control string
            _fontTable.Append(@"{\fonttbl{\f0");
            _fontTable.Append(@"\");

            // If the font's family corresponds to an RTF family, append the
            // RTF family name, else, append the RTF for unknown font family.
            if (rtfFontFamily.Contains(_font.FontFamily.Name))
                _fontTable.Append(rtfFontFamily[_font.FontFamily.Name]);
            else
                _fontTable.Append(rtfFontFamily[FF_UNKNOWN]);

            // \fcharset specifies the character set of a font in the font table.
            // 0 is for ANSI.
            _fontTable.Append(@"\fcharset0 ");

            // Append the name of the font
            _fontTable.Append(_font.Name);

            // Close control string
            _fontTable.Append(@";}}");

            return _fontTable.ToString();
        }

        /// <summary>
        /// Creates a font table from the RtfColor structure.  When an Insert or Append
        /// operation is performed, _textColor and _backColor are either specified
        /// or the default is used.  In any case, on any Insert or Append, only three
        /// colors are used.  The default color of the RichTextBox (signified by a
        /// semicolon (;) without a definition), is always the first color (index 0) in
        /// the color table.  The second color is always the text color, and the third
        /// is always the highlight color (color behind the text).  The color table
        /// should have the form ...
        /// 
        /// {\colortbl ;[TEXT_COLOR];[HIGHLIGHT_COLOR];}
        /// 
        /// </summary>
        /// <param name="_textColor"></param>
        /// <param name="_backColor"></param>
        /// <returns></returns>
        private string GetColorTable(RtfColor _textColor, RtfColor _backColor)
        {

            StringBuilder _colorTable = new StringBuilder();

            // Append color table control string and default font (;)
            _colorTable.Append(@"{\colortbl ;");

            // Append the text color
            _colorTable.Append(rtfColor[_textColor]);
            _colorTable.Append(@";");

            // Append the highlight color
            _colorTable.Append(rtfColor[_backColor]);
            _colorTable.Append(@";}\n");

            return _colorTable.ToString();
        }

        /// <summary>
        /// Called by overrided RichTextBox.Rtf accessor.
        /// Removes the null character from the RTF.  This is residue from developing
        /// the control for a specific instant messaging protocol and can be ommitted.
        /// </summary>
        /// <param name="_originalRtf"></param>
        /// <returns>RTF without null character</returns>
        private string RemoveBadChars(string _originalRtf)
        {
            return _originalRtf.Replace("\0", "");
        }

        #endregion
        ///////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
