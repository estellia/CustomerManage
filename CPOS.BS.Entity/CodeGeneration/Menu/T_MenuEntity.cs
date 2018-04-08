/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016-8-20 14:58:24
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
    public partial class T_MenuEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public T_MenuEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public String menu_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String reg_app_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String menu_code { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String parent_menu_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? menu_level { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String url_path { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String icon_path { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? display_index { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String menu_name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? user_flag { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String menu_eng_name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? status { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String create_user_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String create_time { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String modify_user_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String modify_time { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String customer_id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? IsCanAccess { get; set; }


        #endregion

    }
}