using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Basic.Guide.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Guide {
	public class GetModuleLastStepAH : BaseActionHandler<EmptyRequestParameter, GetMoudleLastStepRD> {
		protected override GetMoudleLastStepRD ProcessRequest(DTO.Base.APIRequest<EmptyRequestParameter> pRequest) {
			var rd =new GetMoudleLastStepRD();
			var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;
			T_UserGuideAccessLogBLL bllUserGuideLog = new T_UserGuideAccessLogBLL(loggingSessionInfo);

			var LogList= bllUserGuideLog.QueryByEntity(new Entity.T_UserGuideAccessLogEntity() { CustomerId = loggingSessionInfo.ClientID,UserId=loggingSessionInfo.CurrentLoggingManager.User_Id, IsDelete = 0 },null).ToList();
			rd.ModuleLastStepList = LogList;

			return rd;
		}
	}
}