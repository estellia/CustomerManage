/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/5/16 13:59:40
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
    public partial class SetoffToolsEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public SetoffToolsEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public Guid? SetoffToolID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Guid? SetoffEventID { get; set; }

		/// <summary>
		/// CTW：创意仓库   Coupon：优惠券   SetoffPoster：集客报
		/// </summary>
		public String ToolType { get; set; }

		/// <summary>
		/// ToolType=CTW，创意活动ID，T_CTW_LEvent.CTWEventId   ToolType=Coupon：优惠券种，CouponType.CouponTypeID   ToolType=SetoffPoster：集客海报，SetoffPoster.SetoffPosterID
		/// </summary>
		public String ObjectId { get; set; }

		/// <summary>
		/// 10：使用中   90：失效
		/// </summary>
		public String Status { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String CustomerId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String CreateBy { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String LastUpdateBy { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? IsDelete { get; set; }


        #endregion

    }
}