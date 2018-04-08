using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIT.CPOS.BS.BLL.RedisOperationBLL.OrderNotPay;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.Common;
using JIT.Utility.DataAccess.Query;
using Xgx.SyncData.Common;
using Zmind.EventBus.Contract;

namespace JIT.CPOS.BS.BLL.RedisOperationBLL.OrderConfirm
{
    public class OrderAutoConfrimMsgSetJobBLL
    {
        /// <summary>
        /// 所有商户
        /// </summary>
        private Dictionary<string, string> _CustomerIDList { get; set; }
        /// <summary>
        /// SessionInfo
        /// </summary>
        private LoggingSessionInfo _T_loggingSessionInfo { get; set; }
        /// <summary>
        /// 订单详情
        /// </summary>
        private InoutService _Inout3Service { get; set; }

        /// <summary>
        /// 订单
        /// </summary>
        private T_InoutBLL _T_InoutBLL { get; set; }

        private VipBLL _VipBLL { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderAutoConfrimMsgSetJobBLL()
        {
            _T_loggingSessionInfo = new LoggingSessionInfo();
            _T_loggingSessionInfo.CurrentLoggingManager = new LoggingManager();
        }

        public void AutoSetOrderNotPayCache()
        {
            foreach (var customer in _CustomerIDList)
            {
                _T_loggingSessionInfo.ClientID = customer.Key;
                _T_loggingSessionInfo.CurrentLoggingManager.Connection_String = customer.Value;

                //
                _T_InoutBLL = new T_InoutBLL(_T_loggingSessionInfo);
                _Inout3Service = new InoutService(_T_loggingSessionInfo);
                var inoutStatus = new TInoutStatusBLL(_T_loggingSessionInfo);
                _VipBLL = new VipBLL(_T_loggingSessionInfo);

                //
                // var t_InoutList = new List<string>();
                try
                {

                    List<IWhereCondition> complexCondition = new List<IWhereCondition>();
                    string[] statusArr = { "620", "610", "600" };
                    complexCondition.Add(new InCondition<string>() { FieldName = "Field7", Values = statusArr });
                    complexCondition.Add(
                        new DirectCondition(
                            " CONVERT(NVARCHAR(10),complete_date,120) = CONVERT(NVARCHAR(10),DATEADD(day, -1, GETDATE()),120)"));

                    var t_InoutEntitys = _T_InoutBLL.Query(complexCondition.ToArray(), null);
                    if (t_InoutEntitys == null || t_InoutEntitys.Count() <= 0)
                    {

                    }
                    //
                    //   roleList = roleEntities.Select(it => it.role_id).ToList();

                    foreach (var t_InoutInfo in t_InoutEntitys)
                    {

                        TInoutStatusEntity info = new TInoutStatusEntity();
                        info.InoutStatusID = Guid.Parse(Utils.NewGuid());
                        info.OrderID = t_InoutInfo.order_id;
                        info.CustomerID = _T_loggingSessionInfo.ClientID;
                        info.Remark = string.Empty;
                        info.OrderStatus = 700;

                        string statusDesc = GetStatusDesc("700");//变更后的状态名称

                        try
                        {
                            info.StatusRemark = "订单状态从" + t_InoutInfo.status_desc + "变为" + statusDesc + "[操作人:自动]";
                            _Inout3Service.UpdateOrderDeliveryStatus(t_InoutInfo.order_id, "700", Utils.GetNow());
                        }
                        catch
                        {
                            continue;
                        }

                        inoutStatus.Create(info);

                        #region 支付成功，调用RabbitMQ发送给ERP

                        try
                        {
                            var msg = new EventContract
                            {
                                Operation = OptEnum.Update,
                                EntityType = EntityTypeEnum.Order,
                                Id = t_InoutInfo.order_id
                            };
                            var eventService = new EventService();
                            eventService.PublishMsg(msg);

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }

                        #endregion

                    }


                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 获取订单对应状态描述
        /// </summary>
        /// <param name="status">订单状态</param>
        /// <returns>状态描述</returns>
        private string GetStatusDesc(string status)
        {
            string str = "";
            OptionsBLL optionsBll = new OptionsBLL(_T_loggingSessionInfo);
            var optionsList = optionsBll.QueryByEntity(new OptionsEntity
            {
                OptionValue = Convert.ToInt32(status)
                ,
                IsDelete = 0
                ,
                OptionName = "TInOutStatus"
                ,
                CustomerID = _T_loggingSessionInfo.ClientID
            }, null);
            if (optionsList != null && optionsList.Length > 0)
            {
                str = optionsList[0].OptionText;
            }
            return str;
        }
    }
}
