using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AppWebAPI.Models.v1.Product;
using System.Web.Http.Description;

namespace AppWebAPI.Controllers.Product
{
    /// <summary>
    /// 權限控管
    /// </summary>
    public class ProductController : ApiController
    {
        [ApiExplorerSettings(IgnoreApi = false)]
        public Download_Casher_Model Download_Casher([FromBody]Download_Casher_Request request)
        {
            return new Download_Casher_Model(request);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        public Download_Pos_Model Download_Pos([FromBody]Download_Pos_Request request)
        {
            return new Download_Pos_Model(request);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        public Download_Payment_Model Download_Payment([FromBody]Download_Payment_Request request)
        {
            return new Download_Payment_Model(request);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        public Download_Agreement_Model Download_Agreement([FromBody]Download_Agreement_Request request)
        {
            return new Download_Agreement_Model(request);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        public Download_Dis_Model Download_Dis([FromBody]Download_Dis_Request request)
        {
            return new Download_Dis_Model(request);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        public Download_Base_Model Download_Base([FromBody]Download_Base_Request request)
        {
            return new Download_Base_Model(request);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        public Download_SpeedKey_Model Download_SpeedKey([FromBody]Download_Dis_Request request)
        {
            return new Download_SpeedKey_Model(request);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        public Upload_PosStatus_Model Upload_PosStatus([FromBody]Upload_PosStatus_Request request)
        {
            return new Upload_PosStatus_Model(request);
        }

        [ApiExplorerSettings (IgnoreApi = false)]
        public Query_SaleData_Up_Model Query_SaleData_Up ( [FromBody]SaleData_Up_Request request )
        {
            return new Query_SaleData_Up_Model (request);
        }
    }
}
