﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JIT.CPOS.DTO.Module.VIP.Login.Response;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.DTO.Module.VIP.Login.Request;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.BS.BLL;
using System.Data;
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess.Query;

namespace JIT.CPOS.Web.ApplicationInterface.Module.VIP.Login
{
    public class SetSignInAH : BaseActionHandler<SetSignInRP, SetSignInRD>
    {
        #region 错误码
        private const int Error_CustomerCode_NotNull = 103;
        public const int Error_CustomerCode_NotExist = 104;
        public const int Error_UserName_InValid = 105;
        public const int Error_Password_InValid = 106;
        public const int Error_UserRole_NotExist = 107;
        #endregion

        protected override SetSignInRD ProcessRequest(DTO.Base.APIRequest<SetSignInRP> pRequest)
        {
            SetSignInRD rd = new SetSignInRD();

            string customerCode = pRequest.Parameters.CustomerCode;

            string phone = pRequest.Parameters.LoginName;

            string password = pRequest.Parameters.Password;

            if (string.IsNullOrEmpty(customerCode))
            {
                throw new APIException("客户代码不能为空") { ErrorCode = Error_CustomerCode_NotNull };
            }

            WMenuBLL menuServer = new WMenuBLL(Default.GetAPLoggingSession(""));
            string customerId = menuServer.GetCustomerIDByCustomerCode(customerCode);

            if (string.IsNullOrEmpty(customerId))
            {
                throw new APIException("客户代码对应的客户不存在") { ErrorCode = Error_CustomerCode_NotExist };
            }
            var currentUserInfo = Default.GetBSLoggingSession(customerId, "1");

            VipBLL vipBll = new VipBLL(currentUserInfo);

            #region 判断用户是否存在
            if (!vipBll.JudgeUserExist(phone, customerId))
            {
                throw new APIException("用户名无效") { ErrorCode = Error_UserName_InValid };
            }

            #endregion

            #region 判断密码是否正确
            if (!vipBll.JudgeUserPasswordExist(phone, customerId, password))
            {
                throw new APIException("登录密码错误") { ErrorCode = Error_Password_InValid };
            }

            #endregion
            #region 判断是否有登录连锁掌柜App权限
            var userRolesDs = vipBll.GetUserRoles(phone, customerId, password);
            bool flag = false;
            foreach (DataRow row in userRolesDs.Tables[0].Rows)
            {
                if (row["Def_App_Code"].ToString().ToUpper() == "APP")
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new APIException("该账号无权登录本系统") { ErrorCode = Error_Password_InValid };
            }
            #endregion
            #region 判断该客服人员是否有客服或操作订单的权限
            //if (!vipBll.JudgeUserRoleExist(phone, customerId, password))
            //{
            //    throw new APIException("该账号无权登录本系统") { ErrorCode = Error_Password_InValid };
            //}
            #endregion

            #region 获取UserId
            var ds = vipBll.GetUserIdByUserNameAndPassword(phone, customerId, password);
            rd.UserId = ds.Tables[0].Rows[0]["user_id"].ToString();
            rd.UserName = ds.Tables[0].Rows[0]["user_name"].ToString();
            rd.Status = int.Parse(ds.Tables[0].Rows[0]["user_status"].ToString());
            rd.CustomerId = customerId;
            var T_SuperRetailTraderbll = new T_SuperRetailTraderBLL(currentUserInfo);
            var T_SuperRetailTraderInfo = T_SuperRetailTraderbll.QueryByEntity(new T_SuperRetailTraderEntity() { CustomerId = customerId, SuperRetailTraderFromId = rd.UserId, SuperRetailTraderFrom = "User" }, new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Desc } }).FirstOrDefault();
            if (T_SuperRetailTraderInfo != null)
            {
                rd.SuperRetailTraderID = T_SuperRetailTraderInfo.SuperRetailTraderID.ToString();
            }
            #endregion
            //如果状态不等于1，就说明已经停用，
            if (rd.Status != 1)
            {
                throw new APIException("该员工已经被停用，请联系管理员") { ErrorCode = Error_Password_InValid };
            }


            #region 获取角色列表
            var roleCodeDs = vipBll.GetRoleCodeByUserId(rd.UserId, customerId);

            var tmp = roleCodeDs.Tables[0].AsEnumerable().Select(t => new RoleCodeInfo()
            {
                RoleCode = t["role_code"].ToString()
            });

            #endregion
            rd.UnitId = vipBll.GetUnitByUserId(rd.UserId);//获取会集店
            TUnitBLL tUnitBLL = new TUnitBLL(currentUserInfo);
            if (!string.IsNullOrEmpty(rd.UnitId))
            {
                rd.UnitName = tUnitBLL.GetByID(rd.UnitId).UnitName;
            }
            else
            {
                rd.UnitName = "";
            }


            //app登陆用户权限 add by henry 2015-3-26
            var roleCodeList = vipBll.GetAppMenuByUserId(rd.UserId);


            //app登陆用户权限 add by henry 2015-3-26
            List<string> lst = new List<string>();
            if (roleCodeDs.Tables[0] != null && roleCodeDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in roleCodeDs.Tables[0].Rows)
                {
                    var menuList = new JIT.CPOS.BS.DataAccess.AppSysService(currentUserInfo).GetRoleMenus(currentUserInfo.ClientID, item["role_id"] + "");
                    if (menuList != null)
                    {
                        lst.AddRange(menuList.Select(m => m.Menu_Code).Distinct().ToList());
                    }
                }
            }

            rd.MenuCodeList = new List<Menu>();
            foreach (var item in lst.Distinct().ToList())
            {
                rd.MenuCodeList.Add(new Menu() { MenuCode = item });
            }

            //if (roleCodeList != null)
            //{
            //    rd.MenuCodeList = DataTableToObject.ConvertToList<Menu>(roleCodeList.Tables[0]);
            //}

            rd.RoleCodeList = tmp.ToArray();
            rd.CustomerName = currentUserInfo.ClientName;


            //销售员头像
            ObjectImagesBLL _ObjectImagesBLL = new ObjectImagesBLL(currentUserInfo);
            ObjectImagesEntity en = new ObjectImagesEntity();
            en.ObjectId = rd.UserId;//根据获取的用户ID
            List<ObjectImagesEntity> ImgList = _ObjectImagesBLL.QueryByEntity(en, null).OrderByDescending(p => p.CreateTime).ToList();
            if (ImgList != null && ImgList.Count != 0)
            {
                // string fileDNS = customerBasicSettingBll.GetSettingValueByCode("FileDNS"); ;//http://182.254.156.57:811
                rd.HeadImg = ImgList[0].ImageURL;
            }


            #region 获取appLogo
            //基础数据初始化
            CustomerBasicSettingBLL customerBasicSettingBLL = new CustomerBasicSettingBLL(currentUserInfo);
            List<CustomerBasicCodeInfo> customerBasicCodeList = new List<CustomerBasicCodeInfo>();
            //查询条件
            CustomerBasicSettingEntity customerBasicSettingEntity = new CustomerBasicSettingEntity()
            {
                CustomerID = currentUserInfo.ClientID,
                SettingCode = "WebLogo"
            };
            //执行查询
            List<CustomerBasicSettingEntity> customerBasicSettingEntityList = customerBasicSettingBLL.QueryByEntity(customerBasicSettingEntity, null).ToList();

            foreach (var a in customerBasicSettingEntityList)
            {
                CustomerBasicCodeInfo customerBasicCodeInfo = new CustomerBasicCodeInfo();

                if (a.SettingCode.Equals("WebLogo"))
                {
                    customerBasicCodeInfo.WebLogo = a.SettingValue.ToString();
                }
                customerBasicCodeList.Add(customerBasicCodeInfo);

            }

            rd.CustomerBasicCodeList = customerBasicCodeList;


            #endregion
            return rd;
        }
    }
}