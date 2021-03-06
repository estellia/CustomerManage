/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/7/3 18:46:09
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
    public partial class GLServicePersonInfoEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public GLServicePersonInfoEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 外键，引用客服表
		/// </summary>
		public String UserID { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// 手机
		/// </summary>
		public String Mobile { get; set; }

		/// <summary>
		/// 头像
		/// </summary>
		public String Picture { get; set; }

		/// <summary>
		/// 评论
		/// </summary>
		public Int32? Star { get; set; }

		/// <summary>
		/// 完成单数
		/// </summary>
		public Int32? OrderCount { get; set; }

		/// <summary>
		/// CustomerID
		/// </summary>
		public String CustomerID { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		public String CreateBy { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 最后更新人
		/// </summary>
		public String LastUpdateBy { get; set; }

		/// <summary>
		/// 最后更新时间
		/// </summary>
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
		/// 删除标识
		/// </summary>
		public Int32? IsDelete { get; set; }


        #endregion

    }
}