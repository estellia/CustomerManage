/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/2/16 14:37:46
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
    /// 表PanicbuyingKJEventJoin的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class PanicbuyingKJEventJoinDAO : Base.BaseCPOSDAO, ICRUDable<PanicbuyingKJEventJoinEntity>, IQueryable<PanicbuyingKJEventJoinEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public PanicbuyingKJEventJoinDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(PanicbuyingKJEventJoinEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(PanicbuyingKJEventJoinEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [PanicbuyingKJEventJoin](");
            strSql.Append("[EventId],[ItemId],[SkuId],[VipId],[SalesPrice],[BargainPersonCount],[EventOrderMappingId],[CreateTime],[CreateBy],[LastUpdateBy],[LastUpdateTime],[IsDelete],[CustomerId],[KJEventJoinId])");
            strSql.Append(" values (");
            strSql.Append("@EventId,@ItemId,@SkuId,@VipId,@SalesPrice,@BargainPersonCount,@EventOrderMappingId,@CreateTime,@CreateBy,@LastUpdateBy,@LastUpdateTime,@IsDelete,@CustomerId,@KJEventJoinId)");            

			Guid? pkGuid;
			if (pEntity.KJEventJoinId == null)
				pkGuid = Guid.NewGuid();
			else
				pkGuid = pEntity.KJEventJoinId;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@EventId",SqlDbType.UniqueIdentifier),
					new SqlParameter("@ItemId",SqlDbType.NVarChar),
					new SqlParameter("@SkuId",SqlDbType.NVarChar),
					new SqlParameter("@VipId",SqlDbType.NVarChar),
					new SqlParameter("@SalesPrice",SqlDbType.Decimal),
					new SqlParameter("@BargainPersonCount",SqlDbType.Int),
					new SqlParameter("@EventOrderMappingId",SqlDbType.UniqueIdentifier),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@CreateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@CustomerId",SqlDbType.NVarChar),
					new SqlParameter("@KJEventJoinId",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.EventId;
			parameters[1].Value = pEntity.ItemId;
			parameters[2].Value = pEntity.SkuId;
			parameters[3].Value = pEntity.VipId;
			parameters[4].Value = pEntity.SalesPrice;
			parameters[5].Value = pEntity.BargainPersonCount;
			parameters[6].Value = pEntity.EventOrderMappingId;
			parameters[7].Value = pEntity.CreateTime;
			parameters[8].Value = pEntity.CreateBy;
			parameters[9].Value = pEntity.LastUpdateBy;
			parameters[10].Value = pEntity.LastUpdateTime;
			parameters[11].Value = pEntity.IsDelete;
			parameters[12].Value = pEntity.CustomerId;
			parameters[13].Value = pkGuid;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.KJEventJoinId = pkGuid;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public PanicbuyingKJEventJoinEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [PanicbuyingKJEventJoin] where KJEventJoinId='{0}'  and isdelete=0 ", id.ToString());
            //读取数据
            PanicbuyingKJEventJoinEntity m = null;
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
        public PanicbuyingKJEventJoinEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [PanicbuyingKJEventJoin] where 1=1  and isdelete=0");
            //读取数据
            List<PanicbuyingKJEventJoinEntity> list = new List<PanicbuyingKJEventJoinEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    PanicbuyingKJEventJoinEntity m;
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
        public void Update(PanicbuyingKJEventJoinEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(PanicbuyingKJEventJoinEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.KJEventJoinId.HasValue)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
			pEntity.LastUpdateTime=DateTime.Now;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PanicbuyingKJEventJoin] set ");
                        if (pIsUpdateNullField || pEntity.EventId!=null)
                strSql.Append( "[EventId]=@EventId,");
            if (pIsUpdateNullField || pEntity.ItemId!=null)
                strSql.Append( "[ItemId]=@ItemId,");
            if (pIsUpdateNullField || pEntity.SkuId!=null)
                strSql.Append( "[SkuId]=@SkuId,");
            if (pIsUpdateNullField || pEntity.VipId!=null)
                strSql.Append( "[VipId]=@VipId,");
            if (pIsUpdateNullField || pEntity.SalesPrice!=null)
                strSql.Append( "[SalesPrice]=@SalesPrice,");
            if (pIsUpdateNullField || pEntity.BargainPersonCount!=null)
                strSql.Append( "[BargainPersonCount]=@BargainPersonCount,");
            if (pIsUpdateNullField || pEntity.EventOrderMappingId!=null)
                strSql.Append( "[EventOrderMappingId]=@EventOrderMappingId,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.CustomerId!=null)
                strSql.Append( "[CustomerId]=@CustomerId");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where KJEventJoinId=@KJEventJoinId ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@EventId",SqlDbType.UniqueIdentifier),
					new SqlParameter("@ItemId",SqlDbType.NVarChar),
					new SqlParameter("@SkuId",SqlDbType.NVarChar),
					new SqlParameter("@VipId",SqlDbType.NVarChar),
					new SqlParameter("@SalesPrice",SqlDbType.Decimal),
					new SqlParameter("@BargainPersonCount",SqlDbType.Int),
					new SqlParameter("@EventOrderMappingId",SqlDbType.UniqueIdentifier),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@CustomerId",SqlDbType.NVarChar),
					new SqlParameter("@KJEventJoinId",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.EventId;
			parameters[1].Value = pEntity.ItemId;
			parameters[2].Value = pEntity.SkuId;
			parameters[3].Value = pEntity.VipId;
			parameters[4].Value = pEntity.SalesPrice;
			parameters[5].Value = pEntity.BargainPersonCount;
			parameters[6].Value = pEntity.EventOrderMappingId;
			parameters[7].Value = pEntity.LastUpdateBy;
			parameters[8].Value = pEntity.LastUpdateTime;
			parameters[9].Value = pEntity.CustomerId;
			parameters[10].Value = pEntity.KJEventJoinId;

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
        public void Update(PanicbuyingKJEventJoinEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(PanicbuyingKJEventJoinEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(PanicbuyingKJEventJoinEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.KJEventJoinId.HasValue)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.KJEventJoinId.Value, pTran);           
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
            sql.AppendLine("update [PanicbuyingKJEventJoin] set  isdelete=1 where KJEventJoinId=@KJEventJoinId;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@KJEventJoinId",SqlDbType=SqlDbType.UniqueIdentifier,Value=pID}
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
        public void Delete(PanicbuyingKJEventJoinEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (!pEntity.KJEventJoinId.HasValue)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.KJEventJoinId;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(PanicbuyingKJEventJoinEntity[] pEntities)
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
            sql.AppendLine("update [PanicbuyingKJEventJoin] set  isdelete=1 where KJEventJoinId in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public PanicbuyingKJEventJoinEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [PanicbuyingKJEventJoin] where 1=1  and isdelete=0 ");
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
            List<PanicbuyingKJEventJoinEntity> list = new List<PanicbuyingKJEventJoinEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    PanicbuyingKJEventJoinEntity m;
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
        public PagedQueryResult<PanicbuyingKJEventJoinEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [KJEventJoinId] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [PanicbuyingKJEventJoin] where 1=1  and isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [PanicbuyingKJEventJoin] where 1=1  and isdelete=0 ");
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
            PagedQueryResult<PanicbuyingKJEventJoinEntity> result = new PagedQueryResult<PanicbuyingKJEventJoinEntity>();
            List<PanicbuyingKJEventJoinEntity> list = new List<PanicbuyingKJEventJoinEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    PanicbuyingKJEventJoinEntity m;
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
        public PanicbuyingKJEventJoinEntity[] QueryByEntity(PanicbuyingKJEventJoinEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<PanicbuyingKJEventJoinEntity> PagedQueryByEntity(PanicbuyingKJEventJoinEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(PanicbuyingKJEventJoinEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.KJEventJoinId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "KJEventJoinId", Value = pQueryEntity.KJEventJoinId });
            if (pQueryEntity.EventId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EventId", Value = pQueryEntity.EventId });
            if (pQueryEntity.ItemId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ItemId", Value = pQueryEntity.ItemId });
            if (pQueryEntity.SkuId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SkuId", Value = pQueryEntity.SkuId });
            if (pQueryEntity.VipId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VipId", Value = pQueryEntity.VipId });
            if (pQueryEntity.SalesPrice!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SalesPrice", Value = pQueryEntity.SalesPrice });
            if (pQueryEntity.BargainPersonCount!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BargainPersonCount", Value = pQueryEntity.BargainPersonCount });
            if (pQueryEntity.EventOrderMappingId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EventOrderMappingId", Value = pQueryEntity.EventOrderMappingId });
            if (pQueryEntity.CreateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateTime", Value = pQueryEntity.CreateTime });
            if (pQueryEntity.CreateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateBy", Value = pQueryEntity.CreateBy });
            if (pQueryEntity.LastUpdateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateBy", Value = pQueryEntity.LastUpdateBy });
            if (pQueryEntity.LastUpdateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateTime", Value = pQueryEntity.LastUpdateTime });
            if (pQueryEntity.IsDelete!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsDelete", Value = pQueryEntity.IsDelete });
            if (pQueryEntity.CustomerId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerId", Value = pQueryEntity.CustomerId });
            if (pQueryEntity.KJEventJoinId != null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "KJEventJoinId", Value = pQueryEntity.KJEventJoinId });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(IDataReader pReader, out PanicbuyingKJEventJoinEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new PanicbuyingKJEventJoinEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["KJEventJoinId"] != DBNull.Value)
			{
				pInstance.KJEventJoinId =  (Guid)pReader["KJEventJoinId"];
			}
			if (pReader["EventId"] != DBNull.Value)
			{
				pInstance.EventId =  (Guid)pReader["EventId"];
			}
			if (pReader["ItemId"] != DBNull.Value)
			{
				pInstance.ItemId =  Convert.ToString(pReader["ItemId"]);
			}
			if (pReader["SkuId"] != DBNull.Value)
			{
				pInstance.SkuId =  Convert.ToString(pReader["SkuId"]);
			}
			if (pReader["VipId"] != DBNull.Value)
			{
				pInstance.VipId =  Convert.ToString(pReader["VipId"]);
			}
			if (pReader["SalesPrice"] != DBNull.Value)
			{
				pInstance.SalesPrice =  Convert.ToDecimal(pReader["SalesPrice"]);
			}
			if (pReader["BargainPersonCount"] != DBNull.Value)
			{
				pInstance.BargainPersonCount =   Convert.ToInt32(pReader["BargainPersonCount"]);
			}
			if (pReader["EventOrderMappingId"] != DBNull.Value)
			{
				pInstance.EventOrderMappingId =  (Guid)pReader["EventOrderMappingId"];
			}
			if (pReader["CreateTime"] != DBNull.Value)
			{
				pInstance.CreateTime =  Convert.ToDateTime(pReader["CreateTime"]);
			}
			if (pReader["CreateBy"] != DBNull.Value)
			{
				pInstance.CreateBy =  Convert.ToString(pReader["CreateBy"]);
			}
			if (pReader["LastUpdateBy"] != DBNull.Value)
			{
				pInstance.LastUpdateBy =  Convert.ToString(pReader["LastUpdateBy"]);
			}
			if (pReader["LastUpdateTime"] != DBNull.Value)
			{
				pInstance.LastUpdateTime =  Convert.ToDateTime(pReader["LastUpdateTime"]);
			}
			if (pReader["IsDelete"] != DBNull.Value)
			{
				pInstance.IsDelete =   Convert.ToInt32(pReader["IsDelete"]);
			}
			if (pReader["CustomerId"] != DBNull.Value)
			{
				pInstance.CustomerId =  Convert.ToString(pReader["CustomerId"]);
			}

        }
        #endregion
    }
}
