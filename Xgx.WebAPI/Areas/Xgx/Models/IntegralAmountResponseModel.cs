using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class IntegralAmountResponseModel : BaseResponeModel
    {
        public decimal IntegralAmount { get; set; }
        public decimal Integral { get; set; }
        public string IntegralDesc { get; set; }
    }
}