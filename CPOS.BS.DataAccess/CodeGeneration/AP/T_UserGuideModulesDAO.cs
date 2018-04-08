/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/8/24 15:41:50
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
    /// 表T_UserGuideModules的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class T_UserGuideModulesDAO : Base.BaseCPOSDAO, ICRUDable<T_UserGuideModulesEntity>, IQueryable<T_UserGuideModulesEntity>
    {
		public string StaticConnectionString { get; set; }
		private ISQLHelper staticSqlHelper;
		#region 构造函数
		/// <summary>
		/// 构造函数 
		/// </summary>
		public T_UserGuideModulesDAO(LoggingSessionInfo pUserInfo, string connectionString)
			: base(pUserInfo) {
			this.StaticConnectionString = connectionString;
			this.SQLHelper = StaticSqlHelper;
		}
		protected ISQLHelper StaticSqlHelper
		{
			get
			{
				if (null == staticSqlHelper)
					staticSqlHelper = new DefaultSQLHelper(StaticConnectionString);
				return staticSqlHelper;
			}
		}
		#endregion

		#region ICRUDable 成员
		/// <summary>
		/// 创建一个新实例
		/// </summary>
		/// <param name="pEntity">实体实例</param>
		public void Create(T_UserGuideModulesEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(T_UserGuideModulesEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [T_UserGuideModules](");
            strSql.Append("[ModuleName],[ModuleCode],[ModuleStep],[ModuleType],[VideoUrl],[ImageUrl1],[ImageUrl2],[ImageUrl3],[Url],[Field1],[Field2],[Field3],[Remark],[ParentModule],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete],[UserGuideModulesId])");
            strSql.Append(" values (");
            strSql.Append("@ModuleName,@ModuleCode,@ModuleStep,@ModuleType,@VideoUrl,@ImageUrl1,@ImageUrl2,@ImageUrl3,@Url,@Field1,@Field2,@Field3,@Remark,@ParentModule,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete,@UserGuideModulesId)");            

			Guid? pkGuid;
			if (pEntity.UserGuideModulesId == null)
				pkGuid = Guid.NewGuid();
			else
				pkGuid = pEntity.UserGuideModulesId;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@ModuleName",SqlDbType.VarChar),
					new SqlParameter("@ModuleCode",SqlDbType.VarChar),
					new SqlParameter("@ModuleStep",SqlDbType.Int),
					new SqlParameter("@ModuleType",SqlDbType.VarChar),
					new SqlParameter("@VideoUrl",SqlDbType.VarChar),
					new SqlParameter("@ImageUrl1",SqlDbType.VarChar),
					new SqlParameter("@ImageUrl2",SqlDbType.VarChar),
					new SqlParameter("@ImageUrl3",SqlDbType.VarChar),
					new SqlParameter("@Url",SqlDbType.VarChar),
					new SqlParameter("@Field1",SqlDbType.NVarChar),
					new SqlParameter("@Field2",SqlDbType.NVarChar),
					new SqlParameter("@Field3",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ParentModule",SqlDbType.UniqueIdentifier),
					new SqlParameter("@CreateBy",SqlDbType.VarChar),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@UserGuideModulesId",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.ModuleName;
			parameters[1].Value = pEntity.ModuleCode;
			parameters[2].Value = pEntity.ModuleStep;
			parameters[3].Value = pEntity.ModuleType;
			parameters[4].Value = pEntity.VideoUrl;
			parameters[5].Value = pEntity.ImageUrl1;
			parameters[6].Value = pEntity.ImageUrl2;
			parameters[7].Value = pEntity.ImageUrl3;
			parameters[8].Value = pEntity.Url;
			parameters[9].Value = pEntity.Field1;
			parameters[10].Value = pEntity.Field2;
			parameters[11].Value = pEntity.Field3;
			parameters[12].Value = pEntity.Remark;
			parameters[13].Value = pEntity.ParentModule;
			parameters[14].Value = pEntity.CreateBy;
			parameters[15].Value = pEntity.CreateTime;
			parameters[16].Value = pEntity.LastUpdateBy;
			parameters[17].Value = pEntity.LastUpdateTime;
			parameters[18].Value = pEntity.IsDelete;
			parameters[19].Value = pkGuid;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.UserGuideModulesId = pkGuid;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public T_UserGuideModulesEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_UserGuideModules] where UserGuideModulesId='{0}'  and isdelete=0 ", id.ToString());
            //读取数据
            T_UserGuideModulesEntity m = null;
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
        public T_UserGuideModulesEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_UserGuideModules] where 1=1  and isdelete=0");
            //读取数据
            List<T_UserGuideModulesEntity> list = new List<T_UserGuideModulesEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    T_UserGuideModulesEntity m;
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
        public void Update(T_UserGuideModulesEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(T_UserGuideModulesEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.UserGuideModulesId.HasValue)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
			pEntity.LastUpdateTime=DateTime.Now;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [T_UserGuideModules] set ");
                        if (pIsUpdateNullField || pEntity.ModuleName!=null)
                strSql.Append( "[ModuleName]=@ModuleName,");
            if (pIsUpdateNullField || pEntity.ModuleCode!=null)
                strSql.Append( "[ModuleCode]=@ModuleCode,");
            if (pIsUpdateNullField || pEntity.ModuleStep!=null)
                strSql.Append( "[ModuleStep]=@ModuleStep,");
            if (pIsUpdateNullField || pEntity.ModuleType!=null)
                strSql.Append( "[ModuleType]=@ModuleType,");
            if (pIsUpdateNullField || pEntity.VideoUrl!=null)
                strSql.Append( "[VideoUrl]=@VideoUrl,");
            if (pIsUpdateNullField || pEntity.ImageUrl1!=null)
                strSql.Append( "[ImageUrl1]=@ImageUrl1,");
            if (pIsUpdateNullField || pEntity.ImageUrl2!=null)
                strSql.Append( "[ImageUrl2]=@ImageUrl2,");
            if (pIsUpdateNullField || pEntity.ImageUrl3!=null)
                strSql.Append( "[ImageUrl3]=@ImageUrl3,");
            if (pIsUpdateNullField || pEntity.Url!=null)
                strSql.Append( "[Url]=@Url,");
            if (pIsUpdateNullField || pEntity.Field1!=null)
                strSql.Append( "[Field1]=@Field1,");
            if (pIsUpdateNullField || pEntity.Field2!=null)
                strSql.Append( "[Field2]=@Field2,");
            if (pIsUpdateNullField || pEntity.Field3!=null)
                strSql.Append( "[Field3]=@Field3,");
            if (pIsUpdateNullField || pEntity.Remark!=null)
                strSql.Append( "[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.ParentModule!=null)
                strSql.Append( "[ParentModule]=@ParentModule,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime");
            strSql.Append(" where UserGuideModulesId=@UserGuideModulesId ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@ModuleName",SqlDbType.VarChar),
					new SqlParameter("@ModuleCode",SqlDbType.VarChar),
					new SqlParameter("@ModuleStep",SqlDbType.Int),
					new SqlParameter("@ModuleType",SqlDbType.VarChar),
					new SqlParameter("@VideoUrl",SqlDbType.VarChar),
					new SqlParameter("@ImageUrl1",SqlDbType.VarChar),
					new SqlParameter("@ImageUrl2",SqlDbType.VarChar),
					new SqlParameter("@ImageUrl3",SqlDbType.VarChar),
					new SqlParameter("@Url",SqlDbType.VarChar),
					new SqlParameter("@Field1",SqlDbType.NVarChar),
					new SqlParameter("@Field2",SqlDbType.NVarChar),
					new SqlParameter("@Field3",SqlDbType.NVarChar),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ParentModule",SqlDbType.UniqueIdentifier),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@UserGuideModulesId",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.ModuleName;
			parameters[1].Value = pEntity.ModuleCode;
			parameters[2].Value = pEntity.ModuleStep;
			parameters[3].Value = pEntity.ModuleType;
			parameters[4].Value = pEntity.VideoUrl;
			parameters[5].Value = pEntity.ImageUrl1;
			parameters[6].Value = pEntity.ImageUrl2;
			parameters[7].Value = pEntity.ImageUrl3;
			parameters[8].Value = pEntity.Url;
			parameters[9].Value = pEntity.Field1;
			parameters[10].Value = pEntity.Field2;
			parameters[11].Value = pEntity.Field3;
			parameters[12].Value = pEntity.Remark;
			parameters[13].Value = pEntity.ParentModule;
			parameters[14].Value = pEntity.LastUpdateBy;
			parameters[15].Value = pEntity.LastUpdateTime;
			parameters[16].Value = pEntity.UserGuideModulesId;

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
        public void Update(T_UserGuideModulesEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(T_UserGuideModulesEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(T_UserGuideModulesEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.UserGuideModulesId.HasValue)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.UserGuideModulesId.Value, pTran);           
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
            sql.AppendLine("update [T_UserGuideModules] set  isdelete=1 where UserGuideModulesId=@UserGuideModulesId;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@UserGuideModulesId",SqlDbType=SqlDbType.UniqueIdentifier,Value=pID}
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
        public void Delete(T_UserGuideModulesEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (!pEntity.UserGuideModulesId.HasValue)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.UserGuideModulesId;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(T_UserGuideModulesEntity[] pEntities)
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
            sql.AppendLine("update [T_UserGuideModules] set  isdelete=1 where UserGuideModulesId in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public T_UserGuideModulesEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_UserGuideModules] where 1=1  and isdelete=0 ");
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
            List<T_UserGuideModulesEntity> list = new List<T_UserGuideModulesEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    T_UserGuideModulesEntity m;
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
        public PagedQueryResult<T_UserGuideModulesEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [UserGuideModulesId] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [T_UserGuideModules] where 1=1  and isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [T_UserGuideModules] where 1=1  and isdelete=0 ");
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
            PagedQueryResult<T_UserGuideModulesEntity> result = new PagedQueryResult<T_UserGuideModulesEntity>();
            List<T_UserGuideModulesEntity> list = new List<T_UserGuideModulesEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    T_UserGuideModulesEntity m;
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
        public T_UserGuideModulesEntity[] QueryByEntity(T_UserGuideModulesEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<T_UserGuideModulesEntity> PagedQueryByEntity(T_UserGuideModulesEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(T_UserGuideModulesEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.UserGuideModulesId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "UserGuideModulesId", Value = pQueryEntity.UserGuideModulesId });
            if (pQueryEntity.ModuleName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ModuleName", Value = pQueryEntity.ModuleName });
            if (pQueryEntity.ModuleCode!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ModuleCode", Value = pQueryEntity.ModuleCode });
            if (pQueryEntity.ModuleStep!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ModuleStep", Value = pQueryEntity.ModuleStep });
            if (pQueryEntity.ModuleType!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ModuleType", Value = pQueryEntity.ModuleType });
            if (pQueryEntity.VideoUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VideoUrl", Value = pQueryEntity.VideoUrl });
            if (pQueryEntity.ImageUrl1!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ImageUrl1", Value = pQueryEntity.ImageUrl1 });
            if (pQueryEntity.ImageUrl2!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ImageUrl2", Value = pQueryEntity.ImageUrl2 });
            if (pQueryEntity.ImageUrl3!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ImageUrl3", Value = pQueryEntity.ImageUrl3 });
            if (pQueryEntity.Url!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Url", Value = pQueryEntity.Url });
            if (pQueryEntity.Field1!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Field1", Value = pQueryEntity.Field1 });
            if (pQueryEntity.Field2!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Field2", Value = pQueryEntity.Field2 });
            if (pQueryEntity.Field3!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Field3", Value = pQueryEntity.Field3 });
            if (pQueryEntity.Remark!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
            if (pQueryEntity.ParentModule!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ParentModule", Value = pQueryEntity.ParentModule });
            if (pQueryEntity.CreateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateBy", Value = pQueryEntity.CreateBy });
            if (pQueryEntity.CreateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CreateTime", Value = pQueryEntity.CreateTime });
            if (pQueryEntity.LastUpdateBy!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateBy", Value = pQueryEntity.LastUpdateBy });
            if (pQueryEntity.LastUpdateTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "LastUpdateTime", Value = pQueryEntity.LastUpdateTime });
            if (pQueryEntity.IsDelete!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsDelete", Value = pQueryEntity.IsDelete });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(IDataReader pReader, out T_UserGuideModulesEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new T_UserGuideModulesEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["UserGuideModulesId"] != DBNull.Value)
			{
				pInstance.UserGuideModulesId =  (Guid)pReader["UserGuideModulesId"];
			}
			if (pReader["ModuleName"] != DBNull.Value)
			{
				pInstance.ModuleName =  Convert.ToString(pReader["ModuleName"]);
			}
			if (pReader["ModuleCode"] != DBNull.Value)
			{
				pInstance.ModuleCode =  Convert.ToString(pReader["ModuleCode"]);
			}
			if (pReader["ModuleStep"] != DBNull.Value)
			{
				pInstance.ModuleStep =   Convert.ToInt32(pReader["ModuleStep"]);
			}
			if (pReader["ModuleType"] != DBNull.Value)
			{
				pInstance.ModuleType =  Convert.ToString(pReader["ModuleType"]);
			}
			if (pReader["VideoUrl"] != DBNull.Value)
			{
				pInstance.VideoUrl =  Convert.ToString(pReader["VideoUrl"]);
			}
			if (pReader["ImageUrl1"] != DBNull.Value)
			{
				pInstance.ImageUrl1 =  Convert.ToString(pReader["ImageUrl1"]);
			}
			if (pReader["ImageUrl2"] != DBNull.Value)
			{
				pInstance.ImageUrl2 =  Convert.ToString(pReader["ImageUrl2"]);
			}
			if (pReader["ImageUrl3"] != DBNull.Value)
			{
				pInstance.ImageUrl3 =  Convert.ToString(pReader["ImageUrl3"]);
			}
			if (pReader["Url"] != DBNull.Value)
			{
				pInstance.Url =  Convert.ToString(pReader["Url"]);
			}
			if (pReader["Field1"] != DBNull.Value)
			{
				pInstance.Field1 =  Convert.ToString(pReader["Field1"]);
			}
			if (pReader["Field2"] != DBNull.Value)
			{
				pInstance.Field2 =  Convert.ToString(pReader["Field2"]);
			}
			if (pReader["Field3"] != DBNull.Value)
			{
				pInstance.Field3 =  Convert.ToString(pReader["Field3"]);
			}
			if (pReader["Remark"] != DBNull.Value)
			{
				pInstance.Remark =  Convert.ToString(pReader["Remark"]);
			}
			if (pReader["ParentModule"] != DBNull.Value)
			{
				pInstance.ParentModule =  (Guid)pReader["ParentModule"];
			}
			if (pReader["CreateBy"] != DBNull.Value)
			{
				pInstance.CreateBy =  Convert.ToString(pReader["CreateBy"]);
			}
			if (pReader["CreateTime"] != DBNull.Value)
			{
				pInstance.CreateTime =  Convert.ToDateTime(pReader["CreateTime"]);
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

        }
        #endregion
    }
}
