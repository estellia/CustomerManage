/*
 * Author		:
 * EMail		:
 * Company		:
 * Create On	:
 * Description	:
 * 1st Modified On	:
 * 1st Modified By	:
 * 1st Modified Desc:
 * 1st Modifier EMail	:
 * 2st Modified On	:
 * 2st Modified By	:
 * 2st Modifier EMail	:
 * 2st Modified Desc:
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.DTO.Module.Order.Delivery.Request;
using JIT.CPOS.DTO.Module.Order.Delivery.Response;
using JIT.Utility.ExtensionMethod;
using JIT.CPOS.DTO.Base;
using JIT.Utility.DataAccess.Query;


namespace JIT.CPOS.Web.ApplicationInterface.Module.Order.Delivery
{
    public class GetOrderDeliveryDateTimeRangeAH :
        BaseActionHandler<GetOrderDeliveryDateTimeRangeRP, GetOrderDeliveryDateTimeRangeRD>
    {
        protected override GetOrderDeliveryDateTimeRangeRD ProcessRequest(
            APIRequest<GetOrderDeliveryDateTimeRangeRP> pRequest)
        {
            var rd = new GetOrderDeliveryDateTimeRangeRD();

            var deliveryid = string.IsNullOrWhiteSpace(pRequest.Parameters.DeliveryId)
                ? "2"
                : pRequest.Parameters.DeliveryId;

            //基础数据初始化
            GetPickingDateRD getPickingDateRD = new GetPickingDateRD();
            var getOrderDeliveryDateTimeRangeRD = new GetOrderDeliveryDateTimeRangeRD();
            CustomerTakeDeliveryBLL customerTakeDeliveryBll = new CustomerTakeDeliveryBLL(CurrentUserInfo);
            CustomerTakeDeliveryEntity customerTakeDeliveryEntity =
                customerTakeDeliveryBll.QueryByEntity(
                    new CustomerTakeDeliveryEntity() {DeliveryId = deliveryid, CustomerId = CurrentUserInfo.ClientID},
                    null).FirstOrDefault();
            SysTimeQuantumBLL sysTimeQuantumBll = new SysTimeQuantumBLL(CurrentUserInfo);
            var sysTimeQuantumEntityList =
                sysTimeQuantumBll.QueryByEntity(
                    new SysTimeQuantumEntity() {DeliveryId = deliveryid, CustomerID = CurrentUserInfo.ClientID},
                    new OrderBy[] {new OrderBy() {FieldName = "Quantum", Direction = OrderByDirections.Asc}});

            var beginDateTime = DateTime.MinValue;
            var endDateTime = DateTime.MinValue;

            var now = DateTime.Now;


            List<DateTimeInfo> dateTimeInfos = new List<DateTimeInfo>();

            int? stockUpPeriod = customerTakeDeliveryEntity.StockUpPeriod == null
                ? 0
                : customerTakeDeliveryEntity.StockUpPeriod;
            int? maxDelivery = customerTakeDeliveryEntity.MaxDelivery == null
                ? 0
                : customerTakeDeliveryEntity.MaxDelivery;

            if (deliveryid == "2")
            {
                if (sysTimeQuantumEntityList.Length == 0)
                {
                    beginDateTime = now.AddHours(Convert.ToDouble(stockUpPeriod));
                }
                else
                {
                    string[] timeTemp = sysTimeQuantumEntityList[0].Quantum.Split('-');
                    string beginTime = now.AddHours(Convert.ToDouble(stockUpPeriod)).ToString("HH:mm");
                    //如果在备货完的结束日期没有时间段可以选择，则后一天为允许选择的开始时间
                    if (beginTime.CompareTo(timeTemp[0]) < 0)
                    {
                        beginDateTime =
                            now.AddHours(Convert.ToDouble(stockUpPeriod));
                    }
                    else
                    {
                        beginDateTime =
                            now.AddHours(Convert.ToDouble(stockUpPeriod)).AddDays(1);
                    }
                }

                int? addMaxDelivery = maxDelivery == 0 ? 0 : customerTakeDeliveryEntity.MaxDelivery - 1;
                endDateTime = beginDateTime.AddDays(Convert.ToDouble(addMaxDelivery));
            }
            else if (deliveryid == "4")
            {
                if (sysTimeQuantumEntityList.Length == 0)
                {
                    beginDateTime = now.AddDays(Convert.ToDouble(stockUpPeriod));
                }
                else
                {
                    string[] timeTemp = sysTimeQuantumEntityList[0].Quantum.Split('-');
                    string beginTime = now.ToString("HH:mm");
                    //如果在备货完的结束日期没有时间段可以选择，则后一天为允许选择的开始时间
                    if (beginTime.CompareTo(timeTemp[1]) > 0)
                    {
                        beginDateTime =
                            now.AddDays(Convert.ToDouble(stockUpPeriod));
                    }
                    else
                    {
                        beginDateTime =
                            now.AddDays(Convert.ToDouble(stockUpPeriod)).AddDays(1);
                    }
                }

                var deliveryServiceConfigDay = "7";//ConfigurationManager.AppSettings["DeliveryServiceConfigDay"]; //服时间为最近的7天

                endDateTime = beginDateTime
                        .AddDays(int.Parse(deliveryServiceConfigDay));
            }

            for (var i = beginDateTime; i <= endDateTime; i = i.AddDays(1))
            {
                
                List<string> timeRange = new List<string>();

                foreach (var entity in sysTimeQuantumEntityList)
                {
                    string[] timeTemp = entity.Quantum.Split('-');
                    string beginTime = string.Empty;
                    if (deliveryid == "2")
                    {
                        beginTime = now.AddHours(Convert.ToDouble(stockUpPeriod)).ToString("HH:mm");
                    }
                    else if (deliveryid == "4")
                    {
                        beginTime = now.AddDays(Convert.ToDouble(stockUpPeriod)).ToString("HH:mm");
                    }

                    if (i == beginDateTime)
                    {
                        //if (beginTime.CompareTo(timeTemp[0]) < 0)
                        //{
                            timeRange.Add(entity.Quantum);
                        //}
                    }
                    else
                    {
                        timeRange.Add(entity.Quantum);
                    }

                    //如果在备货完的结束日期没有时间段可以选择，则后一天为允许选择的开始时间
                    //if (beginTime.CompareTo(timeTemp[0]) < 0)
                    //}
                }
                


                var dateTimeInfo = new DateTimeInfo
                {
                    dataRange = i.ToString("yyyy-MM-dd"),
                    timeRange = timeRange.ToArray()
                };
                dateTimeInfos.Add(dateTimeInfo);
            }

            rd.DateRange = dateTimeInfos.ToArray();


            return rd;
        }
    }
}