﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.Extension;
using JIT.CPOS.Common;
using JIT.Utility.ExtensionMethod;
using JIT.Utility.Reflection;
using JIT.Utility.Web;
using System.Data;
using System.Configuration;
using JIT.Utility.Log;

namespace JIT.CPOS.BS.Web.Module2.BaseData.ItemCategory.Handler
{
    /// <summary>
    /// 商品分类的后台处理
    /// </summary>
    public class ItemCategoryHandler : JIT.CPOS.BS.Web.PageBase.JITCPOSAjaxHandler
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
                case "toggleStatus":
                    content = this.ToggoleItemCategoryStatus();
                    break;
                case "getAll":
                    content = this.GetAllItemCategories();
                    break;
                case "update":
                    content = this.UpdateItemCategory();
                    break;

                case "add":
                    content = this.UpdateItemCategory();
                    break;
                case "GetALDByCategoryId":
                    content = this.GetALDByCategoryId();
                    break;
            }
            pContext.Response.Write(content);
            pContext.Response.End();
        }

        #region 操作
        /// <summary>
        /// 切换商品分类状态
        /// </summary>
        protected string ToggoleItemCategoryStatus()
        {
            // string res = "{success:false}";
            var rsp = new ResponseData();
            string checkRes = "";

            var bll = new ItemCategoryService(this.CurrentUserInfo);
            var id = this.Request("id");
            var status = this.Request("status");
            var bat_id = this.Request("bat_id");
            if (!string.IsNullOrWhiteSpace(id))
            {
                var bllItem = new ItemService(this.CurrentUserInfo);
                if (bllItem.GetItemCountByCategory(id, bat_id) > 0)
                {
                    rsp.success = false;
                    rsp.msg = "该" + bat_id == "1" ? "分类" : "分组" + "下有商品，请先处理商品状态";
                }
                else
                {
                    bll.SetItemCategoryStatus(CurrentUserInfo, id, status, out checkRes);

                    if (!string.IsNullOrEmpty(checkRes))
                    {
                        rsp.success = false;
                        rsp.msg = checkRes;
                    }
                    else
                    {
                        rsp.success = true;
                    }
                }
                #region 原来逻辑  20160103 wujx 屏蔽
                //if (bllItem.SearchItemList(null, null, id, null, null, null, 1, 0) != null)//判断该分类下是否有商品
                //{
                //    bll.SetItemCategoryStatus(CurrentUserInfo, id, status, out checkRes);

                //    if (!string.IsNullOrEmpty(checkRes))
                //    {
                //        // res = "{success:false,msg:\"" + checkRes + "\"}";
                //        rsp.success = false;
                //        rsp.msg = checkRes;
                //    }
                //    else
                //    {
                //        // res = "{success:true}";
                //        rsp.success = true;
                //    }
                //}
                //else
                //{
                //    rsp.success = false;
                //    rsp.msg = "该分类下有商品，请先处理商品状态";
                //}
                #endregion
            }
            return rsp.ToJSON();
        }

        /// <summary>
        /// 根据类别ID获取阿拉丁分类
        /// </summary>
        /// <returns></returns>
        protected string GetALDByCategoryId()
        {
            var data = this.DeserializeJSONContent<ItemCategoryInfo>();
            //同步到ALDCategoryID分类 data.CustomerID，      data.Item_Category_Id. data.ALDCategoryID
            var url = ConfigurationManager.AppSettings["ALDApiURL"].ToString() + "/Gateway.ashx";
            var request = new ItemCategory2ALDRequest()
            {
                Parameters = data
            };
            var res = new MallALDCategoryEntity();//
            var resstr = "";
            try
            {
                resstr = JIT.Utility.Web.HttpClient.GetQueryString(url, string.Format("Action=GetALDByCategoryId&ReqContent={0}", request.ToJSON()));
                Loggers.Debug(new DebugLogInfo() { Message = "调用获取ALD类别接口:" + resstr });
                //   res = resstr.DeserializeJSONTo<MallALDCategoryEntity>();
            }
            catch (Exception ex)
            {
                Loggers.Exception(new ExceptionLogInfo(ex));
                throw new Exception("调用ALD平台失败:" + ex.Message);
            }




            return resstr;
        }



        /// <summary>
        /// 获取所有的商品分类
        /// </summary>
        /// <returns></returns>
        protected string GetAllItemCategories()
        {
            var bll = new ItemCategoryService(this.CurrentUserInfo);
            var list = bll.GetItemCagegoryList("", "");
            return list.ToJSON();
        }

        /// <summary>
        /// 判断商品 品类 名称是否存在
        /// </summary>
        /// <param name="CatelogId">品类标志</param>
        /// <param name="CatelogName">名称</param>
        /// <param name="models">商户所有品类集合</param>
        /// <returns>
        /// true 已经存在 不能执行添加{修改}操作
        /// false  可以执行相应逻辑
        /// </returns>
        public bool CheckCategoryNameIsExist(string CatelogId, string CatelogName, IList<ItemCategoryInfo> models)
        {
            return models.Where(m => m.Item_Category_Name == CatelogName.Trim() && m.Item_Category_Id != CatelogId).Count() > 0;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected string UpdateItemCategory()
        {
            var bll = new ItemCategoryService(this.CurrentUserInfo);
            var data = this.DeserializeJSONContent<ItemCategoryInfo>();
            var rsp = new ResponseData();
            if (data != null)
            {
                IList<ItemCategoryInfo> listdata = bll.GetItemCagegoryList("", "");//获取所有类别

                if (Searchtype(listdata, data.Item_Category_Id, data.Parent_Id))
                {
                    rsp.success = false;
                    rsp.msg = "上级分类不能选择自身下级";
                    return rsp.ToJSON();

                }

                if (CheckCategoryNameIsExist(data.Item_Category_Id, data.Item_Category_Name, listdata))
                {
                    rsp.success = false;
                    rsp.msg = "商品品类不能重复";
                    return rsp.ToJSON();
                }

                if (string.IsNullOrWhiteSpace(data.Item_Category_Code))
                {
                    rsp.success = false;
                    rsp.msg = "类型编码不能为空";
                    return rsp.ToJSON();
                }
                if (string.IsNullOrWhiteSpace(data.Status))
                {
                    rsp.success = false;
                    rsp.msg = "状态不能为空";
                    return rsp.ToJSON();
                }
                if (string.IsNullOrWhiteSpace(data.Parent_Id))
                {
                    rsp.success = false;
                    rsp.msg = "上级分类不能为空";
                    return rsp.ToJSON();
                }

                data.Create_User_Id = CurrentUserInfo.CurrentUser.User_Id;
                data.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                data.Create_User_Name = CurrentUserInfo.CurrentUser.User_Name;
                data.CustomerID = CurrentUserInfo.CurrentUser.customer_id;

                bll.SetItemCategoryInfo(this.CurrentUserInfo, data);
            }

            rsp.success = true;
            rsp.msg = "保存成功";

            //
            return rsp.ToJSON();
        }
        #endregion



        #region 判断是否选择自己的子类
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">所有信息</param>
        /// <param name="id">自身id</param>
        /// <param name="pid">选择的上一级id</param>
        /// <returns></returns>
        public bool Searchtype(IList<ItemCategoryInfo> list, string id, string pid)
        {
            List<ItemCategoryInfo> Itemlist = list.Where(op => op.Parent_Id == id).ToList();
            if (Itemlist != null)
            {
                foreach (ItemCategoryInfo item in Itemlist)
                {
                    if (item.Item_Category_Id == pid)
                    {
                        return true;
                    }
                    if (SonSearchtype(list, item.Item_Category_Id, pid))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">所有信息</param>
        /// <param name="id">自身id</param>
        /// <param name="pid">选择的上一级id</param>
        /// <returns></returns>
        public bool SonSearchtype(IList<ItemCategoryInfo> list, string id, string pid)
        {
            List<ItemCategoryInfo> Itemlist = list.Where(op => op.Parent_Id == id).ToList();
            if (Itemlist != null)
            {
                foreach (ItemCategoryInfo item in Itemlist)
                {
                    if (item.Item_Category_Id == pid)
                    {
                        return true;
                    }
                    SonSearchtype(list, item.Item_Category_Id, pid);
                }
            }
            return false;
        }
        #endregion
    }
    public partial class MallALDCategoryEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public MallALDCategoryEntity()
        {
        }
        #endregion

        #region 属性集
        /// <summary>
        /// 
        /// </summary>
        public Guid? CategoryID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String CategoryName { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? ParentID { get; set; }

        /// <summary>
        /// 图片URL
        /// </summary>
        public String ImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? DisPlayIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String CreateBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String LastUpdateBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32? IsDelete { get; set; }


        #endregion

    }

    public class ItemCategory2ALDRequest
    {
        public int? Locale { get; set; }
        public Guid? UserID { get; set; }
        public int? BusinessZoneID { get; set; }
        public string Token { get; set; }
        public object Parameters { get; set; }
    }
    public class ItemCategory2ALDResponse
    {
        public int? ResultCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public bool IsSuccess()
        {
            if (this.ResultCode.HasValue && this.ResultCode.Value >= 200 && this.ResultCode.Value < 300)
                return true;
            else
                return false;
        }

    }
}