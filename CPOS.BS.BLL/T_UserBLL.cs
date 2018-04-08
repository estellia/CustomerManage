
/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-06-08 20:59:54
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
    public partial class T_UserBLL
    {
        /// <summary>
        /// 得到门店员工列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public T_UserEntity[] GetEntitiesByCustomerIdUnitId(string customerId, string unitId)
        {
            //查询
            return _currentDAO.GetEntitiesByCustomerIdUnitId(customerId, unitId);
        }

        public void AddUserRole(TUserRoleEntity entity)
        {
            _currentDAO.AddUserRole(entity);
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="userEntity"></param>
        /// <param name="unitEntity"></param>
        /// <param name="roleEntity"></param>
        public void AddUser(ref T_UserEntity userEntity, t_unitEntity unitEntity, T_RoleEntity roleEntity)
        {
            //新增员工
            userEntity.user_status = "1";
            userEntity.user_status_desc = "有效";

            userEntity.user_id = Guid.NewGuid().ToString("N");
            userEntity.user_password = "21218cca77804d2ba1922c33e0151105"; //888888
            userEntity.fail_date = DateTime.Now.AddYears(20).ToString("yyyy-MM-dd");
            userEntity.customer_id = CurrentUserInfo.ClientID;
            userEntity.create_user_id = "open";
            userEntity.create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Create(userEntity);

            //新增员工角色
            var userRoleEntity = new TUserRoleEntity();
            userRoleEntity.user_role_id = Guid.NewGuid().ToString();
            userRoleEntity.user_id = userEntity.user_id;
            userRoleEntity.role_id = roleEntity.role_id;
            userRoleEntity.unit_id = unitEntity.unit_id;
            userRoleEntity.status = "1";
            userRoleEntity.create_time = DateTime.Now;
            userRoleEntity.create_user_id = "open";
            userRoleEntity.modify_time = DateTime.Now;
            userRoleEntity.modify_user_id = "open";
            userRoleEntity.default_flag = "1";
            AddUserRole(userRoleEntity);
        }
    }
}