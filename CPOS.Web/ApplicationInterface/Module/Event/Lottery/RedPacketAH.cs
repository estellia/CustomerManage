﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using JIT.CPOS.Web.ApplicationInterface.Base;
using JIT.CPOS.DTO.Module.Event.Lottery.Request;
using JIT.CPOS.DTO.Module.Event.Lottery.Response;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.DTO.Base;
using JIT.Utility.Log;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.DataAccess.Query;
using JIT.CPOS.BS.BLL.WX;

using System.Threading;
namespace JIT.CPOS.Web.ApplicationInterface.Module.Event.Lottery
{
    public class RedPacketAH : BaseActionHandler<LotteryRP, LotteryRD>
    {

        protected override LotteryRD ProcessRequest(DTO.Base.APIRequest<LotteryRP> pRequest)
        {
            var rd = new LotteryRD();//返回值
            var bllPrize = new LPrizesBLL(this.CurrentUserInfo);
            var para = pRequest.Parameters;

            if (para.EventId != null && para.EventId != "")
            {
                try
                {

                    rd = bllPrize.RedisRedPacket(pRequest.UserID, para.EventId, pRequest.CustomerID);

                }
                catch (Exception ex)
                {
                    rd.ErrCode = -1;
                    rd.ResultMsg = ex.Message.ToString();
                }
            }
            else
            {
                rd.ErrCode = -2;
                rd.ResultMsg = "参数EventId有误";

            }

            return rd;
          
        }

        public static void TestMethod(LoggingSessionInfo userinfo,string strUserId,string strEventId,string strCustomerId)
        {
            var bllPrize = new LPrizesBLL(userinfo);

            bllPrize.RedPacket2(strUserId, strEventId, strCustomerId);
        }

    }

}