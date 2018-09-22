using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json.Converters;

using System.Configuration;


namespace AppWebAPI.Models.v1.Product
{
    /// <summary>
    /// 權限控管-登入
    /// </summary>
    public class Download_Pos_Model : _Product_Model<Download_Pos_Request, Download_Pos_Result>
    {
        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Download_Pos_Model ( ) { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Download_Pos_Model ( Download_Pos_Request request ) : base (request) { }

        /// <summary>
        /// 驗證資料
        /// </summary>
        /// <returns></returns>
        protected override bool Verify_Request ( )
        {
            bool OK = true;    //是否驗證通過

            return OK;
        }


        #region DB 判斷值
        string oracledb = ConfigurationManager.AppSettings["oracledb"];
        string postgredb = ConfigurationManager.AppSettings["postgredb"];
        #endregion
     

        /// <summary>
        /// 取得資料
        /// </summary>
        protected override void GetResults ( )
        {
            pos_detail[ ] Lcd = request.pos_detail.ToArray<pos_detail> ( );

            funWriteLog ("Till數量" + Lcd.Length.ToString ( ), "", "");

            var json = new System.Web.Script.Serialization.JavaScriptSerializer ( );
            funWriteLog (json.Serialize (request), "", "");

            strSql = string.Format (@" SELECT * FROM  {0}Till ", DB_Service);
   
            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt = db.ClassDB.GetData (strSql);
            #endregion


            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt = postaredb.GetData (strSql);
         
            #endregion

            #endregion


            if (dt == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }

            bool isnew = false;

            try
            {
                isnew = dt.Columns.Contains ("strCompCode");
            }
            catch
            { }

            for (int i = 0; i < Lcd.Length; i++)
            {
                DataRow[ ] dra = dt.Select ("strTillCode = '" + Lcd[i].strtillcode + "'");

                if (dra.Length > 0)
                {
                    strSql = "update {0}Till set strTillName = '{1}',strStoreCode = '{2}',ysnActive = '{3}',strTillType = '{4}',intTillProfileNo = '{5}',EINV_YSNENABLE='{7}' ,EINV_YSNTESTMODE='{8}' where strTillCode = '{6}' ";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strtillname,
                                            Lcd[i].strstorecode,
                                            Lcd[i].ysnactive,
                                            Lcd[i].strtilltype,
                                            Lcd[i].strtillprofileno,
                                            Lcd[i].strtillcode, Lcd[i].strEinv_ysnenable, Lcd[i].strEinv_ysntestimode);

                    #region 判斷使用DB

                    #region oracledb
                    if (!string.IsNullOrEmpty (oracledb))
                        db.ClassDB.UpdData (strSql);
                    #endregion

                    #region  postgredb
                    if (!string.IsNullOrEmpty (postgredb))
                        postaredb.UpdData (strSql);
                  
                    #endregion

                    #endregion

                }
                else
                {
                    if (isnew)
                    {
                        strSql = @"insert into {0}Till (strTillCode, strTillName, strStoreCode, ysnActive, strTillType, intPosFuncKeyNo, intPOsFastKeyNo, intTillProfileNo, intDualMonitorNo, strCompCode, EINV_YSNENABLE,
                                 EINV_YSNTESTMODE)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '1', '{6}', '{7}', '1', 'AEON','{8}', '{9}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strtillcode,
                                                Lcd[i].strtillname,
                                                Lcd[i].strstorecode,
                                                Lcd[i].ysnactive,
                                                Lcd[i].strtilltype,
                                                Lcd[i].intPosfastkeyno,
                                                Lcd[i].strtillprofileno, Lcd[i].strEinv_ysnenable,
                                                Lcd[i].strEinv_ysntestimode);

                        #region 判斷使用DB

                        #region oracledb
                        if (!string.IsNullOrEmpty (oracledb))
                            db.ClassDB.UpdData (strSql);
                        #endregion

                        #region  postgredb
                        if (!string.IsNullOrEmpty (postgredb))
                            postaredb.UpdData (strSql);
                      
                        #endregion

                        #endregion


                        strSql = @"insert into {0}TILLSTATUS (strTillCode, strStatus, strCompCode)
                                values ('{1}', '{2}', 'AEON')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strtillcode,
                                                "F");

                        #region 判斷使用DB

                        #region oracledb
                        if (!string.IsNullOrEmpty (oracledb))
                            db.ClassDB.UpdData (strSql);
                        #endregion

                        #region  postgredb
                        if (!string.IsNullOrEmpty (postgredb))
                            postaredb.UpdData (strSql);
                    
                        #endregion

                        #endregion

                    }
                    else
                    {
                        strSql = @"insert into {0}Till (strTillCode, strTillName, strStoreCode, ysnActive, strTillType, intPosFuncKeyNo, intPOsFastKeyNo, intTillProfileNo, intDualMonitorNo, EINV_YSNENABLE, EINV_YSNTESTMOD)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '1', '{6}', '{7}', '1', '{8}', '{9}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strtillcode,
                                                Lcd[i].strtillname,
                                                Lcd[i].strstorecode,
                                                Lcd[i].ysnactive,
                                                Lcd[i].strtilltype,
                                                Lcd[i].intPosfastkeyno,
                                                Lcd[i].strtillprofileno, Lcd[i].strEinv_ysnenable,
                                                Lcd[i].strEinv_ysntestimode);

                        #region 判斷使用DB

                        #region oracledb
                        if (!string.IsNullOrEmpty (oracledb))
                            db.ClassDB.UpdData (strSql);
                        #endregion

                        #region  postgredb
                        if (!string.IsNullOrEmpty (postgredb))
                            postaredb.UpdData (strSql);
                     
                        #endregion

                        #endregion


                        strSql = @"insert into {0}TILLSTATUS (strTillCode, strStatus)
                                values ('{1}', '{2}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strtillcode,
                                                "F");

                        #region 判斷使用DB

                        #region oracledb
                        if (!string.IsNullOrEmpty (oracledb))
                            db.ClassDB.UpdData (strSql);
                        #endregion

                        #region  postgredb
                        if (!string.IsNullOrEmpty (postgredb))                     
                            postaredb.UpdData (strSql);
                       
                        #endregion

                        #endregion

                    }

                    strSql = @"insert into {0}TillDevice (strTillCode, strDeviceCode, strDeviceKind, strPort, intBaudRate, strDataFormat, strDeviceTypeCode)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strtillcode,
                                            "PP802",
                                            "ReceiptPrinter",
                                            "COM3",
                                            "9600",
                                            "N-8-1",
                                            "PP802");

                    #region 判斷使用DB

                    #region oracledb
                    if (!string.IsNullOrEmpty (oracledb))
                        db.ClassDB.UpdData (strSql);
                    #endregion

                    #region  postgredb
                    if (!string.IsNullOrEmpty (postgredb))
                        postaredb.UpdData (strSql);
                   
                    #endregion

                    #endregion


                    strSql = @"insert into {0}TillDevice (strTillCode, strDeviceCode, strDeviceKind, strPort, intBaudRate, strDataFormat, strDeviceTypeCode)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strtillcode,
                                            "CashDrawer",
                                            "CashDrawer",
                                            "OPOS",
                                            "",
                                            "",
                                            "RIPAC-CASHDRAWER");

                    #region 判斷使用DB

                    #region oracledb
                    if (!string.IsNullOrEmpty (oracledb))
                        db.ClassDB.UpdData (strSql);
                    #endregion

                    #region  postgredb
                    if (!string.IsNullOrEmpty (postgredb))
                        postaredb.UpdData (strSql);
                 
                    #endregion

                    #endregion


                }
            }

            results = new Download_Pos_Result ( );
        }

        /// <summary>
        /// 設定回傳訊息
        /// </summary>
        protected override void SetRCRM ( )
        {

        }

        /// <summary>
        /// 設定回傳訊息
        /// </summary>
        protected override void SaveRCRM ( )
        {
            var json = new System.Web.Script.Serialization.JavaScriptSerializer ( );
            funWriteLog (json.Serialize (request), "", "");
        }

    }
}