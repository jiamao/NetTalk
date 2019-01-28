using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace GolbalHook
{
    public class HookApi
    {
        //安装钩子原型 
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx
            (
                int hookid,
                HookBase.HookPro pfnhook,
                IntPtr hinst,
                int threadid
            );

        //卸载钩子原型 
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool UnhookWindowsHookEx
            (
               IntPtr hhook
            );

        //回调钩子原型 
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr CallNextHookEx
            (
                IntPtr hhook,
                int code,
                IntPtr wparam,
                IntPtr lparam
            );

        //。。。原型 
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory
            (
                ref HookBase.KBDLLHOOKSTRUCT Source,
                IntPtr Destination, int Length
            );
    }
}
