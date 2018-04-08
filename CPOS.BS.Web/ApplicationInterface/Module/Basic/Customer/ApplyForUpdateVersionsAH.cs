using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.BLL.AP;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Base;
using JIT.Utility.Notification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Customer {
	public class ApplyForUpdateVersionsAH : BaseActionHandler<EmptyRequestParameter, EmptyResponseData> {
		protected override EmptyResponseData ProcessRequest(DTO.Base.APIRequest<EmptyRequestParameter> pRequest) {
			var rd =new EmptyResponseData();
			var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;
			var bllAPCommon = new APCommonBLL(loggingSessionInfo);
			DataSet dsCustomer = bllAPCommon.GetCustomerInfo(loggingSessionInfo.ClientID);

			if (dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0) {
				string strSupportType = "40";
				string strSubject = "商户权限升级申请";
				string strBody = "申请时间：" + DateTime.Now.ToShortDateString() + "<br/>商户名称：" + dsCustomer.Tables[0].Rows[0]["customer_name"] + "<br/>产品套餐：集客宝" + "<br/>申请人名称：" + loggingSessionInfo.CurrentUser.User_Name;
				SendMailBLL bllSendMail = new SendMailBLL();
				bllSendMail.SendMail(loggingSessionInfo, strSubject, strBody, strSupportType);
			}
			return rd;
		}
	}
}