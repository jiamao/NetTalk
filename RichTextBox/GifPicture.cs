using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace RichTextBox
{
    /// <summary> 
    /// MyPicture 的摘要说明。
    /// </summary>
    public class MyPicture : System.Windows.Forms.PictureBox,IDisposable
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;
        //private ContextMenuStrip imagemenu = new ContextMenuStrip();
        
        public MyPicture()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();
            //createMenu();
            // TODO: 在 InitializeComponent 调用后添加任何初始化            
        }
        private void createMenu()
        {
            addmenu("copyimg", "复制(&C)", new EventHandler(copyimg));
            //this.ContextMenuStrip = imagemenu;
        }
        private void addmenu(string name,string strtext,EventHandler menuclick)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = strtext;
            item.Name = name;
            item.Click += menuclick;
            //imagemenu.Items.Add(item);
        }
        
        private void copyimg(object sender,EventArgs e)
        {
            Clipboard.SetImage(initimage);
        }
        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        #region 组件设计器生成的代码
        /// <summary> 
        /// 设计器支持所需的方法 - 不要使用代码编辑器 
        /// 修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        private string filename = "";//GIF图片文件的绝对路径
        public string FileName
        {
            set { filename = value; }
            get { return filename; }
        }
        
        public bool IsSent = false;//标识此图片是否需要发送到对方,默认不发送
        public Image initimage;
        //Image[] allframes;
        //int curframe = 0;
        //int count = 0;
        public void start()
        {
            if (System.Drawing.ImageAnimator.CanAnimate(initimage))
            {
                //if (initimage.Height > 200)
                //{
                //initimage = initimage.GetThumbnailImage((int)(initimage.Width * (200.0 / initimage.Height)), 200, null, IntPtr.Zero);
                //}

                this.Image = initimage;
                //this.Height = initimage.Height; this.Width = initimage.Width;
                //System.Drawing.Imaging.FrameDimension fd = new System.Drawing.Imaging.FrameDimension(initimage.FrameDimensionsList[0]);
                //count = initimage.GetFrameCount(fd);
                //allframes = new Image[count];
                //for (int i = 0; i < count; i++)
                //{
                //    initimage.SelectActiveFrame(fd, i);
                //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //    initimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //    allframes[i] = Image.FromStream(ms);
                //    ms.Close();
                //}
                System.Drawing.ImageAnimator.Animate(initimage, new System.EventHandler(OnFrameChanged));
            }
            else
            {
                this.Image = initimage;
            }            
        }
        public void stop()
        {
            if (System.Drawing.ImageAnimator.CanAnimate(initimage))
            {
                System.Drawing.ImageAnimator.StopAnimate(initimage, new System.EventHandler(OnFrameChanged));
            }
        }
        //private class paintHelper : Control
        //{
        //    public void DefaultWndProc(ref Message m)
        //    {
        //        this.DefWndProc(ref m);
        //    }
        //}

        //private const int WM_PAINT = 0x000F;
        //private int lockPaint;
        //private bool needPaint;
        //private paintHelper pHelp = new paintHelper();

        //public void BeginUpdate()
        //{
        //    lockPaint++;
        //}

        //public void EndUpdate()
        //{
        //    lockPaint--;
        //    if (lockPaint <= 0)
        //    {
        //        lockPaint = 0;
        //        if (needPaint)
        //        {
        //            this.Refresh();
        //            needPaint = false;
        //        }
        //    }
        //}

        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case WM_PAINT:
        //            {
        //                //if (lockPaint <= 0)
        //                //{
        //                //    base.WndProc(ref m);
        //                //}
        //                //else
        //                //{
        //                //    needPaint = true;
        //                    pHelp.DefaultWndProc(ref m);
        //                //}
        //                return;
        //            }
        //        case 12:
        //            {
        //                pHelp.DefaultWndProc(ref m);
        //                return;
        //           }
        //    }

        //    base.WndProc(ref m);
        //}
        private void OnFrameChanged(object sender, EventArgs e)
        {
            try
            {
                this.Invalidate(); 
                //curframe++;                
            }
            catch(Exception ex)
            {
                
            }
        }
        //protected override void OnPaint(PaintEventArgs pe)
        //{
        //    base.OnPaint(pe);
        //    g = pe.Graphics;
        //    if (curframe >= count) curframe = 0;

        //    g.DrawImage(allframes[curframe], 0, 0);
        //    g.Dispose();
            
        //}
        //Graphics g;
       
        #region IDisposable 成员
        //[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        //public static extern bool BitBlt(
        //IntPtr hdcDest,    //目标设备的句柄   
        //int nXDest,    //    目标对象的左上角的X坐标   
        //int nYDest,    //    目标对象的左上角的X坐标   
        //int nWidth,    //    目标对象的矩形的宽度   
        //int nHeight,    //    目标对象的矩形的长度   
        //IntPtr hdcSrc,    //    源设备的句柄   
        //int nXSrc,    //    源对象的左上角的X坐标   
        //int nYSrc,    //    源对象的左上角的X坐标   
        //System.Int32 dwRop    //    光栅的操作值   
        //);
        public void Dispose()
        {
            if (System.Drawing.ImageAnimator.CanAnimate(initimage))
            {
                System.Drawing.ImageAnimator.StopAnimate(initimage, new System.EventHandler(this.OnFrameChanged));
            }

            //if (g != null)
            //{ g.Dispose(); g.ReleaseHdc(dc1); }
            GC.SuppressFinalize(this);
            Dispose();
        }

        #endregion

        //#region IDisposable 成员

        //void IDisposable.Dispose()
        //{
        //    if (System.Drawing.ImageAnimator.CanAnimate(this.Image))
        //        System.Drawing.ImageAnimator.StopAnimate(this.Image, new System.EventHandler(this.OnFrameChanged));
        //    GC.SuppressFinalize(this);
        //    Dispose();
        //}

        //#endregion
    }
}

