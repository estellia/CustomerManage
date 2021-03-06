﻿/*
 * Author		:Jack.Chen
 * EMail		:jack.chen@jitmarketing.cn
 * Company		:JIT
 * Create On	:2014/4/4 20:29:56
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

using JIT.CPOS.BS.Entity;

namespace JIT.CPOS.BS.DataAccess
{
    /// <summary>
    /// MHCategoryAreaDAO 
    /// </summary>
    public partial class MHCategoryAreaDAO
    {
       
        /// <summary>
        /// 根据GroupId获取ObjectTypeId
        /// </summary>
        /// <param name="intGroupId"></param>
        /// <returns></returns>
        public int GetObjectTypeIDByGroupId(int intGroupId)
        {
            string strSql = string.Format("SELECT TOP 1 ObjectTypeId FROM MHCategoryArea WHERE GroupId='{0}' ", intGroupId);
            return Convert.ToInt32(this.SQLHelper.ExecuteScalar(strSql));
        }
        public int DeleteCategoryAreaByGroupId(int intGroupId)
        {
            string strSql = string.Format("Delete  MHCategoryArea WHERE GroupId='{0}'", intGroupId);
            return Convert.ToInt32(this.SQLHelper.ExecuteScalar(strSql));

        }
    }
}
