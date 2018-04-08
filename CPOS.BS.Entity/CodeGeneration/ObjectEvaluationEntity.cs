/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/2/28 17:54:09
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
    public partial class ObjectEvaluationEntity : BaseEntity 
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public ObjectEvaluationEntity()
        {
        }
        #endregion     

        #region ���Լ�
		/// <summary>
		/// 
		/// </summary>
		public String EvaluationID { get; set; }

        /// <summary>
        /// ��ƷID�����ŵ�ID
        /// </summary>
        public String ObjectID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? Type { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String VipID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String CustomerID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Content { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? StarLevel { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? StarLevel1 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? StarLevel2 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? StarLevel3 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? StarLevel4 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? StarLevel5 { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Platform { get; set; }

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
		public String LastUpdateBy { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? IsDelete { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String OrderID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Remark { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? IsAnonymity { get; set; }


        #endregion

    }
}