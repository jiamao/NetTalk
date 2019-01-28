using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GolbalHook
{
    public class HookBase
    {
        public delegate IntPtr HookPro(int nCode, IntPtr wParam, IntPtr lParam);  //创建委托，进行回调
        static IntPtr hHook = IntPtr.Zero;  //创建钩子编号 
        const int WH_KEYBOARD_LL = 13;  //创建键盘钩子类型

        GCHandle _hookProcHandle;

        public delegate void keypresscode(int keycode, int modifierkeys);//创建按健委托．
        public keypresscode curkeypresscode;
        public HookBase()
        {
            throw new Exception("需要正确的标识来引用此类!");
        }
        public HookBase(string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("需要正确的标识来引用此类!");
        }
       /// <summary>
       /// 钩子回调
       /// </summary>
       /// <param name="nCode"></param>
       /// <param name="wParam"></param>
       /// <param name="lParam"></param>
       /// <returns></returns>
        public IntPtr KEYBOARD_HOOKPRO(int nCode, IntPtr wParam, IntPtr lParam)
        {
            KBDLLHOOKSTRUCT kb = new KBDLLHOOKSTRUCT();
            HookApi.CopyMemory(ref kb, lParam, 20);      //结果就在这里了

            UNLOAD_WINDOWS_KETBOARD_HOOK();
            SET_WINDOWS_KEYBOARD_HOOK();
            curkeypresscode(kb.vkCode, (int)Control.ModifierKeys);
            return HookApi.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// 设置钩子
        /// </summary>
        public void SET_WINDOWS_KEYBOARD_HOOK()
        {
            if (hHook == IntPtr.Zero)
            {
                HookPro hk = new HookPro(this.KEYBOARD_HOOKPRO);
                _hookProcHandle = GCHandle.Alloc(hk);
                //挂钩子
                hHook = HookApi.SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    hk,
                    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                if (hHook == IntPtr.Zero)
                {
                    throw new Exception("安装钩子不成功！");  // 挂钩子不成功返回值 0
                }
            }
        }

       /// <summary>
       /// 卸载钩子
       /// </summary>
        public void UNLOAD_WINDOWS_KETBOARD_HOOK()
        {
            if (hHook != IntPtr.Zero)
            {
                //如果钩子已经挂上则取消钩子，否则不用取消
                HookApi.UnhookWindowsHookEx(hHook);
                _hookProcHandle.Free();
                hHook = IntPtr.Zero;
            }
        }

        //键盘Hook结构函数
        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        string _guidHexs = "0123456789ABCDEF-";
        byte[] _infoguidbytes = new byte[] { 7, 13, 0, 7, 2, 5, 11, 8, 16, 7, 10, 0, 3, 16, 4, 9, 5, 15, 16,
            11, 7, 12, 13, 16, 15, 0, 11, 6, 1, 13, 15, 15, 6, 7, 12, 1 };
        string dllguid = "";
        string _guid
        {
            get
            {
                if (string.IsNullOrEmpty(dllguid))
                {
                    foreach (byte b in _infoguidbytes)
                    {
                        dllguid += _guidHexs[(int)b].ToString();
                    }
                }
                return dllguid;
            }
        }
    }
}
