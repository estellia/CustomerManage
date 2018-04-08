/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015/7/6 16:12:44
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
    public partial class T_SkuBLL
    {
        /// <summary>
        /// �����Ʒ�����Ϣ
        /// </summary>
        /// <param name="ItemId">��Ʒ���</param>
        /// <param name="SkuBarcode">������</param>
        /// <param name="sku_prop_id1">��Ʒ�������</param>
        /// <param name="CustomerId">�̻����</param>
        /// <returns></returns>
        public T_SkuEntity CreateNewSku(string ItemId)
        {
            T_SkuEntity tSkuEntity = new T_SkuEntity();
            tSkuEntity.sku_id = Guid.NewGuid().ToString("N");
            //�������
            tSkuEntity.item_id = ItemId;
            tSkuEntity.barcode = "100000";

            //���������
            tSkuEntity.status = "1";
            tSkuEntity.create_user_id = "open";
            tSkuEntity.create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tSkuEntity.modify_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tSkuEntity.if_flag = "0";
            tSkuEntity.bat_id = "1";
            tSkuEntity.modify_user_id = "open";

            Create(tSkuEntity);
            return tSkuEntity;

        }
    }
}