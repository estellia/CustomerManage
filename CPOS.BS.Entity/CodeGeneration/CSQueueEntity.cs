/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/2/27 14:13:24
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
    /// 实体： 客服队列 
    /// </summary>
    public partial class CSQueueEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public CSQueueEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public Guid? CSQueueID { get; set; }

		/// <summary>
		/// CSMessageID
		/// </summary>
		public Guid? CSMessageID { get; set; }

		/// <summary>
		/// CSServiceTypeID
		/// </summary>
		public Int32? CSServiceTypeID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? CSPipelineID { get; set; }

		/// <summary>
		/// ClientID
		/// </summary>
		public String ClientID { get; set; }

		/// <summary>
		/// CreateBy
		/// </summary>
		public String CreateBy { get; set; }

		/// <summary>
		/// CreateTime
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// LastUpdateBy
		/// </summary>
		public String LastUpdateBy { get; set; }

		/// <summary>
		/// LastUpdateTime
		/// </summary>
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
		/// IsDelete
		/// </summary>
		public Int32? IsDelete { get; set; }


        #endregion

    }
}