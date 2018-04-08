﻿Jit.AM.defindPage({
    name: 'Hot',
    onPageLoad: function () {
        //当页面加载完成时触发
        Jit.log('进入Hot.....');
        this.initEvent();
    },
    initEvent: function () {
        var me = this;
        me.windowHeight = window.innerHeight;
        me.windowWidth = window.innerWidth;

        /*页面异步数据请求_获取酒店详细信息*/
        me.ajax({
            url: '../../../OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getStoreDetail',
                'storeId': me.getUrlParam('storeId')
            },
            success: function (data) {
                me.loadStoreData(data.content);
            }
        });
    },
    loadStoreData: function (data) {
        if (data.hotContent != null && data.hotContent != "") {
            $('[jitval=info]').html(data.hotContent);
        } else {
            $('[jitval=info]').html('暂无内容');
        }
    },
    urlGoTo: function (url) {
        var me = this;
        me.toPage(url, '&storeId=' + me.getUrlParam('storeId'));
    }
});