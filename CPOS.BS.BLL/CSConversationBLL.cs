/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2014/3/5 18:00:39
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
    /// ҵ������ ������¼ 
    /// </summary>
    public partial class CSConversationBLL
    {
        /// <summary>
        /// ������Ϣ�б�
        /// </summary>
        /// <param name="isCS">�Ƿ��ǿͷ�1����0����</param>
        /// <param name="personID">��ǰ�û�ID</param>
        /// <param name="messageId">��ǰ��ϢID�����Ҫ��ȡ������Ϣ������ϢIDΪNULL</param>
        /// <param name="pageSize">ҳ��С</param>
        /// <param name="pageIndex">��ǰҳ</param>
        /// <param name="recordCount">�ܼ�¼��</param>
        /// <returns>��Ϣ�б�</returns>
        public DataSet GetMessageVipInfo(string personID, string customerId)
        {
           return  this._currentDAO.GetMessageVipInfo(personID, customerId);
        }

    }

}