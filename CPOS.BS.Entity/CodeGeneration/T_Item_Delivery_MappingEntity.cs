/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/9/23 10:32:38
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
    public partial class T_Item_Delivery_MappingEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public T_Item_Delivery_MappingEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public String Item_Delivery_Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String CustomerId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Item_Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? DeliveryId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Create_Time { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Create_User_Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Modify_Time { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Modify_User_Id { get; set; }


        #endregion

    }
}