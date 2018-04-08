using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.ERPAPI.Requests
{
    public class ReturnOrderToHeadRP : IAPIRequestParameter
    {

        #region 属性
        /// <summary>
        /// 订单编号（必填）
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 门店编号
        /// </summary>
        public string UnitID { get; set; }

        /// <summary>
        /// 更新方式为到店自提时，自提ID，自提开始时间，自提结束时间都不能为空
        /// </summary>
        const int ERROR_CODE_NO_STOREID = 301;
        const int ERROR_CODE_NO_DATERANGE = 302;
        #endregion
        public void Validate()
        {

        }
    }
}
