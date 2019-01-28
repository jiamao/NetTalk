namespace LanTalkServer
{
    partial class formNewUser
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userid = new System.Windows.Forms.TextBox();
            this.userpwd2 = new System.Windows.Forms.TextBox();
            this.btnok = new DevComponents.DotNetBar.ButtonX();
            this.btncan = new DevComponents.DotNetBar.ButtonX();
            this.label3 = new System.Windows.Forms.Label();
            this.userpwd1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码确认：";
            // 
            // userid
            // 
            this.userid.Location = new System.Drawing.Point(81, 15);
            this.userid.MaxLength = 16;
            this.userid.Name = "userid";
            this.userid.Size = new System.Drawing.Size(120, 21);
            this.userid.TabIndex = 2;
            this.userid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.userid_KeyPress);
            // 
            // userpwd2
            // 
            this.userpwd2.Location = new System.Drawing.Point(82, 86);
            this.userpwd2.MaxLength = 32;
            this.userpwd2.Name = "userpwd2";
            this.userpwd2.PasswordChar = '*';
            this.userpwd2.Size = new System.Drawing.Size(119, 21);
            this.userpwd2.TabIndex = 3;
            // 
            // btnok
            // 
            this.btnok.ColorScheme.DockSiteBackColorGradientAngle = 0;
            this.btnok.Location = new System.Drawing.Point(17, 120);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(75, 23);
            this.btnok.TabIndex = 4;
            this.btnok.Text = "确定";
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // btncan
            // 
            this.btncan.ColorScheme.DockSiteBackColorGradientAngle = 0;
            this.btncan.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btncan.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btncan.Location = new System.Drawing.Point(121, 120);
            this.btncan.Name = "btncan";
            this.btncan.Size = new System.Drawing.Size(75, 23);
            this.btncan.TabIndex = 0;
            this.btncan.Text = "取消";
            this.btncan.Click += new System.EventHandler(this.btncan_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "用户密码：";
            // 
            // userpwd1
            // 
            this.userpwd1.Location = new System.Drawing.Point(82, 50);
            this.userpwd1.MaxLength = 32;
            this.userpwd1.Name = "userpwd1";
            this.userpwd1.PasswordChar = '*';
            this.userpwd1.Size = new System.Drawing.Size(119, 21);
            this.userpwd1.TabIndex = 3;
            // 
            // formNewUser
            // 
            this.AcceptButton = this.btnok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btncan;
            this.ClientSize = new System.Drawing.Size(213, 157);
            this.Controls.Add(this.btncan);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.userpwd1);
            this.Controls.Add(this.userpwd2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.userid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formNewUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增用户";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox userid;
        private System.Windows.Forms.TextBox userpwd2;
        private DevComponents.DotNetBar.ButtonX btnok;
        private DevComponents.DotNetBar.ButtonX btncan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox userpwd1;
    }
}