using JIT.Utility.DataAccess.Query;
using JIT.CPOS.DTO.Module.AP.ZMBA.Response;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JIT.CPOS.BS.BLL.AP;

namespace JIT.CPOS.Web.ApplicationInterface.Module.AP.ZMBA
{
    /// <summary>
    /// 获取正念商学院课程列表
    /// </summary>
    public class GetZMBACourseListAH : BaseActionHandler<EmptyRequestParameter, GetZMBACourseRD>
    {
        protected override GetZMBACourseRD ProcessRequest(DTO.Base.APIRequest<EmptyRequestParameter> pRequest)
        {
            //基础数据初始化
          GetZMBACourseRD ZMBACourseRD = new GetZMBACourseRD();
            T_ZMBA_CourseBLL bll = new T_ZMBA_CourseBLL(this.CurrentUserInfo);
            List<T_ZMBA_CourseEntity> ZMBACourseList = new List<T_ZMBA_CourseEntity>();

            //查询条件初始化
            T_ZMBA_CourseEntity pT_ZMBA_CourseEntity = new T_ZMBA_CourseEntity();
            OrderBy[] pOrderBy = new OrderBy[1];
            pOrderBy[0] = new OrderBy(){ FieldName = "DisplayIndex",Direction = OrderByDirections.Asc};

			var bllAPCommon = new APCommonBLL(CurrentUserInfo);
			var VersionId = bllAPCommon.GetCustomerVersion(CurrentUserInfo.ClientID);

			if (VersionId == 3) {

				List<IWhereCondition> complexCondition = new List<IWhereCondition> { };
				complexCondition.Add(new DirectCondition(" CourseName in('新手开店','新干线讲师','员工管理','集客吸粉','在线客服','会员服务','经营数据','卡券核销')"));

				ZMBACourseList = bll.Query(complexCondition.ToArray(), pOrderBy).ToList();

			}
			else {
				//执行查询
				ZMBACourseList = bll.QueryByEntity(pT_ZMBA_CourseEntity, pOrderBy).ToList();

			}
			ZMBACourseRD.ZMBACourseList = ZMBACourseList;

            return ZMBACourseRD;

        }
 

    }
}