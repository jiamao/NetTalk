using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using UtilityLibrary;
using UtilityLibrary.WinControls;
using _poolFactory = MsgPoolFactory.Factory;//消息与好友缓存

namespace LanTalk
{
    public partial class formMain : Form
    {        
        public formMain()
        {
            InitializeComponent();            
        }
        bool _cancel = true;

        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }
        private void formMain_Load(object sender, EventArgs e)
        {
            this.msgtime.Enabled = true;
            this.Icon = MsgHelper.getIconByName(Helper.MyHeader);
            this.notify.Icon = this.Icon;
            initstate(e);
        }
       
        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cancel)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
                notify.Visible = true;
                e.Cancel = _cancel;
            }
            else
            {
                msgtime.Enabled = false; 
            }
        }
        int _position = 0;//窗体的当前位置
        /// <summary>
        /// 当窗体于边上时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formMain_Move(object sender, EventArgs e)
        {
            if (_position == 4) return;
            if (this.Top < 10)
            {
                this.Top = 5 - this.Height;
                _position = 1;
            }
            else if (this.Left < 10)
            {
                this.Left = 5 - this.Width;
                _position = 2;
            }
            else if (this.Left > Screen.PrimaryScreen.Bounds.Width - this.Width + 10)
            {
                this.Left = Screen.PrimaryScreen.Bounds.Width - 5;
                _position = 3;
            }
            else
            {
                _position = 0;
            }
        }
        /// <summary>
        /// 手标进行窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formMain_MouseHover(object sender, EventArgs e)
        {
            if (_position == 0) return;
            
            if (_position == 1)
            {
                _position = 4;
                this.Top = 0;
            }
            else if (_position == 2)
            {
                _position = 4;
                this.Left = 0;
            }
            else if (_position == 3)
            {
                _position = 4;
                this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width;
            }
           
        }
        /// <summary>
        /// 当手标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formMain_MouseLeave(object sender, EventArgs e)
        {
            if (_position != 0)
            {
                _position = 0;
                formMain_Move(sender,e);
            }
        }

        int iconForm = 0;
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void msgtime_Tick(object sender, EventArgs e)
        {
            Helper.checkMSG();              
            flashFriend();
            allFriends.Refresh();
        }
        private void flashFriend()
        {
            try
            {
                bool haveMsg = false;
                foreach (Friend.Friend f in Helper.Friends)
                {
                    OutlookBarItem obi = MsgHelper.getItemByIP(f.ID);
                    if (f.Messages.Count > 0)
                    {
                        haveMsg = true;
                        IntPtr form = MsgHelper.FindWindow(f.ID);
                        if (form != IntPtr.Zero)
                        {
                            formMsg formmsg = (formMsg)Form.FromHandle(form);
                            formmsg.readMsg();//如果窗体打开了，则重读消息
                            MsgHelper.FlashWindow(form, true);
                        }
                        else
                        {
                            int imgindex = obi.ImageIndex;
                            int imgat = MsgHelper.getImageIndex(f.Header);
                            imgindex = imgindex == imgat ? (imgat + 1) : imgat;
                            obi.ImageIndex = imgindex;
                            notify.Icon = MsgHelper.getIconByName(MsgHelper.HeaderLargeImageList.Images.Keys[imgindex]);
                            
                        }
                    }
                    else
                    {
                        if(obi != null)
                        obi.ImageIndex = MsgHelper.getImageIndex(f.Header);
                    }
                }                       
                if(!haveMsg)notify.Icon = MsgHelper.getIconByName(Helper.MyHeader);
            }
            catch
            { }
        }
        /// <summary>
        /// 单击好友
        /// </summary>
        /// <param name="band"></param>
        /// <param name="item"></param>
        private void allFriends_ItemClicked(UtilityLibrary.WinControls.OutlookBarBand band, UtilityLibrary.WinControls.OutlookBarItem item)
        {
            IntPtr form = MsgHelper.FindWindow(item.Tag.ToString());
            if (form == IntPtr.Zero)
            {
                formMsg formmsg = new formMsg();
                formmsg.FriendIP = item.Tag.ToString();
                formmsg.Show();                
            }
            else
            {
                Form f = (Form)Form.FromHandle(form);
                f.Activate();                
                MsgHelper.FlashWindow(form, true);
            }
        }

        private void notify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bool havemsg = false;
            foreach (Friend.Friend f in Helper.Friends)
            {
                if (f.Messages.Count > 0)
                {
                    havemsg = true;
                    formMsg formmsg = new formMsg();
                    formmsg.FriendIP = Helper.getIP(f.Ip);
                    formmsg.Show();
                    formmsg.Activate();
                    MsgHelper.FlashWindow(formmsg.Handle, true);
                    break;
                }                
            }
            if (!havemsg)
            {
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                this.Show();
                MsgHelper.findWinAndFlash(this.Text);
            }
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _cancel = false;
            this.Close();
            Application.Exit();
        }

        private void 发送给指定IPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strip=InputBox.InputTextBox("输入要聊天的IP", "IP:", Helper.getIP(Helper.selfIP));
            if (!string.IsNullOrEmpty(strip) && !Helper.isStringIP(strip))
            {
                MessageBox.Show("ＩＰ地址不正确;"); return;
            }
            if (!string.IsNullOrEmpty(strip))
            {
                formMsg formmsg = new formMsg();
                formmsg.FriendIP = strip;
                formmsg.Show();
            }

        }

        private void 退出EToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            退出EToolStripMenuItem_Click(sender,e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
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
                    Helper.writeObjectToFile(myself, Application.StartupPath + "\\myself.dat");
                    this.Icon = MsgHelper.getIconByName(myself.Header);
                    this.notify.Icon = this.Icon;
                    Helper.initmyself();
                }
            }
            catch
            { }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _cancel = false;
            msgtime.Enabled = false;
            Helper.Face = "lanmsg";
            MsgHelper.GroupList.Clear();
            
            this.Close();
            Program.formmain.Dispose();
            
            Helper.initForm();
        }

        private void 个人设置CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(sender, e);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            退出EToolStripMenuItem1_Click(sender, e);
        }
        /// <summary>
        /// 选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            formCaption form = new formCaption();
            form.ShowDialog();
        }
        /// <summary>
        /// LanMsg样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            toolStripMenuItem2_Click(sender, e);
        }
        /// <summary>
        /// 初始化状态
        /// </summary>
        private void initstate(EventArgs e)
        {
            switch (Helper.Mystate)
            {
                case Friend.Friend.EState.BSYou:
                    {
                        BToolStripMenuItem_Click(不想理NToolStripMenuItem,e);
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
                foreach (ToolStripMenuItem item in toolStripMenuItem4.DropDownItems)
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

        private void lanMsgLToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void formMain_Shown(object sender, EventArgs e)
        {
            Helper.Login();
        }
    }
}