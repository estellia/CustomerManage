﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.Extension;
using JIT.CPOS.Common;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.Reflection;
using JIT.Utility.Web;
using JIT.CPOS.BS.Entity.User;
using System.IO;
using System.Configuration;
using JIT.CPOS.BS.Web.ApplicationInterface.Product;
using JIT.CPOS.DTO.Base;
using JIT.CPOS.DTO.ValueObject;
using JIT.Utility.Log;
using CPOS.Common;
using JIT.CPOS.BS.Web.Base.Excel;
using Aspose.Cells;
using System.Net;
using System.Drawing;
using System.Globalization;
using JIT.Utility;

namespace JIT.CPOS.BS.Web.Module.Basic.User.Handler
{
    /// <summary>
    /// UserHandler
    /// </summary>
    public class UserHandler : JIT.CPOS.BS.Web.PageBase.JITCPOSAjaxHandler
    {
        /// <summary>
        /// 页面入口
        /// </summary>
        /// <param name="pContext"></param>
        protected override void AjaxRequest(HttpContext pContext)
        {
            string content = "";
            switch (pContext.Request.QueryString["method"])
            {
                case "search_user":
                    content = GetUserListData();
                    break;
                case "get_user_by_id":
                    content = GetUserInfoByIdData();
                    break;
                case "get_user_role_info_by_user_id":
                    content = GetUserRoleInfoByUserIdData();
                    break;
                case "user_save":
                    content = SaveUserData();
                    break;
                case "user_delete":  //修改状态
                    content = DeleteData();
                    break;
                case "user_delete2"://物理删除
                    content = DeleteData2();
                    break;
                case "revertPassword":
                    content = RevertPassword();
                    break;
                case "DownloadQRCode"://下载员工固定二维码
                    DownloadQRCode();
                    break;
                case "ImportUser":
                    content = ImportUser();
                    break;
                case "DownloadQRCodeNew":  //员工二维码，新的
                    DownloadQRCodeNew();
                    break;

            }
            pContext.Response.Write(content);
            pContext.Response.End();
        }

        #region GetUserListData
        /// <summary>
        /// 查询用户
        /// </summary>
        public string GetUserListData()
        {
            var form = Request("form").DeserializeJSONTo<UserQueryEntity>();
            var responseData = new ResponseData();

            LoggingSessionInfo loggingSessionInfo = null;
            if (CurrentUserInfo != null)
            {
                loggingSessionInfo = CurrentUserInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request("CustomerID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少商户标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                 responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else
                {
                    loggingSessionInfo = Default.GetBSLoggingSession(Request("CustomerID"), Request("CustomerUserID"));
                }
            }

            var userService = new cUserService(loggingSessionInfo);
            UserInfo data;
            string content = string.Empty;

            string user_code = form.user_code == null ? string.Empty : form.user_code;
            string user_name = form.user_name == null ? string.Empty : form.user_name;
            string user_tel = form.user_tel == null ? string.Empty : form.user_tel;
            string user_status = form.user_status == null ? string.Empty : form.user_status;
            string para_unit_id = form.unit_id ?? "";
            string role_id = form.role_id ?? "";
            //int maxRowCount = PageSize;
            int maxRowCount = Utils.GetIntVal(Request("limit"));
            int startRowIndex = Utils.GetIntVal(Request("start"));
            string NameOrPhone = form.NameOrPhone == null ? string.Empty : form.NameOrPhone;

            string key = string.Empty;
            if (Request("id") != null && Request("id") != string.Empty)
            {
                key = Request("id").ToString().Trim();
            }
          
            data = userService.SearchUserListByUnitID(   //SearchUserListByUnitID
                user_code,
                user_name,
                user_tel,
                user_status,
                maxRowCount,
                startRowIndex,
                CurrentUserInfo==null?"": CurrentUserInfo.CurrentUserRole.UnitId, para_unit_id, role_id,NameOrPhone);

            var jsonData = new JsonData();
            jsonData.totalCount = data.ICount.ToString();
            jsonData.data = data.UserInfoList;

            content = string.Format("{{\"success\":{2},\"msg\":{3},\"totalCount\":{1},\"topics\":{0}}}",
                data.UserInfoList.ToJSON(),
                data.ICount
                ,1,"\"\"");
            return content;
        }
        #endregion

        #region GetUserInfoByIdData
        /// <summary>
        /// 通过ID获取用户信息
        /// </summary>
        public string GetUserInfoByIdData()
        {
            var responseData = new ResponseData();
            LoggingSessionInfo loggingSessionInfo = null;
            if (CurrentUserInfo != null)
            {
                loggingSessionInfo = CurrentUserInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request("CustomerID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少商户标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else
                {
                    loggingSessionInfo = Default.GetBSLoggingSession(Request("CustomerID"), Request("CustomerUserID"));
                }
            }

            var userService = new cUserService(loggingSessionInfo);//使用兼容模式
            UserInfo data;
            string content = string.Empty;

            string key = string.Empty;
            if (Request("user_id") != null && Request("user_id") != string.Empty)
            {
                key = Request("user_id").ToString().Trim();
            }

            data = userService.GetUserById(CurrentUserInfo, key);
            if (data != null)
            {
                data.userRoleInfoList = userService.GetUserRoles(key);
            }

            var jsonData = new JsonData();
            jsonData.totalCount = "1";
            jsonData.data = data;
            jsonData.success = true;
            jsonData.msg = "";

            content = jsonData.ToJSON();
            return content;
        }
        #endregion

        #region GetUserRoleInfoByUserIdData
        /// <summary>
        /// 通过ID获取用户角色信息
        /// </summary>
        public string GetUserRoleInfoByUserIdData()
        {
            var responseData = new ResponseData();
            LoggingSessionInfo loggingSessionInfo = null;
            if (CurrentUserInfo != null)
            {
                loggingSessionInfo = CurrentUserInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request("CustomerID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少商户标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else
                {
                    loggingSessionInfo = Default.GetBSLoggingSession(Request("CustomerID"), Request("CustomerUserID"));
                }
            }

            var userService = new cUserService(loggingSessionInfo);//使用兼容模式
            UserRoleInfo data = new UserRoleInfo();
            string content = string.Empty;

            string key = string.Empty;
            if (Request("user_id") != null && Request("user_id") != string.Empty)
            {
                key = Request("user_id").ToString().Trim();
            }

            data.UserRoleInfoList = userService.GetUserRoles(key);
            if (data.UserRoleInfoList == null) data.UserRoleInfoList = new List<UserRoleInfo>();

            var jsonData = new JsonData();
            jsonData.totalCount = data.UserRoleInfoList.Count.ToString();
            jsonData.data = data.UserRoleInfoList;

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.UserRoleInfoList.ToJSON(),
                data.UserRoleInfoList.Count);
            return content;
        }
        #endregion

        #region SaveUserData
        /// <summary>
        /// 保存用户
        /// </summary>
        public string SaveUserData()
        {
            var responseData = new ResponseData();
            LoggingSessionInfo loggingSessionInfo = null;
            if (CurrentUserInfo != null)
            {
                loggingSessionInfo = CurrentUserInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request("CustomerID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少商户标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else
                {
                    loggingSessionInfo = Default.GetBSLoggingSession(Request("CustomerID"), Request("CustomerUserID"));
                }
            }



            var userService = new cUserService(loggingSessionInfo);//兼容模式
            UserInfo user = new UserInfo();
            string content = string.Empty;
            string error = "";
          //  var responseData = new ResponseData();

            string key = string.Empty;
            string user_id = string.Empty;
            if (Request("user") != null && Request("user") != string.Empty)
            {
                key = Request("user").ToString().Trim();
            }
            if (Request("user_id") != null && Request("user_id") != string.Empty)
            {
                user_id = Request("user_id").ToString().Trim();
            }

            user = key.DeserializeJSONTo<UserInfo>();

            if (userService.IsExistUserCode(user.User_Code, loggingSessionInfo, user_id))//使用兼容模式
            {
                responseData.success = false;
                responseData.msg = "用户名已存在！";
                return responseData.ToJSON();
            }
            if (user.User_Status == null || user.User_Status.Trim().Length == 0)
            {
                user.User_Status = "1";
            }
            if (user_id.Trim().Length == 0 && string.IsNullOrEmpty(user.User_Password))//新增用户才需要提交密码
            {
                responseData.success = false;
                responseData.msg = "用户密码不能为空";
                return responseData.ToJSON();
            }


            if (user_id.Trim().Length == 0)
            {
                user.User_Id = Utils.NewGuid();
                //user.UnitList = loggingSessionInfo.CurrentUserRole.UnitId;
            }
            else
            {
                user.User_Id = user_id;
            }

            if (user.User_Code == null || user.User_Code.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "用户名不能为空";
                return responseData.ToJSON();
            }
            if (user.User_Name == null || user.User_Name.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "姓名不能为空";
                return responseData.ToJSON();
            }

            //if (user.Fail_Date == null || user.Fail_Date.Trim().Length == 0)
            //{
            //    responseData.success = false;
            //    responseData.msg = "用户有效日期不能为空";
            //    return responseData.ToJSON();
            //}
            user.Fail_Date = "2030-12-30";//转换成最大的日期
            //if (user.User_Telephone == null || user.User_Telephone.Trim().Length == 0)
            //{
            //    responseData.success = false;
            //    responseData.msg = "用户手机不能为空";
            //    return responseData.ToJSON();
            //}
            //if (user.User_Email == null || user.User_Email.Trim().Length == 0)
            //{
            //    responseData.success = false;
            //    responseData.msg = "用户邮箱不能为空";
            //    return responseData.ToJSON();
            //}
            if (user.userRoleInfoList == null || user.userRoleInfoList.Count == 0)
            {
                responseData.success = false;
                responseData.msg = "请添加角色配置";
                return responseData.ToJSON();
            }
            //设为归属单位有且只能有一个***
            int countDefaultFlag = user.userRoleInfoList.Where(p => p.DefaultFlag == 1).Count();
            if (countDefaultFlag < 1)
            {
                responseData.success = false;
                responseData.msg = "必须设置一个单位为归属单位";
                return responseData.ToJSON();
            }
            if (countDefaultFlag > 1)
            {
                responseData.success = false;
                responseData.msg = "只能设置一个单位为默认单位";
                return responseData.ToJSON();
            }
            UserRoleInfo roleinfo = user.userRoleInfoList.Where(p => p.DefaultFlag == 1).ToArray()[0];
            t_unitBLL t_unitBLL = new BLL.t_unitBLL(loggingSessionInfo);//使用兼容模式
            t_unitEntity UnitEn = t_unitBLL.GetByID(roleinfo.UnitId);
            string unitName = "";
            if (UnitEn != null && UnitEn.unit_name != null)
            {
                unitName = UnitEn.unit_name;
            }
            //增加用户标识
            foreach (var userRoleItem in user.userRoleInfoList)
            {
                userRoleItem.UserId = user.User_Id;
            }

            user.Create_Time = Utils.GetNow();
            user.Create_User_Id = loggingSessionInfo.CurrentUser.User_Id;//使用兼容模式
            user.Modify_Time = Utils.GetNow();
            user.Modify_User_Id = loggingSessionInfo.CurrentUser.User_Id;//使用兼容模式

            userService.SetUserInfo(user, user.userRoleInfoList, out error);

            #region  生成员工二维码
            /**
          //微信 公共平台
            var wapentity = new WApplicationInterfaceBLL(CurrentUserInfo).QueryByEntity(new WApplicationInterfaceEntity
            {
                CustomerId = CurrentUserInfo.ClientID,
                IsDelete = 0
            }, null).FirstOrDefault();//取默认的第一个微信

            var QRCodeId = Guid.NewGuid();
            var QRCodeManagerentity = new WQRCodeManagerBLL(this.CurrentUserInfo).QueryByEntity(new WQRCodeManagerEntity
                {
                    ObjectId = user.User_Id
                }, null).FirstOrDefault();
            if (QRCodeManagerentity != null)
            {
                QRCodeId = (Guid)QRCodeManagerentity.QRCodeId;
            }
            if (QRCodeManagerentity == null)
            {
                //二维码类别
                var wqrentity = new WQRCodeTypeBLL(this.CurrentUserInfo).QueryByEntity(
                    new WQRCodeTypeEntity { TypeCode = "UserQrCode" }
                    , null).FirstOrDefault();
                if (wqrentity == null)
                {
                    responseData.success = false;
                    responseData.msg = "无法获取员工二维码类别";
                    return responseData.ToJSON();
                }
                //生成了微信二维码
                   var wxCode = CretaeWxCode();               
                   //如果名称不为空，就把图片放在一定的背景下面
                   if (!string.IsNullOrEmpty(user.User_Name))
                   {
                           string apiDomain = ConfigurationManager.AppSettings["original_url"];
                           wxCode.ImageUrl = CombinImage(apiDomain + @"/HeadImage/qrcodeBack.jpg", wxCode.ImageUrl, unitName + "-" + user.User_Name);
                   }

                var WQRCodeManagerbll = new WQRCodeManagerBLL(CurrentUserInfo);

            //    Guid QRCodeId = Guid.NewGuid();

                if (!string.IsNullOrEmpty(wxCode.ImageUrl))
                {
                    WQRCodeManagerbll.Create(new WQRCodeManagerEntity
                    {
                        QRCodeId = QRCodeId,
                        QRCode = wxCode.MaxWQRCod.ToString(),
                        QRCodeTypeId = wqrentity.QRCodeTypeId,
                        IsUse = 1,
                        ObjectId = user.User_Id,
                        CreateBy = CurrentUserInfo.UserID,
                        ApplicationId = wapentity.ApplicationId,
                        IsDelete = 0,
                        ImageUrl = wxCode.ImageUrl,
                        CustomerId = CurrentUserInfo.ClientID

                    });
                }


            }

    

            **/
            #endregion

            string errorMsg = "";
            //下载的时候再生成,这里就不生成了，业务太多
       //     string wxCodeImageUrl = CreateUserWxCode(user, unitName,loggingSessionInfo, out errorMsg);
            if (errorMsg != "")
            {
                responseData.success = false;
                responseData.msg = errorMsg;
                return responseData.ToJSON();
            }


            responseData.success = true;
            responseData.msg = error;


            content = responseData.ToJSON();
            return content;
        }
        #endregion


        //生成员工二维码
        public string CreateUserWxCode(UserInfo user, string unitName,LoggingSessionInfo loggingSessionInfo ,out string errorMsg)
        {
            errorMsg = "";
            #region  生成员工二维码
            //微信 公共平台
        //兼容模式
            var wapentity = new WApplicationInterfaceBLL(loggingSessionInfo).QueryByEntity(new WApplicationInterfaceEntity
            {
                CustomerId = loggingSessionInfo.ClientID,
                IsDelete = 0
            }, null).FirstOrDefault();//取默认的第一个微信

            var QRCodeId = Guid.NewGuid();
            var QRCodeManagerentity = new WQRCodeManagerBLL(loggingSessionInfo).QueryByEntity(new WQRCodeManagerEntity
            {
                ObjectId = user.User_Id
            }, null).FirstOrDefault();
            if (QRCodeManagerentity != null)
            {
                QRCodeId = (Guid)QRCodeManagerentity.QRCodeId;
            }
            if (QRCodeManagerentity == null)
            {
                //二维码类别
                var wqrentity = new WQRCodeTypeBLL(loggingSessionInfo).QueryByEntity(
                    new WQRCodeTypeEntity { TypeCode = "UserQrCode" }
                    , null).FirstOrDefault();
                if (wqrentity == null)
                {
                    //responseData.success = false;
                    //responseData.msg = "无法获取员工二维码类别";
                    //return responseData.ToJSON();
                    errorMsg = "无法获取员工二维码类别";
                }
                //生成了微信二维码
                var wxCode = CretaeWxCode(loggingSessionInfo);//***
                //如果名称不为空，就把图片放在一定的背景下面
                if (!string.IsNullOrEmpty(user.User_Name) && wxCode.success == true)
                {
                    try
                    {
                        string apiDomain = ConfigurationManager.AppSettings["original_url"];
                        wxCode.ImageUrl = CombinImage(apiDomain + @"/HeadImage/qrcodeBack.jpg", wxCode.ImageUrl, unitName + "-" + user.User_Name);
                    }
                    catch
                    {
                        errorMsg = "生成二维码失败";
                    }
                }
                else {
                    errorMsg = "生成二维码失败";
                }

                var WQRCodeManagerbll = new WQRCodeManagerBLL(loggingSessionInfo);//兼容模式

                //    Guid QRCodeId = Guid.NewGuid();

                if (!string.IsNullOrEmpty(wxCode.ImageUrl))
                {
                    WQRCodeManagerbll.Create(new WQRCodeManagerEntity
                    {
                        QRCodeId = QRCodeId,
                        QRCode = wxCode.MaxWQRCod.ToString(),
                        QRCodeTypeId = wqrentity.QRCodeTypeId,
                        IsUse = 1,
                        ObjectId = user.User_Id,
                        CreateBy = loggingSessionInfo.UserID,//兼容模式
                        ApplicationId = wapentity.ApplicationId,
                        IsDelete = 0,
                        ImageUrl = wxCode.ImageUrl,
                        CustomerId = loggingSessionInfo.ClientID

                    });
                }

                return wxCode.ImageUrl;//生成新的二维码地址

            }
            else
            {
                return QRCodeManagerentity.ImageUrl;
            }




            #endregion
        }


        #region new 生成活动二维码
        public WxCode CretaeWxCode(LoggingSessionInfo loggingSessionInfo)//用了兼容模式
        {
            var responseData = new WxCode();
            responseData.success = false;
            responseData.msg = "二维码生成失败!";
         //   var loggingSessionInfo = CurrentUserInfo;
            try
            {
                //微信 公共平台
                var wapentity = new WApplicationInterfaceBLL(loggingSessionInfo).QueryByEntity(new WApplicationInterfaceEntity
                {

                    CustomerId = loggingSessionInfo.ClientID,
                    IsDelete = 0

                }, null).FirstOrDefault();

                //获取当前二维码 最大值
                var MaxWQRCod = new WQRCodeManagerBLL(loggingSessionInfo).GetMaxWQRCod() + 1;

                if (wapentity == null)
                {
                    responseData.success = false;
                    responseData.msg = "无设置微信公众平台!";
                    return responseData;
                }

                string imageUrl = string.Empty;

                #region 生成二维码
                JIT.CPOS.BS.BLL.WX.CommonBLL commonServer = new JIT.CPOS.BS.BLL.WX.CommonBLL();
                imageUrl = commonServer.GetQrcodeUrl(wapentity.AppID.ToString().Trim()
                                                          , wapentity.AppSecret.Trim()
                                                          , "1", MaxWQRCod
                                                          , loggingSessionInfo);

                if (!string.IsNullOrEmpty(imageUrl))
                {

                    string host = ConfigurationManager.AppSettings["DownloadImageUrl"];
                    CPOS.Common.DownloadImage downloadServer = new DownloadImage();
                    imageUrl = downloadServer.DownloadFile(imageUrl, host);
                }
                #endregion
                responseData.success = true;
                responseData.msg = "二维码生成成功!";
                responseData.ImageUrl = imageUrl;
                responseData.MaxWQRCod = MaxWQRCod;


                return responseData;
            }
            catch (Exception ex)
            {
                //throw new APIException(ex.Message);
                return responseData;
            }

        }
        #endregion

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
            System.Drawing.Font font = new System.Drawing.Font("黑体", 25);
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



        #region DeleteData
        /// <summary>
        ///改变状态
        /// </summary>
        public string DeleteData()
        {
            var responseData = new ResponseData();
            LoggingSessionInfo loggingSessionInfo = null;
            if (CurrentUserInfo != null)
            {
                loggingSessionInfo = CurrentUserInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request("CustomerID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少商户标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else
                {
                    loggingSessionInfo = Default.GetBSLoggingSession(Request("CustomerID"), Request("CustomerUserID"));
                }
            }


            var service = new cUserService(loggingSessionInfo);//使用兼容模式

            string content = string.Empty;
            string error = "";
        //    var responseData = new ResponseData();

            string key = string.Empty;
            if (FormatParamValue(Request("ids")) != null && FormatParamValue(Request("ids")) != string.Empty)
            {
                key = FormatParamValue(Request("ids")).ToString().Trim();
            }

            if (key == null || key.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "ID不能为空";
                return responseData.ToJSON();
            }

            var status = "-1";
            if (FormatParamValue(Request("status")) != null && FormatParamValue(Request("status")) != string.Empty)
            {
                status = FormatParamValue(Request("status")).ToString().Trim();
            }

            string[] ids = key.Split(',');
            foreach (var id in ids)
            {
                service.SetUserStatus(key, status, loggingSessionInfo);//使用兼容模式
            }

            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }
        #endregion

        public string DeleteData2()
        {
            var responseData = new ResponseData();
            LoggingSessionInfo loggingSessionInfo = null;
            if (CurrentUserInfo != null)
            {
                loggingSessionInfo = CurrentUserInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request("CustomerID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少商户标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
                else
                {
                    loggingSessionInfo = Default.GetBSLoggingSession(Request("CustomerID"), Request("CustomerUserID"));
                }
            }

            var service = new cUserService(loggingSessionInfo);//使用兼容模式

            string content = string.Empty;
            string error = "";
         //   var responseData = new ResponseData();

            string key = string.Empty;
            if (FormatParamValue(Request("ids")) != null && FormatParamValue(Request("ids")) != string.Empty)
            {
                key = FormatParamValue(Request("ids")).ToString().Trim();
            }

            if (key == null || key.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "ID不能为空";
                return responseData.ToJSON();
            }


            string[] ids = key.Split(',');
            foreach (var id in ids)
            {
                //  service.SetUserStatus(key, status, CurrentUserInfo);
                service.physicalDeleteUser(id);
            }

            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }

        #region RevertPassword
        public string RevertPassword()
        {
            string user_id = Request("user");
            var responseData = new ResponseData();
            LoggingSessionInfo loggingSessionInfo = null;
            if (CurrentUserInfo != null)
            {
                loggingSessionInfo = CurrentUserInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request("CustomerID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少商户标识";
                    return responseData.ToString();
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                    return responseData.ToString();
                }
             
                else
                {
                    loggingSessionInfo = Default.GetBSLoggingSession(Request("CustomerID"), Request("CustomerUserID"));
                }
            }


            string error = "";
         //   var responseData = new ResponseData();
            try
            {
                UserInfo user = new UserInfo();
                var userService = new cUserService(loggingSessionInfo);//使用兼容模式
                userService.SetUserPwd(loggingSessionInfo, user_id, MD5Helper.Encryption(Request("password")), out error);//使用兼容模式
                responseData.success = true;
            }
            catch (Exception)
            {
                responseData.success = false;
                responseData.msg = "密码重置失败";
            }
            return responseData.ToJSON();
        }
        #endregion

        /// <summary>
        /// 下载员工固定二维码
        /// </summary>
        /// <returns></returns>
        private void DownloadQRCode()
        {
            //员工固定二维码磁盘路径
            string targetPath = ConfigurationManager.AppSettings["DiskImagePath"];
            string currentDomain = this.CurrentContext.Request.Url.Host;
            string user_id = user_id = Request("user_id").ToString().Trim();
            string imageName = string.Empty;//图片名称
            string imagePath = string.Empty;//图片路径
            //请求参数
            string pQueryString = "/ApplicationInterface/Stores/StoresGateway.ashx?type=Product&action=getDimensionalCode&req={\"UserID\":\"" + user_id + "\",\"Parameters\":{\"unitId\":\"\",\"VipDCode\":9},\"CustomerID\":\"" + CurrentUserInfo.ClientID + "\",\"OpenID\":\"\",\"JSONP\":\"\",\"Locale\":1,\"Token\":\"\"}";
            var rsp = APIClientProxy.CallAPI(pQueryString, "");
            getDimensionalCodeRespData qrInfo = JsonHelper.JsonDeserialize<getDimensionalCodeRespData>(rsp);
            try
            {
                imageName = qrInfo.Data.imageUrl.Substring(qrInfo.Data.imageUrl.LastIndexOf("/"));
                imagePath = imageName.Substring(1, 8) + imageName;
                imagePath = targetPath + imagePath;
                //要下载的文件名
                FileInfo DownloadFile = new FileInfo(imagePath);
                if (DownloadFile.Exists)
                {
                    CurrentContext.Response.Clear();
                    CurrentContext.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + user_id + ".jpg" + "\"");
                    CurrentContext.Response.AddHeader("Content-Length", DownloadFile.Length.ToString());
                    CurrentContext.Response.ContentType = "application/octet-stream";
                    CurrentContext.Response.TransmitFile(DownloadFile.FullName);
                    CurrentContext.Response.Flush();
                }
                else
                    Loggers.Debug(new DebugLogInfo() { Message = "二维码未找到" });
            }
            catch (Exception ex)
            {
                Loggers.Debug(new DebugLogInfo() { Message = "二维码ii:" + ex.Message });
                CurrentContext.Response.ContentType = "text/plain";
                CurrentContext.Response.Write(ex.Message);
            }
            finally
            {
                CurrentContext.Response.End();
            }
        }

        private void DownloadQRCodeNew()//新的下载二维码的方法
        {
            //string weixinDomain = ConfigurationManager.AppSettings["original_url"];
            //string sourcePath = this.CurrentContext.Server.MapPath("/QRCodeImage/qrcode.jpg");
            //string targetPath = this.CurrentContext.Server.MapPath("/QRCodeImage/");
            //string currentDomain = this.CurrentContext.Request.Url.Host;
            //string itemId = FormatParamValue(Request("item_id"));//商品ID
            //string itemName = FormatParamValue(Request("item_name"));//商品名
            //string imageURL;

            //ObjectImagesBLL objectImagesBLL = new ObjectImagesBLL(CurrentUserInfo);
            ////查找是否已经生成了二维码
            //ObjectImagesEntity[] objectImagesEntityArray = objectImagesBLL.QueryByEntity(new ObjectImagesEntity() { ObjectId = itemId, Description = "自动生成的产品二维码" }, null);

            //if (objectImagesEntityArray.Length == 0)
            //{
            //    //http://api.dev.chainclouds.com
            //    //    http://api.dev.chainclouds.com/WXOAuth/AuthUniversal.aspx?customerId=049b0a8f641f4ca7b17b0b7b6291de1f&applicationId=1D7A01FC1E7D41ECBAC2696D0D363315&goUrl=api.dev.chainclouds.com/HtmlApps/html/public/shop/goods_detail.html?rootPage=true&rootPage=true&goodsId=DBF5326F4C5B4B0F8508AB54B0B0EBD4&ver=1448273310707&scope=snsapi_userinfo

            //    string itemUrl = weixinDomain + "/WXOAuth/AuthUniversal.aspx?customerId=" + CurrentUserInfo.ClientID
            //        + "&goUrl=" + weixinDomain + "/HtmlApps/html/public/shop/goods_detail.html?goodsId="
            //        + itemId + "&scope=snsapi_userinfo";

            //    //  string itemUrl = "http://localhost:1950/" + "/WXOAuth/AuthUniversal.aspx?customerId=" + CurrentUserInfo.ClientID
            //    //      + "&goUrl=" + weixinDomain + "/HtmlApps/html/public/shop/goods_detail.html?rootPage=true&rootPage=true&goodsId="
            //    //      + itemId + "&scope=snsapi_userinfo";
            //    ////原来的老页面  weixinDomain + "/HtmlApps/Auth.html?pageName=GoodsDetail&rootPage=true&customerId=" + CurrentUserInfo.ClientID + "&goodsId=" + itemId
            //   imageURL = Utils.GenerateQRCode(itemUrl, currentDomain, sourcePath, targetPath);
            //    //把下载下来的图片的地址存到ObjectImages
            //    objectImagesBLL.Create(new ObjectImagesEntity()
            //    {
            //        ImageId = Utils.NewGuid(),
            //        CustomerId = CurrentUserInfo.ClientID,
            //        ImageURL = imageURL,
            //        ObjectId = itemId,
            //        Title = itemName
            //        ,
            //        Description = "自动生成的产品二维码"
            //    });

            //    Loggers.Debug(new DebugLogInfo() { Message = "二维码已生成，imageURL:" + imageURL });
            //}
            //else
            //{
            //    imageURL = objectImagesEntityArray[0].ImageURL;
            //}

            //string imagePath = targetPath + imageURL.Substring(imageURL.LastIndexOf("/"));
            //Loggers.Debug(new DebugLogInfo() { Message = "二维码路径，imagePath:" + imageURL });

            var responseData = new ResponseData();
            LoggingSessionInfo loggingSessionInfo = null;
            if (CurrentUserInfo != null)
            {
                loggingSessionInfo = CurrentUserInfo;
            }
            else
            {
                if (string.IsNullOrEmpty(Request("CustomerID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少商户标识";
                 //   return responseData.ToString();
                    throw new Exception("缺少商户标识");
                }
                else if (string.IsNullOrEmpty(Request("CustomerUserID")))
                {
                    responseData.success = false;
                    responseData.msg = "缺少登陆员工的标识";
                   // return responseData.ToString();
                    throw new Exception("缺少登陆员工的标识");
                }            
                else
                {
                    loggingSessionInfo = Default.GetBSLoggingSession(Request("CustomerID"), Request("CustomerUserID"));
                }
            }




            //需要有一个targetPath？？？明天测试一下//参考上面的 方法
            string user_id = user_id = Request("user_id").ToString().Trim();
            //根据id获取到员工信息
            var service = new cUserService(loggingSessionInfo);
            UserInfo user = service.GetUserById(loggingSessionInfo, user_id);//兼容模式
            string errorMsg = "";

            string wxCodeImageUrl = CreateUserWxCode(user, user.UnitName,loggingSessionInfo, out errorMsg);
            //if (errorMsg != "")
            //{
            //    responseData.success = false;
            //    responseData.msg = errorMsg;
            //    return responseData.ToJSON();
            //}

            var dirPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var imageName = wxCodeImageUrl.Substring(wxCodeImageUrl.IndexOf("HeadImage")).Replace("/", @"\");
            var imagePath = dirPath + imageName;//整个
            try
            {
                //要下载的文件名
                FileInfo DownloadFile = new FileInfo(imagePath);  //imagePath原来是这个，明天试试

                if (DownloadFile.Exists)
                {
                    CurrentContext.Response.Clear();
                    CurrentContext.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + System.Web.HttpUtility.UrlEncode(user.User_Name, System.Text.Encoding.UTF8) + ".jpg" + "\"");
                    CurrentContext.Response.AddHeader("Content-Length", DownloadFile.Length.ToString());
                    CurrentContext.Response.ContentType = "application/octet-stream";
                    CurrentContext.Response.TransmitFile(DownloadFile.FullName);
                    CurrentContext.Response.Flush();
                }
                else
                    Loggers.Debug(new DebugLogInfo() { Message = "二维码未找到" });
            }
            catch (Exception ex)
            {
                CurrentContext.Response.ContentType = "text/plain";
                CurrentContext.Response.Write(ex.Message);
            }
            finally
            {
                CurrentContext.Response.End();
            }
        }


        //下载员工二维码 新




        #region 导入用户
        public string ImportUser()
        {
            var responseData = new ResponseData();
            var userService = new cUserService(CurrentUserInfo);
            ExcelHelper excelHelper = new ExcelHelper();
            if (Request("filePath") != null && Request("filePath").ToString() != "")
            {
                try
                {
                    var rp = new ImportRP();
                    string strPath = Request("filePath").ToString();
                    string strFileName = string.Empty;
                    DataSet ds = userService.ExcelToDb(HttpContext.Current.Server.MapPath(strPath), CurrentUserInfo);
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {

                        Workbook wb = JIT.Utility.DataTableExporter.WriteXLS(ds.Tables[0], 0);
                        string savePath = HttpContext.Current.Server.MapPath(@"~/File/ErrFile/User");
                        if (!System.IO.Directory.Exists(savePath))
                        {
                            System.IO.Directory.CreateDirectory(savePath);
                        }
                        strFileName = "\\用户错误信息导出" + DateTime.Now.ToFileTime() + ".xls";
                        savePath = savePath + strFileName;
                        wb.Save(savePath);//保存Excel文件   
                        rp = new ImportRP()
                        {
                            Url = "/File/ErrFile/User" + strFileName,
                            TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            ErrCount = Convert.ToInt32(ds.Tables[1].Rows[0]["ErrCount"].ToString())
                        };
                    }
                    else
                    {
                        rp = new ImportRP()
                        {
                            Url = "",
                            TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"].ToString()),
                            ErrCount = Convert.ToInt32(ds.Tables[1].Rows[0]["ErrCount"].ToString())
                        };

                        responseData.success = true;
                    }
                    responseData.success = true;
                    responseData.data = rp;
                }
                catch (Exception err)
                {
                    responseData.success = false;
                    responseData.msg = err.Message.ToString();
                }
            }
            return responseData.ToJSON();

        }
        #endregion
    }



    #region QueryEntity
    public class UserQueryEntity
    {
        public string user_code;
        public string user_name;
        public string user_tel;
        public string user_status;
        public string unit_id;
        public string role_id;
        public string NameOrPhone;
        
    }

    public class WxCode
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public string ImageUrl { get; set; }
        public int MaxWQRCod { get; set; }
    }



    //成员工固定二维码返回参数 copy by Henry 2015-4-27
    public class getDimensionalCodeRespData
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public getDimensionalCodeRespContentData Data { get; set; }
    }
    public class getDimensionalCodeRespContentData
    {
        public string imageUrl { get; set; }
        public string paraTmp { get; set; }
    }
    public class ImportRP
    {
        public string Url { get; set; }
        public int TotalCount { get; set; }
        public int ErrCount { get; set; }
    }
    #endregion

}