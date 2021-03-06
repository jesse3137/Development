﻿using System;
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
                    strSql = "update {0}Till set strTillName = '{1}',strStoreCode = '{2}',ysnActive = '{3}',strTillType = '{4}',intTillProfileNo = '{5}',EINV_YSNENABLE='{7}' ,EINV_YSNTESTMODE='{8}', INTPOSFUNCKEYNO='{9}', , INTTWEINVPROFILENO='{10}',  EINV_STRACCOUNTID='{11}', EINV_STRPOSID='{12}', EINV_STRACCESSTOKEN='{13}', EINV_STRSHOPID='{14}', EINV_INTTAKEROLLCNT='{15}', EINV_STRHQCHECKFAILACTION='{16}', STRCOMPCODE='{17}', INTPOSFASTKEYNO='{18}' where strTillCode = '{6}' ";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strtillname,
                                            Lcd[i].strstorecode,
                                            Lcd[i].ysnactive,
                                            Lcd[i].strtilltype,
                                            Lcd[i].intTillprofileno, Lcd[i].strtillcode, Lcd[i].strEinv_ysnenable, 
                                            Lcd[i].strEinv_ysntestmode, Lcd[i].intPosfunckeyno, Lcd[i].intTweinvprofileno, 
                                            Lcd[i].strEinv_straccountid, Lcd[i].strEinv_strposid, 
                                            Lcd[i].strEinv_straccesstoken, Lcd[i].strEinv_strshopid, 
                                            Lcd[i].intEinv_inttakerollcnt, Lcd[i].strEINV_STRHQCHECKFAILACTION, 
                                            Lcd[i].strCompcode, Lcd[i].intPosfastkeyno);

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
                        strSql = @"insert into {0}Till (strTillCode,  strTillName ,strStoreCode ,ysnActive ,strTillType ,intTillProfileNo  ,EINV_YSNENABLE ,EINV_YSNTESTMODE, INTPOSFUNCKEYNO, INTTWEINVPROFILENO, EINV_STRACCOUNTID, EINV_STRPOSID, EINV_STRACCESSTOKEN, EINV_STRSHOPID, EINV_INTTAKEROLLCNT, EINV_STRHQCHECKFAILACTION, STRCOMPCODE, INTPOSFASTKEYNO
 )
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}' )";
                        strSql = string.Format (strSql,
                                                DB_Service, Lcd[i].strtillcode,
                                                Lcd[i].strtillname,
                                                Lcd[i].strstorecode,
                                                Lcd[i].ysnactive,
                                                Lcd[i].strtilltype,
                                                Lcd[i].intTillprofileno,
                                                Lcd[i].strEinv_ysnenable, Lcd[i].strEinv_ysntestmode, Lcd[i].intPosfunckeyno, Lcd[i].intTweinvprofileno, Lcd[i].strEinv_straccountid, Lcd[i].strEinv_strposid, Lcd[i].strEinv_straccesstoken, Lcd[i].strEinv_strshopid, Lcd[i].intEinv_inttakerollcnt, Lcd[i].strEINV_STRHQCHECKFAILACTION, Lcd[i].strCompcode, Lcd[i].intPosfastkeyno);

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
                                values ('{1}', '{2}', '{3}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strtillcode, Lcd[i].strStatus,
                                                Lcd[i].strCompcode);

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
                        strSql = @"insert into {0}Till strTillCode,  strTillName ,strStoreCode ,ysnActive ,strTillType ,intTillProfileNo  ,EINV_YSNENABLE ,EINV_YSNTESTMODE, INTPOSFUNCKEYNO, INTTWEINVPROFILENO, EINV_STRACCOUNTID, EINV_STRPOSID, EINV_STRACCESSTOKEN, EINV_STRSHOPID, EINV_INTTAKEROLLCNT, EINV_STRHQCHECKFAILACTION, STRCOMPCODE, INTPOSFASTKEYNO
 )
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strtillcode,
                                                Lcd[i].strtillname,
                                                Lcd[i].strstorecode,
                                                Lcd[i].ysnactive,
                                                Lcd[i].strtilltype,
                                                Lcd[i].intTillprofileno,
                                                Lcd[i].strEinv_ysnenable, Lcd[i].strEinv_ysntestmode, Lcd[i].intPosfunckeyno, Lcd[i].intTweinvprofileno, Lcd[i].strEinv_straccountid, Lcd[i].strEinv_strposid, Lcd[i].strEinv_straccesstoken, Lcd[i].strEinv_strshopid, Lcd[i].intEinv_inttakerollcnt, Lcd[i].strEINV_STRHQCHECKFAILACTION, Lcd[i].strCompcode, Lcd[i].intPosfastkeyno);

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
                                values ('{1}', '{2}'. '{3}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strtillcode,
                                                Lcd[i].strStatus, Lcd[i].strCompcode);

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
                                values ('{1}', '{2}', '{3}', '{4}', {5}, '{6}', '{7}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strtillcode,
                                            "CashDrawer",
                                            "CashDrawer",
                                            "OPOS",
                                             "null",
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