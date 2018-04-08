using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.Pos.Order.Request
{
    public class CancelPosOrderRP :  IAPIRequestParameter
    {
        public string OrderId { get; set; }

        public void Validate() { }
    }
}
