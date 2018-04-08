using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Module.Basic.Guide.Request;
using JIT.CPOS.DTO.Module.Basic.Guide.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Guide {
	public class GetModuleNextStepAH : BaseActionHandler<GetModuleNextStepRP, GetModuleNextStepRD> {
		protected override GetModuleNextStepRD ProcessRequest(DTO.Base.APIRequest<GetModuleNextStepRP> pRequest) {
			var rd = new GetModuleNextStepRD();
			var preModule = new NextModule();
			var nextModule = new NextModule();
			var nowModule = new NextModule();
			var para = pRequest.Parameters;

			var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;

			T_UserGuideModulesBLL bll = new T_UserGuideModulesBLL(loggingSessionInfo);
			T_UserGuideModulesEntity entity = new T_UserGuideModulesEntity();
			///上一步
			entity = bll.QueryByEntity(new T_UserGuideModulesEntity() { ParentModule = new Guid(para.ParentModule), ModuleStep = para.Step - 1, IsDelete = 0 }, null).SingleOrDefault();
			if (entity != null) {
				preModule.ModuleCode = entity.ModuleCode;
				preModule.ParentModule = entity.ParentModule.ToString();
				preModule.Url = entity.Url;
				preModule.VideoUrl = entity.VideoUrl;
				preModule.ModuleStep =(int)entity.ModuleStep;
				//preModule.UserGuideModulesId = entity.UserGuideModulesId.ToString();
				//preModule.ImageUrl1 = entity.ImageUrl1;
				//preModule.ImageUrl2 = entity.ImageUrl2;
				//preModule.ImageUrl3 = entity.ImageUrl3;
			}
			///当前步
			entity = bll.QueryByEntity(new T_UserGuideModulesEntity() { ParentModule = new Guid(para.ParentModule), ModuleStep = para.Step, IsDelete = 0 }, null).SingleOrDefault();
			if (entity != null) {
				nowModule.ModuleCode = entity.ModuleCode;
				nowModule.ParentModule = entity.ParentModule.ToString();
				nowModule.Url = entity.Url;
				nowModule.VideoUrl = entity.VideoUrl;
				nowModule.ModuleStep = (int)entity.ModuleStep;
				//nowModule.UserGuideModulesId = entity.UserGuideModulesId.ToString();
				//nowModule.ImageUrl1 = entity.ImageUrl1;
				//nowModule.ImageUrl2 = entity.ImageUrl2;
				//nowModule.ImageUrl3 = entity.ImageUrl3;

			}
			///下一步
			entity = bll.QueryByEntity(new T_UserGuideModulesEntity() { ParentModule = new Guid(para.ParentModule), ModuleStep = para.Step+1, IsDelete = 0 }, null).SingleOrDefault();
			
			if (entity != null) {
				nextModule.ModuleCode = entity.ModuleCode;
				nextModule.ParentModule = entity.ParentModule.ToString();
				nextModule.Url = entity.Url;
				nextModule.VideoUrl = entity.VideoUrl;
				nextModule.ModuleStep = (int)entity.ModuleStep;
				//nextModule.UserGuideModulesId = entity.UserGuideModulesId.ToString();
				//nextModule.ImageUrl1 = entity.ImageUrl1;
				//nextModule.ImageUrl2 = entity.ImageUrl2;
				//nextModule.ImageUrl3 = entity.ImageUrl3;


			}

			rd.PreModule = preModule;
			rd.NextModule = nextModule;
			rd.NowModule = nowModule;
			return rd;
		}
	}
}