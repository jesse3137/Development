using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using AppWebAPI.Adapters;
using System.Data;
using Newtonsoft.Json.Converters;

using MySql.Data;
using MySql.Data.MySqlClient;

using System.Configuration;


namespace AppWebAPI.Models.v1
{
    /// <summary>
    /// WebAPI共用類別
    /// </summary>
    /// <typeparam name="Request"></typeparam>
    /// <typeparam name="Results"></typeparam>
    public abstract partial class APIBase<Request, Results>
    {

        /// <summary>
        /// WebAPI共用類別
        /// </summary>
        public APIBase ( )
        {           
            db = new WebDB ( );
            postaredb = new PostgreDB ();
            mariadb = new MariaDB ();
        }

        /// <summary>
        /// WebAPI共用類別
        /// </summary>
        /// <param name="request"></param>
        public APIBase ( Request request )
        {
            db = new WebDB ( );
            postaredb = new PostgreDB ( );
            mariadb = new MariaDB ( );

            this.request = request;
            if (request == null)    //必須有Request
            {
                rcrm = new RCRM (RC_Enum.FAIL_400_0001, "", ":request");
            }
            else
            {
                //不是ILoginModel(登入)，需要驗證登入
                if (!(this is Product.ILoginModel))
                {
                    //驗證登入,並取得userid
                    //string login_guid = (request as dynamic).account_id;
                    //string pos_id = (request as dynamic).pos_id;
                    //string account_token = (request as dynamic).access_token;
                    string login_guid = "";
                    string pos_id = "";
                    string account_token = "";
                    if (Verify_Login (login_guid, pos_id, account_token))
                    {
                        if (Verify_Request ( ))   //驗證資料 
                        {
                            //userid = GetUserID(login_guid);
                            GetResults ( ); //取得資料
                            SetRCRM ( ); //設定回傳訊息
                            //如果沒寫rcrm，代表成功
                            if (rcrm == null) rcrm = new RCRM (RC_Enum.OK);
                        }
                    }
                }
                else
                {
                    if (Verify_Request ( ))   //驗證資料 
                    {
                        GetResults ( ); //取得資料
                        SetRCRM ( ); //設定回傳訊息
                        //如果沒寫rcrm，代表成功
                        if (rcrm == null) rcrm = new RCRM (RC_Enum.OK);
                    }
                }
            }
            SaveRCRM ( ); //回存作業結果

            #region
            //var json = new System.Web.Script.Serialization.JavaScriptSerializer ( );
            //var jsonObjList = "";

            //try
            //{
            //    jsonObjList = json.Serialize (request);
            //}
            //catch
            //{ }

            //APIBase.funWriteLog (request.ToString ( ) + " " + jsonObjList, "API");
            //APIBase.funWriteLog (request.ToString ( ) + " " + json.Serialize (rcrm) + "\n" + json.Serialize (results), "API");

            #endregion
        }

        /// <summary>
        /// RCRM = Return Code and Return Message
        /// </summary>
        public RCRM rcrm { get; set; }
        /// <summary>
        /// 登入之使用者ID
        /// </summary>
        protected string userid { get; set; }
        /// <summary>
        /// 取得GUID
        /// </summary>
        protected string userguid { get; set; }
        /// <summary>
        /// 回傳檢查狀態
        /// </summary>
        protected int return_code { get; set; }
        /// <summary>
        /// 查詢條件
        /// </summary>
        protected virtual Request request { get; set; }
        /// <summary>
        /// 回傳資料
        /// </summary>
        public Results results { get; set; }
        /// <summary>
        /// 驗證資料
        /// </summary>
        protected abstract bool Verify_Request ( );
        /// <summary>
        /// 取得資料
        /// </summary>
        /// <returns></returns>
        protected abstract void GetResults ( );

        /// <summary>
        /// 設定回存訊息
        /// </summary>
        protected abstract void SaveRCRM ( );

        /// <summary>
        /// 設定回傳訊息
        /// </summary>
        protected abstract void SetRCRM ( );

        /// <summary>
        /// setDbname
        /// </summary>
        protected string DB_Service { get; set; }

        /// <summary>
        /// setDbname
        /// </summary>
        protected string DB_Url { get; set; }

        public string GetMD5 ( string Data )
        {
            System.Security.Cryptography.MD5 MD5Provider = System.Security.Cryptography.MD5.Create ( );
            byte[ ] source = System.Text.Encoding.Default.GetBytes (Data);//將字串轉為Byte[]
            byte[ ] crypto = MD5Provider.ComputeHash (source);//進行MD5加密

            string result = "";
            for (int i = 0; i < crypto.Length; i++)
            {
                result += crypto[i].ToString ("x2");//把加密後的字串從Byte[]轉為字串
            }

            return result;
        }


        #region 呼叫API 寫入LOG
        /// <summary>
        /// 寫入Log(呼叫API)
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public void funWriteLog ( string strLog, string accfolder, string posfolder )
        {
            string strPathDBLog = @"bin\APILog";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, strPathDBLog, DateTime.Today.ToString ("yyyy"), DateTime.Today.ToString ("MM"));

            if (accfolder != "")
                strPath = Path.Combine (strPath, accfolder);

            if (posfolder != "")
                strPath = Path.Combine (strPath, posfolder);

            funAutoDirectory (strPath);
            strPath = Path.Combine (strPath, String.Format ("APILog_{0}.log", DateTime.Today.ToString ("yyyyMMdd")));

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

    /// <summary>
    /// WebAPI共用類別-資料庫
    /// </summary>
    public abstract partial class APIBase<Request, Results>
    {
        string dbname = ConfigurationManager.AppSettings["dbname"];//分店db

        /// <summary>
        /// Web資料庫類別
        /// </summary>
        protected WebDB db { get; set; }

        /// <summary>
        /// PostgreDB資料庫類別
        /// </summary>
        protected PostgreDB postaredb { get; set; }

        /// <summary>
        /// MariaDB資料庫類別
        /// </summary>
        protected MariaDB mariadb { get; set; }

        /// <summary>
        /// SQL字串
        /// </summary>
        protected string strSql { get; set; }
        /// <summary>
        /// 資料表
        /// </summary>
        protected DataTable dt { get; set; }

        /// <summary>
        /// 驗證登入 - 回傳false=失敗,true=成功
        /// </summary>
        /// <param name="login_guid">登入的GUID</param>
        public bool Verify_Login ( string login_guid, string pos_id, string access_token )
        {
            //            strSql = string.Format(@"
            //                                    select * from user_pos p
            //                                    left join user_m m on m.store_id = p.store_id
            //                                    WHERE ACCESS_TOKEN = '{0}' AND POS_ID = '{1}' AND STATUS = '1'
            //                                    "
            //                                   , access_token
            //                                   , pos_id
            //                                   );

            //            dt = db_sql.GetData(strSql);
            //            if (dt != null && dt.Rows.Count > 0)
            //            {
            //                //檢測
            //                DB_Service = dt.Rows[0]["DB_NAME"].ToString() + ".";
            //                DB_Url = db.FunSqlValue(dt.Rows[0]["DBLINK_URL"].ToString());
            //                if (DB_Url != "")
            //                    db_maria.strDbUrl = DB_Url;

            //                return true;
            //            }
            //            else
            //            {
            //                rcrm = new RCRM(RC_Enum.FAIL_0);
            //                return false;
            //            }
            DB_Service = "";
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account_id"></param>
        /// <param name="pos_id"></param>
        /// <param name="api_request"></param>
        /// <param name="api_result"></param>
        /// <returns></returns>
        public int DoSqlLog ( string account_id, string pos_id, string api_request, string api_result, string api_name, string strrcrm )
        {
            return 0;
            //            strSql = string.Format(
            //                   @"
            //                    INSERT INTO API_LOG
            //                    (
            //                    ACCOUNT_ID,
            //                    POS_ID,
            //                    BTIME,
            //                    API_REQUEST,
            //                    API_RESULT,
            //                    API_NAME,
            //                    RCRM
            //                    )
            //                    VALUES
            //                    ('{0}','{1}','{2}',N'{3}',N'{4}','{5}',N'{6}')
            //                    "
            //                   , db.FunSqlValue(account_id)
            //                   , db.FunSqlValue(pos_id)
            //                   , DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")
            //                   , db.FunSqlValue(api_request)
            //                   , db.FunSqlValue(api_result)
            //                   , db.FunSqlValue(api_name)
            //                   , db.FunSqlValue(strrcrm)
            //                   );
            //            return db_sql.UpdData(strSql);
        }
    }

    /// <summary>
    /// WebAPI共用類別-靜態功能
    /// </summary>
    public abstract partial class APIBase
    {
        /// <summary>
        /// <para>說明 : 多語言keyword檢索 (由IMS UTIL.ASP而來, 譯者：Ray Huang)</para>
        /// <para>作者 : Robinson, Angus</para>
        /// <para>輸入 : I_sourceObj : 欲轉換語系之Keyword</para>
        /// <para>輸出 : 字串</para>
        /// <para>範例 : FunML("專櫃營運管理系統") = Lessee Operation Management System</para>
        /// <para>日期 : 2001/08/14, 2006/09/19</para>
        /// </summary>
        /// <param name="I_sourceObj">欲轉換語系之Keyword</param>
        /// <returns></returns>
        public static string FunML ( string I_sourceObj )
        {
            return FunML (I_sourceObj, new WebDB.langconfig ( ).strlang);
        }

        /// <summary>
        /// <para>說明 : 多語言keyword檢索 (由IMS UTIL.ASP而來, 譯者：Ray Huang)</para>
        /// <para>作者 : Robinson, Angus</para>
        /// <para>輸入 : I_sourceObj : 欲轉換語系之Keyword; I_langCode : 語系代碼</para>
        /// <para>輸出 : 字串</para>
        /// <para>範例 : FunML("專櫃營運管理系統", 0) = Lessee Operation Management System</para>
        /// <para>日期 : 2001/08/14, 2006/09/19</para>
        /// </summary>
        /// <param name="I_sourceObj">欲轉換語系之Keyword</param>
        /// <param name="I_langCode">語系代碼: 0=繁中, 1=英文, 2=簡中</param>
        /// <returns></returns>
        public static string FunML ( string I_sourceObj, string I_langCode )
        {
            string tmpsoureString = I_sourceObj.Trim ( );
            if (I_langCode == "0" || string.IsNullOrEmpty (I_langCode))  //不轉換,以原文顯示
            {
                return tmpsoureString;
            }
            else
            {
                string iniETEKDBUser = "IMS3_SYS.";
                string chaML_Sql = " SELECT TRANS_" + I_langCode + " CC FROM " + iniETEKDBUser + " MULTI_LANGS WHERE BASE_KEY = '" + tmpsoureString + "'";
                WebDB db = new WebDB ( );
                DataTable RS_ML = db.ClassDB.GetData (chaML_Sql);
                if (RS_ML == null)    //資料庫無法連線時
                {
                    return "@" + tmpsoureString;
                }
                else if (RS_ML.Rows.Count > 0)   //找到
                {
                    string strCC = RS_ML.Rows[0]["CC"].ToString ( );
                    if (string.IsNullOrEmpty (strCC)) //未設定,取原文
                    {
                        string update_sql = "";
                        update_sql = "UPDATE " + iniETEKDBUser + "MULTI_LANGS SET ";
                        update_sql = update_sql + "TRANS_3 ='" + tmpsoureString + "', ";
                        update_sql = update_sql + "CUSER = '" + "APP" + "', ";
                        update_sql = update_sql + "CTIME = SYSDATE";
                        update_sql = update_sql + " WHERE  BASE_KEY = '" + tmpsoureString + "'";
                        db.ClassDB.UpdData (update_sql);
                        return "@" + tmpsoureString;
                    }
                    else
                    {
                        return strCC;   //有設定,輸出
                    }
                }
                else
                {
                    string PG_ID = "APP";
                    chaML_Sql = " INSERT INTO " + iniETEKDBUser + " MULTI_LANGS ( BASE_KEY, TRANS_1, TRANS_2, PROG_ID, CTIME, CUSER, CPRIORITY, CGROUP, BUSER, BTIME) VALUES (";
                    chaML_Sql = chaML_Sql + " '" + tmpsoureString + "' ,'', '', '" + PG_ID + "', SYSDATE, 'APP', '1', 'super', 'xcom', SYSDATE)";
                    db.ClassDB.UpdData (chaML_Sql);
                    return "@" + tmpsoureString;
                }
            }
        }

        #region 呼叫API 寫入LOG
        /// <summary>
        /// 寫入Log(呼叫API)
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public static void funWriteLog ( string strLog, string folder )
        {
            string strPathDBLog = @"bin\APILog";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, strPathDBLog, DateTime.Today.ToString ("yyyy"), DateTime.Today.ToString ("MM"));
            if (folder != "")
                strPath = Path.Combine (strPath, folder);

            funAutoDirectory (strPath);
            strPath = Path.Combine (strPath, String.Format ("APILog_{0}.log", DateTime.Today.ToString ("yyyyMMdd")));

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
        public static DirectoryInfo funAutoDirectory ( string sFolderName )
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

    /// <summary>
    /// 轉換成yyyy/MM/dd的DateTime格式
    /// </summary>
    public class yyyyMMddDateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// 轉換成yyyy/MM/dd的DateTime格式
        /// </summary>
        public yyyyMMddDateTimeConverter ( )
        {
            base.DateTimeFormat = "yyyy/MM/dd";
        }
    }

    /// <summary>
    /// 必填的登入資訊介面(協定)
    /// </summary>
    public interface ILoginRequest
    {
    }

    /// <summary>
    /// 必填的登入資訊類別
    /// </summary>
    public class LoginRequest2 : ILoginRequest
    {
        /// <summary>
        /// 登入的GUID
        /// </summary>
        public string login_guid { get; set; }
    }


    /// <summary>
    /// 必填的登入資訊類別
    /// </summary>
    public class LoginRequest : ILoginRequest
    {
        /// <summary>
        /// 傳輸認證資料
        /// </summary>
        public class Credential
        {
            /// <summary>
            /// TOKEN
            /// </summary>
            public string user_access_token;
            /// <summary>
            /// 機器編號
            /// </summary>
            public string device_uuid;
        }

        /// <summary>
        /// 登入的GUID
        /// </summary>
        public Credential credential;
    }
}