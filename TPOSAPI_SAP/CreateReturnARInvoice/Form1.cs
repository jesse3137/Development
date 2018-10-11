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

namespace CreateReturnARInvoice
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

        /// <summary>
        /// A/R 貸項通知單
        /// </summary>
        class CreateReturnARInvoice
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
            /// 稅碼
            /// M檔TAX 稅別(DEFAULT 1)(參數:RS_TAX) 需與SAP對照傳SAP代碼
            /// </summary>
            public string VatGroup;
            /// <summary>
            /// 數量(庫存單位)
            /// QTY 銷售數量
            /// </summary>
            public int Quantity;
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
            public int LineTotal;
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

            this.Close ( );
        }

        #region 銷貨
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

            //先把這次要轉的UPD_FLAG改為T TODO待修
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
                /*    {"CardCode":"zzzz","DocDate":"2018/09/11","DocDueDate":"2018/09/30","TaxDate":"2018/09/12","U_PosDocNum":"201809110001","BPLId":"A2","Comments":"APIsCreate",
                 * "Lines":                      [{"ItemCode":"0000001","Quantity":"1","VatGroup":"X5","PriceAfVAT":"210","WhsCode":"A2","StoreCode":"A2A",
                     * "DepartCode":"A202"},{"ItemCode":"Lav0001","Quantity":"2","VatGroup":"X5","Price":"20","WhsCode":"A2",
                     * "StoreCode":"A2A","DepartCode":"A202"}]} 
                 */
                if (L_m.Count > 0)
                {
                    for (int i = 0; i < L_m.Count; i++)
                    {
                        DataRow dr_m = L_m[i];
                        CreateReturnARInvoice reuqest = new CreateReturnARInvoice ( );
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
                            //dr_lines.VatGroup = dr_m[""].ToString ( );//TODO待修
                            //dr_lines.LineTotal = int.Parse (drd["SALES_AMT"].ToString ( ));//TODO待修                        
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
                                strReturn = Newtonsoft.Json.JsonConvert.SerializeObject (apiresult.rcrm.rm);
                                //}
                                //else
                                //{
                                //失敗改狀態
                                sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = '' where SALES_DATE = '" + DateTime.Parse (dr_m["SALES_DATE"].ToString ( )).ToString ("yyyy/MM/dd") + "' and POS_ID = '" + dr_m["POS_ID"].ToString ( ) + "' and trans_no = '" + dr_m["TRANS_NO"].ToString ( ) + "'";

                                mardb.UpdData (sqlstr);
                                bf.strPathUseLog = @"bin\CreateReturnARInvoice";
                                bf.funWriteLog (strReturn);
                            }
                        }
                        catch
                        {
                            //失敗改狀態
                            sqlstr = @"update " + dbname + ".rm_prod_sales_m m set m.UPD_FLAG = '' where SALES_DATE = '" + DateTime.Parse (dr_m["SALES_DATE"].ToString ( )).ToString ("yyyy/MM/dd") + "' and POS_ID = '" + dr_m["POS_ID"].ToString ( ) + "' and trans_no = '" + dr_m["TRANS_NO"].ToString ( ) + "'";

                            mardb.UpdData (sqlstr);

                            bf.funWriteLog (strReturn);
                        }
                    }


                }
            }

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

    }
}
