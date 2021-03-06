﻿Jit.AM.defindPage({

    name: 'Activity',
    elements: {
        broExplain: '',
        broMore: '',
        btInEmba: ''
    },
    onPageLoad: function() {

        //当页面加载完成时触发
        Jit.log('页面进入' + this.name);

        this.initLoad();
        this.initEvent();
    }, //加载数据
    initLoad: function() {


        //测试清空缓存
        var self = Jit.AM;
        // baseInfo = self.getBaseAjaxParam();

        // baseInfo.userId = '';
        // Jit.AM.setBaseAjaxParam(baseInfo);
        // localStorage.clear();

        this.elements.broExplain = $('#broExplain');
        this.elements.broMore = $('#broMore');



        // WeixinJSBridge.on('menu:share:timeline', function(argv) {
        //     WeixinJSBridge.invoke('shareTimeline', {
        //         "title": "xxx",
        //         "link": "xxxx",
        //         "desc": "关注xxx",
        //         "img_url": "xxxx"
        //     }, function(res) {});
        // });

        // WeixinJSBridge.on('menu:share:appmessage', function(argv) {
        //     WeixinJSBridge.invoke('sendAppMessage', {
        //         "appid": appId,
        //         "img_url": imgUrl,
        //         "img_width": "640",
        //         "img_height": "640",
        //         "link": "http://yige.org/php/",
        //         "desc": "一个网在线教程",
        //         "title": "yige.org"
        //     }, function(res) {})
        // })



    },
    initEvent: function() {
        var self = this;
        self.elements.broMore.bind('click', function() {
            self.elements.broExplain.toggleClass('moreshow');
        });

        $('.commonHideTit').bind('click', function() {
            var me = $(this),
                v = me.data(KeyList.val);
            if (v && v == 1) {
                me.find('em').removeClass('open');
                me.data(KeyList.val, 0);
                me.find('.pTag').hide();
            } else {
                me.find('em').addClass('open');
                me.data(KeyList.val, 1);
                me.find('.pTag').show();
            }

        });



    }

});

