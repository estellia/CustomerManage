/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/8/29 16:01:06
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
    /// 表T_Payment_Type的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class TPaymentTypeDAO : Base.BaseCPOSDAO, ICRUDable<TPaymentTypeEntity>, IQueryable<TPaymentTypeEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public TPaymentTypeDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(TPaymentTypeEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(TPaymentTypeEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //初始化固定字段
            pEntity.CreateTime = DateTime.Now;
            pEntity.CreateBy = CurrentUserInfo.UserID;
            pEntity.LastUpdateTime = pEntity.CreateTime;
            pEntity.LastUpdateBy = CurrentUserInfo.UserID;
            pEntity.IsDelete = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [T_Payment_Type](");
            strSql.Append("[Payment_Type_Name],[Payment_Type_Code],[Status],[PaymentCompany],[PaymentItemType],[LogoURL],[PaymentDesc],[CreateTime],[LastUpdateTime],[CreateBy],[LastUpdateBy],[IsDelete],[Payment_Type_Id])");
            strSql.Append(" values (");
            strSql.Append("@PaymentTypeName,@PaymentTypeCode,@Status,@PaymentCompany,@PaymentItemType,@LogoURL,@PaymentDesc,@CreateTime,@LastUpdateTime,@CreateBy,@LastUpdateBy,@IsDelete,@PaymentTypeID)");            

			string pkString = pEntity.PaymentTypeID;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@PaymentTypeName",SqlDbType.NVarChar),
					new SqlParameter("@PaymentTypeCode",SqlDbType.NVarChar),
					new SqlParameter("@Status",SqlDbType.NVarChar),
					new SqlParameter("@PaymentCompany",SqlDbType.NVarChar),
					new SqlParameter("@PaymentItemType",SqlDbType.NVarChar),
					new SqlParameter("@LogoURL",SqlDbType.NVarChar),
					new SqlParameter("@PaymentDesc",SqlDbType.NVarChar),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@CreateBy",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@PaymentTypeID",SqlDbType.NVarChar)
            };
			parameters[0].Value = pEntity.PaymentTypeName;
			parameters[1].Value = pEntity.PaymentTypeCode;
			parameters[2].Value = pEntity.Status;
			parameters[3].Value = pEntity.PaymentCompany;
			parameters[4].Value = pEntity.PaymentItemType;
			parameters[5].Value = pEntity.LogoURL;
			parameters[6].Value = pEntity.PaymentDesc;
			parameters[7].Value = pEntity.CreateTime;
			parameters[8].Value = pEntity.LastUpdateTime;
			parameters[9].Value = pEntity.CreateBy;
			parameters[10].Value = pEntity.LastUpdateBy;
			parameters[11].Value = pEntity.IsDelete;
			parameters[12].Value = pkString;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            pEntity.PaymentTypeID = pkString;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public TPaymentTypeEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Payment_Type] where Payment_Type_Id='{0}' and IsDelete=0 ", id.ToString());
            //读取数据
            TPaymentTypeEntity m = null;
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
        public TPaymentTypeEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Payment_Type] where isdelete=0");
            //读取数据
            List<TPaymentTypeEntity> list = new List<TPaymentTypeEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    TPaymentTypeEntity m;
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
        public void Update(TPaymentTypeEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity,true,pTran);
        }
        public void Update(TPaymentTypeEntity pEntity , bool pIsUpdateNullField, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.PaymentTypeID==null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
            pEntity.LastUpdateTime = DateTime.Now;
            pEntity.LastUpdateBy = CurrentUserInfo.UserID;

            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [T_Payment_Type] set ");
            if (pIsUpdateNullField || pEntity.PaymentTypeName!=null)
                strSql.Append( "[Payment_Type_Name]=@PaymentTypeName,");
            if (pIsUpdateNullField || pEntity.PaymentTypeCode!=null)
                strSql.Append( "[Payment_Type_Code]=@PaymentTypeCode,");
            if (pIsUpdateNullField || pEntity.Status!=null)
                strSql.Append( "[Status]=@Status,");
            if (pIsUpdateNullField || pEntity.PaymentCompany!=null)
                strSql.Append( "[PaymentCompany]=@PaymentCompany,");
            if (pIsUpdateNullField || pEntity.PaymentItemType!=null)
                strSql.Append( "[PaymentItemType]=@PaymentItemType,");
            if (pIsUpdateNullField || pEntity.LogoURL!=null)
                strSql.Append( "[LogoURL]=@LogoURL,");
            if (pIsUpdateNullField || pEntity.PaymentDesc!=null)
                strSql.Append( "[PaymentDesc]=@PaymentDesc,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where Payment_Type_Id=@PaymentTypeID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@PaymentTypeName",SqlDbType.NVarChar),
					new SqlParameter("@PaymentTypeCode",SqlDbType.NVarChar),
					new SqlParameter("@Status",SqlDbType.NVarChar),
					new SqlParameter("@PaymentCompany",SqlDbType.NVarChar),
					new SqlParameter("@PaymentItemType",SqlDbType.NVarChar),
					new SqlParameter("@LogoURL",SqlDbType.NVarChar),
					new SqlParameter("@PaymentDesc",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@PaymentTypeID",SqlDbType.NVarChar)
            };
			parameters[0].Value = pEntity.PaymentTypeName;
			parameters[1].Value = pEntity.PaymentTypeCode;
			parameters[2].Value = pEntity.Status;
			parameters[3].Value = pEntity.PaymentCompany;
			parameters[4].Value = pEntity.PaymentItemType;
			parameters[5].Value = pEntity.LogoURL;
			parameters[6].Value = pEntity.PaymentDesc;
			parameters[7].Value = pEntity.LastUpdateTime;
			parameters[8].Value = pEntity.LastUpdateBy;
			parameters[9].Value = pEntity.PaymentTypeID;

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
        public void Update(TPaymentTypeEntity pEntity )
        {
            Update(pEntity ,true);
        }
        public void Update(TPaymentTypeEntity pEntity ,bool pIsUpdateNullField )
        {
            this.Update(pEntity, pIsUpdateNullField, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(TPaymentTypeEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(TPaymentTypeEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.PaymentTypeID==null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.PaymentTypeID, pTran);           
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
            sql.AppendLine("update [T_Payment_Type] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where Payment_Type_Id=@PaymentTypeID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@LastUpdateTime",SqlDbType=SqlDbType.DateTime,Value=DateTime.Now},
                new SqlParameter{ParameterName="@LastUpdateBy",SqlDbType=SqlDbType.VarChar,Value=Convert.ToString(CurrentUserInfo.UserID)},
                new SqlParameter{ParameterName="@PaymentTypeID",SqlDbType=SqlDbType.VarChar,Value=pID}
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
        public void Delete(TPaymentTypeEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var item = pEntities[i];
                //参数校验
                if (item == null)
                    throw new ArgumentNullException("pEntity");
                if (item.PaymentTypeID == null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = item.PaymentTypeID;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(TPaymentTypeEntity[] pEntities)
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
            sql.AppendLine("update [T_Payment_Type] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy='"+CurrentUserInfo.UserID+"',IsDelete=1 where Payment_Type_Id in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public TPaymentTypeEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Payment_Type] where isdelete=0 ");
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
            List<TPaymentTypeEntity> list = new List<TPaymentTypeEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    TPaymentTypeEntity m;
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
        public PagedQueryResult<TPaymentTypeEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [PaymentTypeID] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [TPaymentType] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [TPaymentType] where isdelete=0 ");
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
            PagedQueryResult<TPaymentTypeEntity> result = new PagedQueryResult<TPaymentTypeEntity>();
            List<TPaymentTypeEntity> list = new List<TPaymentTypeEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    TPaymentTypeEntity m;
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
        public TPaymentTypeEntity[] QueryByEntity(TPaymentTypeEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<TPaymentTypeEntity> PagedQueryByEntity(TPaymentTypeEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(TPaymentTypeEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.PaymentTypeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PaymentTypeID", Value = pQueryEntity.PaymentTypeID });
            if (pQueryEntity.PaymentTypeName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PaymentTypeName", Value = pQueryEntity.PaymentTypeName });
            if (pQueryEntity.PaymentTypeCode!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PaymentTypeCode", Value = pQueryEntity.PaymentTypeCode });
            if (pQueryEntity.Status!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Status", Value = pQueryEntity.Status });
            if (pQueryEntity.PaymentCompany!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PaymentCompany", Value = pQueryEntity.PaymentCompany });
            if (pQueryEntity.PaymentItemType!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PaymentItemType", Value = pQueryEntity.PaymentItemType });
            if (pQueryEntity.LogoURL!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LogoURL", Value = pQueryEntity.LogoURL });
            if (pQueryEntity.PaymentDesc!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PaymentDesc", Value = pQueryEntity.PaymentDesc });
            if (pQueryEntity.CreateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateTime", Value = pQueryEntity.CreateTime });
            if (pQueryEntity.LastUpdateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateTime", Value = pQueryEntity.LastUpdateTime });
            if (pQueryEntity.CreateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateBy", Value = pQueryEntity.CreateBy });
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
        protected void Load(SqlDataReader pReader, out TPaymentTypeEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new TPaymentTypeEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["Payment_Type_Id"] != DBNull.Value)
			{
				pInstance.PaymentTypeID =  Convert.ToString(pReader["Payment_Type_Id"]);
			}
			if (pReader["Payment_Type_Name"] != DBNull.Value)
			{
				pInstance.PaymentTypeName =  Convert.ToString(pReader["Payment_Type_Name"]);
			}
			if (pReader["Payment_Type_Code"] != DBNull.Value)
			{
				pInstance.PaymentTypeCode =  Convert.ToString(pReader["Payment_Type_Code"]);
			}
			if (pReader["Status"] != DBNull.Value)
			{
				pInstance.Status =  Convert.ToString(pReader["Status"]);
			}
			if (pReader["PaymentCompany"] != DBNull.Value)
			{
				pInstance.PaymentCompany =  Convert.ToString(pReader["PaymentCompany"]);
			}
			if (pReader["PaymentItemType"] != DBNull.Value)
			{
				pInstance.PaymentItemType =  Convert.ToString(pReader["PaymentItemType"]);
			}
			if (pReader["LogoURL"] != DBNull.Value)
			{
				pInstance.LogoURL =  Convert.ToString(pReader["LogoURL"]);
			}
			if (pReader["PaymentDesc"] != DBNull.Value)
			{
				pInstance.PaymentDesc =  Convert.ToString(pReader["PaymentDesc"]);
			}
			if (pReader["CreateTime"] != DBNull.Value)
			{
				pInstance.CreateTime =  Convert.ToDateTime(pReader["CreateTime"]);
			}
			if (pReader["LastUpdateTime"] != DBNull.Value)
			{
				pInstance.LastUpdateTime =  Convert.ToDateTime(pReader["LastUpdateTime"]);
			}
			if (pReader["CreateBy"] != DBNull.Value)
			{
				pInstance.CreateBy =  Convert.ToString(pReader["CreateBy"]);
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
