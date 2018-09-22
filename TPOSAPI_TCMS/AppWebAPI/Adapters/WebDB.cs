using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.IO;
using System.Xml.Linq;
using RSL.EDI.UTIL;
using RSL.DES;

namespace AppWebAPI.Adapters
{
    /// <summary>
    /// Web使用的資料庫類別
    /// </summary>
    public partial class WebDB
    {
        #region 建構子
        /// <summary>
        /// Web使用的資料庫類別
        /// </summary>
        public WebDB ( )
        {
            //讀取資料連線設定檔
            DBconfig dbconfig = new DBconfig ( );
            ClassDB = new ClassDB (dbconfig.strConnectionString);
            conn = new OleDbConnection (dbconfig.strConnectionString);
            ClassFunction = new ClassFunction ( );
        }
        #endregion

        #region 屬性
        /// <summary>
        /// 資料庫類別
        /// </summary>
        public ClassDB ClassDB { get; set; }
        /// <summary>
        /// 連線物件
        /// </summary>
        public OleDbConnection conn { get; set; }
        /// <summary>
        /// 處理SQL值，將「'」改為「''」
        /// </summary>
        /// <param name="SqlValue">SQL值</param>
        /// <returns></returns>
        public string FunSqlValue ( string SqlValue )
        {
            return string.IsNullOrEmpty (SqlValue) ? "" : SqlValue.Replace ("'", "''");
        }
        /// <summary>
        /// 公用功能
        /// </summary>
        public ClassFunction ClassFunction { get; set; }
        #endregion
    }

    public partial class WebDB
    {
        /// <summary>
        /// 設定DB
        /// </summary>
        public class SetDB
        {
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


    public partial class WebDB
    {
        /// <summary>
        /// 語系設定
        /// </summary>
        public class langconfig
        {
            /// <summary>
            /// APP語系設定
            /// </summary>
            public langconfig ( )
            {
                LoadFile ( );
                Get ( );
            }

            #region 屬性
            /// <summary>
            /// XML檔案路徑
            /// </summary>
            private string _path
            {
                get
                {
                    return Path.Combine (AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"App_Data\langconfig.xml");
                }
            }
            /// <summary>
            /// lang設定檔(Linq)
            /// </summary>
            private XElement xlangconfig { get; set; }
            /// <summary>
            /// lang設定區(Linq)
            /// </summary>
            private XElement xlang { get; set; }
            /// <summary>
            /// 語系代碼
            /// </summary>
            public string strlang { get; set; }

            #endregion

            #region 方法
            /// <summary>
            /// 讀取檔案至(XElement)xdbconfig
            /// </summary>
            private void LoadFile ( )
            {
                if (File.Exists (_path))
                {
                    xlangconfig = XElement.Load (_path);
                    xlang = xlangconfig.Element ("lang");
                }
                else
                    throw new Exception ("本機安裝檔遺失，請洽系統管理員。");
            }

            /// <summary>
            /// 取得連線字串
            /// </summary>
            private void Get ( )
            {
                //取值
                strlang = xlang.Element ("value").Value;
            }

            /// <summary>
            /// 設定連線字串
            /// </summary>
            public void Set ( string lang )
            {
                //設定
                xlang.Element ("value").Value = lang;
                //存檔
                xlangconfig.Save (_path);
            }
            #endregion
        }



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
                normal = xconnectstring.Element ("normal").Value;
                sensitive_mask = xconnectstring.Element ("sensitive").Value;
                //解密                
                string sensitive = new DESClass (key, iv).desDecryptBase64 (sensitive_mask);
                strConnectionString = normal + sensitive;
            }

            /// <summary>
            /// 設定連線字串
            /// </summary>
            public void Set ( string normal, string sensitive )
            {
                //加密
                sensitive = new DESClass (key, iv).desEncryptBase64 (sensitive);
                //設定
                xconnectstring.Element ("normal").Value = normal;
                xconnectstring.Element ("sensitive").Value = sensitive;
                //存檔
                xdbconfig.Save (_path);
            }
            #endregion
        }





        /// <summary>
        /// MAIL設定
        /// </summary>
        public class mailconfig
        {
            /// <summary>
            /// APP語系設定
            /// </summary>
            public mailconfig ( )
            {
                LoadFile ( );
                Get ( );
            }

            #region 屬性
            /// <summary>
            /// XML檔案路徑
            /// </summary>
            public string _path
            {
                get
                {
                    return Path.Combine (AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"App_Data\mailconfig.xml");
                }
            }
            /// <summary>
            /// lang設定檔(Linq)
            /// </summary>
            private XElement xlangconfig { get; set; }
            /// <summary>
            /// lang設定區(Linq)
            /// </summary>
            private XElement xlang { get; set; }
            /// <summary>
            /// 寄件者
            /// </summary>
            public string sendaddress { get; set; }
            /// <summary>
            /// smtp
            /// </summary>
            public string smtp { get; set; }
            /// <summary>
            /// smtpport
            /// </summary>
            public string port { get; set; }
            /// <summary>
            /// smtp是否需要用SSL登入
            /// </summary>
            public string enablessl { get; set; }
            /// <summary>
            /// 登入帳號
            /// </summary>
            public string smtpaccount { get; set; }
            /// <summary>
            /// 登入密碼
            /// </summary>
            public string smtppassword { get; set; }

            #endregion

            #region 方法
            /// <summary>
            /// 讀取檔案至(XElement)xdbconfig
            /// </summary>
            private void LoadFile ( )
            {
                if (File.Exists (_path))
                {
                    xlangconfig = XElement.Load (_path);
                    xlang = xlangconfig.Element ("mail");
                }
                else
                    throw new Exception ("本機安裝檔遺失，請洽系統管理員。");
            }

            /// <summary>
            /// 取得連線字串
            /// </summary>
            private void Get ( )
            {
                //取值
                sendaddress = xlang.Element ("sendaddress").Value;
                smtp = xlang.Element ("smtp").Value;
                port = xlang.Element ("port").Value;
                enablessl = xlang.Element ("enablessl").Value;
                smtpaccount = xlang.Element ("smtpaccount").Value;
                smtppassword = xlang.Element ("smtppassword").Value;
            }

            /// <summary>
            /// 設定連線字串
            /// </summary>
            public void Set ( string lang )
            {
                //設定
                xlang.Element ("value").Value = lang;
                //存檔
                xlangconfig.Save (_path);
            }
            #endregion
        }


    }


}