using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class OrderDoneRequestModel
    {
        public decimal RemainingPay { get; set; }
        public string OrderId { get; set; }
        public string VipId { get; set; }
        public string ComplateTime { get; set; }
        public string ModifyUser { get; set; }
    }
}