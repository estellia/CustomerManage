using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.VIP.VipGold.Request;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.Utility.DataAccess.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xgx.SyncData.Common;
using Zmind.EventBus.Contract;

namespace JIT.CPOS.Web.ApplicationInterface.Module.VIP.VipGolden
{
    /// <summary>
    /// 变更卡等级信息
    /// </summary>
    public class UpdateVipCardTypeAH : BaseActionHandler<UpdateVipCardTypeRP, EmptyResponseData>
    {
        protected override EmptyResponseData ProcessRequest(APIRequest<UpdateVipCardTypeRP> pRequest)
        {
            var rd = new EmptyResponseData();
            var para = pRequest.Parameters;
            LoggingSessionInfo loggingSessionInfo = Default.GetBSLoggingSession(pRequest.CustomerID, pRequest.UserID);
            var bllVip = new VipBLL(loggingSessionInfo);
            var bllVipCardVipMapping = new VipCardVipMappingBLL(loggingSessionInfo);
            //获取当前会员信息
            List<IWhereCondition> wheres = new List<IWhereCondition>();
            wheres.Add(new EqualsCondition() { FieldName = "clientid", Value = pRequest.CustomerID });
            wheres.Add(new EqualsCondition() { FieldName = "VipID", Value = pRequest.UserID });
            wheres.Add(new EqualsCondition() { FieldName = "Status", Value = 2 });
            wheres.Add(new DirectCondition("Phone!=''"));
            var vipInfo = bllVip.Query(wheres.ToArray(), null).FirstOrDefault();           
            try
            {
                string strObjectNo=string.Empty;
                if (vipInfo != null)
                {
                    //根据当前会员执行变更卡操作
                    strObjectNo=bllVipCardVipMapping.BindVirtualItem(vipInfo.VIPID, vipInfo.VipCode, vipInfo.CouponInfo, para.VipCardTypeID);
                    if (string.IsNullOrEmpty(strObjectNo))
                    {
                        throw new APIException("绑定失败！") { ErrorCode = ERROR_CODES.INVALID_BUSINESS };
                    }
                    else
                    {
                        //同步相关会员信息(姓名、性别、生日、邮箱、积分、消费金额、消费次数、状态) 
                        bllVip.MergeVipInfo(pRequest.CustomerID, pRequest.UserID, vipInfo.Phone,para.BindVipID);
                        //记录会员信息改变，并且把旧的会员的标识也放在契约里
                        var eventService = new EventService();
                        var vipMsg = new EventContract
                        {
                            Operation = OptEnum.Update,
                            EntityType = EntityTypeEnum.Vip,
                            Id = pRequest.UserID,
                            OtherCon = para.BindVipID
                        };
                        eventService.PublishMsg(vipMsg);

                        throw new APIException("绑定成功！") { ErrorCode = 0 };
                    }
                }
            }
            catch (APIException ex) { throw ex; }
            return rd;
        }
    }
}