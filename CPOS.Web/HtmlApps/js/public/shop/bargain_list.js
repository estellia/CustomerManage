Jit.AM.defindPage({

    name: 'BargainList',
    element: {
        EventId:'',
        PageSize: 5,
        PageIndex: 0
    },
    initWithParam: function (param) {

    },
    onPageLoad: function () {
        this.loadData();
        this.initEvent();
    },
    loadData: function () {
        var that = this;
        that.GetKJEventJoinList(function (_data) {
            if (_data.KJEventJoinInfoList) {
                $("#orderList").html(template.render("tplListItem", _data));
            }
        });
         
    },

    initEvent: function () {
        var that = this;
        $("#orderList").on("click", ".deletebargain", function () {
            if (confirm("是否要删除？")) {
                that.DeleteKJEventJoin($(this).data("kjeventjoinid"), function () {
                    that.loadData();
                });
            }
        });

        $("#orderList").on("click", ".btn_buy", function () {
            var skuid = $(this).data("skuid"),
                salesprice = $(this).data("salesprice"),
                kjeventjoinid = $(this).data("kjeventjoinid"),
                qty = $(this).data("qty");
            that.element.EventId = $(this).data("eventid");
            that.setOrderInfo(skuid, salesprice, qty,kjeventjoinid, function (_data) {

                that.toPage('GoodsOrder', '&orderId=' + _data.orderId + "&KJEventJoinId=" + kjeventjoinid + "&eventId=" + that.element.EventId);
            });
            
        });
    },
    GetKJEventJoinList: function (callback) {//砍价列表
        var that = this;

        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
                'action': 'WEvent.Bargain.GetKJEventJoinList',
                'PageIndex': that.element.PageIndex,
                'PageSize': that.element.PageSize
            },
            success: function (data) {
                if (data.IsSuccess && data.ResultCode == 0) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }

                } else {
                    alert(data.Message);
                }
            }
        });

    },
    DeleteKJEventJoin: function (id,callback) {//砍价列表
        var that = this;

        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
                'action': 'WEvent.Bargain.DeleteKJEventJoin',
                'KJEventJoinId': id,
            },
            success: function (data) {
                if (data.IsSuccess && data.ResultCode == 0) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }

                } else {
                    alert(data.Message);
                }
            }
        });

    },
    setOrderInfo: function (skuid,scalePrice, num,KJEventJoinId, callback) {//提交
        var that = this;
        var list = [{
            'skuId': skuid,
            'salesPrice': scalePrice,
            'qty': num
        }];
        Jit.UI.Loading(true);
        that.ajax({
            url: "/Interface/data/ItemData.aspx",
            data: {
                'action': 'setOrderInfo',
                'qty': list[0].qty,
                'totalAmount': list[0].qty * list[0].salesPrice,
                'orderDetailList': list,
                'isBargain': 1,
                'eventId': that.element.EventId,
				'KJEventJoinId':KJEventJoinId,
                'reqBy': 3
            },
            success: function (data) {
                if (data.code == "200") {
                    var data = data.content;
                    if (callback) {
                        callback(data);
                    }

                } else {
                    alert(data.description);
                }
            },
            complete: function () {
                Jit.UI.Loading(false);
            }
        });

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