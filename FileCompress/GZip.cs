/*
//*****************************************************************************
// GZip.cs  ver 0001
// ��Ŀ��         �����ͱ̱ټ��Ϳ�����ϵͳ
// ҵ����         ��ϵͳ����
// ������         ���ļ�ѹ����
// �����ʶ       ��GZip.cs
//-----------------------------------------------------------------------------
// �İ��Ěs
//    Ver       |����          |��˾            |������        |����
//-----------------------------------------------------------------------------
//    0001      |2008-4-7      |NEC            |�����        |�³������

//-----------------------------------------------------------------------------
// ���ܸ�Ҫ��

//    �ļ�ѹ���ࡣ

// ���������

//   
//    
// ����ֵ��
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
        GZipStream CompressedStream = null;//ѹ����  
        public GZip()
        {
            throw new Exception("��Ҫ��ȷ�ı�ʶ�����ô���!");
        }
        public GZip(string guid)
        {
            if (!guid.Equals(_guid)) throw new Exception("��Ҫ��ȷ�ı�ʶ�����ô���!");
        }
        /// <summary>
        /// ѹ���ļ�
        /// </summary>
        /// <param name="filepath">Դ�ļ�</param>
        /// <param name="dir">Ŀ��Ŀ¼</param>
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
                string filename = Path.GetFileNameWithoutExtension(filepath);//��ȡ�ļ���
                SourceFile = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[SourceFile.Length];
                SourceFile.Read(buffer, 0, buffer.Length);//��ȡԴ�ļ�                
                
                CompressedStream = new GZipStream(SerFile, CompressionMode.Compress, true);
                CompressedStream.Write(buffer,0,buffer.Length);//ѹ��                
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
        /// ѹ����
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
        /// ��ѹ�ļ�
        /// </summary>
        /// <param name="filepath">Դ�ļ�</param>
        /// <param name="dir">Ŀ��Ŀ¼</param>
        /// <returns></returns>
        public string DerFileZip(Stream sourstream, string filepath,int filelen)
        {
            FileStream SerFile = null;
            try
            {
                if (!File.Exists(filepath))
                {
                    return "Err:(GZip.DerFileZIP)��ѹʧ�ܣ�ָ�����ļ�·��(" + filepath + ")����ȷ";
                }
                byte[] buffer=new byte[filelen];
                string filename = Path.GetFileNameWithoutExtension(filepath);//��ȡ�ļ���

                CompressedStream = new GZipStream(sourstream, CompressionMode.Decompress, true);

                int br = CompressedStream.Read(buffer, 0, filelen);

                SerFile = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
                SerFile.Write(buffer, 0, filelen);                
                return "�ɹ�";
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
        /// ��ѹ����
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream DerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream;
            sourcestream.Position = 0;
            GZipStream compressedstream = new GZipStream(sourcestream, CompressionMode.Decompress, true);//ѹ����  
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
