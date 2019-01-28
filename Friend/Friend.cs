using System;
using System.Collections.Generic;
using System.Text;

namespace Friend
{
    [Serializable]
    public class Friend
    {
        byte[] _ip = new byte[4];
        string _ID="";
        /// <summary>
        /// 好友标识
        /// </summary>
        public string ID
        {
            get 
            {
                if (_isNet || !string.IsNullOrEmpty(_ID))
                {
                    return _ID;
                }
                return _ip[0].ToString() + "." + _ip[1].ToString() + "." + _ip[2].ToString() + "." + _ip[3].ToString(); 
            }
            set { _ID = value; }
        }
        string _serverID = "";
        /// <summary>
        /// 好友所属的服务器
        /// </summary>
        public string ServerID
        {
            set { _serverID = value; }
            get { return _serverID; }
        }
        /// <summary>
        /// 好友IP
        /// </summary>
        public byte[] Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        int _port = 9050;
        /// <summary>
        /// 好友端口
        /// </summary>
        public int Port
        {
            set { _port = value; }
            get { return _port; }
        }
        bool _isNet = false;
        /// <summary>
        /// 是否是外网用户
        /// </summary>
        public bool IsNet
        {
            set { _isNet = value; }
            get { return _isNet; }
        }
        DateTime _lastShowTime=DateTime.Now;
        /// <summary>
        /// 最后一次联系时间
        /// </summary>
        public DateTime LastShowTime
        {
            set { _lastShowTime = value; }
            get { return _lastShowTime; }
        }
        string _name;
        /// <summary>
        /// 好友名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        string _groupName;
        /// <summary>
        /// 所在组
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }
        string _header;
        /// <summary>
        /// 头像索引名
        /// </summary>
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        System.Drawing.Image _imgHeader;
        /// <summary>
        /// 头像图像
        /// </summary>
        public System.Drawing.Image IMGHeader
        {
            set { _imgHeader = value; }
            get { return _imgHeader; }
        }
        List<MsgInfo.MsgInfo> _messages = new List<MsgInfo.MsgInfo>();
        /// <summary>
        /// 收到此好友的消息队列
        /// </summary>
        public List<MsgInfo.MsgInfo> Messages
        {
            get { try { return _messages; } catch { return _messages=new List<MsgInfo.MsgInfo>(); } }
            set { _messages = value; }
        }
        List<MsgInfo.MsgInfo> _fileList = new List<MsgInfo.MsgInfo>();
        /// <summary>
        /// 要发送的文件队列
        /// </summary>
        public List<MsgInfo.MsgInfo> FileList
        {
            get { return _fileList; }
            set { _fileList = value; }
        }

        string _ver;
        /// <summary>
        /// 客户端版本
        /// </summary>
        public string Ver
        {
            get { return _ver; }
            set { _ver = value; }
        }
        EState _state=EState.InLine;
        /// <summary>
        /// 当前状态
        /// </summary>
        public EState State
        {
            get { return _state; }
            set { _state = value; }
        }
       
        public enum EState
        {
            Busy,
            Out,
            BSYou,
            InLine,
            OutLine
        }
    }
    /// <summary>
    /// 服务器对象
    /// </summary>
    [Serializable]
    public class SERVER
    {
        string _id = "";
        string _addr = "haofefe.gicp.net";
        int _port = 60000;
        DateTime _lastconnecttiem;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string ADDR
        {
            get { return _addr; }
            set { _addr = value; }
        }
        public int PORT
        {
            set { _port=value; }
            get { return _port; }
        }
        public DateTime LASTCONTIME
        {
            set { _lastconnecttiem = value; }
            get { return _lastconnecttiem; }
        }
    }
}
