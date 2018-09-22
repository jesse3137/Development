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
    public class Download_Agreement_Model : _Product_Model<Download_Agreement_Request, Download_Agreement_Result>
    {
        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Download_Agreement_Model ( ) { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Download_Agreement_Model ( Download_Agreement_Request request ) : base (request) { }

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
            agreement_detail[ ] Lcd = request.agreement_detail.OrderBy (rd => rd.ysnactive).ToArray<agreement_detail> ( );
            //agreement_detail[] Lcd = request.agreement_detail.ToArray<agreement_detail>();

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
            {

                funWriteLog ("Store數量" + Lcd.Length.ToString ( ), "", "");

                strSql = string.Format (@" SELECT * FROM {0}Store", DB_Service);


                dt = db.ClassDB.GetData (strSql);
            }

            #endregion

            #region postgredb
            if (!string.IsNullOrEmpty (postgredb))
            {
                funWriteLog ("Store數量" + Lcd.Length.ToString ( ), "", "");

                strSql = string.Format (@" SELECT * FROM {0}Store", DB_Service);

                dt = postaredb.GetData (strSql);
            }

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
                DataRow[ ] dra = dt.Select ("strStoreCode = '" + Lcd[i].strstorecode + "'");

                if (dra.Length > 0)
                {

                    strSql = "update {0}Store set strStoreName = '{1}',strStoreType = '{2}',ysnActive = '{3}',strPhone = '{4}', strFax = '{5}', strContactName = '{6}', intPosFastKeyNo = '{8}' where strStoreCode = '{7}' ";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strstorename,
                                            Lcd[i].strstoretype,
                                            Lcd[i].ysnactive,
                                            Lcd[i].strphone,
                                            Lcd[i].strfax,
                                            Lcd[i].strcontactname,
                                            Lcd[i].strstorecode,
                                            "1");

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

                    strSql = string.Format (
                                   @"
                                    SELECT  
                                    *
                                    FROM {0}GroupSubStore
                                    where strSubStoreCode = '{1}'
                                    "
                                   , DB_Service
                                   , Lcd[i].strstorecode
                                   );

                    DataTable dt_group = null;

                    #region 判斷使用DB
                    #region oracledb
                    if (!string.IsNullOrEmpty (oracledb))
                        dt_group = db.ClassDB.GetData (strSql);
                    #endregion

                    #region  postgredb
                    if (!string.IsNullOrEmpty (postgredb))
                        dt_group = postaredb.GetData (strSql);
                    #endregion
                    #endregion


                    if (dt_group != null && dt_group.Rows.Count == 0)
                    {
                        strSql = @"insert into {0}GroupSubStore (strGroupStoreCode, strSubStoreCode)
                                values ('{1}', '{2}') ";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "999999",
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
                    }
                }
                else
                {
                    if (isnew)
                    {
                        strSql = @"insert into {0}Store (strStoreCode, strStoreName,strStoreType,ysnActive,strPhone, strFax, strContactName, strParentStore, strAreaCode, intPosFastKeyNo, strCompCode)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', 'ALL', 'ALL', '{8}', 'AEON')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strstorecode,
                                                Lcd[i].strstorename,
                                                Lcd[i].strstoretype,
                                                Lcd[i].ysnactive,
                                                Lcd[i].strphone,
                                                Lcd[i].strfax,
                                                Lcd[i].strcontactname,
                                                "1");

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
                        strSql = @"insert into {0}Store (strStoreCode, strStoreName,strStoreType,ysnActive,strPhone, strFax, strContactName, strParentStore, strAreaCode, intPosFastKeyNo)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', 'ALL', 'ALL', '{8}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strstorecode,
                                                Lcd[i].strstorename,
                                                Lcd[i].strstoretype,
                                                Lcd[i].ysnactive,
                                                Lcd[i].strphone,
                                                Lcd[i].strfax,
                                                Lcd[i].strcontactname,
                                                "1");

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

                    strSql = @"insert into {0}GroupSubStore (strGroupStoreCode, strSubStoreCode)
                                values ('{1}', '{2}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            "999999",
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
                }
            }

            results = new Download_Agreement_Result ( );
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