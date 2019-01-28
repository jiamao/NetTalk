using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Web;

namespace LanTalkServer
{
    class UserHelper
    {
        //static usersDataSetTableAdapters.UsersTableAdapter userdb = new LanTalkServer.usersDataSetTableAdapters.UsersTableAdapter();
        static OleDbConnection oldcn = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["LanTalkServer.Properties.Settings.usersConnectionString"].ConnectionString);
        static OleDbCommand oldcm = new OleDbCommand("",oldcn);
        static OleDbDataAdapter olada = new OleDbDataAdapter();
        private static void open()
        {
            try
            {
                if (oldcn.State == ConnectionState.Closed)
                {
                    oldcn.Open();
                }
            }
            catch
            { }
        }
        /// <summary>
        /// �����û���½
        /// </summary>
        /// <param name="info"></param>
        public static bool netLogin(string userid,string userpwd)
        {
            try
            {
                oldcm.CommandText = "select * from Users where User_ID='" + userid + "' and User_PWD='" + userpwd + "'";
                olada.SelectCommand = oldcm;
                open();
                DataSet ds = new DataSet();
                olada.Fill(ds);
                oldcn.Close();
                return ds.Tables[0].Rows.Count > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                oldcn.Close();
            }
        }
        /// <summary>
        /// �����û��޸�����
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="userpwd"></param>
        /// <returns></returns>
        public static bool netUpdatePWD(string userid, string userpwd)
        {
            try
            {
                oldcm.CommandText = "update Users set User_PWD='" + userpwd + "' where User_ID='" + userid + "'";
                open();
                int re=oldcm.ExecuteNonQuery();
                oldcn.Close();
                return re > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                oldcn.Close();
            }
        }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <param name="usrid"></param>
        /// <param name="userpwd"></param>
        /// <returns></returns>
        public static bool netAddUser(string usrid, string userpwd)
        {
            try
            {
                userpwd = serPWD(userpwd);
                open();
                oldcm.CommandText = "select User_ID from Users where User_ID = '" + usrid + "'";
                olada.SelectCommand = oldcm;
                DataSet ds = new DataSet();
                olada.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    oldcn.Close();
                    throw new Exception("�û����Ѿ�����");
                }
                oldcm.CommandText = "insert into Users(User_ID,User_PWD) values('" + usrid + "','" + userpwd + "')";

                int re = oldcm.ExecuteNonQuery();

                return re > 0;
            }
            //catch
            //{
            //    return false;
            //}
            finally
            {
                oldcn.Close();
            }
        }
        /// <summary>
        /// ���ؼ��ܺ������
        /// </summary>
        /// <param name="userpwd"></param>
        /// <returns></returns>
        public static string serPWD(string userpwd)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(userpwd, "md5"); 
        }
    }
}
