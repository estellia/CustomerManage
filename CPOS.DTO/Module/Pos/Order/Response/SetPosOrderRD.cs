using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.Pos.Order.Response
{
    public class SetPosOrderRD : IAPIResponseData
    {
        public string orderId { get; set; }

        /// <summary>
        /// 还需支付金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
