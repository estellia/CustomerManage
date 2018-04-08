/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-7-6 16:02:58
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
    public partial class T_ItemBLL
    {
        /// <summary>
        /// �����Ʒ
        /// </summary>
        /// <param name="itemEntity"></param>
        /// <param name="ItemCategoryCode"></param>
        public void AddItem(T_ItemEntity itemEntity, out T_SkuEntity skuEntity,string ItemCategoryName, string ItemCategoryCode, string SkuOriginPrice, string SkuSalesPrice)
        {
            var skuBll = new T_SkuBLL(CurrentUserInfo);
            var skuPriceBll = new T_Sku_PriceBLL(CurrentUserInfo);
            var skuProperty = new T_Sku_PropertyBLL(CurrentUserInfo);
            var itemCategoryBll = new T_Item_CategoryBLL(CurrentUserInfo);

            string ItemCategoryId = itemCategoryBll.CreateOrUpdateItemCategory(ItemCategoryName, ItemCategoryCode, CurrentUserInfo.ClientID, "0000", "1");

            T_ItemEntity NewEntity = new T_ItemEntity();
            NewEntity.item_id = Guid.NewGuid().ToString("N");
            NewEntity.item_category_id = ItemCategoryId;
            NewEntity.item_code = itemEntity.item_code;
            NewEntity.item_name = itemEntity.item_name;
            NewEntity.status = "-1";
            NewEntity.status_desc = "�¼�";
            NewEntity.create_user_id = "open";
            NewEntity.create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            NewEntity.modify_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            NewEntity.if_flag = "0";
            NewEntity.ifservice = 0; //  LM,2016/03/30
            NewEntity.IsGB = 0;   //  LM,2016,03,30
            NewEntity.ifgifts = 0;
            NewEntity.data_from = "18";//������Ʒ��Դ
            NewEntity.ifoften = 0;
            NewEntity.display_index = 1;
            NewEntity.CustomerId = CurrentUserInfo.ClientID;
            Create(NewEntity);

            //�������|��ȡ�����Ϣ
            skuEntity = skuBll.CreateNewSku(NewEntity.item_id);

            if (!String.IsNullOrEmpty(skuEntity.sku_id))
            {
                //��Ʒ���۸�   ԭ��|���ۼ�|���|����
                string SaleCount = "0";  //����
                string StoreCount = "0";   //���
                skuPriceBll.CreateNewSkuPrice(skuEntity.sku_id, SkuOriginPrice, SkuSalesPrice, StoreCount, SaleCount, CurrentUserInfo.ClientID);
            }
        }
    }
}