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
                    MessageBox.Show("�û�������Ϊ��");
                    return;
                }
                else if (userid.Text.Length < 4)
                {
                    MessageBox.Show("�û���������Ҫ4λ");
                    return;
                }
                if (string.IsNullOrEmpty(userpwd1.Text))
                {
                    MessageBox.Show("���벻��Ϊ��");
                    return;
                }
                else if (!userpwd1.Text.Equals(userpwd2.Text))
                {
                    MessageBox.Show("���������ȷ�ϲ�һ��");
                    return;
                }
                if (UserHelper.netAddUser(userid.Text, userpwd1.Text))
                {
                    serverHelper.writeLog("�����û�" + userid.Text + "�ɹ���");
                    this.DialogResult = DialogResult.OK;              
                    this.Close();
                }
                else
                {
                    serverHelper.writeLog("�����û�" + userid.Text + "ʧ�ܣ�");
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