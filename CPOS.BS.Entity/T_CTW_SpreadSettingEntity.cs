/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/3/18 14:58:43
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
    /// 实体： Share 分享   Focus 关注   Reg 注册 
    /// </summary>
    public partial class T_CTW_SpreadSettingEntity : BaseEntity 
    {
        #region 属性集
        /// <summary>
        /// 引导关注公众号二维码
        /// </summary>
        public string LeadPageQRCodeImageUrl { get; set; }
        /// <summary>
        /// 背景图片
        /// </summary>
        public string BGImageUrl { get; set; }

        #endregion
    }
}