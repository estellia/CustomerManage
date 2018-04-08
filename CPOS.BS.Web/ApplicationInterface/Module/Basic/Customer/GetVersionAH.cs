using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JIT.CPOS.DTO.Module.Basic.Customer.Request;
using JIT.CPOS.DTO.Module.Basic.Customer.Response;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.BS.BLL.AP;
using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Customer {
	public class GetVersionAH : BaseActionHandler<EmptyRequestParameter, GetVersionRD> {
		protected override GetVersionRD ProcessRequest(DTO.Base.APIRequest<EmptyRequestParameter> pRequest) {
			var rd = new GetVersionRD();

			var para= pRequest.Parameters;
			var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;
			var bllAPCommon = new APCommonBLL(loggingSessionInfo);
			rd.VersionId=bllAPCommon.GetCustomerVersion(loggingSessionInfo.ClientID);
			return rd;
		}
	}
}