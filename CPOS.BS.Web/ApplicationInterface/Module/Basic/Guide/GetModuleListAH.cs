using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Basic.Guide.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Guide {
	public class GetModuleListAH : BaseActionHandler<EmptyRequestParameter, GetModuleListRD> {
		protected override GetModuleListRD ProcessRequest(DTO.Base.APIRequest<EmptyRequestParameter> pRequest) {
			var rd = new GetModuleListRD();
			var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;
			WApplicationInterfaceBLL bllApp = new WApplicationInterfaceBLL(loggingSessionInfo);
			T_UserGuideModulesBLL bll = new T_UserGuideModulesBLL(loggingSessionInfo);
			List<ModuleInfo> moduleList = new List<ModuleInfo>();
			//是否绑定微信公众号
			var entityApp=bllApp.QueryByEntity(new WApplicationInterfaceEntity() { CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 },null);
			if(entityApp.Count()>0) {
				rd.IsBindWeChat = 1;
			}
			///新手引导大模块
			var entity = bll.QueryByEntity(new T_UserGuideModulesEntity() { ModuleStep = 0, IsDelete = 0 }, null);
			IEnumerable<ModuleInfo> ModuleList = from e in entity
										select new ModuleInfo() {
											ModuleName = e.ModuleName,
											ModuleCode=e.ModuleCode,
											UserGuideModulesId=e.UserGuideModulesId.ToString(),
											ParentModule=e.ParentModule.ToString(),
											Url=e.Url,
											VideoUrl=e.VideoUrl,
											//ImageUrl1=e.ImageUrl1,
											//ImageUrl2=e.ImageUrl2,
											//ImageUrl3=e.ImageUrl3

										};
			///每个用户最后一次的记录，已经模块是否完成
			T_UserGuideAccessLogBLL bllUserGuideLog = new T_UserGuideAccessLogBLL(loggingSessionInfo);
			var LogList = bllUserGuideLog.QueryByEntity(new Entity.T_UserGuideAccessLogEntity() { CustomerId = loggingSessionInfo.ClientID, UserId = loggingSessionInfo.CurrentLoggingManager.User_Id, IsDelete = 0 }, null);

			foreach (var m in ModuleList) {
				foreach( var log in LogList) {
					if(m.UserGuideModulesId==log.UserGuideModulesId.ToString()) {
						m.LastStep =(int)log.LastAccessStep;
						m.LastUrl = log.Url;
						m.FinishedStatus = (int)log.FinishedStatus;
					}
				}
				moduleList.Add(m);
			}
			rd.ModuleInfoList = moduleList;

			return rd;
		}
	}
}