/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/7/18 15:41:41
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
    /// 数据访问： 拜访计划 
    /// 表VisitingCalendar的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class VisitingCalendarDAO : Base.BaseCPOSDAO, ICRUDable<VisitingCalendarEntity>, IQueryable<VisitingCalendarEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public VisitingCalendarDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(VisitingCalendarEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(VisitingCalendarEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [VisitingCalendar](");
            strSql.Append("[EnterpriseMemberID],[EnterpriseMemberEmployeeID],[EnterpriseMemberStructureID],[PlanningTime],[ActuallyDate],[VisitingTaskDataID],[ClientUserID],[Sequence],[Status],[Remark],[ClientID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete],[VisitingCalendarID])");
            strSql.Append(" values (");
            strSql.Append("@EnterpriseMemberID,@EnterpriseMemberEmployeeID,@EnterpriseMemberStructureID,@PlanningTime,@ActuallyDate,@VisitingTaskDataID,@ClientUserID,@Sequence,@Status,@Remark,@ClientID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete,@VisitingCalendarID)");            

			Guid? pkGuid;
			if (pEntity.VisitingCalendarID == null)
				pkGuid = Guid.NewGuid();
			else
				pkGuid = pEntity.VisitingCalendarID;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@EnterpriseMemberID",SqlDbType.UniqueIdentifier),
					new SqlParameter("@EnterpriseMemberEmployeeID",SqlDbType.UniqueIdentifier),
					new SqlParameter("@EnterpriseMemberStructureID",SqlDbType.UniqueIdentifier),
					new SqlParameter("@PlanningTime",SqlDbType.DateTime),
					new SqlParameter("@ActuallyDate",SqlDbType.DateTime),
					new SqlParameter("@VisitingTaskDataID",SqlDbType.UniqueIdentifier),
					new SqlParameter("@ClientUserID",SqlDbType.VarChar),
					new SqlParameter("@Sequence",SqlDbType.Int),
					new SqlParameter("@Status",SqlDbType.Int),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.VarChar),
					new SqlParameter("@CreateBy",SqlDbType.VarChar),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@VisitingCalendarID",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.EnterpriseMemberID;
			parameters[1].Value = pEntity.EnterpriseMemberEmployeeID;
			parameters[2].Value = pEntity.EnterpriseMemberStructureID;
			parameters[3].Value = pEntity.PlanningTime;
			parameters[4].Value = pEntity.ActuallyDate;
			parameters[5].Value = pEntity.VisitingTaskDataID;
			parameters[6].Value = pEntity.ClientUserID;
			parameters[7].Value = pEntity.Sequence;
			parameters[8].Value = pEntity.Status;
			parameters[9].Value = pEntity.Remark;
			parameters[10].Value = pEntity.ClientID;
			parameters[11].Value = pEntity.CreateBy;
			parameters[12].Value = pEntity.CreateTime;
			parameters[13].Value = pEntity.LastUpdateBy;
			parameters[14].Value = pEntity.LastUpdateTime;
			parameters[15].Value = pEntity.IsDelete;
			parameters[16].Value = pkGuid;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.VisitingCalendarID = pkGuid;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public VisitingCalendarEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VisitingCalendar] where VisitingCalendarID='{0}' and IsDelete=0 ", id.ToString());
            //读取数据
            VisitingCalendarEntity m = null;
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
        public VisitingCalendarEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VisitingCalendar] where isdelete=0");
            //读取数据
            List<VisitingCalendarEntity> list = new List<VisitingCalendarEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    VisitingCalendarEntity m;
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
        public void Update(VisitingCalendarEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity,true,pTran);
        }
        public void Update(VisitingCalendarEntity pEntity , bool pIsUpdateNullField, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.VisitingCalendarID==null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
            pEntity.LastUpdateTime = DateTime.Now;
            pEntity.LastUpdateBy = CurrentUserInfo.UserID;

            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [VisitingCalendar] set ");
            if (pIsUpdateNullField || pEntity.EnterpriseMemberID!=null)
                strSql.Append( "[EnterpriseMemberID]=@EnterpriseMemberID,");
            if (pIsUpdateNullField || pEntity.EnterpriseMemberEmployeeID!=null)
                strSql.Append( "[EnterpriseMemberEmployeeID]=@EnterpriseMemberEmployeeID,");
            if (pIsUpdateNullField || pEntity.EnterpriseMemberStructureID!=null)
                strSql.Append( "[EnterpriseMemberStructureID]=@EnterpriseMemberStructureID,");
            if (pIsUpdateNullField || pEntity.PlanningTime!=null)
                strSql.Append( "[PlanningTime]=@PlanningTime,");
            if (pIsUpdateNullField || pEntity.ActuallyDate!=null)
                strSql.Append( "[ActuallyDate]=@ActuallyDate,");
            if (pIsUpdateNullField || pEntity.VisitingTaskDataID!=null)
                strSql.Append( "[VisitingTaskDataID]=@VisitingTaskDataID,");
            if (pIsUpdateNullField || pEntity.ClientUserID!=null)
                strSql.Append( "[ClientUserID]=@ClientUserID,");
            if (pIsUpdateNullField || pEntity.Sequence!=null)
                strSql.Append( "[Sequence]=@Sequence,");
            if (pIsUpdateNullField || pEntity.Status!=null)
                strSql.Append( "[Status]=@Status,");
            if (pIsUpdateNullField || pEntity.Remark!=null)
                strSql.Append( "[Remark]=@Remark,");
            if (pIsUpdateNullField || pEntity.ClientID!=null)
                strSql.Append( "[ClientID]=@ClientID,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where VisitingCalendarID=@VisitingCalendarID ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@EnterpriseMemberID",SqlDbType.UniqueIdentifier),
					new SqlParameter("@EnterpriseMemberEmployeeID",SqlDbType.UniqueIdentifier),
					new SqlParameter("@EnterpriseMemberStructureID",SqlDbType.UniqueIdentifier),
					new SqlParameter("@PlanningTime",SqlDbType.DateTime),
					new SqlParameter("@ActuallyDate",SqlDbType.DateTime),
					new SqlParameter("@VisitingTaskDataID",SqlDbType.UniqueIdentifier),
					new SqlParameter("@ClientUserID",SqlDbType.VarChar),
					new SqlParameter("@Sequence",SqlDbType.Int),
					new SqlParameter("@Status",SqlDbType.Int),
					new SqlParameter("@Remark",SqlDbType.NVarChar),
					new SqlParameter("@ClientID",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@VisitingCalendarID",SqlDbType.UniqueIdentifier)
            };
			parameters[0].Value = pEntity.EnterpriseMemberID;
			parameters[1].Value = pEntity.EnterpriseMemberEmployeeID;
			parameters[2].Value = pEntity.EnterpriseMemberStructureID;
			parameters[3].Value = pEntity.PlanningTime;
			parameters[4].Value = pEntity.ActuallyDate;
			parameters[5].Value = pEntity.VisitingTaskDataID;
			parameters[6].Value = pEntity.ClientUserID;
			parameters[7].Value = pEntity.Sequence;
			parameters[8].Value = pEntity.Status;
			parameters[9].Value = pEntity.Remark;
			parameters[10].Value = pEntity.ClientID;
			parameters[11].Value = pEntity.LastUpdateBy;
			parameters[12].Value = pEntity.LastUpdateTime;
			parameters[13].Value = pEntity.VisitingCalendarID;

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
        public void Update(VisitingCalendarEntity pEntity )
        {
            Update(pEntity ,true);
        }
        public void Update(VisitingCalendarEntity pEntity ,bool pIsUpdateNullField )
        {
            this.Update(pEntity, pIsUpdateNullField, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(VisitingCalendarEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(VisitingCalendarEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.VisitingCalendarID==null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.VisitingCalendarID, pTran);           
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
            sql.AppendLine("update [VisitingCalendar] set LastUpdateTime=@LastUpdateTime,LastUpdateBy=@LastUpdateBy,IsDelete=1 where VisitingCalendarID=@VisitingCalendarID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@LastUpdateTime",SqlDbType=SqlDbType.DateTime,Value=DateTime.Now},
                new SqlParameter{ParameterName="@LastUpdateBy",SqlDbType=SqlDbType.VarChar,Value=CurrentUserInfo.UserID},
                new SqlParameter{ParameterName="@VisitingCalendarID",SqlDbType=SqlDbType.UniqueIdentifier,Value=pID}
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
        public void Delete(VisitingCalendarEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var item = pEntities[i];
                //参数校验
                if (item == null)
                    throw new ArgumentNullException("pEntity");
                if (item.VisitingCalendarID==null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = item.VisitingCalendarID;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(VisitingCalendarEntity[] pEntities)
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
            sql.AppendLine("update [VisitingCalendar] set LastUpdateTime='"+DateTime.Now.ToString()+"',LastUpdateBy='"+CurrentUserInfo.UserID+"',IsDelete=1 where VisitingCalendarID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public VisitingCalendarEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VisitingCalendar] where isdelete=0 ");
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
            List<VisitingCalendarEntity> list = new List<VisitingCalendarEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    VisitingCalendarEntity m;
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
        public PagedQueryResult<VisitingCalendarEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [VisitingCalendarID] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [VisitingCalendar] where isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [VisitingCalendar] where isdelete=0 ");
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
            PagedQueryResult<VisitingCalendarEntity> result = new PagedQueryResult<VisitingCalendarEntity>();
            List<VisitingCalendarEntity> list = new List<VisitingCalendarEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    VisitingCalendarEntity m;
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
        public VisitingCalendarEntity[] QueryByEntity(VisitingCalendarEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<VisitingCalendarEntity> PagedQueryByEntity(VisitingCalendarEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(VisitingCalendarEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.VisitingCalendarID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VisitingCalendarID", Value = pQueryEntity.VisitingCalendarID });
            if (pQueryEntity.EnterpriseMemberID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EnterpriseMemberID", Value = pQueryEntity.EnterpriseMemberID });
            if (pQueryEntity.EnterpriseMemberEmployeeID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EnterpriseMemberEmployeeID", Value = pQueryEntity.EnterpriseMemberEmployeeID });
            if (pQueryEntity.EnterpriseMemberStructureID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EnterpriseMemberStructureID", Value = pQueryEntity.EnterpriseMemberStructureID });
            if (pQueryEntity.PlanningTime!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "PlanningTime", Value = pQueryEntity.PlanningTime });
            if (pQueryEntity.ActuallyDate!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ActuallyDate", Value = pQueryEntity.ActuallyDate });
            if (pQueryEntity.VisitingTaskDataID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VisitingTaskDataID", Value = pQueryEntity.VisitingTaskDataID });
            if (pQueryEntity.ClientUserID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ClientUserID", Value = pQueryEntity.ClientUserID });
            if (pQueryEntity.Sequence!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Sequence", Value = pQueryEntity.Sequence });
            if (pQueryEntity.Status!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Status", Value = pQueryEntity.Status });
            if (pQueryEntity.Remark!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "Remark", Value = pQueryEntity.Remark });
            if (pQueryEntity.ClientID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ClientID", Value = pQueryEntity.ClientID });
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
        protected void Load(SqlDataReader pReader, out VisitingCalendarEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new VisitingCalendarEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["VisitingCalendarID"] != DBNull.Value)
			{
				pInstance.VisitingCalendarID =  (Guid)pReader["VisitingCalendarID"];
			}
			if (pReader["EnterpriseMemberID"] != DBNull.Value)
			{
				pInstance.EnterpriseMemberID =  (Guid)pReader["EnterpriseMemberID"];
			}
			if (pReader["EnterpriseMemberEmployeeID"] != DBNull.Value)
			{
				pInstance.EnterpriseMemberEmployeeID =  (Guid)pReader["EnterpriseMemberEmployeeID"];
			}
			if (pReader["EnterpriseMemberStructureID"] != DBNull.Value)
			{
				pInstance.EnterpriseMemberStructureID =  (Guid)pReader["EnterpriseMemberStructureID"];
			}
			if (pReader["PlanningTime"] != DBNull.Value)
			{
				pInstance.PlanningTime =  Convert.ToDateTime(pReader["PlanningTime"]);
			}
			if (pReader["ActuallyDate"] != DBNull.Value)
			{
				pInstance.ActuallyDate =  Convert.ToDateTime(pReader["ActuallyDate"]);
			}
			if (pReader["VisitingTaskDataID"] != DBNull.Value)
			{
				pInstance.VisitingTaskDataID =  (Guid)pReader["VisitingTaskDataID"];
			}
			if (pReader["ClientUserID"] != DBNull.Value)
			{
				pInstance.ClientUserID =  Convert.ToString(pReader["ClientUserID"]);
			}
			if (pReader["Sequence"] != DBNull.Value)
			{
				pInstance.Sequence =   Convert.ToInt32(pReader["Sequence"]);
			}
			if (pReader["Status"] != DBNull.Value)
			{
				pInstance.Status =   Convert.ToInt32(pReader["Status"]);
			}
			if (pReader["Remark"] != DBNull.Value)
			{
				pInstance.Remark =  Convert.ToString(pReader["Remark"]);
			}
			if (pReader["ClientID"] != DBNull.Value)
			{
				pInstance.ClientID =  Convert.ToString(pReader["ClientID"]);
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
