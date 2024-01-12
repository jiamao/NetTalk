using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LanTalk
{
    static class Program
    {
        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainform = new Main();
            Application.Run(mainform);           
        }
        public static Main mainform;
        public static formMain formmain;
        public static formList formlist;
    }
}