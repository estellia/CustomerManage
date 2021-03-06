/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/8/30 9:30:35
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
    public partial class ItemDownloadLogEntity : BaseEntity 
    {
        #region ���Լ�
        public IList<ItemDownloadLogEntity> ItemDownloadLogList { get; set; }

        public string UserName { get; set; }

        public string ImageUrl { get; set; }

        public Int64 DisplayIndex { get; set; }

        public Int32 TotalCount { get; set; }
        #endregion
    }
}