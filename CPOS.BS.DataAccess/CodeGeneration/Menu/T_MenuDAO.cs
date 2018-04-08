/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016-8-20 14:58:24
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
    /// 表T_Menu的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class T_MenuDAO : Base.BaseCPOSDAO, ICRUDable<T_MenuEntity>, IQueryable<T_MenuEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public T_MenuDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(T_MenuEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(T_MenuEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //初始化固定字段


            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [T_Menu](");
            strSql.Append("[reg_app_id],[menu_code],[parent_menu_id],[menu_level],[url_path],[icon_path],[display_index],[menu_name],[user_flag],[menu_eng_name],[status],[create_user_id],[create_time],[modify_user_id],[modify_time],[customer_id],[IsCanAccess],[menu_id])");
            strSql.Append(" values (");
            strSql.Append("@reg_app_id,@menu_code,@parent_menu_id,@menu_level,@url_path,@icon_path,@display_index,@menu_name,@user_flag,@menu_eng_name,@status,@create_user_id,@create_time,@modify_user_id,@modify_time,@customer_id,@IsCanAccess,@menu_id)");            

			string pkString = pEntity.menu_id;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@reg_app_id",SqlDbType.NVarChar),
					new SqlParameter("@menu_code",SqlDbType.NVarChar),
					new SqlParameter("@parent_menu_id",SqlDbType.NVarChar),
					new SqlParameter("@menu_level",SqlDbType.Int),
					new SqlParameter("@url_path",SqlDbType.NVarChar),
					new SqlParameter("@icon_path",SqlDbType.NVarChar),
					new SqlParameter("@display_index",SqlDbType.Int),
					new SqlParameter("@menu_name",SqlDbType.NVarChar),
					new SqlParameter("@user_flag",SqlDbType.Int),
					new SqlParameter("@menu_eng_name",SqlDbType.NVarChar),
					new SqlParameter("@status",SqlDbType.Int),
					new SqlParameter("@create_user_id",SqlDbType.NVarChar),
					new SqlParameter("@create_time",SqlDbType.NVarChar),
					new SqlParameter("@modify_user_id",SqlDbType.NVarChar),
					new SqlParameter("@modify_time",SqlDbType.NVarChar),
					new SqlParameter("@customer_id",SqlDbType.NVarChar),
					new SqlParameter("@IsCanAccess",SqlDbType.Int),
					new SqlParameter("@menu_id",SqlDbType.NVarChar)
            };
			parameters[0].Value = pEntity.reg_app_id;
			parameters[1].Value = pEntity.menu_code;
			parameters[2].Value = pEntity.parent_menu_id;
			parameters[3].Value = pEntity.menu_level;
			parameters[4].Value = pEntity.url_path;
			parameters[5].Value = pEntity.icon_path;
			parameters[6].Value = pEntity.display_index;
			parameters[7].Value = pEntity.menu_name;
			parameters[8].Value = pEntity.user_flag;
			parameters[9].Value = pEntity.menu_eng_name;
			parameters[10].Value = pEntity.status;
			parameters[11].Value = pEntity.create_user_id;
			parameters[12].Value = pEntity.create_time;
			parameters[13].Value = pEntity.modify_user_id;
			parameters[14].Value = pEntity.modify_time;
			parameters[15].Value = pEntity.customer_id;
			parameters[16].Value = pEntity.IsCanAccess;
			parameters[17].Value = pkString;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.menu_id = pkString;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public T_MenuEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Menu] where menu_id='{0}'  and status<>'-1' ", id.ToString());
            //读取数据
            T_MenuEntity m = null;
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
        public T_MenuEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Menu] where 1=1  and status<>'-1'");
            //读取数据
            List<T_MenuEntity> list = new List<T_MenuEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    T_MenuEntity m;
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
        public void Update(T_MenuEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(T_MenuEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.menu_id == null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [T_Menu] set ");
                        if (pIsUpdateNullField || pEntity.reg_app_id!=null)
                strSql.Append( "[reg_app_id]=@reg_app_id,");
            if (pIsUpdateNullField || pEntity.menu_code!=null)
                strSql.Append( "[menu_code]=@menu_code,");
            if (pIsUpdateNullField || pEntity.parent_menu_id!=null)
                strSql.Append( "[parent_menu_id]=@parent_menu_id,");
            if (pIsUpdateNullField || pEntity.menu_level!=null)
                strSql.Append( "[menu_level]=@menu_level,");
            if (pIsUpdateNullField || pEntity.url_path!=null)
                strSql.Append( "[url_path]=@url_path,");
            if (pIsUpdateNullField || pEntity.icon_path!=null)
                strSql.Append( "[icon_path]=@icon_path,");
            if (pIsUpdateNullField || pEntity.display_index!=null)
                strSql.Append( "[display_index]=@display_index,");
            if (pIsUpdateNullField || pEntity.menu_name!=null)
                strSql.Append( "[menu_name]=@menu_name,");
            if (pIsUpdateNullField || pEntity.user_flag!=null)
                strSql.Append( "[user_flag]=@user_flag,");
            if (pIsUpdateNullField || pEntity.menu_eng_name!=null)
                strSql.Append( "[menu_eng_name]=@menu_eng_name,");
            if (pIsUpdateNullField || pEntity.status!=null)
                strSql.Append( "[status]=@status,");
            if (pIsUpdateNullField || pEntity.create_user_id!=null)
                strSql.Append( "[create_user_id]=@create_user_id,");
            if (pIsUpdateNullField || pEntity.create_time!=null)
                strSql.Append( "[create_time]=@create_time,");
            if (pIsUpdateNullField || pEntity.modify_user_id!=null)
                strSql.Append( "[modify_user_id]=@modify_user_id,");
            if (pIsUpdateNullField || pEntity.modify_time!=null)
                strSql.Append( "[modify_time]=@modify_time,");
            if (pIsUpdateNullField || pEntity.customer_id!=null)
                strSql.Append( "[customer_id]=@customer_id,");
            if (pIsUpdateNullField || pEntity.IsCanAccess!=null)
                strSql.Append( "[IsCanAccess]=@IsCanAccess");
            if (strSql.ToString().EndsWith(","))
                strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(" where menu_id=@menu_id ");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@reg_app_id",SqlDbType.NVarChar),
					new SqlParameter("@menu_code",SqlDbType.NVarChar),
					new SqlParameter("@parent_menu_id",SqlDbType.NVarChar),
					new SqlParameter("@menu_level",SqlDbType.Int),
					new SqlParameter("@url_path",SqlDbType.NVarChar),
					new SqlParameter("@icon_path",SqlDbType.NVarChar),
					new SqlParameter("@display_index",SqlDbType.Int),
					new SqlParameter("@menu_name",SqlDbType.NVarChar),
					new SqlParameter("@user_flag",SqlDbType.Int),
					new SqlParameter("@menu_eng_name",SqlDbType.NVarChar),
					new SqlParameter("@status",SqlDbType.Int),
					new SqlParameter("@create_user_id",SqlDbType.NVarChar),
					new SqlParameter("@create_time",SqlDbType.NVarChar),
					new SqlParameter("@modify_user_id",SqlDbType.NVarChar),
					new SqlParameter("@modify_time",SqlDbType.NVarChar),
					new SqlParameter("@customer_id",SqlDbType.NVarChar),
					new SqlParameter("@IsCanAccess",SqlDbType.Int),
					new SqlParameter("@menu_id",SqlDbType.NVarChar)
            };
			parameters[0].Value = pEntity.reg_app_id;
			parameters[1].Value = pEntity.menu_code;
			parameters[2].Value = pEntity.parent_menu_id;
			parameters[3].Value = pEntity.menu_level;
			parameters[4].Value = pEntity.url_path;
			parameters[5].Value = pEntity.icon_path;
			parameters[6].Value = pEntity.display_index;
			parameters[7].Value = pEntity.menu_name;
			parameters[8].Value = pEntity.user_flag;
			parameters[9].Value = pEntity.menu_eng_name;
			parameters[10].Value = pEntity.status;
			parameters[11].Value = pEntity.create_user_id;
			parameters[12].Value = pEntity.create_time;
			parameters[13].Value = pEntity.modify_user_id;
			parameters[14].Value = pEntity.modify_time;
			parameters[15].Value = pEntity.customer_id;
			parameters[16].Value = pEntity.IsCanAccess;
			parameters[17].Value = pEntity.menu_id;

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
        public void Update(T_MenuEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(T_MenuEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(T_MenuEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.menu_id == null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.menu_id, pTran);           
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
            sql.AppendLine("update [T_Menu] set status='-1' where menu_id=@menu_id;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@menu_id",SqlDbType=SqlDbType.VarChar,Value=pID}
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
        public void Delete(T_MenuEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (pEntity.menu_id == null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.menu_id;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(T_MenuEntity[] pEntities)
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
            sql.AppendLine("update [T_Menu] set status='-1' where menu_id in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public T_MenuEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            //sql.AppendFormat("select * from [T_Menu] where 1=1  and status<>'-1' ");
            sql.AppendFormat("select * from [T_Menu] where 1=1 ");
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
            List<T_MenuEntity> list = new List<T_MenuEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    T_MenuEntity m;
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
        public PagedQueryResult<T_MenuEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [menu_id] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [T_Menu] where 1=1  and status<>'-1' ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [T_Menu] where 1=1  and status<>'-1' ");
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
            PagedQueryResult<T_MenuEntity> result = new PagedQueryResult<T_MenuEntity>();
            List<T_MenuEntity> list = new List<T_MenuEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    T_MenuEntity m;
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
        public T_MenuEntity[] QueryByEntity(T_MenuEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<T_MenuEntity> PagedQueryByEntity(T_MenuEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(T_MenuEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.menu_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "menu_id", Value = pQueryEntity.menu_id });
            if (pQueryEntity.reg_app_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "reg_app_id", Value = pQueryEntity.reg_app_id });
            if (pQueryEntity.menu_code!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "menu_code", Value = pQueryEntity.menu_code });
            if (pQueryEntity.parent_menu_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "parent_menu_id", Value = pQueryEntity.parent_menu_id });
            if (pQueryEntity.menu_level!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "menu_level", Value = pQueryEntity.menu_level });
            if (pQueryEntity.url_path!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "url_path", Value = pQueryEntity.url_path });
            if (pQueryEntity.icon_path!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "icon_path", Value = pQueryEntity.icon_path });
            if (pQueryEntity.display_index!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "display_index", Value = pQueryEntity.display_index });
            if (pQueryEntity.menu_name!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "menu_name", Value = pQueryEntity.menu_name });
            if (pQueryEntity.user_flag!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "user_flag", Value = pQueryEntity.user_flag });
            if (pQueryEntity.menu_eng_name!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "menu_eng_name", Value = pQueryEntity.menu_eng_name });
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
            if (pQueryEntity.customer_id!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "customer_id", Value = pQueryEntity.customer_id });
            if (pQueryEntity.IsCanAccess!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "IsCanAccess", Value = pQueryEntity.IsCanAccess });

            return lstWhereCondition.ToArray();
        }
        /// <summary>
        /// 装载实体
        /// </summary>
        /// <param name="pReader">向前只读器</param>
        /// <param name="pInstance">实体实例</param>
        protected void Load(IDataReader pReader, out T_MenuEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new T_MenuEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["menu_id"] != DBNull.Value)
			{
				pInstance.menu_id =  Convert.ToString(pReader["menu_id"]);
			}
			if (pReader["reg_app_id"] != DBNull.Value)
			{
				pInstance.reg_app_id =  Convert.ToString(pReader["reg_app_id"]);
			}
			if (pReader["menu_code"] != DBNull.Value)
			{
				pInstance.menu_code =  Convert.ToString(pReader["menu_code"]);
			}
			if (pReader["parent_menu_id"] != DBNull.Value)
			{
				pInstance.parent_menu_id =  Convert.ToString(pReader["parent_menu_id"]);
			}
			if (pReader["menu_level"] != DBNull.Value)
			{
				pInstance.menu_level =   Convert.ToInt32(pReader["menu_level"]);
			}
			if (pReader["url_path"] != DBNull.Value)
			{
				pInstance.url_path =  Convert.ToString(pReader["url_path"]);
			}
			if (pReader["icon_path"] != DBNull.Value)
			{
				pInstance.icon_path =  Convert.ToString(pReader["icon_path"]);
			}
			if (pReader["display_index"] != DBNull.Value)
			{
				pInstance.display_index =   Convert.ToInt32(pReader["display_index"]);
			}
			if (pReader["menu_name"] != DBNull.Value)
			{
				pInstance.menu_name =  Convert.ToString(pReader["menu_name"]);
			}
			if (pReader["user_flag"] != DBNull.Value)
			{
				pInstance.user_flag =   Convert.ToInt32(pReader["user_flag"]);
			}
			if (pReader["menu_eng_name"] != DBNull.Value)
			{
				pInstance.menu_eng_name =  Convert.ToString(pReader["menu_eng_name"]);
			}
			if (pReader["status"] != DBNull.Value)
			{
				pInstance.status =   Convert.ToInt32(pReader["status"]);
			}
			if (pReader["create_user_id"] != DBNull.Value)
			{
				pInstance.create_user_id =  Convert.ToString(pReader["create_user_id"]);
			}
			if (pReader["create_time"] != DBNull.Value)
			{
				pInstance.create_time =  Convert.ToString(pReader["create_time"]);
			}
			if (pReader["modify_user_id"] != DBNull.Value)
			{
				pInstance.modify_user_id =  Convert.ToString(pReader["modify_user_id"]);
			}
			if (pReader["modify_time"] != DBNull.Value)
			{
				pInstance.modify_time =  Convert.ToString(pReader["modify_time"]);
			}
			if (pReader["customer_id"] != DBNull.Value)
			{
				pInstance.customer_id =  Convert.ToString(pReader["customer_id"]);
			}
			if (pReader["IsCanAccess"] != DBNull.Value)
			{
				pInstance.IsCanAccess =   Convert.ToInt32(pReader["IsCanAccess"]);
			}

        }
        #endregion
    }
}
