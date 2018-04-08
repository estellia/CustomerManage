﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.Order.Delivery.Response
{
    public class GetOrderDeliveryDateTimeRangeRD : IAPIResponseData
    {
        public DateTimeInfo[] DateRange { get; set; }
    }

    public class DateTimeInfo
    {
        public string dataRange { get; set; }
        public string[] timeRange { get; set; }
    }
}
