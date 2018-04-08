using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JIT.CPOS.DTO.Base;

namespace JIT.CPOS.DTO.Module.Pos.Order.Request
{
    public class SetPosOrderRP : IAPIRequestParameter
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 门店Code
        /// </summary>
        public string UnitCode { get; set; }

        /// <summary>
        /// 员工Code
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 员工手机号
        /// </summary>
        public string MobliePhone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// 商品总数
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        ///  折后金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 优惠券Id
        /// </summary>
        public string  CouponId { get; set; }

        /// <summary>
        /// 是否使用积分
        /// </summary>
        public int IntegralFlag { get; set; }

        /// <summary>
        /// 使用积分
        /// </summary>
        public decimal Integral { get; set; }

        /// <summary>
        /// 积分抵扣金额
        /// </summary>
        public decimal IntegralAmount { get; set; }

        /// <summary>
        /// 是否使用余额
        /// </summary>
        public int EndAmountFlag { get; set; }
        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal EndAmount { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OrderDetail> OrderDetailList { get; set; }

        public void Validate() { }
    }

    /// <summary>
    /// 订单明细
    /// </summary>
    public class OrderDetail
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 商品类别编码
        /// </summary>
        public string ItemCategoryCode { get; set; }

        /// <summary>
        /// 商品类别名称
        /// </summary>
        public string ItemCategoryName { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string ItemIntroduce { get; set; }

        /// <summary>
        /// sku条形码
        /// </summary>
        public string SkuBarcode { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public string SkuOriginPrice { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public string SkuSalesPrice { get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>
        public string SkuUnit { get; set; }

        /// <summary>
        /// 商品最低折扣金额
        /// </summary>
        public decimal ItemDiscount { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal price { get; set; }
    }

}
