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
    /// 业务处理：  
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
        /// 添加创意仓库中的触点活动奖品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void AddContactEventPrizeForCTW(LPrizesEntity pEntity)
        {
            this._currentDAO.AddContactEventPrizeForCTW(pEntity);
        }
        /// <summary>
        ///  触点是否已存在
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
        /// 获取创意仓库下的触点活动
        /// </summary>
        /// <param name="strCTWEventId"></param>
        /// <returns></returns>
        public DataSet GetContactEventByCTWEventId(string strCTWEventId)
        {
            return this._currentDAO.GetContactEventByCTWEventId(strCTWEventId);

        }
        /// <summary>
        /// 根据当前活动类型和奖励类型获取赠送积分信息
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