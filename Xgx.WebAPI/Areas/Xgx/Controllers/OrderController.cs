using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Xgx.WebAPI.Areas.Xgx.Models;
using Xgx.WebAPI.Common;
using JIT.CPOS.BS.BLL;
using System.Collections;
using System.Data;
using JIT.CPOS.BS.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using JIT.CPOS.BS.BLL.WX;
using JIT.CPOS.BS.DataAccess;
using JIT.CPOS.BS.DataAccess.Base;
using JIT.CPOS.DTO.Base;
using JIT.Utility.DataAccess.Query;
using JIT.Utility.Log;
using InoutService = JIT.CPOS.BS.BLL.InoutService;

namespace Xgx.WebAPI.Areas.Xgx.Controllers
{
    public class SkuIdAndQtyInfo
    {
        public string SkuId { get; set; }
        public int Qty { get; set; }
    }


    public class OrderController : ApiBase
    {
        private T_InoutDAO _currentDAO;

        [HttpPost]
        public HttpResponseMessage Payment([FromBody] OrderPaymentRequestModel requestParameter)
        {
            var json = new JavaScriptSerializer().Serialize(requestParameter);
            string guid = Guid.NewGuid().ToString();
            Loggers.Debug(new DebugLogInfo()
            {
                Message = $"调用[api/Order/Payment]，requestParameter:{json}",
                UserID = requestParameter.OrderContract.VipNo,
                ClientID = guid
            });

            try
            {
                string customerid = ConfigurationManager.AppSettings["CustomerId"].Trim();
                var loggingSessionInfo = Default.GetLoggingSession(customerid, requestParameter.OrderContract.VipNo);
                var tInoutBll = new TInoutBLL(loggingSessionInfo); //订单业务表

                if (string.IsNullOrEmpty(requestParameter.OrderContract.VipNo))
                {
                    var entity = tInoutBll.GetByID(requestParameter.OrderContract.OrderId);
                    loggingSessionInfo = Default.GetLoggingSession(customerid, entity.VipNo);
                }
                loggingSessionInfo.Conn = ConfigurationManager.AppSettings["Conn"].Trim();

                var remainingPay = 0m;
                var orderContract = requestParameter.OrderContract;
                switch (orderContract.Operation)
                {
                    case OptEnum.Create:
                        remainingPay = CreateOrder(requestParameter, loggingSessionInfo);
                        break;
                    case OptEnum.Update:
                        remainingPay = UpdateOrder(requestParameter, loggingSessionInfo);
                        break;
                    case OptEnum.Delete:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var paymentDetailBll = new T_Payment_detailBLL(loggingSessionInfo);

                List<IWhereCondition> complexCondition = new List<IWhereCondition> { };
                complexCondition.Add(new EqualsCondition() { FieldName = "Inout_Id", Value = requestParameter.OrderContract.OrderId });
                var paymentDetialList = paymentDetailBll.Query(complexCondition.ToArray(), null);
                var payMethodList = new List<PayDetail>();

                foreach (var tmp in paymentDetialList)
                {
                    int displayIndex = 0;
                    EnumPayMethod payMethod = EnumPayMethod.StorePay;
                    if (tmp.Payment_Type_Code == EnumPayMethod.BalancePay.ToString())
                    {
                        displayIndex = 3;
                        payMethod = EnumPayMethod.BalancePay;
                    }
                    else if (tmp.Payment_Type_Code == EnumPayMethod.CouponPay.ToString())
                    {
                        displayIndex = 2;
                        payMethod = EnumPayMethod.CouponPay;

                    }
                    else if (tmp.Payment_Type_Code == EnumPayMethod.IntegralPay.ToString())
                    {
                        displayIndex = 1;
                        payMethod = EnumPayMethod.IntegralPay;
                    }
                    else if (tmp.Payment_Type_Code == EnumPayMethod.OfflinePay.ToString())
                    {
                        displayIndex = 4;
                        payMethod = EnumPayMethod.OfflinePay;
                    }
                    else if (tmp.Payment_Type_Code == EnumPayMethod.StorePay.ToString())
                    {
                        displayIndex = 0;
                        payMethod = EnumPayMethod.StorePay;
                    }
                    else
                    {

                    }
                    var payDetail = new PayDetail
                    {
                        DisplayIndex = displayIndex,
                        Amount = tmp.Price.Value,
                        PayMethod = payMethod,
                        PayTime = tmp.CreateTime.Value.ToString("yyyy-MMMM-dd")
                    };
                    payMethodList.Add(payDetail);
                }

                var responseModel = new OrderPaymentResponseModel
                {
                    IsSucess = true,
                    PayDetailList = payMethodList,
                    ErrorMessage = string.Empty,
                    RemainingPayment = remainingPay
                };

                var returnjson = new JavaScriptSerializer().Serialize(responseModel);
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = $"调用[api/Order/Payment]，返回参数：\"{returnjson}\"",
                    UserID = requestParameter.OrderContract.VipNo,
                    ClientID = guid
                });

                return Request.CreateResponse(HttpStatusCode.OK, responseModel);
            }
            catch (Exception ex)
            {
                var responseModel = new OrderPaymentResponseModel
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = $"[api/Order/Payment]错误，参数:{ex.Message}",
                    UserID = requestParameter.OrderContract.VipNo,
                    ClientID = guid
                });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage Done(OrderDoneRequestModel requestParameter)
        {
            var json = new JavaScriptSerializer().Serialize(requestParameter);
            Loggers.Debug(new DebugLogInfo()
            {
                Message = $"[api/Order/Done]接口，参数:{json}"
            });

            string customerid = ConfigurationManager.AppSettings["CustomerId"].Trim();
            var loggingSessionInfo = Default.GetLoggingSession(customerid, requestParameter.VipId);
            loggingSessionInfo.Conn = ConfigurationManager.AppSettings["Conn"].Trim();

            //实付金额 = (应付金额 - 优惠券抵扣金额 - 余额抵扣金额 - 积分抵扣金额 - 返现抵扣金额)*会员折扣
            IDbTransaction tran = new TransactionHelper(loggingSessionInfo).CreateTransaction();

            try
            {
                using (tran.Connection)
                {
                    var paymentDetailBll = new T_Payment_detailBLL(loggingSessionInfo);
                    //var  = new TInoutBLL(loggingSessionInfo); //订单业务表
                    var tInoutBll = new T_InoutBLL(loggingSessionInfo);
                    var tPaymentTypeBll = new T_Payment_TypeBLL(loggingSessionInfo);
                    var paymentTypeList = tPaymentTypeBll.GetAll();
                    var unitBLL = new t_unitBLL(loggingSessionInfo);
                    var vipIntegralBll = new VipIntegralBLL(loggingSessionInfo);
                    var paymentList = new List<T_Payment_detailEntity>();

                    var offlinePay =
                        paymentTypeList.ToList()
                            .Find(t => t.Payment_Type_Code == EnumPayMethod.OfflinePay.ToString());

                    var entity = tInoutBll.GetByID(requestParameter.OrderId);


                    t_unitEntity unitInfo = null;
                    if (!string.IsNullOrEmpty(entity.sales_unit_id))
                        unitInfo = unitBLL.GetByID(entity.sales_unit_id);

                    EnumDelivery delivery;
                    Enum.TryParse(entity.Field8, out delivery);

                    if (delivery == EnumDelivery.HomeDelivery)
                    {
                        entity.status = "600";
                        entity.status_desc = "已发货";
                        entity.Field7 = "600";
                        entity.Field10 = "已发货";
                    }
                    else if (delivery == EnumDelivery.ShopPickUp)
                    {
                        entity.status = "610";
                        entity.status_desc = "已提货";
                        entity.Field7 = "610";
                        entity.Field10 = "已提货";
                    }
                    else if (delivery == EnumDelivery.ShopService)
                    {
                        entity.status = "620";
                        entity.status_desc = "已服务";
                        entity.Field7 = "620";
                        entity.Field10 = "已服务";
                    }

                    entity.Field1 = "1";
                    entity.complete_date = requestParameter.ComplateTime;

                    tInoutBll.Update(entity, tran);

                    #region 增加订单操作记录 Add By Henry 2015-7-29

                    var tinoutStatusBLL = new TInoutStatusBLL(loggingSessionInfo);
                    TInoutStatusEntity statusEntity = new TInoutStatusEntity()
                    {
                        OrderID = entity.order_id,
                        OrderStatus = int.Parse(entity.Field7),
                        CustomerID = loggingSessionInfo.ClientID,
                        StatusRemark = "线下支付成功[操作人:ERP]"
                    };
                    tinoutStatusBLL.Create(statusEntity, tran);

                    #endregion

                    if (requestParameter.RemainingPay > 0)
                    {
                        //入支付明细表
                        var paymentDetail = new T_Payment_detailEntity()
                        {
                            Payment_Id = Guid.NewGuid().ToString(),
                            Inout_Id = entity.order_id,
                            UnitCode = unitInfo == null ? "" : unitInfo.unit_code,
                            Payment_Type_Id = offlinePay.Payment_Type_Id,
                            Payment_Type_Code = offlinePay.Payment_Type_Code,
                            Payment_Type_Name = offlinePay.Payment_Type_Name,
                            Price = requestParameter.RemainingPay,
                            Total_Amount = entity.total_amount,
                            Pay_Points = 0,
                            CustomerId = loggingSessionInfo.ClientID
                        };
                        paymentList.Add(paymentDetail);

                        #region 订单支付明细

                        foreach (var tmp in paymentList)
                        {
                            paymentDetailBll.Create(tmp, tran);
                        }

                        #endregion

                        #region 分润

                        if (entity.data_from_id == "3")
                            vipIntegralBll.OrderRewardForOffLine(entity, requestParameter.RemainingPay,
                                (SqlTransaction) tran);
                        else if (entity.data_from_id == "38")
                            vipIntegralBll.OrderReward(entity, (SqlTransaction) tran);

                        #endregion
                    }

                    tran.Commit();

                    var responseModel = new OrderDoneResponseModel()
                    {
                        IsSucess = true,
                        ErrorMessage = string.Empty
                    };

                    var returnjson = new JavaScriptSerializer().Serialize(responseModel);
                    Loggers.Debug(new DebugLogInfo()
                    {
                        Message = $"调用[api/Order/Done]，返回参数：\"{returnjson}\"",
                    });

                    return Request.CreateResponse(HttpStatusCode.OK, responseModel);
                }
            }
            catch (Exception ex)
            {
                var responseModel = new OrderDoneResponseModel
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
            }
        }

        public decimal UpdateOrder(OrderPaymentRequestModel para, LoggingSessionInfo loggingSessionInfo)
        {
            string customerid = ConfigurationManager.AppSettings["CustomerId"].Trim();
            var UserLoggingSession = Default.GetLoggingSession(customerid, para.OrderContract.ModifyUserId);
            UserLoggingSession.Conn = ConfigurationManager.AppSettings["Conn"].Trim();


            var tInoutBll = new TInoutBLL(loggingSessionInfo); //订单业务表
            var vipBLL = new VipBLL(loggingSessionInfo);
            var paymentDetailBll = new T_Payment_detailBLL(loggingSessionInfo);
            var unitBLL = new t_unitBLL(loggingSessionInfo);
            var sysVipCardGradeBLL = new SysVipCardGradeBLL(loggingSessionInfo); //获取折扣表
            InoutService inoutService = new InoutService(loggingSessionInfo);
            DateTime createTime = Convert.ToDateTime(para.OrderContract.ModifyTime);
            var tPaymentTypeBll = new T_Payment_TypeBLL(loggingSessionInfo);

            var entity = tInoutBll.GetByID(para.OrderContract.OrderId);

            if (entity == null)
            {
                throw new APIException("该订单不存在！") {ErrorCode = 103};
            }
            if (para.OrderContract.DetailList == null || para.OrderContract.DetailList.Count == 0)
            {
                throw new APIException("没有订单明细！") {ErrorCode = 103};
            }

            var vipInfo = vipBLL.GetByID(entity.VipNo); //会员信息

            var paymentList = new List<T_Payment_detailEntity>();
            var chajia = para.OrderContract.TotalAmount ?? para.OrderContract.DetailList[0].EnterAmount;
            var RemainingPay = para.OrderContract.TotalAmount ?? para.OrderContract.DetailList[0].EnterAmount;
            var paymentTypeList = tPaymentTypeBll.GetAll();

            //优惠券
            var couponFlag = para.CouponFlag;
            var couponId = para.CouponId;
            //积分
            var integralFlag = para.IntegralFlag;
            var integral = para.Integral;
            var integralAmount = para.IntegralAmount;
            //余额
            var vipEndAmountFlag = para.VipEndAmountFlag;
            var vipEndAmount = para.VipEndAmount;
            //返现
            var returnAmountFlag = para.ReturnAmountFlag; //是否使用返现金额（1=使用；0=不适用）
            var returnAmount = para.ReturnAmount;
            //会员抵扣金额
            //var vipDiscountRp = para.VipDiscount;


            //实付金额 = (应付金额 - 优惠券抵扣金额 - 余额抵扣金额 - 积分抵扣金额 - 返现抵扣金额)*会员折扣
            IDbTransaction tran = new TransactionHelper(loggingSessionInfo).CreateTransaction();


            using (tran.Connection)

            {
                try
                {
                    //更新会员最后一次交易时间
                    vipInfo.RecentlySalesTime = DateTime.Now;
                    vipBLL.Update(vipInfo, tran);

                    t_unitEntity unitInfo = null;
                    if (!string.IsNullOrEmpty(para.OrderContract.CreateUnit))
                        unitInfo = unitBLL.GetByID(para.OrderContract.CreateUnit);

                    decimal vipDiscount = sysVipCardGradeBLL.GetVipDiscount(); //会员折扣
                    decimal discountAmount = 0; //抵扣金额汇总
                    var tempAmount = para.OrderContract.DetailList[0].EnterPrice ?? 0;

                    #region 运费处理

                    decimal deliveryAmount = 0;

                    #endregion

                    #region 会员折扣


                    if (vipDiscount > 0)
                    {
                        para.OrderContract.DiscountRate = vipDiscount*10;
                        tempAmount = tempAmount*(vipDiscount/10);
                        RemainingPay = tempAmount;
                    }

                    #endregion

                    #region 积分抵扣业务处理

                    if (integralFlag == 1)
                    {
                        var vipIntegralBll = new VipIntegralBLL(loggingSessionInfo);

                        discountAmount = discountAmount + integralAmount;
                        string sourceId = "20"; //积分抵扣
                        //vipIntegralBll.ProcessPoint(sourceId, rp.CustomerID, rp.UserID, orderId, (SqlTransaction)tran, null, -integral, null, rp.UserID);
                        var IntegralDetail = new VipIntegralDetailEntity()
                        {
                            Integral = -integral,
                            IntegralSourceID = sourceId,
                            ObjectId = entity.OrderID
                        };
                        if (IntegralDetail.Integral != 0)
                        {
                            //变动前积分
                            string OldIntegral = (vipInfo.Integration ?? 0).ToString();
                            //变动积分
                            string ChangeIntegral = (IntegralDetail.Integral ?? 0).ToString();
                            var vipIntegralDetailId = vipIntegralBll.AddIntegral(ref vipInfo, unitInfo, IntegralDetail,
                                (SqlTransaction) tran, UserLoggingSession);
                            //发送微信积分变动通知模板消息
                            if (!string.IsNullOrWhiteSpace(vipIntegralDetailId))
                            {
                                var CommonBLL = new CommonBLL();
                                CommonBLL.PointsChangeMessage(OldIntegral, vipInfo, ChangeIntegral, vipInfo.WeiXinUserId,
                                    loggingSessionInfo);
                            }

                        }
                        entity.PayPoints = integral + entity.PayPoints;
                        entity.ReceivePoints = integral + entity.ReceivePoints;

                        if (integral > 0)
                        {
                            RemainingPay = RemainingPay - integralAmount;

                            var integralPay =
                                paymentTypeList.ToList()
                                    .Find(t => t.Payment_Type_Code == EnumPayMethod.IntegralPay.ToString());
                            //入支付明细表
                            var paymentDetail = new T_Payment_detailEntity()
                            {
                                Payment_Id = Guid.NewGuid().ToString(),
                                Inout_Id = entity.OrderID,
                                UnitCode = unitInfo == null ? "" : unitInfo.unit_code,
                                Payment_Type_Id = integralPay.Payment_Type_Id,
                                Payment_Type_Code = integralPay.Payment_Type_Code,
                                Payment_Type_Name = integralPay.Payment_Type_Name,
                                Price = integral,
                                Total_Amount = para.OrderContract.TotalAmount,
                                Pay_Points = 0,
                                CustomerId = loggingSessionInfo.ClientID
                            };
                            paymentList.Add(paymentDetail);
                        }
                    }

                    #endregion

                    #region 优惠券修改

                    int tmpcouponFlag = string.IsNullOrEmpty(couponId) ? 0 : 1;
                    if (tmpcouponFlag == 1)
                    {
                        #region 判断优惠券是否是该会员的

                        var vipcouponMappingBll = new VipCouponMappingBLL(loggingSessionInfo);

                        var vipcouponmappingList = vipcouponMappingBll.QueryByEntity(new VipCouponMappingEntity()
                        {
                            VIPID = para.OrderContract.VipNo,
                            CouponID = para.CouponId
                        }, null);

                        if (vipcouponmappingList == null || vipcouponmappingList.Length == 0)
                        {
                            throw new APIException("此张优惠券不是该会员的") {ErrorCode = 103};
                        }

                        #endregion

                        #region 判断优惠券是否有效

                        var couponBll = new CouponBLL(loggingSessionInfo);

                        var couponEntity = couponBll.GetByID(couponId);

                        if (couponEntity == null)
                        {
                            throw new APIException("无效的优惠券") {ErrorCode = 103};
                        }

                        if (couponEntity.Status == 1)
                        {
                            throw new APIException("优惠券已使用") {ErrorCode = 103};
                        }

                        if (couponEntity.EndDate < DateTime.Now)
                        {
                            throw new APIException("优惠券已过期") {ErrorCode = 103};
                        }
                        var couponTypeBll = new CouponTypeBLL(loggingSessionInfo);
                        var couponTypeEntity = couponTypeBll.GetByID(couponEntity.CouponTypeID);

                        if (couponTypeEntity == null)
                        {
                            throw new APIException("无效的优惠券类型") {ErrorCode = 103};
                        }

                        #endregion

                        discountAmount = discountAmount + couponTypeEntity.ParValue ?? 0;

                        #region Insert CouponUse


                        var couponUseBll = new CouponUseBLL(loggingSessionInfo);
                        var couponUseEntity = new CouponUseEntity()
                        {
                            CouponUseID = Guid.NewGuid(),
                            CouponID = couponId,
                            VipID = vipInfo.VIPID,
                            UnitID = para.OrderContract.CreateUnit,
                            OrderID = para.OrderContract.OrderId,
                            Comment = "商城使用电子券",
                            CustomerID = loggingSessionInfo.ClientID,
                            CreateBy = vipInfo.VIPID,
                            CreateTime = createTime,
                            LastUpdateBy = vipInfo.VIPID,
                            LastUpdateTime = createTime,
                            IsDelete = 0
                        };
                        couponUseBll.Create(couponUseEntity, tran);

                        #endregion

                        #region 更新CouponType数量

                        var conponTypeBll = new CouponTypeBLL(loggingSessionInfo);
                        var conponTypeEntity =
                            conponTypeBll.QueryByEntity(
                                new CouponTypeEntity()
                                {
                                    CouponTypeID = new Guid(couponEntity.CouponTypeID),
                                    CustomerId = loggingSessionInfo.ClientID
                                }, null).FirstOrDefault();
                        conponTypeEntity.IsVoucher += 1;
                        conponTypeBll.Update(conponTypeEntity, tran);

                        #endregion

                        #region Update Coupon Status = 1

                        couponEntity.Status = 1;
                        couponBll.Update(couponEntity, tran);

                        #endregion

                        var couponPay =
                            paymentTypeList.ToList()
                                .Find(t => t.Payment_Type_Code == EnumPayMethod.CouponPay.ToString());
                        //入支付明细表
                        var paymentDetail = new T_Payment_detailEntity()
                        {
                            Payment_Id = Guid.NewGuid().ToString(),
                            Inout_Id = entity.OrderID,
                            UnitCode = unitInfo == null ? "" : unitInfo.unit_code,
                            Payment_Type_Id = couponPay.Payment_Type_Id,
                            Payment_Type_Code = couponPay.Payment_Type_Code,
                            Payment_Type_Name = couponPay.Payment_Type_Name,
                            Price = couponTypeEntity.ParValue,
                            Total_Amount = para.OrderContract.TotalAmount,
                            Pay_Points = 0,
                            CustomerId = loggingSessionInfo.ClientID
                        };
                        paymentList.Add(paymentDetail);
                        RemainingPay = RemainingPay - couponTypeEntity.ParValue;

                    }

                    #endregion

                    #region 余额和返现修改

                    var vipAmountBll = new VipAmountBLL(loggingSessionInfo);
                    var vipAmountDetailBll = new VipAmountDetailBLL(loggingSessionInfo);

                    var vipAmountEntity =
                        vipAmountBll.QueryByEntity(
                                new VipAmountEntity() {VipId = vipInfo.VIPID, VipCardCode = vipInfo.VipCode}, null)
                            .FirstOrDefault();
                    if (vipAmountEntity != null)
                    {
                        //判断该会员账户是否被冻结
                        if (vipAmountEntity.IsLocking == 1)
                            throw new APIException("账户已被冻结，请先解冻") {ErrorCode = 103};

                        //判断该会员的账户余额是否大于本次使用的余额
                        if (vipAmountEntity.EndAmount < vipEndAmount)
                            throw new APIException(string.Format("账户余额不足，当前余额为【{0}】", vipAmountEntity.EndAmount))
                            {
                                ErrorCode = 103
                            };

                        //所剩余额大于商品价格，扣除余额的数量为商品价格
                        if (tempAmount < vipEndAmount)
                        {
                            vipEndAmount = Convert.ToDecimal(tempAmount);
                        }
                    }

                    //使用余额
                    if (vipEndAmountFlag == 1)
                    {
                        var detailInfo = new VipAmountDetailEntity()
                        {
                            Amount = -vipEndAmount,
                            AmountSourceId = "1",
                            ObjectId = entity.OrderID
                        };
                        var vipAmountDetailId = vipAmountBll.AddVipAmount(vipInfo, unitInfo, ref vipAmountEntity,
                            detailInfo, (SqlTransaction) tran, UserLoggingSession);
                        if (!string.IsNullOrWhiteSpace(vipAmountDetailId))
                        {
//发送微信账户余额变动模板消息
                            var CommonBLL = new CommonBLL();
                            CommonBLL.BalanceChangedMessage(entity.OrderNo, vipAmountEntity, detailInfo,
                                vipInfo.WeiXinUserId, vipInfo.VIPID, loggingSessionInfo);
                        }
                        entity.Field3 =
                            (decimal.Parse(string.IsNullOrEmpty(entity.Field3) ? "0" : entity.Field3) + vipEndAmount)
                                .ToString();
                        //余额算入实付金额里 min.zhang 2016.6.24
                        //discountAmount = discountAmount + vipEndAmount;


                        //如果余额 = 各种商品总金额-（优惠券抵扣金额 + 积分抵扣金额） 设置付款状态 = 1【已付款】
                        if (vipEndAmount >= tempAmount + deliveryAmount - discountAmount)
                        {
                            entity.Field1 = "1";
                        }

                        var balancePay =
                            paymentTypeList.ToList()
                                .Find(t => t.Payment_Type_Code == EnumPayMethod.BalancePay.ToString());
                        //入支付明细表
                        var paymentDetail = new T_Payment_detailEntity()
                        {
                            Payment_Id = Guid.NewGuid().ToString(),
                            Inout_Id = entity.OrderID,
                            UnitCode = unitInfo == null ? "" : unitInfo.unit_code,
                            Payment_Type_Id = balancePay.Payment_Type_Id,
                            Payment_Type_Code = balancePay.Payment_Type_Code,
                            Payment_Type_Name = balancePay.Payment_Type_Name,
                            Price = vipEndAmount,
                            Total_Amount = para.OrderContract.TotalAmount,
                            Pay_Points = 0,
                            CustomerId = loggingSessionInfo.ClientID
                        };
                        paymentList.Add(paymentDetail);

                        RemainingPay = RemainingPay - vipEndAmount;

                    }

                    #endregion

                    if (tempAmount < discountAmount)
                        entity.ActualAmount = 0;
                    else
                        entity.ActualAmount = (tempAmount - discountAmount) + entity.ActualAmount;

                    entity.TotalAmount = chajia + entity.TotalAmount;
                    entity.TotalRetail = para.OrderContract.DetailList[0].EnterAmount + entity.TotalRetail;

                    //创建订单
                    tInoutBll.Update(entity, tran); //修改订单信息

                    //创建订单明细

                    #region 订单明细列表

                    var detailbll = new T_Inout_DetailBLL(loggingSessionInfo);
                    var detailContract = para.OrderContract.DetailList[0];

                    var detailEntity = new T_Inout_DetailEntity()
                    {
                        create_time = detailContract.CreateTime,
                        create_user_id = detailContract.CreateUserId,
                        modify_time = detailContract.ModifyTime,
                        modify_user_id = detailContract.CreateUserId,
                        unit_id = para.OrderContract.CreateUnit,
                        discount_rate = para.OrderContract.DiscountRate,
                        std_price = Convert.ToDecimal(detailContract.StdPrice),
                        enter_price = Convert.ToDecimal(detailContract.EnterPrice),
                        sku_id = detailContract.SKUID,
                        enter_qty = Convert.ToDecimal(detailContract.EnterQty),
                        order_id = entity.OrderID,
                        order_detail_id = detailContract.OrderDetailId,
                        order_qty = detailContract.OrderQty,
                        enter_amount = detailContract.EnterAmount,
                        order_detail_status = "1",
                        retail_amount = detailContract.EnterAmount,
                        retail_price = detailContract.EnterAmount,
                        display_index = 1,
                        if_flag = 0
                    };
                    detailbll.Create(detailEntity, tran);


                    #endregion

                    #region 增加订单操作记录 Add By Henry 2015-7-29

                    var tinoutStatusBLL = new TInoutStatusBLL(loggingSessionInfo);
                    TInoutStatusEntity statusEntity = new TInoutStatusEntity()
                    {
                        OrderID = entity.OrderID,
                        OrderStatus = int.Parse(entity.Field7),
                        CustomerID = loggingSessionInfo.ClientID,
                        StatusRemark = "修改订单[操作人:ERP]"
                    };
                    tinoutStatusBLL.Create(statusEntity, tran);

                    #endregion

                    #region 订单支付明细

                    foreach (var tmp in paymentList)
                    {
                        paymentDetailBll.Create(tmp, tran);
                    }

                    #endregion
                    //下订单，修改抢购商品的数量信息存储过程ProcPEventItemQty
                    //var eventbll = new vwItemPEventDetailBLL(loggingSessionInfo);
                    //eventbll.ExecProcPEventItemQty(para, entity, tran);
                    tran.Commit();

                    return RemainingPay.Value;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }

        }

        public decimal CreateOrder(OrderPaymentRequestModel para, LoggingSessionInfo loggingSessionInfo)
        {
            string customerid = ConfigurationManager.AppSettings["CustomerId"].Trim();
            var UserLoggingSession = Default.GetLoggingSession(customerid, para.OrderContract.ModifyUserId);
            UserLoggingSession.Conn = ConfigurationManager.AppSettings["Conn"].Trim();

            var vipBLL = new VipBLL(loggingSessionInfo);
            var unitBLL = new t_unitBLL(loggingSessionInfo);
            var sysVipCardGradeBLL = new SysVipCardGradeBLL(loggingSessionInfo); //获取折扣表
            var tPaymentTypeBll = new T_Payment_TypeBLL(loggingSessionInfo);
            var paymentDetailBll = new T_Payment_detailBLL(loggingSessionInfo);
            this._currentDAO = new T_InoutDAO(loggingSessionInfo);
            var optionsBll = new OptionsBLL(loggingSessionInfo);
            var vipInfo = vipBLL.GetByID(para.OrderContract.VipNo); //会员信息
            var paymentList = new List<T_Payment_detailEntity>();


            DateTime createTime = Convert.ToDateTime(para.OrderContract.CreateTime);

            #region 活动有效性


            decimal salesPrice = 0; //活动销售价
            decimal qty = Convert.ToInt32(para.OrderContract.TotalQty.Value); //所购数量 

            if (string.IsNullOrEmpty(para.OrderContract.VipNo))
                throw new Exception("会员信息不存在");

            #endregion

            //订单类型

            #region 根据订单参数设置订单类型

            var order_reason_id = GetOrderReason(EnumOrderReason.Pos);

            #endregion

            var dataFromId = "38"; //线下订单
            //var RemainingPay = para.OrderContract.TotalAmount;

            if (para.OrderContract.DetailList[0].EnterAmount <= 0)
            {
                throw new APIException("订单金额为0") {};
            }

            var RemainingPay = para.OrderContract.TotalAmount ?? para.OrderContract.DetailList[0].EnterAmount;

            var paymentTypeList = tPaymentTypeBll.GetAll();

            //创建事务
            IDbTransaction tran = new TransactionHelper(loggingSessionInfo).CreateTransaction();

            using (tran.Connection)
            {
                try
                {
                    //优惠券
                    var couponFlag = para.CouponFlag;
                    var couponId = para.CouponId;
                    //积分
                    var integralFlag = para.IntegralFlag;
                    var integral = para.Integral;
                    var integralAmount = para.IntegralAmount;
                    //余额
                    var vipEndAmountFlag = para.VipEndAmountFlag;
                    var vipEndAmount = para.VipEndAmount;
                    //返现
                    var returnAmountFlag = para.ReturnAmountFlag; //是否使用返现金额（1=使用；0=不适用）
                    var returnAmount = para.ReturnAmount;
                    //会员抵扣金额
                    var vipDiscountRp = para.VipDiscount;
                    //更新会员最后一次交易时间
                    vipInfo.RecentlySalesTime = DateTime.Now;
                    vipBLL.Update(vipInfo, tran);

                    t_unitEntity unitInfo = null;
                    if (!string.IsNullOrEmpty(para.OrderContract.CreateUnit))
                        unitInfo = unitBLL.GetByID(para.OrderContract.CreateUnit);

                    var inoutOptions = optionsBll.Query(
                        new IWhereCondition[]
                        {
                            new EqualsCondition() {FieldName = "OptionName", Value = "TInOutStatus"},
                            new EqualsCondition() {FieldName = "CustomerID", Value = loggingSessionInfo.ClientID},
                            new EqualsCondition()
                            {
                                FieldName = "OptionValue",
                                Value = ((int) para.OrderContract.Status).ToString()
                            }
                        }, null).FirstOrDefault();

                    T_InoutEntity entity = new T_InoutEntity()
                    {
                        #region 订单初始化
                        order_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        order_type_id = "1F0A100C42484454BAEA211D4C14B80F",
                        create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        customer_id = loggingSessionInfo.ClientID,
                        status = inoutOptions.OptionValue.Value.ToString(),
                        total_qty = Convert.ToDecimal(qty),
                        unit_id = para.OrderContract.CreateUnit,
                        order_no = para.OrderContract.OrderNo,
                        order_id = para.OrderContract.OrderId,
                        order_reason_id = order_reason_id, //订单类型:普通,团购,抢购
                        red_flag = "1",
                        carrier_id = para.OrderContract.CreateUnit,
                        warehouse_id = "67bb4c12785c42d4912aff7d34606592", //???是否是这个??
                        create_unit_id = para.OrderContract.CreateUnit,
                        create_user_id = para.OrderContract.CreateUserId,
                        total_amount = Convert.ToDecimal(para.OrderContract.TotalAmount),
                        discount_rate = para.OrderContract.DiscountRate,
                        total_retail = Convert.ToDecimal(para.OrderContract.TotalAmount),
                        print_times = 0,
                        vip_no = para.OrderContract.VipNo,
                        data_from_id = dataFromId,
                        if_flag = "0",
                        modify_time = para.OrderContract.ModifyTime,
                        modify_user_id = para.OrderContract.ModifyUserId,
                        remark = para.OrderContract.Remark,
                        Field1 = ((int) para.OrderContract.IsPay).ToString(),
                        Field3 = string.Empty,
                        Field7 = inoutOptions.OptionValue.Value.ToString(),
                        VipCardCode = vipInfo.VipCardCode,
                        Field8 = ((int) para.OrderContract.Delivery).ToString(),
                        send_time = para.OrderContract.DeliveryDateTime,
                        Field9 = para.OrderContract.DeliveryDateTime,
                        Field4 = para.OrderContract.Address,
                        Field6 = para.OrderContract.Phone,
                        Field12 = string.Empty,
                        Field13 = vipInfo.WeiXinUserId,
                        Field10 = inoutOptions.OptionText.ToString(),
                        Field14 = para.OrderContract.UserName,
                        Field20 = string.Empty,
                        Field16 = string.Empty,
                        Field15 = order_reason_id,
                        reserveDay = para.OrderContract.RequestDate,
                        reserveQuantum = para.OrderContract.RequestDateQuantum,
                        sales_unit_id = para.OrderContract.CreateUnit,
                        purchase_unit_id = para.OrderContract.CreateUnit
                        #endregion
                    };

                    #region 运费处理

                    decimal deliveryAmount = 0;

                    #endregion

                    decimal vipDiscount = sysVipCardGradeBLL.GetVipDiscount(); //会员折扣
                    decimal discountAmount = 0; //抵扣金额汇总

                    #region 会员折扣

                    decimal tempAmount = (para.OrderContract.TotalAmount ?? 0);
                    if (vipDiscount > 0)
                    {
                        para.OrderContract.DiscountRate = vipDiscount*10;
                        tempAmount = tempAmount*(vipDiscount/10);
                        RemainingPay = tempAmount;

                    }

                    #endregion

                    #region 积分抵扣业务处理

                    if (integralFlag == 1)
                    {
                        var vipIntegralBll = new VipIntegralBLL(loggingSessionInfo);

                        discountAmount = discountAmount + integralAmount;
                        string sourceId = "20"; //积分抵扣
                        //vipIntegralBll.ProcessPoint(sourceId, rp.CustomerID, rp.UserID, orderId, (SqlTransaction)tran, null, -integral, null, rp.UserID);
                        var IntegralDetail = new VipIntegralDetailEntity()
                        {
                            Integral = -integral,
                            IntegralSourceID = sourceId,
                            ObjectId = para.OrderContract.OrderId
                        };
                        if (IntegralDetail.Integral != 0)
                        {
                            //变动前积分
                            string OldIntegral = (vipInfo.Integration ?? 0).ToString();
                            //变动积分
                            string ChangeIntegral = (IntegralDetail.Integral ?? 0).ToString();
                            var vipIntegralDetailId = vipIntegralBll.AddIntegral(ref vipInfo, unitInfo, IntegralDetail,
                                (SqlTransaction) tran, UserLoggingSession);
                            //发送微信积分变动通知模板消息
                            if (!string.IsNullOrWhiteSpace(vipIntegralDetailId))
                            {
                                var CommonBLL = new CommonBLL();
                                CommonBLL.PointsChangeMessage(OldIntegral, vipInfo, ChangeIntegral, vipInfo.WeiXinUserId,
                                    loggingSessionInfo);
                            }

                        }
                        entity.pay_points = integral;
                        entity.receive_points = integral;

                        if (integral > 0)
                        {
                            RemainingPay = RemainingPay - integralAmount;

                            var integralPay =
                                paymentTypeList.ToList()
                                    .Find(t => t.Payment_Type_Code == EnumPayMethod.IntegralPay.ToString());
                            //入支付明细表
                            var paymentDetail = new T_Payment_detailEntity()
                            {
                                Payment_Id = Guid.NewGuid().ToString(),
                                Inout_Id = entity.order_id,
                                UnitCode = unitInfo == null ? "" : unitInfo.unit_code,
                                Payment_Type_Id = integralPay.Payment_Type_Id,
                                Payment_Type_Code = integralPay.Payment_Type_Code,
                                Payment_Type_Name = integralPay.Payment_Type_Name,
                                Price = integral,
                                Total_Amount = entity.total_amount,
                                Pay_Points = 0,
                                CustomerId = loggingSessionInfo.ClientID
                            };
                            paymentList.Add(paymentDetail);
                        }


                    }

                    #endregion

                    #region 优惠券修改

                    int tmpcouponFlag = string.IsNullOrEmpty(couponId) ? 0 : 1;
                    if (tmpcouponFlag == 1)
                    {
                        #region 判断优惠券是否是该会员的

                        var vipcouponMappingBll = new VipCouponMappingBLL(loggingSessionInfo);

                        var vipcouponmappingList = vipcouponMappingBll.QueryByEntity(new VipCouponMappingEntity()
                        {
                            VIPID = para.OrderContract.VipNo,
                            CouponID = para.CouponId
                        }, null);

                        if (vipcouponmappingList == null || vipcouponmappingList.Length == 0)
                        {
                            throw new APIException("此张优惠券不是该会员的") {ErrorCode = 103};
                        }

                        #endregion

                        #region 判断优惠券是否有效

                        var couponBll = new CouponBLL(loggingSessionInfo);

                        var couponEntity = couponBll.GetByID(couponId);

                        if (couponEntity == null)
                        {
                            throw new APIException("无效的优惠券") {ErrorCode = 103};
                        }

                        if (couponEntity.Status == 1)
                        {
                            throw new APIException("优惠券已使用") {ErrorCode = 103};
                        }

                        if (couponEntity.EndDate < DateTime.Now)
                        {
                            throw new APIException("优惠券已过期") {ErrorCode = 103};
                        }
                        var couponTypeBll = new CouponTypeBLL(loggingSessionInfo);
                        var couponTypeEntity = couponTypeBll.GetByID(couponEntity.CouponTypeID);

                        if (couponTypeEntity == null)
                        {
                            throw new APIException("无效的优惠券类型") {ErrorCode = 103};
                        }

                        #endregion

                        discountAmount = discountAmount + couponTypeEntity.ParValue ?? 0;

                        #region Insert CouponUse


                        var couponUseBll = new CouponUseBLL(loggingSessionInfo);
                        var couponUseEntity = new CouponUseEntity()
                        {
                            CouponUseID = Guid.NewGuid(),
                            CouponID = couponId,
                            VipID = vipInfo.VIPID,
                            UnitID = para.OrderContract.CreateUnit,
                            OrderID = para.OrderContract.OrderId,
                            Comment = "商城使用电子券",
                            CustomerID = loggingSessionInfo.ClientID,
                            CreateBy = vipInfo.VIPID,
                            CreateTime = createTime,
                            LastUpdateBy = vipInfo.VIPID,
                            LastUpdateTime = createTime,
                            IsDelete = 0
                        };
                        couponUseBll.Create(couponUseEntity, tran);

                        #endregion

                        #region 更新CouponType数量

                        var conponTypeBll = new CouponTypeBLL(loggingSessionInfo);
                        var conponTypeEntity =
                            conponTypeBll.QueryByEntity(
                                new CouponTypeEntity()
                                {
                                    CouponTypeID = new Guid(couponEntity.CouponTypeID),
                                    CustomerId = loggingSessionInfo.ClientID
                                }, null).FirstOrDefault();
                        conponTypeEntity.IsVoucher += 1;
                        conponTypeBll.Update(conponTypeEntity, tran);

                        #endregion

                        #region Update Coupon Status = 1

                        couponEntity.Status = 1;
                        couponBll.Update(couponEntity, tran);

                        #endregion

                        var couponPay =
                            paymentTypeList.ToList()
                                .Find(t => t.Payment_Type_Code == EnumPayMethod.CouponPay.ToString());
                        //入支付明细表
                        var paymentDetail = new T_Payment_detailEntity()
                        {
                            Payment_Id = Guid.NewGuid().ToString(),
                            Inout_Id = entity.order_id,
                            UnitCode = unitInfo == null ? "" : unitInfo.unit_code,
                            Payment_Type_Id = couponPay.Payment_Type_Id,
                            Payment_Type_Code = couponPay.Payment_Type_Code,
                            Payment_Type_Name = couponPay.Payment_Type_Name,
                            Price = couponTypeEntity.ParValue,
                            Total_Amount = entity.total_amount,
                            Pay_Points = 0,
                            CustomerId = loggingSessionInfo.ClientID
                        };
                        paymentList.Add(paymentDetail);
                        RemainingPay = RemainingPay - couponTypeEntity.ParValue;

                    }

                    #endregion

                    #region 余额和返现修改

                    var vipAmountBll = new VipAmountBLL(loggingSessionInfo);
                    var vipAmountDetailBll = new VipAmountDetailBLL(loggingSessionInfo);

                    var vipAmountEntity =
                        vipAmountBll.QueryByEntity(
                                new VipAmountEntity() {VipId = vipInfo.VIPID, VipCardCode = vipInfo.VipCode}, null)
                            .FirstOrDefault();
                    if (vipAmountEntity != null)
                    {
                        //判断该会员账户是否被冻结
                        if (vipAmountEntity.IsLocking == 1)
                            throw new APIException("账户已被冻结，请先解冻") {ErrorCode = 103};

                        //判断该会员的账户余额是否大于本次使用的余额
                        if (vipAmountEntity.EndAmount < vipEndAmount)
                            throw new APIException(string.Format("账户余额不足，当前余额为【{0}】", vipAmountEntity.EndAmount))
                            {
                                ErrorCode = 103
                            };

                        //所剩余额大于商品价格，扣除余额的数量为商品价格
                        if (tempAmount < vipEndAmount)
                        {
                            vipEndAmount = Convert.ToDecimal(tempAmount);
                        }
                    }

                    //使用余额
                    if (vipEndAmountFlag == 1)
                    {
                        var detailInfo = new VipAmountDetailEntity()
                        {
                            Amount = -vipEndAmount,
                            AmountSourceId = "1",
                            ObjectId = para.OrderContract.OrderId
                        };
                        var vipAmountDetailId = vipAmountBll.AddVipAmount(vipInfo, unitInfo, ref vipAmountEntity,
                            detailInfo, (SqlTransaction) tran, UserLoggingSession);
                        if (!string.IsNullOrWhiteSpace(vipAmountDetailId))
                        {
//发送微信账户余额变动模板消息
                            var CommonBLL = new CommonBLL();
                            CommonBLL.BalanceChangedMessage(entity.order_no, vipAmountEntity, detailInfo,
                                vipInfo.WeiXinUserId, vipInfo.VIPID, loggingSessionInfo);
                        }
                        entity.Field3 = vipEndAmount.ToString();
                        //余额算入实付金额里 min.zhang 2016.6.24
                        //discountAmount = discountAmount + vipEndAmount;


                        //如果余额 = 各种商品总金额-（优惠券抵扣金额 + 积分抵扣金额） 设置付款状态 = 1【已付款】
                        if (vipEndAmount >= tempAmount + deliveryAmount - discountAmount)
                        {
                            entity.Field1 = "1";
                        }

                        var balancePay =
                            paymentTypeList.ToList()
                                .Find(t => t.Payment_Type_Code == EnumPayMethod.BalancePay.ToString());
                        //入支付明细表
                        var paymentDetail = new T_Payment_detailEntity()
                        {
                            Payment_Id = Guid.NewGuid().ToString(),
                            Inout_Id = entity.order_id,
                            UnitCode = unitInfo == null ? "" : unitInfo.unit_code,
                            Payment_Type_Id = balancePay.Payment_Type_Id,
                            Payment_Type_Code = balancePay.Payment_Type_Code,
                            Payment_Type_Name = balancePay.Payment_Type_Name,
                            Price = vipEndAmount,
                            Total_Amount = entity.total_amount,
                            Pay_Points = 0,
                            CustomerId = loggingSessionInfo.ClientID
                        };
                        paymentList.Add(paymentDetail);

                        RemainingPay = RemainingPay - vipEndAmount;

                    }

                    #endregion

                    if (tempAmount < discountAmount)
                        entity.actual_amount = 0;
                    else
                        entity.actual_amount = tempAmount - discountAmount;

                    //创建订单
                    this._currentDAO.Create(entity, tran);

                    //创建订单明细

                    #region 订单明细列表

                    var detailbll = new T_Inout_DetailBLL(loggingSessionInfo);
                    var detailContract = para.OrderContract.DetailList[0];

                    var detailEntity = new T_Inout_DetailEntity()
                    {
                        create_time = detailContract.CreateTime,
                        create_user_id = detailContract.CreateUserId,
                        modify_time = detailContract.ModifyTime,
                        modify_user_id = detailContract.CreateUserId,
                        unit_id = para.OrderContract.CreateUnit,
                        discount_rate = para.OrderContract.DiscountRate,
                        std_price = Convert.ToDecimal(detailContract.StdPrice),
                        enter_price = Convert.ToDecimal(detailContract.EnterPrice),
                        sku_id = detailContract.SKUID,
                        enter_qty = Convert.ToDecimal(detailContract.EnterQty),
                        order_id = para.OrderContract.OrderId,
                        order_detail_id = detailContract.OrderDetailId,
                        order_qty = entity.total_qty,
                        enter_amount = detailContract.EnterAmount,
                        order_detail_status = "1",
                        retail_amount = detailContract.EnterAmount,
                        retail_price = detailContract.EnterAmount,
                        display_index = 1,
                        if_flag = 0
                    };
                    detailbll.Create(detailEntity, tran);


                    #endregion

                    #region 增加订单操作记录 Add By Henry 2015-7-29

                    var tinoutStatusBLL = new TInoutStatusBLL(loggingSessionInfo);
                    TInoutStatusEntity statusEntity = new TInoutStatusEntity()
                    {
                        OrderID = entity.order_id,
                        OrderStatus = int.Parse(entity.Field7),
                        CustomerID = loggingSessionInfo.ClientID,
                        StatusRemark = "提交订单[操作人:ERP]"
                    };
                    tinoutStatusBLL.Create(statusEntity, tran);

                    #endregion

                    #region 订单支付明细

                    foreach (var tmp in paymentList)
                    {
                        paymentDetailBll.Create(tmp, tran);
                    }

                    #endregion

                    //下订单，修改抢购商品的数量信息存储过程ProcPEventItemQty
                    //var eventbll = new vwItemPEventDetailBLL(loggingSessionInfo);
                    //eventbll.ExecProcPEventItemQty(para, entity, tran);
                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
            }

            return RemainingPay.Value;
        }

        public string GetOrderReason(EnumOrderReason orderReason)
        {
            switch (orderReason)
            {
                case EnumOrderReason.Pos:
                    return "2F6891A2194A4BBAB6F17B4C99A6C6F5";
                default:
                    throw new Exception("");
            }
        }


    }
}
