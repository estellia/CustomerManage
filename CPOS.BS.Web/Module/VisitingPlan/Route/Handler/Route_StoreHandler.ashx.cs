﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using JIT.CPOS.BS.Entity;
using JIT.CPOS.BS.Web.Extension;
using JIT.Utility.ExtensionMethod;
using JIT.CPOS.BS.BLL;
using JIT.Utility.Reflection;

namespace JIT.CPOS.BS.Web.Module.VisitingPlan.Route.Handler
{
    /// <summary>
    /// Route_StoreHandler 的摘要说明
    /// </summary>
    public class Route_StoreHandler : JIT.CPOS.BS.Web.PageBase.JITCPOSAjaxHandler
    {
        /// <summary>
        /// 页面入口
        /// </summary>
        /// <param name="pContext"></param>
        protected override void AjaxRequest(HttpContext pContext)
        {
            string res = "";
            switch (this.BTNCode)
            {
                case "search":
                    if (!string.IsNullOrEmpty(this.Method))
                    {
                        switch (this.Method)
                        {
                            case "InitGridData": //初使化终端编辑列表的数据
                                res = GetInitGridData(pContext).ToJSON();
                                break;
                            case "PageGridData": //查询数据分页查询
                                res = GetStorePageData(pContext).ToJSON();
                                break;
                            case "QueryView": //得到查询控件
                                res = new JIT.CPOS.BS.Web.Module.BasicData.Store.Handler.StoreHandler().GetStoreQueryConditionControls(pContext).ToJSON();
                                break;
                        }
                    }
                    break;
                case "create":
                    if (!string.IsNullOrEmpty(this.Method))
                    {
                        switch (this.Method)
                        {
                            case "InitGridData": //初使化终端编辑列表的数据
                                res = GetInitGridData(pContext).ToJSON();
                                break;
                            case "PageGridData": //查询数据分页查询
                                res = GetStorePageData(pContext).ToJSON();
                                break;
                            case "QueryView": //得到查询控件
                                res = new JIT.CPOS.BS.Web.Module.BasicData.Store.Handler.StoreHandler().GetStoreQueryConditionControls(pContext).ToJSON();
                                break;
                            case "EditRoutePOPList_Store":
                                res = EditRoutePOPList_Store(pContext);
                                break;
                            case "EditRoutePOPMap_Store":
                                res = EditRoutePOPMap_Store(pContext);
                                break;
                        }
                    }
                    break;
                case "update":
                    if (!string.IsNullOrEmpty(this.Method))
                    {
                        switch (this.Method)
                        {
                            case "InitGridData": //初使化终端编辑列表的数据
                                res = GetInitGridData(pContext).ToJSON();
                                break;
                            case "PageGridData": //查询数据分页查询
                                res = GetStorePageData(pContext).ToJSON();
                                break;
                            case "QueryView": //得到查询控件
                                res = new JIT.CPOS.BS.Web.Module.BasicData.Store.Handler.StoreHandler().GetStoreQueryConditionControls(pContext).ToJSON();
                                break;
                            case "EditRoutePOPList_Store":
                                res = EditRoutePOPList_Store(pContext);
                                break;
                            case "EditRoutePOPMap_Store":
                                res = EditRoutePOPMap_Store(pContext);
                                break;
                        }
                    }
                    break;

            }
            pContext.Response.Write(res);
            pContext.Response.End();
        }

        #region GetInitGridData
        private JIT.CPOS.BS.Web.Module.BasicData.Store.Handler.StoreHandler.GridInitEntity GetInitGridData(HttpContext pContext)
        {
            JIT.CPOS.BS.Web.Module.BasicData.Store.Handler.StoreHandler.GridInitEntity g
                = new JIT.CPOS.BS.Web.Module.BasicData.Store.Handler.StoreHandler.GridInitEntity();

            g.GridDataDefinds = new JIT.CPOS.BS.Web.Module.BasicData.Store.Handler.StoreHandler().GetStoreGridDataModels(pContext);
            //new
            GridColumnModelEntity entity = new GridColumnModelEntity();
            entity.DataType = 1;
            entity.DataIndex = "MappingID";
            g.GridDataDefinds.Add(entity);

            entity = new GridColumnModelEntity();
            entity.DataType = 3;
            entity.DataIndex = "Sequence";
            g.GridDataDefinds.Add(entity);
            
            g.GridColumnDefinds = new JIT.CPOS.BS.Web.Module.BasicData.Store.Handler.StoreHandler().GetStoreGridColumns(pContext);
            //new
            //GridColumnEntity entity1 = new GridColumnEntity();
            //entity1.ColumnText = "MappingID";
            //entity1.DataIndex = "MappingID";
            //g.GridColumnDefinds.Add(entity1);

            //entity1 = new GridColumnEntity();
            //entity1.ColumnText = "Sequence";
            //entity1.DataIndex = "Sequence";
            //g.GridColumnDefinds.Add(entity1);

            //g.GridDatas = GetStorePageData(pContext);
            return g;
        }
        #endregion
        #region GetStorePageData
        public PageResultEntity GetStorePageData(HttpContext pContext)
        {
            RoutePOPMappingBLL_Store b = new RoutePOPMappingBLL_Store(CurrentUserInfo, "Store");
            string pSearch = pContext.Request["pSearch"];
            List<DefindControlEntity> l = new List<DefindControlEntity>();
            if (!string.IsNullOrEmpty(pSearch))
            {
                l = pSearch.DeserializeJSONTo<List<DefindControlEntity>>();

            }

            int? pPageIndex = null;
            int? pPageSize = null;
            if (!string.IsNullOrEmpty(pContext.Request["pPageIndex"]))
            {
                pPageIndex = Convert.ToInt32(pContext.Request["pPageIndex"]);
            }
            if (!string.IsNullOrEmpty(pContext.Request["pPageSize"]))
            {
                pPageSize = Convert.ToInt32(pContext.Request["pPageSize"]);
            }
            return b.GetRouteStoreList(l, pPageSize, pPageIndex, pContext.Request["CorrelationValue"]);


        }
        #endregion
        #region EditRoutePOPList_Store
        private string EditRoutePOPList_Store(HttpContext pContext)
        {
            Guid routeid = Guid.Parse(pContext.Request["id"]);
            RoutePOPMappingViewEntity[] entity = pContext.Request["form"].DeserializeJSONTo<RoutePOPMappingViewEntity[]>();

            new RoutePOPMappingBLL_Store(CurrentUserInfo, "Store").EditRoutePOPList_Store(routeid, entity);
            return "{success:true}";
        }
        #endregion
        #region EditRoutePOPMap_Store
        private string EditRoutePOPMap_Store(HttpContext pContext)
        {
            Guid routeid = Guid.Parse(pContext.Request["id"]);
            RoutePOPMappingViewEntity[] entity = pContext.Request["form"].DeserializeJSONTo<RoutePOPMappingViewEntity[]>();
            string deleteList = pContext.Request["deleteList"];
            new RoutePOPMappingBLL_Store(CurrentUserInfo, "Store").EditRoutePOPMap_Store(routeid, entity, deleteList);
            return "{success:true}";
        }
        #endregion
    }
}