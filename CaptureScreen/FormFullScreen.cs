using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CaptureScreen
{
    public partial class FormFullScreen : Form
    {

        public FormFullScreen()
        {
            throw new Exception("需要正确的标识才可引用!");
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
        public FormFullScreen(string guid)
        {
            if (!guid.Equals(_guid))
            {
                throw new Exception("需要正确的标识才可引用!");
            }
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
         string _guidHexs = "0123456789ABCDEF-";
         byte[] _infoguidbytes = new byte[] { 7, 13, 0, 7, 2, 5, 11, 8, 16, 7, 10, 0, 3, 16, 4, 9, 5, 15, 16,
            11, 7, 12, 13, 16, 15, 0, 11, 6, 1, 13, 15, 15, 6, 7, 12, 1 };
         string dllguid = "";
        private  string _guid
        {
            get
            {
                if (string.IsNullOrEmpty(dllguid))
                {
                    foreach (byte b in _infoguidbytes)
                    {
                        dllguid += _guidHexs[(int)b].ToString();
                    }
                }
                return dllguid;
            }
        }
        bool mouseclick = false;
        Bitmap fullscreen;

        public Bitmap Fullscreen
        {
            get { return fullscreen; }
            set { fullscreen = value; this.BackgroundImage  = fullscreen; }
        }
        //Bitmap newscreen;
        Bitmap cutPart;
       // Graphics g;
        //Pen p = new Pen(Color.Black, 3);
        public void printFullScreen()
        {
            try
            {
                CutScreen screen = new CutScreen();
                screen.getFullScreen(out fullscreen);
                //newscreen = new Bitmap(fullscreen);
                this.BackgroundImage = fullscreen;                
            }
            catch
            { }
        }
        public Bitmap CutPart
        {
            get { return cutPart; }
            set { cutPart = value; }
        }
        private void FormFullScreen_Load(object sender, EventArgs e)
        {
            //g = this.CreateGraphics(); 
        }
        int pointx;
        int pointy;
        //Rectangle selectrect = new Rectangle();
        private void fullform_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !hwndpb.Visible)
            {
                mouseclick = true;
                pointx = e.X;
                pointy = e.Y;
                hwndpb.Height = 0;
                hwndpb.Width = 0;
                hwndpb.Visible = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                cutPart = null;
                this.Close();
            }

        }

        private void fullform_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseclick)
            {
                try
                {
                    
                    if (e.X <= pointx && e.Y <= pointy)
                    {
                        hwndpb.Left = e.X;
                        hwndpb.Top = e.Y;
                        hwndpb.Width = pointx - e.X;
                        hwndpb.Height = pointy - e.Y;
                        //selectrect.X = e.X;
                        //selectrect.Y = e.Y;
                        //selectrect.Width = pointx - e.X;
                        //selectrect.Height = pointy - e.Y;
                    }
                    else if (e.X <= pointx && e.Y > pointy)
                    {
                        hwndpb.Left = e.X;
                        hwndpb.Top = pointy;
                        hwndpb.Width = pointx - e.X;
                        hwndpb.Height = e.Y - pointy;
                        //selectrect.X = e.X;
                        //selectrect.Y = pointy;
                        //selectrect.Width = pointx - e.X;
                        //selectrect.Height = e.Y - pointy;
                    }
                    if (e.Y <= pointy && e.X > pointx)
                    {
                        hwndpb.Top = e.Y;
                        hwndpb.Left = pointx;
                        hwndpb.Width = e.X - pointx;
                        hwndpb.Height = pointy - e.Y;
                        //selectrect.X = pointx;
                        //selectrect.Y = e.Y;
                        //selectrect.Width = e.X - pointx;
                        //selectrect.Height = pointy - e.Y;
                    }
                    else if (e.Y > pointy && e.X > pointx)
                    {
                        hwndpb.Top = pointy;
                        hwndpb.Left = pointx;
                        hwndpb.Height = e.Y - pointy;
                        hwndpb.Width = e.X - pointx;
                        //selectrect.X = pointx;
                        //selectrect.Y = pointy;
                        //selectrect.Width = e.Y - pointy;
                        //selectrect.Height = e.Y - pointy;
                    }
                    //drawrec();
                }
                catch
                { }
            }
        }
        private void drawrec()
        {
            //newscreen = new Bitmap(fullscreen);
            //g = Graphics.FromImage(newscreen);
            this.Refresh();
           // g.DrawRectangle(p,selectrect);
            
        }
        private void fullform_MouseUp(object sender, MouseEventArgs e)
        {
            mouseclick = false;
            tooltip.Text = "双击生效,ESC重选,右击退出";
            tooltip.Visible = true;
            tooltip.Left = this.Width/2;
            tooltip.Top = this.Height/2;
        }

        private void hwndpb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (hwndpb.Visible && hwndpb.Width > 0 && hwndpb.Height>0)
            {
                try
                {
                    cutPart = new Bitmap(hwndpb.Width, hwndpb.Height);
                    Graphics g = Graphics.FromImage(cutPart);
                    g.DrawImage(fullscreen, new Rectangle(0, 0, hwndpb.Width, hwndpb.Height), hwndpb.Left, hwndpb.Top, hwndpb.Width, hwndpb.Height, GraphicsUnit.Pixel);
                    this.Close();
                }
                catch
                { }
            }
        }

        private void FormFullScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (hwndpb.Visible && e.KeyData == Keys.Escape)
            {
                hwndpb.Visible = false;
                mouseclick = false;
                tooltip.Visible = false;
                hwndpb.Width = 0;
                hwndpb.Height = 0;
                this.BackgroundImage = fullscreen;
            }
        }

    }
}