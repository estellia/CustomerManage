/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013-12-14 15:57
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
using System.Text;
using JIT.Utility;
using JIT.Utility.Entity;

namespace JIT.CPOS.BS.Entity
{

    /// <summary>
    /// 实体：  
    /// </summary>
    public partial class CouponTypeEntity : BaseEntity 
    {
        #region 属性集

        /// <summary>
        /// 微信客户VipId，仅作传输用
        /// </summary>
        public string VipId { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Item_Count { get; set; }

              /// <summary>
        /// 微信客户openid，仅作传输用
        /// </summary>
        public string WeiXinUserId { get; set; }

        
        #endregion
    }
}