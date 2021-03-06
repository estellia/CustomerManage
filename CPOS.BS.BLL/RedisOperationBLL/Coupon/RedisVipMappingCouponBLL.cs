﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using JIT.Utility;
using JIT.Utility.ExtensionMethod;
using JIT.CPOS.BS.DataAccess;
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess;
using JIT.Utility.DataAccess.Query;
using System.Data.SqlClient;
using RedisOpenAPIClient;
using RedisOpenAPIClient.Models;
using RedisOpenAPIClient.Models.CC;
using JIT.CPOS.Common;
using JIT.CPOS.BS.BLL.RedisOperationBLL.Connection;
using System.Configuration;
using System.Threading;
using JIT.CPOS.BS.BLL.WX;
//using ServiceStack.Redis;

namespace JIT.CPOS.BS.BLL.RedisOperationBLL.Coupon
{
    public class RedisVipMappingCouponBLL
    {

        /// <summary>
        /// vip绑定coupon入队列
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="strObjectId"></param>
        /// <param name="strVipId"></param>
        /// <param name="strSource"></param>
        public void SetVipMappingCoupon(CC_Coupon coupon, string strObjectId, string strVipId, string strSource)
        {
            LoggingSessionInfo _loggingSessionInfo = new LoggingSessionInfo();
            LoggingManager CurrentLoggingManager = new LoggingManager();
            string strCon = string.Empty;

            var connection = new RedisConnectionBLL().GetConnection(coupon.CustomerId);
            //如果缓存获取不到数据，从数据库读取数据库连接字符串
            if (connection.CustomerID == null)
            {
                connection.ConnectionStr = GetCustomerConn(coupon.CustomerId);
            }

            CurrentLoggingManager.Connection_String = connection.ConnectionStr;

            _loggingSessionInfo.ClientID = coupon.CustomerId;
            _loggingSessionInfo.CurrentLoggingManager = CurrentLoggingManager;

            RedisCouponBLL redisCouponBLL = new RedisCouponBLL();
            //获取优惠券数量
            var count = RedisOpenAPI.Instance.CCCoupon().GetCouponListLength(coupon);
            if (count.Code == ResponseCode.Success)
            {
                if (count.Result == 0)
                {

                    var bllCouponType = new CouponTypeBLL(_loggingSessionInfo);

                    var couponType = bllCouponType.QueryByEntity(new CouponTypeEntity() { CustomerId = coupon.CustomerId, CouponTypeID = new Guid(coupon.CouponTypeId), IsDelete = 0 }, null).SingleOrDefault();
                    if (couponType != null)
                    {
                        //计算剩余的优惠券数量
                        int intCouponLenth = Convert.ToInt32(couponType.IssuedQty) - Convert.ToInt32(couponType.IsVoucher);
                        if (intCouponLenth <= 0)
                        {
                            intCouponLenth = 1000;
                            //重置优惠券数量为1000
                            bllCouponType.UpdateCouponTypeIssuedQty(coupon.CouponTypeId, intCouponLenth);
                        }

                        //这里可能是重置 Redis 缓存中该类型优惠券数据数量
                        RedisOpenAPI.Instance.CCCoupon().SetCouponList(new CC_Coupon()
                        {
                            CustomerId = couponType.CustomerId,
                            CouponTypeId = couponType.CouponTypeID.ToString(),
                            CouponTypeDesc = couponType.CouponTypeDesc,
                            CouponTypeName = couponType.CouponTypeName,
                            BeginTime = couponType.BeginTime.ToString(),
                            EndTime = couponType.EndTime.ToString(),
                            ServiceLife = couponType.ServiceLife ?? 0,
                            CouponLenth = intCouponLenth,
                            CouponCategory = couponType.CouponCategory
                        });

                    }

                }
                //从 Redis 中重新获取优惠券信息
                var response = redisCouponBLL.RedisGetCoupon(coupon);
                if (response.Code == ResponseCode.Success && response.Result.CouponTypeId != null && response.Result.CouponTypeId.Length > 0)
                {
                    string strCouponCode = string.Empty;
                    //从数据库中读取优惠券编码
                    strCouponCode = GetCouponNum(_loggingSessionInfo, response.Result.CustomerId);
                    var _coupon = new CC_Coupon()
                    {
                        CustomerId = response.Result.CustomerId,
                        CouponTypeId = response.Result.CouponTypeId,
                        CouponTypeDesc = response.Result.CouponTypeDesc,
                        CouponTypeName = response.Result.CouponTypeName,
                        CouponCode = strCouponCode,
                        BeginTime = response.Result.BeginTime,
                        EndTime = response.Result.EndTime,
                        ServiceLife = response.Result.ServiceLife,
                        CouponCategory = response.Result.CouponCategory,
                        CouponId = Guid.NewGuid().ToString()
                    };
                    BaseService.WriteLog("---------------------------入vip绑定优惠券队列---------------------------");
                    //将优惠券推送到 Redis 缓存中
                    RedisOpenAPI.Instance.CCVipMappingCoupon().SetVipMappingCoupon(new CC_VipMappingCoupon()
                    {
                        CustomerId = coupon.CustomerId,
                        ObjectId = strObjectId,
                        VipId = strVipId,
                        Source = strSource,
                        Coupon = _coupon
                    });
                }
            }
        }
        /// <summary>
        /// 虚拟商品发优惠券
        /// </summary>
        /// <param name="lstCoupon"></param>
        /// <param name="strObjectId"></param>
        /// <param name="strVipId"></param>
        /// <param name="strSource"></param>
        public void SetVipMappingCoupon(List<CouponTypeEntity> lstCoupon, string strObjectId, string strVipId, string strSource)
        {
            BaseService.WriteLog("---------------------------入vip绑定优惠券队列开始---------------------------");

            LoggingSessionInfo _loggingSessionInfo = new LoggingSessionInfo();
            LoggingManager CurrentLoggingManager = new LoggingManager();
            string strCon = string.Empty;
            var coupon = lstCoupon.FirstOrDefault();
            try
            {
                var connection = new RedisConnectionBLL().GetConnection(coupon.CustomerId);
                //如果缓存获取不到数据，从数据库读取数据库连接字符串
                if (connection.CustomerID == null)
                {
                    connection.ConnectionStr = GetCustomerConn(coupon.CustomerId);
                }

                CurrentLoggingManager.Connection_String = connection.ConnectionStr;

                _loggingSessionInfo.ClientID = coupon.CustomerId;
                _loggingSessionInfo.CurrentLoggingManager = CurrentLoggingManager;

                RedisCouponBLL redisCouponBLL = new RedisCouponBLL();

                //从 Redis 中重新获取优惠券信息
                foreach (var itemCoupon in lstCoupon)  //按照优惠券类型进行依次发送
                {
                    //下面这段话是不是应该放在下面的for循环里面？？
                    //获取Redis中当前 CouponTypeID 类型的优惠券数量
                    var count = RedisOpenAPI.Instance.CCCoupon().GetCouponListLength(new CC_Coupon()
                    {
                        CustomerId = itemCoupon.CustomerId,
                        CouponTypeId = itemCoupon.CouponTypeID.ToString()
                    });

                    if (count.Code != ResponseCode.Success)
                    {
                        BaseService.WriteLog("请求Redist失败，请求函数 GetCouponListLength ,参数 CustomerId:" + itemCoupon.CustomerId + ", CouponTypeId:" + itemCoupon.CouponTypeID.ToString());
                        continue;
                    }

                    //如果 Redis 该类型优惠券已用完
                    if (count.Result == 0)
                    {
                        CheckRedisCouponNum(_loggingSessionInfo, itemCoupon);
                    }

                    int sendRedistCount = 0;
                    //根据虚拟商品中配置的商品数量，插入到 Redist 中
                    for (int i = 0; i < itemCoupon.Item_Count; i++)
                    {
                        //获取优惠券类型数据
                        var response = redisCouponBLL.RedisGetCoupon(new CC_Coupon()
                        {
                            CustomerId = itemCoupon.CustomerId,
                            CouponTypeId = itemCoupon.CouponTypeID.ToString()
                        });
                        #region  发送一张优惠券
                        if (response.Code == ResponseCode.Success && response.Result.CouponTypeId != null && response.Result.CouponTypeId.Length > 0)
                        {
                            string strCouponCode = string.Empty;
                            //从数据库中读取优惠券编码
                            strCouponCode = GetCouponNum(_loggingSessionInfo, response.Result.CustomerId);
                            var _coupon = new CC_Coupon()
                            {
                                CustomerId = response.Result.CustomerId,
                                CouponTypeId = response.Result.CouponTypeId,
                                CouponTypeDesc = response.Result.CouponTypeDesc,
                                CouponTypeName = response.Result.CouponTypeName,
                                CouponCode = strCouponCode,   //优惠券code
                                BeginTime = response.Result.BeginTime,
                                EndTime = response.Result.EndTime,
                                ServiceLife = response.Result.ServiceLife,
                                CouponCategory = response.Result.CouponCategory,
                                CouponId = Guid.NewGuid().ToString()
                            };

                            //将优惠券推送到 Redis 缓存中
                            RedisOpenAPI.Instance.CCVipMappingCoupon().SetVipMappingCoupon(new CC_VipMappingCoupon()
                            {
                                CustomerId = itemCoupon.CustomerId,
                                ObjectId = strObjectId,
                                VipId = strVipId,
                                Source = strSource,
                                Coupon = _coupon
                            });

                            sendRedistCount++;
                        }//if数量
                        #endregion

                    }//里面的for

                    //针对每一种类型的优惠券，发送一个通知
                      DateTime?  BeginDate=null;
                     string EndDate=null;
                    if (itemCoupon.ServiceLife > 0)
                    {
                        BeginDate = DateTime.Now;
                        EndDate = DateTime.Now.Date.AddDays((int)itemCoupon.ServiceLife - 1).ToShortDateString() + " 23:59:59.998";

                    }
                    else
                    {
                       BeginDate = itemCoupon.BeginTime;
                        EndDate = itemCoupon.EndTime.ToString();
                    }
                    string strValidityData = Convert.ToDateTime(BeginDate.ToString()).ToShortDateString() + "-" + Convert.ToDateTime(EndDate.ToString()).ToShortDateString();
//写入到缓存
                    new SendCouponNoticeBLL().CouponsArrivalMessage(itemCoupon.CouponTypeName, itemCoupon.CouponTypeName, strValidityData, itemCoupon.CouponCategory == null ? "" : itemCoupon.CouponCategory, itemCoupon.WeiXinUserId, _loggingSessionInfo, itemCoupon.Item_Count.ToString());//存入到缓存j


                    BaseService.WriteLog(string.Format("---------------------------入vip绑定优惠券队列数量{0}---------------------------", sendRedistCount));
                }//foreach

            }//try
            catch (Exception ex)
            {
                BaseService.WriteLog("向Redis发送优惠券时发生异常，异常信息：" + ex.Message);
                BaseService.WriteLog("向Redis发送优惠券时发生异常，异常堆栈信息：" + ex.StackTrace);
            }

            
        }

        /// <summary>
        /// 检查 Redis 中是否包含该优惠券，如果没有，重置1000张该优惠券
        /// </summary>
        /// <param name="_loggingSessionInfo"></param>
        /// <param name="coupon"></param>
        private void CheckRedisCouponNum(LoggingSessionInfo _loggingSessionInfo, CouponTypeEntity coupon)
        {
            var bllCouponType = new CouponTypeBLL(_loggingSessionInfo);

            var couponType = bllCouponType.QueryByEntity(new CouponTypeEntity()
            {
                CustomerId = coupon.CustomerId,
                CouponTypeID = coupon.CouponTypeID,
                IsDelete = 0
            }, null).SingleOrDefault();

            if (couponType != null)
            {
                //计算剩余的优惠券数量
                int intCouponLenth = Convert.ToInt32(couponType.IssuedQty) - Convert.ToInt32(couponType.IsVoucher);
                if (intCouponLenth <= 0)
                {
                    intCouponLenth = 1000;
                    //重置优惠券数量为1000
                    bllCouponType.UpdateCouponTypeIssuedQty(coupon.CouponTypeID.ToString(), intCouponLenth);
                }

                //这里可能是重置 Redis 缓存中该类型优惠券数据数量
                RedisOpenAPI.Instance.CCCoupon().SetCouponList(new CC_Coupon()
                {
                    CustomerId = couponType.CustomerId,
                    CouponTypeId = couponType.CouponTypeID.ToString(),
                    CouponTypeDesc = couponType.CouponTypeDesc,
                    CouponTypeName = couponType.CouponTypeName,
                    BeginTime = couponType.BeginTime.ToString(),
                    EndTime = couponType.EndTime.ToString(),
                    ServiceLife = couponType.ServiceLife ?? 0,
                    CouponLenth = intCouponLenth,
                    CouponCategory = couponType.CouponCategory
                });

            }
        }

        /// <summary>
        /// 批量
        /// </summary>
        public void InsertDataBase()
        {

            BaseService.WriteLog("---------------------------vip绑定优惠券开始---------------------------");

            var numCount = 30;
            var customerIDs = CustomerBLL.Instance.GetCustomerList();
            foreach (var customer in customerIDs)
            {
                try
                {

                    LoggingSessionInfo loggingSessionInfo = CustomerBLL.Instance.GetBSLoggingSession(customer.Key, "RedisSystem");

                    DataTable dtCoupon = CreateTableCoupon();

                    DataTable dtVipCoupon = CreateTableVipCoupon();

                    var count = RedisOpenAPI.Instance.CCVipMappingCoupon().GetVipMappingCouponLength(new CC_VipMappingCoupon
                    {
                        CustomerId = customer.Key
                    });
                    if (count.Code != ResponseCode.Success)
                    {
                        BaseService.WriteLog("从redis获取待绑定优惠券数量失败");
                        continue;
                    }
                    if (count.Result <= 0)
                    {
                        continue;
                    }
                    BaseService.WriteLog("优惠券redis取数据：" + customer.Key);
                    if (count.Result < numCount)
                    {
                        numCount = Convert.ToInt32(count.Result);
                    }

                    for (var i = 0; i < numCount; i++)
                    {
                        BaseService.WriteLog("---------------------------vip绑定优惠券长度:" + count.Result.ToString());
                        var response = RedisOpenAPI.Instance.CCVipMappingCoupon().GetVipMappingCoupon(new CC_VipMappingCoupon
                        {
                            CustomerId = customer.Key
                        });
                        if (response.Code == ResponseCode.Success)
                        {
                            DataRow dr_Coupon = dtCoupon.NewRow();
                            dr_Coupon["CouponID"] = response.Result.Coupon.CouponId;
                            dr_Coupon["CouponCode"] = response.Result.Coupon.CouponCode;
                            dr_Coupon["CouponDesc"] = response.Result.Coupon.CouponTypeDesc;
                            if (response.Result.Coupon.ServiceLife > 0)
                            {
                                dr_Coupon["BeginDate"] = DateTime.Now;
                                dr_Coupon["EndDate"] = DateTime.Now.Date.AddDays(response.Result.Coupon.ServiceLife - 1).ToShortDateString() + " 23:59:59.998";

                            }
                            else
                            {
                                dr_Coupon["BeginDate"] = response.Result.Coupon.BeginTime;
                                dr_Coupon["EndDate"] = response.Result.Coupon.EndTime;
                            }
                            dr_Coupon["CouponUrl"] = "";
                            dr_Coupon["ImageUrl"] = "";
                            dr_Coupon["Status"] = 2;
                            dr_Coupon["CreateTime"] = DateTime.Now;
                            dr_Coupon["CreateBy"] = "Redis";
                            dr_Coupon["LastUpdateTime"] = DateTime.Now;
                            dr_Coupon["LastUpdateBy"] = "Redis";
                            dr_Coupon["IsDelete"] = 0;
                            dr_Coupon["CouponTypeID"] = response.Result.Coupon.CouponTypeId;
                            dr_Coupon["CoupnName"] = response.Result.Coupon.CouponTypeName;
                            dr_Coupon["DoorID"] = "";
                            dr_Coupon["CouponPwd"] = "";
                            dr_Coupon["CollarCardMode"] = "";
                            dr_Coupon["CustomerID"] = customer.Key;
                            dtCoupon.Rows.Add(dr_Coupon);

                            DataRow dr_VipCoupon = dtVipCoupon.NewRow();
                            dr_VipCoupon["VipCouponMapping"] = Guid.NewGuid().ToString().Replace("-", "");
                            dr_VipCoupon["VIPID"] = response.Result.VipId;
                            dr_VipCoupon["CouponID"] = response.Result.Coupon.CouponId;
                            dr_VipCoupon["UrlInfo"] = "";
                            dr_VipCoupon["IsDelete"] = 0;
                            dr_VipCoupon["LastUpdateBy"] = "Redis";
                            dr_VipCoupon["LastUpdateTime"] = DateTime.Now;
                            dr_VipCoupon["CreateBy"] = "Redis";
                            dr_VipCoupon["CreateTime"] = DateTime.Now;
                            dr_VipCoupon["FromVipId"] = "";
                            dr_VipCoupon["ObjectId"] = response.Result.ObjectId;
                            dr_VipCoupon["CouponSourceId"] = GetSourceId(response.Result.Source);
                            dtVipCoupon.Rows.Add(dr_VipCoupon);
//活动或者虚拟商品发优惠券，不发通知
                            if (response.Result.Source != "Activity" && response.Result.Source != "PayVirtualItem")
                            {//会员活动延迟发送
                                try
                                {
                                    ///优惠券到账通知
                                    var CommonBLL = new CommonBLL();
                                    var bllVip = new VipBLL(loggingSessionInfo);
                                    var vip = bllVip.GetByID(response.Result.VipId);

                                    string strValidityData = Convert.ToDateTime(dr_Coupon["BeginDate"].ToString()).ToShortDateString() + "-" + Convert.ToDateTime(dr_Coupon["EndDate"].ToString()).ToShortDateString();
                                    CommonBLL.CouponsArrivalMessage(response.Result.Coupon.CouponCode, response.Result.Coupon.CouponTypeName, strValidityData, response.Result.Coupon.CouponCategory == null ? "" : response.Result.Coupon.CouponCategory, vip.WeiXinUserId, loggingSessionInfo);
                                }
                                catch (Exception ex)
                                {
                                    BaseService.WriteLog("优惠券到账通知异常：" + ex.Message);
                                    continue;
                                }
                            }
                        }
                    }
                    if (dtCoupon != null && dtCoupon.Rows.Count > 0)
                    {
                        SqlBulkCopy(customer.Value, dtCoupon, "Coupon");
                        var bllCouponType = new CouponTypeBLL(loggingSessionInfo);
                        bllCouponType.UpdateCouponTypeIsVoucher(customer.Key);
                        BaseService.WriteLog("批量插入Coupon:");
                    }
                    if (dtVipCoupon != null && dtVipCoupon.Rows.Count > 0)
                    {
                        SqlBulkCopy(customer.Value, dtVipCoupon, "VipCouponMapping");
                        BaseService.WriteLog("批量插入VipCouponMapping:");
                    }
                    //BaseService.WriteLog("延迟时间开始");
                    //Thread.Sleep(1000);
                    //BaseService.WriteLog("延迟时间结束");

                }
                catch (Exception ex)
                {
                    BaseService.WriteLog("vip绑定优惠券异常" + ex.Message);
                    continue;
                }
            }
            BaseService.WriteLog("---------------------------vip绑定优惠券结束---------------------------");


        }
        public void InsertDataBase(string strCustomerId, string strVipId, string strObjectId, string strSource, CC_Coupon coupon)
        {
            try
            {
                string customerCon = GetCustomerConn(strCustomerId);

                LoggingSessionInfo loggingSessionInfo = new LoggingSessionInfo();
                LoggingManager CurrentLoggingManager = new LoggingManager();
                loggingSessionInfo.ClientID = strCustomerId;
                CurrentLoggingManager.Connection_String = customerCon;
                loggingSessionInfo.CurrentLoggingManager = CurrentLoggingManager;
                loggingSessionInfo.CurrentUser = new BS.Entity.User.UserInfo();
                loggingSessionInfo.CurrentUser.customer_id = strCustomerId;

                DataTable dtCoupon = new DataTable();
                dtCoupon.Columns.Add("CouponID", typeof(string));
                dtCoupon.Columns.Add("CouponCode", typeof(string));
                dtCoupon.Columns.Add("CouponDesc", typeof(string));
                dtCoupon.Columns.Add("BeginDate", typeof(DateTime));
                dtCoupon.Columns.Add("EndDate", typeof(DateTime));
                dtCoupon.Columns.Add("CouponUrl", typeof(string));
                dtCoupon.Columns.Add("ImageUrl", typeof(string));
                dtCoupon.Columns.Add("Status", typeof(Int32));
                dtCoupon.Columns.Add("CreateTime", typeof(DateTime));
                dtCoupon.Columns.Add("CreateBy", typeof(string));
                dtCoupon.Columns.Add("LastUpdateTime", typeof(DateTime));
                dtCoupon.Columns.Add("LastUpdateBy", typeof(string));
                dtCoupon.Columns.Add("IsDelete", typeof(Int32));
                dtCoupon.Columns.Add("CouponTypeID", typeof(string));
                dtCoupon.Columns.Add("CoupnName", typeof(string));
                dtCoupon.Columns.Add("DoorID", typeof(string));
                dtCoupon.Columns.Add("CouponPwd", typeof(string));
                dtCoupon.Columns.Add("CollarCardMode", typeof(string));
                dtCoupon.Columns.Add("CustomerID", typeof(string));
                DataTable dtVipCoupon = new DataTable();
                dtVipCoupon.Columns.Add("VipCouponMapping", typeof(string));
                dtVipCoupon.Columns.Add("VIPID", typeof(string));
                dtVipCoupon.Columns.Add("CouponID", typeof(string));
                dtVipCoupon.Columns.Add("UrlInfo", typeof(string));
                dtVipCoupon.Columns.Add("IsDelete", typeof(Int32));
                dtVipCoupon.Columns.Add("LastUpdateBy", typeof(string));
                dtVipCoupon.Columns.Add("LastUpdateTime", typeof(DateTime));
                dtVipCoupon.Columns.Add("CreateBy", typeof(string));
                dtVipCoupon.Columns.Add("CreateTime", typeof(DateTime));
                dtVipCoupon.Columns.Add("FromVipId", typeof(string));
                dtVipCoupon.Columns.Add("ObjectId", typeof(string));
                dtVipCoupon.Columns.Add("CouponSourceId", typeof(string));

                RedisCouponBLL redisCouponBLL = new RedisCouponBLL();
                var response = redisCouponBLL.RedisGetCoupon(coupon);


                if (response.Code == ResponseCode.Success && response.Result.CouponTypeId != null)
                {
                    //String uperStr = StringUtil.GetRandomUperStr(4);
                    //String strInt = StringUtil.GetRandomStrInt(8);
                    //string strCouponCode = uperStr + "-" + strInt;
                    string strCouponCode = string.Empty;
                    strCouponCode = GetCouponNum(loggingSessionInfo, strCustomerId);
                    string strCouponId = Guid.NewGuid().ToString();

                    DataRow dr_Coupon = dtCoupon.NewRow();
                    dr_Coupon["CouponID"] = strCouponId;
                    dr_Coupon["CouponCode"] = strCouponCode;
                    dr_Coupon["CouponDesc"] = response.Result.CouponTypeDesc;
                    if (response.Result.ServiceLife > 0)
                    {
                        dr_Coupon["BeginDate"] = DateTime.Now;
                        dr_Coupon["EndDate"] = DateTime.Now.Date.AddDays(response.Result.ServiceLife - 1).ToShortDateString() + " 23:59:59.998";

                    }
                    else
                    {
                        dr_Coupon["BeginDate"] = response.Result.BeginTime;
                        dr_Coupon["EndDate"] = response.Result.EndTime;
                    }
                    dr_Coupon["CouponUrl"] = "";
                    dr_Coupon["ImageUrl"] = "";
                    dr_Coupon["Status"] = 2;
                    dr_Coupon["CreateTime"] = DateTime.Now;
                    dr_Coupon["CreateBy"] = "Redis";
                    dr_Coupon["LastUpdateTime"] = DateTime.Now;
                    dr_Coupon["LastUpdateBy"] = "Redis";
                    dr_Coupon["IsDelete"] = 0;
                    dr_Coupon["CouponTypeID"] = response.Result.CouponTypeId;
                    dr_Coupon["CoupnName"] = response.Result.CouponTypeName;
                    dr_Coupon["DoorID"] = "";
                    dr_Coupon["CouponPwd"] = "";
                    dr_Coupon["CollarCardMode"] = "";
                    dr_Coupon["CustomerID"] = strCustomerId;
                    dtCoupon.Rows.Add(dr_Coupon);

                    DataRow dr_VipCoupon = dtVipCoupon.NewRow();
                    dr_VipCoupon["VipCouponMapping"] = Guid.NewGuid().ToString().Replace("-", "");
                    dr_VipCoupon["VIPID"] = strVipId;
                    dr_VipCoupon["CouponID"] = strCouponId;
                    dr_VipCoupon["UrlInfo"] = "";
                    dr_VipCoupon["IsDelete"] = 0;
                    dr_VipCoupon["LastUpdateBy"] = "Redis";
                    dr_VipCoupon["LastUpdateTime"] = DateTime.Now;
                    dr_VipCoupon["CreateBy"] = "Redis";
                    dr_VipCoupon["CreateTime"] = DateTime.Now;
                    dr_VipCoupon["FromVipId"] = "";
                    dr_VipCoupon["ObjectId"] = strObjectId;
                    dr_VipCoupon["CouponSourceId"] = GetSourceId(strSource);


                    dtVipCoupon.Rows.Add(dr_VipCoupon);
                    if (strSource != "Activity")
                    {
                        try
                        {
                            ///优惠券到账通知
                            var CommonBLL = new CommonBLL();
                            var bllVip = new VipBLL(loggingSessionInfo);
                            var vip = bllVip.GetByID(strVipId);

                            string strValidityData = Convert.ToDateTime(dr_Coupon["BeginDate"].ToString()).ToShortDateString() + "-" + Convert.ToDateTime(dr_Coupon["EndDate"].ToString()).ToShortDateString();
                            CommonBLL.CouponsArrivalMessage(response.Result.CouponCode, response.Result.CouponTypeName, strValidityData, response.Result.CouponCategory == null ? "" : response.Result.CouponCategory, vip.WeiXinUserId, loggingSessionInfo);
                        }
                        catch (Exception ex)
                        {
                            BaseService.WriteLog("优惠券到账通知异常：" + ex.Message);

                        }
                    }
                }

                if (dtCoupon != null && dtCoupon.Rows.Count > 0)
                {
                    SqlBulkCopy(customerCon, dtCoupon, "Coupon");

                    var bllCouponType = new CouponTypeBLL(loggingSessionInfo);
                    bllCouponType.UpdateCouponTypeIsVoucher(strCustomerId);

                }
                if (dtVipCoupon != null && dtVipCoupon.Rows.Count > 0)
                {
                    SqlBulkCopy(customerCon, dtVipCoupon, "VipCouponMapping");

                }
            }
            catch (Exception ex)
            {
                BaseService.WriteLog("vip绑定优惠券异常" + ex.Message);
            }
            BaseService.WriteLog("---------------------------vip绑定优惠券结束---------------------------");

        }
        /// <summary>
        /// 批量插入数据库
        /// </summary>
        /// <param name="strCon"></param>
        /// <param name="dt"></param>
        /// <param name="strTableName"></param>
        public void SqlBulkCopy(string strCon, DataTable dt, string strTableName)
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
                        {
                            sqlbulkcopy.DestinationTableName = strTableName;
                            sqlbulkcopy.BatchSize = dt.Rows.Count;
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                            }
                            sqlbulkcopy.WriteToServer(dt);
                            trans.Commit();

                            //sqlbulkcopy.BatchSize = 1000000;
                            //sqlbulkcopy.DestinationTableName = "db_User";
                            //sqlbulkcopy.WriteToServer(dt);
                            //trans.Commit();
                        }
                    }
                    catch(Exception ex)
                    {
                        BaseService.WriteLog(string.Format("批量插入数据库表:{0} 时发生异常，异常信息{1}", strTableName, ex.Message));
                        trans.Rollback();
                    }
                }
            }
        }
        public string GetSourceId(string strType)
        {
            switch (strType)
            {
                case "Reg":
                    return "FD977FBA-03F9-4AC0-805F-75A56BD6A429";
                case "Comment":
                    return "80EF450A-5CEF-4DB6-B28D-420BDDA59894";
                case "Focus":
                    return "945C3C2D88DF4AFAA260A1CED81C6870";
                case "Share":
                    return "5F671057-E6B5-4B5E-B2D4-AC3A64F6710F";
                case "Game":
                    return "7D87E7E1-66AC-403B-9BB4-80AE4278F6A4";
                case "CTW":
                    return "07231C5F-8B9B-4A67-BCF5-B7D1C95CEA8E";
                case "Activity":
                    return "2E71459A-C9F7-493D-8D50-99D462030113";
                case "Grant":
                    return "22E189E1-57C2-488E-A1DA-C42AEBAF3766";
                case "OpenVipCard":
                    return "FEE6AF84-0BC1-4575-AB27-9400B5998D8A";
                default:
                    return "";
            }
        }

        public static string GetCustomerConn(string customerId)
        {
            string sql = string.Format(
                "select 'user id='+a.db_user+';password='+a.db_pwd+';data source='+a.db_server+';database='+a.db_name+';' conn " +
                " from t_customer_connect a where a.customer_id='{0}' ",
                customerId);
            string conn = ConfigurationManager.AppSettings["Conn_ap"];
            DefaultSQLHelper sqlHelper = new DefaultSQLHelper(conn);
            var result = sqlHelper.ExecuteScalar(sql);
            return result == null || result == DBNull.Value ? string.Empty : result.ToString();
        }

        public DataTable CreateTableCoupon()
        {
            DataTable dtCoupon = new DataTable();
            dtCoupon.Columns.Add("CouponID", typeof(string));
            dtCoupon.Columns.Add("CouponCode", typeof(string));
            dtCoupon.Columns.Add("CouponDesc", typeof(string));
            dtCoupon.Columns.Add("BeginDate", typeof(DateTime));
            dtCoupon.Columns.Add("EndDate", typeof(DateTime));
            dtCoupon.Columns.Add("CouponUrl", typeof(string));
            dtCoupon.Columns.Add("ImageUrl", typeof(string));
            dtCoupon.Columns.Add("Status", typeof(Int32));
            dtCoupon.Columns.Add("CreateTime", typeof(DateTime));
            dtCoupon.Columns.Add("CreateBy", typeof(string));
            dtCoupon.Columns.Add("LastUpdateTime", typeof(DateTime));
            dtCoupon.Columns.Add("LastUpdateBy", typeof(string));
            dtCoupon.Columns.Add("IsDelete", typeof(Int32));
            dtCoupon.Columns.Add("CouponTypeID", typeof(string));
            dtCoupon.Columns.Add("CoupnName", typeof(string));
            dtCoupon.Columns.Add("DoorID", typeof(string));
            dtCoupon.Columns.Add("CouponPwd", typeof(string));
            dtCoupon.Columns.Add("CollarCardMode", typeof(string));
            dtCoupon.Columns.Add("CustomerID", typeof(string));

            return dtCoupon;
        }
        public DataTable CreateTableVipCoupon()
        {
            DataTable dtVipCoupon = new DataTable();
            dtVipCoupon.Columns.Add("VipCouponMapping", typeof(string));
            dtVipCoupon.Columns.Add("VIPID", typeof(string));
            dtVipCoupon.Columns.Add("CouponID", typeof(string));
            dtVipCoupon.Columns.Add("UrlInfo", typeof(string));
            dtVipCoupon.Columns.Add("IsDelete", typeof(Int32));
            dtVipCoupon.Columns.Add("LastUpdateBy", typeof(string));
            dtVipCoupon.Columns.Add("LastUpdateTime", typeof(DateTime));
            dtVipCoupon.Columns.Add("CreateBy", typeof(string));
            dtVipCoupon.Columns.Add("CreateTime", typeof(DateTime));
            dtVipCoupon.Columns.Add("FromVipId", typeof(string));
            dtVipCoupon.Columns.Add("ObjectId", typeof(string));
            dtVipCoupon.Columns.Add("CouponSourceId", typeof(string));

            return dtVipCoupon;
        }
        /// <summary>
        /// 获取优惠券码
        /// </summary>
        /// <param name="loggingSession"></param>
        /// <param name="strCustomerId"></param>
        /// <returns></returns>
        public string GetCouponNum(LoggingSessionInfo loggingSession, string strCustomerId)
        {
            string uperStr = StringUtil.GetRandomUperStr(4);
            string strBackInt = StringUtil.GetRandomStrInt(8);
            string strAllInt = StringUtil.GetRandomStrInt(12);
            string strCouponNumRole = string.Empty;
            string strCouponCode = string.Empty;
            //判断是否有特殊的优惠券编码
            BasicSetting.BasicSettingBLL bllBasicSetting = new BasicSetting.BasicSettingBLL();
            var basicSettingList = bllBasicSetting.GetBasicSetting(strCustomerId);
            if (basicSettingList.Count > 0)
            {
                var query = from q in basicSettingList.AsEnumerable()
                            where q.SettingCode.Contains("CouponNumRole")
                            select q;
                if (query.FirstOrDefault() != null)
                    strCouponNumRole = query.FirstOrDefault().SettingValue;
            }
            else
            {
                CustomerBasicSettingBLL bllBasic = new CustomerBasicSettingBLL(loggingSession);
                var entityBasic = bllBasic.QueryByEntity(new CustomerBasicSettingEntity() { SettingCode = "CouponNumRole" }, null).SingleOrDefault();
                if (entityBasic != null)
                {
                    strCouponNumRole = entityBasic.SettingValue;
                }
            }
            if (!string.IsNullOrEmpty(strCouponNumRole) && strCouponNumRole == "1")//设置了“1”，则使用全是数字的优惠券编码规则
                strCouponCode = strAllInt;
            else
                strCouponCode = uperStr + "-" + strBackInt;
            return strCouponCode;
        }
    }

}

