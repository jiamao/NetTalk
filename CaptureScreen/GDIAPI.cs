using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime;

namespace CaptureScreen
{
    public class GDIAPI
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr windowhander);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr windowhander);
        [DllImport("gdi32.dll")]
        public extern static IntPtr GetCurrentObject(IntPtr hdc, ushort objectType);
        [DllImport("user32.dll")]
        public extern static void ReleaseDC(IntPtr hdc);
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern bool BitBlt(
        IntPtr hdcDest,     //    目标设备的句柄   
        int nXDest,         //    目标对象的左上角的X坐标   
        int nYDest,         //    目标对象的左上角的X坐标   
        int nWidth,         //    目标对象的矩形的宽度   
        int nHeight,        //    目标对象的矩形的长度   
        IntPtr hdcSrc,      //    源设备的句柄   
        int nXSrc,          //    源对象的左上角的X坐标   
        int nYSrc,          //    源对象的左上角的X坐标   
        System.Int32 dwRop  //    光栅的操作值   
        );
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern IntPtr CreateDC(
        string lpszDriver,    //    驱动名称   
        string lpszDevice,    //    设备名称   
        string lpszOutput,    //    无用，可以设定位"NULL"   
        IntPtr lpInitData    //    任意的打印机数据   
        );        
    }
}
