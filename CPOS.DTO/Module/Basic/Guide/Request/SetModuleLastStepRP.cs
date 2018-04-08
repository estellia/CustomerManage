using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIT.CPOS.DTO.Module.Basic.Guide.Request {
	public class SetModuleLastStepRP: IAPIRequestParameter {
		public string ModuleCode { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string ParentModule { get; set; }
		public int Step { get; set; }
		public string Url { get; set; }

		public int FinishedStatus { get; set; }
		public void Validate() {

		}
	}
}
