﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.BS.Entity;
using JIT.Utility.ExtensionMethod;
using System.Web.SessionState;
using JIT.Utility.Web.ComponentModel;
using JIT.Utility.Web;
using JIT.CPOS.BS.Entity.Pos;

namespace JIT.CPOS.BS.Web.Framework.Javascript.Biz.Handler
{
    /// <summary>
    /// ECCustomerSelectHandler 的摘要说明
    /// </summary>
    public class ECCustomerSelectHandler : JIT.CPOS.BS.Web.PageBase.JITCPOSAjaxHandler, 
        IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 页面入口
        /// </summary>
        /// <param name="context"></param>
        protected override void AjaxRequest(HttpContext pContext)
        {
            string content = "";
            switch (pContext.Request["method"])
            {
                case "sku":
                    content = GetSkuSelectData();
                    break;
            }
            pContext.Response.Write(content);
            pContext.Response.End();
        }

        #region GetSkuSelectData
        /// <summary>
        /// GetSkuSelectData
        /// </summary>
        private string GetSkuSelectData()
        {
            var skuService = new EEnterpriseCustomersBLL(new SessionManager().CurrentUserLoginInfo);

            string key = string.Empty;
            string content = string.Empty;
            if (Request("query") != null && Request("query") != string.Empty)
            {
                key = Request("query").ToString().Trim();
            }


            var list = skuService.GetTopList(new EEnterpriseCustomersEntity() {
                EnterpriseCustomerName = key
            });

            var jsonData = new JsonData();
            jsonData.totalCount = list.Count.ToString();
            jsonData.data = list;

            content = jsonData.ToJSON();
            return content;
        }
        #endregion

        new public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}