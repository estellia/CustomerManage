/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/9/5 10:22:18
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
    /// 业务处理：  
    /// </summary>
    public partial class MarketSendLogBLL
    {
        #region 短信每天发送次数
        /// <summary>
        /// 短信每天发送次数
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="sendTypeId"></param>
        /// <returns></returns>
        public int GetSendCountByPhone(string Phone, string sendTypeId,string key)
        {
            return _currentDAO.GetSendCountByPhone(Phone,sendTypeId,key);
        }
        #endregion
    }
}