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
    /// ҵ����  
    /// </summary>
    public partial class T_MenuBLL
    {
        /// <summary>
        /// �����޸Ĳ˵�����Ȩ��
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
		/// ������ҵ�汾�����ײ�
		/// </summary>
		public void UpgradeMenu(string strCustomerId,string strVocaVerMappingID)
        {
			UpgradeMenuFromAp(strCustomerId, strVocaVerMappingID);
			//��������֪ͨ��һ���˵�
			T_MenuEntity fstzMenuInfo = this._currentDAO.QueryByEntity(new T_MenuEntity() { menu_code = "fstz", customer_id = CurrentUserInfo.ClientID }, null).FirstOrDefault();
            if (fstzMenuInfo != null)//�˵�����
            {
                fstzMenuInfo.status = 1;
                this._currentDAO.Update(fstzMenuInfo);
            }
            else//�˵�������
            {
                fstzMenuInfo=new T_MenuEntity();
                fstzMenuInfo.menu_id = "40E5AED8066044CA9B23DEE269901FF5" + CurrentUserInfo.ClientID;//����menuId+�̻�ID���
                fstzMenuInfo.reg_app_id = "D8C5FF6041AA4EA19D83F924DBF56F93";//�̻���̨��Ӧ��ID
                fstzMenuInfo.menu_code = "fstz";//����
                fstzMenuInfo.parent_menu_id = "-";//���ڵ�
                fstzMenuInfo.menu_level = 1;
                fstzMenuInfo.url_path = "";
                fstzMenuInfo.icon_path = "";
                fstzMenuInfo.display_index = 11;//����
                fstzMenuInfo.menu_name = "����֪ͨ";//�˵�����
                fstzMenuInfo.user_flag = 1;
                fstzMenuInfo.menu_eng_name = "";
                fstzMenuInfo.status = 1;//����
                fstzMenuInfo.create_user_id = "system";
                fstzMenuInfo.create_time = DateTime.Now.ToString();
                fstzMenuInfo.modify_user_id = "system";
                fstzMenuInfo.modify_time = DateTime.Now.ToString();
                fstzMenuInfo.customer_id = CurrentUserInfo.ClientID;
                fstzMenuInfo.IsCanAccess = 1;//�ɲ���
                this._currentDAO.Create(fstzMenuInfo);
            }
            //��������֪ͨ�������˵�
            T_MenuEntity fstz2MenuInfo = this._currentDAO.QueryByEntity(new T_MenuEntity() { menu_code = "fstz_fstz", customer_id = CurrentUserInfo.ClientID }, null).FirstOrDefault();
            if (fstz2MenuInfo != null)//�˵�����
            {
                fstz2MenuInfo.status = 1;
                this._currentDAO.Update(fstz2MenuInfo);
            }
            else//�˵�������
            {
                fstz2MenuInfo = new T_MenuEntity();
                fstz2MenuInfo.menu_id = "2857DDEC054A4AC9A05E2BBF5D384447" + CurrentUserInfo.ClientID;//����menuId+�̻�ID���
                fstz2MenuInfo.reg_app_id = "D8C5FF6041AA4EA19D83F924DBF56F93";//�̻���̨��Ӧ��ID
                fstz2MenuInfo.menu_code = "fstz_fstz";//����
                fstz2MenuInfo.parent_menu_id = "40E5AED8066044CA9B23DEE269901FF5" + CurrentUserInfo.ClientID;//���ڵ�
                fstz2MenuInfo.menu_level = 2;
                fstz2MenuInfo.url_path = "";
                fstz2MenuInfo.icon_path = "icon_fstz_fstz";
                fstz2MenuInfo.display_index = 1;//����
                fstz2MenuInfo.menu_name = "����֪ͨ";//�˵�����
                fstz2MenuInfo.user_flag = 1;
                fstz2MenuInfo.menu_eng_name = "";
                fstz2MenuInfo.status = 1;//����
                fstz2MenuInfo.create_user_id = "system";
                fstz2MenuInfo.create_time = DateTime.Now.ToString();
                fstz2MenuInfo.modify_user_id = "system";
                fstz2MenuInfo.modify_time = DateTime.Now.ToString();
                fstz2MenuInfo.customer_id = CurrentUserInfo.ClientID;
                fstz2MenuInfo.IsCanAccess = 1;//�ɲ���
                this._currentDAO.Create(fstz2MenuInfo);
            }
            //��������֪ͨ�������˵�
            T_MenuEntity zbxxMenuInfo = this._currentDAO.QueryByEntity(new T_MenuEntity() { menu_code = "fsxx_fsxx_zbxx", customer_id = CurrentUserInfo.ClientID }, null).FirstOrDefault();
            if (zbxxMenuInfo != null)//�˵�����
            {
                zbxxMenuInfo.status = 1;
                this._currentDAO.Update(zbxxMenuInfo);
            }
            else//�˵�������
            {
                zbxxMenuInfo = new T_MenuEntity();
                zbxxMenuInfo.menu_id = "9F17BD379EB34FD1B5EF85D721901008" + CurrentUserInfo.ClientID;//����menuId+�̻�ID���
                zbxxMenuInfo.reg_app_id = "D8C5FF6041AA4EA19D83F924DBF56F93";//�̻���̨��Ӧ��ID
                zbxxMenuInfo.menu_code = "fsxx_fsxx_zbxx";//����
                zbxxMenuInfo.parent_menu_id = "2857DDEC054A4AC9A05E2BBF5D384447" + CurrentUserInfo.ClientID;//���ڵ�
                zbxxMenuInfo.menu_level = 3;
                zbxxMenuInfo.url_path = "/module/massTexting/internalMessage.aspx";
                zbxxMenuInfo.icon_path = "";
                zbxxMenuInfo.display_index = 1;//����
                zbxxMenuInfo.menu_name = "�ܲ���Ϣ";//�˵�����
                zbxxMenuInfo.user_flag = 1;
                zbxxMenuInfo.menu_eng_name = "";
                zbxxMenuInfo.status = 1;//����
                zbxxMenuInfo.create_user_id = "system";
                zbxxMenuInfo.create_time = DateTime.Now.ToString();
                zbxxMenuInfo.modify_user_id = "system";
                zbxxMenuInfo.modify_time = DateTime.Now.ToString();
                zbxxMenuInfo.customer_id = CurrentUserInfo.ClientID;
                zbxxMenuInfo.IsCanAccess = 1;//�ɲ���
                this._currentDAO.Create(zbxxMenuInfo);
            }
        }
    }
}