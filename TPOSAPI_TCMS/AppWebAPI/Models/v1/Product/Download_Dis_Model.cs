﻿using System;
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
    public class Download_Dis_Model : _Product_Model<Download_Dis_Request, Download_Dis_Result>
    {
        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Download_Dis_Model ( ) { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Download_Dis_Model ( Download_Dis_Request request ) : base (request) { }

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
            discount_detail[ ] Lcd = request.discount_detail.OrderBy (rd => rd.ysnactive).ToArray<discount_detail> ( );

            funWriteLog ("Item數量" + Lcd.Length.ToString ( ), "", "");

            strSql = string.Format (@" SELECT * FROM {0}Item", DB_Service);

            DataTable dt_item = null;

            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_item = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_item = postaredb.GetData (strSql);

            #endregion

            #endregion


            if (dt_item == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }

            int maxnoc = 1;

            // Cost
            strSql = string.Format (@" SELECT  * FROM {0}Cost order by intCostNo desc ", DB_Service);


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
                maxnoc = int.Parse (dt.Rows[0]["intCostNo"].ToString ( )) + 1;

            int maxnop = 1;

            // Price
            strSql = string.Format (@" SELECT * FROM {0}Price order by intPriceNo desc ", DB_Service);


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
                maxnop = int.Parse (dt.Rows[0]["intPriceNo"].ToString ( )) + 1;


            //Class
            strSql = string.Format (@" SELECT  * FROM {0}ClassifyConsolidation ", DB_Service);

            DataTable dt_class = null;

            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_class = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_class = postaredb.GetData (strSql);

            #endregion

            #endregion

            if (dt_class == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }


            //Classify1
            strSql = string.Format (@" SELECT * FROM {0}Classify1 ", DB_Service);

            DataTable dt_class1 = null;

            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_class1 = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_class1 = postaredb.GetData (strSql);

            #endregion

            #endregion


            if (dt_class1 == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }


            //Classify2
            strSql = string.Format (@" SELECT * FROM {0}Classify2 ", DB_Service);

            DataTable dt_class2 = null;

            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_class2 = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_class2 = postaredb.GetData (strSql);

            #endregion

            #endregion

            if (dt_class2 == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }


            //Classify3
            strSql = string.Format (@" SELECT * FROM {0}Classify3 ", DB_Service);

            DataTable dt_class3 = null;

            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_class3 = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_class3 = postaredb.GetData (strSql);

            #endregion

            #endregion

            if (dt_class3 == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }


            //Classify4
            strSql = string.Format (@" SELECT * FROM {0}Classify4 ", DB_Service);

            DataTable dt_class4 = null;

            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_class4 = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_class4 = postaredb.GetData (strSql);

            #endregion

            #endregion

            if (dt_class4 == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }



            // NEXTNO
            strSql = string.Format (@" SELECT * FROM {0}NEXTNO order by STRCODE desc ", DB_Service);

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



            // update 
            for (int i = 0; i < Lcd.Length; i++)
            {
                // string keyno = "9" + Lcd[ i ].intitemno + int.Parse ( Lcd[ i ].strextsref1 ).ToString ( );
                string keyno = Lcd[i].intitemno;
                DataRow[ ] dra = dt_item.Select ("strEXTSRef1 = '" + keyno + "'");

                if (dra.Length > 0)
                {
                    strSql = "update {0}Item set strItemNamePOS = '{1}' ,ysnActive = '{2}', strSaletaxcode = '{3}', strPurchasetaxcode='{5}', stritemnameshort='{1}', stritemnamebo='{1}'  where intItemNo = '{4}' ";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            Lcd[i].stritemnamepos,
                                            Lcd[i].ysnactive,
                                            Lcd[i].strTax,
                                            keyno, Lcd[i].strLprc_ax);

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

                    #region Item
                    //Item
                    strSql = @"insert into {0}Item (intItemNo, strItemNamePOS, ysnActive, ysnRecipeItem, ysnSetMeal, ysnTrackSerialNo, ysnChargeService, ysnDiscountable, 
                                            ysnKeepStockCount, strMinUnit, ysnAskComment, strClassify1Code, strClassify2Code, strClassify3Code, strClassify4Code, strEXTSRef1,ysnAskPriceIfZero, strSaletaxcode, strPurchasetaxcode, stritemnameshort, stritemnamebo )
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}','{17}','{18}','{19}', '{2}', '{2}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            keyno,
                                            Lcd[i].stritemnamepos,
                                            Lcd[i].ysnactive,
                                            "F",
                                            "F",
                                            "F",
                                            "F",
                                            "T",
                                            "F",
                                            "1",
                                            "F",
                                            Lcd[i].strclassify1code, Lcd[i].strclassify2code, Lcd[i].strclassify3code,
                                            Lcd[i].strclassify4code, keyno, "T", Lcd[i].strTax, Lcd[i].strLprc_ax);

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
                    #endregion

                    #region Barcode
                    strSql = @"insert into {0}Barcode (intItemNo, strBarcode, strEXTSRef1)
                                values ('{1}', '{2}', '{3}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            keyno,
                                            Lcd[i].strBarcode,
                                            keyno);

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
                    #endregion

                }

                #region Cost

                strSql = @" SELECT  * FROM {0}Cost order by intCostNo desc where strStoreCode='{1}' and intItemNo='{2}'";
                strSql = string.Format (strSql, DB_Service, Lcd[i].strstorecode, keyno);
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

                //update
                if (dt.Rows.Count > 0)
                {
                    strSql = @"update {0}Cost set strSupplierCode='{1}', ysnActive='{3}', dtmEffective='{4}', intMinOrder='{5}', intCartonSize='{6}', curExtaxCost='{7}', curCost='{8}')
                               where strStoreCode='{9}' and intItemNo='{2}'";
                    strSql = String.Format (strSql, DB_Service, "9999", keyno,
                                            "T", DateTime.Now.ToString ("yyyy/MM/dd"),
                                            "1", "1", "0", "0",
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
                else
                {
                    strSql = @"insert into {0}Cost (intCostNo, strSupplierCode, strStoreCode, intItemNo, ysnActive,dtmEffective,intMinOrder,intCartonSize,curExtaxCost,curCost)
                                values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            maxnoc,
                                            "9999",
                                            Lcd[i].strstorecode,
                                            keyno,
                                            "T",
                                            DateTime.Now.ToString ("yyyy/MM/dd"),
                                            "1",
                                            "1",
                                            "0",
                                            "0");

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
                    #endregion

                }

                #region Price

                strSql = @" SELECT  * FROM {0}Price order by intPriceNo desc where strStoreCode='{1}' and intItemNo='{2}'";
                strSql = string.Format (strSql, DB_Service, Lcd[i].strstorecode, keyno);

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

                if (dt.Rows.Count > 0)
                {
                    strSql = @" update {0}Price set  ysnActive='{3}', dtmEffective='{4}' where strStoreCode='{1}' and intItemNo='{2}'";
                    strSql = string.Format (strSql, DB_Service, Lcd[i].strstorecode, keyno,
                                             "T", DateTime.Now.ToString ("yyyy/MM/dd"));
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
                    strSql = @"insert into {0}Price (intPriceNo, strStoreCode, intItemNo, ysnActive, dtmEffective,curPrice, intCostno) values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            maxnop,
                                            Lcd[i].strstorecode,
                                            keyno,
                                            "T",
                                            DateTime.Now.ToString ("yyyy/MM/dd"),
                                            Lcd[i].strcurPrice, maxnoc);


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

                    maxnoc += 1;
                    maxnop += 1;
                }

                    #endregion


                #region StoreStocks

                strSql = @"select * from {0}StoreStock where strStoreCode='{1}' and intItemNo='{2}')";
                strSql = string.Format (strSql, DB_Service,  Lcd[i].strstorecode, keyno );
                
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

                if (dt.Rows.Count > 0)
                {
                    strSql = @"update {0}StoreStock set dblSOH='{1}', curAvgCost='{2}' where  strStoreCode='{3}' and intItemNo='{4}'";                            
                    strSql = string.Format (strSql,
                                            DB_Service,
                                            "0", "0",
                                            Lcd[i].strstorecode, keyno);
                }
                else { 
                strSql = @"insert into {0}StoreStock (strStoreCode, intItemNo, dblSOH, curAvgCost)
                                values ('{1}', '{2}', '{3}', '{4}')";
                strSql = string.Format (strSql,
                                        DB_Service,
                                        Lcd[i].strstorecode,
                                        keyno,
                                        "0",
                                        "0");

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
                #endregion

                }




                #region CLASS  ClassifyConsolidation

                DataRow[ ] dra_class = dt_class.Select ("strClassify1Code='" + Lcd[i].strclassify1code + "' and strClassify2Code='" + Lcd[i].strclassify2code + "' and strClassify3Code='" + Lcd[i].strclassify3code + "' and strClassify4Code='" + Lcd[i].strclassify4code + "'");
                if (dra_class.Length == 0)
                {
                    strSql = @" select * from {0}ClassifyConsolidation  where strClassify1Code='{1}' and strClassify2Code='{2}' and strClassify3Code='{3}'";
                    strSql = string.Format (strSql,
                                           DB_Service,
                                           Lcd[i].strclassify1code,
                                           Lcd[i].strclassify2code,
                                           Lcd[i].strclassify3code);

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

                    // 判斷分類是否已存在
                    if (dt.Rows.Count != 1)
                    {

                        strSql = @"insert into {0}ClassifyConsolidation (strClassify1Code, strClassify2Code, strClassify3Code, strClassify4Code)
                                values ('{1}', '{2}', '{3}', '{4}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strclassify1code,
                                                Lcd[i].strclassify2code,
                                                Lcd[i].strclassify3code,
                                                Lcd[i].strclassify4code);

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
                #endregion

                #region CLASS1

                DataRow[ ] dra_class1 = dt_class1.Select ("strClassify1Code='" + Lcd[i].strclassify1code + "'");
                if (dra_class1.Length == 0)
                {
                    strSql = @" select * from {0}Classify1  where strClassify1Code='{1}' ";
                    strSql = string.Format (strSql,
                                           DB_Service,
                                           Lcd[i].strclassify1code);

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

                    // 判斷分類是否已存在
                    if (dt.Rows.Count != 1)
                    {
                        strSql = @"insert into {0}Classify1 (strClassify1Code, strClassify1Name)
                                values ('{1}', '{2}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strclassify1code,
                                                Lcd[i].strCategory1_Name);


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
                #endregion

                #region  CLASS2

                DataRow[ ] dra_class2 = dt_class2.Select ("strClassify2Code='" + Lcd[i].strclassify2code + "'");
                if (dra_class2.Length == 0)
                {
                    strSql = @" select * from {0}Classify2  where strClassify2Code='{1}' ";
                    strSql = string.Format (strSql,
                                           DB_Service,
                                           Lcd[i].strclassify2code);

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

                    // 判斷分類是否已存在
                    if (dt.Rows.Count != 1)
                    {
                        strSql = @"insert into {0}Classify2 (strClassify2Code, strClassify2Name)
                                values ('{1}', '{2}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strclassify2code,
                                                Lcd[i].strCategory2_Name);


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
                #endregion

                #region CLASS3

                DataRow[ ] dra_class3 = dt_class3.Select ("strClassify3Code='" + Lcd[i].strclassify3code + "'");
                if (dra_class3.Length == 0)
                {

                    strSql = @" select * from {0}Classify3  where strclassify3code='{1}' ";
                    strSql = string.Format (strSql,
                                           DB_Service,
                                           Lcd[i].strclassify3code);

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

                    // 判斷分類是否已存在
                    if (dt.Rows.Count != 1)
                    {
                        strSql = @"insert into {0}Classify3 (strClassify3Code, strClassify3Name)
                                values ('{1}', '{2}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strclassify3code,
                                                Lcd[i].strCategory3_Name);


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
                #endregion

                #region CLASS4 不跑回圈只放一筆

                DataRow[ ] dra_class4 = dt_class4.Select ("strClassify4Code='" + Lcd[i].strclassify4code + "'");
                if (dra_class4.Length == 0)
                {
                    if (Lcd.Length - 1 == i)
                    {
                        strSql = @"insert into {0}Classify4 (strClassify4Code, strClassify4Name)
                                values ('{1}', '{2}')";
                        strSql = string.Format (strSql,
                                                DB_Service,
                                                Lcd[i].strclassify4code,
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
                }
                #endregion


            }

            #region NEXTNO

            int max = 0;
            int max_n = 0;
            DataTable dt_Next = null;

            //Item
            strSql = string.Format (@" SELECT max(intitemno) FROM {0}Item", DB_Service);

            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_item = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_item = postaredb.GetData (strSql);

            #endregion

            #endregion


            if (dt_item == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }

            foreach (DataRow dr in dt_item.Rows)
            {
                max = int.Parse (dr["max"].ToString ( ));
            }

            strSql = @"SELECT lintvalue FROM {0}nextno Where strcode = 'Item'";
            strSql = string.Format (strSql, DB_Service);


            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_Next = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_Next = postaredb.GetData (strSql);
            #endregion
            #endregion
            if (dt_Next == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }
            foreach (DataRow dr in dt_Next.Rows)
            {
                max_n = int.Parse (dr["lintvalue"].ToString ( ));
            }
            if (max_n == 0)
            {
                strSql = @"INSERT INTO {0}nextno ( strcode, lintvalue ) VALUES ('{1}', '{2}')";
                strSql = string.Format (strSql, DB_Service, "Item", max + 1);

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
                if ((max + 1) > max_n)
                {
                    strSql = @"UPDADE {0}nextno SET lintvalue={2} WHERE strcode={1}";
                    strSql = string.Format (strSql, DB_Service, "Item", max + 1);

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


            // Cost
            strSql = string.Format (@" SELECT  max(intCostNo) FROM {0}Cost ", DB_Service);


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

            foreach (DataRow dr in dt.Rows)
            {
                max = int.Parse (dr["max"].ToString ( ));
            }

            strSql = @"SELECT lintvalue FROM {0}nextno Where strcode = 'Cost'";
            strSql = string.Format (strSql, DB_Service);


            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_Next = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_Next = postaredb.GetData (strSql);
            #endregion
            #endregion
            if (dt_Next == null)
            {
                results = null;
                rcrm = new RCRM (RC_Enum.FAIL_401_0099);
                throw new Exception ("sql err");
            }
            foreach (DataRow dr in dt_Next.Rows)
            {
                max_n = int.Parse (dr["lintvalue"].ToString ( ));
            }
            if (max_n == 0)
            {
                strSql = @"INSERT INTO {0}nextno ( strcode, lintvalue ) VALUES ('{1}', '{2}')";
                strSql = string.Format (strSql, DB_Service, "Cost", max + 1);

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
                if ((max + 1) > max_n)
                {
                    strSql = @"UPDATE {0}nextno SET lintvalue='{2}' where strcode='{1}'";
                    strSql = string.Format (strSql, DB_Service, "Cost", max + 1);

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



            // Price
            strSql = string.Format (@" SELECT  max(intPriceNo) FROM {0}Price ", DB_Service);

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

            foreach (DataRow dr in dt.Rows)
            {
                max = int.Parse (dr["max"].ToString ( ));
            }
            strSql = @"SELECT lintvalue FROM {0}nextno Where strcode = 'Price'";
            strSql = string.Format (strSql, DB_Service);


            #region 判斷使用DB

            #region oracledb
            if (!string.IsNullOrEmpty (oracledb))
                dt_Next = db.ClassDB.GetData (strSql);
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                dt_Next = postaredb.GetData (strSql);
            #endregion
            #endregion

            foreach (DataRow dr in dt_Next.Rows)
            {
                max_n = int.Parse (dr["lintvalue"].ToString ( ));
            }
            if (max_n == 0)
            {
                strSql = @"INSERT INTO {0}nextno ( strcode, lintvalue ) VALUES ('{1}', '{2}')";
                strSql = string.Format (strSql, DB_Service, "Price", max + 1);

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
                if ((max + 1) > max_n)
                {
                    strSql = @"UPDATE {0}nextno SET  lintvalue='{2}' WHERE strcode='{1}'";
                    strSql = string.Format (strSql, DB_Service, "Price", max + 1);

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
            var json = new System.Web.Script.Serialization.JavaScriptSerializer ( );
            funWriteLog (json.Serialize (request), "", "");
        }
    }
}