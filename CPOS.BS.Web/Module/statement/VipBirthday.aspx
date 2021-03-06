﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/light.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8" />
    <title>会员生日统计</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/style.css?v=0.4" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="allPage" id="section" data-js="js/VipBirthday.js?ver=0.1">
        <!-- 内容区域 -->
        <div class="contentArea_vipquery">
            <!--个别信息查询-->
            <div class="queryTermArea" id="simpleQuery" >
                <div class="item">
                <form></form>
                           <form id="seach">
                            <div class="commonSelectWrap">
                              <em class="tit">生日月：</em>
                              <div class="selectBox">
                               <input id="Month" data-flag="Month" name="Month" type="text" value="">
                              </div>
                            </div>
                               <div class="commonSelectWrap">
                                   <em class="tit">办卡门店：</em>
                                    <div class="selectBox">
                                     <input id="unitTree" name="UnitID"  class="easyui-combotree" data-options="width:160,height:32"/>
                                    </div>

                               </div>

                                 <div class="commonSelectWrap">
                                      <em class="tit">性别：</em>
                                            <div class="selectBox">
                                                 <input id="Gender" name="Gender"  class="easyui-combobox" data-options="width:160,height:32" />
                                            </div>
                                    </div>
                                  <div class="commonSelectWrap">
                                                                        <em class="tit">卡状态：</em>
                                                                        <div class="selectBox">
                                                                          <input id="payment" name="VipCardStatusId"  class="easyui-combobox" data-options="width:160,height:32"/>
                                                                        </div>
                                                                 </div>
                                   <div class="commonSelectWrap">
                                                                         <em class="tit">最近消费：</em>
                                                                         <div class="selectBox">
                                                                           <input id="Consumption" name="Consumption"  class="easyui-combobox" data-options="width:160,height:32"/>
                                                                         </div>
                                                                  </div>

                            </form>
                </div>
                <div class="itemBtn">
                    <div class="moreQueryWrap">
                        <a href="javascript:;" class="commonBtn queryBtn select">查询</a>
                    </div>
                </div>
            </div>
            <div class="tableWrap cursorDef" id="tableWrap">
                <div class="dataTable gridLoading" id="gridTable">
                    <div class="loading">
                        <span>
                            <img src="../static/images/loading.gif"></span>
                    </div>
                </div>
                <div id="pageContianer">
                    <div class="dataMessage">
                        数据没有对应记录</div>
                    <div id="kkpager">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none">
        <div id="win" class="easyui-window" data-options="modal:true,shadow:false,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true">
            <div class="easyui-layout" data-options="fit:true" id="panlconent">
                <div data-options="region:'center'" style="padding: 10px;">
                    指定的模板添加内容
                </div>
                <div class="btnWrap" id="btnWrap" data-options="region:'south',border:false" style="height: 80px;
                    text-align: center; padding: 5px 0 0;">
                    <a class="easyui-linkbutton commonBtn saveBtn">确定</a> <a class="easyui-linkbutton commonBtn cancelBtn"
                        href="javascript:void(0)" onclick="javascript:$('#win').window('close')">取消</a>
                </div>
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
    <script type="text/javascript" src="<%=StaticUrl+"/Module/static/js/lib/require.min.js"%>"
        defer async="true" data-main="<%=StaticUrl+"/module/commodity/js/main.js"%>"></script>
</asp:Content>
