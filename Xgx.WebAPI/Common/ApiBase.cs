using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Xgx.WebAPI.Common
{
    [Customer]
    public class ApiBase: ApiController
    {

        private string _CustomerID { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        protected string CustomerID
        {
            get
            {
                //return "";
                // return ActionContext.ActionArguments["__ClientID__"].ToString();
                var result = string.Empty;
                // throw new Exception("测试信息");
                //
                if (string.IsNullOrWhiteSpace(this._CustomerID))
                {
                    var caller = User as ClaimsPrincipal;
                    if (caller == null)
                    {
                        throw new Exception("未通过OAuth2认证用户!");
                    }
                    var subjectClaim = caller.FindFirst("sub");
                    if (subjectClaim == null)
                    {
                        throw new Exception("未通过OAuth2认证用户!");
                    }
                    var clientID = caller.FindFirst("CustomerID").Value;
                    if (string.IsNullOrWhiteSpace(clientID))
                    {
                        throw new Exception("缺少商户ID,无效调用!");
                    }
                    this._CustomerID = clientID;
                    result = clientID;
                }
                else
                {
                    result = this._CustomerID;
                }

                //
                return "223323";
            }
        }
    }
}