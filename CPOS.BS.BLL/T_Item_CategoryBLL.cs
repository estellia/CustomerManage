/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015/7/6 16:12:43
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
    public partial class T_Item_CategoryBLL
    {
        public DataSet GetCategoryByCustomerId(string strCustomerId, string strBatId)
        {
          return  this._currentDAO.GetCategoryByCustomerId(strCustomerId, strBatId);
        }

        /// <summary>
        /// ��ȡ��ƷƷ�� ͨ����ƷƷ������
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public T_Item_CategoryEntity GetItemCategoryByCode(string Code, string CustomerId)
        {
            return this._currentDAO.GetItemCategoryByCode(Code, CustomerId);
        }

        public T_Item_CategoryEntity GetItemCategoryByCode(string CategoryName, string parentId, string CustomerId)
        {
            return this._currentDAO.GetItemCategoryByCode(CategoryName, parentId, CustomerId);
        }

        /// <summary>
        /// ���/��ȡ/�޸� ��Ʒ�����Ϣ  {���� ���� ��Ʒ ����}
        /// </summary>
        /// <param name="ItemCategoryName">�������</param>
        /// <param name="ItemCategoryCode">�����</param>
        /// <param name="CustomerID">�̻����</param>
        /// <param name="ParentItemCategoryCode">���ڵ���</param> 
        /// <param name="Status">״̬</param>
        public string CreateOrUpdateItemCategory(string ItemCategoryName, string ItemCategoryCode, string CustomerID, string ParentItemCategoryCode, string Status)
        {

            //�������
            T_Item_CategoryEntity itemcategory = new T_Item_CategoryEntity();
            itemcategory.item_category_name = ItemCategoryName;
            itemcategory.item_category_code = ItemCategoryCode;
            //���������
            itemcategory.CustomerID = CustomerID;
            itemcategory.item_category_id = Guid.NewGuid().ToString();
            itemcategory.status = "1";

            //ͨ����Ʒ����ź��̻��� ��ȡ��Ʒ�����Ϣ
            var ItemInfoCategory = GetItemCategoryByCode(ItemCategoryCode, CustomerID);

            //ͨ�����ڵ��Ż�ȡ ��Ʒ�����Ϣ
            var ParentItemInfoCategory = GetItemCategoryByCode(ParentItemCategoryCode, CustomerID);

            //������ڵ�Ϊ�� ���� ǰ��û�д��ݸ��ڵ� �����Ǿ�Ĭ�ϸ��ڵ���Ϣ
            if (ParentItemInfoCategory == null || (ParentItemCategoryCode + "").Trim() == "0000" || String.IsNullOrWhiteSpace(ParentItemCategoryCode))
            {
                var entity = GetItemCategoryByCode("ȫ��", "-99", CustomerID);
                if (entity != null)
                {
                    itemcategory.parent_id = entity.item_category_id;
                }
                else
                {
                    throw new Exception("����𸸽ڵ㲻���ڣ�����ϵ����Ա");
                }
            }
            else
            {
                itemcategory.parent_id = ParentItemInfoCategory.item_category_id;
            }

            if (ItemInfoCategory == null)
            {
                Create(itemcategory);
                return itemcategory.item_category_id;
            }
            else
            {
                //���������
                ItemInfoCategory.status = Status;
                ItemInfoCategory.item_category_name = ItemCategoryName;
                ItemInfoCategory.CustomerID = CustomerID;
                //�������
                ItemInfoCategory.item_category_id = ItemInfoCategory.item_category_id;
                ItemInfoCategory.item_category_code = ItemInfoCategory.item_category_code;
                ItemInfoCategory.parent_id = itemcategory.parent_id;

                Update(ItemInfoCategory);
                return ItemInfoCategory.item_category_id;
            }
        }
    }
}