using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppWebAPI.Adapters;

namespace AppWebAPI.UnitTests.Adapters
{
    [TestClass]
    public class WebDBUnitTest
    {
        /// <summary>
        /// 測試資料連線
        /// </summary>
        [TestMethod]
        public void Test_DB_Connect()
        {
            WebDB db = new WebDB();
            string sysdate = db.ClassDB.GetData("select to_char(sysdate,'yyyymmdd') from dual").Rows[0][0].ToString();
            Assert.AreEqual(DateTime.Now.ToString("yyyyMMdd"), sysdate);
        }
    }
}
