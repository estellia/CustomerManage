using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.BLL.WX;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.Common;
using JIT.CPOS.DTO.Base;
using JIT.Utility.DataAccess.Query;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.Log;
using Xgx.WebAPI.Areas.Xgx.Models;
using Xgx.WebAPI.Common;

namespace Xgx.WebAPI.Areas.Xgx.Controllers
{

    
    public class VipController : ApiBase
    {

        [HttpGet]
        public HttpResponseMessage MemberBenefits(string vipId,decimal? TotalPayAmount)
        {
            string guid = Guid.NewGuid().ToString();
            Loggers.Debug(new DebugLogInfo()
            {
                Message = $"调用[api/Vip/MemberBenefits]，参数：\"vipId:{vipId}\"",
                UserID = vipId,
                ClientID = guid
            });
            try
            {
                if (string.IsNullOrEmpty(vipId))
                {
                    throw new Exception("会员数据不能为空！");
                }

                var orderResponse = new MemberBenefitsResponeModel();

                #region MyRegion

                string customerid = ConfigurationManager.AppSettings["CustomerId"].Trim();
                var loggingSessionInfo = Default.GetLoggingSession(customerid, vipId);
                loggingSessionInfo.Conn = ConfigurationManager.AppSettings["Conn"].Trim();

                var bll = new VipBLL(loggingSessionInfo);
                var vipInfo = bll.GetByID(vipId); //会员信息

                if (vipInfo == null)
                {
                    throw new APIException("没有该会员信息") {ErrorCode = 302};
                }

                #endregion

                //获取社会化销售配置和积分返现配置
                var basicSettingBll = new CustomerBasicSettingBLL(loggingSessionInfo);
                Hashtable htSetting = basicSettingBll.GetSocialSetting();

                orderResponse.EnableIntegral = int.Parse(htSetting["enableIntegral"].ToString());
                orderResponse.EnableRewardCash = int.Parse(htSetting["enableRewardCash"].ToString());

                //应付金额
                decimal? totalPayAmount = 0;
                if (TotalPayAmount != null)
                {
                    totalPayAmount = TotalPayAmount;
                }

                #region 启用积分

                if (orderResponse.EnableIntegral == 1)
                {
                    //2.获取会员的积分和账户余额
                    var vipIntegralbll = new VipIntegralBLL(loggingSessionInfo);
                    //var vipIntegralEntity = vipIntegralbll.GetByID(rp.UserID);
                    //根据会员和会员卡号获取积分
                    var vipIntegralEntity =
                        vipIntegralbll.QueryByEntity(
                                new VipIntegralEntity() {VipID = vipInfo.UserId, VipCardCode = vipInfo.VipCode}, null)
                            .FirstOrDefault();
                    if (vipIntegralEntity == null)
                    {
                        orderResponse.Integral = 0;
                        orderResponse.IntegralAmount = 0;
                    }
                    else
                    {
                        decimal validIntegral = vipIntegralEntity.ValidIntegral ?? 0; //会员积分

                        int totalIntegral = 0; //可使用积分(取整)                
                        //if (int.Parse(htSetting["rewardsType"].ToString()) == 1)//按商品奖励
                        //    totalIntegral = (int)Math.Round(bll.GetIntegralBySkuId(skuIdList), 1);

                        //积分使用上限比例
                        decimal pointsRedeemUpLimit = decimal.Parse(htSetting["pointsRedeemUpLimit"].ToString())/100;
                        //3.获取积分与金额的兑换比例
                        var integralAmountPre = bll.GetIntegralAmountPre(customerid);
                        if (integralAmountPre == 0)
                            integralAmountPre = (decimal) 0.01;

                        totalIntegral = (int) Math.Round(totalPayAmount.Value*pointsRedeemUpLimit*integralAmountPre, 1);
                        //可使用的积分
                        orderResponse.Integral = validIntegral > totalIntegral ? totalIntegral : validIntegral;

                        if (totalPayAmount == 0)
                        {
                            orderResponse.Integral = Convert.ToDecimal(vipIntegralEntity.ValidIntegral);
                        }

                        //rd.IntegralAmount = rd.Integral * integralAmountPre;
                        orderResponse.IntegralAmount = bll.GetAmountByIntegralPer(loggingSessionInfo.ClientID,
                            orderResponse.Integral);
                        orderResponse.IntegralDesc = "使用积分" + orderResponse.Integral.ToString("0") + ",可兑换"
                           + orderResponse.IntegralAmount.ToString("0.00") + "元";
                        orderResponse.PointsRedeemLowestLimit =
                            int.Parse(htSetting["pointsRedeemLowestLimit"].ToString());
                    }
                }

                #endregion

                //根据会员和会员卡号获取余额和返现
                var vipAmountBll = new VipAmountBLL(loggingSessionInfo);
                var vipAmountInfo =
                    vipAmountBll.QueryByEntity(
                            new VipAmountEntity() {VipId = vipInfo.VIPID, VipCardCode = vipInfo.VipCode}, null)
                        .FirstOrDefault();

                #region 启用返现

                if (orderResponse.EnableRewardCash == 1)
                {
                    if (vipAmountInfo != null)
                    {
                        //累计返现金额
                        decimal returnAmount = vipAmountInfo.ValidReturnAmount == null
                            ? 0
                            : vipAmountInfo.ValidReturnAmount.Value;
                        //订单可使用最大返现金额
                        decimal returnAmountOrder = totalPayAmount.Value*
                                                    (decimal.Parse(htSetting["cashRedeemUpLimit"].ToString())/100);
                        orderResponse.ReturnAmount = returnAmount > returnAmountOrder ? returnAmountOrder : returnAmount;
                        orderResponse.CashRedeemLowestLimit =
                            decimal.Parse(htSetting["cashRedeemLowestLimit"].ToString());
                    }
                }

                #endregion

                //账户余额
                //var vipEndAmount = bll.GetVipEndAmount(rp.UserID);
                //rd.VipEndAmount = totalPayAmount > vipEndAmount ? vipEndAmount : totalPayAmount;
                if (vipAmountInfo != null)
                    orderResponse.VipEndAmount = vipAmountInfo.EndAmount.Value;

                //获取会员折扣
                var sysVipCardGradeBLL = new SysVipCardGradeBLL(loggingSessionInfo);
                decimal vipDiscount = 10; //会员折扣
                //如果订单业务类型为null
                var orderReason = GetOrderReason(EnumOrderReason.Pos);
                //超级分销、团购、抢购、砍价商品没有会员折扣
                if (orderReason != "CB43DD7DD1C94853BE98C4396738E00C" &&
                    orderReason != "671E724C85B847BDA1E96E0E5A62055A" &&
                    orderReason != "096419BFDF394F7FABFE0DFCA909537F")
                {
                    //if (rp.Parameters.DiscountType == 0)
                    vipDiscount = sysVipCardGradeBLL.GetVipDiscount();
                }

                orderResponse.VipDiscount = vipDiscount;

                var tempAmount = totalPayAmount.Value;
                if (totalPayAmount.Value > 0 && vipDiscount>0)
                {
                    tempAmount = totalPayAmount.Value * (vipDiscount / 10);
                }

                var ds = bll.GetVipCouponDataSet(vipInfo.VIPID, tempAmount, 2, string.Empty, 1, customerid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var temp = ds.Tables[0].AsEnumerable().Select(t => new CouponModel()
                    {
                        CouponId = t["CouponID"].ToString(),
                        CouponCode = t["CouponCode"].ToString(),
                        CouponAmount = Convert.ToDecimal(t["parValue"]),
                        CouponName = t["CoupnName"].ToString(),
                        CouponDesc = t["CouponDesc"].ToString(),
                        DisplayIndex = Convert.ToInt32(t["displayIndex"]),
                        EnableFlag = Convert.ToInt32(t["EnableFlag"]),
                        //ValidDateDesc = t["ValidDateDesc"].ToString(),
                        StartDate = t["BeginDate"].ToString(),
                        EndDate = t["EndDate"].ToString()
                    });
                    orderResponse.CouponInfoList = temp.ToList();
                }

                orderResponse.IsSucess = true;

                var json = new JavaScriptSerializer().Serialize(orderResponse);
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = $"调用[api/Vip/MemberBenefits]，返回参数：\"{json}\"",
                    UserID = vipId,
                    ClientID = guid
                });

                return Request.CreateResponse(HttpStatusCode.OK, orderResponse);

            }
            catch (Exception ex)
            {
                var orderResponse = new MemberBenefitsResponeModel()
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = $"[api/Vip/MemberBenefits]错误，参数：\"ErrorMsg:{ex.Message}\"",
                    UserID = vipId,
                    ClientID = guid
                });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, orderResponse);
            }
        }

        [HttpGet]
        public HttpResponseMessage QRCode(string UnitId, string ObjectID)
        {
            Loggers.Debug(new DebugLogInfo()
            {
                Message = $"[api/Vip/QRCode]接口，参数：\"UnitId:{UnitId}&ObjectID:{ObjectID}\""
            });
            try
            {
                string customerId = ConfigurationManager.AppSettings["CustomerId"].Trim();
                var loggingSessionInfo = Default.GetLoggingSession(customerId, ObjectID);
                loggingSessionInfo.Conn = ConfigurationManager.AppSettings["Conn"].Trim();

                var imageUrl = string.Empty;
                Random ro = new Random();
                var iUp = 100000000;
                var iDown = 50000000;


                var rpVipDCode = 0; //临时二维码
                var iResult = ro.Next(iDown, iUp); //随机数

                string objectid = ObjectID;

                #region 获取微信帐号

                //门店关联的公众号
                var tuBll = new t_unitBLL(loggingSessionInfo);
                var tuEntity = new t_unitEntity();
                if (!string.IsNullOrEmpty(UnitId))
                {
                    tuEntity =
                        tuBll.QueryByEntity(new t_unitEntity() {unit_id = UnitId}, null)
                            .FirstOrDefault();
                }

                var server = new WApplicationInterfaceBLL(loggingSessionInfo);
                var wxObj = new WApplicationInterfaceEntity();
                if (tuEntity != null && !string.IsNullOrEmpty(tuEntity.weiXinId))
                {
                    wxObj =
                        server.QueryByEntity(
                            new WApplicationInterfaceEntity {WeiXinID = tuEntity.weiXinId, CustomerId = customerId},
                            null).FirstOrDefault();
                }
                else
                {
                    wxObj =
                        server.QueryByEntity(new WApplicationInterfaceEntity {CustomerId = customerId}, null)
                            .FirstOrDefault();
                }

                if (wxObj == null)
                {
                    throw new APIException("不存在对应的微信帐号") {ErrorCode = 302};
                }
                else
                {
                    var commonServer = new CommonBLL();

                    imageUrl = commonServer.GetQrcodeUrl(wxObj.AppID
                        , wxObj.AppSecret
                        , rpVipDCode.ToString("") //二维码类型  0： 临时二维码  1：永久二维码
                        , iResult, loggingSessionInfo); //iResult作为场景值ID，临时二维码时为32位整型，永久二维码时只支持1--100000
                    //"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQGN7zoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL1dreENCS1htX0xxQk94SEJ6MktIAAIEUk88VwMECAcAAA==";
                    //"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQGN7zoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL1dreENCS1htX0xxQk94SEJ6MktIAAIEUk88VwMECAcAAA==";
                    if (imageUrl != null && !imageUrl.Equals(""))
                    {
                        DownloadImage downloadServer = new DownloadImage();
                        string downloadImageUrl = ConfigurationManager.AppSettings["website_WWW"];
                        imageUrl = downloadServer.DownloadFile(imageUrl, downloadImageUrl);

                    }
                }

                #endregion


                #region 创建临时匹配表

                VipDCodeBLL vipDCodeServer = new VipDCodeBLL(loggingSessionInfo);
                VipDCodeEntity info = new VipDCodeEntity();
                info.DCodeId = iResult.ToString(); //记录传过去的二维码场景值****（保存到数据库时没加空格）
                info.CustomerId = customerId;
                info.UnitId = UnitId; //获取会集店
                info.Status = "0";
                info.IsReturn = 0;
                info.DCodeType = 2; // add by donal 2014-9-22 10:02:08
                loggingSessionInfo.UserID = ObjectID;
                info.CreateBy = ObjectID;
                info.ImageUrl = imageUrl;
                //info.VipId = RP.UserID;
                info.ObjectId = ObjectID; //分享经销商的vipid
                vipDCodeServer.Create(info);

                #endregion


                var model = new QRCodeResponseModel()
                {
                    IsSucess = true,
                    ImageUrl = imageUrl,
                    paraTmp = iResult.ToString().Insert(4, " ")
                };

                Loggers.Debug(new DebugLogInfo()
                {
                    Message = $"setSignUp content: {model}"
                });


                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {

                var model = new QRCodeResponseModel()
                {
                    IsSucess = false,
                    ErrorMessage = string.Format("获取二维码出错:{0}", ex.Message)
                };
                Loggers.Debug(new DebugLogInfo()
                {
                    Message = string.Format("获取二维码出错:{0}", ex.Message)
                });
                return Request.CreateResponse(HttpStatusCode.InternalServerError, model);
            }
        }

        [HttpGet]
        public HttpResponseMessage Password(string vipId, string password)
        {
            Loggers.Debug(new DebugLogInfo()
            {
                Message = $"[api/Vip/Password]接口，参数：\"vipId:{vipId}&&password:{password}\""
            });
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    throw new APIException("请求参数中缺少Password或值为空.") { ErrorCode = 121 };
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new APIException("请求参数中缺少Password或值为空.") { ErrorCode = 121 };
                }

                var rd = new EmptyResponseData();

                string customerid = ConfigurationManager.AppSettings["CustomerId"].Trim();
                var loggingSessionInfo = Default.GetLoggingSession(customerid, "1");

                var vipbll = new VipBLL(loggingSessionInfo);
                var vipEntity = vipbll.GetByID(vipId);
                if (vipEntity == null)
                {
                    throw new APIException("无效的会员ID【VipId】") { ErrorCode = 121 };
                }


                var bll = new VipAmountBLL(loggingSessionInfo);
                var vipAmountEntity = bll.GetByID(vipId);

                if (vipAmountEntity == null)
                {
                    throw new APIException("该会员暂无绑定账户") { ErrorCode = 121 };
                }
                else
                {
                    if (vipAmountEntity.PayPassword != password)
                    {
                        throw new APIException("密码不正确") { ErrorCode = 122 };
                    }
                }

                var responseModel = new BaseResponeModel
                {
                    IsSucess = true
                };
                return Request.CreateResponse(HttpStatusCode.OK, responseModel);
            }
            catch (Exception ex)
            {
                var responseModel = new BaseResponeModel
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
            }
        }

        [HttpPost]
        public HttpResponseMessage ScanQRCode([FromBody] ScanQRCodeRequestModel requestParameter)
        {
            var json = new JavaScriptSerializer().Serialize(requestParameter);
            Loggers.Debug(new DebugLogInfo()
            {
                Message = $"[api/Order/ScanQRCode]接口，参数：\"{json}\""
            });
            try
            {

                #region MyRegion

                string customerId = ConfigurationManager.AppSettings["CustomerId"].Trim();
                string content = string.Empty;

                #endregion

                #region

                var loggingSessionInfo = Default.GetLoggingSession(customerId, "1");

                if (string.IsNullOrWhiteSpace(requestParameter.ParaTmp))
                {
                    throw new APIException("paraTmp不能为空") {ErrorCode = 302};
                }

                #endregion

                #region 创建临时匹配表

                VipDCodeBLL vipDCodeServer = new VipDCodeBLL(loggingSessionInfo);
                VipDCodeEntity info = new VipDCodeEntity();
                //由于CodeId有重复的概率，因此只取出最新的一条记录
                info = vipDCodeServer.QueryByEntity(
                    new VipDCodeEntity() {DCodeId = ToStr(requestParameter.ParaTmp.Replace(" ", ""))} //又去掉了中间的空格
                    , new OrderBy[] {new OrderBy() {FieldName = "CreateTime", Direction = OrderByDirections.Desc}}
                )[0];
                string status = string.Empty;
                string vipId = string.Empty;
                string openId = string.Empty;
                if (info == null || info.DCodeId == null)
                {
                    throw new APIException("不存在对应的记录") {ErrorCode = 303};
                }
                else
                {
                    status = info.Status;
                    openId = info.OpenId;
                    vipId = info.VipId;
                }
                string mode = "Inbound";
                //if ((RP.Parameters.special.Mode == null || (!string.IsNullOrEmpty(RP.Parameters.special.Mode) && RP.Parameters.special.Mode.Equals("Inbound"))) && !string.IsNullOrEmpty(info.VipId) && info.VipId != RP.UserID)
                if ((mode == null || (!string.IsNullOrEmpty(mode) && mode.Equals("Inbound"))) &&
                    !string.IsNullOrEmpty(info.VipId) && info.VipId != requestParameter.UserId)
                {
                    VipBLL vipBll = new VipBLL(loggingSessionInfo);
                    var unitBll = new t_unitBLL(loggingSessionInfo);
                    var UserBll = new T_UserBLL(loggingSessionInfo);

                    var vipInfo = vipBll.GetByID(info.VipId);
                    var tt = vipBll.GetUnitByUserId(requestParameter.UserId); //获取员工的会集店****
                    var UserEntity = UserBll.GetByID(vipInfo.SetoffUserId); //当前会员集客员工

                    //
                    string UserStatus = "";
                    if (UserEntity != null)
                        UserStatus = UserEntity.user_status;

                    #region 会员会籍店、集客员工变动处理

                    //string.IsNullOrWhiteSpace(vipInfo.CouponInfo) || string.IsNullOrWhiteSpace(vipInfo.SetoffUserId) 目前未用到
                    if (string.IsNullOrWhiteSpace(vipInfo.HigherVipID) &&
                        string.IsNullOrWhiteSpace(vipInfo.SetoffUserId))
                    {
//当会员会籍店、集客员工为空时
                        if (!string.IsNullOrEmpty(tt))
                        {
                            vipInfo.CouponInfo = tt; //设为门店
                            vipInfo.SetoffUserId = requestParameter.UserId; //设为门店员工
                            vipInfo.Col21 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //集客时间*****
                            vipInfo.Col23 = "1";
                        }
                        if (vipInfo != null && vipInfo.SetoffUserId == requestParameter.UserId)
                        {
                            //rsp.Message = "恭喜你集客成功。会员需要用心经营才会有订单哦！";
                        }
                    }
                    else
                    {
                        if (vipInfo != null && !string.IsNullOrEmpty(vipInfo.CouponInfo) &&
                            vipInfo.SetoffUserId != requestParameter.UserId)
                        {
                            //rsp.Message = "此客户已是会员，无需再集客。老会员更要服务好哦！";
                        }
                        else if (vipInfo != null && vipInfo.SetoffUserId == requestParameter.UserId &&
                                 !string.IsNullOrEmpty(vipInfo.Col21) &&
                                 Convert.ToDateTime(vipInfo.Col21).AddSeconds(3) < DateTime.Now) //col21：员工集客/或者分销商集客时间
                        {
                            //rsp.Message = "此客户此前已经被您集客，无需重复集客。！";
                        }
                    }
                    if (UserStatus.Trim().Equals("-1"))
                    {
// 当前会员的集客员工离职时
                        if (!string.IsNullOrEmpty(tt))
                        {
                            vipInfo.CouponInfo = tt; //设为门店
                            vipInfo.SetoffUserId = requestParameter.UserId; //设为门店员工
                            vipInfo.Col21 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //集客时间*****
                            vipInfo.Col23 = "1";
                        }
                    }

                    #endregion

                    vipBll.Update(vipInfo);

                }

                #endregion


                var responseModel = new ScanQRCodeResponseModel()
                {
                    IsSucess = true,
                    Status = status,
                    VipId = vipId
                };
                return Request.CreateResponse(HttpStatusCode.OK, responseModel);
            }
            catch (Exception ex)
            {
                var responseModel = new ScanQRCodeResponseModel()
                {
                    IsSucess = false,
                    ErrorMessage = ex.Message
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
            }
        }

        public string ToStr(string obj)
        {
            if (obj == null) return string.Empty;
            return obj.ToString();
        }

        public string GetOrderReason(EnumOrderReason orderReason)
        {
            switch (orderReason)
            {
                case EnumOrderReason.Pos:
                    return "2F6891A2194A4BBAB6F17B4C99A6C6F5";
                default:
                    throw new Exception("");
            }
        }

        public static string CombinImage(string imgBack, string destImg, string strData)
        {
            //1、上面的图片部分
            HttpWebRequest request_qrcode = (HttpWebRequest)WebRequest.Create(destImg);
            WebResponse response_qrcode = null;
            Stream qrcode_stream = null;
            response_qrcode = request_qrcode.GetResponse();
            qrcode_stream = response_qrcode.GetResponseStream();//把要嵌进去的图片转换成流


            Bitmap _bmpQrcode1 = new Bitmap(qrcode_stream);//把流转换成Bitmap
            Bitmap _bmpQrcode = new Bitmap(_bmpQrcode1, 327, 327);//缩放图片           
            //把二维码由八位的格式转为24位的
            Bitmap bmpQrcode = new Bitmap(_bmpQrcode.Width, _bmpQrcode.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb); //并用上面图片的尺寸做了一个位图
            //用上面空的位图生成了一个空的画板
            Graphics g3 = Graphics.FromImage(bmpQrcode);
            g3.DrawImageUnscaled(_bmpQrcode, 0, 0);//把原来的图片画了上去


            //2、背景部分
            HttpWebRequest request_backgroup = (HttpWebRequest)WebRequest.Create(imgBack);
            WebResponse response_keleyi = null;
            Stream backgroup_stream = null;
            response_keleyi = request_backgroup.GetResponse();
            backgroup_stream = response_keleyi.GetResponseStream();//把背景图片转换成流

            Bitmap bmp = new Bitmap(backgroup_stream);
            Graphics g = Graphics.FromImage(bmp);//生成背景图片的画板

            //3、画上文字
            //  String str = "文峰美容";
            Font font = new Font("黑体", 25);
            SolidBrush sbrush = new SolidBrush(Color.White);
            SizeF sizeText = g.MeasureString(strData, font);

            g.DrawString(strData, font, sbrush, (bmp.Width - sizeText.Width) / 2, 490);


            // g.DrawString(str, font, sbrush, new PointF(82, 490));


            g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);//又把背景图片的位图画在了背景画布上。必须要这个，否则无法处理阴影

            //4.合并图片
            g.DrawImage(bmpQrcode, 130, 118, bmpQrcode.Width, bmpQrcode.Height);

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            System.Drawing.Image newImg = Image.FromStream(ms);//生成的新的图片
            //把新图片保存下来
            string DownloadUrl = ConfigurationManager.AppSettings["website_WWW"];
            string host = DownloadUrl + "/HeadImage/";
            //创建下载根文件夹
            //var dirPath = @"C:\DownloadFile\";
            var dirPath = System.AppDomain.CurrentDomain.BaseDirectory + "HeadImage\\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            //根据年月日创建下载子文件夹
            var ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + @"\";
            host += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            //下载到本地文件
            var fileExt = Path.GetExtension(destImg).ToLower();
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";//+ fileExt;
            var filePath = dirPath + newFileName;
            host += newFileName;

            newImg.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            return host;
        }
    }
}
