using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.BLL.AP;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Basic.Unit.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Unit {
	public class GetUnitPriceAH : BaseActionHandler<EmptyRequestParameter, GetUnitPriceRD> {
		protected override GetUnitPriceRD ProcessRequest(DTO.Base.APIRequest<EmptyRequestParameter> pRequest) {
			var rd = new GetUnitPriceRD();
			var para = pRequest.Parameters;
			var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;
			var bllAPCommon = new APCommonBLL(loggingSessionInfo);

			var unitService = new UnitService(CurrentUserInfo);

			DataSet ds = bllAPCommon.GetCustomerLimtUnitCount(loggingSessionInfo.ClientID);

			
			if(ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0) {
				rd.LimitCount = Convert.ToInt16(ds.Tables[0].Rows[0]["units"]);
				rd.Price = Convert.ToDouble(ds.Tables[1].Rows[0]["SingleUnitPrice"]);
				rd.UnitCount = unitService.GetUnitCountByCustomerId(loggingSessionInfo.ClientID);
			}
			return rd;
		}
	}
}