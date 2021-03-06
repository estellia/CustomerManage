/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-7-3 10:30:21
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
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.DataAccess.Base;

namespace JIT.CPOS.BS.DataAccess
{

    /// <summary>
    /// 数据访问：  
    /// 表T_RefundOrder的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class T_RefundOrderDAO : Base.BaseCPOSDAO, ICRUDable<T_RefundOrderEntity>, IQueryable<T_RefundOrderEntity>
    {

        /// <summary>
        /// 根据状态获取退款单
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public DataSet GetWhereRefundOrder(string RefundNo, string paymentcenterId, string payId, int Status, string CustomerID)
        {
            StringBuilder StrSql = new StringBuilder();
            StrSql.Append("select a.RefundNo as '订单号',a.ActualRefundAmount as '退款金额',b.VipName as '会员名称',");
            StrSql.Append("case a.[Status] when 1 then '待退款' when 2 then'已完成' else '' end as '订单状态',a.CreateTime as '创建时间',t.paymentcenter_id as '商户单号',p.Payment_Type_Name as '支付方式' from T_RefundOrder as a ");
            StrSql.Append("left join t_inout as t on a.OrderID=t.order_id ");
            StrSql.Append("left join T_Payment_Type as p on t.pay_id=p.Payment_Type_Id and p.IsDelete=0 ");
            StrSql.AppendFormat("left join Vip as b on a.VipID=b.VIPID and b.IsDelete=0 where a.IsDelete=0 and a.CustomerID='{0}' ", CustomerID);
            //条件
            if (!string.IsNullOrWhiteSpace(RefundNo))
                StrSql.AppendFormat("and a.RefundNo='{0}' ", RefundNo);
            if (!string.IsNullOrWhiteSpace(paymentcenterId))
                StrSql.AppendFormat("and t.paymentcenter_id='{0}' ", paymentcenterId);
            if (!string.IsNullOrWhiteSpace(payId))
                StrSql.AppendFormat("and t.pay_id='{0}' ", payId);
            if (Status > 0)
            {
                StrSql.AppendFormat("and a.[Status]={0} ", Status);
            }
            //排序
            StrSql.Append("order by a.CreateTime desc");
            return this.SQLHelper.ExecuteDataset(StrSql.ToString());
        }

    }
}
