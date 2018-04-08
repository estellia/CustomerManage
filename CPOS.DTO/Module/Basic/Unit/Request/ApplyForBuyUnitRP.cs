using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIT.CPOS.DTO.Module.Basic.Unit.Request {
	public class ApplyForBuyUnitRP : IAPIRequestParameter {
		public int ByeUnitCount { get; set; }
		public decimal ApplyUnitsPrice { get; set; }
		public void Validate() {

		}
	}
}
