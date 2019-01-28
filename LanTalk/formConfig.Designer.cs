namespace LanTalk
{
    partial class formConfig
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
            this.txtname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.headerimg = new UtilityLibrary.WinControls.ImageComboBox();
            this.btnok = new DevComponents.DotNetBar.ButtonX();
            this.btncan = new DevComponents.DotNetBar.ButtonX();
            this.txtgroupname = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtname
            // 
            this.txtname.Location = new System.Drawing.Point(72, 52);
            this.txtname.MaxLength = 20;
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(154, 21);
            this.txtname.TabIndex = 1;
            this.txtname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtgroup_KeyPress);
            this.txtname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtgroup_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "所在组：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "头像：";
            // 
            // headerimg
            // 
            this.headerimg.BitmapNames = null;
            this.headerimg.Bitmaps = null;
            this.headerimg.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.headerimg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.headerimg.FormattingEnabled = true;
            this.headerimg.Images = null;
            this.headerimg.Location = new System.Drawing.Point(72, 93);
            this.headerimg.Name = "headerimg";
            this.headerimg.Size = new System.Drawing.Size(154, 22);
            this.headerimg.TabIndex = 7;
            this.headerimg.ToolBarUse = false;
            // 
            // btnok
            // 
            this.btnok.ColorScheme.DockSiteBackColorGradientAngle = 0;
            this.btnok.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnok.Location = new System.Drawing.Point(39, 136);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(75, 23);
            this.btnok.TabIndex = 8;
            this.btnok.Text = "确定";
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // btncan
            // 
            this.btncan.ColorScheme.DockSiteBackColorGradientAngle = 0;
            this.btncan.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btncan.Location = new System.Drawing.Point(145, 136);
            this.btncan.Name = "btncan";
            this.btncan.Size = new System.Drawing.Size(75, 23);
            this.btncan.TabIndex = 0;
            this.btncan.Text = "取消";
            this.btncan.Click += new System.EventHandler(this.btncan_Click);
            // 
            // txtgroupname
            // 
            this.txtgroupname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtgroupname.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.txtgroupname.FormattingEnabled = true;
            this.txtgroupname.Location = new System.Drawing.Point(72, 12);
            this.txtgroupname.Name = "txtgroupname";
            this.txtgroupname.Size = new System.Drawing.Size(154, 20);
            this.txtgroupname.TabIndex = 9;
            // 
            // formConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(259, 177);
            this.Controls.Add(this.txtgroupname);
            this.Controls.Add(this.btncan);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.headerimg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtname);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formConfig";
            this.Opacity = 0.95;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "个人设置";
            this.Load += new System.EventHandler(this.formConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private UtilityLibrary.WinControls.ImageComboBox headerimg;
        private DevComponents.DotNetBar.ButtonX btnok;
        private DevComponents.DotNetBar.ButtonX btncan;
        private System.Windows.Forms.ComboBox txtgroupname;
    }
}