/*
//*****************************************************************************
// GZip.cs  ver 0001
// 项目名         ：中油碧辟加油卡服务系统
// 业务名         ：系统服务
// 程序名         ：文件压缩类
// 程序标识       ：GZip.cs
//-----------------------------------------------------------------------------
// 改版履歴
//    Ver       |日期          |公司            |编制人        |内容
//-----------------------------------------------------------------------------
//    0001      |2008-4-7      |NEC            |丁峰峰        |新程序编制

//-----------------------------------------------------------------------------
// 功能概要：

//    文件压缩类。

// 传入参数：

//   
//    
// 返回值：
//    
//    
//*****************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
namespace FileCompress
{
    public class GZip
    {
        GZipStream CompressedStream = null;//压缩流  
        public GZip()
        {
            throw new Exception("需要正确的标识来引用此类!");
        }
        public GZip(string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("需要正确的标识来引用此类!");
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public MemoryStream SerFileZip(string filepath)
        {
            FileStream SourceFile=null;
            MemoryStream SerFile=new MemoryStream();
            try
            {
            //    if (!File.Exists(filepath))
            //    {
            //        return null;
            //    }
                byte[] buffer;
                string filename = Path.GetFileNameWithoutExtension(filepath);//获取文件名
                SourceFile = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[SourceFile.Length];
                SourceFile.Read(buffer, 0, buffer.Length);//读取源文件                
                
                CompressedStream = new GZipStream(SerFile, CompressionMode.Compress, true);
                CompressedStream.Write(buffer,0,buffer.Length);//压缩                
                CompressedStream.Flush();
                return SerFile;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (SourceFile != null)
                    SourceFile.Close();
               
                if (CompressedStream != null)
                    CompressedStream.Close();
            }
        }
        /// <summary>
        /// 压缩流
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream SerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream = new MemoryStream();
            GZipStream compressedstream = new GZipStream(zpistream, CompressionMode.Compress);//
            try
            {
                byte[] buffer =new byte[sourcestream.Length];
                sourcestream.Position = 0;
                sourcestream.Read(buffer,0,buffer.Length);
                compressedstream.Write(buffer,0,buffer.Length);
                compressedstream.Close();                
                return zpistream;
            }
            catch
            {
                return null;
            }            
        }
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public string DerFileZip(Stream sourstream, string filepath,int filelen)
        {
            FileStream SerFile = null;
            try
            {
                if (!File.Exists(filepath))
                {
                    return "Err:(GZip.DerFileZIP)解压失败：指定的文件路径(" + filepath + ")不正确";
                }
                byte[] buffer=new byte[filelen];
                string filename = Path.GetFileNameWithoutExtension(filepath);//获取文件名

                CompressedStream = new GZipStream(sourstream, CompressionMode.Decompress, true);

                int br = CompressedStream.Read(buffer, 0, filelen);

                SerFile = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
                SerFile.Write(buffer, 0, filelen);                
                return "成功";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SerFile != null)
                    SerFile.Close();
                if (CompressedStream != null)
                    CompressedStream.Close();
            }
        }
        /// <summary>
        /// 解压缩流
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream DerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream;
            sourcestream.Position = 0;
            GZipStream compressedstream = new GZipStream(sourcestream, CompressionMode.Decompress, true);//压缩流  
            try
            {
                byte[] buffer = new byte[sourcestream.Length * 100];                
                int len = compressedstream.Read(buffer, 0, buffer.Length);
                compressedstream.Close();
                zpistream = new MemoryStream(buffer,0,len);                
                return zpistream;
            }
             catch
            {
                return null;
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
    }
}
