/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/8/17 9:49:11
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
    public partial class ApplicationSupportLogEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public ApplicationSupportLogEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public Guid? ID { get; set; }

		/// <summary>
		/// 10：集客指标   20：售卡指标
		/// </summary>
		public String SurportType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Title { get; set; }

		/// <summary>
		/// 00：未生效   10：生效
		/// </summary>
		public String Content { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String SentUser { get; set; }

		/// <summary>
		/// 1：男   2：女
		/// </summary>
		public Int32? SentUserSex { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String SentUserPhone { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String SentEMail { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ReceiveEMail { get; set; }

		/// <summary>
		/// 1：成功   0：失败
		/// </summary>
		public Int32? IsSuccess { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String DefaultDescripton { get; set; }

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