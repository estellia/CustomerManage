﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/CPOS.Master" 
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>微信模板分类管理</title>
    
    <script src="/Framework/javascript/Biz/WModel.js" type="text/javascript"></script>
    <script src="/Framework/javascript/Biz/Status.js" type="text/javascript"></script>
    <script src="/Framework/javascript/Biz/WApplicationInterface.js" type="text/javascript"></script>
    <script src="/Framework/javascript/Biz/UnitSelectTree.js" type="text/javascript"></script>

    <script src="Controller/WModelListCtl.js" type="text/javascript"></script>
    <script src="Model/WModelListVM.js" type="text/javascript"></script>
    <script src="Store/WModelListVMStore.js" type="text/javascript"></script>
    <script src="View/WModelListView.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="section">
        <div class="m10 article">
            <div class="art-tit">
                <div id="view_Search" class="view_Search" style="height:44px;">
                    <div id='span_panel' style="float:left; width:620px;"></div>
                    <div id='btn_panel' style="float:left; width:200px;"></div>
                </div>
            </div>
            <div class="art-titbutton">
                <div class="view_Button">
                    <span id='span_create'></span>
                </div>
            </div>
            <div class="DivGridView" id="DivGridView">
            </div>
            <div class="cb">
            </div>
        </div>
    </div>
    <input type="hidden" id="hAppId" value="" />
</asp:Content>
