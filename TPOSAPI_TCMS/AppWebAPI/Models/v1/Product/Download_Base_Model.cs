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
    public class Download_Base_Model : _Product_Model<Download_Base_Request, Download_Base_Result>
    {
        /// <summary>
        /// 權限控管-登入
        /// </summary>
        public Download_Base_Model() { }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">查詢條件</param>
        public Download_Base_Model(Download_Base_Request request) : base(request) { }

        /// <summary>
        /// 驗證資料
        /// </summary>
        /// <returns></returns>
        protected override bool Verify_Request()
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
        protected override void GetResults()
        {
           
            #region 收銀機功能鍵設定
           /*  strSql = @"insert into {0}PosFuncKey (intPosFuncKeyNo
                                                    ,strPosFuncKeyName
                                                    ,ysnHaveReturn
                                                    ,ysnHaveMember
                                                    ,ysnHaveReprint
                                                    ,ysnHaveOrder
                                                    ,ysnHaveHold
                                                    ,ysnHaveTable
                                                    ,ysnHaveReceipt
                                                    ,ysnHaveXZRead
                                                    ,ysnHaveTrainingMode
                                                    ,ysnHavePayInPayOut
                                                    ,ysnHaveItemDiscount
                                                    ,ysnHaveSaleDiscount
                                                    ,ysnHaveChangePrice
                                                    ,ysnHaveOpenCD
                                                    ,ysnHaveSaleComment
                                                    ,ysnHaveItemComment
                                                    ,ysnHavePay
                                                    ,ysnHaveQueryPrice
                                                    ,ysnHaveHistory
                                                    ,ysnHaveQueryBulletin
                                                    ,ysnHaveExitPOS
                                                    ,ysnHaveCloneSale
                                                    )
                                            values ('1'
                                                    ,'TC10'
                                                    ,'T'
                                                    ,''
                                                    ,'T'
                                                    ,''
                                                    ,'T'
                                                    ,''
                                                    ,'T'
                                                    ,'T'
                                                    ,'T'
                                                    ,'T'
                                                    ,'T'
                                                    ,'T'
                                                    ,'T'
                                                    ,'T'
                                                    ,''
                                                    ,''
                                                    ,'T'
                                                    ,''
                                                    ,'T'
                                                    ,'T'
                                                    ,'F'
                                                    ,'F')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region 收銀機側寫設定
            strSql = @"insert into {0}TillProfile (intTillProfileNo
                                                    ,strRoundingType
                                                    ,intRoundingDecimal
                                                    ,strCountryCode
                                                    ,strBuType
                                                    ,strPOSReportFontName
                                                    ,strPOSUIFontName
                                                    ,strTradeDateMode
                                                    ,strTradeSwitchTime
                                                    ,ysnEnableReceipt
                                                    ,intReceiptPagesPerRoll
                                                    ,intReceiptLinesPerPage
                                                    ,intReceiptFirstLine
                                                    ,intReceiptLastLine
                                                    ,intReceiptCharsPerLine
                                                    ,intReceiptNoPrefixFixLen
                                                    ,intReceiptNoFixLen
                                                    ,memReceiptHeader
                                                    ,memReceiptFooter
                                                    ,ysnAskLogonPassword
                                                    ,ysnPrintSaleComment
                                                    ,ysnPrintInSale
                                                    ,ysnPrintInReturn
                                                    ,ysnPrintInOrder
                                                    ,ysnPrintInFOrder
                                                    ,ysnPrintInCLOrder
                                                    ,ysnPrintIfZeroSale
                                                    ,strDollarSymbol
                                                    ,strTaxSymbol
                                                    ,ysnAskPriceIfZero
                                                    ,ysnCheckDailyReload
                                                    ,ysnAskPartialReturn
                                                    ,ysnZReadClearHold
                                                    ,intPosTransKeepDays
                                                    ,ysnReturnPaymentMustEqualSale
                                                    ,ysnReturnRemoteCheck
                                                    ,strCustDisplayLine1
                                                    ,strCustDisplayLine2
                                                    ,ysnBRcpUseShortName
                                                    ,strCVolChkRule
                                                    ,ysnKeepComOpen
                                                    ,intLogKeepDays
                                                    ,ysnUploadLog
                                                    ,ysnLogInfo
                                                    ,ysnLogError
                                                    ,ysnLogWarning
                                                    ,ysnLogDebug
                                                    ,ysnSyncTime
                                                    ,ysnCalServiceFee
                                                    ,ysnAutoPrtRcpAftPayment
                                                    ,ysnPrintZReceipt
                                                    ,ysnZRHasDisc
                                                    ,ysnZRHasInvInfo
                                                    ,ysnZRHasClassifyInfo
                                                    ,ysnZRHasTableStat
                                                    ,ysnZRHasInvList
                                                    ,ysnZRInvListHasPmtDtl
                                                    ,intReceiptCopies
                                                    ,strReceiptCopiesNameList
                                                    ,intXZCharsPerLine
                                                    ,strXZPrintBy
                                                    ,intXZPreviewFontSize
                                                    ,intReceiptLastCopySpecDay
                                                    ,intZRDefPrtCopies
                                                    ,strTradeSwitchBeginTime
                                                    ,strActionAfterZRead
                                                    ,strActionAfterExitPos
                                                    ,INTTRANSDONESCREENSTAYSEC
                                                    )
                                                    values ('1'
                                                            ,'RoundOff'
                                                            ,'2'
                                                            ,'CN'
                                                            ,'M'
                                                            ,'細明體'
                                                            ,'微軟正黑體'
                                                            ,'O'
                                                            ,'03:00'
                                                            ,'T'
                                                            ,'-1'
                                                            ,'9999'
                                                            ,'1'
                                                            ,'9999'
                                                            ,'32'
                                                            ,''
                                                            ,''
                                                            ,'<LoGo>
租戶:<StoreName>(<StoreCode>)
收銀機號:<TillCode> 交易序號:<PosTransNo>
收銀員:<UserCode>
入帳日期:<TradeDate>
日期時間:<DateTime>
================================'
                                                            ,'--------------------------------
  THANK YOU ,HAVE A NICE DAY

      永旺夢樂城武漢金銀潭
備註：
憑此小票辦理退、換貨事宜。請妥善
保管小票，錢和物品請當面點清。
http://jinyintan.aeonmall-china
.com/'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'¥'
                                                            ,''
                                                            ,'T'
                                                            ,'T'
                                                            ,''
                                                            ,'T'
                                                            ,'180'
                                                            ,'T'
                                                            ,'T'
                                                            ,'歡迎光臨'
                                                            ,'AEON歡迎您'
                                                            ,'F'
                                                            ,'W'
                                                            ,'T'
                                                            ,'30'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'F'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'T'
                                                            ,'F'
                                                            ,'F'
                                                            ,'T'
                                                            ,'4'
                                                            ,'商场联,顾客联,专柜联,公益日联'
                                                            ,'32'
                                                            ,'R'
                                                            ,'16'
                                                            ,'21'
                                                            ,'2'
                                                            ,''
                                                            ,'S'
                                                            ,'A'
                                                            ,'5'
                                                            )";

            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region 第二片螢幕設定
            strSql = @"insert into {0}DualMonitor (intDualMonitorNo
                                                    ,strDualMonitorName
                                                    ,intWidth
                                                    ,intHeight
                                                    ,ysnShowGrid
                                                    ,intGridTop
                                                    ,intGridLeft
                                                    ,intGridWidth
                                                    ,intGridHeight
                                                    ,intGridTransparency
                                                    ,strGridBGColor
                                                    ,strGridTitleBGColor
                                                    ,ysnGrid_ShowitemName
                                                    ,intGrid_itemNameWidth
                                                    ,ysnGrid_showModifier
                                                    ,ysnGrid_showPrice
                                                    ,ysnGrid_showQTY
                                                    ,ysnGrid_showoriamount
                                                    ,intGrid_oriamountwidth
                                                    ,intgridfontsize
                                                    ,intgridtitlefontsize
                                                    ,ysnshowmessage
                                                    ,intmessagetop
                                                    ,intmessageleft
                                                    ,intmessagewidth
                                                    ,intmessageheight
                                                    ,intmessagetransparency
                                                    ,strmessagebgcolor
                                                    ,strmessagefontcolor
                                                    ,ysnshowmedia
                                                    ,intMessageFontSize
                                                    )
                                            values ('1'
                                                    ,'TC_10'
                                                    ,'1280'
                                                    ,'800'
                                                    ,'T'
                                                    ,'0'
                                                    ,'0'
                                                    ,'1280'
                                                    ,'500'
                                                    ,'255'
                                                    ,'rgb(255, 255, 255)'
                                                    ,'rgb(255, 255, 255)'
                                                    ,'T'
                                                    ,'880'
                                                    ,'F'
                                                    ,'F'
                                                    ,'F'
                                                    ,'T'
                                                    ,'400'
                                                    ,'50'
                                                    ,'60'
                                                    ,'T'
                                                    ,'500'
                                                    ,'0'
                                                    ,'1280'
                                                    ,'300'
                                                    ,'255'
                                                    ,'rgb(255, 255, 255)'
                                                    ,'rgb(0, 0, 0)'
                                                    ,'F'
                                                    ,'65'
                                                    )";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region 裝置類型設定
            strSql = @"insert into {0}DeviceType (strDeviceTypeCode
                                                    ,strModel
                                                    ,strPaperCutCMD
                                                    ,strStartReceiptCMD
                                                    ,strPageEndCMD
                                                    ,strPrintLogocmd
                                                    ,strPrintDataBeginCMD
                                                    ,strPrintDataEndCMD
                                                    ,intWriteComDelayMSec
                                                    )
                                            values ('PP802'
                                                    ,'PP802'
                                                    ,'1B6D'
                                                    ,''
                                                    ,'1B64031B6D'
                                                    ,'1C700100'
                                                    ,'1B246500'
                                                    ,'0D0A'
                                                    ,'50'
                                                    )";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}DeviceType (strDeviceTypeCode
                                                    ,strModel
                                                    ,strOpenCD1CMD
                                                    )
                                            values ('RIPAC-CASHDRAWER'
                                                    ,'TC10_CD'
                                                    ,'TC10\OpenCD.bat'
                                                    )";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region 原因代碼
            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('PayIn' ,'1' ,'零钱')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('PayIn' ,'2' ,'小费')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('PayOut' ,'1' ,'现金投库')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('PayOut' ,'2' ,'购买原料')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('PayOut' ,'3' ,'钞票')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('POSReturn' ,'1' ,'改变心意')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('POSReturn' ,'2' ,'不喜欢')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('POSReturn' ,'3' ,'产品有瑕疵')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Reasons (strType ,strReasonCode ,strReasonName)
                                            values ('POSReturn' ,'4' ,'其他')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region TAX
            strSql = @"insert into {0}TAX (strTaxcode ,strTaxName ,dblTaxRate)
                                            values ('1' ,'Zero_Tax' ,'0')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region STORE_GROUP
            strSql = @"insert into {0}Store(strStoreCode,strStoreName,strStoreType,ysnActive,dtmTradeBegin,dtmCreate,strCreator) values('ALL','ALL','Group','T',sysdate,sysdate,'System')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region ROLE
            strSql = @"insert into {0}ROLE(strRoleCode, strRoleName, DTMCreate) values ('ADMINISTRATOR','ADMINISTRATOR',sysdate)";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}ROLE(strRoleCode, strRoleName, DTMCreate) values ('Front_Cashier','Front_Cashier',sysdate)";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}ROLE(strRoleCode, strRoleName, DTMCreate) values ('Finance_Admin','Finance_Admin',sysdate)";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region ROLEPOSAUTHORITY
            strSql = @"insert into {0}ROLEPOSAUTHORITY(strRoleCode, ysnCanReturn, ysnCanOrder, ysnCanHandleItemDiscount, ysnCanHandleSaleDiscount, ysnCanChangePrice, ysnCanTraining, ysnCanHold, ysnCanSetReceipt, ysnCanXZMode, ysnCanPayInPayOut, ysnCanReturnbyHand, ysnCanMobileStockTake) values ('ADMINISTRATOR','T','F','T','T','T','T','T','F','T','F','F','F')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}ROLEPOSAUTHORITY(strRoleCode, ysnCanReturn, ysnCanOrder, ysnCanHandleItemDiscount, ysnCanHandleSaleDiscount, ysnCanChangePrice, ysnCanTraining, ysnCanHold, ysnCanSetReceipt, ysnCanXZMode, ysnCanPayInPayOut, ysnCanReturnbyHand, ysnCanMobileStockTake) values ('Front_Cashier','F','F','T','T','F','F','F','F','T','F','F','F')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}ROLEPOSAUTHORITY(strRoleCode, ysnCanReturn, ysnCanOrder, ysnCanHandleItemDiscount, ysnCanHandleSaleDiscount, ysnCanChangePrice, ysnCanTraining, ysnCanHold, ysnCanSetReceipt, ysnCanXZMode, ysnCanPayInPayOut, ysnCanReturnbyHand, ysnCanMobileStockTake) values ('Finance_Admin','T','F','T','T','F','T','F','F','T','F','F','F')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            #endregion

            #region ADMIN
            strSql = @"insert into {0}Role(strRoleCode,strRoleName,dtmCreate,strCreator) values('Root','Root',sysdate,'System')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}Users(strUserCode,strUserName,strBOPassword,ysnActive,strStoreCode,strAreaCode,intBOMenuNo,intPosUserNo,intPosPassword) values('Root','Root','Root','T','ALL','ALL',0,'99999','99999')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}UserRole(strUserCode,strRoleCode) values('Root','ADMINISTRATOR')";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);

            strSql = @"insert into {0}UserPOSStore(strUserCode,strStoreCode,dtmEffective) values ('Root','ALL',sysdate)";
            strSql = string.Format(strSql, DB_Service);
            db.ClassDB.UpdData(strSql);
            */
            #endregion
            
            #region 行動支付按鈕
        strSql = @"Insert into {0}Payment (INTPAYMENTNO,STRBOPAYMENTNAME,STRPOSPAYMENTNAME,YSNACTIVE,INTSHOWORDER,YSNTAXEDBEFORE,YSNUSEFORREFUND,YSNCANCANCEL,YSNOPENCASHDRAWER,CURMAXVALUE,YSNASKCOMMENT,STRASKCOMMENTMSG,YSNPRINTASKEDCOMMENT,YSNCANPAYDECIMAL,INTAUTOVALUEPERCENT,YSNALLOWOVERPAY,YSNUSEOVERPAYCHANGERULE,INTOVERPAYTOPAYMENTNO,INTOVERPAYTOITEMNO,YSNPOINTPAY,YSNIFMODE,YSNIFINCLUDEITEMCONTENT,STRIFEXECUTEFILENAME,YSNIFPRINTCARDNO,YSNIFPRINTRECEIPT1,YSNIFPRINTRECEIPT2,YSNIFPRINTRECEIPT3,YSNIFPRINTRECEIPT4,YSNIFPRINTRECEIPT5,DTMCREATE,DTMLASTUPDATE,STRCREATOR,STRLASTUPDATEPERSON,STREXTSREF1,YSNRETURNMATCHSALE,STRPAYMENTTYPE) values (7,'行動支付','行動支付','T',1,'F','T','T','T',null,'F',null,'F','T',null,'F','F',null,null,'F','T','T','3rdPayment_AEON\IFPayment.exe OLink Payment','F','F','F','F','F','F',null,to_date('29-8月 -17','DD-MON-RR'),null,'ADMINISTRATOR',null,'T',null)";
            strSql = string.Format(strSql, DB_Service);
   
            #region TODO 測試是否連接資連
    /*        db.ClassDB.UpdData ("INSERT INTO TC_POS14.Store (STRSTORECODE,STRSTORETYPE, YSNACTIVE) VALUES  ('6', '測試6', '6')");
            DataTable dt = db.ClassDB.GetData ("select * from TC_POS14.Store");*/
            #endregion

            #region 判斷使用DB
            #region oracledb
            if ( !string.IsNullOrEmpty ( oracledb ) )
                db.ClassDB.UpdData ( strSql );
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                postaredb.UpdData (strSql);
         
            #endregion
            #endregion

            strSql = @"insert into {0}PaymentStore(intPaymentNo,strStoreCode) values ('7','ALL')";
            strSql = string.Format(strSql, DB_Service);
        

            #region 判斷使用DB
            #region oracledb
            if ( !string.IsNullOrEmpty ( oracledb ) )
                db.ClassDB.UpdData ( strSql );
            #endregion

            #region  postgredb
            if (!string.IsNullOrEmpty (postgredb))
                postaredb.UpdData (strSql);
          
            #endregion
            #endregion

            #endregion
        }

        /// <summary>
        /// 設定回傳訊息
        /// </summary>
        protected override void SetRCRM()
        {

        }

        /// <summary>
        /// 設定回傳訊息
        /// </summary>
        protected override void SaveRCRM()
        {
        }

    }
}