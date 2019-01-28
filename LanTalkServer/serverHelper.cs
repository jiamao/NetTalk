using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Configuration;
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

namespace LanTalkServer
{
    class serverHelper
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
        static bool _oldver = false;//是否兼容旧版本
        static string _face;
        static int _threadCount = 1;
        static bool _downLoading = false;
        static int _lanPort = 9050;
        static Friend.Friend.EState mystate = Friend.Friend.EState.InLine;
        static string _historypath;
        static string _sendmsgButton = "enter";
        public static FormLanTalkServer MainForm;
        public static objectToMsg.turnMsg _turnmsg;
        static FileCompress.GZip gzpi;
        public static int friendTimeOutSeconds = -180;
        public static bool autostart = false;
        public static bool islevelserver = false;
        public static bool isloginedlevelserver = false;
        public static bool islostmastserver = false;
        public static bool listenlocal = true;//是否对本地监听
        public static string serverguid = "";
        public static string mainserveraddr = "haofefe.gicp.net";
        public static int mainserverport = 6000;

        public static string SendmsgButton
        {
            get { return serverHelper._sendmsgButton; }
            set { serverHelper._sendmsgButton = value; }
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
                return serverHelper._historypath; 
            }
            set { serverHelper._historypath = value; }
        }

        public static Friend.Friend.EState Mystate
        {
            get { return serverHelper.mystate; }
            set { serverHelper.mystate = value; }
        }

        public static bool DownLoading
        {
            get { return serverHelper._downLoading; }
            set { serverHelper._downLoading = value; }
        }
        public static int ThreadCount
        {
            get { return serverHelper._threadCount; }
            set { serverHelper._threadCount = value; }
        }

        public static string Face
        {
            get { return serverHelper._face; }
            set { serverHelper._face = value; }
        }
        public static bool Oldver
        {
            get { return serverHelper._oldver; }
            set { serverHelper._oldver = value; }
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
            get { return serverHelper._iflag; }
            set { serverHelper._iflag = value; }
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
            get { return serverHelper._listUserHeader; }
            set { serverHelper._listUserHeader = value; }
        }
        static ImageList _listUserFace = new ImageList();
        /// <summary>
        /// 用户发送图标集合
        /// </summary>
        public static ImageList ListUserFace
        {
            get { return serverHelper._listUserFace; }
            set { serverHelper._listUserFace = value; }
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
                    localhostip = getByteIP(ips[0].ToString());                    
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
        /// <summary>
        /// 本地通信端口
        /// </summary>
        public static int LanPort
        {
            set { _lanPort = value; }
            get { return _lanPort; }
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
        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        public static void init()
        {
            try
            {
                _iflag = 0;
                if (_turnmsg==null) _turnmsg = new objectToMsg.turnMsg(_guid);
                try
                {
                    islevelserver = ConfigurationManager.AppSettings["LEVELSERVER"].ToString().ToLower().Equals("true");
                    listenlocal = ConfigurationManager.AppSettings["LISTENLOCAL"].ToString().ToLower().Equals("true");
                    mainserveraddr = ConfigurationManager.AppSettings["SERVER"].ToString();
                    mainserverport = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());
                }
                catch
                { }
                gzpi = new FileCompress.GZip(_guid);
                listener = new MsgListener.MsgListener(_lanPort, _guid);
                listener.getMsg -= addMsg;
                listener.getMsg += addMsg;//获取消息
                listener.getFile -= addFile;//接收文件  
                listener.getFile += addFile;//接收文件                
                
                if (_send==null) _send = new MsgSend.MsgSend(_lanPort,_guid);//初始化本地通信端口
                if (!islevelserver)
                {
                    if (listenlocal) listener.init();//对本地进行监听
                    listener.initserver();//如果是主服务器才启用远程监听
                }
                else
                {
                    lastlistenhellofrommainserver = DateTime.Now;
                    _send.NetSendPortChanged -= listener.resetNetListen;
                    _send.NetSendPortChanged += listener.resetNetListen;//如果不是主服务器则对主服务器进行监听
                    listener.init();//对本地进行监听
                }
                byte[] myip=serverHelper.selfIP;
                Login();                
                serverMsgHelper.init();                
                writeLog("启动成功");
            }
            catch (Exception ex)
            {
                throw ex;       
            }

        }
        public static void initmyself()
        {
           
        }
        public static void initForm()
        {
            
        }
        /// <summary>
        /// 退出
        /// </summary>
        public static void exitme()
        {
            _iflag = 1;
            if(listener != null)listener.stop();
            isloginedlevelserver = false;
            _poolFactory.clear();
            MainForm.lvlan.Items.Clear();
            MainForm.lvnet.Items.Clear();
            writeLog("服务停止");
            GC.Collect();
        }
        /// <summary>
        /// 登入
        /// </summary>
        public static void Login()
        {
            if(!islostmastserver)sendLogin(IPAddress.Broadcast.ToString(),serverHelper.mystate);
            if (islevelserver)
            {
                writeLog("开始登录主服务器：" + mainserveraddr + "\r\n如果一段时间内没有返回信息，请重新登陆!");
                serverguid=Guid.NewGuid().ToString("n");
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
                msginfo.InfoGuid = _guid;
                msginfo.IsNet = true;
                msginfo.NetUserID = serverguid;
                msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SERVER_LOGIN;
                msginfo.SendFromID = serverguid;
                msginfo.SendInfo = serverguid;
                msginfo.SendTo = mainserveraddr;
                msginfo.SendToID = mainserveraddr;
                msginfo.Ver = "NetTalk";
                _poolFactory.addSendInfo(msginfo,mainserveraddr,mainserverport,true);
            }
        }
        /// <summary>
        /// 登出
        /// </summary>
        public static void LogOut()
        {
            sendLoginOut(IPAddress.Broadcast.ToString());
        }
        private static void LoginFriend(MsgInfo.MsgInfo info,IPEndPoint iep)
        {
            try
            {                
                //如果对方是外网用户,则返回所有好友
                if (info.IsNet)
                {
                    for (int i = 0; i < _poolFactory.Friends.Count; i++)
                    {
                        if (_poolFactory.Friends[i].ServerID.Equals(info.SendFromID) || (_poolFactory.Friends[i].ID.Equals(info.NetUserID))) continue;
                        MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
                        msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                        msginfo.InfoGuid = _guid;
                        msginfo.Ver = _poolFactory.Friends[i].Ver;
                        msginfo.NetUserID = _poolFactory.Friends[i].ID;
                        msginfo.IsNet = _poolFactory.Friends[i].IsNet;
                        msginfo.SendFromID = _poolFactory.Friends[i].ID;
                        msginfo.SendToID = info.NetUserID;
                        msginfo.SendTo = info.NetUserID;
                        msginfo.SendInfo = _poolFactory.Friends[i].Name + "," + _poolFactory.Friends[i].GroupName + "," + getIP(_poolFactory.Friends[i].Ip) + "," + _poolFactory.Friends[i].Header + "," + (int)(_poolFactory.Friends[i].State);
                        _poolFactory.addSendInfo(msginfo, iep.Address.ToString(),iep.Port,true);
                    }
                }
                else//如果是内网用户则返回外网好友
                {
                    //if (info.Ver.Equals("NetTalk"))
                    //{
                        MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
                        msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                        msginfo.InfoGuid = _guid;
                        List<Friend.Friend> netfriends = _poolFactory.frinNetFriends();
                        foreach (Friend.Friend f in netfriends)
                        {
                            msginfo.NetUserID = f.ID;
                            msginfo.IsNet = true;
                            msginfo.SendFromID = f.ID;
                            msginfo.Ver = f.Ver;
                            msginfo.SendToID = info.SendFromID;
                            msginfo.SendTo = info.SendFromID;
                            msginfo.SendInfo = f.Name + "," + f.GroupName + "," + getIP(f.Ip) + "," + f.Header + "," + (int)(f.State);
                            _poolFactory.addSendInfo(msginfo, new string[] { msginfo.SendTo});
                        }
                    //}
                    //else
                    //{
                    //    WinLanMsg.Util.UserInfo userinfo = new WinLanMsg.Util.UserInfo();
                    //    userinfo.ProtocolType = 1;
                    //    userinfo.SendFromIp = selfIP;
                    //    userinfo.SendToIp = getByteIP(info.SendFromID);
                    //    foreach (Friend.Friend f in netfriends)
                    //    {

                    //        _poolFactory.addSendInfo(userinfo, new string[] { getByteIP(info.SendFromID) });
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                writeLog("serverhelper.loginfriend" + ex.ToString());
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
                if (_oldver)
                {
                    WinLanMsg.Util.UserInfo userinfo = new WinLanMsg.Util.UserInfo();                    
                    userinfo.ProtocolType = 3;
                    userinfo.SendFromIp = selfIP;
                    userinfo.SendToIp = getByteIP(ip);
                    userinfo.SendInfo = "";
                    _poolFactory.addSendInfo(userinfo, new string[] { ip });
                }
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
                msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_MEMBER_LIST;
                msginfo.SendFromID = getIP(selfIP);
                msginfo.SendToID = ip;
                msginfo.SendTo = ip;
                msginfo.InfoGuid = _guid;
                msginfo.SendInfo = "server";
                _poolFactory.addSendInfo(msginfo, new string[] { ip });                
            }
            catch (Exception ex)
            {
                writeLog("helper.sendlogin "+ex.Message);
            }
        }
        /// <summary>
        /// 发送的进入消息
        /// </summary>
        /// <param name="msginfo"></param>
        public static void sendChangeState(MsgInfo.MsgInfo msginfo,IPEndPoint iep)
        {
            if (string.IsNullOrEmpty(msginfo.SendToID)) return;
            if (msginfo.IsNet)
            {
                MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(msginfo);
                info.InfoGuid = _guid;
                info.IsNet = msginfo.IsNet;
                info.NetUserID = msginfo.NetUserID;
                info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                info.SendInfo = msginfo.SendInfo.Substring(0, msginfo.SendInfo.LastIndexOf(','));
                info.SendFromID = getIP(selfIP);
                info.SendToID = IPAddress.Broadcast.ToString();
                info.SendTo = info.SendToID;
                _poolFactory.addSendInfo(info, IPAddress.Broadcast.ToString(), _lanPort);
            }
            
                if (!islevelserver)
                {
                    List<Friend.Friend> netfriends = _poolFactory.frinNetFriends();
                    foreach (Friend.Friend f in netfriends)
                    {
                        MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(msginfo);
                        info.SendTo = f.ID;
                        info.SendInfo = msginfo.SendInfo.Substring(0, msginfo.SendInfo.LastIndexOf(','));
                        info.SendToID = f.ID;
                        _poolFactory.addSendInfo(info, new string[] { f.ID });
                    }
                    for (int i = 0; i < _poolFactory.SERVERS.Count; i++)//给所有的次级服务器发送一遍
                    {
                        Friend.SERVER server=_poolFactory.findServerByADDR(iep.Address.ToString());
                        if (server != null) if (server.PORT==iep.Port) continue;//如果此消息来自此服务器不给这服务器转发
                        MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(msginfo);
                        info.SendTo = _poolFactory.SERVERS[i].ADDR;
                        info.SendInfo = msginfo.SendInfo.Substring(0, msginfo.SendInfo.LastIndexOf(','));
                        info.SendToID = _poolFactory.SERVERS[i].ID;
                        _poolFactory.addSendInfo(info, _poolFactory.SERVERS[i].ADDR, _poolFactory.SERVERS[i].PORT, true);
                    }
                }
                else if(!msginfo.IsNet && isloginedlevelserver)
                {
                    MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(msginfo);
                    info.SendTo = mainserveraddr;
                    info.SendInfo = msginfo.SendInfo.Substring(0, msginfo.SendInfo.LastIndexOf(','));
                    info.SendToID = mainserveraddr;
                    _poolFactory.addSendInfo(info, mainserveraddr,mainserverport,true);
                }
            
            
        }
        /// <summary>
        /// 发送的登出
        /// </summary>
        /// <param name="msginfo"></param>
        public static void turnSendLoginOut(MsgInfo.MsgInfo msginfo,IPEndPoint iep)
        {
            if (string.IsNullOrEmpty(msginfo.SendToID)) return;
            if (msginfo.IsNet)
            {
                MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(msginfo);
                info.SendFromID = getIP(selfIP);
                info.SendToID = IPAddress.Broadcast.ToString();
                info.SendTo = info.SendToID;
                _poolFactory.addSendInfo(info, IPAddress.Broadcast.ToString(), _lanPort);
            }
            if (islevelserver && isloginedlevelserver)
            {
                if (!msginfo.IsNet)
                {
                    MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(msginfo);
                    info.SendTo = mainserveraddr;                    
                    info.SendToID = mainserveraddr;
                    _poolFactory.addSendInfo(info, mainserveraddr, mainserverport,true);
                }
            }
            else
            {
                List<Friend.Friend> netfriends = _poolFactory.frinNetFriends();
                foreach (Friend.Friend f in netfriends)
                {
                    MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(msginfo);
                    info.SendToID = f.ID;
                    info.SendTo = f.ID;
                    _poolFactory.addSendInfo(info, new string[] { f.ID });
                }
                for (int i = 0; i < _poolFactory.SERVERS.Count; i++)//给所有的次级服务器发送一遍
                {
                    if (_poolFactory.findServerByADDRAndPORT(iep.Address.ToString(),iep.Port) != null) continue;//如果此消息来自此服务器不给这服务器转发
                    MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(msginfo);
                    info.SendTo = _poolFactory.SERVERS[i].ADDR;                    
                    info.SendToID = _poolFactory.SERVERS[i].ID;
                    _poolFactory.addSendInfo(info, _poolFactory.SERVERS[i].ADDR, _poolFactory.SERVERS[i].PORT, true);
                }
            }
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
                //sendChangeState(IPAddress.Broadcast.ToString(), mystate);
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
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
                msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_LOGOUT;
                msginfo.SendFromID = getIP(selfIP);
                msginfo.SendToID = ip;
                msginfo.SendTo = ip;
                msginfo.InfoGuid = _guid;
                msginfo.SendInfo = MyName + "," + MyGroupName + "," + getIP(selfIP) + "," + _myHeader;
                _send.SendBytes(ojectToByteArray(msginfo), ip);

            }
            catch (Exception ex)
            {

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
                //if (_poolFactory.SendedMsgImpLife.Count > 0)
                //{
                //    List<string> guids = new List<string>();
                //    DateTime imgdt;
                //    foreach (string guid in _poolFactory.SendedMsgImpLife.Keys)
                //    {

                //        try
                //        {
                //            imgdt = ((DateTime)_poolFactory.SendedMsgImpLife[guid]);
                //        }
                //        catch
                //        {
                //            guids.Add(guid);
                //            continue;
                //        }
                //        if (imgdt < DateTime.Now)//如果超过一分钟
                //        {
                //            _poolFactory.removeSendedImgsByGuid(guid);//移除超时的消息
                //            guids.Add(guid);
                //        }
                //    }
                //    foreach (string guid in guids)
                //    {
                //        _poolFactory.SendedMsgImpLife.Remove(guid);
                //    }
                //    guids.Clear();
                //    GC.Collect();
                //}

            }
            catch (Exception ex)
            {
                writeLog("helper.chekcmsg " + ex.ToString());
            }
        }
        public static void ThreadCheckMSG()
        {
            
                try
                {
                    if (HaveMsg)
                    {
                        int count = 0;
                        for (int i = 0; i < _poolFactory.Messages.Count; i++)
                        {
                            count++;
                            if (_poolFactory.Messages[i] == null) continue;
                            MsgInfo.MsgInfo msginfo = byteArrayToObject(_poolFactory.Messages[i].UserMsg);
                            //writeLog("收到来自" + _poolFactory.Messages[i].UserIpEndPoint.Address.ToString() + "的数据");
                            if (msginfo == null) continue;
                            THshowMsg(msginfo, _poolFactory.Messages[i].UserIpEndPoint);
                            _poolFactory.Messages[i] = null;
                        }
                        _poolFactory.removeAllMsg(count);//从消息队列中移除所有        
                    }
                    else 
                    {
                        checkTimeOutNetFriend();//检查外网好友是否掉线
                        Thread.Sleep(20);
                    }
                }
                catch (Exception ex)
                {
                    writeLog("helper.ThreadCheckMSG"+ex.Message);
                }
            
        }
        /// <summary>
        /// 检查掉线好友
        /// </summary>
        public static void checkTimeOutNetFriend()
        {
            try
            {
                DateTime outtime = DateTime.Now.AddSeconds(friendTimeOutSeconds);//此时好友掉线时间

                List<Friend.Friend> netfriends = _poolFactory.frinNetFriends();
                Friend.Friend[] netfs = new Friend.Friend[netfriends.Count];
                
                netfriends.CopyTo(netfs);
                
                if (!islevelserver)
                {
                    foreach (Friend.Friend f in netfs)
                    {
                        if (f.LastShowTime < outtime)
                        {
                            serverMsgHelper.outUIFriend(f);
                            sendLoginOut(f);
                            _poolFactory.removeFriend(f.ID);
                            writeLog(f.ID + "连接超时!");
                        }
                    }
                    for (int i = 0; i < _poolFactory.SERVERS.Count; i++)
                    {
                        if (_poolFactory.SERVERS[i].LASTCONTIME < outtime)
                        {
                            _poolFactory.SERVERS[i].ID = "";
                            writeLog("服务器超时:" + _poolFactory.SERVERS[i].ADDR);
                            sendServerLoginOut(_poolFactory.SERVERS[i]);
                        }
                    }
                    _poolFactory.removeOutTimeServers();
                }
                else if (isloginedlevelserver && lastlistenhellofrommainserver < outtime)//如果是次级服务器且与主机连接超时
                {
                    isloginedlevelserver = false;
                    islostmastserver = true;
                    writeLog("与主服务器连接超时!" + mainserveraddr);
                    foreach (Friend.Friend f in netfs)
                    {
                        serverMsgHelper.outUIFriend(f);
                        sendLoginOut(f);
                        _poolFactory.removeFriend(f.ID); 
                    }
                }
                else if (islostmastserver)
                {
                    writeLog("丢失服务器 重新请求主服务器...");
                    Login();
                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                writeLog("helper..checkTimeOutNetFriend" + ex.Message);
            }
        }
        public static void lostMainServer()
        {
            isloginedlevelserver = false;

        }
        /// <summary>
        /// 登出好友
        /// </summary>
        /// <param name="friend"></param>
        private static void sendLoginOut(Friend.Friend friend)
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
            info.SendFromID = getIP(selfIP);
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_LOGOUT;
            info.IsNet = true;
            info.InfoGuid = _guid;
            info.NetUserID = friend.ID;
            info.SendToID = IPAddress.Broadcast.ToString();
            info.SendTo = IPAddress.Broadcast.ToString();
            _poolFactory.addSendInfo(info, IPAddress.Broadcast.ToString(), _lanPort);


            List<Friend.Friend> netfriends = _poolFactory.frinNetFriends();
            foreach (Friend.Friend f in netfriends)
            {
                MsgInfo.MsgInfo info2 = new MsgInfo.MsgInfo(info);
                info.SendFromID = friend.ID;
                info2.SendTo = f.ID;
                info2.SendToID = f.ID;
                _poolFactory.addSendInfo(info2, new string[] { f.ID });
            }

            if (!islevelserver)
            {
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo(info);
                msginfo.SendFromID = friend.ID;
                msginfo.SendInfo = "bye";
                msginfo.Ver = "NetTalk";
                for (int i = 0; i < _poolFactory.SERVERS.Count; i++)
                {
                    _poolFactory.addSendInfo(new MsgInfo.MsgInfo(msginfo), _poolFactory.SERVERS[i].ADDR, _poolFactory.SERVERS[i].PORT, true);
                }
            }
            else if(isloginedlevelserver)
            {
                MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo(info);
                msginfo.SendFromID = friend.ID;
                msginfo.SendInfo = "bye";
                msginfo.Ver = "NetTalk";
                _poolFactory.addSendInfo(new MsgInfo.MsgInfo(msginfo), serverHelper.mainserveraddr, mainserverport, true);//向主服务器发送
            }
        }
        /// <summary>
        /// 登出服务器
        /// </summary>
        /// <param name="server"></param>
        public static void sendServerLoginOut(Friend.SERVER server)
        {
            List<Friend.Friend> serverfriends = _poolFactory.findFriendsByServerID(server.ID);
            if(serverfriends != null && serverfriends.Count>0)
            {
                for (int i = 0; i < serverfriends.Count; i++)
                {
                    sendLoginOut(serverfriends[i]);
                }
            }
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="msg"></param>
        private static void showMsg(MsgInfo.MsgInfo msg,IPEndPoint iep)
        {
            try
            {
                switch (msg.ProtocolType)
                {
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE://出现新好友
                        {
                            //if (msg.IsNet && !exsistedfriend && msg.ProtocolType == MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE) break; //外网用户不允许如此登录
                            msg.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                            Friend.Friend friend = new Friend.Friend();
                            if (msg.IsNet)//k如果是来自外网的消息
                            {
                                friend.Ip = getByteIP(iep.Address.ToString());
                                friend.ID = msg.NetUserID;
                                friend.Port = iep.Port;
                            }
                            else
                            {
                                friend.Ip = getByteIP(msg.SendFromID);
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
                            if (!islevelserver)
                            {
                                Friend.SERVER server = _poolFactory.findServerByADDR(iep.Address.ToString());
                                if (server != null)
                                {
                                    friend.ServerID = server.ID;
                                }
                            }
                            serverMsgHelper.addUIFriend(friend,iep);
                            if (_poolFactory.findFriend(friend.ID) == null)
                                writeLog((msg.IsNet || !string.IsNullOrEmpty(friend.ServerID)?"外网":"内网") + "用户:" + friend.Name + "[" + friend.ID + "]" + "登入");
                            _poolFactory.resetFriend(friend);
                            sendChangeState(msg,iep);                            
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN://外网用户登陆
                        {
                            if (islevelserver) break;//如果次级服务器则直接放弃
                            string[] friendinfo = (string.IsNullOrEmpty(msg.SendInfo) ? "" : msg.SendInfo).Split(',');
                            string userid = "";
                            string usrpwd = "";
                            for (int i = 0; i < friendinfo.Length; i++)
                            {
                                if (i == 5) userid = friendinfo[i];//ID
                                if (i == 6) usrpwd = friendinfo[i];//密码
                            }
                            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(usrpwd)) break;
                            Friend.Friend havefriend=_poolFactory.findFriend(userid);                            
                            if (UserHelper.netLogin(userid, usrpwd))
                            {
                                if (havefriend != null)
                                {
                                    returnNetErrInfo(userid, new IPEndPoint(IPAddress.Parse(getIP(havefriend.Ip)), havefriend.Port), "此用户在其它地方重复登陆!");
                                    Thread.Sleep(50);
                                }
                                returnNetLoginOkInfo(userid, iep);
                                Thread.Sleep(100);
                                LoginFriend(msg, iep);
                                goto case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                                break;
                            }
                            else
                            {
                                returnNetErrInfo(userid, iep, "登陆失败，是否输入了错误的用户名或密码!");//返回失败信息
                            }
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_LOGOUT://登出
                        {
                            Friend.Friend friend;
                            if (msg.IsNet)
                            {
                                friend = _poolFactory.findFriend(msg.NetUserID);
                            }
                            else
                            {
                                friend = _poolFactory.findFriend(msg.SendFromID);
                            }
                            if (friend == null) return;
                            serverMsgHelper.outUIFriend(friend);
                            _poolFactory.removeFriend(friend);
                            turnSendLoginOut(msg,iep);
                            writeLog((msg.IsNet ? "外网" : "内网") + "用户:" + friend.Name + "[" + friend.ID + "]" + "登出");
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT://消息
                        {
                            TurnSendMsg(msg);
                            break;
                        }

                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_FILE:
                        {
                            goto case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT;
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_MEMBER_LIST:
                        {
                            goto case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
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
        }
        static bool exsistedfriend = false;
        public static DateTime lastlistenhellofrommainserver;
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="msg"></param>
        private static void THshowMsg(MsgInfo.MsgInfo msg,IPEndPoint iep)
        {
            //if (string.IsNullOrEmpty(msg.SendFromID) || string.IsNullOrEmpty(msg.SendToID)) return;//如何为恶意消息，直接放弃
            if (string.IsNullOrEmpty(msg.SendFromID))
            {
                msg.SendFromID = iep.Address.ToString();                
            }
            if (msg.IsNet)
            {
                if (islevelserver)
                {
                    lastlistenhellofrommainserver = DateTime.Now;
                }
                exsistedfriend = false;
                Friend.Friend friend = _poolFactory.findFriend(msg.NetUserID);
                if (friend != null)
                {
                    if (!equalIP(friend.Ip, getByteIP(iep.Address.ToString())))
                    {
                        returnNetErrInfo(msg.NetUserID, iep, "此用户在其它地方重复登陆!");
                        return;//如果好友已经存在,但IP不一至则不理.
                    }
                    exsistedfriend = true;
                    friend.LastShowTime = DateTime.Now;                    
                    friend.Port = iep.Port;
                }                
            }
            if (!islevelserver)
            {
                Friend.SERVER server = _poolFactory.findServerByADDRAndPORT(iep.Address.ToString(),iep.Port);
                if (server != null)
                {
                    server.LASTCONTIME = DateTime.Now;                    
                }
            }
            //writeLog("接收到来自" + msg.SendFromID + "的消息,消息类型为:" + msg.ProtocolType.ToString());
            try
            {
                switch (msg.ProtocolType)
                {                   
                    
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_MEMBER_LIST:
                        {
                            LoginFriend(msg,iep);
                            break;
                        }                   
                    
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_IMG:
                        {
                            TurnSendMsg(msg);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_IMGPACK://好友重新请求消息包
                        {
                            TurnSendMsg(msg);
                            break;
                        }                    
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_FILE: //接收文件
                        {
                            TurnSendMsg(msg);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_FILEPACK:
                        {
                            TurnSendMsg(msg);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_DIR:
                        {
                            //if (msg.Ver.ToLower().Equals("lanmsg")) break;
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETUSER_HELLO://用户连接消息
                        {
                            returnHelloToUser(msg, iep);//返回打招呼消息
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SERVER_LOGIN://次级服务器登陆
                        {
                            if (islevelserver) break;
                            returnLevelServerLoginOK(msg,iep);
                            Friend.SERVER server = _poolFactory.findServerByADDR(iep.Address.ToString());
                            if (server == null)
                            {
                                server = new Friend.SERVER();
                                server.ADDR = iep.Address.ToString();
                                server.ID = msg.SendInfo;
                                server.PORT = iep.Port;
                                server.LASTCONTIME = DateTime.Now;
                                _poolFactory.SERVERS.Add(server);
                            }
                            else
                            {
                                server.ID = msg.SendInfo;
                                server.ADDR = iep.ToString();
                                server.PORT = iep.Port;
                                server.LASTCONTIME = DateTime.Now;
                            }
                            LoginFriend(msg, iep);//返回所有好友
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SERVER_LOGIN_ERR:
                        {
                            isloginedlevelserver = false;

                            writeLog("登陆主服务器出错：" + msg.SendInfo);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SERVER_LOGIN_OK:
                        {
                            isloginedlevelserver = true;
                            islostmastserver = false;
                            sendAllFriendsToMainServer();
                            writeLog("登录主服务器成功:" + msg.SendInfo);
                            break;
                        }
                    case MsgInfo.MsgInfo.sProtocolType.MSGTYPE_LEVELSERVER_HELLO:
                        {
                            Friend.SERVER server = _poolFactory.findServerByID(msg.NetUserID);
                            if (server != null)
                            {
                                server.LASTCONTIME = DateTime.Now;
                                server.ADDR = iep.Address.ToString();
                                server.PORT = iep.Port;
                            }
                            returnHelloToLevelServer(msg, iep);
                            break;
                        }
                    default:
                        {
                            showMsg(msg, iep);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                writeLog("helper.thshowmsg "+ ex.Message);
            }
            finally
            {
                msg.InfoGuid = "";
            }
        }
        private static void returnHelloToLevelServer(MsgInfo.MsgInfo oldinfo, IPEndPoint iep)
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo(oldinfo);
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_MAINSERVER_HELLO;
            info.SendFromID = "mainserver";
            info.SendInfo = "mainserver";
            info.SendTo = iep.Address.ToString();
            info.SendToID = iep.Address.ToString();
            info.Ver = "NetTalk";
            _poolFactory.addSendInfo(info, iep.Address.ToString(), iep.Port,true);
        }
        /// <summary>
        /// 返回连接打招呼消息
        /// </summary>
        /// <param name="oldinfo"></param>
        /// <param name="iep"></param>
        private static void returnHelloToUser(MsgInfo.MsgInfo oldinfo, IPEndPoint iep)
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
            info.InfoGuid = _guid;
            info.IsNet = true;
            info.NetUserID = "server";
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SERVER_HELLO;
            info.SendFromID = oldinfo.SendToID;
            info.SendInfo = "hello";
            info.SendToID = oldinfo.IsNet? oldinfo.NetUserID:oldinfo.SendFromID;
            info.SendTo = info.SendToID;
            info.Ver = "NetTalk";
            _poolFactory.addSendInfo(info, iep.Address.ToString(), iep.Port,true);
        }
        /// <summary>
        /// 转发消息
        /// </summary>
        /// <param name="oldinfo"></param>
        public static void TurnSendMsg(MsgInfo.MsgInfo oldinfo)
        {
            if (oldinfo.IsNet && !islevelserver)
            {
                foreach (string id in oldinfo.SendToID.Split(','))
                {
                    _poolFactory.addSendInfo(new MsgInfo.MsgInfo(oldinfo, id), new string[] { id },true);
                }               
            }
            else
            {
                if (string.IsNullOrEmpty(oldinfo.SendTo)) return;
                _poolFactory.addSendInfo(new MsgInfo.MsgInfo(oldinfo), new string[] { oldinfo.SendTo });
            }
        }
        /// <summary>
        /// 向主服务器发送所有好友
        /// </summary>
        private static void sendAllFriendsToMainServer()
        {
            for (int i = 0; i < _poolFactory.Friends.Count; i++)
            {
                MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
                info.InfoGuid = _guid;
                info.IsNet = false;
                info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_STATE;
                info.SendFromID = _poolFactory.Friends[i].ID;
                info.SendTo = mainserveraddr;
                info.SendInfo = _poolFactory.Friends[i].Name + "," + _poolFactory.Friends[i].GroupName + "," + _poolFactory.Friends[i].ID + "," + _poolFactory.Friends[i].Header;
                info.SendToID = mainserveraddr;
                info.Ver = "NetTalk";
                _poolFactory.addSendInfo(info, mainserveraddr, mainserverport, true);
            }
        }
        /// <summary>
        /// 转发消息
        /// </summary>
        /// <param name="oldinfo"></param>
        public static void TurnSendMsg(MsgInfo.MsgInfo oldinfo,IPEndPoint iep)
        {
            if (oldinfo.IsNet)
            {
                foreach (string id in oldinfo.SendToID.Split(','))
                {
                    _poolFactory.addSendInfo(new MsgInfo.MsgInfo(oldinfo, id), new string[] { id });
                }
            }
            else
            {
                if (string.IsNullOrEmpty(oldinfo.SendTo)) return;
                _poolFactory.addSendInfo(new MsgInfo.MsgInfo(oldinfo), new string[] { oldinfo.SendTo });
            }
        }
        public static void returnLevelServerLoginOK(MsgInfo.MsgInfo oldinfo,IPEndPoint iep)
        {
            MsgInfo.MsgInfo okinfo = new MsgInfo.MsgInfo(oldinfo);
            okinfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SERVER_LOGIN_OK;
            okinfo.SendFromID = "mainserver";
            okinfo.SendInfo = "您登录本服务器成功！IP:"+ iep.Address.ToString();
            okinfo.SendTo = iep.Address.ToString();
            okinfo.SendToID = oldinfo.SendFromID;
            _poolFactory.addSendInfo(okinfo, iep.Address.ToString(), iep.Port, true);
        }
        public static void returnLevelServerLoginERR(MsgInfo.MsgInfo oldinfo, IPEndPoint iep)
        {
            MsgInfo.MsgInfo okinfo = new MsgInfo.MsgInfo(oldinfo);
            okinfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SERVER_LOGIN_ERR;
            okinfo.SendFromID = "mainserver";
            okinfo.SendInfo = "您登录本服务器失败！IP:" + iep.Address.ToString();
            okinfo.SendTo = iep.Address.ToString();
            okinfo.SendToID = oldinfo.SendFromID;
            _poolFactory.addSendInfo(okinfo, iep.Address.ToString(), iep.Port, true);
        }
        /// <summary>
        /// 返回外网错误信息
        /// </summary>
        /// <param name="oldinfo"></param>
        /// <param name="iep"></param>
        public static void returnNetErrInfo(string userid, IPEndPoint iep,string errMsg)
        {
            MsgInfo.MsgInfo errInfo = new MsgInfo.MsgInfo();
            errInfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_ERR;//返回错误信息
            errInfo.InfoGuid = _guid;
            errInfo.SendFromID = "server";
            errInfo.SendToID = iep.Address.ToString();
            errInfo.SendTo = iep.Address.ToString();
            errInfo.SendInfo = errMsg;
            _poolFactory.addSendInfo(errInfo, iep.Address.ToString(), iep.Port,true);
            writeLog(userid + " [" + iep.Address.ToString() + "]" + errInfo.SendInfo);
        }
        /// <summary>
        /// 返回外网错误信息
        /// </summary>
        /// <param name="oldinfo"></param>
        /// <param name="iep"></param>
        public static void returnNetLoginOkInfo(string userid, IPEndPoint iep)
        {
            MsgInfo.MsgInfo OKInfo = new MsgInfo.MsgInfo();
            OKInfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETLOGIN_OK;//返回登录成功信息
            OKInfo.InfoGuid = _guid;
            OKInfo.SendFromID = "server";
            OKInfo.SendToID = iep.Address.ToString();
            OKInfo.SendTo = iep.Address.ToString();
            OKInfo.SendInfo = "登录成功!";
            _poolFactory.addSendInfo(OKInfo, iep.Address.ToString(), iep.Port,true);            
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
            string[] strs = strip.Split('.');
            byte[] bs = new byte[strs.Length];
            for (int i = 0; i < bs.Length;i++ )
            {
                byte.TryParse(strs[i], out bs[i]);
            }
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
                MsgInfo.MsgInfo info=_turnmsg.ServerbyteToMsg(msgbuf);
                //MemoryStream serializationStream = new MemoryStream(msgbuf);
                //Object obj = new BinaryFormatter().Deserialize(serializationStream);
                //serializationStream.Close();
                ////bool isrightpack = false;
                //try
                //{
                //    info = (MsgInfo.MsgInfo)obj;
                //    if (string.IsNullOrEmpty(info.SendFromID))
                //    {
                //        throw new Exception(" ");
                //    }
                //    info.Ver = "NetTalk";
                //    //isrightpack = true;
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
                //                //isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendFromID"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendFromID = robj.ToString();
                //                //isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendFromIp"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendFromID = getIP((byte[])robj);
                //                //isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendToID"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendToID = robj.ToString();
                //                //isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendToIp"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendToID = getIP((byte[])robj);
                //                //isrightpack = true;
                //            }
                //            else if (method.Name.Contains("get_SendInfo"))
                //            {
                //                robj = method.Invoke(obj, null);
                //                info.SendInfo = (robj == null ? null : robj.ToString());
                //                //isrightpack = true;
                //            }
                //        }
                //        catch
                //        { }
                //    }
                //    info.Ver = "LanTalk";
                //}
                if (string.IsNullOrEmpty(info.SendToID)) info.SendToID = getIP(selfIP);
                if (info.IsNet && string.IsNullOrEmpty(info.NetUserID)) return null;
                if (!string.IsNullOrEmpty(info.SendFromID) && info.SendFromID.Contains(getIP(selfIP)))
                {
                    return null;
                }
                return info;
            }
            catch (Exception ex)
            {
                //writeLog("helper.byteArrayToObject "+ex.Message);
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
        /// 转为字节地址
        /// </summary>
        /// <param name="allip"></param>
        /// <returns></returns>
        public static byte[] turnToByteIP(List<string> allip)
        {
            byte[] allbip=new byte[allip.Count * 4];
            for (int i = 0; i < allip.Count;i++ )
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
        /// 获取包含在ＩＰ中的好友的名称
        /// </summary>
        /// <param name="allip"></param>
        /// <returns></returns>
        public static string getAllName(byte[] allip)
        {
            string name = ",";
            try
            {
                byte[] curip = new byte[4];
                List<string> receivedip = new List<string>();
                for (int i = 0; i < allip.Length; i += 4)
                {
                    curip[0] = allip[i];
                    curip[1] = allip[i + 1];
                    curip[2] = allip[i + 2];
                    curip[3] = allip[i + 3];
                    receivedip.Add(getIP(curip));
                    //if (containMe(curip))
                    //{
                    //    name += MyName + ",";
                    //    continue;
                    //}
                    //name += _poolFactory.findFriend(curip).Name + ",";
                }
                for (int i = 0; i < receivedip.Count; i++)
                {
                    if (string.IsNullOrEmpty(receivedip[i])) continue;
                    List<Friend.Friend> groupfriend = _poolFactory.findFriendsByGroupName(_poolFactory.findFriend(getByteIP(receivedip[i])).GroupName);
                    if (groupfriend != null && groupfriend.Count > 0)
                    {
                        bool allgroupfriendscontian = true;
                        foreach (Friend.Friend f in groupfriend)
                        {
                            if (!receivedip.Contains(getIP(f.Ip)))
                            {
                                allgroupfriendscontian = false;
                                break;
                            }
                        }
                        if (allgroupfriendscontian)
                        {
                            name += " { " + _poolFactory.findFriend(getByteIP(receivedip[i])).GroupName + " } ,";
                            foreach (Friend.Friend f in groupfriend)
                            {
                                string curgroupnameip = getIP(f.Ip);
                                receivedip[receivedip.IndexOf(curgroupnameip)] = "";
                            }
                        }
                        else
                        {
                            name += _poolFactory.findFriend(getByteIP(receivedip[i])).Name + ",";
                        }
                    }
                }
                if (name.Equals(_poolFactory.getAllGroupName())) return " 所有人 ";
                if (!string.IsNullOrEmpty(name)) name = name.Substring(1, name.Length - 2);
            }
            catch
            { }
            return name;
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
                ConfigManager.ConfigManager config = new ConfigManager.ConfigManager(Path.Combine(Application.StartupPath,"config.xml"),_guid);
                config.writeConfig(key,value);
            }
            catch
            { }
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="strinfo"></param>
        public static void writeInfo(string strinfo)
        {
            try
            {
                MainForm.lbinfo.Items.Add("("+DateTime.Now.ToShortTimeString() +")"+strinfo);
                MainForm.lbinfo.SelectedIndex = MainForm.lbinfo.Items.Count - 1;
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
                if (startATpc)
                {
                    regkey.SetValue("NetTalkServer",Path.Combine(Application.StartupPath ,Process.GetCurrentProcess().MainModule.ModuleName));
                }
                else
                {
                    regkey.DeleteValue("NetTalkServer");
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
                writeInfo(strloginfo);
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
        /// <summary>
        /// 显示XP风格函数
        /// </summary>
        [DllImport("comctl32.dll")]
        public extern static void InitCommonControls();
        [DllImport("advapi32.dll")]
        private extern static int InitiateSystemShutdownA(string lpmachinename, string lpmessage, int dwtimeout, bool bforceappsclosed, bool brebootaftershutdown);
        public static void closeComputer()
        {
            int re=InitiateSystemShutdownA(null,"关机",0,false,false);
        }
        public static void resetComputer()
        {
            int re = InitiateSystemShutdownA(null, "重启", 0, false, true);
        }
    }
}
