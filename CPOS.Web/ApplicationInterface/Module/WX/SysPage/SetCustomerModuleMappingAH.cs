/*
 * Author		:Alex.Tian
 * EMail		:changjian.tian@jitmarketing.cn
 * Company		:JIT
 * Create On	:2014/5/23 09:35
 * Description	:设置客户模板映射
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
using System.Web;

using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.DTO.Module.WeiXin.SysPage.Request;
using JIT.CPOS.DTO.Module.WeiXin.SysPage.Response;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.Utility.ExtensionMethod;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.BS.BLL.AP;

namespace JIT.CPOS.Web.ApplicationInterface.Module.WX.SysPage
{
    public class SetCustomerModuleMappingAH : BaseActionHandler<SetCustomerModuleMappingRP, SetCustomerModuleMappingRD>
    {
        protected override SetCustomerModuleMappingRD ProcessRequest(APIRequest<SetCustomerModuleMappingRP> pRequest)
        {
            SetCustomerModuleMappingRD rd = new SetCustomerModuleMappingRD();
            var para = pRequest.Parameters;

            //获取当前商户的数据库链接信息
            var customerCurrentUserInfo = this.CurrentUserInfo;
            if (string.IsNullOrEmpty(this.CurrentUserInfo.Conn))
                customerCurrentUserInfo = Default.GetBSLoggingSession(para.CustomerID, "system");
            
            //获取主库数据库链接信息
            var userInfo = this.CurrentUserInfo;
            userInfo.CurrentLoggingManager.Connection_String = System.Configuration.ConfigurationManager.AppSettings["Conn_ap"];
            CustomerModuleMappingBLL bll = new CustomerModuleMappingBLL(userInfo);
            CustomerModuleMappingEntity entity = new CustomerModuleMappingEntity();

            SysVocationVersionMappingBLL vocationVersionMappingBLL = new SysVocationVersionMappingBLL(userInfo);//行业版本关系业务逻辑层
            T_MenuBLL menuBLL = new T_MenuBLL(customerCurrentUserInfo);//菜单业务逻辑层


            string strArrVocaverMappingID = para.VocaVerMappingID.ToString();
            // string[] strArrVocaverMappingID = str.Split(',');
            var tran = bll.GetTran();
            using (tran.Connection)
            {
                try
                {
                    //for (int i = 0; i < strArrVocaverMappingID.Length; i++)
                    //{
                    //var tempList = bll.QueryByEntity(new CustomerModuleMappingEntity { CustomerID = para.CustomerID, VocaVerMappingID = Guid.Parse(strArrVocaverMappingID[i].ToString()) }, null);  //之前是判断只有一个套餐。现在可以有多个套餐
                    if (strArrVocaverMappingID.Contains(','))
                    {
                        throw new APIException(string.Format("不能应用到多个套餐。")) { ErrorCode = 301 };
                    }
                    else
                    {
                        var tempList = bll.QueryByEntity(new CustomerModuleMappingEntity { CustomerID = para.CustomerID }, null);
                        if (tempList.Length > 0)  //如果存在该客户的信息。则更新
                        {
                            string MappingId = tempList[0].MappingID.ToString();
                            entity = new CustomerModuleMappingEntity();
                            entity.MappingID = Guid.Parse(MappingId);
                            entity.VocaVerMappingID = Guid.Parse(strArrVocaverMappingID.ToString());
                            entity.CustomerID = para.CustomerID;
                            bll.Update(entity);
                        }
                        else
                        {
                            entity = new CustomerModuleMappingEntity();
                            entity.MappingID = Guid.NewGuid();
                            entity.VocaVerMappingID = Guid.Parse(strArrVocaverMappingID.ToString());
                            entity.CustomerID = para.CustomerID;
                            bll.Create(entity);
                        }
                    }

					/////针对集客宝版本，更新CustomerEx中的限制门店数(建仙代码，稍后他合并上来)
					var bllAPCommon = new APCommonBLL(userInfo);
					bllAPCommon.UpdateCustomerEx(para.CustomerID, userInfo.CurrentLoggingManager.User_Name);
					////}

					//更新菜单和可操作权限
					var vocationVersionMappingInfo = vocationVersionMappingBLL.GetByID(para.VocaVerMappingID);
                    if (vocationVersionMappingInfo != null)
                    {
                        if (vocationVersionMappingInfo.VersionID != 3)//版本不等于集客宝版本时更新
                        {
                            //批量修改菜单操作权限
                            menuBLL.UpdateIsCanAccess(para.CustomerID);
                            //设置隐藏的菜单可用（紧急方案：集客宝中只包含发送通知隐藏）
                            menuBLL.UpgradeMenu(para.CustomerID, vocationVersionMappingInfo.VocaVerMappingID.ToString());
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();//回滚事物

                    throw new APIException(ex.Message);
                }
            }
            return rd;
        }
    }
}