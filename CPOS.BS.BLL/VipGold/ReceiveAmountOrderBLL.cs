/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/6/27 14:14:27
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
    /// 业务处理： 10：支付成功   90：支付失败 
    /// </summary>
    public partial class ReceiveAmountOrderBLL
    {

        /// <summary>
        /// 获取订单总数,应付总金额，实付总金额
        /// </summary>
        /// <param name="ServiceUserId"></param>
        /// <returns></returns>
        public List<ReceiveAmountOrderEntity> GetOrderCount(string ServiceUserId)
        {
            return _currentDAO.GetOrderCount(ServiceUserId);
        }
    }
}