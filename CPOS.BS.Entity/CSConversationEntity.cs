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
    /// ʵ�壺 ������¼ 
    /// </summary>
    public partial class CSConversationEntity : BaseEntity 
    {
        #region ���Լ�
        /// <summary>
        /// �Ự��Ա��ͷ��(΢�Ŷ��õ�)
        /// </summary>
        public string UserHeadUrl { get; set; }
     
    
        /// <summary>
        /// �Ự�Ļ�Աͷ��
        /// </summary>
        public string VipHeadImage { get; set; }
        /// <summary>
        /// �Ự�Ļ�Ա��ʶ
        /// </summary>
        public string VipID { get; set; }

  /// <summary>
        /// �Ự�Ļ�Ա����
        /// </summary>
        public string VipName { get; set; }
        
        #endregion
    }
}