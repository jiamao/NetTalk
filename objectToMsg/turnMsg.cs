using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using WinLanMsg.Util;
using System.Reflection;
using System.IO;

namespace objectToMsg
{
    public class turnMsg
    {
        string _guid;
        FileCompress.GZip gzip;
        public turnMsg(string guid)
        {
            _guid = guid;
            gzip = new FileCompress.GZip(guid);
        }
        /// <summary>
        /// 将字节转为消息
        /// </summary>
        /// <param name="msgbs"></param>
        /// <returns></returns>
        public MsgInfo.MsgInfo byteToMsg(byte[] msgbs)
        {
            MemoryStream ms = new MemoryStream(msgbs);
            BinaryFormatter bf=new BinaryFormatter();
            object obj=null;
            try
            {
                obj = bf.Deserialize(ms);
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                MemoryStream newms = gzip.DerStreamZip(ms);
                obj = bf.Deserialize(newms);
            }
            if (obj.GetType() == typeof(MsgInfo.MsgInfo))
            {
                MsgInfo.MsgInfo info = obj as MsgInfo.MsgInfo;
                if (string.IsNullOrEmpty(info.SendFromID)) goto turntomsg;
                //info.Ver = "NetTalk";
                return info;
            }            
            turntomsg:return userinfoToMsginfo(obj);
            
        }
        /// <summary>
        /// 将字节转为消息
        /// </summary>
        /// <param name="msgbs"></param>
        /// <returns></returns>
        public MsgInfo.MsgInfo ServerbyteToMsg(byte[] msgbs)
        {
            MemoryStream ms = new MemoryStream(msgbs);
            BinaryFormatter bf = new BinaryFormatter();
            object obj = null;
            try
            {
                obj = bf.Deserialize(ms);
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                MemoryStream newms = gzip.DerStreamZip(ms);
                obj = bf.Deserialize(newms);
            }
            if (obj.GetType() == typeof(MsgInfo.MsgInfo))
            {
                MsgInfo.MsgInfo info = obj as MsgInfo.MsgInfo;
                if (string.IsNullOrEmpty(info.SendFromID)) goto turntomsg;
                //info.Ver = "NetTalk";
                return info;
            }
        turntomsg: return null;

        }
        /// <summary>
        /// 将老版本消息转为新版本的
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public MsgInfo.MsgInfo userinfoToMsginfo(object obj)
        {
            MsgInfo.MsgInfo info = new MsgInfo.MsgInfo();
            Type t = obj.GetType();
            MethodInfo[] methods = t.GetMethods();
            info.InfoGuid = _guid;
            foreach (MethodInfo method in methods)
            {
                object robj;
                try
                {
                    if (method.Name.Contains("get_ProtocolType"))
                    {
                        robj = method.Invoke(obj, null);
                        if (robj.GetType() == typeof(MsgInfo.MsgInfo.sProtocolType))
                        {
                            info.ProtocolType = (MsgInfo.MsgInfo.sProtocolType)(robj);
                            info.Ver = "LanTalk";
                        }
                        else
                        {
                            int p = int.Parse(robj == null ? "0" : robj.ToString());
                            if (p > 5 || p < 1) return null;
                            info.ProtocolType = (MsgInfo.MsgInfo.sProtocolType)(p);
                            info.Ver = "LanMsg";
                        }
                       
                    }
                    else if (method.Name.Contains("get_SendFromID"))
                    {
                        robj = method.Invoke(obj, null);
                        info.SendFromID = robj.ToString();
                        
                    }
                    else if (method.Name.Contains("get_SendFromIp"))
                    {
                        robj = method.Invoke(obj, null);
                        info.SendFromID = getIP((byte[])robj);
                        
                    }
                    else if (method.Name.Contains("get_SendToID"))
                    {
                        robj = method.Invoke(obj, null);
                        info.SendToID = robj.ToString();
                        
                    }
                    else if (method.Name.Contains("get_SendToIp"))
                    {
                        robj = method.Invoke(obj, null);
                        info.SendToID = getIP((byte[])robj);
                       
                    }
                    else if (method.Name.Contains("get_SendInfo"))
                    {
                        robj = method.Invoke(obj, null);
                        info.SendInfo = (robj == null ? null : robj.ToString());
                        
                    }
                }
                catch
                { }
            }
            
            return info;
        }
        public string getIP(byte[] byteip)
        {
            if (byteip == null || byteip.Length < 4) return "";
            return string.Concat(byteip[0], '.', byteip[1], '.', byteip[2], '.', byteip[3]);
        }
        /// <summary>
        /// 生成一个旧版本的消息类
        /// </summary>
        /// <returns></returns>
        public object createUserInfo(byte[] fromip, byte[] toip, byte type, string sendinfo)
        {
            UserInfo userinfo = new UserInfo();
            userinfo.ProtocolType = type;
            userinfo.SendFromIp = fromip;
            userinfo.SendToIp = toip;
            userinfo.SendInfo = sendinfo;
            return userinfo;
        }
    }
}
