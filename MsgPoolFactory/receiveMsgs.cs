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
        /// ��Ϣ��Դ
        /// </summary>
        public System.Net.IPEndPoint UserIpEndPoint
        {
            set { _userIpEndPoint = value; }
            get { return _userIpEndPoint; }
        }
        byte[] _userMsg;
        /// <summary>
        /// �յ����ѵ���Ϣ
        /// </summary>
        public byte[] UserMsg
        {
            set { _userMsg = value; }
            get { return _userMsg; }
        }
    }
}
