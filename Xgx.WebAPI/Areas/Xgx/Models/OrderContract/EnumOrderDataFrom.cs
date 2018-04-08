using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public enum EnumOrderDataFrom
    {
        /// <summary>
        /// 线上微信订单
        /// </summary>
        Weixin = 3,

        /// <summary>
        /// 线下订单
        /// </summary>
        Offline = 4
    }
}
