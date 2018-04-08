using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xgx.WebAPI.Areas.Xgx.Models
{
    public enum EnumDelivery
    {
        /// <summary>
        /// 送货上门
        /// </summary>
        HomeDelivery = 1,
        /// <summary>
        /// 到店自提
        /// </summary>
        ShopPickUp = 2,
        /// <summary>
        /// 到店服务
        /// </summary>
        ShopService = 4
    }
}
