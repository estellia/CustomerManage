/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/7/18 15:41:40
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
using JIT.CPOS.BS.DataAccess.Utility;
using JIT.Utility;
using JIT.Utility.Entity;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.DataAccess;
using JIT.Utility.Log;
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.DataAccess.Base;
using JIT.Utility.Reflection;

namespace JIT.CPOS.BS.DataAccess
{
    
    /// <summary>
    /// ���ݷ��ʣ�  
    /// ��EnterpriseMember�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class EnterpriseMemberDAO : Base.BaseCPOSDAO, ICRUDable<EnterpriseMemberEntity>, IQueryable<EnterpriseMemberEntity>
    {
        #region GetList
        public PagedQueryResult<EnterpriseMemberEntity> GetList(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pageIndex, int pageSize)
        {
            StringBuilder sqlWhere = new StringBuilder();
            if (pWhereConditions != null)
            {
                foreach (var item in pWhereConditions)
                {
                    sqlWhere.AppendFormat(" and {0}", item.GetExpression());
                }
            }
            StringBuilder sqlOrder = new StringBuilder();
            if (pOrderBys != null && pOrderBys.Length > 0)
            {
                foreach (var item in pOrderBys)
                {
                    sqlOrder.AppendFormat(" {0} {1},", item.FieldName, item.Direction == OrderByDirections.Asc ? "asc" : "desc");
                }
                sqlOrder.Remove(sqlOrder.Length - 1, 1);
            }

            //ͨ�÷�ҳ��ѯ
            UtilityEntity model = new UtilityEntity();
            //TODO:�ý�壬�������
            model.TableName = "(" + "select * from dbo.EnterpriseMember  WHERE IsDelete=0" + sqlWhere.ToString() + ") as A";
            model.PageIndex = pageIndex;
            model.PageSize = pageSize;
            model.PageSort = sqlOrder.ToString();
            new UtilityDAO(this.CurrentUserInfo).PagedQuery(model);

            //����ֵ
            PagedQueryResult<EnterpriseMemberEntity> pEntity = new PagedQueryResult<EnterpriseMemberEntity>();
            pEntity.RowCount = model.PageTotal;
            if (model.PageDataSet != null
                && model.PageDataSet.Tables != null
                && model.PageDataSet.Tables.Count > 0)
            {
                pEntity.Entities = DataLoader.LoadFrom<EnterpriseMemberEntity>(model.PageDataSet.Tables[0], new DirectPropertyNameMapping());
            }
            return pEntity;
        }
        #endregion
    }
}