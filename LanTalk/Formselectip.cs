using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Windows.Forms;

namespace LanTalk
{
    public partial class Formselectip : Form
    {
        public Formselectip()
        {
            InitializeComponent();
        }
        IPAddress[] _ips;

        public IPAddress[] Ips
        {
            get { return _ips; }
            set { _ips = value;
            cbip.DataSource = _ips;
            }
        }
        string _selectip;

        public string Selectip
        {
            get { return _selectip; }
            set { _selectip = value; }
        }
        private void btnok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            _selectip = cbip.Text;
            this.Close();
        }
    }
}