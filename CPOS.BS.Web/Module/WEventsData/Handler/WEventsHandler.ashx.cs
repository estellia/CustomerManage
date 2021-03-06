﻿using System.Collections.Generic;
using System.Web;
using System.Linq;
using JIT.Utility.ExtensionMethod;
using JIT.CPOS.BS.BLL;
using JIT.CPOS.BS.Entity;
using JIT.Utility.DataAccess.Query;
using System;
using System.Text;
using System.Data;
using JIT.Utility.Log;
using System.Configuration;
using JIT.CPOS.Common;
using System.Threading;

namespace JIT.CPOS.BS.Web.Module.WEventsData.Handler 
{
    /// <summary>
    /// EventsHandler
    /// </summary>
    public class WEventsHandler : JIT.CPOS.BS.Web.PageBase.JITCPOSAjaxHandler
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
                case "events_query":      //活动查询
                    content = GetEventsData();
                    break;
                case "GetEventsVipData":  //报名活动人员
                    content = GetEventsVipData();
                    break;
                case "events_delete":     //活动删除
                    content = EventsDeleteData();
                    break;
                case "prizes_delete":     //奖品删除
                    content = PrizesDeleteData();
                    break;
                case "get_events_by_id":  //根据ID获取活动信息
                    content = GetEventsById();
                    break;
                case "get_prizes_by_id":  //根据ID获取奖品信息
                    content = GetPrizesById();
                    break;
                case "events_save":       //保存活动信息
                    content = SaveEvents();
                    break;
                case "prizes_save":       //保存奖品信息
                    content = SavePrizes();
                    break;
                case "events_user_list_query":      //活动人员查询
                    content = GetEventsUserListData();
                    break;
                case "events_user_list_query2":      //活动人员查询
                    content = GetEventsUserListData2(); //微活动报名人员
                    break;
                case "events_prizes_list_query":      //奖品查询
                    content = GetEventsPrizesListData();
                    break;
                case "events_lotterylog_list_query":      //抽奖日志查询
                    content = GetEventsLotteryLogListData();
                    break;
                case "SetEventWXCode":  //生成活动二维码
                    content = SetEventWXCode();
                    break;
                case "EventVip_query":    //查询活动会员
                    content = GetEventVip();
                    break;
                case "GenerateQR":  //生成个人二维码名片
                    content = GenerateQR();
                    break;
                case "PushWeixin":  //推送微信消息
                    content = PushWeixin();
                    break;
                case "SaveEventVip":
                    content = SaveEventVip();
                    break;
                case "GetEventVipById":
                    content = GetEventVipById();
                    break;
                case "Unregister":
                    content = Unregister();
                    break;
                case "DeleteEventVip":
                    content = DeleteEventVip();
                    break;
                case "ForbidLottery":
                    content = ForbidLottery();
                    break;
                case "events_round_list_query":
                    content = GetEventsRoundListData();
                    break;
                case "events_round_prizes_list_query":
                    content = GetEventRoundPrizesListData();
                    break;
                case "get_round_by_id": 
                    content = GetRoundById();
                    break;
                case "round_save":
                    content = SaveRound();
                    break;
                case "round_delete":
                    content = DeleteRound();
                    break;
                case "events_pool":
                    content = EventsPool();
                    break;
                case "get_prizes_winner_list":
                    content = GetPrizesWinnerList();
                    break;
            }
            pContext.Response.Write(content);
            pContext.Response.End();
        }

        #region EventsDeleteData 活动删除

        /// <summary>
        /// 活动删除
        /// </summary>
        public string EventsDeleteData()
        {
            string content = string.Empty;
            string error = "";
            var responseData = new ResponseData();

            string key = string.Empty;
            if (FormatParamValue(Request("ids")) != null && FormatParamValue(Request("ids")) != string.Empty)
            {
                key = FormatParamValue(Request("ids")).ToString().Trim();
            }

            if (key == null || key.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "活动ID不能为空";
                return responseData.ToJSON();
            }

            string[] ids = key.Split(',');
            new WLEventsBLL(this.CurrentUserInfo).Delete(ids);

            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }

        #endregion

        #region 奖品删除

        /// <summary>
        /// 奖品删除
        /// </summary>
        public string PrizesDeleteData()
        {
            string content = string.Empty;
            string error = "";
            var responseData = new ResponseData();

            string key = string.Empty;
            if (FormatParamValue(Request("ids")) != null && FormatParamValue(Request("ids")) != string.Empty)
            {
                key = FormatParamValue(Request("ids")).ToString().Trim();
            }

            if (key == null || key.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "奖品ID不能为空";
                return responseData.ToJSON();
            }

            string[] ids = key.Split(',');
            new LPrizesBLL(this.CurrentUserInfo).Delete(ids);

            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }

        #endregion

        #region GetEventsData 查询活动列表

        /// <summary>
        /// 查询活动列表
        /// </summary>
        public string GetEventsData()
        {
            var form = Request("form").DeserializeJSONTo<EventsQueryEntity>();
            var eventsService = new WLEventsBLL(this.CurrentUserInfo);

            string content = string.Empty;

            string EventType = FormatParamValue(form.EventType);
            string Title = FormatParamValue(form.Title);
            string DateBegin = FormatParamValue(form.DateBegin);
            string DateEnd = FormatParamValue(form.DateEnd);
            string ParentEventID = FormatParamValue(Request("ParentEventID"));
            int pageIndex = Utils.GetIntVal(FormatParamValue(Request("page"))) - 1;
            PageSize = Utils.GetIntVal(FormatParamValue(Request("limit")));
            LEventsEntity queryEntity = new LEventsEntity();
            queryEntity.EventType = EventType == null || EventType == "" ? 0 : int.Parse(EventType);
            queryEntity.Title = Title;
            queryEntity.StartTimeText = DateBegin;
            if (ParentEventID != null && ParentEventID != "-1" && ParentEventID != "root" && ParentEventID.Length != 0)
            {
                queryEntity.ParentEventID = ParentEventID;
            }
            if (this.CurrentUserInfo.CurrentUser.User_Name.ToLower() == "admin")
            {
                queryEntity.CreateBy = this.CurrentUserInfo.CurrentUser.User_Name.ToLower();
            }
            else
            {
                queryEntity.CreateBy = this.CurrentUserInfo.UserID;
            }
            if (DateEnd != null && DateEnd.Length > 0)
            {
                queryEntity.EndTimeText = Convert.ToDateTime(DateEnd).AddDays(1).ToString("yyyy-MM-dd hh:mm:ss");
            }

            int pPageCount = 0;
            var data = eventsService.WEventGetWebEvents(queryEntity, pageIndex, PageSize, out pPageCount);
            var dataTotalCount = eventsService.WEventGetWebEventsCount(queryEntity);

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                pPageCount);
            return content;
        }


        /// <summary>
        /// 报名活动人员列表
        /// </summary>
        public string GetEventsVipData()
        {
            
            var eventsService = new WLEventsBLL(this.CurrentUserInfo);

            string content = string.Empty;
         
            string EventID = FormatParamValue(Request("EventID"));
            int pageIndex = Utils.GetIntVal(FormatParamValue(Request("page"))) - 1;
            PageSize = Utils.GetIntVal(FormatParamValue(Request("limit")));
            LEventsEntity queryEntity = new LEventsEntity(); 

            int pPageCount = 0;
            var data = eventsService.GetEventsVipData(EventID, pageIndex, PageSize, out pPageCount);
            var dataTotalCount = eventsService.WEventGetWebEventsCount(queryEntity);

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                pPageCount);
            return content;
        }
        #endregion

        #region GetEventsById 根据ID获取活动信息

        /// <summary>
        /// 根据ID获取活动信息
        /// </summary>
        public string GetEventsById()
        {
            var eventsService = new WLEventsBLL(this.CurrentUserInfo);
            //var eventWeiXinMappingBLL = new EventWeiXinMappingBLL(this.CurrentUserInfo);
            string content = string.Empty;

            string key = string.Empty;
            if (FormatParamValue(Request("EventID")) != null && FormatParamValue(Request("EventID")) != string.Empty)
            {
                key = FormatParamValue(Request("EventID")).ToString().Trim();
            }

            var condition = new List<IWhereCondition>();
            if (!key.Equals(string.Empty))
            {
                condition.Add(new EqualsCondition() { FieldName = "EventID", Value = key });
            }

            var data = eventsService.Query(condition.ToArray(), null).ToList().FirstOrDefault();

            data.Description =  HttpUtility.HtmlDecode(data.Description);

            var parentEventObj = eventsService.QueryByEntity(new LEventsEntity() { EventID = data.ParentEventID }, null);
            if (parentEventObj != null && parentEventObj.Length > 0 && parentEventObj[0] != null)
            {
                data.ParentEventTitle = parentEventObj[0].Title;
            }
            WQRCodeManagerBLL wQRCodeManagerBLL = new WQRCodeManagerBLL(CurrentUserInfo);
            var eventsWXList = wQRCodeManagerBLL.QueryByEntity(new WQRCodeManagerEntity
            {
                ObjectId = key
                ,IsDelete = 0
            }, null);
            if (eventsWXList != null && eventsWXList.Length > 0 && eventsWXList[0] != null)
            {
                data.WXCodeImageUrl = eventsWXList[0].ImageUrl;
                data.WXCode = eventsWXList[0].QRCode;
                data.QRCodeTypeId = eventsWXList[0].QRCodeTypeId != null ? eventsWXList[0].QRCodeTypeId.ToString() : "";
            }
            //var eventWeiXinMappings = eventWeiXinMappingBLL.QueryByEntity(new EventWeiXinMappingEntity()
            //{
            //    EventID = key
            //}, null);

            //if (eventWeiXinMappings != null && eventWeiXinMappings.Length > 0)
            //{
            //    data.WeiXinPublicID = eventWeiXinMappings[0].WeiXinPublicID;
            //}

            var jsonData = new JsonData();
            jsonData.totalCount = (data == null) ? "0" : "1";
            jsonData.data = data;

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                (data == null) ? "0" : "1");

            return content;
        }

        #endregion

        #region 根据ID获取奖品信息

        /// <summary>
        /// 根据ID获取奖品信息
        /// </summary>
        public string GetPrizesById()
        {
            var eventsService = new LPrizesBLL(this.CurrentUserInfo);
            //var eventWeiXinMappingBLL = new EventWeiXinMappingBLL(this.CurrentUserInfo);
            string content = string.Empty;

            string key = string.Empty;
            if (FormatParamValue(Request("PrizesID")) != null && FormatParamValue(Request("PrizesID")) != string.Empty)
            {
                key = FormatParamValue(Request("PrizesID")).ToString().Trim();
            }

            var condition = new List<IWhereCondition>();
            if (!key.Equals(string.Empty))
            {
                condition.Add(new EqualsCondition() { FieldName = "PrizesID", Value = key });
            }

            var data = eventsService.Query(condition.ToArray(), null).ToList().FirstOrDefault();
            //if (data != null)
            //{
            //    data.StrPublishTime = data.PublishTime.Value.ToString("yyyy-MM-dd");
            //}

            //data.StartDateText = data.StartTime == null ? string.Empty :
            //    Convert.ToDateTime(data.StartTime).ToString("yyyy-MM-dd");
            //data.EndDateText = data.StartTime == null ? string.Empty :
            //    Convert.ToDateTime(data.StartTime).ToString("yyyy-MM-dd");

            //data.Description =  HttpUtility.HtmlDecode(data.Description);

            //var eventWeiXinMappings = eventWeiXinMappingBLL.QueryByEntity(new EventWeiXinMappingEntity()
            //{
            //    EventID = key
            //}, null);

            //if (eventWeiXinMappings != null && eventWeiXinMappings.Length > 0)
            //{
            //    data.WeiXinPublicID = eventWeiXinMappings[0].WeiXinPublicID;
            //}

            var jsonData = new JsonData();
            jsonData.totalCount = (data == null) ? "0" : "1";
            jsonData.data = data;

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                (data == null) ? "0" : "1");

            return content;
        }

        #endregion

        #region SaveEvents 保存活动信息

        /// <summary>
        /// 保存活动信息
        /// </summary>
        public string SaveEvents()
        {
            var eventsService = new WLEventsBLL(this.CurrentUserInfo);
            //var eventWeiXinMappingBLL = new EventWeiXinMappingBLL(this.CurrentUserInfo);

            string content = string.Empty;
            string error = "";
            var responseData = new ResponseData();

            string key = string.Empty;
            string EventID = string.Empty;
            var events = Request("events");

            if (FormatParamValue(events) != null && FormatParamValue(events) != string.Empty)
            {
                key = FormatParamValue(events).ToString().Trim();
            }
            if (FormatParamValue(Request("EventID")) != null && FormatParamValue(Request("EventID")) != string.Empty)
            {
                EventID = FormatParamValue(Request("EventID")).ToString().Trim();
            }

            var eventsEntity = key.DeserializeJSONTo<LEventsEntity>();

            if (eventsEntity.Title == null || eventsEntity.Title.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "活动标题不能为空";
                return responseData.ToJSON();
            }
            if (eventsEntity.StartTimeText == null || eventsEntity.StartTimeText.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "起始时间不能为空";
                return responseData.ToJSON();
            }
            if (eventsEntity.EndTimeText == null || eventsEntity.EndTimeText.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "结束时间不能为空";
                return responseData.ToJSON();
            } 

            try
            {
                eventsEntity.BeginTime = eventsEntity.StartTimeText;
                eventsEntity.BeginTime = Convert.ToDateTime(eventsEntity.BeginTime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                responseData.success = false;
                responseData.msg = "起始时间格式错误";
                return responseData.ToJSON();
            }

            try
            {
                eventsEntity.EndTime = eventsEntity.EndTimeText;
                eventsEntity.EndTime = Convert.ToDateTime(eventsEntity.EndTime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                responseData.success = false;
                responseData.msg = "结束时间格式错误";
                return responseData.ToJSON();
            }

            eventsEntity.Description = HttpUtility.HtmlEncode(eventsEntity.Description);
            eventsEntity.CustomerId = this.CurrentUserInfo.CurrentLoggingManager.Customer_Id.ToString().Trim();

            if (eventsEntity.ParentEventID != null && eventsEntity.ParentEventID.Length > 0)
            {
                eventsEntity.IsSubEvent = 1;
                var tmpParentObj = eventsService.GetByID(eventsEntity.ParentEventID);
                eventsEntity.EventLevel = tmpParentObj.EventLevel + 1;
            }
            else 
            {
                eventsEntity.IsSubEvent = 0;
                eventsEntity.EventLevel = 1;
            }

            var tmpObj = eventsService.GetByID(eventsEntity.EventID);
            if (tmpObj == null)
            {
                //eventsEntity.EventID = Utils.NewGuid();
                eventsEntity.EventStatus = 0;
                eventsService.Create(eventsEntity);
            }
            else
            {
                eventsEntity.EventID = EventID;

                eventsService.Update(eventsEntity, false);
            }
            //Jermyn20140102
            LEventsWXBLL eventsWXServer = new LEventsWXBLL(CurrentUserInfo);
            var eventsWXList = eventsWXServer.QueryByEntity(new LEventsWXEntity
            {
                EventId = EventID
                ,
                IsDelete = 0
            }, null);
            LEventsWXEntity eventsWXInfo = new LEventsWXEntity();
            if (eventsWXList == null || eventsWXList.Length == 0 || eventsWXList[0]== null)
            {
                eventsWXInfo.EventWXId = BaseService.NewGuidPub();
                eventsWXInfo.EventId = EventID;
                eventsWXInfo.WXCode = eventsEntity.WXCode;
                eventsWXInfo.ImageUrl = eventsEntity.WXCodeImageUrl;
                eventsWXServer.Create(eventsWXInfo);
            }
            else
            {
                eventsWXInfo.EventWXId = eventsWXList[0].EventWXId;
                eventsWXInfo.ImageUrl = eventsEntity.WXCodeImageUrl;
                eventsWXInfo.WXCode = eventsEntity.WXCode;
                eventsWXServer.Update(eventsWXInfo,false);
            }
            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }

        #endregion

        #region 查询活动人员列表

        /// <summary>
        /// 查询活动人员列表
        /// </summary>
        public string GetEventsUserListData()
        {
            var service = new LEventSignUpBLL(this.CurrentUserInfo);

            string content = string.Empty;

            string EventId = FormatParamValue(Request("EventId"));
            int pageIndex = Utils.GetIntVal(FormatParamValue(Request("page"))) - 1;

            GetResponseParams<LEventSignUpEntity> dataList =
                service.GetEventApplies(EventId, pageIndex, PageSize);
            IList<LEventSignUpEntity> data = new List<LEventSignUpEntity>();
            int dataTotalCount = 0;

            if (dataList.Flag == "1")
            {
                data = dataList.Params.EntityList;
                dataTotalCount = dataList.Params.ICount;
            }

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                dataTotalCount);

            return content;
        }

        #endregion

        #region 查询活动人员列表2
        /// <summary>
        /// 查询活动人员列表2
        /// </summary>
        public string GetEventsUserListData2()
        {
            var eventsService = new WLEventsBLL(this.CurrentUserInfo);
            string content = string.Empty;

            string EventId = FormatParamValue(Request("EventID"));
            string searchSql = FormatParamValue(Request("SearchOptionValue")).Trim();

            Loggers.Debug(new DebugLogInfo() { Message = "GetEventsUserListData2 searchSql: " + searchSql });

            DataTable data = new DataTable();
            WEventUserMappingBLL service = new WEventUserMappingBLL(this.CurrentUserInfo);
            QuesQuestionsBLL quesQuestionsBLL = new QuesQuestionsBLL(this.CurrentUserInfo);
            data = service.SearchEventUserList(EventId, searchSql).Tables[0];

            var dataApplyQues = service.getEventApplyQues(EventId);
            int dataTotalCount = data.Rows.Count;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<table class=\"z_tb1_e\" style=\"width:100%;\">");
            sb.AppendFormat("<tr>");
            for (var i = 1; i < data.Columns.Count; i++)
            {
                var col = data.Columns[i];
                string colName = service.GetQuestionsDesc(EventId, col.ColumnName);
                sb.AppendFormat("<th class=\"z_tb1_e_th\">{0}</th>", colName);
            }
            sb.AppendFormat("</tr>");

            for (var i = 0; i < data.Rows.Count; i++)
            {
                var dr = data.Rows[i];
                sb.AppendFormat("<tr>");
                for (var c = 1; c < data.Columns.Count; c++)
                {
                    sb.AppendFormat("<td class=\"{1}\">{0}</td>",
                        dr[c],
                        (i % 2 == 0 ? "z_tb1_e_td" : "z_tb1_e_td z_tb1_e_td_alt"));
                }
                sb.AppendFormat("</tr>");
            }
            sb.AppendFormat("</table>");

            // options
            StringBuilder sbOptions = new StringBuilder();
            sbOptions.AppendFormat("<table class=\"z_tb2_e\" style=\"width:100%; margin-bottom:10px;\">");
            int colCount = 3;
            for (var i = 0; i < dataApplyQues.Params.QuesQuestionEntityList.Count; i++)
            {
                var obj = dataApplyQues.Params.QuesQuestionEntityList[i];
                if (i % colCount == 0)
                {
                    sbOptions.AppendFormat("<tr>");
                }

                StringBuilder sbOp = new StringBuilder();
                if (obj.QuestionType == 3)
                {
                    sbOp.AppendFormat("<select name=\"op\" cookie_name=\"{0}\" class=\"z_tb2_e_select\">", obj.CookieName);
                    sbOp.AppendFormat("<option value=\"\" class=\"z_tb2_e_op\"></option>");
                    for (var c = 0; c < obj.QuesOptionEntityList.Count; c++)
                    {
                        sbOp.AppendFormat("<option value=\"{1}\" class=\"z_tb2_e_op\">{0}</option>",
                            obj.QuesOptionEntityList[c].OptionsText,
                            obj.QuesOptionEntityList[c].OptionsText);
                    }
                    sbOp.AppendFormat("</select>");
                }
                else
                {
                    sbOp.AppendFormat("<input name=\"op\" cookie_name=\"{0}\" class=\"z_tb2_e_text\" />", obj.CookieName);
                }

                sbOptions.AppendFormat("<td class=\"z_tb2_e_td\"><div class=\"z_tb2_e_title\" title=\"{2}\">{0}</div>" +
                    "<div class=\"z_tb2_e_input\">{1}</div></td>",
                    GetMaxStr(obj.QuestionDesc, 18),
                    sbOp.ToString(),
                    obj.QuestionDesc);

                if (i % colCount == colCount - 1 ||
                    i + 1 == dataApplyQues.Params.QuesQuestionEntityList.Count)
                {
                    sbOptions.AppendFormat("</tr>");
                }
            }
            sbOptions.AppendFormat("</table>");

            content = string.Format("{{\"totalCount\":{1}, \"html\":{0}, \"projectName\":{3}, \"options\":{2}}}",
                sb.ToString().ToJSON(),
                dataTotalCount,
                sbOptions.ToString().ToJSON(),
                "\"WEvent\"");

            return content;
        }

        public string GetMaxStr(string str, int length)
        {
            str = str.Trim();
            if (str.Length > length) return str.Substring(0, length) + "...";
            return str;
        }
        #endregion

        #region GetEventsUserListDataFormEmba 查询活动人员列表（EMBA）
        ///// <summary>
        ///// 查询活动人员列表（EMBA）
        ///// </summary>
        //public string GetEventsUserListDataFormEmba()
        //{
        //    var service = new WEventUserMappingBLL(this.CurrentUserInfo);
        //    string content = string.Empty;
        //    string EventId = FormatParamValue(Request("EventID"));

        //    var data = service.SearchEventUserList(EventId).Tables[0];
        //    int dataTotalCount = data.Rows.Count;

        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat("<tr>");
        //    for (var i = 0; i < data.Columns.Count; i++)
        //    {
        //        sb.AppendFormat("<th class=\"z_tb1_th\">{0}</th>", data.Columns[i].ColumnName);
        //    }
        //    sb.AppendFormat("</tr>");

        //    for (var i = 0; i < data.Rows.Count; i++)
        //    {
        //        var dr = data.Rows[i];
        //        sb.AppendFormat("<tr>");
        //        for (var c = 0; c < data.Columns.Count; c++)
        //        {
        //            sb.AppendFormat("<td class=\"{1}\">{0}</td>",
        //                dr[c],
        //                (i % 2 == 0 ? "z_tb1_td" : "z_tb1_td z_tb1_td_alt"));
        //        }
        //        sb.AppendFormat("</tr>");
        //    }

        //    content = string.Format("{{\"totalCount\":{1}, \"html\":{0}, \"projectName\":{3}, \"options\":{2}}}",
        //        sb.ToString().ToJSON(),
        //        dataTotalCount,
        //        "\"\"",
        //        "\"EMBA\"");

        //    return content;
        //}

        #endregion

        #region 查询活动奖品列表

        /// <summary>
        /// 查询活动奖品列表
        /// </summary>
        public string GetEventsPrizesListData()
        {
            var service = new LPrizesBLL(this.CurrentUserInfo);

            string content = string.Empty;

            string EventId = FormatParamValue(Request("EventId"));
            int pageIndex = Utils.GetIntVal(FormatParamValue(Request("page"))) - 1;

            GetResponseParams<LPrizesEntity> dataList =
                service.GetEventPrizes(EventId, pageIndex, PageSize);
            IList<LPrizesEntity> data = new List<LPrizesEntity>();
            int dataTotalCount = 0;

            if (dataList.Flag == "1")
            {
                data = dataList.Params.EntityList;
                dataTotalCount = dataList.Params.ICount;
            }

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                dataTotalCount);

            return content;
        }

        #endregion

        #region 查询活动抽奖列表

        /// <summary>
        /// 查询活动抽奖列表
        /// </summary>
        public string GetEventsLotteryLogListData()
        {
            var service = new LLotteryLogBLL(this.CurrentUserInfo);

            string content = string.Empty;

            string EventId = FormatParamValue(Request("EventId"));
            int pageIndex = Utils.GetIntVal(FormatParamValue(Request("page"))) - 1;

            GetResponseParams<LLotteryLogEntity> dataList =
                service.GetEventLotteryLog(EventId, pageIndex, PageSize);
            IList<LLotteryLogEntity> data = new List<LLotteryLogEntity>();
            int dataTotalCount = 0;

            if (dataList.Flag == "1")
            {
                data = dataList.Params.EntityList;
                dataTotalCount = dataList.Params.ICount;
            }

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                dataTotalCount);

            return content;
        }

        #endregion

        #region 保存奖品信息

        /// <summary>
        /// 保存奖品信息
        /// </summary>
        public string SavePrizes()
        {
            var eventsService = new LPrizesBLL(this.CurrentUserInfo);

            string content = string.Empty;
            string error = "";
            var responseData = new ResponseData();

            string key = string.Empty;
            string PrizesID = string.Empty;
            var events = Request("events");

            if (FormatParamValue(events) != null && FormatParamValue(events) != string.Empty)
            {
                key = FormatParamValue(events).ToString().Trim();
            }
            if (FormatParamValue(Request("PrizesID")) != null && FormatParamValue(Request("PrizesID")) != string.Empty)
            {
                PrizesID = FormatParamValue(Request("PrizesID")).ToString().Trim();
            }

            var eventsEntity = key.DeserializeJSONTo<LPrizesEntity>();

            //if (eventsEntity.EventType == null || eventsEntity.EventType.ToString().Trim().Length == 0)
            //{
            //    responseData.success = false;
            //    responseData.msg = "活动类型不能为空";
            //    return responseData.ToJSON();
            //}
            if (eventsEntity.PrizeName == null || eventsEntity.PrizeName.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "奖品名称不能为空";
                return responseData.ToJSON();
            }

            if (PrizesID.Trim().Length == 0)
            {
                eventsEntity.PrizesID = Utils.NewGuid();

                eventsService.Create(eventsEntity);
            }
            else
            {
                eventsEntity.PrizesID = PrizesID;

                eventsService.Update(eventsEntity, false);
            }

            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }

        #endregion

        #region 生成活动二维码
        /// <summary>
        /// 生成活动二维码
        /// </summary>
        /// <returns></returns>
        public string SetEventWXCode()
        {
            #region 参数处理
            string WeiXinId = Request("WeiXinId");
            string EventID = Request("EventID");
            string WXCode = Request("WXCode");
            var responseData = new ResponseData();
            if (WeiXinId == null || WeiXinId.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "公众号不能为空";
                return responseData.ToJSON();
            }

            if (EventID == null || EventID.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "活动标识不能为空";
                return responseData.ToJSON();
            }
            
            if (WXCode == null || WXCode.Equals(""))
            {
                responseData.success = false;
                responseData.msg = "固定二维码类型不能为空";
                return responseData.ToJSON();
            }
            #endregion

            #region 获取微信公众号信息
            WApplicationInterfaceBLL server = new WApplicationInterfaceBLL(CurrentUserInfo);
            var wxObj = server.QueryByEntity(new WApplicationInterfaceEntity
            {
                CustomerId = CurrentUserInfo.CurrentUser.customer_id
                ,
                IsDelete = 0
                ,
                WeiXinID = WeiXinId
            }, null);
            if (wxObj == null || wxObj.Length == 0)
            {
                responseData.success = false;
                responseData.msg = "不存在对应的微信帐号";
                return responseData.ToJSON().ToString();
            }
            else
            {
                WApplicationInterfaceBLL wApplicationInterfaceBLL = new WApplicationInterfaceBLL(CurrentUserInfo);
                var appObj = wApplicationInterfaceBLL.QueryByEntity(new WApplicationInterfaceEntity()
                {
                    WeiXinID = WeiXinId
                }, null);
                var appId = "";
                if (appObj != null && appObj.Length > 0)
                {
                    appId = appObj[0].ApplicationId;
                }

                WQRCodeManagerBLL wQRCodeManagerBLL = new WQRCodeManagerBLL(CurrentUserInfo);
                WQRCodeTypeBLL wQRCodeTypeBLL = new WQRCodeTypeBLL(CurrentUserInfo);
                var qrTypeList = wQRCodeTypeBLL.GetList(new WQRCodeTypeEntity()
                {
                    QRCodeTypeId = Guid.Parse(WXCode)
                }, 0, 1);
                if (qrTypeList == null && qrTypeList.Count == 0)
                {
                    responseData.success = false;
                    responseData.msg = "二维码类型获取失败";
                    return responseData.ToJSON();
                }
                string qrTypeId = qrTypeList[0].QRCodeTypeId.ToString();
                if (!wQRCodeManagerBLL.CheckByObjectId(EventID))
                {
                    var tmpQRObj = wQRCodeManagerBLL.GetOne(appId, qrTypeId);
                    if (tmpQRObj != null)
                    {
                        tmpQRObj.ObjectId = EventID;
                        tmpQRObj.IsUse = 1;
                        wQRCodeManagerBLL.Update(tmpQRObj, false);
                        responseData.success = true;
                        responseData.msg = tmpQRObj.ImageUrl;
                        responseData.data = tmpQRObj.QRCode;
                        return responseData.ToJSON().ToString();
                    }
                    else
                    {
                        responseData.success = false;
                        responseData.msg = "二维码获取失败，请先去添加固定二维码";
                        return responseData.ToJSON();
                    }
                }
                else
                {
                    responseData.success = false;
                    responseData.msg = "二维码已绑定";
                    return responseData.ToJSON();
                }

            }
            #endregion

        }
        #endregion

        #region 查询活动员工列表
        public string GetEventVip()
        {
            var form = Request("form").DeserializeJSONTo<dynamic>();

            string content = string.Empty;

            int pageNo = Utils.GetIntVal(FormatParamValue(Request("page")));
            int pageSize = Utils.GetIntVal(FormatParamValue(Request("limit")));

            string vipName = form["VipName"];
            string phone = form["Phone"];
            string register = form["Register"].Value;
            string sign = form["Sign"].Value;
            string eventId = Request("eventId");

            if (!string.IsNullOrEmpty(eventId))
            {
                EventVipBLL fStaffBLL = new EventVipBLL(CurrentUserInfo);
                DataSet dataSet = fStaffBLL.GetEventVipJoinVip(eventId, vipName, phone, register, sign, pageNo, pageSize);

                content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                    dataSet.Tables[0].ToJSON(),
                    dataSet.Tables[1].Rows[0][1]);
            }

            return content;       
        }
        #endregion

        #region 异步生成员工二维码
        public delegate string[] DInvokeCreateQrcode(EventVipEntity fStaffEntity);

        public static string[] CallCreateQrcode(EventVipEntity vip)
        {
            string[] result = new string[2];
            var qrImageUrl = "";
            var qrFlag = cUserService.CreateQrcode(vip.VipName, vip.Email, "", vip.Phone, "",
                vip.VipCompany, vip.VipPost, "", vip.Profile, ref qrImageUrl);

            result[0] = qrFlag.ToString();
            result[1] = qrImageUrl;

            return result;
        }

        DInvokeCreateQrcode dInvokeCreateQrcode = CallCreateQrcode;

        public void CreateQRCode(ResponseData respData, EventVipBLL eventVipBLL, EventVipEntity eventVip)
        {
            try
            {
                // CreateQrcode
                IAsyncResult result = dInvokeCreateQrcode.BeginInvoke(eventVip, null, null);
                var QRResult = dInvokeCreateQrcode.EndInvoke(result);
                if (QRResult[0].ToLower() == "true")
                {
                    eventVipBLL.Update(new EventVipEntity()
                    {
                        EventVipID = eventVip.EventVipID,
                        DCodeImageUrl = QRResult[1]
                    }, false);
                }
                else
                {
                    respData.status = "执行完成但有错误";
                    respData.msg += eventVip.VipName + "的二维码生成错误，请为该用户重新生成二维码；";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateQR()
        {
            var ids = Request("ids").ToString();

            string content = string.Empty;
            var respData = new ResponseData();

            EventVipBLL fStaffBLL = new EventVipBLL(CurrentUserInfo);
            if (ids == "")
            {
                var fStaffEntity = fStaffBLL.Query(new IWhereCondition[] { 
                    new EqualsCondition() { FieldName = "CustomerId", Value = this.CurrentUserInfo.ClientID }
                }, null);

                foreach (var vip in fStaffEntity)
                {
                    CreateQRCode(respData, fStaffBLL, vip);
                }
            }
            else
            {
                foreach (var id in ids.Split(','))
                {
                    var vip = fStaffBLL.GetByID(id);
                    CreateQRCode(respData, fStaffBLL, vip);
                }
            }

            respData.success = true;
            content = respData.ToJSON();

            return content;
        }
        
        #endregion

        #region 根据选择的模板推送微信消息
        public string PushWeixin()
        {
            var ids = Request("ids").ToString();
            var template = Request("template").ToString();
            var eventId = Request("eventId").ToString();

            string content = string.Empty;
            var respData = new ResponseData();

            EventVipBLL eventVipBLL = new EventVipBLL(CurrentUserInfo);

            EventVipEntity eventVipEntity = new EventVipEntity();
            eventVipEntity.CustomerId = CurrentUserInfo.ClientID;
            
            string message = "";
            IEnumerable<EventVipEntity> eventVipEntityList = eventVipBLL.GetList(eventVipEntity, 0, 999999);

            if (ids == "")
            {
                eventVipEntityList = eventVipEntityList.Where(
                    s => s.EventId == eventId
                    && s.CustomerId == CurrentUserInfo.ClientID
                );
            }
            else
            {
                var idArray = ids.Split(',');
                eventVipEntityList = eventVipEntityList.Where(s => idArray.Contains(s.EventVipID.ToString()));
            }

            WLEventsBLL WLEventsBLL=new WLEventsBLL(CurrentUserInfo);
            var eventEntity = WLEventsBLL.GetByID(eventId);

            foreach (var eventVip in eventVipEntityList)
            {
                VipEntity vipEntity = new VipEntity();
                vipEntity.VIPID = eventVip.VipVipId;
                vipEntity.WeiXin = eventVip.WeiXin;
                vipEntity.WeiXinUserId = eventVip.WeiXinUserId;

                message = eventVipBLL.ReplaceTemplate(template, eventVip, vipEntity, eventEntity);

                //message = template.Replace("$VipName$", eventVip.VipName);
                //message = message.Replace("$Region$", eventVip.Seat);
                //message = message.Replace("$Seat$", eventVip.Seat);
                //message = message.Replace("$Ver$", new Random().Next().ToString());
                //message = message.Replace("$CustomerId$", eventVip.CustomerId);
                //message = message.Replace("$UserId$", eventVip.VipVipId);
                //message = message.Replace("$OpenId$", eventVip.WeiXinUserId);

                string code = JIT.CPOS.BS.BLL.WX.CommonBLL.SendWeixinMessage(message, "1", CurrentUserInfo, vipEntity);
                switch (code)
                {
                    case "103":
                        respData.status = "执行完成但有错误";
                        respData.msg += eventVip.VipName + "未查询到匹配的公众账号信息;";
                        break;
                    case "203":
                        respData.status = "执行完成但有错误";
                        respData.msg += eventVip.VipName + "发送失败;";
                        break;
                    default:
                        break;
                }
            }
            respData.success = true;
            content = respData.ToJSON();

            return content;
        }
        #endregion

        #region SaveEventVip 保存活动员工信息

        /// <summary>
        /// 保存活动员工信息
        /// </summary>
        public string SaveEventVip()
        {
            var eventVipBLL = new EventVipBLL(this.CurrentUserInfo);
            
            string content = string.Empty;
            var responseData = new ResponseData();

            string key = string.Empty;
            var vip = Request("vip");

            if (FormatParamValue(vip) != null && FormatParamValue(vip) != string.Empty)
            {
                key = FormatParamValue(vip).ToString().Trim();
            }

            var vipEntity = key.DeserializeJSONTo<EventVipEntity>();

            if (vipEntity.VipName == null || vipEntity.VipName.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "名称不能为空";
                return responseData.ToJSON();
            }
            if (vipEntity.Phone == null || vipEntity.Phone.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "联系电话不能为空";
                return responseData.ToJSON();
            }

            if (vipEntity.EventVipID == null)
            {
                vipEntity.EventVipID = Guid.NewGuid();
                vipEntity.CustomerId = CurrentUserInfo.ClientID;
                vipEntity.EventId = vipEntity.EventId;
                vipEntity.CanLottery = 1;
                eventVipBLL.Create(vipEntity);
            }
            else
            {
                eventVipBLL.Update(vipEntity, false);
            }

            //Updae QR Code
            CreateQRCode(responseData, eventVipBLL, vipEntity);

            responseData.success = true;

            content = responseData.ToJSON();
            return content;
        }

        #endregion

        #region GetEventsById 根据ID获取活动信息

        /// <summary>
        /// 根据ID获取活动信息
        /// </summary>
        public string GetEventVipById()
        {
            var fStaffBLL = new EventVipBLL(this.CurrentUserInfo);
            //var eventWeiXinMappingBLL = new EventWeiXinMappingBLL(this.CurrentUserInfo);
            string content = string.Empty;

            string key = string.Empty;
            if (FormatParamValue(Request("id")) != null && FormatParamValue(Request("id")) != string.Empty)
            {
                key = FormatParamValue(Request("id")).ToString().Trim();
            }

            var condition = new List<IWhereCondition>();
            if (!key.Equals(string.Empty))
            {
                condition.Add(new EqualsCondition() { FieldName = "EventVipId", Value = key });
            }

            var data = fStaffBLL.Query(condition.ToArray(), null).ToList().FirstOrDefault();

            var jsonData = new JsonData();
            jsonData.totalCount = (data == null) ? "0" : "1";
            jsonData.data = data;

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                (data == null) ? "0" : "1");

            return content;
        }

        #endregion

        #region Unregister 取消注册
        public string Unregister()
        {
            var ids = Request("ids").Split(',');

            string content = string.Empty;
            var respData = new ResponseData();
            VipEntity vipEntity = new VipEntity();

            EventVipBLL eventVipBLL = new EventVipBLL(CurrentUserInfo);
            VipBLL vipBLL = new VipBLL(CurrentUserInfo);
            
            respData.success = true;
            
            if (ids != null && ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    EventVipEntity eventVipEntity = new EventVipEntity();
                    eventVipEntity.EventVipID = Guid.Parse(id);
                    eventVipEntity = eventVipBLL.GetList(eventVipEntity, 0, 10).FirstOrDefault();

                    if (eventVipEntity != null && !string.IsNullOrEmpty(eventVipEntity.VipVipId))
                    {
                        vipEntity = vipBLL.GetByID(eventVipEntity.VipVipId);
                        vipEntity.Phone = null;
                        vipEntity.Status = 1;

                        vipBLL.Update(vipEntity, true);
                    }
                }
            }

            content = respData.ToJSON();

            return content;
        }
        #endregion

        #region DeleteEventVip 删除活动员工
        public string DeleteEventVip()
        {
            var ids = Request("ids").Split(',');

            string content = string.Empty;
            var respData = new ResponseData();

            EventVipBLL fStaffBLL = new EventVipBLL(CurrentUserInfo);
            if (ids != null && ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    EventVipEntity fStaffEntity = new EventVipEntity();
                    fStaffEntity = fStaffBLL.GetByID(id);

                    if (fStaffEntity != null)
                    {
                        fStaffBLL.Delete(fStaffEntity);
                    }
                }
            }

            respData.success = true;
            content = respData.ToJSON();

            return content;
        }
        #endregion

        #region ForbidLottery   取消抽奖资格
        public string ForbidLottery()
        {
            var nos = Request("nos").Split(',');

            string content = string.Empty;
            var respData = new ResponseData();

            EventVipBLL fStaffBLL = new EventVipBLL(CurrentUserInfo);
            if (nos != null && nos.Length > 0)
            {
                foreach (var no in nos)
                {
                    EventVipEntity fStaffEntity = new EventVipEntity();
                    fStaffEntity = fStaffBLL.GetByID(no);

                    if (fStaffEntity != null)
                    {
                        fStaffEntity.CanLottery = 0;
                        fStaffBLL.Update(fStaffEntity);
                    }
                }
            }

            respData.success = true;
            content = respData.ToJSON();

            return content;
        }
        #endregion

        #region 查询活动轮次列表

        /// <summary>
        /// 查询活动轮次列表
        /// </summary>
        public string GetEventsRoundListData()
        {
            var service = new LEventRoundBLL(this.CurrentUserInfo);

            string content = string.Empty;

            string EventId = FormatParamValue(Request("EventId"));
            int pageIndex = Utils.GetIntVal(FormatParamValue(Request("page")));

            LEventRoundEntity queryObj = new LEventRoundEntity();
            queryObj.EventId = EventId;

            var dataList = service.GetList(queryObj, pageIndex, PageSize);
            IList<LEventRoundEntity> data = new List<LEventRoundEntity>();
            int dataTotalCount = 0;

            data = dataList;
            dataTotalCount = service.GetListCount(queryObj);

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                dataTotalCount);

            return content;
        }

        #endregion

        #region 查询活动轮次奖品列表
        /// <summary>
        /// 查询活动轮次奖品列表
        /// </summary>
        public string GetEventRoundPrizesListData()
        {
            var service = new LPrizesBLL(this.CurrentUserInfo);

            string content = string.Empty;

            string EventId = FormatParamValue(Request("EventId"));
            string roundId = FormatParamValue(Request("roundId"));
            int pageIndex = Utils.GetIntVal(FormatParamValue(Request("page"))) - 1;

            LPrizesEntity queryObj = new LPrizesEntity();
            queryObj.EventId = EventId;

            var dataList = service.GetEventRoundPrizesList(EventId, roundId, pageIndex, 1000);
            IList<LPrizesEntity> data = new List<LPrizesEntity>();
            int dataTotalCount = 0;

            data = dataList;
            dataTotalCount = service.GetEventRoundPrizesCount(EventId, roundId);

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                dataTotalCount);

            return content;
        }
        #endregion

        #region 根据ID获取奖品信息
        /// <summary>
        /// 根据ID获取Round信息
        /// </summary>
        public string GetRoundById()
        {
            var eventsService = new LEventRoundBLL(this.CurrentUserInfo);
            //var eventWeiXinMappingBLL = new EventWeiXinMappingBLL(this.CurrentUserInfo);
            string content = string.Empty;

            string key = string.Empty;
            if (FormatParamValue(Request("RoundId")) != null && FormatParamValue(Request("RoundId")) != string.Empty)
            {
                key = FormatParamValue(Request("RoundId")).ToString().Trim();
            }

            var condition = new List<IWhereCondition>();
            if (!key.Equals(string.Empty))
            {
                condition.Add(new EqualsCondition() { FieldName = "RoundId", Value = key });
            }

            var data = eventsService.Query(condition.ToArray(), null).ToList().FirstOrDefault();
            //if (data != null)
            //{
            //    data.StrPublishTime = data.PublishTime.Value.ToString("yyyy-MM-dd");
            //}

            //data.StartDateText = data.StartTime == null ? string.Empty :
            //    Convert.ToDateTime(data.StartTime).ToString("yyyy-MM-dd");
            //data.EndDateText = data.StartTime == null ? string.Empty :
            //    Convert.ToDateTime(data.StartTime).ToString("yyyy-MM-dd");

            //data.Description =  HttpUtility.HtmlDecode(data.Description);

            //var eventWeiXinMappings = eventWeiXinMappingBLL.QueryByEntity(new EventWeiXinMappingEntity()
            //{
            //    EventID = key
            //}, null);

            //if (eventWeiXinMappings != null && eventWeiXinMappings.Length > 0)
            //{
            //    data.WeiXinPublicID = eventWeiXinMappings[0].WeiXinPublicID;
            //}

            var jsonData = new JsonData();
            jsonData.totalCount = (data == null) ? "0" : "1";
            jsonData.data = data;

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                (data == null) ? "0" : "1");

            return content;
        }
        #endregion

        #region SaveRound 保存轮次信息
        /// <summary>
        /// 保存轮次信息
        /// </summary>
        public string SaveRound()
        {
            var eventsService = new LEventRoundBLL(this.CurrentUserInfo);

            string content = string.Empty;
            string error = "";
            var responseData = new ResponseData();

            string key = string.Empty;
            string RoundId = string.Empty;
            string EventId = string.Empty;
            var events = Request("events");

            if (FormatParamValue(events) != null && FormatParamValue(events) != string.Empty)
            {
                key = FormatParamValue(events).ToString().Trim();
            }
            if (FormatParamValue(Request("RoundId")) != null && FormatParamValue(Request("RoundId")) != string.Empty)
            {
                RoundId = FormatParamValue(Request("RoundId")).ToString().Trim();
            }
            if (FormatParamValue(Request("EventId")) != null && FormatParamValue(Request("EventId")) != string.Empty)
            {
                EventId = FormatParamValue(Request("EventId")).ToString().Trim();
            }

            var eventsEntity = key.DeserializeJSONTo<LEventRoundEntity>();

            if (eventsEntity.Round == null || eventsEntity.Round.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "轮次不能为空";
                return responseData.ToJSON();
            }
            if (eventsEntity.RoundStatus == null)
            {
                responseData.success = false;
                responseData.msg = "状态不能为空";
                return responseData.ToJSON();
            }

            if (eventsEntity.RoundId == null || eventsEntity.RoundId == "")
            {
                eventsEntity.RoundId = Utils.NewGuid();
                var tmpExsitList = eventsService.QueryByEntity(new LEventRoundEntity() {
                    Round = eventsEntity.Round,
                    EventId = eventsEntity.EventId
                }, null);
                if (tmpExsitList != null && tmpExsitList.Length > 0)
                {
                    responseData.success = false;
                    responseData.msg = "轮次已存在";
                    return responseData.ToJSON();
                }
                eventsService.Create(eventsEntity);
            }
            else
            {
                //var tmpObj = eventsService.GetByID(eventsEntity.RoundId);
                eventsService.Update(eventsEntity, false);
            }

            LEventRountPrizesMappingBLL lEventRountPrizesMappingBLL = new LEventRountPrizesMappingBLL(CurrentUserInfo);
            var prizesCount = 0;
            if (eventsEntity.Prizes != null && eventsEntity.Prizes.Count > 0)
            {
                foreach (var prize in eventsEntity.Prizes)
                {
                    prizesCount += prize.PrizesCount;
                    var tmpMapList = lEventRountPrizesMappingBLL.QueryByEntity(new LEventRountPrizesMappingEntity() { 
                        RoundId = eventsEntity.RoundId,
                        PrizesID = prize.PrizesID
                    }, null);
                    if (tmpMapList == null || tmpMapList.Length == 0)
                    {
                        lEventRountPrizesMappingBLL.Create(new LEventRountPrizesMappingEntity() { 
                            MappingId = Utils.NewGuid(),
                            RoundId = eventsEntity.RoundId,
                            PrizesID  = prize.PrizesID,
                            PrizesCount = prize.PrizesCount
                        });
                    }
                    else
                    {
                        var tmpMapObj = tmpMapList[0];
                        lEventRountPrizesMappingBLL.Update(new LEventRountPrizesMappingEntity() {
                            MappingId = tmpMapObj.MappingId,
                            PrizesCount = prize.PrizesCount
                        }, false);
                    }

                }
            }
            eventsEntity.PrizesCount = prizesCount;
            eventsService.Update(eventsEntity, false);

            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }

        #endregion

        #region DeleteRound 轮次删除

        /// <summary>
        /// 活动删除
        /// </summary>
        public string DeleteRound()
        {
            string content = string.Empty;
            string error = "";
            var responseData = new ResponseData();

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
            new LEventRoundBLL(this.CurrentUserInfo).Delete(ids);

            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }

        #endregion

        #region EventsPool 生成奖品池

        /// <summary>
        /// 生成奖品池
        /// </summary>
        public string EventsPool()
        {
            string content = string.Empty;
            string error = "";
            var responseData = new ResponseData();

            var eventsService = new LEventRoundBLL(this.CurrentUserInfo);
            string key = string.Empty;
            if (FormatParamValue(Request("EventID")) != null && FormatParamValue(Request("EventID")) != string.Empty)
            {
                key = FormatParamValue(Request("EventID")).ToString().Trim();
            }

            if (key == null || key.Trim().Length == 0)
            {
                responseData.success = false;
                responseData.msg = "ID不能为空";
                return responseData.ToJSON();
            }

            var flag = eventsService.PrizePoolSetting(key);
            if (flag != 1)
            {
                responseData.success = false;
                responseData.msg = "生成奖品池失败";
                return responseData.ToJSON();
            }

            responseData.success = true;
            responseData.msg = error;

            content = responseData.ToJSON();
            return content;
        }

        #endregion

        #region 查询中奖人员列表
        /// <summary>
        /// 查询中奖人员列表
        /// </summary>
        public string GetPrizesWinnerList()
        {
            var service = new LPrizeWinnerBLL(this.CurrentUserInfo);

            string content = string.Empty;

            string EventId = FormatParamValue(Request("EventId"));
            int pageIndex = Utils.GetIntVal(FormatParamValue(Request("page")));

            LPrizesEntity queryObj = new LPrizesEntity();
            queryObj.EventId = EventId;

            var dataList = service.GetPrizeWinnerListByEventId(EventId, pageIndex, PageSize);
            IList<LPrizeWinnerEntity> data = new List<LPrizeWinnerEntity>();
            int dataTotalCount = 0;

            data = dataList.PrizeWinnerList;
            dataTotalCount = dataList.ICount;

            content = string.Format("{{\"totalCount\":{1},\"topics\":{0}}}",
                data.ToJSON(),
                dataTotalCount);

            return content;
        }
        #endregion
    }

    #region QueryEntity

    public class EventsQueryEntity
    {
        public string EventID;
        public string EventType;
        public string Title;
        public string CityID;
        public string DateBegin;
        public string DateEnd;
    }

    #endregion
}