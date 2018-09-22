﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;
using AppWebAPI.Adapters;
using System.Configuration;
using AppWebAPI.Models;
using AppWebAPI.Models.v1;
using AppWebAPI.Models.v1.Product;
using System.Web;


namespace TCMS_TO_TDB
{
    public partial class Form2 : Form
    {
        /// <summary>
        /// RCRM = Return Code and Return Message
        /// </summary>
        public RCRM rcrm { get; set; }

        /// <summary>
        /// 判斷API是否傳值
        /// </summary>
        class rcrm1
        {
            public string rc { get; set; }

            public string rm { get; set; }
        }
        /// <summary>
        /// Json傳過來的值
        /// </summary>
        class results
        {
            /// <summary>
            /// M 檔List
            /// </summary>
            public List<Sale_detail_result> Sale_detail { get; set; }
            /// <summary>
            /// P 檔List
            /// </summary>
            public List<Pay_detail_result> Pay_detail { get; set; }
            /// <summary>
            /// D 檔List
            /// </summary>
            public List<Good_detail_result> Good_detail { get; set; }

        }

        /// <summary>
        /// json 用
        /// </summary>
        class apiresult
        {

            public rcrm1 rcrm { get; set; }
            /// <summary>
            /// 接Json
            /// </summary>
            public results results { get; set; }
        }

        public Form2 ( )
        {
            InitializeComponent ( );
            InsertMariaDb ( );
        }

        #region insert MariaDB

        private void InsertMariaDb ( )
        {
            //MariaDd Server rm_prod_sales_m
            string apiurl = ConfigurationManager.AppSettings["apiurl"];
            string mdbstr = ConfigurationManager.AppSettings["mdbstr"];//Server
            MariaDB db_maria = new MariaDB (mdbstr);//連MariaDB
            string dbname_M = ConfigurationManager.AppSettings["dbname_M"];//MaricaDbName       
            string strSql = "";
            string str_request = "";
            string strReturn = "";
            string apiname = "";


            apiresult apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);

            if (apiresult.rcrm.rc != "1")
            {
                str_request = Newtonsoft.Json.JsonConvert.SerializeObject (apiresult.rcrm.rm);
            }


            //防止在寫入過程中任一筆失敗就還原整個資料
            db_maria.conn.Open ( );
            var trans = db_maria.conn.BeginTransaction ( );
            try
            {

                // HQ 交易紀錄 Header rm_prod_sales_m

                Sale_detail_result[ ] Lcd_m = apiresult.results.Sale_detail.ToArray<Sale_detail_result> ( ); //未填接資料

                if (Lcd_m.Length > 0)
                {

                    //TODO
                    // P 檔 table                    
                    Pay_detail_result[ ] Lcd_p = apiresult.results.Pay_detail.ToArray<Pay_detail_result> ( );
                    // D 檔 table
                    Good_detail_result[ ] Lcd_d = apiresult.results.Good_detail.ToArray<Good_detail_result> ( );

                    SaleData_Up_Result api_m_request = new SaleData_Up_Result ( );
                    List<Sale_detail_result> apiL_m = new List<Sale_detail_result> ( );

                    for (int k = 0; k < Lcd_m.Length; k++)
                    {
                        Sale_detail_result api_m = new Sale_detail_result ( );
                        //檢查是否已記錄
                        if (Lcd_m[k].API_URL.Equals ("Y"))
                            continue;
                        else
                        {
                            #region HQ 交易紀錄 Header rm_prod_sales_m M檔(主檔)

                            funInsertLog ("開始" + dbname_M + "rm_prod_sales_m");
                            apiname = "Query_SaleData_Up";
                            strSql = "insert into {0}rm_prod_sales_m ( ) values (  )";
                            strSql = string.Format (strSql, dbname_M, Lcd_m[k].intLintglobaltransno, Lcd_m[k].intIntpostransno, Lcd_m[k].dateDtmwhen.ToString ("yyyy-MM-dd"), Lcd_m[k].dateDtmfirstitem.ToString ("yyyy-MM-dd"), Lcd_m[k].dateDtmcheckout.ToString ("yyyy-MM-dd"), Lcd_m[k].dateDtmtrade.ToString ("yyyy-MM-dd"), Lcd_m[k].strStrstorecode, Lcd_m[k].strStrtillcode, Lcd_m[k].strStrtranstype, Lcd_m[k].strStrusercode, Lcd_m[k].intDblqty, Lcd_m[k].intCurfinalamount, Lcd_m[k].intCurchange, Lcd_m[k].intCurtax, Lcd_m[k].intCurpaymenttaxed, Lcd_m[k].intCurpaymentnotax, Lcd_m[k].strStrcusttaxid, Lcd_m[k].dateDtmoritransdatetime.ToString ("yyyy-MM-dd"), Lcd_m[k].strStroricusttaxid, Lcd_m[k].intLintoriglobaltransno, Lcd_m[k].intIntrefpostransno, Lcd_m[k].strStroritranstype, Lcd_m[k].strTweinv_Ysngenprove, Lcd_m[k].strTweinv_Ysnprintprove, Lcd_m[k].strTweinv_Ysnprinttransdtl, Lcd_m[k].strTweinv_Strfullinvnum, Lcd_m[k].strTweinv_Strbarcode, Lcd_m[k].strTweinv_Strqrcode1, Lcd_m[k].strTweinv_Strqrcode2, Lcd_m[k].strTweinv_Strrandom, Lcd_m[k].intTweinv_Curtotamtnotax, Lcd_m[k].intTweinv_Curtottax, Lcd_m[k].intTweinv_Curtotamtinctax, Lcd_m[k].intTweinv_Intrlno, Lcd_m[k].strTweinv_Ysndonate == "T" ? "" : "", Lcd_m[k].strTweinv_Strnpoban, Lcd_m[k].strTweinv_Ysnusecellphonebarcode == "T" ? "" : "", Lcd_m[k].strTweinv_Strcellphonebarcode, Lcd_m[k].strTweinv_Ysnusenaturepersonid == "T" ? "" : "", Lcd_m[k].strTweinv_Strnaturepersonid, Lcd_m[k].intCurdiscount, Lcd_m[k].dateDtmoritrade.ToString ("yyyy-MM-dd"), Lcd_m[k].strStroritillcode, Lcd_m[k].intCuroriamount, Lcd_m[k].intIntreprintcount, Lcd_m[k].strYsnvoid == "t" ? "" : "", Lcd_m[k].strStrcompcode, Lcd_m[k].strYsndiplomat == "T" ? "" : "", Lcd_m[k].strStrdiplomatcode, Lcd_m[k].intCurfinalamountnotax);//更新資料未填

                            if (db_maria.UpdData (strSql, db_maria.conn, trans) == -1)
                            {
                                //results = null;
                                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                                throw new Exception ("sql err");
                            }
                            #endregion

                            #region 更新 POSTRANSHDR API_URL表示資料已處理
                            api_m.API_URL = "Y";

                            funInsertLog ("結束" + dbname_M + "rm_prod_sales_m");

                            #endregion


                            #region PayMent rm_pay_type P檔

                            if (Lcd_p != null)
                            {
                                funInsertLog ("開始" + dbname_M + "rm_pay_type");
                                for (int p = 0; p < Lcd_p.Length; p++)
                                {
                                    if (Lcd_p[p].intLintglobaltransno != Lcd_m[k].intLintglobaltransno) continue;
                                    strSql = @"insert into {0}rm_pay_type (, ,   )values ('{1}', '{2}', '{3}')";
                                    strSql = string.Format (strSql, dbname_M, Lcd_p[p].intLintglobaltransno, Lcd_p[p].intIntsortno, Lcd_p[p].intIntpaymentno, Lcd_p[p].strStrpospaymentname, Lcd_p[p].intCurvalue, Lcd_p[p].dateDtmwhen.ToString ("yyyy-MM-dd"), Lcd_p[p].intIntreceiptpageno, Lcd_p[p].intCurfullvalue, Lcd_p[p].strYsntaxedbefore == "F" ? "" : "", Lcd_p[p].strYsncancancel, Lcd_p[p].strYsnopencashdrawer, Lcd_p[p].strYsnprintaskedcomment, Lcd_p[p].strYsnpointpay, Lcd_p[p].strYsnifmode, Lcd_p[p].strStrreceipt1, Lcd_p[p].strStrreceipt2, Lcd_p[p].strStrreceipt3, Lcd_p[p].strStrreceipt4, Lcd_p[p].strStrreceipt5, Lcd_p[p].strStrremark1, Lcd_p[p].strStrremark2, Lcd_p[p].strStrremark3, Lcd_p[p].strStrremark4, Lcd_p[p].strStrremark5,
                                        (Lcd_p[p].strStrifcardno.Substring (0, 2) == ("34,37") ? "A" : Lcd_p[p].strStrifcardno.Substring (0, 1) == ("3") ? "J" : Lcd_p[p].strStrifcardno.Substring (0, 4) == ("2131,1800") ? "J" : Lcd_p[p].strStrpaymenttype), Lcd_p[p].strYsnifprintcardno, Lcd_p[p].strYsnifprintreceipt1, Lcd_p[p].strYsnifprintreceipt2, Lcd_p[p].strYsnifprintreceipt3, Lcd_p[p].strYsnifprintreceipt4, Lcd_p[p].strYsnifprintreceipt5, Lcd_p[p].strStrcomment, Lcd_p[p].strStrauthcode, Lcd_p[p].strYsnvoid, Lcd_p[p].strStrsourceid, Lcd_p[p].intIntcount);
                                }

                                if (db_maria.UpdData (strSql, db_maria.conn, trans) == -1)
                                {
                                    //results = null;
                                    rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                                    throw new Exception ("sql err");
                                }


                                funInsertLog ("結束" + dbname_M + "rm_pay_type");
                            }
                            #endregion


                            #region HQ交易記錄 rm_prod_sales_d D檔

                            if (Lcd_d != null)
                            {
                                funInsertLog ("開始" + dbname_M + "rm_prod_sales_d");
                                for (int d = 0; d < Lcd_d.Length; d++)
                                {
                                    if (Lcd_d[d].intLintglobaltransno != Lcd_m[k].intLintglobaltransno) continue;
                                    strSql = @"insert into {0}rm_prod_sales_d (, ,   )values ('{1}', '{2}', '{3}')";
                                    strSql = string.Format (strSql, dbname_M, Lcd_d[d].intLintglobaltransno, Lcd_d[d].intIntsortno, Lcd_d[d].strStrtype, Lcd_d[d].intIntitemno, Lcd_d[d].intStritemnamepos, Lcd_d[d].intStrmodifier, Lcd_d[d].intCurprice, Lcd_d[d].intDblqty, Lcd_d[d].intCuroriamount, Lcd_d[d].intCurdiscount, Lcd_d[d].intCurfinalamount, Lcd_d[d].intCurtax, Lcd_d[d].intIntpriceno, Lcd_d[d].intCuroriprice, Lcd_d[d].intDblsaletaxrate, Lcd_d[d].intDbloriqty, Lcd_d[d].strYsnchangedprice, Lcd_d[d].strYsntrackserialno, Lcd_d[d].strYsnsetmeal, Lcd_d[d].strYsndiscountable, Lcd_d[d].strYsnaskcomment, Lcd_d[d].strYsnprintcomment, Lcd_d[d].strYsnchargeservice, Lcd_d[d].strStrclassify1code, Lcd_d[d].strStrclassify2code, Lcd_d[d].strStrclassify3code, Lcd_d[d].strStrclassify4code, Lcd_d[d].strStritemproperty1code, Lcd_d[d].strStritemproperty2code, Lcd_d[d].strStritemproperty3code, Lcd_d[d].intIntdiscountno_Mi, Lcd_d[d].intIntdiscountno_Ms, Lcd_d[d].intIntdiscountno_Am, Lcd_d[d].intCurunidiscount_Mi, Lcd_d[d].intCurunidiscount_Ms, Lcd_d[d].intCurunidiscount_Am, Lcd_d[d].strStrserialno, Lcd_d[d].strStritemcomment, Lcd_d[d].intIntoriitemno, Lcd_d[d].strStroriname, Lcd_d[d].strStrsourceid, Lcd_d[d].strStrextsref1);
                                }
                                if (db_maria.UpdData (strSql, db_maria.conn, trans) == -1)
                                {
                                    //results = null;
                                    rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                                    throw new Exception ("sql err");
                                }
                                //更新資料未填

                                funInsertLog ("結束" + dbname_M + "rm_prod_sales_d");
                            }

                            #endregion
                        }
                        //M檔
                        apiL_m.Add (api_m);
                    }

                    api_m_request.Sale_detail_result = apiL_m;

                    //轉成JSON格式
                    str_request = Newtonsoft.Json.JsonConvert.SerializeObject (api_m_request);

                    strReturn = GoOtherAPI (str_request, apiurl + apiname);

                    try
                    {
                        apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);

                        if (apiresult.rcrm.rc != "1")
                        {
                            //轉成JSON格式
                            str_request = Newtonsoft.Json.JsonConvert.SerializeObject (apiresult.rcrm.rm);
                        }
                    }
                    catch
                    {
                    }
                }
                trans.Commit ( );
            }
            catch (Exception e)
            {

                //results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                rcrm.RM = e.ToString ( );
                trans.Rollback ( );
            }
            finally
            {
                db_maria.conn.Close ( );
            }
        }
        #endregion


        /// <summary>
        /// 執行外部API
        /// </summary>
        /// <param name="request"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        private string GoOtherAPI ( string request, string URL )
        {

            try
            {
                var jsonBytes = Encoding.UTF8.GetBytes (request);
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create (URL);
                webrequest.Proxy = null;
                webrequest.Method = WebRequestMethods.Http.Post;
                webrequest.ContentType = "application/json";
                webrequest.ContentLength = jsonBytes.Length;

                using (var requestStream = webrequest.GetRequestStream ( ))
                {
                    requestStream.Write (jsonBytes, 0, jsonBytes.Length);
                    requestStream.Flush ( );
                    requestStream.Close ( );
                }
                #region 寫入log
                funInsertLog ("\r\n" + "JSON格式:" + request + "\r\n" + "URL:" + URL + "\r\n");
                #endregion

                #region 寫入log
                //funWriteLog ("\r\n" + webrequest.GetRequestStream ( ) + "\r\n");
                #endregion

                using (var webresponse = (HttpWebResponse)webrequest.GetResponse ( ))
                {

                    using (var stream = webresponse.GetResponseStream ( ))
                    using (var reader = new StreamReader (stream))
                    {
                        var webresult = reader.ReadToEnd ( );
                        webresponse.Close ( );
                        return webresult;
                    }
                }
            }
            catch (Exception ex)
            {
                #region 寫入log
                funInsertLog ("\r\n" + ex.Message + "\r\n" + "JSON格式=" + request + "\r\n" + "URL=" + URL + "\r\n");
                #endregion
                return ex.Message;
            }

        }

        #region 呼叫執行外部API 寫入LOG
        /// <summary>
        /// 寫入Log
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public static void funInsertLog ( string strLog )
        {
            string strPathDBLog = @"bin\IsertMariaDbLog";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, strPathDBLog, DateTime.Today.ToString ("yyyy"), DateTime.Today.ToString ("MM"));
            funAutoDirectory (strPath);
            strPath = Path.Combine (strPath, String.Format ("IsertMariaDbLog_{0}.log", DateTime.Today.ToString ("yyyyMMdd")));

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
}
