using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace RichTextBox
{
    /// <summary> 
    /// MyPicture ��ժҪ˵����
    /// </summary>
    public class MyPicture : System.Windows.Forms.PictureBox,IDisposable
    {
        /// <summary> 
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;
        //private ContextMenuStrip imagemenu = new ContextMenuStrip();
        
        public MyPicture()
        {
            // �õ����� Windows.Forms ���������������ġ�
            InitializeComponent();
            //createMenu();
            // TODO: �� InitializeComponent ���ú�����κγ�ʼ��            
        }
        private void createMenu()
        {
            addmenu("copyimg", "����(&C)", new EventHandler(copyimg));
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
        /// ������������ʹ�õ���Դ��
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
        
        #region �����������ɵĴ���
        /// <summary> 
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
        /// �޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        private string filename = "";//GIFͼƬ�ļ��ľ���·��
        public string FileName
        {
            set { filename = value; }
            get { return filename; }
        }
        
        public bool IsSent = false;//��ʶ��ͼƬ�Ƿ���Ҫ���͵��Է�,Ĭ�ϲ�����
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
       
        #region IDisposable ��Ա
        //[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        //public static extern bool BitBlt(
        //IntPtr hdcDest,    //Ŀ���豸�ľ��   
        //int nXDest,    //    Ŀ���������Ͻǵ�X����   
        //int nYDest,    //    Ŀ���������Ͻǵ�X����   
        //int nWidth,    //    Ŀ�����ľ��εĿ��   
        //int nHeight,    //    Ŀ�����ľ��εĳ���   
        //IntPtr hdcSrc,    //    Դ�豸�ľ��   
        //int nXSrc,    //    Դ��������Ͻǵ�X����   
        //int nYSrc,    //    Դ��������Ͻǵ�X����   
        //System.Int32 dwRop    //    ��դ�Ĳ���ֵ   
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

        //#region IDisposable ��Ա

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

