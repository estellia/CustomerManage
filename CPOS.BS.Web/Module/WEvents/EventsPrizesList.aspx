﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Framework/MasterPage/ChildPage.Master"
    ValidateRequest="false" AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITChildPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>活动奖品</title>
    <script src="Model/EventsVM.js" type="text/javascript"></script>
    <script src="Model/PrizesVM.js" type="text/javascript"></script>
    <script src="Store/EventsPrizesListVMStore.js" type="text/javascript"></script>
    <script src="View/EventsPrizesListView.js" type="text/javascript"></script>
    <script src="Controller/EventsPrizesListCtl.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section">
        <div class="m10 article">
            <div class="art-titbutton">
                <div class="view_Button">
                    <span id='span_create'></span>
                </div>
            </div>
            <div class="DivGridView" id="DivGridView">
            </div>
            <div class="DivGridView" id="divBtn">
            </div>
            <div class="cb">
            </div>
        </div>
    </div>
</asp:Content>
