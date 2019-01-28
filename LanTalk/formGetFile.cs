using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using _poolFactory = MsgPoolFactory.Factory;//消息与好友缓存

namespace LanTalk
{
    public partial class formGetFile : Form
    {
        public formGetFile()
        {
            InitializeComponent();
        }
        long filelength;
        string strfilelen = "0";
        int ipart = 60000;
        int count = 0;
        public long Filelength
        {
            get { return filelength; }
            set { filelength = value;
            strfilelen = " / "+((int)(value / 1024)).ToString() + " (KB)";
            }
        }
        string remotefilename;

        public string Remotefilename
        {
            get { return remotefilename; }
            set { remotefilename = value; }
        }
        string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value; this.Text = value; }
        }
        string[] tempfilenames;

        public string[] Tempfilenames
        {
            get { return tempfilenames; }
            set { tempfilenames = value; }
        }
        string _guid;

        public string Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        string filefrom;

        public string Filefrom
        {
            get { return filefrom; }
            set { filefrom = value; }
        }
        long filecount = 0;
        DateTime oldtime = DateTime.Now;
        long receivelen = 0;
        string strreceivelen = "0";
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                long templen = receivelen;
                if (_poolFactory.Lockedfilebyte == 2)
                {
                    _poolFactory.Lockedfilebyte = 1;
                    System.Threading.Thread.Sleep(5);
                    for (int i = 0; i < _poolFactory.FileByte1.Count; i++)
                    {
                        writeBytes(_poolFactory.FileByte1[i], i);
                        _poolFactory.FileByte1[i].Clear();
                    }
                }
                else
                {
                    _poolFactory.Lockedfilebyte = 2;
                    System.Threading.Thread.Sleep(5);
                    for (int i = 0; i < _poolFactory.FileByte2.Count; i++)
                    {
                        writeBytes(_poolFactory.FileByte2[i], i);
                        _poolFactory.FileByte2[i].Clear();
                    }
                }
                if (receivelen != templen)
                {
                    oldtime = DateTime.Now;
                    fileprocess.Value = (int)(receivelen / ipart);
                    //float rec=((float)receivelen / filelength) * 100;
                    combin.Text = ((int)(receivelen /1024)).ToString() + strfilelen ;
                }
                //if (oldtime < DateTime.Now.AddMilliseconds(-10000))
                //{
                //    timer1.Enabled = false;
                //    MessageBox.Show("超时!");
                //    btncannel_Click(sender,e);
                //}
                if (receivelen >= filelength)
                {
                    Helper.listener.stopreceivefile();
                    timer1.Enabled = false;
                    combin.Text = "合并文件中...";
                    Application.DoEvents();
                    foreach (FileStream f in fs)
                    {
                        f.Close();
                    }
                    this.DialogResult = DialogResult.OK;
                    Helper.combinFiles(filename, tempfilenames);
                    this.Close();
                    string msgpeople = Helper.MyName + "[" + Helper.getIP(Helper.selfIP) + "] (" + DateTime.Now.ToShortTimeString() + ") \r\n";
                    if (Helper.Face.ToLower().Equals("lanmsg"))
                    {
                       Program.formlist.txtMsg.AppendText(msgpeople);
                       Program.formlist.txtMsg.AppendText("成功保存文件["+Path.GetFileName(filename)+"]到：");
                       Program.formlist.txtMsg.InsertLink(Path.GetDirectoryName(filename));
                       Program.formlist.txtMsg.AppendText(Environment.NewLine);
                    }
                    
                    Helper.writeLog(msgpeople + "\\\\成功保存文件[" + Path.GetFileName(filename) + "]到：" + Path.GetDirectoryName(filename));
                }
            }
            catch
            { }
        }
        private void writeBytes(List<byte[]> listbs, int index)
        {
            for (int i = 0; i < listbs.Count;i++ )
            {
                fs[index].Write(listbs[i], 0, listbs[i].Length);
                receivelen += listbs[i].Length;               
            }
        }
        private int getPackCount()
        {
            return _poolFactory.FileByte1.Count;
        }
        private long getPackTotalLen()
        {
            long len = 0;
            foreach (List<byte[]> pack in _poolFactory.FileByte1)
            {
                foreach (byte[] bs in pack)
                {
                    len += (long)bs.Length;
                }
            }
            foreach (List<byte[]> pack in _poolFactory.FileByte2)
            {
                foreach (byte[] bs in pack)
                {
                    len += (long)bs.Length;
                }
            }
            return len;
        }
        FileStream[] fs;
        private void formGetFile_Load(object sender, EventArgs e)
        {
            int lastpart = (int)(filelength % ipart);
            if (lastpart == 0)
            {
                count = (int)(filelength / ipart);
            }
            else
            {
                count = (int)(filelength / ipart) + 1;
            }
            fs=new FileStream[tempfilenames.Length];
            for (int i = 0; i < tempfilenames.Length;i++ )
            {
                fs[i] = new FileStream(tempfilenames[i],FileMode.OpenOrCreate,FileAccess.Write);
            }
            fileprocess.Maximum = count;
            timer1.Enabled = true;
        }

        private void formGetFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Enabled = false;
            Helper.DownLoading = false;
            _poolFactory.FileByte1.Clear();
            _poolFactory.FileByte2.Clear();
            Helper.listener.stopreceivefile();
        }

        private void btncannel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();            
        }
        bool mouseclick = false;
        Point mouseclickpoint = new Point();
        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            mouseclick = true;
            mouseclickpoint = Control.MousePosition;
        }

        private void control_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseclick)
            {
                mouseclick = false;
            }
        }

        private void control_MouseLeave(object sender, EventArgs e)
        {
            mouseclick = false;
        }

        private void control_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseclick)
            {
                Point newpoint = Control.MousePosition;
                int offsetx = newpoint.X - mouseclickpoint.X;
                int offsety = newpoint.Y - mouseclickpoint.Y;
                if (Math.Abs(offsetx) > 5 || Math.Abs(offsety) > 5)
                {
                    this.Left += offsetx;
                    this.Top += offsety;
                    mouseclickpoint = newpoint;
                }
            }
        }

    }
}