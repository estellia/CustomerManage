/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/11/4 14:54:54
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
    /// 表APPUpgrade的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class APPUpgradeDAO : Base.BaseCPOSDAO, ICRUDable<APPUpgradeEntity>, IQueryable<APPUpgradeEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public APPUpgradeDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(APPUpgradeEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(APPUpgradeEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [APPUpgrade](");
            strSql.Append("[CustomerId],[Plat],[IOSUpgradeUrl],[AndroidUpgradeUrl],[IOSUpgradeCon],[AndroidUpgradeCon],[Versions],[AppName],[IsMandatoryUpdate],[ServerUrl],[CreateTime],[CreateBy],[LastUpdateTime],[LastUpdateBy],[IsDelete],[AppUpgradeId])");
            strSql.Append(" values (");
            strSql.Append("@CustomerId,@Plat,@IOSUpgradeUrl,@AndroidUpgradeUrl,@IOSUpgradeCon,@AndroidUpgradeCon,@Versions,@AppName,@IsMandatoryUpdate,@ServerUrl,@CreateTime,@CreateBy,@LastUpdateTime,@LastUpdateBy,@IsDelete,@AppUpgradeId)");            

			Guid? pkGuid;
			if (pEntity.AppUpgradeId == null)
				pkGuid = Guid.NewGuid();
			else
				pkGuid = pEntity.AppUpgradeId;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@CustomerId",SqlDbType.NVarChar),
					new SqlParameter("@Plat",SqlDbType.NVarChar),
					new SqlParameter("@IOSUpgradeUrl",SqlDbType.NVarChar),
					new SqlParameter("@AndroidUpgradeUrl",SqlDbType.NVarChar),
					new SqlParameter("@IOSUpgradeCon",SqlDbType.NVarChar),
					new SqlParameter("@AndroidUpgradeCon",SqlDbType.NVarChar),
					new SqlParameter("@Versions",SqlDbType.NVarChar),
					new SqlParameter("@AppName",SqlDbType.NVarChar),
					new SqlParameter("@IsMandatoryUpdate",SqlDbType.Int),
					new SqlParameter("@ServerUrl",SqlDbType.NVarChar),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@CreateBy",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@AppUpgradeId",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.CustomerId;
			parameters[1].Value = pEntity.Plat;
			parameters[2].Value = pEntity.IOSUpgradeUrl;
			parameters[3].Value = pEntity.AndroidUpgradeUrl;
			parameters[4].Value = pEntity.IOSUpgradeCon;
			parameters[5].Value = pEntity.AndroidUpgradeCon;
			parameters[6].Value = pEntity.Versions;
			parameters[7].Value = pEntity.AppName;
			parameters[8].Value = pEntity.IsMandatoryUpdate;
			parameters[9].Value = pEntity.ServerUrl;
			parameters[10].Value = pEntity.CreateTime;
			parameters[11].Value = pEntity.CreateBy;
			parameters[12].Value = pEntity.LastUpdateTime;
			parameters[13].Value = pEntity.LastUpdateBy;
			parameters[14].Value = pEntity.IsDelete;
			parameters[15].Value = pkGuid;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.AppUpgradeId = pkGuid;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public APPUpgradeEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [APPUpgrade] where AppUpgradeId='{0}' and IsDelete=0 ", id.ToString());
            //读取数据
            APPUpgradeEntity m = null;
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
        public APPUpgradeEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [APPUpgrade] where isdelete=0");
            //读取数据
            List<APPUpgradeEntity> list = new List<APPUpgradeEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    APPUpgradeEntity m;
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
        public void Update(APPUpgradeEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity,true,pTran);
        }
        public void Update(APPUpgradeEntity pEntity , bool pIsUpdateNullField, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.AppUpgradeId==null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
            pEntity.LastUpdateTime = DateTime.Now;
            pEntity.LastUpdateBy = CurrentUserInfo.UserID;

            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [APPUpgrade] set ");
            if (pIsUpdateNullField || pEntity.CustomerId!=null)
                strSql.Append( "[CustomerId]=@CustomerId,");
            if (pIsUpdateNullField || pEntity.Plat!=null)
                strSql.Append( "[Plat]=@Plat,");
            if (pIsUpdateNullField || pEntity.IOSUpgradeUrl!=null)
                strSql.Append( "[IOSUpgradeUrl]=@IOSUpgradeUrl,");
            if (pIsUpdateNullField || pEntity.AndroidUpgradeUrl!=null)
                strSql.Append( "[AndroidUpgradeUrl]=@AndroidUpgradeUrl,");
            if (pIsUpdateNullField || pEntity.IOSUpgradeCon!=null)
                strSql.Append( "[IOSUpgradeCon]=@IOSUpgradeCon,");
            if (pIsUpdateNullField || pEntity.AndroidUpgradeCon!=null)
                strSql.Append( "[AndroidUpgradeCon]=@AndroidUpgradeCon,");
            if (pIsUpdateNullField || pEntity.Versions!=null)
                strSql.Append( "[Versions]=@Versions,");
            if (pIsUpdateNullField || pEntity.AppName!=null)
                strSql.Append( "[AppName]=@AppName,");
            if (pIsUpdateNullField || pEntity.IsMandatoryUpdate!=null)
                strSql.Append( "[IsMandatoryUpdate]=@IsMandatoryUpdate,");
            if (pIsUpdateNullField || pEntity.ServerUrl!=null)
                strSql.Append( "[ServerUrl]=@ServerUrl,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where AppUpgradeId=@AppUpgradeId ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@CustomerId",SqlDbType.NVarChar),
					new SqlParameter("@Plat",SqlDbType.NVarChar),
					new SqlParameter("@IOSUpgradeUrl",SqlDbType.NVarChar),
					new SqlParameter("@AndroidUpgradeUrl",SqlDbType.NVarChar),
					new SqlParameter("@IOSUpgradeCon",SqlDbType.NVarChar),
					new SqlParameter("@AndroidUpgradeCon",SqlDbType.NVarChar),
					new SqlParameter("@Versions",SqlDbType.NVarChar),
					new SqlParameter("@AppName",SqlDbType.NVarChar),
					new SqlParameter("@IsMandatoryUpdate",SqlDbType.Int),
					new SqlParameter("@ServerUrl",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@AppUpgradeId",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.CustomerId;
			parameters[1].Value = pEntity.Plat;
			parameters[2].Value = pEntity.IOSUpgradeUrl;
			parameters[3].Value = pEntity.AndroidUpgradeUrl;
			parameters[4].Value = pEntity.IOSUpgradeCon;
			parameters[5].Value = pEntity.AndroidUpgradeCon;
			parameters[6].Value = pEntity.Versions;
			parameters[7].Value = pEntity.AppName;
			parameters[8].Value = pEntity.IsMandatoryUpdate;
			parameters[9].Value = pEntity.ServerUrl;
			parameters[10].Value = pEntity.LastUpdateTime;
			parameters[11].Value = pEntity.LastUpdateBy;
			parameters[12].Value = pEntity.AppUpgradeId;

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
        public void Update(APPUpgradeEntity pEntity )
        {
            Update(pEntity ,true);
        }
        public void Update(APPUpgradeEntity pEntity ,bool pIsUpdateNullField )
        {
            this.Update(pEntity, pIsUpdateNullField, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(APPUpgradeEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(APPUpgradeEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.AppUpgradeId==null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.AppUpgradeId, pTran);           
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
            sql.AppendLine("update [APPUpgrade] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where AppUpgradeId=@AppUpgradeId;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@LastUpdateTime",SqlDbType=SqlDbType.DateTime,Value=DateTime.Now},
                new SqlParameter{ParameterName="@LastUpdateBy",SqlDbType=SqlDbType.VarChar,Value=Convert.ToString(CurrentUserInfo.UserID)},
                new SqlParameter{ParameterName="@AppUpgradeId",SqlDbType=SqlDbType.UniqueIdentifier,Value=pID}
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
        public void Delete(APPUpgradeEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var item = pEntities[i];
                //参数校验
                if (item == null)
                    throw new ArgumentNullException("pEntity");
                if (item.AppUpgradeId==null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = item.AppUpgradeId;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(APPUpgradeEntity[] pEntities)
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
            sql.AppendLine("update [APPUpgrade] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy='"+CurrentUserInfo.UserID+"',IsDelete=1 where AppUpgradeId in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public APPUpgradeEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [APPUpgrade] where isdelete=0 ");
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
            List<APPUpgradeEntity> list = new List<APPUpgradeEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    APPUpgradeEntity m;
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
        public PagedQueryResult<APPUpgradeEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [AppUpgradeId] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [APPUpgrade] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [APPUpgrade] where isdelete=0 ");
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
            PagedQueryResult<APPUpgradeEntity> result = new PagedQueryResult<APPUpgradeEntity>();
            List<APPUpgradeEntity> list = new List<APPUpgradeEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    APPUpgradeEntity m;
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
        public APPUpgradeEntity[] QueryByEntity(APPUpgradeEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<APPUpgradeEntity> PagedQueryByEntity(APPUpgradeEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(APPUpgradeEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.AppUpgradeId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AppUpgradeId", Value = pQueryEntity.AppUpgradeId });
            if (pQueryEntity.CustomerId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerId", Value = pQueryEntity.CustomerId });
            if (pQueryEntity.Plat!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Plat", Value = pQueryEntity.Plat });
            if (pQueryEntity.IOSUpgradeUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IOSUpgradeUrl", Value = pQueryEntity.IOSUpgradeUrl });
            if (pQueryEntity.AndroidUpgradeUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AndroidUpgradeUrl", Value = pQueryEntity.AndroidUpgradeUrl });
            if (pQueryEntity.IOSUpgradeCon!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IOSUpgradeCon", Value = pQueryEntity.IOSUpgradeCon });
            if (pQueryEntity.AndroidUpgradeCon!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AndroidUpgradeCon", Value = pQueryEntity.AndroidUpgradeCon });
            if (pQueryEntity.Versions!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Versions", Value = pQueryEntity.Versions });
            if (pQueryEntity.AppName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "AppName", Value = pQueryEntity.AppName });
            if (pQueryEntity.IsMandatoryUpdate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsMandatoryUpdate", Value = pQueryEntity.IsMandatoryUpdate });
            if (pQueryEntity.ServerUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ServerUrl", Value = pQueryEntity.ServerUrl });
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
        protected void Load(SqlDataReader pReader, out APPUpgradeEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new APPUpgradeEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["AppUpgradeId"] != DBNull.Value)
			{
				pInstance.AppUpgradeId =  (Guid)pReader["AppUpgradeId"];
			}
			if (pReader["CustomerId"] != DBNull.Value)
			{
				pInstance.CustomerId =  Convert.ToString(pReader["CustomerId"]);
			}
			if (pReader["Plat"] != DBNull.Value)
			{
				pInstance.Plat =  Convert.ToString(pReader["Plat"]);
			}
			if (pReader["IOSUpgradeUrl"] != DBNull.Value)
			{
				pInstance.IOSUpgradeUrl =  Convert.ToString(pReader["IOSUpgradeUrl"]);
			}
			if (pReader["AndroidUpgradeUrl"] != DBNull.Value)
			{
				pInstance.AndroidUpgradeUrl =  Convert.ToString(pReader["AndroidUpgradeUrl"]);
			}
			if (pReader["IOSUpgradeCon"] != DBNull.Value)
			{
				pInstance.IOSUpgradeCon =  Convert.ToString(pReader["IOSUpgradeCon"]);
			}
			if (pReader["AndroidUpgradeCon"] != DBNull.Value)
			{
				pInstance.AndroidUpgradeCon =  Convert.ToString(pReader["AndroidUpgradeCon"]);
			}
			if (pReader["Versions"] != DBNull.Value)
			{
				pInstance.Versions =  Convert.ToString(pReader["Versions"]);
			}
			if (pReader["AppName"] != DBNull.Value)
			{
				pInstance.AppName =  Convert.ToString(pReader["AppName"]);
			}
			if (pReader["IsMandatoryUpdate"] != DBNull.Value)
			{
				pInstance.IsMandatoryUpdate =   Convert.ToInt32(pReader["IsMandatoryUpdate"]);
			}
			if (pReader["ServerUrl"] != DBNull.Value)
			{
				pInstance.ServerUrl =  Convert.ToString(pReader["ServerUrl"]);
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
