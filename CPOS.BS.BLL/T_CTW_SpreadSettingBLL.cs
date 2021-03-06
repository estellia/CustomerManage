/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/3/18 14:58:43
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
    /// 业务处理： Share 分享   Focus 关注   Reg 注册 
    /// </summary>
    public partial class T_CTW_SpreadSettingBLL
    {
        public void DeleteByCTWEventID(string strCTWEventId)
        {
            this._currentDAO.DeleteByCTWEventID(strCTWEventId);
        }
        public DataSet GetSpreadSettingByCTWEventId(string strCTWEventId)
        {
            return this._currentDAO.GetSpreadSettingByCTWEventId(strCTWEventId);
        }
        public DataSet GetSpreadSettingQRImageByCTWEventId(string strCTWEventId, string strSpreadType)
        {
            return this._currentDAO.GetSpreadSettingQRImageByCTWEventId(strCTWEventId, strSpreadType);
        }
    }
}