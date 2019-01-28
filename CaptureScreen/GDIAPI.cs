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
        IntPtr hdcDest,     //    Ŀ���豸�ľ��   
        int nXDest,         //    Ŀ���������Ͻǵ�X����   
        int nYDest,         //    Ŀ���������Ͻǵ�X����   
        int nWidth,         //    Ŀ�����ľ��εĿ��   
        int nHeight,        //    Ŀ�����ľ��εĳ���   
        IntPtr hdcSrc,      //    Դ�豸�ľ��   
        int nXSrc,          //    Դ��������Ͻǵ�X����   
        int nYSrc,          //    Դ��������Ͻǵ�X����   
        System.Int32 dwRop  //    ��դ�Ĳ���ֵ   
        );
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern IntPtr CreateDC(
        string lpszDriver,    //    ��������   
        string lpszDevice,    //    �豸����   
        string lpszOutput,    //    ���ã������趨λ"NULL"   
        IntPtr lpInitData    //    ����Ĵ�ӡ������   
        );        
    }
}
