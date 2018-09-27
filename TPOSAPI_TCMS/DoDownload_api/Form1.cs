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
using AppWebAPI.Adapters;
//TODO Test
using AppWebAPI.Models;
using AppWebAPI.Models.v1;
using AppWebAPI.Models.v1.Product;

namespace DoDownload_api
{
    public partial class Form1 : Form
    {
        #region class

        class rcrm
        {
            public string rc { get; set; }

            public string rm { get; set; }
        }

        class apiresult
        {
            public rcrm rcrm { get; set; }
        }


        class results
        {

            public List<payment_detail_result> payment_detail_result { get; set; }

        }

        class apiresult_payment
        {
            public rcrm rcrm { get; set; }

            public results results { get; set; }
        }

        class pos_status
        {
            /// <summary>
            /// 機號
            /// </summary>
            public string strtillcode;
            /// <summary>
            /// 狀態
            /// </summary>
            public string strstatus;
        }

        class results_posstatus
        {
            public List<pos_status> pos_status { get; set; }
        }

        class apiresult_posstatus
        {
            public rcrm rcrm { get; set; }

            public results_posstatus results { get; set; }
        }

        /// <summary>
        /// 支付方式回傳
        /// </summary>
        class payment_detail_result
        {
            /// <summary>
            /// TPOS流水號
            /// </summary>
            public string intpaymentno;
            /// <summary>
            /// 名稱
            /// </summary>
            public string strbopaymentname;
            /// <summary>
            /// TCMS欄位
            /// </summary>
            public string strextsref1;
        }
        #endregion

        string dowhat;
        public Form1 ( string param )
        {
            dowhat = param;
            InitializeComponent ( );
        }

        private void Form1_Load ( object sender, EventArgs e )
        {
            string dostatus = ConfigurationManager.AppSettings["dostatus"];

            if (dostatus != "Y")
                DO ( );
            else
                // UPPOS_STATUS ( );
                this.Close ( );
        }




        private void DO ( )
        {
            string strSql = "";
            //string strWhere = "";
            string str_request = "";
            string apiurl = ConfigurationManager.AppSettings["apiurl"];
            string apiname = "";
            string dbname = ConfigurationManager.AppSettings["dbname"];
            string shop_id = "";
            string strline = ConfigurationManager.AppSettings["line"];
            string strdoshop = ConfigurationManager.AppSettings["doshop"];//分店代號
            string mdbstr = ConfigurationManager.AppSettings["mdbstr"];

            //TODO test
            string postgredb = ConfigurationManager.AppSettings["postgredb"];
            WebDB db = new WebDB ( );
            MariaDB db_maria = new MariaDB (mdbstr);

            #region  test
            //  Test db_postgre = new Test (postgredb);
            //  strSql = string.Format (@" SELECT * FROM  pg_catalog.pg_extension ");
            //DataTable dt_test= db_postgre.GetData (strSql);
            //strSql = "INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY,JOIN_DATE) VALUES (15, 'John', 33, 'Calaefeg', 28000.00 ,'2001-10-13');";
            //insert
            //db_postgre.UpdData(strSql);
            /*        strSql = "SELECT * FROM COMPANY;";
                    DataTable dt_test =db_postgre.GetData(strSql);
                    List<AppWebAPI.Models.Test.drtest> list_test = new List<AppWebAPI.Models.Test.drtest>();
                    foreach (DataRow dr_test in dt_test.Rows)
                    {
                      AppWebAPI.Models.Test.drtest dr=new AppWebAPI.Models.Test.drtest();
                      dr.ID = dr_test["ID"].ToString();
                      dr.NAME = dr_test["NAME"].ToString();
                      dr.AGE = dr_test["AGE"].ToString();
                      dr.ADDRESS = dr_test["ADDRESS"].ToString();
                      dr.SALARY = dr_test["SALARY"].ToString();
                      list_test.Add(dr);
                    }
                    str_request = Newtonsoft.Json.JsonConvert.SerializeObject(list_test);
                    GoOtherAPI(str_request, apiurl + apiname);
                    */

            //strSql = "UPDATE COMPANY SET SALARY = 25000 WHERE SALARY = 20001;";
            //db_postgre.UpdData(strSql);
            //DataTable dt_test= db_postgre.GetData("SELECT * FROM COMPANY");
            //db_postgre.UpdData("DELETE FROM COMPANY WHERE ID=15;");
            #endregion



            strSql = "SELECT * FROM " + dbname + "base_shop";

            #region 判斷是否設定分店,無全部執行
            if (!string.IsNullOrEmpty (strdoshop))
            {
                if (strdoshop.Split (',').Length > 1)
                {
                    string[ ] strarry = strdoshop.Split (',');
                    string strShop = "";
                    for (int i = 0; i < strarry.Length; i++)
                    {
                        strShop += "'" + strarry[i].ToString ( ) + "'";
                        if (strarry.Length - 1 > i) strShop += ",";
                    }
                    strSql = strSql + " where SHOP_ID in (" + strShop + ")";
                }
                else strSql = strSql + "where SHOP_ID =" + strdoshop;
            }
            #endregion

            //DataTable dt_shop = db.ClassDB.GetData(strSql);
            DataTable dt_shop = db_maria.GetData (strSql);

            if (dt_shop != null)
            {
                for (int s = 0; s < dt_shop.Rows.Count; s++)
                {
                    shop_id = dt_shop.Rows[s]["SHOP_ID"].ToString ( );
                    apiurl = dt_shop.Rows[s]["API_URL"].ToString ( );

                    db.ClassFunction.funWriteRunLog ("開始" + shop_id);

                    #region POS
                    strSql = "SELECT * FROM " + dbname + "rm_pos where 1 = 1";
                    string strReturn = "";
                    //DataTable dt = db.ClassDB.GetData(strSql);
                    DataTable dt = db_maria.GetData (strSql);

                    if (dt != null)
                    {
                        apiname = "download_pos";
                        // DataRow[] dra_pos = dt.Select("DATA_TYPE = '18'");

                        strSql = "SELECT * FROM " + dbname + "rm_pos  where rm_pos.SHOP_ID=" + shop_id;
                        //DataTable dt_pos = db.ClassDB.GetData(strSql);
                        DataTable dt_pos = db_maria.GetData (strSql);

                        AppWebAPI.Models.v1.Product.Download_Pos_Request pos_request = new AppWebAPI.Models.v1.Product.Download_Pos_Request ( );
                        List<AppWebAPI.Models.v1.Product.pos_detail> pos_detail = new List<AppWebAPI.Models.v1.Product.pos_detail> ( );

                        #region 將統收櫃額外紀錄
                        //將統收櫃額外紀錄
                        /*                 string str_storeall = "";
                                         for (int p = 0; p < dra_pos.Length; p++)
                                         {
                                             string[] L_till = dra_pos[p]["DATA"].ToString().Replace("\r", "").Replace("\n", "").Split(',');
                                             //因為POS DB目前沒有統收(專櫃編號必填)
                                             if (L_till.Length == 2)
                                             {
                                                 //看看POS機是否要更新到TPOS
                                                 DataRow[] dra_posYN = dt_pos.Select("POS_ID = '" + L_till[0].ToString() + "'");
                                                 if (dra_posYN.Length > 0)
                                                 {
                                                     if (dra_posYN[0]["TPOS_YN"].ToString() != "Y")
                                                         continue;
                                                 }
                                                 else
                                                     continue;

                                                 string strstore = "999999";
                                                 if (L_till[1].ToString().Trim() != "")
                                                 {
                                                     strstore = L_till[1].ToString().Substring(1, L_till[1].ToString().Length - 2);
                                                     str_storeall += strstore + ";";
                                                 }

                                                 AppWebAPI.Models.v1.Product.pos_detail pd = new AppWebAPI.Models.v1.Product.pos_detail();
                                                 pd.strtillcode = L_till[0].ToString();
                                                 pd.strtillname = "";
                                                 pd.strstorecode = strstore;
                                                 pd.ysnactive = "T";
                                                 pd.strtilltype = "C";
                                                 pd.strtillprofileno = dra_posYN[0]["POS_MODULE"].ToString();
                                                 pos_detail.Add(pd);
                                             }
                                         }
                                         pos_request.pos_detail = pos_detail;

                                         str_request = Newtonsoft.Json.JsonConvert.SerializeObject(pos_request);

                                         strReturn = GoOtherAPI(str_request, apiurl + apiname);
                 */
                        #endregion


                        foreach (DataRow dr_Pos in dt_pos.Rows)
                        {
                            AppWebAPI.Models.v1.Product.pos_detail pd = new AppWebAPI.Models.v1.Product.pos_detail ( );

                            pd.strtillcode = dr_Pos["POS_ID"].ToString ( );//收銀機代碼
                            pd.strtillname = dr_Pos["POS_ID"].ToString()+"-"+dr_Pos["POS_ID"].ToString();//收銀機描述
                            pd.strstorecode = dr_Pos["SHOP_ID"].ToString ( );//租戶代碼
                            pd.ysnactive = "T";//啟用/停用(收銀機狀態 (Z:虛擬機台))
                            pd.strtilltype = "C";//收銀機類型(POS SERVER代碼)
                            pd.intTillprofileno = 1;
                            pd.intPosfastkeyno = 1;
                            pd.strEinv_ysnenable = "T";
                            pd.strEinv_ysntestmode = "F";
                            pd.intPosfunckeyno = int.Parse (dr_Pos["POS_AGREEMENT"].ToString ( ) == "0" ? "999999" : dr_Pos["SHOP_ID"].ToString ( ));                         
                            pd.intTweinvprofileno = 1;
                            pd.strEinv_straccountid = "";//TODO 要再修改                        
                            pd.strEinv_strposid = pd.strtillcode;
                            pd.strEinv_straccesstoken = "AES";
                            pd.strEinv_strshopid = dr_Pos["SHOP_ID"].ToString ( );
                            if (dr_Pos["ROLL_CNT"].ToString ( ) != "") pd.intEinv_inttakerollcnt = int.Parse (dr_Pos["ROLL_CNT"].ToString ( ));
                            pd.strEINV_STRHQCHECKFAILACTION = "W";
                            pd.strCompcode = "AENO";
                            pd.strStatus = "F";


                            /*
                            //收銀機區分 (0:統收或1:獨立)
                            if (dr_Pos["POS_AGREEMENT"].ToString ( ).Equals ("0"))
                            {
                                pd.strtillprofileno = "1";
                            }
                            if (dr_Pos["POS_AGREEMENT"].ToString ( ).Equals ("1"))
                            {
                                pd.strtillprofileno = "2";
                            }
                            if (string.IsNullOrEmpty (dr_Pos["POS_AGREEMENT"].ToString ( )))
                            {
                                pd.strtillprofileno = "1";
                            }
                             */

                            pos_detail.Add (pd);
                        }

                        pos_request.pos_detail = pos_detail;


                        str_request = Newtonsoft.Json.JsonConvert.SerializeObject (pos_request);

                        strReturn = GoOtherAPI (str_request, apiurl + apiname);

                        str_request = Newtonsoft.Json.JsonConvert.SerializeObject (pos_request);

                        try
                        {
                            apiresult apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);

                            if (apiresult.rcrm.rc != "1")
                            {
                                str_request = Newtonsoft.Json.JsonConvert.SerializeObject (apiresult.rcrm.rm);
                            }
                        }
                        catch
                        {
                        }

                    #endregion


                        #region Casher
                        apiname = "download_casher";
                        // DataRow[] dra_casher = dt.Select("DATA_TYPE = '6'");

                        strSql = "SELECT * FROM " + dbname + "rm_tena_per where rm_tena_per.SHOP_ID=" + shop_id;

                        // DataTable dt_casher_all = db.ClassDB.GetData(strSql);
                        DataTable dt_casher_all = db_maria.GetData (strSql);

                        AppWebAPI.Models.v1.Product.Download_Casher_Request casher_request = new AppWebAPI.Models.v1.Product.Download_Casher_Request ( );
                        List<AppWebAPI.Models.v1.Product.casher_data> casher_data = new List<AppWebAPI.Models.v1.Product.casher_data> ( );

                        #region 舊
                        /*                
                                for (int c = 0; c < dra_casher.Length; c++)
                                {
                                    string strcasher = dra_casher[c]["DATA"].ToString().Replace("\r", "").Replace("\n", "");
                                    if (strcasher.Substring(strcasher.Length - 1, 1) == "*")
                                        strcasher = strcasher.Substring(0, strcasher.Length - 1);

                                    string strstatus = strcasher.Substring(0, 1).Trim();
                                    string strcardno = strcasher.Substring(1, 16).Trim();
                                    string strcasherid = strcasher.Substring(17, 10).Trim();

                                    //從後面抓(因為文字檔中文算兩碼)
                                    string strclassno = strcasher.Substring(strcasher.Length - 8, 8).Trim();
                                    string strcasherpwd = strcasher.Substring(strcasher.Length - 16, 8).Trim();
                                    string strlevel = strcasher.Substring(strcasher.Length - 17, 1);

                                    if (strclassno != "")
                                    {
                                        AppWebAPI.Models.v1.Product.casher_data cd = new AppWebAPI.Models.v1.Product.casher_data();
                                        cd.strusercode = strcasherid;
                                        cd.intposuserno = strcasherid;
                                        cd.ysnactive = strstatus == "A" ? "T" : "F";
                                        try
                                        {
                                            int.Parse(strcasherpwd);
                                            cd.intpospassword = strcasherpwd;
                                        }
                                        catch
                                        {
                                            //密碼改成帳號 且停用
                                            cd.intpospassword = strcasherid;
                                            cd.ysnactive = "F";
                                        }

                                        //if (str_storeall.IndexOf(strclassno) == -1)

                                        if (dt_casher_all.Select(" TENANT_PER_ID = '" + strcasherid + "'").Length > 0)

                                            cd.strstorecode = "999999";
                                        else
                                            cd.strstorecode = strclassno;

                                        cd.strareacode = "ALL";
                                        cd.strrolecode = strlevel;
                                        casher_data.Add(cd);
                                    }
                                }
                         casher_request.casher_data = casher_data;

                        str_request = Newtonsoft.Json.JsonConvert.SerializeObject(casher_request);

                        strReturn = GoOtherAPI(str_request, apiurl + apiname);

                        try
                        {
                            apiresult apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult>(strReturn);

                            if (apiresult.rcrm.rc != "1")
                            {
                            }
                        }
                        catch
                        {
                        }
                */
                        #endregion


                        foreach (DataRow dr_Casher in dt_casher_all.Rows)
                        {
                            AppWebAPI.Models.v1.Product.casher_data cd = new AppWebAPI.Models.v1.Product.casher_data ( );

                            cd.strusercode = dr_Casher["TENA_PER_ID"].ToString ( );//收銀員名稱(人員編號)
                            cd.intposuserno = dr_Casher["TENA_PER_ID"].ToString ( );//POS前台帳號(人員編號)
                            cd.ysnactive = dr_Casher["STATUS"].ToString ( ); ;//啟用/停用
                            cd.Strcompcode = "AEON";//測試先給AEON,正式換分店代號


                            #region  判斷是否有設密碼
                            if (!string.IsNullOrEmpty (cd.intpospassword = dr_Casher["PWD"].ToString ( )))//原密碼
                            {
                                cd.intpospassword = dr_Casher["PWD"].ToString ( );//密碼(MD5) 原密碼

                                //判斷是否啟用狀態
                                if (!string.IsNullOrEmpty (dr_Casher["STATUS"].ToString ( )))//狀態
                                {
                                    if (dr_Casher["STATUS"].ToString ( ).Equals ("0"))
                                    {
                                        cd.ysnactive = "F";
                                    }
                                    if (dr_Casher["STATUS"].ToString ( ).Equals ("1"))
                                    {
                                        cd.ysnactive = "T";
                                    }
                                }
                            }

                            //密碼改成帳號 且停用
                            if (string.IsNullOrEmpty (dr_Casher["PWD"].ToString ( )))//原密碼
                            {
                                cd.intpospassword = dr_Casher["TENA_PER_ID"].ToString ( );//人員編號
                                cd.ysnactive = "F";
                            }

                            #endregion

                            if (dt_casher_all.Select (" TENA_PER_ID = '" + dr_Casher["TENA_PER_ID"].ToString ( ) + "'").Length > 0)
                            {
                                if (!string.IsNullOrEmpty (dr_Casher["SHOP_ID"].ToString ( )))//分店
                                    cd.strstorecode = dr_Casher["SHOP_ID"].ToString ( );//所屬分店
                            }

                            else
                                cd.strstorecode = "999999";//租戶代碼

                            cd.strareacode = "ALL";//區域代碼
                            cd.strrolecode = dr_Casher["LEVEL"].ToString ( );//等級
                            casher_data.Add (cd);
                        }

                        casher_request.casher_data = casher_data;

                        str_request = Newtonsoft.Json.JsonConvert.SerializeObject (casher_request);

                        strReturn = GoOtherAPI (str_request, apiurl + apiname);

                        try
                        {
                            apiresult apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);

                            if (apiresult.rcrm.rc != "1")
                            {
                            }
                        }
                        catch
                        {
                        }


                        #endregion


                        #region Agreement
                        apiname = "DOWNLOAD_Agreement";
                        //DataRow[] dra_agreement = dt.Select("DATA_TYPE = '1'");

                        strSql = "SELECT * FROM " + dbname + "base_shop where SHOP_ID=" + shop_id;

                        DataTable dt_ls_agreement_all = db_maria.GetData (strSql);

                        AppWebAPI.Models.v1.Product.Download_Agreement_Request agreement_request = new AppWebAPI.Models.v1.Product.Download_Agreement_Request ( );
                        List<AppWebAPI.Models.v1.Product.agreement_detail> agreement_detail = new List<AppWebAPI.Models.v1.Product.agreement_detail> ( );

                        #region 舊的
                        /*
                for (int a = 0; a < dra_agreement.Length; a++)
                {
                    string strAgree = dra_agreement[a]["DATA"].ToString ( ).Replace ("\r", "").Replace ("\n", "");
                    string strstatus = strAgree.Substring (0, 1).Trim ( );
                    string strclass = strAgree.Substring (1, 8).Trim ( );
                    string strdept = strAgree.Substring (9, 6).Trim ( );
                    string strdeptname = strAgree.Substring (19, strAgree.Length - 20).Trim ( );

                    AppWebAPI.Models.v1.Product.agreement_detail ad = new AppWebAPI.Models.v1.Product.agreement_detail ( );
                    ad.strstorecode = strdept.Substring (1);
                    ad.strstorename = strdeptname;
                    ad.strstoretype = "Store";
                    ad.ysnactive = strstatus == "A" ? "T" : "F";
                    ad.strphone = "";
                    ad.strfax = "";
                    ad.strcontactname = "";
                    agreement_detail.Add (ad);
                }     
  */
                        #endregion

                        foreach (DataRow dr_agreement in dt_ls_agreement_all.Rows)
                        {

                            AppWebAPI.Models.v1.Product.agreement_detail ad = new AppWebAPI.Models.v1.Product.agreement_detail ( );
                            ad.strstorecode = dr_agreement["SHOP_ID"].ToString ( );//分店代號
                            ad.strstorename = dr_agreement["SHOP_NAME"].ToString ( );//租戶名稱(分店名稱)
                            // ad.strstoretype = dr_agreement["TYPE_ID"].ToString ( );//租戶類型TYPE_ID店型態 (對應 SHOP_TYPE)
                            ad.strstoretype = "Store";
                            ad.ysnactive = dr_agreement["TYPE_ID"].ToString ( ) == "1" ? "T" : "F";//啟用停用>>店況(參數 SHOP_STATUS)
                            ad.strphone = dr_agreement["TEL"].ToString ( );//租戶/公司電話
                            ad.strfax = dr_agreement["FAX"].ToString ( );//租戶/公司傳真
                            ad.strcontactname = dr_agreement["CONTACT_STAFF"].ToString ( );//租戶聯絡人名稱
                            agreement_detail.Add (ad);
                        }

                        agreement_request.agreement_detail = agreement_detail;

                        str_request = Newtonsoft.Json.JsonConvert.SerializeObject (agreement_request);

                        strReturn = GoOtherAPI (str_request, apiurl + apiname);

                        // str_request = Newtonsoft.Json.JsonConvert.SerializeObject (pos_request);
                        try
                        {
                            apiresult apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);

                            if (apiresult.rcrm.rc != "1")
                            {
                                str_request = Newtonsoft.Json.JsonConvert.SerializeObject (apiresult.rcrm.rm);
                            }
                        }
                        catch
                        {
                        }

                        #endregion


                        #region Discount

                        apiname = "DOWNLOAD_DIS";
                        // DataRow[] dra_discount = dt.Select("DATA_TYPE = '2'");

                        //strSql = "SELECT * FROM " + dbname + "rs_goods_d ";
                        strSql = "SELECT m.SC_ID, m.BARCODE_ID, m.GOODS_NAME, m.TAX, m.LPRC_TAX, d.* FROM "
                            + dbname + "rs_goods_d d," + dbname + "rs_goods_m m where d.GOODS_ID=m.GOODS_ID and d.SHOP_ID=" + shop_id;
                        DataTable dt_rm_pos_speed_key = db_maria.GetData (strSql);
                        strSql = "SELECT * FROM " + dbname + "rs_category";
                        DataTable dt_category = db_maria.GetData (strSql);
                        AppWebAPI.Models.v1.Product.Download_Dis_Request dis_request = new AppWebAPI.Models.v1.Product.Download_Dis_Request ( );
                        List<AppWebAPI.Models.v1.Product.discount_detail> discount_detail = new List<AppWebAPI.Models.v1.Product.discount_detail> ( );

                        #region 舊
                        //for (int d = 0; d < dra_discount.Length; d++)
                        //{
                        //    string strDiscount = dra_discount[d]["DATA"].ToString().Replace("\r", "").Replace("\n", "");
                        //    string strstatus = strDiscount.Substring(0, 1);
                        //    string strdept = strDiscount.Substring(1, 6);
                        //    string strgroup = strDiscount.Substring(7, 6);
                        //    string strgroupname = strDiscount.Substring(13, strDiscount.Length - 14).Trim();

                        //    AppWebAPI.Models.v1.Product.discount_detail dd = new AppWebAPI.Models.v1.Product.discount_detail();
                        //    dd.intitemno = strdept.Substring(1);
                        //    dd.stritemnamepos = strgroupname;
                        //    dd.ysnactive = strstatus == "A" ? "T" : "F";
                        //    dd.strclassify1code = "0";
                        //    dd.strclassify2code = "0";
                        //    dd.strclassify3code = "0";
                        //    dd.strclassify4code = "0";
                        //    dd.strextsref1 = strgroup;
                        //    discount_detail.Add(dd);
                        //}
                        #endregion


                        foreach (DataRow dr_rm_pos_speed_key in dt_rm_pos_speed_key.Rows)
                        {
                            AppWebAPI.Models.v1.Product.discount_detail dd = new AppWebAPI.Models.v1.Product.discount_detail ( );
                            dd.intitemno = dr_rm_pos_speed_key["GOODS_ID"].ToString ( );//(商品代碼)店內碼(進銷碼) 商品進貨碼
                            dd.stritemnamepos = dr_rm_pos_speed_key["GOODS_NAME"].ToString ( );//(顯示名稱 for pos,客顯)   
                            dd.ysnactive = dr_rm_pos_speed_key["IS_OVER"].ToString ( ) == "Y" ? "F" : "T";
                            dd.strclassify1code = dr_rm_pos_speed_key["SC_ID"].ToString ( ).Substring (0, 2);//租戶大分類(2)
                            dd.strclassify2code = dr_rm_pos_speed_key["SC_ID"].ToString ( ).Substring (0, 4);//租戶中分類(4)
                            dd.strclassify3code = dr_rm_pos_speed_key["SC_ID"].ToString ( ).Substring (0, 6);//租戶小分類(6)
                            dd.strclassify4code = "--";//租戶細分類(全)
                            foreach (DataRow dr_category in dt_category.Rows)
                            {
                                if (dr_category["CATEGORY_ID"].ToString ( ) == dd.strclassify1code)
                                    dd.strCategory1_Name = dr_category["CATEGORY_NAME"].ToString ( );//租戶大分類(2)
                                if (dr_category["CATEGORY_ID"].ToString ( ) == dd.strclassify2code)
                                    dd.strCategory2_Name = dr_category["CATEGORY_NAME"].ToString ( );//租戶中分類(4)
                                if (dr_category["CATEGORY_ID"].ToString ( ) == dd.strclassify3code)
                                    dd.strCategory3_Name = dr_category["CATEGORY_NAME"].ToString ( );//租戶小分類(6)    
                            }
                            if (!string.IsNullOrEmpty (dr_rm_pos_speed_key["GOODS_ID"].ToString ( )))
                                dd.strextsref1 = dr_rm_pos_speed_key["GOODS_ID"].ToString ( );//系統其他參考欄位
                            else dd.strextsref1 = "999999";
                            dd.strcurPrice = dr_rm_pos_speed_key["MPRC"].ToString ( );//參考售價
                            dd.strBarcode = dr_rm_pos_speed_key["BARCODE_ID"].ToString ( );//國際條碼                    
                            dd.strTax = dr_rm_pos_speed_key["TAX"].ToString ( ) == "0,3" ? "NOTAX" : "TAX";//銷售稅別
                            dd.strLprc_ax = dr_rm_pos_speed_key["LPRC_TAX"].ToString ( ) == "0,3" ? "NOTAX" : "TAX";//進價稅別
                            dd.strstorecode = dr_rm_pos_speed_key["SHOP_ID"].ToString ( );//分店代號
                            discount_detail.Add (dd);
                        }

                        dis_request.discount_detail = discount_detail;

                        str_request = Newtonsoft.Json.JsonConvert.SerializeObject (dis_request);
                        #region 寫入log
                        funWriteLog ("\r\n" + "JSON格式:" + str_request + "\r\n" + "URL:" + apiurl + "\r\n");
                        #endregion
                        strReturn = GoOtherAPI (str_request, apiurl + apiname);

                        try
                        {
                            apiresult apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);

                            if (apiresult.rcrm.rc != "1")
                            {
                            }
                        }
                        catch
                        {
                        }

                        #endregion


                        #region Speedkey 不用
                        /*
                        //有傳商品才更新快速鍵
                        if (dt_rm_pos_speed_key.Rows.Count > 0)
                        {
                            apiname = "DOWNLOAD_Speedkey";
                            strReturn = GoOtherAPI (str_request, apiurl + apiname);

                            try
                            {
                                apiresult apiresult = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult> (strReturn);

                                if (apiresult.rcrm.rc != "1")
                                {
                                }
                            }
                            catch
                            {
                            }
                        }
                        
                        */
                        #endregion


                        //線號6為正式使用，要把更新過的刪除
                        /*        if (strline == "6")
                                   {
                                       strSql = @"DELETE " + dbname + "UPDTBL" + strline + " where 1 = 1 " + strWhere;

                                      db.ClassDB.UpdData (strSql);
                                      
                                   }
                                   */

                        db.ClassFunction.funWriteRunLog ("結束" + shop_id);

                    }
                }


            }
        }



        #region 不用
        /*
        private void UPPOS_STATUS ( )
        {
            string apiurl = ConfigurationManager.AppSettings["apiurl"];
            string dbname = ConfigurationManager.AppSettings["dbname"];
            string shop_id = "";
            WebDB db = new WebDB ( );
            string mdbstr = ConfigurationManager.AppSettings["mdbstr"];
            MariaDB db_maria = new MariaDB (mdbstr);
            string strSql = "SELECT SHOP_ID,SHOP_DB_USER FROM TC_COMMON.SHOP";//是否要改資料庫

            //DataTable dt_shop = db.ClassDB.GetData (strSql);

            DataTable dt_shop = db_maria.GetData (strSql);

            if (dt_shop != null)
            {
                for (int s = 0; s < dt_shop.Rows.Count; s++)
                {
                    dbname = dt_shop.Rows[s]["SHOP_DB_USER"].ToString ( ) + ".";
                    shop_id = dt_shop.Rows[s]["SHOP_ID"].ToString ( );

                    if (dt_shop.Rows[s]["SHOP_DB_USER"].ToString ( ).Trim ( ) == "")
                        continue;

                    //大於日結日才執行
                    strSql = "select CLOSE_DATE from " + dbname + "day_close where FLAG = 'Y' order by CLOSE_DATE desc";
                    DataTable dt_close = db.ClassDB.GetData (strSql);//table是那張

                    if (dt_close != null && dt_close.Rows.Count > 0)
                    {
                        DateTime date_close = DateTime.Parse (dt_close.Rows[0]["CLOSE_DATE"].ToString ( ));
                        if (!(DateTime.Now.Date > date_close))
                            continue;
                    }
                    else
                        continue;

                    //取出api路徑
                    strSql = "SELECT * FROM " + dbname + "tc_sysprarm where PARAM_ID = 'API_URL'";//table那張
                    // DataTable dt_param = db.ClassDB.GetData (strSql);
                    DataTable dt_param = db_maria.GetData (strSql);

                    if (dt_param != null && dt_param.Rows.Count > 0)
                        apiurl = dt_param.Rows[0]["VALUE"].ToString ( );
                    else
                        continue;

                    string strReturn = GoOtherAPI ("{}", apiurl + "upload_posstatus");

                    db.ClassFunction.funWriteRunLog (strReturn);
                    try
                    {
                        apiresult_posstatus re_pos_status = Newtonsoft.Json.JsonConvert.DeserializeObject<apiresult_posstatus> (strReturn);
                        if (re_pos_status.rcrm.rc == "1")
                        {
                            //POS資料
                            strSql = "SELECT POS_ID,TPOS_YN,POS_MODULE FROM " + dbname + "pos";//無TPOS_YN POS_MODULE
                            //  DataTable dt_pos = db.ClassDB.GetData (strSql);
                            DataTable dt_pos = db_maria.GetData (strSql);

                            //當天 只有rm_nonetrans table 無Nonetrans
                            strSql = "SELECT POS_ID,ACTION_TYPE FROM " + dbname + "Nonetrans WHERE to_char(ACC_DATE,'yyyy/MM/dd') = '" + DateTime.Now.ToString ("yyyy/MM/dd") + "' and ACTION_TYPE in ('0','1')";
                           // DataTable dt = db.ClassDB.GetData (strSql);
                            DataTable dt = db_maria.GetData (strSql);

                            //前一天
                            strSql = "SELECT POS_ID,ACTION_TYPE FROM " + dbname + "Nonetrans WHERE to_char(ACC_DATE,'yyyy/MM/dd') = '" + DateTime.Now.AddDays (-1).ToString ("yyyy/MM/dd") + "' and ACTION_TYPE in ('0','1')";
                            // DataTable dt_yday = db.ClassDB.GetData (strSql);
                            DataTable dt_yday = db_maria.GetData (strSql);

                            for (int p = 0; p < re_pos_status.results.pos_status.ToList ( ).Count; p++)
                            {
                                pos_status ps = re_pos_status.results.pos_status[p];

                                //POS機的類型
                                string strStatus = "S";
                                DataRow[ ] dra_pos = dt_pos.Select ("POS_ID = '" + ps.strtillcode + "' AND POS_MODULE = '2'");
                                if (dra_pos.Length > 0)
                                    strStatus = "M";

                                //當天資料
                                if (dt != null)
                                {
                                    string strdate = DateTime.Now.ToString ("yyyy/MM/dd");

                                    db.ClassFunction.funWriteLog (ps.strstatus);
                                    //檢查是否存過開機
                                    if (ps.strstatus == "O" || ps.strstatus == "C")
                                    {
                                        DataRow[ ] dra = dt.Select ("POS_ID = '" + ps.strtillcode + "' and ACTION_TYPE = '0'");

                                        if (dra.Length == 0)
                                        {
                                            strSql = @"INSERT INTO " + dbname + @"Nonetrans (POS_ID, TRANS_NO, ACC_DATE, ACC_TIME, ACTION_TYPE, NOTE, EVENT, STATUS, SHOP_ID)
                                                    VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')";

                                            strSql = string.Format (strSql, ps.strtillcode, "", strdate, DateTime.Now.ToString ("HHmm"), "0", "開機", "10000B", strStatus, shop_id);
                                            //db.ClassDB.UpdData (strSql);
                                            db_maria.UpdData (strSql);

                                        }
                                    }

                                    else if (ps.strstatus == "F")
                                    {
                                        DataRow[ ] dra = dt.Select ("POS_ID = '" + ps.strtillcode + "' and ACTION_TYPE = '0'");
                                        DataRow[ ] dra2 = dt.Select ("POS_ID = '" + ps.strtillcode + "' and ACTION_TYPE = '1'");
                                        if (dra2.Length == 0 && dra.Length > 0)
                                        {
                                            strSql = @"INSERT INTO " + dbname + @"Nonetrans (POS_ID, TRANS_NO, ACC_DATE, ACC_TIME, ACTION_TYPE, NOTE, EVENT, STATUS, SHOP_ID)                                          VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')";

                                            strSql = string.Format (strSql, ps.strtillcode, "", strdate, DateTime.Now.ToString ("HHmm"), "1", "關機", "10000S", "S", shop_id);
                                            // db.ClassDB.UpdData (strSql);
                                            db_maria.UpdData (strSql);

                                            //CLEARN_REPORT 無table
                                            strSql = @"INSERT INTO " + dbname + @"CLEARN_REPORT (POS_ID, CASHER_ID, JOB_ID,  EVENT, SALES_QTY, SALES_AMT, ACC_DATE,CUSER,CTIME,BUSER, BTIME, REC_TIME, CLEARN_TYPE,SHOP_ID)
                              VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',SYSDATE,'{10}','{11}','{12}')";

                                            strSql = string.Format (strSql, ps.strtillcode, " ", "1", "14001A", "0", "0", strdate, "", "", "TC", DateTime.Now.ToString ("HHmm"), "", shop_id);

                                            //db.ClassDB.UpdData (strSql);
                                            db_maria.UpdData (strSql);

                                            //CLEARN_REPORT
                                            strSql = @"INSERT INTO " + dbname + @"CLEARN_REPORT (POS_ID, CASHER_ID, JOB_ID,  EVENT, SALES_QTY, SALES_AMT, ACC_DATE,CUSER,CTIME,BUSER, BTIME, REC_TIME, CLEARN_TYPE,SHOP_ID)
                                                                                        VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',SYSDATE,'{10}','{11}','{12}')";

                                            strSql = string.Format (strSql, ps.strtillcode, " ", "1", "1TIME_", "0", "0", strdate, "", "", "TC", DateTime.Now.ToString ("HHmm"), "", shop_id);

                                            // db.ClassDB.UpdData (strSql);
                                            db_maria.UpdData (strSql);

                                            //更新時間
                                            strSql = @"UPDATE " + dbname + @"CLEARN_REPORT C SET (REC_TIME) =
                                            (SELECT ACC_TIME FROM " + dbname + @"NONETRANS N WHERE N.ACC_DATE = C.ACC_DATE
                                              AND N.POS_ID = C.POS_ID
                                              AND N.ACTION_TYPE = '1'
                                              AND N.ACC_DATE = '" + strdate + @"' )              
                                              WHERE EXISTS (SELECT ACC_TIME  FROM " + dbname + @"NONETRANS N
                                              WHERE N.ACC_DATE = C.ACC_DATE  AND N.POS_ID = C.POS_ID AND N.ACTION_TYPE = '1'
                                              AND N.ACC_DATE = '" + strdate + @"'   )
                                              AND C.EVENT NOT LIKE '%TIME_'";
                                            // db.ClassDB.UpdData (strSql);
                                            db_maria.UpdData (strSql);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    catch
                    { }
                }
            }
        }
        */
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
                webrequest.Proxy = null;
                webrequest.Method = WebRequestMethods.Http.Post;
                webrequest.ContentType = "application/json";
                webrequest.ContentLength = jsonBytes.Length;

                using (var requestStream = webrequest.GetRequestStream ( ))
                {
                    requestStream.Write (jsonBytes, 0, jsonBytes.Length);
                    requestStream.Flush ( );
                    requestStream.Close ( );
                }
                #region 寫入log
                funWriteLog ("\r\n" + "JSON格式:" + request + "\r\n" + "URL:" + URL + "\r\n");
                #endregion

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
                #region 寫入log
                funWriteLog ("\r\n" + ex.Message + "\r\n" + "JSON格式=" + request + "\r\n" + "URL=" + URL + "\r\n");
                #endregion
                return ex.Message;
            }



        }

        #region
        /// <summary>
        /// 寫入MariaDBLog
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public static void funInsertLog ( string strLog )
        {
            string strPathDBLog = @"bin\IsertMariaDbLog";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, strPathDBLog, DateTime.Today.ToString ("yyyy"), DateTime.Today.ToString ("MM"));
            funAutoDirectory (strPath);
            strPath = Path.Combine (strPath, String.Format ("IsertMariaDbLog_{0}.log", DateTime.Today.ToString ("yyyyMMdd")));

            try
            {
                using (StreamWriter sw = new StreamWriter (strPath, true))
                {
                    string strMessage = DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss") + ">" + strLog;
                    Console.WriteLine (strMessage);
                    sw.WriteLine (strMessage);
                }
            }
            catch { }

        }

        #endregion

        #region 呼叫執行外部API 寫入LOG
        /// <summary>
        /// 寫入Log(執行外部API)
        /// </summary>
        /// <param name="strLog">LOG內容</param>
        public static void funWriteLog ( string strLog )
        {
            string strPathDBLog = @"bin\OutAPILog";
            //1.14.2.12 Log記錄區分年月
            string strPath = Path.Combine (System.AppDomain.CurrentDomain.BaseDirectory, strPathDBLog, DateTime.Today.ToString ("yyyy"), DateTime.Today.ToString ("MM"));
            funAutoDirectory (strPath);
            strPath = Path.Combine (strPath, String.Format ("OutAPILog_{0}.log", DateTime.Today.ToString ("yyyyMMdd")));

            try
            {
                using (StreamWriter sw = new StreamWriter (strPath, true))
                {
                    string strMessage = DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss") + ">" + strLog;
                    Console.WriteLine (strMessage);
                    sw.WriteLine (strMessage);
                }
            }
            catch { }

        }

        /// <summary>
        /// 自動建立資料夾
        /// </summary>
        /// <param name="sFolderName"></param>
        public static DirectoryInfo funAutoDirectory ( string sFolderName )
        {
            if (!Directory.Exists (sFolderName))
            {
                return Directory.CreateDirectory (sFolderName);
            }
            else
            {
                return new DirectoryInfo (sFolderName);
            }
        }
        #endregion

    }


}

