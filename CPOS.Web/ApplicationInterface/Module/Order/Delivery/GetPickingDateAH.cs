﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.DTO.Module.Order.Delivery.Request;
using JIT.CPOS.DTO.Module.Order.Delivery.Response;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess.Query;
using JIT.Utility.ExtensionMethod;
using JIT.CPOS.DTO.Base;


namespace JIT.CPOS.Web.ApplicationInterface.Module.Order.Delivery
{
    public class GetPickingDateAH : BaseActionHandler<GetPickingDateRP,GetPickingDateRD>
    {
        /// <summary>
        /// 选择提货日期
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        protected override GetPickingDateRD ProcessRequest(APIRequest<GetPickingDateRP> pRequest)
        {
            var deliveryid = string.IsNullOrWhiteSpace(pRequest.Parameters.DeliveryId)
                ? "2"
                : pRequest.Parameters.DeliveryId;

            //基础数据初始化
            GetPickingDateRD getPickingDateRD = new GetPickingDateRD();
            CustomerTakeDeliveryBLL customerTakeDeliveryBll = new CustomerTakeDeliveryBLL(CurrentUserInfo);
            CustomerTakeDeliveryEntity customerTakeDeliveryEntity =
                customerTakeDeliveryBll.QueryByEntity(
                    new CustomerTakeDeliveryEntity() {DeliveryId = deliveryid, CustomerId = CurrentUserInfo.ClientID},
                    null).FirstOrDefault();
            SysTimeQuantumBLL sysTimeQuantumBll = new SysTimeQuantumBLL(CurrentUserInfo);
            SysTimeQuantumEntity sysTimeQuantumEntity =
                sysTimeQuantumBll.QueryByEntity(
                        new SysTimeQuantumEntity() {DeliveryId = deliveryid, CustomerID = CurrentUserInfo.ClientID},
                        new OrderBy[] {new OrderBy() {FieldName = "Quantum", Direction = OrderByDirections.Desc}})
                    .FirstOrDefault();

            if (deliveryid == "2")
            {
                int? stockUpPeriod = customerTakeDeliveryEntity.StockUpPeriod == null
                    ? 0
                    : customerTakeDeliveryEntity.StockUpPeriod;
                int? maxDelivery = customerTakeDeliveryEntity.MaxDelivery == null
                    ? 0
                    : customerTakeDeliveryEntity.MaxDelivery;

                if (sysTimeQuantumEntity == null)
                {
                    getPickingDateRD.BeginDate =
                        DateTime.Now.AddHours(Convert.ToDouble(stockUpPeriod)).ToString("yyyy-MM-dd");
                    getPickingDateRD.IsDisplay = 1; //显示日期
                }
                else
                {
                    string[] timeTemp = sysTimeQuantumEntity.Quantum.Split('-');
                    string beginTime = DateTime.Now.AddHours(Convert.ToDouble(stockUpPeriod)).ToString("HH:mm");
                    getPickingDateRD.IsDisplay = 2; //都显示
                    //如果在备货完的结束日期没有时间段可以选择，则后一天为允许选择的开始时间
                    if (beginTime.CompareTo(timeTemp[0]) < 0)
                    {
                        getPickingDateRD.BeginDate =
                            DateTime.Now.AddHours(Convert.ToDouble(stockUpPeriod)).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        getPickingDateRD.BeginDate =
                            DateTime.Now.AddHours(Convert.ToDouble(stockUpPeriod)).AddDays(1).ToString("yyyy-MM-dd");
                    }
                }

                if (maxDelivery == 0 && sysTimeQuantumEntity == null)
                {
                    getPickingDateRD.IsDisplay = 0; //都不显示
                }

                int? addMaxDelivery = maxDelivery == 0 ? 0 : customerTakeDeliveryEntity.MaxDelivery - 1;
                getPickingDateRD.EndDate =
                    Convert.ToDateTime(getPickingDateRD.BeginDate)
                        .AddDays(Convert.ToDouble(addMaxDelivery))
                        .ToString("yyyy-MM-dd");

                return getPickingDateRD;
            }
            else if (deliveryid == "4")
            {
                var stockUpPeriod = customerTakeDeliveryEntity.StockUpPeriod == null
                    ? 0
                    : customerTakeDeliveryEntity.StockUpPeriod; //需要提前多少天预约
                var deliveryServiceConfigDay = ConfigurationManager.AppSettings["DeliveryServiceConfigDay"]; //服时间为最近的7天

                DateTime today = DateTime.Now;

                DateTime beginDateTime =
                    today.AddHours(Convert.ToDouble(stockUpPeriod));

                DateTime endDate =
                    beginDateTime.AddHours(Convert.ToDouble(stockUpPeriod));

                getPickingDateRD.BeginDate = beginDateTime.ToString("yyyy-MM-dd");
                getPickingDateRD.EndDate = endDate.ToString("yyyy-MM-dd");
                getPickingDateRD.IsDisplay = 2; //都显示
                return getPickingDateRD;
            }
            else
            {
                return getPickingDateRD;
            }
        }
    }
}