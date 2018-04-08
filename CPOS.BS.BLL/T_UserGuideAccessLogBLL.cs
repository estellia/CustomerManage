/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016/8/24 10:01:44
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
    public partial class T_UserGuideAccessLogBLL
    {  
		public void SetUserGuideLog(T_UserGuideAccessLogEntity entityLog,LoggingSessionInfo loggingSessionInfo) {
			T_UserGuideAccessLogBLL bllUserGuideLog = new T_UserGuideAccessLogBLL(loggingSessionInfo);

			var entity=bllUserGuideLog.QueryByEntity(new T_UserGuideAccessLogEntity() {UserId= entityLog.UserId,CustomerId= entityLog.CustomerId,UserGuideModulesId= entityLog.UserGuideModulesId,ModuleCode=entityLog.ModuleCode,IsDelete=0},null).FirstOrDefault();
			if(entity!=null) {
				entityLog.Id = entity.Id;
				if (entity.FinishedStatus == 1)//判断是否已经完成了，如已完成则不改变状态继续为1
					{
					entityLog.FinishedStatus = 1;
					entityLog.LastAccessStep = 1;
					entityLog.Url = "";

				}
				bllUserGuideLog.Update(entityLog);
			}
			else {
				bllUserGuideLog.Create(entityLog);
			}
		}
	}
}