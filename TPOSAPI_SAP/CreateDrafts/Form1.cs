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
using System.Configuration;
using System.Xml.Serialization;
using System.Xml;
using WinSCP;
using BaseSet;
using System.Web;


namespace CreateARInvoice
{
    public partial class Form1 : Form
    {
        #region Class
        class rcrm
        {
            public string rc { get; set; }

            public string rm { get; set; }
        }
        class apiresult
        {
            public rcrm rcrm { get; set; }
        }
        /*
     {"CardCode":"V0003","U_PosDocNum":"201809100001","BPLId":"A2","DocDate":"2018/09/10","DocDueDate":"2018/09/30",
      * "TaxDate":"2018/09/11","U_ApplicationCode":"A2201809100001","U_PuInvNum":"AB12345678","U_PuInvDate":"2018/09/11",
      * "U_ApplicationDesc":"活動禮物","Comments":"APIs Create",
      * "Lines":[{"ItemCode":"0000001","Quantity":"1","VatGroup":"J5","Price":"100","PriceAfVAT":"","WhsCode :"A202",
      * "StoreCode":"A2A","DepartCode":"A202"},{"ItemCode":"Lav0001","Quantity":"2","VatGroup":"J5",
      * "Pric e":"","PriceAfVAT":"1050","WhsCode":"A201","StoreCode":"A2A","DepartCode":"A201"}]} 
     */
        /// <summary>
        /// 建立A/R發票
        /// </summary>
        class CreateARInvoice
        {
            /// <summary>
            /// 客戶代碼 由SAP提供固定值
            /// </summary>
            public string CardCode;
            /// <summary>
            /// 分公司代碼(第一層) (exe心之芳庭D1) 對應TaxIdNum(找到BPLId)
            /// </summary>
            public int BPLId;
            /// <summary>
            /// POS單據號碼(全事業體唯一碼) 兩碼英文+店號4碼+西元年月日8碼+流水編3碼
            /// 分店+系統日+一代碼(代表正向發票)
            /// </summary>
            public string U_PosDocNum;
            /// <summary>
            /// 過帳日期，交易發生的日期
            /// 系統日
            /// </summary>
            public string DocDate;
            /// <summary>
            /// 到期日，預計向客戶收款的日期
            /// 系統日
            /// </summary>
            public string DocDueDate;
            /// <summary>
            /// 文件日期，新增本文件的日期
            /// 系統日
            /// </summary>
            public string TaxDate;
            /// <summary>
            /// 備註
            /// 可不傳此欄位
            /// </summary>
            public string Comments;
            /// <summary>
            /// Lines A/R發票明細(資料如下)
            /// </summary>
            /// <returns></returns>
            public List<Lines> Lines;

        }

        /// <summary>
        /// Lines A/R發票明細(資料如下)
        /// </summary>
        public class Lines
        {
            /// <summary>
            /// 商品代碼
            /// GOODS_ID 店內碼-進銷碼
            /// </summary>
            public string ItemCode;
            /// <summary>
            /// 數量(庫存單位)
            /// QTY 銷售數量
            /// </summary>
            public int Quantity;
            /// <summary>
            /// 稅碼
            /// M檔TAX 稅別(DEFAULT 1)(參數:RS_TAX) 需與SAP對照傳SAP代碼
            /// </summary>
            public string VatGroup;
            /// <summary>
            /// 未稅單價
            /// (與PriceAfVAT兩者選一拋)
            /// 可不傳此欄位
            /// </summary>
            public int Price;
            /// <summary>
            /// 含稅單價
            /// (與Price兩者選一拋)
            /// 可不傳此欄位
            /// </summary>
            public int PriceAfVAT;
            /// <summary>
            /// 列總計
            /// SUM(SALES_AMT) 實售價總計
            /// </summary>
            public decimal LineTotal;
            /// <summary>
            /// 倉庫代碼(建議和第三層部門別代碼一致)
            /// SHOP_ID
            /// </summary>
            public string WhsCode;
            /// <summary>
            /// 店別(第二層) (exe:約會區)
            /// </summary>
            public string StoreCode;
            /// <summary>
            /// 部門別(第三層) (exe:約會區吧台(親親下午))
            /// SHOP_ID 分店
            /// </summary>
            public string DepartCode;
        }

        /// <summary>
        /// Response 錯誤資訊
        /// </summary>
        private class Response
        {
            /// <summary>
            /// A/P發票號碼
            /// </summary>
            public string DocEntry;
            /// <summary>
            /// 錯誤明細
            /// </summary>
            public List<Error> Error;


        }
        /// <summary>
        /// 錯誤明細
        /// </summary>
        class Error
        {  /// <summary>
            /// 錯誤代碼
            /// </summary>
            public string Code;
            /// <summary>
            /// 錯誤訊息
            /// </summary>
            public string Description;
        }
        #endregion

        public Form1 ( )
        {
            InitializeComponent ( );
        }
        private void Form1_Load ( object sender, EventArgs e )
        {
            //DoInv();
            //先銷貨
            DoRM ( );
            //後退貨
            DoRM_B ( );
            //清機
            DoClear ( );

            this.Close ( );
        }

        /// <summary>
        /// 
        /// </summary>
        private class sales_request
        {
            public string FUNC_ID;
            public string ACNT_NO;
            public string SYS_DATE;
            public string FUNC_SIGNATURE;
            public FUNC_DATA_ADD FUNC_DATA;
        }

        /// <summary>
        /// 
        /// </summary>
        private class TXN_DTL_INFO
        {
            public int TXN_Item;
            public string TXN_GUINo;
            public string PLU_No;
            public string PLU_MagNo;
            public string PLU_Name;
            public int TXN_Qty;
            public decimal PLU_SalePrc;
            public decimal TXN_Disc;
            public decimal TXN_SaleAmt;
            public decimal TXN_SaleTax;
            public decimal TXN_SaleNoTax;
            public decimal TXN_Net;
            public decimal TXN_Tax;
            public string PLU_TaxType;
            public decimal TXN_AmusementTax;
            public string PLU_Type;
            public decimal TXN_ExtraAmt;
            public decimal TXN_LuxuryTax;
        }

        /// <summary>
        /// 
        /// </summary>
        private class TXN_PAY_INFO
        {
            public int TXN_Item;
            public string PAY_No;
            public string TXN_PayDesc;
            public string PAY_TaxType;
            public decimal TXN_PayAmt;
            public decimal TXN_OverAmt;
            public string PAY_Name;
            public string PAY_Type;
        }

        /// <summary>
        /// 
        /// </summary>
        private class sales_b_request
        {
            public string FUNC_ID;
            public string ACNT_NO;
            public string SYS_DATE;
            public string FUNC_SIGNATURE;
            public FUNC_DATA_BACK FUNC_DATA;
        }

        /// <summary>
        /// 
        /// </summary>
        private class FUNC_DATA_BACK
        {
            public string STO_No;
            public string TXN_Date;
            public string ECR_No;
            public string TXN_No;
            public string TXN_GUIBegNo;
            public string TXN_Status;
            public string TXN_VoidStoreNo;
            public string TXN_VoidDT;
            public string TXN_VoidEcrNo;
            public string TXN_VoidTxnNo;
            public string TXN_VoidUsrNo;
            public string TXN_VoidTime;
            public string TXN_VoidShiftNo;
            public string TXN_DATA_HASH;
        }

        /// <summary>
        /// 
        /// </summary>
        private class clear_request
        {
            public string FUNC_ID;
            public string ACNT_NO;
            public string SYS_DATE;
            public string FUNC_SIGNATURE;
            public FUNC_DATA_CLEAR FUNC_DATA;
        }

        /// <summary>
        /// 
        /// </summary>
        private class FUNC_DATA_CLEAR
        {
            public string STO_No;
            public string TXN_Date;
            public string ECR_No;
            public string USR_No;
            public string TXN_DATA_HASH;
            public string SLE_TxnBegNo;
            public string SLE_TxnEndNo;
            public string SLE_GUIBegNo;
            public string SLE_GUIEndNo;
            public int SLE_SaleCustCnt;
            public int SLE_SaleQty;
            public string SLE_SaleAmt;
            public string SLE_SaleTax;
            public string SLE_SaleNoTax;
            public string SLE_SaleZeroTax;
            public string SLE_RjtCustCnt;
            public string SLE_RjtQty;
            public string SLE_RjtAmt;
            public string SLE_RjtTax;
            public string SLE_RjtNoTax;
            public string SLE_RjtZeroTax;
            public string SLE_VoidCustCnt;
            public string SLE_VoidQty;
            public string SLE_VoidAmt;
            public string SLE_VoidTax;
            public string SLE_VoidNoTax;
            public string SLE_VoidZeroTax;
            public string SLE_DiscSCnt;
            public string SLE_DiscSAmt;
            public string SLE_CancelCnt;
            public string SLE_DiscCnt1;
            public string SLE_DiscAmt1;
            public string SLE_PayCnt1;
            public string SLE_PayCnt14;
            public string SLE_PayCnt27;
            public string SLE_PayAmt1;
            public string SLE_PayAmt14;
            public string SLE_PayAmt27;
            public string SLE_GUICnt;
            public string SLE_VoidGUICnt;
        }

        /// <summary>
        /// 
        /// </summary>
        private class sales_result
        {
            public string FUNC_ID;
            public string ACNT_NO;
            public string SYS_DATE;
            public RSPN_DATA RSPN_DATA;
            public string RSPN_CODE;
            public string RSPN_MSG;
        }

        /// <summary>
        /// 
        /// </summary>
        private class FUNC_DATA_ADD
        {
            public string STO_No;
            public string TXN_Date;
            public string ECR_No;
            public string TXN_No;
            public string TXN_Time;
            public string USR_No;
            public string TXN_Uniform;
            public string TXN_GUIBegNo;
            public string TXN_GUIEndNo;
            public int TXN_GUICnt;
            public int TXN_TotQty;
            public decimal TXN_TotDisc;
            public decimal TXN_TotSaleAmt;
            public decimal TXN_TotSaleTax;
            public decimal TXN_TotSaleNoTax;
            public decimal TXN_TotNet;
            public decimal TXN_TotTax;
            public decimal TXN_TotGUI;
            public decimal TXN_TotOver;
            public int TXN_DtlCnt;
            public int TXN_PayCnt;
            public int TXN_CustCnt;
            public string TXN_Status;
            public string TXN_ShiftNo;
            public decimal TXN_TotPayAmt;
            public string TXN_TaxType;
            public string TXN_GUIRemark;
            public string TXN_GUILoveCode;
            public string TXN_GUI_CarrType;
            public string TXN_GUI_CarrIdEx;
            public string TXN_GUI_CarrId;
            public string TXN_DATA_HASH;
            public decimal TXN_TotAmusementTax;
            public string TXN_VIP;
            public string TXN_SaleNoteNo;
            public decimal TXN_TotExtraAmt;
            public decimal TXN_TotHaveTax;
            public string STO_Uniform;
            public decimal TXN_TotLuxuryTax;
            public string TXN_MemCard;
            public List<TXN_DTL_INFO> TXN_DTL_INFO;
            public List<TXN_PAY_INFO> TXN_PAY_INFO;
        }

        /// <summary>
        /// 
        /// </summary>
        private class RSPN_DATA
        {
            public string STO_No;
            public string TXN_Date;
            public string ECR_No;
            public string TXN_No;
            public string TXN_GUINO;
            public int TXN_PrintTimes;
            public string TXN_GUIRemark;
        }


        /// <summary>
        /// 先銷貨
        /// </summary>
        private void DoRM ( )
        {
            BaseSet.BaseFunc bf = new BaseSet.BaseFunc ( );

            string apiurl = ConfigurationManager.AppSettings["apiurl"];//TODO 待修改
            string dbname = ConfigurationManager.AppSettings["dbname"];//TODO 待修改
            string dbstr = ConfigurationManager.AppSettings["dbstr"];//TODO 待修改
            string strposid = ConfigurationManager.AppSettings["posid"];//TODO 待修改
            string uniform = ConfigurationManager.AppSettings["uniform"];//TODO 待修改
            string ACNT_NO = ConfigurationManager.AppSettings["ACNT_NO"];//TODO 待修改
            string FUNC_SIGNATURE = ConfigurationManager.AppSettings["FUNC_SIGNATURE"];//TODO 待修改

            string wherepos = " 1 = 1";
            if (strposid != "")
                wherepos += " and m.pos_id = '" + strposid + "'";

            //只取銷貨
            wherepos += " and ifnull(m.UPD_FLAG,'') = '' and TRANS_TYPE = '01'";

            BaseSet.ClassDB_MariaDB mardb = new BaseSet.ClassDB_MariaDB (dbstr);

            //匯出
            bf.funWriteUseLog ("開始匯出銷售");

            //POS
            string sqlstr = @"select * from " + dbname + ".rm_pos ";

            DataTable dtPOS = mardb.GetData (sqlstr);

            //先把這次要轉的UPD_FLAG改為T  TODO待修
            //  sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = 'T' where " + wherepos;

            // if (!(mardb.UpdData (sqlstr)))
            //     return;

            //加條件取有改T的資料以便檢查
            // wherepos = " ifnull(m.UPD_FLAG,'') = 'T' and TRANS_TYPE = '01'";
            wherepos = "TRANS_TYPE = '01'";

            #region M
            sqlstr = @"select m.* from " + dbname + ".rm_prod_sales_m m " +
                             "  left join " + dbname + ".ls_agreement a on a.STORE_ID = m.STORE_ID " +
                " where " + wherepos +
                " order by sales_date, pos_id, trans_no ";

            DataTable dtm = mardb.GetData (sqlstr);
            #endregion

            #region D
            sqlstr = @"select distinct ifnull(ld.ND_TYPE,0) ND,case when ifnull(sp.ITEM_CODE2,'') = '' then (select ITEM_CODE2 from " + dbname + ".rm_pos_speed_key sp2 where sp2.ND_TYPE = ifnull(ld.ND_TYPE,0) and sp2.STORE_ID = m.STORE_ID and sp2.pos_id = m.pos_id and ifnull(sp2.ITEM_CODE2,'') != '' limit 1) else sp.ITEM_CODE2 end ITEM_CODE2,d.* from " + dbname + ".rm_prod_sales_d d " +
                      "     left join " + dbname + ".rm_prod_sales_m m on d.POS_ID = m.POS_ID and  d.sales_date = m.sales_date and d.trans_no = m.trans_no " +
                      "     left join " + dbname + ".rm_pos_speed_key sp on sp.ITEM_CODE = d.GOODS_ID and sp.STORE_ID = m.STORE_ID and sp.pos_id = m.pos_id " +
                      "     left join " + dbname + ".ls_discount ld on ld.ND_TYPE = ifnull(sp.ND_TYPE,d.GOODS_ID) and ld.STORE_ID = m.STORE_ID " +
                " where " + wherepos;

            DataTable dtd = mardb.GetData (sqlstr);
            #endregion

            #region P
            sqlstr = @"select p.*,el.pos_tender from " + dbname + ".rm_pay_type p " +
                      "     left join " + dbname + ".rm_prod_sales_m m on p.POS_ID = m.POS_ID and  p.sales_date = m.sales_date and p.trans_no = m.trans_no " +
                      "     left join " + dbname + ".rm_event_list el on p.TENDER = el.PAY_TYPE_ID " +
                      " where " + wherepos;

            DataTable dtp = mardb.GetData (sqlstr);
            #endregion

            //EVENT_LIST(未開發票)
            sqlstr = @"select * from " + dbname + ".rm_event_list p ";

            DataTable dte = mardb.GetData (sqlstr);
            string str_e = "";
            for (int e = 0; e < dte.Rows.Count; e++)
            {
                if (dte.Rows[e]["PAY_TYPE_INVOICE_FLAG"].ToString ( ) == "N")
                    str_e += dte.Rows[e]["pay_type_id"].ToString ( ) + ",";
            }

            //商品資料

            //EVENT_LIST(未開發票)
            sqlstr = @"select * from " + dbname + ".rs_goods_m m ";

            DataTable dtg = mardb.GetData (sqlstr);

            //開始逐筆轉出
            #region 開始逐筆轉出
            for (int pi = 0; pi < dtPOS.Rows.Count; pi++)
            {
                //M檔
                var querym = from row in dtm.AsEnumerable ( )
                             where row.Field<string> ("pos_id") == dtPOS.Rows[pi]["pos_id"].ToString ( )
                             select row;

                var L_m = querym.ToList ( );

                //D檔
                var queryd = from row in dtd.AsEnumerable ( )
                             where row.Field<string> ("pos_id") == dtPOS.Rows[pi]["pos_id"].ToString ( )
                             select row;


                //P檔
                var queryp = from row in dtp.AsEnumerable ( )
                             where row.Field<string> ("pos_id") == dtPOS.Rows[pi]["pos_id"].ToString ( )
                             select row;
                /*
      {"CardCode":"V0003","U_PosDocNum":"201809100001","BPLId":"A2","DocDate":"2018/09/10","DocDueDate":"2018/09/30",
       * "TaxDate":"2018/09/11","U_ApplicationCode":"A2201809100001","U_PuInvNum":"AB12345678","U_PuInvDate":"2018/09/11",
       * "U_ApplicationDesc":"活動禮物","Comments":"APIs Create",
       * "Lines":[{"ItemCode":"0000001","Quantity":"1","VatGroup":"J5","Price":"100","PriceAfVAT":"","WhsCode :"A202",
       * "StoreCode":"A2A","DepartCode":"A202"},{"ItemCode":"Lav0001","Quantity":"2","VatGroup":"J5",
       * "Pric e":"","PriceAfVAT":"1050","WhsCode":"A201","StoreCode":"A2A","DepartCode":"A201"}]} 
      */
                if (L_m.Count > 0)
                {
                    for (int i = 1; i < L_m.Count; i++)
                    {
                        DataRow dr_m = L_m[i];
                        CreateARInvoice reuqest = new CreateARInvoice ( );
                        reuqest.U_PosDocNum = dr_m["SHOP_ID"].ToString ( ) + DateTime.Now.Date.ToString ("yyyyMMdd") + "1";
                        reuqest.DocDate = DateTime.Now.Date.ToString ("yyyy/MM/dd");
                        reuqest.DocDueDate = DateTime.Now.Date.ToString ("yyyy/MM/dd");
                        reuqest.TaxDate = DateTime.Now.Date.ToString ("yyyy/MM/dd");


                        //D檔
                        var queryd2 = from row in queryd
                                      where row.Field<string> ("pos_id") == dr_m["pos_id"].ToString ( )
                                          && DateTime.Parse (row["sales_date"].ToString ( )).ToString ("yyyy/MM/dd") == DateTime.Parse (dr_m["sales_date"].ToString ( )).ToString ("yyyy/MM/dd")
                                          && row.Field<string> ("trans_no") == dr_m["trans_no"].ToString ( )
                                      select row;



                        var L_d = queryd2.ToList ( );

                        //P檔
                        var queryp2 = from row in queryp
                                      where row.Field<string> ("pos_id") == dr_m["pos_id"].ToString ( )
                                          && DateTime.Parse (row["sales_date"].ToString ( )).ToString ("yyyy/MM/dd") == DateTime.Parse (dr_m["sales_date"].ToString ( )).ToString ("yyyy/MM/dd")
                                          && row.Field<string> ("trans_no") == dr_m["trans_no"].ToString ( )
                                      select row;

                        var L_p = queryp2.ToList ( );

                        List<Lines> Lines = new List<Lines> ( );
                        //D                              
                        for (int d = 0; d < L_d.Count ( ); d++)
                        {
                            DataRow drd = L_d[d];
                            Lines dr_lines = new Lines ( );
                            dr_lines.ItemCode = drd["GOODS_ID"].ToString ( );
                            dr_lines.Quantity = int.Parse (drd["QTY"].ToString ( ));
                            //D檔對應的RS_GOODS_M
                            var querydm = from row in dtg.AsEnumerable ( )
                                          where row.Field<string> ("GOODS_ID") == drd["GOODS_ID"].ToString ( )
                                          select row;
                            var L_dm = querydm.ToList ( );
                            for (int dm = 0; dm < L_dm.Count ( ); dm++)
                            {
                                DataRow drm = L_dm[dm];
                              
                                dr_lines.VatGroup = drm["TAX"].ToString ( ) == "1" ? "X5" : "";//TODO待修
                            }
                            if(drd["SALES_AMT"].ToString ( )!="")
                            dr_lines.LineTotal =decimal.Parse(drd["SALES_AMT"].ToString ( ));//TODO待修 
                            dr_lines.WhsCode = dr_m["SHOP_ID"].ToString ( );
                            dr_lines.DepartCode = dr_m["SHOP_ID"].ToString ( );
                            Lines.Add (dr_lines);
                        }

                        reuqest.Lines = Lines;


                        //轉成字串傳送API
                        string str_rmrequest = Newtonsoft.Json.JsonConvert.SerializeObject (reuqest);
                        string strReturn = GoOtherAPI (str_rmrequest, apiurl);

                        try
                        {
                            apiresult apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);
                            if (apiresult.rcrm.rc != "1")
                            {
                                //Response response = new Response ( );
                                //List<Error> listError = new List<Error> ( );
                                //Error error = new Error ( );
                                //error.Code = apiresult.rcrm.rc;
                                //error.Description = apiresult.rcrm.rm;
                                //listError.Add (error);
                                //response.Error = listError;
                                strReturn = Newtonsoft.Json.JsonConvert.SerializeObject (apiresult.rcrm.rm);
                                //}
                                //else
                                //{
                                //失敗改狀態
                                sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = '' where SALES_DATE = '" + DateTime.Parse (dr_m["SALES_DATE"].ToString ( )).ToString ("yyyy/MM/dd") + "' and POS_ID = '" + dr_m["POS_ID"].ToString ( ) + "' and trans_no = '" + dr_m["TRANS_NO"].ToString ( ) + "'";

                                mardb.UpdData (sqlstr);
                                bf.strPathUseLog = @"bin\CreateDrafts";
                                bf.funWriteLog (strReturn);
                            }
                        }
                        catch
                        {
                            //失敗改狀態
                            sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = '' where SALES_DATE = '" + DateTime.Parse (dr_m["SALES_DATE"].ToString ( )).ToString ("yyyy/MM/dd") + "' and POS_ID = '" + dr_m["POS_ID"].ToString ( ) + "' and trans_no = '" + dr_m["TRANS_NO"].ToString ( ) + "'";

                            mardb.UpdData (sqlstr);
                            bf.strPathUseLog = @"bin\CreateDrafts";
                            bf.funWriteLog (strReturn);
                        }
                    }


                }
            }
            #endregion

            wherepos = " (POS_ID,TRANS_NO,SALES_DATE) in (select m.POS_ID,m.TRANS_NO,m.SALES_DATE from " + dbname + ".rm_prod_sales_m m where ifnull(m.upd_flag,'') = 'T') and TRANS_TYPE = '01'";

            //把這次要轉的UPD_FLAG T改為Y
            //D
            //sqlstr = @"update " + dbname + ".rm_prod_sales_d set UPD_FLAG = 'Y' where ifnull(UPD_FLAG,'') = '' and " + wherepos;

            //mardb.UpdData(sqlstr);
            //P
            //sqlstr = @"update " + dbname + ".rm_pay_type set status = 'Y' where ifnull(status,'') = '' and " + wherepos;

            //mardb.UpdData(sqlstr);

            sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = 'Y' where ifnull(m.upd_flag,'') = 'T' and TRANS_TYPE = '01'";

            mardb.UpdData (sqlstr);
        }

        /// <summary>
        /// 後退貨
        /// </summary>
        private void DoRM_B ( )
        {
            string apiurl = ConfigurationManager.AppSettings["apiurl"];
            BaseSet.BaseFunc bf = new BaseSet.BaseFunc ( );

            string dbname = ConfigurationManager.AppSettings["dbname"];
            string dbstr = ConfigurationManager.AppSettings["dbstr"];
            string strposid = ConfigurationManager.AppSettings["posid"];
            string uniform = ConfigurationManager.AppSettings["uniform"];
            string ACNT_NO = ConfigurationManager.AppSettings["ACNT_NO"];
            string FUNC_SIGNATURE = ConfigurationManager.AppSettings["FUNC_SIGNATURE"];

            string wherepos = " 1 = 1";
            if (strposid != "")
                wherepos += " and m.pos_id = '" + strposid + "'";

            //不取銷貨
            wherepos += " and ifnull(m.UPD_FLAG,'') = '' and TRANS_TYPE != '01'";

            BaseSet.ClassDB_MariaDB mardb = new BaseSet.ClassDB_MariaDB (dbstr);

            /////匯出
            bf.funWriteUseLog ("開始匯出退貨作廢");

            //POS
            string sqlstr = @"select * from " + dbname + ".rm_pos ";

            DataTable dtPOS = mardb.GetData (sqlstr);

            //先把這次要轉的UPD_FLAG改為T
            sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = 'T' where " + wherepos;

            if (!(mardb.UpdData (sqlstr)))
                return;

            //加條件取有改T的資料以便檢查
            wherepos = " ifnull(m.UPD_FLAG,'') = 'T' and TRANS_TYPE != '01'";

            //M
            sqlstr = @"select m.* from " + dbname + ".rm_prod_sales_m m " +
                             "  left join " + dbname + ".ls_agreement a on a.STORE_ID = m.STORE_ID " +
                " where " + wherepos +
                " order by sales_date, pos_id, trans_no ";

            DataTable dtm = mardb.GetData (sqlstr);

            for (int pi = 0; pi < dtPOS.Rows.Count; pi++)
            {
                //M檔
                var querym = from row in dtm.AsEnumerable ( )
                             where row.Field<string> ("pos_id") == dtPOS.Rows[pi]["pos_id"].ToString ( )
                             select row;

                var L_m = querym.ToList ( );

                if (L_m.Count > 0)
                {
                    for (int i = 0; i < L_m.Count; i++)
                    {
                        sales_b_request sales_b_request = new sales_b_request ( );

                        sales_b_request.FUNC_ID = "KGINV_VOID_DATA";
                        sales_b_request.ACNT_NO = ACNT_NO;
                        sales_b_request.SYS_DATE = DateTime.Now.ToString ("yyyyMMddHHmmss");
                        sales_b_request.FUNC_SIGNATURE = FUNC_SIGNATURE;

                        DataRow drm = L_m[i];

                        FUNC_DATA_BACK FUNC_DATA = new FUNC_DATA_BACK ( );
                        FUNC_DATA.STO_No = drm["SHOP_ID"].ToString ( );
                        FUNC_DATA.TXN_Date = DateTime.Parse (drm["ORG_GUI_DATE"].ToString ( )).ToString ("yyyy-MM-dd");
                        FUNC_DATA.ECR_No = drm["ORG_POS_ID"].ToString ( );
                        FUNC_DATA.TXN_No = drm["ORG_TRANS_NO"].ToString ( );
                        FUNC_DATA.TXN_GUIBegNo = drm["RECE_TRACK"].ToString ( ) + drm["GUI_BEGIN"].ToString ( );
                        if (FUNC_DATA.TXN_GUIBegNo == "")
                            FUNC_DATA.TXN_GUIBegNo = "00000000";
                        FUNC_DATA.TXN_Status = drm["TRANS_TYPE"].ToString ( ) == "02" ? "E" : "D";
                        FUNC_DATA.TXN_VoidDT = DateTime.Parse (drm["SALES_DATE"].ToString ( )).ToString ("yyyy-MM-dd");
                        //當發票日與帳務日不同 檢查M是否要用作廢
                        if (DateTime.Parse (drm["SALES_DATE"].ToString ( )) != DateTime.Parse (drm["TDATE"].ToString ( )))
                        {
                            sqlstr = @"select m.TDATE from " + dbname + ".rm_prod_sales_m m " +
                                " where RECE_TRACK = '" + drm["RECE_TRACK"].ToString ( ) + "' AND GUI_BEGIN = '" + drm["GUI_BEGIN"].ToString ( ) + "' AND TRANS_TYPE = '01'";

                            DataTable dtm_chk = mardb.GetData (sqlstr);
                            if (dtm_chk != null && dtm_chk.Rows.Count > 0)
                            {
                                //當發票日同一天，則為作廢
                                if (DateTime.Parse (dtm_chk.Rows[0]["TDATE"].ToString ( )) == DateTime.Parse (drm["TDATE"].ToString ( )))
                                {
                                    FUNC_DATA.TXN_Status = "D";
                                    FUNC_DATA.TXN_VoidDT = DateTime.Parse (drm["TDATE"].ToString ( )).ToString ("yyyy-MM-dd");
                                }
                            }
                        }

                        //FUNC_DATA.TXN_Status = 
                        FUNC_DATA.TXN_VoidStoreNo = drm["SHOP_ID"].ToString ( );
                        FUNC_DATA.TXN_VoidEcrNo = drm["POS_ID"].ToString ( );
                        FUNC_DATA.TXN_VoidTxnNo = drm["TRANS_NO"].ToString ( );
                        FUNC_DATA.TXN_VoidUsrNo = drm["RECEIVER_ID"].ToString ( );
                        FUNC_DATA.TXN_VoidTime = DateTime.Parse (drm["TDATE"].ToString ( )).ToString ("yyyy-MM-dd");
                        if (drm["RECORD_BEGIN"].ToString ( ).Length > 4)
                            FUNC_DATA.TXN_VoidTime += " " + drm["RECORD_BEGIN"].ToString ( ).Substring (0, 2) + ":" + drm["RECORD_BEGIN"].ToString ( ).Substring (2, 2) + ":" + drm["RECORD_BEGIN"].ToString ( ).Substring (4, 2);
                        else
                            FUNC_DATA.TXN_VoidTime += " " + drm["RECORD_BEGIN"].ToString ( ).Substring (0, 2) + ":" + drm["RECORD_BEGIN"].ToString ( ).Substring (2, 2) + ":00";
                        FUNC_DATA.TXN_VoidShiftNo = "01";
                        FUNC_DATA.TXN_DATA_HASH = FUNC_DATA.TXN_DATA_HASH = GetMD5 (FUNC_SIGNATURE + sales_b_request.FUNC_ID + FUNC_DATA.STO_No + FUNC_DATA.TXN_Date + FUNC_DATA.ECR_No + FUNC_DATA.TXN_No + FUNC_DATA.TXN_VoidTime + FUNC_DATA.TXN_GUIBegNo);

                        sales_b_request.FUNC_DATA = FUNC_DATA;

                        //轉成字串傳送API
                        string str_rmrequest = Newtonsoft.Json.JsonConvert.SerializeObject (sales_b_request);
                        string strReturn = GoOtherAPI (str_rmrequest, apiurl);

                        try
                        {
                            sales_result apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<sales_result> (strReturn);
                            if (apiresult.RSPN_CODE == "1")
                            { }
                            else
                            {
                                //失敗改狀態
                                sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = '' where SALES_DATE = '" + DateTime.Parse (drm["SALES_DATE"].ToString ( )).ToString ("yyyy/MM/dd") + "' and POS_ID = '" + drm["POS_ID"].ToString ( ) + "' and trans_no = '" + drm["TRANS_NO"].ToString ( ) + "'";

                                mardb.UpdData (sqlstr);

                                bf.funWriteLog (str_rmrequest);
                                bf.funWriteLog (strReturn);
                            }
                        }
                        catch
                        {
                            //失敗改狀態
                            sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = '' where SALES_DATE = '" + DateTime.Parse (drm["SALES_DATE"].ToString ( )).ToString ("yyyy/MM/dd") + "' and POS_ID = '" + drm["POS_ID"].ToString ( ) + "' and trans_no = '" + drm["TRANS_NO"].ToString ( ) + "'";

                            mardb.UpdData (sqlstr);

                            bf.funWriteLog (str_rmrequest);
                            bf.funWriteLog (strReturn);
                        }
                    }


                }
            }

            wherepos = " (POS_ID,TRANS_NO,SALES_DATE) in (select m.POS_ID,m.TRANS_NO,m.SALES_DATE from " + dbname + ".rm_prod_sales_m m where ifnull(m.upd_flag,'') = 'T') and TRANS_TYPE != '01'";

            //把這次要轉的UPD_FLAG T改為Y
            //D
            //sqlstr = @"update " + dbname + ".rm_prod_sales_d set UPD_FLAG = 'Y' where ifnull(UPD_FLAG,'') = '' and " + wherepos;

            //mardb.UpdData(sqlstr);
            //P
            //sqlstr = @"update " + dbname + ".rm_pay_type set status = 'Y' where ifnull(status,'') = '' and " + wherepos;

            //mardb.UpdData(sqlstr);

            sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = 'Y' where ifnull(m.upd_flag,'') = 'T' and TRANS_TYPE != '01'";

            mardb.UpdData (sqlstr);
        }

        /// <summary>
        /// 清機
        /// </summary>
        private void DoClear ( )
        {
            BaseSet.BaseFunc bf = new BaseSet.BaseFunc ( );
            string apiurl = ConfigurationManager.AppSettings["apiurl"];
            string dbname = ConfigurationManager.AppSettings["dbname"];
            string dbstr = ConfigurationManager.AppSettings["dbstr"];
            string strposid = ConfigurationManager.AppSettings["posid"];
            string uniform = ConfigurationManager.AppSettings["uniform"];
            string ACNT_NO = ConfigurationManager.AppSettings["ACNT_NO"];
            string FUNC_SIGNATURE = ConfigurationManager.AppSettings["FUNC_SIGNATURE"];

            string wherepos = " 1 = 1";
            if (strposid != "")
                wherepos += " and m.pos_id = '" + strposid + "'";

            //不取銷貨
            wherepos += " and ifnull(m.EVENT,'') = '' ";

            BaseSet.ClassDB_MariaDB mardb = new BaseSet.ClassDB_MariaDB (dbstr);

            //匯出
            bf.funWriteUseLog ("開始匯出退貨作廢");

            //POS
            string sqlstr = @"select * from " + dbname + ".rm_pos ";

            DataTable dtPOS = mardb.GetData (sqlstr);

            //先把這次要轉的UPD_FLAG改為T
            sqlstr = @"update " + dbname + ".rm_posreport m set m.EVENT = 'T' where " + wherepos;

            if (!(mardb.UpdData (sqlstr)))
                return;

            //加條件取有改T的資料以便檢查
            wherepos = " ifnull(EVENT,'') = 'T' ";

            //M
            sqlstr = @"select * from " + dbname + ".rm_posreport " +
                " where " + wherepos;

            DataTable dtm = mardb.GetData (sqlstr);

            for (int pi = 0; pi < dtPOS.Rows.Count; pi++)
            {
                var querym = from row in dtm.AsEnumerable ( )
                             where row.Field<string> ("pos_id") == dtPOS.Rows[pi]["pos_id"].ToString ( )
                             select row;

                var L_m = querym.ToList ( );

                if (L_m.Count > 0)
                {
                    clear_request clear_request = new clear_request ( );
                    clear_request.FUNC_ID = "KGINV_Z_REPORT";
                    clear_request.ACNT_NO = ACNT_NO;
                    clear_request.SYS_DATE = DateTime.Now.ToString ("yyyyMMddHHmmss");
                    clear_request.FUNC_SIGNATURE = FUNC_SIGNATURE;

                    FUNC_DATA_CLEAR FUNC_DATA = new FUNC_DATA_CLEAR ( );
                    FUNC_DATA.STO_No = L_m[0]["SHOP_ID"].ToString ( );
                    FUNC_DATA.TXN_Date = DateTime.Parse (L_m[0]["SALES_DATE"].ToString ( )).ToString ("yyyy-MM-dd");
                    FUNC_DATA.ECR_No = L_m[0]["POS_ID"].ToString ( );
                    FUNC_DATA.USR_No = L_m[0]["POS_ID"].ToString ( );
                    FUNC_DATA.TXN_DATA_HASH = FUNC_DATA.TXN_DATA_HASH = GetMD5 (FUNC_SIGNATURE + clear_request.FUNC_ID + FUNC_DATA.STO_No + FUNC_DATA.TXN_Date + FUNC_DATA.ECR_No + FUNC_DATA.USR_No);

                    for (int i = 0; i < L_m.Count; i++)
                    {
                        FUNC_DATA.SLE_TxnBegNo = "000001";
                        //FUNC_DATA.SLE_TxnEndNo = "";

                        if (L_m[i]["TITLE"].ToString ( ) == "T008")
                        {
                            FUNC_DATA.SLE_GUIBegNo = L_m[i]["GUI_BEGIN"].ToString ( );
                            FUNC_DATA.SLE_GUIEndNo = L_m[i]["GUI_END"].ToString ( );
                        }

                        if (L_m[i]["TITLE"].ToString ( ).Length == 2)
                        {
                            if (L_m[i]["TITLE"].ToString ( ) == "10")
                                FUNC_DATA.SLE_PayAmt1 += int.Parse (L_m[i]["SALES_AMT"].ToString ( ));

                            if (L_m[i]["TITLE"].ToString ( ).Substring (0, 1) == "4")
                                FUNC_DATA.SLE_PayAmt14 += int.Parse (L_m[i]["SALES_AMT"].ToString ( ));

                            if (L_m[i]["TITLE"].ToString ( ).Substring (0, 1) == "8")
                                FUNC_DATA.SLE_PayAmt27 += int.Parse (L_m[i]["SALES_AMT"].ToString ( ));
                        }
                    }

                    clear_request.FUNC_DATA = FUNC_DATA;

                    //轉成字串傳送API
                    string str_rmrequest = Newtonsoft.Json.JsonConvert.SerializeObject (clear_request);
                    string strReturn = GoOtherAPI (str_rmrequest, apiurl);

                    try
                    {
                        sales_result apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<sales_result> (strReturn);
                        //if (apiresult.RSPN_CODE == "1")
                        //    //"成功"
                        //else
                        //    //"失敗"
                    }
                    catch
                    {
                        //"失敗"
                    }
                }
            }
        }


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
                webrequest.Method = WebRequestMethods.Http.Post;
                webrequest.ContentType = "application/json";
                webrequest.ContentLength = jsonBytes.Length;

                using (var requestStream = webrequest.GetRequestStream ( ))
                {
                    requestStream.Write (jsonBytes, 0, jsonBytes.Length);
                    requestStream.Flush ( );
                }

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
                return ex.Message;
            }
        }

        public string GetMD5 ( string Data )
        {
            System.Security.Cryptography.MD5 MD5Provider = System.Security.Cryptography.MD5.Create ( );
            byte[ ] source = System.Text.Encoding.Default.GetBytes (Data);//將字串轉為Byte[]
            byte[ ] crypto = MD5Provider.ComputeHash (source);//進行MD5加密

            string result = "";
            for (int i = 0; i < crypto.Length; i++)
            {
                result += crypto[i].ToString ("x2");//把加密後的字串從Byte[]轉為字串
            }

            return result;
        }

    }
}
