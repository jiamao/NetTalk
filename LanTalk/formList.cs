using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using _poolFactory = MsgPoolFactory.Factory;//消息与好友缓存

namespace LanTalk
{
    public partial class formList : Form
    {
        public formList()
        {
            InitializeComponent();
        }
        bool _cancal = true;
        DateTime logindt;
        bool logined = true;
        int loginerrm =60;//登录超时的秒钟
        GolbalHook.HookBase keyhook = new GolbalHook.HookBase(Helper._guid);
        public bool Cancal
        {
            get { return _cancal; }
            set { _cancal = value; }
        }
        string _curicon = Helper.MyHeader;
        bool _changeicon = false;
        private void msgtimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!logined && logindt < DateTime.Now)
                {
                    txtMsg.AppendText("登录超时!\r\n\r\n");
                    loginmenu.Enabled = true;
                    logined = true;
                }
                Helper.checkMSG();
                if ((_poolFactory.Msg.Count > 0 && !this.Visible) || _changeicon)
                {
                    _changeicon = true;
                    if (_curicon.Equals(Helper.MyHeader))
                    {
                        _curicon = "_1";
                    }
                    else
                    {
                        _curicon = Helper.MyHeader;
                    }
                    Thread.Sleep(300);
                    notify.Icon = MsgHelper.getIconByName(_curicon);
                }
                else if (!_curicon.Equals(Helper.MyHeader))
                {
                    notify.Icon = MsgHelper.getIconByName(Helper.MyHeader);
                    _curicon = Helper.MyHeader;
                }
                if (_poolFactory.Msg.Count > 0)
                {
                    readMsg();
                    GC.Collect();
                }
                //如果长久接收不到服务器消息
                if (MsgHelper.netlogined && (Helper.LastConnectServerTime < DateTime.Now.AddSeconds(Helper.ConnectServerTimeOut)))
                {
                    MsgHelper.netlogined = false;
                    _poolFactory.removeAllFriends();
                    listfriend.Nodes[0].Nodes.Clear();//清理所有好友
                    txtMsg.AppendText("连接服务器超时,请重新登陆!\r\n\r\n");
                    loginmenu.Enabled = true;
                    logined = true;
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog("formlist.msgtimer_Tick " + ex.ToString() + ex.StackTrace);
            }            
        }
        private void readmsgFromKey(int keycode, int modifierkeys)
        {
            if (!this.Actived && keycode == (int)Keys.Z && modifierkeys == (int)Keys.Control + (int)Keys.Alt)
            {
                if (!this.Visible) this.Visible = true;
                this.Focus();
                this.Activate();
            }
        }
        private void formList_Load(object sender, EventArgs e)
        {
            msgtimer.Enabled = true;
            this.Text = Helper.MyName + "[" + Helper.getIP(Helper.selfIP) + "]";
            this.Icon = MsgHelper.getIconByName(Helper.MyHeader);
            thisicon.BackgroundImage = MsgHelper.getImageByName(Helper.MyHeader);
            notify.Icon = this.Icon;
            listfriend.ImageList = MsgHelper.HeaderSmallImageList;
            lanMsgLToolStripMenuItem.Checked = true;
            txtMsg.ForeColor = Color.DarkGreen;
            listfriend.Nodes[0].SelectedImageKey=listfriend.Nodes[0].ImageKey = "_105";
            //Helper.initmyself();
            txtsend.DragDrop += txtsend_dragdrop;
            txtsend.DragEnter += txtsend_dragenter;
            //if (Helper.getConfigByName("FACE","false").ToLower().Trim().Equals("true"))
            //{
            //    界面SToolStripMenuItem.Visible = true;
            //    toolStripMenuItem5.Visible = true;
            //}
            //else
            //{
            界面SToolStripMenuItem.Visible = false;
            toolStripMenuItem5.Visible = false;
            //}
            if (!Helper.IsNet)
            {
                initstate(e);
            }
            if (Helper.SendmsgButton.Equals("enter"))
            {
                toolTip1.SetToolTip(btnsend, "ENTER发送消息");
                toolTip1.SetToolTip(txtsend, "ENTER发送消息");
            }
            else
            {
                toolTip1.SetToolTip(btnsend, "CTRL+ENTER发送消息");
                toolTip1.SetToolTip(txtsend, "CTRL+ENTER发送消息");
            }
            keyhook.curkeypresscode += readmsgFromKey;//绑定全局钩子
            keyhook.SET_WINDOWS_KEYBOARD_HOOK();
        }
        
        private void listfriend_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode tn in e.Node.Nodes)
            {
                tn.Checked = e.Node.Checked;
            }
          
        }
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
                    txtMsg.AppendText("没有登录,请登录后再发送消息!\r\n");
                    return;
                }
                
                btnsend.Enabled = false;
                mainmenu.Enabled = false;
                notifymenu.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                txtsend.ReadOnly = true;
                cutscreen.Enabled = false;
                if (String.IsNullOrEmpty(txtsend.Text)) return;
                List<string> allip = new List<string>();
                foreach (TreeNode tn in listfriend.Nodes[0].Nodes)
                {
                    foreach (TreeNode ftn in tn.Nodes)
                    {
                        if (ftn.Checked)
                        {
                            allip.Add(ftn.Name);
                        }
                    }
                }
                if (allip.Count <= 0)
                {
                    txtMsg.AppendText("请至少选择一个好友！\r\n");
                    return;
                }
                string ballip = Helper.turnToByteIP(allip);
                string msgpeople = "你对 " + Helper.getAllName(ballip) + " 说：(" + DateTime.Now.ToShortTimeString() + ")\r\n";
                txtMsg.AppendText(msgpeople);                
                StringBuilder sendrtf = new StringBuilder(txtsend.Rtf);
                int oldp = txtMsg.Text.Length;
                txtMsg.SelectedRtf = txtsend.Rtf;
                RichTextBox.REOBJECT reObject = new RichTextBox.REOBJECT();
                for (int i = 0; i < this.txtsend.GetRichEditOleInterface().GetObjectCount(); i++)
                {
                    this.txtsend.GetRichEditOleInterface().GetObject(i, reObject, RichTextBox.GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
                    tag = reObject.dwUser;
                    RichTextBox.MyPicture mypic = _sendIMG.Find(findPic);
                    if (mypic != null)
                    {
                        int p = int.Parse(mypic.Name.Substring(1));
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
                //if (Helper.IsNet && sendrtf.Length > 10000)
                //{
                //    MessageBox.Show("外网用户不允许发送过大的消息!");
                //    txtsend.Clear();
                //    return;
                //}
                
                
                foreach (RichTextBox.MyPicture mypic in _sendIMG)
                {
                    mypic.stop();
                }
                _sendIMG.Clear();
                txtsend.Clear();                
                string[] allips = new string[allip.Count];
                allip.CopyTo(allips);
                MsgHelper.sendMsg(allips, sendrtf.ToString(), ballip);
                allip.Clear();
                txtMsg.AppendText(Environment.NewLine);
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex.Message);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                txtMsg.ScrollToCaret();
                txtsend.Focus();
                btnsend.Enabled = true;
                mainmenu.Enabled = true;
                notifymenu.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                txtsend.ReadOnly = false;
                cutscreen.Enabled = true;
            }
        }
        uint tag = 0;
        private bool findPic(RichTextBox.MyPicture pic)
        {
            return pic.Tag.ToString().Equals(tag.ToString());
        }
        List<string> sendips;
        string bips;
        string sendrtfs;
        private void ThSendMsg()
        {
            List<string> ips = sendips;
            string sendbips = bips;
            string sendinfo = sendrtfs;
            sendrtfs = "";

            string[] allips = new string[ips.Count];
            ips.CopyTo(allips);
            MsgHelper.sendMsg(allips, sendinfo, sendbips);
            ips.Clear();
        }
        /// <summary>
        /// 重读消息
        /// </summary>
        public void readMsg()
        {
            try
            {
                for (int i = 0; i < _poolFactory.Msg.Count;i++ )
                {
                    MsgInfo.MsgInfo info = _poolFactory.Msg[i];
                    if (info.Ver.Equals("readed")) continue;
                    info.Ver = "readed";//标记消息为已读
                    
                    if ((!logined || MsgHelper.netlogined) && info.ProtocolType == MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_ERR)//登录错误
                    {
                        logined = true;
                        MsgHelper.netlogined = false;
                        Thread.Sleep(100);
                        _poolFactory.removeAllFriends();
                        listfriend.Nodes[0].Nodes.Clear();                        
                        loginmenu.Enabled = true;
                        txtMsg.AppendText(info.SendInfo + "\r\n");
                        continue;
                    }
                    else if (!logined && info.ProtocolType == MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_OK)//登录成功
                    {
                        logined = true;
                        loginmenu.Enabled = false;
                        txtMsg.AppendText("登录成功!\r\n");
                        this.Text = Helper.MyName + "[" + (Helper.IsNet ? Helper.MyNetID : Helper.getIP(Helper.selfIP)) + "]";
                        continue;
                    }
                    if (info.ProtocolType == MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_OK || info.ProtocolType == MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_ERR) continue;
                    int position;
                    Friend.Friend _curFriend = _poolFactory.findFriend(info.SendFromID);
                    if (_curFriend == null) { _curFriend = new Friend.Friend(); _curFriend.GroupName = "未知组"; _curFriend.Ip = Helper.getByteIP(info.SendFromID); _curFriend.Name = info.SendFromID; }
                    ////如果此消息只发给本人
                    //if (Helper.equalIP(info.SendToIp, Helper.selfIP))
                    //{
                    IntPtr formmsghandle = MsgHelper.FindWindow(info.SendFromID); 
                    //如果已打开么聊窗口   
                        if (formmsghandle != IntPtr.Zero) 
                        { 
                            formMsg formmsg = (formMsg)Form.FromHandle(formmsghandle); 
                            formmsg.Activate();
                            _curFriend.Messages.Add(info);
                            formmsg.readMsg();                            
                            continue;
                        }
                    //}
                    txtMsg.AppendText(_curFriend.GroupName + "->");
                    txtMsg.InsertLink(" " + _curFriend.Name + "[" + _curFriend.ID + "] ");
                    string msgpeple = " 对 [" + Helper.getAllName(info.SendToID) + "] 说：(" + DateTime.Now.ToShortTimeString() + ")\r\n";
                    txtMsg.AppendText(msgpeple);                    
                    position = txtMsg.Text.Length;
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
                                        MsgHelper.getLostedImgPack(guid, strimg, _curFriend.ID);
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
                        getinfo=receinfo.ToString();
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
                        if (Helper.Mystate != Friend.Friend.EState.InLine && _curFriend.State == Friend.Friend.EState.InLine && !info.SendToID.Contains(",") && !info.SendToID.Contains(IPAddress.Broadcast.ToString()))
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
                        float filem = (float)filelen / 1024;
                        txtMsg.AppendText("收到来自组:" + _curFriend.GroupName + "->" + _curFriend.Name + " 的文件(" + filem.ToString("0.00") + "KB)：");

                        string guid = Guid.NewGuid().ToString("n");
                        txtMsg.InsertLink(Path.GetFileName(info.SendInfo.Split('|')[0]), guid);
                        info.InfoGuid = guid;
                        _poolFactory.Filemsg.Add(info);
                    }
                    txtMsg.AppendText(Environment.NewLine);     
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog("formlist.readmsg "+ex.Message);
            }
            finally
            {
                _poolFactory.removeReadedMsg();
                txtMsg.ScrollToCaret();
                _changeicon = _poolFactory.Msg.Count > 0 || !this.Visible;
            }
        }
        #region 插入图片
        List<RichTextBox.MyPicture> _sendIMG = new List<RichTextBox.MyPicture>();

        private void button1_Click(object sender, EventArgs e)
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
                    FileInfo fileinfo=new FileInfo(strimgfile);
                    if (Helper.IsNet && fileinfo.Length > 80 * 1024)
                    {
                        txtMsg.AppendText("外网用户暂不支持发送大于80KB的图片!\r\n\r\n");
                        return;
                    }
                    Image img=Image.FromFile(strimgfile);
                    insertImage(img);                    
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
        int piccount = 0;
        private void insertImage(Image img)
        {
            try
            {
                if (img == null) return;
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
                img.Tag = piccount;
                pic.initimage = img;
                pic.start();
                txtsend.InsertMyControl(pic);
                _sendIMG.Add(pic);
                piccount++;
                txtsend.ScrollToCaret();
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex.Message);
            }
        }
        object _curtag;
        private bool findsendpic(RichTextBox.MyPicture pic)
        {
            return pic.Tag.Equals(_curtag);
        }
        #endregion
        //bool msginviate = false;
        private void insertMsgPicture(RichTextBox.MyPicture mypic)
        {
            //if (!msginviate)
            //System.Drawing.ImageAnimator.Animate(mypic.Image, new System.EventHandler(this.OnFrameMSGChanged));
            txtMsg.InsertMyControl(mypic);
            txtMsg.AppendText(" "); 
            
            mypic.start();
        }
        List<RichTextBox.MyPicture> msgpics = new List<RichTextBox.MyPicture>();
        private void insertMsgPicture(Image img,int p)
        {
            RichTextBox.MyPicture pic = new RichTextBox.MyPicture();            
            pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pic.BackColor = this.txtMsg.BackColor;
           
            //string guid = Guid.NewGuid().ToString("n");
            //img.Tag = guid;
            pic.initimage = img;
            pic.start();
            txtMsg.SelectionStart = p;
            txtMsg.InsertMyControl(pic);
            //txtMsg.AppendText(" ");
            
            //if (System.Drawing.ImageAnimator.CanAnimate(img))
            //System.Drawing.ImageAnimator.Animate(pic.Image, new System.EventHandler(this.OnFrameMSGChanged));
           
            //pic.Tag = guid;
            
            msgpics.Add(pic);
        }
        #region 重新绘制 richbox 事件
        private void RTBRecordOnFrameChanged(object sender, EventArgs e)
        {
            //this.txtsend.Invalidate();
            //this.RTBSend.Focus();
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            Image img = (sender as Image);
            if (img == null) return;
            _curtag = img.Tag;
            RichTextBox.MyPicture mypic = _sendIMG.Find(findsendpic);
            if (mypic == null) return;
            mypic.Invalidate();  
        }
        private void OnFrameMSGChanged(object sender, EventArgs e)
        {
            try
            {
                Image img =(sender as Image);
                if(img==null)return;
                RichTextBox.MyPicture mypic = findmsgpic(img.Tag);
                if (mypic == null) return;
                mypic.Invalidate();                
            }
            catch
            { }
        }
        private RichTextBox.MyPicture findmsgpic(object guid)
        {
            _curobj = guid;
            return msgpics.Find(findmsgpic);
            
        }
        object _curobj;
        private bool findmsgpic(RichTextBox.MyPicture pic)
        {
            return pic.Tag.Equals(_curobj);
        }
        #endregion
        private void button2_Click(object sender, EventArgs e)
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

        private void txtsend_KeyDown(object sender, KeyEventArgs e)
        {
            if (Helper.SendmsgButton.Equals("ctrlenter") && e.Modifiers == Keys.Control && e.KeyCode == Keys.Enter)
            {
                btnsend_Click(sender,e);
                e.Handled = true;
            }
            else if (Helper.SendmsgButton.Equals("enter") && e.KeyCode == Keys.Enter && e.Modifiers != Keys.Control)
            {
                btnsend_Click(sender, e);
                e.Handled = true;
            }
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _cancal = false;
            this.Close();
            Application.Exit();
        }

        private void formList_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_cancal)
                {
                    this.Visible = false;
                    e.Cancel = true;
                }
                else
                {
                    keyhook.UNLOAD_WINDOWS_KETBOARD_HOOK();//释放钩子
                    if (!string.IsNullOrEmpty(txtMsg.Text))
                    {
                        toolStripMenuItem12_Click(sender,null);//保存聊天记录  
                    }
                    foreach (RichTextBox.MyPicture pic in _sendIMG)
                    {
                        pic.stop();
                    }
                    _sendIMG.Clear();
                }
            }
            catch (Exception ex)
            {
                Helper.writeLog("formlist.formclosing"+ex.Message);
            }
        }

        private void notify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.Activate();            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string strip = InputBox.InputTextBox("输入要聊天的IP", "IP:", Helper.getIP(Helper.selfIP));
            if (!string.IsNullOrEmpty(strip) && !Helper.isStringIP(strip))
            {
                MessageBox.Show("ＩＰ地址不正确;"); return;
            }
            if (!string.IsNullOrEmpty(strip))
            {
                IntPtr formmsghandle = MsgHelper.FindWindow(strip);
                formMsg formmsg;
                if (formmsghandle != IntPtr.Zero) { formmsg = (formMsg)Form.FromHandle(formmsghandle); formmsg.Activate(); return; }
                formmsg = new formMsg();
                formmsg.FriendIP = strip;
                formmsg.Show();
            }
        }
        /// <summary>
        /// 个人设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                formConfig config = new formConfig();
                if (config.ShowDialog() == DialogResult.OK)
                {
                    Friend.Friend myself = new Friend.Friend();
                    myself.GroupName = Helper.MyGroupName;
                    myself.Header = Helper.MyHeader;
                    myself.Name = Helper.MyName;
                    Helper.writeObjectToFile(myself, Path.Combine(Application.StartupPath,"myself.dat"));
                    //Helper.initmyself();
                    this.Text = myself.Name + "[" + (Helper.IsNet?Helper.MyNetID:Helper.getIP(Helper.selfIP)) + "]";
                    this.Icon =MsgHelper.getIconByName(myself.Header);
                    thisicon.BackgroundImage = MsgHelper.getImageByName(Helper.MyHeader);
                    notify.Icon = this.Icon;
                    Helper.initmyself();
                }
            }
            catch
            { }
        }

        private void 退出EToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            退出EToolStripMenuItem_Click(sender, e);
        }

        private void 个人设置CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem2_Click(sender, e);
        }

        private void qQ样式QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            msgtimer.Enabled = false;
            _cancal = false;
            Helper.Face = "qq";
            this.Close();
            Program.formlist.Dispose();
            
            Helper.initForm();            
        }

        private void listfriend_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level >= 2 && e.Button==MouseButtons.Right)
            {
                return;
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            qQ样式QToolStripMenuItem_Click(sender, e);
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 文件FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.IsNet)
            {
                txtMsg.AppendText("外网用户暂不支持发送文件!\r\n\r\n");
                return;
            }
            List<string> allip = new List<string>();
            foreach (TreeNode tn in listfriend.Nodes[0].Nodes)
            {
                foreach (TreeNode ftn in tn.Nodes)
                {
                    if (ftn.Checked)
                    {
                        allip.Add(ftn.Name);
                    }
                }
            }                
            if (allip.Count <= 0)
            {
                txtMsg.AppendText("请至少选择一个好友！\r\n\r\n");
                return;
            }
            string ballip = Helper.turnToByteIP(allip);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.InitialDirectory = Environment.CurrentDirectory;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] allips = new string[allip.Count];
                allip.CopyTo(allips);
                foreach (string filename in ofd.FileNames)
                {
                    txtMsg.AppendText("发送文件(" + filename + ")给:" + Helper.getAllName(ballip) + "!\r\n");
                    MsgHelper.sendFileMsg(filename, allips, ballip);
                }
            }
            txtMsg.AppendText(Environment.NewLine);
        }
        
        delegate DialogResult showmodal();
        /// <summary>
        /// 单击link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMsg_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                if (e.LinkText.Contains("#") && !e.LinkText.Contains("\\") && !e.LinkText.ToLower().StartsWith("http://") && !e.LinkText.ToLower().StartsWith("\\\\"))
                {
                    if (Helper.DownLoading) {txtMsg.AppendText("暂不支持多个文件同时下载!\r\n"); return;}
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
                        string ports = Helper.listener.startreceivefile(Helper.getIP(Helper.selfIP),Helper.ThreadCount);
                        _poolFactory.Lockedfilebyte = 2;
                        string[] allport=ports.Split(',');
                        string[] tempfiles = new string[allport.Length];
                        for (int i = 0; i < allport.Length;i++ )
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
                    string friendip = e.LinkText.Substring(e.LinkText.IndexOf('[')+1);
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
                Helper.writeLog("formlist.txtMsg_LinkClicked"+ex.Message);
            }           
        }
        #region 拖放文件
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
                                    List<string> allip = new List<string>();
                                    foreach (TreeNode tn in listfriend.Nodes[0].Nodes)
                                    {
                                        foreach (TreeNode ftn in tn.Nodes)
                                        {
                                            if (ftn.Checked)
                                            {
                                                allip.Add(ftn.Name);
                                            }
                                        }
                                    }
                                    if (allip.Count <= 0)
                                    {
                                        MessageBox.Show("请至少选择一个好友！");
                                        return;
                                    }
                                    string ballip = Helper.turnToByteIP(allip);
                                    string[] allips=new string[allip.Count];
                                    allip.CopyTo(allips);
                                    MsgHelper.sendFileMsg(file, allips, ballip);
                                    
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
            catch(Exception ex)
            {
                Helper.writeLog("formlist.txtsend_dragdrop" + ex.ToString());
            }
            finally
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion
        private void 刷新RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listfriend.Nodes[0].Nodes.Clear();
            _poolFactory.removeAllFriends();
            if (Helper.IsNet && !MsgHelper.netlogined)
            {
                txtMsg.AppendText("没有登录,请登录后再刷新!\r\n\r\n");
                return;
            }
            Helper.getListFriends();            
        }
        bool actived = true;

        public bool Actived
        {
            get { return actived; }
            set { actived = value; }
        }
        private void formList_Activated(object sender, EventArgs e)
        {
            try
            {
                txtsend.Focus();
                actived = true;
                drawFormFace();
            }
            catch
            { }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formabout about = new formabout();
            about.Icon = this.Icon;
            about.ShowDialog();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            toolStripMenuItem6.Enabled = false;
            toolStripMenuItem7.Enabled = true;
            listfriend.ImageList = MsgHelper.HeaderLargeImageList;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            toolStripMenuItem7.Enabled = false;
            toolStripMenuItem6.Enabled = true;
            listfriend.ImageList = MsgHelper.HeaderSmallImageList;
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
            try
            {
                if (txtsend.Focused)
                {
                    if (Clipboard.ContainsImage())
                    {
                        insertImage(Clipboard.GetImage());
                    }
                    txtsend.Paste();
                }
            }
            catch
            { }
        }

        private void txtMsg_Enter(object sender, EventArgs e)
        {
            剪切CToolStripMenuItem.Enabled = false;
            粘贴PToolStripMenuItem.Enabled = false;
            清空消息SToolStripMenuItem.Visible = true;
            this.Cursor = Cursors.Default;
        }

        private void txtsend_Enter(object sender, EventArgs e)
        {
            剪切CToolStripMenuItem.Enabled = true;
            粘贴PToolStripMenuItem.Enabled = true;
            清空消息SToolStripMenuItem.Visible = false;
            this.Cursor = Cursors.Default;
        }

        private void txtsend_MouseDown(object sender, MouseEventArgs e)
        {
            txtsend.Focus();
        }
        //截屏
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
                Helper.writeLog("formlist.cutscreen_Click"+ex.Message);
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            bool oldver = Helper.Oldver;
            formCaption form = new formCaption();
            form.ShowDialog();
            if (!Helper.Oldver.Equals(oldver))
            {
                刷新RToolStripMenuItem_Click(sender, e);
            }
            if (Helper.SendmsgButton.Equals("enter"))
            {
                toolTip1.SetToolTip(btnsend, "ENTER发送消息");
                toolTip1.SetToolTip(txtsend, "ENTER发送消息");
            }
            else
            {
                toolTip1.SetToolTip(btnsend, "CTRL+ENTER发送消息");
                toolTip1.SetToolTip(txtsend, "CTRL+ENTER发送消息");
            }
        }

        private void listfriend_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                listfriend.SelectedNode = e.Node;
                if (e.Node.Level == 2)
                {
                    toolStripMenuItem13.Visible = true;
                }
                else
                {
                    toolStripMenuItem13.Visible = false;
                }
            }
            
        }
        private void initstate(EventArgs e)
        {
            switch (Helper.Mystate)
            {
                case Friend.Friend.EState.BSYou:
                    {
                        BToolStripMenuItem_Click(不想理NToolStripMenuItem, e);
                        break;
                    }
                case Friend.Friend.EState.Busy:
                    {
                        BToolStripMenuItem_Click(忙碌BToolStripMenuItem, e);
                        break;
                    }
                case Friend.Friend.EState.InLine:
                    {
                        BToolStripMenuItem_Click(在线LToolStripMenuItem, e);
                        break;
                    }
                case Friend.Friend.EState.Out:
                    {
                        BToolStripMenuItem_Click(外出OToolStripMenuItem, e);
                        break;
                    }
                case Friend.Friend.EState.OutLine:
                    {
                        BToolStripMenuItem_Click(隐身GToolStripMenuItem, e);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        /// <summary>
        /// 状态更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int tag = 0;
                foreach (ToolStripMenuItem item in toolStripMenuItem3.DropDownItems)
                {
                    item.Checked = false;
                    item.Tag = tag;
                    tag++;
                }
                System.Windows.Forms.ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
                menuitem.Checked = true;
                Helper.changeState(int.Parse(menuitem.Tag.ToString()));
                
            }
            catch
            { }
        }

        private void 帮助PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath+"\\说明.txt");
            }
            catch
            { }
        }

        private void notify_BalloonTipClicked(object sender, EventArgs e)
        {
            this.notify_MouseDoubleClick(sender, null);
        }

        private void formList_Deactivate(object sender, EventArgs e)
        {
            actived = false;
        }

        int r = 8;
        /// <summary>
        /// 获取边框线
        /// </summary>
        /// <returns></returns>
        private PointF[] initbianjiao()
        {
            float mincir = (float)0.001;
            int minline = 1;
            PointF[] points;
            List<PointF> allpoints = new List<PointF>();
            initcircle(ref allpoints, (float)Math.PI / 2, (float)Math.PI, mincir, new Point(r, r), r);
           // initline(ref allpoints, new Point(r, r),new Point(0,this.Height-r),minline);
            initcircle(ref allpoints, (float)Math.PI, (float)Math.PI + (float)Math.PI / 2, mincir, new Point(r, this.Height - r), r);
            //initline(ref allpoints, new Point(r, this.Height), new Point(this.Width-r, this.Height), minline);
            initcircle(ref allpoints, (float)Math.PI + (float)Math.PI / 2, (float)Math.PI * 2, mincir, new Point(this.Width - r, this.Height - r), r);
            //initline(ref allpoints, new Point(this.Width, this.Height-r), new Point(this.Width, r), minline);
            initcircle(ref allpoints, 0, (float)Math.PI / 2, mincir, new Point(this.Width - r, r), r);
            //initline(ref allpoints, new Point(this.Width-r, 0), new Point(r, 0), minline);
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

        bool moveclick = false;
        bool resizeclick = false;
        Point mouseclickpoint = new Point();
        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < this.Width - 5 && e.Y < this.Height - 5)
            {
                moveclick = true;
            }
            else
            {
                resizeclick = true;
            }
            mouseclickpoint = Control.MousePosition;            
        }

        private void control_MouseUp(object sender, MouseEventArgs e)
        {
            if (moveclick)
            {
                moveclick = false;                
            }
            else if (resizeclick)
            {
                resizeclick = false;
            }
        }

        private void control_MouseLeave(object sender, EventArgs e)
        {
            moveclick = false;
        }

        private void control_MouseMove(object sender, MouseEventArgs e)
        {
            if (moveclick)
            {
                try
                {
                    Point newpoint = Control.MousePosition;
                    int offsetx = newpoint.X - mouseclickpoint.X;
                    int offsety = newpoint.Y - mouseclickpoint.Y;
                    if (Math.Abs(offsetx) > 2 || Math.Abs(offsety) > 2)
                    {
                        Helper.MoveOrResizeForm(this.Handle, this.Left + offsetx, this.Top + offsety, this.Width, this.Height);
                        mouseclickpoint = newpoint;
                    }
                }
                catch
                { }
            }
            else if (resizeclick)
            {
                try
                {
                    Point newpoint = Control.MousePosition;
                    int offsetx = newpoint.X - mouseclickpoint.X;
                    int offsety = newpoint.Y - mouseclickpoint.Y;
                    if (Math.Abs(offsetx) > 5 || Math.Abs(offsety) > 5)
                    {
                        Helper.MoveOrResizeForm(this.Handle, this.Left, this.Top , this.Width + offsetx, this.Height+ offsety);
                        mouseclickpoint = newpoint;
                    }
                }
                catch
                { }
            }
            
            if (e.X > this.Width - 2)
            {
                this.Cursor = Cursors.SizeWE;
            }
            else if (e.Y > this.Height - 2)
            {
                this.Cursor = Cursors.SizeNS;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void panel2_MouseDoubleClick(object sender, MouseEventArgs e)
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
        Graphics g = null;
        //Pen p = null;
        /// <summary>
        /// 画界面
        /// </summary>
        private void drawFormFace()
        {
            GraphicsPath gp=null;
            try
            {
                this.Refresh();
                g = this.CreateGraphics();
                gp = new GraphicsPath();
                gp.AddClosedCurve(initbianjiao(), 1);
               // p = new Pen(Color.Red);
                this.Region = new Region(gp);
                //g.DrawPath(p, gp);
                //g.DrawLine(p, new Point(0, r), new Point(0, this.Height - r));
                //g.DrawLine(p, new Point(r, this.Height - 1), new Point(this.Width - r, this.Height - 1));
                //g.DrawLine(p, new Point(this.Width - 1, this.Height - r), new Point(this.Width - 1, r));
                gp.Dispose();
                //p.Dispose();
            }
            catch (Exception ex)
            {
                Helper.writeLog("formlist.drawFormFace" + ex.Message);
            }
            
        }
        public void changetitle(bool isnet)
        {
            if (isnet)
            {
                lblTiTle.Text = this.Text + "  NetTalk-外网版" + Helper.CurVer;
            }
            else
            {
                lblTiTle.Text = this.Text + "  NetTalk-内网版" + Helper.CurVer; ;
            }
        }
        private void formList_TextChanged(object sender, EventArgs e)
        {
            changetitle(Helper.IsNet);
        }
        VideoCapture.formCapture formcap;
        /// <summary>
        /// 打开视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            if (formcap == null)
            {
                formcap = new VideoCapture.formCapture();
                formcap.FormClosed += formcap_closed;
                formcap.Show();                
            }
            else
            {
                formcap.Close();                            
            }
        }
        private void formcap_closed(object sender, FormClosedEventArgs e)
        {
            formcap = null;            
        }

        private void formList_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                drawFormFace();
            }
            catch
            { }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            cutscreen_Click(sender, e);
        }

        private void formList_Shown(object sender, EventArgs e)
        {
            IniFace();//初始化表情
            if (Helper.IsNet)
            {
                loginmenu.Visible = true;                
            }
            else
            {
                loginmenu.Visible = false;
                Helper.Login();
            }
        }
        #region 初始化表情菜单
        private void IniFace()
        {
            int j = 0;
            DevComponents.DotNetBar.ItemContainer itemCon = null;
            for (int i = 0; i < MsgHelper.MsgFaceList.Count; i++)
            {
                DevComponents.DotNetBar.ButtonItem item = new DevComponents.DotNetBar.ButtonItem();
                //buttonitem item = new buttonitem();
                item.Tag = i;
                item.Tooltip = i.ToString();
                item.Image = MsgHelper.MsgFaceList[i];
                //item.InitImage = MsgHelper.MsgFaceList[i];
                //item.start();
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
            catch{}
        }
        #endregion 
        private void listfriend_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
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
                Helper.writeLog("formlist.btndrawinfo " + ex.Message);
            }
        }
        private void okdrawimg(Bitmap bmp)
        {
            try
            {
                insertImage(bmp);
                txtsend.Focus();
            }
            catch
            { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IntPtr windc = CaptureScreen.GDIAPI.CreateDC("DISPLAY", null, null, IntPtr.Zero);
            Graphics gwin = Graphics.FromHdc(windc);

            gwin.DrawString("你\r\n是\r\n一\r\n只\r\n猪", new Font("宋体", 30), Brushes.Red, new Point(0, 0));
        }

        private void 重启RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认重启机器？", "重启", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _cancal = false;
                Helper.resetComputer();
                this.Close();
            }
        }

        private void 闭机CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认关闭机器？", "关机", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _cancal = false;
                Helper.closeComputer();
                this.Close();
            }
        }
        private void formList_VisibleChanged(object sender, EventArgs e)
        {
            _changeicon = !this.Visible && _poolFactory.Msg.Count > 0;
        }
        /// <summary>
        /// 清空消息并保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMsg.Text))
            {
                try
                {
                    txtMsg.SaveFile(Helper.Historypath);//保存聊天记录  
                    for (int i = 0; i < msgpics.Count; i++)
                    {
                        msgpics[i].stop();
                    }
                    Thread.Sleep(10);
                    msgpics.Clear();
                    txtMsg.Clear();                    
                    GC.Collect();
                }
                catch
                { }
            }
        }

        private void 清空消息SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem12_Click(sender, e);
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
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginmenu_Click(object sender, EventArgs e)
        {
            FormNETLogin formlogin = new FormNETLogin();            
            formlogin.Icon = this.Icon;
            formlogin.Text = this.Text;
            if (formlogin.ShowDialog() == DialogResult.OK)
            {
                logindt = DateTime.Now.AddSeconds(loginerrm);
                logined = false;
                loginmenu.Enabled = false;
                txtMsg.AppendText("登录中...(将在" + loginerrm.ToString() + "秒内无反应后超时)\r\n");
            }
        }

        private void bar1_MouseMove(object sender, MouseEventArgs e)
        {
            bar1.Invalidate();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            if (listfriend.SelectedNode.Level == 2)
            {
                if (MsgHelper.FindWindow(listfriend.SelectedNode.Name) == IntPtr.Zero)
                {
                    formMsg formmsg = new formMsg();
                    formmsg.FriendIP = listfriend.SelectedNode.Name;
                    formmsg.Show();
                }
            }
        }

        private void listfriend_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                toolStripMenuItem13.Visible = false;
            }
        }       
        
    }
}