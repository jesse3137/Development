using System;
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


        class results
        {
            /// <summary>
            /// M 檔List
            /// </summary>
            public List<Sale_detail_result> Sale_detail_result { get; set; }

        }

        class apiresult
        {

            public RCRM rcrm { get; set; }
            /// <summary>
            /// 接Json
            /// </summary>
            public results results { get; set; }
        }

        string dowhat;
        public Form2 ( string param )
        {
            dowhat = param;
            InitializeComponent ( );
        }

        public void Form2_Load ( object sender, EventArgs e )
        {
            string dostatus = ConfigurationManager.AppSettings["dostatus"];

            if (dostatus != "Y")
                InsertMariaDb ( );
            else
                // UPPOS_STATUS ( );
                this.Close ( );
        }

        #region insert MariaDB

        private void InsertMariaDb ( )
        {

            string apiurl = ConfigurationManager.AppSettings["apiurl"];
            string mdbstr = ConfigurationManager.AppSettings["mdbstr"];//Server
            MariaDB db_maria = new MariaDB (mdbstr);//連MariaDB
            string dbname_M = ConfigurationManager.AppSettings["dbname_M"];//MaricaDbName       
            string strSql = "";
            string str_request = "";
            string apiname = "";
            string strReturn = "";
            apiresult apiresult = null;
            string strQuerySales_Date = ConfigurationManager.AppSettings["strQuerySales_Date"];//查詢日期
            string strQueryPosId = ConfigurationManager.AppSettings["strQueryPosId"];//查詢POSID

            List<SaleData_Up_Request> api_m_request1 = new List<SaleData_Up_Request> ( );
            SaleData_Up_Request dr = new SaleData_Up_Request ( );
            dr.Sales_Date = strQuerySales_Date;
            dr.PosId = strQueryPosId;
            apiname = "Query_SaleData_Up";

            api_m_request1.Add (dr);
            str_request = Newtonsoft.Json.JsonConvert.SerializeObject (dr);
            strReturn = GoOtherAPI (str_request, apiurl + apiname);
            try
            {
                apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);
                if (apiresult.rcrm.RC != "1")
                {
                    str_request = Newtonsoft.Json.JsonConvert.SerializeObject (apiresult.rcrm.RM);
                }

            }
            catch
            {
            }
            // HQ 交易紀錄 Header rm_prod_sales_m

            Sale_detail_result[ ] Lcd_m = apiresult.results.Sale_detail_result.ToArray<Sale_detail_result> ( );

            if (Lcd_m.Length > 0)
            {
                SaleData_Up_Result api_m_request = new SaleData_Up_Result ( );
                List<Sale_detail_result> apiL_m = new List<Sale_detail_result> ( );

                for (int k = 0; k < Lcd_m.Length; k++)
                {
                    Sale_detail_result api_m = new Sale_detail_result ( );
                    //檢查是否已記錄
                    strSql = String.Format (@"select * from {0}rm_prod_sales_m where TRANS_NO='{1}' and POS_ID='{2}' and SALES_DATE='{3}' and SHOP_ID='{4}'", dbname_M, Lcd_m[k].intIntpostransno, Lcd_m[k].strStrtillcode, Lcd_m[k].dateDtmwhen.ToString ("yyyy-MM-dd"), Lcd_m[k].strSubstorecode);
                    DataTable dt = db_maria.GetData (strSql);
                    if (dt.Rows.Count != 0) continue;

                    //防止在寫入過程中任一筆失敗就還原整個資料
                    db_maria.conn.Open ( );
                    var trans = db_maria.conn.BeginTransaction ( );
                    try
                    {
                        #region HQ 交易紀錄 Header rm_prod_sales_m M檔 銷售主檔紀錄 (主檔)
                        funInsertLog ("開始" + dbname_M + "rm_prod_sales_m");
                        if (Lcd_m[k].strStrtranstype == "Reprint") continue;
                        apiname = "Query_SaleData_Up";
                        strSql = @"insert into {0}rm_prod_sales_m (TRANS_NO, TDATE, RECORD_BEGIN, SALES_DATE, SHOP_ID, POS_ID, TRANS_TYPE, RECEIVER_ID, TOT_QTY, NET, GROSSPLUS, GROSSNG, RATE_AMT, COMP_ID, ORG_GUI_DATE, ORG_TRANS_NO, ORG_POS_ID , EUI_PRINT, EUI_PRINT_TRANS, RECE_TRACK, GUI_BEGIN, EUI_RANDOM_CODE, EUI_DONATE, EUI_DONATE_NO, EUI_VEHICLE_TYPE_NO, EUI_VEHICLE_NO, EUI_PRINT_CNT, NRT_CARDNO, STORE_ID, RECORD_END) values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', {15}, '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{3}')";

                        decimal decGross = 0;//負向金額 
                        decimal decGrossPlus = 0;//正負向金額 
                        string strTrans_Type = "";//正向塞01 當日反向(作廢)塞03 不同日(退貨)塞02
                        string strOrg_Trand_No = "";//被作廢、退貨的發票序號
                        string strOrg_Pos_Id = "";//被作廢、退貨的發票機台

                        if (Lcd_m[k].strYsnvoid.ToUpper ( ) == "T")
                        {
                            decGross = Math.Abs (decimal.Parse (Lcd_m[k].strCurfinalamount));
                            strTrans_Type = "03";
                            strOrg_Trand_No = Lcd_m[k].strIntrefpostransno;
                            strOrg_Pos_Id = Lcd_m[k].strStrtillcode;
                        }
                        else if (Lcd_m[k].strStrtranstype == "Return")
                        { decGross = Math.Abs (decimal.Parse (Lcd_m[k].strCurfinalamount)); strTrans_Type = "02"; }
                        else if (decGross == 0)
                        { decGrossPlus = decimal.Parse (Lcd_m[k].strCurfinalamount); strTrans_Type = "01"; }


                        String strEuiVehicleTypeNo = "";//電子發票載具類別編號 手機條碼：3J0002、自然人憑證：CQ0001
                        String strEuiVehicleNo = "";//電子發票載具卡號
                        if (Lcd_m[k].strTweinv_Ysnusecellphonebarcode.ToUpper ( ) == "T")
                        {
                            strEuiVehicleTypeNo = "3J0002"; strEuiVehicleNo = Lcd_m[k].strTweinv_Strcellphonebarcode;
                        }
                        else if (Lcd_m[k].strTweinv_Ysnusenaturepersonid.ToUpper ( ) == "T")
                        { strEuiVehicleTypeNo = "CQ0001"; strEuiVehicleNo = Lcd_m[k].strTweinv_Strnaturepersonid; }

                        strSql = String.Format (strSql, dbname_M, Lcd_m[k].intIntpostransno, Lcd_m[k].dateDtmwhen.ToString ("yyyy-MM-dd"),
                            Lcd_m[k].dateDtmcheckout.ToString ("HHmmss"), Lcd_m[k].dateDtmtrade.ToString ("yyyy-MM-dd"),
                            Lcd_m[k].strSubstorecode, Lcd_m[k].strStrtillcode,
                            strTrans_Type, Lcd_m[k].strStrusercode,
                            Lcd_m[k].strDblqty, Lcd_m[k].strCurfinalamount,
                            decGrossPlus, decGross,
                            Lcd_m[k].strTweinv_Curtottax, Lcd_m[k].strStrcusttaxid,
                            strTrans_Type == "01" ? "null" : ("'" + Lcd_m[k].dateDtmoritransdatetime.ToString ("yyyy-MM-dd") + "'"), strOrg_Trand_No,
                            strOrg_Pos_Id, Lcd_m[k].strTweinv_Ysnprintprove.ToUpper ( ) == "T" ? 1 : 2,
                            Lcd_m[k].strTweinv_Ysnprinttransdtl.ToUpper ( ) == "T" ? 1 : 2, Lcd_m[k].strTweinv_Strfullinvnum.Substring (0, 2),
                            Lcd_m[k].strTweinv_Strfullinvnum.Substring (2, 8), Lcd_m[k].strTweinv_Strrandom,
                            Lcd_m[k].strTweinv_Ysndonate.ToUpper ( ) == "T" ? "T" : "", Lcd_m[k].strTweinv_Strnpoban, strEuiVehicleTypeNo, strEuiVehicleNo, Lcd_m[k].strIntreprintcount != "" ? int.Parse (Lcd_m[k].strIntreprintcount) : 0,
                            Lcd_m[k].strStrdiplomatcode, Lcd_m[k].strSubstorecode);

                        #region test
                        /*
                        strSql = @"insert into {0}rm_prod_sales_m (TENANTS_SELL_ORG_NO, SALES_DATE, RECORD_BEGIN, TDATE, SHOP_ID, POS_ID, TRANS_TYPE, RECEIVER_ID, TOT_QTY, NET, RATE_AMT, COMP_ID, TENANTS_SELL_TIME, TRANS_NO, TENANTS_SELL_TRANSTYPE, EUI_PRINT, EUI_PRINT_TRANS, EUI_RANDOM_CODE, EUI_DONATE, EUI_DONATE_NO, EUI_VEHICLE_TYPE_NO, EUI_VEHICLE_NO, TENANTS_SELL_ORG_DAY, TENANTS_SELL_ORG_POSID, EUI_PRINT_CNT, BAD_YN) values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}')";

                        strSql = string.Format (strSql, dbname_M,
                        Lcd_m[k].intLintglobaltransno, Lcd_m[k].dateDtmwhen.ToString ("yyyy-MM-dd"),
                        Lcd_m[k].dateDtmcheckout.ToString ("yyyy-MM-dd"), Lcd_m[k].dateDtmtrade.ToString ("yyyy-MM-dd"),
                        Lcd_m[k].strStrstorecode, Lcd_m[k].strStrtillcode,
                        Lcd_m[k].strStrtranstype, Lcd_m[k].strStrusercode,
                        Lcd_m[k].intDblqty, Lcd_m[k].intCurfinalamount, Lcd_m[k].intCurtax,
                        Lcd_m[k].strStrcusttaxid, Lcd_m[k].dateDtmoritransdatetime.ToString ("yyyy-MM-dd"),
                        Lcd_m[k].intIntrefpostransno, Lcd_m[k].strStroritranstype,
                        Lcd_m[k].strTweinv_Ysnprintprove, Lcd_m[k].strTweinv_Ysnprinttransdtl,
                        Lcd_m[k].strTweinv_Strrandom,
                        Lcd_m[k].strTweinv_Ysndonate == "T" ? "T" : "", Lcd_m[k].strTweinv_Strnpoban,
                        Lcd_m[k].strTweinv_Ysnusecellphonebarcode == "T" ? "3J0002" : Lcd_m[k].strTweinv_Ysnusenaturepersonid == "T" ? "CQ0001" : "",
                        Lcd_m[k].strTweinv_Ysnusecellphonebarcode == "T" ? Lcd_m[k].strTweinv_Strcellphonebarcode : Lcd_m[k].strTweinv_Ysnusenaturepersonid == "T" ? Lcd_m[k].strTweinv_Strnaturepersonid : "",
                       Lcd_m[k].dateDtmoritrade.ToString ("yyyy-MM-dd"),
                        Lcd_m[k].strStroritillcode,
                        Lcd_m[k].intIntreprintcount, Lcd_m[k].strYsnvoid == "t" ? "Y" : "");//更新資料未填
                        */
                        #endregion

                        if (db_maria.UpdData (strSql, db_maria.conn, trans) == -1)
                        {
                            rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                            throw new Exception ("sql err");
                        }
                        #endregion

                        funInsertLog ("結束" + dbname_M + "rm_prod_sales_m");

                        #region PayMent rm_pay_type P檔 支付工具紀錄檔

                        List<Pay_detail_result> Lcd_p = Lcd_m[k].Pay_detail;
                        if (Lcd_p != null)
                        {
                            funInsertLog ("開始" + dbname_M + "rm_pay_type");
                            for (int p = 0; p < Lcd_p.Count; p++)
                            {
                                if (Lcd_p[p].intLintglobaltransno != Lcd_m[k].intLintglobaltransno) continue;
                                strSql = @"insert into {0}rm_pay_type (TENDER, AMOUNT, SEC_NO, APPROVAL_CODE, SEC_TYPE, TRANS_NO, SALES_DATE, POS_ID, STORE_ID, SHOP_ID) values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{9}')";

                                string strStrpaymenttype = "";//卡別
                                if (Lcd_p[p].strStrpaymenttype == "" && Lcd_p[p].strStrifcardno != "")
                                {
                                    if (Lcd_p[p].strStrifcardno.Substring (0, 2) == "34" || Lcd_p[p].strStrifcardno.Substring (0, 2) == "37") strStrpaymenttype = "A";
                                    else if (Lcd_p[p].strStrifcardno.Substring (0, 1) == "3" || Lcd_p[p].strStrifcardno.Substring (0, 4) == "2131" || Lcd_p[p].strStrifcardno.Substring (0, 4) == "1800") strStrpaymenttype = "J";
                                    else if (Lcd_p[p].strStrifcardno.Substring (0, 1) == "4") strStrpaymenttype = "V";
                                    else if (51 <= int.Parse (Lcd_p[p].strStrifcardno.Substring (0, 2)) && int.Parse (Lcd_p[p].strStrifcardno.Substring (0, 2)) <= 54) strStrpaymenttype = "M";
                                }
                                else if (Lcd_p[p].strStrpaymenttype != "") strStrpaymenttype = Lcd_p[p].strStrpaymenttype;

                                strSql = string.Format (strSql, dbname_M, Lcd_p[p].intIntpaymentno, Lcd_p[p].strCurvalue,
                                    Lcd_p[p].strStrifcardno, Lcd_p[p].strStrauthcode,
                                    strStrpaymenttype, Lcd_m[k].intIntpostransno,
                                    Lcd_m[k].dateDtmtrade.ToString ("yyyy-MM-dd"), Lcd_m[k].strStrtillcode,
                                    Lcd_m[k].strSubstorecode);
                                //Lcd_p[p].strStrsourceid  //db無此欄位         

                                #region test
                                /*
                                strSql = @"insert into {0}rm_pay_type (TRANS_NO,POS_ID , TENDER, KEEPCHANGE, SALES_DATE, OVER_PAY, USE_BONUS, SEC_NO, APPROVAL_CODE, SEC_TYPE )values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')";
                                string type = Lcd_p[p].strStrpaymenttype != "" ? Lcd_p[p].strStrpaymenttype : Lcd_p[p].strStrifcardno.Substring (0, 2) == ("34,37") ? "A" : Lcd_p[p].strStrifcardno.Substring (0, 1) == ("3") ? "J" : Lcd_p[p].strStrifcardno.Substring (0, 4) == ("2131,1800") ? "J" : "";

                                strSql = string.Format (strSql, dbname_M,
                                Lcd_p[p].intLintglobaltransno, "0001",
                                Lcd_p[p].intIntpaymentno,
                                Lcd_p[p].intCurvalue,
                                Lcd_p[p].dateDtmwhen.ToString ("yyyy-MM-dd"),
                                Lcd_p[p].intCurfullvalue,
                                Lcd_p[p].strYsnpointpay, Lcd_p[p].strStrifcardno, Lcd_p[p].strStrauthcode,
                                    type);
                                */
                                #endregion


                                if (db_maria.UpdData (strSql, db_maria.conn, trans) == -1)
                                {
                                    //results = null;
                                    rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                                    throw new Exception ("sql err");
                                }

                            }

                            funInsertLog ("結束" + dbname_M + "rm_pay_type");
                        }
                        #endregion


                        #region HQ交易記錄 rm_prod_sales_d D檔

                        List<Good_detail_result> Lcd_d = Lcd_m[k].Good_detail;

                        if (Lcd_d != null)
                        {
                            funInsertLog ("開始" + dbname_M + "rm_prod_sales_d");
                            for (int d = 0; d < Lcd_d.Count; d++)
                            {
                                if (Lcd_d[d].intLintglobaltransno != Lcd_m[k].intLintglobaltransno) continue;
                                if (Lcd_d[d].strIntitemno == "") continue;
                                strSql = @"insert into {0}rm_prod_sales_d (TRANS_NO, SALES_DATE, POS_ID, GOODS_ID, GOODS_NAME, LIST_PRICE, QTY, SELL_PRICE, DISC_AMT, SALES_AMT, RATE_AMT, STORE_ID, SHOP_ID, GUI_NO)values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{12}', '{13}')";
                                strSql = string.Format (strSql, dbname_M,
                                    Lcd_m[k].intIntpostransno, Lcd_m[k].dateDtmtrade.ToString ("yyyy-MM-dd"),
                                    Lcd_m[k].strStrtillcode, Lcd_d[d].strIntitemno,
                                    Lcd_d[d].strStritemnamepos, Lcd_d[d].strCurprice,
                                    Lcd_d[d].strDblqty, Lcd_d[d].strCuroriamount,
                                    Lcd_d[d].strCurdiscount, Lcd_d[d].strCurfinalamount,
                                    Lcd_m[k].strTweinv_Curtottax, Lcd_m[k].strSubstorecode,
                                     Lcd_m[k].strTweinv_Strfullinvnum);

                                //Lcd_d[d].strStrsourceid, Lcd_d[d].strStrextsref1);//db無此欄位
                                #region test
                                /*
                                strSql = @"insert into {0}rm_prod_sales_d ( TRANS_NO, ND_TYPE, GOODS_NAME, SELL_PRICE, QTY, LIST_PRICE, PRIMARY_CATEGORY, SECONDARY_CATEGORY, MINOR_CATEGORY)values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";
                                strSql = string.Format (strSql, dbname_M,
                            Lcd_d[d].intLintglobaltransno, Lcd_d[d].intIntitemno,
                            Lcd_d[d].intStritemnamepos,
                            Lcd_d[d].intCurprice, Lcd_d[d].intDblqty, Lcd_d[d].intCuroriprice,
                            Lcd_d[d].strStrclassify1code,
                            Lcd_d[d].strStrclassify2code, Lcd_d[d].strStrclassify3code);
                                */
                                #endregion

                                if (db_maria.UpdData (strSql, db_maria.conn, trans) == -1)
                                {
                                    //results = null;
                                    rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                                    throw new Exception ("sql err");
                                }
                            }

                            funInsertLog ("結束" + dbname_M + "rm_prod_sales_d");
                        }

                        #endregion


                        trans.Commit ( );
                    }
                    catch (Exception e)
                    {
                        rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                        rcrm.RM = e.ToString ( );
                        funInsertLog (e.ToString ( ));
                        trans.Rollback ( );
                    }
                    finally
                    {
                        db_maria.conn.Close ( );
                    }
                }
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
        public static void funJsonLog ( string strLog )
        {
            string strPathDBLog = @"bin\JsonLog";
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
