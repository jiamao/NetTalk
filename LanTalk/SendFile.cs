using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LanTalk
{
    class SendFile
    {
        string _filename;
        string _guid;
        byte[] _sendto;
        long _position;
        long _sendlen;
        int _partlen;
        int _port;
        int _index;
        int _portcount;
        Stream _fileStream = null;

        public Stream FileStream
        {
            get { return _fileStream; }
            set { _fileStream = value; }
        }
        MsgSend.MsgSend _send = new MsgSend.MsgSend(Helper._guid);

        public SendFile(string filename, string guid, byte[] sendto, int partlen, long position, long count, int port, int index, int partcount)
        {
            _filename = filename;            
            _guid = guid;
            _sendto = sendto;
            _position = position;
            _portcount = partcount;
            _sendlen = count;
            _index = index;
            _partlen = partlen;
            _port = port;
        }
        public void sendPackByIndex()
        {
            Stream fs = new FileStream(_filename, FileMode.Open, FileAccess.Read);
            byte[] bs;
            BinaryReader br = new BinaryReader(fs);
            br.BaseStream.Position = _position;
            bs = br.ReadBytes((int)_sendlen);
            fs.Close();
            br.Close();
            MsgHelper.TCPsendFile(_send,bs, _filename, _sendto, _guid, _index, _port);
            _send.stopFile();
        }
        public void send()
        {
            System.IO.BinaryReader br=null;
            try
            {
                System.IO.Stream stream = new System.IO.FileStream(_filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                br = new System.IO.BinaryReader(stream);
                br.BaseStream.Position = _position;
                long sendedlen = 0;
                int count = (int)(_sendlen / _partlen);
                if (_sendlen % _partlen != 0) count = count + 1;
                for (int i = 0; i < count;i++ )
                {
                    if (i == count - 1) { _partlen = (int)(_sendlen - sendedlen);}
                    sendedlen += _partlen;
                    byte[] bs = br.ReadBytes(_partlen);
                    MsgHelper.TCPsendFile(_send, bs, _filename, _sendto, _guid, _index, _port);
                }
                if(_portcount==_index +1)
                    Helper.writeLog("发送文件给:" + Helper.getIP(_sendto) + "完成！"); 
            }
            catch(Exception ex)
            {
                Helper.writeLog("sendfile.send "+ex.Message);
            }
            finally
            {
                if (br != null) br.Close();
                _send.stopFile();                
            }
        }
        public void sendstream()
        {
            try
            {
                //System.IO.BinaryReader br = new System.IO.BinaryReader(_fileStream);
                //br.BaseStream.Position = _position;
                _fileStream.Position = _position;
                long sendedlen = 0;
                int count = (int)(_sendlen / _partlen);
                if (_sendlen % _partlen != 0) count = count + 1;
                for (int i = 0; i < count; i++)
                {
                    if (i == count - 1) _partlen = (int)(_sendlen - sendedlen);
                    sendedlen += _partlen;
                    byte[] bs = new byte[_partlen];
                    _fileStream.Read(bs, (int)_position, (int)_partlen);
                    _position += _partlen;
                    MsgHelper.TCPsendFile(_send, bs, _filename, _sendto, _guid, _index, _port);
                }
                //br.Close();
            }
            catch
            { }
            finally
            {
                _send.stopFile();
            }
        }
    }
}
