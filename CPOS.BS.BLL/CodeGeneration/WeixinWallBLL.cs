/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/8/14 11:26:51
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
    /// ҵ������  
    /// </summary>
    public partial class WeixinWallBLL
    {
        private LoggingSessionInfo CurrentUserInfo;
        private WeixinWallDAO _currentDAO;
        #region ���캯��
        /// <summary>
        /// ���캯�� 
        /// </summary>
        public WeixinWallBLL(LoggingSessionInfo pUserInfo)
        {
            this.CurrentUserInfo = pUserInfo;
            this._currentDAO = new WeixinWallDAO(pUserInfo);
        }
        #endregion
        #region ICRUDable ��Ա
        /// <summary>
        /// ����һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        public void Create(WeixinWallEntity pEntity)
        {
            _currentDAO.Create(pEntity);
        }


        /// <summary>
        /// �������ڴ���һ����ʵ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Create(WeixinWallEntity pEntity, IDbTransaction pTran)
        {
            _currentDAO.Create(pEntity,pTran);
        }

        /// <summary>
        /// ���ݱ�ʶ����ȡʵ��
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        public WeixinWallEntity GetByID(object pID)
        {
            return _currentDAO.GetByID(pID);
        }

        /// <summary>
        /// ��ȡ����ʵ��
        /// </summary>
        /// <returns></returns>
        public WeixinWallEntity[] GetAll()
        {
            return _currentDAO.GetAll();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>        
        public void Update(WeixinWallEntity pEntity , IDbTransaction pTran)
        {
            Update(pEntity,true, pTran);
        }
        public void Update(WeixinWallEntity pEntity, bool pIsUpdateNullField , IDbTransaction pTran)
        {
            _currentDAO.Update(pEntity,pIsUpdateNullField,pTran);
        }


        /// <summary>
        /// ����
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        public void Update(WeixinWallEntity pEntity)
        {
            Update(pEntity , true);
        }
        public void Update(WeixinWallEntity pEntity , bool pIsUpdateNullField)
        {
            _currentDAO.Update(pEntity,pIsUpdateNullField);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity"></param>
        public void Delete(WeixinWallEntity pEntity)
        {
            _currentDAO.Delete(pEntity);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pEntity">ʵ��ʵ��</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(WeixinWallEntity pEntity, IDbTransaction pTran)
        {
            _currentDAO.Delete(pEntity,pTran);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="pID">��ʶ����ֵ</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(object pID, IDbTransaction pTran)
        {
            _currentDAO.Delete(pID,pTran);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pEntities">ʵ��ʵ������</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(WeixinWallEntity[] pEntities, IDbTransaction pTran)
        {
            _currentDAO.Delete(pEntities,pTran);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pEntities">ʵ��ʵ������</param>
        public void Delete(WeixinWallEntity[] pEntities)
        { 
            _currentDAO.Delete(pEntities);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pIDs">��ʶ��ֵ����</param>
        public void Delete(object[] pIDs)
        {
            _currentDAO.Delete(pIDs);
        }

        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="pIDs">��ʶ��ֵ����</param>
        /// <param name="pTran">����ʵ��,��Ϊnull,���Ϊnull,��ʹ������������</param>
        public void Delete(object[] pIDs, IDbTransaction pTran) 
        {
            _currentDAO.Delete(pIDs,pTran);
        }
        #endregion

        #region IQueryable ��Ա
        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <param name="pWhereConditions">ɸѡ����</param>
        /// <param name="pOrderBys">����</param>
        /// <returns></returns>
        public WeixinWallEntity[] Query(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys)
        {
           return _currentDAO.Query(pWhereConditions,pOrderBys);
        }

        /// <summary>
        /// ִ�з�ҳ��ѯ
        /// </summary>
        /// <param name="pWhereConditions">ɸѡ����</param>
        /// <param name="pOrderBys">����</param>
        /// <param name="pPageSize">ÿҳ�ļ�¼��</param>
        /// <param name="pCurrentPageIndex">��0��ʼ�ĵ�ǰҳ��</param>
        /// <returns></returns>
        public PagedQueryResult<WeixinWallEntity> PagedQuery(IWhereCondition[] pWhereConditions, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
           return _currentDAO.PagedQuery(pWhereConditions,pOrderBys,pPageSize,pCurrentPageIndex);
        }

        /// <summary>
        /// ����ʵ��������ѯʵ��
        /// </summary>
        /// <param name="pQueryEntity">��ʵ����ʽ����Ĳ���</param>
        /// <param name="pOrderBys">�������</param>
        /// <returns>����������ʵ�弯</returns>
        public WeixinWallEntity[] QueryByEntity(WeixinWallEntity pQueryEntity, OrderBy[] pOrderBys)
        {
           return _currentDAO.QueryByEntity(pQueryEntity,pOrderBys);
        }

        /// <summary>
        /// ��ҳ����ʵ��������ѯʵ��
        /// </summary>
        /// <param name="pQueryEntity">��ʵ����ʽ����Ĳ���</param>
        /// <param name="pOrderBys">�������</param>
        /// <returns>����������ʵ�弯</returns>
        public PagedQueryResult<WeixinWallEntity> PagedQueryByEntity(WeixinWallEntity pQueryEntity, OrderBy[] pOrderBys, int pPageSize, int pCurrentPageIndex)
        {
           return _currentDAO.PagedQueryByEntity(pQueryEntity,pOrderBys,pPageSize,pCurrentPageIndex);
        }

        #endregion
    }
}