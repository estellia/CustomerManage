/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/6/1 19:09:45
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
    public partial class R_SRT_RTTopBLL
    {
        /// <summary>
        /// ��ȡ ������ ����ͳ��
        /// </summary>
        /// <param name="pageIndex">��ǰҳ��</param>
        /// <param name="pageSize">úҵ��ʾ����</param>
        /// <param name="BusiType">��������</param>
        /// <param name="CustomerID">�̻����</param>
        /// <param name="SortOrder">����ʽ</param>
        /// <returns></returns>
        public List<R_SRT_RTTopEntity> GetRsrtrtTopList(string CustomerID, string BusiType)
        {
            return _currentDAO.GetRsrtrtTopList(CustomerID, BusiType);
        }
    }
}