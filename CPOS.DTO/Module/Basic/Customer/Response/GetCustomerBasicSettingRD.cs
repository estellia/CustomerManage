﻿using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIT.CPOS.DTO.Module.Basic.Customer.Response
{
    public class GetCustomerBasicSettingRD : IAPIResponseData
    {
        public string SettingValue { get; set; }
    }
}
