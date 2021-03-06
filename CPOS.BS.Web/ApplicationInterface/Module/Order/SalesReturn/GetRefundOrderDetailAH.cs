﻿using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.CPOS.Common;
using JIT.CPOS.DTO.Module.Order.SalesReturn.Request;
using JIT.CPOS.DTO.Module.Order.SalesReturn.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Order.SalesReturn
{
    public class GetRefundOrderDetailAH : BaseActionHandler<GetRefundOrderDetailRP, GetRefundOrderDetailRD>
    {
        protected override GetRefundOrderDetailRD ProcessRequest(DTO.Base.APIRequest<GetRefundOrderDetailRP> pRequest)
        {
            var rd = new GetRefundOrderDetailRD();
            var para = pRequest.Parameters;

            var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;
            var refundOrderBLL = new T_RefundOrderBLL(loggingSessionInfo);

            var inoutService = new InoutService(loggingSessionInfo);
            var tInoutDetailBll = new TInoutDetailBLL(loggingSessionInfo);
            var t_InoutBLL = new T_InoutBLL(loggingSessionInfo);
            var PaymentTtpeBLL = new T_Payment_TypeBLL(loggingSessionInfo);
            var refundEntity = refundOrderBLL.GetByID(para.RefundID);

            if (refundEntity != null)
            {
                rd.RefundID = refundEntity.RefundID;
                rd.RefundNo = refundEntity.RefundNo;
                rd.Status = refundEntity.Status;
                rd.OrderNo = refundEntity.OrderNo;
                rd.Contacts = refundEntity.Contacts;
                rd.Phone = refundEntity.Phone;
                rd.ConfirmAmount = refundEntity.ConfirmAmount;
                rd.ActualRefundAmount = refundEntity.ActualRefundAmount;
                rd.Points = refundEntity.Points == null ? 0 : refundEntity.Points;
                rd.PointsAmount = refundEntity.PointsAmount == null ? 0 : refundEntity.PointsAmount;
                rd.ReturnAmount = refundEntity.ReturnAmount == null ? 0 : refundEntity.RefundAmount;
                rd.Amount = refundEntity.Amount == null ? 0 : refundEntity.Amount;

                //支付方式名称
                var OrderData=t_InoutBLL.GetByID(refundEntity.OrderID);
                string m_PayTypeName="";
                if(OrderData!=null){
                    var PayTypeData=PaymentTtpeBLL.GetByID(OrderData.pay_id);
                    if(PayTypeData!=null)
                        m_PayTypeName=PayTypeData.Payment_Type_Name;
                }
                rd.PayTypeName = m_PayTypeName;


                rd.PayOrderID = refundEntity.PayOrderID;
                rd.OrderID = refundEntity.OrderID;
                rd.ItemID = refundEntity.ItemID;

                if (!string.IsNullOrEmpty(refundEntity.ItemID)) //取消订单时，直接跳转到订单详情
                {

                    //根据订单ID获取订单明细[复用]
                    DataRow drItem = inoutService.GetOrderDetailByOrderId(refundEntity.OrderID).Tables[0].Select(" item_id= '" + refundEntity.ItemID + "'").FirstOrDefault();
                    //获取商品的图片[复用]
                    string itemImage = string.Empty;
                    DataSet ds = tInoutDetailBll.GetOrderDetailImageList("'" + refundEntity.ItemID + "'");
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        itemImage = ds.Tables[0].Rows[0]["imageUrl"].ToString();
                    }
                    //获取订单详细列表中的商品规格[复用]
                    DataRow[] drSku = inoutService.GetInoutDetailGgByOrderId(refundEntity.OrderID).Tables[0].Select(" sku_id='" + refundEntity.SkuID + "'");

                    //订单的商品信息
                    var orderDetail = new OrderInfoDetail();

                    orderDetail.ItemName = drItem["item_name"].ToString();
                    orderDetail.SalesPrice = Convert.ToDecimal(drItem["enter_price"]);
                    orderDetail.Qty = Convert.ToInt32(drItem["enter_qty"]);
                    if (!string.IsNullOrEmpty(itemImage))
                    {
                        orderDetail.ImageUrl = ImagePathUtil.GetImagePathStr(itemImage, "240");
                    }
                    rd.OrderDetail = orderDetail;
                    //订单的商品规格
                    if (drSku.Count() > 0)
                    {
                        SkuDetailInfo skuDetail = new SkuDetailInfo();
                        skuDetail.PropName1 = drSku[0]["prop_1_name"].ToString();
                        skuDetail.PropDetailName1 = drSku[0]["prop_1_detail_name"].ToString();
                        skuDetail.PropName2 = drSku[0]["prop_2_name"].ToString();
                        skuDetail.PropDetailName2 = drSku[0]["prop_2_detail_name"].ToString();
                        skuDetail.PropName3 = drSku[0]["prop_3_name"].ToString();
                        skuDetail.PropDetailName3 = drSku[0]["prop_3_detail_name"].ToString();
                        rd.OrderDetail.SkuDetail = skuDetail;
                    }
                }
            }
            return rd;
        }
    }
}