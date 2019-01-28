namespace DRAWINFO
{
    partial class formDrawInfo
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
            this.pbdraw = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnpen = new System.Windows.Forms.Button();
            this.btnclear = new System.Windows.Forms.Button();
            this.btncolor = new System.Windows.Forms.Button();
            this.tmdL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bwcom = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnallclear = new System.Windows.Forms.Button();
            this.btncopy = new System.Windows.Forms.Button();
            this.optia = new System.Windows.Forms.TrackBar();
            this.btnback = new System.Windows.Forms.Button();
            this.btnnext = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbdraw)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optia)).BeginInit();
            this.SuspendLayout();
            // 
            // pbdraw
            // 
            this.pbdraw.BackColor = System.Drawing.Color.White;
            this.pbdraw.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pbdraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbdraw.Location = new System.Drawing.Point(0, 0);
            this.pbdraw.Name = "pbdraw";
            this.pbdraw.Size = new System.Drawing.Size(285, 225);
            this.pbdraw.TabIndex = 0;
            this.pbdraw.TabStop = false;
            this.pbdraw.MouseLeave += new System.EventHandler(this.pbdraw_MouseLeave);
            this.pbdraw.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbdraw_MouseMove);
            this.pbdraw.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbdraw_MouseDown);
            this.pbdraw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbdraw_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.pbdraw);
            this.panel1.Location = new System.Drawing.Point(57, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 225);
            this.panel1.TabIndex = 1;
            // 
            // btnpen
            // 
            this.btnpen.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpen.ForeColor = System.Drawing.Color.Fuchsia;
            this.btnpen.Location = new System.Drawing.Point(5, 38);
            this.btnpen.Name = "btnpen";
            this.btnpen.Size = new System.Drawing.Size(46, 23);
            this.btnpen.TabIndex = 2;
            this.btnpen.Text = "钢笔";
            this.btnpen.UseVisualStyleBackColor = true;
            this.btnpen.Click += new System.EventHandler(this.btnpen_Click);
            // 
            // btnclear
            // 
            this.btnclear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnclear.ForeColor = System.Drawing.Color.Fuchsia;
            this.btnclear.Location = new System.Drawing.Point(5, 69);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(46, 23);
            this.btnclear.TabIndex = 3;
            this.btnclear.Text = "橡皮";
            this.btnclear.UseVisualStyleBackColor = true;
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // btncolor
            // 
            this.btncolor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncolor.ForeColor = System.Drawing.Color.Fuchsia;
            this.btncolor.Location = new System.Drawing.Point(5, 100);
            this.btncolor.Name = "btncolor";
            this.btncolor.Size = new System.Drawing.Size(46, 23);
            this.btncolor.TabIndex = 3;
            this.btncolor.Text = "颜色";
            this.btncolor.UseVisualStyleBackColor = true;
            this.btncolor.Click += new System.EventHandler(this.btncolor_Click);
            // 
            // tmdL
            // 
            this.tmdL.AutoSize = true;
            this.tmdL.ForeColor = System.Drawing.Color.Cyan;
            this.tmdL.Location = new System.Drawing.Point(155, 8);
            this.tmdL.Name = "tmdL";
            this.tmdL.Size = new System.Drawing.Size(29, 12);
            this.tmdL.TabIndex = 17;
            this.tmdL.Text = "100%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "透明度:";
            // 
            // bwcom
            // 
            this.bwcom.FormattingEnabled = true;
            this.bwcom.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.bwcom.Location = new System.Drawing.Point(276, 5);
            this.bwcom.Name = "bwcom";
            this.bwcom.Size = new System.Drawing.Size(39, 20);
            this.bwcom.TabIndex = 19;
            this.bwcom.Text = "1";
            this.bwcom.SelectedIndexChanged += new System.EventHandler(this.bwcom_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label2.Location = new System.Drawing.Point(235, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "线宽:";
            // 
            // btnallclear
            // 
            this.btnallclear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnallclear.ForeColor = System.Drawing.Color.Fuchsia;
            this.btnallclear.Location = new System.Drawing.Point(5, 131);
            this.btnallclear.Name = "btnallclear";
            this.btnallclear.Size = new System.Drawing.Size(46, 23);
            this.btnallclear.TabIndex = 3;
            this.btnallclear.Text = "清除";
            this.btnallclear.UseVisualStyleBackColor = true;
            this.btnallclear.Click += new System.EventHandler(this.btnallclear_Click);
            // 
            // btncopy
            // 
            this.btncopy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncopy.ForeColor = System.Drawing.Color.Fuchsia;
            this.btncopy.Location = new System.Drawing.Point(5, 162);
            this.btncopy.Name = "btncopy";
            this.btncopy.Size = new System.Drawing.Size(46, 23);
            this.btncopy.TabIndex = 3;
            this.btncopy.Text = "插入";
            this.btncopy.UseVisualStyleBackColor = true;
            this.btncopy.Click += new System.EventHandler(this.btncopy_Click);
            // 
            // optia
            // 
            this.optia.AutoSize = false;
            this.optia.Location = new System.Drawing.Point(45, 1);
            this.optia.Maximum = 255;
            this.optia.Name = "optia";
            this.optia.Size = new System.Drawing.Size(104, 24);
            this.optia.TabIndex = 20;
            this.optia.TickStyle = System.Windows.Forms.TickStyle.None;
            this.optia.Value = 255;
            this.optia.Scroll += new System.EventHandler(this.optia_Scroll);
            // 
            // btnback
            // 
            this.btnback.Enabled = false;
            this.btnback.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnback.ForeColor = System.Drawing.Color.Fuchsia;
            this.btnback.Location = new System.Drawing.Point(5, 193);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(46, 23);
            this.btnback.TabIndex = 3;
            this.btnback.Text = "后退";
            this.btnback.UseVisualStyleBackColor = true;
            this.btnback.Click += new System.EventHandler(this.btnback_Click);
            // 
            // btnnext
            // 
            this.btnnext.Enabled = false;
            this.btnnext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnnext.ForeColor = System.Drawing.Color.Fuchsia;
            this.btnnext.Location = new System.Drawing.Point(5, 224);
            this.btnnext.Name = "btnnext";
            this.btnnext.Size = new System.Drawing.Size(46, 23);
            this.btnnext.TabIndex = 3;
            this.btnnext.Text = "恢复";
            this.btnnext.UseVisualStyleBackColor = true;
            this.btnnext.Click += new System.EventHandler(this.btnnext_Click);
            // 
            // formDrawInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(349, 271);
            this.Controls.Add(this.optia);
            this.Controls.Add(this.bwcom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tmdL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnnext);
            this.Controls.Add(this.btnback);
            this.Controls.Add(this.btncopy);
            this.Controls.Add(this.btnallclear);
            this.Controls.Add(this.btncolor);
            this.Controls.Add(this.btnclear);
            this.Controls.Add(this.btnpen);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "formDrawInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "涂鸦";
            this.Load += new System.EventHandler(this.formDrawInfo_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formDrawInfo_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pbdraw)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optia)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbdraw;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnpen;
        private System.Windows.Forms.Button btnclear;
        private System.Windows.Forms.Button btncolor;
        private System.Windows.Forms.Label tmdL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox bwcom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnallclear;
        private System.Windows.Forms.Button btncopy;
        private System.Windows.Forms.TrackBar optia;
        private System.Windows.Forms.Button btnback;
        private System.Windows.Forms.Button btnnext;
    }
}