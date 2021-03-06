/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-7-3 10:30:22
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
    public partial class T_SalesReturnBLL
    {
        public System.Data.SqlClient.SqlTransaction GetTran()
        {
            return this._currentDAO.GetTran();
        }
        /// <summary>
        /// 判断是否可在申请退换货服务 0=可申请；1=不可申请
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public int CheckSalesReturn(string orderId, string skuId)
        {
            var salesReturnList = this._currentDAO.QueryByEntity(new T_SalesReturnEntity() {OrderID= orderId,SkuID= skuId}, null);
            return salesReturnList.Count();
        }
        /// <summary>
        /// 条件获取退货单
        /// </summary>
        /// <param name="SalesReturnNo">退货单号</param>
        /// <param name="DeliveryType">配送凡是</param>
        /// <param name="Status">状态</param>
        /// <param name="paymentcenterId">商户单号</param>
        /// <param name="payId">支付方式</param>
        /// <returns></returns>
        public DataSet GetWhereSalesReturnOrder(string SalesReturnNo, int DeliveryType, int Status, string paymentcenterId, string payId) {
            return this._currentDAO.GetWhereSalesReturnOrder(SalesReturnNo, DeliveryType, Status, paymentcenterId, payId);
        }
    }
}