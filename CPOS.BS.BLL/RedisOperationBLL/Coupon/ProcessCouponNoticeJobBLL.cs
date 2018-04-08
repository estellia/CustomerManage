using JIT.CPOS.BS.BLL.WX;
using JIT.CPOS.BS.Entity.WX;
using RedisOpenAPIClient;
using RedisOpenAPIClient.Models;
using RedisOpenAPIClient.Models.CC.OrderPaySuccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisOpenAPIClient.MethodExtensions.ObjectExtensions;
using JIT.CPOS.BS.Entity;
using RedisOpenAPIClient.Models.CC.CouponNotice;

namespace JIT.CPOS.BS.BLL.RedisOperationBLL.Coupon
{
    public class ProcessCouponNoticeJobBLL
    {
        /// <summary>
        /// 出 订单支付完成 队列
        /// </summary>
        public void ProcessCouponNotice()
        {
            //一次最多取100条数据
            var numCount = 100;
            var commonBLL = new CommonBLL();
            //
            var customerIDs = CustomerBLL.Instance.GetCustomerList();
            foreach (var customer in customerIDs)
            {
                //
                var count = RedisOpenAPI.Instance.CCCouponNotice().GetCouponNoticeLength(new CC_CouponNotice
                {
                    CustomerID = customer.Key
                });
                if (count.Code != ResponseCode.Success)
                {
                    continue;
                }
                if (count.Result <= 0)
                {
                    continue;
                }

                //
                if (count.Result < numCount)
                {
                    numCount = Convert.ToInt32(count.Result);
                }

                //
                for (var i = 0; i < numCount; i++)
                {
                    //
                    var response = RedisOpenAPI.Instance.CCCouponNotice().GetCouponNotice(new CC_CouponNotice
                    {
                        CustomerID = customer.Key
                    });
                    if (response.Code == ResponseCode.Success)
                    {
                        var templateID = response.Result.ConfigData.TemplateID;
                        var openID = response.Result.ConfigData.OpenID;
                        var vipID = response.Result.ConfigData.VipID;
                        var loggingSessionInfo = CustomerBLL.Instance.GetBSLoggingSession(customer.Key, "1");//response.Result.ConfigData.LogSession.JsonDeserialize<LoggingSessionInfo>();
                  
                        var CouponsArrivalData = new CouponsArrival();
                        CouponsArrivalData.first = new DataInfo() { value = response.Result.CouponNoticeData.first.value, color = response.Result.CouponNoticeData.first.color };
                        CouponsArrivalData.keyword1 = new DataInfo() { value = response.Result.CouponNoticeData.keyword1.value, color = response.Result.CouponNoticeData.keyword1.color };//券码
                        CouponsArrivalData.keyword2 = new DataInfo() { value = response.Result.CouponNoticeData.keyword2.value, color = response.Result.CouponNoticeData.keyword2.color };//券名称
                        CouponsArrivalData.keyword3 = new DataInfo() { value = response.Result.CouponNoticeData.keyword3.value, color = response.Result.CouponNoticeData.keyword3.color };//可用数量
                        CouponsArrivalData.keyword4 = new DataInfo() { value = response.Result.CouponNoticeData.keyword4.value, color = response.Result.CouponNoticeData.keyword4.color };//有效期
                        CouponsArrivalData.keyword5 = new DataInfo() { value = response.Result.CouponNoticeData.keyword5.value, color = response.Result.CouponNoticeData.keyword5.color };//
                        CouponsArrivalData.Remark = new DataInfo() { value = response.Result.CouponNoticeData.remark.value, color = response.Result.CouponNoticeData.remark.color };
                        //
                        //return commonBLL.SendMatchWXTemplateMessage(wxTMConfigData.TemplateID, null, null, null, PaySuccessData, null, "15", OpenID, VipID, loggingSessionInfo);

                        commonBLL.SendMatchWXTemplateMessage(templateID, null, null, null, null, null, CouponsArrivalData, null, null, "3", openID, null, loggingSessionInfo);
                    }
                }
            }
        }
   
    }
}
