/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/2/28 17:54:09
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
using System.Data.SqlClient;

namespace JIT.CPOS.BLL
{
    /// <summary>
    /// 业务处理：  
    /// </summary>
    public partial class ObjectEvaluationBLL
    {
        public ObjectEvaluationEntity[] GetByVIPAndObject(string pClientID, string pMemberID, string pObjectID, int? page, int? pagesize)
        {
            return this._currentDAO.GetByVIPAndObject(pClientID, pMemberID, pObjectID, page ?? 0, pagesize ?? 5);
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <returns></returns>
        public SqlTransaction GetTran()
        {
            return this._currentDAO.GetTran();
        }

        /// <summary>
        /// 获取商品评论个数
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="starLevel">评论等级</param>
        /// <returns></returns>
        public int GetEvaluationCount(string objectID, int starLevel)
        {
            return this._currentDAO.GetEvaluationCount(objectID, starLevel);
        }
    }
}