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
        /// 读写配置文件
        /// </summary>
        /// <param name="ConfigPath">配置文件路径</param>
        public ConfigManager(string ConfigPath)
        {
            throw new Exception("需要正确的标识来引用此类!");
            //configPath = ConfigPath;
            //if (!System.IO.File.Exists(ConfigPath) || !writeConfig("SendButton", readConfig("SendButton")) || !writeConfig("ThreadCount", readConfig("ThreadCount")) || !writeConfig("LanMsg", readConfig("LanMsg")) || !writeConfig("ISNET", readConfig("ISNET")) || !writeConfig("ServerAddr", readConfig("ServerAddr")) || !writeConfig("ServerPort", readConfig("ServerPort")))
            //{
            //    System.IO.File.Delete(ConfigPath);
            //    System.IO.StreamWriter sw = new System.IO.StreamWriter(ConfigPath, true);
            //    sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            //    sw.Write("<LanTalk>\r\n");
            //    sw.Write("<Settings>\r\n");
            //    sw.Write("<!--传输文件线程数-->");
            //    sw.Write("<node key=\"ThreadCount\" value=\"1\" />\r\n");
            //    sw.Write("<!--是否兼容LanMsg-->");
            //    sw.Write("<node key=\"LanMsg\" value=\"true\" />\r\n");
            //    sw.Write("<!--发送消息的快捷健，enter表示按回车发送，ctrlenter表示按控制健加回车发送-->");
            //    sw.Write("<node key=\"SendButton\" value=\"enter\" />\r\n");
            //    sw.Write("<!--是否为外网用户 true/false-->");
            //    sw.Write("<node key=\"ISNET\" value=\"false\" />\r\n");
            //    sw.Write("<!--如果是外网用户，则应配置服务器地址-->");
            //    sw.Write("<node key=\"ServerAddr\" value=\"127.0.0.1\" />\r\n");
            //    sw.Write("<!--如果是外网用户，则应配置服务器端口-->");
            //    sw.Write("<node key=\"ServerPort\" value=\"9050\" />\r\n");                
            //    sw.Write("</Settings>\r\n");
            //    sw.Write("</LanTalk>");
            //    sw.Close();
            //}           
            
        }
        /// <summary>
        /// 读写配置文件
        /// </summary>
        /// <param name="ConfigPath">配置文件路径</param>
        public ConfigManager(string ConfigPath,string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("需要正确的标识来引用此类!");
            configPath = ConfigPath;
            if (!System.IO.File.Exists(ConfigPath) || !writeConfig("SendButton", readConfig("SendButton")) || !writeConfig("ThreadCount", readConfig("ThreadCount")) || !writeConfig("LanMsg", readConfig("LanMsg")) || !writeConfig("ISNET", readConfig("ISNET")) || !writeConfig("ServerAddr", readConfig("ServerAddr")) || !writeConfig("ServerPort", readConfig("ServerPort")))
            {
                System.IO.File.Delete(ConfigPath);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(ConfigPath, true);
                sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
                sw.Write("<LanTalk>\r\n");
                sw.Write("<Settings>\r\n");
                sw.Write("<!--传输文件线程数-->");
                sw.Write("<node key=\"ThreadCount\" value=\"1\" />\r\n");
                sw.Write("<!--是否兼容LanMsg-->");
                sw.Write("<node key=\"LanMsg\" value=\"true\" />\r\n");
                sw.Write("<!--发送消息的快捷健，enter表示按回车发送，ctrlenter表示按控制健加回车发送-->");
                sw.Write("<node key=\"SendButton\" value=\"enter\" />\r\n");
                sw.Write("<!--是否为外网用户 true/false-->");
                sw.Write("<node key=\"ISNET\" value=\"false\" />\r\n");
                sw.Write("<!--如果是外网用户，则应配置服务器地址-->");
                sw.Write("<node key=\"ServerAddr\" value=\"127.0.0.1\" />\r\n");
                sw.Write("<!--如果是外网用户，则应配置服务器端口-->");
                sw.Write("<node key=\"ServerPort\" value=\"9050\" />\r\n");
                sw.Write("</Settings>\r\n");
                sw.Write("</LanTalk>");
                sw.Close();
            }

        }
        /// <summary>
        /// 读取配置
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
        /// 读取配置
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
        /// 修改节点
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
