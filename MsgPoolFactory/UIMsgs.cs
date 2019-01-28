using System;
using System.Collections.Generic;
using System.Text;

namespace MsgPoolFactory
{
    public class UIMsgs
    {
        public UIMsgs()
        { }
        public UIMsgs(MsgInfo.MsgInfo msg, System.Net.IPEndPoint addr)
        {
            info = msg;
            iep = addr;
        }
        /// <summary>
        /// 消息
        /// </summary>
        MsgInfo.MsgInfo info;
        public MsgInfo.MsgInfo Info
        {
            set { info = value; }
            get { return info; }
        }
        /// <summary>
        /// 消息来源
        /// </summary>
        System.Net.IPEndPoint iep;
        public System.Net.IPEndPoint IEP
        {
            set { iep = value; }
            get { return iep; }
        }
    }
}
