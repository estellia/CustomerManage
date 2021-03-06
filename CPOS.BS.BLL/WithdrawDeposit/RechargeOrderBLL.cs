/*
 * Author		:CodeGeneration
 * EMail		:
 * Company		:JIT
 * Create On	:2015-4-16 17:36:32
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
using System.Data.SqlClient;
using JIT.CPOS.Common;
using JIT.CPOS.BS.BLL.WX;
using JIT.CPOS.BS.BLL.RedisOperationBLL.OrderPaySuccess;
using JIT.CPOS.BS.BLL.RedisOperationBLL.Order;
using JIT.CPOS.BS.BLL.RedisOperationBLL.OrderReward;

namespace JIT.CPOS.BS.BLL
{
    /// <summary>
    /// 业务处理：  
    /// </summary>
    public partial class RechargeOrderBLL
    {
        /// <summary>
        /// 创建对象，返回ID
        /// </summary>
        /// <param name="pEntity">实体实例</param>
        /// <param name="pTran">事务实例,可为null,如果为null,则不使用事务来更新</param>
        public Guid CreateReturnID(RechargeOrderEntity pEntity, IDbTransaction pTran)
        {
            return _currentDAO.CreateReturnID(pEntity, pTran);
        }
        /// <summary>
        /// 计算充值分润
        /// </summary>
        /// <param name="loggingSessionInfo"></param>
        /// <param name="rechargeOrderInfo"></param>
        public void CalculateRechargeOrder(LoggingSessionInfo loggingSessionInfo, RechargeOrderEntity rechargeOrderInfo)
        {
           
            VipCardProfitRuleBLL bllVipCardProfitRule = new VipCardProfitRuleBLL(loggingSessionInfo);
            VipCardProfitRuleEntity[] entityVipCardProfitRule=null ;

            entityVipCardProfitRule = bllVipCardProfitRule.QueryByEntity(new VipCardProfitRuleEntity() { VipCardTypeID = rechargeOrderInfo.VipCardTypeId, IsDelete = 0 }, null);
            if (entityVipCardProfitRule != null)
            {
                var bllVipCardGradeChangeLog = new VipCardGradeChangeLogBLL(loggingSessionInfo);
                VipAmountBLL bllVipAmount = new VipAmountBLL(loggingSessionInfo);
                VipAmountDetailBLL bllVipAmountDetail = new VipAmountDetailBLL(loggingSessionInfo);
                VipAmountEntity entityVipAmount = new VipAmountEntity();

                string connString = string.Empty;
                foreach (var ProfitRule in entityVipCardProfitRule)
                {
                    decimal amount = 0;
                    string strAmountSourceId = string.Empty;
                    string strVipId = string.Empty;
                    string strUserType = string.Empty;
                    if (ProfitRule.IsApplyAllUnits == 0)
                    {
                        VipCardProfitRuleUnitMappingBLL bllVipCardProfitRuleUnitMapping = new VipCardProfitRuleUnitMappingBLL(loggingSessionInfo);
                        var vipCardProfitRuleUnitMapping = bllVipCardProfitRuleUnitMapping.QueryByEntity(new VipCardProfitRuleUnitMappingEntity() { CardBuyToProfitRuleId = ProfitRule.CardBuyToProfitRuleId, UnitID = rechargeOrderInfo.UnitId, IsDelete = 0, CustomerID = loggingSessionInfo.ClientID }, null).SingleOrDefault();
                        if (vipCardProfitRuleUnitMapping == null)
                        {
                            continue;
                        }
                    }
                    if (ProfitRule.ProfitOwner == "Employee")
                    {
                        strAmountSourceId = "38";
                        strVipId = rechargeOrderInfo.UserId;
                        strUserType = "User";
                    }
                    if (ProfitRule.ProfitOwner == "Unit")
                    {
                        strAmountSourceId = "42";
                        strVipId = rechargeOrderInfo.UnitId;
                        strUserType = "Unit";
                    }
                    var entityVipCardGradeChangeLog = bllVipCardGradeChangeLog.QueryByEntity(new VipCardGradeChangeLogEntity() { OrderId = rechargeOrderInfo.OrderID.ToString() }, null).SingleOrDefault();
                    if (entityVipCardGradeChangeLog != null)//首充
                    {
                        amount = (decimal)ProfitRule.FirstRechargeProfitPct * (decimal)rechargeOrderInfo.ActuallyPaid * (decimal)0.01;
                    }
                    else//续充
                    {
                        VipCardReRechargeProfitRuleBLL bllVipCardReRechargeProfitRule = new VipCardReRechargeProfitRuleBLL(loggingSessionInfo);
                        var entityVipCardReRechargeProfitRule = bllVipCardReRechargeProfitRule.QueryByEntity(new VipCardReRechargeProfitRuleEntity() { VipCardTypeID = rechargeOrderInfo.VipCardTypeId, IsDelete = 0 }, null);
                        if (entityVipCardReRechargeProfitRule != null)
                        {
                            decimal discount = 0;
                            foreach (var ReRechargeProfitRule in entityVipCardReRechargeProfitRule)
                            {
                                if (ReRechargeProfitRule.ProfitType == "Superposition")
                                {
                                    discount = ((decimal)rechargeOrderInfo.ActuallyPaid / (decimal)ReRechargeProfitRule.LimitAmount) * (decimal)ReRechargeProfitRule.ProfitPct * (decimal)0.01;
                                }
                                if (ReRechargeProfitRule.ProfitType == "Step")
                                {
                                    if (rechargeOrderInfo.ActuallyPaid >= ReRechargeProfitRule.LimitAmount)
                                        discount = (decimal)ReRechargeProfitRule.ProfitPct;
                                }

                            }
                            amount = (decimal)rechargeOrderInfo.ActuallyPaid * discount;

                        }
                    }
                    //入库
                    if (amount > 0)
                    {
                        IDbTransaction tran = new DataAccess.Base.TransactionHelper(loggingSessionInfo).CreateTransaction();

                        VipAmountDetailEntity entityVipAmountDetail = new VipAmountDetailEntity
                        {
                            VipAmountDetailId = Guid.NewGuid(),
                            VipId = strVipId,
                            Amount = amount,
                            UsedReturnAmount = 0,
                            EffectiveDate = DateTime.Now,
                            DeadlineDate = Convert.ToDateTime("9999-12-31 23:59:59"),
                            AmountSourceId = strAmountSourceId,
                            ObjectId = rechargeOrderInfo.OrderID.ToString(),
                            CustomerID = loggingSessionInfo.ClientID,
                            Reason = "超级分销商"
                        };
                        bllVipAmountDetail.Create(entityVipAmountDetail, (SqlTransaction)tran);

                        T_SplitProfitRecordBLL bllSplitProfitRecord = new T_SplitProfitRecordBLL(loggingSessionInfo);
                        T_SplitProfitRecordEntity entitySplitProfitRecord = new T_SplitProfitRecordEntity()
                        {
                            ID = Guid.NewGuid().ToString(),
                            SourceType="Amount",
                            SourceId = strAmountSourceId,
                            ObjectId = rechargeOrderInfo.OrderID.ToString(),
                            UserType = strUserType,
                            UserId = rechargeOrderInfo.UserId,
                            SplitAmount = amount,
                            SplitSattus="10",
                            CustomerID = loggingSessionInfo.ClientID,
                            IsDelete=0
                        };
                        bllSplitProfitRecord.Create(entitySplitProfitRecord);
                        
                        entityVipAmount = bllVipAmount.QueryByEntity(new VipAmountEntity() { VipId = strVipId, IsDelete = 0, CustomerID = loggingSessionInfo.ClientID }, null).SingleOrDefault();
                        if (entityVipAmount == null)
                        {
                            entityVipAmount = new VipAmountEntity
                            {
                                VipId = strVipId,
                                BeginAmount = 0,
                                InAmount = amount,
                                OutAmount = 0,
                                EndAmount = amount,
                                TotalAmount = amount,
                                BeginReturnAmount = 0,
                                InReturnAmount = 0,
                                OutReturnAmount = 0,
                                ReturnAmount = 0,
                                ImminentInvalidRAmount = 0,
                                InvalidReturnAmount = 0,
                                ValidReturnAmount = 0,
                                TotalReturnAmount = 0,
                                IsLocking = 0,
                                CustomerID = loggingSessionInfo.ClientID,
                                VipCardCode = ""

                            };
                            bllVipAmount.Create(entityVipAmount, tran);
                        }
                        else
                        {

                            entityVipAmount.InReturnAmount = (entityVipAmount.InReturnAmount == null ? 0 : entityVipAmount.InReturnAmount.Value) + amount;
                            entityVipAmount.TotalReturnAmount = (entityVipAmount.TotalReturnAmount == null ? 0 : entityVipAmount.TotalReturnAmount.Value) + amount;

                            entityVipAmount.ValidReturnAmount = (entityVipAmount.ValidReturnAmount == null ? 0 : entityVipAmount.ValidReturnAmount.Value) + amount;
                            entityVipAmount.ReturnAmount = (entityVipAmount.ReturnAmount == null ? 0 : entityVipAmount.ReturnAmount.Value) + amount;
                            bllVipAmount.Update(entityVipAmount);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="rechargeOrderInfo"></param>
        /// <param name="vipInfo"></param>
        /// <param name="unitInfo"></param>
        /// <param name="paymentTypeId"></param>
        public void Recharge(RechargeOrderEntity rechargeOrderInfo,VipEntity vipInfo,t_unitEntity unitInfo, string paymentTypeId)
        {
            var rechargeOrderBll = new RechargeOrderBLL(CurrentUserInfo);
            //会员
            var vipCardVipMappingBll = new VipCardVipMappingBLL(CurrentUserInfo);
            var vipAmountBll = new VipAmountBLL(CurrentUserInfo);
            //门店
            var unitBLL = new t_unitBLL(CurrentUserInfo);
            rechargeOrderInfo.Status = 1;//已支付
            rechargeOrderInfo.PayID = paymentTypeId;
            rechargeOrderInfo.PayDateTime = DateTime.Now;
            rechargeOrderBll.Update(rechargeOrderInfo);

            //获取会员卡绑卡信息
            var vipCardVipMappingList = vipCardVipMappingBll.QueryByEntity(new VipCardVipMappingEntity() { VIPID = vipInfo.VIPID }, null);
            //会员无卡并且是续费充值的
            if (vipCardVipMappingList.Count() == 0 && rechargeOrderInfo.OrderDesc == "ReRecharge")
            {
                vipCardVipMappingBll.BindVipCard(vipInfo.VIPID, vipInfo.VipCardCode, rechargeOrderInfo.UnitId);
            }

            var vipAmountEntity = vipAmountBll.QueryByEntity(new VipAmountEntity() { VipId = vipInfo.VIPID, VipCardCode = vipInfo.VipCode }, null).FirstOrDefault();
            //充值
            var amountDetailInfo = new VipAmountDetailEntity()
            {
                Amount = rechargeOrderInfo.TotalAmount.Value,
                AmountSourceId = "4",
                ObjectId = rechargeOrderInfo.OrderID.ToString()
            };

            var vipAmountDetailId = vipAmountBll.AddVipAmount(vipInfo, unitInfo, ref vipAmountEntity, amountDetailInfo, null, CurrentUserInfo);
            if (!string.IsNullOrWhiteSpace(vipAmountDetailId) && rechargeOrderInfo.TotalAmount.Value != 0)
            {
                //发送账户余额变动微信模板消息
                var CommonBLL = new CommonBLL();
                CommonBLL.BalanceChangedMessage(rechargeOrderInfo.OrderNo, vipAmountEntity, amountDetailInfo, vipInfo.WeiXinUserId, vipInfo.VIPID, CurrentUserInfo);
            }

            //赠送
            if (rechargeOrderInfo.ReturnAmount.Value != 0)
            {
                var returnAmountInfo = new VipAmountDetailEntity()
                {
                    Amount = rechargeOrderInfo.ReturnAmount.Value,
                    AmountSourceId = "6",
                    ObjectId = rechargeOrderInfo.OrderID.ToString()
                };
                var vipReturnDetailId = vipAmountBll.AddVipAmount(vipInfo, unitInfo, ref vipAmountEntity, returnAmountInfo, null, CurrentUserInfo);

                if (!string.IsNullOrWhiteSpace(vipReturnDetailId) && rechargeOrderInfo.ReturnAmount.Value != 0)
                {
                    //发送账户余额变动微信模板消息
                    var CommonBLL = new CommonBLL();
                    CommonBLL.BalanceChangedMessage(rechargeOrderInfo.OrderNo, vipAmountEntity, returnAmountInfo, vipInfo.WeiXinUserId, vipInfo.VIPID, CurrentUserInfo);
                }
            }
            if (rechargeOrderInfo.OrderDesc == "Upgrade")
            {
                //会员卡升级
                vipCardVipMappingBll.BindVirtualItem(vipInfo.VIPID, vipInfo.VipCode, rechargeOrderInfo.UnitId, rechargeOrderInfo.VipCardTypeId ?? 0, "Recharge", orderId: rechargeOrderInfo.OrderID.ToString());

            }

            //充值分润
            RedisRechargeOrderBLL redisRechargeOrderBll = new RedisRechargeOrderBLL();
            redisRechargeOrderBll.SetRedisToRechargeOrder(CurrentUserInfo, rechargeOrderInfo);

        }
    }
}