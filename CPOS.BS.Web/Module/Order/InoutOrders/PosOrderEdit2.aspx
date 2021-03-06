﻿<%@ Page Title="POS小票" Language="C#" MasterPageFile="~/Framework/MasterPage/ChildPage.Master"
    AutoEventWireup="true" Inherits="JIT.CPOS.BS.Web.PageBase.JITPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>POS小票</title>
    <script src="/Framework/javascript/Biz/CustomerUnit.js" type="text/javascript"></script>
    <script src="/Framework/javascript/Biz/SupplierUnit.js" type="text/javascript"></script>
    <script src="/Framework/javascript/Biz/OrderStatus.js" type="text/javascript"></script>
    <script src="/Framework/javascript/Biz/UnitSelectTree.js" type="text/javascript"></script>
    <script src="/Framework/javascript/Biz/SkuPropCfg.js" type="text/javascript"></script>
    <script src="/Framework/javascript/Biz/SkuSelect.js" type="text/javascript"></script>
    <script src="/Framework/javascript/biz/Warehouse.js" type="text/javascript"></script>
    <script src="/Framework/javascript/biz/OrderNo.js" type="text/javascript"></script>
    <script src="Controller/PosOrderEdit2Ctl.js" type="text/javascript"></script>
    <script src="Model/InoutOrderEntity.js" type="text/javascript"></script>
    <script src="Model/InoutOrderDetailItemVM.js" type="text/javascript"></script>
    <script src="Store/SalesOutOrderEditVMStore.js" type="text/javascript"></script>
    <script src="View/PosOrderEdit2View.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section">
        <div class="m10 article">
            <div class="z_event_border" style="font-weight: bold; height: 36px; line-height: 36px;
                padding-left: 10px; background: rgb(241, 242, 245);">
                订单主信息</div>
            <div class="DivGridView" id="divMain">
            </div>
            <div class="z_event_border" style="font-weight: bold; height: 36px; line-height: 36px;
                padding-left: 10px; background: rgb(241, 242, 245);">
                配送信息</div>
            <div class="DivGridView" id="divDetail">
            </div>
            <div class="z_event_border" style="font-weight: bold; height: 36px; line-height: 36px;
                padding-left: 10px; background: rgb(241, 242, 245);">
                订单明细</div>
            <div class="DivGridView" id="divDetail1">
                <div style="float: left; width: 100%; margin-left: 0px; background: rgb(241, 242, 245);
                    border: 1px solid #d0d0d0; border-top: 0px;">
                    <div style="clear: both; width: 100%; padding-left: 10px; padding-right: 10px; padding-top: 10px;
                        padding-bottom: 10px;">
                        <div id="grid">
                        </div>
                    </div>
                </div>
            </div>
            <div class="DivGridView" id="divBtn">
            </div>
            <div class="cb">
            </div>
        </div>
    </div>
</asp:Content>
