/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-8-14 21:54:10
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
    public partial class VipCardBalanceChangeEntity : BaseEntity 
    {
        #region 属性集
        /// <summary>
        /// 门店
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateByName { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageURL { get; set; }
        #endregion
    }
}