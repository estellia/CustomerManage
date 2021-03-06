﻿Ext.Loader.setConfig({
    enabled: true
});

Ext.Loader.setPath('Ext.ux', '/Lib/Javascript/Ext4.1.0/ux');

Ext.require(['Ext.grid.*', 'Ext.data.*', 'Ext.util.*', 'Ext.state.*', 'Ext.form.*', 'Ext.ux.CheckColumn']);

Ext.onReady(function () {
    //加载需要的文件
    InitVE();
    InitStore();
    InitView();

    //页面加载
    JITPage.HandlerUrl.setValue("Handler/EventsHandler.ashx?mid=" + __mid);

    fnSearch();
});

function fnCreate() {
    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 550,
        id: "EventsEdit",
        title: "活动内容",
        url: "EventsEdit.aspx?mid=" + __mid + "&op=new"
    });

    win.show();
}

function fnSearch() {
    var beginDate = Ext.getCmp("txtPublishDateBegin").jitGetValue();
    var endDate = Ext.getCmp("txtPublishDateEnd").jitGetValue();
    if (endDate != "" && beginDate > endDate) {
        alert("起始时间不能大于终止时间");
        return;
    }
    Ext.getStore("eventsStore").proxy.url = JITPage.HandlerUrl.getValue() + "&method=events_query";
    //alert(Ext.getCmp("txtPublishDateBegin").getValue())
    Ext.getStore("eventsStore").pageSize = JITPage.PageSize.getValue();
    Ext.getStore("eventsStore").proxy.extraParams = {
        form: Ext.JSON.encode(Ext.getCmp("searchPanel").getValues()),
        pEventTypeID: Ext.getCmp("cmbEventTypeID").jitGetValue()
    };

    Ext.getStore("eventsStore").load();
}

function fnView(id) {
    if (id == undefined || id == null) id = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 530,
        id: "EventsEdit",
        title: "活动内容",
        url: "EventsEdit.aspx?EventID=" + id
    });

    win.show();
}

function fnViewUserList(id, type) {
    if (id == undefined || id == null) id = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 550,
        id: "EventsUserList",
        title: "活动人员",
        url: "EventsUserList.aspx?EventID=" + id
    });

    win.show();
}

function fnViewUserList2(id, type) {
    if (id == undefined || id == null) id = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 550,
        id: "EventsUserList",
        title: "活动人员",
        url: "EventsUserList2.aspx?EventID=" + id
    });

    win.show();
}

function fnViewPrizesList(id, type) {
    if (id == undefined || id == null) id = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 550,
        id: "EventsPrizesList",
        title: "活动奖品",
        url: "EventsPrizesList.aspx?EventID=" + id
    });

    win.show();
}
function fnViewPrizesWinnerList(id, type) {
    if (id == undefined || id == null) id = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 550,
        id: "PrizesWinnerList",
        title: "中奖人员",
        url: "PrizesWinnerList.aspx?EventID=" + id
    });

    win.show();
}
function fnViewRoundList(id, type) {
    if (id == undefined || id == null) id = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 550,
        id: "EventsRoundList",
        title: "活动轮次",
        url: "EventsRoundList.aspx?EventID=" + id
    });

    win.show();
}

function fnViewLotteryLogList(id, type) {
    if (id == undefined || id == null) id = "";

    var win = Ext.create('jit.biz.Window', {
        jitSize: "large",
        height: 550,
        id: "EventsLotteryLogList",
        title: "抽奖日志",
        url: "EventsLotteryLogList.aspx?EventID=" + id
    });

    win.show();
}

function fnDelete(id) {
    JITPage.deleteList({
        ids: JITPage.getSelected({
            gridView: Ext.getCmp("gridView"),
            id: "EventID"
        }),
        url: JITPage.HandlerUrl.getValue() + "&method=events_delete",
        params: {
            ids: JITPage.getSelected({
                gridView: Ext.getCmp("gridView"),
                id: "EventID"
            })
        },
        handler: function () {
            Ext.getStore("eventsStore").load();
        }
    });
}

fnReset = function () {
    Ext.getCmp("cmbEventTypeID").jitSetValue("");
    Ext.getCmp("txtTitle").jitSetValue("");
    Ext.getCmp("txtPublishDateBegin").jitSetValue("");
    Ext.getCmp("txtPublishDateEnd").jitSetValue("");
}

function fnViewUserList_New(pEventID) {
    var parameters = "?mid=" + __mid + "&r=" + Math.random() + "&pEventID=" + pEventID;
    var src = "/Module/XieHuiBao/Events/Events.aspx" + parameters;

    Ext.getCmp("userListWin").show();
    document.getElementById("frame").setAttribute("src", src);
}