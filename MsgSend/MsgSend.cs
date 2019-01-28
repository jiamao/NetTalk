using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MsgSend
{
    public class MsgSend:IDisposable
    {
        Socket _send;
        Socket _sendfile;
        int _port = 9050;
        int _sendNetPort = 9050;
        IPEndPoint _epp;
        byte[] _msg;
        public delegate void changeNetSendPort(UdpClient serverListener);
        public changeNetSendPort NetSendPortChanged;
        public MsgSend()
        { throw new Exception("需要正确的标识来引用此类!"); }
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="lanport">本地发送消息端口</param>
        public MsgSend(string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("需要正确的标识来引用此类!");            
        }
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="lanport">本地发送消息端口</param>
        public MsgSend(int lanport,string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("需要正确的标识来引用此类!");
            _port = lanport;
        }
        /// <summary>
        /// 要发送的消息
        /// </summary>
        public byte[] Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        /// <summary>
        /// 发送到服务器时使用的端口
        /// </summary>
        public int SendNetPort
        {
            get { return _sendNetPort; }
        }
        /// <summary>
        /// 发送字节
        /// </summary>
        /// <param name="msg"></param>
        public void SendBytes(byte[] msg)
        {
            _send = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _epp = new IPEndPoint(IPAddress.Broadcast, _port);        
            _send.Connect(_epp);
            _send.Send(msg);
            _send.Close();
        }
        /// <summary>
        /// 发送到特定对象
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="iep"></param>
        public void SendBytes(byte[] msg, IPEndPoint iep)
        {
            _send = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _send.Connect(iep);
            _send.Send(msg);
            _send.Close();
        }
        UdpClient localsend = new UdpClient();
        /// <summary>
        /// 发送到特定对象
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="iep"></param>
        public void SendBytes(byte[] msg, string ip)
        {
            //_send = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //_send.Connect(new IPEndPoint(IPAddress.Parse(ip), _port));
            if(localsend==null)localsend = new UdpClient();
            localsend.Send(msg, msg.Length, ip, _port);
            //_send.Send(msg);
            //_send.Close();                 
        }
        UdpClient _serversend;
        /// <summary>
        /// 发送到特定对象,主要用于外网
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="iep"></param>
        public void SendBytes(byte[] msg, string ip,int port)
        {
            //IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);
            if (_serversend == null)
            {
                _serversend = new UdpClient();
                //_serversend.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            }
            _serversend.Send(msg, msg.Length, ip, port);
            if(NetSendPortChanged !=null)
            NetSendPortChanged(_serversend); 
        }
        TcpClient _mysendfile=new TcpClient();
        NetworkStream netstream;
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ip"></param>
        public void sendFile(byte[] file, string ip, int port)
        {
            if (_mysendfile == null)
                _mysendfile = new TcpClient(ip, port);
            if (!_mysendfile.Connected)
            {
                _mysendfile.Connect(ip, port);
                //_mysendfile.Client.Send(file);
                netstream = _mysendfile.GetStream();
            }
            netstream.Write(file, 0, file.Length);
            netstream.Flush();
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ip"></param>
        public void sendFile(string filename, string ip, int port)
        {
            if (_sendfile == null) _sendfile = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (!_sendfile.Connected) _sendfile.Connect(ip, port);
            _sendfile.SendFile(filename);
        }
        public void stopFile()
        {
            try
            {
                if (_mysendfile != null && _mysendfile.Connected) _mysendfile.Close();
                if (_sendfile != null) _sendfile.Close();
                _sendfile = null;
            }
            catch
            { }
        }
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
