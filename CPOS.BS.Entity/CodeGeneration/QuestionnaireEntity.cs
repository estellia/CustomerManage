/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/8/5 9:34:36
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
    public partial class QuestionnaireEntity : BaseEntity 
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public QuestionnaireEntity()
        {
        }
        #endregion     

        #region 属性集
		/// <summary>
		/// 
		/// </summary>
		public String QuestionnaireID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String QuestionnaireName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String QuestionnaireDesc { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? DisplayIndex { get; set; }

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
		public String CustomerId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? ReleaseState { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? ReleaseTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime? EndTime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? Type { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? Redoable { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Remark { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String SurfaceImage { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? PassScore { get; set; }


        #endregion

    }
}