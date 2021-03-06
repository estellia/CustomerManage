/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013-12-14 15:57
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
    /// 表CouponType的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class CouponTypeDAO : Base.BaseCPOSDAO, ICRUDable<CouponTypeEntity>, IQueryable<CouponTypeEntity>
    {


        /// <summary>
        /// 获取已被会员领取的优惠券统计
        /// </summary>
        /// <param name="CouponTypeID"></param>
        /// <returns></returns>
        public int GetCouponCount(string CouponTypeID)
        {
            int num = 0;

            string sql = string.Format(@"select count(1) from coupon as a 
                                         inner join VipCouponMapping as b on a.CouponID=b.CouponID and b.IsDelete=0 
                                         where a.CustomerID='{0}' and a.CouponTypeID='{1}'", CurrentUserInfo.ClientID, CouponTypeID);

            var Result = this.SQLHelper.ExecuteScalar(sql);
            if (Result != null)
                num = Convert.ToInt32(Result);
            return num;
        }

        #region 获取优惠劵类别
        /// <summary>
        /// 获取优惠劵列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetCouponType()
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("select convert(nvarchar(50),CouponTypeID),CoupontypeName from CouponType where IsDelete='0' and (CustomerID is null or CustomerID = '");
            strb.Append(CurrentUserInfo.ClientID);
            strb.Append("')");
            return this.SQLHelper.ExecuteDataset(strb.ToString());
        }
        #endregion

        #region 获取优惠券类别
        public DataSet GetCouponTypeList()
        {
            //updated by Willie: CustomerId is null 为通用类型 on 2014-09-17
            string sql = @"SELECT 
                         C.CouponTypeID
                        , C.CouponTypeName
                        , SUM(c.[IssuedQty]) IssuedQty
                        ,SUM(CountTotal) CountTotal
                        ,c.CreateTime
                        ,C.IsNotLimitQty
                         FROM  CouponType c
                        LEFT JOIN PrizeCouponTypeMapping p ON CAST(c.CouponTypeID AS NVARCHAR(200)) = p.CouponTypeID 
                        LEFT JOIN dbo.LPrizes l ON l.PrizesID = p.PrizesID AND [PrizeTypeId] ='Coupon'  
                        where  C.IsDelete='0' and c.IssuedQty>0 
                                and   C.CustomerId='" + this.CurrentUserInfo.ClientID + "' AND ((EndTime IS NULL AND ServiceLife IS NOT NULL) OR (EndTime IS NOT NULL AND EndTime >getdate())) GROUP BY c.CouponTypeID,c.CouponTypeName,c.CreateTime,C.IsNotLimitQty ORDER BY c.CreateTime DESC";
            return this.SQLHelper.ExecuteDataset(sql);

        }
        #endregion

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public Guid CreateReturnID(CouponTypeEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");

            //初始化固定字段
            pEntity.IsDelete = 0;
            pEntity.CreateTime = DateTime.Now;
            pEntity.LastUpdateTime = pEntity.CreateTime;
            pEntity.CreateBy = CurrentUserInfo.UserID;
            pEntity.LastUpdateBy = CurrentUserInfo.UserID;


            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [CouponType](");
            strSql.Append("[CouponTypeName],[CouponTypeCode],[CouponCategory],[ParValue],[Discount],[ConditionValue],[IsRepeatable],[IsMixable],[CouponSourceID],[ValidPeriod],[LastUpdateTime],[LastUpdateBy],[CreateTime],[CreateBy],[IsDelete],[CustomerId],[IssuedQty],[IsVoucher],[UsableRange],[ServiceLife],[SuitableForStore],[BeginTime],[EndTime],[CouponTypeDesc],[IsNotLimitQty],[CouponTypeID])");
            strSql.Append(" values (");
            strSql.Append("@CouponTypeName,@CouponTypeCode,@CouponCategory,@ParValue,@Discount,@ConditionValue,@IsRepeatable,@IsMixable,@CouponSourceID,@ValidPeriod,@LastUpdateTime,@LastUpdateBy,@CreateTime,@CreateBy,@IsDelete,@CustomerId,@IssuedQty,@IsVoucher,@UsableRange,@ServiceLife,@SuitableForStore,@BeginTime,@EndTime,@CouponTypeDesc,@IsNotLimitQty,@CouponTypeID)");

            Guid? pkGuid;
            if (pEntity.CouponTypeID == null)
                pkGuid = Guid.NewGuid();
            else
                pkGuid = pEntity.CouponTypeID;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@CouponTypeName",SqlDbType.NVarChar),
					new SqlParameter("@ParValue",SqlDbType.Decimal),
					new SqlParameter("@Discount",SqlDbType.Decimal),
					new SqlParameter("@ConditionValue",SqlDbType.Decimal),
					new SqlParameter("@IsRepeatable",SqlDbType.Int),
					new SqlParameter("@IsMixable",SqlDbType.Int),
					new SqlParameter("@CouponSourceID",SqlDbType.NVarChar),
					new SqlParameter("@ValidPeriod",SqlDbType.Int),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@CreateBy",SqlDbType.NVarChar),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@CustomerId",SqlDbType.NVarChar),
					new SqlParameter("@IssuedQty",SqlDbType.Int),
					new SqlParameter("@IsVoucher",SqlDbType.Int),
					new SqlParameter("@UsableRange",SqlDbType.Int),
					new SqlParameter("@ServiceLife",SqlDbType.Int),
					new SqlParameter("@SuitableForStore",SqlDbType.Int),
					new SqlParameter("@BeginTime",SqlDbType.DateTime),
					new SqlParameter("@EndTime",SqlDbType.DateTime),
					new SqlParameter("@CouponTypeDesc",SqlDbType.NVarChar),
					new SqlParameter("@CouponTypeID",SqlDbType.UniqueIdentifier),
                    new SqlParameter("@CouponTypeCode",SqlDbType.VarChar),
					new SqlParameter("@CouponCategory",SqlDbType.VarChar),
					new SqlParameter("@IsNotLimitQty",SqlDbType.Int)
            };
            parameters[0].Value = pEntity.CouponTypeName;
            parameters[1].Value = pEntity.ParValue;
            parameters[2].Value = pEntity.Discount;
            parameters[3].Value = pEntity.ConditionValue;
            parameters[4].Value = pEntity.IsRepeatable;
            parameters[5].Value = pEntity.IsMixable;
            parameters[6].Value = pEntity.CouponSourceID;
            parameters[7].Value = pEntity.ValidPeriod;
            parameters[8].Value = pEntity.LastUpdateTime;
            parameters[9].Value = pEntity.LastUpdateBy;
            parameters[10].Value = pEntity.CreateTime;
            parameters[11].Value = pEntity.CreateBy;
            parameters[12].Value = pEntity.IsDelete;
            parameters[13].Value = pEntity.CustomerId;
            parameters[14].Value = pEntity.IssuedQty;
            parameters[15].Value = pEntity.IsVoucher;
            parameters[16].Value = pEntity.UsableRange;
            parameters[17].Value = pEntity.ServiceLife;
            parameters[18].Value = pEntity.SuitableForStore;
            parameters[19].Value = pEntity.BeginTime;
            parameters[20].Value = pEntity.EndTime;
            parameters[21].Value = pEntity.CouponTypeDesc;
            parameters[22].Value = pkGuid;
            parameters[23].Value = pEntity.CouponTypeCode;
            parameters[24].Value = pEntity.CouponCategory;
            parameters[25].Value = pEntity.IsNotLimitQty;

            //执行并将结果回写
            int result;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            pEntity.CouponTypeID = pkGuid;
            return pkGuid.Value;
        }

        #region 获取活动中优惠券使用情况
        /// <summary>
        /// 获取奖品人员列表
        /// </summary>
        /// <param name="EventID"></param>
        /// <returns></returns>
        public DataSet GetCouponTypeCount()
        {
            string sql = "SELECT  c.CouponTypeID,SUM(c.[IssuedQty])[IssuedQty],SUM(CountTotal) CountTotal  "
                        + " FROM [dbo].[CouponType] C "
                        + " LEFT JOIN PrizeCouponTypeMapping  p ON CAST(C.CouponTypeID AS NVARCHAR(200))=p.CouponTypeID  "

                        + " INNER JOIN dbo.LPrizes l ON l.PrizesID=p.PrizesID  AND  [PrizeTypeId]=1   "
                        + " WHERE  c.IsDelete=0 "
                        + " GROUP BY c.CouponTypeID ";
            DataSet ds = new DataSet();
            return this.SQLHelper.ExecuteDataset(sql);
        }
        #endregion
        /// <summary>
        /// 根据CouponTypeID获取生成了多少券
        /// </summary>
        /// <param name="strCouponTypeID"></param>
        /// <returns></returns>
        public int GetCouponCountByCouponTypeID(string strCouponTypeID)
        {
            string strSql = string.Format("SELECT IssuedQty FROM CouponType WHERE CouponTypeID='{0}'", strCouponTypeID);
            return Convert.ToInt32(this.SQLHelper.ExecuteScalar(strSql));
        }
        /// <summary>
        /// 优惠券名是否重复
        /// </summary>
        /// <param name="strConponTypeName"></param>
        /// <returns></returns>
        public int ExistsCouponTypeName(string strConponTypeName,string strCustomerId)
        {
            string strSql = string.Format("SELECT Count(1) FROM CouponType WHERE IsDelete=0 and CouponTypeName='{0}' and CustomerId='{1}'", strConponTypeName, strCustomerId);
            return Convert.ToInt32(this.SQLHelper.ExecuteScalar(strSql));
        }
        /// <summary>
        /// 更新优惠券已使用量
        /// </summary>
        /// <returns></returns>
        public void UpdateCouponTypeIsVoucher(string strCustomerId)
        {
            string strSql = string.Format(@"UPDATE dbo.CouponType
                                                SET IsVoucher=a.CouponCount
                                            FROM (
                                                    SELECT CouponTypeID
                                                        ,COUNT(1)CouponCount 
                                                    FROM dbo.Coupon
                                                    WHERE  CustomerId = '{0}'
                                                    GROUP BY CouponTypeID
                                                ) a,CouponType b
                                            WHERE a.CouponTypeID=b.CouponTypeID  
                                                    AND b.CustomerId='{0}' ", strCustomerId);
            this.SQLHelper.ExecuteScalar(strSql);
        }
        /// <summary>
        /// 更新优惠券总量
        /// </summary>
        /// <param name="strCustomerId"></param>
        /// <param name="intIssuedQty"></param>
        public void UpdateCouponTypeIssuedQty(string strCouponTypeId, int intIssuedQty)
        {
            string strSql = string.Format(@"UPDATE dbo.CouponType
                                                SET IssuedQty=IssuedQty+{1}
                                            WHERE  CAST(CouponTypeID AS NVARCHAR(50))='{0}' ", strCouponTypeId, intIssuedQty);
            this.SQLHelper.ExecuteScalar(strSql);
        }
    }
}
