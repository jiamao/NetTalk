using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MsgInfo
{
    [Serializable]
    public class FilePack
    {
        string _guid;

        public string Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }
        string _filename;

        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }
        byte[] _fromIP;

        public byte[] FromIP
        {
            get { return _fromIP; }
            set { _fromIP = value; }
        }
        byte[] _toIP;

        public byte[] ToIP
        {
            get { return _toIP; }
            set { _toIP = value; }
        }
        long _index;

        public long Index
        {
            get { return _index; }
            set { _index = value; }
        }
        byte[] _bytes;

        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; }
        }
    }
}
