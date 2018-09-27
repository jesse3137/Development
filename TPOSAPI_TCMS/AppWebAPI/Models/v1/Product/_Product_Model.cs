using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AppWebAPI.Adapters;
using System.Data;

namespace AppWebAPI.Models.v1.Product
{
    /// <summary>
    /// 權限控管
    /// </summary>
    /// <typeparam name="Request">AppWebAPI.Models.v2.Permissions.?Request</typeparam>
    /// <typeparam name="Results">AppWebAPI.Models.v2.Permissions.?Results</typeparam>
    public abstract class _Product_Model<Request, Results> : APIBase<Request, Results>
        where Request : IPermissionsRequest
        where Results : IPermissionsResults
    {
        /// <summary>
        /// 權限控管
        /// </summary>
        public _Product_Model ( ) { }
        /// <summary>
        /// 權限控管
        /// </summary>
        /// <param name="request"></param>
        public _Product_Model ( Request request ) : base (request) { }
    }

    /// <summary>
    /// POS資料
    /// </summary>
    public class pos_detail
    {
        /// <summary>
        /// 收銀機代碼
        /// </summary>
        public string strtillcode;
        /// <summary>
        /// 收銀機描述
        /// </summary>
        public string strtillname;
        /// <summary>
        /// 租戶代碼
        /// </summary>
        public string strstorecode;
        /// <summary>
        /// 啟用(T) / 停用(F)
        /// </summary>
        public string ysnactive;
        /// <summary>
        /// 收銀機類型
        /// </summary>
        public string strtilltype;      
        /// <summary>
        /// 快速鍵參數編號 
        /// </summary>
        public int intPosfastkeyno;
        /// <summary>
        /// 是否使用電子發票 預設 T
        /// </summary>
        public string strEinv_ysnenable;
        /// <summary>
        /// 是否為電子發票測試模式 預設 F
        /// </summary>
        public string strEinv_ysntestmode;

        //20180925新熷
        /// <summary>
        /// 功能鍵參數編號
        /// </summary>
        public int intPosfunckeyno;
        /// <summary>
        /// 收銀機描述檔編號 
        /// </summary>
        public int intTillprofileno;
        /// <summary>
        /// 第二片螢幕設定
        /// </summary>
        public int intDualmonitorno;
        /// <summary>
        /// 電子發票設定檔編號
        /// </summary>
        public int intTweinvprofileno;
        /// <summary>
        /// 電子發票的取用帳戶 客戶ID
        /// </summary>
        public string strEinv_straccountid;
        /// <summary>
        /// 電子發票的收銀機號 Till.strTillCode
        /// </summary>
        public string strEinv_strposid;
        /// <summary>
        /// AES 預設 AES
        /// </summary>
        public string strEinv_straccesstoken;
        /// <summary>
        /// tshop的shopID
        /// </summary>
        public string strEinv_strshopid;
        /// <summary>
        /// tshop發票下傳卷數
        /// </summary>
        public int intEinv_inttakerollcnt;
        /// <summary>
        /// 警告 預設 W
        /// </summary>
        public string strEINV_STRHQCHECKFAILACTION;
        /// <summary>
        /// 分店代碼(TEST先寫AENO)
        /// </summary>
        public string strCompcode;
        /// <summary>
        /// 收銀機狀態 預設 F
        /// </summary>
        public string strStatus;
    }

    /// <summary>
    /// 收銀員
    /// </summary>
    public class casher_data
    {
        /// <summary>
        /// 收銀員名稱
        /// </summary>
        public string strusercode;
        /// <summary>
        /// POS前台帳號
        /// </summary>
        public string intposuserno;
        /// <summary>
        /// POS前台密碼
        /// </summary>
        public string intpospassword;
        /// <summary>
        /// 啟用(T) / 停用(F)
        /// </summary>
        public string ysnactive;
        /// <summary>
        /// 租戶代碼
        /// </summary>
        public string strstorecode;
        /// <summary>
        /// 租戶、財務人員所屬群組(Front_Cashier低權限、Finance_Admin高權限)
        /// </summary>
        public string strrolecode;
        /// <summary>
        /// 區域代碼(ALL)
        /// </summary>
        public string strareacode;
        /// <summary>
        /// 分公司代碼STRCOMPCODE
        /// </summary>
        public string Strcompcode;
    }

    /// <summary>
    /// 抽成類別明細
    /// </summary>
    public class agreement_detail
    {
        /// <summary>
        /// 租戶代碼
        /// </summary>
        public string strstorecode;
        /// <summary>
        /// 租戶名稱
        /// </summary>
        public string strstorename;
        /// <summary>
        /// 租戶類型
        /// </summary>
        public string strstoretype;
        /// <summary>
        /// 啟用(T) / 停用(F)
        /// </summary>
        public string ysnactive;
        /// <summary>
        /// 租戶電話號碼
        /// </summary>
        public string strphone;
        /// <summary>
        /// 租戶傳真號碼
        /// </summary>
        public string strfax;
        /// <summary>
        /// 租戶聯絡人名稱
        /// </summary>
        public string strcontactname;
    }

    /// <summary>
    /// 租戶抽成類別主檔
    /// </summary>
    public class discount_detail
    {
        /// <summary>
        /// 商品代碼
        /// </summary>
        public string intitemno;
        /// <summary>
        /// 顯示名稱 for POS,客顯
        /// </summary>
        public string stritemnamepos;
        /// <summary>
        /// 店內碼
        /// </summary>
        public string ysnactive;
        /// <summary>
        /// 租戶的大分類
        /// </summary>
        public string strclassify1code;
        /// <summary>
        /// 租戶的中分類
        /// </summary>
        public string strclassify2code;
        /// <summary>
        /// 租戶的小分類
        /// </summary>
        public string strclassify3code;
        /// <summary>
        /// 租戶的細分類
        /// </summary>
        public string strclassify4code;
        /// <summary>
        /// 其他系統參考欄位
        /// </summary>
        public string strextsref1;
        /// <summary>
        /// 稅率
        /// </summary>
        public string strtaxcode;
        /// <summary>
        /// 參考售價
        /// </summary>
        public string strcurPrice;
        /// <summary>
        /// 國際條碼
        /// </summary>
        public string strBarcode;
        /// <summary>
        /// 銷售稅別 
        /// </summary>
        public string strTax;
        /// <summary>
        /// 進價稅別
        /// </summary>
        public string strLprc_ax;
        /// <summary>
        /// 分店代號
        /// </summary>
        public string strstorecode;
        //20180921新增
        /// <summary>
        /// 分類名字strClassify1Name
        /// </summary>
        public string strCategory1_Name;
        /// <summary>
        /// 分類名字strClassify2Name
        /// </summary>
        public string strCategory2_Name;
        /// <summary>
        /// 分類名字strClassify3Name
        /// </summary>
        public string strCategory3_Name;
    }

    /// <summary>
    /// 支付方式
    /// </summary>
    public class payment_detail
    {
        /// <summary>
        /// 支付代碼流水號
        /// </summary>
        public string intpaymentno;
        /// <summary>
        /// 後台支付別名稱
        /// </summary>
        public string strbopaymentname;
        /// <summary>
        /// 前台支付別名稱
        /// </summary>
        public string strpospaymentname;
        /// <summary>
        /// 啟用(T) / 停用(F)
        /// </summary>
        public string ysnactive;
        /// <summary>
        /// 顯示在前台的順序，-1為不顯示
        /// </summary>
        public string intshoworder;
        /// <summary>
        /// 顯示在前台的順序，-1為不顯示
        /// </summary>
        public string ysnuseforrefund;
        /// <summary>
        /// 課稅 已課(T) / 未課(F)
        /// </summary>
        public string ysntaxedbefore;
        /// <summary>
        /// 一旦使用此支付，可否取消返回 啟用(T) / 停用(F)
        /// </summary>
        public string ysncancancel;
        /// <summary>
        /// 開錢箱 啟用(T) / 停用(F)
        /// </summary>
        public string ysncanopencashdrawer;
        /// <summary>
        /// 支付到小數點 啟用(T) / 停用(F)
        /// </summary>
        public string ysncanpaydecimal;
        /// <summary>
        /// 可以超額支付 現金(T)/ 信用卡(F)
        /// </summary>
        public string ysnallowoverpay;
        /// <summary>
        /// 第三方支付 啟用(T) / 停用(F)
        /// </summary>
        public string ysnifmode;
        /// <summary>
        /// 第三方支付包含商品資訊 啟用(T) / 停用(F)
        /// </summary>
        public string ysnifincludeitemcontent;
        /// <summary>
        /// 第三方支付執行程式與參數
        /// </summary>
        public string strifexecutefilename;
        /// <summary>
        /// 退貨時必須符合原支付方式
        /// </summary>
        public string ysnreturnmatchsale;
        /// <summary>
        /// 是否顯示卡號 啟用(T) / 停用(F)
        /// </summary>
        public string ysnifprintcardno;
        /// <summary>
        /// 他系統參考欄位
        /// </summary>
        public string strextsref1;
    }

    /// <summary>
    /// 支付方式回傳
    /// </summary>
    public class payment_detail_result
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

    /// <summary>
    /// POS狀態回傳
    /// </summary>
    public class pos_status
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

    /// <summary>
    /// PostgreSQL 支付 P 查詢資料回傳
    /// </summary>
    public class Pay_detail_result
    {
        /// <summary>
        /// 全域交易序號
        /// </summary>
        public int intLintglobaltransno;
        /// <summary>
        /// 付款的順序
        /// </summary>
        public int intIntsortno;
        /// <summary>
        /// 支付交易按鍵編號
        /// </summary>
        public int intIntpaymentno;
        /// <summary>
        /// Pos上的支付名稱
        /// </summary>
        public string strStrpospaymentname;
        /// <summary>
        /// (90元付100，存90)
        /// </summary>
        public int intCurvalue;
        /// <summary>
        /// 支付日期
        /// </summary>
        public DateTime dateDtmwhen;
        /// <summary>
        /// Receipt Page No(對應發票第幾張)
        /// </summary>
        public int intIntreceiptpageno;
        /// <summary>
        /// Full Value(90元付100，存100)
        /// </summary>
        public int intCurfullvalue;
        /// <summary>
        /// Taxed Before?(T:提貨券類 F:現金類)
        /// </summary>
        public string strYsntaxedbefore;
        /// <summary>
        /// Can Cancel?
        /// </summary>
        public string strYsncancancel;
        /// <summary>
        /// Open Cashdrawer?
        /// </summary>
        public string strYsnopencashdrawer;
        /// <summary>
        /// Print Asked Comment?
        /// </summary>
        public string strYsnprintaskedcomment;
        /// <summary>
        /// 是否為點數扣點
        /// </summary>
        public string strYsnpointpay;
        /// <summary>
        /// 是否為三方交易
        /// </summary>
        public string strYsnifmode;
        /// <summary>
        /// Receipt 1
        /// </summary>
        public string strStrreceipt1;
        /// <summary>
        /// Receipt 2
        /// </summary>
        public string strStrreceipt2;
        /// <summary>
        /// Receipt 3
        /// </summary>
        public string strStrreceipt3;
        /// <summary>
        /// Receipt 4
        /// </summary>
        public string strStrreceipt4;
        /// <summary>
        /// Receipt 5
        /// </summary>
        public string strStrreceipt5;
        /// <summary>
        ///	Remark 1
        /// </summary>
        public string strStrremark1;
        /// <summary>
        ///	Remark 2
        /// </summary>
        public string strStrremark2;
        /// <summary>
        ///	Remark 3
        /// </summary>
        public string strStrremark3;
        /// <summary>
        ///	Remark 4
        /// </summary>
        public string strStrremark4;
        /// <summary>
        ///	Remark 5
        /// </summary>
        public string strStrremark5;
        /// <summary>
        /// 信用卡號 
        /// AMEX	34, 37
        /// JCB	3
        /// JCB	2131, 1800
        /// MasterCard	51-55
        /// Visa	4
        /// </summary>
        public string strStrifcardno;
        /// <summary>
        /// 是否列印信用卡號
        /// </summary>
        public string strYsnifprintcardno;
        /// <summary>
        /// If Mode - Print Receipt 1?
        /// </summary>
        public string strYsnifprintreceipt1;
        /// <summary>
        /// If Mode - Print Receipt 2?
        /// </summary>
        public string strYsnifprintreceipt2;
        /// <summary>
        /// If Mode - Print Receipt 3?
        /// </summary>
        public string strYsnifprintreceipt3;
        /// <summary>
        /// If Mode - Print Receipt 4?
        /// </summary>
        public string strYsnifprintreceipt4;
        /// <summary>
        /// If Mode - Print Receipt 5?
        /// </summary>
        public string strYsnifprintreceipt5;
        /// <summary>
        /// 銷售註解
        /// </summary>
        public string strStrcomment;
        /// <summary>
        /// Auth Code
        /// </summary>
        public string strStrauthcode;
        /// <summary>
        /// 支付類別註記(例:V:Visa M:Master J:Jcb…)
        /// </summary>
        public string strStrpaymenttype;
        /// <summary>
        /// 是否為作廢交易
        /// </summary>
        public string strYsnvoid;
        /// <summary>
        /// 解交易來自於哪間分店
        /// </summary>
        public string strStrsourceid;
        /// <summary>
        /// 支付次數
        /// </summary>
        public int intIntcount;
        /// <summary>
        /// 記錄資料是否處理,是=Y
        /// </summary>
        public string API_URL;
    }
    /// <summary>
    /// PostgreSQL  商品 D 查詢回傳
    /// </summary>
    public class Good_detail_result
    {
        /// <summary>
        /// 全域流水號
        /// </summary>
        public int intLintglobaltransno;
        /// <summary>
        /// 支付順序流水號
        /// </summary>
        public int intIntsortno;
        /// <summary>
        /// 交易類別，It: 實體商品
        /// </summary>
        public string strStrtype;
        /// <summary>
        /// 租戶抽成類別主檔
        /// </summary>
        public int intIntitemno;
        /// <summary>
        /// 租戶抽成類別主檔在pos上的名稱
        /// </summary>
        public int intStritemnamepos;
        /// <summary>
        /// 商品細項
        /// </summary>
        public int intStrmodifier;
        /// <summary>
        /// 單價
        /// </summary>
        public int intCurprice;
        /// <summary>
        /// 銷貨數量(退貨為負)
        /// </summary>
        public int intDblqty;
        /// <summary>
        /// 未折扣前原金額
        /// </summary>
        public int intCuroriamount;
        /// <summary>
        /// 折扣金額
        /// </summary>
        public int intCurdiscount;
        /// <summary>
        /// 折扣後金額
        /// </summary>
        public int intCurfinalamount;
        /// <summary>
        /// 稅額
        /// </summary>
        public int intCurtax;
        /// <summary>
        /// 價格流水號
        /// </summary>
        public int intIntpriceno;
        /// <summary>
        /// 原價
        /// </summary>
        public int intCuroriprice;
        /// <summary>
        /// 銷售稅率
        /// </summary>
        public int intDblsaletaxrate;
        /// <summary>
        /// 銷售數量
        /// </summary>
        public int intDbloriqty;
        /// <summary>
        ///有無變價過 
        /// </summary>
        public string strYsnchangedprice;
        /// <summary>
        /// 商品序號
        /// </summary>
        public string strYsntrackserialno;
        /// <summary>
        /// 是否為套餐
        /// </summary>
        public string strYsnsetmeal;
        /// <summary>
        /// 是否可折價
        /// </summary>
        public string strYsndiscountable;
        /// <summary>
        /// 是否有註解
        /// </summary>
        public string strYsnaskcomment;
        /// <summary>
        /// 是否列印註解
        /// </summary>
        public string strYsnprintcomment;
        ///<summary>
        /// 是否收取服務費
        /// </summary>
        public string strYsnchargeservice;
        /// <summary>
        /// 大分類1
        /// </summary>
        public string strStrclassify1code;
        /// <summary>
        /// 中分類2
        /// </summary>
        public string strStrclassify2code;
        /// <summary>
        /// 小分類3
        /// </summary>
        public string strStrclassify3code;
        /// <summary>
        /// 細分類4
        /// </summary>
        public string strStrclassify4code;
        /// <summary>
        /// 品項內容1
        /// </summary>
        public string strStritemproperty1code;
        /// <summary>
        /// 品項內容2
        /// </summary>
        public string strStritemproperty2code;
        /// <summary>
        /// 品項內容3
        /// </summary>
        public string strStritemproperty3code;
        /// <summary>
        /// 手動折扣
        /// </summary>
        public int intIntdiscountno_Mi;
        /// <summary>
        /// 商品折扣
        /// </summary>
        public int intIntdiscountno_Ms;
        /// <summary>
        /// 自動折扣
        /// </summary>
        public int intIntdiscountno_Am;
        /// <summary>
        /// 手動折扣後金額
        /// </summary>
        public int intCurunidiscount_Mi;
        /// <summary>
        /// 商品折扣後金額
        /// </summary>
        public int intCurunidiscount_Ms;
        /// <summary>
        /// 自動折扣後金額
        /// </summary>
        public int intCurunidiscount_Am;
        /// <summary>
        /// 商品序號
        /// </summary>
        public string strStrserialno;
        /// <summary>
        /// 商品註解
        /// </summary>
        public string strStritemcomment;
        /// <summary>
        /// 原租戶抽成類別主檔
        /// </summary>
        public int intIntoriitemno;
        /// <summary>
        /// 原租戶抽成類別名稱
        /// </summary>
        public string strStroriname;
        /// <summary>
        /// 解交易來自哪間分店
        /// </summary>
        public string strStrsourceid;
        /// <summary>
        /// 外部介接系統參考值
        /// </summary>
        public string strStrextsref1;
        /// <summary>
        /// 記錄資料是否處理,是=Y
        /// </summary>
        public string API_URL;

    }
    /// <summary>
    /// PostgreSQL M 銷售查詢回傳
    /// </summary>
    public class Sale_detail_result
    {
        /// <summary>
        /// P 檔List
        /// </summary>
        public List<Pay_detail_result> Pay_detail;
        /// <summary>
        /// D 檔List
        /// </summary>
        public List<Good_detail_result> Good_detail;
        /// <summary>
        /// 系統全域流水號
        /// </summary>
        public int intLintglobaltransno;
        /// <summary>
        /// Pos本地端流水號
        /// </summary>
        public int intIntpostransno;
        /// <summary>
        /// 銷售日期時間
        /// </summary>
        public DateTime dateDtmwhen;
        /// <summary>
        /// 加入第一個商品的時間
        /// </summary>
        public DateTime dateDtmfirstitem;
        /// <summary>
        /// 結帳開始時間
        /// </summary>
        public DateTime dateDtmcheckout;
        /// <summary>
        /// 交易日
        /// </summary>
        public DateTime dateDtmtrade;
        /// <summary>
        /// 分店代碼
        /// </summary>
        public string strStrstorecode;
        /// <summary>
        /// 收銀機編號
        /// </summary>
        public string strStrtillcode;
        /// <summary>
        /// 交易類別
        /// </summary>
        public string strStrtranstype;
        /// <summary>
        /// 收銀員代碼
        /// </summary>
        public string strStrusercode;
        /// <summary>
        /// 銷售數量
        /// </summary>
        public int intDblqty;
        /// <summary>
        /// 含稅合計金額
        /// </summary>
        public int intCurfinalamount;
        /// <summary>
        /// 找零金額
        /// </summary>
        public int intCurchange;
        /// <summary>
        /// 稅額
        /// </summary>
        public int intCurtax;
        /// <summary>
        /// 已開支付總額
        /// </summary>
        public int intCurpaymenttaxed;
        /// <summary>
        /// 未開支付總額
        /// </summary>
        public int intCurpaymentnotax;
        /// <summary>
        /// 統一編號
        /// </summary>
        public string strStrcusttaxid;
        /// <summary>
        /// 參照POS銷退交易帶入原銷售交易時間
        /// </summary>
        public DateTime dateDtmoritransdatetime;
        /// <summary>
        /// 銷退交易帶入原統一編號
        /// </summary>
        public string strStroricusttaxid;
        /// <summary>
        /// 原始全域交易流水號
        /// </summary>
        public int intLintoriglobaltransno;
        /// <summary>
        ///	參照pos交易序號
        /// </summary>
        public int intIntrefpostransno;
        /// <summary>
        /// 原交易類型
        /// </summary>
        public string strStroritranstype;
        /// <summary>
        /// 是否產生電子發票證明聯
        /// </summary>
        public string strTweinv_Ysngenprove;
        /// <summary>
        /// 是否列印電子發票證明聯
        /// </summary>
        public string strTweinv_Ysnprintprove;
        /// <summary>
        /// 是否列印銷貨明細
        /// </summary>
        public string strTweinv_Ysnprinttransdtl;
        /// <summary>
        /// 完整發票號碼
        /// </summary>
        public string strTweinv_Strfullinvnum;
        /// <summary>
        /// 電子發票證明聯一維條碼值
        /// </summary>
        public string strTweinv_Strbarcode;
        /// <summary>
        /// 左側qrcode
        /// </summary>
        public string strTweinv_Strqrcode1;
        /// <summary>
        /// 右側qrcode
        /// </summary>
        public string strTweinv_Strqrcode2;
        /// <summary>
        /// 隨機碼
        /// </summary>
        public string strTweinv_Strrandom;
        /// <summary>
        /// 未稅總金額
        /// </summary>
        public int intTweinv_Curtotamtnotax;
        /// <summary>
        /// 總營業稅
        /// </summary>
        public int intTweinv_Curtottax;
        /// <summary>
        /// 含稅總金額
        /// </summary>
        public int intTweinv_Curtotamtinctax;
        /// <summary>
        /// 發票卷號
        /// </summary>
        public int intTweinv_Intrlno;
        /// <summary>
        /// 是否捐贈；T捐贈
        /// </summary>
        public string strTweinv_Ysndonate;
        /// <summary>
        /// 捐贈碼
        /// </summary>
        public string strTweinv_Strnpoban;
        /// <summary>
        /// 是否為手機條碼；T為手機條碼
        /// </summary>
        public string strTweinv_Ysnusecellphonebarcode;
        /// <summary>
        /// 手機條碼
        /// </summary>
        public string strTweinv_Strcellphonebarcode;
        /// <summary>
        /// 是否為自然人憑證；T為自然人憑證
        /// </summary>
        public string strTweinv_Ysnusenaturepersonid;
        /// <summary>
        /// 自然人憑證條碼
        /// </summary>
        public string strTweinv_Strnaturepersonid;
        /// <summary>
        /// 折扣總計
        /// </summary>
        public int intCurdiscount;
        /// <summary>
        /// 原始交易帳務日
        /// </summary>
        public DateTime dateDtmoritrade;
        /// <summary>
        /// 原始收銀機代碼
        /// </summary>
        public string strStroritillcode;
        /// <summary>
        /// 原始價格(未折扣與變價前)
        /// </summary>
        public int intCuroriamount;
        /// <summary>
        /// 重列印次數
        /// </summary>
        public int intIntreprintcount;
        /// <summary>
        /// 是否為作廢交易，作廢交易則為t
        /// </summary>
        public string strYsnvoid;
        /// <summary>
        /// 分公司代碼
        /// </summary>
        public string strStrcompcode;
        /// <summary>
        /// T表示為宣告外交官
        /// </summary>
        public string strYsndiplomat;
        /// <summary>
        /// 外交官證號
        /// </summary>
        public string strStrdiplomatcode;
        /// <summary>
        /// 未稅合計金額
        /// </summary>
        public int intCurfinalamountnotax;
        /// <summary>
        /// 記錄資料是否處理,是=Y
        /// </summary>
        public string API_URL;

    }
    #region ?Request 查詢條件
    /// <summary>
    /// 權限控管-查詢條件-介面
    /// </summary>
    public interface IPermissionsRequest { }

    /// <summary>
    /// 收銀員下傳
    /// </summary>
    public class Download_Casher_Request : IPermissionsRequest
    {
        /// <summary>
        /// 客戶ID
        /// </summary>
        public string account_id;
        /// <summary>
        /// POS機號
        /// </summary>
        public string pos_id;
        /// <summary>
        /// POS_TOKEN
        /// </summary>
        public string access_token;

        public List<casher_data> casher_data;
    }

    /// <summary>
    /// POS機
    /// </summary>
    public class Download_Pos_Request : IPermissionsRequest
    {
        public List<pos_detail> pos_detail;
    }

    /// <summary>
    /// 環境參數設定
    /// </summary>
    public class Download_Base_Request : IPermissionsRequest
    {
    }

    /// <summary>
    /// 租戶基本資料主檔
    /// </summary>
    public class Download_Agreement_Request : IPermissionsRequest
    {
        public List<agreement_detail> agreement_detail;
    }

    /// <summary>
    /// 下載抽成資料
    /// </summary>
    public class Download_Dis_Request : IPermissionsRequest
    {
        public List<discount_detail> discount_detail;
    }

    /// <summary>
    /// 快速鍵基本資料處理
    /// </summary>
    public class Download_Payment_Request : IPermissionsRequest
    {
        public List<payment_detail> payment_detail;
    }

    /// <summary>
    /// POS機狀態回傳
    /// </summary>
    public class Upload_PosStatus_Request : IPermissionsRequest
    {
    }
    #endregion

    #region ?Result 查詢結果
    /// <summary>
    /// 權限控管-查詢結果-介面
    /// </summary>
    public interface IPermissionsResults { }

    /// <summary>
    /// 收銀員下傳
    /// </summary>
    public class Download_Casher_Result : IPermissionsResults
    {
    }

    /// <summary>
    /// 環境參數設定
    /// </summary>
    public class Download_Pos_Result : IPermissionsResults
    {
    }

    /// <summary>
    /// 租戶基本資料主檔
    /// </summary>
    public class Download_Agreement_Result : IPermissionsResults
    {
    }

    /// <summary>
    /// 下載抽成資料
    /// </summary>
    public class Download_Dis_Result : IPermissionsResults
    {
    }

    /// <summary>
    /// 快速鍵基本資料
    /// </summary>
    public class Download_Payment_Result : IPermissionsResults
    {
        public List<payment_detail_result> payment_detail_result;
    }

    /// <summary>
    /// 基本資料
    /// </summary>
    public class Download_Base_Result : IPermissionsResults
    {
    }

    /// <summary>
    /// POS狀態回傳
    /// </summary>
    public class Upload_PosStatus_Result : IPermissionsResults
    {
        public List<pos_status> pos_status;
    }
    #endregion

    /// <summary>
    /// PostgreSQL 查詢條件
    /// </summary>
    public class SaleData_Up_Request : IPermissionsRequest
    {
        /// <summary>
        /// 銷售日期時間
        /// </summary>
        public string Sales_Date;
        /// <summary>
        /// 收銀機編號
        /// </summary>
        public string PosId;
        /// <summary>
        /// 全域交易序號
        /// </summary>
        public string strLintglobaltransno;
        /// <summary>
        /// M 檔
        /// </summary>
        public List<Sale_detail_result> Sale_detail_result;

    }
    /// <summary>
    ///  PostgreSQL 查詢結果
    /// </summary>
    public class SaleData_Up_Result : IPermissionsResults
    {
        public List<Pay_detail_result> Pay_detail_result;
        public List<Good_detail_result> Good_detail_result;
        public List<Sale_detail_result> Sale_detail_result;
    }



    /// <summary>
    /// RCRM = Return Code and Return Message
    /// </summary>
    public class RCRM_Result : IPermissionsRequest
    {
        /// <summary>
        /// RCRM = Return Code and Return Message
        /// </summary>
        public IEnumerable<RCRM> rcrm_list;
    }

    #region I?Model 介面
    /// <summary>
    /// 登入 - 介面
    /// </summary>
    public interface ILoginModel { }

    /// <summary>
    /// 註冊 - 介面
    /// </summary>
    public interface IRegisterModel { }
    #endregion
}