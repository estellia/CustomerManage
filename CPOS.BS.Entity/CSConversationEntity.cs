/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/3/5 18:00:39
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
    /// 实体： 交流记录 
    /// </summary>
    public partial class CSConversationEntity : BaseEntity 
    {
        #region 属性集
        /// <summary>
        /// 会话的员工头像(微信端用的)
        /// </summary>
        public string UserHeadUrl { get; set; }
     
    
        /// <summary>
        /// 会话的会员头像
        /// </summary>
        public string VipHeadImage { get; set; }
        /// <summary>
        /// 会话的会员标识
        /// </summary>
        public string VipID { get; set; }

  /// <summary>
        /// 会话的会员名称
        /// </summary>
        public string VipName { get; set; }
        
        #endregion
    }
}