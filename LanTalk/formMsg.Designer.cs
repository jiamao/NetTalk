namespace LanTalk
{
    partial class formMsg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMsg));
            this.tpbtnface = new System.Windows.Forms.ToolTip(this.components);
            this.richmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.剪切CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelsend = new System.Windows.Forms.Panel();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.button2 = new DevComponents.DotNetBar.ButtonItem();
            this.btnMsgFace = new DevComponents.DotNetBar.ButtonItem();
            this.button1 = new DevComponents.DotNetBar.ButtonItem();
            this.cutscreen = new DevComponents.DotNetBar.ButtonItem();
            this.butSendFile = new DevComponents.DotNetBar.ButtonItem();
            this.btndrawinfo = new DevComponents.DotNetBar.ButtonItem();
            this.btnsend = new DevComponents.DotNetBar.ButtonX();
            this.panel2 = new System.Windows.Forms.Panel();
            this.thisicon = new System.Windows.Forms.PictureBox();
            this.lblTiTle = new System.Windows.Forms.Label();
            this.btnMin = new System.Windows.Forms.Button();
            this.btnMax = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtMsg = new RichTextBox.MyExtRichTextBox();
            this.txtsend = new RichTextBox.MyExtRichTextBox();
            this.richmenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelsend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thisicon)).BeginInit();
            this.SuspendLayout();
            // 
            // richmenu
            // 
            this.richmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.剪切CToolStripMenuItem,
            this.复制CToolStripMenuItem,
            this.粘贴PToolStripMenuItem});
            this.richmenu.Name = "richmenu";
            this.richmenu.Size = new System.Drawing.Size(113, 70);
            // 
            // 剪切CToolStripMenuItem
            // 
            this.剪切CToolStripMenuItem.Name = "剪切CToolStripMenuItem";
            this.剪切CToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.剪切CToolStripMenuItem.Text = "剪切(&T)";
            this.剪切CToolStripMenuItem.Click += new System.EventHandler(this.剪切CToolStripMenuItem_Click);
            // 
            // 复制CToolStripMenuItem
            // 
            this.复制CToolStripMenuItem.Name = "复制CToolStripMenuItem";
            this.复制CToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.复制CToolStripMenuItem.Text = "复制(&C)";
            this.复制CToolStripMenuItem.Click += new System.EventHandler(this.复制CToolStripMenuItem_Click);
            // 
            // 粘贴PToolStripMenuItem
            // 
            this.粘贴PToolStripMenuItem.Name = "粘贴PToolStripMenuItem";
            this.粘贴PToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.粘贴PToolStripMenuItem.Text = "粘贴(&P)";
            this.粘贴PToolStripMenuItem.Click += new System.EventHandler(this.粘贴PToolStripMenuItem_Click);
            // 
            // fontDialog1
            // 
            this.fontDialog1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(134)));
            this.fontDialog1.ShowColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(12, 31);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtMsg);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelsend);
            this.splitContainer1.Size = new System.Drawing.Size(434, 388);
            this.splitContainer1.SplitterDistance = 231;
            this.splitContainer1.TabIndex = 16;
            // 
            // panelsend
            // 
            this.panelsend.BackColor = System.Drawing.Color.Teal;
            this.panelsend.Controls.Add(this.bar1);
            this.panelsend.Controls.Add(this.btnsend);
            this.panelsend.Controls.Add(this.txtsend);
            this.panelsend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelsend.Location = new System.Drawing.Point(0, 0);
            this.panelsend.Name = "panelsend";
            this.panelsend.Size = new System.Drawing.Size(430, 149);
            this.panelsend.TabIndex = 0;
            // 
            // bar1
            // 
            this.bar1.AccessibleDescription = "DotNetBar Bar (bar1)";
            this.bar1.AccessibleName = "DotNetBar Bar";
            this.bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.bar1.BackColor = System.Drawing.Color.Transparent;
            this.bar1.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.button2,
            this.btnMsgFace,
            this.button1,
            this.cutscreen,
            this.butSendFile,
            this.btndrawinfo});
            this.bar1.ItemSpacing = 8;
            this.bar1.Location = new System.Drawing.Point(1, 3);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(348, 29);
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.VS2005;
            this.bar1.TabIndex = 19;
            this.bar1.TabStop = false;
            this.bar1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.bar1_MouseMove);
            // 
            // button2
            // 
            this.button2.Icon = ((System.Drawing.Icon)(resources.GetObject("button2.Icon")));
            this.button2.Name = "button2";
            this.button2.Tooltip = "设置字体";
            this.button2.Click += new System.EventHandler(this.btnfont_Click);
            // 
            // btnMsgFace
            // 
            this.btnMsgFace.Icon = ((System.Drawing.Icon)(resources.GetObject("btnMsgFace.Icon")));
            this.btnMsgFace.Name = "btnMsgFace";
            this.btnMsgFace.Text = "发送表情";
            this.btnMsgFace.Tooltip = "发送表情";
            // 
            // button1
            // 
            this.button1.Icon = ((System.Drawing.Icon)(resources.GetObject("button1.Icon")));
            this.button1.Name = "button1";
            this.button1.Text = "buttonItem3";
            this.button1.Tooltip = "发送图片";
            this.button1.Click += new System.EventHandler(this.btnface_Click);
            // 
            // cutscreen
            // 
            this.cutscreen.Image = ((System.Drawing.Image)(resources.GetObject("cutscreen.Image")));
            this.cutscreen.Name = "cutscreen";
            this.cutscreen.Text = "butCapture";
            this.cutscreen.Tooltip = "发送“屏幕截图”";
            this.cutscreen.Click += new System.EventHandler(this.cutscreen_Click);
            // 
            // butSendFile
            // 
            this.butSendFile.Icon = ((System.Drawing.Icon)(resources.GetObject("butSendFile.Icon")));
            this.butSendFile.Name = "butSendFile";
            this.butSendFile.Text = "发送文件";
            this.butSendFile.Tooltip = "发送文件";
            this.butSendFile.Click += new System.EventHandler(this.btnsendfile_Click);
            // 
            // btndrawinfo
            // 
            this.btndrawinfo.Image = global::LanTalk.ResourceUserHeader._32_1;
            this.btndrawinfo.Name = "btndrawinfo";
            this.btndrawinfo.Text = "涂鸦";
            this.btndrawinfo.Tooltip = "涂鸦";
            this.btndrawinfo.Click += new System.EventHandler(this.btndrawinfo_Click);
            // 
            // btnsend
            // 
            this.btnsend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnsend.ColorScheme.DockSiteBackColorGradientAngle = 0;
            this.btnsend.Location = new System.Drawing.Point(359, 122);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(67, 23);
            this.btnsend.TabIndex = 18;
            this.btnsend.Text = "发送";
            this.btnsend.Click += new System.EventHandler(this.btnsend_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnMax);
            this.panel2.Controls.Add(this.btnMin);
            this.panel2.Controls.Add(this.thisicon);
            this.panel2.Controls.Add(this.lblTiTle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(460, 25);
            this.panel2.TabIndex = 15;
            this.panel2.MouseLeave += new System.EventHandler(this.control_MouseLeave);
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.control_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.control_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.control_MouseUp);
            // 
            // thisicon
            // 
            this.thisicon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("thisicon.BackgroundImage")));
            this.thisicon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.thisicon.Location = new System.Drawing.Point(9, 3);
            this.thisicon.Name = "thisicon";
            this.thisicon.Size = new System.Drawing.Size(22, 20);
            this.thisicon.TabIndex = 9;
            this.thisicon.TabStop = false;
            // 
            // lblTiTle
            // 
            this.lblTiTle.AutoSize = true;
            this.lblTiTle.BackColor = System.Drawing.Color.Transparent;
            this.lblTiTle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTiTle.ForeColor = System.Drawing.Color.GreenYellow;
            this.lblTiTle.Location = new System.Drawing.Point(36, 6);
            this.lblTiTle.Name = "lblTiTle";
            this.lblTiTle.Size = new System.Drawing.Size(119, 14);
            this.lblTiTle.TabIndex = 0;
            this.lblTiTle.Text = "NetTalk-聊天工具";
            this.lblTiTle.MouseLeave += new System.EventHandler(this.control_MouseLeave);
            this.lblTiTle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.control_MouseDown);
            this.lblTiTle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.control_MouseMove);
            this.lblTiTle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.control_MouseUp);
            // 
            // btnMin
            // 
            this.btnMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMin.BackColor = System.Drawing.Color.Transparent;
            this.btnMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMin.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMin.ForeColor = System.Drawing.Color.Red;
            this.btnMin.Location = new System.Drawing.Point(391, 5);
            this.btnMin.Margin = new System.Windows.Forms.Padding(0);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(15, 15);
            this.btnMin.TabIndex = 8;
            this.btnMin.Text = "-";
            this.btnMin.UseVisualStyleBackColor = false;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnMax
            // 
            this.btnMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMax.BackColor = System.Drawing.Color.Transparent;
            this.btnMax.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMax.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMax.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMax.ForeColor = System.Drawing.Color.Red;
            this.btnMax.Location = new System.Drawing.Point(411, 5);
            this.btnMax.Margin = new System.Windows.Forms.Padding(0);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(15, 15);
            this.btnMax.TabIndex = 8;
            this.btnMax.Text = "口";
            this.btnMax.UseVisualStyleBackColor = false;
            this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("宋体", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.ForeColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(431, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(15, 15);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.Color.White;
            this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg.HiglightColor = RichTextBox.RtfColor.White;
            this.txtMsg.Location = new System.Drawing.Point(0, 0);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ReadOnly = true;
            this.txtMsg.Size = new System.Drawing.Size(430, 227);
            this.txtMsg.TabIndex = 0;
            this.txtMsg.Text = "";
            this.txtMsg.TextColor = RichTextBox.RtfColor.Black;
            this.txtMsg.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.txtsend_LinkClicked);
            this.txtMsg.MouseEnter += new System.EventHandler(this.txtMsg_Enter);
            this.txtMsg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMsg_KeyDown);
            this.txtMsg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtMsg_MouseDown);
            // 
            // txtsend
            // 
            this.txtsend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtsend.BackColor = System.Drawing.Color.White;
            this.txtsend.EnableAutoDragDrop = true;
            this.txtsend.HiglightColor = RichTextBox.RtfColor.White;
            this.txtsend.Location = new System.Drawing.Point(0, 31);
            this.txtsend.MaxLength = 10000;
            this.txtsend.Name = "txtsend";
            this.txtsend.Size = new System.Drawing.Size(430, 86);
            this.txtsend.TabIndex = 17;
            this.txtsend.Text = "";
            this.txtsend.TextColor = RichTextBox.RtfColor.Black;
            this.txtsend.MouseEnter += new System.EventHandler(this.txtsend_Enter);
            this.txtsend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtsend_KeyDown);
            this.txtsend.TextChanged += new System.EventHandler(this.txtsend_TextChanged);
            this.txtsend.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtsend_MouseDown);
            // 
            // formMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(460, 431);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "formMsg";
            this.Text = "formMsg";
            this.SizeChanged += new System.EventHandler(this.formMsg_SizeChanged);
            this.Shown += new System.EventHandler(this.formMsg_Shown);
            this.Activated += new System.EventHandler(this.formMsg_Activated);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.control_MouseUp);
            this.MouseLeave += new System.EventHandler(this.control_MouseLeave);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMsg_FormClosing);
            this.TextChanged += new System.EventHandler(this.formMsg_TextChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.control_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.control_MouseDown);
            this.Load += new System.EventHandler(this.formMsg_Load);
            this.richmenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelsend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thisicon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip tpbtnface;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ContextMenuStrip richmenu;
        private System.Windows.Forms.ToolStripMenuItem 剪切CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 粘贴PToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox thisicon;
        private System.Windows.Forms.Label lblTiTle;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.Button btnMax;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelsend;
        private RichTextBox.MyExtRichTextBox txtMsg;
        private RichTextBox.MyExtRichTextBox txtsend;
        private DevComponents.DotNetBar.ButtonX btnsend;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem button2;
        private DevComponents.DotNetBar.ButtonItem btnMsgFace;
        private DevComponents.DotNetBar.ButtonItem button1;
        private DevComponents.DotNetBar.ButtonItem cutscreen;
        private DevComponents.DotNetBar.ButtonItem butSendFile;
        private DevComponents.DotNetBar.ButtonItem btndrawinfo;
    }
}