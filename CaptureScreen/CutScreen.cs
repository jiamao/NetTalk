using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CaptureScreen
{
    class CutScreen
    {
        public Bitmap getFullScreen()
        {
            //方法一
            //IntPtr window = GDIAPI.GetDesktopWindow();
            //IntPtr windc = GDIAPI.GetDC(window);
            //IntPtr winbitmap = GDIAPI.GetCurrentObject(windc, 7);
            //Bitmap mimage = Image.FromHbitmap(winbitmap);
            //try
            //{
            //    GDIAPI.ReleaseDC(windc);
            //}
            //catch { } 
            //方法二
            //Bitmap mimage = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            //Graphics gp = Graphics.FromImage(mimage);
            //gp.CopyFromScreen(new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y),new Point(0,0),mimage.Size,CopyPixelOperation.SourceCopy);
            
            //方法三
            IntPtr windc = GDIAPI.CreateDC("DISPLAY", null, null, (IntPtr)null);
            Graphics g1 = Graphics.FromHdc(windc);
            Bitmap mimage = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height,g1);
            Graphics g2 = Graphics.FromImage(mimage);
            IntPtr dc1 = g1.GetHdc();
            IntPtr dc2 = g2.GetHdc();
            GDIAPI.BitBlt(dc2, 0, 0, mimage.Width, mimage.Height, dc1, 0, 0, 13369376);
            g1.ReleaseHdc(dc1);
            g2.ReleaseHdc(dc2);
            //GDIAPI.ReleaseDC(windc);
            g1.Dispose();
            g2.Dispose();
            GC.Collect();
            return mimage;
        }
        public void getFullScreen(out Bitmap mimage)
        {
            //方法三
            IntPtr windc = GDIAPI.CreateDC("DISPLAY", null, null,IntPtr.Zero);
            //Graphics g1 = Graphics.FromHdc(windc);
            mimage = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            Graphics g2 = Graphics.FromImage(mimage);
            //IntPtr dc1 = g1.GetHdc();
            IntPtr dc2 = g2.GetHdc();
            GDIAPI.BitBlt(dc2, 0, 0, mimage.Width, mimage.Height, windc, 0, 0, 13369376);
            //g1.ReleaseHdc(dc1);
            g2.ReleaseHdc(dc2); 
            //try
            //{
            //    GDIAPI.ReleaseDC(windc);
            //}
            //catch
            //{ }
            //g1.Dispose();
            g2.Dispose();
            GC.Collect();
        }
    }
}
