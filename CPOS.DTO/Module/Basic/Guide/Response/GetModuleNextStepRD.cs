﻿using JIT.CPOS.BS.Entity;
using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIT.CPOS.DTO.Module.Basic.Guide.Response {
	public class GetModuleNextStepRD: IAPIResponseData {
		public NextModule PreModule { get; set; }
		public NextModule NowModule { get; set; }

		public NextModule NextModule { get; set; }
	}
	public class NextModule {
		public  string ModuleCode { get; set; }
		public string UserGuideModulesId { get; set; }
		public string ParentModule { get; set; }
		public int LastStep { get; set; }
		public string LastUrl { get; set; }
		public int ModuleStep { get; set; }
		private string _url = "/module/Index/IndexPage.aspx";
		public string Url
		{
			set { _url = value; }
			get { return _url; }
		}
		public string VideoUrl { get; set; }
		//public string ImageUrl1 { get; set; }
		//public string ImageUrl2 { get; set; }
		//public string ImageUrl3 { get; set; }

	}
}
