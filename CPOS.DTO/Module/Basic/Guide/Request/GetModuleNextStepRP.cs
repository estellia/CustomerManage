using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIT.CPOS.DTO.Module.Basic.Guide.Request {
	public class GetModuleNextStepRP : IAPIRequestParameter {
		/// <summary>
		/// 商户ID
		/// </summary>
		public string ModuleCode { get; set; }
		public string UserGuideModulesId { get; set; }
		public string ParentModule { get; set; }
		
		public int Step { get; set; }
		public void Validate() {

		}
	}
}
