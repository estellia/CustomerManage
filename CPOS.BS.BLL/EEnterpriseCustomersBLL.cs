/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/10/28 10:12:41
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
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using JIT.Utility;
using JIT.Utility.ExtensionMethod;
using JIT.CPOS.BS.DataAccess;
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess;
using JIT.Utility.DataAccess.Query;

namespace JIT.CPOS.BS.BLL
{
    /// <summary>
    /// 业务处理：  
    /// </summary>
    public partial class EEnterpriseCustomersBLL
    {
        #region 列表获取
        /// <summary>
        /// 列表获取
        /// </summary>
        public IList<EEnterpriseCustomersEntity> GetList(EEnterpriseCustomersEntity entity, int Page, int PageSize)
        {
            if (PageSize <= 0) PageSize = 15;

            IList<EEnterpriseCustomersEntity> eventsList = new List<EEnterpriseCustomersEntity>();
            DataSet ds = new DataSet();
            ds = _currentDAO.GetList(entity, Page, PageSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                eventsList = DataTableToObject.ConvertToList<EEnterpriseCustomersEntity>(ds.Tables[0]);
            }
            return eventsList;
        }
        /// <summary>
        /// 列表数量获取
        /// </summary>
        public int GetListCount(EEnterpriseCustomersEntity entity)
        {
            return _currentDAO.GetListCount(entity);
        }
        #endregion

        #region 修改状态
        /// <summary>
        /// 修改状态
        /// </summary>
        public bool SetStatus(EEnterpriseCustomersEntity obj)
        {
            return _currentDAO.SetStatus(obj);
        }
        #endregion

        #region Top列表获取
        /// <summary>
        /// 列表获取
        /// </summary>
        public IList<EEnterpriseCustomersEntity> GetTopList(EEnterpriseCustomersEntity entity)
        {
            IList<EEnterpriseCustomersEntity> eventsList = new List<EEnterpriseCustomersEntity>();
            DataSet ds = new DataSet();
            ds = _currentDAO.GetTopList(entity);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                eventsList = DataTableToObject.ConvertToList<EEnterpriseCustomersEntity>(ds.Tables[0]);
            }
            return eventsList;
        }
        #endregion
    }
}