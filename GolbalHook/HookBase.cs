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
        public delegate IntPtr HookPro(int nCode, IntPtr wParam, IntPtr lParam);  //����ί�У����лص�
        static IntPtr hHook = IntPtr.Zero;  //�������ӱ�� 
        const int WH_KEYBOARD_LL = 13;  //�������̹�������

        GCHandle _hookProcHandle;

        public delegate void keypresscode(int keycode, int modifierkeys);//��������ί�У�
        public keypresscode curkeypresscode;
        public HookBase()
        {
            throw new Exception("��Ҫ��ȷ�ı�ʶ�����ô���!");
        }
        public HookBase(string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("��Ҫ��ȷ�ı�ʶ�����ô���!");
        }
       /// <summary>
       /// ���ӻص�
       /// </summary>
       /// <param name="nCode"></param>
       /// <param name="wParam"></param>
       /// <param name="lParam"></param>
       /// <returns></returns>
        public IntPtr KEYBOARD_HOOKPRO(int nCode, IntPtr wParam, IntPtr lParam)
        {
            KBDLLHOOKSTRUCT kb = new KBDLLHOOKSTRUCT();
            HookApi.CopyMemory(ref kb, lParam, 20);      //�������������

            UNLOAD_WINDOWS_KETBOARD_HOOK();
            SET_WINDOWS_KEYBOARD_HOOK();
            curkeypresscode(kb.vkCode, (int)Control.ModifierKeys);
            return HookApi.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// ���ù���
        /// </summary>
        public void SET_WINDOWS_KEYBOARD_HOOK()
        {
            if (hHook == IntPtr.Zero)
            {
                HookPro hk = new HookPro(this.KEYBOARD_HOOKPRO);
                _hookProcHandle = GCHandle.Alloc(hk);
                //�ҹ���
                hHook = HookApi.SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    hk,
                    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                if (hHook == IntPtr.Zero)
                {
                    throw new Exception("��װ���Ӳ��ɹ���");  // �ҹ��Ӳ��ɹ�����ֵ 0
                }
            }
        }

       /// <summary>
       /// ж�ع���
       /// </summary>
        public void UNLOAD_WINDOWS_KETBOARD_HOOK()
        {
            if (hHook != IntPtr.Zero)
            {
                //��������Ѿ�������ȡ�����ӣ�������ȡ��
                HookApi.UnhookWindowsHookEx(hHook);
                _hookProcHandle.Free();
                hHook = IntPtr.Zero;
            }
        }

        //����Hook�ṹ����
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
