using System;
using System.Collections.Generic;
using System.Text;

namespace MsgPoolFactory
{
    public class SendMsgInfo
    {
        string[] sendto;

        public string[] Sendto
        {
            get { return sendto; }
            set { sendto = value; }
        }
        int sendport = 9050;
        public int SendPort
        {
            set { sendport = value; }
            get { return sendport; }
        }
        bool _isnet = false;
        public bool IsNET
        {
            set { _isnet = value; }
            get { return _isnet; }
        }
        object lantalkMsg;

        public object LantalkMsg
        {
            get { return lantalkMsg; }
            set { lantalkMsg = value; }
        }

        string sendtobip;

        public string Sendtobip
        {
            get { return sendtobip; }
            set { sendtobip = value; }
        }
        string lanmsgRTF;

        public string LanmsgRTF
        {
            get { return lanmsgRTF; }
            set { lanmsgRTF = value; }
        }
    }
}
