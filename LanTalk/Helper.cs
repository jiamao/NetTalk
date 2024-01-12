using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Reflection;
using System.Drawing;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Windows.Forms;
using _poolFactory = MsgPoolFactory.Factory;//消息与好友缓存
using WinLanMsg.Util;
using objectToMsg;

namespace LanTalk
{
    class Helper
    {
        static byte[] localhostip;
        static Icon _defaultIcon;
        static bool _haveMsg = false;
        static Thread threadcheckmsg;
        static string _myName="丁峰峰";
        static string _myGroupName = "7MULTIPLE";
        static string _myHeader = "";
        static int _iflag = 0;        
        static byte[] _curfriendIP;
        static bool _oldver = true;//是否兼容旧版本
        static string _face;
        static int _threadCount = 1;
        static bool _downLoading = false;
        static Friend.Friend.EState mystate = Friend.Friend.EState.InLine;
        static string _historypath;
        static string _sendmsgButton = "enter";
        static string _serverAddr = "192.168.1.28";
        static int _lanPort = 9050;
        static int _serverPort = 9050;
        static bool _isNET = false;
        static string _myNetID = "net.id";
        public static string CurVer = "2.0";
        public static turnMsg _turnmsg;
        public static FileCompress.GZip gzpi;
        public static DateTime LastConnectServerTime;
        public static int ConnectServerTimeOut = -180;//与服务器连接超时秒数
        public static string SendmsgButton
        {
            get { return Helper._sendmsgButton; }
            set { Helper._sendmsgButton = value; }
        }
        public static Icon DefaultIcon
        {
            get
            {
                return _defaultIcon;
            }
            set
            {
                _defaultIcon = value;
            }
        }
        /// <summary>
        /// 聊天记录存放路径
        /// </summary>
        public static string Historypath
        {
            get 
            {
                //生成此次聊天记录的路径
                _historypath = Path.Combine(Application.StartupPath, "history\\" + DateTime.Now.ToLongDateString() + "\\0001.rtf");
                if (!Directory.Exists(Path.Combine(Application.StartupPath, "history\\" + DateTime.Now.ToLongDateString()))) Directory.CreateDirectory(Path.Combine(Application.StartupPath, "history\\" + DateTime.Now.ToLongDateString()));
                int historyindex = 2;
                while (true)
                {
                    if (File.Exists(_historypath))
                    {
                        _historypath = Path.Combine(Application.StartupPath, "history\\" + DateTime.Now.ToLongDateString() + "\\" + historyindex.ToString("0000") + ".rtf");
                        historyindex++;
                    }
                    else
                    {
                        break;
                    }
                }
                return Helper._historypath; 
            }
            set { Helper._historypath = value; }
        }

        public static Friend.Friend.EState Mystate
        {
            get { return Helper.mystate; }
            set { Helper.mystate = value; }
        }

        public static bool DownLoading
        {
            get { return Helper._downLoading; }
            set { Helper._downLoading = value; }
        }
        public static int ThreadCount
        {
            get { return Helper._threadCount; }
            set { Helper._threadCount = value; }
        }

        public static string Face
        {
            get { return Helper._face; }
            set { Helper._face = value; }
        }
        public static bool Oldver
        {
            get { return Helper._oldver; }
            set { Helper._oldver = value; }
        }
        //消息标识
        static string _guidHexs = "0123456789ABCDEF-";
        static byte[] _infoguidbytes = new byte[] { 7, 13, 0, 7, 2, 5, 11, 8, 16, 7, 10, 0, 3, 16, 4, 9, 5, 15, 16,
            11, 7, 12, 13, 16, 15, 0, 11, 6, 1, 13, 15, 15, 6, 7, 12, 1 };
        static string infoguid="";
        public static string _guid
        {
            get
            {
                if (string.IsNullOrEmpty(infoguid))
                {
                    foreach (byte b in _infoguidbytes)
                    {
                        infoguid += _guidHexs[(int)b].ToString();
                    }
                }
                return infoguid;
            }
        }

        public static MsgListener.MsgListener listener;
        public static MsgSend.MsgSend _send;

        #region 属性
        public static string MyName
        {
            get { return _myName; }
            set { _myName = value; }
        }
        public static string MyGroupName
        {
            get { return _myGroupName; }
            set { _myGroupName = value; }
        }
        public static string MyHeader
        {
            get { return _myHeader; }
            set { _myHeader = value; }
        }
        /// <summary>
        /// 外网登陆ID
        /// </summary>
        public static string MyNetID
        {
            get { return Helper._myNetID; }
            set { Helper._myNetID = value; }
        }

        /// <summary>
        /// 当前所有好友
        /// </summary>
        public static List<Friend.Friend> Friends
        {
            get { return _poolFactory.Friends; }
            set { _poolFactory.Friends = value; }
        }

        /// <summary>
        /// 线程退出标记
        /// </summary>
        public static int Iflag
        {
            get { return Helper._iflag; }
            set { Helper._iflag = value; }
        }
        /// <summary>
        /// 是否有消息未处理
        /// </summary>
        public static bool HaveMsg
        {
            get { return _poolFactory.Messages.Count > 0; }            
        }
        static ImageList _listUserHeader = new ImageList();
        /// <summary>
        /// 用户头像集合
        /// </summary>
        public static ImageList ListUserHeader
        {
            get { return Helper._listUserHeader; }
            set { Helper._listUserHeader = value; }
        }
        static ImageList _listUserFace = new ImageList();
        /// <summary>
        /// 用户发送图标集合
        /// </summary>
        public static ImageList ListUserFace
        {
            get { return Helper._listUserFace; }
            set { Helper._listUserFace = value; }
        }
        /// <summary>
        /// 本机ＩＰ
        /// </summary>
        public static byte[] selfIP
        {
            get {
                if (localhostip == null)
                {
                    IPAddress[] ips = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                    if (!_isNET && ips.Length > 1)
                    {
                        foreach (IPAddress ipa in ips)
                        {
                            if (checkRightIP(ipa))
                            {
                                localhostip = getByteIP(ipa.ToString());
                                return localhostip;
                            }
                        }
                        Formselectip selip = new Formselectip();
                        selip.Ips = ips;
                        selip.ShowDialog();
                        localhostip = getByteIP(selip.Selectip);
                    }
                    else
                    {
                        localhostip = getByteIP(ips[0].ToString());
                    }
                }
                return localhostip;
            }
        }
        private static bool checkRightIP(IPAddress ip)
        {
            try
            {
                byte[] checkipbs = Guid.NewGuid().ToByteArray();
                _send.SendBytes(checkipbs, ip.ToString());
                Thread.Sleep(100);
                if (_poolFactory.Messages.Count > 0)
                {
                    for (int i = 0; i < _poolFactory.Messages.Count; i++)
                    {
                        if (_poolFactory.Messages[i].UserMsg.Length == checkipbs.Length && checkBS(_poolFactory.Messages[i].UserMsg, checkipbs))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                _poolFactory.Messages.Clear();
            }
        }
        private static bool checkBS(byte[] sourbs, byte[] checkbs)
        {
            if (sourbs.Length == checkbs.Length)
            {
                int index = 0;
                foreach (byte b in sourbs)
                {
                    if (b != checkbs[index])
                    {
                        return false;
                    }
                    index++;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 本地通信端口
        /// </summary>
        public static int LanPort
        {
            set { _lanPort = value; }
            get { return _lanPort; }
        }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string ServerAddr
        {
            set { _serverAddr = value; }
            get { return _serverAddr; }
        }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public static int ServerPort
        {
            set { _serverPort = value; }
            get { return _serverPort; }
        }
        /// <summary>
        /// 是否为外网用户
        /// </summary>
        public static bool IsNet
        {
            set { _isNET = value; }
            get { return _isNET; }
        }
        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        public static void init()
        {
            try
            {
                if (_turnmsg == null) _turnmsg = new turnMsg(_guid);
                if (listener == null) listener = new MsgListener.MsgListener(_lanPort,_guid);
                if (gzpi == null) gzpi = new FileCompress.GZip(_guid);
                if (_send == null) _send = new MsgSend.MsgSend(_lanPort,_guid);
                try
                {
                    if (File.Exists(Path.Combine(Application.StartupPath, "myself.dat")))
                    {
                        Friend.Friend myself = (Friend.Friend)readFileToObject(Path.Combine(Application.StartupPath, "myself.dat"));
                        MyGroupName = myself.GroupName;
                        MyName = myself.Name;
                        MyHeader = myself.Header;
                    }
                }
                catch
                { }
                try
                {
                    _oldver = bool.Parse(getConfigByName("LanMsg", _oldver.ToString()));
                }
                catch
                { }

                _sendmsgButton = getConfigByName("SendButton", _sendmsgButton.ToString()).ToLower();
                try
                {
                    _threadCount = int.Parse(getConfigByName("ThreadCount", _threadCount.ToString()));
                }
                catch
                { }
                _threadCount = _threadCount > 10 ? 10 : _threadCount;
                _threadCount = _threadCount < 1 ? 1 : _threadCount;
                try
                {
                    _isNET = bool.Parse(getConfigByName("ISNET", _oldver.ToString()));
                }
                catch { }

                _serverAddr = getConfigByName("ServerAddr", _serverAddr);

                try
                {
                    _serverPort = int.Parse(getConfigByName("ServerPort", _serverPort.ToString()));
                }
                catch
                {
                    if(IsNet) MessageBox.Show("请设置正确的服务器端口!");
                }
                _send.NetSendPortChanged += listener.resetNetListen;
                listener.getMsg += addMsg;//获取消息
                listener.getFile += addFile;//接收文件
                //如果是内网则初始化本地监听
                if (!_isNET)
                {
                    listener.init();
                }
                byte[] myip = Helper.selfIP;

                MsgHelper.init();
                writeLog("启动成功");
            }
            catch (Exception ex)
            {
                writeLog("Helper.init " + ex.Message);
                throw ex;
            }

        }
        public static void initmyself()
        {
            Friend.Friend myself = new Friend.Friend();
            myself.Header = _myHeader;
            myself.GroupName = _myGroupName;
            myself.Ip = selfIP;
            if(_isNET)myself.ID = _myNetID;
            myself.IsNet = _isNET;
            myself.Name = _myName;
            myself.Ver = "NetTalk";
            changeState((int)mystate);
            MsgHelper.addUIFriend(myself);
            _poolFactory.resetFriend(myself);
        }
        public static void initForm()
        {
            try
            {
                if (Helper.Face.Equals("qq"))
                {
                    Program.formmain = new formMain();
                    MsgHelper.GroupBar = Program.formmain.allFriends;
                    MsgHelper.GroupList.Clear();
                    Program.formmain.allFriends.refreshFriends += Helper.getListFriends;
                    Program.formmain.Show();
                    _defaultIcon = Program.formmain.Icon;
                }
                else
                {
                    Program.formlist = new formList();
                    Program.formlist.Show();
                    _defaultIcon = Program.formlist.Icon;
                }
            }
            catch(Exception ex)
            {
                writeLog("helper.initform " + ex.ToString());
            }
            if (threadcheckmsg == null)
            {
                threadcheckmsg = new Thread(new ThreadStart(ThreadCheckMSG));
                threadcheckmsg.IsBackground = true;
                threadcheckmsg.Start();
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        public static void exitme()
        {
            _iflag = 1;
            if (listener!=null) listener.stop();
            LogOut();
            _poolFactory.clear();
            GC.Collect();
        }
        /// <summary>
        /// 登入
        /// </summary>
        public static void Login()
        {
            if (_isNET) return;
            sendLogin(IPAddress.Broadcast.ToString(),Helper.mystate);
        }
        /// <summary>
        /// 登入
        /// </summary>
        public static void Login(string usreid,string userpwd)
        {
            MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
            msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN;
            msginfo.SendFromID = usreid;
            msginfo.SendToID = _serverAddr;
            msginfo.SendTo = _serverAddr;
            msginfo.NetUserID = usreid;
            msginfo.IsNet = _isNET;
            msginfo.InfoGuid = _guid;
            msginfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP) + "," + _myHeader + "," + ((int)mystate).ToString() + "," + usreid + "," + serPWD(userpwd);
            _poolFactory.addSendInfo(msginfo,  _serverAddr ,_serverPort);
            _myNetID = usreid;
        }
        /// <summary>
        /// 登出
        /// </summary>
        public static void LogOut()
        {
            sendLoginOut(IPAddress.Broadcast.ToString());
        }
        private static void LoginFriend(MsgInfo.MsgInfo info)
        {
            try
            {
                if (!_isNET && !info.IsNet && _oldver && mystate != Friend.Friend.EState.OutLine)//对旧版本的兼容
                {
                    WinLanMsg.Util.UserInfo userinfo = new UserInfo();
                    userinfo.ProtocolType = 1;
                    userinfo.SendFromIp = selfIP;
                    userinfo.SendToIp = getByteIP(info.SendFromID);
                    userinfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP);                    
                    _poolFactory.addSendInfo(userinfo, new string[] { info.SendFromID });
                }
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
                msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                msginfo.IsNet = Helper.IsNet;
                msginfo.NetUserID = Helper.MyNetID;
                if (msginfo.IsNet)
                {
                    msginfo.SendFromID = Helper.MyNetID;
                }
                else
                {
                    msginfo.SendFromID = Helper.getIP(Helper.selfIP);
                }
                msginfo.SendToID = info.SendFromID;               
                msginfo.InfoGuid = _guid;
                msginfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP) + "," + _myHeader + "," + (int)(mystate);
                if (info.IsNet)
                {
                    _poolFactory.addSendInfo(new MsgInfo.MsgInfo(msginfo,info.NetUserID), new string[] { info.NetUserID });
                }
                else
                {
                    _poolFactory.addSendInfo(new MsgInfo.MsgInfo(msginfo, info.SendFromID), new string[] { info.SendFromID });
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 发送登陆消息
        /// </summary>
        /// <param name="ip"></param>
        private static void sendLogin(string ip,Friend.Friend.EState state)
        {
            try
            {
                if (_isNET) return;
                if (_oldver && state != Friend.Friend.EState.OutLine)//对旧版本的兼容
                {
                    WinLanMsg.Util.UserInfo userinfo = new UserInfo();
                    userinfo.ProtocolType = 1;
                    userinfo.SendFromIp = selfIP;
                    userinfo.SendToIp = getByteIP(ip);
                    userinfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP);

                    _poolFactory.addSendInfo(userinfo, new string[] { ip });
                    userinfo = new UserInfo();
                    userinfo.ProtocolType = 3;
                    userinfo.SendFromIp = selfIP;
                    userinfo.SendToIp = getByteIP(ip);
                    userinfo.SendInfo = "";

                    _poolFactory.addSendInfo(userinfo, new string[] { ip });
                }
                else if (_oldver && state == Friend.Friend.EState.OutLine)
                {
                    WinLanMsg.Util.UserInfo userinfo = new UserInfo();
                    userinfo.ProtocolType = 2;
                    userinfo.SendFromIp = selfIP;
                    userinfo.SendToIp = getByteIP(ip);
                    userinfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP);
                    
                    _poolFactory.addSendInfo(userinfo, new string[] { ip });
                }
                Thread.Sleep(50);
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
                msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                msginfo.Ver = "NetTalk";
                msginfo.SendFromID = getIP(selfIP);
                msginfo.SendToID = ip;
                msginfo.InfoGuid = _guid;
                msginfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP) + "," + _myHeader + "," + ((int)state).ToString();
                msginfo.SendTo = ip;
                _poolFactory.addSendInfo(new MsgInfo.MsgInfo(msginfo,ip), new string[] { ip });
                
                msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_MEMBER_LIST;

                _poolFactory.addSendInfo(new MsgInfo.MsgInfo(msginfo, ip), new string[] { ip });
            }
            catch (Exception ex)
            {
                writeLog("helper.sendlogin "+ex.Message);
            }
        }
        public static void sendChangeState(string ip, Friend.Friend.EState state)
        {
            if (!_isNET && _oldver && state != Friend.Friend.EState.OutLine)//对旧版本的兼容
            {
                WinLanMsg.Util.UserInfo userinfo = new UserInfo();
                userinfo.ProtocolType = 1;
                userinfo.SendFromIp = selfIP;
                userinfo.SendToIp = getByteIP(ip);
                userinfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP);                
                _poolFactory.addSendInfo(userinfo, new string[] { ip });
            }
            else if (!_isNET && _oldver && state == Friend.Friend.EState.OutLine)
            {
                WinLanMsg.Util.UserInfo userinfo = new UserInfo();
                userinfo.ProtocolType = 2;
                userinfo.SendFromIp = selfIP;
                userinfo.SendToIp = getByteIP(ip);
                userinfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP);
                _send.SendBytes(ojectToByteArray(userinfo), ip);
                _poolFactory.addSendInfo(userinfo, new string[] { ip });
            }

            MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
            msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;            
            msginfo.SendToID = ip;
            msginfo.InfoGuid = _guid;
            msginfo.IsNet = _isNET;
            msginfo.Ver = "NetTalk";
            msginfo.SendTo = ip;
            msginfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP) + "," + _myHeader + "," + ((int)state).ToString();
            if (_isNET)
            {
                msginfo.IsNet = _isNET;
                msginfo.NetUserID = _myNetID;
                msginfo.SendFromID = _myNetID;
                _poolFactory.addSendInfo(new MsgInfo.MsgInfo(msginfo, _serverAddr), _serverAddr, _serverPort,true);
                return;
            }
            
            msginfo.SendFromID = getIP(selfIP);
            _poolFactory.addSendInfo(new MsgInfo.MsgInfo(msginfo, ip), new string[] { ip });
        }
        /// <summary>
        /// 改变登陆状态
        /// </summary>
        /// <param name="istate"></param>
        public static void changeState(int istate)
        {
            try
            {
                mystate=(Friend.Friend.EState)istate;
                sendChangeState(IPAddress.Broadcast.ToString(), mystate);
            }
            catch
            { }
        }
        /// <summary>
        /// 发送登出消息
        /// </summary>
        /// <param name="ip"></param>
        public static void sendLoginOut(string ip)
        {
            try
            {
                if (!_isNET && _oldver)//对旧版本的兼容
                {
                    WinLanMsg.Util.UserInfo userinfo = new UserInfo();
                    userinfo.ProtocolType = 2;
                    userinfo.SendFromIp = selfIP;
                    userinfo.SendToIp = getByteIP(ip);
                    userinfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP);
                    _send.SendBytes(ojectToByteArray(userinfo), ip);

                }
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
                msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_LOGOUT;
                msginfo.SendFromID = getIP(selfIP);
                msginfo.SendToID = ip;
                msginfo.SendTo = ip;
                msginfo.InfoGuid = _guid;
                msginfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP) + "," + _myHeader;
                if (_isNET)
                {
                    msginfo.SendFromID =_myNetID;
                    msginfo.NetUserID = _myNetID;
                    msginfo.IsNet = _isNET;                    
                    _send.SendBytes(ojectToByteArray(msginfo), _serverAddr,_serverPort);
                    return;
                }
                _send.SendBytes(ojectToByteArray(msginfo), ip);
                //_poolFactory.addSendInfo(msginfo, new string[] { ip });


            }
            catch (Exception ex)
            {

            }
        }
        public static void getListFriends()
        {
            MsgHelper.GroupList.Clear();
            if (_face.Equals("qq"))
            {
                Program.formmain.allFriends.Bands.Clear();
            }
            if(!_isNET)
            initmyself();
            try
            {
                if (!_isNET && _oldver && mystate != Friend.Friend.EState.OutLine)//对旧版本的兼容
                {
                    WinLanMsg.Util.UserInfo userinfo;
                    userinfo = new UserInfo();
                    userinfo.ProtocolType = 3;
                    userinfo.SendFromIp = selfIP;
                    userinfo.SendToIp = getByteIP(IPAddress.Broadcast.ToString());
                    userinfo.SendInfo = "";
                    _poolFactory.addSendInfo(userinfo, new string[] { IPAddress.Broadcast.ToString() });
                }
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();                
                msginfo.SendFromID = getIP(selfIP);
                msginfo.NetUserID = _myNetID;
                msginfo.SendToID = IPAddress.Broadcast.ToString();
                msginfo.IsNet = _isNET;
                msginfo.InfoGuid = _guid;
                msginfo.Ver = "NetTalk";
                msginfo.SendTo = IPAddress.Broadcast.ToString();
                msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_MEMBER_LIST;
                if (_isNET)
                {
                    msginfo.SendFromID = _myNetID;                    
                    msginfo.SendTo = msginfo.SendToID = _serverAddr;
                   // msginfo.SendInfo = MyName + "," + MyGroupName + "," + _myNetID + "," + _myHeader + "," + ((int)mystate).ToString();                    
                }
                //else
                //{
                //    //msginfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP) + "," + _myHeader + "," + ((int)mystate).ToString();
                //}
                _poolFactory.addSendInfo(msginfo, new string[] { IPAddress.Broadcast.ToString() });
            }
            catch (Exception ex)
            {
                writeLog("helper.sendlogin " + ex.Message);
            }
        }
        /// <summary>
        /// 发送登陆消息
        /// </summary>
        /// <param name="ip"></param>
        private static void sendLogin(byte[] ip)
        {
            try
            {
                sendLogin(getIP(ip),Friend.Friend.EState.InLine);
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 审核消息
        /// </summary>
        public static void checkMSG()
        {
            try
            {
                if (_poolFactory.ReceivedTextMsg.Count > 0)
                {
                    for (int i = 0; i < _poolFactory.ReceivedTextMsg.Count; i++)
                    {
                        showMsg(_poolFactory.ReceivedTextMsg[i].Info, _poolFactory.ReceivedTextMsg[i].IEP);
                    }
                    _poolFactory.removeReaded();
                }
                Thread.Sleep(50);
                //定时清理已发送的消息
                if (_poolFactory.SendedMsgImpLife.Count > 0)
                {
                    List<string> guids = new List<string>();
                    DateTime imgdt;
                    foreach (string guid in _poolFactory.SendedMsgImpLife.Keys)
                    {

                        try
                        {
                            imgdt = ((DateTime)_poolFactory.SendedMsgImpLife[guid]);
                        }
                        catch
                        {
                            guids.Add(guid);
                            continue;
                        }
                        if (imgdt < DateTime.Now)//如果超过一分钟
                        {
                            _poolFactory.removeSendedImgsByGuid(guid);//移除超时的消息
                            guids.Add(guid);
                        }
                    }
                    foreach (string guid in guids)
                    {
                        _poolFactory.SendedMsgImpLife.Remove(guid);
                    }
                    guids.Clear();
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                writeLog("helper.chekcmsg " + ex.ToString());
            }
        }
        public static void ThreadCheckMSG()
        {
            while (_iflag == 0)
            {
                try
                {
                    if (HaveMsg)
                    {
                        int count = 0;
                        for (int i = 0; i < _poolFactory.Messages.Count;i++ )
                        {
                            count++;
                            if (_poolFactory.Messages[i] == null) continue;
                            MsgInfo.MsgInfo msginfo = byteArrayToObject(_poolFactory.Messages[i].UserMsg);                            
                            if (msginfo == null) continue;
                            THshowMsg(msginfo,_poolFactory.Messages[i].UserIpEndPoint);
                            _poolFactory.Messages[i] = null;
                        }
                        _poolFactory.removeAllMsg(count);//从消息队列中移除所有        
                    }                    
                    Thread.Sleep(100);
                    
                }
                catch (Exception ex)
                {
                    writeLog("helper.ThreadCheckMSG"+ex.Message);
                }
            }
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="msg"></param>
        private static void showMsg(MsgInfo.MsgInfo msg,IPEndPoint iep)
        {
            if (string.IsNullOrEmpty(msg.SendFromID) || msg.SendToID==null) return;//如何为恶意消息，直接放弃
            try
            {
                switch (msg.ProtocolType)
                {
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE://出现新好友
                        {
                            Friend.Friend f = _poolFactory.findFriend(msg.IsNet?msg.NetUserID:msg.SendFromID);
                            if (f != null && msg.Ver.ToLower().Equals("lanmsg")) return;
                            Friend.Friend friend = new Friend.Friend();
                            friend.ID = msg.IsNet ? msg.NetUserID : msg.SendFromID;
                            if (_isNET)
                            {
                                friend.Ip = getByteIP(iep.Address.ToString());
                                friend.Port = _serverPort;
                            }
                            else
                            {
                                friend.Ip = getByteIP(iep.Address.ToString());
                                friend.Port = _lanPort;
                            }
                            friend.GroupName = "未知组";
                            friend.Name = "没名字的猪";
                            string[] friendinfo = (string.IsNullOrEmpty(msg.SendInfo) ? "" : msg.SendInfo).Split(',');
                            for (int i = 0; i < friendinfo.Length; i++)
                            {
                                if (i == 0) friend.Name = friendinfo[i];//名称
                                if (i == 1) friend.GroupName = friendinfo[i];//组群
                                if (i == 3) friend.Header = friendinfo[i];//头像标识
                                if (i == 4) friend.State = (Friend.Friend.EState)int.Parse(friendinfo[i]);//状态
                            }
                            friend.Header = (friend.Header == null ? "" : friend.Header);
                            friend.Ver = msg.Ver;
                            friend.IsNet = msg.IsNet;
                            //如果是外网用户
                            if (msg.IsNet)
                            {
                                friend.ID = msg.NetUserID;
                            }

                            if (friend.State == Friend.Friend.EState.OutLine)
                            {
                                MsgHelper.outUIFriend(friend);
                            }
                            else
                            {
                                MsgHelper.addUIFriend(friend);
                            }
                            _poolFactory.resetFriend(friend);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_ERR://,外网登录失败
                        {
                            _poolFactory.Msg.Add(msg);                            
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_OK://外网登陆成功
                        {
                            _poolFactory.Msg.Add(msg);
                            MsgHelper.netlogined = true;
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_LOGOUT://登出
                        {
                            Friend.Friend friend = msg.IsNet?_poolFactory.findFriend(msg.NetUserID):_poolFactory.findFriend(msg.SendFromID);
                            if (friend == null) return;
                            MsgHelper.outUIFriend(friend);
                            _poolFactory.removeFriend(friend);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT://消息
                        {
                            Friend.Friend friend = msg.IsNet ? _poolFactory.findFriend(msg.NetUserID) : _poolFactory.findFriend(msg.SendFromID);

                            if (friend == null)
                            {
                                sendLogin(msg.SendFromID, mystate);
                                friend = new Friend.Friend();
                                friend.ID = msg.IsNet ? msg.NetUserID : msg.SendFromID;
                                if (_isNET)
                                {
                                    friend.Ip = getByteIP(_serverAddr);
                                }
                                else
                                {
                                    friend.Ip = getByteIP(iep.Address.ToString());
                                    friend.Port = msg.IsNet ? iep.Port : _lanPort;
                                }
                                friend.Name = getIP(friend.Ip);
                                friend.GroupName = "未知组";
                                friend.Messages.Add(msg);
                                friend.Ver = msg.Ver;
                                friend.Header = "";
                                friend.IsNet = msg.IsNet;
                                if (msg.IsNet)
                                {
                                    friend.ID = msg.NetUserID;
                                }
                                MsgHelper.addUIFriend(friend);
                                _poolFactory.resetFriend(friend);
                            }

                            if (_face.Equals("qq"))
                            {
                                friend.Messages.Add(msg);
                                if (MsgHelper.FindWindow(friend.ID) == IntPtr.Zero)
                                {
                                    Program.formmain.notify.ShowBalloonTip(60, "消息", "收到来自组:" + friend.GroupName + "-" + friend.Name + "的消息", ToolTipIcon.Info);
                                }
                                Application.DoEvents();
                            }
                            else if (_face.Equals("lanmsg"))
                            {
                                _poolFactory.Msg.Add(msg);
                                if ((!Program.formlist.Actived || !Program.formlist.Visible) && MsgHelper.FindWindow(friend.ID) == IntPtr.Zero)
                                {
                                    Program.formlist.notify.BalloonTipText = "收到来自组:" + friend.GroupName + "-" + friend.Name + "的消息";
                                    Program.formlist.notify.ShowBalloonTip(30);
                                    Application.DoEvents();
                                }
                            }
                            break;
                        }

                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_FILE:
                        {
                            if (msg.Ver.ToLower().Equals("lanmsg")) break;
                            goto case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT;
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_MEMBER_LIST:
                        {
                            if (msg.SendFromID.Equals(getIP(selfIP)) || msg.SendFromID.Equals(_myNetID)) break;
                            goto case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_FILE:
                        {
                            if(_face.ToLower().Equals("lanmsg"))
                            Program.formlist.txtMsg.AppendText(getAllName(msg.SendFromID) + "接受文件:" + msg.SendInfo.Split('|')[0] + "\r\n\r\n");
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                writeLog("helper.showMsg"+ex.Message);
            }
            finally
            {
                msg.InfoGuid = "";
            }
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="msg"></param>
        private static void THshowMsg(MsgInfo.MsgInfo msg,IPEndPoint iep)
        {
            try
            {
                if (string.IsNullOrEmpty(msg.SendFromID))
                    msg.SendFromID = iep.Address.ToString();
                if (_isNET) LastConnectServerTime = DateTime.Now;//更新最后一次收到服务器消息时间
                switch (msg.ProtocolType)
                {                   
                    
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_MEMBER_LIST:
                        {
                            if (msg.SendFromID.Equals(getIP(selfIP)) || (_isNET && msg.NetUserID.Equals(_myNetID))) break;
                            
                            LoginFriend(msg);
                           
                            break;
                        }                   
                    
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_IMG:
                        {
                            _poolFactory.Img.Add(msg);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_IMGPACK://好友重新请求消息包
                        {
                            MsgHelper.sendRequestImgPack(msg);
                            break;
                        }                    
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_FILE: //接收文件
                        {
                            _poolFactory.ReceivedTextMsg.Add(new MsgPoolFactory.UIMsgs(msg, iep));
                            MsgHelper.fileinfo = msg;
                            Thread thsendfile = new Thread(new ThreadStart(MsgHelper.sendFile));
                            thsendfile.IsBackground = true;
                            thsendfile.Start();
                            if (!msg.IsNet)
                            {
                                writeLog(msg.SendFromID + " 接收文件：" + msg.SendInfo);
                            }
                            else
                            {
                                writeLog(msg.NetUserID + " 接收文件：" + msg.SendInfo);
                            }
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_FILEPACK:
                        {
                            //if (msg.Ver.ToLower().Equals("lanmsg")) break;
                            MsgHelper.sendFilePackIndex(msg);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_DIR:
                        {
                            //if (msg.Ver.ToLower().Equals("lanmsg")) break;
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SERVER_HELLO:
                        {
                            break;
                        }
                    default:
                        {
                            _poolFactory.ReceivedTextMsg.Add(new MsgPoolFactory.UIMsgs(msg,iep));
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                writeLog("helper.thshowmsg "+ ex.Message);
            }           
        }
        /// <summary>
        /// 获取指定进程名的进
        /// </summary>
        /// <param name="processname"></param>
        /// <returns></returns>
        public static Process[] getProcessesByName(string processname)
        {
            return Process.GetProcessesByName(processname);
        }
        /// <summary>
        /// 是否是正确的ＩＰ地址
        /// </summary>
        /// <param name="bip"></param>
        /// <returns></returns>
        public static bool isByteIP(byte[] bip)
        {
            try
            {
                string strip = getIP(bip);
                IPAddress ipadd;
                return IPAddress.TryParse(strip, out ipadd);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是正确的ＩＰ地址
        /// </summary>
        /// <param name="bip"></param>
        /// <returns></returns>
        public static bool isStringIP(string strip)
        {
            try
            {                
                IPAddress ipadd;
                return IPAddress.TryParse(strip, out ipadd);
            }
            catch
            {
                return false;
            }
        }
        private bool findCurFriend(Friend.Friend friend)
        {
            return equalIP( friend.Ip,_curfriendIP);
        }
        /// <summary>
        /// 把ＩＰ转为字符串格式
        /// </summary>
        /// <param name="byteip"></param>
        /// <returns></returns>
        public static string getIP(byte[] byteip)
        {
            if (byteip == null || byteip.Length < 4) return "";
            return string.Concat(byteip[0], '.', byteip[1], '.', byteip[2], '.', byteip[3]);
        }
        /// <summary>
        /// 把ＩＰ转为字符集
        /// </summary>
        /// <param name="strip"></param>
        /// <returns></returns>
        public static byte[] getByteIP(string strip)
        {
            byte[] bs;
            List<byte> allbs = new List<byte>();
            foreach (string ip in strip.Split(','))
            {
                string[] strs = ip.Split('.');
                for (int i = 0; i < strs.Length; i++)
                {
                    byte b;
                    byte.TryParse(strs[i], out b);
                    allbs.Add(b);
                }
            }
            bs = new byte[allbs.Count];
            allbs.CopyTo(bs);
            return bs;
        }
        public static MsgInfo.FilePack byteArrayToFilePack(byte[] filebuf)
        {
            try
            {
                MemoryStream serializationStream = new MemoryStream(filebuf);
                Object obj = new BinaryFormatter().Deserialize(serializationStream);
                serializationStream.Close();
                MsgInfo.FilePack fp= (MsgInfo.FilePack)obj;
                if (!containMe(fp.ToIP)) return null;
                return fp;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="msgbuf"></param>
        /// <returns></returns>
        public static MsgInfo.MsgInfo byteArrayToObject(byte[] msgbuf)
        {
            try
            {
                MsgInfo.MsgInfo info=_turnmsg.byteToMsg(msgbuf);
                //MemoryStream serializationStream = new MemoryStream(msgbuf);
                //Object obj = new BinaryFormatter().Deserialize(serializationStream);
                //serializationStream.Close();
                //bool isrightpack = false;
                //try
                //{
                //    info = (MsgInfo.MsgInfo)obj;
                    
                //    if (string.IsNullOrEmpty(info.SendFromID))
                //    {

                //        throw new Exception(" ");
                //    }
                //    info.Ver = "LanTalk";
                //    isrightpack = true;
                //}
                //catch
                //{
                //    if (!_oldver) return null;
                //    info = new MsgInfo.MsgInfo();
                //    Type t = obj.GetType();
                //    MethodInfo[] methods = t.GetMethods();
                //    info.InfoGuid = _guid;
                //    foreach (MethodInfo method in methods)
                //    {
                //        object robj;
                //        try
                //        {
                //            if (method.Name.Contains("get_ProtocolType"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                if (robj.GetType() == typeof(MsgInfo.MsgInfo.sProtocolType))
                //                {
                //                    info.ProtocolType = (MsgInfo.MsgInfo.sProtocolType)(robj);
                //                }
                //                else
                //                {
                //                    int p = int.Parse(robj == null ? "0" : robj.ToString());
                //                    if (p > 5 || p < 1) return null;
                //                    info.ProtocolType = (MsgInfo.MsgInfo.sProtocolType)(p);
                //                }
                //                isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendFromID"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendFromID = robj.ToString();
                //                isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendFromIp"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendFromID = getIP((byte[])robj);
                //                isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendToID"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendToID = robj.ToString();
                //                isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendToIp"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendToID = getIP((byte[])robj);
                //                isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendInfo"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendInfo = (robj == null ? null : robj.ToString());
                //                isrightpack = true;
                //            }
                //        }
                //        catch
                //        { }
                //    }
                //    info.Ver = "LanMsg";
                //}
                if (!_oldver && !info.Ver.ToLower().Equals("nettalk")) return null;
                if (_isNET && !MsgHelper.netlogined && info.ProtocolType != MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_ERR && info.ProtocolType != MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_OK) return null;
                if (string.IsNullOrEmpty(info.SendToID)) info.SendToID = getIP(selfIP);
                //过滤不符合规则的数据
                if (!containMe(info.SendToID) || !info.InfoGuid.ToLower().Equals(_guid.ToLower())) //(equalIP(info.SendFromID, selfIP) && info.ProtocolType != MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT && info.ProtocolType != MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_IMG) ||
                    return null;                
                return info;
            }
            catch (Exception ex)
            {
                writeLog("helper.byteArrayToObject "+ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 是否包含本机ＩＰ
        /// </summary>
        /// <param name="ips"></param>
        /// <returns></returns>
        public static bool containMe(byte[] ips)
        {
            try
            {
                for (int i = 0; i < ips.Length; i += 4)
                { 
                byte[] ip=new byte[4];
                ip[0] = ips[i];
                ip[1] = ips[i+1];
                ip[2] = ips[i+2];
                ip[3] = ips[i+3];
                if (equalIP(ip, selfIP) || equalIP(ip,getByteIP(IPAddress.Broadcast.ToString()))) 
                { return true; }
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 是否包含本机ＩＰ
        /// </summary>
        /// <param name="ips"></param>
        /// <returns></returns>
        public static bool containMe(string ips)
        {
            try
            {
                return ips.Contains(getIP(selfIP)) || ips.Contains(IPAddress.Broadcast.ToString()) || ips.Contains(_myNetID) || _isNET;
                //return ips.Contains(_myNetID);
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 转为字节地址
        /// </summary>
        /// <param name="allip"></param>
        /// <returns></returns>
        public static string turnToByteIP(List<string> allip)
        {
            string allips = "";
            foreach (string ip in allip)
            {
                allips += ip + ",";
            }
            if (allips.EndsWith(","))
            {
                allips = allips.Substring(0, allips.Length - 1);
            }
            return allips;
        }
        /// <summary>
        /// 转为字节地址
        /// </summary>
        /// <param name="allip"></param>
        /// <returns></returns>
        public static byte[] turnToByteIP(string[] allip)
        {
            byte[] allbip = new byte[allip.Length * 4];
            for (int i = 0; i < allip.Length; i++)
            {
                string[] ip = allip[i].Split('.');
                allbip[i * 4] = byte.Parse(ip[0]);
                allbip[i * 4 + 1] = byte.Parse(ip[1]);
                allbip[i * 4 + 2] = byte.Parse(ip[2]);
                allbip[i * 4 + 3] = byte.Parse(ip[3]);
            }
            return allbip;
        }
        /// <summary>
        /// 将字节ＩＰ转为字符串
        /// </summary>
        /// <param name="allip"></param>
        /// <returns></returns>
        public static string[] turnToStringIP(byte[] allip)
        {
            string[] strips = new string[allip.Length / 4];
            for (int i = 0; i < allip.Length; i += 4)
            {
                strips[i / 4] = allip[i].ToString() + "." + allip[i+1].ToString() + "." + allip[i+2].ToString() + "." + allip[i+3].ToString();
            }
            return strips;
        }
        /// <summary>
        /// 获取当前所有存在的组
        /// </summary>
        /// <returns></returns>
        public static List<string> getGroupNames()
        {
            List<string> allgroupnames = new List<string>();
            for (int i = 0; i < Friends.Count; i++)
            {
                if (!allgroupnames.Contains(Friends[i].GroupName))
                {
                    allgroupnames.Add(Friends[i].GroupName);
                }
            }
            return allgroupnames;
        }
        /// <summary>
        /// 获取包含在ＩＰ中的好友的名称
        /// </summary>
        /// <param name="allip"></param>
        /// <returns></returns>
        public static string getAllName(string allip)
        {
            string name = ",";
            try
            {
                
                string[] receivedip = allip.Split(',');
                
                for (int i = 0; i < receivedip.Length; i++)
                {
                    if (string.IsNullOrEmpty(receivedip[i])) continue;
                    try
                    {
                        List<Friend.Friend> groupfriend = _poolFactory.findFriendsByGroupName(_poolFactory.findFriend(receivedip[i]).GroupName);
                        if (groupfriend != null && groupfriend.Count > 0)
                        {
                            bool allgroupfriendscontian = true;
                            foreach (Friend.Friend f in groupfriend)
                            {
                                if (!containString(receivedip, f.ID))
                                {
                                    allgroupfriendscontian = false;
                                    break;
                                }
                            }
                            if (allgroupfriendscontian)
                            {
                                name += " { " + _poolFactory.findFriend(receivedip[i]).GroupName + " } ,";
                                foreach (Friend.Friend f in groupfriend)
                                {
                                    string curgroupnameip = f.ID;
                                    for (int j = i + 1; j < receivedip.Length; j++)
                                    {
                                        if (receivedip[j].Equals(curgroupnameip)) receivedip[j] = "";
                                    }
                                }
                            }
                            else
                            {
                                name += _poolFactory.findFriend(receivedip[i]).Name + ",";
                            }
                        }
                    }
                    catch
                    {
                        name += receivedip[i] + ",";
                    }
                }
                if (name.Equals(_poolFactory.getAllGroupName())) return " 所有人 ";
                if (!string.IsNullOrEmpty(name)) name = name.Substring(1, name.Length - 2);
            }
            catch
            { }
            return name;
        }
        public static bool containString(string[] strs,string str)
        {
            foreach (string s in strs)
            {
                if (s.Equals(str)) return true;
            }
            return false;
        }
        /// <summary>
        /// ＩＰ是否相等
        /// </summary>
        /// <param name="ip1"></param>
        /// <param name="ip2"></param>
        /// <returns></returns>
        public static bool equalIP(byte[] ip1, byte[] ip2)
        {
            try
            {
                return (ip1[0] == ip2[0] && ip1[1] == ip2[1] && ip1[2] == ip2[2] && ip1[3] == ip2[3]);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 把对象转为字节
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ojectToByteArray(object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream,obj);
            if (stream.Length > 1000)//如果消息过大则进行压缩
            {
                stream = gzpi.SerStreamZip(stream);
            }
            byte[] bs = stream.ToArray();            
            stream.Close();            
            return bs;
        }
        /// <summary>
        /// 加密字符
        /// </summary>
        /// <param name="infobs"></param>
        /// <returns></returns>
        public static string serInfo(string info)
        {
            int H;
            int L;
            int blen = sizeof(byte) * 8;            
            string Hexs = "0123456789abcdef";
            byte[] bs = Encoding.UTF32.GetBytes(info);
            for (int i = 0; i < bs.Length; i++)
            {
                //H = bs[i] >> 4;
                //L = bs[i] & 0xf;
                bs[i]=(byte)(bs[i] ^ (255)) ;
            }
            return Encoding.UTF32.GetString(bs);
        }
        /// <summary>
        /// 解密字符
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string derInfo(string strcode)
        {
            int H;
            int L;
            int blen = sizeof(byte) * 8;
            string newstr = "";
            string Hexs = "0123456789abcdef";
            byte[] bs=new byte[strcode.Length/2];
            for (int i = 0; i < strcode.Length; i++)
            {
                //H = Hexs.IndexOf(strcode[i]);
                //L = Hexs.IndexOf(strcode[i+1]);
                bs[i] = (byte)(bs[i] ^ (255));
            }
            return Encoding.UTF32.GetString(bs);
        }
        /// <summary>
        /// 将对象写入文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        public static void writeObjectToFile(object obj, string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(filename)) File.Delete(filename);
            FileStream stream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);
            formatter.Serialize(stream, obj);
            stream.Close();
        }
        /// <summary>
        /// 从文件中获取对象
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static object readFileToObject(string filename)
        {
            if (!File.Exists(filename)) return null;
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            IFormatter formatter = new BinaryFormatter();
            object obj= formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }
        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="msg"></param>
        public static void addMsg(byte[] msg,IPEndPoint iep)
        {
            _poolFactory.addMsg(new MsgPoolFactory.receiveMsgs(msg,iep));
                    
        }
        /// <summary>
        /// 收到文件
        /// </summary>
        /// <param name="msg"></param>
        public static void addFile(byte[] file,int index)
        {
            try
            {
                if (_poolFactory.Lockedfilebyte == 2)
                {
                    _poolFactory.FileByte1[index].Add(file);
                }
                else
                {
                    _poolFactory.FileByte2[index].Add(file);
                }               
            }
            catch
            { }
        }
        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="files"></param>
        public static void combinFiles(string filename, string[] files)
        {
            try
            {
                if (File.Exists(filename)) File.Delete(filename);
                if (files.Length == 1) { File.Move(files[0], filename); return; }
                FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);
                foreach (string file in files)
                {
                    byte[] bs = File.ReadAllBytes(file);
                    fs.Write(bs, 0, bs.Length);
                    File.Delete(file);
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// 字符串转RTF
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string strToRTF(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            System.Windows.Forms.RichTextBox rtb = new System.Windows.Forms.RichTextBox();
            rtb.AppendText(str);
            return rtb.Rtf;
            //string strResult = "{\\rtf1\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052{\\fonttbl{\\f0\\fnil\\fcharset134 \\'cb\\'ce\\'cc\\'e5;}}\r\n\\viewkind4\\uc1\\pard\\lang2052\\f0\\fs18";
            //string Hexs = "0123456789abcdef";
            //System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("gb2312");            
            //foreach (char c in str)
            //{
            //    if (c>0 && c < 256)
            //    {
            //        if (c > 32 && c < 127)
            //        {
            //            if (c == ' ' || c == '{' || c == '}')
            //            {
            //                strResult += "\\" + c.ToString();
            //            }
            //            else
            //            {
            //                strResult += c.ToString();

            //            }
            //        }
            //        else
            //        {
            //            strResult += "'" + ((byte)c).ToString();
            //        }
            //    }
            //    else
            //    {
            //        byte[] bs = encoding.GetBytes(c.ToString());
            //        foreach (byte b in bs)
            //        {
            //            int h = (b & 0xf0) >> 4;
            //            int l = b & 0xf;
            //            strResult += "\\'" + Hexs[h] + Hexs[l];                        
            //        }
            //    }

            //}
            //return strResult + "\\par\r\n}\r\n";
        }
        /// <summary>
        /// RTF转为字符串
        /// </summary>
        /// <param name="strRTF"></param>
        /// <returns></returns>
        public static string RTFToString(string strRTF)
        {
            System.Windows.Forms.RichTextBox rtb = new System.Windows.Forms.RichTextBox();
            rtb.Rtf = strRTF;
            return rtb.Text;
            //strRTF = strRTF.Replace("{\\rtf1\\ansi\\ansicpg936\\deff0\\deflang1033\\deflangfe2052{\\fonttbl{\\f0\\fnil\\fcharset134 \\'cb\\'ce\\'cc\\'e5;}}\r\n\\viewkind4\\uc1\\pard\\lang2052\\f0\\fs18", "").Replace("\\par\r\n}\r\n", "");
            //string Hexs = "0123456789abcdef";
            //string strResult = "";
            //System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("gb2312");
            //for (int i = 0; i < strRTF.Length; i++)
            //{
            //    if (strRTF[i] == '\\' && i<strRTF.Length-1)
            //    {
            //        if (strRTF[i + 1] == '\'' && i < strRTF.Length - 7 && strRTF[i + 4] == '\\' && strRTF[i + 5] == '\'')
            //        {
            //            byte[] bchar = new byte[2];
            //            char c1 = strRTF[i + 2];
            //            char c2 = strRTF[i + 3];
            //            int h = Hexs.IndexOf(c1);
            //            int l = Hexs.IndexOf(c2);
            //            int r = (h << 4) | l;
            //            bchar[0] = byte.Parse(r.ToString());
            //            c1 = strRTF[i + 6];
            //            c2 = strRTF[i + 7];
            //            h = Hexs.IndexOf(c1);
            //            l = Hexs.IndexOf(c2);
            //            r = (h << 4) | l;
            //            bchar[1] = byte.Parse(r.ToString());
            //            strResult += encoding.GetString(bchar);
            //            i += 7;
            //        }
            //        else
            //        {
            //            strResult += strRTF[i+1].ToString();
            //            i++;
            //        }
            //    }
            //    else if (strRTF[i] == '\'' && i < strRTF.Length - 1)
            //    {
            //        int index = strRTF.IndexOf(' ', i);
            //        if (index > i && index < i + 5)
            //        {
            //            int len = index - i - 1;
            //            string strtemp = strRTF.Substring(i + 1, len);
            //            strResult += char.Parse(strtemp);
            //            i += len + 1;
            //        }
            //        else
            //        {
            //            strResult += strRTF[i];
            //        }
            //    }
            //    else
            //    {
            //        strResult += strRTF[i];
            //    }
            //}
            //return strResult;
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="configkey"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static string getConfigByName(string configkey, string defaultvalue)
        {
            try
            {
                ConfigManager.ConfigManager config = new ConfigManager.ConfigManager(Path.Combine(Application.StartupPath, "config.xml"),_guid);
                return config.readConfig(configkey, defaultvalue);
            }
            catch
            {
                return defaultvalue;
            }
        }
        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void setConfig(string key, string value)
        {
            try
            {
                ConfigManager.ConfigManager config = new ConfigManager.ConfigManager(Path.Combine(Application.StartupPath, "config.xml"), _guid);
                config.writeConfig(key,value);
            }
            catch
            { }
        }
        /// <summary>
        /// 设置自动启动
        /// </summary>
        /// <param name="startATpc"></param>
        /// <returns></returns>
        public static bool startATpc(bool startATpc)
        {
            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                regkey.SetValue("WinLanMsg", "");
                regkey.DeleteValue("WinLanMsg");
                if (startATpc)
                {
                    regkey.SetValue("LanTalk",Path.Combine(Application.StartupPath ,Process.GetCurrentProcess().MainModule.ModuleName));
                }
                else
                {
                    regkey.DeleteValue("LanTalk");
                }
                return startATpc;
            }
            catch
            {
                return false;
            }
        }
        public static bool readAutoStart()
        {
            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");                
                
                string regvalue=regkey.GetValue("LanTalk").ToString();
                return regvalue.Contains(Process.GetCurrentProcess().MainModule.ModuleName);
                
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strloginfo"></param>
        public static void writeLog(string strloginfo)
        {
            try
            {
                string logpath = Path.Combine(Application.StartupPath, "Log\\" + DateTime.Now.ToShortDateString() + ".log");
                if (!Directory.Exists(Path.GetDirectoryName(logpath))) Directory.CreateDirectory(Path.GetDirectoryName(logpath));
                //if (!File.Exists(logpath)) File.Create(logpath);
                FileStream stream = new FileStream(logpath,FileMode.Append,FileAccess.Write);
                strloginfo = "[" + DateTime.Now.ToShortTimeString() + "] " + strloginfo + Environment.NewLine;
                byte[] bs = System.Text.Encoding.GetEncoding("utf-8").GetBytes(strloginfo);
                stream.Write(bs,0,bs.Length);
                stream.Close();
            }
            catch
            { }

        }
        [DllImport("user32")]
        private static extern Int16 SendMessage(IntPtr hwnd, Int16 Msg, Int16 wparam, Int16 lparam);
        /// <summary>
        /// 关闭应用程序
        /// </summary>
        /// <param name="hwnd"></param>
        public static void closeWindowByHwnd(IntPtr hwnd)
        {
            SendMessage(hwnd, 0x0010, 0, 0);
        }
        /// <summary>
        /// 转为图标
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Icon imageToIcon(Image img)
        {
            try
            {
                MemoryStream stream = new MemoryStream();

                img.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);

                return Icon.FromHandle(new Bitmap(stream).GetHicon());
            }
            catch (Exception ex)
            {
                writeLog("helper.imagetoicon " + ex.ToString());
                return _defaultIcon;
            }
        }
        /// <summary>
        /// 返回加密后的密码
        /// </summary>
        /// <param name="userpwd"></param>
        /// <returns></returns>
        public static string serPWD(string userpwd)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(userpwd, "md5");
        }
        public static bool pressedKey(int key)
        {
            return GetAsyncKeyState(key) != 0;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr windowhander);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr windowhander);
        [DllImport("gdi32.dll")]
        public extern static IntPtr GetCurrentObject(IntPtr hdc, ushort objectType);
        [DllImport("user32.dll")]
        public extern static void ReleaseDC(IntPtr hdc);
        [DllImport("user32.dll")]
        public extern static int GetAsyncKeyState(int vkey);
        /// <summary>
        /// 将窗体移至最顶端
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static int BringWindowToTop(IntPtr hwnd);

        const int HWND_TOP = 0;
        const int SWP_SHOWWINDOW=32;
        [DllImport("user32.dll")]
        public extern static IntPtr BeginDeferWindowPos(int nNumWindows);
        [DllImport("user32.dll")]
        public extern static IntPtr DeferWindowPos(IntPtr hWindowPosInfo,IntPtr hwnd,int hwndinsertafter,int x,int y,int cx,int cy,int uflags);
        [DllImport("user32.dll")]
        public extern static IntPtr EndDeferWindowPos(IntPtr hWindowPosInfo);
       /// <summary>
       /// 移动窗体
       /// </summary>
       /// <param name="windhwnd"></param>
       /// <param name="x"></param>
       /// <param name="y"></param>
       /// <param name="cw"></param>
       /// <param name="ch"></param>
        public static void MoveOrResizeForm(IntPtr windhwnd, int x, int y, int cw, int ch)
        {
            IntPtr hdwp;
            hdwp = BeginDeferWindowPos(1);
            DeferWindowPos(hdwp, windhwnd, HWND_TOP, x, y, cw, ch, SWP_SHOWWINDOW);
            EndDeferWindowPos(hdwp);
        }
        #region 关机用到的API代码

        [StructLayout(LayoutKind.Sequential, Pack = 1)]

        internal struct TokPriv1Luid
        {
            public int Count;

            public long Luid;

            public int Attr;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]

        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]

        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]

        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]

        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,

            ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]

        internal static extern bool ExitWindowsEx(int flg, int rea);

        internal  const int SE_PRIVILEGE_ENABLED = 0x00000002;

        internal  const int TOKEN_QUERY = 0x00000008;

        internal  const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;

        internal  const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        internal const int EWX_LOGOFF = 0x00000000;//注销

        internal const int EWX_SHUTDOWN = 0x00000001;//关机

        internal const int EWX_REBOOT = 0x00000002;//重新启动

        internal const int EWX_FORCE = 0x00000004;//强制注销

        internal const int EWX_POWEROFF = 0x00000008;//强制关机

        internal const int EWX_FORCEIFHUNG = 0x00000010;

        private static void DoExitWin(int flg)
        {
            bool ok;

            TokPriv1Luid tp;

            IntPtr hproc = GetCurrentProcess();

            IntPtr htok = IntPtr.Zero;

            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);

            tp.Count = 1;

            tp.Luid = 0;

            tp.Attr = SE_PRIVILEGE_ENABLED;

            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);

            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);

            ok = ExitWindowsEx(flg, 0);
        }

        

        /// <summary>
        /// 显示XP风格函数
        /// </summary>
        [DllImport("comctl32.dll")]
        public extern static void InitCommonControls();
        [DllImport("advapi32.dll")]
        private extern static int InitiateSystemShutdownA(string lpmachinename, string lpmessage, int dwtimeout, bool bforceappsclosed, bool brebootaftershutdown);
        public static void closeComputer()
        {
            //int re=InitiateSystemShutdownA(null,"关机",0,false,false);
            DoExitWin(EWX_POWEROFF);
        }
        public static void resetComputer()
        {
            //int re = InitiateSystemShutdownA(null, "重启", 0, false, true);
            DoExitWin(EWX_FORCEIFHUNG);
        }
        #endregion
    }
}
