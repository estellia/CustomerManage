/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-8-14 20:18:28
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
    /// ʵ�壺  
    /// </summary>
    public partial class VipCardUnitMappingEntity : BaseEntity 
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public VipCardUnitMappingEntity()
        {
        }
        #endregion     

        #region ���Լ�
		/// <summary>
		/// 
		/// </summary>
		public String MappingID { get; set; }

		/// <summary>
		/// �ŵ��ʶ
		/// </summary>
		public String UnitID { get; set; }

		/// <summary>
		/// ��Ա����ʶ
		/// </summary>
		public String VipCardID { get; set; }

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

		/// <summary>
		/// 
		/// </summary>
		public String CustomerID { get; set; }


        #endregion

    }
}