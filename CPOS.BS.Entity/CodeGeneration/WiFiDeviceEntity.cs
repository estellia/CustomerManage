/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/5/10 12:44:21
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
    /// WiFi�豸  
    /// </summary>
    public partial class WiFiDeviceEntity : BaseEntity 
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public WiFiDeviceEntity()
        {
        }
        #endregion     

        #region ���Լ�
		/// <summary>
        /// ����
		/// </summary>
		public Guid? DeviceID { get; set; }

        /// <summary>
        /// �ڵ���
        /// </summary>
        public String NodeSn { get; set; }

		/// <summary>
        /// �ŵ�ID
		/// </summary>
		public String UnitID { get; set; }

		/// <summary>
        /// λ������
		/// </summary>
		public String LocationDesc { get; set; }

		/// <summary>
        /// ״̬
		/// </summary>
		public String Status { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String CustomerID { get; set; }

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