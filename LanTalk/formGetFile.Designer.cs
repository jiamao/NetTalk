namespace LanTalk
{
    partial class formGetFile
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btncannel = new System.Windows.Forms.Button();
            this.combin = new System.Windows.Forms.Label();
            this.fileprocess = new UtilityLibrary.WinControls.ProgressBarEx();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btncannel
            // 
            this.btncannel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btncannel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btncannel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncannel.Location = new System.Drawing.Point(61, 40);
            this.btncannel.Name = "btncannel";
            this.btncannel.Size = new System.Drawing.Size(75, 23);
            this.btncannel.TabIndex = 1;
            this.btncannel.Text = "取消";
            this.btncannel.UseVisualStyleBackColor = false;
            this.btncannel.Click += new System.EventHandler(this.btncannel_Click);
            // 
            // combin
            // 
            this.combin.BackColor = System.Drawing.Color.Transparent;
            this.combin.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.combin.ForeColor = System.Drawing.Color.Red;
            this.combin.Location = new System.Drawing.Point(16, 4);
            this.combin.Name = "combin";
            this.combin.Size = new System.Drawing.Size(164, 20);
            this.combin.TabIndex = 2;
            this.combin.Text = "0.00%";
            this.combin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.combin.MouseLeave += new System.EventHandler(this.control_MouseLeave);
            this.combin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.control_MouseDown);
            this.combin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.control_MouseMove);
            this.combin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.control_MouseUp);
            // 
            // fileprocess
            // 
            this.fileprocess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileprocess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.fileprocess.BackgroundBitmap = null;
            this.fileprocess.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(217)))), ((int)(((byte)(211)))));
            this.fileprocess.Border3D = System.Windows.Forms.Border3DStyle.Flat;
            this.fileprocess.BorderColor = System.Drawing.Color.Yellow;
            this.fileprocess.EnableBorder3D = false;
            this.fileprocess.ForeColor = System.Drawing.Color.Red;
            this.fileprocess.ForegroundBitmap = null;
            this.fileprocess.ForegroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.fileprocess.GradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.fileprocess.GradientMiddleColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.fileprocess.GradientStartColor = System.Drawing.Color.White;
            this.fileprocess.Location = new System.Drawing.Point(10, 24);
            this.fileprocess.Maximum = 100;
            this.fileprocess.Minimum = 0;
            this.fileprocess.Name = "fileprocess";
            this.fileprocess.ProgressTextColor = System.Drawing.Color.Empty;
            this.fileprocess.ProgressTextHiglightColor = System.Drawing.Color.Empty;
            this.fileprocess.ShowProgressText = false;
            this.fileprocess.Size = new System.Drawing.Size(176, 14);
            this.fileprocess.Smooth = true;
            this.fileprocess.Step = 1;
            this.fileprocess.TabIndex = 0;
            this.fileprocess.Value = 0;
            this.fileprocess.MouseLeave += new System.EventHandler(this.control_MouseLeave);
            this.fileprocess.MouseDown += new System.Windows.Forms.MouseEventHandler(this.control_MouseDown);
            this.fileprocess.MouseMove += new System.Windows.Forms.MouseEventHandler(this.control_MouseMove);
            this.fileprocess.MouseUp += new System.Windows.Forms.MouseEventHandler(this.control_MouseUp);
            // 
            // formGetFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(196, 68);
            this.Controls.Add(this.combin);
            this.Controls.Add(this.btncannel);
            this.Controls.Add(this.fileprocess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formGetFile";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "formGetFile";
            this.TopMost = true;
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.control_MouseUp);
            this.MouseLeave += new System.EventHandler(this.control_MouseLeave);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formGetFile_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.control_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.control_MouseDown);
            this.Load += new System.EventHandler(this.formGetFile_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UtilityLibrary.WinControls.ProgressBarEx fileprocess;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btncannel;
        private System.Windows.Forms.Label combin;
    }
}