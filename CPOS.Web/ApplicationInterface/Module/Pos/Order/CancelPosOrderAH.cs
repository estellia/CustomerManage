using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;

using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.BLL.CodeGeneration.Order;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Pos.Order.Request;
using JIT.CPOS.DTO.Module.Pos.Order.Response;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.DataAccess.Base;
using JIT.CPOS.BS.BLL.WX;
using RedisOpenAPIClient;
using RedisOpenAPIClient.Models;
using RedisOpenAPIClient.Models.CC.OrderPaySuccess;
using RedisOpenAPIClient.Models.CC.OrderReward;

namespace JIT.CPOS.Web.ApplicationInterface.Module.Pos.Order
{
    public class CancelPosOrderAH : BaseActionHandler<CancelPosOrderRP,EmptyResponseData>
    {
        protected override EmptyResponseData ProcessRequest(APIRequest<CancelPosOrderRP> pRequest)
        {
            var rp = pRequest.Parameters;
            var rd = new EmptyResponseData();
            var vipBll = new VipBLL(CurrentUserInfo);                    //会员业务实例化
            var inoutBLL = new T_InoutBLL(CurrentUserInfo);
            var vipIntegralBLL = new VipIntegralBLL(CurrentUserInfo);    //积分BLL实例化


        
            var count = RedisOpenAPI.Instance.CCOrderReward().GetOrderRewardLength(new CC_OrderReward
            {
                CustomerID = CurrentUserInfo.ClientID,
            });
            if(count.Result > 0)
            {
                throw new APIException("队列正在运行，请稍后再试。。") { ErrorCode = 100 };
            }


            //获取订单信息
            var inoutInfo = inoutBLL.GetInoutInfo(rp.OrderId, CurrentUserInfo);
            if (inoutInfo == null)
            {
                throw new APIException("未找到该订单信息") { ErrorCode = 101 };
            }
            //获取会员信息
            var vipInfo = vipBll.GetByID(inoutInfo.vip_no);
            
            //处理积分、余额、返现和优惠券
            vipBll.ProcSetCancelOrder(CurrentUserInfo.ClientID, rp.OrderId, pRequest.UserID);
            //取消订单奖励
            vipIntegralBLL.CancelReward(inoutInfo, vipInfo, null);

            return rd;
        }
    }
}