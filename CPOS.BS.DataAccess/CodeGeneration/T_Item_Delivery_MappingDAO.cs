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
    /// ���ݷ��ʣ�  
    /// ��T_Item_Delivery_Mapping�����ݷ�����     
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class T_Item_Delivery_MappingDAO : BaseCPOSDAO, ICRUDable<T_Item_Delivery_MappingEntity>, IQueryable<T_Item_Delivery_MappingEntity>
    {
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public T_Item_Delivery_MappingDAO(LoggingSessionInfo pUserInfo)
            : base(pUserInfo)
        {
        }
        #endregion

        #region ICRUDable ��Ա
        /// <summary>
        /// ����һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        public void Create(T_Item_Delivery_MappingEntity pEntity)
        {
            this.Create(pEntity, null);
        }

        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(T_Item_Delivery_MappingEntity pEntity, IDbTransaction pTran)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            
            //��ʼ���̶��ֶ�


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

            //ִ�в��������д
            int result;
            if (pTran != null)
               result= this.SQLHelper.ExecuteNonQuery((SqlTransaction)pTran, CommandType.Text, strSql.ToString(), parameters);
            else
               result= this.SQLHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters); 
            pEntity.Item_Delivery_Id = pkString;
        }

        /// <summary>
        /// ���ݱ�ʶ����ȡʵ��
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        public T_Item_Delivery_MappingEntity GetByID(object pID)
        {
            //�������
            if (pID == null)
                return null;
            string id = pID.ToString();
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Item_Delivery_Mapping] where Item_Delivery_Id='{0}'  ", id.ToString());
            //��ȡ����
            T_Item_Delivery_MappingEntity m = null;
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
        public T_Item_Delivery_MappingEntity[] GetAll()
        {
            //��֯SQL
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from [T_Item_Delivery_Mapping] where 1=1 ");
            //��ȡ����
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
            //����
            return list.ToArray();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(T_Item_Delivery_MappingEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity , pTran,true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Update(T_Item_Delivery_MappingEntity pEntity , IDbTransaction pTran,bool pIsUpdateNullField)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.Item_Delivery_Id == null)
            {
                throw new ArgumentException("ִ�и���ʱ,ʵ�����������ֵ����Ϊnull.");
            }
             //��ʼ���̶��ֶ�


            //��֯������SQL
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
        public void Update(T_Item_Delivery_MappingEntity pEntity )
        {
            this.Update(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(T_Item_Delivery_MappingEntity pEntity)
        {
            this.Delete(pEntity, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(T_Item_Delivery_MappingEntity pEntity, IDbTransaction pTran)
        {
            //����У��
            if (pEntity == null)
                throw new ArgumentNullException("pEntity");
            if (pEntity.Item_Delivery_Id == null)
            {
                throw new ArgumentException("ִ��ɾ��ʱ,ʵ�����������ֵ����Ϊnull.");
            }
            //ִ�� 
            this.Delete(pEntity.Item_Delivery_Id, pTran);           
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
            sql.AppendLine("update [T_Item_Delivery_Mapping] set  where Item_Delivery_Id=@Item_Delivery_Id;");
            SqlParameter[] parameters = new SqlParameter[] 
            { 
                new SqlParameter{ParameterName="@Item_Delivery_Id",SqlDbType=SqlDbType.VarChar,Value=pID}
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
        public void Delete(T_Item_Delivery_MappingEntity[] pEntities, IDbTransaction pTran)
        {
            //��������ֵ
            object[] entityIDs = new object[pEntities.Length];
            for (int i = 0; i < pEntities.Length; i++)
            {
                var pEntity = pEntities[i];
                //����У��
                if (pEntity == null)
                    throw new ArgumentNullException("pEntity");
                if (pEntity.Item_Delivery_Id == null)
                {
                    throw new ArgumentException("ִ��ɾ��ʱ,ʵ�����������ֵ����Ϊnull.");
                }
                entityIDs[i] = pEntity.Item_Delivery_Id;
            }
            Delete(entityIDs, pTran);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pEntities">ʵ��ʵ������</param>
        public void Delete(T_Item_Delivery_MappingEntity[] pEntities)
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
          //  sql.AppendLine("update [T_Item_Delivery_Mapping] set  where Item_Delivery_Id in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
            sql.AppendLine("delete from  [T_Item_Delivery_Mapping] where Item_Delivery_Id in (" + primaryKeys.ToString().Substring(0, primaryKeys.ToString().Length - 1) + ");");
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
        public T_Item_Delivery_MappingEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            //��֯SQL
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
            //ִ��SQL
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
        public PagedQueryResult<T_Item_Delivery_MappingEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
                pagedSql.AppendFormat(" [Item_Delivery_Id] desc"); //Ĭ��Ϊ����ֵ����
            }
            pagedSql.AppendFormat(") as ___rn,* from [T_Item_Delivery_Mapping] where 1=1  ");
            //�ܼ�¼��SQL
            totalCountSql.AppendFormat("select count(1) from [T_Item_Delivery_Mapping] where 1=1  ");
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
        public T_Item_Delivery_MappingEntity[] QueryByEntity(T_Item_Delivery_MappingEntity pQueryEntity, OrderBy[] pOrderBys)
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
        public PagedQueryResult<T_Item_Delivery_MappingEntity> PagedQueryByEntity(T_Item_Delivery_MappingEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
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
        protected IWhereCondition[] GetWhereConditionByEntity(T_Item_Delivery_MappingEntity pQueryEntity)
        { 
            //��ȡ�ǿ���������
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
        /// װ��ʵ��
        /// </summary>
        /// <param name="pReader">��ǰֻ����</param>
        /// <param name="pInstance">ʵ��ʵ��</param>
        protected void Load(IDataReader pReader, out T_Item_Delivery_MappingEntity pInstance)
        {
            //�����е����ݴ�SqlDataReader�ж�ȡ��Entity��
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
