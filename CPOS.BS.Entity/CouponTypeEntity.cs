/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013-12-14 15:57
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
    public partial class CouponTypeEntity : BaseEntity 
    {
        #region ���Լ�

        /// <summary>
        /// ΢�ſͻ�VipId������������
        /// </summary>
        public string VipId { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public int Item_Count { get; set; }

              /// <summary>
        /// ΢�ſͻ�openid������������
        /// </summary>
        public string WeiXinUserId { get; set; }

        
        #endregion
    }
}