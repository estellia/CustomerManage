/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/5/19 16:23:34
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
    public partial class R_SetoffHomeEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public R_SetoffHomeEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public Guid? ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? DateCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String CustomerId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? SetoffType { get; set; }

		/// <summary>
		/// CTW：创意仓库   Coupon：优惠券   SetoffPoster：集客报   Goods：商品
		/// </summary>
		public String SetoffToolType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? SetoffCount { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Decimal? SetoffCountPer { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }


        #endregion

    }
}