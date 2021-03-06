/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/3/26 16:10:52
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
    public partial class CustomerBasicSettingBLL
    {
        private LoggingSessionInfo CurrentUserInfo;
        private CustomerBasicSettingDAO _currentDAO;
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public CustomerBasicSettingBLL(LoggingSessionInfo pUserInfo)
        {
            this.CurrentUserInfo = pUserInfo;
            this._currentDAO = new CustomerBasicSettingDAO(pUserInfo);
        }
        #endregion
        #region ICRUDable 成员
        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Create(CustomerBasicSettingEntity pEntity)
        {
            _currentDAO.Create(pEntity);
        }


        /// <summary>
        /// 在事务内创建一个新实例
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Create(CustomerBasicSettingEntity pEntity, IDbTransaction pTran)
        {
            _currentDAO.Create(pEntity, pTran);
        }

        /// <summary>
        /// 根据标识符获取实例
        /// </summary>
        /// <param name="pID">标识符的值</param>
        public CustomerBasicSettingEntity GetByID(object pID)
        {
            return _currentDAO.GetByID(pID);
        }

        /// <summary>
        /// 获取所有实例
        /// </summary>
        /// <returns></returns>
        public CustomerBasicSettingEntity[] GetAll()
        {
            return _currentDAO.GetAll();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Update(CustomerBasicSettingEntity pEntity, IDbTransaction pTran)
        {
            _currentDAO.Update(pEntity, pTran);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        public void Update(CustomerBasicSettingEntity pEntity)
        {
            _currentDAO.Update(pEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(CustomerBasicSettingEntity pEntity)
        {
            _currentDAO.Delete(pEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(CustomerBasicSettingEntity pEntity, IDbTransaction pTran)
        {
            _currentDAO.Delete(pEntity, pTran);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pID">标识符的值</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(object pID, IDbTransaction pTran)
        {
            _currentDAO.Delete(pID, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(CustomerBasicSettingEntity[] pEntities, IDbTransaction pTran)
        {
            _currentDAO.Delete(pEntities, pTran);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pEntities">实体实例数组</param>
        public void Delete(CustomerBasicSettingEntity[] pEntities)
        {
            _currentDAO.Delete(pEntities);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pIDs">标识符值数组</param>
        public void Delete(object[] pIDs)
        {
            _currentDAO.Delete(pIDs);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pIDs">标识符值数组</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public void Delete(object[] pIDs, IDbTransaction pTran)
        {
            _currentDAO.Delete(pIDs, pTran);
        }
        #endregion

        #region IQueryable 成员
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="pWhereConditions">筛选条件</param>
        /// <param name="pOrderBys">排序</param>
        /// <returns></returns>
        public CustomerBasicSettingEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
            return _currentDAO.Query(pWhereConditions, pOrderBys);
        }

        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="pWhereConditions">筛选条件</param>
        /// <param name="pOrderBys">排序</param>
        /// <param name="pPageSize">每页的记录数</param>
        /// <param name="pCurrentPageIndex">以0开始的当前页码</param>
        /// <returns></returns>
        public PagedQueryResult<CustomerBasicSettingEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            return _currentDAO.PagedQuery(pWhereConditions, pOrderBys, pPageSize, pCurrentPageIndex);
        }

        /// <summary>
        /// 根据实体条件查询实体
        /// </summary>
        /// <param name="pQueryEntity">以实体形式传入的参数</param>
        /// <param name="pOrderBys">排序组合</param>
        /// <returns>符合条件的实体集</returns>
        public CustomerBasicSettingEntity[] QueryByEntity(CustomerBasicSettingEntity pQueryEntity, OrderBy[] pOrderBys)
        {
            return _currentDAO.QueryByEntity(pQueryEntity, pOrderBys);
        }

        /// <summary>
        /// 分页根据实体条件查询实体
        /// </summary>
        /// <param name="pQueryEntity">以实体形式传入的参数</param>
        /// <param name="pOrderBys">排序组合</param>
        /// <returns>符合条件的实体集</returns>
        public PagedQueryResult<CustomerBasicSettingEntity> PagedQueryByEntity(CustomerBasicSettingEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
            return _currentDAO.PagedQueryByEntity(pQueryEntity, pOrderBys, pPageSize, pCurrentPageIndex);
        }

        #endregion

        #region GetCustomerBasicSettingByKey
        /// <summary>
        ///根据客户ID获取客户信息
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public DataSet GetCustomerBasicSettingByKey(string customerID)
        {
            return this._currentDAO.GetCustomerBasicSettingByKey(customerID);
        }
        #endregion

        #region GetCustomerBasicSettingByKey
        #region GetCustomerBasicSettingByKey
        /// <summary>
        ///根据客户ID获取客户信息
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public DataSet GetCustomerInfo(string customerID)
        {
            return this._currentDAO.GetCustomerInfo(customerID);
        }
        #endregion
        #endregion

        #region SaveustomerBasicrInfo
        public int SaveCustomerBasicInfo(string strCustomerId,List<CustomerBasicSettingEntity> list)
        {
            int i = this._currentDAO.SaveustomerBasicrInfo(list);
            if(i>0)///2016-05-17 wujx 种植缓存
            {
                RedisOperationBLL.BasicSetting.BasicSettingBLL bllBasicSetting = new RedisOperationBLL.BasicSetting.BasicSettingBLL();
                bllBasicSetting.SetBasicSetting(strCustomerId);
            }
            return i;
        }
        #endregion

        #region GetCousCustomerType
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetCousCustomerType()
        {
            DataSet ds = this._currentDAO.GetCousCustomerType();
            return ds;
        }
        #endregion

        #region GetIsAld
        public string GetIsAld()
        {
            return this._currentDAO.GetIsAld();
        }
        #endregion


    }
}