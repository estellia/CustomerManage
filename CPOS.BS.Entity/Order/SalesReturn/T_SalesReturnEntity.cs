/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-7-3 10:30:22
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
    public partial class T_SalesReturnEntity : BaseEntity 
    {
        #region 属性集
        public string OrderNo { get; set; }
        public string VipName { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayTypeName { get; set; }
        /// <summary>
        /// 商户单号
        /// </summary>
        public string paymentcenterId { get; set; }
        #endregion
    }
}