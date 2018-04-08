using JIT.CPOS.BS.Entity;
using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIT.CPOS.DTO.Module.Basic.Guide.Response {
	public class GetMoudleLastStepRD : IAPIResponseData {
		public List<T_UserGuideAccessLogEntity> ModuleLastStepList { get;set;}
	}

}
