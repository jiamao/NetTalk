using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DRAWINFO
{
    public partial class formDrawInfo : Form
    {
        public formDrawInfo()
        {
            throw new Exception("需要正确的标识来引用此类!");
            InitializeComponent();
        }
        public formDrawInfo(string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("需要正确的标识来引用此类!");
            InitializeComponent();
        }
        Bitmap drawimg;//画布
        Graphics g;
        Pen drawpen = new Pen(Color.Black);
        int penlen = 1;//笔宽
        int flag = 0;//0表示钢笔，1表示橡皮
        Color curcolor = Color.Black;
        List<Bitmap> history = new List<Bitmap>();//历史记录
        //透明度
        byte tmd = 255;
        private void formDrawInfo_Load(object sender, EventArgs e)
        {
            drawimg = new Bitmap(this.pbdraw.Width, this.pbdraw.Height);
            g = Graphics.FromImage(drawimg);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.Clear(this.pbdraw.BackColor);
            this.bwcom.SelectedIndex = 0;
            //addhistory(drawimg);
        }

        private void bwcom_SelectedIndexChanged(object sender, EventArgs e)
        {
            penlen = int.Parse(bwcom.Text);
            if (flag == 0)
            {
                drawpen = new Pen(curcolor, penlen);
            }
            else if (flag == 1)
            {
                drawpen = new Pen(Color.White, penlen);
            }
        }

        private void btncolor_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                curcolor = Color.FromArgb(tmd,colorDialog1.Color);
                btncolor.BackColor = colorDialog1.Color;
                btncolor.ForeColor = Color.FromArgb(colorDialog1.Color.ToArgb() + 1000);
                if (flag == 0)
                {
                    drawpen = new Pen(curcolor, penlen);
                }
            }
        }

        private void btnup_Click(object sender, EventArgs e)
        {
            if (tmd < 255)
            {
                tmd++;
                changetmd();
            }
        }

        private void btndown_Click(object sender, EventArgs e)
        {
            if (tmd > 1)
            {
                tmd--;
                changetmd();
            }
        }
        private void changetmd()
        {
            curcolor = Color.FromArgb(tmd, curcolor);
            if (flag == 0)
            {
                drawpen = new Pen(curcolor, penlen);
            }
            float f = (float)tmd / 255 * 100;
            tmdL.Text = f.ToString("0.00") + "%";
        }
        private void btnpen_Click(object sender, EventArgs e)
        {
            drawpen = new Pen(curcolor, penlen);
            this.pbdraw.Cursor = Cursors.Cross;
            flag = 0;
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            drawpen = new Pen(Color.White, penlen);
            this.pbdraw.Cursor = Cursors.NoMove2D;
            flag = 1;
        }
        bool mouseclick = false;
        Point mouseclickposition;
        private void pbdraw_MouseDown(object sender, MouseEventArgs e)
        {
            mouseclick = true;
            mouseclickposition = new Point(e.X,e.Y);
            
        }

        private void pbdraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseclick)
            {
                Point curp=new Point(e.X, e.Y);
                g.DrawLine(drawpen, mouseclickposition, curp);
                mouseclickposition = curp;
                this.pbdraw.Image = drawimg;
            }
        }

        private void pbdraw_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseclick)
            {                
                mouseclick = false;
                addhistory(drawimg);
            }
        }

        private void btnallclear_Click(object sender, EventArgs e)
        {
            //drawimg = new Bitmap(this.pbdraw.Width, this.pbdraw.Height);
            //g.Dispose();
            //g = Graphics.FromImage(drawimg);
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.Clear(this.pbdraw.BackColor);
            this.pbdraw.Image = drawimg;
            addhistory(drawimg);
        }
        public delegate void drawimgend(Bitmap bmp);
        public event drawimgend sendimg;
        private void btncopy_Click(object sender, EventArgs e)
        {
            if (sendimg != null)
                sendimg(new Bitmap(drawimg));
        }

        private void pbdraw_MouseLeave(object sender, EventArgs e)
        {
            mouseclick = false;
        }

        private void optia_Scroll(object sender, EventArgs e)
        {
            tmd = (byte)optia.Value;
            changetmd();
        }

        private void formDrawInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                history.Clear();                
                g.Dispose();
                drawimg.Dispose();                
                GC.Collect();
            }
            catch
            { }
        }
        private void addhistory(Bitmap bmp)
        {
            if (history.Count > 9)
            {
                history.Remove(history[0]);
            }
            Bitmap hisbmp = new Bitmap(bmp);
            history.Add(hisbmp);
            btnback.Enabled = true;
            curindex = history.Count - 1;
            btnnext.Enabled = false;
        }
        int curindex = 0;
        private void back()
        {
            if (curindex > 0)
            {
                curindex--;
                g.DrawImage(history[curindex], new Point(0, 0));
                //int tmp = curindex;
                //addhistory(drawimg);
                //curindex = tmp;
                btnnext.Enabled = true;
                pbdraw.Refresh();                
            }
            else
            {
                btnback.Enabled = false;
            }
        }
        private void next()
        {
            if (curindex < history.Count - 1)
            {
                curindex++;
                g.DrawImage(history[curindex], new Point(0, 0));
                //int tmp = curindex;
                //addhistory(drawimg);
               // btnnext.Enabled = true;
                //curindex = tmp;
                btnback.Enabled = true;
                pbdraw.Refresh();                
            }
            else
            {
                btnnext.Enabled = false;
            }
        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            next();
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            back();
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
    }
}