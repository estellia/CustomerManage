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
using System.Data;
using System.Data.SqlClient;
using System.Text;

using JIT.Utility;
using JIT.Utility.Entity;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.DataAccess;
using JIT.Utility.Log;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.DataAccess.Base;

namespace JIT.CPOS.BS.DataAccess
{

    /// <summary>
    /// ���ݷ��ʣ�  
    /// ��T_Menu�����ݷ����� 
    /// TODO:
    /// 1.ʵ��ICRUDable�ӿ�
    /// 2.ʵ��IQueryable�ӿ�
    /// 3.ʵ��Load����
    /// </summary>
    public partial class T_MenuDAO : Base.BaseCPOSDAO, ICRUDable<T_MenuEntity>, IQueryable<T_MenuEntity>
    {
        /// <summary>
        /// �����޸Ĳ˵�����Ȩ��
        /// </summary>
        /// <param name="customerId"></param>

        public void UpdateIsCanAccess(string customerId)
        {
            string strSql = "UPDATE T_MENU SET ISCANACCESS=1 WHERE CUSTOMER_ID=@CUSTOMER_ID";
            SqlParameter[] para = new SqlParameter[] { 
            new SqlParameter("@CUSTOMER_ID",customerId)
            };
            this.SQLHelper.ExecuteDataset(CommandType.Text, strSql, para);
        }
		public void UpgradeMenuFromAp(string strCustomerId,string strVocaVerMappingID) {
			string strSql = string.Format(@"
					DELETE  FROM T_Menu
					WHERE   customer_id = '{0}'
					INSERT T_Menu
							( menu_id ,
							  reg_app_id ,
							  menu_code ,
							  parent_menu_id ,
							  menu_level ,
							  url_path ,
							  icon_path ,
							  display_index ,
							  menu_name ,
							  user_flag ,
							  menu_eng_name ,
							  status ,
							  create_user_id ,
							  create_time ,
							  modify_user_id ,
							  modify_time ,
							  customer_id ,
							  IsCanAccess
							)
							SELECT  menu_id + '{0}' ,  ---��Ϊģ���̻���Ĳ˵��е�������ӵģ������ڹ���ƽ̨��ӵģ����Բ����̻�id 
									app_id ,
									menu_code ,
									( CASE WHEN parent_menu_id = '--' THEN '--'
										   ELSE parent_menu_id +'{0}'
									  END ) ,
									menu_level ,
									url_path ,
									icon_path ,
									display_index ,
									menu_name ,
									1 ,
									menu_name_en ,
									menu_status ,    ----user_flag����Ϊ��1 
									create_user_id ,
									create_time ,
									modify_user_id ,
									modify_time ,
									'{0}' ,
									1
							FROM    cpos_ap..t_def_menu dm
									INNER JOIN cpos_ap..T_VocationVersionMenuMapping vvm ON dm.menu_id = vvm.menuid
							WHERE   customer_visible = 1
									AND vvm.isdelete = 0
									AND vvm.STATUS = 1
									AND vvm.VocaVerMappingID = '{1}'
									AND menu_status = 1

", strCustomerId,strVocaVerMappingID);
			this.SQLHelper.ExecuteNonQuery(strSql);
		}

	}
}
