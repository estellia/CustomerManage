/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015/4/15 11:47:42
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
    /// 表T_ItemSkuProp的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class T_ItemSkuPropDAO : Base.BaseCPOSDAO, ICRUDable<T_ItemSkuPropEntity>, IQueryable<T_ItemSkuPropEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public T_ItemSkuPropDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(T_ItemSkuPropEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(T_ItemSkuPropEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //初始化固定字段
			pEntity.IsDelete=0;


            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [T_ItemSkuProp](");
            strSql.Append("[Item_id],[ItemSku_prop_1_id],[ItemSku_prop_2_id],[ItemSku_prop_3_id],[ItemSku_prop_4_id],[ItemSku_prop_5_id],[status],[create_user_id],[create_time],[modify_user_id],[modify_time],[IsDelete],[ItemSkuPropID])");
            strSql.Append(" values (");
            strSql.Append("@Item_id,@ItemSku_prop_1_id,@ItemSku_prop_2_id,@ItemSku_prop_3_id,@ItemSku_prop_4_id,@ItemSku_prop_5_id,@status,@create_user_id,@create_time,@modify_user_id,@modify_time,@IsDelete,@ItemSkuPropID)");            

			string pkString = pEntity.ItemSkuPropID;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@Item_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_1_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_2_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_3_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_4_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_5_id",SqlDbType.NVarChar),
					new SqlParameter("@status",SqlDbType.NVarChar),
					new SqlParameter("@create_user_id",SqlDbType.NVarChar),
					new SqlParameter("@create_time",SqlDbType.NVarChar),
					new SqlParameter("@modify_user_id",SqlDbType.NVarChar),
					new SqlParameter("@modify_time",SqlDbType.NVarChar),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@ItemSkuPropID",SqlDbType.NVarChar)
            };
			parameters[0].Value = pEntity.Item_id;
			parameters[1].Value = pEntity.ItemSku_prop_1_id;
			parameters[2].Value = pEntity.ItemSku_prop_2_id;
			parameters[3].Value = pEntity.ItemSku_prop_3_id;
			parameters[4].Value = pEntity.ItemSku_prop_4_id;
			parameters[5].Value = pEntity.ItemSku_prop_5_id;
			parameters[6].Value = pEntity.status;
			parameters[7].Value = pEntity.create_user_id;
			parameters[8].Value = pEntity.create_time;
			parameters[9].Value = pEntity.modify_user_id;
			parameters[10].Value = pEntity.modify_time;
			parameters[11].Value = pEntity.IsDelete;
			parameters[12].Value = pkString;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.ItemSkuPropID = pkString;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public T_ItemSkuPropEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_ItemSkuProp] where ItemSkuPropID='{0}'  and isdelete=0 ", id.ToString());
            //读取数据
            T_ItemSkuPropEntity m = null;
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
        public T_ItemSkuPropEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_ItemSkuProp] where 1=1  and isdelete=0");
            //读取数据
            List<T_ItemSkuPropEntity> list = new List<T_ItemSkuPropEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    T_ItemSkuPropEntity m;
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
        public void Update(T_ItemSkuPropEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(T_ItemSkuPropEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.ItemSkuPropID == null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [T_ItemSkuProp] set ");
                        if (pIsUpdateNullField || pEntity.Item_id!=null)
                strSql.Append( "[Item_id]=@Item_id,");
            if (pIsUpdateNullField || pEntity.ItemSku_prop_1_id!=null)
                strSql.Append( "[ItemSku_prop_1_id]=@ItemSku_prop_1_id,");
            if (pIsUpdateNullField || pEntity.ItemSku_prop_2_id!=null)
                strSql.Append( "[ItemSku_prop_2_id]=@ItemSku_prop_2_id,");
            if (pIsUpdateNullField || pEntity.ItemSku_prop_3_id!=null)
                strSql.Append( "[ItemSku_prop_3_id]=@ItemSku_prop_3_id,");
            if (pIsUpdateNullField || pEntity.ItemSku_prop_4_id!=null)
                strSql.Append( "[ItemSku_prop_4_id]=@ItemSku_prop_4_id,");
            if (pIsUpdateNullField || pEntity.ItemSku_prop_5_id!=null)
                strSql.Append( "[ItemSku_prop_5_id]=@ItemSku_prop_5_id,");
            if (pIsUpdateNullField || pEntity.status!=null)
                strSql.Append( "[status]=@status,");
            if (pIsUpdateNullField || pEntity.create_user_id!=null)
                strSql.Append( "[create_user_id]=@create_user_id,");
            if (pIsUpdateNullField || pEntity.create_time!=null)
                strSql.Append( "[create_time]=@create_time,");
            if (pIsUpdateNullField || pEntity.modify_user_id!=null)
                strSql.Append( "[modify_user_id]=@modify_user_id,");
            if (pIsUpdateNullField || pEntity.modify_time!=null)
                strSql.Append( "[modify_time]=@modify_time");
            strSql.Append(" where ItemSkuPropID=@ItemSkuPropID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@Item_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_1_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_2_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_3_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_4_id",SqlDbType.NVarChar),
					new SqlParameter("@ItemSku_prop_5_id",SqlDbType.NVarChar),
					new SqlParameter("@status",SqlDbType.NVarChar),
					new SqlParameter("@create_user_id",SqlDbType.NVarChar),
					new SqlParameter("@create_time",SqlDbType.NVarChar),
					new SqlParameter("@modify_user_id",SqlDbType.NVarChar),
					new SqlParameter("@modify_time",SqlDbType.NVarChar),
					new SqlParameter("@ItemSkuPropID",SqlDbType.NVarChar)
            };
			parameters[0].Value = pEntity.Item_id;
			parameters[1].Value = pEntity.ItemSku_prop_1_id;
			parameters[2].Value = pEntity.ItemSku_prop_2_id;
			parameters[3].Value = pEntity.ItemSku_prop_3_id;
			parameters[4].Value = pEntity.ItemSku_prop_4_id;
			parameters[5].Value = pEntity.ItemSku_prop_5_id;
			parameters[6].Value = pEntity.status;
			parameters[7].Value = pEntity.create_user_id;
			parameters[8].Value = pEntity.create_time;
			parameters[9].Value = pEntity.modify_user_id;
			parameters[10].Value = pEntity.modify_time;
			parameters[11].Value = pEntity.ItemSkuPropID;

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
        public void Update(T_ItemSkuPropEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(T_ItemSkuPropEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(T_ItemSkuPropEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.ItemSkuPropID == null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.ItemSkuPropID, pTran);           
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
            sql.AppendLine("update [T_ItemSkuProp] set  isdelete=1 where ItemSkuPropID=@ItemSkuPropID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@ItemSkuPropID",SqlDbType=SqlDbType.VarChar,Value=pID}
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
        public void Delete(T_ItemSkuPropEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (pEntity.ItemSkuPropID == null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.ItemSkuPropID;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(T_ItemSkuPropEntity[] pEntities)
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
            sql.AppendLine("update [T_ItemSkuProp] set  isdelete=1 where ItemSkuPropID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public T_ItemSkuPropEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_ItemSkuProp] where 1=1  and isdelete=0 ");
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
            List<T_ItemSkuPropEntity> list = new List<T_ItemSkuPropEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    T_ItemSkuPropEntity m;
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
        public PagedQueryResult<T_ItemSkuPropEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [ItemSkuPropID] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [T_ItemSkuProp] where 1=1  and isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [T_ItemSkuProp] where 1=1  and isdelete=0 ");
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
            PagedQueryResult<T_ItemSkuPropEntity> result = new PagedQueryResult<T_ItemSkuPropEntity>();
            List<T_ItemSkuPropEntity> list = new List<T_ItemSkuPropEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    T_ItemSkuPropEntity m;
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
        public T_ItemSkuPropEntity[] QueryByEntity(T_ItemSkuPropEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<T_ItemSkuPropEntity> PagedQueryByEntity(T_ItemSkuPropEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(T_ItemSkuPropEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.ItemSkuPropID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ItemSkuPropID", Value = pQueryEntity.ItemSkuPropID });
            if (pQueryEntity.Item_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Item_id", Value = pQueryEntity.Item_id });
            if (pQueryEntity.ItemSku_prop_1_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ItemSku_prop_1_id", Value = pQueryEntity.ItemSku_prop_1_id });
            if (pQueryEntity.ItemSku_prop_2_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ItemSku_prop_2_id", Value = pQueryEntity.ItemSku_prop_2_id });
            if (pQueryEntity.ItemSku_prop_3_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ItemSku_prop_3_id", Value = pQueryEntity.ItemSku_prop_3_id });
            if (pQueryEntity.ItemSku_prop_4_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ItemSku_prop_4_id", Value = pQueryEntity.ItemSku_prop_4_id });
            if (pQueryEntity.ItemSku_prop_5_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ItemSku_prop_5_id", Value = pQueryEntity.ItemSku_prop_5_id });
            if (pQueryEntity.status!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "status", Value = pQueryEntity.status });
            if (pQueryEntity.create_user_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "create_user_id", Value = pQueryEntity.create_user_id });
            if (pQueryEntity.create_time!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "create_time", Value = pQueryEntity.create_time });
            if (pQueryEntity.modify_user_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "modify_user_id", Value = pQueryEntity.modify_user_id });
            if (pQueryEntity.modify_time!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "modify_time", Value = pQueryEntity.modify_time });
            if (pQueryEntity.IsDelete!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsDelete", Value = pQueryEntity.IsDelete });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(IDataReader pReader, out T_ItemSkuPropEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new T_ItemSkuPropEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["ItemSkuPropID"] != DBNull.Value)
			{
				pInstance.ItemSkuPropID =  Convert.ToString(pReader["ItemSkuPropID"]);
			}
			if (pReader["Item_id"] != DBNull.Value)
			{
				pInstance.Item_id =  Convert.ToString(pReader["Item_id"]);
			}
			if (pReader["ItemSku_prop_1_id"] != DBNull.Value)
			{
				pInstance.ItemSku_prop_1_id =  Convert.ToString(pReader["ItemSku_prop_1_id"]);
			}
			if (pReader["ItemSku_prop_2_id"] != DBNull.Value)
			{
				pInstance.ItemSku_prop_2_id =  Convert.ToString(pReader["ItemSku_prop_2_id"]);
			}
			if (pReader["ItemSku_prop_3_id"] != DBNull.Value)
			{
				pInstance.ItemSku_prop_3_id =  Convert.ToString(pReader["ItemSku_prop_3_id"]);
			}
			if (pReader["ItemSku_prop_4_id"] != DBNull.Value)
			{
				pInstance.ItemSku_prop_4_id =  Convert.ToString(pReader["ItemSku_prop_4_id"]);
			}
			if (pReader["ItemSku_prop_5_id"] != DBNull.Value)
			{
				pInstance.ItemSku_prop_5_id =  Convert.ToString(pReader["ItemSku_prop_5_id"]);
			}
			if (pReader["status"] != DBNull.Value)
			{
				pInstance.status =  Convert.ToString(pReader["status"]);
			}
			if (pReader["create_user_id"] != DBNull.Value)
			{
				pInstance.create_user_id =  Convert.ToString(pReader["create_user_id"]);
			}
			if (pReader["create_time"] != DBNull.Value)
			{
				pInstance.create_time =  Convert.ToDateTime(pReader["create_time"]);
			}
			if (pReader["modify_user_id"] != DBNull.Value)
			{
				pInstance.modify_user_id =  Convert.ToString(pReader["modify_user_id"]);
			}
			if (pReader["modify_time"] != DBNull.Value)
			{
				pInstance.modify_time =  Convert.ToDateTime(pReader["modify_time"]);
			}
			if (pReader["IsDelete"] != DBNull.Value)
			{
				pInstance.IsDelete =   Convert.ToInt32(pReader["IsDelete"]);
			}

        }
        #endregion
    }
}
