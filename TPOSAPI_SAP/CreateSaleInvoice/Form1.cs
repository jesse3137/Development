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

namespace CreateSaleInvoice
{
    public partial class Form1 : Form
    {
        #region Class

        /// <summary>
        /// 
        /// </summary>
        class CreateSaleInvoice
        {
            /// <summary>
            /// 發票號碼
            /// </summary>
            public string GUIDocNum;
            /// <summary>
            /// 分公司代碼 (Ex: 心之方庭 → D1)
            /// </summary>
            public string CorpCode;
            /// <summary>
            /// 排序號碼 (Ex: 1,2,3… etc.)
            /// </summary>        
            public int VisOrder;
            /// <summary>
            /// 發票日期 (Ex: 2018/01/01)
            /// </summary>
            public string InvDate;
            /// <summary>
            /// 買受人統一編號 (Ex: 客戶統一編號，若無，則填空)
            /// </summary>
            public string UniFormID;
            /// <summary>
            /// "發票格式代碼 (以下為國稅局規定的發票格式：
            /// 31-三聯式發票 (銷項)
            /// 32-二聯式發票 包含收據 (銷項)
            /// 33-三聯式 - 銷項退回或折讓證明單
            /// 34-二聯式 - 銷項退回或折讓證明單 (含銷項免用發票)
            /// 35-三聯式收銀機發票/電子發票
            /// 36-銷項免用發票)"
            /// </summary>
            public string FrmCode;
            /// <summary>
            /// 稅類別代碼 (以下為國稅局規定的稅類別：
            /// 1-應稅, 2-零稅, 3-免稅, F-作廢)
            /// </summary>
            public string TaxType;
            /// <summary>
            /// 發票銷售金額 (未稅的發票金額 → 整數)
            /// </summary>
            public int DocAmount;
            /// <summary>
            ///發票稅額 (發票的5% 稅額 → 整數) 
            /// </summary>
            public int TaxAmount;
            /// <summary>
            /// 日結A/R發票號碼
            /// </summary>
            public int DocEntry;
            /// <summary>
            /// 日結A/R發票的稅群組代碼 (Ex: J0-進項稅0%, X0-銷項稅0%, X5-銷售稅5%, J5-進項稅5%)
            /// </summary>
            public string VatGroup;
        }

        class rcrm
        {
            public string rc { get; set; }

            public string rm { get; set; }
        }
        class apiresult
        {
            public rcrm rcrm { get; set; }
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
        /*
       {"GUIDocNum": "HB00000001",CorpCode": "D1",VisOrder": "1",InvDate": "2018/01/01",UniFormID": " ","FrmCode": "35",
           TaxType": "1",DocAmount": "1000",TaxAmount": "50",DocEntry": "1097",VatGroup": "X1",}
              */

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
                /*
               {"GUIDocNum": "HB00000001",CorpCode": "D1",VisOrder": "1",InvDate": "2018/01/01",UniFormID": " ","FrmCode": "35",
                   TaxType": "1",DocAmount": "1000",TaxAmount": "50",DocEntry": "1097",VatGroup": "X1",}
                      */
                if (L_m.Count > 0)
                {
                    for (int i = 0; i < L_m.Count; i++)
                    {
                        DataRow dr_m = L_m[i];
                        CreateSaleInvoice reuqest = new CreateSaleInvoice ( );
                        reuqest.GUIDocNum = dr_m["RECE_TRACK"].ToString ( ) + dr_m["GUI_BEGIN"].ToString ( );
                        reuqest.CorpCode = dr_m["SHOP_ID"].ToString ( );//TODO 待修 
                        reuqest.VisOrder = i + 1;//TODO 待修 
                        reuqest.InvDate = DateTime.Parse (dr_m["SALES_DATE"].ToString ( )).ToString ("yyyy/MM/dd");
                        reuqest.UniFormID = dr_m["COMP_ID"].ToString ( );
                        reuqest.FrmCode = "";//TODO 待修 
                        reuqest.TaxType = dr_m["TRANS_TYPE"].ToString ( )=="03"?"F":"1";//TODO 待修 退貨要給應稅??
                        reuqest.DocAmount = int.Parse (dr_m["NET"].ToString ( ));
                        reuqest.TaxAmount = 50;//TODO 待修 
                        reuqest.DocEntry = int.Parse (dr_m["RECE_TRACK"].ToString ( ) + dr_m["GUI_BEGIN"].ToString ( ));//TODO 待修 
                        reuqest.VatGroup = "X5";//TODO 待修

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

                        //D                              
                        for (int d = 0; d < L_d.Count ( ); d++)
                        {
                            DataRow drd = L_d[d];

                        }


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
                                bf.strPathUseLog = @"bin\CreateSaleInvoice";
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
