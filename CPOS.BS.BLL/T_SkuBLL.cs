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
    /// 业务处理：  
    /// </summary>
    public partial class T_SkuBLL
    {
        /// <summary>
        /// 添加商品规格信息
        /// </summary>
        /// <param name="ItemId">商品编号</param>
        /// <param name="SkuBarcode">条形码</param>
        /// <param name="sku_prop_id1">单品规格属性</param>
        /// <param name="CustomerId">商户编号</param>
        /// <returns></returns>
        public T_SkuEntity CreateNewSku(string ItemId)
        {
            T_SkuEntity tSkuEntity = new T_SkuEntity();
            tSkuEntity.sku_id = Guid.NewGuid().ToString("N");
            //请求参数
            tSkuEntity.item_id = ItemId;
            tSkuEntity.barcode = "100000";

            //非请求参数
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