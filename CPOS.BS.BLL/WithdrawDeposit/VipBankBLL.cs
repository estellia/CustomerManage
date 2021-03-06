/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014-12-28 11:40:41
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
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using JIT.Utility;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.DataAccess;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.Entity;

namespace JIT.CPOS.BS.BLL
{
    /// <summary>
    /// 业务处理：  
    /// </summary>
    public partial class VipBankBLL
    {  
         /// <summary>
        /// 获取银行卡列表
        /// </summary>
        /// <param name="vipID"></param>
        /// <returns></returns>
        public DataSet GetVipBankList(string vipID)
        {
            return this._currentDAO.GetVipBankList(vipID);
        }
        /// <summary>
        /// 根据标识符获取实例[包含已删除]
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public VipBankEntity GetVipBankByID(object pID)
        {
            return this._currentDAO.GetVipBankByID(pID);
        }
    }
}