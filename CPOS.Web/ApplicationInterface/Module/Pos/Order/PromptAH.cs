using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.BLL.CodeGeneration.Order;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.DataAccess.Base;


namespace JIT.CPOS.Web.ApplicationInterface.Module.Pos.Order
{
    public class PromptAH : BaseActionHandler<EmptyRequestParameter,EmptyResponseData>
    {
        protected override EmptyResponseData ProcessRequest(APIRequest<EmptyRequestParameter> pRequest)
        {
            EmptyResponseData rd = new EmptyResponseData();
            var vipAmountBll = new VipAmountBLL(CurrentUserInfo);
            if (!string.IsNullOrEmpty(pRequest.UserID))
            {
                var vipAmountEntity = vipAmountBll.QueryByEntity(new VipAmountEntity() { VipId = pRequest.UserID }, null).FirstOrDefault();
                if (vipAmountEntity != null || vipAmountEntity.EndAmount == 0)
                {
                    if (vipAmountEntity.PayPassword == null)
                    {
                        throw new APIException("客户尚未设置密码，请提示客户到会员中心设置密码") { ErrorCode = 100 };
                    }
                }
            }
            return rd;
        }
    }
}