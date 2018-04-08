using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.BLL.AP;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Basic.Unit.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Unit {
	public class ApplyForBuyUnitAH : BaseActionHandler<ApplyForBuyUnitRP, EmptyResponseData> {
		protected override EmptyResponseData ProcessRequest(DTO.Base.APIRequest<ApplyForBuyUnitRP> pRequest) {
			var rd = new EmptyResponseData();
			var para = pRequest.Parameters;

			var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;
			var bllAPCommon = new APCommonBLL(loggingSessionInfo);
			DataSet dsCustomer = bllAPCommon.GetCustomerInfo(loggingSessionInfo.ClientID);

			if (dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0) {
				string strSupportType = "30";
				string strSubject = "商户购买门店申请";
				string strBody = "申请时间：" + DateTime.Now.ToShortDateString() + "<br/>商户名称：" + dsCustomer.Tables[0].Rows[0]["customer_name"] + "<br/>产品套餐：集客宝" + "<br/>申请人名称：" + loggingSessionInfo.CurrentUser.User_Name + "<br/>购买数量：" + para.ByeUnitCount.ToString();
				SendMailBLL bllSendMail = new SendMailBLL();
				bllSendMail.SendMail(loggingSessionInfo, strSubject, strBody, strSupportType);

				T_UnitNumApplyOrderBLL bll = new T_UnitNumApplyOrderBLL(loggingSessionInfo);
				T_UnitNumApplyOrderEntity entity = new T_UnitNumApplyOrderEntity();
				entity.ApplyUnitNum = para.ByeUnitCount;
				entity.ApplyUnitsPrice = para.ApplyUnitsPrice;
				entity.UnitNumLimitConfigId =new Guid(bllAPCommon.GetSysUnitNumLimitConfigId());
				entity.CustomerId = loggingSessionInfo.ClientID;
				bll.Create(entity);
			}
			return rd;
		}
	}
}