using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xgx.SyncData.Common
{
    public static class ConfigMgr
    {
        internal static string RabbitMqHost
        {
            get
            {
                if (string.IsNullOrEmpty(_rabbitMqHost))
                {
                    _rabbitMqHost = ConfigurationManager.AppSettings["RabbitMqHost"];
                }
                return _rabbitMqHost;
            }
        }
        public static string CustomerId
        {
            get
            {
                if (string.IsNullOrEmpty(_customerId))
                {
                    _customerId = ConfigurationManager.AppSettings["CustomerId"];
                }
                return _customerId;
            }
        }
        public static bool IsOpenEventBus
        {
            get
            {
                if (_isOpenEventBus == null)
                {
                    _isOpenEventBus = bool.Parse(ConfigurationManager.AppSettings["IsOpenEventBus"]);
                }
                return _isOpenEventBus.Value;
            }
        }
        public static bool IsOpenVipEvent
        {
            get
            {
                if (_isOpenVipEvent == null)
                {
                    _isOpenVipEvent = bool.Parse(ConfigurationManager.AppSettings["IsOpenVipEvent"]);
                }
                return _isOpenVipEvent.Value;
            }
        }
        public static bool IsOpenUnitEvent
        {
            get
            {
                if (_isOpenUnitEvent == null)
                {
                    _isOpenUnitEvent = bool.Parse(ConfigurationManager.AppSettings["IsOpenUnitEvent"]);
                }
                return _isOpenUnitEvent.Value;
            }
        }
        public static bool IsOpenUserEvent
        {
            get
            {
                if (_isOpenUserEvent == null)
                {
                    _isOpenUserEvent = bool.Parse(ConfigurationManager.AppSettings["IsOpenUserEvent"]);
                }
                return _isOpenUserEvent.Value;
            }
        }
        public static bool IsOpenOrderEvent
        {
            get
            {
                if (_isOpenOrderEvent == null)
                {
                    _isOpenOrderEvent = bool.Parse(ConfigurationManager.AppSettings["IsOpenOrderEvent"]);
                }
                return _isOpenOrderEvent.Value;
            }
        }
        public static bool IsOpenVipCardTypeEvent
        {
            get
            {
                if (_isOpenVipCardTypeEvent == null)
                {
                    _isOpenVipCardTypeEvent = bool.Parse(ConfigurationManager.AppSettings["IsOpenVipCardTypeEvent"]);
                }
                return _isOpenVipCardTypeEvent.Value;
            }
        }
        public static bool IsOpenOrderPaymanetEvent
        {
            get
            {
                if (_isOpenOrderPaymanetEvent == null)
                {
                    _isOpenOrderPaymanetEvent = bool.Parse(ConfigurationManager.AppSettings["IsOpenOrderPaymanetEvent"]);
                }
                return _isOpenOrderPaymanetEvent.Value;
            }
        }
        public static bool IsOpenOrderEvaluationEvent
        {
            get
            {
                if (_isOpenOrderEvaluationEvent == null)
                {
                    _isOpenOrderEvaluationEvent = bool.Parse(ConfigurationManager.AppSettings["IsOpenOrderEvaluationEvent"]);
                }
                return _isOpenOrderEvaluationEvent.Value;
            }
        }
        private static string _rabbitMqHost;
        private static string _customerId;
        private static bool? _isOpenEventBus;
        private static bool? _isOpenVipEvent;
        private static bool? _isOpenUnitEvent;
        private static bool? _isOpenUserEvent;
        private static bool? _isOpenOrderEvent;
        private static bool? _isOpenVipCardTypeEvent;
        private static bool? _isOpenOrderPaymanetEvent;
        private static bool? _isOpenOrderEvaluationEvent;
    }
}
