using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.VIP.VipGold.Request
{
    public class SetRechargeOrderRP : IAPIRequestParameter
    {
        /// <summary>
        /// 实付款
        /// </summary>
        public decimal ActuallyPaid { get; set; }


        /// <summary>
        /// 门店Id
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// 会员Id
        /// </summary>
        public string VipId { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public int PayStatus { get; set; }

        /// <summary>
        /// Pos来源名称
        /// </summary>
        public string PosSourceName { get; set; }


        public void Validate() { }
    }
}
