/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/4/27 20:28:30
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
    /// 表PanicbuyingKJEventSkuMapping的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class PanicbuyingKJEventSkuMappingDAO : BaseCPOSDAO, ICRUDable<PanicbuyingKJEventSkuMappingEntity>, IQueryable<PanicbuyingKJEventSkuMappingEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public PanicbuyingKJEventSkuMappingDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(PanicbuyingKJEventSkuMappingEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(PanicbuyingKJEventSkuMappingEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [PanicbuyingKJEventSkuMapping](");
            strSql.Append("[EventItemMappingID],[SkuID],[AddedTime],[Qty],[KeepQty],[SoldQty],[SinglePurchaseQty],[Price],[BasePrice],[BargainStartPrice],[BargainEndPrice],[DisplayIndex],[IsFirst],[Status],[CreateTime],[CreateBy],[LastUpdateBy],[LastUpdateTime],[CustomerId],[IsDelete],[EventSKUMappingId])");
            strSql.Append(" values (");
            strSql.Append("@EventItemMappingID,@SkuID,@AddedTime,@Qty,@KeepQty,@SoldQty,@SinglePurchaseQty,@Price,@BasePrice,@BargainStartPrice,@BargainEndPrice,@DisplayIndex,@IsFirst,@Status,@CreateTime,@CreateBy,@LastUpdateBy,@LastUpdateTime,@CustomerId,@IsDelete,@EventSKUMappingId)");            

			Guid? pkGuid;
			if (pEntity.EventSKUMappingId == null)
				pkGuid = Guid.NewGuid();
			else
				pkGuid = pEntity.EventSKUMappingId;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@EventItemMappingID",SqlDbType.VarChar),
					new SqlParameter("@SkuID",SqlDbType.VarChar),
					new SqlParameter("@AddedTime",SqlDbType.DateTime),
					new SqlParameter("@Qty",SqlDbType.Int),
					new SqlParameter("@KeepQty",SqlDbType.Int),
					new SqlParameter("@SoldQty",SqlDbType.Int),
					new SqlParameter("@SinglePurchaseQty",SqlDbType.Int),
					new SqlParameter("@Price",SqlDbType.Decimal),
					new SqlParameter("@BasePrice",SqlDbType.Decimal),
					new SqlParameter("@BargainStartPrice",SqlDbType.Decimal),
					new SqlParameter("@BargainEndPrice",SqlDbType.Decimal),
					new SqlParameter("@DisplayIndex",SqlDbType.Int),
					new SqlParameter("@IsFirst",SqlDbType.Int),
					new SqlParameter("@Status",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@CreateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@CustomerId",SqlDbType.VarChar),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@EventSKUMappingId",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.EventItemMappingID;
			parameters[1].Value = pEntity.SkuID;
			parameters[2].Value = pEntity.AddedTime;
			parameters[3].Value = pEntity.Qty;
			parameters[4].Value = pEntity.KeepQty;
			parameters[5].Value = pEntity.SoldQty;
			parameters[6].Value = pEntity.SinglePurchaseQty;
			parameters[7].Value = pEntity.Price;
			parameters[8].Value = pEntity.BasePrice;
			parameters[9].Value = pEntity.BargainStartPrice;
			parameters[10].Value = pEntity.BargainEndPrice;
			parameters[11].Value = pEntity.DisplayIndex;
			parameters[12].Value = pEntity.IsFirst;
			parameters[13].Value = pEntity.Status;
			parameters[14].Value = pEntity.CreateTime;
			parameters[15].Value = pEntity.CreateBy;
			parameters[16].Value = pEntity.LastUpdateBy;
			parameters[17].Value = pEntity.LastUpdateTime;
			parameters[18].Value = pEntity.CustomerId;
			parameters[19].Value = pEntity.IsDelete;
			parameters[20].Value = pkGuid;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.EventSKUMappingId = pkGuid;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public PanicbuyingKJEventSkuMappingEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [PanicbuyingKJEventSkuMapping] where EventSKUMappingId='{0}'  and isdelete=0 ", id.ToString());
            //读取数据
            PanicbuyingKJEventSkuMappingEntity m = null;
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
        public PanicbuyingKJEventSkuMappingEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [PanicbuyingKJEventSkuMapping] where 1=1  and isdelete=0");
            //读取数据
            List<PanicbuyingKJEventSkuMappingEntity> list = new List<PanicbuyingKJEventSkuMappingEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    PanicbuyingKJEventSkuMappingEntity m;
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
        public void Update(PanicbuyingKJEventSkuMappingEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(PanicbuyingKJEventSkuMappingEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.EventSKUMappingId.HasValue)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
			pEntity.LastUpdateTime=DateTime.Now;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PanicbuyingKJEventSkuMapping] set ");
                        if (pIsUpdateNullField || pEntity.EventItemMappingID!=null)
                strSql.Append( "[EventItemMappingID]=@EventItemMappingID,");
            if (pIsUpdateNullField || pEntity.SkuID!=null)
                strSql.Append( "[SkuID]=@SkuID,");
            if (pIsUpdateNullField || pEntity.AddedTime!=null)
                strSql.Append( "[AddedTime]=@AddedTime,");
            if (pIsUpdateNullField || pEntity.Qty!=null)
                strSql.Append( "[Qty]=@Qty,");
            if (pIsUpdateNullField || pEntity.KeepQty!=null)
                strSql.Append( "[KeepQty]=@KeepQty,");
            if (pIsUpdateNullField || pEntity.SoldQty!=null)
                strSql.Append( "[SoldQty]=@SoldQty,");
            if (pIsUpdateNullField || pEntity.SinglePurchaseQty!=null)
                strSql.Append( "[SinglePurchaseQty]=@SinglePurchaseQty,");
            if (pIsUpdateNullField || pEntity.Price!=null)
                strSql.Append( "[Price]=@Price,");
            if (pIsUpdateNullField || pEntity.BasePrice!=null)
                strSql.Append( "[BasePrice]=@BasePrice,");
            if (pIsUpdateNullField || pEntity.BargainStartPrice!=null)
                strSql.Append( "[BargainStartPrice]=@BargainStartPrice,");
            if (pIsUpdateNullField || pEntity.BargainEndPrice!=null)
                strSql.Append( "[BargainEndPrice]=@BargainEndPrice,");
            if (pIsUpdateNullField || pEntity.DisplayIndex!=null)
                strSql.Append( "[DisplayIndex]=@DisplayIndex,");
            if (pIsUpdateNullField || pEntity.IsFirst!=null)
                strSql.Append( "[IsFirst]=@IsFirst,");
            if (pIsUpdateNullField || pEntity.Status!=null)
                strSql.Append( "[Status]=@Status,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.CustomerId!=null)
                strSql.Append( "[CustomerId]=@CustomerId");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where EventSKUMappingId=@EventSKUMappingId ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@EventItemMappingID",SqlDbType.VarChar),
					new SqlParameter("@SkuID",SqlDbType.VarChar),
					new SqlParameter("@AddedTime",SqlDbType.DateTime),
					new SqlParameter("@Qty",SqlDbType.Int),
					new SqlParameter("@KeepQty",SqlDbType.Int),
					new SqlParameter("@SoldQty",SqlDbType.Int),
					new SqlParameter("@SinglePurchaseQty",SqlDbType.Int),
					new SqlParameter("@Price",SqlDbType.Decimal),
					new SqlParameter("@BasePrice",SqlDbType.Decimal),
					new SqlParameter("@BargainStartPrice",SqlDbType.Decimal),
					new SqlParameter("@BargainEndPrice",SqlDbType.Decimal),
					new SqlParameter("@DisplayIndex",SqlDbType.Int),
					new SqlParameter("@IsFirst",SqlDbType.Int),
					new SqlParameter("@Status",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@CustomerId",SqlDbType.VarChar),
					new SqlParameter("@EventSKUMappingId",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.EventItemMappingID;
			parameters[1].Value = pEntity.SkuID;
			parameters[2].Value = pEntity.AddedTime;
			parameters[3].Value = pEntity.Qty;
			parameters[4].Value = pEntity.KeepQty;
			parameters[5].Value = pEntity.SoldQty;
			parameters[6].Value = pEntity.SinglePurchaseQty;
			parameters[7].Value = pEntity.Price;
			parameters[8].Value = pEntity.BasePrice;
			parameters[9].Value = pEntity.BargainStartPrice;
			parameters[10].Value = pEntity.BargainEndPrice;
			parameters[11].Value = pEntity.DisplayIndex;
			parameters[12].Value = pEntity.IsFirst;
			parameters[13].Value = pEntity.Status;
			parameters[14].Value = pEntity.LastUpdateBy;
			parameters[15].Value = pEntity.LastUpdateTime;
			parameters[16].Value = pEntity.CustomerId;
			parameters[17].Value = pEntity.EventSKUMappingId;

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
        public void Update(PanicbuyingKJEventSkuMappingEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(PanicbuyingKJEventSkuMappingEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(PanicbuyingKJEventSkuMappingEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.EventSKUMappingId.HasValue)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.EventSKUMappingId.Value, pTran);           
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
            sql.AppendLine("update [PanicbuyingKJEventSkuMapping] set  isdelete=1 where EventSKUMappingId=@EventSKUMappingId;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@EventSKUMappingId",SqlDbType=SqlDbType.UniqueIdentifier,Value=pID}
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
        public void Delete(PanicbuyingKJEventSkuMappingEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (!pEntity.EventSKUMappingId.HasValue)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.EventSKUMappingId;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(PanicbuyingKJEventSkuMappingEntity[] pEntities)
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
            sql.AppendLine("update [PanicbuyingKJEventSkuMapping] set  isdelete=1 where EventSKUMappingId in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public PanicbuyingKJEventSkuMappingEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [PanicbuyingKJEventSkuMapping] where 1=1  and isdelete=0 ");
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
            List<PanicbuyingKJEventSkuMappingEntity> list = new List<PanicbuyingKJEventSkuMappingEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    PanicbuyingKJEventSkuMappingEntity m;
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
        public PagedQueryResult<PanicbuyingKJEventSkuMappingEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [EventSKUMappingId] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [PanicbuyingKJEventSkuMapping] where 1=1  and isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [PanicbuyingKJEventSkuMapping] where 1=1  and isdelete=0 ");
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
            PagedQueryResult<PanicbuyingKJEventSkuMappingEntity> result = new PagedQueryResult<PanicbuyingKJEventSkuMappingEntity>();
            List<PanicbuyingKJEventSkuMappingEntity> list = new List<PanicbuyingKJEventSkuMappingEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    PanicbuyingKJEventSkuMappingEntity m;
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
        public PanicbuyingKJEventSkuMappingEntity[] QueryByEntity(PanicbuyingKJEventSkuMappingEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<PanicbuyingKJEventSkuMappingEntity> PagedQueryByEntity(PanicbuyingKJEventSkuMappingEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(PanicbuyingKJEventSkuMappingEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.EventSKUMappingId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EventSKUMappingId", Value = pQueryEntity.EventSKUMappingId });
            if (pQueryEntity.EventItemMappingID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EventItemMappingID", Value = pQueryEntity.EventItemMappingID });
            if (pQueryEntity.SkuID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SkuID", Value = pQueryEntity.SkuID });
            if (pQueryEntity.AddedTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AddedTime", Value = pQueryEntity.AddedTime });
            if (pQueryEntity.Qty!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Qty", Value = pQueryEntity.Qty });
            if (pQueryEntity.KeepQty!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "KeepQty", Value = pQueryEntity.KeepQty });
            if (pQueryEntity.SoldQty!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SoldQty", Value = pQueryEntity.SoldQty });
            if (pQueryEntity.SinglePurchaseQty!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "SinglePurchaseQty", Value = pQueryEntity.SinglePurchaseQty });
            if (pQueryEntity.Price!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Price", Value = pQueryEntity.Price });
            if (pQueryEntity.BasePrice!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BasePrice", Value = pQueryEntity.BasePrice });
            if (pQueryEntity.BargainStartPrice!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BargainStartPrice", Value = pQueryEntity.BargainStartPrice });
            if (pQueryEntity.BargainEndPrice!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BargainEndPrice", Value = pQueryEntity.BargainEndPrice });
            if (pQueryEntity.DisplayIndex!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DisplayIndex", Value = pQueryEntity.DisplayIndex });
            if (pQueryEntity.IsFirst!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsFirst", Value = pQueryEntity.IsFirst });
            if (pQueryEntity.Status!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Status", Value = pQueryEntity.Status });
            if (pQueryEntity.CreateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateTime", Value = pQueryEntity.CreateTime });
            if (pQueryEntity.CreateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateBy", Value = pQueryEntity.CreateBy });
            if (pQueryEntity.LastUpdateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateBy", Value = pQueryEntity.LastUpdateBy });
            if (pQueryEntity.LastUpdateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateTime", Value = pQueryEntity.LastUpdateTime });
            if (pQueryEntity.CustomerId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerId", Value = pQueryEntity.CustomerId });
            if (pQueryEntity.IsDelete!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsDelete", Value = pQueryEntity.IsDelete });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(IDataReader pReader, out PanicbuyingKJEventSkuMappingEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new PanicbuyingKJEventSkuMappingEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["EventSKUMappingId"] != DBNull.Value)
			{
				pInstance.EventSKUMappingId =  (Guid)pReader["EventSKUMappingId"];
			}
			if (pReader["EventItemMappingID"] != DBNull.Value)
			{
				pInstance.EventItemMappingID =  Convert.ToString(pReader["EventItemMappingID"]);
			}
			if (pReader["SkuID"] != DBNull.Value)
			{
				pInstance.SkuID =  Convert.ToString(pReader["SkuID"]);
			}
			if (pReader["AddedTime"] != DBNull.Value)
			{
				pInstance.AddedTime =  Convert.ToDateTime(pReader["AddedTime"]);
			}
			if (pReader["Qty"] != DBNull.Value)
			{
				pInstance.Qty =   Convert.ToInt32(pReader["Qty"]);
			}
			if (pReader["KeepQty"] != DBNull.Value)
			{
				pInstance.KeepQty =   Convert.ToInt32(pReader["KeepQty"]);
			}
			if (pReader["SoldQty"] != DBNull.Value)
			{
				pInstance.SoldQty =   Convert.ToInt32(pReader["SoldQty"]);
			}
			if (pReader["SinglePurchaseQty"] != DBNull.Value)
			{
				pInstance.SinglePurchaseQty =   Convert.ToInt32(pReader["SinglePurchaseQty"]);
			}
			if (pReader["Price"] != DBNull.Value)
			{
				pInstance.Price =  Convert.ToDecimal(pReader["Price"]);
			}
			if (pReader["BasePrice"] != DBNull.Value)
			{
				pInstance.BasePrice =  Convert.ToDecimal(pReader["BasePrice"]);
			}
			if (pReader["BargainStartPrice"] != DBNull.Value)
			{
				pInstance.BargainStartPrice =  Convert.ToDecimal(pReader["BargainStartPrice"]);
			}
			if (pReader["BargainEndPrice"] != DBNull.Value)
			{
				pInstance.BargainEndPrice =  Convert.ToDecimal(pReader["BargainEndPrice"]);
			}
			if (pReader["DisplayIndex"] != DBNull.Value)
			{
				pInstance.DisplayIndex =   Convert.ToInt32(pReader["DisplayIndex"]);
			}
			if (pReader["IsFirst"] != DBNull.Value)
			{
				pInstance.IsFirst =   Convert.ToInt32(pReader["IsFirst"]);
			}
			if (pReader["Status"] != DBNull.Value)
			{
				pInstance.Status =   Convert.ToInt32(pReader["Status"]);
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
			if (pReader["CustomerId"] != DBNull.Value)
			{
				pInstance.CustomerId =  Convert.ToString(pReader["CustomerId"]);
			}
			if (pReader["IsDelete"] != DBNull.Value)
			{
				pInstance.IsDelete =   Convert.ToInt32(pReader["IsDelete"]);
			}

        }
        #endregion
    }
}
