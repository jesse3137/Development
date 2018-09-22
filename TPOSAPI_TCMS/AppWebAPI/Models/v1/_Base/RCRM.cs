using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebAPI.Models.v1
{
    /// <summary>
    /// RCRM = Return Code and Return Message
    /// </summary>
    public partial class RCRM
    {
        /// <summary>
        /// RCRM = Return Code and Return Message
        /// </summary>
        public RCRM() { }

        /// <summary>
        /// RCRM = Return Code and Return Message
        /// </summary>
        /// <param name="I_RC_Enum">回傳代碼</param>
        public RCRM(RC_Enum I_RC_Enum)
        {
            rc_enum = I_RC_Enum;
            FunTranslateRCRM();
        }

        /// <summary>
        /// RCRM = Return Code and Return Message
        /// </summary>
        /// <param name="I_RC_Enum">回傳代碼</param>
        /// <param name="I_BeforeRM">Return Message 前面的字串</param>
        /// <param name="I_AfterRM">Return Message 後面的字串</param>
        public RCRM(RC_Enum I_RC_Enum, string I_BeforeRM, string I_AfterRM)
        {
            rc_enum = I_RC_Enum;
            FunTranslateRCRM();
            BeforeRM = I_BeforeRM;
            AfterRM = I_AfterRM;
        }

        /// <summary>
        /// RCRM = Return Code and Return Message
        /// </summary>
        /// <param name="I_RC">自訂 Return Code</param>
        /// <param name="I_RM">自訂 Return Message</param>
        /// <param name="I_BeforeRM">Return Message 前面的字串</param>
        /// <param name="I_AfterRM">Return Message 後面的字串</param>
        public RCRM(string I_RC, string I_RM, string I_BeforeRM, string I_AfterRM)
        {
            RC = I_RC;
            RM = I_RM;
            BeforeRM = I_BeforeRM;
            AfterRM = I_AfterRM;
        }

        /// <summary>
        /// 回傳代碼
        /// </summary>
        private RC_Enum rc_enum { get; set; }

        /// <summary>
        /// Return Code
        /// </summary>
        public string RC { get; set; }
        private string _RM { get; set; }
        /// <summary>
        /// Return Message
        /// </summary>
        public string RM
        {
            get { return BeforeRM + APIBase.FunML(_RM) + AfterRM; }
            set { _RM = value; }
        }

        /// <summary>
        /// Return Message 前面的字串
        /// </summary>
        private string BeforeRM { get; set; }
        /// <summary>
        /// Return Message 後面的字串
        /// </summary>
        private string AfterRM { get; set; }
    }

    public partial class RCRM
    {
        /// <summary>
        /// 由RC_Enum轉換成RC及RM
        /// </summary>
        public void FunTranslateRCRM()
        {
            FunTranslateRCRM(rc_enum);
        }

        /// <summary>
        /// 由RC_Enum轉換成RC及RM
        /// </summary>
        /// <param name="I_RC_Enum">回傳代碼</param>
        public void FunTranslateRCRM(RC_Enum I_RC_Enum)
        {
            //由RC_Enum轉換成RC及RM
            switch (I_RC_Enum)
            {
                case RC_Enum.OK:
                    RC = "1";
                    RM = "成功";
                    break;
                case RC_Enum.MAILOK:
                    RC = "1";
                    RM = "已寄送成功";
                    break;
                case RC_Enum.FAIL_0:
                    RC = "0";
                    RM = "系統將自動登出會員身份，可能原因包含：登入逾時、在其他裝置重複登入、系統安全性調整等。請您再次登入會員，謝謝您的配合。";
                    break;
                case RC_Enum.FAIL_1:
                    RC = "-1";
                    RM = "查無資料";
                    break;
                case RC_Enum.FAIL_2:
                    RC = "0";
                    RM = "超過認證時間，請重新登入";
                    break;
                case RC_Enum.FAIL_10:
                    RC = "-10";
                    RM = "系統繁忙，請稍後再試！";
                    break;
                case RC_Enum.FAIL_400_0001:
                    RC = "-400.0001";
                    RM = "必須輸入";
                    break;
                case RC_Enum.FAIL_400_0002:
                    RC = "-400.0002";
                    RM = "長度有誤";
                    break;
                case RC_Enum.FAIL_400_0003:
                    RC = "-400.0003";
                    RM = "格式有誤";
                    break;
                case RC_Enum.FAIL_401_0001:
                    RC = "-401.0001";
                    RM = "使用者名稱、密碼輸入錯誤!!  請重新輸入!";
                    break;
                case RC_Enum.FAIL_401_0002:
                    RC = "-401.0002";
                    RM = "已存在帳號，尚未通過認證!";
                    break;
                case RC_Enum.FAIL_401_0003:
                    RC = "-401.0003";
                    RM = "已存在帳號，尚未開通!";
                    break;
                case RC_Enum.FAIL_401_0004:
                    RC = "-401.0004";
                    RM = "已存在帳號，尚未通過認證!";
                    break;
                case RC_Enum.FAIL_401_0005:
                    RC = "-401.0005";
                    RM = "帳號已存在!";
                    break;
                case RC_Enum.FAIL_401_0006:
                    RC = "-401.0006";
                    RM = "此會員卡已被鎖卡!";
                    break;
                case RC_Enum.FAIL_401_0007:
                    RC = "-401.0007";
                    RM = "重新認證次數超過上限，無法再次寄發認證，請跟客服連絡!";
                    break;
                case RC_Enum.FAIL_401_0008:
                    RC = "-401.0008";
                    RM = "查無GUID相關資料";
                    break;
                case RC_Enum.FAIL_401_0011:
                    RC = "-401.0011";
                    RM = "您並無任何權限 ! 請洽系統管理者。";
                    break;
                case RC_Enum.FAIL_401_0012:
                    RC = "-401.0012";
                    RM = "認證碼錯誤。";
                    break;
                case RC_Enum.FAIL_401_0013:
                    RC = "-401.0013";
                    RM = "認證時效已過。";
                    break;
                case RC_Enum.FAIL_401_0014:
                    RC = "-401.0014";
                    RM = "已開過卡或卡號重覆。";
                    break;
                case RC_Enum.FAIL_401_0015:
                    RC = "-401.0015";
                    RM = "裝置數已達上限。";
                    break;
                case RC_Enum.FAIL_401_0016:
                    RC = "-401.0016";
                    RM = "手機驗證錯誤。";
                    break;
                case RC_Enum.FAIL_401_0017:
                    RC = "-401.0017";
                    RM = "裝置已存在。";
                    break;
                case RC_Enum.FAIL_401_0018:
                    RC = "-401.0018";
                    RM = "裝置數已達年度上限。";
                    break;
                case RC_Enum.FAIL_401_0019:
                    RC = "-401.0019";
                    RM = "申辦會員年齡未滿16歲。";
                    break;
                case RC_Enum.FAIL_401_0020:
                    RC = "-401.0020";
                    RM = "此身分證字號不存在會員資料，请查明。";
                    break;
                case RC_Enum.FAIL_401_0021:
                    RC = "-401.0021";
                    RM = "此裝置已被啟用過。";
                    break;
                case RC_Enum.FAIL_402_0001:
                    RC = "-402.0001";
                    RM = "查無此原始交易資料。";
                    break;
                case RC_Enum.FAIL_402_0002:
                    RC = "-402.0002";
                    RM = "查無此原始交易資料。";
                    break;
                case RC_Enum.FAIL_402_0003:
                    RC = "-402.0003";
                    RM = "已有相同交易資料。";
                    break;

                case RC_Enum.FAIL_401_0099:
                    RC = "-401.0099";
                    RM = "資料庫取值失敗。";
                    break;
            }
        }
    }

    /// <summary>
    /// 回傳代碼列舉
    /// </summary>
    public enum RC_Enum
    {
        /// <summary>
        /// 成功
        /// </summary>
        OK,
        /// <summary>
        /// 寄送成功
        /// </summary>
        MAILOK,
        /// <summary>
        /// 系統已過期請重新登入
        /// </summary>
        FAIL_0,
        /// <summary>
        /// 查無資料
        /// </summary>
        FAIL_1,
        /// <summary>
        /// 超過認證時間，請重新登入。
        /// </summary>
        FAIL_2,
        /// <summary>
        /// 系統繁忙，請稍後再試！
        /// </summary>
        FAIL_10,
        /// <summary>
        /// 必須輸入
        /// </summary>
        FAIL_400_0001,
        /// <summary>
        /// 長度有誤
        /// </summary>
        FAIL_400_0002,
        /// <summary>
        /// 格式有誤
        /// </summary>
        FAIL_400_0003,
        /// <summary>
        /// 使用者名稱、密碼輸入錯誤!!  請重新輸入!
        /// </summary>
        FAIL_401_0001,
        /// <summary>
        /// 已存在帳號，尚未通過認證
        /// </summary>
        FAIL_401_0002,
        /// <summary>
        /// 已存在帳號，尚未開通
        /// </summary>
        FAIL_401_0003,
        /// <summary>
        /// 已存在帳號，尚未通過認證
        /// </summary>
        FAIL_401_0004,
        /// <summary>
        /// 已存在帳號
        /// </summary>
        FAIL_401_0005,
        /// <summary>
        /// 會員卡被鎖卡
        /// </summary>
        FAIL_401_0006,
        /// <summary>
        /// 認證次數超過上限
        /// </summary>
        FAIL_401_0007,
        /// <summary>
        /// 查無GUID資料
        /// </summary>
        FAIL_401_0008,
        /// <summary>
        /// 您並無任何權限 ! 請洽系統管理者。
        /// </summary>
        FAIL_401_0011,
        ///<summary>
        ///認證碼錯誤。
        ///</summary>
        FAIL_401_0012,
        ///<summary>
        ///認證時效已過。
        ///</summary>
        FAIL_401_0013,
        /// <summary>
        /// 已開過卡或卡號重覆
        /// </summary>
        FAIL_401_0014,
        /// <summary>
        /// 裝置數已達上限
        /// </summary>
        FAIL_401_0015,
        /// <summary>
        /// MAIL驗證錯誤
        /// </summary>
        FAIL_401_0016,
        /// <summary>
        /// 裝置已存在
        /// </summary>
        FAIL_401_0017,
        /// <summary>
        /// 裝置數已達年度上限
        /// </summary>
        FAIL_401_0018,
        /// <summary>
        /// 申辦會員年齡未滿16歲
        /// </summary>
        FAIL_401_0019,
        /// <summary>
        /// 此身分證字號不存在會員資料，请查明。
        /// </summary>
        FAIL_401_0020,
        /// <summary>
        /// 此裝置已被啟用過。
        /// </summary>
        FAIL_401_0021,

        /// <summary>
        /// 查無此原始交易資料。
        /// </summary>
        FAIL_402_0001,
        /// <summary>
        /// 此單據已退貨或作廢。
        /// </summary>
        FAIL_402_0002,
        /// <summary>
        /// 已有相同交易資料。
        /// </summary>
        FAIL_402_0003,

        /// <summary>
        /// 資料庫取資料錯誤
        /// </summary>
        FAIL_401_0099

    }
}