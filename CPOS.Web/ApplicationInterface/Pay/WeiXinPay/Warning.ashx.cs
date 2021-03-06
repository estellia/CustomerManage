﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.DTO.Base;
using JIT.Utility.ExtensionMethod;

namespace JIT.CPOS.Web.ApplicationInterface.Pay.WeiXinPay
{
    /// <summary>
    /// Warning 的摘要说明
    /// </summary>
    public class Warning : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");

            try
            {
                string xmlStr = string.Empty;
                using (var stream = context.Request.InputStream)
                {
                    using (var rd = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        xmlStr = rd.ReadToEnd();
                        JIT.Utility.Log.Loggers.Debug(new Utility.Log.DebugLogInfo()
                        {
                            Message = string.Format("警告通知:{0}", xmlStr)
                        });
                    }
                }
                var doc = new XmlDocument();
                doc.LoadXml(xmlStr);
                var dic = new Dictionary<string, string>();
                
                //遍历参数节点
                var list = doc.SelectNodes("xml/*");

                foreach (XmlNode item in list)
                {
                    dic[item.Name] = item.InnerText;
                }
                var warning = dic.ToJSON().DeserializeJSONTo<WxWarningInfo>();

                var customerWxMappingBll = new TCustomerWeiXinMappingBLL(Default.GetAPLoggingSession(""));

                var customerId = customerWxMappingBll.GetCustomerIdByAppId(warning.AppId);
                if (customerId == "")
                {
                    throw new APIException("客户ID为空") { ErrorCode = 121 };
                }

                var currentUserInfo = Default.GetBSLoggingSession(customerId, "1");

                
                var alarmBll = new WXAlarmNoticeBLL(currentUserInfo);

                var alarmEntity = new WXAlarmNoticeEntity();
                alarmEntity.AlarmNoticeId = Guid.NewGuid();
                alarmEntity.AlarmNoticeCode = warning.ErrorType;//错误类型
                alarmEntity.AlarmNoticeDesc = warning.AlarmContent;//错误详情
                alarmEntity.AlarmNoticeRemark = warning.Description;//错误描述
                alarmEntity.AlarmNoticeStatus = 10;
                alarmEntity.Priority = 10;
                alarmEntity.CustomerId = customerId;
                if (warning.ErrorType == "1001")//发货超时
                {
                    alarmEntity.ProposalPlan = warning.AlarmContent.Substring(warning.AlarmContent.IndexOf('=') + 1);//微信订单号
                }
                alarmBll.Create(alarmEntity);


                #region 向表中记录调用的微信接口

                var wxInterfaceLogBll = new WXInterfaceLogBLL(currentUserInfo);
                var wxInterfaceLogEntity = new WXInterfaceLogEntity();
                wxInterfaceLogEntity.LogId = Guid.NewGuid();
                wxInterfaceLogEntity.InterfaceUrl = "微信公众号配置的警告URL";
                wxInterfaceLogEntity.AppId = warning.AppId;
                wxInterfaceLogEntity.RequestParam = warning.ToJSON();
                wxInterfaceLogEntity.IsSuccess = 1;
                wxInterfaceLogBll.Create(wxInterfaceLogEntity);

                #endregion

                context.Response.Write("success");

            }
            catch (Exception ex)
            {
                JIT.Utility.Log.Loggers.Debug(new Utility.Log.DebugLogInfo() { Message = ex.Message });
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class WxWarningInfo
    {
        public string AppId { get; set; }
        public string ErrorType { get; set; }
        public string Description { get; set; }
        public string AlarmContent { get; set; }
        public string TimeStamp { get; set; }
        public string AppSignature { get; set; }
        public  string SignMethod { get; set; }
    }
}