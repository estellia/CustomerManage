﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using JIT.CPOS.BS.BLL.WX.Enum;
using JIT.CPOS.BS.DataAccess;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Entity.WX;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.Log;
using JIT.Utility.DataAccess;
using JIT.Utility.Cache2;
using System.Text.RegularExpressions;
using JIT.CPOS.Common;
using System.Xml;

namespace JIT.CPOS.BS.BLL.WX
{
    /// <summary>
    /// 微信公共类
    /// </summary>
    public class CommonBLL
    {
        #region 构造函数

        public CommonBLL() { }

        #endregion

        #region 提交表单数据

        /// <summary>
        /// 提交表单数据
        /// </summary>
        /// <param name="uri">提交数据的URI</param>
        /// <param name="method">GET, POST</param>
        /// <param name="content">提交内容</param>
        /// <returns></returns>
        public static string GetRemoteData(string uri, string method, string content)
        {
            string respData = "";
            method = method.ToUpper();
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            req.KeepAlive = false;
            req.Method = method.ToUpper();
            req.Credentials = System.Net.CredentialCache.DefaultCredentials;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            if (method == "POST")
            {
                //byte[] buffer = Encoding.ASCII.GetBytes(content);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
                req.ContentLength = buffer.Length;
                req.ContentType = "application/x-www-form-urlencoded";
                Stream postStream = req.GetRequestStream();
                postStream.Write(buffer, 0, buffer.Length);
                postStream.Close();
            }
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;//提交参数，获取结果
            Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
            StreamReader loResponseStream = new StreamReader(resp.GetResponseStream(), enc);
            respData = loResponseStream.ReadToEnd();
            loResponseStream.Close();
            resp.Close();
            return respData;
        }
        public static string GetRemoteDataForRedis(string uri, string method, string content)
        {
            string respData = "";
            method = method.ToUpper();
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            req.KeepAlive = false;
            req.Method = method.ToUpper();
            req.Credentials = System.Net.CredentialCache.DefaultCredentials;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            if (method == "POST")
            {
                //byte[] buffer = Encoding.ASCII.GetBytes(content);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
                req.ContentLength = buffer.Length;
                req.ContentType = "application/json";
                Stream postStream = req.GetRequestStream();
                postStream.Write(buffer, 0, buffer.Length);
                postStream.Close();
            }
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;//提交参数，获取结果
            Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
            StreamReader loResponseStream = new StreamReader(resp.GetResponseStream(), enc);
            respData = loResponseStream.ReadToEnd();
            loResponseStream.Close();
            resp.Close();
            return respData;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #endregion

        #region 验证token

        /// <summary>
        /// 验证token
        /// </summary>
        public void ValidToken(HttpContext httpContext, string token)
        {
            BaseService.WriteLogWeixin("开始执行token验证");

            if (httpContext.Request["echoStr"] != null)
            {
                var echostr = httpContext.Request["echoStr"].ToString();

                BaseService.WriteLogWeixin("echoStr = " + echostr);

                if (CheckSignature(httpContext, token) && !string.IsNullOrEmpty(echostr))
                {
                    BaseService.WriteLogWeixin("结束执行token验证");

                    //推送...不然微信平台无法验证token
                    httpContext.Response.Write(echostr);
                }
            }
            else
            {
                BaseService.WriteLogWeixin("echoStr is null");
            }
        }

        /// <summary>
        /// 加密/校验流程：
        /// 1. 将token、timestamp、nonce三个参数进行字典序排序
        /// 2. 将三个参数字符串拼接成一个字符串进行sha1加密
        /// 3. 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信
        /// </summary>
        /// <returns></returns>
        private bool CheckSignature(HttpContext httpContext, string token)
        {
            var signature = httpContext.Request["signature"].ToString(); //获取到signature
            var timestamp = httpContext.Request["timestamp"].ToString();
            var nonce = httpContext.Request["nonce"].ToString();

            BaseService.WriteLogWeixin("token = " + token);
            BaseService.WriteLogWeixin("微信传过来的 signature = " + signature + "   \n");

            //字典排序
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);

            //sha1加密
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            BaseService.WriteLogWeixin("本地加密生成的 signature = " + tmpStr + "   \n");

            if (tmpStr == signature)
            {
                BaseService.WriteLogWeixin("token验证成功");
                return true;
            }
            else
            {
                BaseService.WriteLogWeixin("token验证失败");
                return false;
            }
        }

        #endregion

        #region 获取凭证接口
        /// <summary>
        /// 在使用通用接口前，你需要做以下两步工作:
        /// 1.拥有一个微信公众账号，并获取到appid和appsecret
        /// 2.通过获取凭证接口获取到access_token
        /// access_token是第三方访问微信公众平台api资源的票据。
        /// </summary>
        /// <param name="appID">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <returns></returns>
        public AccessTokenEntity GetAccessTokenByCache(string appID, string appSecret, LoggingSessionInfo loggingSessionInfo)
        {
            //MarketSendLogBLL logServer = new MarketSendLogBLL(loggingSessionInfo);
            //MarketSendLogEntity logInfo = new MarketSendLogEntity();
            //logInfo.LogId = BaseService.NewGuidPub();
            //logInfo.VipId = appID;
            //logInfo.MarketEventId = appSecret;
            //logInfo.TemplateContent = loggingSessionInfo.CurrentUser.customer_id.ToString();
            //logInfo.IsSuccess = 1;
            //logInfo.SendTypeId = "2";
            //logInfo.CreateTime = System.DateTime.Now;

            //logServer.Create(logInfo);

            WApplicationInterfaceBLL wApplicationInterfaceBLL = new WApplicationInterfaceBLL(loggingSessionInfo);
            WApplicationInterfaceEntity appObj = null;
            var appList = wApplicationInterfaceBLL.QueryByEntity(new WApplicationInterfaceEntity
            {
                AppID = appID,
                AppSecret = appSecret,
                CustomerId = loggingSessionInfo.ClientID.ToString()//用clientid比较稳妥
            }, null);
            //var appList = wApplicationInterfaceBLL.GetWebWApplicationInterface(new WApplicationInterfaceEntity() {
            //    AppID = appID,
            //    AppSecret = appSecret
            //}, 0, 1);
            ////WApplicationInterfaceEntity appObj = null;
            if (appList != null && appList.Length > 0)
            {
                if (appList[0].IsHeight == 0)
                {
                    //获取云店公众号信息 Add by Henry 
                    appList = wApplicationInterfaceBLL.QueryByEntity(new WApplicationInterfaceEntity
                    {
                        AppID = appID,
                        AppSecret = appSecret,
                        CustomerId = ConfigurationManager.AppSettings["CloudCustomerId"]
                    }, null);
                }
                appObj = appList[0];
            }
            else
            {
                //获取云店公众号信息 Add by Henry
                appList = wApplicationInterfaceBLL.QueryByEntity(new WApplicationInterfaceEntity
                {
                    AppID = appID,
                    AppSecret = appSecret,
                    CustomerId = ConfigurationManager.AppSettings["CloudCustomerId"]
                }, null);
                appObj = appList[0];
                //throw new Exception("未查询到公众号");
            }
            var accessToken = new AccessTokenEntity();
            if (appObj.ExpirationTime == null || appObj.ExpirationTime <= DateTime.Now)
            {
                BaseService.WriteLogWeixin("获取凭证接口： ");
                BaseService.WriteLogWeixin("appID： " + appID);
                BaseService.WriteLogWeixin("appSecret： " + appSecret);
                string uri = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appID + "&secret=" + appSecret;
                string method = "GET";
                string data = string.Empty;
                OpenAccessTokenEntity openAccessToken;
                //调用微信登录授权接口刷新凭证
                if (string.IsNullOrEmpty(appSecret))
                {
                    var openOAuthUrl = ConfigurationManager.AppSettings["openOAuthUrl"];
                    var openuri = openOAuthUrl + "/OpenOAuth/RefreshAuthorizerAccessToken?authorizerId=" + appID;
                    var opendata = GetRemoteData(openuri, method, string.Empty);
                    openAccessToken = opendata.DeserializeJSONTo<OpenAccessTokenEntity>();
                    accessToken.access_token = openAccessToken.authorizer_access_token;
                    accessToken.expires_in = openAccessToken.expires_in;
                    accessToken.errcode = openAccessToken.errcode;
                    accessToken.errmsg = openAccessToken.errmsg;
                }
                else
                {
                    data = GetRemoteData(uri, method, string.Empty);
                    accessToken = data.DeserializeJSONTo<AccessTokenEntity>();
                }



                BaseService.WriteLogWeixin("调用获取凭证接口返回值： " + data);
                Loggers.Debug(new DebugLogInfo() { Message = "调用获取凭证接口返回值： " + data });


                //AccessTokenEntity accessToken = new AccessTokenEntity();
                //accessToken.access_token = "jDsQzSF8o68i-YqVyNZUaorxpA4-EMhliWBi5Y1XKNuHB_bjGS3UYlwc_G5iHkv_FKdbheftp_FMZk1StB7gfSFkkjnKZGJP78fZ104DsSXw-6WzNl_Os_HnbEoonx9Sz2mcxSJMssZ02WndXZfedw";
                //accessToken.expires_in = "7200";

                appObj.RequestToken = accessToken.access_token;
                appObj.ExpirationTime = DateTime.Now.AddHours(1);//token的过期时间不是两小时吗？为什么这里只加了一小时
                wApplicationInterfaceBLL.Update(appObj, false);
            }
            else
            {
                accessToken = new AccessTokenEntity();
                accessToken.access_token = appObj.RequestToken;
                accessToken.expires_in = "7200";

                Loggers.Debug(new DebugLogInfo() { Message = "使用未过期的access token:" + appObj.RequestToken + ", 到期时间：" + appObj.ExpirationTime });
            }
            //logInfo.LogId = BaseService.NewGuidPub();
            //logInfo.VipId = appObj.AppID;
            //logInfo.MarketEventId = appObj.AppSecret;
            //logInfo.TemplateContent = appObj.RequestToken;
            //logInfo.IsSuccess = 1;
            //logInfo.SendTypeId = "2";
            //logInfo.WeiXinUserId = appObj.WeiXinID;

            //logServer.Create(logInfo);
            return accessToken;
        }

        #endregion

        #region 获取jsapi_ticket
        /// <summary>
        /// 在使用通用接口前，你需要做以下两步工作:
        /// 1.拥有一个微信公众账号，并获取到appid和appsecret
        /// 2.通过获取凭证接口获取到access_token
        /// access_token是第三方访问微信公众平台api资源的票据。
        /// </summary>
        /// <param name="appID">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <returns></returns>
        public JsApiTicketEntity GetJsApiTicketByCache(string appID, string appSecret, LoggingSessionInfo loggingSessionInfo)
        {
            MarketSendLogBLL logServer = new MarketSendLogBLL(loggingSessionInfo);
            MarketSendLogEntity logInfo = new MarketSendLogEntity();
            logInfo.LogId = BaseService.NewGuidPub();
            logInfo.VipId = appID;
            logInfo.MarketEventId = appSecret;
            logInfo.TemplateContent = loggingSessionInfo.CurrentUser.customer_id.ToString();
            logInfo.IsSuccess = 1;
            logInfo.SendTypeId = "2";
            logInfo.CreateTime = System.DateTime.Now;

            logServer.Create(logInfo);

            WApplicationInterfaceBLL wApplicationInterfaceBLL = new WApplicationInterfaceBLL(loggingSessionInfo);
            WApplicationInterfaceEntity appObj = null;
            var appList = wApplicationInterfaceBLL.QueryByEntity(new WApplicationInterfaceEntity
            {
                AppID = appID
                ,
                AppSecret = appSecret
                ,
                CustomerId = loggingSessionInfo.CurrentUser.customer_id.ToString()
            }, null);

            if (appList != null && appList.Length > 0)
            {
                appObj = appList[0];
            }
            else
            {
                throw new Exception("未查询到公众号");
            }
            JsApiTicketEntity jsApiTicket = null;
            if (appObj.TicketExpirationTime == null || appObj.TicketExpirationTime <= DateTime.Now)
            {
                BaseService.WriteLogWeixin("获取jsapi_ticket接口： ");
                BaseService.WriteLogWeixin("appID： " + appID);
                BaseService.WriteLogWeixin("appSecret： " + appSecret);

                AccessTokenEntity token = this.GetAccessTokenByCache(appID, appSecret, loggingSessionInfo);

                string uri = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?type=jsapi&access_token=" + token.access_token;
                string method = "GET";
                string data = GetRemoteData(uri, method, string.Empty);

                BaseService.WriteLogWeixin("调用获取jsapi_ticket接口返回值： " + data);
                Loggers.Debug(new DebugLogInfo() { Message = "调用获取jsapi_ticket接口返回值： " + data });

                jsApiTicket = data.DeserializeJSONTo<JsApiTicketEntity>();

                appObj.JsApiTicket = jsApiTicket.ticket;
                appObj.TicketExpirationTime = DateTime.Now.AddHours(1);
                wApplicationInterfaceBLL.Update(appObj, false);
            }
            else
            {
                jsApiTicket = new JsApiTicketEntity();
                jsApiTicket.ticket = appObj.JsApiTicket;
                jsApiTicket.expires_in = "7200";

                Loggers.Debug(new DebugLogInfo() { Message = "使用未过期的jsapi_ticket:" + appObj.JsApiTicket + ", 到期时间：" + appObj.TicketExpirationTime });
            }
            logInfo.LogId = BaseService.NewGuidPub();
            logInfo.VipId = appObj.AppID;
            logInfo.MarketEventId = appObj.AppSecret;
            logInfo.TemplateContent = appObj.JsApiTicket;
            logInfo.IsSuccess = 1;
            logInfo.SendTypeId = "2";
            logInfo.WeiXinUserId = appObj.WeiXinID;

            logServer.Create(logInfo);
            return jsApiTicket;
        }

        #endregion

        #region 创建自定义菜单

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="loggingSessionInfo"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public ResultEntity CreateMenu(LoggingSessionInfo loggingSessionInfo, string applicationId)
        {
            BaseService.WriteLogWeixin("创建自定义菜单");

            var appService = new WApplicationInterfaceBLL(loggingSessionInfo);
            var appEntity = appService.GetByID(applicationId);
            var result = new ResultEntity();

            //获取access_token
            var accessToken = this.GetAccessTokenByCache(appEntity.AppID, appEntity.AppSecret, loggingSessionInfo);

            if (accessToken.errcode == null || accessToken.errcode.Equals(string.Empty))
            {
                string content = string.Empty;
                string uri = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accessToken.access_token);
                string method = "POST";

                #region 动态生成菜单项

                var menuDAO = new WMenuDAO(loggingSessionInfo);
                var dsFirst = menuDAO.GetFirstMenus(appEntity.WeiXinID);

                MenusEntity menusEntity = new MenusEntity();
                menusEntity.button = new List<MenuEntity>();
                if (dsFirst != null && dsFirst.Tables.Count > 0 && dsFirst.Tables[0].Rows.Count > 0)
                {
                    var button = "{\"button\":[";
                    foreach (DataRow dr in dsFirst.Tables[0].Rows)
                    {
                        MenuEntity menu = new MenuEntity();
                        menu.type = dr["type"].ToString();
                        menu.name = dr["name"].ToString();
                        menu.key = dr["key"].ToString();
                        menu.url = dr["menuURL"].ToString();

                        button += "{";
                        button += "\"type\": \"" + dr["type"].ToString() + "\",";
                        button += "\"name\": \"" + dr["name"].ToString() + "\",";
                        button += "\"key\": \"" + dr["key"].ToString() + "\",";
                        button += "\"url\": \"" + dr["menuURL"].ToString() + "\",";
                        button += "\"sub_button\": ";

                        var dsSecond = menuDAO.GetSecondMenus(appEntity.WeiXinID, dr["ID"].ToString());
                        if (dsSecond != null && dsSecond.Tables.Count > 0 && dsSecond.Tables[0].Rows.Count > 0)
                        {
                            var subButton = "[";
                            foreach (DataRow drSecond in dsSecond.Tables[0].Rows)
                            {
                                subButton += "{";
                                subButton += "\"type\": \"" + drSecond["type"].ToString() + "\",";
                                subButton += "\"name\": \"" + drSecond["name"].ToString() + "\",";
                                subButton += "\"key\": \"" + drSecond["key"].ToString() + "\",";
                                subButton += "\"url\": \"" + drSecond["menuURL"].ToString() + "\"";
                                subButton += "},";
                            }

                            if (dsSecond.Tables[0].Rows.Count > 0)
                            {
                                subButton = subButton.Substring(0, subButton.Length - 1);
                            }

                            subButton += "]";
                            button += subButton;
                            menu.sub_button = subButton;
                        }
                        else
                        {
                            button += "[]";
                            menu.sub_button = "[]";
                        }

                        button += "},";

                        menusEntity.button.Add(menu);
                    }

                    if (dsFirst.Tables[0].Rows.Count > 0)
                    {
                        button = button.Substring(0, button.Length - 1);
                    }

                    button += "]}";

                    content = button;
                }

                #endregion

                BaseService.WriteLogWeixin("content：" + content);

                string data = GetRemoteData(uri, method, content);
                BaseService.WriteLogWeixin("创建自定义菜单返回值： " + data);
                result = data.DeserializeJSONTo<ResultEntity>();
            }

            return result;
        }

        #endregion

        #region 回复文本消息

        /// <summary>
        /// 回复文本消息
        /// </summary>
        /// <param name="weixinID">开发者微信号</param>
        /// <param name="openID">接收方帐号（收到的OpenID）</param>
        /// <param name="content">文本消息内容</param>
        public void ResponseTextMessage(string weixinID, string openID, string content, HttpContext httpContext, RequestParams requestParams)
        {
            var response = "<xml>";
            response += "<ToUserName><![CDATA[" + openID + "]]></ToUserName>";
            response += "<FromUserName><![CDATA[" + weixinID + "]]></FromUserName>";
            response += "<CreateTime>" + new BaseService().ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";
            response += "<MsgType><![CDATA[text]]></MsgType>";
            response += "<Content><![CDATA[" + content + "]]></Content> ";
            response += "<FuncFlag>0</FuncFlag>";
            response += "</xml>";
            BaseService.WriteLogWeixin("加密前:  " + response);
            //安全模式下加密
            response = WXEncryptMsg(requestParams, response);


            BaseService.WriteLogWeixin("回复文本消息结束-------------------------------------------\n");

            httpContext.Response.Write(response);
        }

        #endregion

        #region 回复图片消息

        /// <summary>
        /// 回复图片消息
        /// </summary>
        /// <param name="weixinID">开发者微信号</param>
        /// <param name="openID">接收方帐号（收到的OpenID）</param>
        /// <param name="content">文本消息内容</param>
        public void ResponseImageMessage(string weixinID, string openID, string mediaID, HttpContext httpContext, RequestParams requestParams)
        {
            var response = "<xml>";
            response += "<ToUserName><![CDATA[" + openID + "]]></ToUserName>";
            response += "<FromUserName><![CDATA[" + weixinID + "]]></FromUserName>";
            response += "<CreateTime>" + new BaseService().ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";
            response += "<MsgType><![CDATA[image]]></MsgType>";
            response += "<Image>";
            response += "<MediaId><![CDATA[" + mediaID + "]]></MediaId>";
            response += "</Image>";
            response += "<FuncFlag>0</FuncFlag>";
            response += "</xml>";

            BaseService.WriteLogWeixin("加密前:  " + response);
            //安全模式下加密
            response = WXEncryptMsg(requestParams, response);

            BaseService.WriteLogWeixin("回复图片消息结束-------------------------------------------\n");

            httpContext.Response.Write(response);
        }

        #endregion

        #region 回复图文消息

        /// <summary>
        /// 回复图文消息
        /// </summary>
        /// <param name="weixinID">开发者微信号</param>
        /// <param name="openID">接收方帐号（收到的OpenID）</param>
        /// <param name="newsList">图文消息实体类集合</param>
        public void ResponseNewsMessage(string weixinID, string openID, List<WMaterialTextEntity> newsList, HttpContext httpContext, RequestParams requestParams)
        {
            if (newsList != null && newsList.Count > 0)
            {
                var response = "<xml>";
                response += "<ToUserName><![CDATA[" + openID + "]]></ToUserName>";
                response += "<FromUserName><![CDATA[" + weixinID + "]]></FromUserName>";
                response += "<CreateTime>" + new BaseService().ConvertDateTimeInt(DateTime.Now) + "</CreateTime>";
                response += "<MsgType><![CDATA[news]]></MsgType>";
                response += "<ArticleCount>" + newsList.Count + "</ArticleCount>";
                response += "<Articles>";

                foreach (var item in newsList)
                {
                    response += "<item>";
                    response += "<Title><![CDATA[" + item.Title + "]]></Title> ";
                    response += "<Description><![CDATA[" + item.Text + "]]></Description>";
                    response += "<PicUrl><![CDATA[" + item.CoverImageUrl + "]]></PicUrl>";
                    response += "<Url><![CDATA[" + item.OriginalUrl + "]]></Url>";
                    response += "</item>";
                }

                response += "</Articles>";
                response += "<FuncFlag>1</FuncFlag>";
                response += "</xml>";

                BaseService.WriteLogWeixin("加密前:  " + response);
                //安全模式下加密
                response = WXEncryptMsg(requestParams, response);

                BaseService.WriteLogWeixin("回复图文消息结束-------------------------------------------\n");



                httpContext.Response.Write(response);
            }
        }

        #endregion

        #region 保存用户信息

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="openID">发送方帐号（一个OpenID）</param>
        /// <param name="weixinID">开发者微信号</param>
        /// <param name="isShow">1： 关注  0： 取消关注</param>
        public void SaveUserInfo(string openID, string weixinID, string isShow, string appId, string appSecret, string qrcodeId, LoggingSessionInfo loggingSessionInfo)
        {
            //获取调用微信接口的凭证
            var accessToken = GetAccessTokenByCache(appId, appSecret, loggingSessionInfo);

            if (accessToken.errcode == null || accessToken.errcode.Equals(string.Empty))
            {
                //通过openID获取用户信息
                var userInfo = GetUserInfo(accessToken.access_token, openID);//从微信服务器获取会员的信息***

                if (userInfo.errcode == null || userInfo.errcode.Equals(string.Empty))
                {
                    //记录用户信息
                    BaseService.WriteLogWeixin("userInfo.subscribe:  " + userInfo.subscribe);//用户是否订阅该公众号标识，值为0时，拉取不到其余信息,
                    BaseService.WriteLogWeixin("userInfo.openid:  " + userInfo.openid);
                    BaseService.WriteLogWeixin("userInfo.nickname:  " + userInfo.nickname);
                    BaseService.WriteLogWeixin("userInfo.sex:  " + userInfo.sex);
                    BaseService.WriteLogWeixin("userInfo.city:  " + userInfo.city);
                    BaseService.WriteLogWeixin("userInfo.language:  " + userInfo.language);
                    BaseService.WriteLogWeixin("userInfo.headimgurl:  " + userInfo.headimgurl);
                    BaseService.WriteLogWeixin("userInfo.unionid:  " + userInfo.unionid);

                    string webUrl = ConfigurationManager.AppSettings["website_WWW"];
                    var qrcode = webUrl + "/Member.aspx?weixin_id=" + weixinID + "&open_id=" + openID;

                    string uri = webUrl + "/weixin/data.aspx?datatype=SignIn";//调用用户关注事件
                    uri += "&openID=" + HttpUtility.UrlEncode(openID);
                    uri += "&weixin_id=" + HttpUtility.UrlEncode(weixinID);
                    uri += "&gender=" + (string.IsNullOrEmpty(userInfo.sex) ? "0" : HttpUtility.UrlEncode(userInfo.sex));
                    uri += "&city=" + (string.IsNullOrEmpty(userInfo.city) ? "0" : HttpUtility.UrlEncode(userInfo.city));
                    uri += "&vipName=" + (string.IsNullOrEmpty(userInfo.nickname) ? "0" : HttpUtility.UrlEncode(userInfo.nickname));
                    uri += "&headimgurl=" + HttpUtility.UrlEncode(userInfo.headimgurl);
                    uri += "&isShow=" + HttpUtility.UrlEncode(isShow);
                    uri += "&qrcode=" + HttpUtility.UrlEncode(qrcode);//
                    uri += "&qrcode_id=" + HttpUtility.UrlEncode(qrcodeId);
                    uri += "&unionid=" + HttpUtility.UrlEncode(userInfo.unionid);
                    //  uri += "&unionid=" + HttpUtility.UrlEncode(request);


                    string method = "GET";
                    //这里返还的data不会返回给微信服务器的，只是用在了我们自己的服务器上
                    string data = CommonBLL.GetRemoteData(uri, method, string.Empty);

                    BaseService.WriteLogWeixin("uri:  " + uri);
                    BaseService.WriteLogWeixin("调用保存用户信息接口返回值:  " + data);
                }
                else
                {
                    BaseService.WriteLogWeixin("userInfo.errcode:  " + userInfo.errcode);
                    BaseService.WriteLogWeixin("userInfo.errmsg:  " + userInfo.errmsg);
                }
            }
            else
            {
                BaseService.WriteLogWeixin("accessToken.errcode:  " + accessToken.errcode);
                BaseService.WriteLogWeixin("accessToken.errmsg:  " + accessToken.errmsg);
            }
        }

        #endregion

        #region 通过工作平台给会员推送文本消息并保存日志，仅限48小时内有互动的会员
        //add by Willie Yan 2014-01-09
        public static string SendWeixinMessage(string message, string fromVipId, LoggingSessionInfo loggingSessionInfo, VipEntity vip)
        {
            string code = "";
            JIT.CPOS.BS.BLL.WX.CommonBLL commonService;
            WUserMessageBLL wUserMessageBLL;
            WUserMessageEntity queryObj;

            Loggers.Debug(new DebugLogInfo() { Message = "loggingSessionInfo: " + loggingSessionInfo.ToJSON() });

            //保存消息日志
            commonService = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            wUserMessageBLL = new WUserMessageBLL(loggingSessionInfo);

            queryObj = new WUserMessageEntity();//先保存到WUserMessage
            queryObj.MessageId = JIT.CPOS.Common.Utils.NewGuid();
            queryObj.VipId = vip.VIPID;
            queryObj.Text = message;
            queryObj.OpenId = vip.WeiXinUserId;
            queryObj.DataFrom = 2;
            queryObj.CreateTime = DateTime.Now;
            queryObj.CreateBy = fromVipId;
            queryObj.LastUpdateBy = fromVipId;
            queryObj.LastUpdateTime = DateTime.Now;
            queryObj.MaterialTypeId = "1";
            queryObj.IsDelete = 0;

            wUserMessageBLL.Create(queryObj);

            string appID = "";
            string appSecret = "";
            //获取appid, appsecret
            WApplicationInterfaceBLL waServer = new WApplicationInterfaceBLL(loggingSessionInfo);
            var waObj = waServer.QueryByEntity(new WApplicationInterfaceEntity
            {
                WeiXinID = vip.WeiXin
            }, null);
            if (waObj == null || waObj.Length == 0 || waObj[0] == null)
            {
                code = "103";
            }
            else
            {
                appID = waObj[0].AppID;
                appSecret = waObj[0].AppSecret;

                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("appID:{0}, appSecret, {1}", appID, appSecret)
                });

                //发送消息
                SendMessageEntity sendInfo = new SendMessageEntity();
                sendInfo.msgtype = "text";
                sendInfo.touser = vip.WeiXinUserId;
                //sendInfo.articles = newsList;
                sendInfo.content = message;

                JIT.CPOS.BS.Entity.WX.ResultEntity msgResultObj = new JIT.CPOS.BS.Entity.WX.ResultEntity();
                msgResultObj = commonService.SendMessage(sendInfo, appID, appSecret, loggingSessionInfo);

                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("PushMsgResult:{0}", msgResultObj)
                });

                //更新消息日志
                if (msgResultObj != null)
                {
                    queryObj.IsPushWX = 1;
                    queryObj.PushWXTime = DateTime.Now;
                    if (msgResultObj.errcode == "0")
                    {
                        queryObj.IsPushSuccess = 1;
                    }
                    else
                    {
                        queryObj.IsPushSuccess = 0;
                        queryObj.FailureReason = msgResultObj.ToJSON();
                    }
                    wUserMessageBLL.Update(queryObj, false);//保存到wUserMessage

                    code = "200";
                }
                else
                {
                    code = "203";
                }
            }

            return code;
        }
        #endregion

        #region 高级账号功能

        #region 获取用户信息接口

        /// <summary>
        /// 第三方通过openid获取用户信息
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="openID">普通用户的标识，对当前公众号唯一</param>
        /// <returns></returns>
        public UserInfoEntity GetUserInfo(string accessToken, string openID)
        {
            string uri = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + accessToken + "&openid=" + openID;
            string method = "GET";
            string data = GetRemoteData(uri, method, string.Empty);

            var userInfo = data.DeserializeJSONTo<UserInfoEntity>();
            return userInfo;
        }

        #endregion

        #region 主动推送消息接口

        /// <summary>
        /// 主动推送消息
        /// </summary>
        /// <param name="sendMessage">发送消息实体</param>
        /// <param name="appID"></param>
        /// <param name="appSecret"></param>
        /// <param name="isCustomMsg">
        /// 是否为客服消息  true：发送客服消息(默认值)  false：发送不受限制的消息
        /// </param>
        /// <returns></returns>
        public ResultEntity SendMessage(SendMessageEntity sendMessage, string appID, string appSecret, LoggingSessionInfo loggingSessionInfo, bool isCustomMsg = true)
        {
            var result = new ResultEntity();
            var accessToken = this.GetAccessTokenByCache(appID, appSecret, loggingSessionInfo);

            if (accessToken.errcode == null || accessToken.errcode.Equals(string.Empty))
            {
                string uri = string.Empty;
                string method = "POST";
                string content = string.Empty;
                if (isCustomMsg)
                {
                    uri = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + accessToken.access_token;
                }
                else
                {
                    uri = "https://api.weixin.qq.com/cgi-bin/message/send?access_token=" + accessToken.access_token;
                }

                switch (sendMessage.msgtype.ToLower())
                {
                    case "image":
                        content = "{\"touser\":\"" + sendMessage.touser + "\",\"msgtype\":\"image\",\"image\":{\"mediaid\":\"" + sendMessage.media_id + "\"}}";
                        break;    //图片消息
                    case "voice": break;    //语音信息
                    case "video": break;    //视频信息
                    case "music": break;    //音乐信息
                    case "news":    //图文信息
                        content = "{\"touser\":\"" + sendMessage.touser + "\",\"msgtype\":\"news\",\"news\":{\"articles\": [";
                        foreach (var news in sendMessage.articles)
                        {
                            content += "{";
                            content += "\"title\": \"" + news.title + "\",";
                            content += "\"description\": \"" + news.description + "\",";
                            content += "\"url\": \"" + news.url + "\",";
                            content += "\"picurl\": \"" + news.picurl + "\"";
                            content += "},";
                        }
                        content += "]}}";
                        break;
                    default:    //默认发送文本消息
                        content = "{\"touser\":\"" + sendMessage.touser + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + sendMessage.content + "\"}}";
                        break;
                }

                string data = GetRemoteData(uri, method, content);
                result = data.DeserializeJSONTo<ResultEntity>();

                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("中欧扫描--关注SendMessage 进入:data:{0},content {1}", data, content)
                });
            }

            return result;
        }

        #endregion

        #region 主动推送模板消息接口
        /// <summary>
        /// 主动推送模板消息接口
        /// </summary>
        /// <param name="sendMessage">发送消息实体</param>
        /// <param name="accessToken">调用微信公众平台接口的凭证</param>
        /// <returns></returns>
        public string SendTemplateMessage(string weixinID, string message)
        {
            string data = string.Empty;
            LoggingSessionInfo loggingSessionInfo = BaseService.GetWeixinLoggingSession(weixinID);

            var appService = new WApplicationInterfaceBLL(loggingSessionInfo);
            var appList = appService.QueryByEntity(new WApplicationInterfaceEntity { WeiXinID = weixinID }, null);

            if (appList != null && appList.Length > 0)
            {
                var appEntity = appList.FirstOrDefault();
                //获取access_token
                var accessToken = this.GetAccessTokenByCache(appEntity.AppID, appEntity.AppSecret, loggingSessionInfo);
                string uri = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + accessToken.access_token;
                string method = "POST";
                string content = message;
                Loggers.Debug(new DebugLogInfo() { Message = string.Format("发送模板消息使用的token:{0}", accessToken.access_token) });
                data = GetRemoteData(uri, method, content);
            }

            return data;
        }

        #endregion

        #region 媒体文件上传接口

        /// <summary>
        /// 媒体文件上传接口（临时素材）
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="mediaUri">媒体文件的 URI</param>
        /// <param name="mediaType">媒体文件类型</param>
        /// <returns>上传成功后，返回mediaID</returns>
        public UploadMediaEntity UploadMediaFile(string accessToken, string mediaUri, MediaType mediaType)
        {
            BaseService.WriteLogWeixin("开始调用媒体文件上传接口UploadMediaFile");

            //下载媒体文件
            string filePath = DownloadFile(mediaUri);

            if (!string.IsNullOrEmpty(filePath))
            {
                //上传图片
                string uriString = "http://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + accessToken + "&type=" + mediaType.ToString().ToLower();

                //创建一个新的 WebClient 实例.
                WebClient myWebClient = new WebClient();

                //直接上传，并获取返回的二进制数据
                byte[] responseArray = myWebClient.UploadFile(uriString, "POST", filePath);
                var data = System.Text.Encoding.Default.GetString(responseArray);
                BaseService.WriteLogWeixin("调用微信平台媒体上传接口返回值： " + data);

                var media = data.DeserializeJSONTo<UploadMediaEntity>();
                //删除临时文件
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return media;
            }
            else
            {
                BaseService.WriteLogWeixin("下载失败，请检查媒体文件的URI是否正确");
                return new UploadMediaEntity() { errcode = "400", errmsg = "下载失败，请检查媒体文件的URI是否正确" };
            }
        }
        //再加一个永久素材的
        /// <summary>
        /// 媒体文件上传接口（永久素材）（图文素材除外）
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="mediaUri">媒体文件的 URI</param>
        /// <param name="mediaType">媒体文件类型</param>
        /// <returns>上传成功后，返回mediaID</returns>
        public UploadMediaEntity UploadMediaFileFOREVER(string accessToken, string mediaUri, MediaType mediaType)
        {
            BaseService.WriteLogWeixin("开始调用媒体文件上传接口UploadMediaFile");

            //下载媒体文件
            string filePath = DownloadFile(mediaUri);

            if (!string.IsNullOrEmpty(filePath))
            {
                //上传图片
                string uriString = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token=" + accessToken;
                FileStream TextFile = File.Open(filePath, FileMode.Append); ;
                long fileLength = TextFile.Length;
                //创建一个新的 WebClient 实例.
                WebClient myWebClient = new WebClient();
                // string Json = "{\"media\":{\"filename\":\"" + mediaUri + "\",\"content-type\":\"" + mediaType.ToString().ToLower() + "\",\"filelength\":\"" + fileLength + "\"  }}";
                //直接上传，并获取返回的二进制数据
                byte[] responseArray = myWebClient.UploadFile(uriString, "POST", filePath);
                //   string data = GetRemoteData(uriString, "POST", Json);
                var data = System.Text.Encoding.Default.GetString(responseArray);
                BaseService.WriteLogWeixin("调用微信平台媒体上传接口返回值： " + data);

                var media = data.DeserializeJSONTo<UploadMediaEntity>();
                return media;
            }
            else
            {
                BaseService.WriteLogWeixin("下载失败，请检查媒体文件的URI是否正确");
                return new UploadMediaEntity() { errcode = "400", errmsg = "下载失败，请检查媒体文件的URI是否正确" };
            }
        }
        /// <summary>
        /// 将具有指定 URI 的资源下载到本地文件。
        /// </summary>
        /// <param name="address">从中下载数据的 URI。</param>
        /// <returns>本地文件保存路径</returns>
        public string DownloadFile(string address)
        {
            BaseService.WriteLogWeixin("将具有指定 URI 的资源下载到本地文件");
            BaseService.WriteLogWeixin("文件URI： " + address);

            try
            {
                WebClient webClient = new WebClient();

                //创建下载根文件夹
                var dirPath = @"C:\DownloadFile\";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                //根据年月日创建下载子文件夹
                var ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                dirPath += ymd + @"\";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                //下载到本地文件
                var fileExt = Path.GetExtension(address).ToLower();
                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
                var filePath = dirPath + newFileName;

                webClient.DownloadFile(address, filePath);

                BaseService.WriteLogWeixin("文件下载成功！");
                BaseService.WriteLogWeixin("文件保存路径： " + filePath);

                return filePath;//以@开头
            }
            catch (Exception ex)
            {
                BaseService.WriteLogWeixin("图片下载异常信息：  " + ex.Message);
                return string.Empty;
            }
        }

        #endregion

        #region 获取二维码图片地址

        /// <summary>
        /// 获取二维码图片地址
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="type">二维码类型  0： 临时二维码  1：永久二维码</param>
        /// <param name="sceneId">场景值ID，临时二维码时为32位整型，永久二维码时只支持1--100000</param>
        /// <returns></returns>
        public string GetQrcodeUrl(string appId, string appSecret, string type, int sceneId, LoggingSessionInfo loggingSessionInfo)
        {
            BaseService.WriteLogWeixin("获取二维码图片地址");
            var qrcodeUrl = string.Empty;

            //获取access_token
            var accessToken = this.GetAccessTokenByCache(appId, appSecret, loggingSessionInfo);

            if (accessToken.errcode == null || accessToken.errcode.Equals(string.Empty))
            {
                string uri = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accessToken.access_token;//通过token创建二维码
                string method = "POST";
                string content = string.Empty;

                if (type.Equals("0"))
                {
                    content = "{\"expire_seconds\": 1800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sceneId + "}}}";//这个二维码就用半小时的？！！
                }
                else if (type.Equals("1"))
                {
                    content = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sceneId + "}}}";
                }

                string data = GetRemoteData(uri, method, content);

                if (data.IndexOf("40001") > -1 && data.ToLower().IndexOf("invalid credential") > -1)
                    Loggers.Debug(new DebugLogInfo() { Message = "获取二维码失败，40001:invalid credential, accessToken.access_token=" + accessToken.access_token });

                var qrcode = data.DeserializeJSONTo<QrCodeEntity>();//主要获取ticket的

                #region 处理第三方使用token，没有更改过期时间问题
                if (qrcode.errcode == "40001")
                {
                    var wAppInteBLL = new WApplicationInterfaceBLL(loggingSessionInfo);
                    var wAppInteInfo = wAppInteBLL.QueryByEntity(new WApplicationInterfaceEntity() { CustomerId = loggingSessionInfo.ClientID }, null).FirstOrDefault();
                    if (wAppInteInfo != null)
                    {
                        //修改过期时间
                        wAppInteInfo.ExpirationTime = DateTime.Now.AddSeconds(-10);
                        wAppInteBLL.Update(wAppInteInfo);
                        //重新调用接口
                        data = GetRemoteData(uri, method, content);
                        qrcode = data.DeserializeJSONTo<QrCodeEntity>();
                    }
                }
                if (qrcode.errcode == "48001")
                {
                    qrcode.errmsg = "api功能未授权，请确认公众号已获得该接口，可以在公众平台官网-开发者中心页中查看接口权限";
                }
                #endregion

                BaseService.WriteLogWeixin("获取二维码图片返回值：" + data);

                if (string.IsNullOrEmpty(qrcode.errcode) || qrcode.errcode == "0")//	0代表请求成功
                {
                    qrcodeUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + qrcode.ticket;//通过ticket获取二维码地址
                }
                else
                {
                    throw new Exception("生成二维码失败，" + qrcode.errcode + ":" + qrcode.errmsg);
                }
            }

            BaseService.WriteLogWeixin("qrcodeUrl：" + qrcodeUrl);

            return qrcodeUrl;
        }


        /// <summary>
        /// 长链接转短链接接口
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="long_url">	需要转换的长链接</param>     
        /// <returns></returns>
        public string GetShorturl(string appId, string appSecret, string long_url, LoggingSessionInfo loggingSessionInfo)
        {
            BaseService.WriteLogWeixin("长链接转短链接接口");
            var shorturl = string.Empty;

            //获取access_token
            var accessToken = this.GetAccessTokenByCache(appId, appSecret, loggingSessionInfo);

            if (accessToken.errcode == null || accessToken.errcode.Equals(string.Empty))
            {
                string uri = "https://api.weixin.qq.com/cgi-bin/shorturl?access_token=" + accessToken.access_token;//通过token创建二维码
                string method = "POST";
                string content = string.Empty;

                content = "{\"action\": \"long2short\", \"long_url\":\"" + long_url + "\"}";        //此处填long2short，代表长链接转短链接     



                string data = GetRemoteData(uri, method, content);

                if (data.IndexOf("40001") > -1 && data.ToLower().IndexOf("invalid credential") > -1)
                    Loggers.Debug(new DebugLogInfo() { Message = "长链接转短链接失败，40001:invalid credential, accessToken.access_token=" + accessToken.access_token });

                var shorturlEn = data.DeserializeJSONTo<ShorturlEntity>();//主要获取ticket的

                #region 处理第三方使用token，没有更改过期时间问题
                if (shorturlEn.errcode == "40001")
                {
                    var wAppInteBLL = new WApplicationInterfaceBLL(loggingSessionInfo);
                    var wAppInteInfo = wAppInteBLL.QueryByEntity(new WApplicationInterfaceEntity() { CustomerId = loggingSessionInfo.ClientID }, null).FirstOrDefault();
                    if (wAppInteInfo != null)
                    {
                        //修改过期时间
                        wAppInteInfo.ExpirationTime = DateTime.Now.AddSeconds(-10);//设置当前的token为过期
                        wAppInteBLL.Update(wAppInteInfo);
                        //重新调用接口
                        data = GetRemoteData(uri, method, content);
                        shorturlEn = data.DeserializeJSONTo<ShorturlEntity>();
                    }
                }

                #endregion

                BaseService.WriteLogWeixin("长链接转短链接返回值：" + data);

                if (!string.IsNullOrEmpty(shorturlEn.errcode) && shorturlEn.errcode != "0")
                {
                    throw new Exception("长链接转短链接失败，" + shorturlEn.errcode + ":" + shorturlEn.errmsg);
                }

                shorturl = shorturlEn.short_url;

            }

            BaseService.WriteLogWeixin("shorturl：" + shorturl);

            return shorturl;
        }





        #endregion

        #region 导入微信公众账号用户信息

        /// <summary>
        /// 导入微信公众账号用户信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="weixinId"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <param name="vipCode">当前VIP表vipCode的最大值</param>
        /// <returns></returns>
        public bool ImportUserInfo(string appId, string appSecret, string weixinId, LoggingSessionInfo loggingSessionInfo, int vipCode, int appCount)
        {
            try
            {
                BaseService.WriteLogWeixin("导入微信公众账号用户信息:  ImportUserInfo()");
                var accessToken = GetAccessTokenByCache(appId, appSecret, loggingSessionInfo);
                string uri = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + accessToken.access_token;
                string method = "GET";
                string data = GetRemoteData(uri, method, string.Empty);
                BaseService.WriteLogWeixin("data:  " + data);

                var entity = data.DeserializeJSONTo<GetUserInfoEntity>();

                if (entity != null)
                {
                    //获取所有用户openId集合
                    if (entity.count == "10000")
                    {
                        //数量大于1万时，递归调用
                        AddUserInfo(entity, accessToken.access_token);
                    }
                    else
                    {
                        userList.Add(entity);
                    }

                    BaseService.WriteLogWeixin("请求微信，获取用户信息:  ");
                    DateTime startTime = DateTime.Now;
                    BaseService.WriteLogWeixin("开始时间:  " + startTime);

                    List<UserInfoEntity> userInfoList = new List<UserInfoEntity>();
                    //通过openId获取用户信息
                    if (userList.Count > 0)
                    {
                        UserInfoEntity userInfo = new UserInfoEntity();
                        foreach (var user in userList)
                        {
                            if (user.data != null)
                            {
                                foreach (var id in user.data.openid)
                                {
                                    uri = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + accessToken.access_token + "&openid=" + id;
                                    method = "GET";
                                    data = GetRemoteData(uri, method, string.Empty);

                                    userInfo = data.DeserializeJSONTo<UserInfoEntity>();

                                    userInfoList.Add(userInfo);
                                }
                            }
                        }
                    }

                    DateTime endTime = DateTime.Now;
                    BaseService.WriteLogWeixin("结束时间:  " + endTime);
                    BaseService.WriteLogWeixin("总共耗时:  " + (endTime - startTime));

                    //将用户信息写入本地数据库
                    BaseService.WriteLogWeixin("将用户信息写入本地数据库");
                    startTime = DateTime.Now;
                    BaseService.WriteLogWeixin("开始时间:  " + startTime);

                    string sql = string.Empty;

                    int count = vipCode;
                    string code = "Vip";
                    switch (vipCode.ToString().Length)
                    {
                        case 1: code += "0000000"; break;
                        case 2: code += "000000"; break;
                        case 3: code += "00000"; break;
                        case 4: code += "0000"; break;
                        case 5: code += "000"; break;
                        case 6: code += "00"; break;
                        case 7: code += "0"; break;
                    }

                    var vipBLL = new VipBLL(loggingSessionInfo);
                    var wxUserInfoBLL = new WXUserInfoBLL(loggingSessionInfo);
                    foreach (var item in userInfoList)
                    {
                        var wxUserInfo = wxUserInfoBLL.QueryByEntity(new WXUserInfoEntity() { CustomerID = loggingSessionInfo.ClientID, WeiXinUserID = item.openid }, null).FirstOrDefault();
                        VipEntity vipInfo = null;
                        if (wxUserInfo != null)
                            vipInfo = vipBLL.GetByID(wxUserInfo.VipID);
                        else
                            vipInfo = vipBLL.QueryByEntity(new VipEntity() { ClientID = loggingSessionInfo.ClientID, WeiXinUserId = item.openid }, null).FirstOrDefault();

                        if (vipInfo == null)
                        {
                            //新增会员信息
                            var nickname = item.nickname.Replace("'", "");
                            var city = item.country + " " + item.province + " " + item.city;
                            var tempCode = code + count;
                            count++;

                            sql += "   DECLARE @VipID" + tempCode + " varchar(50) SET @VipID" + tempCode + "=REPLACE(NEWID(),'-','') ";
                            sql += " INSERT INTO dbo.Vip( ";
                            sql += " VIPID ,VipName ,VipLevel ,VipCode , ";
                            sql += " WeiXin ,WeiXinUserId ,Gender ,Status , ";
                            sql += " VipSourceId ,ClientID ,CreateTime ,CreateBy , ";
                            sql += " LastUpdateTime ,LastUpdateBy ,IsDelete ,City ,HeadImgUrl,UnionID) ";
                            sql += " VALUES  ( ";
                            sql += " @VipID" + tempCode + ", '" + nickname + "', 1, '" + tempCode + "', ";
                            sql += " '" + weixinId + "','" + item.openid + "','" + item.sex + "','1', ";
                            sql += " '3','" + loggingSessionInfo.ClientID + "' ,GETDATE(), '1', ";
                            sql += " GETDATE(), '1', 0, '" + city + "','" + item.headimgurl + "','" + item.unionid + "' ";
                            sql += " ) ";

                            if (appCount > 1)
                            {
                                sql += " insert into wxuserinfo ";
                                sql += " (WXUserID,VipID,WeiXin,WeiXinUserID,UnionID,CustomerID,CreateTime,CreateBy,LastUpdateTime,LastUpdateBy,IsDelete)  ";
                                sql += " values ( ";
                                sql += " newid(),@vipid" + tempCode + ",'" + weixinId + "','" + item.openid + "','" + item.unionid + "','" + loggingSessionInfo.ClientID + "',getdate(),'sys',getdate(),'sys',0";
                                sql += " ) ";
                            }
                        }
                        else
                        {
                            //修改会员信息
                            var nickname = item.nickname.Replace("'", "");
                            var city = item.country + " " + item.province + " " + item.city;

                            sql += " UPDATE Vip SET VipName='" + nickname + "',City='" + city + "',HeadImgUrl='" + item.headimgurl + "',UnionID='" + item.unionid + "',LastUpdateTime = GetDate() WHERE vipid='" + vipInfo.VIPID + "' ";

                            if (appCount > 1)
                            {
                                if (wxUserInfo == null)
                                {
                                    sql += " insert into wxuserinfo ";
                                    sql += " (WXUserID,VipID,WeiXin,WeiXinUserID,UnionID,CustomerID,CreateTime,CreateBy,LastUpdateTime,LastUpdateBy,IsDelete)  ";
                                    sql += " values ( ";
                                    sql += " newid(),'" + vipInfo.VIPID + "','" + weixinId + "','" + item.openid + "','" + item.unionid + "','" + loggingSessionInfo.ClientID + "',getdate(),'sys',getdate(),'sys',0";
                                    sql += " ) ";
                                }
                            }
                        }
                    }

                    DefaultSQLHelper sqlHelper = new DefaultSQLHelper(loggingSessionInfo.CurrentLoggingManager.Connection_String);

                    BaseService.WriteLogWeixin("sql:  " + sql);
                    var result = sqlHelper.ExecuteScalar(sql);
                    BaseService.WriteLogWeixin("result:  " + result);

                    endTime = DateTime.Now;
                    BaseService.WriteLogWeixin("结束时间:  " + endTime);
                    BaseService.WriteLogWeixin("总共耗时:  " + (endTime - startTime));

                    userList.RemoveAt(0);
                    var tmp = string.Empty;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        List<GetUserInfoEntity> userList = new List<GetUserInfoEntity>();
        private void AddUserInfo(GetUserInfoEntity entity, string accessToken)
        {
            if (entity != null)
            {
                if (entity.count == "10000")
                {
                    var uri = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + accessToken + "&next_openid=" + entity.next_openid;
                    var method = "GET";
                    var data = GetRemoteData(uri, method, string.Empty);
                    BaseService.WriteLogWeixin("data:  " + data);

                    var tmpEntity = data.DeserializeJSONTo<GetUserInfoEntity>();

                    //递归调用
                    AddUserInfo(tmpEntity, accessToken);

                    userList.Add(entity);
                }
                else
                {
                    userList.Add(entity);
                }
            }
        }

        /// <summary>
        /// 用户集合
        /// </summary>
        private class GetUserInfoEntity
        {
            /// <summary>
            /// 关注该公众账号的总用户数
            /// </summary>
            public string total { get; set; }
            /// <summary>
            /// 拉取的OPENID个数，最大值为10000
            /// </summary>
            public string count { get; set; }
            /// <summary>
            /// 列表数据，OPENID的列表
            /// </summary>
            public UserEntity data { get; set; }
            /// <summary>
            /// 拉取列表的后一个用户的OPENID
            /// </summary>
            public string next_openid { get; set; }
        }

        private class UserEntity
        {
            public List<string> openid { get; set; }
        }

        #endregion


        #region New导入微信公众号用户信息
        public bool NewImportUserInfo(string appId, string appSecret, string weixinId, LoggingSessionInfo loggingSessionInfo, int vipCode)
        {

            try
            {
                BaseService.WriteLogWeixin("导入微信公众账号用户信息:  ImportUserInfo()");

                var accessToken = GetAccessTokenByCache(appId, appSecret, loggingSessionInfo);
                string uri = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + accessToken.access_token;
                string method = "GET";
                string data = GetRemoteData(uri, method, string.Empty);
                BaseService.WriteLogWeixin("data:  " + data);

                var entity = data.DeserializeJSONTo<GetUserInfoEntity>();

                if (entity != null)
                {
                    //获取所有用户openId集合
                    if (entity.count == "10000")
                    {
                        //数量大于1万时，递归调用
                        NewAddUserInfo(entity, accessToken.access_token);
                    }
                    else
                    {
                        newuserList.Add(entity);
                    }

                    BaseService.WriteLogWeixin("请求微信，获取用户信息:  ");
                    DateTime startTime = DateTime.Now;
                    BaseService.WriteLogWeixin("开始时间:  " + startTime);

                    List<UserInfoEntity> userInfoList = new List<UserInfoEntity>();
                    //通过openId获取用户信息
                    if (newuserList.Count > 0)
                    {
                        UserInfoEntity userInfo = new UserInfoEntity();
                        foreach (var user in newuserList)
                        {
                            if (user.data != null)
                            {
                                foreach (var id in user.data.openid)
                                {
                                    uri = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + accessToken.access_token + "&openid=" + id;
                                    method = "GET";
                                    data = GetRemoteData(uri, method, string.Empty);
                                    userInfo = data.DeserializeJSONTo<UserInfoEntity>();

                                    userInfoList.Add(userInfo);
                                }
                            }
                        }
                    }

                    DateTime endTime = DateTime.Now;
                    BaseService.WriteLogWeixin("结束时间:  " + endTime);
                    BaseService.WriteLogWeixin("总共耗时:  " + (endTime - startTime));

                    //将用户信息写入本地数据库
                    BaseService.WriteLogWeixin("将用户信息写入本地数据库");
                    startTime = DateTime.Now;
                    BaseService.WriteLogWeixin("开始时间:  " + startTime);

                    string sql = string.Empty;

                    int count = vipCode;
                    string code = "Vip";
                    //switch (vipCode.ToString().Length)
                    //{
                    //    case 1: code += "0000000"; break;
                    //    case 2: code += "000000"; break;
                    //    case 3: code += "00000"; break;
                    //    case 4: code += "0000"; break;
                    //    case 5: code += "000"; break;
                    //    case 6: code += "00"; break;
                    //    case 7: code += "0"; break;
                    //}
                    bool bl = Replace(loggingSessionInfo, userInfoList, code, vipCode, weixinId);

                    // BaseService.WriteLogWeixin("result:  " + result);
                    //   DataTable dt = ConvertListToDatatTable(userInfoList);

                    //  InsertDataBase(loggingSessionInfo, dt);

                    //foreach (var item in userInfoList)
                    //{
                    //    var nickname = item.nickname.Replace("'", "''");
                    //    var city = item.country + " " + item.province + " " + item.city;
                    //    var tempCode = code + count;
                    //    count++;

                    //    sql += " INSERT INTO dbo.Vip( ";
                    //    sql += " VIPID ,VipName ,VipLevel ,VipCode , ";
                    //    sql += " WeiXin ,WeiXinUserId ,Gender ,Status , ";
                    //    sql += " VipSourceId ,ClientID ,CreateTime ,CreateBy , ";
                    //    sql += " LastUpdateTime ,LastUpdateBy ,IsDelete ,City ,HeadImgUrl) ";
                    //    sql += " VALUES  ( ";
                    //    sql += " REPLACE(NEWID(),'-','') , '" + item.nickname + "', 1, '" + tempCode + "', ";
                    //    sql += " '" + weixinId + "','" + item.openid + "','" + item.sex + "','1', ";
                    //    sql += " '3','86a575e616044da3ac2c3ab492e44445', GETDATE(), '1', ";
                    //    sql += " GETDATE(), '1', 0, '" + city + "','" + item.headimgurl + "' ";
                    //    sql += " ) ";
                    //    DefaultSQLHelper sqlHelper = new DefaultSQLHelper(loggingSessionInfo.Conn);
                    //    var result = sqlHelper.ExecuteScalar(sql);
                    //}

                    //
                    //BaseService.WriteLogWeixin("sql:  " + sql);
                    //
                    endTime = DateTime.Now;
                    BaseService.WriteLogWeixin("结束时间:  " + endTime);
                    BaseService.WriteLogWeixin("总共耗时:  " + (endTime - startTime));

                    var tmp = string.Empty;
                }

                return true;
            }
            catch
            {
                return false;
            }



        }
        List<GetUserInfoEntity> newuserList = new List<GetUserInfoEntity>();
        private void NewAddUserInfo(GetUserInfoEntity entity, string accessToken)
        {
            if (entity != null)
            {
                if (entity.count == "10000")
                {
                    var uri = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + accessToken + "&next_openid=" + entity.next_openid;
                    var method = "GET";
                    var data = GetRemoteData(uri, method, string.Empty);
                    BaseService.WriteLogWeixin("data:  " + data);

                    var tmpEntity = data.DeserializeJSONTo<GetUserInfoEntity>();

                    //递归调用
                    NewAddUserInfo(tmpEntity, accessToken);

                    newuserList.Add(entity);
                }
                else
                {
                    newuserList.Add(entity);
                }
            }
        }

        #region 处理数据
        private static bool Replace(LoggingSessionInfo loggingSessionInfo, List<UserInfoEntity> data, string Code, int vipCode, string weixinId)
        {
            VipBLL bll = new VipBLL(loggingSessionInfo);
            try
            {
                List<UserInfoEntity> list = data;
                string BatNo = Guid.NewGuid().ToString().Replace("-", "");
                CPOS.Common.DownloadImage downloadServer = new JIT.CPOS.Common.DownloadImage();
                foreach (UserInfoEntity item in list)
                {
                    var tempCode = Code + vipCode;
                    vipCode++;
                    item.BatNo = BatNo;
                    item.VipCode = tempCode;
                    item.WeXin = weixinId;

                    string downloadImageUrl = ConfigurationManager.AppSettings["website_WWW"];

                    if (!string.IsNullOrEmpty(item.headimgurl))
                    {
                        item.headimgurl = downloadServer.DownloadFile(item.headimgurl, downloadImageUrl);   //处理图片

                    }
                    item.IsDelete = "0";
                    item.nickname = ReplaceStr(item.nickname);
                    item.VipId = Guid.NewGuid().ToString().Replace("-", "");
                    item.CustomerId = loggingSessionInfo.ClientID;
                    // item.pa
                    bll.AddVipWXDownload(item);
                }
                int result = bll.WXToVip(BatNo);
                BaseService.WriteLogWeixin("导入数据记录: " + result);
                return true;
            }
            catch (Exception ex)
            {
                BaseService.WriteLogWeixin("导入失败");
                return false;
            }
        }
        #endregion

        #region 将微信信息导入数据库
        private static void InsertDataBase(LoggingSessionInfo loggingSessionInfo, DataTable dt)
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(loggingSessionInfo.Conn))
            {
                conn.Open();
                System.Data.SqlClient.SqlTransaction sqlbulkTransaction = conn.BeginTransaction();
                using (System.Data.SqlClient.SqlBulkCopy copy = new System.Data.SqlClient.SqlBulkCopy(conn, System.Data.SqlClient.SqlBulkCopyOptions.CheckConstraints, sqlbulkTransaction))
                {
                    copy.DestinationTableName = "VipWXDownload";
                    copy.BulkCopyTimeout = 100;
                    copy.ColumnMappings.Add("VipId", "VipId");
                    copy.ColumnMappings.Add("VipCode", "VipCode");
                    copy.ColumnMappings.Add("nickname", "VipCode");
                    copy.ColumnMappings.Add("openid", "OpenId");
                    copy.ColumnMappings.Add("openid", "WeiXin");
                    copy.ColumnMappings.Add("sex", "Gender");
                    copy.ColumnMappings.Add("city", "City");
                    //  copy.ColumnMappings.Add("city", "Age");
                    copy.ColumnMappings.Add("BatNo", "BatNo");
                    copy.ColumnMappings.Add("headimgurl", "HeadImgUrl");
                    copy.ColumnMappings.Add("CustomerId", "CustomerId");
                    try
                    {
                        copy.WriteToServer(dt);
                        sqlbulkTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        sqlbulkTransaction.Rollback();
                        BaseService.WriteLogWeixin("Vip信息插入数据库失败");
                    }
                }
                conn.Close();
            }
        }
        #endregion

        #region 替换特殊字符
        /// <summary>
        ///替换特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ReplaceStr(string str)
        {
            string pStr = string.Empty;
            // Regex regChina = new Regex("^[^\x00-\xFF]");
            Regex regChina = new Regex("^[\u4e00-\u9fa5]+$");
            Regex regEnglish = new Regex("^[a-zA-Z]");
            Regex regInter = new Regex(@"^\d*$");
            foreach (char item in str.ToArray())
            {
                if (regChina.IsMatch(item.ToString()) || regEnglish.IsMatch(item.ToString()) || regInter.IsMatch(item.ToString()))
                {
                    pStr += item.ToString();
                }

            }
            return pStr;


            //string[] aryReg ={
            //            @"<script[^>]*?>.*?</script>",
            //            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            //            @"([\r\n])[\s]+",
            //            @"-->",
            //            @"<!--.*\n"
            //            };
            //string resStr = str.Replace("'", "");
            //for (int i = 0; i < aryReg.Length; i++)
            //{
            //    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(aryReg[i], System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //    resStr = regex.Replace(resStr, "");
            //}
            //return resStr;
        }
        #endregion

        #region 将List转为DataTable
        private static DataTable ConvertListToDatatTable<T>(List<T> entitys)
        {
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            Type entityType = entitys[0].GetType();
            System.Reflection.PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
        #endregion
        #endregion

        #endregion

        #region 发货通知接口
        /// <summary>
        /// 发货通知接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <param name="postData">发货通知的参数，包含appid,openid,out_trade_no,transid,deliver_timestamp,deliver_status,deliver_msg,app_signature 的json体</param>
        /// <returns></returns>
        public string DeliverNotify(string accessToken, LoggingSessionInfo loggingSessionInfo, string postData)
        {
            var url = "https://api.weixin.qq.com/pay/delivernotify";
            url = url + "?access_token=" + accessToken;

            Loggers.Debug(new DebugLogInfo() { Message = "微信发货通知请求URL:" + url });

            Loggers.Debug(new DebugLogInfo() { Message = "微信发货通知请求PostData:" + postData });

            var method = "POST";
            string result = GetRemoteData(url, method, postData);

            var data = result.DeserializeJSONTo<WxErrMessage>();

            #region 向表中记录调用的微信接口

            var wxInterfaceLogBll = new WXInterfaceLogBLL(loggingSessionInfo);
            var wxInterfaceLogEntity = new WXInterfaceLogEntity();
            wxInterfaceLogEntity.LogId = Guid.NewGuid();
            wxInterfaceLogEntity.InterfaceUrl = "https://api.weixin.qq.com/pay/delivernotify";
            wxInterfaceLogEntity.RequestParam = postData;
            wxInterfaceLogEntity.ResponseParam = result;
            wxInterfaceLogEntity.IsSuccess = data.errcode == 0 ? 1 : 0; //errcode = 0 标识成功
            wxInterfaceLogBll.Create(wxInterfaceLogEntity);

            #endregion
            return result;

        }

        #endregion

        #region 通知微信维权

        public string UpdatePayFeedBack(string accessToken, LoggingSessionInfo loggingSessionInfo, string openId, string feedbackId)
        {
            var url = "https://api.weixin.qq.com/payfeedback/update";
            url = url + "?access_token=" + accessToken + "&openid=" + openId + "&feedbackid=" + feedbackId;

            Loggers.Debug(new DebugLogInfo() { Message = "微信维权更新URL:" + url });

            var result = GetRemoteData(url, "GET", string.Empty);

            var data = result.DeserializeJSONTo<WxErrMessage>();

            #region 向表中记录调用的微信接口

            var wxInterfaceLogBll = new WXInterfaceLogBLL(loggingSessionInfo);
            var wxInterfaceLogEntity = new WXInterfaceLogEntity();
            wxInterfaceLogEntity.LogId = Guid.NewGuid();
            wxInterfaceLogEntity.InterfaceUrl = "https://api.weixin.qq.com/payfeedback/update";
            wxInterfaceLogEntity.OpenId = openId;
            wxInterfaceLogEntity.RequestParam = "&openid=" + openId + "&feedbackid=" + feedbackId;
            wxInterfaceLogEntity.ResponseParam = result;
            wxInterfaceLogEntity.IsSuccess = data.errcode == 0 ? 1 : 0; //errcode = 0 标识成功
            wxInterfaceLogEntity.CustomerId = loggingSessionInfo.ClientID;
            wxInterfaceLogBll.Create(wxInterfaceLogEntity);

            #endregion

            return result;
        }

        #endregion

        /// <summary>
        /// 根据用户发送的二维码去二维码表中VipDCode匹配
        /// </summary>
        /// <param name="content"></param>
        /// <param name="vipID"></param>
        public static void StoreRebate(string content, string SalesAmount, string PushInfo, decimal ReturnAmount, string vipID, string openId, System.Data.SqlClient.SqlTransaction tran, LoggingSessionInfo LoggingSessionInfo)
        {

            VipDCodeBLL bll = new VipDCodeBLL(LoggingSessionInfo);

            try
            {
                //var temp = bll.QueryByEntity(new VipDCodeEntity { IsDelete = 0, DCodeId = content }, null);
                var temp = bll.GetByID(content);

                //if (temp != null && temp.Length > 0)   //如果可以匹配，则更新二维码表中的会员ID，OpenId
                if (temp != null)
                {
                    var vipBll = new VipBLL(LoggingSessionInfo);

                    var vipEntity = vipBll.GetByID(vipID);

                    #region 添加返现码过期和被领取的消息提醒
                    if (temp.IsReturn == 1)
                    {
                        //发送消息

                        JIT.CPOS.BS.BLL.WX.CommonBLL.SendWeixinMessage("对不起，该返利码已经被领取", "1", LoggingSessionInfo, vipEntity);
                        return;
                    }

                    if (DateTime.Now > (temp.CreateTime ?? DateTime.Now).AddDays(1))
                    {
                        //发送消息
                        JIT.CPOS.BS.BLL.WX.CommonBLL.SendWeixinMessage("对不起，您的返利码已经过期，请在收到返利码后的24小时内使用", "1", LoggingSessionInfo, vipEntity);
                        return;
                    }
                    #endregion
                    #region 1.更新返现金额。更新返现状态
                    VipDCodeEntity entity = new VipDCodeEntity();
                    entity = temp;
                    entity.OpenId = openId;
                    entity.VipId = vipID;
                    entity.ReturnAmount = ReturnAmount;
                    entity.DCodeId = content;
                    entity.IsReturn = 1;
                    bll.Update(entity, tran); //更新返现金额
                    #endregion


                    var vipamountBll = new VipAmountBLL(LoggingSessionInfo);
                    var validReturnAmount = vipamountBll.GetVipValidReturnAmountByID(vipID, tran);
                    var message = PushInfo.Replace("#SalesAmount#", SalesAmount.ToString()).Replace("#ReturnAmount#", ReturnAmount.ToString("0.00")).Replace("#ValidReturnAmount#", validReturnAmount.ToString("0.00")).Replace("#VipName#", vipEntity.VipName);
                    #region 插入门店返现推送消息日志表
                    WXSalesPushLogBLL PushLogbll = new WXSalesPushLogBLL(LoggingSessionInfo);
                    WXSalesPushLogEntity pushLog = new WXSalesPushLogEntity();
                    pushLog.LogId = Guid.NewGuid();
                    //pushLog.WinXin = requestParams.WeixinId;
                    pushLog.OpenId = openId;
                    pushLog.VipId = vipID;
                    pushLog.PushInfo = message;
                    pushLog.DCodeId = content;
                    pushLog.RateId = Guid.NewGuid();
                    PushLogbll.Create(pushLog, tran);
                    #endregion

                    string code = JIT.CPOS.BS.BLL.WX.CommonBLL.SendWeixinMessage(message, "1", LoggingSessionInfo, vipEntity);

                    Loggers.Debug(new DebugLogInfo() { Message = "消息推送完成，code=" + code + ", message=" + message });
                }

            }
            catch (Exception)
            {

                throw;
            }

        }



        #region 发送微信模板消息方法

        #region inner method
        /// <summary>
        /// 根据订单ID获取商品名称
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public string GetItemName(string order_id, LoggingSessionInfo loggingSessionInfo)
        {
            var InoutDetailBLL = new T_Inout_DetailBLL(loggingSessionInfo);
            var skuBLL = new T_SkuBLL(loggingSessionInfo);
            var ItembBLL = new T_ItemBLL(loggingSessionInfo);
            string Result = "";

            var InoutDetailEntity = InoutDetailBLL.QueryByEntity(new T_Inout_DetailEntity() { order_id = order_id }, null).FirstOrDefault();
            if (InoutDetailEntity == null)
                return Result;
            var SkuEntity = skuBLL.GetByID(InoutDetailEntity.sku_id);
            if (SkuEntity == null)
                return Result;
            var ItemEntity = ItembBLL.GetByID(SkuEntity.item_id);
            if (ItemEntity == null)
                return Result;
            return ItemEntity.item_name;
        }


        /// <summary>
        /// 根据模板编号发送匹配模板微信消息底层方法
        /// </summary>
        /// <param name="CommonData"></param>
        /// <param name="balanceData"></param>
        /// <param name="CashBackData"></param>
        /// <param name="PaySuccessData"></param>
        /// <param name="IntegralChangeData"></param>
        /// <param name="code"></param>
        /// <param name="openID"></param>
        /// <param name="VipID"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public ResultEntity SendMatchWXTemplateMessage(string TemplateId, CommonData CommonData, Balance balanceData, 
            CashBack CashBackData, PaySuccess PaySuccessData, IntegralChange IntegralChangeData,
            CouponsArrival CouponsArrivalData, CouponsUpcomingExpired CouponsUpcomingExpiredData,NotPay NotPayData,
            string code, string openID, string VipID, LoggingSessionInfo loggingSessionInfo)
        {
            var ResultData = new ResultEntity();
            var WApplicationInterfaceBLL = new WApplicationInterfaceBLL(loggingSessionInfo);
            var WAData = WApplicationInterfaceBLL.QueryByEntity(new WApplicationInterfaceEntity() { CustomerId = loggingSessionInfo.ClientID }, null)[0];
            //获取AccessToke
            var AccessToke = GetAccessToken(loggingSessionInfo);
            if (AccessToke.errcode == null || AccessToke.errcode.Equals(string.Empty))
            {
                string uri = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + AccessToke.access_token;
                string method = "POST";
                Loggers.Debug(new DebugLogInfo() { Message = string.Format("发送模板消息使用的token:{0}", AccessToke.access_token) });
                string DataJson = "";//
                string templateIdShort = "";
                string WeiXinId = "";
                if (WAData != null)
                    WeiXinId = WAData.WeiXinID;
                switch (code)
                {
                    case "1"://礼券即将到期提醒
                        templateIdShort = "OPENTM206623166";
                        TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        DataJson = JsonHelper.JsonSerializer<CouponsUpcomingExpired>(CouponsUpcomingExpiredData);
                        break;
                    case "2": //订单未付款通知
                        templateIdShort = "TM00701";
                        TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        DataJson = JsonHelper.JsonSerializer<NotPay>(NotPayData);
                        break;
                    case "3": //电子券到账提醒
                        templateIdShort = "OPENTM207444083";
                        TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        DataJson = JsonHelper.JsonSerializer<CouponsArrival>(CouponsArrivalData);
                        break;
                    case "4": //积分即将过期通知
                        //templateIdShort = "OPENTM204537469";
                        //TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        //DataJson = JsonHelper.JsonSerializer<Data>(baseData);
                        break;
                    case "5": //积分变动通知
                        templateIdShort = "TM00230";
                        DataJson = JsonHelper.JsonSerializer<IntegralChange>(IntegralChangeData);
                        break;
                    case "6": //积分兑换成功通知
                        //templateIdShort = "TM00232";
                        //TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        //DataJson = JsonHelper.JsonSerializer<Data>(baseData);
                        break;
                    case "7": //取货提醒
                        //templateIdShort = "OPENTM202354605";
                        //TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        //DataJson = JsonHelper.JsonSerializer<Data>(baseData);
                        break;
                    case "8": //订单发货提醒
                        templateIdShort = "OPENTM200565259";
                        DataJson = JsonHelper.JsonSerializer<CommonData>(CommonData);
                        break;
                    case "9": //优惠券过期提醒
                        //templateIdShort = "OPENTM206706184";
                        //TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        //DataJson = JsonHelper.JsonSerializer<Data>(baseData);
                        break;
                    case "10": //账户余额变动通知
                        templateIdShort = "OPENTM205454780";
                        DataJson = JsonHelper.JsonSerializer<Balance>(balanceData);
                        break;
                    case "11": //订单未支付通知
                        //templateIdShort = "TM00184";
                        //TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        //DataJson = JsonHelper.JsonSerializer<OrderData>(orderData);
                        break;
                    case "12": //返现到账通知
                        templateIdShort = "TM00211";
                        DataJson = JsonHelper.JsonSerializer<CashBack>(CashBackData);
                        break;
                    case "13": //退款成功通知
                        //templateIdShort = "OPENTM202723917";
                        //TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        //DataJson = JsonHelper.JsonSerializer<OrderData>(orderData);
                        break;
                    case "14": //退货确认通知
                        //templateIdShort = "OPENTM202849987";
                        //TemplateId = GetWXTemplateID(WeiXinId, templateIdShort, loggingSessionInfo);
                        //DataJson = JsonHelper.JsonSerializer<ReturnData>(returnData);
                        break;
                    case "15"://付款通知
                        templateIdShort = "TM00398";
                        DataJson = JsonHelper.JsonSerializer<PaySuccess>(PaySuccessData);
                        break;
                }
                //跳到会员中心（会员卡页面）
                //string url = "http://api.test.chainclouds.com/HtmlApps/html/common/vipCard/getCard.html?customerId=2ad24e9a668b439a8acbd67a23f77dc6&openId=" + openID;
                string url = WApplicationInterfaceBLL.GetOsVipMemberCentre(WAData.WeiXinID);

                string Json = "{\"touser\":\"" + openID + "\",\"template_id\":\"" + TemplateId + "\",\"url\":\"" + url + "\",\"data\":";
                Json += DataJson + "}";

                string Result = GetRemoteData(uri, method, Json);
                ResultData = JsonHelper.JsonDeserialize<ResultEntity>(Result);
            }
            return ResultData;
        }
        /// <summary>
        /// 获取配置表中的模板ID
        /// </summary>
        /// <param name="templateIdShort"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private string GetWXTemplateID(string weiXinId, string templateIdShort, LoggingSessionInfo loggingSessionInfo)
        {
            string TemplateID = "";
            var WXTMConfigBLL = new WXTMConfigBLL(loggingSessionInfo);
            var ResultData = WXTMConfigBLL.QueryByEntity(new WXTMConfigEntity() { WeiXinId = weiXinId, TemplateIdShort = templateIdShort }, null).FirstOrDefault();
            if (ResultData != null)
                TemplateID = ResultData.TemplateID;
            return TemplateID;
        }
        /// <summary>
        /// 设置微信消息模板所属行业
        /// </summary>
        /// <param name="accessToke"></param>
        public void SetWXTemplateIndustry(AccessTokenEntity accessToke)
        {
            if (accessToke.errcode == null || accessToke.errcode.Equals(string.Empty))
            {
                string uri = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token=ACCESS_TOKEN=" + accessToke.access_token;
                string method = "POST";
                string Json = "{\"industry_id1\":\"1\",\"industry_id2\":\"31\"}";
                string Result = GetRemoteData(uri, method, Json);
            }
        }
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        private AccessTokenEntity GetAccessToken(LoggingSessionInfo loggingSessionInfo)
        {
            var WXUserInfoBLL = new WXUserInfoBLL(loggingSessionInfo);
            var appService = new WApplicationInterfaceBLL(loggingSessionInfo);

            //var WXUserInfoList = WXUserInfoBLL.QueryByEntity(new WXUserInfoEntity() { CustomerID=loggingSessionInfo.ClientID}, null).ToList();

            var appEntity = appService.QueryByEntity(new WApplicationInterfaceEntity() { CustomerId = loggingSessionInfo.ClientID }, null)[0];


            var AccessToke = new CommonBLL().GetAccessTokenByCache(appEntity.AppID, appEntity.AppSecret, loggingSessionInfo);
            return AccessToke;
        }
        #endregion

        #region out method

        #endregion
        #region 发送
        /// <summary>
        /// 付款成功通知
        /// </summary>
        /// <param name="Inout"></param>
        /// <param name="OpenID"></param>
        /// <param name="VipID"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public ResultEntity SentPaymentMessage(T_InoutEntity Inout, string OpenID, string VipID, LoggingSessionInfo loggingSessionInfo)
        {
            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();

            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = "TM00398", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();
            if (WXTMConfigData == null)
                return new ResultEntity();
            if (WXTMConfigData == null)
                return new ResultEntity();

            string ItemName = GetItemName(Inout.order_no, loggingSessionInfo);
            PaySuccess PaySuccessData = new PaySuccess();
            PaySuccessData.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            PaySuccessData.orderProductPrice = new DataInfo() { value = Math.Round(Inout.actual_amount ?? 0, 2).ToString(), color = WXTMConfigData.Colour1 };
            PaySuccessData.orderProductName = new DataInfo() { value = ItemName, color = WXTMConfigData.Colour2 };
            PaySuccessData.orderAddress = new DataInfo() { value = Inout.Field4, color = WXTMConfigData.Colour3 };
            PaySuccessData.orderName = new DataInfo() { value = Inout.order_no, color = WXTMConfigData.Colour3 };
            PaySuccessData.remark = new DataInfo() { value = WXTMConfigData.RemarkText, color = WXTMConfigData.RemarkColour };

            return SendMatchWXTemplateMessage(WXTMConfigData.TemplateID, null, null, null, PaySuccessData, null,null,null,null, "15", OpenID, VipID, loggingSessionInfo);
        }
        /// <summary>
        /// 发货提醒通知
        /// </summary>
        /// <param name="Inout"></param>
        /// <param name="OpenID"></param>
        /// <param name="VipID"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public ResultEntity SentShipMessage(InoutInfo Inout, string OpenID, string VipID, LoggingSessionInfo loggingSessionInfo)
        {
            if (string.IsNullOrWhiteSpace(OpenID))
            {
                var VipInfo = new VipBLL(loggingSessionInfo).GetByID(Inout.vipId);
                OpenID = VipInfo == null ? "" : VipInfo.WeiXinUserId;
            }

            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() 
            { TemplateIdShort = "OPENTM200565259", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();
            if (WXTMConfigData == null)
                return new ResultEntity();
            if (WXTMConfigData == null)
                return new ResultEntity();
            CommonData CommonData = new CommonData();
            CommonData.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            CommonData.keyword1 = new DataInfo() { value = Inout.order_no, color = WXTMConfigData.Colour1 };
            CommonData.keyword2 = new DataInfo() { value = Inout.carrier_name, color = WXTMConfigData.Colour2 };
            CommonData.keyword3 = new DataInfo() { value = Inout.Field2, color = WXTMConfigData.Colour3 };
            CommonData.remark = new DataInfo() { value = WXTMConfigData.RemarkText, color = WXTMConfigData.RemarkColour };

            return SendMatchWXTemplateMessage(WXTMConfigData.TemplateID, CommonData, null, null, null, null, null, null, null, "8", OpenID, VipID, loggingSessionInfo);
        }
        /// <summary>
        /// 积分变动通知
        /// </summary>
        /// <param name="OldIntegration">变动前积分</param>
        /// <param name="vipInfo">会员信息</param>
        /// <param name="ChangeIntegral">变动积分</param>
        /// <param name="OpenID">微信UserID</param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public ResultEntity PointsChangeMessage(string OldIntegration, VipEntity vipInfo, string ChangeIntegral, string OpenID, LoggingSessionInfo loggingSessionInfo)
        {
            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = "TM00230", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();
            if (WXTMConfigData == null)
                return new ResultEntity();
            if (ChangeIntegral.Equals("0"))
                return null;
            IntegralChange IntegralChangeData = new IntegralChange();
            IntegralChangeData.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            IntegralChangeData.FieldName = new DataInfo() { value = "更新前积分", color = WXTMConfigData.Colour1 };
            IntegralChangeData.Account = new DataInfo() { value = Convert.ToInt32(Convert.ToDouble(OldIntegration)).ToString(), color = WXTMConfigData.Colour1 };
            IntegralChangeData.change = new DataInfo() { value = "变动", color = WXTMConfigData.Colour2 };
            IntegralChangeData.CreditChange = new DataInfo() { value = Convert.ToInt32(Convert.ToDouble(ChangeIntegral)).ToString(), color = WXTMConfigData.Colour2 };
            IntegralChangeData.CreditTotal = new DataInfo() { value = Convert.ToInt32(Convert.ToDouble(vipInfo.Integration)).ToString(), color = WXTMConfigData.Colour3 };
            IntegralChangeData.Remark = new DataInfo() { value = WXTMConfigData.RemarkText, color = WXTMConfigData.RemarkColour };

            return SendMatchWXTemplateMessage(WXTMConfigData.TemplateID, null, null, null, null, IntegralChangeData, null, null, null, "5", OpenID, vipInfo.VIPID, loggingSessionInfo);
        }
        /// <summary>
        /// 账户余额变动
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="vipAmountEntity"></param>
        /// <param name="VipAmountDetailEntity"></param>
        /// <param name="OpenID"></param>
        /// <param name="VipID"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public ResultEntity BalanceChangedMessage(string OrderNo, VipAmountEntity vipAmountEntity, VipAmountDetailEntity detailInfo, string OpenID, string VipID, LoggingSessionInfo loggingSessionInfo)
        {
            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = "OPENTM205454780", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();
            if (WXTMConfigData == null)
                return new ResultEntity();
            if ((vipAmountEntity.EndAmount ?? 0) == 0)
                return null;
            Balance BalanceData = new Balance();
            BalanceData.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            BalanceData.keyword1 = new DataInfo() { value = "客户余额账户", color = WXTMConfigData.Colour1 };
            BalanceData.keyword2 = new DataInfo() { value = detailInfo.Reason, color = WXTMConfigData.Colour2 };
            BalanceData.keyword3 = new DataInfo() { value = OrderNo, color = WXTMConfigData.Colour2 };
            BalanceData.keyword4 = new DataInfo() { value = Math.Round(detailInfo.Amount ?? 0, 2).ToString(), color = WXTMConfigData.Colour3 };
            BalanceData.keyword5 = new DataInfo() { value = Math.Round(vipAmountEntity.EndAmount ?? 0, 2).ToString(), color = WXTMConfigData.Colour3 };
            BalanceData.remark = new DataInfo() { value = WXTMConfigData.RemarkText, color = WXTMConfigData.RemarkColour };

            return SendMatchWXTemplateMessage(WXTMConfigData.TemplateID, null, BalanceData, null, null, null, null, null, null, "10", OpenID, VipID, loggingSessionInfo);
        }
        /// <summary>
        /// 返现到账通知
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="money"></param>
        /// <param name="OpenID"></param>
        /// <param name="VipID"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public ResultEntity CashBackMessage(string OrderNo, decimal? money, string OpenID, string VipID, LoggingSessionInfo loggingSessionInfo)
        {
            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = "TM00211", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();
            if (WXTMConfigData == null)
                return new ResultEntity();
            if ((money ?? 0) == 0)
                return null;
            CashBack CashBackData = new CashBack();
            CashBackData.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            CashBackData.order = new DataInfo() { value = OrderNo, color = WXTMConfigData.Colour1 };
            CashBackData.money = new DataInfo() { value = Math.Round(money ?? 0, 2).ToString(), color = WXTMConfigData.Colour2 };
            CashBackData.remark = new DataInfo() { value = WXTMConfigData.RemarkText + DateTime.Now.ToString("yyyy-MM-dd"), color = WXTMConfigData.RemarkColour };

            return SendMatchWXTemplateMessage(WXTMConfigData.TemplateID, null, null, CashBackData, null, null, null, null, null, "12", OpenID, VipID, loggingSessionInfo);
        }

        /// <summary>
        /// 电子券到账通知
        /// </summary>
        /// <param name="CouponCode">券编号</param>
        /// <param name="CouponName">券名称</param>
        /// <param name="ValidityData">有效期 例：2015.09.01-2015.10.31</param>
        /// <param name="Scope">适用范围 例：通用券、门店券、云点券等等</param>
        /// <param name="loggingSessionInfo"></param>
        /// <param name="CouponCount">可选参数：优惠券数量，默认数量1</param>
        /// <returns></returns>
        public ResultEntity CouponsArrivalMessage(string CouponCode, string CouponName, string ValidityData, string Scope, string OpenID, LoggingSessionInfo loggingSessionInfo, string CouponCount = "1")
        {
            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = "OPENTM207444083", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();
            if (WXTMConfigData == null)
                return new ResultEntity();
            
            var CouponsArrivalData = new CouponsArrival();
            CouponsArrivalData.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            CouponsArrivalData.keyword1 = new DataInfo() { value = CouponCode, color = WXTMConfigData.Colour1 };//券码
            CouponsArrivalData.keyword2 = new DataInfo() { value = CouponName, color = WXTMConfigData.Colour1 };//券名称
            CouponsArrivalData.keyword3 = new DataInfo() { value = CouponCount, color = WXTMConfigData.Colour1 };//可用数量
            CouponsArrivalData.keyword4 = new DataInfo() { value = ValidityData, color = WXTMConfigData.Colour1 };//有效期
            CouponsArrivalData.keyword5 = new DataInfo() { value = Scope, color = WXTMConfigData.Colour1 };//
            CouponsArrivalData.Remark = new DataInfo() { value = WXTMConfigData.RemarkText, color = WXTMConfigData.RemarkColour };

            return SendMatchWXTemplateMessage(WXTMConfigData.TemplateID, null, null, null, null, null, CouponsArrivalData, null, null, "3", OpenID, null, loggingSessionInfo);
        }
        /// <summary>
        /// 礼券即将到期提醒
        /// </summary>
        /// <param name="CouponType">券类型</param>
        /// <param name="CouponCode">券码</param>
        /// <param name="EffectiveData">生效日期</param>
        /// <param name="FailData">失效日期</param>
        /// <param name="OpenID">WeiXinUeserId</param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public ResultEntity CouponsUpcomingExpiredMessage(string CouponType, string CouponCode, string EffectiveData, string FailData, string OpenID, LoggingSessionInfo loggingSessionInfo)
        {
            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = "OPENTM206623166", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();

            var Data = new CouponsUpcomingExpired();
            Data.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            Data.keyword1 = new DataInfo() { value = CouponType, color = WXTMConfigData.Colour1 };//券类型
            Data.keyword2 = new DataInfo() { value = CouponCode, color = WXTMConfigData.Colour1 };//券码
            Data.keyword3 = new DataInfo() { value = EffectiveData, color = WXTMConfigData.Colour1 };//生效日期
            Data.keyword4 = new DataInfo() { value = FailData, color = WXTMConfigData.Colour1 };//失效日期
            Data.remark = new DataInfo() { value = WXTMConfigData.RemarkText, color = WXTMConfigData.RemarkColour };

            return SendMatchWXTemplateMessage(WXTMConfigData.TemplateID, null, null, null, null, null, null, Data, null, "1", OpenID, null, loggingSessionInfo);
        }
        /// <summary>
        /// 订单未付款通知
        /// </summary>
        /// <param name="orderProductPrice">订单金额</param>
        /// <param name="orderProductName">商品详情</param>
        /// <param name="orderAddress">收货信息</param>
        /// <param name="orderName">订单编号</param>
        /// <param name="OpenID">WeiXinUeserId</param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public ResultEntity NotPayMessage(string orderProductPrice, string orderProductName, string orderAddress, string orderName, string OpenID, LoggingSessionInfo loggingSessionInfo)
        {
            var CommonBLL = new JIT.CPOS.BS.BLL.WX.CommonBLL();
            var WXTMConfigData = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = "TM00701", CustomerId = loggingSessionInfo.ClientID, IsDelete = 0 }, null).FirstOrDefault();

            var Data = new NotPay();
            Data.first = new DataInfo() { value = WXTMConfigData.FirstText, color = WXTMConfigData.FirstColour };
            Data.orderProductPrice = new DataInfo() { value = orderProductPrice, color = WXTMConfigData.Colour1 };//订单金额
            Data.orderProductName = new DataInfo() { value = orderProductName, color = WXTMConfigData.Colour1 };//商品详情
            Data.orderAddress = new DataInfo() { value = orderAddress, color = WXTMConfigData.Colour1 };//收货信息
            Data.orderName = new DataInfo() { value = orderName, color = WXTMConfigData.Colour1 };//订单编号
            Data.remark = new DataInfo() { value = WXTMConfigData.RemarkText, color = WXTMConfigData.RemarkColour };

            return SendMatchWXTemplateMessage(WXTMConfigData.TemplateID, null, null, null, null, null, null, null, Data, "2", OpenID, null, loggingSessionInfo);
        }
        #endregion

        #region 设置
        /// <summary>
        /// 根据微信模板编号设置公众号模板消息，并返回模板ID录入到系统微信消息模板配置表中
        /// </summary>
        /// <param name="templateIdShort">模板编号</param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public void AddWXTemplateID(string applicationId, string templateIdShort, string FirstText, string RemarkText, LoggingSessionInfo loggingSessionInfo)
        {
            var WApplicationInterfaceData = new WApplicationInterfaceBLL(loggingSessionInfo).GetByID(applicationId);
            var accessToke = GetAccessTokenByCache(WApplicationInterfaceData.AppID, WApplicationInterfaceData.AppSecret, loggingSessionInfo);
            var WXTMConfigBLL = new WXTMConfigBLL(loggingSessionInfo);
            string TemplateID = "";
            if (accessToke.errcode == null || accessToke.errcode.Equals(string.Empty))
            {
                //#region 设置行业模板
                //SetWXTemplateIndustry(accessToke);
                //#endregion

                #region 移除原模板消息
                string OldTemplateID = GetTemplateID(templateIdShort, loggingSessionInfo);
                if (!string.IsNullOrWhiteSpace(OldTemplateID))
                {
                    //移除微信设置的模板
                    string Deluri = "https://api.weixin.qq.com/cgi-bin/template/del_private_template?access_token=" + accessToke.access_token;
                    string Delmethod = "POST";
                    string DelJson = "{\"template_id\":\"" + OldTemplateID + "\"}";
                    string DelResult = GetRemoteData(Deluri, Delmethod, DelJson);
                    var DelResultData = JsonHelper.JsonDeserialize<ResultEntity>(DelResult);
                    //删除数据库库中原模板消息
                    if (DelResultData.errcode.Equals("0"))
                    {

                        //WXTMConfigBLL.Delete(templateIdShort);
                        var WXTMConfigData = WXTMConfigBLL.QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = templateIdShort, IsDelete = 0, CustomerId = loggingSessionInfo.ClientID }, null).FirstOrDefault();
                        if (WXTMConfigData != null) 
                            WXTMConfigBLL.Delete(WXTMConfigData);
                    }
                }
                #endregion


                #region 设置模板ID
                string uri = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token=" + accessToke.access_token;
                string method = "POST";
                string Json = "{\"template_id_short\":\"" + templateIdShort + "\"}";
                string Result = GetRemoteData(uri, method, Json);
                ResultEntity ResultData = JsonHelper.JsonDeserialize<ResultEntity>(Result);
                TemplateID = ResultData.template_id;
                #endregion
            }
            #region 录入微信消息模板配置表
            //获取微信公众号相关信息

            if (WApplicationInterfaceData != null)
            {
                if (!string.IsNullOrWhiteSpace(TemplateID))
                {
                    var AddData = new WXTMConfigEntity()
                    {
                        TemplateID = TemplateID,
                        WeiXinId = WApplicationInterfaceData.WeiXinID,
                        TemplateIdShort = templateIdShort,
                        AppId = WApplicationInterfaceData.AppID,
                        FirstText = FirstText,
                        RemarkText = RemarkText,
                        FirstColour = "#173177",
                        RemarkColour = "#173177",
                        AmountColour = "#173177",
                        Colour1 = "#173177",
                        Colour2 = "#173177",
                        Colour3 = "#173177",
                        Colour4 = "#173177",
                        Colour5 = "#173177",
                        Colour6 = "#173177",
                        CustomerId = loggingSessionInfo.ClientID,
                        IsDelete = 0,
                        CreateTime = DateTime.Now,
                        CreateBy = loggingSessionInfo.UserID,
                        LastUpdateBy = loggingSessionInfo.UserID,
                        LastUpdateTime = DateTime.Now,
                        Title = ""
                    };
                    WXTMConfigBLL.Create(AddData);


                }
            }
            #endregion

        }
        /// <summary>
        /// 根据模板编号找到对应的模板ID
        /// </summary>
        /// <param name="templateIdShort"></param>
        /// <param name="loggingSessionInfo"></param>
        private string GetTemplateID(string templateIdShort, LoggingSessionInfo loggingSessionInfo)
        {
            string GetTemplateID = "";

            var Result = new WXTMConfigBLL(loggingSessionInfo).QueryByEntity(new WXTMConfigEntity() { TemplateIdShort = templateIdShort, CustomerId = loggingSessionInfo.ClientID,IsDelete=0 },null).FirstOrDefault();
            if (Result != null)
                GetTemplateID = Result.TemplateID;

            return GetTemplateID;
        }
        #endregion

        #endregion

        #region 群发微信消息
        /// <summary>
        /// 群发微信消息(最多支持一万条)
        /// </summary>
        /// <param name="OpenIdArray"></param>
        /// <param name="Content"></param>
        /// <param name="loggingSessionInfo"></param>
        /// <returns></returns>
        public string BulkSendWXTemplateMessage(string[] OpenIdArray, string Content, LoggingSessionInfo loggingSessionInfo)
        {
            string Result = "";
            var appService = new WApplicationInterfaceBLL(loggingSessionInfo);
            var appEntity = appService.QueryByEntity(new WApplicationInterfaceEntity() { CustomerId = loggingSessionInfo.ClientID }, null)[0];
            var AccessToke = new CommonBLL().GetAccessTokenByCache(appEntity.AppID, appEntity.AppSecret, loggingSessionInfo);

            if (AccessToke.errcode == null || AccessToke.errcode.Equals(string.Empty))
            {
                string uri = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=" + AccessToke.access_token;
                string method = "POST";
                Loggers.Debug(new DebugLogInfo() { Message = string.Format("发送模板消息使用的token:{0}", AccessToke.access_token) });

                #region 处理json
                StringBuilder Json = new StringBuilder();
                Json.Append("{\"touser\":[");
                for (int i = 0; i < OpenIdArray.Length; i++)
                {
                    Json.AppendFormat("\"{0}\",", OpenIdArray[i]);
                }
                string NewJson = Json.ToString().TrimEnd(',');
                Json = new StringBuilder();
                Json.Append(NewJson);
                Json.Append("],\"msgtype\":\"text\",\"text\":{\"content\":\"" + Content + "\"}}");
                #endregion

                Result = GetRemoteData(uri, method, Json.ToString());
            }

            return Result;
        }
        #endregion


        #region 消息加解密
        //消息解密
        /// <summary>
        /// 在安全模式或兼容模式下，url上会新增两个参数encrypt_type和msg_signature。encrypt_type表示加密类型，msg_signature:表示对消息体的签名。
        /// url上无encrypt_type参数或者其值为raw时表示为不加密；encrypt_type为aes时，表示aes加密（暂时只有raw和aes两种值)。
        /// 公众帐号开发者根据此参数来判断微信公众平台发送的消息是否加密。
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="postStr"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public string WXDecryptMsg(HttpContext httpContext, string postStr, WApplicationInterfaceEntity wAppEntity, LoggingSessionInfo loggingSessionInfo, out int ret, out string TrueEncodingAESKey)//传入token
        {
            //需要验证msg_signature
            //如果encrypt_type和原来公众号里存的加密方式不一样了，是否需要修改数据库里的记录，看看后面回复信息时是否需要！！
            var timestamp = httpContext.Request["timestamp"] == null ? "" : httpContext.Request["timestamp"].ToString();
            var nonce = httpContext.Request["nonce"] == null ? "" : httpContext.Request["nonce"].ToString();
            BaseService.WriteLogWeixin("时间戳timestamp:  " + timestamp);
            BaseService.WriteLogWeixin("消息体的签名nonce:  " + nonce);
            //新增加的encrypt_type和msg_signature
            var encrypt_type = httpContext.Request["encrypt_type"] == null ? "raw" : httpContext.Request["encrypt_type"].ToString();//encrypt_type如果为空就设为未加密
            var msg_signature = httpContext.Request["msg_signature"] == null ? "" : httpContext.Request["msg_signature"].ToString();//明文模式下没有加密消息签名
            BaseService.WriteLogWeixin("加密类型encrypt_type:  " + encrypt_type);
            BaseService.WriteLogWeixin("消息体的签名msg_signature:  " + msg_signature);
            //获取xml信息
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(postStr);

            XmlNodeList list = doc.GetElementsByTagName("xml");
            XmlNode xn = list[0];
            var application = new WApplicationInterfaceBLL(loggingSessionInfo);
            //如果数据库存的加密方式和查询到的
            int EncryptType = 0;//默认明文
            if (encrypt_type == "aes")  //为了兼容之前的商户，最好更新数据，
            {
                var openIDNode = xn.SelectSingleNode("//FromUserName");
                if (openIDNode != null)
                {
                    EncryptType = 1;//兼容模式下，既有加密的数据，又有没加密的数据
                }
                else
                {
                    EncryptType = 2;//安全模式
                }
            }
            else if (encrypt_type == "raw")
            {
                EncryptType = 0;
            }
            /// <summary>
            /// 消息加密类型0:默认明文模式，不加密，1:兼容模式，接收的消息包含明文和密文，
            /// 发送消息可以使用密文或明文，但不能同时使用
            /// 2:安全模式，采用AES加密
            /// </summary>
            if (wAppEntity.EncryptType != EncryptType)
            {
                wAppEntity.EncryptType = EncryptType;//加密模式
                application.Update(wAppEntity, false);
            }
            //    出于安全考虑，公众平台网站提供了修改EncodingAESKey的功能（在EncodingAESKey可能泄漏时进行修改），所以建议公众账号保存当前的和上一次的EncodinAESKey，若当前EncodingAESKey解密失败，则尝试用上一次的EncodingAESKey的解密。回包时，用哪个Key解密成功，则用此Key加密对应的回包 
            TrueEncodingAESKey = "";
            ret = 0;
            string Token = wAppEntity.Token;
            string CurrentEncodingAESKey = wAppEntity.CurrentEncodingAESKey;
            string PrevEncodingAESKey = wAppEntity.PrevEncodingAESKey;
            string AppID = wAppEntity.AppID;




            if (EncryptType == 2)//安全模式下需要解密,安全模式才需要加密，兼容模式不需要加密
            {
                //如果授权给开放平台了，就用开放平台的
                if (!string.IsNullOrEmpty(wAppEntity.OpenOAuthAppid))
                {
                    Token = wAppEntity.OpenToken;
                    CurrentEncodingAESKey = wAppEntity.OpenCurrentEncodingAESKey;
                    PrevEncodingAESKey = wAppEntity.OpenPrevEncodingAESKey;
                    AppID = wAppEntity.OpenAppID;
                }

                string sMsg = "";  //解析之后的明文
                //当前的加密key不行，就试试上一次的加密key，如果再不行，就失败了
                JIT.CPOS.BS.BLL.WX.WXBizMsgCrypt wxcpt = new JIT.CPOS.BS.BLL.WX.WXBizMsgCrypt(Token, CurrentEncodingAESKey, AppID);

                ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, postStr, ref sMsg);
                if (ret != 0)
                {
                    System.Console.WriteLine("当前EncodingAESKey 错误ERR: Decrypt fail, ret: " + ret);
                    //使用上一次的EncodingAESKey
                    JIT.CPOS.BS.BLL.WX.WXBizMsgCrypt wxcptOld = new JIT.CPOS.BS.BLL.WX.WXBizMsgCrypt(Token, PrevEncodingAESKey, AppID);
                    ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, postStr, ref sMsg);
                    System.Console.WriteLine("上一次EncodingAESKey 错误ERR: Decrypt fail, ret: " + ret);
                    if (ret == 0)//如果原来的PrevEncodingAESKey就用原来的
                    {
                        TrueEncodingAESKey = PrevEncodingAESKey;
                    }
                }
                else if (ret == 0)//等于0时，把用于后面加密的key设为当前EncodingAESKey
                {
                    TrueEncodingAESKey = CurrentEncodingAESKey;
                }

                postStr = sMsg;
                BaseService.WriteLogWeixin("使用当前和上次EncodingAESKey解密后 ret: " + postStr);
            }
            else if (EncryptType == 1 || EncryptType == 0)//兼容模式和明文模式不需要解密
            {//raw明文方式下直接返回数据
                // return postStr;
            }

            return postStr;
        }

        public string WXEncryptMsg(RequestParams requestParams, string response)
        {
            if (requestParams.EncryptType == 2)
            {
                JIT.CPOS.BS.BLL.WX.WXBizMsgCrypt wxcpt = new JIT.CPOS.BS.BLL.WX.WXBizMsgCrypt(requestParams.Token, requestParams.TrueEncodingAESKey, requestParams.AppID);
                string sEncryptMsg = ""; //xml格式的密文
                int ret = wxcpt.EncryptMsg(response, requestParams.Timestamp, requestParams.Nonce, ref sEncryptMsg);
                if (ret != 0)
                {
                    BaseService.WriteLogWeixin("请求参数:" + requestParams.ToJSON());
                }
                response = sEncryptMsg;
                BaseService.WriteLogWeixin("安全模式下的加密信息sEncryptMsg：" + response);
            }

            return response;
        }

        #endregion 消息加解密

        #region 微信回调时，根据微信id获取微信信息
        public WApplicationInterfaceEntity GetWAppEntity(string postStr, out  LoggingSessionInfo LoggingSessionInfo)
        {
            //获取xml信息
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(postStr);

            XmlNodeList list = doc.GetElementsByTagName("xml");
            XmlNode xn = list[0];
            string weixinID = xn.SelectSingleNode("//ToUserName").InnerText;    //开发者微信号
            //根据weixinid获取商户信息和相关wapplicationinterface信息
            //    LoggingSessionInfo
            LoggingSessionInfo = BaseService.GetWeixinLoggingSession(weixinID);//获取登陆信息
            var application = new WApplicationInterfaceBLL(LoggingSessionInfo);
            var appEntitys = application.QueryByEntity(new WApplicationInterfaceEntity() { WeiXinID = weixinID, IsDelete = 0, CustomerId = LoggingSessionInfo.ClientID }, null);
            var wapentity = new WApplicationInterfaceEntity();
            if (appEntitys != null && appEntitys.Length > 0)
            {
                wapentity = appEntitys.FirstOrDefault();
                //BaseService.WriteLogWeixin("通过微信类型生成对应的业务处理类");
                //BaseService.WriteLogWeixin("WeiXinTypeId(微信类型):  " + wapentity.WeiXinTypeId);
            }

            //下面的取开发平台的放在解密的代码里，因为明文用不到
            //如果公众帐号授权给第三方平台管理了
            if (!string.IsNullOrEmpty(wapentity.OpenOAuthAppid))
            {
                DataSet WXOpenOAuthDs = application.GetWXOpenOAuth(wapentity.OpenOAuthAppid);
                if (WXOpenOAuthDs.Tables != null && WXOpenOAuthDs.Tables != null && WXOpenOAuthDs.Tables.Count != 0 && WXOpenOAuthDs.Tables[0].Rows.Count != 0)
                {
                    DataRow WXOpenOAuthDr = WXOpenOAuthDs.Tables[0].Rows[0];
                    wapentity.OpenToken = WXOpenOAuthDr["Token"] == null ? "" : WXOpenOAuthDr["Token"].ToString();
                    wapentity.OpenPrevEncodingAESKey = WXOpenOAuthDr["PrevEncodingAESKey"] == null ? "" : WXOpenOAuthDr["PrevEncodingAESKey"].ToString();
                    wapentity.OpenCurrentEncodingAESKey = WXOpenOAuthDr["EncodingAESKey"] == null ? "" : WXOpenOAuthDr["EncodingAESKey"].ToString();
                    wapentity.OpenAppID = WXOpenOAuthDr["Appid"] == null ? "" : WXOpenOAuthDr["Appid"].ToString();
                }
            }
            return wapentity;
        }

        #endregion


        /// <summary>  
        /// 将时间戳TimeStamp转换为DateTime  
        /// </summary>  
        /// <param name="timeStamp"></param>  
        /// <returns></returns>  
        public string GetRealTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNowTime = new TimeSpan(lTime);
            return startTime.Add(toNowTime).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
