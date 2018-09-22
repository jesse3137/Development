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
    public class Download_SpeedKey_Model : _Product_Model<Download_Dis_Request, Download_Dis_Result>
    {
        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Download_SpeedKey_Model ( ) { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Download_SpeedKey_Model ( Download_Dis_Request request ) : base (request) { }

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
            discount_detail[ ] Lcd = request.discount_detail.ToArray<discount_detail> ( );

            strSql = "DELETE FROM POSFASTKEY_KEY ";

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

            strSql = "DELETE FROM PosFastKey_Page ";

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

            strSql = "DELETE FROM PosFastKey ";

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



            //排序一下方便填順序
            List<discount_detail> L_dd = Lcd.OrderBy (rd => rd.intitemno).ThenBy (rd => rd.strextsref1).ToList ( );

            int i_page = 1;
            int i_key = 1;
            string l_intitemno = "";
            int l_page = 1;
            for (int i = 0; i < L_dd.Count; i++)
            {
                if (L_dd[i].ysnactive == "F")
                    continue;

                //換專櫃時將前一專櫃快速鍵補滿36個
                if (i_key != 1 && l_intitemno != L_dd[i].intitemno)
                {
                    for (int k = i_key; k <= 32; k++)
                    {
                        //POSFASTKEY_KEY
                        strSql = @"insert into {0}POSFASTKEY_KEY (intPosFastKeyNo, strPageCode, intKeyOrder, intItemNo, STRTEXT)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                               "1",
                                                i_page.ToString ( ).PadLeft (2, '0'),
                                                k.ToString ( ),
                                                "",
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

                    i_key = 1;
                    i_page = 1;

                    for (int t = 1; t <= l_page; t++)
                    {
                        //33
                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                                values ('{1}', '{2}', '33','','<<','01','15')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                (t).ToString ( ).PadLeft (2, '0'));

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


                        //34
                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                                values ('{1}', '{2}', '34','','<','{3}','15')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                (t).ToString ( ).PadLeft (2, '0'),
                                                (t) <= 2 ? "01" : (t - 1).ToString ( ).PadLeft (2, '0'));

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


                        //35
                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                                values ('{1}', '{2}', '35','','>','{3}','15')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                (t).ToString ( ).PadLeft (2, '0'),
                                                l_page == t ? (l_page).ToString ( ).PadLeft (2, '0') : (t + 1).ToString ( ).PadLeft (2, '0'));

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


                        //36
                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                                values ('{1}', '{2}', '36','','>>','{3}','15')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                (t).ToString ( ).PadLeft (2, '0'),
                                                (l_page).ToString ( ).PadLeft (2, '0'));

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

                //                    //換頁但還沒換專櫃時，前一頁要補上下頁功能
                //                    if (i_key == 1 && i_page != 1)
                //                    {
                //                        //33
                //                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                //                                values ('{1}', '{2}', '33','','<<','01','16')";
                //                        strSql = string.Format(strSql,
                //                                                DB_Service,
                //                                                "9" + l_intitemno,
                //                                                (i_page - 1).ToString().PadLeft(2, '0'));
                //                        db.ClassDB.UpdData(strSql);

                //                        //34
                //                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                //                                values ('{1}', '{2}', '34','','<','{3}','16')";
                //                        strSql = string.Format(strSql,
                //                                                DB_Service,
                //                                                "9" + l_intitemno,
                //                                                (i_page - 1).ToString().PadLeft(2, '0'),
                //                                                (i_page - 1) <= 2 ? "01" : (i_page - 2).ToString().PadLeft(2, '0'));
                //                        db.ClassDB.UpdData(strSql);

                //                        //35
                //                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                //                                values ('{1}', '{2}', '35','','>','{3}','16')";
                //                        strSql = string.Format(strSql,
                //                                                DB_Service,
                //                                                "9" + l_intitemno,
                //                                                (i_page - 1).ToString().PadLeft(2, '0'),
                //                                                (i_page).ToString().PadLeft(2, '0'));
                //                        db.ClassDB.UpdData(strSql);

                //                        //36
                //                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                //                                values ('{1}', '{2}', '36','','>>','{3}','16')";
                //                        strSql = string.Format(strSql,
                //                                                DB_Service,
                //                                                "9" + l_intitemno,
                //                                                (i_page - 1).ToString().PadLeft(2, '0'),
                //                                                (i_page).ToString().PadLeft(2, '0'));
                //                        db.ClassDB.UpdData(strSql);

                //                        //更新跳到最後一頁頁碼
                //                        strSql = @"update {0}POSFASTKEY_KEY SET strLinkPageCode = '{2}'
                //                                where INTPOSFASTKEYNO = '{1}' and INTKEYORDER = '36'";
                //                        strSql = string.Format(strSql,
                //                                                DB_Service,
                //                                                "9" + l_intitemno,
                //                                                (i_page).ToString().PadLeft(2, '0'));
                //                        db.ClassDB.UpdData(strSql);
                //                    }

                //string keyno = "9" + L_dd[i].intitemno + int.Parse(L_dd[i].strextsref1).ToString();
                string keyno = L_dd[i].intitemno;
                if (i_page == 1 && i_key == 1)
                {
                    //PosFastKey
                    strSql = @"insert into {0}PosFastKey (intPosFastKeyNo, strPosFastKeyName, dtmCreate)
                                                values ('{1}', '{2}', to_char(SYSDATE,'yyyy/MM/dd'))";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            "1",
                                            "9" + L_dd[i].intitemno
                                            );

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

                //POSFASTKEY_KEY
                strSql = @"insert into {0}POSFASTKEY_KEY (intPosFastKeyNo, strPageCode, intKeyOrder, intItemNo, STRTEXT)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}')";
                strSql = string.Format (strSql,
                                        DB_Service,
                                        "1",
                                        i_page.ToString ( ).PadLeft (2, '0'),
                                        i_key.ToString ( ),
                                        keyno,
                                        L_dd[i].stritemnamepos.Length > 8 ? L_dd[i].stritemnamepos.Substring (0, 8) : L_dd[i].stritemnamepos);

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


                if (i_key == 1)
                {
                    //PosFastKey_Page
                    strSql = @"insert into {0}PosFastKey_Page (intPosFastKeyNo, strPageCode, ysnDEFAULTPAGE, strPageName)
                                values ('{1}', '{2}', '{3}', '{4}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            "1",
                                            i_page.ToString ( ).PadLeft (2, '0'),
                                            i_page == 1 ? "T" : "F",
                                            i_page.ToString ( ).PadLeft (2, '0'));

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

                if (i_key == 32)
                {
                    i_page += 1;
                    i_key = 1;
                }
                else
                {
                    i_key += 1;
                }

                //存專櫃
                l_intitemno = L_dd[i].intitemno;
                l_page = i_page;

                //最後一筆時將專櫃快速鍵補滿36個
                if (i == L_dd.Count - 1 && i_key != 1)
                {
                    for (int k = i_key; k <= 32; k++)
                    {
                        //POSFASTKEY_KEY
                        strSql = @"insert into {0}POSFASTKEY_KEY (intPosFastKeyNo, strPageCode, intKeyOrder, intItemNo, STRTEXT)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                i_page.ToString ( ).PadLeft (2, '0'),
                                                k.ToString ( ),
                                                "",
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

                    for (int t = 1; t <= l_page; t++)
                    {
                        //33
                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                                values ('{1}', '{2}', '33','','<<','01','15')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                (t).ToString ( ).PadLeft (2, '0'));

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


                        //34
                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                                values ('{1}', '{2}', '34','','<','{3}','15')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                (t).ToString ( ).PadLeft (2, '0'),
                                                (t) <= 2 ? "01" : (t - 1).ToString ( ).PadLeft (2, '0'));

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


                        //35
                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                                values ('{1}', '{2}', '35','','>','{3}','15')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                (t).ToString ( ).PadLeft (2, '0'),
                                                l_page == t ? (l_page).ToString ( ).PadLeft (2, '0') : (t + 1).ToString ( ).PadLeft (2, '0'));

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


                        //36
                        strSql = @"insert into {0}POSFASTKEY_KEY(INTPOSFASTKEYNO,STRPAGECODE,INTKEYORDER,INTITEMNO,STRTEXT,strLinkPageCode, intFontSize)
                                values ('{1}', '{2}', '36','','>>','{3}','15')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                "1",
                                                (t).ToString ( ).PadLeft (2, '0'),
                                                (l_page).ToString ( ).PadLeft (2, '0'));

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
            }

            strSql = @"update {0}POSFASTKEY_KEY set STRBACKGROUNDCOLOR = 'RGB(109,181,209)' where strText is not null";
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


            strSql = @"update {0}POSFASTKEY_KEY set STRFONTCOLOR = 'RGB(255,255,255)' where strText is not null";
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


            strSql = @"update {0}POSFASTKEY_KEY set INTFONTSIZE = '16'";
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


            results = new Download_Dis_Result ( );
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