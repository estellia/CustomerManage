using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xgx.WebAPI.Areas.Xgx.Controllers;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public class OrderPaymentResponseModel: BaseResponeModel
    {
        public decimal RemainingPayment { get; set; }
        public List<PayDetail> PayDetailList { get; set; }
    }

    public class PayDetail
    {
        public EnumPayMethod PayMethod { get; set; }
        public decimal Amount { get; set; }
        public int DisplayIndex { get; set; }
        public string PayTime { get; set; }
    }
}