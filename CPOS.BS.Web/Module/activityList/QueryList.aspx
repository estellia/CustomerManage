﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/optimize.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8" />
    <title>活动列表</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link href="<%=StaticUrl+"/module/activityList/css/style.css?v=0.4"%>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <div class="allPage" id="section" data-js="js/queryList.js?ver=0.3">
            <!-- 内容区域 -->
            <div class="contentArea_vipquery">
                <!--个别信息查询-->
                <div class="queryTermArea" id="simpleQuery" style="display: inline-block; width: 100%;">
                        <div class="item">
                          <form></form>
                          <form id="seach">
                              <div class="commonSelectWrap">
                                  <em class="tit">活动名称：</em>
                                  <label class="searchInput" >
                                      <input data-text="活动名称" data-flag="item_name" name="item_name" type="text" value="">
                                  </label>
                              </div>
                              <div class="commonSelectWrap" style="width: 325px;">
                                  <em class="tit">活动时间：</em>
                                  <div class="selectBox">
                                        <input type="text" data-flag="item_startTime" name="item_startTime" id="startDate" class="easyui-datebox" data-options="width:100,height:30"/>&nbsp;&nbsp;至&nbsp;&nbsp;<input type="text"  data-flag="item_endTime" name="item_endTime" class="easyui-datebox" validType="compareDate[$('#startDate').datebox('getText'),'前面选择的时间必须晚于该时间']" data-options="width:100,height:30"/>
                                  </div>
                              </div>
                              <div class="moreQueryWrap">
                                 <a href="javascript:;" class="commonBtn queryBtn">查询</a>
                              </div>
                          </form>

                        </div>

                    <!--<h2 class="commonTitle">会员查询</h2>-->

                </div>
                <div class="optionBtn">
                	<a class="commonBtn  icon w100  icon_add r" id="addNewGamesBtn" href="javascript:;">添加游戏</a>
                </div>
                <div class="tableWrap" id="tableWrap" style="display:inline-block;width:100%;">

                   <div class="dataTable" id="gridTable">

                          <div  class="loading">
                                   <span>
                                 <img src="../static/images/loading.gif"></span>
                            </div>


                   </div>
                    <div id="pageContianer">
                    <div class="dataMessage" >没有符合条件的查询记录</div>
                        <div id="kkpager">
                        </div>
                    </div>
                </div>
            </div>
        </div>
      
      
      
    <!-- 遮罩层 -->
    <div class="jui-mask" style="display:none;"></div>
    <!--显示参与人数，弹出-->
    <div class="jui-dialog jui-dialog-table" style="display:none;">
        <div class="jui-dialog-tit">
            <h2>人数统计</h2>
            <span class="jui-dialog-close"></span>
        </div>
        <div class="" id="tableWrap2">
        	<div class="exportTable">
            	<a href="javascript:;" class="exportBtn">全部导出</a>
            </div>
            <div>
            	<div class="dataTable" id="gridTable2"></div>
                <!--
              	<thead>
                	<tr class="tableHead">
                        <th>会员名称</th>
                        <th>抽奖次数</th>
                        <th>时间</th>
                    </tr>
                </thead>
                <tbody>
                  <tr class="">
                    <td>丁丁</td>
                    <td>10</td>
                    <td>2015-04-30 14:22:35</td>
                  </tr>
                </tbody>
                -->
                <div id="pageContianer">
                   <div class="dataMessage">没有符合条件的查询记录</div>
                   <div id="kkpager2"></div>
                </div>
            </div>
            <div class="btnWrap">
                <a href="javascript:;" class="commonBtn saveBtn">确定</a>
            </div>
        </div>
    </div>
      
      
      
      
      
      
       <!-- 取消订单-->
       <script id="tpl_OrderCancel" type="text/html">
            <form id="payOrder">
			   <div class="commonSelectWrap">
					 <em class="tit">备注：</em>
					<div class="searchInput">
					   <input type="text" name="Remark" />
				   </div>
			   </div>
	
			   <p class="winfont">你确认取消此笔订单吗？</p>
           </form>
       </script>
       
       
        <!--头部名称-->
        <script id="tpl_thead" type="text/html">
            <#for(var i in obj){#>
                <th><#=obj[i]#></th>
            <#}#>
      </script>

        <!--数据部分-->
       <script id="tpl_content" type="text/html">
           <#for(var i=0,idata;i<list.finalList.length;i++){ idata=list.finalList[i]; #>
           <tr data-yearamount="<#=list.otherItems[i].YearAmount #>" data-vipid="<#=list.otherItems[i].VIPID#>" data-vipcardid="<#=list.otherItems[i].VipCardID #>" data-vipcardtypeid="<#=list.otherItems[i].VipCardTypeID#>">
               <#for(var e in idata){#>
               <td>

                    <#if(e.toLowerCase()=='vipcardcode'){#>
                             <p class="textLeft"><#= idata[e]#>
                             <#if(list.finalList[i].PayStatus!='已付款'){#>
                                   <b class="fontC" data-type="payment">收款</b>
                              <#}#>
                              </p>
                   <# }else{#>
                       <#= idata[e]#>
                   <#}#>

              </td>
               <#}#>
           </tr>
           <#} #>
       </script>
</asp:Content>

