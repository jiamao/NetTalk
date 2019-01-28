using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
namespace MsgInfo
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class MsgInfo
    {
         private sProtocolType _ProtocolType;
         private byte[] _SendFromIp;
         private byte[] _SendToIp;
         private string _SendInfo;
         private string _infoGuid;
        private string _ver;
        private Color _fontColor;
         private Font _infoFont=new Font("����",10);
        /// <summary>
        /// �����ʶ
        /// </summary>
         public string InfoGuid
         {
             get { return _infoGuid; }
             set { _infoGuid = value; }
         }
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public enum sProtocolType
        {
            MSGTYPE_STATE=1,
            MSGTYPE_LOGOUT = 2,
            MSGTYPE_GET_MEMBER_LIST = 3,
            MSGTYPE_SEND_MEMBER_LIST = 4,
            MSGTYPE_SEND_TEXT = 5,
            MSGTYPE_SEND_FILE = 6,
            MSGTYPE_SEND_DIR = 7
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void init()
        {
            this._ProtocolType = 0;
            this._SendFromIp = null;
            this._SendInfo = null;
            this._SendToIp = null;
        }
        /// <summary>
        /// ������
        /// </summary>
        public sProtocolType ProtocolType
        {
            get
            {
                return this._ProtocolType;
            }
            set
            {
                this._ProtocolType = value;
            }
        }
        /// <summary>
        /// �����ߣɣ�
        /// </summary>
        public byte[] SendFromIp
        {
            get
            { return this._SendFromIp; }
            set
            { this._SendFromIp = value; }
        }
        /// <summary>
        /// �����ߣɣ�
        /// </summary>
        public byte[] SendToIp
        {
            set { this._SendToIp = value; }
            get { return this._SendToIp; }
        }
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string SendInfo
        {
            get
            { return this._SendInfo; }
            set
            {
                this._SendInfo = value;
            }
        }

        /// <summary>
        /// �������ɫ
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public Font InfoFont
        {
            get { return _infoFont; }
            set { _infoFont = value; }
        }
        /// <summary>
        /// �ͻ��˰汾
        /// </summary>
        public string Ver
        {
            get { return _ver; }
            set { _ver = value; }
        }
    }
}
