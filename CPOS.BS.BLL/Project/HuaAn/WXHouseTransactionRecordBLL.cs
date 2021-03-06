/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/6/18 13:46:24
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
using JIT.CPOS.BS.DataAccess;
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess;
using JIT.Utility.DataAccess.Query;

namespace JIT.CPOS.BS.BLL
{
    /// <summary>
    /// 业务处理： 操作历史记录表：购买 支付 赎回（三种操作 都需要记录在此表中） 
    /// </summary>
    public partial class WXHouseTransactionRecordBLL
    {
        /// <summary>
        /// 根据PrePaymentID获取支付记录。
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public WXHouseTransactionRecordEntity GetByPrePaymentID(string customerID, string prePaymentID)
        {
            return _currentDAO.GetByPrePaymentID(customerID, prePaymentID);
        }
    }
}