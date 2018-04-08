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
    public partial class T_Sku_PriceBLL
    {
        /// <summary>
        /// ��ӹ��۸���Ϣ
        /// </summary>
        /// <param name="Price">�۸�</param>
        /// <param name="skuid">�����</param>
        /// <param name="pricetypeid">�۸�����</param>
        /// <param name="customerid">�̻����</param>
        /// <returns>
        /// true �������۸�
        /// false ���۸��Ѿ�������
        /// </returns>
        /// 
        public void AddSkuPrice(string Price, string skuid, string pricetypeid, string customerid)
        {
            var skuEntity = QueryByEntity(new T_Sku_PriceEntity() { sku_id = skuid, item_price_type_id = pricetypeid }, null).FirstOrDefault();
            if (skuEntity == null)
            {
                Create(new T_Sku_PriceEntity
                {
                    sku_price_id = Guid.NewGuid().ToString("N"),
                    sku_id = skuid,
                    item_price_type_id = pricetypeid,//���ۼ�
                    sku_price = Convert.ToDecimal(Price),
                    status = "1",
                    create_user_id = "open",
                    modify_user_id = "open",
                    create_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    modify_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    customer_id = customerid,
                    bat_id = "0",
                    if_flag = "0"
                });
            }
        }
        /// <summary>
        /// ��ӹ��۸�
        /// </summary>
        /// <param name="sku_id">�����</param>
        /// <param name="SkuOriginPrice">ԭ��</param>
        /// <param name="SkuSalesPrice">���ۼ�</param>
        /// <param name="StoreCount">���</param>
        /// <param name="SaleCount">����</param>
        /// <param name="clientId">�̻����</param>
        public void CreateNewSkuPrice(string sku_id, string SkuOriginPrice, string SkuSalesPrice, string StoreCount, string SaleCount, string clientId)
        {
            //ԭ��
            AddSkuPrice(SkuOriginPrice, sku_id, "77850286E3F24CD2AC84F80BC625859D", clientId);
            //���ۼ�
            AddSkuPrice(SkuSalesPrice, sku_id, "75412168A16C4D2296B92CA0E596A188", clientId);
            //���
            AddSkuPrice(StoreCount, sku_id, "77850286E3F24CD2AC84F80BC625859E", clientId);
            //����
            AddSkuPrice(SaleCount, sku_id, "77850286E3F24CD2AC84F80BC625859f", clientId);
        }
    }
}