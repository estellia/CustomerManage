using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;

using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.BLL.CodeGeneration.Order;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.Module.Pos.Order.Request;
using JIT.CPOS.DTO.Module.Pos.Order.Response;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.BS.DataAccess.Base;
using JIT.CPOS.BS.BLL.WX;
using JIT.CPOS.BS.BLL.RedisOperationBLL.OrderReward;

namespace JIT.CPOS.Web.ApplicationInterface.Module.Pos.Order
{
    public class SetPosOrderAH : BaseActionHandler<SetPosOrderRP, SetPosOrderRD>
    {
         //<summary>
         //Pos订单入库
         //</summary>
         //<param name="pRequest"></param>
         //<returns></returns>
        protected override SetPosOrderRD ProcessRequest(APIRequest<SetPosOrderRP> pRequest)
        {
            //请求参数
            var rp = pRequest.Parameters;
            //返回参数
            var rd = new SetPosOrderRD();

            //订单
            var inoutBll = new T_InoutBLL(CurrentUserInfo);
            var inoutDetail = new T_Inout_DetailBLL(CurrentUserInfo);
            //商品
            var itemCategoryBll = new T_Item_CategoryBLL(CurrentUserInfo);
            var itemBll = new T_ItemBLL(CurrentUserInfo);
            var porpBll = new T_PropBLL(CurrentUserInfo);
            var itemSkuPropBll = new T_ItemSkuPropBLL(CurrentUserInfo);
            var skuBll = new T_SkuBLL(CurrentUserInfo);
            var skuPriceBll = new T_Sku_PriceBLL(CurrentUserInfo);
            var skuProperty = new T_Sku_PropertyBLL(CurrentUserInfo);
            //获取会员信息
            var vipBll = new VipBLL(CurrentUserInfo);
            var vipInfo = vipBll.GetByID(pRequest.UserID);
            var vipCardVipMappingBll = new VipCardVipMappingBLL(CurrentUserInfo);
            var vipCardBll = new VipCardBLL(CurrentUserInfo);
            var vipCardVipMappingEntity = vipCardVipMappingBll.QueryByEntity(new VipCardVipMappingEntity() { VIPID = vipInfo.VIPID, CustomerID = CurrentUserInfo.ClientID }, null).FirstOrDefault();
            string VipCardTypeID = ""; //卡类型Id
            if (vipCardVipMappingEntity != null)
            {
                var vipCardEntity = vipCardBll.GetByID(vipCardVipMappingEntity.VipCardID);
                VipCardTypeID = vipCardEntity.VipCardTypeID.ToString();
            }
            //员工
            var userBll = new T_UserBLL(CurrentUserInfo);
            
            //获取门店信息
            var unitBll = new t_unitBLL(CurrentUserInfo);
            t_unitEntity unitInfo = null;
            if(!string.IsNullOrEmpty(rp.UnitCode))
            {
                unitInfo = unitBll.QueryByEntity(new t_unitEntity() { unit_code = rp.UnitCode,customer_id = CurrentUserInfo.ClientID },null).FirstOrDefault();
                if (unitInfo == null)
                {
                    throw new APIException("请在正念商户后台录入相应门店") { ErrorCode = 100 };
                }
            }
            else
            {
                throw new APIException("缺少请求参数：门店编码") { ErrorCode = 102 };
            }

            //获取员工信息
            T_UserEntity userEntity = null;
            if (!string.IsNullOrEmpty(rp.MobliePhone))
            {
                userEntity = userBll.QueryByEntity(new T_UserEntity() { user_telephone  = rp.MobliePhone,customer_id = CurrentUserInfo.ClientID }, null).FirstOrDefault();
                //没有员工，新增默认员工(店员APP)
                if(userEntity == null)
                {
                    var roleBll = new T_RoleBLL(CurrentUserInfo);
                    var roleEntity = roleBll.QueryByEntity(new T_RoleEntity() { role_code = "clerkAPP", customer_id = CurrentUserInfo.ClientID },null).FirstOrDefault();
                    if (roleEntity == null)
                    {
                        throw new APIException("请在正念商户后台录入相应角色") { ErrorCode = 100 };
                    }
                    userEntity = new T_UserEntity();
                    userEntity.user_telephone = rp.MobliePhone;
                    userEntity.user_code = rp.UserCode;
                    userEntity.user_name = rp.UserCode;
                    userEntity.user_birthday = rp.Birthday;
                    userEntity.user_email = rp.EmailAddress;
                    userEntity.user_address = rp.Address;
                    userEntity.user_postcode = rp.Zip;
                    userBll.AddUser(ref userEntity, unitInfo, roleEntity);
                }
            }
            else
            {
                throw new APIException("缺少请求参数：员工手机号") { ErrorCode = 102 };
            }
            //获取会员折扣
            var sysVipCardGradeBLL = new SysVipCardGradeBLL(CurrentUserInfo);
            decimal vipDiscount = sysVipCardGradeBLL.GetVipDiscount() * 10;


            //订单号
            string orderId = BaseService.NewGuidPub();
            T_InoutEntity tInoutEntity = new T_InoutEntity();
            tInoutEntity.order_id = orderId;
            tInoutEntity.order_no = rp.OrderNo;

            //拼接ItemCodes
            //StringBuilder ItemCodes = new StringBuilder();
            //for (int j = 0; j < pRequest.Parameters.OrderDetailList.Count(); j++)
            //{
            //    if (j != 0)
            //    {
            //        ItemCodes.Append(",");
            //    }
            //    ItemCodes.Append(string.Format("{0}", pRequest.Parameters.OrderDetailList[j].ItemCode));
            //}

            ////通过itemCodes取出商品价格
            //SkuPriceService skuPriceService = new SkuPriceService(CurrentUserInfo);
            //List<SkuPrice> skuPriceList = skuPriceService.GetPriceListByItemCodes(ItemCodes.ToString(), CurrentUserInfo.ClientID);
            //if(skuPriceList.Count == 0)
            //{
            //    throw new APIException("未找到商品") { ErrorCode = 100 };
            //}

            //订单总金额
            decimal totalAmount = rp.TotalAmount;
            //订单实付金额
            decimal ActualAmount = rp.DiscountAmount;
            //订单折扣后金额
            decimal DiscountAmount = rp.DiscountAmount;
            //订单明细显示顺序
            int i = 1;
            //商品价格重新计算
            foreach (var item in pRequest.Parameters.OrderDetailList)
            {
                T_ItemEntity itemEntity = null; //商品
                T_SkuEntity skuEntity = null; //sku
                if (!string.IsNullOrEmpty(item.ItemCode))
                {
                    itemEntity = itemBll.QueryByEntity(new T_ItemEntity() { item_code = item.ItemCode, CustomerId = CurrentUserInfo.ClientID }, null).FirstOrDefault();
                    if (itemEntity == null)
                    {
                        if (string.IsNullOrEmpty(item.ItemCategoryCode))
                        {
                            throw new APIException("缺少参数：商品类别名称") { ErrorCode = 200 };
                        }
                        itemEntity = new T_ItemEntity();
                        itemEntity.item_code = item.ItemCode;
                        itemEntity.item_name = item.ItemName;

                        itemBll.AddItem(itemEntity, out skuEntity,item.ItemCategoryName, item.ItemCategoryCode, item.SkuOriginPrice, item.SkuSalesPrice);
                    }
                    else
                    {
                        skuEntity = skuBll.QueryByEntity(new T_SkuEntity(){ item_id = itemEntity.item_id},null).FirstOrDefault();
                    }
                }
                else
                {
                    throw new APIException("缺少参数：商品编码") { ErrorCode = 300 };
                }

                //订单明细相关处理
                T_Inout_DetailBLL inoutDetailBll = new T_Inout_DetailBLL(CurrentUserInfo);
                T_Inout_DetailEntity inoutDetailEntity = new T_Inout_DetailEntity()
                {
                    order_detail_id = BaseService.NewGuidPub(),
                    order_id = orderId, //订单Id
                    sku_id = skuEntity.sku_id, //skuId
                    unit_id = unitInfo.unit_id, //门店Id
                    order_qty = item.Qty, //订单qty
                    enter_qty = item.Qty, //实际qty
                    enter_price = item.price, //折扣价
                    enter_amount = item.price * item.Qty, //折扣价
                    std_price = item.price, //原价
                    discount_rate = vipDiscount, //折扣
                    retail_price = item.price * item.Qty, //零售价
                    retail_amount = item.price * item.Qty, //零售价
                    order_detail_status = "1",
                    display_index = i,
                    if_flag = 0
                };
                inoutDetailBll.Create(inoutDetailEntity);
                i++;
            }

            //优惠券使用
            if (!string.IsNullOrEmpty(rp.CouponId))
            {
                #region 判断优惠券是否是该会员的

                var vipcouponMappingBll = new VipCouponMappingBLL(CurrentUserInfo);

                var vipcouponmappingList = vipcouponMappingBll.QueryByEntity(new VipCouponMappingEntity()
                {
                    VIPID = pRequest.UserID,
                    CouponID = rp.CouponId
                }, null);

                if (vipcouponmappingList == null || vipcouponmappingList.Length == 0)
                {
                    throw new APIException("此张优惠券不是该会员的") { ErrorCode = 103 };
                }

                #endregion

                #region 判断优惠券是否有效

                var couponBll = new CouponBLL(CurrentUserInfo);

                var couponEntity = couponBll.GetByID(rp.CouponId);

                if (couponEntity == null)
                {
                    throw new APIException("无效的优惠券") { ErrorCode = 103 };
                }

                if (couponEntity.Status == 1)
                {
                    throw new APIException("优惠券已使用") { ErrorCode = 103 };
                }

                if (couponEntity.EndDate < DateTime.Now)
                {
                    throw new APIException("优惠券已过期") { ErrorCode = 103 };
                }
                var couponTypeBll = new CouponTypeBLL(CurrentUserInfo);
                var couponTypeEntity = couponTypeBll.GetByID(couponEntity.CouponTypeID);

                if (couponTypeEntity == null)
                {
                    throw new APIException("无效的优惠券类型") { ErrorCode = 103 };
                }

                #endregion

                #region 优惠券核销
                var couponUseBll = new CouponUseBLL(CurrentUserInfo);
                var couponUseEntity = new CouponUseEntity()
                {
                    CouponUseID = Guid.NewGuid(),
                    CouponID = rp.CouponId,
                    VipID = pRequest.UserID,
                    UnitID = unitInfo.unit_id,
                    OrderID = orderId,
                    Comment = "商城使用电子券",
                    CustomerID = pRequest.CustomerID,
                    CreateBy = pRequest.UserID,
                    CreateTime = DateTime.Now,
                    LastUpdateBy = pRequest.UserID,
                    LastUpdateTime = DateTime.Now,
                    IsDelete = 0
                };
                couponUseBll.Create(couponUseEntity);
                #endregion

                #region 更新CouponType数量
                var conponTypeBll = new CouponTypeBLL(CurrentUserInfo);
                var conponTypeEntity = conponTypeBll.QueryByEntity(new CouponTypeEntity() { CouponTypeID = new Guid(couponEntity.CouponTypeID), CustomerId = pRequest.CustomerID }, null).FirstOrDefault();
                conponTypeEntity.IsVoucher += 1;
                conponTypeBll.Update(conponTypeEntity);

                #endregion

                #region 更新优惠券状态

                couponEntity.Status = 1;
                couponBll.Update(couponEntity);

                #endregion

                ActualAmount -= couponTypeEntity.ParValue ?? 0;
            }

            #region 使用积分
            //使用积分
            if (rp.IntegralFlag == 1)
            {
                var vipIntegralBll = new VipIntegralBLL(CurrentUserInfo);

                string sourceId = "20"; //积分抵扣
                var IntegralDetail = new VipIntegralDetailEntity()
                {
                    Integral = -Convert.ToInt32(rp.Integral),
                    IntegralSourceID = sourceId,
                    ObjectId = orderId
                };
                if (IntegralDetail.Integral != 0)
                {
                    //变动前积分
                    string OldIntegral = (vipInfo.Integration ?? 0).ToString();
                    //变动积分
                    string ChangeIntegral = (IntegralDetail.Integral ?? 0).ToString();
                    var vipIntegralDetailId = vipIntegralBll.AddIntegral(ref vipInfo, unitInfo, IntegralDetail, CurrentUserInfo);
                    //发送微信积分变动通知模板消息
                    if (!string.IsNullOrWhiteSpace(vipIntegralDetailId))
                    {
                        var CommonBLL = new CommonBLL();
                        CommonBLL.PointsChangeMessage(OldIntegral, vipInfo, ChangeIntegral, vipInfo.WeiXinUserId, CurrentUserInfo);
                    }
                }
                tInoutEntity.pay_points = rp.Integral;
                tInoutEntity.receive_points = rp.Integral;
                ActualAmount -= rp.IntegralAmount;
            }
            #endregion

            #region 余额和返现修改

            var vipAmountBll = new VipAmountBLL(CurrentUserInfo);
            var vipAmountDetailBll = new VipAmountDetailBLL(CurrentUserInfo);

            var vipAmountEntity = vipAmountBll.QueryByEntity(new VipAmountEntity() { VipId = pRequest.UserID, VipCardCode = vipInfo.VipCode }, null).FirstOrDefault();
            if (vipAmountEntity != null)
            {
                //判断该会员账户是否被冻结
                if (vipAmountEntity.IsLocking == 1)
                    throw new APIException("账户已被冻结，请先解冻") { ErrorCode = 103 };

                //判断该会员的账户余额是否大于本次使用的余额
                if (vipAmountEntity.EndAmount < rp.EndAmount)
                    throw new APIException(string.Format("账户余额不足，当前余额为【{0}】", vipAmountEntity.EndAmount)) { ErrorCode = 103 };
            }

            //使用余额
            if (rp.EndAmountFlag == 1)
            {
                var detailInfo = new VipAmountDetailEntity()
                {
                    Amount = -rp.EndAmount,
                    AmountSourceId = "1",
                    ObjectId = orderId
                };
                var vipAmountDetailId = vipAmountBll.AddVipAmount(vipInfo, unitInfo, ref vipAmountEntity, detailInfo, CurrentUserInfo);
                if (!string.IsNullOrWhiteSpace(vipAmountDetailId))
                {//发送微信账户余额变动模板消息
                    var CommonBLL = new CommonBLL();
                    CommonBLL.BalanceChangedMessage(tInoutEntity.order_no, vipAmountEntity, detailInfo, vipInfo.WeiXinUserId, vipInfo.VIPID, CurrentUserInfo);
                }
                tInoutEntity.Field3 = rp.EndAmount.ToString();
                
            }

            #endregion
            //订单主表更新
            tInoutEntity.VipCardCode = vipInfo.VipCardCode;//会员卡号
            tInoutEntity.order_reason_id = "2F6891A2194A4BBAB6F17B4C99A6C6F5"; 
            tInoutEntity.order_type_id = "1F0A100C42484454BAEA211D4C14B80F";
            tInoutEntity.warehouse_id = "67bb4c12785c42d4912aff7d34606592";
            tInoutEntity.data_from_id = "";
            tInoutEntity.red_flag = "1";
            tInoutEntity.order_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //订单时间
            tInoutEntity.create_unit_id = unitInfo.unit_id; //门店
            tInoutEntity.unit_id = unitInfo.unit_id; //门店
            tInoutEntity.sales_unit_id = unitInfo.unit_id; //门店
            tInoutEntity.purchase_unit_id = unitInfo.unit_id;
            tInoutEntity.sales_user = userEntity.user_id;
            tInoutEntity.total_amount = totalAmount; //订单金额
            tInoutEntity.discount_rate = vipDiscount; //会员折扣
            tInoutEntity.actual_amount = ActualAmount;//实付金额
            tInoutEntity.total_qty = rp.qty;
            tInoutEntity.total_retail = totalAmount; //订单金额
            tInoutEntity.vip_no = vipInfo.VIPID;
            tInoutEntity.Field6 = vipInfo.Phone;
            tInoutEntity.Field14 = vipInfo.VipName;
            tInoutEntity.Field17 = VipCardTypeID;
            tInoutEntity.Field12 = DiscountAmount.ToString();
            tInoutEntity.Field11 = "知行易";
            tInoutEntity.customer_id = CurrentUserInfo.ClientID;

            tInoutEntity.Field1 = "1"; //支付完成
            tInoutEntity.Field7 = "700"; //已完成
            tInoutEntity.status = "700"; //已完成
            tInoutEntity.status_desc = "已完成";
            tInoutEntity.Field10 = "已完成";
            rd.Amount = ActualAmount - rp.EndAmount;
            
            inoutBll.Create(tInoutEntity);

            //订单奖励
            new SendOrderRewardMsgBLL().OrderReward(tInoutEntity, this.CurrentUserInfo, null);//存入到缓存
            rd.orderId = orderId;
            return rd;
        }

    }
}