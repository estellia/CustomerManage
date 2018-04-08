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
    /// 业务处理：  
    /// </summary>
    public partial class T_Item_CategoryBLL
    {
        public DataSet GetCategoryByCustomerId(string strCustomerId, string strBatId)
        {
          return  this._currentDAO.GetCategoryByCustomerId(strCustomerId, strBatId);
        }

        /// <summary>
        /// 获取商品品类 通过商品品类名称
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
        /// 添加/获取/修改 商品类别信息  {仅供 订单 商品 调用}
        /// </summary>
        /// <param name="ItemCategoryName">类别名称</param>
        /// <param name="ItemCategoryCode">类别编号</param>
        /// <param name="CustomerID">商户编号</param>
        /// <param name="ParentItemCategoryCode">父节点编号</param> 
        /// <param name="Status">状态</param>
        public string CreateOrUpdateItemCategory(string ItemCategoryName, string ItemCategoryCode, string CustomerID, string ParentItemCategoryCode, string Status)
        {

            //请求参数
            T_Item_CategoryEntity itemcategory = new T_Item_CategoryEntity();
            itemcategory.item_category_name = ItemCategoryName;
            itemcategory.item_category_code = ItemCategoryCode;
            //非请求参数
            itemcategory.CustomerID = CustomerID;
            itemcategory.item_category_id = Guid.NewGuid().ToString();
            itemcategory.status = "1";

            //通过商品类别编号和商户号 获取商品类别信息
            var ItemInfoCategory = GetItemCategoryByCode(ItemCategoryCode, CustomerID);

            //通过父节点编号获取 商品类别信息
            var ParentItemInfoCategory = GetItemCategoryByCode(ParentItemCategoryCode, CustomerID);

            //如果父节点为空 或者 前端没有传递父节点 那我们就默认父节点信息
            if (ParentItemInfoCategory == null || (ParentItemCategoryCode + "").Trim() == "0000" || String.IsNullOrWhiteSpace(ParentItemCategoryCode))
            {
                var entity = GetItemCategoryByCode("全部", "-99", CustomerID);
                if (entity != null)
                {
                    itemcategory.parent_id = entity.item_category_id;
                }
                else
                {
                    throw new Exception("该类别父节点不存在，请联系管理员");
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
                //非请求参数
                ItemInfoCategory.status = Status;
                ItemInfoCategory.item_category_name = ItemCategoryName;
                ItemInfoCategory.CustomerID = CustomerID;
                //请求参数
                ItemInfoCategory.item_category_id = ItemInfoCategory.item_category_id;
                ItemInfoCategory.item_category_code = ItemInfoCategory.item_category_code;
                ItemInfoCategory.parent_id = itemcategory.parent_id;

                Update(ItemInfoCategory);
                return ItemInfoCategory.item_category_id;
            }
        }
    }
}