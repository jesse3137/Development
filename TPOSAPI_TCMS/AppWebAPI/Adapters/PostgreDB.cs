using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using Npgsql;
using System.Data.Entity;
using System.Data;
using RSL.DES;
using System.Xml.Linq;

namespace AppWebAPI.Adapters
{
    /// <summary>
    /// PostgreDB使用的資料庫類別
    /// </summary>
    public partial class PostgreDB
    {
        #region 建構子
        /// <summary>
        /// PostgreDB使用的資料庫類別
        /// </summary>
        public PostgreDB ( )
        {
            ////讀取資料連線設定檔
            DBconfig dbconfig = new DBconfig ( );
          
            //讀取資料連線設定檔          
            conn = new NpgsqlConnection (dbconfig.strConnectionString);
        }

        /// <summary>
        /// PostgreDB使用的資料庫類別
        /// </summary>
        public PostgreDB ( string strConnectionString )
        {
            //讀取資料連線設定檔
            conn = new NpgsqlConnection (strConnectionString);

        }
        #endregion

        /// <summary>
        /// 連線物件
        /// </summary>
        public NpgsqlConnection conn { get; set; }
        /// <summary>
        /// 處理SQL值，將「'」改為「''」
        /// </summary>
        /// <param name="SqlValue">SQL值</param>
        /// <returns></returns>
        public string FunSqlValue ( string SqlValue )
        {
            return string.IsNullOrEmpty (SqlValue) ? "" : SqlValue.Replace ("'", "''");
        }

        //先自己寫
        /// <summary>
        /// 連線
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private bool GoConn ( NpgsqlConnection conn )
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open ( );
                }

                return true;
            }
            catch (NpgsqlException ex)
            {               
                    funWriteLog ("DB連線OPEN失敗:" + "\r\n" + ex.Message + conn.ConnectionString);
                    return false;            
            }
        }

        public void Reconn ( )
        {
            if (conn == null)
            {
                DBconfig dbconfig = new DBconfig ( );
                if (strDbUrl == "")
                    strDbUrl = dbconfig.strConnectionString;
                conn = new NpgsqlConnection (strDbUrl);

            }

            GoConn (conn);
        }

        /// <summary>
        /// SELECT
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetData ( string strSql )
        {
            if (conn == null)
            {
                DBconfig dbconfig = new DBconfig ( );
                if (strDbUrl == "")
                    strDbUrl = dbconfig.strConnectionString;
                conn = new NpgsqlConnection (strDbUrl);
            }

            DataTable dt = new DataTable ( );

            bool isreconn = true;
            if (conn.State == ConnectionState.Open)
            {
                isreconn = false;
            }

            //if (isreconn)
            //{
            if (!(GoConn (conn)))
            {
                dt = null;
                return dt;
            }
            //}

            NpgsqlCommand cmd = new NpgsqlCommand (strSql, conn);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter (cmd);
            try
            {
                da.Fill (dt);
            }
            catch (Exception ex)
            {
                funWriteLog ("SQL SELECT錯誤: " + strSql + "\r\n" + ex.ToString ( ));
                dt = null;
            }
            finally
            {
                //if (isreconn)
                conn.Close ( );

                cmd = null;
                da = null;
                GC.Collect ( );
            }

            return dt;
        }

        /// <summary>
        /// SELECT
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetData ( string strSql, NpgsqlConnection conn )
        {
            if (conn == null)
            {
                DBconfig dbconfig = new DBconfig ( );
                if (strDbUrl == "")
                    strDbUrl = dbconfig.strConnectionString;
                conn = new NpgsqlConnection (strDbUrl);
            }

            DataTable dt = new DataTable ( );

            NpgsqlCommand cmd = new NpgsqlCommand (strSql, conn);
            NpgsqlDataAdapter da = new NpgsqlDataAdapter (cmd);
            try
            {
                da.Fill (dt);
            }
            catch (Exception ex)
            {
                funWriteLog ("SQL SELECT錯誤: " + strSql + "\r\n" + ex.ToString ( ));
                dt = null;
            }
            finally
            {
                //conn.Close();
                cmd = null;
                da = null;
                GC.Collect ( );
            }

            return dt;
        }

        /// <summary>
        /// 更新 無TRANS 有錯誤回傳-1
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int UpdData ( string strSql )
        {
            if (conn == null)
            {
                DBconfig dbconfig = new DBconfig ( );
                if (strDbUrl == "")
                    strDbUrl = dbconfig.strConnectionString;
                conn = new NpgsqlConnection (strDbUrl);
            }

            int cnt = 0;
            if (GoConn (conn))
            {
                NpgsqlCommand cmd = new NpgsqlCommand (strSql, conn);
                try
                {
                    cnt = cmd.ExecuteNonQuery ( );
                }
                catch (Exception ex)
                {
                    funWriteLog ("SQL EXECUTE錯誤: " + strSql + "\r\n" + ex.ToString ( ));
                    cnt = -1;
                }
                finally
                {
                    cmd = null;
                    conn.Close ( );
                }
            }

            return cnt;
        }

        /// <summary>
        /// 更新  要先開TRANS, 有錯誤回傳-1
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int UpdData ( string strSql, NpgsqlConnection conn, NpgsqlTransaction trans )
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand (strSql, conn, trans);
                cmd.CommandTimeout = 600;
                int i = cmd.ExecuteNonQuery ( );

                return i;
            }
            catch (Exception ex)
            {
                funWriteLog ("SQL EXECUTE錯誤: " + strSql + "\r\n" + ex.ToString ( ));
                return -1;
            }
        }


        /// <summary>
        /// DBLog路徑
        /// </summary>
        public string strPathDBLog = @"bin\PostgreDBLog";

        /// <summary>
        /// DB Url路徑
        /// </summary>
        public string strDbUrl;


        #region  寫入Log(錯誤用)
        /// <summary>
        /// 寫入Log(錯誤用)
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public void funWriteLog ( string strLog )
        {
            //string strPath = strPathDBLog + @"\ErrLog_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, strPathDBLog, DateTime.Today.ToString ("yyyy"), DateTime.Today.ToString ("MM"));
            funAutoDirectory (strPath);
            strPath = Path.Combine (strPath, String.Format ("ErrLog_{0}.log", DateTime.Today.ToString ("yyyyMMdd")));

            try
            {
                using (StreamWriter sw = new StreamWriter (strPath, true))
                {
                    string strMessage = DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss") + ">" + strLog;
                    Console.WriteLine (strMessage);
                    sw.WriteLine (strMessage);
                }
            }
            catch { }

        }

        /// <summary>
        /// 自動建立資料夾
        /// </summary>
        /// <param name="sFolderName"></param>
        public DirectoryInfo funAutoDirectory ( string sFolderName )
        {
            if (!Directory.Exists (sFolderName))
            {
                return Directory.CreateDirectory (sFolderName);
            }
            else
            {
                return new DirectoryInfo (sFolderName);
            }
        }
        #endregion
    }
    public partial class PostgreDB
    {
        /// <summary>
        /// 設定DB
        /// </summary>
        public class SetDB
        {
            /**/
            /// <summary>
            /// 設定DB
            /// </summary>
            public SetDB ( )
            {
                //讀取資料連線設定檔
                DBconfig dbconfig = new DBconfig ( );
                //設定連線字串-normal
                normal = dbconfig.normal;
                //設定連線字串-sensitive_mask
                sensitive_mask = dbconfig.sensitive_mask;
            }

            #region 屬性
            /// <summary>
            /// 連線字串-normal
            /// </summary>
            public string normal { get; set; }
            /// <summary>
            /// 連線字串-sensitive_mask(已加密)
            /// </summary>
            public string sensitive_mask { get; set; }
            #endregion

            #region 方法
            /// <summary>
            /// 設定資料庫連線
            /// </summary>
            /// <param name="normal"></param>
            /// <param name="sensitive"></param>
            public void Set ( string normal, string sensitive )
            {
                DBconfig dbconfig = new DBconfig ( );
                dbconfig.Set (normal, sensitive);
            }
            #endregion
        }
    }

    public partial class PostgreDB
    {
        /// <summary>
        /// DB Config
        /// </summary>
        private class DBconfig
        {
            /// <summary>
            /// DB連線設定
            /// </summary>
            public DBconfig ( )
            {
                LoadFile ( );
                GetKeyAndIv ( );
                Get ( );
            }

            #region 屬性
            /// <summary>
            /// 完整連線字串
            /// </summary>
            public string strConnectionString { get; set; }
            /// <summary>
            /// 連線字串-normal
            /// </summary>
            public string normal { get; set; }
            /// <summary>
            /// 連線字串-sensitive_mask(已加密)
            /// </summary>
            public string sensitive_mask { get; set; }

            /// <summary>
            /// XML檔案路徑
            /// </summary>
            private string _path
            {
                get
                {
                    return //HttpContext.Current.Server.MapPath("~/App_Data/dbconfig.xml");
                        //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dbconfig.xml");
                        //Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                        Path.Combine (AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"App_Data\dbconfig.xml");
                }
            }
            /// <summary>
            /// key
            /// </summary>
            private string key { get; set; }
            /// <summary>
            /// iv
            /// </summary>
            private string iv { get; set; }
            /// <summary>
            /// 資料庫設定檔(Linq)
            /// </summary>
            private XElement xdbconfig { get; set; }
            /// <summary>
            /// 資料庫連線設定檔(Linq)
            /// </summary>
            private XElement xconnectstring { get; set; }
            #endregion

            #region 方法
            /// <summary>
            /// 讀取檔案至(XElement)xdbconfig
            /// </summary>
            private void LoadFile ( )
            {
                if (File.Exists (_path))
                {
                    xdbconfig = XElement.Load (_path);
                    xconnectstring = xdbconfig.Element ("connectstring");
                }
                else
                    throw new Exception ("本機安裝檔遺失，請洽系統管理員。");
            }

            /// <summary>
            /// 取得Key跟IV
            /// </summary>
            private void GetKeyAndIv ( )
            {
                key = xconnectstring.Element ("reg_code1").Value;
                iv = xconnectstring.Element ("reg_code2").Value;
            }

            /// <summary>
            /// 取得連線字串
            /// </summary>
            private void Get ( )
            {
                //取值
                normal = xconnectstring.Element ("Postgrenormal").Value;
                sensitive_mask = xconnectstring.Element ("sensitive").Value;
                //解密                
                string sensitive = new DESClass (key, iv).desDecryptBase64 (sensitive_mask);
                strConnectionString = normal + sensitive;
                //strConnectionString = normal;
            }

            /// <summary>
            /// 設定連線字串
            /// </summary>
            public void Set ( string normal, string sensitive )
            {
                //加密
                sensitive = new DESClass (key, iv).desEncryptBase64 (sensitive);
                //設定
                xconnectstring.Element ("Postgrenormal").Value = normal;
                xconnectstring.Element ("mariasensitive").Value = sensitive;
                //存檔
                xdbconfig.Save (_path);
            }

            #endregion
        }

    }
}