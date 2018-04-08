﻿/*
Jermyn 2013-04-02
*/

Ext.Loader.setConfig({
    enabled: true
});
Ext.Loader.setPath('Ext.ux', '/Lib/Javascript/Ext4.1.0/ux');
Ext.require([
    'Ext.grid.*',
    'Ext.data.*',
    'Ext.util.*',
    'Ext.state.*',
    'Ext.form.*',
    'Ext.ux.CheckColumn'
]);

Ext.onReady(function () {
    //加载需要的文件
    InitVE();
    InitStore();
    InitView();

    //页面加载
    //JITPage.PageSize.setValue(15);
    JITPage.HandlerUrl.setValue("Handler/InoutHandler.ashx?mid=" + __mid);

    fnSearch();
});

function fnCreate() {
    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: detailWinHeight,
        id: "MvInOutOrderEdit",
        title: "调拨单明细管理",
        url: "MvInOutOrderEdit.aspx?mid=" + __mid + "&op=new"
    });
    win.show();

}

function fnSearch() {
    Ext.getStore("salesOutOrderStore").proxy.url = JITPage.HandlerUrl.getValue()
        + "&method=MvInOutOrder";
    Ext.getStore("salesOutOrderStore").pageSize = JITPage.PageSize.getValue();

    Ext.getStore("salesOutOrderStore").proxy.extraParams = {
        form: Ext.JSON.encode(Ext.getCmp("searchPanel").getValues())
         , purchase_unit_id: Ext.getCmp("txtPurchaseUnit").jitGetValue()
        , sales_unit_id: Ext.getCmp("txtSalesUnit").jitGetValue()
    };
    Ext.getStore("salesOutOrderStore").load();
}

function fnView(id, op) {
    if (id == undefined || id == null) id = "";
    //if (op == undefined || op == null) op = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 500,
        id: "MvInOutOrderEdit",
        title: "调拨单明细管理",
        url: "MvInOutOrderEdit.aspx?order_id=" + id + "&op=" + op
    });
    win.show();

}
function fnDelete(id) {
    //    //debugger;
    JITPage.deleteList({
        ids: JITPage.getSelected({ gridView: Ext.getCmp("gridView"), id: "order_id" }),
        url: JITPage.HandlerUrl.getValue() + "&method=InoutOrderDelete",
        params: {
            ids: JITPage.getSelected({ gridView: Ext.getCmp("gridView"), id: "order_id" })
        },
        handler: function () {
            Ext.getStore("salesOutOrderStore").load();
        }
    });
}
function fnPass(id) {
    if (!confirm("确认审核?")) return;
    if (id == undefined || id == null) id = "";
    Ext.Ajax.request({
        method: 'GET',
        sync: true,
        url: 'Handler/InoutHandler.ashx?method=InoutOrderPass&order_id=' + id,
        params: {},
        success: function (result, request) {
            var d = Ext.decode(result.responseText);
            if (!d.success) {
                alert("操作失败：" + d.msg);
            } else {
                alert("审核成功");
                fnSearch();
            }
        },
        failure: function (result) {
            alert("操作失败：" + result.responseText);
        }
    });
}

function fnColumnUpdate(value, p, r) {

}

function fnColumnDelete(value, p, r) {
    return "<a style='color:blue;' href=\"javascript:;\" onclick=\"fnDelete()\">删除</a>";
}

function fnColumnMustDo(value, p, r) {
    return (value == 1 ? "√" : "")
}

function fnMoreSearchView(type) {
    if (!Ext.getCmp("searchPanel").isExpand) {
        document.getElementById("view_Search").style.height = "104px";
        Ext.getCmp("searchPanel").isExpand = true;
        Ext.getCmp("txtOrderDate").hidden = false;
        Ext.getCmp("txtOrderDate").setVisible(true);
        Ext.getCmp("txtPurchaseUnit").hidden = false;
        Ext.getCmp("txtPurchaseUnit").setVisible(true);
        Ext.getCmp("txtPurchaseWarehuouse").hidden = false;
        Ext.getCmp("txtPurchaseWarehuouse").setVisible(true);
        Ext.getCmp("txtOrderStatus").hidden = false;
        Ext.getCmp("txtOrderStatus").setVisible(true);
        Ext.getCmp("btnMoreSearchView").setText(SearchPanelMoreBtnHideText);
        Ext.getCmp("searchPanel").doLayout();
    } else {
        document.getElementById("view_Search").style.height = "44px";
        Ext.getCmp("searchPanel").isExpand = false;
        Ext.getCmp("txtOrderDate").hidden = true;
        Ext.getCmp("txtOrderDate").setVisible(false);
        Ext.getCmp("txtPurchaseUnit").hidden = true;
        Ext.getCmp("txtPurchaseUnit").setVisible(false);
        Ext.getCmp("txtPurchaseWarehuouse").hidden = true;
        Ext.getCmp("txtPurchaseWarehuouse").setVisible(false);
        Ext.getCmp("txtOrderStatus").hidden = true;
        Ext.getCmp("txtOrderStatus").setVisible(false);
        Ext.getCmp("btnMoreSearchView").setText(SearchPanelMoreBtnText);
        Ext.getCmp("searchPanel").doLayout();
    }
}
