namespace CaptureScreen
{
    partial class FormFullScreen
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
            this.hwndpb = new System.Windows.Forms.Panel();
            this.tooltip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hwndpb
            // 
            this.hwndpb.BackColor = System.Drawing.Color.Transparent;
            this.hwndpb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hwndpb.Location = new System.Drawing.Point(306, 189);
            this.hwndpb.Name = "hwndpb";
            this.hwndpb.Size = new System.Drawing.Size(11, 10);
            this.hwndpb.TabIndex = 2;
            this.hwndpb.Visible = false;
            this.hwndpb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fullform_MouseDown);
            this.hwndpb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.hwndpb_MouseDoubleClick);
            // 
            // tooltip
            // 
            this.tooltip.AutoSize = true;
            this.tooltip.BackColor = System.Drawing.Color.Transparent;
            this.tooltip.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tooltip.ForeColor = System.Drawing.Color.Red;
            this.tooltip.Location = new System.Drawing.Point(116, 64);
            this.tooltip.Name = "tooltip";
            this.tooltip.Size = new System.Drawing.Size(127, 16);
            this.tooltip.TabIndex = 3;
            this.tooltip.Text = "框选开始截屏！";
            // 
            // FormFullScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.tooltip);
            this.Controls.Add(this.hwndpb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFullScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormFullScreen";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.hwndpb_MouseDoubleClick);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fullform_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fullform_MouseMove);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormFullScreen_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fullform_MouseDown);
            this.Load += new System.EventHandler(this.FormFullScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel hwndpb;
        private System.Windows.Forms.Label tooltip;
    }
}