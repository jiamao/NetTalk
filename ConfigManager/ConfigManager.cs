using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace ConfigManager
{
    public class ConfigManager
    {
        string configPath = "";
        XmlDocument xmldoc = new XmlDocument();
        /// <summary>
        /// ��д�����ļ�
        /// </summary>
        /// <param name="ConfigPath">�����ļ�·��</param>
        public ConfigManager(string ConfigPath)
        {
            throw new Exception("��Ҫ��ȷ�ı�ʶ�����ô���!");
            //configPath = ConfigPath;
            //if (!System.IO.File.Exists(ConfigPath) || !writeConfig("SendButton", readConfig("SendButton")) || !writeConfig("ThreadCount", readConfig("ThreadCount")) || !writeConfig("LanMsg", readConfig("LanMsg")) || !writeConfig("ISNET", readConfig("ISNET")) || !writeConfig("ServerAddr", readConfig("ServerAddr")) || !writeConfig("ServerPort", readConfig("ServerPort")))
            //{
            //    System.IO.File.Delete(ConfigPath);
            //    System.IO.StreamWriter sw = new System.IO.StreamWriter(ConfigPath, true);
            //    sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            //    sw.Write("<LanTalk>\r\n");
            //    sw.Write("<Settings>\r\n");
            //    sw.Write("<!--�����ļ��߳���-->");
            //    sw.Write("<node key=\"ThreadCount\" value=\"1\" />\r\n");
            //    sw.Write("<!--�Ƿ����LanMsg-->");
            //    sw.Write("<node key=\"LanMsg\" value=\"true\" />\r\n");
            //    sw.Write("<!--������Ϣ�Ŀ�ݽ���enter��ʾ���س����ͣ�ctrlenter��ʾ�����ƽ��ӻس�����-->");
            //    sw.Write("<node key=\"SendButton\" value=\"enter\" />\r\n");
            //    sw.Write("<!--�Ƿ�Ϊ�����û� true/false-->");
            //    sw.Write("<node key=\"ISNET\" value=\"false\" />\r\n");
            //    sw.Write("<!--����������û�����Ӧ���÷�������ַ-->");
            //    sw.Write("<node key=\"ServerAddr\" value=\"127.0.0.1\" />\r\n");
            //    sw.Write("<!--����������û�����Ӧ���÷������˿�-->");
            //    sw.Write("<node key=\"ServerPort\" value=\"9050\" />\r\n");                
            //    sw.Write("</Settings>\r\n");
            //    sw.Write("</LanTalk>");
            //    sw.Close();
            //}           
            
        }
        /// <summary>
        /// ��д�����ļ�
        /// </summary>
        /// <param name="ConfigPath">�����ļ�·��</param>
        public ConfigManager(string ConfigPath,string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("��Ҫ��ȷ�ı�ʶ�����ô���!");
            configPath = ConfigPath;
            if (!System.IO.File.Exists(ConfigPath) || !writeConfig("SendButton", readConfig("SendButton")) || !writeConfig("ThreadCount", readConfig("ThreadCount")) || !writeConfig("LanMsg", readConfig("LanMsg")) || !writeConfig("ISNET", readConfig("ISNET")) || !writeConfig("ServerAddr", readConfig("ServerAddr")) || !writeConfig("ServerPort", readConfig("ServerPort")))
            {
                System.IO.File.Delete(ConfigPath);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(ConfigPath, true);
                sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
                sw.Write("<LanTalk>\r\n");
                sw.Write("<Settings>\r\n");
                sw.Write("<!--�����ļ��߳���-->");
                sw.Write("<node key=\"ThreadCount\" value=\"1\" />\r\n");
                sw.Write("<!--�Ƿ����LanMsg-->");
                sw.Write("<node key=\"LanMsg\" value=\"true\" />\r\n");
                sw.Write("<!--������Ϣ�Ŀ�ݽ���enter��ʾ���س����ͣ�ctrlenter��ʾ�����ƽ��ӻس�����-->");
                sw.Write("<node key=\"SendButton\" value=\"enter\" />\r\n");
                sw.Write("<!--�Ƿ�Ϊ�����û� true/false-->");
                sw.Write("<node key=\"ISNET\" value=\"false\" />\r\n");
                sw.Write("<!--����������û�����Ӧ���÷�������ַ-->");
                sw.Write("<node key=\"ServerAddr\" value=\"127.0.0.1\" />\r\n");
                sw.Write("<!--����������û�����Ӧ���÷������˿�-->");
                sw.Write("<node key=\"ServerPort\" value=\"9050\" />\r\n");
                sw.Write("</Settings>\r\n");
                sw.Write("</LanTalk>");
                sw.Close();
            }

        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string readConfig(string strKey)
        {
            XmlReader xmlr = null;
            try
            {
                xmlr = XmlReader.Create(configPath);
                while (xmlr.Read())
                {
                    if (xmlr.NodeType == XmlNodeType.Element && xmlr.Name == "node")
                    {
                        if (xmlr.GetAttribute("key") == strKey)
                        {
                            return xmlr.GetAttribute("value").ToString();
                        }
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
            finally
            {
                if (xmlr != null) { xmlr.Close(); }
            }
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string readConfig(string strKey,string defaultValue)
        {
            XmlReader xmlr = null;
            try
            {
                xmlr = XmlReader.Create(configPath);
                while (xmlr.Read())
                {
                    if (xmlr.NodeType == XmlNodeType.Element && xmlr.Name == "node")
                    {
                        if (xmlr.GetAttribute("key") == strKey)
                        {
                            return xmlr.GetAttribute("value").ToString();
                        }
                    }
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
            finally
            {
                if (xmlr != null) { xmlr.Close(); }
            }
        }
        /// <summary>
        /// �޸Ľڵ�
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public bool writeConfig(string strKey, string strValue)
        {
            try
            {
                xmldoc.Load(configPath);
                XmlNode xmlnode = xmldoc.SelectSingleNode("LanTalk/Settings");
                int nodeexists = 0;
                foreach (XmlNode xn in xmlnode.ChildNodes)
                {
                    if (xn.NodeType == XmlNodeType.Element && xn.Name == "node" && xn.Attributes["key"].Value == strKey)
                    {
                        xn.Attributes["value"].Value = strValue;
                        nodeexists = 1;
                        break;
                    }
                }
                if (nodeexists < 1)
                {
                    XmlNode newnode = xmldoc.CreateNode(XmlNodeType.Element, "node", "LanTalk/Settings");
                    newnode.Attributes["key"].Value = strKey;
                    newnode.Attributes["value"].Value = strValue;

                }
                xmldoc.Save(configPath);
                return true;
            }
            catch
            {
                return false;
            }

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
        public void Dispose()
        {
            Dispose();
        }
    }
}
