/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2016-8-20 14:58:24
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
    public partial class T_MenuBLL
    {
        /// <summary>
        /// 批量修改菜单操作权限
        /// </summary>
        /// <param name="customerId"></param>

        public void UpdateIsCanAccess(string customerId)
        {
            this._currentDAO.UpdateIsCanAccess(customerId);
        }
		public void UpgradeMenuFromAp(string strCustomerId, string strVocaVerMappingID) {
			this._currentDAO.UpgradeMenuFromAp(strCustomerId, strVocaVerMappingID);

		}
		/// <summary>
		/// 根据行业版本升级套餐
		/// </summary>
		public void UpgradeMenu(string strCustomerId,string strVocaVerMappingID)
        {
			UpgradeMenuFromAp(strCustomerId, strVocaVerMappingID);
			//处理“发送通知”一级菜单
			T_MenuEntity fstzMenuInfo = this._currentDAO.QueryByEntity(new T_MenuEntity() { menu_code = "fstz", customer_id = CurrentUserInfo.ClientID }, null).FirstOrDefault();
            if (fstzMenuInfo != null)//菜单存在
            {
                fstzMenuInfo.status = 1;
                this._currentDAO.Update(fstzMenuInfo);
            }
            else//菜单不存在
            {
                fstzMenuInfo=new T_MenuEntity();
                fstzMenuInfo.menu_id = "40E5AED8066044CA9B23DEE269901FF5" + CurrentUserInfo.ClientID;//主库menuId+商户ID组成
                fstzMenuInfo.reg_app_id = "D8C5FF6041AA4EA19D83F924DBF56F93";//商户后台的应用ID
                fstzMenuInfo.menu_code = "fstz";//编码
                fstzMenuInfo.parent_menu_id = "-";//父节点
                fstzMenuInfo.menu_level = 1;
                fstzMenuInfo.url_path = "";
                fstzMenuInfo.icon_path = "";
                fstzMenuInfo.display_index = 11;//排序
                fstzMenuInfo.menu_name = "发送通知";//菜单名称
                fstzMenuInfo.user_flag = 1;
                fstzMenuInfo.menu_eng_name = "";
                fstzMenuInfo.status = 1;//正常
                fstzMenuInfo.create_user_id = "system";
                fstzMenuInfo.create_time = DateTime.Now.ToString();
                fstzMenuInfo.modify_user_id = "system";
                fstzMenuInfo.modify_time = DateTime.Now.ToString();
                fstzMenuInfo.customer_id = CurrentUserInfo.ClientID;
                fstzMenuInfo.IsCanAccess = 1;//可操作
                this._currentDAO.Create(fstzMenuInfo);
            }
            //处理“发送通知”二级菜单
            T_MenuEntity fstz2MenuInfo = this._currentDAO.QueryByEntity(new T_MenuEntity() { menu_code = "fstz_fstz", customer_id = CurrentUserInfo.ClientID }, null).FirstOrDefault();
            if (fstz2MenuInfo != null)//菜单存在
            {
                fstz2MenuInfo.status = 1;
                this._currentDAO.Update(fstz2MenuInfo);
            }
            else//菜单不存在
            {
                fstz2MenuInfo = new T_MenuEntity();
                fstz2MenuInfo.menu_id = "2857DDEC054A4AC9A05E2BBF5D384447" + CurrentUserInfo.ClientID;//主库menuId+商户ID组成
                fstz2MenuInfo.reg_app_id = "D8C5FF6041AA4EA19D83F924DBF56F93";//商户后台的应用ID
                fstz2MenuInfo.menu_code = "fstz_fstz";//编码
                fstz2MenuInfo.parent_menu_id = "40E5AED8066044CA9B23DEE269901FF5" + CurrentUserInfo.ClientID;//父节点
                fstz2MenuInfo.menu_level = 2;
                fstz2MenuInfo.url_path = "";
                fstz2MenuInfo.icon_path = "icon_fstz_fstz";
                fstz2MenuInfo.display_index = 1;//排序
                fstz2MenuInfo.menu_name = "发送通知";//菜单名称
                fstz2MenuInfo.user_flag = 1;
                fstz2MenuInfo.menu_eng_name = "";
                fstz2MenuInfo.status = 1;//正常
                fstz2MenuInfo.create_user_id = "system";
                fstz2MenuInfo.create_time = DateTime.Now.ToString();
                fstz2MenuInfo.modify_user_id = "system";
                fstz2MenuInfo.modify_time = DateTime.Now.ToString();
                fstz2MenuInfo.customer_id = CurrentUserInfo.ClientID;
                fstz2MenuInfo.IsCanAccess = 1;//可操作
                this._currentDAO.Create(fstz2MenuInfo);
            }
            //处理“发送通知”三级菜单
            T_MenuEntity zbxxMenuInfo = this._currentDAO.QueryByEntity(new T_MenuEntity() { menu_code = "fsxx_fsxx_zbxx", customer_id = CurrentUserInfo.ClientID }, null).FirstOrDefault();
            if (zbxxMenuInfo != null)//菜单存在
            {
                zbxxMenuInfo.status = 1;
                this._currentDAO.Update(zbxxMenuInfo);
            }
            else//菜单不存在
            {
                zbxxMenuInfo = new T_MenuEntity();
                zbxxMenuInfo.menu_id = "9F17BD379EB34FD1B5EF85D721901008" + CurrentUserInfo.ClientID;//主库menuId+商户ID组成
                zbxxMenuInfo.reg_app_id = "D8C5FF6041AA4EA19D83F924DBF56F93";//商户后台的应用ID
                zbxxMenuInfo.menu_code = "fsxx_fsxx_zbxx";//编码
                zbxxMenuInfo.parent_menu_id = "2857DDEC054A4AC9A05E2BBF5D384447" + CurrentUserInfo.ClientID;//父节点
                zbxxMenuInfo.menu_level = 3;
                zbxxMenuInfo.url_path = "/module/massTexting/internalMessage.aspx";
                zbxxMenuInfo.icon_path = "";
                zbxxMenuInfo.display_index = 1;//排序
                zbxxMenuInfo.menu_name = "总部消息";//菜单名称
                zbxxMenuInfo.user_flag = 1;
                zbxxMenuInfo.menu_eng_name = "";
                zbxxMenuInfo.status = 1;//正常
                zbxxMenuInfo.create_user_id = "system";
                zbxxMenuInfo.create_time = DateTime.Now.ToString();
                zbxxMenuInfo.modify_user_id = "system";
                zbxxMenuInfo.modify_time = DateTime.Now.ToString();
                zbxxMenuInfo.customer_id = CurrentUserInfo.ClientID;
                zbxxMenuInfo.IsCanAccess = 1;//可操作
                this._currentDAO.Create(zbxxMenuInfo);
            }
        }
    }
}