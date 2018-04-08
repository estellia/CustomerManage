using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.Utility.Log;
using Xgx.WebAPI.Areas.Xgx.Models;
using Xgx.WebAPI.Common;

namespace Xgx.WebAPI.Areas.Xgx.Controllers
{
    public class IntegralController : ApiBase
    {
        [HttpGet]
        public HttpResponseMessage Amount(string vipId, decimal amount)
        {
            Loggers.Debug(new DebugLogInfo()
            {
                Message = $"[api/Integral/Amount]接口，参数：\"vipId:{vipId}&&amount:{amount}\""
            });
            try
            {
                string customerid = ConfigurationManager.AppSettings["CustomerId"].Trim();
                var loggingSessionInfo = Default.GetLoggingSession(customerid, vipId);
                loggingSessionInfo.Conn = ConfigurationManager.AppSettings["Conn"].Trim();
                var bll = new VipBLL(loggingSessionInfo);
                var vipInfo = bll.GetByID(vipId); //会员信息

                var integralAmountResponseModel = new IntegralAmountResponseModel();

                //获取社会化销售配置和积分返现配置
                var basicSettingBll = new CustomerBasicSettingBLL(loggingSessionInfo);
                Hashtable htSetting = basicSettingBll.GetSocialSetting();

                //2.获取会员的积分和账户余额
                var vipIntegralbll = new VipIntegralBLL(loggingSessionInfo);
                //var vipIntegralEntity = vipIntegralbll.GetByID(rp.UserID);
                //根据会员和会员卡号获取积分
                var vipIntegralEntity =
                    vipIntegralbll.QueryByEntity(
                            new VipIntegralEntity() {VipID = vipInfo.UserId, VipCardCode = vipInfo.VipCode}, null)
                        .FirstOrDefault();
                if (vipIntegralEntity == null)
                {
                    integralAmountResponseModel.Integral = 0;
                    integralAmountResponseModel.IntegralAmount = 0;
                }
                else
                {
                    decimal validIntegral = vipIntegralEntity.ValidIntegral ?? 0; //会员积分

                    int totalIntegral = 0; //可使用积分(取整)                
                    //if (int.Parse(htSetting["rewardsType"].ToString()) == 1)//按商品奖励
                    //    totalIntegral = (int)Math.Round(bll.GetIntegralBySkuId(skuIdList), 1);

                    //积分使用上限比例
                    decimal pointsRedeemUpLimit = decimal.Parse(htSetting["pointsRedeemUpLimit"].ToString())/100;
                    //3.获取积分与金额的兑换比例
                    var integralAmountPre = bll.GetIntegralAmountPre(customerid);
                    if (integralAmountPre == 0)
                        integralAmountPre = (decimal) 0.01;

                    totalIntegral = (int) Math.Round(amount*pointsRedeemUpLimit*integralAmountPre, 1);
                    //可使用的积分
                    integralAmountResponseModel.Integral = validIntegral > totalIntegral ? totalIntegral : validIntegral;

                    if (amount == 0)
                    {
                        integralAmountResponseModel.Integral = Convert.ToDecimal(vipIntegralEntity.ValidIntegral);
                    }

                    //rd.IntegralAmount = rd.Integral * integralAmountPre;
                    integralAmountResponseModel.IntegralAmount = bll.GetAmountByIntegralPer(loggingSessionInfo.ClientID,
                        integralAmountResponseModel.Integral);
                    integralAmountResponseModel.IntegralDesc = "使用积分" +
                                                               integralAmountResponseModel.Integral.ToString("0") +
                                                               ",可兑换"
                                                               +
                                                               integralAmountResponseModel.IntegralAmount.ToString(
                                                                   "0.00") + "元";

                }
                integralAmountResponseModel.IsSucess = true;
                return Request.CreateResponse(HttpStatusCode.OK, integralAmountResponseModel);

            }
            catch (Exception ex)
            {
                var response = new IntegralAmountResponseModel()
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
