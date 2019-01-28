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
using FileCompress;
using UtilityLibrary;
using UtilityLibrary.WinControls;

namespace LanTalk
{
    class MsgHelper
    {
        static OutlookBar _groupBar=new OutlookBar();
        static System.Windows.Forms.ImageList _headerLargeImageList = new System.Windows.Forms.ImageList();
        static string _msgHistoryDir =System.IO.Path.Combine(Application.StartupPath ,"history");
        public static System.Windows.Forms.ImageList HeaderLargeImageList
        {
            get { return MsgHelper._headerLargeImageList; }
            set { MsgHelper._headerLargeImageList = value; }
        }
        static System.Windows.Forms.ImageList _headerSmallImageList = new System.Windows.Forms.ImageList();

        public static System.Windows.Forms.ImageList HeaderSmallImageList
        {
            get { return MsgHelper._headerSmallImageList; }
            set { MsgHelper._headerSmallImageList = value; }
        }
        static List<Image> _MsgFaceList = new List<Image>();

        public static List<Image> MsgFaceList
        {
            get { return MsgHelper._MsgFaceList; }
            set { MsgHelper._MsgFaceList = value; }
        }
        static Hashtable _headerIconList = new Hashtable();
        public static Hashtable HeaderIconList
        {
            get { return MsgHelper._headerIconList; }
            set { MsgHelper._headerIconList = value; }
        }
        static ResourceManager _resourceManagerHeader = ResourceUserHeader.ResourceManager;
        /// <summary>
        /// 用户头像管理器
        /// </summary>
        public static ResourceManager ResourceManagerHeader
        {
            get { return MsgHelper._resourceManagerHeader; }
            set { MsgHelper._resourceManagerHeader = value; }
        }
        //static ResourceManager _resourceManagerInfoImg = ResourceInfoIMG.ResourceManager;
        /// <summary>
        /// 消息图片管理器
        /// </summary>
        //public static ResourceManager ResourceManagerInfoImg
        //{
        //    get { return MsgHelper._resourceManagerInfoImg; }
        //    set { MsgHelper._resourceManagerInfoImg = value; }
        //}
        static List<OutlookBarBand> _groupList=new List<OutlookBarBand>();//所有群组

        public static List<OutlookBarBand> GroupList
        {
            get { return MsgHelper._groupList; }
            set { MsgHelper._groupList = value; }
        }
        static string _curGroupName = "";
        /// <summary>
        /// 好友列表
        /// </summary>
        public static OutlookBar GroupBar
        {
            get { return MsgHelper._groupBar; }
            set { MsgHelper._groupBar = value; }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void init()
        {
            initimagelist();            
            Thread threadsendmsg = new Thread(new ThreadStart(THREADSENDMSG));
            threadsendmsg.IsBackground = true;
            threadsendmsg.Start();
        }
        static DateTime sayhellotoservertime;
        public static int sayhellotimes = -30;
        public static bool netlogined = false;
        /// <summary>
        /// 定时发送消息
        /// </summary>
        private static void THREADSENDMSG()
        {
            while (Helper.Iflag==0)
            {
                try
                {
                    if (_poolFactory.SendmsgINFO.Count > 0)
                    {
                        for (int i = 0; i < _poolFactory.SendmsgINFO.Count; i++)
                        {
                            if (_poolFactory.SendmsgINFO[i].Sendto == null || _poolFactory.SendmsgINFO[i].Sendto.Length == 0) continue;
                            foreach (string sendto in _poolFactory.SendmsgINFO[i].Sendto)
                            {
                                string receiver = sendto;
                                if (!Helper.IsNet && !sendto.Contains("."))
                                {
                                    Friend.Friend f = _poolFactory.findFriend(sendto);
                                    if (f == null) continue;
                                    receiver = Helper.getIP(f.Ip);
                                }
                                Thread.Sleep(20);
                                if (string.IsNullOrEmpty(_poolFactory.SendmsgINFO[i].LanmsgRTF))
                                {
                                    //如果做为外网用户
                                    if (Helper.IsNet)
                                    {
                                        sendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, Helper.ServerAddr, Helper.ServerPort);
                                        sayhellotoservertime = DateTime.Now;
                                        continue;
                                    }
                                    sendMsg(_poolFactory.SendmsgINFO[i].LantalkMsg, receiver);                                    
                                }
                                else
                                {
                                    //如果做为外网用户
                                    if (Helper.IsNet)
                                    {
                                        sayhellotoservertime = DateTime.Now;
                                        sendMsg(_poolFactory.SendmsgINFO[i].LanmsgRTF,_poolFactory.SendmsgINFO[i].Sendtobip, Helper.ServerAddr, Helper.ServerPort);
                                        continue;
                                    }
                                    sendMsg(_poolFactory.SendmsgINFO[i].LanmsgRTF, _poolFactory.SendmsgINFO[i].Sendtobip, receiver);
                                }                                
                            }
                            _poolFactory.SendmsgINFO[i].Sendto = null;
                        }
                        _poolFactory.clearSendMsgInfo();//清除已发送的消息
                    }
                    else
                    {
                        //如果超过一定时间没有与服务器对话则发送一条打招呼消息
                        if (Helper.IsNet && netlogined && sayhellotoservertime < DateTime.Now.AddSeconds(sayhellotimes))
                        {
                            sendMsg(sayHelloToServer(), Helper.ServerAddr, Helper.ServerPort);
                            sayhellotoservertime = DateTime.Now;
                        }
                        Thread.Sleep(20);
                    }
                }
                catch (Exception ex)
                {
                    Helper.writeLog("msghelper.THREADSENDMSG "+ex.Message);
                }
            }
        }
        public static void initimagelist()
        {
            try
            {
                Image img = (Image)_resourceManagerHeader.GetObject("_128");
                _headerLargeImageList.ImageSize = new Size(25, 25);                
                //_MsgFaceList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
                //_MsgFaceList.ImageSize = new System.Drawing.Size(20, 20);
                //_MsgFaceList.TransparentColor = System.Drawing.Color.White;
                object obj;
                for (int i = 1; i < 135; i++)
                {
                    obj = _resourceManagerHeader.GetObject("_" + i.ToString());                    
                    if (obj == null) continue;
                    Image imge = (obj as Image);
                    if (imge == null) continue;
                    _headerLargeImageList.Images.Add("_" + i.ToString(), imge);
                    _headerSmallImageList.Images.Add("_" + i.ToString(), imge);
                    try
                    {
                        _headerIconList.Add("_" + i.ToString(), Helper.imageToIcon(imge));
                    }
                    catch(Exception ex)
                    {
                        Helper.writeLog("msghelper.initmsg.imgtoicon" + ex.ToString());
                    }
                }
                
                for (int i = 0; i < 96; i++)
                {
                    Image imge = ImageResources.Face.getGifByName(i.ToString());
                    if (imge == null) 
                        continue;
                    imge.Tag = "face|" + i.ToString();
                    _MsgFaceList.Add(imge);                  
                }
            }
            catch
            { }
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
        public static void addUIFriend(Friend.Friend friend)
        {
            Friend.Friend f = _poolFactory.findFriend(friend.ID);
            if (f != null && friend.Ver.ToLower().Equals("lanmsg"))
            {
                return;
            }
            if (Helper.Face.Equals("qq"))
            {
                OutlookBarBand obb = findGroupByName(friend.GroupName);                
                if (f != null)
                {
                    try
                    {
                        OutlookBarBand mygroup = findGroupByName(f.GroupName);
                        mygroup.BackColor = Color.Teal;
                        mygroup.Items.Remove(getItemByIP(f.ID));
                    }
                    catch
                    { }
                }
                if (obb == null)
                {
                    obb = new OutlookBarBand(friend.GroupName);
                    obb.BackColor = Color.White;
                    obb.IconView = IconView.Large;
                    obb.LargeImageList = _headerLargeImageList;
                    obb.SmallImageList = _headerSmallImageList;
                    obb.Name = friend.GroupName;
                    //obb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(obb_MouseDoubleClick);
                    _groupList.Add(obb);
                    _groupBar.Bands.Add(obb);
                    //_groupBar.Refresh();
                }

                OutlookBarItem obi = new OutlookBarItem(friend.Name, getImageIndex(friend.Header));
                obi.Tag = friend.ID;
                obb.Items.Add(obi); obi.ImageIndex = HeaderLargeImageList.Images.IndexOfKey(friend.Header) < 0 ? 2 : HeaderLargeImageList.Images.IndexOfKey(friend.Header);
            
            }
            else
            {
                if (f == null)
                {
                    TreeNode[] groups = Program.formlist.listfriend.Nodes[0].Nodes.Find(friend.GroupName, false);
                    TreeNode group;
                    if (groups == null || groups.Length == 0)
                    {
                        addGroupNode(out group, friend);
                    }
                    else { group = groups[0]; }
                    TreeNode frienditem = new TreeNode(friend.Name); frienditem.Name = friend.ID; frienditem.ImageKey = friend.Header; frienditem.SelectedImageKey = friend.Header;
                    frienditem.ToolTipText = (friend.IsNet?"网外":"网内") + "[" + friend.ID +"]"+ getToolTip(friend.State);
                    group.Nodes.Add(frienditem);
                }
                else
                {
                    TreeNode[] groups = Program.formlist.listfriend.Nodes[0].Nodes.Find(f.GroupName, false);
                    TreeNode group;
                    if (groups == null || groups.Length == 0)
                    {
                        addGroupNode(out group, friend);
                    }
                    else { group = groups[0]; }
                    if (string.IsNullOrEmpty(friend.Header)) friend.Header = f.Header;
                    TreeNode frienditem;
                    if (!friend.GroupName.Equals(f.GroupName))
                    {                        
                        group.Nodes.RemoveByKey(f.ID);
                        if (group.Nodes.Count <= 0) Program.formlist.listfriend.Nodes[0].Nodes.Remove(group);
                        groups = Program.formlist.listfriend.Nodes[0].Nodes.Find(friend.GroupName, false);
                        if (groups == null || groups.Length == 0)
                        {
                            addGroupNode(out group, friend);
                        }
                        else { group = groups[0]; }
                        frienditem = new TreeNode(friend.Name);
                        frienditem.SelectedImageKey = friend.Header;
                        frienditem.Name = f.ID;
                        frienditem.ToolTipText = (friend.IsNet ? "网外" : "网内") + "[" + friend.ID + "]" + getToolTip(friend.State);
                        frienditem.ImageKey = friend.Header;
                        group.Nodes.Add(frienditem);
                    }
                    else
                    {
                        TreeNode[] items = group.Nodes.Find(f.ID, true);
                        if (items !=null && items.Length >0 && friend.Name.Equals(f.Name) && friend.Header.Equals(f.Header) && (friend.State==f.State || friend.Ver.Equals("LanMsg"))) return;
                        
                            group.Nodes.RemoveByKey(f.ID);
                            groups = Program.formlist.listfriend.Nodes[0].Nodes.Find(friend.GroupName, false);
                            if (groups == null || groups.Length == 0)
                            {
                                addGroupNode(out group, friend); 
                            }
                            else { group = groups[0]; }
                            frienditem = new TreeNode(friend.Name);
                            frienditem.SelectedImageKey = friend.Header;
                            frienditem.Name = f.ID;
                            frienditem.ImageKey = friend.Header;
                            frienditem.ToolTipText = (friend.IsNet ? "网外" : "网内") + "[" + friend.ID + "]" + getToolTip(friend.State);
                            group.Nodes.Add(frienditem);
                        
                    }                    
                }                
            }
            
        }
        private static void addGroupNode(out TreeNode group, Friend.Friend friend)
        {
            group = new TreeNode(friend.GroupName); group.Name = friend.GroupName; group.SelectedImageKey = group.ImageKey = "_128";
            if (friend.GroupName.Equals(Helper.MyGroupName))
            {
                Program.formlist.listfriend.Nodes[0].Nodes.Insert(0, group);
            }
            else
            {
                Program.formlist.listfriend.Nodes[0].Nodes.Add(group);
            }
            Program.formlist.listfriend.Nodes[0].Expand();
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
                    return "";
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
                Friend.Friend f = _poolFactory.findFriend(friend.ID);
                if (Helper.Face.Equals("qq"))
                {
                    if (f != null)
                    {
                        OutlookBarBand mygroup = findGroupByName(f.GroupName);
                        mygroup.Items.Remove(getItemByIP(f.ID));
                        if (mygroup.Items.Count <= 0)
                        {
                            _groupBar.Bands.Remove(mygroup);
                        }
                        return;
                    }
                }
                else
                {
                    if (f != null)
                    {
                        TreeNode[] groups = Program.formlist.listfriend.Nodes[0].Nodes.Find(f.GroupName, false);

                        if (groups != null && groups.Length != 0)
                        {
                            groups[0].Nodes.RemoveByKey(f.ID);
                            if (groups[0].Nodes.Count <= 0)
                            {
                                Program.formlist.listfriend.Nodes[0].Nodes.Remove(groups[0]);
                            }
                        }
                    }

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
                if (img == null) return Helper.DefaultIcon;
                return img;
            }
            catch
            {
                return Helper.DefaultIcon;
            }
        }
        /// <summary>
        /// 写入图片
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="imgname"></param>
        public static void setImgToBox(ref RichTextBox.MyExtRichTextBox txt, string imgname)
        {
            try
            {                
                Clipboard.SetImage(getImageByName(imgname));                
                Image obj = Clipboard.GetImage();
                if (obj != null)
                {
                    txt.SelectionStart = txt.Text.Length;
                    txt.Paste();
                }
            }
            catch(Exception ex)
            {
                Helper.writeLog(ex.Message);
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="info"></param>
        public static void sendMsg(object info,string ip)
        {
            Helper._send.SendBytes(Helper.ojectToByteArray(info),ip);
        }
        public static void sendMsg(object info, string ip,int port)
        {
            if (info == null) return;
            Helper._send.SendBytes(Helper.ojectToByteArray(info), ip,port);
        }
        public static void sendMsg(string msg, string ip)
        {
            WinLanMsg.Util.UserInfo info = new WinLanMsg.Util.UserInfo();
            info.ProtocolType = 5;
            info.SendFromIp = Helper.selfIP;
            info.SendToIp = Helper.getByteIP(ip);
            info.SendInfo = msg;
            Helper._send.SendBytes(Helper.ojectToByteArray(info), ip);
        }
        public static void sendMsg(string msg, string ip,string strip)
        {
            WinLanMsg.Util.UserInfo info = new WinLanMsg.Util.UserInfo();
            info.ProtocolType = 5;
            info.SendFromIp = Helper.selfIP;
            info.SendToIp = Helper.getByteIP(ip);
            info.SendInfo = msg;
            Helper._send.SendBytes(Helper.ojectToByteArray(info), strip);
        }
        public static void sendMsg(string msg, string ip,string strip, int port)
        {
            WinLanMsg.Util.UserInfo info = new WinLanMsg.Util.UserInfo();
            info.ProtocolType = 5;
            info.SendFromIp = Helper.selfIP;
            info.SendToIp = Helper.getByteIP(ip);
            info.SendInfo = msg;
            Helper._send.SendBytes(Helper.ojectToByteArray(info), strip, port);
        }
        /// <summary>
        /// 与服务器定时打招呼
        /// </summary>
        public static MsgInfo.MsgInfo sayHelloToServer()
        {
            MsgInfo.MsgInfo msginfo = new MsgInfo.MsgInfo();
            msginfo.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_NETUSER_HELLO;
            msginfo.IsNet = Helper.IsNet;
            msginfo.InfoGuid = Helper._guid;
            msginfo.NetUserID = Helper.MyNetID;
            msginfo.SendFromID = Helper.MyNetID;
            msginfo.SendInfo = "hello";
            msginfo.SendTo = Helper.ServerAddr;
            msginfo.SendToID = Helper.ServerAddr;
            msginfo.Ver = "NetTalk";
            return msginfo;
        }
        /// <summary>
        /// 发送传输文件消息
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="curip"></param>
        /// <param name="ballip"></param>
        public static void sendFileMsg(string filename,string[] curips,string ballip)
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
            info.IsNet = Helper.IsNet;
            info.NetUserID = Helper.MyNetID;
            if (info.IsNet)
            {
                info.SendFromID = Helper.MyNetID;
            }
            else
            {
                info.SendFromID = Helper.getIP(Helper.selfIP);
            }
            info.InfoGuid = Helper._guid;
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_FILE;
            info.SendToID = ballip;
            
            info.SendInfo = filename +"|"+ (new System.IO.FileInfo(filename)).Length;
            info.Ver = "NetTalk";
            foreach (string ip in curips)
            {
                _poolFactory.addSendInfo(new MsgInfo.MsgInfo(info, ip), new string[] { ip});
            }
        }
        /// <summary>
        /// 接收文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="curip"></param>
        public static void sendGetFileMsg(string filename, string curip)
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
            info.IsNet = Helper.IsNet;
            info.NetUserID = Helper.MyNetID;
            if (info.IsNet)
            {
                info.SendFromID = Helper.MyNetID;
            }
            else
            {
                info.SendFromID = Helper.getIP(Helper.selfIP);
            }
            info.InfoGuid = Helper._guid;
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_FILE;
            info.SendToID = curip;            
            info.SendInfo = filename;
            info.Ver = "LanTalk";
            info.SendTo = curip;
            _poolFactory.addSendInfo(info, new string[] { curip });
        }
        /// <summary>
        /// 重新请求文件包
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="curip"></param>
        /// <param name="index"></param>
        /// <param name="guid"></param>
        public static void sendGetFilePackIndex(string filename, string curip, int index,string guid)
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
            info.InfoGuid = Helper._guid;
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_FILEPACK;
            info.SendToID = curip;
            info.SendFromID = Helper.getIP(Helper.selfIP);
            info.SendInfo = filename + "|" + guid + "|"+index.ToString();
            info.Ver = "LanTalk";
            info.SendTo = curip;
            Helper._send.SendBytes(Helper.ojectToByteArray(info), curip);
        }
        /// <summary>
        /// 发送重请的包
        /// </summary>
        /// <param name="info"></param>
        public static void sendFilePackIndex(MsgInfo.MsgInfo info)
        {
            //string filename = info.SendInfo.Split('|')[0];
            //string guid = info.SendInfo.Split('|')[1];
            //int index = int.Parse(info.SendInfo.Split('|')[2]);
            //string sendto=info.SendFromID;
            //_poolFactory.Msg.Remove(info);
            //byte[] bs = System.IO.File.ReadAllBytes(filename);
            //int ipart = 60000;
            //int lastpart = bs.Length % ipart;
            //int count = 0;
            //if (lastpart == 0)
            //{
            //    count = bs.Length / ipart;
            //}
            //else
            //{
            //    count = bs.Length / ipart + 1;
            //}

            //MsgSend.MsgSend send = new MsgSend.MsgSend();
            //    int vercount = ipart;
            //    if (index == count - 1) vercount = lastpart;
            //    byte[] b = new byte[vercount];
            //    for (int j = 0; j < vercount; j++)
            //    {
            //        b[j] = bs[index * ipart + j];
            //    }
            //    TCPsendFile(send,b, filename, sendto, guid, index, 9056);
           
        }
        public static MsgInfo.MsgInfo fileinfo;
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="info"></param>
        public static void sendFile()
        {
            try
            {
                byte[] SendToID = Helper.getByteIP(fileinfo.SendFromID);
                string ports = fileinfo.SendInfo.Split('|')[2];
                string filename = fileinfo.SendInfo.Split('|')[0];
                string guid = fileinfo.SendInfo.Split('|')[1];                
                
                _poolFactory.Msg.Remove(fileinfo);
               
                int ipart = 60000;                
                
                System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                long filelen = fi.Length;                

                int lastpart = (int)(filelen % ipart);
                
                string[] port = ports.Split(',');
                int portcount = port.Length;
                if (portcount > Helper.ThreadCount)
                {
                    portcount = Helper.ThreadCount;
                }
                long lastcount = filelen % portcount;
                long vercounts = filelen / portcount;
                lastcount += vercounts;
                long position = 0;
                for (int i = 0; i < portcount; i++)
                {
                    long curlen = vercounts;
                    if (i == portcount - 1) curlen = lastcount;
                    SendFile sf = new SendFile(filename, guid, SendToID, ipart, position, curlen, int.Parse(port[i]), i, portcount);
                    
                    Thread thsendfile = new Thread(new ThreadStart(sf.send));
                    thsendfile.IsBackground = true;
                    thsendfile.Start();
                    position += vercounts;
                }
               
            }
            catch (Exception ex)
            {
                Helper.writeLog("msghelper.sendfile "+ex.Message);
            }           
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
                _s.sendFile(bs, Helper.getIP(sendto), port);
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
        public static void sendMsg(string[] curiips, string rtf, string ballip)
        {
            thsendip = curiips;
            thsendrtf = rtf;
            thsendips = ballip;
            Thread threadmsg = new Thread(new ThreadStart(threadSendMsg));
            threadmsg.IsBackground = true;
            threadmsg.Start();
            //_sendmsg = new desendmsg(sendmsg);

            //IAsyncResult ir = _sendmsg.BeginInvoke(curiips, rtf, Color.Black, null, ballip, null, null);
            //while (!ir.IsCompleted)
            //{
            //    Application.DoEvents();
            //}
            //_sendmsg.EndInvoke(ir);  

        }
        static string[] thsendip;
        static string thsendrtf;        
        static string thsendips;
        static bool portused = false;
        private static void threadSendMsg()
        {
            try
            {
                string[] sendip = thsendip;
                string sendrtf = thsendrtf;
                string sendips = thsendips;
                //while (portused)
                //{
                //    if (Helper.Iflag == 1)
                //    {
                //        return;
                //    }
                //    Thread.Sleep(100);
                //}
                
                //portused = true;
                sendmsg(sendip, sendrtf, Color.Black, null, sendips);
                sendrtf = string.Empty;
                           
            }
            catch (Exception ex)
            {
                Helper.writeLog("msghelper.threadSendMsg "+ex.Message);
            }
            //finally
            //{
            //    portused=false;
            //}
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="curiip"></param>
        public static void sendmsg(string[] curiips, string rtf, Color c, Font f, string ballip)
        {
            MsgInfo.MsgInfo info;
            //info.InfoGuid = Helper._guid;
            //info.SendFromID = Helper.selfIP;
            //info.SendToID = ballip;
            //info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT;

            int partlen = 10000;
            //if (Helper.IsNet) partlen = 800;
            if (rtf.Length < partlen)//如果数据小
            {
                foreach (string curip in curiips)
                {
                    Friend.Friend _curFriend = _poolFactory.findFriend(curip);
                    //if (_curFriend == null && !curip.Contains('.')) continue;
                    if (_curFriend == null) { _curFriend = new Friend.Friend(); _curFriend.Ip = Helper.getByteIP(curip); _curFriend.Name = curip; _curFriend.GroupName = "未知组"; _curFriend.Ver = "none"; }

                    //if (netsended && _curFriend.IsNet) continue;//对外网用户只发送一次
                    //if (_curFriend.IsNet) netsended = true;
                    if (!Helper.IsNet &&  Helper.Oldver && !_curFriend.Ver.ToLower().Equals("nettalk"))//对旧版本兼容
                    {
                        _poolFactory.addSendInfo(rtf, new string[] { curip }, ballip);
                        continue;
                    }
                    info = new MsgInfo.MsgInfo();
                    info.IsNet = Helper.IsNet;
                    info.NetUserID = Helper.MyNetID;
                    info.InfoGuid = Helper._guid;
                    info.Ver = "NetTalk";
                    if (info.IsNet)
                    {
                        info.SendFromID = info.NetUserID;
                        info.SendTo = _curFriend.ID;
                    }
                    else
                    {
                        info.SendFromID = Helper.getIP(Helper.selfIP);
                        info.SendTo=Helper.getIP(_curFriend.Ip);
                    }
                    info.SendToID = ballip;                    
                    info.SendInfo = rtf;
                    info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT;
                    info.SendTo = curip;
                    _poolFactory.addSendInfo(info, new string[] { Helper.getIP(_curFriend.Ip) });
                    if (Helper.IsNet) break;
                }
                return;
            }
            
            int partcount = 1;
            int lastlen = rtf.Length % partlen;//最后一部分的长度
            if (lastlen == 0)
            {
                partcount = rtf.Length / partlen;
            }
            else
            {
                partcount = rtf.Length / partlen + 1;
            }

            string sendrtf = "";
            string guid = Guid.NewGuid().ToString("n");           

            for (int i = 0; i < partcount; i++)
            {
                string part;
                if (i == partcount - 1)
                {
                    part = rtf.Substring(i * partlen);
                }
                else
                {
                    part = rtf.Substring(i * partlen, partlen);
                }
                sendrtf += "{\\part" + i.ToString() + "}";
                info = new MsgInfo.MsgInfo();
                info.IsNet = Helper.IsNet;
                info.NetUserID = Helper.MyNetID;
                info.InfoGuid = Helper._guid;
                if (info.IsNet)
                {
                    info.SendFromID = info.NetUserID;
                }
                else
                {
                    info.SendFromID = Helper.getIP(Helper.selfIP);
                }
                info.SendToID = ballip;
                info.InfoType = guid;
                info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_IMG;
                info.SendInfo = "{\\part" + i.ToString() + "}" + part;
                foreach (string ip in curiips)
                {
                    _poolFactory.addSendInfo(new MsgInfo.MsgInfo(info, ip), new string[] { ip});
                    if (Helper.IsNet) break;
                }
                _poolFactory.SendedMsgImg.Add(info);
            }
            info = new MsgInfo.MsgInfo();
            info.IsNet = Helper.IsNet;
            info.NetUserID = Helper.MyNetID;
            info.InfoType = guid;
            info.InfoGuid = Helper._guid;
            if (info.IsNet)
            {
                info.SendFromID = info.NetUserID;
            }
            else
            {
                info.SendFromID = Helper.getIP(Helper.selfIP);
            }
            info.SendToID = ballip;
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_SEND_TEXT;
            info.SendInfo = sendrtf;
            Thread.Sleep(100);
            foreach (string ip in curiips)
            {
                _poolFactory.addSendInfo(new MsgInfo.MsgInfo(info, ip), new string[] { ip });
                if (Helper.IsNet) break;
            }
            _poolFactory.SendedMsgImpLife.Add(guid, DateTime.Now.AddSeconds(60));
        }
        /// <summary>
        /// 重新获取丢失的包
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="index"></param>
        public static void getLostedImgPack(string guid, string index,string friendip)
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
            info.IsNet = Helper.IsNet;
            info.NetUserID = Helper.MyNetID;
            info.InfoGuid = Helper._guid;
            info.InfoType = guid;
            info.ProtocolType = MsgInfo.MsgInfo.sProtocolType.MSGTYPE_GET_IMGPACK;
            if (info.IsNet)
            {
                info.SendFromID = info.NetUserID;
            }
            else
            {
                info.SendFromID = Helper.getIP(Helper.selfIP);
            }
            info.SendInfo = index;
            info.SendToID = friendip;
            info.Ver = "NetTalk";
            info.SendTo = friendip;
            _poolFactory.addSendInfo(info, new string[] { friendip });
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
            lostedinfo.SendTo =info.IsNet?info.NetUserID : info.SendFromID;
            _poolFactory.addSendInfo(lostedinfo, new string[] { lostedinfo.SendTo });
        }
        private static bool findSavedHistory(string[][] history)
        {
            return history[0][0].Equals("saved");
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
