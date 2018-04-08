/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2013/7/17 14:52:46
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
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.DataAccess.Base;

namespace JIT.CPOS.BS.DataAccess
{
    
    /// <summary>
    /// ���ݷ��ʣ�  
    /// ��LLotteryLog�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class LLotteryLogDAO : Base.BaseCPOSDAO, ICRUDable<LLotteryLogEntity>, IQueryable<LLotteryLogEntity>
    {
        #region ��齱��־�б�
        /// <summary>
        /// ���ݱ�ʶ��ȡ�齱��־����
        /// </summary>
        /// <param name="EventID"></param>
        /// <returns></returns>
        public int GetEventLotteryLogCount(string EventID)
        {
            string sql = GetEventLotteryLogSql(EventID);
            sql = sql + " select count(*) as icount From #tmp; ";
            return Convert.ToInt32(this.SQLHelper.ExecuteScalar(sql));
        }
        /// <summary>
        /// ���ݱ�ʶ��ȡ�齱��־��Ϣ
        /// </summary>
        public DataSet GetEventLotteryLogList(string EventID, int Page, int PageSize)
        {
            int beginSize = Page * PageSize + 1;
            int endSize = Page * PageSize + PageSize;
            DataSet ds = new DataSet();
            string sql = GetEventLotteryLogSql(EventID);
            sql = sql + "select * From #tmp a where 1=1 and a.displayindex between '" + beginSize + "' and '" + endSize + "' order by  a.displayindex ";
            ds = this.SQLHelper.ExecuteDataset(sql);
            return ds;
        }
        private string GetEventLotteryLogSql(string EventID)
        {
            BasicUserInfo pUserInfo = new BasicUserInfo();

            string sql = "";
            sql += "SELECT a.* "
                + " ,DisplayIndex = row_number() over(order by a.createTime desc ) "
                + " ,b.vipName VipName "
                + " into #tmp FROM LLotteryLog a "
                + " left join vip b on a.vipId=b.vipId "
                + " where a.IsDelete='0' and a.EventID = '" + EventID + "' ";
            return sql;
        }
        #endregion
        public int GetEventLotteryLogByEventId(string strEventId,string strVipId)
        {
            string strSql = string.Format("SELECT COUNT(1) LogCount from LLotteryLog where EventId='{0}' and VipId='{1}'", strEventId, strVipId);
            return Convert.ToInt32(SQLHelper.ExecuteScalar(strSql));
        }
    }
}