﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.Event.ContactEvent.Response
{
    public class SetContactEventRD : IAPIResponseData
    {
        
        public string ContactEventId { get; set; }
        public bool Success { get; set; }
        public string ErrMsg { get; set; }

    }
}
