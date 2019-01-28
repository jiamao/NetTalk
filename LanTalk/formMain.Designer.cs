namespace LanTalk
{
    partial class formMain
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
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.mainbtnmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.发送给指定IPSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.退出EToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.notifymenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msgtime = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.allFriends = new UtilityLibrary.WinControls.OutlookBar();
            this.mainmenu = new System.Windows.Forms.MenuStrip();
            this.菜单FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.个人设置CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.忙碌BToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.外出OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.不想理NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.在线LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐身GToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.界面SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lanMsgLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qQ样式QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainbtnmenu.SuspendLayout();
            this.notifymenu.SuspendLayout();
            this.mainmenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notify
            // 
            this.notify.ContextMenuStrip = this.mainbtnmenu;
            this.notify.Text = "LanTalk";
            this.notify.Visible = true;
            this.notify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notify_MouseDoubleClick);
            // 
            // mainbtnmenu
            // 
            this.mainbtnmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.发送给指定IPSToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.退出EToolStripMenuItem1});
            this.mainbtnmenu.Name = "mainbtnmenu";
            this.mainbtnmenu.Size = new System.Drawing.Size(161, 92);
            // 
            // 发送给指定IPSToolStripMenuItem
            // 
            this.发送给指定IPSToolStripMenuItem.Name = "发送给指定IPSToolStripMenuItem";
            this.发送给指定IPSToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.发送给指定IPSToolStripMenuItem.Text = "发送给指定IP(&S)";
            this.发送给指定IPSToolStripMenuItem.Click += new System.EventHandler(this.发送给指定IPSToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem2.Text = "LanMsg(&L)";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem1.Text = "个人设置(&C)";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 退出EToolStripMenuItem1
            // 
            this.退出EToolStripMenuItem1.Name = "退出EToolStripMenuItem1";
            this.退出EToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.退出EToolStripMenuItem1.Text = "退出(&E)";
            this.退出EToolStripMenuItem1.Click += new System.EventHandler(this.退出EToolStripMenuItem1_Click);
            // 
            // notifymenu
            // 
            this.notifymenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出EToolStripMenuItem});
            this.notifymenu.Name = "notifymenu";
            this.notifymenu.Size = new System.Drawing.Size(113, 26);
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.退出EToolStripMenuItem.Text = "退出(&E)";
            this.退出EToolStripMenuItem.Click += new System.EventHandler(this.退出EToolStripMenuItem_Click);
            // 
            // msgtime
            // 
            this.msgtime.Interval = 300;
            this.msgtime.Tick += new System.EventHandler(this.msgtime_Tick);
            // 
            // allFriends
            // 
            this.allFriends.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.allFriends.AnimationSpeed = 20;
            this.allFriends.BackColor = System.Drawing.Color.Teal;
            this.allFriends.BackgroundBitmap = null;
            this.allFriends.BorderType = UtilityLibrary.WinControls.BorderType.Fixed3D;
            this.allFriends.FlatArrowButtons = false;
            this.allFriends.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.allFriends.LeftTopColor = System.Drawing.Color.Empty;
            this.allFriends.Location = new System.Drawing.Point(12, 26);
            this.allFriends.Name = "allFriends";
            this.allFriends.RightBottomColor = System.Drawing.Color.Empty;
            this.allFriends.Size = new System.Drawing.Size(169, 636);
            this.allFriends.TabIndex = 0;
            this.allFriends.ItemClicked += new UtilityLibrary.WinControls.OutlookBarItemClickedHandler(this.allFriends_ItemClicked);
            // 
            // mainmenu
            // 
            this.mainmenu.BackColor = System.Drawing.Color.Transparent;
            this.mainmenu.Dock = System.Windows.Forms.DockStyle.None;
            this.mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.菜单FToolStripMenuItem,
            this.toolStripMenuItem4,
            this.界面SToolStripMenuItem,
            this.toolStripMenuItem5,
            this.帮助HToolStripMenuItem});
            this.mainmenu.Location = new System.Drawing.Point(-4, -1);
            this.mainmenu.Name = "mainmenu";
            this.mainmenu.Size = new System.Drawing.Size(185, 24);
            this.mainmenu.TabIndex = 10;
            this.mainmenu.Text = "menu";
            // 
            // 菜单FToolStripMenuItem
            // 
            this.菜单FToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.菜单FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.个人设置CToolStripMenuItem,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem3});
            this.菜单FToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.菜单FToolStripMenuItem.Name = "菜单FToolStripMenuItem";
            this.菜单FToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.菜单FToolStripMenuItem.Text = "菜单(&F)";
            // 
            // 个人设置CToolStripMenuItem
            // 
            this.个人设置CToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.个人设置CToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.个人设置CToolStripMenuItem.Name = "个人设置CToolStripMenuItem";
            this.个人设置CToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.个人设置CToolStripMenuItem.Text = "个人设置(&C)";
            this.个人设置CToolStripMenuItem.Click += new System.EventHandler(this.个人设置CToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.BackColor = System.Drawing.Color.Teal;
            this.toolStripMenuItem6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem6.Text = "选项(&P)";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.BackColor = System.Drawing.Color.Teal;
            this.toolStripMenuItem7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem7.Text = "LanMsg样式(&L)";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.BackColor = System.Drawing.Color.Teal;
            this.toolStripMenuItem3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem3.Text = "退出(&E)";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.BackColor = System.Drawing.Color.Transparent;
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.忙碌BToolStripMenuItem,
            this.外出OToolStripMenuItem,
            this.不想理NToolStripMenuItem,
            this.在线LToolStripMenuItem,
            this.隐身GToolStripMenuItem});
            this.toolStripMenuItem4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItem4.Text = "状态(&T)";
            // 
            // 忙碌BToolStripMenuItem
            // 
            this.忙碌BToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.忙碌BToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.忙碌BToolStripMenuItem.Name = "忙碌BToolStripMenuItem";
            this.忙碌BToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.忙碌BToolStripMenuItem.Text = "忙碌(&B)";
            this.忙碌BToolStripMenuItem.Click += new System.EventHandler(this.BToolStripMenuItem_Click);
            // 
            // 外出OToolStripMenuItem
            // 
            this.外出OToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.外出OToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.外出OToolStripMenuItem.Name = "外出OToolStripMenuItem";
            this.外出OToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.外出OToolStripMenuItem.Text = "外出(&O)";
            this.外出OToolStripMenuItem.Click += new System.EventHandler(this.BToolStripMenuItem_Click);
            // 
            // 不想理NToolStripMenuItem
            // 
            this.不想理NToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.不想理NToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.不想理NToolStripMenuItem.Name = "不想理NToolStripMenuItem";
            this.不想理NToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.不想理NToolStripMenuItem.Text = "不想理(&N)";
            this.不想理NToolStripMenuItem.Click += new System.EventHandler(this.BToolStripMenuItem_Click);
            // 
            // 在线LToolStripMenuItem
            // 
            this.在线LToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.在线LToolStripMenuItem.Checked = true;
            this.在线LToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.在线LToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.在线LToolStripMenuItem.Name = "在线LToolStripMenuItem";
            this.在线LToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.在线LToolStripMenuItem.Text = "在线(&L)";
            this.在线LToolStripMenuItem.Click += new System.EventHandler(this.BToolStripMenuItem_Click);
            // 
            // 隐身GToolStripMenuItem
            // 
            this.隐身GToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.隐身GToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.隐身GToolStripMenuItem.Name = "隐身GToolStripMenuItem";
            this.隐身GToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.隐身GToolStripMenuItem.Text = "隐身(&G)";
            this.隐身GToolStripMenuItem.Click += new System.EventHandler(this.BToolStripMenuItem_Click);
            // 
            // 界面SToolStripMenuItem
            // 
            this.界面SToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.界面SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lanMsgLToolStripMenuItem,
            this.qQ样式QToolStripMenuItem});
            this.界面SToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.界面SToolStripMenuItem.Name = "界面SToolStripMenuItem";
            this.界面SToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.界面SToolStripMenuItem.Text = "界面(&S)";
            this.界面SToolStripMenuItem.Visible = false;
            // 
            // lanMsgLToolStripMenuItem
            // 
            this.lanMsgLToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.lanMsgLToolStripMenuItem.Name = "lanMsgLToolStripMenuItem";
            this.lanMsgLToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lanMsgLToolStripMenuItem.Text = "LanMsg(&L)";
            this.lanMsgLToolStripMenuItem.Click += new System.EventHandler(this.lanMsgLToolStripMenuItem_Click);
            // 
            // qQ样式QToolStripMenuItem
            // 
            this.qQ样式QToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.qQ样式QToolStripMenuItem.Name = "qQ样式QToolStripMenuItem";
            this.qQ样式QToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.qQ样式QToolStripMenuItem.Text = "QQ样式(&Q)";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8});
            this.toolStripMenuItem5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItem5.Text = "工具(&L)";
            this.toolStripMenuItem5.Visible = false;
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.BackColor = System.Drawing.Color.Teal;
            this.toolStripMenuItem8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItem8.Text = "选项(&P)";
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助PToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.帮助HToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 帮助PToolStripMenuItem
            // 
            this.帮助PToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.帮助PToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.帮助PToolStripMenuItem.Name = "帮助PToolStripMenuItem";
            this.帮助PToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.帮助PToolStripMenuItem.Text = "帮助(&P)";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.BackColor = System.Drawing.Color.Teal;
            this.关于ToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.关于ToolStripMenuItem.Text = "关于(&B)";
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(192, 674);
            this.ContextMenuStrip = this.mainbtnmenu;
            this.Controls.Add(this.mainmenu);
            this.Controls.Add(this.allFriends);
            this.MaximumSize = new System.Drawing.Size(200, 2000);
            this.MinimumSize = new System.Drawing.Size(200, 27);
            this.Name = "formMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LanTalk";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Shown += new System.EventHandler(this.formMain_Shown);
            this.MouseEnter += new System.EventHandler(this.formMain_MouseHover);
            this.Move += new System.EventHandler(this.formMain_Move);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMain_FormClosing);
            this.Load += new System.EventHandler(this.formMain_Load);
            this.mainbtnmenu.ResumeLayout(false);
            this.notifymenu.ResumeLayout(false);
            this.mainmenu.ResumeLayout(false);
            this.mainmenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public UtilityLibrary.WinControls.OutlookBar allFriends;
        private System.Windows.Forms.Timer msgtime;
        public System.Windows.Forms.NotifyIcon notify;
        public System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip notifymenu;
        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mainbtnmenu;
        private System.Windows.Forms.ToolStripMenuItem 发送给指定IPSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.MenuStrip mainmenu;
        private System.Windows.Forms.ToolStripMenuItem 菜单FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 个人设置CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 忙碌BToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 外出OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 不想理NToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 在线LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐身GToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 界面SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lanMsgLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qQ样式QToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;



    }
}

