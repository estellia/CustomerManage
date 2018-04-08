using JIT.CPOS.BS.BLL.AP;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Basic.Customer.Request;
using JIT.CPOS.DTO.Module.Basic.Customer.Response;
using JIT.CPOS.Web.ApplicationInterface.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIT.CPOS.Web.ApplicationInterface.Module.Customer.Basic {
	public class GetVersionAH : BaseActionHandler<EmptyRequestParameter, GetVersionRD> {
		protected override GetVersionRD ProcessRequest(DTO.Base.APIRequest<EmptyRequestParameter> pRequest) {
			var rd = new GetVersionRD();

			var para = pRequest.Parameters;
			var bllAPCommon = new APCommonBLL(this.CurrentUserInfo);
			rd.VersionId = bllAPCommon.GetCustomerVersion(this.CurrentUserInfo.ClientID);
			return rd;
		}
	}
}