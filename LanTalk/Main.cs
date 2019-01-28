using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
namespace LanTalk
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();            
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                showinfo();                
                Application.DoEvents();               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void showinfo()
        {
            Graphics g = this.CreateGraphics();
            g.Clear(Color.White);            
            GraphicsPath gp = new GraphicsPath();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            StringFormat sf = new StringFormat();
            gp.AddString("程序初始化中...", new FontFamily("Arial"), 1, 30, new Point(0, 30), sf);
            Pen mypen = new Pen(Color.Green);
            g.DrawPath(mypen, gp);
            this.Region = new System.Drawing.Region(gp);
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Helper.Face.Equals("qq"))
                {
                    Program.formmain.Cancel = false;
                    Program.formmain.Close();
                }
                else
                {
                    Program.formlist.Cancal = false;
                    Program.formlist.Close();
                }
                Helper.exitme();
                Helper.writeLog("退出程序");
            }
            catch
            {
            }
            finally
            {
                Process[] lanmsg = Helper.getProcessesByName(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName));
                if (lanmsg.Length > 1)
                {
                    foreach (Process p in lanmsg)
                    {
                        p.Kill();
                        p.WaitForExit();
                    }
                }
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            selectForm();
        }
       
        private void selectForm()
        {
            try
            {
                if (rblanmsg.Checked)
                {
                    Helper.Face = "lanmsg";
                    //Program.formlist = new formList();
                }
                else
                {
                    Helper.Face = "qq";
                    //Program.formmain = new formMain();
                }
                this.Opacity = 0;
                Helper.initForm();
            }
            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show("初始化端口" + Helper.LanPort.ToString() + "失败!");
                this.Close();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
                Application.Exit();
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            try
            {
                Helper.init();
                if (!Helper.IsNet)
                {
                   /* Process[] lanmsg = Helper.getProcessesByName("WinLanMsg");
                    if (lanmsg.Length > 0)
                    {
                        if (MessageBox.Show("已有较旧版本正在运行，是否关闭它，运行此程序？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                        {
                            this.Close();
                        }
                        foreach (Process p in lanmsg)
                        {
                            p.Kill();
                            p.WaitForExit();
                        }
                    }*/
                    var lanmsg = Helper.getProcessesByName(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName));
                    if (lanmsg.Length > 1)
                    {
                        MessageBox.Show("已一个实列正在运行！", "Ｅrror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                }
                selectForm();                
            }
            catch(System.Net.Sockets.SocketException ex)
            {                
                MessageBox.Show("程序初始化通信端口失败！");
                this.Close();
                Application.Exit();
            }
            catch (Exception ex)
            {
                Helper.writeLog(ex.Message);
                MessageBox.Show(ex.Message);
                this.Close();
                Application.Exit();
            }
        }

    }
}