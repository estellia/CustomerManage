using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class CouponModel
    {
        public string CouponId { get; set; }
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public string CouponDesc { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int DisplayIndex { get; set; }
        public int EnableFlag { get; set; }
        public decimal CouponAmount { get; set; }

    }
}