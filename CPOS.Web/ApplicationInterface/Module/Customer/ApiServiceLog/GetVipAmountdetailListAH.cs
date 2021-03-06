﻿using JIT.CPOS.BS.BLL;
using JIT.CPOS.DTO.Module.VIP.ServicesLog.Request;
using JIT.CPOS.DTO.Module.VIP.ServicesLog.Response;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.Utility.DataAccess.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIT.CPOS.Web.ApplicationInterface.Module.Customer.ApiServiceLog
{
    
    public class GetVipAmountdetailListAH : BaseActionHandler<GetVipAmountdetailListRP, GetVipAmountdetailListRD>
    {
        /// <summary>
        /// api获取当前门店返利信息
        /// </summary>
        protected override GetVipAmountdetailListRD ProcessRequest(DTO.Base.APIRequest<GetVipAmountdetailListRP> pRequest)
        {
            var rd = new GetVipAmountdetailListRD();
            var para = pRequest.Parameters;
            var VipAmountdetailBLL = new VipAmountDetailBLL(CurrentUserInfo);
            var VipBLL = new VipBLL(CurrentUserInfo);
            var UserBLL = new T_UserBLL(CurrentUserInfo);
            //查询参数
            List<IWhereCondition> complexCondition = new List<IWhereCondition> { };
            #region 门店条件处理
            string UnitId = "";
            if (CurrentUserInfo.CurrentUserRole != null)
                if (!string.IsNullOrWhiteSpace(CurrentUserInfo.CurrentUserRole.UnitId))
                    UnitId = CurrentUserInfo.CurrentUserRole.UnitId;

            if (!string.IsNullOrWhiteSpace(UnitId))
                complexCondition.Add(new EqualsCondition() { FieldName = "UnitID", Value = CurrentUserInfo.CurrentUserRole.UnitId });
            else
                return rd;
            #endregion
            //排序参数
            List<OrderBy> lstOrder = new List<OrderBy> { };
            lstOrder.Add(new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Desc });


            var Result = VipAmountdetailBLL.PagedQuery(complexCondition.ToArray(), lstOrder.ToArray(), para.pageSize, para.pageIndex);
            rd.TotalPageCount = Result.PageCount;
            rd.TotalCount = Result.RowCount;

            rd.VipAmountdetailList = Result.Entities.Select(t => new VipAmountdetailData()
            {
                VipID = t.VipId,
                VipName = "",
                CreateTime = t.CreateTime.ToString(),
                SalesAmout = t.SalesAmount??0,
                Amout =t.Amount??0,
                CreateBy=t.CreateBy
            }).ToList();


            foreach (var item in rd.VipAmountdetailList)
            {
                //会员名称
                var VipData = VipBLL.GetByID(item.VipID);
                if (VipData != null)
                {
                    item.VipName = VipData.VipName ?? "";
                    item.HeadImgUrl = VipData.HeadImgUrl ?? "";
                }

                //服务人员名称
                var UserData = UserBLL.GetByID(item.CreateBy);
                if (UserData != null)
                    item.CreateByName = UserData.user_name ?? "";
            }

            return rd;
        }
    }
}