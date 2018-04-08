using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Basic.Guide.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Guide {
	public class SetModuleLastStepAH : BaseActionHandler<SetModuleLastStepRP, EmptyResponseData> {
		public object T_UserGuideAccessLogEmpty { get; private set; }

		protected override EmptyResponseData ProcessRequest(DTO.Base.APIRequest<SetModuleLastStepRP> pRequest) {
			var rd = new EmptyResponseData();
			var para = pRequest.Parameters;
			var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;

			T_UserGuideAccessLogBLL bllUserGuideLog = new T_UserGuideAccessLogBLL(loggingSessionInfo);	
			T_UserGuideAccessLogEntity entityUserGuideLog = new T_UserGuideAccessLogEntity();

			entityUserGuideLog.UserId = loggingSessionInfo.CurrentLoggingManager.User_Id;
			entityUserGuideLog.UserGuideModulesId =new Guid(para.ParentModule);
			entityUserGuideLog.LastAccessStep = para.Step;
			entityUserGuideLog.FinishedStatus = para.FinishedStatus;
			entityUserGuideLog.CustomerId = loggingSessionInfo.ClientID;
			entityUserGuideLog.IsDelete = 0;
			entityUserGuideLog.Url = para.Url;
			bllUserGuideLog.SetUserGuideLog(entityUserGuideLog, loggingSessionInfo);
			return rd; ;
		}
	}
}