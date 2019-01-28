using System;
using System.Collections.Generic;
using System.Text;

namespace MsgPoolFactory
{
    public class receiveMsgs
    {
        public receiveMsgs()
        {
       }
        public receiveMsgs(byte[] msg, System.Net.IPEndPoint iep)
        {
            _userMsg = msg;
            _userIpEndPoint = iep;
        }
        System.Net.IPEndPoint _userIpEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Broadcast,9050);
        /// <summary>
        /// 消息来源
        /// </summary>
        public System.Net.IPEndPoint UserIpEndPoint
        {
            set { _userIpEndPoint = value; }
            get { return _userIpEndPoint; }
        }
        byte[] _userMsg;
        /// <summary>
        /// 收到好友的消息
        /// </summary>
        public byte[] UserMsg
        {
            set { _userMsg = value; }
            get { return _userMsg; }
        }
    }
}
