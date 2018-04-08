Jit.AM.defindPage({
    name: 'GoodsBargainDetail',
    element: {
        ScreenWidth:0,      //屏幕宽度
        EventId: "",           //活动主标识
        ItemId: "",            //商品Id
        ItemName: '',            //商品名称
        shareimgurl: '',            //分享图片
        VipId: "",           //发起者Id
        SkuId: "",           //规格Id
        isself: 0,            //是否是自己发起砍价
        type: 1,             //1=抢购商品详情；2=帮砍商品
        Disablebtn:false ,   //禁用操作按钮
        EventSKUMappingId: '',
        cleartimer:null,    //计时器
        ismycart: false,  //我也要砍价
        isPromoted: 0,  //是否发起了砍价
        KJEventJoinId:'',
        isJoinInKJEvent:false,//是否执行了JoinInKJEvent事件
        Prop1DetailId: '',
        Prop2DetailId: '',
        SkuInfoList:[],   //商品sku信息
        limitAmount:1,  //限购数量
        timeStr:0,  //抢购时间秒
        PageSize:1000,
        PageIndex:0
    },
    JoinInKJEvent: {
        MinPrice: ""           //原价
    },
    initWithParam: function (param) {
        if (param.commentHide) {
            $('#commentArea').hide();
        };
        if (param['gdeDisplay'] == false) {
            $('#ppTitle').hide();
            $('#ppBody').hide();
        }
    },
    onPageLoad: function () {
        var that = this;
        
        that.element.ScreenWidth = $(window).width();
        that.element.ItemId = that.getUrlParam('goodsId') || that.getParams('goodsId');
        that.element.EventId = that.getUrlParam('eventId') || that.getParams('eventId');
        that.element.type = that.getUrlParam('type') ? that.getUrlParam('type') : 1;
        that.element.isself = that.getUrlParam('isself') ? that.getUrlParam('isself') : "0";
        if (that.element.type == 2) {
            that.element.VipId = that.getUrlParam('VipId');
            that.element.SkuId = that.getUrlParam('SkuId');
            that.element.KJEventJoinId = that.getUrlParam('KJEventJoinId');
        } 
        that.initEvent();

        that.loadData();

    },
    loadData: function () {
        var that = this;

        that.GetPanicbuyingKJEventDetail(function (_data) {
            if (_data) {


                //赋值
                that.element.ItemName = _data.ItemName;
                that.element.EventSKUMappingId = _data.EventSKUMappingId;
                that.JoinInKJEvent.MinPrice = _data.MinPrice;
                that.element.isPromoted = _data.isPromoted;
                _data.EventId = that.element.EventId;
                _data.type = that.element.type;
                _data.isself = that.element.isself;
                _data.ismycart = that.element.ismycart;
                that.element.SkuInfoList = _data.SkuInfoList;

                

                that.element.Disablebtn = true;

                //获取活动信息状态根据不同状态提示相关信息
                if (that.element.KJEventJoinId != null && that.element.KJEventJoinId != "") {
                    that.GetEventItemStatus(function (_data1) {

                        if (_data1.status != 0) {
                            that.element.Disablebtn = false;
                            if (_data1.status == 2) {//提示该砍价已经失效!
                                that.showbargainconfirm1Panel();
                            } else {//提示该砍价已经失效，您是否需要发起砍价
                                that.showbargainconfirmPanel();
                            }
                        } else {
                            that.element.Disablebtn = true
                        }

                    });


                } else {
                    that.element.Disablebtn = true
                }

                //库存不足禁用按钮并弹出提示
                if (_data.CurrentQty < 1) {
                    that.element.Disablebtn = false;
                    that.showbargainconfirm1Panel();
                }

                //活动已结束
                if (_data.isEventEnd == 0) {
                    that.element.Disablebtn = false;
                    that.showbargainconfirm1Panel();
                }

                //活动未开始
                if (_data.isEventEnd == 2) {
                    that.element.Disablebtn = false;
                }

                if (_data.status == 5) {//您已发起过砍价
                    $('#bargainconfirm3').show();
                }

                _data.Disablebtn = that.element.Disablebtn;

                //禁用按钮样式
                if (_data.Disablebtn) {
                    _data.Disablebtnclass = "";
                } else {
                    _data.Disablebtnclass = "no";
                }

                

                var html = template.render("Tpl_goods_detail", _data);
                $("#goodsDetail").html(html);
                $("#description").html(_data.ItemIntroduce);

                //砍价进度条样式
                var scheduleW = $(".schedule").width();
                var progressW = $(".progress").width();
                var SMarginRight = scheduleW - progressW;
                if (SMarginRight < 50) {
                    $(".schedulestatus").css("margin-right", -SMarginRight + "px");
                    $(".schedulestatus").css("margin-right", -SMarginRight + "px");
                }
                if (progressW < 50) {
                    $(".schedulestatus").css("margin-right", (progressW - 100) + "px");
                } else {
                    $(".BargainedPrice").css({
                        "right":"5px","left": "initial"});
                }
                //砍价进度条样式end


                if (that.element.ScreenWidth == 0) {
                    that.element.ScreenWidth = 320;
                }


                $(".thisseasontouchslider-item").css("width", that.element.ScreenWidth + "px");
                $(".thisseasontouchslider-item img").css("max-width", that.element.ScreenWidth + "px");
                $(".thisseasontouchslider-item img").css("height", "200px");

                that.GetBuyerList(function (_data1) {
                    if (_data1.BuyerList) {
                        $(".Buyercount").html(_data1.BuyerList.length);
                    }
                });


                if (that.element.KJEventJoinId != null && that.element.KJEventJoinId != "") {
                    that.GetHelperList(function (_data1) {
                        $(".Cutlist").html("")
                        $(".Cutlist").html(template.render("tpl_cutList", _data1));
                        if (_data1.HelperList) {
                            $(".Helpercount").html(_data1.HelperList.length);
                        }
                    });
                }



                that.element.limitAmount = _data.SinglePurchaseQty ? _data.SinglePurchaseQty : 1;

                if ($(".Prop1 a").get(0)) {
                    $(".Prop1 a").get(0).click();
                } else {
                    if (_data.Prop1List && _data.Prop1List.length > 0) {
                        that.element.SkuId = _data.Prop1List[0].skuId;
                    }
                }


                var timestr = $("#spareTime").data("seconds")
                that.SetSpareTime(timestr);


                if (_data.ImageList) {

                    if (_data.ImageList.length > 0) {
                        that.element.shareimgurl = _data.ImageList[0].imageURL;
                    }

                    //赋值导航start
                    var navhtml = "<span class='thisseasontouchslider-prev'></span>";
                    for (i = 0; i < _data.ImageList.length; i++) {
                        if (i == 0)
                            navhtml += " <span class='thisseasontouchslider-nav-item thisseasontouchslider-nav-item-current'></span>";
                        else
                            navhtml += " <span class='thisseasontouchslider-nav-item '></span>";
                    }
                    navhtml += "<span class='thisseasontouchslider-next'></span>";

                    $(".thisseasontouchslider-nav").html(navhtml);
                    //赋值导航end


                    $(".touchsliderthisseason").touchSlider({
                        container: this,
                        duration: 350, // 动画速度
                        delay: 3000, // 动画时间间隔
                        margin: 5,
                        mouseTouch: true,
                        namespace: "touchslider",
                        next: ".thisseasontouchslider-next", // next 样式指定
                        pagination: ".thisseasontouchslider-nav-item",
                        currentClass: "thisseasontouchslider-nav-item-current", // current 样式指定
                        prev: ".thisseasontouchslider-prev", // prev 样式指定
                        autoplay: true, // 自动播放
                        viewport: ".thisseasontouchslider-viewport"
                    });
                }

                if (that.element.KJEventJoinId != null && that.element.KJEventJoinId != "") {
                    that.shareBargain();
                } else {
                    that.share();
                }

            }

        });
    },
    share: function () {

        

        //分享设置
        var that = this;
        var shareinfo = {
            title: that.element.ItemName,
            imgUrl: that.element.shareimgurl,
            link: location.href,
            desc: "我发现一个不错的商品，赶紧来看看吧!"
        };
        try {
           
            that.initIsShareEvent(shareinfo);
        } catch (e) {

            alert(e);
        }
    },
    //分享砍价
    shareBargain: function () {
        var that = this;
        var info = Jit.AM.getBaseAjaxParam();
        var _link = location.href;
        _link = _link.replace('?VipId=' + Jit.AM.getUrlParam('VipId'), '');
        _link = _link.replace('&VipId=' + Jit.AM.getUrlParam('VipId'), '');
        _link = _link.replace('?SkuId=' + Jit.AM.getUrlParam('SkuId'), '');
        _link = _link.replace('&SkuId=' + Jit.AM.getUrlParam('SkuId'), '');
        _link = _link.replace('?type=' + Jit.AM.getUrlParam('type'), '');
        _link = _link.replace('&type=' + Jit.AM.getUrlParam('type'), '');
        _link = _link.replace('?isself=' + Jit.AM.getUrlParam('isself'), '');
        _link = _link.replace('&isself=' + Jit.AM.getUrlParam('isself'), '');
        _link = _link.replace('?KJEventJoinId=' + Jit.AM.getUrlParam('KJEventJoinId'), '');
        _link = _link.replace('&KJEventJoinId=' + Jit.AM.getUrlParam('KJEventJoinId'), '');
        _link += '&VipId=' + info.userId;
        _link += '&SkuId=' + that.element.SkuId;
        _link += '&KJEventJoinId=' + that.element.KJEventJoinId;
        _link += '&type=2';
        var shareinfo = {
            title: that.element.ItemName,
            imgUrl: that.element.shareimgurl,
            link: _link,
            desc: "我在砍（" + that.element.ItemName + "）大家快来帮忙砍"
        }

        that.initIsShareEvent(shareinfo);

    },

    SetSpareTime: function () {
        var that = this,
			element = $('#spareTime'),
			secound = 0;

        if (that.element.cleartimer) {
            clearInterval(that.element.cleartimer);
        }

        that.element.timeStr= element.data("seconds");
        function Adjust() {
            second = that.element.timeStr;

            if (second > 2 * 86400) {

                $("#spareTime").html(parseInt(second / 86400)+"<span>天<span>");
            }
            else {
                if (second == 0) {
                    clearInterval(that.element.cleartimer);
                    //self.onPageLoad();
                }
                _h = Math.floor(second / 3600);

                _m = Math.floor((second % 3600) / 60);

                _s = Math.floor(((second % 3600) % 60));

                //超过99小时全部显示99
                _m = _h > 99 ? 99 : _m;
                _s = _h > 99 ? 99 : _s;
                _h = _h > 99 ? 99 : _h;

                that.element.timeStr = --that.element.timeStr;
                //$(dom).attr('time-date', endtime - 1);
                //console.log(_h+' '+_m+' '+_s);

                var h1 = Math.floor(_h / 10) > 9 ? 9 : Math.floor(_h / 10),
                    h2 = Math.floor(_h % 10),
                    m1 = Math.floor(_m / 10),
                    m2 = Math.floor(_m % 10),
                    s1 = Math.floor(_s / 10),
                    s2 = Math.floor(_s % 10),
                    timeStr = h1.toString() + h2.toString() + ':' + m1.toString() + m2.toString() + ':' + s1.toString() + s2.toString();

                element.html(timeStr);
                if (timeStr == '00:00:00') {
                    $('#submitOrderBtn').addClass("on").show();
                    $('#submitOrderBtn').css({ 'background': '#ccc' }).attr('href', 'javascript:;').text('已结束');
                    element.text('已结束');
                }
            }
        }

        that.element.cleartimer = setInterval(Adjust, 1000);
    },
    showSharePanel: function () {
        $('#shareMask').show();
    },
    hideSharePanel: function () {
        $('#shareMask').hide();
    },
    showAlertPanel: function () {
        $('#alert').show();
    },
    hideAlertPanel: function () {
        $('#alert').hide();
        this.loadData();
    },
    showbargainconfirmPanel: function () {
        $('#bargainconfirm').show();
    },
    showbargainconfirm1Panel: function () {
        $('#bargainconfirm1').show();
    },
    hidebargainconfirmPanel: function () {
        $('#bargainconfirm').hide();
    },
    initEvent: function () {
        var that = this;
        $(".goodsDetailArea").on('click', "#btn_delNum", function () {
            var num = $('#goods_number').val();

            num--;

            if (num <= 1) {
                num = 1;
            }

            $('#goods_number').val(num);

        });

        $(".goodsDetailArea").on('click', "#btn_addNum", function () {

            var num = $('#goods_number').val();

            num++;
            if (num <= that.element.limitAmount) {
                $('#goods_number').val(num);
            }

        });


        $(".goodsDetailArea").on("click", ".titlist", function () {
            $(".titlist").removeClass("seleced");
            $(this).addClass("seleced");
            var type = $(this).data("type");
            if (type == 1) {
                that.GetHelperList(function (_data) {
                    $(".Cutlist").html("")
                    $(".Cutlist").html(template.render("tpl_cutList", _data));
                    if (_data.HelperList) {
                        $(".Helpercount").html(_data.HelperList.length);
                    }
                });
            } else if (type == 2) {
                that.GetBuyerList(function (_data) {

                    $(".Cutlist").html("")
                    $(".Cutlist").html(template.render("tpl_BuyerList", _data));
                    if (_data.BuyerList) {
                        $(".Buyercount").html(_data.BuyerList.length);
                    }
                });
            }

        })

        //砍价
        $(".goodsDetailArea").on("click", ".btn_helpcart", function () {

            if (that.element.Disablebtn) {
                that.AddKJEventJoinDetail(function (_data) {
                    $(".infotext span").html(_data.BargainPrice);
                    that.showAlertPanel();
                    that.element.ismycart = true;
                });
            }

        })

        //立即购买
        $(".goodsDetailArea").on("click", ".btn_buy", function () {
            if (that.element.Disablebtn) {
                var scalePrice = $(".scalePrice").val();
                var num = $("#goods_number").val();

                that.setOrderInfo(scalePrice, num, function (_data) {
                    if (that.element.KJEventJoinId != "" && that.element.KJEventJoinId != null) {
                        that.toPage('GoodsOrder', '&orderId=' + _data.orderId + "&KJEventJoinId=" + that.element.KJEventJoinId + "&eventId=" + that.element.EventId);
                    } else {
                        that.toPage('GoodsOrder', '&orderId=' + _data.orderId + "&eventId=" + that.element.EventId);
                    }

                });

            }

        })


        //立即砍价
        $(".goodsDetailArea").on("click", ".btn_nowcart", function () {
            if (that.element.Disablebtn) {

                if ($(".CurrentQty").html() < 1) {
                    $('#bargainconfirm2').show();
                    return;
                }

                if (that.element.isPromoted == 0) {
                    if (!that.element.isJoinInKJEvent) {
                        that.element.isJoinInKJEvent = true;
                        that.JoinInKJEvent(function (_data) {
                            that.element.KJEventJoinId = _data.KJEventJoinId;

                            that.AddKJEventJoinDetail(function (_data) {
                                $(".infotext span").html(_data.BargainPrice);
                                that.showAlertPanel();
                                that.element.isself = 1;
                                that.element.type = 2;
                                that.element.VipId = Jit.AM.getBaseAjaxParam().userId;
                            });

                        });
                    }
                }
            }
            
        });

        //帮砍
        $(".goodsDetailArea").on("click", ".btn_cart", function () {
            that.showSharePanel();
            that.shareBargain();
        })

        $(".goodsDetailArea").on("click", ".Prop1 a", function () {
            $(".Prop1 a").removeClass("selected");
            $(this).addClass("selected");
            that.element.SkuId = $(this).data("skuid");
            that.element.Prop1DetailId = $(this).data("prop_detail_id");

            that.changeprice($(this).data("skuid"));

            that.getSkuProp2List(that.element.Prop1DetailId, function (_data2) {
                $(".Prop2").html(template.render("tpl_PropList", _data2));

                $(".Prop2 a").get(0).click();
            });
        });

        $(".goodsDetailArea").on("click", ".Prop2 a", function () {
            $(".Prop2 a").removeClass("selected");
            $(this).addClass("selected");
            that.element.SkuId = $(this).data("skuid");

            that.changeprice($(this).data("skuid"));
            that.element.Prop2DetailId = $(this).data("prop_detail_id");
        });

        


       
    },
    newBargain: function () {
        var that = this;
        Jit.AM.toPage('GoodsBargainDetail', 'goodsId=' + that.element.ItemId + '&eventId=' + that.element.EventId);
    },
    //点击不同商品规格改变销售价格
    changeprice: function (skuid) {
        var that = this;
        if (that.element.SkuInfoList && that.element.SkuInfoList.length>0) {
            for (var i = 0; i < that.element.SkuInfoList.length; i++) {
                if (skuid == that.element.SkuInfoList[i].skuId) {
                    $(".itemPrice").html(that.element.SkuInfoList[i].BasePrice);
                    $(".MinPrice").html(that.element.SkuInfoList[i].price);
                    $(".CurrentQty").html(that.element.SkuInfoList[i].Stock);
                }
            }
        }
    }
    ,
    GetPanicbuyingKJEventDetail: function (callback) {//抢购商品详情
        debugger;
        var pamas={};
        var that = this;
        if (that.element.type == 1)
        {
            pamas = {
                'action': 'WEvent.Bargain.GetPanicBuyingKJItemDetail',
                'EventId': that.element.EventId,
                'ItemId': that.element.ItemId,
				'KJEventJoinId': that.element.KJEventJoinId,
                'type': that.element.type
            }
        } else if (that.element.type == 2)
        {
            pamas = {
                'action': 'WEvent.Bargain.GetPanicBuyingKJItemDetail',
                'EventId': that.element.EventId,
                'ItemId': that.element.ItemId,
				'KJEventJoinId': that.element.KJEventJoinId,
                'VipId': that.element.VipId,
                'SkuId': that.element.SkuId,
                'type': that.element.type
            }
        }

        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: pamas,
            success: function (data) {
                if (data.IsSuccess && data.ResultCode == 0) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }

                } else {
                    that.showbargainconfirm1Panel();
                    that.element.Disablebtn = false;
                    console.log("GetPanicBuyingKJItemDetail方法:错误信息-" + data.Message);
                }
            }
        });

    },

    GetHelperList: function (callback) {//帮砍人数列表
        var that = this;
        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
                'action': 'WEvent.Bargain.GetHelperList',
                'EventId': that.element.EventId,
                'ItemId': that.element.ItemId,
                'VipId': that.element.VipId,
                'KJEventJoinId': that.element.KJEventJoinId,
                'SkuId': that.element.SkuId,
                'PageSize': that.element.PageSize,
                'PageIndex': that.element.PageIndex
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
    GetBuyerList: function (callback) {//购买者列表
        var that = this;
        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
                'action': 'WEvent.Bargain.GetBuyerList',
                'EventId': that.element.EventId,
                'ItemId': that.element.ItemId,
                'SkuId': that.element.SkuId,
                'PageSize': that.element.PageSize,
                'PageIndex': that.element.PageIndex
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
    GetEventItemStatus: function (callback) {//获取活动信息状态
        var that = this;
        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
                'action': 'WEvent.Bargain.GetEventItemStatus',
                'KJEventJoinId': that.element.KJEventJoinId
            },
            async: false,
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
    JoinInKJEvent: function (callback) {//找人帮砍
        var that = this;
        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
			async:false,
            data: {
                'action': 'WEvent.Bargain.JoinInKJEvent',
                'EventId': that.element.EventId,
                'ItemId': that.element.ItemId,
                'SkuId': that.element.SkuId,
                'Price': that.JoinInKJEvent.MinPrice
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

    AddKJEventJoinDetail: function (callback) {//砍价
        var that = this;
        Jit.UI.Loading(true);
        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
                'action': 'WEvent.Bargain.AddKJEventJoinDetail',
                'EventId': that.element.EventId,
                'ItemId': that.element.ItemId,
                'SkuId': that.element.SkuId,
                'KJEventJoinId': that.element.KJEventJoinId,
                'EventSKUMappingId': that.element.EventSKUMappingId
            },
            success: function (data) {
                if (data.IsSuccess && data.ResultCode == 0) {
                    
                    if (data.Data == null)
                    {
                        alert(data.Message);
                    } else {
                        var data = data.Data;
                        if (callback) {
                            callback(data);
                        }
                    }

                } else {
                    alert(data.Message);
                }

            },
            complete: function () {
                Jit.UI.Loading(false);
            }
        });

    },
  
   
    getSkuProp2List: function (propDetailId,callback) {//根据规格1货取规格2的信息接口
        var that = this;
        that.ajax({
            url: "/OnlineShopping/data/Data.aspx",
            data: {
                'action': 'getSkuProp2List',
                'propDetailId': propDetailId,
                'ItemId': that.element.ItemId,
                'Type': 3,
                'EventId': that.element.EventId
            },
            success: function (data) {
                if (data.code =="200") {
                    var data = data.content;
                    if (callback) {
                        callback(data);
                    }

                } else {
                    alert(data.Message);
                }
            }
        });

    },
    setOrderInfo: function (scalePrice,num,callback) {//提交
        var that = this;
        if (!num || num == ""||num==0)
        {
            num = 1;
        }
        var list = [{
            'skuId': that.element.SkuId,
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
                'reqBy':3,
                'KJEventJoinId':that.element.KJEventJoinId || ''
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
    //是分享活动
    initIsShareEvent: function (shareInfo) {
        var that = this;
        var info = Jit.AM.getBaseAjaxParam();
        //是分享活动 + '&eventId=' + that.eventId
        shareInfo.link = shareInfo.link;
        shareInfo.isAuth = true;//需要高级auth认证
        Jit.WX.shareFriends(shareInfo);
        Jit.WX.shareTimeline(shareInfo);
        
    },
    checkOAuth: function () {

        if (JitCfg.CheckOAuth == 'unAttention') {

            this.toPage('unAttention');

            return false;
        }

        return true;
    }
});