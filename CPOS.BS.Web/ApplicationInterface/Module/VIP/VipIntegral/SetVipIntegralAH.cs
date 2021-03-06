﻿using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.BLL.WX;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.VIP.VipIntegral.Request;
using JIT.Utility.DataAccess.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.VIP.VipIntegral
{
    public class SetVipIntegralAH : BaseActionHandler<SetVipIntegralRP, EmptyResponseData>
    {
        protected override EmptyResponseData ProcessRequest(APIRequest<SetVipIntegralRP> pRequest)
        {
            var rd = new EmptyResponseData();
            var para = pRequest.Parameters;
            var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo; //登录状态信息
            var vipIntegralDetail = new VipIntegralDetailBLL(loggingSessionInfo);//积分变更业务对象实例化
            var objectImagesBLL = new ObjectImagesBLL(loggingSessionInfo);       //图片表业务对象实例化
            var unitBLL = new t_unitBLL(loggingSessionInfo);
            var vipIntegralBLL = new VipIntegralBLL(loggingSessionInfo);
            var vipBLL = new VipBLL(loggingSessionInfo);

            var vipInfo = vipBLL.GetByID(para.VipID);
            var unitInfo = unitBLL.GetByID(loggingSessionInfo.CurrentUserRole.UnitId);
            var pTran = vipIntegralDetail.GetTran();   //事务  
            var vipIntegral = vipIntegralBLL.GetByID(para.VipID);
            using (pTran.Connection)
            {
                try
                {
                    var IntegralDetail = new VipIntegralDetailEntity()
                    {
                        Integral = para.Qty,
                        IntegralSourceID = para.IntegralSourceID,
                        ObjectId = "",
                        Reason = para.Reason,
                        Remark = para.Remark
                    };
                    if (IntegralDetail.Integral != 0)
                    {
                        //变动前积分
                        string OldIntegral = string.Empty;
                        if (vipIntegral != null)
                        {
                            OldIntegral = (vipIntegral.ValidIntegral ?? 0).ToString();
                        }
                        else
                        {
                            OldIntegral = "0";
                        }
                        
                        //变动积分
                        string ChangeIntegral = (IntegralDetail.Integral ?? 0).ToString();
                        string vipIntegralDetailId = vipIntegralBLL.AddIntegral(ref vipInfo, unitInfo, IntegralDetail, pTran, loggingSessionInfo);
                        //发送微信积分变动通知模板消息
                        if (!string.IsNullOrWhiteSpace(vipIntegralDetailId))
                        {
                            var CommonBLL = new CommonBLL();
                            CommonBLL.PointsChangeMessage(OldIntegral, vipInfo, ChangeIntegral, vipInfo.WeiXinUserId, loggingSessionInfo);
                        }
                        //增加图片上传
                        if (!string.IsNullOrEmpty(para.ImageUrl))
                        {
                            var objectImagesEntity = new ObjectImagesEntity()
                            {
                                ImageId = Guid.NewGuid().ToString(),
                                ObjectId = vipIntegralDetailId,
                                ImageURL = para.ImageUrl
                            };
                            objectImagesBLL.Create(objectImagesEntity);
                        }
                    }    
                    pTran.Commit();  //提交事物
                }
                catch (APIException apiEx)
                {
                    pTran.Rollback();//回滚事物
                    throw new APIException(apiEx.ErrorCode, apiEx.Message);
                }
                catch (Exception ex)
                {
                    pTran.Rollback();//回滚事物
                    throw new Exception(ex.Message);
                }
            }
            return rd;
        }
    }
}