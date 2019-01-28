using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LanTalkServer
{
    public partial class formNewUser : Form
    {
        public formNewUser()
        {
            InitializeComponent();
        }

        private void btncan_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(userid.Text))
                {
                    MessageBox.Show("用户名不可为空");
                    return;
                }
                else if (userid.Text.Length < 4)
                {
                    MessageBox.Show("用户名至少需要4位");
                    return;
                }
                if (string.IsNullOrEmpty(userpwd1.Text))
                {
                    MessageBox.Show("密码不可为空");
                    return;
                }
                else if (!userpwd1.Text.Equals(userpwd2.Text))
                {
                    MessageBox.Show("密码和密码确认不一至");
                    return;
                }
                if (UserHelper.netAddUser(userid.Text, userpwd1.Text))
                {
                    serverHelper.writeLog("新增用户" + userid.Text + "成功！");
                    this.DialogResult = DialogResult.OK;              
                    this.Close();
                }
                else
                {
                    serverHelper.writeLog("新增用户" + userid.Text + "失败！");
                    this.DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                serverHelper.writeLog(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void userid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar > 'z' || e.KeyChar < 'A') && (e.KeyChar > '9' || e.KeyChar < '0') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            //if (e.KeyChar == ',' || e.KeyChar == '#' || e.KeyChar == '[' || e.KeyChar == ']' || e.KeyChar == '\\' || e.KeyChar == '/')
            //{
            //    e.Handled = true;
            //}
        }
    }
}