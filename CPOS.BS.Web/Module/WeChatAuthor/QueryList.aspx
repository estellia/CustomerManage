﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/light.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="UTF-8" />
    <title>微信授权</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link href="css/style.css?v=0.4" rel="stylesheet" type="text/css" />

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
                                                      <em class="tit">微信账号：</em>
                                                      <label class="searchInput">
                                                          <input data-text="微信账号名称" data-flag="CouponTypeName" name="WeiXinName" type="text"
                                                              value="">
                                                      </label>
                                                  </div>
                                                  <div class="commonSelectWrap">
                                                      <em class="tit">原始ID：</em>
                                                      <div class="searchInput">
                                                         <input data-text="原始ID" data-flag="ParValue"  name="WeiXinID" type="text" value="">
                                                      </div>
                                                  </div>
                                                  <div class="moreQueryWrap">
                                                                             <a href="javascript:;" class="commonBtn queryBtn">查询</a>
                                                                           </div>

                                                  </form>

                        </div>

                    <!--<h2 class="commonTitle">会员查询</h2>-->

                </div>
                <div class="tableWrap" id="tableWrap">
                <div class="optionBtn" id="opt" >
                 <div class="commonBtn sales w120" data-flag="add" id="sales" style="display: none"> 绑定微信账号
                 <div class="authBtn">
                  <iframe id="wxAuth"  width="180px" height="50px" scrolling="0" frameborder="0" src=""></iframe>
                 </div>

                 </div>
                <div class="commonBtn sales ImportWXUser w120" data-flag="ImportWXUser" id="Div1"> 拉取微信粉丝
                </div>
                </div>
                   <div  id="gridTable" class="gridLoading">
                         <div  class="loading">
                                  <span>
                                <img src="../static/images/loading.gif"></span>
                           </div>
                   </div>
                    <div id="pageContianer">
                    <div class="dataMessage" >没有符合条件的查询记录</div>
                        <div id="kkpager" >
                        </div>
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
      				<a class="easyui-linkbutton commonBtn cancelBtn"  href="javascript:void(0)" onclick="javascript:$('#win').window('close')" >取消</a>
      			</div>
      		</div>

      	</div>
      </div>


        <!--收款-->
         <script id="tpl_AddNumber" type="text/html">
            <form id="optionForm">


            <div class="optionclass">
               <div class="commonSelectWrap">
                             <em class="tit">数量:</em>
                                <div class="borderNone" >
                                 <input id="Amount" class="easyui-numberbox" data-options="width:180,height:34,min:0,precision:0,max:10000" name="IssuedQty" />
                               </div>
                </div>
            </div>
                </form>
                </script>
    <!--拉取微信粉丝-->
         <script id="tpl_ImportWXUser" type="text/html">
            <form id="Form1">

                            <div class="loadImg"><img src="../WeChatAuthor/images/loading.gif" /><br/>拉取中…</div>
                   <div class="PromptInfor">拉取时间可能较长，请耐心等待，您可以进行其他操作，可在会员管理中查看结果。</div>
            
                </form>
                </script>


       <script type="text/javascript" src="<%=StaticUrl+"/Module/static/js/lib/require.min.js"%>"
            defer async="true" data-main="<%=StaticUrl+"/module/couponManage/js/main.js"%>"></script>
</asp:Content>
