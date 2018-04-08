/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/9/3 9:52:33
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
    public partial class WAutoReplyEntity : BaseEntity 
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public WAutoReplyEntity()
        {
        }
        #endregion     

        #region ���Լ�
		/// <summary>
		/// 
		/// </summary>
		public String ReplyId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Int32? NewsType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String Text { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String VoiceId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ImageId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String VideoId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String ApplicationId { get; set; }

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
		public String ModelId { get; set; }


        #endregion

    }
}