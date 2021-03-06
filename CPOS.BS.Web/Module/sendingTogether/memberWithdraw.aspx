﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/CPOS.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<meta charset="UTF-8" />
    <title>会员提现管理</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../static/css/kkpager.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="css/sendingTogether.css"/>

</asp:Content>
<asp:Content ID="Content2"  ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<body cache>
<div class="allPage" id="section" data-js="js/memberWithdraw.js?ver=0.1">
    <!-- 内容区域 -->
    <div class="contentArea_vipquery">
        <!--个别信息查询-->
        <div class="queryTermArea" id="simpleQuery" style="display:inline-block;width: 100%; " >
             <form></form>
            <form id="queryFrom"> <div class="item">
            <div class="commonSelectWrap">
                <em class="tit">提现单号：</em>
                <label class="searchInput"><input  name="WithdrawNo" type="text" value=""></label>
            </div>
            <div class="commonSelectWrap">
                <em class="tit">会员名称：</em>
                <label class="searchInput"><input   name="VipName" type="text" value=""></label>
            </div>
            
            <div class="commonSelectWrap">
                <em class="tit">状态：</em>
                <div class="selectBox bordernone">
                  <input id="cc" class="easyui-combobox" name="Status"  />



                </div>
            </div>
            </div>
             </form>
            
               <div class="moreQueryWrap">
                                       <a href="javascript:;" class="commonBtn queryBtn">查询</a>
                                       </div>
        </div>

        <!--表格操作按钮-->
        <div id="menuItems" class="optionBtn">
            <!--<span class="commonBtn _addVip">添加新会员</span>-->
            <span class="commonBtn exportBtn w80 r" style="display:none;">打印</span>
            <span class="commonBtn affirmBtn w80 l" data-statusid=1>确认</span>
            <span class="commonBtn finishBtn w80 l" data-statusid=2>完成</span>

        </div>    
        <div class="tableWrap">
            <div class="dataTable"  id="dataTable">

            </div>
            <div id="pageContianer">
             <div class="dataMessage" >没有符合条件的查询记录</div>
                <div id="kkpager" style="text-align:center;"></div>
            </div>
        </div>
    </div>
</div>
    <div style="display: none">
      <div id="win" class="easyui-window" data-options="modal:true,shadow:false,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true" >
      		<div class="easyui-layout" data-options="fit:true" id="panlconent">

      			<div data-options="region:'center'" style="padding:10px;">
      				指定的模板添加内容
      			</div>
      			<div class="btnWrap" id="btnWrap" data-options="region:'south',border:false" style="height:80px;text-align:center;padding:5px 0 0;">
      				<a class="easyui-linkbutton commonBtn saveBtn" >确定</a>
      				<a class="easyui-linkbutton commonBtn cancelBtn"  href="javascript:void(0)" onClick="javascript:$('#win').window('close')" >取消</a>
      			</div>
      		</div>

      	</div>
      </div>
<script type="text/javascript" src="<%=StaticUrl+"/Module/static/js/lib/require.min.js"%>" defer async data-main="<%=StaticUrl+"/module/sendingTogether/js/main.js"%>" ></script>
    </body>
</asp:Content>