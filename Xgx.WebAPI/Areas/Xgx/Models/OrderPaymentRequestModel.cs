using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class OrderPaymentRequestModel
    {
        public OrderContract OrderContract { get; set; }
        public int CouponFlag { get; set; }
        public string CouponId { get; set; }
        public int IntegralFlag { get; set; }
        public int Integral { get; set; }
        public decimal IntegralAmount { get; set; }
        public int VipEndAmountFlag { get; set; }
        public decimal VipEndAmount { get; set; }
        public int ReturnAmountFlag { get; set; }
        public decimal ReturnAmount { get; set; }
        public decimal VipDiscount { get; set; }
        public string Remark { get; set; }
    }
}