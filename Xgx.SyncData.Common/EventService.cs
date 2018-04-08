using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyNetQ;
using Zmind.EventBus.Contract;

namespace Xgx.SyncData.Common
{
    public class EventService
    {

        public void PublishMsg(EventContract contract)
        {
            var bus = MqBusMgr.GetInstance();
            if (!ConfigMgr.IsOpenEventBus) return;
            if (contract.EntityType == EntityTypeEnum.Vip && ConfigMgr.IsOpenVipEvent)
            {
                bus.Publish(contract);
            }
            else if(contract.EntityType == EntityTypeEnum.Unit && ConfigMgr.IsOpenUnitEvent)
            {
                bus.Publish(contract);
            }
            else if (contract.EntityType == EntityTypeEnum.User && ConfigMgr.IsOpenUserEvent)
            {
                bus.Publish(contract);
            }
            else if (contract.EntityType == EntityTypeEnum.Order && ConfigMgr.IsOpenOrderEvent)
            {
                bus.Publish(contract);
            }
            else if (contract.EntityType == EntityTypeEnum.VipCardType && ConfigMgr.IsOpenVipCardTypeEvent)
            {
                bus.Publish(contract);
            }
            else if (contract.EntityType == EntityTypeEnum.OrderComment && ConfigMgr.IsOpenOrderEvaluationEvent)
            {
                bus.Publish(contract);
            }
            else if (contract.EntityType == EntityTypeEnum.OrderPayment && ConfigMgr.IsOpenOrderPaymanetEvent)
            {
                bus.Publish(contract);
            }
        }
    }
}
