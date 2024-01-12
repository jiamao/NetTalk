using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace WinLanMsg.Util
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class UserInfo
    {    
         public const byte MSGTYPE_LOGIN = 1;
         public const byte MSGTYPE_LOGOUT = 2;
         public const byte MSGTYPE_GET_MEMBER_LIST = 3;
         public const byte MSGTYPE_SEND_MEMBER_LIST = 4;
         public const byte MSGTYPE_SEND_TEXT = 5;
         public const byte MSGTYPE_SEND_FILE = 6;
         public const byte MSGTYPE_SEND_DIR = 7;
         private byte _ProtocolType;
         private byte[] _SendFromIp;
         private byte[] _SendToIp;
         private string _SendInfo;
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void init()
        {
            this._ProtocolType = 0;
            this._SendFromIp = null;
            this._SendInfo = null;
            this._SendToIp = null;
        }
        /// <summary>
        /// ������
        /// </summary>
        public byte ProtocolType
        {
            get
            {
                return this._ProtocolType;
            }
            set
            {
                this._ProtocolType = value;
            }
        }
        /// <summary>
        /// �����ߣɣ�
        /// </summary>
        public byte[] SendFromIp
        {
            get
            { return this._SendFromIp; }
            set
            { this._SendFromIp = value; }
        }
        /// <summary>
        /// �����ߣɣ�
        /// </summary>
        public byte[] SendToIp
        {
            set { this._SendToIp = value; }
            get { return this._SendToIp; }
        }
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string SendInfo
        {
            get
            { return this._SendInfo; }
            set
            {
                this._SendInfo = value;
            }
        }        
    }
}
