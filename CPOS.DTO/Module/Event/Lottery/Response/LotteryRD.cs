﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIT.CPOS.DTO.Base;
namespace JIT.CPOS.DTO.Module.Event.Lottery.Response
{
    public class LotteryRD : IAPIResponseData
    {
        public string PrizeId { get; set; }
        public string PrizeName { get; set; }
        public string ResultMsg { get; set; }
        public string CouponTypeName { get; set; }
        public decimal ParValue { get; set; }
        public int ErrCode { get; set; }
        public int Location { get; set; }
    }
}
