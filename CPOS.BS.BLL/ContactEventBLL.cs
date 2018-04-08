/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015/10/26 20:41:45
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
using JIT.Utility.DataAccess;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.Entity;

namespace JIT.CPOS.BS.BLL
{
    /// <summary>
    /// ҵ������  
    /// </summary>
    public partial class ContactEventBLL
    {
        public DataSet GetContactEventList(int pPageSize, int pCurrentPageIndex)
        {
            return this._currentDAO.GetContactEventList(pPageSize, pCurrentPageIndex);
        }
        public void AddContactEventPrize(LPrizesEntity pEntity)
        {
            this._currentDAO.AddContactEventPrize(pEntity);
        }
        /// <summary>
        /// ���Ӵ���ֿ��еĴ�����Ʒ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void AddContactEventPrizeForCTW(LPrizesEntity pEntity)
        {
            this._currentDAO.AddContactEventPrizeForCTW(pEntity);
        }
        /// <summary>
        ///  �����Ƿ��Ѵ���
        /// </summary>
        /// <param name="strContactType"></param>
        /// <param name="strShareEventId"></param>
        /// <returns></returns>
        public int ExistsContact(string strContactTypeCode, string strShareEventId)
        {
            return this._currentDAO.ExistsContact(strContactTypeCode, strShareEventId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strEventId"></param>
        /// <returns></returns>
        public int DeleteContact(string strEventId)
        {

            return this._currentDAO.DeleteContact(strEventId);
        }
        public int DeleteContactPrize(string strEventId)
        {

            return this._currentDAO.DeleteContactPrize(strEventId);
        }
        /// <summary>
        /// ��ȡ����ֿ��µĴ���
        /// </summary>
        /// <param name="strCTWEventId"></param>
        /// <returns></returns>
        public DataSet GetContactEventByCTWEventId(string strCTWEventId)
        {
            return this._currentDAO.GetContactEventByCTWEventId(strCTWEventId);

        }
        /// <summary>
        /// ���ݵ�ǰ����ͺͽ������ͻ�ȡ���ͻ�����Ϣ
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="ContactTypeCode"></param>
        /// <param name="PrizeType"></param>
        /// <param name="IsCTW"></param>
        /// <returns></returns>
        public int GetContactEventIntegral(string CustomerID, string ContactTypeCode, string PrizeType, int IsCTW)
        {
            return this._currentDAO.GetContactEventIntegral(CustomerID,ContactTypeCode,PrizeType,IsCTW);
        }
    }
}