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
    /// 数据访问：  
    /// 表T_Item_Category的数据访问类 
    /// TODO:
    /// 1.实现ICRUDable接口
    /// 2.实现IQueryable接口
    /// 3.实现Load方法
    /// </summary>
    public partial class T_Item_CategoryDAO : Base.BaseCPOSDAO, ICRUDable<T_Item_CategoryEntity>, IQueryable<T_Item_CategoryEntity>
    {
        public DataSet GetCategoryByCustomerId(string strCustomerId,string strBatId)
        {
            string strSql = string.Format("SELECT item_category_id id,item_category_name text,'close' state,'true' checked FROM dbo.T_Item_Category WHERE CustomerId = '{0}' AND status=1 AND bat_id='{1}'", strCustomerId,strBatId);
            return this.SQLHelper.ExecuteDataset(strSql);
        }

        /// <summary>
        /// 获取商品品类 通过商品品类名称
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
         public T_Item_CategoryEntity GetItemCategoryByCode(string Code, string CustomerId)
        {
            T_Item_CategoryEntity entity = null;
            var sqlText = " SELECT top 1 * FROM dbo.T_Item_Category where 1=1 AND Status=1 AND [item_category_code] =@item_category_code and CustomerID =@CustomerID ";
            var param = new SqlParameter[]
            {
                new SqlParameter("@item_category_code",SqlDbType.NVarChar),
                new SqlParameter("@CustomerID",SqlDbType.NVarChar)
            };
            param[0].Value = Code;
            param[1].Value = CustomerId;
            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(CommandType.Text, sqlText.ToString(), param))
            {
                while (rdr.Read())
                {           
                    this.Load(rdr, out entity);
                }
            }
            return entity;
         }

         /// <summary>
         /// 获取商品类别信息 通过类别名称 上级Id
         /// </summary>
         /// <param name="ItemCode"></param>
         /// <returns></returns>
         public T_Item_CategoryEntity GetItemCategoryByCode(string CategoryName, string parentId, string CustomerId)
         {
             T_Item_CategoryEntity entity = null;
            var sqlText = " SELECT top 1 * FROM dbo.T_Item_Category where 1=1 AND [item_category_name] =@item_category_name and CustomerID =@CustomerID AND parent_id=@parent_id ";
            var param = new SqlParameter[]
            {
                new SqlParameter("@item_category_name",SqlDbType.NVarChar),
                new SqlParameter("@CustomerID",SqlDbType.NVarChar),
                new SqlParameter("@parent_id",SqlDbType.NVarChar)
            };
            param[0].Value = CategoryName;
            param[1].Value = CustomerId;
            param[2].Value = parentId;

            using (SqlDataReader rdr = this.SQLHelper.ExecuteReader(CommandType.Text, sqlText.ToString(), param))
            {
                while (rdr.Read())
                {
                    this.Load(rdr, out entity);
                }
            }
            return entity;
         }
    }
}
