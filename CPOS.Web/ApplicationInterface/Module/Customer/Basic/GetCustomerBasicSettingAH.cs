﻿using JIT.CPOS.BS.BLL;
using JIT.CPOS.DTO.Module.Basic.Customer.Request;
using JIT.CPOS.DTO.Module.Basic.Customer.Response;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.Utility.DataAccess.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JIT.CPOS.Web.ApplicationInterface.Module.Customer.Basic
{
    public class GetCustomerBasicSettingAH : BaseActionHandler<GetCustomerBasicSettingRP, GetCustomerBasicSettingRD>
    {
        protected override GetCustomerBasicSettingRD ProcessRequest(DTO.Base.APIRequest<GetCustomerBasicSettingRP> pRequest)
        {
            var rd = new GetCustomerBasicSettingRD();
            var para = pRequest.Parameters;
            var customerBasicSettingBLL = new CustomerBasicSettingBLL(CurrentUserInfo);

            if (!string.IsNullOrWhiteSpace(para.SettingCode))
            {
                var Data =new  JIT.CPOS.BS.Entity.CustomerBasicSettingEntity();
                JIT.CPOS.BS.BLL.RedisOperationBLL.BasicSetting.BasicSettingBLL bllBasicSetting = new JIT.CPOS.BS.BLL.RedisOperationBLL.BasicSetting.BasicSettingBLL();
                var basicSettingList = bllBasicSetting.GetBasicSetting(CurrentUserInfo.ClientID);
                if (basicSettingList != null && basicSettingList.Count > 0)
                {
                    Data = basicSettingList.Where(b => b.SettingCode == para.SettingCode).FirstOrDefault();
                }
                else
                {
                    //查询参数
                    var complexCondition = new List<IWhereCondition> { };
                    complexCondition.Add(new EqualsCondition() { FieldName = "CustomerID", Value = CurrentUserInfo.ClientID });
                    complexCondition.Add(new DirectCondition("SettingCode='" + para.SettingCode + "' "));
                    Data = customerBasicSettingBLL.Query(complexCondition.ToArray(), null).FirstOrDefault();
                }
                if (Data != null)
                    rd.SettingValue = Data.SettingValue;
                else
                    rd.SettingValue = "欢迎光临!请问有什么可以帮助到您?";
            }
            return rd;
        }
    }
}