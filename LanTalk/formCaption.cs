using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LanTalk
{
    public partial class formCaption : Form
    {
        public formCaption()
        {
            InitializeComponent();
        }

        private void formCaption_Load(object sender, EventArgs e)
        {
            //基本设置
            cboldver.Checked = Helper.Oldver;
            cbthread.Text = Helper.ThreadCount.ToString();
            cbautostart.Checked = Helper.readAutoStart();
            cbbutton.Checked = Helper.SendmsgButton.Equals("enter");

            //网络
            tabControl1.SelectedTabIndex = 0;
            cbisnet.Checked = Helper.getConfigByName("ISNET","false").ToLower().Trim().Equals("true"); ;
            serveraddr.Text = Helper.getConfigByName("ServerAddr", Helper.ServerAddr);
            serverport.Text = Helper.getConfigByName("ServerPort", Helper.ServerPort.ToString());
            serverport.Enabled = serveraddr.Enabled = cbisnet.Checked;
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            Helper.setConfig("ISNET", cbisnet.Checked.ToString());            
            
                Helper.setConfig("ServerAddr", serveraddr.Text);
            
                Helper.setConfig("ServerPort", serverport.Text.Trim());
            
            Helper.Oldver = cboldver.Checked;
            Helper.ThreadCount = int.Parse(cbthread.Text);
            Helper.startATpc(cbautostart.Checked);
            Helper.setConfig("ThreadCount",cbthread.Text);
            Helper.setConfig("LanMsg", cboldver.Checked.ToString());
            if (cbbutton.Checked)
            {
                Helper.SendmsgButton = "enter";
                Helper.setConfig("SendButton", "enter");
            }
            else
            {
                Helper.SendmsgButton = "ctrlenter";
                Helper.setConfig("SendButton", "ctrlenter");
            }

            if (cbisnet.Checked != Helper.IsNet)
            {
                if (MsgHelper.netlogined || !Helper.IsNet)
                    MessageBox.Show("网络设置重启后生效！");
                else if (Helper.IsNet)
                {
                    if (MessageBox.Show("更改为内网，是否立即生效？", "网络设置", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Helper.init();
                        Helper.Login();
                        Program.formlist.changetitle(Helper.IsNet);
                    }
                }
            }

            this.Close();
        }

        private void btncan_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void serverport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '\b'))
            {
                e.Handled = true;
            }
           
        }

        private void cbisnet_CheckedChanged(object sender, EventArgs e)
        {
            serverport.Enabled = serveraddr.Enabled = cbisnet.Checked;
        }
    }
}