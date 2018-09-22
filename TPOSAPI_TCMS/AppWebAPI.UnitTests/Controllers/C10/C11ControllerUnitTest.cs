using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AppWebAPI.Controllers;
using AppWebAPI.Models.C10;
using AppWebAPI.Controllers.C10;
using System.Web.Http;
using System.Net;
using AppWebAPI.UnitTests.Adapters;

namespace AppWebAPI.UnitTests.Controllers.C10
{
    [TestClass]
    public class C11ControllerUnitTest
    {
        /// <summary>
        /// Post 異常-應回傳403
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Post_Throws()
        {
            //排列
            C11Controller controller = new C11Controller();

            //作用
            C11Request request = new C11Request { userid = "", password = "" };
            C11Model result;

            try
            {
                result = controller.Post(request);
            }
            catch (HttpResponseException ex)
            {
                //判斷提示
                Assert.AreEqual(ex.Response.StatusCode, HttpStatusCode.Forbidden, "非403錯誤");                
                throw ex;
            }
        }

        /// <summary>
        /// Post
        /// </summary>
        [TestMethod]
        public void Post()
        {
            //排列
            C11Controller controller = new C11Controller();

            //作用
            C11Request request = new C11Request { userid = "xcom", password = "xcom7035" };
            C11Model result = controller.Post(request);

            //判斷提示
            Assert.IsNotNull(result);
            Assert.AreEqual(result.results.username, "權限管理者");
            Assert.AreEqual(result.results.login_guid.Length, 32);            
        }
    }
}
