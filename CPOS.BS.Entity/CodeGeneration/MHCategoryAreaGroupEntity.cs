/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015/11/2 21:32:01
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
    public partial class MHCategoryAreaGroupEntity : BaseEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public MHCategoryAreaGroupEntity()
        {
        }
        #endregion

        #region 属性集
        /// <summary>
        /// 
        /// </summary>
        public Int32? GroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String ModelName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String ModelDesc { get; set; }

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
        public Int32? ModelTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String CustomerID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? GroupValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String StyleType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String TitleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String TitleStyle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? ShowCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? ShowName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? ShowPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? ShowSalesPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? ShowDiscount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? ShowSalesQty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? DisplayIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String HomeId { get; set; }


        #endregion

    }
}