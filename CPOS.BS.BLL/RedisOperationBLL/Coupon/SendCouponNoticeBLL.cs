using JIT.CPOS.BS.BLL.Utility;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Entity.WX;
using RedisOpenAPIClient;
using RedisOpenAPIClient.Models;
using RedisOpenAPIClient.Models.CC.CouponToBeExpired;
using RedisOpenAPIClient.Models.CC.OrderNotPay;
using RedisOpenAPIClient.Models.CC.OrderPaySuccess;
using RedisOpenAPIClient.Models.CC.OrderSend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisOpenAPIClient.MethodExtensions.ObjectExtensions;
using RedisOpenAPIClient.Models.CC.CouponNotice;

namespace JIT.CPOS.BS.BLL.RedisOperationBLL.Coupon
{
    public class SendCouponNoticeBLL
    {
        public void CouponsArrivalMessage(string CouponCode, string CouponName, string ValidityData, string Scope, string OpenID, LoggingSessionInfo loggingSessionInfo, string CouponCount = "1")
        {

            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = "OPENTM207444083", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();


            var CouponsArrivalData = new CouponsArrival();
            CouponsArrivalData.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            CouponsArrivalData.keyword1 = new DataInfo() { value = CouponCode, color = WXTMConfigData.Colour1 };//券码
            CouponsArrivalData.keyword2 = new DataInfo() { value = CouponName, color = WXTMConfigData.Colour1 };//券名称
            CouponsArrivalData.keyword3 = new DataInfo() { value = CouponCount, color = WXTMConfigData.Colour1 };//可用数量
            CouponsArrivalData.keyword4 = new DataInfo() { value = ValidityData, color = WXTMConfigData.Colour1 };//有效期
            CouponsArrivalData.keyword5 = new DataInfo() { value = Scope, color = WXTMConfigData.Colour1 };//
            CouponsArrivalData.Remark = new DataInfo() { value = WXTMConfigData.RemarkText, color = WXTMConfigData.RemarkColour };
            
            //下面往redis里存入数据
            var response = RedisOpenAPI.Instance.CCCouponNotice().SetCouponNotice(new CC_CouponNotice
            {
                CustomerID = loggingSessionInfo.ClientID,
                ConfigData = new CC_ConfigData
                {
                    LogSession = loggingSessionInfo.JsonSerialize(),
                    OpenID = OpenID,
                    TemplateID = WXTMConfigData.TemplateID,
                    //   VipID = VipID
                },
                CouponNoticeData = new CC_CouponNoticeData
                {
                    first = new CC_DataInfo { value = CouponsArrivalData.first.value, color = CouponsArrivalData.first.color },
                    keyword1 = new CC_DataInfo { value = CouponsArrivalData.keyword1.value, color = CouponsArrivalData.keyword1.color },
                    keyword2 = new CC_DataInfo { value = CouponsArrivalData.keyword2.value, color = CouponsArrivalData.keyword2.color },
                    keyword3 = new CC_DataInfo { value = CouponsArrivalData.keyword3.value, color = CouponsArrivalData.keyword3.color },
                    keyword4 = new CC_DataInfo { value = CouponsArrivalData.keyword4.value, color = CouponsArrivalData.keyword4.color },
                    keyword5 = new CC_DataInfo { value = CouponsArrivalData.keyword5.value, color = CouponsArrivalData.keyword4.color },
                    remark = new CC_DataInfo { value = CouponsArrivalData.Remark.value, color = CouponsArrivalData.Remark.color }
                }
            });
            //如果往缓存redis里写入不成功，还是按照原来的老方法直接发送**
            if (response.Code != ResponseCode.Success)
            {
                CommonBLL.CouponsArrivalMessage(CouponCode, CouponName, ValidityData, Scope, OpenID, loggingSessionInfo, CouponCount);//传入优惠券数量

                new RedisXML().RedisReadDBCount("CouponsUpcomingExpired", "优惠券发送成功通知", 2);
            }
            else
            {
                new RedisXML().RedisReadDBCount("CouponsUpcomingExpired", "优惠券发送成功通知", 1);

            }

        }
    }
}
