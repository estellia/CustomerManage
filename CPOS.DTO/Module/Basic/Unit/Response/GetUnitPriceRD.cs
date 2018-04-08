using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIT.CPOS.DTO.Module.Basic.Unit.Response {
	public class GetUnitPriceRD : IAPIResponseData {
		public int UnitCount { get; set; }
		public int LimitCount { get; set; }
		public double Price { get; set; }
	}
}
