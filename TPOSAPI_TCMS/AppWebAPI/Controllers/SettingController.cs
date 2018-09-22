using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWebAPI.Models;
using AppWebAPI.Adapters;

namespace AppWebAPI.Controllers
{
    /// <summary>
    /// 設定頁面
    /// </summary>
    public class SettingController : Controller
    {
        /// <summary>
        /// 首頁-初始畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //非本機登入不可使用
            if (Request.Url.Host != "localhost" && Request.Url.Host != "127.0.0.1") return null;

            WebDB.SetDB setdb = new WebDB.SetDB();
            SettingModel model
                = new SettingModel
                {
                    normal = setdb.normal,
                    sensitive = setdb.sensitive_mask,
                    lang = new WebDB.langconfig().strlang
                };
            return View(model);
        }

        /// <summary>
        /// 首頁-設定後的畫面
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(SettingModel model)
        {
            //非本機登入不可使用
            if (Request.Url.Host != "localhost" && Request.Url.Host != "127.0.0.1") return null;

            if(string.IsNullOrEmpty(model.normal)
                || string.IsNullOrEmpty(model.sensitive))
            {
                model.message = "設定失敗，請重新設定!!";
                model.message += "失敗原因：";
                model.message += "normal及sensitive皆是必填項目!!";
            }
            else
            {          
                try
                {                    
                    string strConnectionString = model.normal + model.sensitive;
                    RSL.EDI.UTIL.ClassDB cdb = new RSL.EDI.UTIL.ClassDB(strConnectionString);
                    //測試連線成功才存檔
                    //if (cdb.TryConnection())
                    //{
                        new WebDB.SetDB().Set(model.normal, model.sensitive);
                        new WebDB.langconfig().Set(model.lang);
                        model.message = "設定成功!!";                     
                    //}
                    //else
                    //{
                    //    model.message = "設定失敗，請重新設定!!";
                    //    model.message += "失敗原因：";
                    //    model.message += "資料庫無法連線。";
                    //}
                }
                catch (Exception ex)
                {
                    model.message = "設定失敗，請重新設定!!";
                    model.message += "失敗原因：";
                    model.message += ex.ToString();
                }
            }
            return View(model);
        }
    }
}
