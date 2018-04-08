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

namespace JIT.CPOS.BS.Web.Framework.Javascript.Biz.Handler
{
    /// <summary>
    /// EventStatusHandler 的摘要说明
    /// </summary>
    public class EventStatusHandler : JIT.CPOS.BS.Web.PageBase.JITCPOSAjaxHandler, 
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
                case "EventStatus":
                default:
                    content = GetEventStatusData();
                    break;
            }
            pContext.Response.Write(content);
            pContext.Response.End();
        }

        #region GetEventStatusData
        /// <summary>
        /// 集合
        /// </summary>
        public string GetEventStatusData()
        {
            //var billService = new cBillService(new SessionManager().CurrentUserLoginInfo);
            IList<BillStatusModel> list = new List<BillStatusModel>();
            //list.Add(new BillStatusModel() { Id = "1", Description = "有效" });
            //list.Add(new BillStatusModel() { Id = "0", Description = "无效" });
            list.Add(new BillStatusModel() { Id = "10", Description = "未开始" });
            list.Add(new BillStatusModel() { Id = "20", Description = "运行中" });
            list.Add(new BillStatusModel() { Id = "30", Description = "暂停" });
            list.Add(new BillStatusModel() { Id = "40", Description = "结束" });
            string content = string.Empty;

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