using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using _poolFactory = MsgPoolFactory.Factory;//消息与好友缓存

namespace LanTalk
{
    public partial class formMsg : Form
    {
        public formMsg()
        {
            InitializeComponent();
        }
        string _friendIP;
        public string FriendIP
        {
            get { return _friendIP; }
            set { _friendIP = value; }
        }
        string _strMsg;
        public string Message
        {
            get { return _strMsg; }
            set  
            {
                txtMsg.Text = _strMsg = value;
            }
        }
        Friend.Friend _curFriend;
        private void formMsg_Load(object sender, EventArgs e)
        {
            _curFriend = _poolFactory.findFriend(FriendIP);
            if (_curFriend == null)
            {
                _curFriend = new Friend.Friend();
                _curFriend.GroupName = "未知组";
                _curFriend.Header = "_1";
                _curFriend.Ip = Helper.getByteIP(FriendIP);
                _curFriend.Name = FriendIP;
                _curFriend.State = Friend.Friend.EState.InLine;
                _curFriend.Ver = "LanTalk";
            }
            this.Text = _friendIP;
            lblTiTle.Text = _curFriend.Name + " [" + _friendIP + "]";
            this.Icon = MsgHelper.getIconByName(_curFriend.Header);
            thisicon.Image = MsgHelper.getImageByName(_curFriend.Header);
            txtsend.DragDrop+=this.txtsend_dragdrop;
            txtsend.DragEnter+=this.txtsend_dragenter;
            txtMsg.ForeColor = Color.DarkGreen;
        }
        /// <summary>
        /// 重读消息
        /// </summary>
        public void readMsg()
        {
            try
            {
                _curFriend = _poolFactory.findFriend(FriendIP);
                if (_curFriend.Messages.Count > 0)
                {
                    MsgInfo.MsgInfo[] allmsg = new MsgInfo.MsgInfo[_curFriend.Messages.Count];
                    _curFriend.Messages.CopyTo(allmsg, 0);
                    _curFriend.Messages.Clear();
                    foreach (MsgInfo.MsgInfo info in allmsg)
                    {
                        txtMsg.AppendText(_curFriend.GroupName + "->");
                        //txtMsg.InsertLink(" " + _curFriend.Name + "[" + Helper.getIP(_curFriend.Ip) + "] ");
                        string msgpeple = _curFriend.Name +" 对 [" + Helper.getAllName(info.SendToID) + "] 说：(" + DateTime.Now.ToShortTimeString() + ")\r\n";
                        txtMsg.AppendText(msgpeple);
                        int position = txtMsg.Text.Length;
                        txtMsg.SelectionStart = position;

                        if (info.ProtocolType == MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT)
                        {
                            string getinfo = info.SendInfo;

                            StringBuilder receinfo = new StringBuilder(getinfo);
                            int index = -1;
                            string guid = info.InfoType;
                            int regetcount = 0;
                            int findposition = 0;
                            while (true)
                            {
                                index = getinfo.IndexOf("{\\part", findposition);
                                if (index < 0) break;
                                else
                                {
                                    int len = getinfo.IndexOf("}", index + 1) - index + 1;
                                    string strimg = getinfo.Substring(index, len);

                                    MsgInfo.MsgInfo imginfo = _poolFactory.findImgPart(guid, strimg);
                                    if (imginfo != null)
                                    {
                                        findposition = index + 1;
                                        regetcount = 0;
                                        string part = imginfo.SendInfo.Replace(strimg, "");
                                        receinfo = receinfo.Replace(strimg, part);
                                        imginfo = null;
                                    }
                                    else
                                    {
                                        if (regetcount < 2)
                                        {
                                            MsgHelper.getLostedImgPack(guid, strimg, Helper.getIP(_curFriend.Ip));
                                            regetcount++;
                                            Thread.Sleep(200);
                                            if (_curFriend.IsNet || Helper.IsNet)
                                            {
                                                Thread.Sleep(300);
                                            }
                                            continue;
                                        }
                                        break;
                                        //getinfo = getinfo.Replace(strimg, "");
                                    }
                                }
                            }
                            _poolFactory.removeImgByGuid(guid);
                            int oldtextlen = txtMsg.Text.Length;
                            getinfo = receinfo.ToString();
                            int imgp = getinfo.IndexOf("{\\image");
                            string imagertf = "";
                            if (imgp > 0)
                            {
                                imagertf = getinfo.Substring(imgp);
                                getinfo = getinfo.Substring(0, imgp);
                                imgp = 0;
                            }
                            txtMsg.SelectedRtf = getinfo;

                            ///自动回复
                            if (Helper.Mystate != Friend.Friend.EState.InLine && _curFriend.State == Friend.Friend.EState.InLine && !info.SendToID.Contains(",") && !info.SendToID.Contains(System.Net.IPAddress.Broadcast.ToString()))
                            {
                                MsgHelper.sendmsg(new string[] { info.SendFromID }, Helper.strToRTF(MsgHelper.getToolTip(Helper.Mystate)), txtsend.ForeColor, txtsend.Font, info.SendFromID);
                            }
                            //如果有图片存在
                            try
                            {
                                while (imgp >= 0)
                                {
                                    int imgheader = imagertf.IndexOf(",", imgp);
                                    int p = int.Parse(imagertf.Substring(imgp + 7, imgheader - imgp - 7));
                                    imgp = imagertf.IndexOf("{\\image", imgp + 1);
                                    string[] imgs;
                                    if (imgp < 0)
                                    {
                                        imgs = imagertf.Substring(imgheader + 1).Split('|');
                                    }
                                    else
                                    {
                                        imgs = imagertf.Substring(imgheader + 1, imgp - imgheader - 1).Split('|');
                                    }
                                    byte[] bs = new byte[imgs.Length];
                                    for (int k = 0; k < bs.Length; k++)
                                    {
                                        bs[k] = byte.Parse(imgs[k]);
                                    }
                                    MemoryStream ms = new MemoryStream(bs);
                                    System.Drawing.Image myimg = Image.FromStream(ms);
                                    insertMsgPicture(myimg, oldtextlen + p);
                                }
                            }
                            catch
                            { }
                        }
                        else if (info.ProtocolType == MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_FILE)
                        {
                            long filelen = long.Parse(info.SendInfo.Split('|')[1]);
                            float filem = (float)filelen / (1024 * 1024);
                            txtMsg.AppendText("收到来自组:" + _curFriend.GroupName + "->" + _curFriend.Name + " 的文件(" + filem.ToString("0.00") + "M)：");

                            string guid = Guid.NewGuid().ToString("n");
                            txtMsg.InsertLink(Path.GetFileName(info.SendInfo.Split('|')[0]), guid);
                            info.InfoGuid = guid;
                            _poolFactory.Filemsg.Add(info);
                            _poolFactory.SendedMsgImpLife.Add(guid, DateTime.Now.AddMinutes(10));
                        }
                        txtMsg.AppendText(Environment.NewLine);
                        txtMsg.ScrollToCaret();
                        txtsend.Focus();
                    }                    
                }

            }
            catch (Exception ex)
            {
                Helper.writeLog("formmsg.readmsg"+ex.Message);
            }            
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
                    //Helper.moveForm(this.Handle, this.Left + offsetx, this.Top + offsety, this.Width, this.Height);
                    this.Left += offsetx;
                    this.Top += offsety;
                    mouseclickpoint = newpoint;
                }
            }
        }

        /// <summary>
        /// 粘贴图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtsend_KeyDown(object sender, KeyEventArgs e)
        {
            if (Helper.SendmsgButton.Equals("ctrlenter") && e.Modifiers == Keys.Control && e.KeyCode == Keys.Enter)
            {
                btnsend_Click(sender, e);
                e.Handled = true;
            }
            else if (Helper.SendmsgButton.Equals("enter") && e.KeyCode == Keys.Enter && e.Modifiers != Keys.Control)
            {
                btnsend_Click(sender, e);
                e.Handled = true;
            }
        }
        private void setimg(Image img,int position)
        {
            if (img != null)
            {
                txtsend.SelectionStart = position;
                txtsend.Paste();
            }
        }
        private void redimg(Image img, int position)
        {
            if (img != null)
            {
                txtMsg.SelectionStart = position;
                txtMsg.Paste();
            }
        }
        uint tag = 0;
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsend_Click(object sender, EventArgs e)
        {
            try
            {
                if (Helper.IsNet && !MsgHelper.netlogined)
                {
                    txtMsg.AppendText("没有登录,请登录后再发送消息!");
                    return;
                }
                btnsend.Enabled = false;
                if (String.IsNullOrEmpty(txtsend.Text)) return;
                string msgpeople ="你对 " + Helper.getAllName(_friendIP) + " 说：(" + DateTime.Now.ToShortTimeString() + ")\r\n";
                txtMsg.AppendText(msgpeople);
                StringBuilder sendrtf = new StringBuilder(txtsend.Rtf);
                int oldp = txtMsg.Text.Length;
                txtMsg.SelectedRtf = sendrtf.ToString();
                RichTextBox.REOBJECT reObject = new RichTextBox.REOBJECT();
                for (int i = 0; i < this.txtsend.GetRichEditOleInterface().GetObjectCount(); i++)
                {
                    this.txtsend.GetRichEditOleInterface().GetObject(i, reObject, RichTextBox.GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
                    tag = reObject.dwUser;
                    RichTextBox.MyPicture mypic = _sendIMG.Find(findPic);
                    if (mypic != null)
                    {
                        int p = int.Parse(mypic.Name.Substring(1));
                        //txtMsg.SelectionStart = oldp + p;                        
                        MemoryStream ms = new MemoryStream();
                        mypic.initimage.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        insertMsgPicture(Image.FromStream(ms), oldp + p);
                        sendrtf.Append("{\\image" + p.ToString() + ",");
                        byte[] bs = ms.ToArray();
                        for (int j = 0; j < bs.Length; j++)
                        {
                            if (j == bs.Length - 1)
                            {
                                sendrtf.Append(bs[j].ToString());
                                break;
                            }
                            sendrtf.Append(bs[j].ToString() + "|");
                        }
                    }
                }
                txtMsg.AppendText(Environment.NewLine);
                //MsgHelper.sendMsg(new string[] { _friendIP }, sendrtf.ToString(), _friendIP);
                _poolFactory.addSendInfo(sendrtf.ToString(), new string[] { _friendIP},_friendIP);
                txtMsg.ScrollToCaret();                
            }
            catch (Exception ex)
            {
                Helper.writeLog("formmsg.sendmsg "+ex.Message);
            }
            finally
            {
                btnsend.Enabled = true;
                txtsend.Clear();
                _sendIMG.Clear();
                txtsend.Focus();
            }

        }
        private bool findPic(RichTextBox.MyPicture pic)
        {
            return pic.Tag.ToString().Equals(tag.ToString());
        }
        /// <summary>
        /// 字体设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnfont_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = txtsend.Font;
            fontDialog1.Color = txtsend.ForeColor;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                if (txtsend.SelectionLength > 0)
                {
                    txtsend.SelectionColor = fontDialog1.Color;
                    txtsend.SelectionFont = fontDialog1.Font;
                }
                else
                {
                    txtsend.Font = fontDialog1.Font;
                    txtsend.ForeColor = fontDialog1.Color;
                }
            }
            txtsend.Focus();
        }
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnface_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openImage = new OpenFileDialog();
                openImage.Filter = "BMP(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|PNG(*.png)|*.png|Images(*.bmp;*;jpg;*.gif;*.png)|*.bmp;*;jpg;*.gif;*.png";
                openImage.FilterIndex = 5;
                openImage.InitialDirectory = Path.Combine(Application.StartupPath, "Images");
                openImage.Multiselect = false;
                openImage.Title = "如果图片过大，请确保对方使用的是本版本！如果是GIF动画请确保高度不要大于200，不然当静态图片处理。";

                if (openImage.ShowDialog() == DialogResult.OK)
                {
                    string strimgfile = openImage.FileName;
                    FileInfo fileinfo = new FileInfo(strimgfile);
                    if (Helper.IsNet && fileinfo.Length > 80 * 1024)
                    {
                        txtMsg.AppendText("外网用户暂不支持发送大于80KB的图片!\r\n");
                        return;
                    }
                    insertImage(Image.FromFile(strimgfile));                                                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtsend.Focus();
            }
        }

        private void txtMsg_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void txtsend_dragenter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }
        private void txtsend_dragdrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    object obj = e.Data.GetData(DataFormats.FileDrop);
                    if (obj != null)
                    {
                        string[] files = (string[])obj;
                        {
                            foreach (string file in files)
                            {
                                FileInfo fi = new FileInfo(file);
                                if (fi.Extension.ToLower().Equals(".txt") || fi.Extension.ToLower().Equals(".html") || fi.Extension.ToLower().Equals(".sql"))
                                {

                                    txtsend.AppendText(File.ReadAllText(file, Encoding.GetEncoding("gb2312")));

                                }
                                else if (fi.Extension.ToLower().Equals(".rtf"))
                                {
                                    StreamReader sr = fi.OpenText();
                                    txtsend.SelectedRtf = sr.ReadToEnd();
                                    sr.Close();

                                }
                                else if (fi.Extension.ToLower().Equals(".bmp") || fi.Extension.ToLower().Equals(".gif") || fi.Extension.ToLower().Equals(".jpg") || fi.Extension.ToLower().Equals(".png"))
                                {
                                    FileStream fs = fi.Open(FileMode.Open, FileAccess.Read);
                                    Image img = Image.FromStream(fs);
                                    fs.Close();
                                    insertImage(img);

                                }
                                else
                                {
                                    MsgHelper.sendFileMsg(file, new string[] { _friendIP }, _friendIP);                                    
                                }
                            }
                        }
                    }
                }
                if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    object obj = e.Data.GetData(DataFormats.Text);
                    if (obj != null)
                        txtsend.AppendText(obj.ToString());
                }
                if (e.Data.GetDataPresent(DataFormats.Rtf))
                {
                    object obj = e.Data.GetData(DataFormats.Rtf);
                    if (obj != null)
                        txtsend.SelectedRtf = obj.ToString();
                }
                if (e.Data.GetDataPresent(DataFormats.Html))
                {
                    object obj = e.Data.GetData(DataFormats.Html);
                    if (obj != null)
                        txtsend.AppendText(obj.ToString());
                }
            }
            catch
            { }
            finally
            {
                e.Effect = DragDropEffects.None;
            }
        }
        /// <summary>
        /// 单击超链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtsend_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                if (e.LinkText.Contains("#") && !e.LinkText.Contains("\\") && !e.LinkText.ToLower().StartsWith("http://") && !e.LinkText.ToLower().StartsWith("\\\\"))
                {
                    if (Helper.DownLoading) { MessageBox.Show("暂不支持多个文件同时下载!"); return; }
                    string guid = e.LinkText.Substring(e.LinkText.LastIndexOf("#") + 1);
                    string filename = e.LinkText.Substring(0, e.LinkText.LastIndexOf("#"));
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.CheckPathExists = true;
                    sfd.FileName = filename;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string localfilename = sfd.FileName;
                        MsgInfo.MsgInfo fileinfo = _poolFactory.findFileMsg(guid);
                        string filepath = fileinfo.SendInfo.Split('|')[0];
                        long filelen = long.Parse(fileinfo.SendInfo.Split('|')[1]);
                        string ports = Helper.listener.startreceivefile(Helper.getIP(Helper.selfIP), Helper.ThreadCount);
                        _poolFactory.Lockedfilebyte = 2;
                        string[] allport = ports.Split(',');
                        string[] tempfiles = new string[allport.Length];
                        for (int i = 0; i < allport.Length; i++)
                        {
                            tempfiles[i] = Path.GetTempFileName();
                            _poolFactory.FileByte1.Add(new List<byte[]>());
                            _poolFactory.FileByte2.Add(new List<byte[]>());
                        }
                        MsgHelper.sendGetFileMsg(filepath + "|" + guid + "|" + ports, fileinfo.SendFromID);//发送获取文件消息

                        formGetFile getfile = new formGetFile();
                        getfile.StartPosition = FormStartPosition.Manual;
                        getfile.Top = Screen.PrimaryScreen.WorkingArea.Top;
                        getfile.Left = Screen.PrimaryScreen.WorkingArea.Right - getfile.Width;
                        getfile.Tempfilenames = tempfiles;
                        getfile.Filelength = filelen;
                        getfile.Filename = localfilename;
                        getfile.Guid = guid;
                        getfile.Filefrom = fileinfo.SendFromID;
                        getfile.Remotefilename = filepath;
                        Helper.DownLoading = true;
                        getfile.Show();
                    }
                }
                else if (e.LinkText.Contains("[") && e.LinkText.EndsWith("] ") && !e.LinkText.ToLower().StartsWith("http://") && !e.LinkText.ToLower().StartsWith("\\\\"))
                {
                    string friendip = e.LinkText.Substring(e.LinkText.IndexOf('[') + 1);
                    friendip = friendip.Substring(0, friendip.Length - 2);
                    IntPtr formmsghandle = MsgHelper.FindWindow(friendip);
                    formMsg formmsg;
                    if (formmsghandle != IntPtr.Zero) { formmsg = (formMsg)Form.FromHandle(formmsghandle); formmsg.Activate(); return; }
                    formmsg = new formMsg();
                    formmsg.StartPosition = FormStartPosition.CenterScreen;
                    formmsg.FriendIP = friendip;
                    formmsg.Show();
                    formmsg.Activate();
                }
                else
                {
                    System.Diagnostics.Process.Start("IExplore.exe", e.LinkText);
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex.Message);
            }           
        }

        private void 剪切CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtMsg.Focused)
            {
                txtMsg.Cut();
            }
            else if (txtsend.Focused)
            {
                txtsend.Cut();
            }
        }

        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtMsg.Focused)
            {
                txtMsg.Copy();
            }
            else if (txtsend.Focused)
            {
                txtsend.Copy();
            }
        }

        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtMsg.Focused)
            {
                txtMsg.Paste();
            }
            else if (txtsend.Focused)
            {
                txtsend.Paste();
            }
        }

        private void txtMsg_Enter(object sender, EventArgs e)
        {
            剪切CToolStripMenuItem.Enabled = false;
            粘贴PToolStripMenuItem.Enabled = false;
        }

        private void txtsend_Enter(object sender, EventArgs e)
        {
            剪切CToolStripMenuItem.Enabled = true;
            粘贴PToolStripMenuItem.Enabled = true;
        }
        /// <summary>
        /// 截屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cutscreen_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                Thread.Sleep(100);
                CaptureScreen.FormFullScreen formscreen = new CaptureScreen.FormFullScreen(Helper._guid);
                
                formscreen.printFullScreen();
                formscreen.ShowDialog();
                this.Visible = true;
                if (formscreen.CutPart == null) return;
                txtsend.InsertImage((Image)formscreen.CutPart);
            }
            catch (Exception ex)
            {
                Helper.writeLog("formmsg.cutscreen_Click"+ex.Message);
            }
        }
        List<RichTextBox.MyPicture> _sendIMG = new List<RichTextBox.MyPicture>();
        List<RichTextBox.MyPicture> _recIMG = new List<RichTextBox.MyPicture>();
        int piccount = 0;
        private void insertImage(Image img)
        {
            RichTextBox.MyPicture pic = new RichTextBox.MyPicture();
            pic.Name = "p" + txtsend.SelectionStart.ToString();
            pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pic.BackColor = this.txtsend.BackColor;
            pic.Tag = piccount;
            //foreach (RichTextBox.MyPicture tempic in _sendIMG)
            //{
            //    try
            //    {
            //        int p = int.Parse(tempic.Name.Substring(1));
            //        if (p >= txtsend.SelectionStart)
            //        {
            //            p++;
            //            tempic.Name = "p" + p.ToString();
            //        }
            //    }
            //    catch
            //    { }
            //}
            pic.initimage = img;
            //System.Drawing.ImageAnimator.Animate(pic.Image, new System.EventHandler(this.OnFrameChanged));
            txtsend.InsertMyControl(pic);
            _sendIMG.Add(pic);
            pic.start();
            piccount++;
            txtsend.ScrollToCaret();
        }
        //bool msginviate = false;
        private void insertMsgPicture(RichTextBox.MyPicture mypic)
        {
            //if (!msginviate)
            //System.Drawing.ImageAnimator.Animate(mypic.Image, new System.EventHandler(this.OnFrameMSGChanged));
            txtMsg.InsertMyControl(mypic);            
        }
        private void insertMsgPicture(Image img, int p)
        {
            RichTextBox.MyPicture pic = new RichTextBox.MyPicture();
            pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pic.BackColor = this.txtMsg.BackColor;
            pic.initimage = img;
            
            txtMsg.SelectionStart = p;
            txtMsg.InsertMyControl(pic);
            pic.start();
            _recIMG.Add(pic);
        }
        #region 重新绘制 richbox 事件
        private void RTBRecordOnFrameChanged(object sender, EventArgs e)
        {
            this.txtsend.Invalidate();
            //this.RTBSend.Focus();
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            this.txtsend.Invalidate();
            //MessageBox.Show("");
        }
        private void OnFrameMSGChanged(object sender, EventArgs e)
        {
            this.txtMsg.Invalidate();
        }
        #endregion
        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsendfile_Click(object sender, EventArgs e)
        {
            if (Helper.IsNet)
            {
                txtMsg.AppendText("外网用户暂不支持发送文件!\r\n");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in ofd.FileNames)
                {
                    txtMsg.AppendText("发送文件(" + filename + ")给:" + Helper.getAllName(_friendIP) + "!\r\n");
                    MsgHelper.sendFileMsg(ofd.FileName, new string[] { _friendIP }, _friendIP);
                }
            }
        }
        Graphics g;
        /// <summary>
        /// 画界面
        /// </summary>
        private void drawFormFace()
        {
            try
            {
                this.Refresh();
                g = this.CreateGraphics();
                GraphicsPath gp = new GraphicsPath();
                gp.AddClosedCurve(initbianjiao(), 1);
                Pen p = new Pen(Color.Red);
                this.Region = new Region(gp);
                g.DrawPath(p, gp);
                //g.DrawLine(p, new Point(0, r), new Point(0, this.Height - r));
                g.DrawLine(p, new Point(r, this.Height - 1), new Point(this.Width - r, this.Height - 1));
                g.DrawLine(p, new Point(this.Width - 1, this.Height - r), new Point(this.Width - 1, r));
                gp.Dispose();
                p.Dispose();
                if (Helper.SendmsgButton.Equals("enter"))
                {
                    tpbtnface.SetToolTip(btnsend, "ENTER发送消息");
                    tpbtnface.SetToolTip(txtsend, "ENTER发送消息");
                }
                else
                {
                    tpbtnface.SetToolTip(btnsend, "CTRL+ENTER发送消息");
                    tpbtnface.SetToolTip(txtsend, "CTRL+ENTER发送消息");
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog("formmsg.drawFormFace"+ex.Message);
            }
        }
        int r = 8;
        /// <summary>
        /// 获取边框线
        /// </summary>
        /// <returns></returns>
        private PointF[] initbianjiao()
        {
            float mincir = (float)0.01;
            int minline = 1;
            PointF[] points;
            List<PointF> allpoints = new List<PointF>();
            initcircle(ref allpoints, (float)Math.PI / 2, (float)Math.PI, mincir, new Point(r, r), r);
            //initline(ref allpoints, new Point(r, r), new Point(0, this.Height), minline);
            initcircle(ref allpoints, (float)Math.PI, (float)Math.PI + (float)Math.PI / 2, mincir, new Point(r, this.Height - r), r);
            //initline(ref allpoints, new Point(0, this.Height), new Point(this.Width, this.Height), minline);
            initcircle(ref allpoints, (float)Math.PI + (float)Math.PI / 2, (float)Math.PI * 2, mincir, new Point(this.Width - r, this.Height - r), r);
            //initline(ref allpoints, new Point(this.Width, this.Height), new Point(this.Width, r), minline);
            initcircle(ref allpoints, 0, (float)Math.PI / 2, mincir, new Point(this.Width - r, r), r);
            points = new PointF[allpoints.Count];
            allpoints.CopyTo(points);
            return points;
        }
        private void initcircle(ref List<PointF> allpoints, float startcir, float endcir, float mincir, Point circenter, int R)
        {
            for (float f = startcir; f <= endcir; f += mincir)
            {
                PointF newpoint = new PointF();
                newpoint.X = (float)(Math.Cos(f) * R) + circenter.X;
                newpoint.Y = (float)(circenter.Y - Math.Sin(f) * R);
                if (newpoint.X < 0) newpoint.X = 0;
                else if (newpoint.X > this.Width)
                    newpoint.X = this.Width;
                if (newpoint.Y < 0)
                    newpoint.Y = 0;
                else if (newpoint.Y > this.Height)
                    newpoint.Y = this.Height;
                allpoints.Add(newpoint);
            }
        }
        private void initline(ref List<PointF> allpoints, Point startp, Point endp, int mincir)
        {
            if (startp.X == endp.X)
            {
                for (int f = startp.Y; f <= endp.Y; f += mincir)
                {
                    allpoints.Add(new PointF(startp.X, f));
                }
            }
            else
            {
                for (int f = startp.X; f <= endp.X; f += mincir)
                {
                    allpoints.Add(new PointF(f, startp.Y));
                }
            }
        }

        private void formMsg_TextChanged(object sender, EventArgs e)
        {
            //lblTiTle.Text = this.Text;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtsend_MouseDown(object sender, MouseEventArgs e)
        {
            txtsend.Focus();
        }

        private void txtMsg_MouseDown(object sender, MouseEventArgs e)
        {
            txtMsg.Focus();
        }

        private void formMsg_SizeChanged(object sender, EventArgs e)
        {
            drawFormFace();
        }
      

        private void formMsg_Activated(object sender, EventArgs e)
        {
            txtsend.Focus();
            drawFormFace();
        }
        DRAWINFO.formDrawInfo drawinfo;
        //涂鸦
        private void btndrawinfo_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    IntPtr hw = drawinfo.Handle;
                    drawinfo.Activate();
                }
                catch
                {
                    drawinfo = new DRAWINFO.formDrawInfo(Helper._guid);
                    drawinfo.sendimg += this.okdrawimg;
                    drawinfo.Left = this.Left + this.Width / 2;
                    drawinfo.Top = this.Top + this.Height / 2;
                    drawinfo.Icon = this.Icon;
                    drawinfo.Text = lblTiTle.Text;
                    drawinfo.Show();
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog("formmsg.btndrawinfo_Click"+ex.Message);
            }
        }
        private void okdrawimg(Bitmap bmp)
        {
            try
            {
                txtsend.InsertImage(bmp);
                txtsend.Focus();
            }
            catch
            { }
        }

        private void formMsg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (drawinfo != null)
            {
                drawinfo.Close();                
            }
            foreach (RichTextBox.MyPicture pic in _sendIMG)
            {
                pic.stop();
            }
            foreach (RichTextBox.MyPicture pic in _recIMG)
            {
                pic.stop();
            }
            _sendIMG.Clear();
            _recIMG.Clear();
            txtMsg.Clear();
        }
        int oldsendmsglength = 0;
        private void txtsend_TextChanged(object sender, EventArgs e)
        {
            int sendchangelength = txtsend.Text.Length - oldsendmsglength;
            foreach (RichTextBox.MyPicture tempic in _sendIMG)
            {
                try
                {
                    int p = int.Parse(tempic.Name.Substring(1));
                    if (p >= txtsend.SelectionStart-1)
                    {
                        p = p + sendchangelength;
                        tempic.Name = "p" + p.ToString();
                    }
                }
                catch
                { }
            }
            oldsendmsglength = txtsend.Text.Length;
        }

        private void formMsg_Shown(object sender, EventArgs e)
        {
            IniFace();
        }
        #region 初始化表情菜单
        private void IniFace()
        {
            int j = 0;
            DevComponents.DotNetBar.ItemContainer itemCon = null;
            for (int i = 0; i < MsgHelper.MsgFaceList.Count; i++)
            {
                DevComponents.DotNetBar.ButtonItem item = new DevComponents.DotNetBar.ButtonItem();
                item.Tag = i;
                item.Tooltip = i.ToString();
                item.Image = MsgHelper.MsgFaceList[i];
                if (i % 15 == 0)
                {
                    DevComponents.DotNetBar.ItemContainer itemC = new DevComponents.DotNetBar.ItemContainer();
                    this.btnMsgFace.SubItems.Add(itemC, j);
                    itemCon = itemC;
                    itemCon.Name = j.ToString();
                    itemCon.MinimumSize = new Size(0, 0);
                    j++;
                }
                itemCon.SubItems.Add(item, i % 15);
                item.Click += new EventHandler(item_Click);
            }
        }
        #endregion
        #region 表情菜单 单击事件
        private void item_Click(object sender, EventArgs e)//表情单击事件代码
        {
            try
            {
                DevComponents.DotNetBar.ButtonItem btnitem = sender as DevComponents.DotNetBar.ButtonItem;
                if (btnitem != null)
                {
                    string face = btnitem.Tag.ToString();
                    //Image img = ImageResources.Face.getGifByName(face);
                    insertImage(MsgHelper.MsgFaceList[int.Parse(face)]);
                }
            }
            catch { }
        }
        #endregion 

        private void bar1_MouseMove(object sender, MouseEventArgs e)
        {
            bar1.Invalidate();
        }
    }
}