/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/9/23 10:32:38
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
    /// 表T_Item_Delivery_Mapping的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class T_Item_Delivery_MappingDAO : BaseCPOSDAO, ICRUDable<T_Item_Delivery_MappingEntity>, IQueryable<T_Item_Delivery_MappingEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public T_Item_Delivery_MappingDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(T_Item_Delivery_MappingEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(T_Item_Delivery_MappingEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //初始化固定字段


            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [T_Item_Delivery_Mapping](");
            strSql.Append("[CustomerId],[Item_Id],[DeliveryId],[Create_Time],[Create_User_Id],[Modify_Time],[Modify_User_Id],[Item_Delivery_Id])");
            strSql.Append(" values (");
            strSql.Append("@CustomerId,@Item_Id,@DeliveryId,@Create_Time,@Create_User_Id,@Modify_Time,@Modify_User_Id,@Item_Delivery_Id)");            

			string pkString = pEntity.Item_Delivery_Id;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@CustomerId",SqlDbType.NVarChar),
					new SqlParameter("@Item_Id",SqlDbType.NVarChar),
					new SqlParameter("@DeliveryId",SqlDbType.Int),
					new SqlParameter("@Create_Time",SqlDbType.NVarChar),
					new SqlParameter("@Create_User_Id",SqlDbType.NVarChar),
					new SqlParameter("@Modify_Time",SqlDbType.NVarChar),
					new SqlParameter("@Modify_User_Id",SqlDbType.NVarChar),
					new SqlParameter("@Item_Delivery_Id",SqlDbType.VarChar)
            };
			parameters[0].Value = pEntity.CustomerId;
			parameters[1].Value = pEntity.Item_Id;
			parameters[2].Value = pEntity.DeliveryId;
			parameters[3].Value = pEntity.Create_Time;
			parameters[4].Value = pEntity.Create_User_Id;
			parameters[5].Value = pEntity.Modify_Time;
			parameters[6].Value = pEntity.Modify_User_Id;
			parameters[7].Value = pkString;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.Item_Delivery_Id = pkString;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public T_Item_Delivery_MappingEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Item_Delivery_Mapping] where Item_Delivery_Id='{0}'  ", id.ToString());
            //读取数据
            T_Item_Delivery_MappingEntity m = null;
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
        public T_Item_Delivery_MappingEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Item_Delivery_Mapping] where 1=1 ");
            //读取数据
            List<T_Item_Delivery_MappingEntity> list = new List<T_Item_Delivery_MappingEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    T_Item_Delivery_MappingEntity m;
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
        public void Update(T_Item_Delivery_MappingEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(T_Item_Delivery_MappingEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.Item_Delivery_Id == null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [T_Item_Delivery_Mapping] set ");
                        if (pIsUpdateNullField || pEntity.CustomerId!=null)
                strSql.Append( "[CustomerId]=@CustomerId,");
            if (pIsUpdateNullField || pEntity.Item_Id!=null)
                strSql.Append( "[Item_Id]=@Item_Id,");
            if (pIsUpdateNullField || pEntity.DeliveryId!=null)
                strSql.Append( "[DeliveryId]=@DeliveryId,");
            if (pIsUpdateNullField || pEntity.Create_Time!=null)
                strSql.Append( "[Create_Time]=@Create_Time,");
            if (pIsUpdateNullField || pEntity.Create_User_Id!=null)
                strSql.Append( "[Create_User_Id]=@Create_User_Id,");
            if (pIsUpdateNullField || pEntity.Modify_Time!=null)
                strSql.Append( "[Modify_Time]=@Modify_Time,");
            if (pIsUpdateNullField || pEntity.Modify_User_Id!=null)
                strSql.Append( "[Modify_User_Id]=@Modify_User_Id");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where Item_Delivery_Id=@Item_Delivery_Id ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@CustomerId",SqlDbType.NVarChar),
					new SqlParameter("@Item_Id",SqlDbType.NVarChar),
					new SqlParameter("@DeliveryId",SqlDbType.Int),
					new SqlParameter("@Create_Time",SqlDbType.NVarChar),
					new SqlParameter("@Create_User_Id",SqlDbType.NVarChar),
					new SqlParameter("@Modify_Time",SqlDbType.NVarChar),
					new SqlParameter("@Modify_User_Id",SqlDbType.NVarChar),
					new SqlParameter("@Item_Delivery_Id",SqlDbType.VarChar)
            };
			parameters[0].Value = pEntity.CustomerId;
			parameters[1].Value = pEntity.Item_Id;
			parameters[2].Value = pEntity.DeliveryId;
			parameters[3].Value = pEntity.Create_Time;
			parameters[4].Value = pEntity.Create_User_Id;
			parameters[5].Value = pEntity.Modify_Time;
			parameters[6].Value = pEntity.Modify_User_Id;
			parameters[7].Value = pEntity.Item_Delivery_Id;

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
        public void Update(T_Item_Delivery_MappingEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(T_Item_Delivery_MappingEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(T_Item_Delivery_MappingEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.Item_Delivery_Id == null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.Item_Delivery_Id, pTran);           
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
            sql.AppendLine("update [T_Item_Delivery_Mapping] set  where Item_Delivery_Id=@Item_Delivery_Id;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@Item_Delivery_Id",SqlDbType=SqlDbType.VarChar,Value=pID}
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
        public void Delete(T_Item_Delivery_MappingEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (pEntity.Item_Delivery_Id == null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.Item_Delivery_Id;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(T_Item_Delivery_MappingEntity[] pEntities)
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
          //  sql.AppendLine("update [T_Item_Delivery_Mapping] set  where Item_Delivery_Id in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
            sql.AppendLine("delete from  [T_Item_Delivery_Mapping] where Item_Delivery_Id in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public T_Item_Delivery_MappingEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Item_Delivery_Mapping] where 1=1  ");
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
            List<T_Item_Delivery_MappingEntity> list = new List<T_Item_Delivery_MappingEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    T_Item_Delivery_MappingEntity m;
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
        public PagedQueryResult<T_Item_Delivery_MappingEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [Item_Delivery_Id] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [T_Item_Delivery_Mapping] where 1=1  ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [T_Item_Delivery_Mapping] where 1=1  ");
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
            PagedQueryResult<T_Item_Delivery_MappingEntity> result = new PagedQueryResult<T_Item_Delivery_MappingEntity>();
            List<T_Item_Delivery_MappingEntity> list = new List<T_Item_Delivery_MappingEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    T_Item_Delivery_MappingEntity m;
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
        public T_Item_Delivery_MappingEntity[] QueryByEntity(T_Item_Delivery_MappingEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<T_Item_Delivery_MappingEntity> PagedQueryByEntity(T_Item_Delivery_MappingEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(T_Item_Delivery_MappingEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.Item_Delivery_Id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Item_Delivery_Id", Value = pQueryEntity.Item_Delivery_Id });
            if (pQueryEntity.CustomerId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerId", Value = pQueryEntity.CustomerId });
            if (pQueryEntity.Item_Id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Item_Id", Value = pQueryEntity.Item_Id });
            if (pQueryEntity.DeliveryId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DeliveryId", Value = pQueryEntity.DeliveryId });
            if (pQueryEntity.Create_Time!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Create_Time", Value = pQueryEntity.Create_Time });
            if (pQueryEntity.Create_User_Id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Create_User_Id", Value = pQueryEntity.Create_User_Id });
            if (pQueryEntity.Modify_Time!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Modify_Time", Value = pQueryEntity.Modify_Time });
            if (pQueryEntity.Modify_User_Id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Modify_User_Id", Value = pQueryEntity.Modify_User_Id });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(IDataReader pReader, out T_Item_Delivery_MappingEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new T_Item_Delivery_MappingEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["Item_Delivery_Id"] != DBNull.Value)
			{
				pInstance.Item_Delivery_Id =  Convert.ToString(pReader["Item_Delivery_Id"]);
			}
			if (pReader["CustomerId"] != DBNull.Value)
			{
				pInstance.CustomerId =  Convert.ToString(pReader["CustomerId"]);
			}
			if (pReader["Item_Id"] != DBNull.Value)
			{
				pInstance.Item_Id =  Convert.ToString(pReader["Item_Id"]);
			}
			if (pReader["DeliveryId"] != DBNull.Value)
			{
				pInstance.DeliveryId =   Convert.ToInt32(pReader["DeliveryId"]);
			}
			if (pReader["Create_Time"] != DBNull.Value)
			{
				pInstance.Create_Time =  Convert.ToString(pReader["Create_Time"]);
			}
			if (pReader["Create_User_Id"] != DBNull.Value)
			{
				pInstance.Create_User_Id =  Convert.ToString(pReader["Create_User_Id"]);
			}
			if (pReader["Modify_Time"] != DBNull.Value)
			{
				pInstance.Modify_Time =  Convert.ToString(pReader["Modify_Time"]);
			}
			if (pReader["Modify_User_Id"] != DBNull.Value)
			{
				pInstance.Modify_User_Id =  Convert.ToString(pReader["Modify_User_Id"]);
			}

        }
        #endregion
    }
}
