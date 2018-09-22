using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebAPI.Controllers
{
    /// <summary>
    /// API測試
    /// </summary>
    public class TestController : Controller
    {
        /// <summary>
        /// API測試
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
