﻿Ext.Loader.setConfig({
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
    JITPage.HandlerUrl.setValue("Handler/WQRCodeManagerHandler.ashx?mid=" + __mid);

    fnSearch();
});

function fnCreate() {
    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: detailWinHeight,
        id: "WQRCodeManagerEdit",
        title: "二维码管理",
        url: "WQRCodeManagerEdit.aspx?mid=" + __mid
    });
	win.show();
}

fnSearch = function() {
    var store = Ext.getStore("wQRCodeManagerStore");
    store.proxy.url = JITPage.HandlerUrl.getValue()
        + "&method=search_wQRCodeManager";
    store.pageSize = JITPage.PageSize.getValue();
    store.proxy.extraParams = {
        form: Ext.JSON.encode(Ext.getCmp("searchPanel").getValues())
    };
    //alert(Ext.JSON.encode(Ext.getCmp("searchPanel").getValues()));
    store.load();
}

function fnView(id) {
    if (id == undefined || id == null) id = "";
    //if (op == undefined || op == null) op = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: detailWinHeight,
        id: "WQRCodeManagerEdit",
        title: "二维码管理",
        url: "WQRCodeManagerEdit.aspx?wQRCodeManager_id=" + id
    });
	win.show();
}
function fnDelete(id) {
    if (!confirm("确认删除?")) return;
    Ext.Ajax.request({
        url: JITPage.HandlerUrl.getValue() + "&method=wQRCodeManager_delete",
        params: { ids: id },
        method: 'POST',
        sync: true,
        async: false,
        success: function (response) {
            var d = Ext.decode(response.responseText);
            if (!d.success) {
                alert(d.msg);
                return;
            }
            fnSearch();
        },
        failure: function () {
            Ext.Msg.alert("提示", "获取参数数据失败");
        }
    });
    return true;
}
