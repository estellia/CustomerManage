Jit.AM.defindPage({

    name: 'GoodsBargainList',
    ele: {
        activityList: $("#activityList"),
        timer:null
    },
    initWithParam: function (param) {

    },
    onPageLoad: function () {
        this.eventTypeId = Jit.AM.getUrlParam("eventTypeId");
        if (!this.eventTypeId) {
            this.eventTypeId = 4;
        }

        this.loadData();
        this.initEvent();
    },
    loadData: function () {
        var self = this,
            paramList = {
                'action': 'WEvent.Bargain.GetPanicbuyingKJEventList',
                'eventTypeId': this.eventTypeId
            };
        self.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: paramList,
            beforeSend: function () {
                Jit.UI.AjaxTips.Loading(true);
            },
            success: function (data) {
                if (data.IsSuccess && data.Data) {
                    if (data.Data) {
                        debugger;
                        self.ele.activityList.html("");
                        var html = template.render("Tpl_goods_list", data.Data);
                        self.ele.activityList.html(html);
                        if (self.ele.timer)
                        {
                            clearInterval(self.ele.timer);
                        }

                        self.ele.timer= setInterval(function () {
                            self.timeDown();
                        }, 1000);
                    } else {
                        self.ele.activityList.html('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">活动已经结束，敬请期待下期！</div>');
                    }
                    self.sharePage();
                } else {
                    self.alert(data.Message);
                }
            },
            complete: function () {
                Jit.UI.AjaxTips.Loading(false);
            }
        });
    },

    initEvent: function () {
        var self = this;
        this.ele.activityList.delegate(".snappedList-item", "click", function () {
            var itemId = $(this).attr("data-itemid"),
                qty = $(this).data("qty"),
                status = $(this).data("status"),
                eventId = $(this).attr("data-eventid");
                Jit.AM.toPage('GoodsBargainDetail', 'goodsId=' + itemId + '&eventId=' + eventId);
        });
    },
    timeDown: function () {
        var self = this;
        var domlist = $('[time-date]'), endtime, second;

        var _h, _m, _s;

        domlist.each(function (idx, dom) {

            endtime = $(dom).attr('time-date');

            second = parseInt(endtime);
            if (second > 2 * 86400)
            {

                $(dom).html("<em class='resize'>" + parseInt(second / 86400) + "</em><span>天<span>");
            }
            else {
                if (second == 0) {
                    self.loadData();
                }
                _h = Math.floor(second / 3600);

                _m = Math.floor((second % 3600) / 60);

                _s = Math.floor(((second % 3600) % 60));

                //超过99小时全部显示99
                _m = _h > 99 ? 99 : _m;
                _s = _h > 99 ? 99 : _s;
                _h = _h > 99 ? 99 : _h;


                $(dom).attr('time-date', endtime - 1);
                //console.log(_h+' '+_m+' '+_s);

                $(dom).find('[tn=time-h-max]').html(Math.floor(_h / 10) > 9 ? 9 : Math.floor(_h / 10));

                $(dom).find('[tn=time-h-min]').html(Math.floor(_h % 10));

                $(dom).find('[tn=time-m-max]').html(Math.floor(_m / 10));

                $(dom).find('[tn=time-m-min]').html(Math.floor(_m % 10));

                $(dom).find('[tn=time-s-max]').html(Math.floor(_s / 10));

                $(dom).find('[tn=time-s-min]').html(Math.floor(_s % 10));
            }
        });
    },
    getDay: function (_unixTime) { //获取剩余天数

        var unixTime = parseInt(_unixTime) * 1000; //转换成JS时间

        var endTime = new Date(unixTime),
            seconds = 1000,
            min = 60 * seconds,
            hour = 60 * min,
            days = 24 * hour,
            now = new Date().getTime(),
            dateDiff = endTime - now,
            tipe;
        if (dateDiff <= 0)
            tipe = 1;
        else {
            tipe = parseInt(dateDiff / days);
        }
        return tipe || 1;
    },

    alert: function (text, callback) {
        Jit.UI.Dialog({
            type: "Alert",
            content: text,
            CallBackOk: function () {
                Jit.UI.Dialog("CLOSE");
                if (callback) {
                    callback();
                }
            }
        });
    },
    sharePage: function () {//分享设置
        var shareInfo = {
            'title': document.title,
            'desc': '我发现一个不错的商品，赶紧来看看吧!',
            'link': location.href,
            'imgUrl': $('img').eq(0).attr('src'),
            'isAuth': true//需要高级auth认证
        };
        Jit.AM.initShareEvent(shareInfo);

    }
});