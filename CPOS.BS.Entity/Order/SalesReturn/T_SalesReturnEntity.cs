/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-7-3 10:30:22
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
    public partial class T_SalesReturnEntity : BaseEntity 
    {
        #region ���Լ�
        public string OrderNo { get; set; }
        public string VipName { get; set; }
        /// <summary>
        /// ֧����ʽ
        /// </summary>
        public string PayTypeName { get; set; }
        /// <summary>
        /// �̻�����
        /// </summary>
        public string paymentcenterId { get; set; }
        #endregion
    }
}