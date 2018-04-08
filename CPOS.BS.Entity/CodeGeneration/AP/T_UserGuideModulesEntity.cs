/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/8/24 15:41:50
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
    public partial class T_UserGuideModulesEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public T_UserGuideModulesEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public Guid? UserGuideModulesId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ModuleName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ModuleCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? ModuleStep { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ModuleType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String VideoUrl { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ImageUrl1 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ImageUrl2 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ImageUrl3 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Url { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Field1 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Field2 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Field3 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Remark { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Guid? ParentModule { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String CreateBy { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String LastUpdateBy { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? IsDelete { get; set; }


        #endregion

    }
}