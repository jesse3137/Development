using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using System.Net;

namespace AppWebAPI.Models.v1.Product
{
    /// <summary>
    /// 權限控管-登入
    /// </summary>
    public class Query_SaleData_Up_Model : _Product_Model<SaleData_Up_Request, SaleData_Up_Result>
    {

        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Query_SaleData_Up_Model ( ) { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Query_SaleData_Up_Model ( SaleData_Up_Request request ) : base (request) { }


        /// <summary>
        /// 驗證資料
        /// </summary>
        /// <returns></returns>
        protected override bool Verify_Request ( )
        {
            bool OK = true;    //是否驗證通過

            #region 判斷銷售日期
            if (string.IsNullOrEmpty (request.Sales_Date))
            {
                rcrm = new RCRM (RC_Enum.FAIL_400_0001, "", ":Sales_Date");
                return false;
            }
            #endregion

            #region 判斷POSID
            if (string.IsNullOrEmpty (request.PosId))
            {
                rcrm = new RCRM (RC_Enum.FAIL_400_0001, "", ":PosId");
                return false;
            }
            #endregion

            #region 處理SQL值，將「'」改為「''」
            try
            {
                DateTime.Parse (db.FunSqlValue (request.Sales_Date));
            }
            catch
            {
                rcrm = new RCRM (RC_Enum.FAIL_400_0003, "", ":Sales_Date");
                return false;
            }

            #endregion

            return OK;
        }

        /// <summary>
        /// 取得資料
        /// </summary>

        #region 取得資料
        protected override void GetResults ( )
        {
            //查詢字串
            string str_where = "";
                
            //判斷 M 銷售日期 
            if (!string.IsNullOrEmpty (request.Sales_Date))
            {
                //查詢條件 M  銷售日期
                str_where = string.Format (@" AND m.DTMWHEN = '{0}' ", request.Sales_Date.Replace ("-", "/"));

                //判斷 M 收銀機編號
                if (!string.IsNullOrEmpty (request.PosId))
                    //查詢條件 M  銷售日期 收銀機編號
                    str_where += "AND m.STRTILLCODE = '" + request.PosId + "'";
            }
            //判斷 M 收銀機編號
            else if (!string.IsNullOrEmpty (request.PosId))
                //查詢條件 M  收銀機編號
                str_where = string.Format (@" AND m.STRTILLCODE = '{0}' ", request.PosId);

            int cint = 0;

            #region 主檔 M Table
            strSql = string.Format (@"SELECT * FROM {0}POSTRANSHDR m WHERE 1=1 {1} ", DB_Service, str_where);
            DataTable dtM = postaredb.GetData (strSql);
            #endregion

            if (dtM != null)
            {
                if (dtM.Rows.Count == 0)
                {
                    //rcrm = new RCRM(RC_Enum.FAIL_1);
                    List<Sale_detail_result> rm_detail = new List<Sale_detail_result> ( );

                    results = new SaleData_Up_Result ( );
                    results.Sale_detail_result = rm_detail;
                    return;
                }
                else
                {
                    //if (!(request.sell_invoice == null || request.sell_invoice == "" || request.sell_invoice.Length < 2))
                    //{
                    //    //用主檔資訊去取其他副檔
                    //    request.sell_org_shopid = dt.Rows[0]["SHOP_ID"].ToString ( );
                    //    request.sell_org_posid = dt.Rows[0]["POS_ID"].ToString ( );
                    //}
                }
            }
            else
            {
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                return;
            }

            #region 取P檔 之後對應

            strSql = string.Format (@"SELECT * FROM  {0}POSTRANSPAYMENT p LEFT JOIN {0}POSTRANSHDR m on p.LINTGLOBALTRANSNO=m.LINTGLOBALTRANSNO WHERE 1=1 {1}", DB_Service, str_where);
            DataTable dt_p = postaredb.GetData (strSql);
            if (dt_p == null)
            {
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                return;

            }

            #endregion

            #region 取D檔 之後對應
            strSql = string.Format (@"SELECT * FROM {0} POSTRANSDTL d LEFT JOIN {0}POSTRANSHDR m on d.LINTGLOBALTRANSNO=m.LINTGLOBALTRANSNO WHERE 1=1 {1}", DB_Service, str_where);
            DataTable dt_d = postaredb.GetData (strSql);
            if (dt_d == null)
            {
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                return;

            }
            #endregion

            results = new SaleData_Up_Result ( );
            List<Sale_detail_result> Sale_detail = new List<Sale_detail_result> ( );

            #region 撈 M 檔
            for (int k = 0; k < dtM.Rows.Count; k++)
            {
                cint += 1;
                Sale_detail_result dr_m = new Sale_detail_result ( );
                dr_m.intLintglobaltransno = int.Parse (dtM.Rows[k]["LINTGLOBALTRANSNO"].ToString ( )); dr_m.intIntpostransno = int.Parse (dtM.Rows[k]["INTPOSTRANSNO"].ToString ( ));
                dr_m.dateDtmwhen = DateTime.Parse (dtM.Rows[k]["DTMWHEN"].ToString ( ));
                dr_m.dateDtmfirstitem = DateTime.Parse (dtM.Rows[k]["DTMFIRSTITEM"].ToString ( ));
                dr_m.dateDtmcheckout = DateTime.Parse (dtM.Rows[k]["DTMCHECKOUT"].ToString ( ));
                dr_m.dateDtmtrade = DateTime.Parse (dtM.Rows[k]["DTMTRADE"].ToString ( ));
                dr_m.strStrstorecode = dtM.Rows[k]["STRSTORECODE"].ToString ( );
                dr_m.strStrtillcode = dtM.Rows[k]["STRTILLCODE"].ToString ( );
                dr_m.strStrtranstype = dtM.Rows[k]["STRTRANSTYPE"].ToString ( );
                dr_m.strStrusercode = dtM.Rows[k]["STRUSERCODE"].ToString ( );
                dr_m.intDblqty = int.Parse (dtM.Rows[k]["DBLQTY"].ToString ( ));
                dr_m.intCurfinalamount = int.Parse (dtM.Rows[k]["CURFINALAMOUNT"].ToString ( ));
                dr_m.intCurchange = int.Parse (dtM.Rows[k]["CURCHANGE"].ToString ( ));
                dr_m.intCurpaymenttaxed = int.Parse (dtM.Rows[k]["CURPAYMENTTAXED"].ToString ( ));
                dr_m.intCurpaymentnotax = int.Parse (dtM.Rows[k]["CURPAYMENTNOTAX"].ToString ( ));
                dr_m.strStrcusttaxid = dtM.Rows[k]["STRCUSTTAXID"].ToString ( );
                dr_m.dateDtmoritransdatetime = DateTime.Parse (dtM.Rows[k]["DTMORITRANSDATETIME"].ToString ( ));
                dr_m.strStroricusttaxid = dtM.Rows[k]["STRORICUSTTAXID"].ToString ( );
                dr_m.intLintoriglobaltransno = int.Parse (dtM.Rows[k]["LINTORIGLOBALTRANSNO"].ToString ( ));
                dr_m.intIntrefpostransno = int.Parse (dtM.Rows[k]["INTREFPOSTRANSNO"].ToString ( ));
                dr_m.strStroritranstype = dtM.Rows[k]["STRORITRANSTYPE"].ToString ( );
                dr_m.strTweinv_Ysngenprove = dtM.Rows[k]["TWEINV_YSNGENPROVE"].ToString ( );
                dr_m.strTweinv_Ysnprintprove = dtM.Rows[k]["TWEINV_YSNPRINTPROVE"].ToString ( );
                dr_m.strTweinv_Ysnprinttransdtl = dtM.Rows[k]["TWEINV_YSNPRINTTRANSDTL"].ToString ( );
                dr_m.strTweinv_Strfullinvnum = dtM.Rows[k]["TWEINV_STRFULLINVNUM"].ToString ( );
                dr_m.strTweinv_Strbarcode = dtM.Rows[k]["TWEINV_STRBARCODE"].ToString ( );
                dr_m.strTweinv_Strqrcode1 = dtM.Rows[k]["TWEINV_STRQRCODE1"].ToString ( );
                dr_m.strTweinv_Strqrcode2 = dtM.Rows[k]["TWEINV_STRQRCODE2"].ToString ( );
                dr_m.strTweinv_Strrandom = dtM.Rows[k]["TWEINV_STRRANDOM"].ToString ( );
                dr_m.intTweinv_Curtotamtnotax = int.Parse (dtM.Rows[k]["TWEINV_CURTOTAMTNOTAX"].ToString ( ));
                dr_m.intTweinv_Curtottax = int.Parse (dtM.Rows[k]["TWEINV_CURTOTTAX"].ToString ( ));
                dr_m.intTweinv_Curtotamtinctax = int.Parse (dtM.Rows[k]["TWEINV_CURTOTAMTINCTAX"].ToString ( ));
                dr_m.intTweinv_Intrlno = int.Parse (dtM.Rows[k]["TWEINV_INTRLNO"].ToString ( ));
                dr_m.strTweinv_Ysndonate = dtM.Rows[k]["TWEINV_YSNDONATE"].ToString ( );
                dr_m.strTweinv_Strnpoban = dtM.Rows[k]["TWEINV_STRNPOBAN"].ToString ( );
                dr_m.strTweinv_Ysnusecellphonebarcode = dtM.Rows[k]["TWEINV_YSNUSECELLPHONEBARCODE"].ToString ( );
                dr_m.strTweinv_Strcellphonebarcode = dtM.Rows[k]["TWEINV_STRCELLPHONEBARCODE"].ToString ( );
                dr_m.strTweinv_Ysnusenaturepersonid = dtM.Rows[k]["TWEINV_YSNUSENATUREPERSONID"].ToString ( );
                dr_m.strTweinv_Strnaturepersonid = dtM.Rows[k]["TWEINV_STRNATUREPERSONID"].ToString ( );
                dr_m.intCurdiscount = int.Parse (dtM.Rows[k]["CURDISCOUNT"].ToString ( ));
                dr_m.dateDtmoritrade = DateTime.Parse (dtM.Rows[k]["DTMORITRADE"].ToString ( ));
                dr_m.strStroritillcode = dtM.Rows[k]["STRORITILLCODE"].ToString ( );
                dr_m.intCuroriamount = int.Parse (dtM.Rows[k]["CURORIAMOUNT"].ToString ( ));
                dr_m.intIntreprintcount = int.Parse (dtM.Rows[k]["INTREPRINTCOUNT"].ToString ( ));
                dr_m.strYsnvoid = dtM.Rows[k]["YSNVOID"].ToString ( );
                dr_m.strStrcompcode = dtM.Rows[k]["STRCOMPCODE"].ToString ( );
                dr_m.strYsndiplomat = dtM.Rows[k]["YSNDIPLOMAT"].ToString ( );
                dr_m.strStrdiplomatcode = dt.Rows[k]["STRDIPLOMATCODE"].ToString ( );
                dr_m.intCurfinalamountnotax = int.Parse (dt.Rows[k]["CURFINALAMOUNTNOTAX"].ToString ( ));

                #region 撈 P檔
                DataRow[ ] dra_p = dt_p.Select (" LINTGLOBALTRANSNO = '" + dtM.Rows[k]["LINTGLOBALTRANSNO"].ToString ( ));

                if (dra_p != null)
                {
                    List<Pay_detail_result> Pay_detail = new List<Pay_detail_result> ( );
                    for (int i = 0; i < dra_p.Length; i++)
                    {
                        DataRow dr_p = dra_p[i];
                        Pay_detail_result drt_p = new Pay_detail_result ( );
                        drt_p.intLintglobaltransno = int.Parse (dr_p["LINTGLOBALTRANSNO"].ToString ( ));
                        drt_p.intIntsortno = int.Parse (dr_p["INTSORTNO"].ToString ( ));
                        drt_p.intIntpaymentno = int.Parse (dr_p["INTPAYMENTNO"].ToString ( ));
                        drt_p.strStrpospaymentname = dr_p["STRPOSPAYMENTNAME"].ToString ( );
                        drt_p.intCurvalue = int.Parse (dr_p["CURVALUE"].ToString ( ));
                        drt_p.dateDtmwhen = DateTime.Parse (dr_p["DTMWHEN"].ToString ( ));
                        drt_p.intIntreceiptpageno = int.Parse (dr_p["INTRECEIPTPAGENO"].ToString ( ));
                        drt_p.intCurfullvalue = int.Parse (dr_p["CURFULLVALUE"].ToString ( ));
                        drt_p.strYsntaxedbefore = dr_p["YSNTAXEDBEFORE"].ToString ( );
                        drt_p.strYsntaxedbefore = dr_p["YSNTAXEDBEFORE"].ToString ( );
                        drt_p.strYsncancancel = dr_p["YSNCANCANCEL"].ToString ( );
                        drt_p.strYsnopencashdrawer = dr_p["YSNOPENCASHDRAWER"].ToString ( );
                        drt_p.strYsnprintaskedcomment = dr_p["YSNPRINTASKEDCOMMENT"].ToString ( );
                        drt_p.strYsnpointpay = dr_p["YSNPOINTPAY"].ToString ( );
                        drt_p.strYsnifmode = dr_p["YSNIFMODE"].ToString ( );
                        drt_p.strStrreceipt1 = dr_p["STRRECEIPT1"].ToString ( );
                        drt_p.strStrreceipt2 = dr_p["STRRECEIPT2"].ToString ( );
                        drt_p.strStrreceipt3 = dr_p["STRRECEIPT3"].ToString ( );
                        drt_p.strStrreceipt4 = dr_p["STRRECEIPT4"].ToString ( );
                        drt_p.strStrreceipt5 = dr_p["STRRECEIPT5"].ToString ( );
                        drt_p.strStrremark1 = dr_p["STRREMARK1"].ToString ( );
                        drt_p.strStrremark2 = dr_p["STRREMARK2"].ToString ( );
                        drt_p.strStrremark3 = dr_p["STRREMARK3"].ToString ( );
                        drt_p.strStrremark4 = dr_p["STRREMARK4"].ToString ( );
                        drt_p.strStrremark5 = dr_p["STRREMARK5"].ToString ( );
                        drt_p.strStrifcardno = dr_p["STRIFCARDNO"].ToString ( );
                        drt_p.strYsnifprintcardno = dr_p["YSNIFPRINTCARDNO"].ToString ( );
                        drt_p.strYsnifprintreceipt1 = dr_p["YSNIFPRINTRECEIPT1"].ToString ( );
                        drt_p.strYsnifprintreceipt2 = dr_p["YSNIFPRINTRECEIPT2"].ToString ( );
                        drt_p.strYsnifprintreceipt3 = dr_p["YSNIFPRINTRECEIPT3"].ToString ( );
                        drt_p.strYsnifprintreceipt4 = dr_p["YSNIFPRINTRECEIPT4"].ToString ( );
                        drt_p.strYsnifprintreceipt5 = dr_p["YSNIFPRINTRECEIPT5"].ToString ( );
                        drt_p.strStrcomment = dr_p["STRCOMMENT"].ToString ( );
                        drt_p.strStrauthcode = dr_p["STRAUTHCODE"].ToString ( );
                        drt_p.strStrpaymenttype = dr_p["STRPAYMENTTYPE"].ToString ( );
                        drt_p.strYsnvoid = dr_p["YSNVOID"].ToString ( );
                        drt_p.strStrsourceid = dr_p["STRSOURCEID"].ToString ( );
                        drt_p.intIntcount = int.Parse (dr_p["INTCOUNT"].ToString ( ));
                        Pay_detail.Add (drt_p);
                    }
                    dr_m.Pay_detail = Pay_detail;
                }
                else
                {
                    rcrm = new RCRM (RC_Enum.FAIL_401_0099); return;
                }
                #endregion

                #region 撈 D 檔
                DataRow[ ] dra_d = dt_d.Select (" LINTGLOBALTRANSNO = '" + dtM.Rows[k]["LINTGLOBALTRANSNO"].ToString ( ) + "' ");
                if (dra_d != null)
                {
                    List<Good_detail_result> Good_detail = new List<Good_detail_result> ( );
                    for (int i = 0; i < dra_d.Length; i++)
                    {
                        DataRow dr_d = dra_d[i];
                        Good_detail_result drt_d = new Good_detail_result ( );
                        drt_d.intLintglobaltransno = int.Parse (dr_d["LINTGLOBALTRANSNO"].ToString ( ));
                        drt_d.intIntsortno = int.Parse (dr_d["INTSORTNO"].ToString ( ));
                        drt_d.strStrtype = dr_d["STRTYPE"].ToString ( );
                        drt_d.intIntitemno = int.Parse (dr_d["INTITEMNO"].ToString ( ));
                        drt_d.intStritemnamepos = int.Parse (dr_d["STRITEMNAMEPOS"].ToString ( ));
                        drt_d.intStrmodifier = int.Parse (dr_d["STRMODIFIER"].ToString ( ));
                        drt_d.intCurprice = int.Parse (dr_d["CURPRICE"].ToString ( ));
                        drt_d.intDblqty = int.Parse (dr_d["DBLQTY"].ToString ( ));
                        drt_d.intCuroriamount = int.Parse (dr_d["CURORIAMOUNT"].ToString ( ));
                        drt_d.intCurdiscount = int.Parse (dr_d["CURDISCOUNT"].ToString ( ));
                        drt_d.intCurfinalamount = int.Parse (dr_d["CURFINALAMOUNT"].ToString ( ));
                        drt_d.intCurtax = int.Parse (dr_d["CURTAX"].ToString ( ));
                        drt_d.intIntpriceno = int.Parse (dr_d["INTPRICENO"].ToString ( ));
                        drt_d.intCuroriprice = int.Parse (dr_d["CURORIPRICE"].ToString ( ));
                        drt_d.intDblsaletaxrate = int.Parse (dr_d["DBLSALETAXRATE"].ToString ( ));
                        drt_d.intDbloriqty = int.Parse (dr_d["DBLORIQTY"].ToString ( ));
                        drt_d.strYsnchangedprice = dr_d["YSNCHANGEDPRICE"].ToString ( );
                        drt_d.strYsntrackserialno = dr_d["YSNTRACKSERIALNO"].ToString ( );
                        drt_d.strYsnsetmeal = dr_d["YSNSETMEAL"].ToString ( );
                        drt_d.strYsndiscountable = dr_d["YSNDISCOUNTABLE"].ToString ( );
                        drt_d.strYsnaskcomment = dr_d["YSNASKCOMMENT"].ToString ( );
                        drt_d.strYsnprintcomment = dr_d["YSNPRINTCOMMENT"].ToString ( );
                        drt_d.strYsnchargeservice = dr_d["YSNCHARGESERVICE"].ToString ( );
                        drt_d.strStrclassify1code = dr_d["STRCLASSIFY1CODE"].ToString ( );
                        drt_d.strStrclassify2code = dr_d["STRCLASSIFY2CODE"].ToString ( );
                        drt_d.strStrclassify3code = dr_d["STRCLASSIFY3CODE"].ToString ( );
                        drt_d.strStrclassify4code = dr_d["STRCLASSIFY4CODE"].ToString ( );
                        drt_d.strStritemproperty1code = dr_d["STRITEMPROPERTY1CODE"].ToString ( );
                        drt_d.strStritemproperty2code = dr_d["STRITEMPROPERTY2CODE"].ToString ( );
                        drt_d.strStritemproperty3code = dr_d["STRITEMPROPERTY3CODE"].ToString ( );
                        drt_d.intIntdiscountno_Mi = int.Parse (dr_d["INTDISCOUNTNO_MI"].ToString ( ));
                        drt_d.intIntdiscountno_Ms = int.Parse (dr_d["INTDISCOUNTNO_MS"].ToString ( ));
                        drt_d.intIntdiscountno_Am = int.Parse (dr_d["INTDISCOUNTNO_AM"].ToString ( ));
                        drt_d.intCurunidiscount_Mi = int.Parse (dr_d["CURUNIDISCOUNT_MI"].ToString ( ));
                        drt_d.intCurunidiscount_Ms = int.Parse (dr_d["CURUNIDISCOUNT_MS"].ToString ( ));
                        drt_d.intCurunidiscount_Am = int.Parse (dr_d["CURUNIDISCOUNT_AM"].ToString ( ));
                        drt_d.strStrserialno = dr_d["STRSERIALNO"].ToString ( );
                        drt_d.strStritemcomment = dr_d["STRITEMCOMMENT"].ToString ( );
                        drt_d.intIntoriitemno = int.Parse (dr_d["INTORIITEMNO"].ToString ( ));
                        drt_d.strStroriname = dr_d["STRORINAME"].ToString ( );
                        drt_d.strStrsourceid = dr_d["STRSOURCEID"].ToString ( );
                        drt_d.strStrextsref1 = dr_d["STREXTSREF1"].ToString ( );

                        Good_detail.Add (drt_d);
                    }
                    dr_m.Good_detail = Good_detail;
                }
                else
                {
                    rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                    return;
                }
                #endregion

                Sale_detail.Add (dr_m);
            }

            results.Sale_detail_result = Sale_detail;

            #endregion

        }
        #endregion

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