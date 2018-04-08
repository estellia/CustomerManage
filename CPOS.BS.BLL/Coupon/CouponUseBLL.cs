/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-1-15 20:06:20
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
    /// ҵ������  
    /// </summary>
    public partial class CouponUseBLL
    {
        #region ����ʹ�õ��Ż�ȯ�б�

        /// <summary>
        /// ����ʹ�õ��Ż�ȯ�б�
        /// </summary>
        /// <param name="vipId">�û�ID</param>
        /// <param name="orderId">����ID</param>
        /// <returns></returns>
        public DataSet GetOrderCouponUseList(string vipId, string orderId)
        {
            return this._currentDAO.GetOrderCouponUseList(vipId, orderId);
        }

        /// <summary>
        /// ��ȡ�û�ʹ�õ��Ż�ȯ�б�
        /// </summary>
        /// <param name="vipID">�û�ID</param>
        /// <param name="CouponID">�Ż�ȯID</param>
        /// <returns></returns>
        public DataSet GetCouponUseListByCouponID(string vipID, string CouponID)
        {
            return this._currentDAO.GetCouponUseListByCouponID(vipID, CouponID);
        }

        #endregion

        public decimal GetCouponParValue(string orderId)
        {
            return this._currentDAO.GetCouponParValue(orderId);
        }
    }
}