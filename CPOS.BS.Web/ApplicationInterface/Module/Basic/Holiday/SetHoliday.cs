﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JIT.CPOS.BS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.Web.Session;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.BLL;
using JIT.Utility.DataAccess;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Basic.Holiday.Request;
namespace JIT.CPOS.BS.Web.ApplicationInterface.Module.Basic.Holiday
{
    public class SetHolidayAH : BaseActionHandler<SetHolidayRP, EmptyResponseData>
    {
        protected override EmptyResponseData ProcessRequest(DTO.Base.APIRequest<SetHolidayRP> pRequest)
        {
            var rd = new EmptyResponseData();
            var para = pRequest.Parameters;
            var loggingSessionInfo = new SessionManager().CurrentUserLoginInfo;

            var HolidayBLL = new HolidayBLL(loggingSessionInfo);
            var SpecialDataBLL=new SpecialDateBLL(loggingSessionInfo);
            try
            {
                if (!string.IsNullOrWhiteSpace(para.HolidayId))
                {
                    SpecialDateEntity SpecialDate = SpecialDataBLL.QueryByEntity(new SpecialDateEntity() { HolidayID = para.HolidayId }, null).FirstOrDefault();
                    if (SpecialDate != null) {
                        throw new APIException("该假日正在使用中不能更改或删除哦！") { ErrorCode = ERROR_CODES.INVALID_BUSINESS };
                    }
                    //更新
                    HolidayEntity UpdateData = HolidayBLL.GetByID(para.HolidayId);
                    if (UpdateData == null)
                    {
                        throw new APIException("假日对象为NULL！") { ErrorCode = ERROR_CODES.INVALID_BUSINESS };
                    }
                    UpdateData.HolidayName = para.HolidayName;
                    UpdateData.BeginDate = para.BeginDate;
                    UpdateData.EndDate = para.EndDate.AddHours(23).AddMinutes(59);
                    UpdateData.Desciption = para.Desciption;
                    UpdateData.CustomerID = loggingSessionInfo.ClientID;
                    //执行
                    HolidayBLL.Update(UpdateData);
                }
                else
                {
                    
                    //新增
                    HolidayEntity AddData = new HolidayEntity();
                    //AddData.HolidayId = System.Guid.NewGuid().ToString(); 
                    AddData.HolidayName = para.HolidayName;
                    AddData.BeginDate = para.BeginDate;
                    AddData.EndDate = para.EndDate.AddHours(23).AddMinutes(59);
                    AddData.Desciption = para.Desciption;
                    AddData.CustomerID = loggingSessionInfo.ClientID;
                    //执行
                    HolidayBLL.Create(AddData);
                }
            }
            catch (APIException apiEx)
            {
                throw new APIException(apiEx.ErrorCode, apiEx.Message);
            }
            

            return rd;
        }
    }
}