/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/6/25 9:57:34
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
    public partial class ReservationServiceScheduleEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public ReservationServiceScheduleEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public String ReservationServiceScheduleID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? ReservationServiceID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? ReservationServiceBigClassTermID { get; set; }

		/// <summary>
		/// 
		/// </summary>
        public Int32? ReservationServiceSmallClassTermID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String VIPID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ReserveDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Remark { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? PositionID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? ReserveTypeID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? StatusID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? ReservationStoreID { get; set; }

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