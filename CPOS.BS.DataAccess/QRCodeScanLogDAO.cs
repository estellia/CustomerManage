/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/2/20 11:34:32
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
using System.Data;
using System.Data.SqlClient;
using System.Text;

using JIT.Utility;
using JIT.Utility.Entity;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.DataAccess;
using JIT.Utility.Log;
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.DataAccess.Base;

namespace JIT.CPOS.BS.DataAccess
{
    
    /// <summary>
    /// 数据访问：  
    /// 表QRCodeScanLog的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class QRCodeScanLogDAO : Base.BaseCPOSDAO, ICRUDable<QRCodeScanLogEntity>, IQueryable<QRCodeScanLogEntity>
    {
        public string CheckVipEventQRCode(string vipId, string eventId)
        {
            string result;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@VipId", vipId));
            parameters.Add(new SqlParameter("@EventId", eventId));

            result = (this.SQLHelper.ExecuteScalar(CommandType.StoredProcedure, "CheckVipEventQRCode", parameters.ToArray()) ?? "").ToString();

            return result;
        }
    }
}
