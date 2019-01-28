namespace LanTalkServer
{
    partial class FormLanTalkServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLanTalkServer));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.控制CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始服务SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新增用户NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvlan = new System.Windows.Forms.ListView();
            this.lanid = new System.Windows.Forms.ColumnHeader();
            this.langroup = new System.Windows.Forms.ColumnHeader();
            this.lanname = new System.Windows.Forms.ColumnHeader();
            this.lanip = new System.Windows.Forms.ColumnHeader();
            this.lanport = new System.Windows.Forms.ColumnHeader();
            this.lanver = new System.Windows.Forms.ColumnHeader();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbinfo = new System.Windows.Forms.ListBox();
            this.servertimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.lanstate = new System.Windows.Forms.ColumnHeader();
            this.lvnet = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.控制CToolStripMenuItem,
            this.工具TToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(755, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 控制CToolStripMenuItem
            // 
            this.控制CToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始服务SToolStripMenuItem,
            this.退出EToolStripMenuItem});
            this.控制CToolStripMenuItem.Name = "控制CToolStripMenuItem";
            this.控制CToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.控制CToolStripMenuItem.Text = "控制(&C)";
            // 
            // 开始服务SToolStripMenuItem
            // 
            this.开始服务SToolStripMenuItem.Name = "开始服务SToolStripMenuItem";
            this.开始服务SToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.开始服务SToolStripMenuItem.Text = "开始服务(&S)";
            this.开始服务SToolStripMenuItem.Click += new System.EventHandler(this.开始服务SToolStripMenuItem_Click);
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.退出EToolStripMenuItem.Text = "退出(&E)";
            this.退出EToolStripMenuItem.Click += new System.EventHandler(this.退出EToolStripMenuItem_Click);
            // 
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增用户NToolStripMenuItem});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            this.工具TToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.工具TToolStripMenuItem.Text = "工具(&T)";
            // 
            // 新增用户NToolStripMenuItem
            // 
            this.新增用户NToolStripMenuItem.Name = "新增用户NToolStripMenuItem";
            this.新增用户NToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.新增用户NToolStripMenuItem.Text = "新增用户(&N)";
            this.新增用户NToolStripMenuItem.Click += new System.EventHandler(this.新增用户NToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(755, 404);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvlan);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(747, 379);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "内网用户";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvlan
            // 
            this.lvlan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvlan.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lanid,
            this.langroup,
            this.lanname,
            this.lanstate,
            this.lanip,
            this.lanport,
            this.lanver});
            this.lvlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvlan.FullRowSelect = true;
            this.lvlan.Location = new System.Drawing.Point(3, 3);
            this.lvlan.Name = "lvlan";
            this.lvlan.Size = new System.Drawing.Size(741, 373);
            this.lvlan.TabIndex = 0;
            this.lvlan.UseCompatibleStateImageBehavior = false;
            this.lvlan.View = System.Windows.Forms.View.Details;
            this.lvlan.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvlan_ColumnClick);
            // 
            // lanid
            // 
            this.lanid.Text = "ID";
            this.lanid.Width = 113;
            // 
            // langroup
            // 
            this.langroup.Text = "群组";
            this.langroup.Width = 139;
            // 
            // lanname
            // 
            this.lanname.Text = "用户名";
            this.lanname.Width = 110;
            // 
            // lanip
            // 
            this.lanip.Text = "IP地址";
            this.lanip.Width = 119;
            // 
            // lanport
            // 
            this.lanport.Text = "端口";
            this.lanport.Width = 57;
            // 
            // lanver
            // 
            this.lanver.Text = "版本";
            this.lanver.Width = 76;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lvnet);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(747, 379);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "外网用户";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbinfo
            // 
            this.lbinfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbinfo.FormattingEnabled = true;
            this.lbinfo.ItemHeight = 12;
            this.lbinfo.Location = new System.Drawing.Point(4, 442);
            this.lbinfo.Name = "lbinfo";
            this.lbinfo.Size = new System.Drawing.Size(747, 76);
            this.lbinfo.TabIndex = 2;
            // 
            // servertimer
            // 
            this.servertimer.Tick += new System.EventHandler(this.servertimer_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "NetTalk服务程序";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // lanstate
            // 
            this.lanstate.Text = "状态";
            this.lanstate.Width = 80;
            // 
            // lvnet
            // 
            this.lvnet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvnet.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvnet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvnet.FullRowSelect = true;
            this.lvnet.Location = new System.Drawing.Point(3, 3);
            this.lvnet.Name = "lvnet";
            this.lvnet.Size = new System.Drawing.Size(741, 373);
            this.lvnet.TabIndex = 1;
            this.lvnet.UseCompatibleStateImageBehavior = false;
            this.lvnet.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 113;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "群组";
            this.columnHeader2.Width = 139;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "用户名";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "状态";
            this.columnHeader4.Width = 80;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "IP地址";
            this.columnHeader5.Width = 119;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "端口";
            this.columnHeader6.Width = 57;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "版本";
            this.columnHeader7.Width = 76;
            // 
            // FormLanTalkServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 528);
            this.Controls.Add(this.lbinfo);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormLanTalkServer";
            this.Text = "NetTalkServer服务程序";
            this.Shown += new System.EventHandler(this.FormLanTalkServer_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLanTalkServer_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 控制CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始服务SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具TToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ColumnHeader lanid;
        private System.Windows.Forms.ColumnHeader langroup;
        private System.Windows.Forms.ColumnHeader lanname;
        private System.Windows.Forms.ColumnHeader lanip;
        private System.Windows.Forms.ColumnHeader lanport;
        private System.Windows.Forms.ColumnHeader lanver;
        public System.Windows.Forms.ListView lvlan;
        public System.Windows.Forms.ListBox lbinfo;
        private System.Windows.Forms.Timer servertimer;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem 新增用户NToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader lanstate;
        public System.Windows.Forms.ListView lvnet;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}

