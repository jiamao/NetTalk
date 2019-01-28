using System;
using System.Collections.Generic;
using System.Text;

namespace MsgPoolFactory
{
    public class Factory:IDisposable
    {
        static string _curFriendID="";
        static List<receiveMsgs> _messages = new List<receiveMsgs>();

        static int _lockedfilebyte = 2;

        public static int Lockedfilebyte
        {
            get { return Factory._lockedfilebyte; }
            set { Factory._lockedfilebyte = value; }
        }
        static List<List<byte[]>> _filebyte1 = new List<List<byte[]>>();
        static List<List<byte[]>> _filebyte2 = new List<List<byte[]>>();
        static List<MsgInfo.MsgInfo> _sendedMsgImg = new List<MsgInfo.MsgInfo>();
        static System.Collections.Hashtable _sendedMsgImpLife = new System.Collections.Hashtable();
        static byte[] _curFriendIP = new byte[4];
        static List<MsgInfo.MsgInfo> _msg = new List<MsgInfo.MsgInfo>();
        static List<MsgInfo.MsgInfo> _img = new List<MsgInfo.MsgInfo>();
        static List<MsgInfo.MsgInfo> _filemsg = new List<MsgInfo.MsgInfo>();
        static List<MsgInfo.FilePack> _filepack = new List<MsgInfo.FilePack>();
        static List<SendMsgInfo> _sendmsgINFO = new List<SendMsgInfo>();
        static List<UIMsgs> _receivedTextMsg = new List<UIMsgs>();
        static List<Friend.SERVER> _Servers = new List<Friend.SERVER>();

        public static List<UIMsgs> ReceivedTextMsg
        {
            get { return Factory._receivedTextMsg; }
            set { Factory._receivedTextMsg = value; }
        }
        public static void removeReaded()
        {
            _receivedTextMsg.RemoveAll(removeAllReadedMsg);
        }
        private static bool removeAllReadedMsg(UIMsgs info)
        {
            return string.IsNullOrEmpty(info.Info.InfoGuid);
        }
        public static List<SendMsgInfo> SendmsgINFO
        {
            get { return Factory._sendmsgINFO; }
            set { Factory._sendmsgINFO = value; }
        }
        public static void addSendInfo(object msginfo, string[] sendto)
        {
            SendMsgInfo sendinfo = new SendMsgInfo();
            sendinfo.Sendto = sendto;
            sendinfo.LantalkMsg = msginfo;
            _sendmsgINFO.Add(sendinfo);
        }
        public static void addSendInfo(object msginfo, string[] sendto,bool isnet)
        {
            SendMsgInfo sendinfo = new SendMsgInfo();
            sendinfo.Sendto = sendto;
            sendinfo.LantalkMsg = msginfo;
            sendinfo.IsNET = isnet;
            _sendmsgINFO.Add(sendinfo);
        }
        public static void addSendInfo(object msginfo, string sendto,int port)
        {
            SendMsgInfo sendinfo = new SendMsgInfo();
            sendinfo.Sendto = new string[] { sendto };
            sendinfo.SendPort = port;
            sendinfo.LantalkMsg = msginfo;
            _sendmsgINFO.Add(sendinfo);
        }
        public static void addSendInfo(object msginfo, string sendto, int port,bool isnet)
        {
            SendMsgInfo sendinfo = new SendMsgInfo();
            sendinfo.Sendto = new string[] { sendto };
            sendinfo.SendPort = port;
            sendinfo.LantalkMsg = msginfo;
            sendinfo.IsNET = isnet;
            _sendmsgINFO.Add(sendinfo);
        }
        public static void addSendInfo(string msginfo, string[] sendto,string sendtobips)
        {
            SendMsgInfo sendinfo = new SendMsgInfo();
            sendinfo.Sendtobip = sendtobips;
            sendinfo.Sendto = sendto;
            sendinfo.LanmsgRTF = msginfo;
            _sendmsgINFO.Add(sendinfo);
        }
        public static void clearSendMsgInfo()
        {
            _sendmsgINFO.RemoveAll(getAllSendedInfo);
        }
        private static bool getAllSendedInfo(SendMsgInfo sended)
        {
            return sended.Sendto == null || sended.Sendto.Length == 0;
        }
        public static List<MsgInfo.MsgInfo> SendedMsgImg
        {
            get { return Factory._sendedMsgImg; }
            set { Factory._sendedMsgImg = value; }
        }
        public static System.Collections.Hashtable SendedMsgImpLife
        {
            set { _sendedMsgImpLife = value; }
            get { return _sendedMsgImpLife; }
        }
        public static List<MsgInfo.MsgInfo> Filemsg
        {
            get { return Factory._filemsg; }
            set { Factory._filemsg = value; }
        }
        public static List<MsgInfo.FilePack> FilePack
        {
            get { return Factory._filepack; }
            set { Factory._filepack = value; }
        }
        public static List<MsgInfo.MsgInfo> Img
        {
            get { return Factory._img; }
            set { Factory._img = value; }
        }
        public static List<MsgInfo.MsgInfo> Msg
        {
            get { return Factory._msg; }
            set { Factory._msg = value; }
        }
        public static List<receiveMsgs> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }
        public static List<List<byte[]>> FileByte1
        {
            get { return _filebyte1; }
            set { _filebyte1 = value; }
        }
        public static List<List<byte[]>> FileByte2
        {
            get { return _filebyte2; }
            set { _filebyte2 = value; }
        }
        static List<Friend.Friend> _friends = new List<Friend.Friend>();

        public static List<Friend.Friend> Friends
        {
            get { return _friends; }
            set { _friends = value; }
        }
        public static List<Friend.SERVER> SERVERS
        {
            set { _Servers = value; }
            get { return _Servers; }
        }
        static string _curServerID;
        public static Friend.SERVER findServerByID(string serverid)
        {
            try
            {
                _curServerID = serverid;
                return _Servers.Find(delegtefindserverbyid);
            }
            catch
            {
                return null;
            }
        }
        private static bool delegtefindserverbyid(Friend.SERVER server)
        {
            return server.ID.Equals(_curServerID);
        }
        static string _curServerIP;
        public static Friend.SERVER findServerByADDR(string serveraddr)
        {
            try
            {
                _curServerIP = serveraddr;
                return _Servers.Find(delegtefindserverbyaddr);
            }
            catch
            {
                return null;
            }
        }
        private static bool delegtefindserverbyaddr(Friend.SERVER server)
        {
            return server.ADDR.Equals(_curServerIP);
        }
        /// <summary>
        /// 称除所有超时的服务器
        /// </summary>
        public static void removeOutTimeServers()
        {
            try
            {
                _Servers.RemoveAll(delegeteremoveallouttimeservers);
            }
            catch
            { }
        }
        private static bool delegeteremoveallouttimeservers(Friend.SERVER server)
        {
            try
            {
                return string.IsNullOrEmpty(server.ID);
            }
            catch
            {
                return false;
            }
        }
        static string _curServerADDR;
        static int _curServerPort;
        public static Friend.SERVER findServerByADDRAndPORT(string serveraddr, int port)
        {
            try
            {
                _curServerPort = port;
                _curServerADDR = serveraddr;
                return _Servers.Find(delegtefindserverbyaddrandport);
            }
            catch
            {
                return null;
            }
        }
        private static bool delegtefindserverbyaddrandport(Friend.SERVER server)
        {
            return server.ADDR.Equals(_curServerADDR) && server.PORT.Equals(_curServerPort);
        }
        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="friend"></param>
        public static void removeFriend(Friend.Friend friend)
        {
            //_curFriendID = friend.ID;
            _friends.Remove(findFriend(friend.Ip));
        }
        /// <summary>
        /// 通过ＧＵＩＤ获取图片
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static MsgInfo.MsgInfo findImg(string guid)
        {
            _guid = guid;
            return _img.Find(findimg);
        }
        static string _guid;
        private static bool findimg(MsgInfo.MsgInfo info)
        {
            return info ==null || info.InfoType.Equals(_guid);
        }
        static string _removeguid;
        private static bool findremoveimg(MsgInfo.MsgInfo info)
        {
            return info == null || info.InfoType.Equals(_removeguid);
        }
        /// <summary>
        /// 移除所有当前项
        /// </summary>
        /// <param name="guid"></param>
        public static void removeImgByGuid(string guid)
        {
            _removeguid = guid;
            _img.RemoveAll(findremoveimg);
        }
        /// <summary>
        /// 通过ＧＵＩＤ获取图片的某一部分
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static MsgInfo.MsgInfo findImgPart(string guid,string index)
        {
            _guid = guid;
            _index=index;
            return _img.Find(findimgpart);
        }
        static string _index;
        private static bool findimgpart(MsgInfo.MsgInfo info)
        {
            return info!=null && info.InfoType.Equals(_guid) && info.SendInfo.StartsWith(_index);
        }
        static string _sendedguid;
        static string _sendedindex;
        /// <summary>
        /// 通过ＧＵＩＤ获取已发送的图片的某一部分
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static MsgInfo.MsgInfo findSendedImgPart(string guid, string index)
        {
            _sendedguid = guid;
            _sendedindex = index;
            return _sendedMsgImg.Find(findsendedimgpart);
        }
        
        private static bool findsendedimgpart(MsgInfo.MsgInfo info)
        {
            return info.InfoType.Equals(_sendedguid) && info.SendInfo.StartsWith(_sendedindex);
        }
        static string _removesendedguid;
        /// <summary>
        /// 移除已发送消息的缓存
        /// </summary>
        /// <param name="guid"></param>
        public static void removeSendedImgsByGuid(string guid)
        {
            _removesendedguid = guid;
            _sendedMsgImg.RemoveAll(findremovesendedimgpart);
            _filemsg.RemoveAll(findremovesendedimgpart);
        }
        private static bool findremovesendedimgpart(MsgInfo.MsgInfo info)
        {
            return info.InfoType.Equals(_removesendedguid) || info.InfoGuid.Equals(_removesendedguid);
        }
        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="friend"></param>
        public static void removeFriend(string friendID)
        {
            _curFriendID = friendID;
            _curFriendIP = new byte[4];
            _friends.Remove(_friends.Find(findCurFrind));
        }
        /// <summary>
        /// 查找好友
        /// </summary>
        /// <param name="friendIP"></param>
        /// <returns></returns>
        public static Friend.Friend findFriend(string friendID)
        {
            _curFriendID = friendID;
            _curFriendIP = new byte[4];
            return _friends.Find(findCurFrind);
        }
        static string findfriendbyserverid;
        /// <summary>
        /// 通过所在服务器查找好友
        /// </summary>
        /// <param name="serverid"></param>
        /// <returns></returns>
        public static List<Friend.Friend> findFriendsByServerID(string serverid)
        {
            try
            {
                findfriendbyserverid = serverid;
                return _friends.FindAll(delegetefindfriendbyserverid);
            }
            catch
            {
                return null;
            }
        }
        private static bool delegetefindfriendbyserverid(Friend.Friend friend)
        {
            try
            {
                return friend.ServerID.Equals(findfriendbyserverid);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 查找好友通过ＩＤ和服务器ＩＤ
        /// </summary>
        /// <param name="friendid"></param>
        /// <param name="serverid"></param>
        /// <returns></returns>
        public static Friend.Friend findFriendByServerIDAndFriendID(string friendid, string serverid)
        {
            findfriendbyserveridandfriendidfriendid = friendid;
            findfriendbyserveridandfriendidserverid = serverid;
            return _friends.Find(delegetefindfriendbyserveridandfriendid);
        }
        static string findfriendbyserveridandfriendidfriendid;
        static string findfriendbyserveridandfriendidserverid;
        private static bool delegetefindfriendbyserveridandfriendid(Friend.Friend friend)
        {
            try
            {
                return friend.ID.Equals(findfriendbyserveridandfriendidfriendid) && friend.ServerID.Equals(findfriendbyserveridandfriendidserverid);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 查找所有外网好友
        /// </summary>
        /// <returns></returns>
        public static List<Friend.Friend> frinNetFriends()
        {
            return _friends.FindAll(findnetfriend);
        }
        private static bool findnetfriend(Friend.Friend friend)
        {
            return friend.IsNet;
        }
        /// <summary>
        /// 查找好友
        /// </summary>
        /// <param name="friendIP"></param>
        /// <returns></returns>
        public static Friend.Friend findFriend(byte[] friendIP)
        {
            _curFriendIP = friendIP;
            _curFriendID = "";
            return _friends.Find(findCurFrind);
        }
        private static bool findCurFrind(Friend.Friend friend)
        {
            try
            {
                return friend.ID.Equals(_curFriendID) || (friend.Ip[0].Equals(_curFriendIP[0]) && friend.Ip[1].Equals(_curFriendIP[1]) && friend.Ip[2].Equals(_curFriendIP[2]) && friend.Ip[3].Equals(_curFriendIP[3]));
            }
            catch
            {
                return false;
            }
        }
        static string _curgroupname;
        /// <summary>
        /// 通过群组查找好友
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public static List<Friend.Friend> findFriendsByGroupName(string groupname)
        {
            _curgroupname = groupname;
            return _friends.FindAll(findfriendsbygroupname);
        }
        private static bool findfriendsbygroupname(Friend.Friend f)
        {
            return f.GroupName.Equals(_curgroupname);
        }
        /// <summary>
        /// 获取所有组名
        /// </summary>
        /// <returns></returns>
        public static string getAllGroupName()
        {
            string allgroupnames = ",";
            foreach (Friend.Friend f in _friends)
            {
                if (!allgroupnames.Contains(f.GroupName))
                {
                    allgroupnames += " { " + f.GroupName + " } ,";
                }
            }
            return allgroupnames;
        }
        /// <summary>
        /// 移除所有好友
        /// </summary>
        public static void removeAllFriends()
        {
            _friends.Clear();
        }
        /// <summary>
        /// 查找文件消息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static MsgInfo.MsgInfo findFileMsg(byte[] ip, string filename)
        {
            _curfileip = ip;
            _curfilename = filename;            
            return _filemsg.Find(findfile);
        }
        /// <summary>
        /// 查找文件消息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static MsgInfo.MsgInfo findFileMsg(string guid)
        {           
            _fileguid = guid;
            return _filemsg.Find(findfilebyguid);
        }
        public static void removeFileMsgByGuid(string guid)
        {
            try
            {
                _fileguid = guid;
                _filemsg.RemoveAll(findfilebyguid);
            }
            catch
            { }
        }
        static string _fileguid;
        static byte[] _curfileip;
        static string _curfilename;
        private static bool findfilebyguid(MsgInfo.MsgInfo info)
        {
            try
            {
                return info.InfoGuid.Equals(_fileguid);
            }
            catch
            {
                return false;
            }
        }
        private static bool findfile(MsgInfo.MsgInfo info)
        {
            try
            {
                return (info.SendFromID[0].Equals(_curfileip[0]) && info.SendFromID[1].Equals(_curfileip[1]) && info.SendFromID[2].Equals(_curfileip[2]) && info.SendFromID[3].Equals(_curfileip[3]) && System.IO.Path.GetFileName(info.SendInfo).Equals(_curfilename));
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 加入文件包
        /// </summary>
        /// <param name="filepack"></param>
        public static void addFilePack(MsgInfo.FilePack filepack)
        {
            _filepack.Add(filepack);
        }
        static string _filepackguid;
        /// <summary>
        /// 通过GUID移除文件包
        /// </summary>
        /// <param name="guid"></param>
        public static void removeFilePackByGuid(string guid)
        {
            _filepackguid = guid;
            _filepack.RemoveAll(findfilepack);
        }
        private static bool findfilepack(MsgInfo.FilePack fp)
        {
            return fp.Guid.Equals(_filepackguid);
        }
        static string findfpguid;
        static int findfpindex;
        /// <summary>
        /// 查找文件包
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static MsgInfo.FilePack findFilePackByGuidAndIndex(string guid, int index)
        {
            try
            {
                findfpguid = guid;
                findfpindex = index;
                return _filepack.Find(findfilepackbyguidandindex);
            }
            catch
            {
                return null;
            }

        }
        private static bool findfilepackbyguidandindex(MsgInfo.FilePack fp)
        {
            try
            {
                return fp.Guid.Equals(findfpguid) && fp.Index == findfpindex;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 移除已读消息
        /// </summary>
        public static void removeReadedMsg()
        {
            _msg.RemoveAll(findreadedmsg);
            //_filemsg.RemoveAll(findreadedmsg);
        }
        private static bool findreadedmsg(MsgInfo.MsgInfo info)
        {
            return info.Ver.Equals("readed");
        }
        /// <summary>
        /// 重设或添回好友
        /// </summary>
        /// <param name="friend"></param>
        public static void resetFriend(Friend.Friend myfriend)
        {
            Friend.Friend friended = findFriend(myfriend.ID);
            if (friended != null)
            {
                myfriend.Messages = friended.Messages;
            }
            _friends.Remove(friended);
            _friends.Add(myfriend);
        }
        public static void clear()
        {
            _friends.Clear();
            _img.Clear();
            _messages.Clear();
            _msg.Clear();
            _filebyte1.Clear();
            _filebyte2.Clear();
            _filemsg.Clear();
            _sendedMsgImg.Clear();
            _sendedMsgImpLife.Clear();
            GC.Collect();
        }
        #region 消息队列
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="msg"></param>
        public static void addMsg(receiveMsgs msg)
        {
            _messages.Add(msg);
        }
        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="msg"></param>
        public static void removeMsg(receiveMsgs msg)
        {
            _messages.Remove(msg);
        }
        /// <summary>
        /// 移除所有消息
        /// </summary>
        public static void removeAllMsg(int count)
        {
            _messages.RemoveRange(0,count);
            _messages.RemoveAll(removeallnullmsg);
        }
        private static bool removeallnullmsg(receiveMsgs msg)
        {
            return msg == null;
        }
        #endregion
        #region IDisposable 成员

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose();
        }

        #endregion
    }
}
