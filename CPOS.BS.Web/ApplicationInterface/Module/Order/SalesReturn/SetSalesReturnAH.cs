﻿using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.BLL.WX;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Order.SalesReturn.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Order.SalesReturn
{
    public class SetSalesReturnAH : BaseActionHandler<SetSalesReturnRP, EmptyResponseData>
    {
        protected override EmptyResponseData ProcessRequest(DTO.Base.APIRequest<SetSalesReturnRP> pRequest)
        {
            var rd = new EmptyResponseData();
            var para = pRequest.Parameters;
            var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;
            var salesReturnBLL = new T_SalesReturnBLL(loggingSessionInfo);
            var historyBLL = new T_SalesReturnHistoryBLL(loggingSessionInfo);
            var refundOrderBLL = new T_RefundOrderBLL(loggingSessionInfo);

            var inoutBLL = new T_InoutBLL(loggingSessionInfo);
            var inoutService = new InoutService(loggingSessionInfo);

            var vipAmountBLL = new VipAmountBLL(loggingSessionInfo);  //余额返现BLL实例化
            var vipIntegralBLL = new VipIntegralBLL(loggingSessionInfo);    //积分BLL实例化

            var pTran = salesReturnBLL.GetTran();//事务
            T_SalesReturnEntity salesReturnEntity = null;
            T_SalesReturnHistoryEntity historyEntity = null;

            T_RefundOrderEntity refundEntity = null;

            var vipBll = new VipBLL(loggingSessionInfo);        //会员BLL实例化
            var userBll = new T_UserBLL(loggingSessionInfo);    //店员BLL实例化
            T_UserEntity userEntity = null;   //店员信息
            var unitBLL = new t_unitBLL(loggingSessionInfo);

            salesReturnEntity = salesReturnBLL.GetByID(para.SalesReturnID);
            //获取订单信息
            var inoutInfo = inoutBLL.GetInoutInfo(salesReturnEntity.OrderID, loggingSessionInfo);
            //获取会员信息
            var vipInfo = vipBll.GetByID(inoutInfo.vip_no);
            //获取门店信息
            t_unitEntity unitInfo = null;
            if (!string.IsNullOrEmpty(inoutInfo.sales_unit_id))
                unitInfo = unitBLL.GetByID(inoutInfo.sales_unit_id);
            userEntity = userBll.GetByID(loggingSessionInfo.UserID);
            using (pTran.Connection)
            {
                try
                {
                    switch (para.OperationType)
                    {
                        case 1:
                            #region 审核通过
                            salesReturnEntity.Status = 4;//审核通过

                            #region 计算应退金额
                            decimal payable = 0;   //计算后应退金额

                            //根据订单ID获取订单明细[复用]
                            DataRow drItem = inoutService.GetOrderDetailByOrderId(salesReturnEntity.OrderID).Tables[0].Select(" item_id= '" + salesReturnEntity.ItemID + "'").FirstOrDefault();
                            decimal salesPrice = Convert.ToDecimal(drItem["enter_price"]);
                            if (inoutInfo != null)
                            {
                                //订单总金额-运费
                                decimal tempAmount = inoutInfo.total_amount.Value - inoutInfo.DeliveryAmount;
                                //计算会员折扣
                                decimal discount = 1;
                                if (inoutInfo.discount_rate.Value > 0)
                                    discount = inoutInfo.discount_rate.Value / 100;
                                //计算后应退金额
                                //payable = (((tempAmount - inoutInfo.CouponAmount) * discount) / tempAmount) * (salesPrice * salesReturnEntity.ActualQty.Value);
                                //公式：比例 =（应付金额 * 会员折扣 - 优惠券）/ 应付金额
                                //      应退金额 = 退货数量 * 销售价 * 比例
                                payable = ((tempAmount * discount - inoutInfo.CouponAmount) / tempAmount) * (salesPrice * salesReturnEntity.ActualQty.Value);
                            }
                            salesReturnEntity.RefundAmount = payable;   //应退金额
                            salesReturnEntity.ConfirmAmount = payable;  //实退金额，默认为应退金额，可支持修改
                            #endregion

                            salesReturnBLL.Update(salesReturnEntity, pTran);

                            historyEntity = new T_SalesReturnHistoryEntity()
                            {
                                SalesReturnID = salesReturnEntity.SalesReturnID,
                                OperationType = 4,
                                OperationDesc = "审核",
                                OperatorID = loggingSessionInfo.UserID,
                                HisRemark = "您的服务单已审核通过，请将商品寄回",
                                OperatorName = userEntity.user_name,
                                OperatorType = 1
                            };
                            historyBLL.Create(historyEntity, pTran);

                            #endregion
                            break;
                        case 2:
                            #region 审核不通过
                            salesReturnEntity.Status = 3;//审核不通过
                            salesReturnBLL.Update(salesReturnEntity, pTran);

                            historyEntity = new T_SalesReturnHistoryEntity()
                            {
                                SalesReturnID = salesReturnEntity.SalesReturnID,
                                OperationType = 3,
                                OperationDesc = "审核",
                                OperatorID = loggingSessionInfo.UserID,
                                HisRemark = para.Desc,
                                OperatorName = userEntity.user_name,
                                OperatorType = 1
                            };
                            historyBLL.Create(historyEntity, pTran);
                            break;
                        #endregion
                        case 3:
                            #region 确认收货
                            salesReturnEntity.Status = 6;//确认收货
                            salesReturnBLL.Update(salesReturnEntity, pTran);

                            historyEntity = new T_SalesReturnHistoryEntity()
                            {
                                SalesReturnID = salesReturnEntity.SalesReturnID,
                                OperationType = 6,
                                OperationDesc = "收货",
                                OperatorID = loggingSessionInfo.UserID,
                                HisRemark = "您的服务单" + salesReturnEntity.SalesReturnNo + "的商品已经收到",
                                OperatorName = userEntity.user_name,
                                OperatorType = 1
                            };
                            historyBLL.Create(historyEntity, pTran);
                            //退货时生成退款单
                            if (salesReturnEntity.ServicesType == 1)
                            {
                                refundEntity = new T_RefundOrderEntity();
                                refundEntity.SalesReturnID = salesReturnEntity.SalesReturnID;
                                refundEntity.RefundNo = DateTime.Now.ToString("yyyyMMddhhmmfff");
                                refundEntity.VipID = salesReturnEntity.VipID;
                                refundEntity.DeliveryType = salesReturnEntity.DeliveryType;
                                refundEntity.OrderID = salesReturnEntity.OrderID;
                                refundEntity.ItemID = salesReturnEntity.ItemID;
                                refundEntity.SkuID = salesReturnEntity.SkuID;
                                refundEntity.Qty = salesReturnEntity.Qty;
                                refundEntity.ActualQty = salesReturnEntity.ActualQty;
                                refundEntity.UnitID = salesReturnEntity.UnitID;
                                refundEntity.UnitName = salesReturnEntity.UnitName;
                                refundEntity.UnitTel = salesReturnEntity.UnitTel;
                                refundEntity.Address = salesReturnEntity.Address;
                                refundEntity.Contacts = salesReturnEntity.Contacts;
                                refundEntity.Phone = salesReturnEntity.Phone;
                                refundEntity.RefundAmount = salesReturnEntity.RefundAmount;     //退款金额
                                refundEntity.ConfirmAmount = salesReturnEntity.ConfirmAmount;   //确认退款金额

                                #region 计算应退现金金额、余额、积分、返现
                                var inoutDetail = inoutBLL.GetInoutInfo(salesReturnEntity.OrderID, loggingSessionInfo);
                                if (inoutDetail != null)
                                {
                                    //订单实付金额-运费 >= 应退金额
                                    if (inoutDetail.actual_amount - inoutDetail.DeliveryAmount >= salesReturnEntity.ConfirmAmount)
                                    {
                                        refundEntity.ActualRefundAmount = salesReturnEntity.ConfirmAmount;
                                    }
                                    //订单实付金额-运费+余额抵扣 >= 应退金额
                                    else if (inoutDetail.actual_amount - inoutDetail.DeliveryAmount + inoutDetail.VipEndAmount >= salesReturnEntity.ConfirmAmount)
                                    {
                                        refundEntity.ActualRefundAmount = inoutDetail.actual_amount - inoutDetail.DeliveryAmount;  //实付金额
                                        //refundEntity.Amount = salesReturnEntity.ConfirmAmount - inoutDetail.actual_amount;  //退回余额
                                        refundEntity.Amount = salesReturnEntity.ConfirmAmount - refundEntity.ActualRefundAmount;  //退回余额
                                    }
                                    //订单实付金额-运费+余额抵扣+积分抵扣 >= 应退金额
                                    else if (inoutDetail.actual_amount - inoutDetail.DeliveryAmount + inoutDetail.VipEndAmount + inoutDetail.IntegralAmount >= salesReturnEntity.ConfirmAmount)
                                    {
                                        refundEntity.ActualRefundAmount = inoutDetail.actual_amount - inoutDetail.DeliveryAmount;  //实付金额
                                        refundEntity.Amount = inoutDetail.VipEndAmount;  //退回余额
                                        //退回积分抵扣金额
                                        refundEntity.PointsAmount = salesReturnEntity.ConfirmAmount.Value - (inoutDetail.actual_amount.Value - inoutDetail.DeliveryAmount) - inoutDetail.VipEndAmount;
                                        //退回积分
                                        refundEntity.Points = (int)Math.Round(refundEntity.PointsAmount.Value * (inoutDetail.pay_points.Value / inoutDetail.IntegralAmount), 1);

                                    }
                                    else//订单实付金额-运费+余额抵扣+积分抵扣 < 应退金额
                                    {
                                        refundEntity.Points = (int)Math.Round(inoutDetail.pay_points.Value, 1);                     //退回积分
                                        refundEntity.PointsAmount = inoutDetail.IntegralAmount;                                     //退回积分抵扣金额
                                        refundEntity.Amount = inoutDetail.VipEndAmount;                                             //退回余额
                                        refundEntity.ActualRefundAmount = salesReturnEntity.ConfirmAmount;   //实付金额
                                    }
                                    //订单实付金额-运费+余额抵扣+积分抵扣 >= 应退金额
                                    //取消返现
                                    //else if (inoutDetail.actual_amount - inoutDetail.DeliveryAmount + inoutDetail.VipEndAmount + inoutDetail.IntegralAmount + inoutDetail.ReturnAmount >= salesReturnEntity.ConfirmAmount)
                                    //{
                                    //    refundEntity.ActualRefundAmount = inoutDetail.actual_amount - inoutDetail.DeliveryAmount;//实付金额
                                    //    refundEntity.Amount = inoutDetail.VipEndAmount;             //退回余额
                                    //    refundEntity.Points = (int)Math.Round(inoutDetail.pay_points.Value, 1);//退回积分
                                    //    refundEntity.PointsAmount = inoutDetail.IntegralAmount;       //退回积分抵扣金额
                                    //    //退回的返现
                                    //    refundEntity.ReturnAmount = salesReturnEntity.ConfirmAmount.Value - (inoutDetail.actual_amount.Value - inoutDetail.DeliveryAmount) - inoutDetail.VipEndAmount - inoutDetail.IntegralAmount;
                                    //}
                                }
                                #endregion

                                refundEntity.Status = 1;      //待退款
                                refundEntity.CustomerID = loggingSessionInfo.ClientID;
                                refundOrderBLL.Create(refundEntity, pTran);

                                var vipAmountEntity = vipAmountBLL.QueryByEntity(new VipAmountEntity() { VipId = vipInfo.VIPID, VipCardCode = vipInfo.VipCode }, null).FirstOrDefault();

                                //退货返回余额
                                if (refundEntity.Amount > 0)
                                {
                                    //变更余额和余额记录
                                    var detailInfo = new VipAmountDetailEntity()
                                    {
                                        Amount = refundEntity.Amount.Value,
                                        AmountSourceId = "21",
                                        ObjectId = refundEntity.RefundID.ToString()
                                    };
                                    var vipAmountDetailId = vipAmountBLL.AddVipAmount(vipInfo, unitInfo, ref vipAmountEntity, detailInfo, pTran, loggingSessionInfo);
                                    if (!string.IsNullOrWhiteSpace(vipAmountDetailId))
                                    {//发送微信账户余额变动模板消息
                                        var CommonBLL = new CommonBLL();
                                        CommonBLL.BalanceChangedMessage(inoutInfo.order_no, vipAmountEntity, detailInfo, vipInfo.WeiXinUserId, vipInfo.VIPID, loggingSessionInfo);
                                    }
                                }
                                //退货返回返现
                                if (refundEntity.ReturnAmount > 0)
                                {
                                    var detailInfo = new VipAmountDetailEntity()
                                    {
                                        Amount = refundEntity.ReturnAmount.Value,
                                        ObjectId = refundEntity.RefundID.ToString(),
                                        AmountSourceId = "22"
                                    };
                                    var vipAmountDetailId = vipAmountBLL.AddReturnAmount(vipInfo, unitInfo, vipAmountEntity, ref detailInfo, pTran, loggingSessionInfo);
                                    if (!string.IsNullOrWhiteSpace(vipAmountDetailId))
                                    {//发送返现到账通知微信模板消息
                                        var CommonBLL = new CommonBLL();
                                        CommonBLL.CashBackMessage(salesReturnEntity.OrderNo, detailInfo.Amount, vipInfo.WeiXinUserId, vipInfo.VIPID, loggingSessionInfo);

                                    }
                                }
                                //退货返回积分
                                if (refundEntity.Points > 0)
                                {
                                    var IntegralDetail = new VipIntegralDetailEntity()
                                    {
                                        Integral = refundEntity.Points.Value,
                                        IntegralSourceID = "26",
                                        ObjectId = refundEntity.RefundID.ToString()
                                    };

                                    if (IntegralDetail.Integral != 0)
                                    {
                                        //变动前积分
                                        string OldIntegral = (vipInfo.Integration ?? 0).ToString();
                                        //变动积分
                                        string ChangeIntegral = (IntegralDetail.Integral ?? 0).ToString();
                                        var vipIntegralDetailId = vipIntegralBLL.AddIntegral(ref vipInfo, unitInfo, IntegralDetail, pTran, loggingSessionInfo);
                                        //发送微信积分变动通知模板消息
                                        if (!string.IsNullOrWhiteSpace(vipIntegralDetailId))
                                        {
                                            var CommonBLL = new CommonBLL();
                                            CommonBLL.PointsChangeMessage(OldIntegral, vipInfo, ChangeIntegral, vipInfo.WeiXinUserId, loggingSessionInfo);
                                        }
                                    }

                                }
                                //确认收货时退回订单奖励积分、返现和佣金
                                vipIntegralBLL.CancelReward(inoutInfo, vipInfo, pTran);
                            }
                            #endregion
                            break;
                        case 4:
                            #region 拒绝收货
                            salesReturnEntity.Status = 5;//拒绝收货
                            salesReturnBLL.Update(salesReturnEntity, pTran);

                            historyEntity = new T_SalesReturnHistoryEntity()
                            {
                                SalesReturnID = salesReturnEntity.SalesReturnID,
                                OperationType = 5,
                                OperationDesc = "收货",
                                OperatorID = loggingSessionInfo.UserID,
                                HisRemark = para.Desc,
                                OperatorName = userEntity.user_name,
                                OperatorType = 1
                            };
                            historyBLL.Create(historyEntity, pTran);
                            #endregion
                            break;
                        case 5:
                            #region 修改信息
                            if (para.ServicesType > 0) //修改服务方式
                            {
                                salesReturnEntity.ServicesType = para.ServicesType;
                                string servicesTypeDesc = para.ServicesType == 1 ? "退货" : "换货";
                                historyEntity = new T_SalesReturnHistoryEntity()
                                {
                                    SalesReturnID = salesReturnEntity.SalesReturnID,
                                    OperationType = 11,
                                    OperationDesc = "修改信息",
                                    OperatorID = loggingSessionInfo.UserID,
                                    HisRemark = "修改服务方式为" + servicesTypeDesc,
                                    OperatorName = userEntity.user_name,
                                    OperatorType = 1,
                                    IsDelete = 1
                                };
                                historyBLL.Create(historyEntity, pTran);
                            }
                            if (para.ActualQty > 0)//修改申请数量
                            {
                                salesReturnEntity.ActualQty = para.ActualQty;
                                historyEntity = new T_SalesReturnHistoryEntity()
                                {
                                    SalesReturnID = salesReturnEntity.SalesReturnID,
                                    OperationType = 11,
                                    OperationDesc = "修改信息",
                                    OperatorID = loggingSessionInfo.UserID,
                                    HisRemark = "修改确认退货数量为" + para.ActualQty,
                                    OperatorName = userEntity.user_name,
                                    OperatorType = 1,
                                    IsDelete = 1
                                };
                                historyBLL.Create(historyEntity, pTran);
                            }
                            if (para.ConfirmAmount >= 0)//处理实退金额
                            {
                                salesReturnEntity.ConfirmAmount = para.ConfirmAmount;

                                historyEntity = new T_SalesReturnHistoryEntity()
                                {
                                    SalesReturnID = salesReturnEntity.SalesReturnID,
                                    OperationType = 13,
                                    OperationDesc = "修改信息",
                                    OperatorID = loggingSessionInfo.UserID,
                                    HisRemark = "修改实退金额为" + para.ConfirmAmount,
                                    OperatorName = userEntity.user_name,
                                    OperatorType = 1,
                                    IsDelete = 1
                                };
                                historyBLL.Create(historyEntity, pTran);
                            }
                            salesReturnBLL.Update(salesReturnEntity, pTran);
                            #endregion
                            break;
                        case 6:
                            #region 取消
                            salesReturnEntity.Status = 2;//取消申请
                            salesReturnBLL.Update(salesReturnEntity, pTran);
                            historyEntity = new T_SalesReturnHistoryEntity()
                            {
                                SalesReturnID = salesReturnEntity.SalesReturnID,
                                OperationType = 2,
                                OperationDesc = "取消申请",
                                OperatorID = CurrentUserInfo.UserID,
                                HisRemark = para.Desc,
                                OperatorName = userEntity.user_name,
                                OperatorType = 0
                            };
                            historyBLL.Create(historyEntity, pTran);
                            #endregion
                            break;
                        default:
                            break;
                    }
                    pTran.Commit();  //提交事物
                }
                catch (Exception ex)
                {
                    pTran.Rollback();//回滚事务
                    throw new APIException(ex.Message);
                }
            }
            return rd;


        }
    }
}