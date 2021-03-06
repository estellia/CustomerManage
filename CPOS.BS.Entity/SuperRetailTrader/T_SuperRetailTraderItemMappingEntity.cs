/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/5/28 10:12:43
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
    public partial class T_SuperRetailTraderItemMappingEntity : BaseEntity 
    {
        #region ���Լ�

        public string ItemName { get; set; }

        public string ItemCode { get; set; }

        public int DisplayIndex { get; set; }

        public int IsSelected { get; set; }

        public string ImageUrl { get; set; }

        public string Prop1Name { get; set; }

        public string Prop2Name { get; set; }

        public decimal Price { get; set; }
        public string ItemIntroduce { get; set; }

        #endregion
    }
}