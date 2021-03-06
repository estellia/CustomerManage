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
    /// 表VipAmount的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class VipAmountDAO : Base.BaseCPOSDAO, ICRUDable<VipAmountEntity>, IQueryable<VipAmountEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public VipAmountDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(VipAmountEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(VipAmountEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [VipAmount](");
            strSql.Append("[VipCardCode],[BeginAmount],[InAmount],[OutAmount],[EndAmount],[TotalAmount],[BeginReturnAmount],[InReturnAmount],[OutReturnAmount],[ReturnAmount],[ImminentInvalidRAmount],[InvalidReturnAmount],[ValidReturnAmount],[TotalReturnAmount],[PayPassword],[IsLocking],[CustomerID],[CreateTime],[CreateBy],[LastUpdateTime],[LastUpdateBy],[IsDelete],[VipId])");
            strSql.Append(" values (");
            strSql.Append("@VipCardCode,@BeginAmount,@InAmount,@OutAmount,@EndAmount,@TotalAmount,@BeginReturnAmount,@InReturnAmount,@OutReturnAmount,@ReturnAmount,@ImminentInvalidRAmount,@InvalidReturnAmount,@ValidReturnAmount,@TotalReturnAmount,@PayPassword,@IsLocking,@CustomerID,@CreateTime,@CreateBy,@LastUpdateTime,@LastUpdateBy,@IsDelete,@VipId)");            

			string pkString = pEntity.VipId;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@VipCardCode",SqlDbType.VarChar),
					new SqlParameter("@BeginAmount",SqlDbType.Decimal),
					new SqlParameter("@InAmount",SqlDbType.Decimal),
					new SqlParameter("@OutAmount",SqlDbType.Decimal),
					new SqlParameter("@EndAmount",SqlDbType.Decimal),
					new SqlParameter("@TotalAmount",SqlDbType.Decimal),
					new SqlParameter("@BeginReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@InReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@OutReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@ReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@ImminentInvalidRAmount",SqlDbType.Decimal),
					new SqlParameter("@InvalidReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@ValidReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@TotalReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@PayPassword",SqlDbType.NVarChar),
					new SqlParameter("@IsLocking",SqlDbType.Int),
					new SqlParameter("@CustomerID",SqlDbType.VarChar),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@CreateBy",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@VipId",SqlDbType.NVarChar)
            };
			parameters[0].Value = pEntity.VipCardCode;
			parameters[1].Value = pEntity.BeginAmount;
			parameters[2].Value = pEntity.InAmount;
			parameters[3].Value = pEntity.OutAmount;
			parameters[4].Value = pEntity.EndAmount;
			parameters[5].Value = pEntity.TotalAmount;
			parameters[6].Value = pEntity.BeginReturnAmount;
			parameters[7].Value = pEntity.InReturnAmount;
			parameters[8].Value = pEntity.OutReturnAmount;
			parameters[9].Value = pEntity.ReturnAmount;
			parameters[10].Value = pEntity.ImminentInvalidRAmount;
			parameters[11].Value = pEntity.InvalidReturnAmount;
			parameters[12].Value = pEntity.ValidReturnAmount;
			parameters[13].Value = pEntity.TotalReturnAmount;
			parameters[14].Value = pEntity.PayPassword;
			parameters[15].Value = pEntity.IsLocking;
			parameters[16].Value = pEntity.CustomerID;
			parameters[17].Value = pEntity.CreateTime;
			parameters[18].Value = pEntity.CreateBy;
			parameters[19].Value = pEntity.LastUpdateTime;
			parameters[20].Value = pEntity.LastUpdateBy;
			parameters[21].Value = pEntity.IsDelete;
			parameters[22].Value = pkString;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.VipId = pkString;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public VipAmountEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipAmount] where VipId='{0}'  and isdelete=0 ", id.ToString());
            //读取数据
            VipAmountEntity m = null;
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
        public VipAmountEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipAmount] where 1=1  and isdelete=0");
            //读取数据
            List<VipAmountEntity> list = new List<VipAmountEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    VipAmountEntity m;
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
        public void Update(VipAmountEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(VipAmountEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.VipId == null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
			pEntity.LastUpdateTime=DateTime.Now;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [VipAmount] set ");
                        if (pIsUpdateNullField || pEntity.VipCardCode!=null)
                strSql.Append( "[VipCardCode]=@VipCardCode,");
            if (pIsUpdateNullField || pEntity.BeginAmount!=null)
                strSql.Append( "[BeginAmount]=@BeginAmount,");
            if (pIsUpdateNullField || pEntity.InAmount!=null)
                strSql.Append( "[InAmount]=@InAmount,");
            if (pIsUpdateNullField || pEntity.OutAmount!=null)
                strSql.Append( "[OutAmount]=@OutAmount,");
            if (pIsUpdateNullField || pEntity.EndAmount!=null)
                strSql.Append( "[EndAmount]=@EndAmount,");
            if (pIsUpdateNullField || pEntity.TotalAmount!=null)
                strSql.Append( "[TotalAmount]=@TotalAmount,");
            if (pIsUpdateNullField || pEntity.BeginReturnAmount!=null)
                strSql.Append( "[BeginReturnAmount]=@BeginReturnAmount,");
            if (pIsUpdateNullField || pEntity.InReturnAmount!=null)
                strSql.Append( "[InReturnAmount]=@InReturnAmount,");
            if (pIsUpdateNullField || pEntity.OutReturnAmount!=null)
                strSql.Append( "[OutReturnAmount]=@OutReturnAmount,");
            if (pIsUpdateNullField || pEntity.ReturnAmount!=null)
                strSql.Append( "[ReturnAmount]=@ReturnAmount,");
            if (pIsUpdateNullField || pEntity.ImminentInvalidRAmount!=null)
                strSql.Append( "[ImminentInvalidRAmount]=@ImminentInvalidRAmount,");
            if (pIsUpdateNullField || pEntity.InvalidReturnAmount!=null)
                strSql.Append( "[InvalidReturnAmount]=@InvalidReturnAmount,");
            if (pIsUpdateNullField || pEntity.ValidReturnAmount!=null)
                strSql.Append( "[ValidReturnAmount]=@ValidReturnAmount,");
            if (pIsUpdateNullField || pEntity.TotalReturnAmount!=null)
                strSql.Append( "[TotalReturnAmount]=@TotalReturnAmount,");
            if (pIsUpdateNullField || pEntity.PayPassword!=null)
                strSql.Append( "[PayPassword]=@PayPassword,");
            if (pIsUpdateNullField || pEntity.IsLocking!=null)
                strSql.Append( "[IsLocking]=@IsLocking,");
            if (pIsUpdateNullField || pEntity.CustomerID!=null)
                strSql.Append( "[CustomerID]=@CustomerID,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy");
            strSql.Append(" where VipId=@VipId and VipCardCode=@VipCardCode2");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@VipCardCode",SqlDbType.VarChar),
					new SqlParameter("@BeginAmount",SqlDbType.Decimal),
					new SqlParameter("@InAmount",SqlDbType.Decimal),
					new SqlParameter("@OutAmount",SqlDbType.Decimal),
					new SqlParameter("@EndAmount",SqlDbType.Decimal),
					new SqlParameter("@TotalAmount",SqlDbType.Decimal),
					new SqlParameter("@BeginReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@InReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@OutReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@ReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@ImminentInvalidRAmount",SqlDbType.Decimal),
					new SqlParameter("@InvalidReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@ValidReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@TotalReturnAmount",SqlDbType.Decimal),
					new SqlParameter("@PayPassword",SqlDbType.NVarChar),
					new SqlParameter("@IsLocking",SqlDbType.Int),
					new SqlParameter("@CustomerID",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@VipId",SqlDbType.NVarChar),
                    new SqlParameter("@VipCardCode2",SqlDbType.NVarChar)

            };
			parameters[0].Value = pEntity.VipCardCode;
			parameters[1].Value = pEntity.BeginAmount;
			parameters[2].Value = pEntity.InAmount;
			parameters[3].Value = pEntity.OutAmount;
			parameters[4].Value = pEntity.EndAmount;
			parameters[5].Value = pEntity.TotalAmount;
			parameters[6].Value = pEntity.BeginReturnAmount;
			parameters[7].Value = pEntity.InReturnAmount;
			parameters[8].Value = pEntity.OutReturnAmount;
			parameters[9].Value = pEntity.ReturnAmount;
			parameters[10].Value = pEntity.ImminentInvalidRAmount;
			parameters[11].Value = pEntity.InvalidReturnAmount;
			parameters[12].Value = pEntity.ValidReturnAmount;
			parameters[13].Value = pEntity.TotalReturnAmount;
			parameters[14].Value = pEntity.PayPassword;
			parameters[15].Value = pEntity.IsLocking;
			parameters[16].Value = pEntity.CustomerID;
			parameters[17].Value = pEntity.LastUpdateTime;
			parameters[18].Value = pEntity.LastUpdateBy;
			parameters[19].Value = pEntity.VipId;
            parameters[20].Value = pEntity.VipCardCode;


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
        public void Update(VipAmountEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(VipAmountEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(VipAmountEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.VipId == null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.VipId, pTran);           
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
            sql.AppendLine("update [VipAmount] set  isdelete=1 where VipId=@VipId;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@VipId",SqlDbType=SqlDbType.VarChar,Value=pID}
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
        public void Delete(VipAmountEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (pEntity.VipId == null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.VipId;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(VipAmountEntity[] pEntities)
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
            sql.AppendLine("update [VipAmount] set  isdelete=1 where VipId in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public VipAmountEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipAmount] where 1=1  and isdelete=0 ");
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
            List<VipAmountEntity> list = new List<VipAmountEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    VipAmountEntity m;
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
        public PagedQueryResult<VipAmountEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [VipId] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [VipAmount] where 1=1  and isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [VipAmount] where 1=1  and isdelete=0 ");
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
            PagedQueryResult<VipAmountEntity> result = new PagedQueryResult<VipAmountEntity>();
            List<VipAmountEntity> list = new List<VipAmountEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    VipAmountEntity m;
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
        public VipAmountEntity[] QueryByEntity(VipAmountEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<VipAmountEntity> PagedQueryByEntity(VipAmountEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(VipAmountEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.VipId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VipId", Value = pQueryEntity.VipId });
            if (pQueryEntity.VipCardCode!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VipCardCode", Value = pQueryEntity.VipCardCode });
            if (pQueryEntity.BeginAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BeginAmount", Value = pQueryEntity.BeginAmount });
            if (pQueryEntity.InAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "InAmount", Value = pQueryEntity.InAmount });
            if (pQueryEntity.OutAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "OutAmount", Value = pQueryEntity.OutAmount });
            if (pQueryEntity.EndAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EndAmount", Value = pQueryEntity.EndAmount });
            if (pQueryEntity.TotalAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "TotalAmount", Value = pQueryEntity.TotalAmount });
            if (pQueryEntity.BeginReturnAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BeginReturnAmount", Value = pQueryEntity.BeginReturnAmount });
            if (pQueryEntity.InReturnAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "InReturnAmount", Value = pQueryEntity.InReturnAmount });
            if (pQueryEntity.OutReturnAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "OutReturnAmount", Value = pQueryEntity.OutReturnAmount });
            if (pQueryEntity.ReturnAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ReturnAmount", Value = pQueryEntity.ReturnAmount });
            if (pQueryEntity.ImminentInvalidRAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ImminentInvalidRAmount", Value = pQueryEntity.ImminentInvalidRAmount });
            if (pQueryEntity.InvalidReturnAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "InvalidReturnAmount", Value = pQueryEntity.InvalidReturnAmount });
            if (pQueryEntity.ValidReturnAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ValidReturnAmount", Value = pQueryEntity.ValidReturnAmount });
            if (pQueryEntity.TotalReturnAmount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "TotalReturnAmount", Value = pQueryEntity.TotalReturnAmount });
            if (pQueryEntity.PayPassword!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PayPassword", Value = pQueryEntity.PayPassword });
            if (pQueryEntity.IsLocking!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsLocking", Value = pQueryEntity.IsLocking });
            if (pQueryEntity.CustomerID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerID", Value = pQueryEntity.CustomerID });
            if (pQueryEntity.CreateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateTime", Value = pQueryEntity.CreateTime });
            if (pQueryEntity.CreateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateBy", Value = pQueryEntity.CreateBy });
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
        protected void Load(IDataReader pReader, out VipAmountEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new VipAmountEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["VipId"] != DBNull.Value)
			{
				pInstance.VipId =  Convert.ToString(pReader["VipId"]);
			}
			if (pReader["VipCardCode"] != DBNull.Value)
			{
				pInstance.VipCardCode =  Convert.ToString(pReader["VipCardCode"]);
			}
			if (pReader["BeginAmount"] != DBNull.Value)
			{
				pInstance.BeginAmount =  Convert.ToDecimal(pReader["BeginAmount"]);
			}
			if (pReader["InAmount"] != DBNull.Value)
			{
				pInstance.InAmount =  Convert.ToDecimal(pReader["InAmount"]);
			}
			if (pReader["OutAmount"] != DBNull.Value)
			{
				pInstance.OutAmount =  Convert.ToDecimal(pReader["OutAmount"]);
			}
			if (pReader["EndAmount"] != DBNull.Value)
			{
				pInstance.EndAmount =  Convert.ToDecimal(pReader["EndAmount"]);
			}
			if (pReader["TotalAmount"] != DBNull.Value)
			{
				pInstance.TotalAmount =  Convert.ToDecimal(pReader["TotalAmount"]);
			}
			if (pReader["BeginReturnAmount"] != DBNull.Value)
			{
				pInstance.BeginReturnAmount =  Convert.ToDecimal(pReader["BeginReturnAmount"]);
			}
			if (pReader["InReturnAmount"] != DBNull.Value)
			{
				pInstance.InReturnAmount =  Convert.ToDecimal(pReader["InReturnAmount"]);
			}
			if (pReader["OutReturnAmount"] != DBNull.Value)
			{
				pInstance.OutReturnAmount =  Convert.ToDecimal(pReader["OutReturnAmount"]);
			}
			if (pReader["ReturnAmount"] != DBNull.Value)
			{
				pInstance.ReturnAmount =  Convert.ToDecimal(pReader["ReturnAmount"]);
			}
			if (pReader["ImminentInvalidRAmount"] != DBNull.Value)
			{
				pInstance.ImminentInvalidRAmount =  Convert.ToDecimal(pReader["ImminentInvalidRAmount"]);
			}
			if (pReader["InvalidReturnAmount"] != DBNull.Value)
			{
				pInstance.InvalidReturnAmount =  Convert.ToDecimal(pReader["InvalidReturnAmount"]);
			}
			if (pReader["ValidReturnAmount"] != DBNull.Value)
			{
				pInstance.ValidReturnAmount =  Convert.ToDecimal(pReader["ValidReturnAmount"]);
			}
			if (pReader["TotalReturnAmount"] != DBNull.Value)
			{
				pInstance.TotalReturnAmount =  Convert.ToDecimal(pReader["TotalReturnAmount"]);
			}
			if (pReader["PayPassword"] != DBNull.Value)
			{
				pInstance.PayPassword =  Convert.ToString(pReader["PayPassword"]);
			}
			if (pReader["IsLocking"] != DBNull.Value)
			{
				pInstance.IsLocking =   Convert.ToInt32(pReader["IsLocking"]);
			}
			if (pReader["CustomerID"] != DBNull.Value)
			{
				pInstance.CustomerID =  Convert.ToString(pReader["CustomerID"]);
			}
			if (pReader["CreateTime"] != DBNull.Value)
			{
				pInstance.CreateTime =  Convert.ToDateTime(pReader["CreateTime"]);
			}
			if (pReader["CreateBy"] != DBNull.Value)
			{
				pInstance.CreateBy =  Convert.ToString(pReader["CreateBy"]);
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
