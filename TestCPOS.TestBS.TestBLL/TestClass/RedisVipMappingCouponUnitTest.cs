using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.BLL.RedisOperationBLL.Coupon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedisOpenAPIClient.Models.CC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JIT.TestCPOS.TestBS.TestBLL.TestClass
{
    [TestClass]
    public class RedisVipMappingCouponUnitTest
    {

        [TestMethod]
        public void SetVipMappingCoupon()
        {
            Stopwatch stop = new Stopwatch();

            stop.Start();

            CouponTypeBLL couponTypeBLL = new CouponTypeBLL(new CPOS.BS.Entity.LoggingSessionInfo()
            {
                CurrentLoggingManager = new CPOS.BS.Entity.LoggingManager()
                {
                    Connection_String = "user id=admin;password=JtLaxT7668;data source=115.159.97.144,2433;database=cpos_bs_xgx",
                    Customer_Id = "584feb4e99024962bf1a91b2a590d095"
                }
            });

            var couponTypes = couponTypeBLL.GetVirtualItemCouponTypes("1b7b8bd417c54da3af337cc70c042721");

            RedisVipMappingCouponBLL redisVipMappingCouponBLL = new RedisVipMappingCouponBLL();

            redisVipMappingCouponBLL.SetVipMappingCoupon(couponTypes.ToList(), "", couponTypes.FirstOrDefault().VipId, "PayVirtualItem");

            stop.Stop();

            Console.WriteLine(stop.ElapsedMilliseconds);
        }

        [TestMethod]
        public void InsertDataBase()
        {
            Stopwatch stop = new Stopwatch();

            stop.Start();

            RedisVipMappingCouponBLL bll = new RedisVipMappingCouponBLL();

            bll.InsertDataBase();

            stop.Stop();

            Console.WriteLine(stop.ElapsedMilliseconds);
        }
    }
}
