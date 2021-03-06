/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015/10/30 15:25:27
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
    /// 表VipIntegralDetail的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class VipIntegralDetailDAO : Base.BaseCPOSDAO, ICRUDable<VipIntegralDetailEntity>, IQueryable<VipIntegralDetailEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public VipIntegralDetailDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(VipIntegralDetailEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(VipIntegralDetailEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //初始化固定字段
			pEntity.IsDelete=0;
			pEntity.CreateTime=DateTime.Now;
			pEntity.LastUpdateTime=pEntity.CreateTime;
			pEntity.CreateBy=CurrentUserInfo.UserID;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [VipIntegralDetail](");
            strSql.Append("[VIPID],[VipCardCode],[UnitID],[UnitName],[SalesAmount],[Integral],[UsedIntegral],[Reason],[IntegralSourceID],[EffectiveDate],[DeadlineDate],[IsAdd],[FromVipID],[ObjectId],[Remark],[CustomerID],[CreateBy],[CreateTime],[LastUpdateTime],[LastUpdateBy],[IsDelete],[VipIntegralDetailID])");
            strSql.Append(" values (");
            strSql.Append("@VIPID,@VipCardCode,@UnitID,@UnitName,@SalesAmount,@Integral,@UsedIntegral,@Reason,@IntegralSourceID,@EffectiveDate,@DeadlineDate,@IsAdd,@FromVipID,@ObjectId,@Remark,@CustomerID,@CreateBy,@CreateTime,@LastUpdateTime,@LastUpdateBy,@IsDelete,@VipIntegralDetailID)");            

			string pkString = pEntity.VipIntegralDetailID;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@VIPID",SqlDbType.VarChar),
					new SqlParameter("@VipCardCode",SqlDbType.VarChar),
					new SqlParameter("@UnitID",SqlDbType.NVarChar),
					new SqlParameter("@UnitName",SqlDbType.NVarChar),
					new SqlParameter("@SalesAmount",SqlDbType.Decimal),
					new SqlParameter("@Integral",SqlDbType.Int),
					new SqlParameter("@UsedIntegral",SqlDbType.Int),
					new SqlParameter("@Reason",SqlDbType.NVarChar),
					new SqlParameter("@IntegralSourceID",SqlDbType.NVarChar),
					new SqlParameter("@EffectiveDate",SqlDbType.DateTime),
					new SqlParameter("@DeadlineDate",SqlDbType.DateTime),
					new SqlParameter("@IsAdd",SqlDbType.Int),
					new SqlParameter("@FromVipID",SqlDbType.NVarChar),
					new SqlParameter("@ObjectId",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@CustomerID",SqlDbType.VarChar),
					new SqlParameter("@CreateBy",SqlDbType.VarChar),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@VipIntegralDetailID",SqlDbType.VarChar)
            };
			parameters[0].Value = pEntity.VIPID;
			parameters[1].Value = pEntity.VipCardCode;
			parameters[2].Value = pEntity.UnitID;
			parameters[3].Value = pEntity.UnitName;
			parameters[4].Value = pEntity.SalesAmount;
			parameters[5].Value = pEntity.Integral;
			parameters[6].Value = pEntity.UsedIntegral;
			parameters[7].Value = pEntity.Reason;
			parameters[8].Value = pEntity.IntegralSourceID;
			parameters[9].Value = pEntity.EffectiveDate;
			parameters[10].Value = pEntity.DeadlineDate;
			parameters[11].Value = pEntity.IsAdd;
			parameters[12].Value = pEntity.FromVipID;
			parameters[13].Value = pEntity.ObjectId;
			parameters[14].Value = pEntity.Remark;
			parameters[15].Value = pEntity.CustomerID;
			parameters[16].Value = pEntity.CreateBy;
			parameters[17].Value = pEntity.CreateTime;
			parameters[18].Value = pEntity.LastUpdateTime;
			parameters[19].Value = pEntity.LastUpdateBy;
			parameters[20].Value = pEntity.IsDelete;
			parameters[21].Value = pkString;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.VipIntegralDetailID = pkString;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public VipIntegralDetailEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipIntegralDetail] where VipIntegralDetailID='{0}'  and isdelete=0 ", id.ToString());
            //读取数据
            VipIntegralDetailEntity m = null;
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    this.Load(rdr, out m);
                    break;
                }
            }
            //返回
            return m;
        }

        /// <summary>
        /// 获取所有实例
        /// </summary>
        /// <returns></returns>
        public VipIntegralDetailEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipIntegralDetail] where 1=1  and isdelete=0");
            //读取数据
            List<VipIntegralDetailEntity> list = new List<VipIntegralDetailEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    VipIntegralDetailEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            //返回
            return list.ToArray();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(VipIntegralDetailEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(VipIntegralDetailEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.VipIntegralDetailID == null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
			pEntity.LastUpdateTime=DateTime.Now;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [VipIntegralDetail] set ");
                        if (pIsUpdateNullField || pEntity.VIPID!=null)
                strSql.Append( "[VIPID]=@VIPID,");
            if (pIsUpdateNullField || pEntity.VipCardCode!=null)
                strSql.Append( "[VipCardCode]=@VipCardCode,");
            if (pIsUpdateNullField || pEntity.UnitID!=null)
                strSql.Append( "[UnitID]=@UnitID,");
            if (pIsUpdateNullField || pEntity.UnitName!=null)
                strSql.Append( "[UnitName]=@UnitName,");
            if (pIsUpdateNullField || pEntity.SalesAmount!=null)
                strSql.Append( "[SalesAmount]=@SalesAmount,");
            if (pIsUpdateNullField || pEntity.Integral!=null)
                strSql.Append( "[Integral]=@Integral,");
            if (pIsUpdateNullField || pEntity.UsedIntegral!=null)
                strSql.Append( "[UsedIntegral]=@UsedIntegral,");
            if (pIsUpdateNullField || pEntity.Reason!=null)
                strSql.Append( "[Reason]=@Reason,");
            if (pIsUpdateNullField || pEntity.IntegralSourceID!=null)
                strSql.Append( "[IntegralSourceID]=@IntegralSourceID,");
            if (pIsUpdateNullField || pEntity.EffectiveDate!=null)
                strSql.Append( "[EffectiveDate]=@EffectiveDate,");
            if (pIsUpdateNullField || pEntity.DeadlineDate!=null)
                strSql.Append( "[DeadlineDate]=@DeadlineDate,");
            if (pIsUpdateNullField || pEntity.IsAdd!=null)
                strSql.Append( "[IsAdd]=@IsAdd,");
            if (pIsUpdateNullField || pEntity.FromVipID!=null)
                strSql.Append( "[FromVipID]=@FromVipID,");
            if (pIsUpdateNullField || pEntity.ObjectId!=null)
                strSql.Append( "[ObjectId]=@ObjectId,");
            if (pIsUpdateNullField || pEntity.Remark!=null)
                strSql.Append( "[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.CustomerID!=null)
                strSql.Append( "[CustomerID]=@CustomerID,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy");
            strSql.Append(" where VipIntegralDetailID=@VipIntegralDetailID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@VIPID",SqlDbType.VarChar),
					new SqlParameter("@VipCardCode",SqlDbType.VarChar),
					new SqlParameter("@UnitID",SqlDbType.NVarChar),
					new SqlParameter("@UnitName",SqlDbType.NVarChar),
					new SqlParameter("@SalesAmount",SqlDbType.Decimal),
					new SqlParameter("@Integral",SqlDbType.Int),
					new SqlParameter("@UsedIntegral",SqlDbType.Int),
					new SqlParameter("@Reason",SqlDbType.NVarChar),
					new SqlParameter("@IntegralSourceID",SqlDbType.NVarChar),
					new SqlParameter("@EffectiveDate",SqlDbType.DateTime),
					new SqlParameter("@DeadlineDate",SqlDbType.DateTime),
					new SqlParameter("@IsAdd",SqlDbType.Int),
					new SqlParameter("@FromVipID",SqlDbType.NVarChar),
					new SqlParameter("@ObjectId",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@CustomerID",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@VipIntegralDetailID",SqlDbType.VarChar)
            };
			parameters[0].Value = pEntity.VIPID;
			parameters[1].Value = pEntity.VipCardCode;
			parameters[2].Value = pEntity.UnitID;
			parameters[3].Value = pEntity.UnitName;
			parameters[4].Value = pEntity.SalesAmount;
			parameters[5].Value = pEntity.Integral;
			parameters[6].Value = pEntity.UsedIntegral;
			parameters[7].Value = pEntity.Reason;
			parameters[8].Value = pEntity.IntegralSourceID;
			parameters[9].Value = pEntity.EffectiveDate;
			parameters[10].Value = pEntity.DeadlineDate;
			parameters[11].Value = pEntity.IsAdd;
			parameters[12].Value = pEntity.FromVipID;
			parameters[13].Value = pEntity.ObjectId;
			parameters[14].Value = pEntity.Remark;
			parameters[15].Value = pEntity.CustomerID;
			parameters[16].Value = pEntity.LastUpdateTime;
			parameters[17].Value = pEntity.LastUpdateBy;
			parameters[18].Value = pEntity.VipIntegralDetailID;

            //执行语句
            int result = 0;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Update(VipIntegralDetailEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(VipIntegralDetailEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(VipIntegralDetailEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.VipIntegralDetailID == null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.VipIntegralDetailID, pTran);           
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pID">标识符的值</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(object pID, IDbTransaction pTran)
        {
            if (pID == null)
                return ;   
            //组织参数化SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [VipIntegralDetail] set  isdelete=1 where VipIntegralDetailID=@VipIntegralDetailID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@VipIntegralDetailID",SqlDbType=SqlDbType.VarChar,Value=pID}
            };
            //执行语句
            int result = 0;
            if (pTran != null)
                result=this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, sql.ToString(), parameters);
            else
                result=this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), parameters);
            return ;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(VipIntegralDetailEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (pEntity.VipIntegralDetailID == null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.VipIntegralDetailID;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(VipIntegralDetailEntity[] pEntities)
        { 
            Delete(pEntities, null);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pIDs">标识符值数组</param>
        public void Delete(object[] pIDs)
        {
            Delete(pIDs,null);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pIDs">标识符值数组</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(object[] pIDs, IDbTransaction pTran) 
        {
            if (pIDs == null || pIDs.Length==0)
                return ;
            //组织参数化SQL
            StringBuilder primaryKeys = new StringBuilder();
            foreach (object item in pIDs)
            {
                primaryKeys.AppendFormat("'{0}',",item.ToString());
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [VipIntegralDetail] set  isdelete=1 where VipIntegralDetailID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
            //执行语句
            int result = 0;   
            if (pTran == null)
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), null);
            else
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran,CommandType.Text, sql.ToString());       
        }
        #endregion

        #region IQueryable 成员
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="pWhereConditions">筛选条件</param>
        /// <param name="pOrderBys">排序</param>
        /// <returns></returns>
        public VipIntegralDetailEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipIntegralDetail] where 1=1  and isdelete=0 ");
            if (pWhereConditions != null)
            {
                foreach (var item in pWhereConditions)
                {
                    sql.AppendFormat(" and {0}", item.GetExpression());
                }
            }
            if (pOrderBys != null && pOrderBys.Length > 0)
            {
                sql.AppendFormat(" order by ");
                foreach (var item in pOrderBys)
                {
                    sql.AppendFormat(" {0} {1},", item.FieldName, item.Direction == OrderByDirections.Asc ? "asc" : "desc");
                }
                sql.Remove(sql.Length - 1, 1);
            }
            //执行SQL
            List<VipIntegralDetailEntity> list = new List<VipIntegralDetailEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    VipIntegralDetailEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            //返回结果
            return list.ToArray();
        }
        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="pWhereConditions">筛选条件</param>
        /// <param name="pOrderBys">排序</param>
        /// <param name="pPageSize">每页的记录数</param>
        /// <param name="pCurrentPageIndex">以0开始的当前页码</param>
        /// <returns></returns>
        public PagedQueryResult<VipIntegralDetailEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            //组织SQL
            StringBuilder pagedSql = new StringBuilder();
            StringBuilder totalCountSql = new StringBuilder();
            //分页SQL
            pagedSql.AppendFormat("select * from (select row_number()over( order by ");
            if (pOrderBys != null && pOrderBys.Length > 0)
            {
                foreach (var item in pOrderBys)
                {
                    if(item!=null)
                    {
                        pagedSql.AppendFormat(" {0} {1},", StringUtils.WrapperSQLServerObject(item.FieldName), item.Direction == OrderByDirections.Asc ? "asc" : "desc");
                    }
                }
                pagedSql.Remove(pagedSql.Length - 1, 1);
            }
            else
            {
                pagedSql.AppendFormat(" [VipIntegralDetailID] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [VipIntegralDetail] where 1=1  and isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [VipIntegralDetail] where 1=1  and isdelete=0 ");
            //过滤条件
            if (pWhereConditions != null)
            {
                foreach (var item in pWhereConditions)
                {
                    if(item!=null)
                    {
                        pagedSql.AppendFormat(" and {0}", item.GetExpression());
                        totalCountSql.AppendFormat(" and {0}", item.GetExpression());
                    }
                }
            }
            pagedSql.AppendFormat(") as A ");
            //取指定页的数据
            pagedSql.AppendFormat(" where ___rn >{0} and ___rn <={1}", pPageSize * (pCurrentPageIndex-1), pPageSize * (pCurrentPageIndex));
            //执行语句并返回结果
            PagedQueryResult<VipIntegralDetailEntity> result = new PagedQueryResult<VipIntegralDetailEntity>();
            List<VipIntegralDetailEntity> list = new List<VipIntegralDetailEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    VipIntegralDetailEntity m;
                    this.Load(rdr, out m);
                    list.Add(m);
                }
            }
            result.Entities = list.ToArray();
            int totalCount = Convert.ToInt32(this.SQLHelper.ExecuteScalar(totalCountSql.ToString()));    //计算总行数
            result.RowCount = totalCount;
            int remainder = 0;
            result.PageCount = Math.DivRem(totalCount, pPageSize, out remainder);
            if (remainder > 0)
                result.PageCount++;
            return result;
        }

        /// <summary>
        /// 根据实体条件查询实体
        /// </summary>
        /// <param name="pQueryEntity">以实体形式传入的参数</param>
        /// <param name="pOrderBys">排序组合</param>
        /// <returns>符合条件的实体集</returns>
        public VipIntegralDetailEntity[] QueryByEntity(VipIntegralDetailEntity pQueryEntity, OrderBy[] pOrderBys)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity(pQueryEntity);
            return Query(queryWhereCondition,  pOrderBys);            
        }

        /// <summary>
        /// 分页根据实体条件查询实体
        /// </summary>
        /// <param name="pQueryEntity">以实体形式传入的参数</param>
        /// <param name="pOrderBys">排序组合</param>
        /// <returns>符合条件的实体集</returns>
        public PagedQueryResult<VipIntegralDetailEntity> PagedQueryByEntity(VipIntegralDetailEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity( pQueryEntity);
            return PagedQuery(queryWhereCondition, pOrderBys, pPageSize, pCurrentPageIndex);
        }

        #endregion

        #region 工具方法
        /// <summary>
        /// 根据实体非Null属性生成查询条件。
        /// </summary>
        /// <returns></returns>
        protected IWhereCondition[] GetWhereConditionByEntity(VipIntegralDetailEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.VipIntegralDetailID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VipIntegralDetailID", Value = pQueryEntity.VipIntegralDetailID });
            if (pQueryEntity.VIPID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VIPID", Value = pQueryEntity.VIPID });
            if (pQueryEntity.VipCardCode!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VipCardCode", Value = pQueryEntity.VipCardCode });
            if (pQueryEntity.UnitID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "UnitID", Value = pQueryEntity.UnitID });
            if (pQueryEntity.UnitName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "UnitName", Value = pQueryEntity.UnitName });
            if (pQueryEntity.SalesAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SalesAmount", Value = pQueryEntity.SalesAmount });
            if (pQueryEntity.Integral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Integral", Value = pQueryEntity.Integral });
            if (pQueryEntity.UsedIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "UsedIntegral", Value = pQueryEntity.UsedIntegral });
            if (pQueryEntity.Reason!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Reason", Value = pQueryEntity.Reason });
            if (pQueryEntity.IntegralSourceID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IntegralSourceID", Value = pQueryEntity.IntegralSourceID });
            if (pQueryEntity.EffectiveDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EffectiveDate", Value = pQueryEntity.EffectiveDate });
            if (pQueryEntity.DeadlineDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DeadlineDate", Value = pQueryEntity.DeadlineDate });
            if (pQueryEntity.IsAdd!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsAdd", Value = pQueryEntity.IsAdd });
            if (pQueryEntity.FromVipID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "FromVipID", Value = pQueryEntity.FromVipID });
            if (pQueryEntity.ObjectId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ObjectId", Value = pQueryEntity.ObjectId });
            if (pQueryEntity.Remark!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
            if (pQueryEntity.CustomerID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerID", Value = pQueryEntity.CustomerID });
            if (pQueryEntity.CreateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateBy", Value = pQueryEntity.CreateBy });
            if (pQueryEntity.CreateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateTime", Value = pQueryEntity.CreateTime });
            if (pQueryEntity.LastUpdateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateTime", Value = pQueryEntity.LastUpdateTime });
            if (pQueryEntity.LastUpdateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateBy", Value = pQueryEntity.LastUpdateBy });
            if (pQueryEntity.IsDelete!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsDelete", Value = pQueryEntity.IsDelete });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(IDataReader pReader, out VipIntegralDetailEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new VipIntegralDetailEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["VipIntegralDetailID"] != DBNull.Value)
			{
				pInstance.VipIntegralDetailID =  Convert.ToString(pReader["VipIntegralDetailID"]);
			}
			if (pReader["VIPID"] != DBNull.Value)
			{
				pInstance.VIPID =  Convert.ToString(pReader["VIPID"]);
			}
			if (pReader["VipCardCode"] != DBNull.Value)
			{
				pInstance.VipCardCode =  Convert.ToString(pReader["VipCardCode"]);
			}
			if (pReader["UnitID"] != DBNull.Value)
			{
				pInstance.UnitID =  Convert.ToString(pReader["UnitID"]);
			}
			if (pReader["UnitName"] != DBNull.Value)
			{
				pInstance.UnitName =  Convert.ToString(pReader["UnitName"]);
			}
			if (pReader["SalesAmount"] != DBNull.Value)
			{
				pInstance.SalesAmount =  Convert.ToDecimal(pReader["SalesAmount"]);
			}
			if (pReader["Integral"] != DBNull.Value)
			{
				pInstance.Integral =   Convert.ToInt32(pReader["Integral"]);
			}
			if (pReader["UsedIntegral"] != DBNull.Value)
			{
				pInstance.UsedIntegral =   Convert.ToInt32(pReader["UsedIntegral"]);
			}
			if (pReader["Reason"] != DBNull.Value)
			{
				pInstance.Reason =  Convert.ToString(pReader["Reason"]);
			}
			if (pReader["IntegralSourceID"] != DBNull.Value)
			{
				pInstance.IntegralSourceID =  Convert.ToString(pReader["IntegralSourceID"]);
			}
			if (pReader["EffectiveDate"] != DBNull.Value)
			{
				pInstance.EffectiveDate =  Convert.ToDateTime(pReader["EffectiveDate"]);
			}
			if (pReader["DeadlineDate"] != DBNull.Value)
			{
				pInstance.DeadlineDate =  Convert.ToDateTime(pReader["DeadlineDate"]);
			}
			if (pReader["IsAdd"] != DBNull.Value)
			{
				pInstance.IsAdd =   Convert.ToInt32(pReader["IsAdd"]);
			}
			if (pReader["FromVipID"] != DBNull.Value)
			{
				pInstance.FromVipID =  Convert.ToString(pReader["FromVipID"]);
			}
			if (pReader["ObjectId"] != DBNull.Value)
			{
				pInstance.ObjectId =  Convert.ToString(pReader["ObjectId"]);
			}
			if (pReader["Remark"] != DBNull.Value)
			{
				pInstance.Remark =  Convert.ToString(pReader["Remark"]);
			}
			if (pReader["CustomerID"] != DBNull.Value)
			{
				pInstance.CustomerID =  Convert.ToString(pReader["CustomerID"]);
			}
			if (pReader["CreateBy"] != DBNull.Value)
			{
				pInstance.CreateBy =  Convert.ToString(pReader["CreateBy"]);
			}
			if (pReader["CreateTime"] != DBNull.Value)
			{
				pInstance.CreateTime =  Convert.ToDateTime(pReader["CreateTime"]);
			}
			if (pReader["LastUpdateTime"] != DBNull.Value)
			{
				pInstance.LastUpdateTime =  Convert.ToDateTime(pReader["LastUpdateTime"]);
			}
			if (pReader["LastUpdateBy"] != DBNull.Value)
			{
				pInstance.LastUpdateBy =  Convert.ToString(pReader["LastUpdateBy"]);
			}
			if (pReader["IsDelete"] != DBNull.Value)
			{
				pInstance.IsDelete =   Convert.ToInt32(pReader["IsDelete"]);
			}

        }
        #endregion
    }
}
