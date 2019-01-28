using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LanTalkServer
{
    public partial class FormLanTalkServer : Form
    {
        public FormLanTalkServer()
        {
            InitializeComponent();
        }
        bool _canel = false;
        private void FormLanTalkServer_Shown(object sender, EventArgs e)
        {
            serverHelper.autostart = ConfigurationManager.AppSettings["AUTOSTART"].ToString().ToLower().Equals("true");
            serverHelper.startATpc(serverHelper.autostart);
            if (serverHelper.autostart)//自动启动
            {
                开始服务SToolStripMenuItem_Click(sender, e);
            }
        }

        private void servertimer_Tick(object sender, EventArgs e)
        {
            serverHelper.ThreadCheckMSG();
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确定要退出服务?", "退出服务", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                serverHelper.exitme();
                servertimer.Enabled = false;
                _canel = true;
                this.Close();
            }
        }

        private void FormLanTalkServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_canel)
            {
                e.Cancel = true;
                this.Visible = false;
            }            
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.Visible)
            {
                this.Visible = true;
            }
            else
            {
                this.Activate();
            }
        }

        private void 新增用户NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formNewUser newuser = new formNewUser();
            newuser.Icon = this.Icon;
            if (newuser.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("新增用户成功！");
            }           
        }
        bool started = false;
        private void 开始服务SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (started)
            {
                if (MessageBox.Show("是否确定要停止服务?", "停止", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    servertimer.Enabled = false;
                    开始服务SToolStripMenuItem.Text = "开启服务(&S)";
                    started = false;
                    serverHelper.exitme();                   
                    
                }
            }
            else
            {
                try
                {
                    serverHelper.init();
                    started = true;
                    servertimer.Enabled = true;
                    开始服务SToolStripMenuItem.Text = "停止服务(&S)";
                }
                catch(Exception ex)
                {
                    serverHelper.writeLog(ex.Message);
                }
            }
        }

        private void lvnet_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 1)
            {
                serverMsgHelper.sortItemByGroupName(true);
            }
        }

        private void lvlan_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 1)
            {
                serverMsgHelper.sortItemByGroupName(false);
            }
        }
    }
}