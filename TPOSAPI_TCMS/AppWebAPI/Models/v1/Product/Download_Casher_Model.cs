using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Configuration;

namespace AppWebAPI.Models.v1.Product
{
    /// <summary>
    /// 權限控管-登入
    /// </summary>
    public class Download_Casher_Model : _Product_Model<Download_Casher_Request, Download_Casher_Result>
    {
        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Download_Casher_Model ( ) { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Download_Casher_Model ( Download_Casher_Request request ) : base (request) { }

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
            casher_data[ ] Lcd = request.casher_data.OrderBy (rd => rd.ysnactive).ToArray<casher_data> ( );

            funWriteLog ("Casher數量" + Lcd.Length.ToString ( ), "", "");

            strSql = string.Format (@" SELECT *  FROM {0}Users ", DB_Service);

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
                DataRow[ ] dra = dt.Select ("strusercode = '" + Lcd[i].strusercode + "'");

                if (dra.Length > 0)
                {
                    strSql = "update {0}Users set intPOSUserNo = '{1}',intPOSPassword = '{2}',ysnActive = '{3}',strStoreCode = '{4}',strAreaCode = '{5}',strEXTSRef1 = '{6}', strCompcode='{8}' where strusercode = '{7}' ";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].intposuserno.ToString ( ),
                                            Lcd[i].intpospassword.ToString ( ),
                                            Lcd[i].ysnactive,
                                            Lcd[i].strstorecode,
                                            Lcd[i].strareacode,
                                            "ALL",
                                            Lcd[i].strusercode, Lcd[i].Strcompcode);


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

                    strSql = "update {0}UserPosStore set strStoreCode = '{1}'where strusercode = '{2}' ";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strstorecode,
                                            Lcd[i].strusercode);


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

                    strSql = "update {0}USERROLE set strRoleCode = '{1}'where strUserCode = '{2}' ";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strrolecode == "1" ? "FRONT_CASHIER" : "FINANCE_ADMIN",
                                            Lcd[i].strusercode);


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
                        strSql = @"insert into {0}Users (strUserCode, intPOSUserNo, intPOSPassword, ysnActive, strStoreCode, strAreaCode, strEXTSRef1, strCompCode)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strusercode,
                                                Lcd[i].intposuserno.ToString ( ),
                                                Lcd[i].intpospassword.ToString ( ),
                                                Lcd[i].ysnactive,
                                                Lcd[i].strstorecode,
                                                Lcd[i].strareacode,
                                                " ",
                                                Lcd[i].Strcompcode);


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
                        strSql = @"insert into {0}Users (strUserCode, intPOSUserNo, intPOSPassword, ysnActive, strStoreCode, strAreaCode, strEXTSRef1)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strusercode,
                                                Lcd[i].intposuserno.ToString ( ),
                                                Lcd[i].intpospassword.ToString ( ),
                                                Lcd[i].ysnactive,
                                                Lcd[i].strstorecode,
                                                Lcd[i].strareacode,
                                                "");


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

                    strSql = @"insert into {0}UserPosStore (strUserCode, strStoreCode)
                                values ('{1}', '{2}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strusercode,
                                            Lcd[i].strstorecode);


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

                    strSql = @"insert into {0}USERROLE (strUserCode, strRoleCode)
                                values ('{1}', '{2}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strusercode, 
                                            Lcd[i].strrolecode == "1" ? "FRONT_CASHIER" : "FINANCE_ADMIN");


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
            results = new Download_Casher_Result ( );
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