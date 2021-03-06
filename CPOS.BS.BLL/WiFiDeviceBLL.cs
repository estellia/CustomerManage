/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/5/10 12:44:22
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
    public partial class WiFiDeviceBLL
    {

        #region  根据节点编号获取实例
        /// <summary>
        /// 根据节点编号获取实例
        /// </summary>
        /// <param name="NodeSn">节点编号</param>
        /// <returns></returns>
        public WiFiDeviceEntity GetByNodeSn(string NodeSn)
        {
            return _currentDAO.GetByNodeSn(NodeSn);
        }
        #endregion


    }
}