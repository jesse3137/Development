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
    public class Upload_PosStatus_Model : _Product_Model<Upload_PosStatus_Request, Upload_PosStatus_Result>
    {
        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Upload_PosStatus_Model ( ) { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Upload_PosStatus_Model ( Upload_PosStatus_Request request ) : base ( request ) { }

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
        string oracledb = ConfigurationManager.AppSettings[ "oracledb" ];
        string postgredb = ConfigurationManager.AppSettings[ "postgredb" ];
        #endregion


        /// <summary>
        /// 取得資料
        /// </summary>
        protected override void GetResults ( )
        {
            strSql = string.Format (
                                   @"
                                    SELECT  STRTILLCODE, STRSTATUS
                                    FROM {0}TillStatus
                                    "
                                   , DB_Service
                                   );


            #region 判斷使用DB

            #region oracledb
            if ( !string.IsNullOrEmpty ( oracledb ) )
                dt = db.ClassDB.GetData ( strSql );
            #endregion

            #region  postgredb
            if ( !string.IsNullOrEmpty ( postgredb ) )
                dt = postaredb.GetData ( strSql );
            #endregion

            #endregion


            if ( dt == null )
            {
                results = null;
                rcrm = new RCRM ( RC_Enum.FAIL_401_0099 );
                throw new Exception ( "sql err" );
            }

            results = new Upload_PosStatus_Result ( );
            List<pos_status> pos_status = new List<pos_status> ( );

            for ( int i = 0 ; i < dt.Rows.Count ; i++ )
            {
                pos_status ps = new pos_status ( );
                ps.strtillcode = dt.Rows[ i ][ "STRTILLCODE" ].ToString ( );
                ps.strstatus = dt.Rows[ i ][ "STRSTATUS" ].ToString ( );

                pos_status.Add ( ps );
            }

            results.pos_status = pos_status;
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
            funWriteLog ( json.Serialize ( request ), "", "" );
        }

    }
}