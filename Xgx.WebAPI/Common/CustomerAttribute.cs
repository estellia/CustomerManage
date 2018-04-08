using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;

namespace Xgx.WebAPI.Common
{
    /// <summary>
    /// 获取 CustomerID
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CustomerAttribute : ActionFilterAttribute
    {
        public string CustomerID { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // 0011 
            //CustomerID = "b40cde63be3240f18302e001111a9cfb";
            //actionContext.ActionArguments.Add("__ClientID__", CustomerID);

            //
            try
            {
                
            }
            catch (Exception ex)
            {
               
                return;
            }

            //
            base.OnActionExecuting(actionContext);
        }

    }
}