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
    /// ESalesProductHandler 的摘要说明
    /// </summary>
    public class ESalesProductHandler : JIT.CPOS.BS.Web.PageBase.JITCPOSAjaxHandler,
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
                case "get_list":
                     content = GetListData();
                    break;
                default:
                    content = GetListData();
                    break;
            }
            pContext.Response.Write(content);
            pContext.Response.End();
        }

        #region GetListData
        /// <summary>
        /// 获取列表
        /// </summary>
        public string GetListData()
        {
            var service = new ESalesProductBLL(new SessionManager().CurrentUserLoginInfo);
            IList<ESalesProductEntity> data = new List<ESalesProductEntity>();
            string content = string.Empty;
            data = service.QueryByEntity(new ESalesProductEntity
            {
                CustomerId = new SessionManager().CurrentUserLoginInfo.CurrentUser.customer_id.ToString()
            }, null).ToList();
            
            var jsonData = new JsonData();
            jsonData.totalCount = data.Count.ToString();
            jsonData.data = data;

            content = jsonData.ToJSON();
            return content;
        }
        #endregion

       
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}