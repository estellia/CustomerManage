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
    /// ���ݷ��ʣ�  
    /// ��T_UserGuideModules�����ݷ�����     
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class T_UserGuideModulesDAO : Base.BaseCPOSDAO, ICRUDable<T_UserGuideModulesEntity>, IQueryable<T_UserGuideModulesEntity>
    {
		public string StaticConnectionString { get; set; }
		private ISQLHelper staticSqlHelper;
		#region ���캯��
		/// <summary>
		/// ���캯�� 
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

		#region ICRUDable ��Ա
		/// <summary>
		/// ����һ����ʵ��
		/// </summary>
		/// <param name="pEntity">ʵ��ʵ��</param>
		public void Create(T_UserGuideModulesEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(T_UserGuideModulesEntity pEntity, IDbTransaction pTran)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //��ʼ���̶��ֶ�
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

            //ִ�в��������д
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.UserGuideModulesId = pkGuid;
        }

        /// <summary>
        /// ���ݱ�ʶ����ȡʵ��
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        public T_UserGuideModulesEntity GetByID(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_UserGuideModules] where UserGuideModulesId='{0}'  and isdelete=0 ", id.ToString());
            //��ȡ����
            T_UserGuideModulesEntity m = null;
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(sql.ToString()))
            {
                while (rdr.Read())
                {
                    this.Load(rdr, out m);
                    break;
                }
            }
            //����
            return m;
        }

        /// <summary>
        /// ��ȡ����ʵ��
        /// </summary>
        /// <returns></returns>
        public T_UserGuideModulesEntity[] GetAll()
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_UserGuideModules] where 1=1  and isdelete=0");
            //��ȡ����
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
            //����
            return list.ToArray();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(T_UserGuideModulesEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(T_UserGuideModulesEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.UserGuideModulesId.HasValue)
            {
                throw new ArgumentException("ִ�и���ʱ,ʵ�����������ֵ����Ϊnull.");
            }
             //��ʼ���̶��ֶ�
			pEntity.LastUpdateTime=DateTime.Now;
			pEntity.LastUpdateBy=CurrentUserInfo.UserID;


            //��֯������SQL
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

            //ִ�����
            int result = 0;
            if (pTran != null)
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        public void Update(T_UserGuideModulesEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(T_UserGuideModulesEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(T_UserGuideModulesEntity pEntity, IDbTransaction pTran)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (!pEntity.UserGuideModulesId.HasValue)
            {
                throw new ArgumentException("ִ��ɾ��ʱ,ʵ�����������ֵ����Ϊnull.");
            }
            //ִ�� 
            this.Delete(pEntity.UserGuideModulesId.Value, pTran);           
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(object pID, IDbTransaction pTran)
        {
            if (pID == null)
                return ;   
            //��֯������SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [T_UserGuideModules] set  isdelete=1 where UserGuideModulesId=@UserGuideModulesId;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@UserGuideModulesId",SqlDbType=SqlDbType.UniqueIdentifier,Value=pID}
            };
            //ִ�����
            int result = 0;
            if (pTran != null)
                result=this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, sql.ToString(), parameters);
            else
                result=this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), parameters);
            return ;
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pEntities">ʵ��ʵ������</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(T_UserGuideModulesEntity[] pEntities, IDbTransaction pTran)
        {
            //��������ֵ
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //����У��
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (!pEntity.UserGuideModulesId.HasValue)
                {
                    throw new ArgumentException("ִ��ɾ��ʱ,ʵ�����������ֵ����Ϊnull.");
                }
                entityIDs[i] = pEntity.UserGuideModulesId;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pEntities">ʵ��ʵ������</param>
        public void Delete(T_UserGuideModulesEntity[] pEntities)
        { 
            Delete(pEntities, null);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pIDs">��ʶ��ֵ����</param>
        public void Delete(object[] pIDs)
        {
            Delete(pIDs,null);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pIDs">��ʶ��ֵ����</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(object[] pIDs, IDbTransaction pTran) 
        {
            if (pIDs == null || pIDs.Length==0)
                return ;
            //��֯������SQL
            StringBuilder primaryKeys = new StringBuilder();
            foreach (object item in pIDs)
            {
                primaryKeys.AppendFormat("'{0}',",item.ToString());
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update [T_UserGuideModules] set  isdelete=1 where UserGuideModulesId in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
            //ִ�����
            int result = 0;   
            if (pTran == null)
                result = this.SQLHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), null);
            else
                result = this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran,CommandType.Text, sql.ToString());       
        }
        #endregion

        #region IQueryable ��Ա
        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <param name="pWhereConditions">ɸѡ����</param>
        /// <param name="pOrderBys">����</param>
        /// <returns></returns>
        public T_UserGuideModulesEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //��֯SQL
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
            //ִ��SQL
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
            //���ؽ��
            return list.ToArray();
        }
        /// <summary>
        /// ִ�з�ҳ��ѯ
        /// </summary>
        /// <param name="pWhereConditions">ɸѡ����</param>
        /// <param name="pOrderBys">����</param>
        /// <param name="pPageSize">ÿҳ�ļ�¼��</param>
        /// <param name="pCurrentPageIndex">��0��ʼ�ĵ�ǰҳ��</param>
        /// <returns></returns>
        public PagedQueryResult<T_UserGuideModulesEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            //��֯SQL
            StringBuilder pagedSql = new StringBuilder();
            StringBuilder totalCountSql = new StringBuilder();
            //��ҳSQL
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
                pagedSql.AppendFormat(" [UserGuideModulesId] desc"); //Ĭ��Ϊ����ֵ����
            }
            pagedSql.AppendFormat(") as ___rn,* from [T_UserGuideModules] where 1=1  and isdelete=0 ");
            //�ܼ�¼��SQL
            totalCountSql.AppendFormat("select count(1) from [T_UserGuideModules] where 1=1  and isdelete=0 ");
            //��������
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
            //ȡָ��ҳ������
            pagedSql.AppendFormat(" where ___rn >{0} and ___rn <={1}", pPageSize * (pCurrentPageIndex-1), pPageSize * (pCurrentPageIndex));
            //ִ����䲢���ؽ��
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
            int totalCount = Convert.ToInt32(this.SQLHelper.ExecuteScalar(totalCountSql.ToString()));    //����������
            result.RowCount = totalCount;
            int remainder = 0;
            result.PageCount = Math.DivRem(totalCount, pPageSize, out remainder);
            if (remainder > 0)
                result.PageCount++;
            return result;
        }

        /// <summary>
        /// ����ʵ��������ѯʵ��
        /// </summary>
        /// <param name="pQueryEntity">��ʵ����ʽ����Ĳ���</param>
        /// <param name="pOrderBys">�������</param>
        /// <returns>����������ʵ�弯</returns>
        public T_UserGuideModulesEntity[] QueryByEntity(T_UserGuideModulesEntity pQueryEntity, OrderBy[] pOrderBys)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity(pQueryEntity);
            return Query(queryWhereCondition,  pOrderBys);            
        }

        /// <summary>
        /// ��ҳ����ʵ��������ѯʵ��
        /// </summary>
        /// <param name="pQueryEntity">��ʵ����ʽ����Ĳ���</param>
        /// <param name="pOrderBys">�������</param>
        /// <returns>����������ʵ�弯</returns>
        public PagedQueryResult<T_UserGuideModulesEntity> PagedQueryByEntity(T_UserGuideModulesEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            IWhereCondition[] queryWhereCondition = GetWhereConditionByEntity( pQueryEntity);
            return PagedQuery(queryWhereCondition, pOrderBys, pPageSize, pCurrentPageIndex);
        }

        #endregion

        #region ���߷���
        /// <summary>
        /// ����ʵ���Null�������ɲ�ѯ������
        /// </summary>
        /// <returns></returns>
        protected IWhereCondition[] GetWhereConditionByEntity(T_UserGuideModulesEntity pQueryEntity)
        { 
            //��ȡ�ǿ���������
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
        /// װ��ʵ��
        /// </summary>
        /// <param name="pReader">��ǰֻ����</param>
        /// <param name="pInstance">ʵ��ʵ��</param>
        protected void Load(IDataReader pReader, out T_UserGuideModulesEntity pInstance)
        {
            //�����е����ݴ�SqlDataReader�ж�ȡ��Entity��
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
