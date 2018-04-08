/*
 * Author		:Bear.Shens
 * EMail		:junhong.shen@zmind.cn  
 * Company		:
 * Create On	:2016/9/25 
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
using JIT.CPOS.BS.BLL;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.ERPAPI.Requests;
using JIT.CPOS.DTO.Module.ERPAPI.Response;

namespace JIT.CPOS.Web.ApplicationInterface.Module.ERPAPI
{
    public class ReturnOrderToHeadAH : BaseActionHandler<ReturnOrderToHeadRP, ReturnOrderToHeadRD>
    {
        protected override ReturnOrderToHeadRD ProcessRequest(APIRequest<ReturnOrderToHeadRP> pRequest)
        {
            var rd = new ReturnOrderToHeadRD();

            #region 错误码

            const int ERROR_ORDERID_NOEXISTS = 301;

            #endregion

            var orderId = pRequest.Parameters.OrderID; //订单ID
            var returnUnit = pRequest.Parameters.UnitID; //退回部门经

            T_InoutBLL tInoutbll = new T_InoutBLL(this.CurrentUserInfo); //订单表
            var pTran = tInoutbll.GetTran();

            #region 更新订单门店编号

            using (pTran.Connection)
            {
                try
                {
                    //根据订单ID获取实例
                    var entity = tInoutbll.GetByID(orderId);
                    if (entity == null)
                    {
                        throw new APIException(string.Format("未找到对应OrderID：{0}订单", orderId))
                        {
                            ErrorCode = ERROR_ORDERID_NOEXISTS
                        };
                    }
                    entity.sales_unit_id = returnUnit;
#warning 更新操作user_id

                    #region

                    entity.modify_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //更新时间
                    entity.modify_user_id = CurrentUserInfo.UserID; //更新人 

                    #endregion

                    tInoutbll.Update(entity, pTran); //用事物更新订单表（T_Inout）表中信息
                    pTran.Commit(); //提交事物
                }
                catch (Exception ex)
                {
                    pTran.Rollback();
                    throw new APIException(ex.Message);
                }

                #endregion

                return rd;
            }
        }
    }
}