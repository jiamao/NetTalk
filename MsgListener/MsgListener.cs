using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace MsgListener
{
    public class MsgListener:IDisposable
    {
        UdpClient clientlistener;        
        UdpClient serverlistener;
        TcpListener[] tcplistener = new TcpListener[1];
        List<TcpListener> usedsocket;
        bool bFlag = true;
        bool bNetFlag = true;
        IPEndPoint receiveEP;
        int netReceivePort = 9050;
        int _localport = 9050;

        public MsgListener() { throw new Exception("需要正确的标识来引用此类!"); }
        public MsgListener(int localport)
        {
            throw new Exception("需要正确的标识来引用此类!");
            _localport = localport;
        }
        public MsgListener(int localport, string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("需要正确的标识来引用此类!");
            _localport = localport;
        }
        public delegate void receiveMsg(byte[] bMsg,IPEndPoint iep);
        public event receiveMsg getMsg;
        public delegate void receiveFile(byte[] bFile , int index);
        public event receiveFile getFile;
        public IPEndPoint ReceiveEP
        {
            get { return receiveEP; }
            set { receiveEP = value; }
        }
        public int LocalPort
        {
            set { _localport = value; }
            get { return _localport; }
        }
        public int NetReceivePort
        {
            get { return netReceivePort; }
            set { netReceivePort = value; }
        }
        
        #region 服务器监听
        Thread thserverlisten;
        int serverport = 9050;
        public void initserver()
        {
            
            try
            {
                bNetFlag = true;
                string temp = System.Configuration.ConfigurationManager.AppSettings["PORT"].ToString();
                serverport = int.Parse(string.IsNullOrEmpty(temp) ? "9050" : temp);
                if (serverport != 9050)
                {
                    if (serverlistener == null) serverlistener = new UdpClient(serverport);
                    if (thserverlisten == null || thserverlisten.ThreadState == ThreadState.Aborted || thserverlisten.ThreadState == ThreadState.Stopped)
                    {
                        thserverlisten = new Thread(new ThreadStart(serverlisten));
                        thserverlisten.IsBackground = true;
                        thserverlisten.Start();
                    }                   
                }
                
            }
            catch
            {
                throw new Exception("端口初始化失败!");
            }
        }
        public void serverSendMsg(byte[] msg, string ip, int port)
        {
            if (serverlistener != null)
            {
                serverlistener.Send(msg, msg.Length, ip, port);
            }
        }
        private void serverlisten()
        {
            byte[] buffer;            
            while (bFlag)
            {
                try
                {
                    IPEndPoint iep = new IPEndPoint(IPAddress.Any, serverport);
                    buffer = serverlistener.Receive(ref iep);
                    getMsg(buffer, iep);
                }
                finally
                {                 
                }
            }
        }
        #endregion
        #region 客户端监听
        public void stop()
        {
            try
            {
                bFlag = false;                
                if (thserverlisten != null) thserverlisten.Abort();
                Thread.Sleep(10);
                if (clientlistener != null)
                    clientlistener.Close();
                if (serverlistener != null)
                    serverlistener.Close();
            }
            catch
            {

            }
        }
        Thread listenthread;
        /// <summary>
        /// 内网临听初始化
        /// </summary>
        public void init()
        {
            bFlag = true;
            if(receiveEP==null) receiveEP = new IPEndPoint(IPAddress.Any, _localport);
            clientlistener = new UdpClient(_localport);
            if (listenthread == null || listenthread.ThreadState == ThreadState.Stopped || listenthread.ThreadState == ThreadState.Aborted)
            {
                listenthread = new Thread(new ThreadStart(start));
                listenthread.IsBackground = true;
                listenthread.Start();
            }            
        }

        /// <summary>
        /// 开启接收文件的监听
        /// </summary>
        /// <param name="ip"></param>
        public string startreceivefile(string remoteip,int portcount)
        {
            int port = 9051;
            iflag = 0;
            tcplistener = new TcpListener[portcount];
            string ports="";
            for (int i = 0; i < tcplistener.Length;i++ )
            {
            resetlisten: 
                try
                {
                   tcplistener[i] = new TcpListener(IPAddress.Parse(remoteip),port);
                   
                    tcplistener[i].Start();

                    ports = ports + "," + port.ToString();
                    
                    port++;
                    
                }
                catch
                {
                    port++;
                    goto resetlisten;
                }
            }
            usedsocket = new List<TcpListener>();
            
            Thread[] receivefile = new Thread[tcplistener.Length];
            for (int i = 0; i < receivefile.Length;i++ )
            {
                receivefile[i] = new Thread(new ThreadStart(listenfile));

                receivefile[i].IsBackground = true;

                receivefile[i].Start();
            }
            if (!string.IsNullOrEmpty(ports))
            {
                return ports.Substring(1);
            }
            return "";
        }
        public void stopreceivefile()
        {
            try
            {
                iflag = 1;
                foreach (TcpListener s in tcplistener)
                {
                    s.Stop(); ;
                }
            }
            catch
            { }
        }
        int iflag = 0;
        
        //EndPoint ep1;
        //EndPoint ep2;
        private void listenfile()
        {
            try
            {
                byte[] buffer;
                byte[] file;
                TcpListener s = null;
                int index = 1;
                NetworkStream netstream;
                for (int i=0;i<tcplistener.Length;i++ )
                {
                    if (!usedsocket.Contains(tcplistener[i]))
                    {
                        s = tcplistener[i];
                        usedsocket.Add(tcplistener[i]);
                        index = i;
                        break;
                    }
                }
                if (s == null) return;
                TcpClient receS = s.AcceptTcpClient();
                netstream = receS.GetStream();
                while (bFlag && iflag == 0)
                {
                    try
                    {
                        buffer = new byte[60000];
                        //int len = receS.Client.Receive(buffer);
                        int len=netstream.Read(buffer,0,buffer.Length);
                        if (len <= 0) continue;
                        file = new byte[len];
                        for (int i = 0; i < len; i++)
                        {
                            file[i] = buffer[i];
                        }
                        getFile(file, index);
                    }
                    catch
                    { }
                }
                netstream.Close();
            }
            catch
            { }
        }
        
        private void start()
        {            
            byte[] buffer;
            while(bFlag)
            {
                try
                {
                    buffer = clientlistener.Receive(ref receiveEP);
                    getMsg(buffer, receiveEP);
                }
                catch
                { }
            }
        }
        Thread netThreadListen;
        UdpClient netserverlistener;
        /// <summary>
        /// 开启外网监听
        /// </summary>
        /// <param name="port"></param>
        public void resetNetListen(UdpClient netlistener)
        {
            if (netThreadListen == null)
            {
                netserverlistener = netlistener;
                bNetFlag = true;
                netThreadListen = new Thread(new ThreadStart(netStart));
                netThreadListen.IsBackground = true;
                netThreadListen.Start();
            }
            else
            { 
                IPEndPoint oldiep=(netserverlistener.Client.LocalEndPoint as IPEndPoint);
                IPEndPoint newiep=netlistener.Client.LocalEndPoint as IPEndPoint;
                if (oldiep == null || newiep == null || oldiep.Port == newiep.Port) return;
                bNetFlag = false;
                netThreadListen.Abort();
                Thread.Sleep(100);
                bNetFlag = true;
                netserverlistener = netlistener;
                netThreadListen.Start();
            }
        }
        IPEndPoint netIEP;
        private void netStart()
        {
            byte[] buffer;
            netIEP = new IPEndPoint(IPAddress.Any, netReceivePort);
            while (bFlag && bNetFlag)
            {
                try
                {
                    buffer = netserverlistener.Receive(ref netIEP);
                    getMsg(buffer, netIEP);
                }
                catch
                { }
            }
        }
        #endregion
        string _guidHexs = "0123456789ABCDEF-";
        byte[] _infoguidbytes = new byte[] { 7, 13, 0, 7, 2, 5, 11, 8, 16, 7, 10, 0, 3, 16, 4, 9, 5, 15, 16,
            11, 7, 12, 13, 16, 15, 0, 11, 6, 1, 13, 15, 15, 6, 7, 12, 1 };
        string dllguid = "";
        string _guid
        {
            get
            {
                if (string.IsNullOrEmpty(dllguid))
                {
                    foreach (byte b in _infoguidbytes)
                    {
                        dllguid += _guidHexs[(int)b].ToString();
                    }
                }
                return dllguid;
            }
        }
        #region IDisposable 成员

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose();
        }
        #endregion
    }
}
