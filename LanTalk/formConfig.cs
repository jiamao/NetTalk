using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LanTalk
{
    public partial class formConfig : Form
    {
        public formConfig()
        {
            InitializeComponent();           
        }

        private void txtgroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 188)
            {
                e.Handled = true;
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            Helper.MyGroupName = txtgroupname.Text;
            Helper.MyName = txtname.Text;
            Helper.MyHeader = headerimg.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btncan_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void formConfig_Load(object sender, EventArgs e)
        {
            this.txtgroupname.DataSource = Helper.getGroupNames();
            this.txtgroupname.Text = Helper.MyGroupName;
            txtname.Text = Helper.MyName;
            headerimg.Images = MsgHelper.HeaderLargeImageList;
            foreach (string img in MsgHelper.HeaderLargeImageList.Images.Keys)
            {
                //if (img.Equals("allfriends")) continue;
                
                Image ig = MsgHelper.HeaderLargeImageList.Images[img];
                
                headerimg.Items.Add(img);
            }
            headerimg.SelectedItem = String.IsNullOrEmpty(Helper.MyHeader) ? "_100" : Helper.MyHeader;
        }

        private void txtgroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',' || e.KeyChar == '#' || e.KeyChar == '[' || e.KeyChar == ']' || e.KeyChar == '\\' || e.KeyChar=='/')
            {
                e.Handled = true;
            }
        }
    }
}