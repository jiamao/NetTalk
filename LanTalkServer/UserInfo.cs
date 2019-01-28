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
        /// 初始化
        /// </summary>
        public void init()
        {
            this._ProtocolType = 0;
            this._SendFromIp = null;
            this._SendInfo = null;
            this._SendToIp = null;
        }
        /// <summary>
        /// 包类型
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
        /// 发送者ＩＰ
        /// </summary>
        public byte[] SendFromIp
        {
            get
            { return this._SendFromIp; }
            set
            { this._SendFromIp = value; }
        }
        /// <summary>
        /// 接收者ＩＰ
        /// </summary>
        public byte[] SendToIp
        {
            set { this._SendToIp = value; }
            get { return this._SendToIp; }
        }
        /// <summary>
        /// 消息内容
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
