using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
namespace MsgInfo
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class MsgInfo
    {
         private sProtocolType _ProtocolType;
         private string _SendFromId;
         private string _netUserID="";
         private string _SendToId="";
        private string _SendTo = "";
         private string _SendInfo="";
         private string _infoGuid="";
         private string _ver="NetTalk";
         private bool _isNet = false;
         private string _infoType = "";
        public MsgInfo() { }
        public MsgInfo(MsgInfo oldinfo)
        {
            _ProtocolType = oldinfo.ProtocolType;
            _SendFromId = oldinfo.SendFromID;
            _netUserID = oldinfo.NetUserID;
            _SendToId = oldinfo.SendToID;
            _SendInfo = oldinfo.SendInfo;
            _infoGuid = oldinfo.InfoGuid;
            _ver = oldinfo.Ver;
            _isNet = oldinfo.IsNet;
            _infoType = oldinfo.InfoType;
            _SendTo = oldinfo.SendTo;
        }
        public MsgInfo(MsgInfo oldinfo,string sendto)
        {
            _ProtocolType = oldinfo.ProtocolType;
            _SendFromId = oldinfo.SendFromID;
            _netUserID = oldinfo.NetUserID;
            _SendToId = oldinfo.SendToID;
            _SendInfo = oldinfo.SendInfo;
            _infoGuid = oldinfo.InfoGuid;
            _ver = oldinfo.Ver;
            _isNet = oldinfo.IsNet;
            _infoType = oldinfo.InfoType;
            _SendTo = sendto;
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string InfoType
         {
             get { return _infoType; }
             set { _infoType = value; }
         }
        /// <summary>
        /// 是否是外网消息
        /// </summary>
        public bool IsNet
        {
            set { _isNet = value; }
            get { return _isNet; }
        }
        /// <summary>
        /// 如果是外网用户则有此ID
        /// </summary>
        public string NetUserID
        {
            set { _netUserID = value; }
            get { return _netUserID; }
        }
        /// <summary>
        /// 程序标识
        /// </summary>
         public string InfoGuid
         {
             get { return _infoGuid; }
             set { _infoGuid = value; }
         }
        /// <summary>
        /// 消息类型
        /// </summary>
        public enum sProtocolType
        {
            MSGTYPE_STATE=1,
            MSGTYPE_STATE_HEADER=28,
            MSGTYPE_LOGOUT = 2,
            MSGTYPE_GET_MEMBER_LIST = 3,
            MSGTYPE_SEND_MEMBER_LIST = 4,
            MSGTYPE_SEND_TEXT = 5,
            MSGTYPE_SEND_FILE = 6,
            MSGTYPE_SEND_DIR = 7,
            MSGTYPE_SEND_IMG = 8,
            MSGTYPE_GET_FILE=9,
            MSGTYPE_GET_FILEPACK=10,
            MSGTYPE_GET_IMGPACK=11,
            MSGTYPE_NETLOGIN=12,
            MSGTYPE_NETLOGIN_ERR=13,
            MSGTYPE_NETLOGIN_OK=14,
            MSGTYPE_NETUSER_HELLO=15,
            MSGTYPE_SERVER_HELLO=16,
            MSGTYPE_VIDEO_REQUEST=17,
            MSGTYPE_VIDEO_ACCEPT=18,
            MSGTYPE_VIDEO_MSG=19,
            MSGTYPE_SOUND_REQUEST=20,
            MSGTYPE_SOUND_ACCEPT=21,
            MSGTYPE_SOUND_MSG=22,
            MSGTYPE_SERVER_LOGIN=23,
            MSGTYPE_SERVER_LOGIN_OK=24,
            MSGTYPE_SERVER_LOGIN_ERR=25,
            MSGTYPE_MAINSERVER_HELLO=26,
            MSGTYPE_LEVELSERVER_HELLO=27
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void init()
        {
            this._ProtocolType = 0;
            this._SendFromId = null;
            this._SendInfo = null;
            this._SendToId = null;
        }
        /// <summary>
        /// 包类型
        /// </summary>
        public sProtocolType ProtocolType
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
        public string SendFromID
        {
            get
            { return this._SendFromId; }
            set
            { this._SendFromId = value; }
        }
        /// <summary>
        /// 接收者ＩＰ
        /// </summary>
        public string SendToID
        {
            set { this._SendToId = value; }
            get { return this._SendToId; }
        }
        /// <summary>
        /// 唯一接收者
        /// </summary>
        public string SendTo
        {
            set { this._SendTo = value; }
            get { return this._SendTo; }
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
       
        /// <summary>
        /// 客户端版本
        /// </summary>
        public string Ver
        {
            get { return _ver; }
            set { _ver = value; }
        }
    }
}
