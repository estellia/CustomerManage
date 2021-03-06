/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015/10/30 15:25:27
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
    /// 表VipIntegral的数据访问类     
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class VipIntegralDAO : Base.BaseCPOSDAO, ICRUDable<VipIntegralEntity>, IQueryable<VipIntegralEntity>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public VipIntegralDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(VipIntegralEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(VipIntegralEntity pEntity, IDbTransaction pTran)
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
            strSql.Append("insert into [VipIntegral](");
            strSql.Append("[VipCardCode],[BeginIntegral],[InIntegral],[OutIntegral],[EndIntegral],[ImminentInvalidIntegral],[InvalidIntegral],[ValidIntegral],[CumulativeIntegral],[ValidNotIntegral],[CustomerID],[CreateBy],[CreateTime],[LastUpdateBy],[LastUpdateTime],[IsDelete],[VipID])");
            strSql.Append(" values (");
            strSql.Append("@VipCardCode,@BeginIntegral,@InIntegral,@OutIntegral,@EndIntegral,@ImminentInvalidIntegral,@InvalidIntegral,@ValidIntegral,@CumulativeIntegral,@ValidNotIntegral,@CustomerID,@CreateBy,@CreateTime,@LastUpdateBy,@LastUpdateTime,@IsDelete,@VipID)");            

			string pkString = pEntity.VipID;

            SqlParameter[] parameters = 
            {
					new SqlParameter("@VipCardCode",SqlDbType.VarChar),
					new SqlParameter("@BeginIntegral",SqlDbType.Int),
					new SqlParameter("@InIntegral",SqlDbType.Int),
					new SqlParameter("@OutIntegral",SqlDbType.Int),
					new SqlParameter("@EndIntegral",SqlDbType.Int),
					new SqlParameter("@ImminentInvalidIntegral",SqlDbType.Int),
					new SqlParameter("@InvalidIntegral",SqlDbType.Int),
					new SqlParameter("@ValidIntegral",SqlDbType.Int),
					new SqlParameter("@CumulativeIntegral",SqlDbType.Int),
					new SqlParameter("@ValidNotIntegral",SqlDbType.Int),
					new SqlParameter("@CustomerID",SqlDbType.VarChar),
					new SqlParameter("@CreateBy",SqlDbType.VarChar),
					new SqlParameter("@CreateTime",SqlDbType.DateTime),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@IsDelete",SqlDbType.Int),
					new SqlParameter("@VipID",SqlDbType.VarChar)
            };
			parameters[0].Value = pEntity.VipCardCode;
			parameters[1].Value = pEntity.BeginIntegral;
			parameters[2].Value = pEntity.InIntegral;
			parameters[3].Value = pEntity.OutIntegral;
			parameters[4].Value = pEntity.EndIntegral;
			parameters[5].Value = pEntity.ImminentInvalidIntegral;
			parameters[6].Value = pEntity.InvalidIntegral;
			parameters[7].Value = pEntity.ValidIntegral;
			parameters[8].Value = pEntity.CumulativeIntegral;
			parameters[9].Value = pEntity.ValidNotIntegral;
			parameters[10].Value = pEntity.CustomerID;
			parameters[11].Value = pEntity.CreateBy;
			parameters[12].Value = pEntity.CreateTime;
			parameters[13].Value = pEntity.LastUpdateBy;
			parameters[14].Value = pEntity.LastUpdateTime;
			parameters[15].Value = pEntity.IsDelete;
			parameters[16].Value = pkString;

            //执行并将结果回写
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.VipID = pkString;
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public VipIntegralEntity GetByID(object pID)
        {
            //参数检查
            if (pID == null)
                return null;
            string id = pID.ToString();
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipIntegral] where VipID='{0}'  and isdelete=0 ", id.ToString());
            //读取数据
            VipIntegralEntity m = null;
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
        public VipIntegralEntity[] GetAll()
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipIntegral] where 1=1  and isdelete=0");
            //读取数据
            List<VipIntegralEntity> list = new List<VipIntegralEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    VipIntegralEntity m;
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
        public void Update(VipIntegralEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(VipIntegralEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.VipID == null)
            {
                throw new ArgumentException("执行更新时,实体的主键属性值不能为null.");
            }
             //初始化固定字段
			pEntity.LastUpdateTime=DateTime.Now;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            //组织参数化SQL
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [VipIntegral] set ");
                        if (pIsUpdateNullField || pEntity.VipCardCode!=null)
                strSql.Append( "[VipCardCode]=@VipCardCode,");
            if (pIsUpdateNullField || pEntity.BeginIntegral!=null)
                strSql.Append( "[BeginIntegral]=@BeginIntegral,");
            if (pIsUpdateNullField || pEntity.InIntegral!=null)
                strSql.Append( "[InIntegral]=@InIntegral,");
            if (pIsUpdateNullField || pEntity.OutIntegral!=null)
                strSql.Append( "[OutIntegral]=@OutIntegral,");
            if (pIsUpdateNullField || pEntity.EndIntegral!=null)
                strSql.Append( "[EndIntegral]=@EndIntegral,");
            if (pIsUpdateNullField || pEntity.ImminentInvalidIntegral!=null)
                strSql.Append( "[ImminentInvalidIntegral]=@ImminentInvalidIntegral,");
            if (pIsUpdateNullField || pEntity.InvalidIntegral!=null)
                strSql.Append( "[InvalidIntegral]=@InvalidIntegral,");
            if (pIsUpdateNullField || pEntity.ValidIntegral!=null)
                strSql.Append( "[ValidIntegral]=@ValidIntegral,");
            if (pIsUpdateNullField || pEntity.CumulativeIntegral!=null)
                strSql.Append( "[CumulativeIntegral]=@CumulativeIntegral,");
            if (pIsUpdateNullField || pEntity.ValidNotIntegral!=null)
                strSql.Append( "[ValidNotIntegral]=@ValidNotIntegral,");
            if (pIsUpdateNullField || pEntity.CustomerID!=null)
                strSql.Append( "[CustomerID]=@CustomerID,");
            if (pIsUpdateNullField || pEntity.LastUpdateBy!=null)
                strSql.Append( "[LastUpdateBy]=@LastUpdateBy,");
            if (pIsUpdateNullField || pEntity.LastUpdateTime!=null)
                strSql.Append( "[LastUpdateTime]=@LastUpdateTime");
            strSql.Append(" where VipID=@VipID and VipCardCode=@VipCardCode2");
            SqlParameter[] parameters = 
            {
					new SqlParameter("@VipCardCode",SqlDbType.VarChar),
					new SqlParameter("@BeginIntegral",SqlDbType.Int),
					new SqlParameter("@InIntegral",SqlDbType.Int),
					new SqlParameter("@OutIntegral",SqlDbType.Int),
					new SqlParameter("@EndIntegral",SqlDbType.Int),
					new SqlParameter("@ImminentInvalidIntegral",SqlDbType.Int),
					new SqlParameter("@InvalidIntegral",SqlDbType.Int),
					new SqlParameter("@ValidIntegral",SqlDbType.Int),
					new SqlParameter("@CumulativeIntegral",SqlDbType.Int),
					new SqlParameter("@ValidNotIntegral",SqlDbType.Int),
					new SqlParameter("@CustomerID",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateBy",SqlDbType.VarChar),
					new SqlParameter("@LastUpdateTime",SqlDbType.DateTime),
					new SqlParameter("@VipID",SqlDbType.VarChar),
                    new SqlParameter("@VipCardCode2",SqlDbType.VarChar),
            };
			parameters[0].Value = pEntity.VipCardCode;
			parameters[1].Value = pEntity.BeginIntegral;
			parameters[2].Value = pEntity.InIntegral;
			parameters[3].Value = pEntity.OutIntegral;
			parameters[4].Value = pEntity.EndIntegral;
			parameters[5].Value = pEntity.ImminentInvalidIntegral;
			parameters[6].Value = pEntity.InvalidIntegral;
			parameters[7].Value = pEntity.ValidIntegral;
			parameters[8].Value = pEntity.CumulativeIntegral;
			parameters[9].Value = pEntity.ValidNotIntegral;
			parameters[10].Value = pEntity.CustomerID;
			parameters[11].Value = pEntity.LastUpdateBy;
			parameters[12].Value = pEntity.LastUpdateTime;
			parameters[13].Value = pEntity.VipID;
            parameters[14].Value = pEntity.VipCardCode;

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
        public void Update(VipIntegralEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(VipIntegralEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(VipIntegralEntity pEntity, IDbTransaction pTran)
        {
            //参数校验
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.VipID == null)
            {
                throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
            }
            //执行 
            this.Delete(pEntity.VipID, pTran);           
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
            sql.AppendLine("update [VipIntegral] set  isdelete=1 where VipID=@VipID;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@VipID",SqlDbType=SqlDbType.VarChar,Value=pID}
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
        public void Delete(VipIntegralEntity[] pEntities, IDbTransaction pTran)
        {
            //整理主键值
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //参数校验
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (pEntity.VipID == null)
                {
                    throw new ArgumentException("执行删除时,实体的主键属性值不能为null.");
                }
                entityIDs[i] = pEntity.VipID;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(VipIntegralEntity[] pEntities)
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
            sql.AppendLine("update [VipIntegral] set  isdelete=1 where VipID in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public VipIntegralEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //组织SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [VipIntegral] where 1=1  and isdelete=0 ");
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
            List<VipIntegralEntity> list = new List<VipIntegralEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    VipIntegralEntity m;
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
        public PagedQueryResult<VipIntegralEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [VipID] desc"); //默认为主键值倒序
            }
            pagedSql.AppendFormat(") as ___rn,* from [VipIntegral] where 1=1  and isdelete=0 ");
            //总记录数SQL
            totalCountSql.AppendFormat("select count(1) from [VipIntegral] where 1=1  and isdelete=0 ");
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
            PagedQueryResult<VipIntegralEntity> result = new PagedQueryResult<VipIntegralEntity>();
            List<VipIntegralEntity> list = new List<VipIntegralEntity>();
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(pagedSql.ToString()))
            {
                while (rdr.Read())
                {
                    VipIntegralEntity m;
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
        public VipIntegralEntity[] QueryByEntity(VipIntegralEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<VipIntegralEntity> PagedQueryByEntity(VipIntegralEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(VipIntegralEntity pQueryEntity)
        { 
            //获取非空属性数量
            List<EqualsCondition> lstWhereCondition = new List<EqualsCondition>();
            if (pQueryEntity.VipID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VipID", Value = pQueryEntity.VipID });
            if (pQueryEntity.VipCardCode!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "VipCardCode", Value = pQueryEntity.VipCardCode });
            if (pQueryEntity.BeginIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "BeginIntegral", Value = pQueryEntity.BeginIntegral });
            if (pQueryEntity.InIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "InIntegral", Value = pQueryEntity.InIntegral });
            if (pQueryEntity.OutIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "OutIntegral", Value = pQueryEntity.OutIntegral });
            if (pQueryEntity.EndIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "EndIntegral", Value = pQueryEntity.EndIntegral });
            if (pQueryEntity.ImminentInvalidIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ImminentInvalidIntegral", Value = pQueryEntity.ImminentInvalidIntegral });
            if (pQueryEntity.InvalidIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "InvalidIntegral", Value = pQueryEntity.InvalidIntegral });
            if (pQueryEntity.ValidIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ValidIntegral", Value = pQueryEntity.ValidIntegral });
            if (pQueryEntity.CumulativeIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CumulativeIntegral", Value = pQueryEntity.CumulativeIntegral });
            if (pQueryEntity.ValidNotIntegral!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "ValidNotIntegral", Value = pQueryEntity.ValidNotIntegral });
            if (pQueryEntity.CustomerID!=null)
                lstWhereCondition.Add(new EqualsCondition() { FieldName = "CustomerID", Value = pQueryEntity.CustomerID });
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
        protected void Load(IDataReader pReader, out VipIntegralEntity pInstance)
        {
            //将所有的数据从SqlDataReader中读取到Entity中
            pInstance = new VipIntegralEntity();
            pInstance.PersistenceHandle = new PersistenceHandle();
            pInstance.PersistenceHandle.Load();

			if (pReader["VipID"] != DBNull.Value)
			{
				pInstance.VipID =  Convert.ToString(pReader["VipID"]);
			}
			if (pReader["VipCardCode"] != DBNull.Value)
			{
				pInstance.VipCardCode =  Convert.ToString(pReader["VipCardCode"]);
			}
			if (pReader["BeginIntegral"] != DBNull.Value)
			{
				pInstance.BeginIntegral =   Convert.ToInt32(pReader["BeginIntegral"]);
			}
			if (pReader["InIntegral"] != DBNull.Value)
			{
				pInstance.InIntegral =   Convert.ToInt32(pReader["InIntegral"]);
			}
			if (pReader["OutIntegral"] != DBNull.Value)
			{
				pInstance.OutIntegral =   Convert.ToInt32(pReader["OutIntegral"]);
			}
			if (pReader["EndIntegral"] != DBNull.Value)
			{
				pInstance.EndIntegral =   Convert.ToInt32(pReader["EndIntegral"]);
			}
			if (pReader["ImminentInvalidIntegral"] != DBNull.Value)
			{
				pInstance.ImminentInvalidIntegral =   Convert.ToInt32(pReader["ImminentInvalidIntegral"]);
			}
			if (pReader["InvalidIntegral"] != DBNull.Value)
			{
				pInstance.InvalidIntegral =   Convert.ToInt32(pReader["InvalidIntegral"]);
			}
			if (pReader["ValidIntegral"] != DBNull.Value)
			{
				pInstance.ValidIntegral =   Convert.ToInt32(pReader["ValidIntegral"]);
			}
			if (pReader["CumulativeIntegral"] != DBNull.Value)
			{
				pInstance.CumulativeIntegral =   Convert.ToInt32(pReader["CumulativeIntegral"]);
			}
			if (pReader["ValidNotIntegral"] != DBNull.Value)
			{
				pInstance.ValidNotIntegral =   Convert.ToInt32(pReader["ValidNotIntegral"]);
			}
			if (pReader["CustomerID"] != DBNull.Value)
			{
				pInstance.CustomerID =  Convert.ToString(pReader["CustomerID"]);
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
