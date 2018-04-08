using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xgx.WebAPI.Areas.Xgx.Controllers;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class MemberBenefitsResponeModel : BaseResponeModel
    {
        public decimal EndIntegral { get; set; }
        public decimal VipDiscount { get; set; }
        public int EnableIntegral { get; set; }
        public int EnableRewardCash { get; set; }
        public decimal Integral { get; set; }
        public string IntegralDesc { get; set; }
        public decimal IntegralAmount { get; set; }
        public decimal VipEndAmount { get; set; }
        public decimal ReturnAmount { get; set; }
        public int PointsRedeemLowestLimit { get; set; }
        public decimal CashRedeemLowestLimit { get; set; }
        public List<CouponModel> CouponInfoList { get; set; }

    }
}