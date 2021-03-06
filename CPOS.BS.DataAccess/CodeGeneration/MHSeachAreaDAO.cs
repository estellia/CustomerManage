/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/12/23 14:13:25
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
    /// 表MHSeachArea的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class MHSeachAreaDAO : Base.BaseCPOSDAO, ICRUDable<MHSeachAreaEntity>, IQueryable<MHSeachAreaEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public MHSeachAreaDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(MHSeachAreaEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(MHSeachAreaEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [MHSeachArea](");
            strSql.Append("[HomeId],[ImageUrl],[ObjectId],[ObjectTypeId],[DisplayIndex],[Url],[Status],[CreateTime],[CreateBy],[LastUpdateBy],[LastUpdateTime],[IsDelete],[titleName],[titleStyle],[styleType],[show],[MHSearchAreaID])");
            strSql.Append(" values (");
            strSql.Append("@HomeId,@ImageUrl,@ObjectId,@ObjectTypeId,@DisplayIndex,@Url,@Status,@CreateTime,@CreateBy,@LastUpdateBy,@LastUpdateTime,@IsDelete,@titleName,@titleStyle,@styleType,@show,@MHSearchAreaID)");            

			Guid? pkGuid;
			if (pEntity.MHSearchAreaID == null)
				pkGuid = Guid.NewGuid();
			else
				pkGuid = pEntity.MHSearchAreaID;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@HomeId",SqlDbType.UniqueIdentifier),
					new SqlParameter("@ImageUrl",SqlDbType.NVarChar),
					new SqlParameter("@ObjectId",SqlDbType.NVarChar),
					new SqlParameter("@ObjectTypeId",SqlDbType.Int),
					new SqlParameter("@DisplayIndex",SqlDbType.Int),
					new SqlParameter("@Url",SqlDbType.NVarChar),
					new SqlParameter("@Status",SqlDbType.Int),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@CreateBy",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@titleName",SqlDbType.NVarChar),
					new SqlParameter("@titleStyle",SqlDbType.NVarChar),
					new SqlParameter("@styleType",SqlDbType.NVarChar),
					new SqlParameter("@show",SqlDbType.NVarChar),
					new SqlParameter("@MHSearchAreaID",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.HomeId;
			parameters[1].Value = pEntity.ImageUrl;
			parameters[2].Value = pEntity.ObjectId;
			parameters[3].Value = pEntity.ObjectTypeId;
			parameters[4].Value = pEntity.DisplayIndex;
			parameters[5].Value = pEntity.Url;
			parameters[6].Value = pEntity.Status;
			parameters[7].Value = pEntity.CreateTime;
			parameters[8].Value = pEntity.CreateBy;
			parameters[9].Value = pEntity.LastUpdateBy;
			parameters[10].Value = pEntity.LastUpdateTime;
			parameters[11].Value = pEntity.IsDelete;
			parameters[12].Value = pEntity.titleName;
			parameters[13].Value = pEntity.titleStyle;
			parameters[14].Value = pEntity.styleType;
			parameters[15].Value = pEntity.show;
			parameters[16].Value = pkGuid;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.MHSearchAreaID = pkGuid;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public MHSeachAreaEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [MHSeachArea] where MHSearchAreaID='{0}'  and isdelete=0 ", id.ToString());
            //读取数据
            MHSeachAreaEntity m = null;
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
        public MHSeachAreaEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [MHSeachArea] where 1=1  and isdelete=0");
            //读取数据
            List<MHSeachAreaEntity> list = new List<MHSeachAreaEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    MHSeachAreaEntity m;
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
        public void Update(MHSeachAreaEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(MHSeachAreaEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.MHSearchAreaID.HasValue)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
			pEntity.LastUpdateTime=DateTime.Now;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [MHSeachArea] set ");
                        if (pIsUpdateNullField || pEntity.HomeId!=null)
                strSql.Append( "[HomeId]=@HomeId,");
            if (pIsUpdateNullField || pEntity.ImageUrl!=null)
                strSql.Append( "[ImageUrl]=@ImageUrl,");
            if (pIsUpdateNullField || pEntity.ObjectId!=null)
                strSql.Append( "[ObjectId]=@ObjectId,");
            if (pIsUpdateNullField || pEntity.ObjectTypeId!=null)
                strSql.Append( "[ObjectTypeId]=@ObjectTypeId,");
            if (pIsUpdateNullField || pEntity.DisplayIndex!=null)
                strSql.Append( "[DisplayIndex]=@DisplayIndex,");
            if (pIsUpdateNullField || pEntity.Url!=null)
                strSql.Append( "[Url]=@Url,");
            if (pIsUpdateNullField || pEntity.Status!=null)
                strSql.Append( "[Status]=@Status,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime,");
            if (pIsUpdateNullField || pEntity.titleName!=null)
                strSql.Append( "[titleName]=@titleName,");
            if (pIsUpdateNullField || pEntity.titleStyle!=null)
                strSql.Append( "[titleStyle]=@titleStyle,");
            if (pIsUpdateNullField || pEntity.styleType!=null)
                strSql.Append( "[styleType]=@styleType,");
            if (pIsUpdateNullField || pEntity.show!=null)
                strSql.Append( "[show]=@show");
            strSql.Append(" where MHSearchAreaID=@MHSearchAreaID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@HomeId",SqlDbType.UniqueIdentifier),
					new SqlParameter("@ImageUrl",SqlDbType.NVarChar),
					new SqlParameter("@ObjectId",SqlDbType.NVarChar),
					new SqlParameter("@ObjectTypeId",SqlDbType.Int),
					new SqlParameter("@DisplayIndex",SqlDbType.Int),
					new SqlParameter("@Url",SqlDbType.NVarChar),
					new SqlParameter("@Status",SqlDbType.Int),
					new SqlParameter("@LastUpdateBy",SqlDbType.NVarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@titleName",SqlDbType.NVarChar),
					new SqlParameter("@titleStyle",SqlDbType.NVarChar),
					new SqlParameter("@styleType",SqlDbType.NVarChar),
					new SqlParameter("@show",SqlDbType.NVarChar),
					new SqlParameter("@MHSearchAreaID",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.HomeId;
			parameters[1].Value = pEntity.ImageUrl;
			parameters[2].Value = pEntity.ObjectId;
			parameters[3].Value = pEntity.ObjectTypeId;
			parameters[4].Value = pEntity.DisplayIndex;
			parameters[5].Value = pEntity.Url;
			parameters[6].Value = pEntity.Status;
			parameters[7].Value = pEntity.LastUpdateBy;
			parameters[8].Value = pEntity.LastUpdateTime;
			parameters[9].Value = pEntity.titleName;
			parameters[10].Value = pEntity.titleStyle;
			parameters[11].Value = pEntity.styleType;
			parameters[12].Value = pEntity.show;
			parameters[13].Value = pEntity.MHSearchAreaID;

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
        public void Update(MHSeachAreaEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(MHSeachAreaEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(MHSeachAreaEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.MHSearchAreaID.HasValue)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.MHSearchAreaID.Value, pTran);           
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
            sql.AppendLine("update [MHSeachArea] set  isdelete=1 where MHSearchAreaID=@MHSearchAreaID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@MHSearchAreaID",SqlDbType=SqlDbType.UniqueIdentifier,Value=pID}
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
        public void Delete(MHSeachAreaEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (!pEntity.MHSearchAreaID.HasValue)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.MHSearchAreaID;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(MHSeachAreaEntity[] pEntities)
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
            sql.AppendLine("update [MHSeachArea] set  isdelete=1 where MHSearchAreaID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public MHSeachAreaEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [MHSeachArea] where 1=1  and isdelete=0 ");
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
            List<MHSeachAreaEntity> list = new List<MHSeachAreaEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    MHSeachAreaEntity m;
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
        public PagedQueryResult<MHSeachAreaEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [MHSearchAreaID] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [MHSeachArea] where 1=1  and isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [MHSeachArea] where 1=1  and isdelete=0 ");
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
            PagedQueryResult<MHSeachAreaEntity> result = new PagedQueryResult<MHSeachAreaEntity>();
            List<MHSeachAreaEntity> list = new List<MHSeachAreaEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    MHSeachAreaEntity m;
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
        public MHSeachAreaEntity[] QueryByEntity(MHSeachAreaEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<MHSeachAreaEntity> PagedQueryByEntity(MHSeachAreaEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(MHSeachAreaEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.MHSearchAreaID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "MHSearchAreaID", Value = pQueryEntity.MHSearchAreaID });
            if (pQueryEntity.HomeId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "HomeId", Value = pQueryEntity.HomeId });
            if (pQueryEntity.ImageUrl!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ImageUrl", Value = pQueryEntity.ImageUrl });
            if (pQueryEntity.ObjectId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ObjectId", Value = pQueryEntity.ObjectId });
            if (pQueryEntity.ObjectTypeId!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ObjectTypeId", Value = pQueryEntity.ObjectTypeId });
            if (pQueryEntity.DisplayIndex!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "DisplayIndex", Value = pQueryEntity.DisplayIndex });
            if (pQueryEntity.Url!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Url", Value = pQueryEntity.Url });
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
            if (pQueryEntity.IsDelete!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsDelete", Value = pQueryEntity.IsDelete });
            if (pQueryEntity.titleName!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "titleName", Value = pQueryEntity.titleName });
            if (pQueryEntity.titleStyle!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "titleStyle", Value = pQueryEntity.titleStyle });
            if (pQueryEntity.styleType!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "styleType", Value = pQueryEntity.styleType });
            if (pQueryEntity.show!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "show", Value = pQueryEntity.show });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(IDataReader pReader, out MHSeachAreaEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new MHSeachAreaEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["MHSearchAreaID"] != DBNull.Value)
			{
				pInstance.MHSearchAreaID =  (Guid)pReader["MHSearchAreaID"];
			}
			if (pReader["HomeId"] != DBNull.Value)
			{
				pInstance.HomeId =  (Guid)pReader["HomeId"];
			}
			if (pReader["ImageUrl"] != DBNull.Value)
			{
				pInstance.ImageUrl =  Convert.ToString(pReader["ImageUrl"]);
			}
			if (pReader["ObjectId"] != DBNull.Value)
			{
				pInstance.ObjectId =  Convert.ToString(pReader["ObjectId"]);
			}
			if (pReader["ObjectTypeId"] != DBNull.Value)
			{
				pInstance.ObjectTypeId =   Convert.ToInt32(pReader["ObjectTypeId"]);
			}
			if (pReader["DisplayIndex"] != DBNull.Value)
			{
				pInstance.DisplayIndex =   Convert.ToInt32(pReader["DisplayIndex"]);
			}
			if (pReader["Url"] != DBNull.Value)
			{
				pInstance.Url =  Convert.ToString(pReader["Url"]);
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
			if (pReader["IsDelete"] != DBNull.Value)
			{
				pInstance.IsDelete =   Convert.ToInt32(pReader["IsDelete"]);
			}
			if (pReader["titleName"] != DBNull.Value)
			{
				pInstance.titleName =  Convert.ToString(pReader["titleName"]);
			}
			if (pReader["titleStyle"] != DBNull.Value)
			{
				pInstance.titleStyle =  Convert.ToString(pReader["titleStyle"]);
			}
			if (pReader["styleType"] != DBNull.Value)
			{
				pInstance.styleType =  Convert.ToString(pReader["styleType"]);
			}
			if (pReader["show"] != DBNull.Value)
			{
				pInstance.show =  Convert.ToString(pReader["show"]);
			}

        }
        #endregion
    }
}
