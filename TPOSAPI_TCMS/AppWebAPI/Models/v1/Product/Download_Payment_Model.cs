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
    public class Download_Payment_Model : _Product_Model<Download_Payment_Request, Download_Payment_Result>
    {
        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Download_Payment_Model ( ) { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Download_Payment_Model ( Download_Payment_Request request ) : base (request) { }

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
            int maxno = 0;
            payment_detail[ ] Lcd = request.payment_detail.ToArray<payment_detail> ( );
            List<payment_detail_result> payment_detail_result = new List<payment_detail_result> ( );

            strSql = string.Format (
                                   @"
                                    SELECT  
                                    *
                                    FROM {0}Payment
                                    order by intPaymentNo desc
                                    "
                                   , DB_Service
                                   );



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
            else if (dt.Rows.Count > 0)
                maxno = int.Parse (dt.Rows[0]["intPaymentNo"].ToString ( )) + 1;

            for (int i = 0; i < Lcd.Length; i++)
            {
                Lcd[i].intpaymentno = maxno.ToString ( );
                DataRow[ ] dra = dt.Select ("strExtSRef1 = '" + Lcd[i].strextsref1 + "'");

                if (dra.Length > 0)
                {
                    strSql = @"update {0}Payment set strBOPaymentName = '{1}',strPOSPaymentName = '{2}',ysnActive = '{3}'
                                where strExtSRef1 = '{4}' ";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].strbopaymentname,
                                            Lcd[i].strpospaymentname,
                                            Lcd[i].ysnactive,
                                            Lcd[i].strextsref1);

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
                    strSql = @"insert into {0}Payment (intPaymentNo, strBOPaymentName, strPOSPaymentName, ysnActive, intShowOrder, strExtSRef1, STRIFEXECUTEFILENAME, ysnAskedCommentAsCreditCard)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', 'T')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].intpaymentno,
                                            Lcd[i].strbopaymentname,
                                            Lcd[i].strpospaymentname,
                                            Lcd[i].ysnactive,
                                            Lcd[i].intshoworder,
                                            Lcd[i].strextsref1,
                                            Lcd[i].strextsref1.Substring (0, 1) == "8" ? @"3rdPayment_AEON\IFPayment.exe OLink Payment" : Lcd[i].strextsref1 == "41" ? @"3rdPayment_AEON\IFPayment.exe CN BankComm" : "");

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


                    strSql = @"insert into {0}PaymentStore (intPaymentNo, strStoreCode)
                                values ('{1}', '{2}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].intpaymentno,
                                            "ALL");

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


                    payment_detail_result pdr = new payment_detail_result ( );
                    pdr.intpaymentno = Lcd[i].intpaymentno;
                    pdr.strbopaymentname = Lcd[i].strbopaymentname;
                    pdr.strextsref1 = Lcd[i].strextsref1;
                    payment_detail_result.Add (pdr);

                    maxno += 1;
                }
            }

            /*更新支付工具參數*/
            strSql = @"update {0}payment set ysnAskComment = 'T' where strExtSRef1 = '42'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set ysnAskedCommentAsCreditCard = 'T' where strExtSRef1 = '42'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set intshoworder ='-1' where strExtSRef1 = '80' or strExtSRef1 = '82'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set ysntaxedbefore ='F' where ysntaxedbefore is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set ysnUseForRefund ='T' where ysnUseForRefund is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNCANCANCEL ='T' where YSNCANCANCEL is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNOPENCASHDRAWER ='T' where YSNOPENCASHDRAWER is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNPRINTASKEDCOMMENT ='F' where YSNOPENCASHDRAWER is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNASKCOMMENT ='F' where YSNASKCOMMENT is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNCANPAYDECIMAL ='T' where YSNCANPAYDECIMAL is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNALLOWOVERPAY ='T' where strExtSRef1 = '10'";
            strSql = @"update {0}payment set YSNUSEOVERPAYCHANGERULE ='F' where YSNUSEOVERPAYCHANGERULE = '0'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNPOINTPAY ='F' where YSNPOINTPAY = '0'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNIFMODE ='T' where strExtSRef1 = '80'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNIFMODE ='T' where strExtSRef1 = '81'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNIFMODE ='T' where strExtSRef1 = '82'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNIFMODE ='T' where strExtSRef1 = '83'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNIFMODE ='T' where strExtSRef1 = '41'";
            strSql = @"update {0}payment set YSNIFMODE ='T' where STRLASTUPDATEPERSON = 'ADMINISTRATOR'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNIFINCLUDEITEMCONTENT = 'T' where YSNIFINCLUDEITEMCONTENT is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNIFPRINTCARDNO = 'T' where YSNIFPRINTCARDNO is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set YSNRETURNMATCHSALE = 'T' where YSNRETURNMATCHSALE is null";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set strIfExeCuteFileName = '3rdPayment_AEON\IFPayment.exe OLink Payment Offline' where strExtSRef1 = '81'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set strIfExeCuteFileName = '3rdPayment_AEON\IFPayment.exe OLink Payment Offline' where strExtSRef1 = '83'";
            strSql = string.Format (strSql, DB_Service);

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

            strSql = @"update {0}payment set ysnIfMode = 'F' where strExtSRef1 = '42'";
            strSql = string.Format (strSql, DB_Service);

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


            results = new Download_Payment_Result ( );
            results.payment_detail_result = payment_detail_result;
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
        }

    }
}