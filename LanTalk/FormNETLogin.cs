using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LanTalk
{
    public partial class FormNETLogin : Form
    {
        public FormNETLogin()
        {
            InitializeComponent();
        }

        private void FormNETLogin_Load(object sender, EventArgs e)
        {
            
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtuser.Text))
            {
                MessageBox.Show("�û�������Ϊ��");
                return;
            }
            if (string.IsNullOrEmpty(txtpwd.Text))
            {
                MessageBox.Show("���벻��Ϊ��");
                return;
            }
            Helper.Login(txtuser.Text, txtpwd.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}