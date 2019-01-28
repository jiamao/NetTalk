using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using _poolFactory = MsgPoolFactory.Factory;//消息与好友缓存
using UtilityLibrary;
using UtilityLibrary.WinControls;

namespace LanTalkServer
{
    class serverMsgHelper
    {
        static OutlookBar _groupBar=new OutlookBar();
        static System.Windows.Forms.ImageList _headerLargeImageList = new System.Windows.Forms.ImageList();
        static string _msgHistoryDir =System.IO.Path.Combine(Environment.CurrentDirectory ,"history");
        public static DateTime lastconnecttoserver;
        public static System.Windows.Forms.ImageList HeaderLargeImageList
        {
            get { return serverMsgHelper._headerLargeImageList; }
            set { serverMsgHelper._headerLargeImageList = value; }
        }
        static System.Windows.Forms.ImageList _headerSmallImageList = new System.Windows.Forms.ImageList();

        public static System.Windows.Forms.ImageList HeaderSmallImageList
        {
            get { return serverMsgHelper._headerSmallImageList; }
            set { serverMsgHelper._headerSmallImageList = value; }
        }
        static Hashtable _headerIconList = new Hashtable();
        public static Hashtable HeaderIconList
        {
            get { return serverMsgHelper._headerIconList; }
            set { serverMsgHelper._headerIconList = value; }
        }
        
        static List<OutlookBarBand> _groupList=new List<OutlookBarBand>();//所有群组

        public static List<OutlookBarBand> GroupList
        {
            get { return serverMsgHelper._groupList; }
            set { serverMsgHelper._groupList = value; }
        }
        static string _curGroupName = "";
        /// <summary>
        /// 好友列表
        /// </summary>
        public static OutlookBar GroupBar
        {
            get { return serverMsgHelper._groupBar; }
            set { serverMsgHelper._groupBar = value; }
        }
        static Thread threadsendmsg;
        /// <summary>
        /// 初始化
        /// </summary>
        public static void init()
        {
            if (threadsendmsg != null) threadsendmsg.Abort();
                threadsendmsg = new Thread(new ThreadStart(THREADSENDMSG));
                threadsendmsg.IsBackground = true;
                      
            threadsendmsg.Start();
        }
        /// <summary>
        /// 处理要发送的消息线程
        /// </summary>
        private static void THREADSENDMSG()
        {
            while (serverHelper.Iflag==0)
            {
                try
                {
                    if (_poolFactory.SendmsgINFO.Count > 0)
                    {
                        for (int i = 0; i < _poolFactory.SendmsgINFO.Count; i++)
                        {
                            try
                            {
                                if (_poolFactory.SendmsgINFO[i].Sendto == null || _poolFactory.SendmsgINFO[i].Sendto.Length == 0) continue;
                                foreach (string sendto in _poolFactory.SendmsgINFO[i].Sendto)
                                {
                                    Friend.Friend friend = _poolFactory.findFriend(sendto);
                                    if (!string.IsNullOrEmpty(sendto) && sendto.Contains("."))//IP用户
                                    {
                                        if (_poolFactory.SendmsgINFO[i].IsNET)
                                        {
                                            if (serverHelper.islevelserver)
                                            {
                                                sendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, serverHelper.mainserveraddr, serverHelper.mainserverport);
                                                lastconnecttoserver = DateTime.Now;
                                            }
                                            else
                                            {
                                                if (friend != null && !string.IsNullOrEmpty(friend.ServerID))//如果此用户为次级服务器的用户
                                                {
                                                    Friend.SERVER erver = _poolFactory.findServerByID(friend.ServerID);
                                                    if (erver != null)
                                                    {
                                                        ServersendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, erver.ADDR, erver.PORT);
                                                        continue;
                                                    }
                                                }
                                                ServersendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, sendto, _poolFactory.SendmsgINFO[i].SendPort);
                                            }
                                        }
                                        else
                                        {
                                            if (serverHelper.islevelserver && friend != null && friend.IsNet)
                                            {
                                                sendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, serverHelper.mainserveraddr, serverHelper.mainserverport);
                                                continue;
                                            }
                                            sendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, sendto);
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(sendto))//ID用户
                                    {
                                        if (friend != null)
                                        {
                                            if (serverHelper.islevelserver && serverHelper.isloginedlevelserver)
                                            {
                                                sendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, serverHelper.mainserveraddr, serverHelper.mainserverport);
                                                lastconnecttoserver = DateTime.Now;
                                            }
                                            else
                                            {
                                                ServersendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, serverHelper.getIP(friend.Ip), friend.Port);
                                            }
                                        }
                                    }
                                    Thread.Sleep(20);
                                }
                            }
                            catch (Exception ex)
                            {
                                serverHelper.writeLog("发送消息时出错" + ex.Message);
                            }
                            finally
                            {
                                _poolFactory.SendmsgINFO[i].Sendto = null;
                            }
                        }
                        _poolFactory.clearSendMsgInfo();//清除已发送的消息
                    }
                    else
                    {
                        if (serverHelper.islevelserver && lastconnecttoserver < DateTime.Now.AddSeconds(-30))
                        {
                            sendMsg(sayHelloToServer(), serverHelper.mainserveraddr, serverHelper.mainserverport);
                            lastconnecttoserver = DateTime.Now;
                        }
                        Thread.Sleep(20);
                    }
                }
                catch (Exception ex)
                {
                    serverHelper.writeLog("msghelper.THREADSENDMSG "+ex.Message);
                    _poolFactory.clearSendMsgInfo();//清除已发送的消息
                }
            }
        }
        private static MsgInfo.MsgInfo sayHelloToServer()
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
            info.InfoGuid = serverHelper._guid;
            info.IsNet = true;
            info.NetUserID = serverHelper.serverguid;
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_LEVELSERVER_HELLO;
            info.SendFromID = serverHelper.serverguid;
            info.SendInfo = "hello";
            info.SendTo = serverHelper.serverguid;
            info.SendToID = serverHelper.serverguid;
            info.Ver = "NetTalk";
            return info;
        }
        public static void initimagelist()
        {
           
        }
        /// <summary>
        /// 查找群组名
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public static OutlookBarBand findGroupByName(string groupname)
        {
            _curGroupName = groupname;
            return _groupList.Find(findCurGroup);
        }
        private static bool findCurGroup(OutlookBarBand obb)
        {
            return obb.Name.Equals(_curGroupName);
        }
        /// <summary>
        /// 检查用户是否已是好友
        /// </summary>
        /// <param name="friend"></param>
        public static void addUIFriend(Friend.Friend friend,System.Net.IPEndPoint iep)
        {
            Friend.Friend f = _poolFactory.findFriend(friend.ID);
            if (f == null)
            {
                addnewitem(friend);
            }
            else
            {
                if (friend.IsNet)
                {
                    ListViewItem lvi = findNetItemByID(friend.ID);
                    if (lvi == null)
                    {
                        addnewitem(friend);                        
                    }
                    else
                    {
                        lvi.SubItems[0].Text = friend.ID;
                        lvi.SubItems[2].Text = friend.Name;
                        lvi.SubItems[3].Text = getToolTip(friend.State);
                        lvi.SubItems[4].Text = serverHelper.getIP(friend.Ip);
                        lvi.SubItems[5].Text = friend.Port.ToString();
                        lvi.SubItems[6].Text = friend.Ver;
                        if (lvi.SubItems[1].Text != friend.GroupName)
                        {
                            lvi.SubItems[1].Text = friend.GroupName;
                            //sortItemByGroupName(friend.IsNet);
                        }
                        
                    }
                }
                else
                {
                    ListViewItem lvi = findLanItemByID(friend.ID);
                    if (lvi == null)
                    {
                        addnewitem(friend);
                    }
                    else
                    {
                        lvi.SubItems[0].Text = friend.ID;                        
                        lvi.SubItems[2].Text = friend.Name;
                        lvi.SubItems[3].Text = getToolTip(friend.State);
                        lvi.SubItems[4].Text = serverHelper.getIP(friend.Ip);
                        lvi.SubItems[5].Text = friend.Port.ToString();
                        lvi.SubItems[6].Text = friend.Ver;
                        if (lvi.SubItems[1].Text != friend.GroupName)
                        {
                            lvi.SubItems[1].Text = friend.GroupName;
                            //sortItemByGroupName(friend.IsNet);
                        }
                    }
                }
            }
        }
        public static void addnewitem(Friend.Friend friend)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Name = friend.ID;
            
            //lvi.ToolTipText = getToolTip(friend.State);
            if (friend.IsNet)
            {
                lvi.SubItems.AddRange(new string[] { " ", "", "", "", "", "" });
                lvi.SubItems[0].Text = friend.ID;
                lvi.SubItems[1].Text = friend.GroupName;
                lvi.SubItems[2].Text = friend.Name;
                lvi.SubItems[3].Text = getToolTip(friend.State);
                lvi.SubItems[4].Text = serverHelper.getIP(friend.Ip);
                lvi.SubItems[5].Text = friend.Port.ToString();
                lvi.SubItems[6].Text = friend.Ver;
                serverHelper.MainForm.lvnet.Items.Add(lvi);                
            }
            else
            {
                lvi.SubItems.AddRange(new string[] { " ", "", "", "", "",""});
                lvi.SubItems[0].Text = friend.ID;
                lvi.SubItems[1].Text = friend.GroupName;
                lvi.SubItems[2].Text = friend.Name;
                lvi.SubItems[3].Text = getToolTip(friend.State);
                lvi.SubItems[4].Text = serverHelper.getIP(friend.Ip);
                lvi.SubItems[5].Text = friend.Port.ToString();
                lvi.SubItems[6].Text = friend.Ver;
                serverHelper.MainForm.lvlan.Items.Add(lvi);                
            }            
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="isnet"></param>
        public static void sortItemByGroupName(bool isnet)
        {
            try
            {
                if (isnet)
                {
                    List<string> groups = findAllGroup();
                    serverHelper.MainForm.lvnet.Items.Clear();
                    foreach (string group in groups)
                    {
                        List<Friend.Friend> friends = _poolFactory.findFriendsByGroupName(group);
                        foreach (Friend.Friend f in friends)
                        {
                            if (!f.IsNet) continue;
                            addnewitem(f);
                        }
                    }
                }
                else
                {
                    List<string> groups = findAllGroup();
                    serverHelper.MainForm.lvlan.Items.Clear();
                    foreach (string group in groups)
                    {
                        List<Friend.Friend> friends = _poolFactory.findFriendsByGroupName(group);
                        foreach (Friend.Friend f in friends)
                        {
                            if (f.IsNet) continue;
                            addnewitem(f);
                        }
                    }  
                }
            }
            catch
            { }
        }
        private static List<string> findAllGroup()
        {
            List<string> groups = new List<string>();
            for (int i = 0; i < _poolFactory.Friends.Count; i++)
            {
                if (!groups.Contains(_poolFactory.Friends[i].GroupName))
                {
                    groups.Add(_poolFactory.Friends[i].GroupName);
                }
            }
            return groups;
        }
        /// <summary>
        /// 查找内网用户项
        /// </summary>
        /// <param name="friendid"></param>
        /// <returns></returns>
        public static ListViewItem findLanItemByID(string friendid)
        {
            ListViewItem[] lvts= serverHelper.MainForm.lvlan.Items.Find(friendid, false);
            if (lvts == null || lvts.Length <= 0)
            {
                return null;
            }
            return lvts[0];
        }
        /// <summary>
        /// 查找外网用户项
        /// </summary>
        /// <param name="friendid"></param>
        /// <returns></returns>
        public static ListViewItem findNetItemByID(string friendid)
        {
            ListViewItem[] lvts = serverHelper.MainForm.lvnet.Items.Find(friendid, false);
            if (lvts == null || lvts.Length <= 0)
            {
                return null;
            }
            return lvts[0];
        }
        private static void addGroupNode(out TreeNode group, Friend.Friend friend)
        {
            group = null;
        }
        /// <summary>
        /// 获取当前提示
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string getToolTip(Friend.Friend.EState state)
        {
            switch (state)
            {
                case Friend.Friend.EState.BSYou:
                    {
                        return "懒得理你！！！";
                    }
                case Friend.Friend.EState.Busy:
                    {
                        return "忙碌中！无重要事情请勿打扰.";
                    }
                case Friend.Friend.EState.InLine:
                    {
                        return "我在线！";
                    }
                case Friend.Friend.EState.Out:
                    {
                        return "人已外出，稍候回复您！";
                    }
                default:
                    return "未知状态";
            }
        }
        /// <summary>
        /// 登出好友
        /// </summary>
        /// <param name="friend"></param>
        public static void outUIFriend(Friend.Friend friend)
        {
            try
            {
                if (friend.IsNet)
                {
                    serverHelper.MainForm.lvnet.Items.RemoveByKey(friend.ID);
                }
                else
                {
                    serverHelper.MainForm.lvlan.Items.RemoveByKey(friend.ID);
                }
            }
            catch
            { }
        }
        
        public static OutlookBarItem getItemByIP(string friendid)
        {
            try
            {
                foreach (OutlookBarBand obb in _groupList)
                {
                    foreach (OutlookBarItem obi in obb.Items)
                    {
                        if (obi.Tag.Equals(friendid)) return obi;
                    }
                }
            }
            catch
            { }
            return null;
        }
        /// <summary>
        /// 获取头像的索引
        /// </summary>
        /// <param name="imagename"></param>
        /// <returns></returns>
        public static int getImageIndex(string imagename)
        {
            int imageindex = _headerLargeImageList.Images.IndexOfKey(imagename);
            imageindex = imageindex < 0 ? 2 : imageindex;
            return imageindex;
        }
        /// <summary>
        /// 获取图像
        /// </summary>
        /// <param name="imgname"></param>
        /// <returns></returns>
        public static Image getImageByName(string imgname)
        {
            try
            {
                Image img= _headerLargeImageList.Images[imgname];
                if (img == null) return _headerLargeImageList.Images["_131"];
                return img;
            }
            catch
            {
                return _headerLargeImageList.Images["_131"];
            }
        }
        public static Icon getIconByName(string iconname)
        {
            try
            {
                Icon img = _headerIconList[iconname] as Icon;
                if (img == null) return serverHelper.DefaultIcon;
                return img;
            }
            catch
            {
                return serverHelper.DefaultIcon;
            }
        }
        
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="info"></param>
        public static void sendMsg(object info,string ip)
        {
            serverHelper._send.SendBytes(serverHelper.ojectToByteArray(info),ip);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="info"></param>
        public static void sendMsg(object info, string ip,int port)
        {
            serverHelper._send.SendBytes(serverHelper.ojectToByteArray(info), ip,port);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="info"></param>
        public static void ServersendMsg(object info, string ip, int port)
        {
            serverHelper.listener.serverSendMsg(serverHelper.ojectToByteArray(info), ip, port);
        }
        public static void sendMsg(string msg, string ip)
        {
           
        }
        public static void sendMsg(string msg, string ip,string strip)
        {
           
        }
        /// <summary>
        /// 发送传输文件消息
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="curip"></param>
        /// <param name="ballip"></param>
        public static void sendFileMsg(string filename,string[] curips,byte[] ballip)
        {
           
        }
        /// <summary>
        /// 接收文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="curip"></param>
        public static void sendGetFileMsg(string filename, byte[] curip)
        {
            
        }
        /// <summary>
        /// 重新请求文件包
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="curip"></param>
        /// <param name="index"></param>
        /// <param name="guid"></param>
        public static void sendGetFilePackIndex(string filename, byte[] curip, int index,string guid)
        {
            
        }
        /// <summary>
        /// 发送重请的包
        /// </summary>
        /// <param name="info"></param>
        public static void sendFilePackIndex(MsgInfo.MsgInfo info)
        {
            
           
        }
        public static MsgInfo.MsgInfo fileinfo;
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="info"></param>
        public static void sendFile()
        {
            
        }
        private static void getBSPart(int i, out byte[] buffer,byte[] bs, int lastpart, int ipart,int count)
        {
            if (i >= count) { buffer = null; return; }
            if (i == count - 1)
            {
                buffer = new byte[lastpart];
            }
            else
            {
                buffer = new byte[ipart];
            }
            for (int j = 0; j < buffer.Length; j++)
            {
                buffer[j] = bs[i * ipart + j];
            }
            i++;
        }
        static Hashtable usedport = new Hashtable();//端口正在使用情况
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="_s"></param>
        /// <param name="bs"></param>
        /// <param name="filename"></param>
        /// <param name="sendto"></param>
        /// <param name="guid"></param>
        /// <param name="index"></param>
        /// <param name="port"></param>
        public static void TCPsendFile(MsgSend.MsgSend _s, byte[] bs, string filename, byte[] sendto, string guid, int index,int port)
        {
            try
            {
                _s.sendFile(bs, serverHelper.getIP(sendto), port);
                Thread.Sleep(5);                
            }
            finally
            {
                //usedport[port] = false;
            }
        }
        private delegate void desendmsg(string[] curiips, string rtf, Color c, Font f, byte[] ballip);
        private static desendmsg _sendmsg;
        
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="curiip"></param>
        /// <param name="rtf"></param>
        /// <param name="c"></param>
        /// <param name="f"></param>
        /// <param name="ballip"></param>
        public static void sendMsg(string[] curiips, string rtf, byte[] ballip)
        {
            thsendip = curiips;
            thsendrtf = rtf;
            thsendips = ballip;
            Thread threadmsg = new Thread(new ThreadStart(threadSendMsg));
            threadmsg.IsBackground = true;
            threadmsg.Start();            
        }
        static string[] thsendip;
        static string thsendrtf;        
        static byte[] thsendips;
        static bool portused = false;
        private static void threadSendMsg()
        {
            try
            {
                string[] sendip = thsendip;
                string sendrtf = thsendrtf;
                byte[] sendips = thsendips;
                
                sendmsg(sendip, sendrtf, Color.Black, null, sendips);
                sendrtf = string.Empty;
                           
            }
            catch (Exception ex)
            {
                serverHelper.writeLog("msghelper.threadSendMsg "+ex.Message);
            }           
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="curiip"></param>
        public static void sendmsg(string[] curiips, string rtf, Color c, Font f, byte[] ballip)
        {
            
        }
        /// <summary>
        /// 转发消息
        /// </summary>
        /// <param name="oldinfo"></param>
        public static void turnSendTextMsg(MsgInfo.MsgInfo oldinfo,System.Net.IPEndPoint iep)
        {
            //Friend.Friend friend;
            //friend = _poolFactory.findFriend(oldinfo.SendToID);
            //if (friend != null)
            //{
            //    if(!friend.Ver.Equals("NetTalk"))
            //    {
            //    WinLanMsg.Util.UserInfo info = new WinLanMsg.Util.UserInfo();
            //    info.ProtocolType = 5;
            //    info.SendFromIp = serverHelper.selfIP;
            //    info.SendToIp = serverHelper.getByteIP("255.255.255.255");
            //    info.SendInfo = oldinfo.SendInfo;
            //    _poolFactory.addSendInfo(info,new string[]{"255.255.255.255"},friend.Port);
            //    }
            //    else
            //    {
            //        _poolFactory.addSendInfo(oldinfo,oldinfo,friend.Port);
            //    }
            //}            
        }
        /// <summary>
        /// 重新获取丢失的包
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="index"></param>
        public static void getLostedImgPack(string guid, string index,string friendip)
        {
            
        }
        /// <summary>
        /// 重新发送好友丢失并重新请求的包
        /// </summary>
        /// <param name="info"></param>
        public static void sendRequestImgPack(MsgInfo.MsgInfo info)
        {
            string guid = info.InfoType;
            string index = info.SendInfo;
            MsgInfo.MsgInfo lostedinfo = _poolFactory.findSendedImgPart(guid,index);
            if (lostedinfo == null) return;
            _poolFactory.addSendInfo(lostedinfo, new string[] { info.SendFromID});
        }
        private static bool findSavedHistory(string[][] history)
        {
            return history[0][0].Equals("saved");
        }
        /// <summary>
        /// 转发内网外网消息
        /// </summary>
        /// <param name="info"></param>
        public static void turnMsgInLanAndNet(MsgInfo.MsgInfo info)
        {
            
        }
        #region 查找窗体
        
        [DllImport("User32.dll")]
        private static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("User32.dll")]
        public static extern bool FlashWindow(IntPtr hwnd, bool binvert);
        public static IntPtr FindWindow(string windowname)
        {
            //
            return FindWindow(null,windowname);
        }

        public static void findWinAndFlash(string winName)
        {
            IntPtr win = FindWindow(winName);
            if(win !=IntPtr.Zero)
            {                
                //for (int i = 0; i < 10; i++)
                //{
                    FlashWindow(win, false);
                //    Application.DoEvents();                    
                //}                
            }
        }
        #endregion
    }
}
