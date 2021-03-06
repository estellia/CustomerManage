﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JIT.Utility;
using JIT.Utility.Entity;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.DataAccess;
using JIT.Utility.Log;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.DataAccess.Base;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace JIT.CPOS.BS.DataAccess
{
    public partial class UnitDAO : Base.BaseCPOSDAO
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public UnitDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        /// <summary>
        /// 获取门店列表
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public DataSet GetUnitList(Hashtable ht)
        {
            var parm = new SqlParameter[10];
            parm[0] = new SqlParameter("@VipID", System.Data.SqlDbType.NVarChar) { Value = ht["MemberID"] };
            parm[1] = new SqlParameter("@PageIndex", System.Data.SqlDbType.Int) { Value = ht["PageIndex"] };
            parm[2] = new SqlParameter("@PageSize", System.Data.SqlDbType.Int) { Value = ht["PageSize"] };
            parm[3] = new SqlParameter("@Latitude", System.Data.SqlDbType.Decimal) { Value = ht["Latitude"] };
            parm[4] = new SqlParameter("@Longitude", System.Data.SqlDbType.Decimal) { Value = ht["Longitude"] };
            parm[5] = new SqlParameter("@Distance", System.Data.SqlDbType.Int) { Value = ht["Distance"] };
            parm[6] = new SqlParameter("@SortField", System.Data.SqlDbType.VarChar) { Value = ht["SortField"] };
            parm[7] = new SqlParameter("@SortType", System.Data.SqlDbType.VarChar) { Value = ht["SortType"] };
            parm[8] = new SqlParameter("@IndustryID", System.Data.SqlDbType.VarChar) { Value = ht["IndustryID"] };
            parm[9] = new SqlParameter("@SearchConditonal", System.Data.SqlDbType.VarChar) { Value = ht["UnitName"] };

            Loggers.Debug(new DebugLogInfo()
            {
                Message = parm.ToJSON()
            });
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "ProcGetUnitList", parm);
        }
        /// <summary>
        /// 获取门店详情
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public DataSet GetUnitDetail(string memberId, string unitId)
        {
            var parm = new SqlParameter[2];
            parm[0] = new SqlParameter("@VipID", System.Data.SqlDbType.NVarChar) { Value = memberId };
            parm[1] = new SqlParameter("@UnitID", System.Data.SqlDbType.NVarChar) { Value = unitId };

            Loggers.Debug(new DebugLogInfo()
            {
                Message = parm.ToJSON()
            });
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "ProcUnitDetail", parm);
        }

        /// <summary>
        /// 获取门店详情
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public t_unitEntity GetUnitDetail(string unitId)
        {
            string Sql = "SELECT * FROM T_Unit WHERE unit_id=@UnitID";
            SqlParameter[] parameter = new SqlParameter[]{
                 new SqlParameter("@UnitID",unitId)
            };
            t_unitEntity model = new t_unitEntity();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(CommandType.Text, Sql, parameter))
            {
                while (rdr.Read())
                {
                    model.unit_name = rdr["unit_name"] + "";
                }
            }
            return model;
        }

        #region 个人积分列表与汇总
        /// <summary>
        /// 获取个人积分列表
        /// </summary>
        /// <param name="CustomerId">商户编号</param>
        /// <param name="wherecondition">条件汇总</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageSize">每页显示条数</param>
        /// <returns></returns>
        public PagedQueryResult<VipIntegralDetailEntity> GetMyIntegral(IWhereCondition[] wherecondition, int PageIndex, int PageSize)
        {
       
            StringBuilder pagedSql = new StringBuilder();
            StringBuilder totalCountSql = new StringBuilder();
            pagedSql.Append(@"SELECT *
                                FROM   (
                                           SELECT ROW_NUMBER() OVER(ORDER BY L.CreateTime DESC) AS RowNumber,
                                                  S.IntegralSourceName    AS UpdateReason,
                                                  CONVERT(INT, Integral)  AS UpdateCount,
                                                  L.CreateTime            AS UpdateTime,
                                                  L.VipCardCode,
                                                   Integral
                                           FROM   VipIntegralDetail       AS L
                                                  INNER JOIN SysIntegralSource AS S
                                                       ON  S.IntegralSourceID = L.IntegralSourceID
                                           WHERE  L.IsDelete = 0 ");


            totalCountSql.Append(@"select count(1) from VipIntegralDetail       AS L
                                                  INNER JOIN SysIntegralSource AS S
                                                       ON  S.IntegralSourceID = L.IntegralSourceID
                                           WHERE  L.IsDelete = 0");

            foreach (var item in wherecondition)
            {
                pagedSql.AppendFormat(" and {0}", item.GetExpression());
                totalCountSql.AppendFormat(" and {0}", item.GetExpression());

            }
            pagedSql.Append(@" ) AS Temp
                                WHERE  Temp.RowNumber > " + (PageIndex) * PageSize + @"
                                       AND Temp.RowNumber <= " + (PageIndex + 1) * PageSize);


       
            PagedQueryResult<VipIntegralDetailEntity> result = new PagedQueryResult<VipIntegralDetailEntity>();
            List<VipIntegralDetailEntity> list = new List<VipIntegralDetailEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    VipIntegralDetailEntity m = new VipIntegralDetailEntity();

                    if (rdr["UpdateReason"] != DBNull.Value)
                    {
                        m.UpdateReason = Convert.ToString(rdr["UpdateReason"]);
                    }

                    if (rdr["UpdateCount"] != DBNull.Value)
                    {
                        m.UpdateCount = Convert.ToInt32(rdr["UpdateCount"]);
                    }

                    if (rdr["UpdateTime"] != DBNull.Value)
                    {
                        m.UpdateTime = Convert.ToDateTime(rdr["UpdateTime"]);
                    }
                    list.Add(m);
                }
            }

            result.Entities = list.ToArray();

            int totalCount = Convert.ToInt32(this.SQLHelper.ExecuteScalar(totalCountSql.ToString()));    //计算总行数
            result.RowCount = totalCount;
            int remainder = 0;
            result.PageCount = Math.DivRem(totalCount, PageSize, out remainder);
            if (remainder > 0)
                result.PageCount++;
        
            return result;
        }

        /// <summary>
        /// 统计收入积分 支出积分 总积分信息
        /// </summary>
        /// <param name="CustomerId">商户编号</param>
        /// <param name="VipId">会员编号</param>
        /// <returns>
        /// decimal[0]=总积分
        /// decimal[1]=收入积分
        /// decimal[2]=支出积分
        /// </returns>
        public decimal[] GetMyTotalIntegral(string CustomerId, string VipId, string VipCardCode)
        {

            string totalsql = "SELECT SUM(Integral) AS Integral FROM VipIntegralDetail WHERE VipCardCode=@VipCardCode AND CustomerId=@CustomerId AND VIPID=@VipId UNION ALL";
            totalsql += " SELECT SUM(Integral) AS Integral  FROM VipIntegralDetail WHERE  VipCardCode=@VipCardCode AND CustomerId=@CustomerId AND VIPID=@VipId AND Integral > 0 UNION ALL ";
            totalsql += " SELECT SUM(Integral) AS Integral FROM VipIntegralDetail WHERE  VipCardCode=@VipCardCode AND CustomerId=@CustomerId AND VIPID=@VipId AND Integral < 0";
            List<decimal> list = new List<decimal>();

            SqlParameter[] parameter = new SqlParameter[]{
                new SqlParameter("@VipCardCode",VipCardCode),
                new SqlParameter("@CustomerId",CustomerId),
                new SqlParameter("@VipId",VipId)
            };

            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(CommandType.Text, totalsql, parameter))
            {
                while (rdr.Read())
                {
                    try
                    {
                        list.Add(Convert.ToInt32(rdr["Integral"] == null ? 0 : rdr["Integral"]));
                    }
                    catch (Exception)
                    {

                        list.Add(0);
                    }
                }
            }
            return list.ToArray();
        }

            #endregion
        /// <summary>
        /// 获取账户余额
        /// </summary>
        /// <param name="hsPara"></param>
        /// <returns></returns>
        public DataSet GetMyAccount(Hashtable hsPara)
        {
            var parm = new SqlParameter[4];
            parm[0] = new SqlParameter("@VipID", System.Data.SqlDbType.NVarChar) { Value = hsPara["MemberID"] };
            parm[1] = new SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar) { Value = hsPara["CustomerID"] };
            parm[2] = new SqlParameter("@PageSize", System.Data.SqlDbType.NVarChar) { Value = hsPara["PageSize"] };
            parm[3] = new SqlParameter("@PageIndex", System.Data.SqlDbType.NVarChar) { Value = hsPara["PageIndex"] };
            Loggers.Debug(new DebugLogInfo()
            {
                Message = parm.ToJSON()
            });
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "ProcGetVipAmountDetail", parm);
        }

        /// <summary>
        /// 获取店铺优惠券
        /// </summary>
        /// <param name="hsPara"></param>
        /// <returns></returns>
        public DataSet GetCouponList(Hashtable hsPara)
        {
            var parm = new SqlParameter[7];
            parm[0] = new SqlParameter("@VipID", System.Data.SqlDbType.NVarChar) { Value = hsPara["MemberID"] };
            parm[1] = new SqlParameter("@CustomerID", System.Data.SqlDbType.NVarChar) { Value = hsPara["CustomerID"] };
            parm[2] = new SqlParameter("@Status", System.Data.SqlDbType.NVarChar) { Value = hsPara["Status"] };
            parm[3] = new SqlParameter("@PageSize", System.Data.SqlDbType.NVarChar) { Value = hsPara["PageSize"] };
            parm[4] = new SqlParameter("@PageIndex", System.Data.SqlDbType.NVarChar) { Value = hsPara["PageIndex"] };
            parm[5] = new SqlParameter("@UsableRange", System.Data.SqlDbType.Int) { Value = (hsPara["UsableRange"]) };
            parm[6] = new SqlParameter("@ObjectID", System.Data.SqlDbType.NVarChar) { Value = hsPara["ObjectID"] };

            Loggers.Debug(new DebugLogInfo()
            {
                Message = parm.ToJSON()
            });
            return this.SQLHelper.ExecuteDataset(CommandType.StoredProcedure, "ProcGetVipCouponDetail", parm);
        }
        /// <summary>
        /// 根据商品业务类型获取商品信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="itemSortId"></param>
        /// <returns></returns>
        public DataSet GetItemBySortId(string customerId, int itemSortId)
        {
            string sql = string.Format(@"
            SELECT ItemId FROM dbo.T_Item a
            INNER JOIN ItemItemSortMapping b ON a.item_id=b.ItemId AND ItemSortId={0}
            WHERE CustomerId='{1}'", itemSortId, customerId);
            return this.SQLHelper.ExecuteDataset(sql);
        }
        /// <summary>
        /// 根据SkuID获取价格信息
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public DataSet GetSkuPirce(string skuId)
        {
            string sql = string.Format(@"
            SELECT ReturnCash,SalesPrice FROM dbo.vw_sku_detail WHERE sku_id='{0}'", skuId);
            return this.SQLHelper.ExecuteDataset(sql);
        }
    }

}
