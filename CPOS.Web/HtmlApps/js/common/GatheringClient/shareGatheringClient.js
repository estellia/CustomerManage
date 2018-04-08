Jit.AM.defindPage({
    name: 'shareGethringClient',
    initWithParam: function (param) {
    },
    element: {
        paraTmp: '' //8位的随机数
    },
    onPageLoad: function () {
        this.initPage();
    },
    initPage: function () {
        var that = this;
        Jit.WX.OptionMenu(false);
        that.loadData();
        that.initEvent();
    },
    loadData:function()
    {
        var that = this;
        that.GetSetoffToolList(function (_data) {
            var html = template.render('tpl_gc_content', _data);
            $(".gc_list").html(html);
            if (_data.SetoffRegAwardType == 1) {
                $(".SetoffRegUnit").html("元");
                $(".SetoffRegPrize").html(_data.SetoffRegPrize ? _data.SetoffRegPrize : 0);
            } else {
                $(".SetoffRegUnit").html("积分");
                $(".SetoffRegPrize").html(_data.SetoffRegPrize ? _data.SetoffRegPrize : 0);
            }
            $(".SetoffOrderPer").html(_data.SetoffOrderPer?_data.SetoffOrderPer:0);
        });

        that.getDimensionalCode(function (_data) {
            $(".QRcode img").attr("src", _data.imageUrl);
            that.element.paraTmp = _data.paraTmp;
            //等待扫一扫
            QrTimer = setInterval(function () {
                that.getDimensionalCodeByVipInfo(function (_data1) {
                    if (_data1.ResultCode != 0) {
                        clearInterval(QrTimer);
                        Jit.UI.Dialog({
                            type: "Alert", content: _data1.Message,
                            CallBackOk: function (data) {
                                Jit.UI.Dialog("CLOSE");

                            }
                        });
                    }
                });
            }, 3000)
        });

    },
    initEvent: function () {
        var that = this;
        $(".operation").on("click", "a", function () {
            $(".operation a").addClass("on");
            $(this).removeClass("on");

            //根据点击改变按钮的图标
            $(".operation a img").each(function () {
                var original = $(this).data("original");
                $(this).attr("src", original);
            });
            var on = $(this).find("img").data("on");
            $(this).find("img").attr("src", on);

            //点击需要显示内容
            var showlayer = $(this).data("showlayer");
            $(".layer").hide();
            $("." + showlayer).show();

        });


        $(".gc_list").on("click", ".gc_content", function () {
            var tooltype = $(this).data("tooltype");
            var id = $(this).data("id");
            var setofftoolid = $(this).data("setofftoolid");
            var URL = $(this).data("url");
            var info = Jit.AM.getBaseAjaxParam();
            $(this).parents(".gc_content").find(".title span").remove();
            if (tooltype == "SetoffPoster")
            {
                Jit.AM.toPage('poster', '&ShareVipId=' + info.userId + "&ObjectId=" + id + "&SetoffToolID=" + setofftoolid);
            }

            if (tooltype == "Coupon") {
                Jit.AM.toPage('GCCoupon', "&couponId=" + id + "&SetoffToolID=" + setofftoolid);
            }

            if (tooltype == "CTW") {
                that.GetSetoffToolDetail(id, setofftoolid, function () {

                });
                location.href = URL;
            }

        });



    },
    //获取分享集客列表数据
    GetSetoffToolList: function (callback) {
        var that = this;
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.VipGolden.GetSetoffToolList',
                'ApplicationType': 1,
                'PageIndex': 1,
                'PageSize': 100
            },
            success: function (data) {
                if (data.IsSuccess && data.Data) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }


                } else {
                    Jit.UI.Dialog({
                        type: "Alert", content: data.Message,
                        CallBackOk: function (data) {
                            Jit.UI.Dialog("CLOSE");

                        }
                    });
                }
            }
        });

    },
    //获取面对面集客数据
    getDimensionalCode: function (callback) {
        var that = this;
        that.ajax({
            url: '/ApplicationInterface/Stores/StoresGateway.ashx',
            data: {
                'action': 'getDimensionalCode',
                'PlatformType': 1,
                'VipDCode': 1
            },
            success: function (data) {
                if (data.IsSuccess && data.Data) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }


                } else {
                    Jit.UI.Dialog({
                        type: "Alert", content: data.Message,
                        CallBackOk: function (data) {
                            Jit.UI.Dialog("CLOSE");

                        }
                    });
                }
            }
        });

    },
    //扫描二维码触发事件
    getDimensionalCodeByVipInfo: function (callback) {
        var that = this;
        that.ajax({
            url: '/ApplicationInterface/Stores/StoresGateway.ashx',
            data: {
                'action': 'getDimensionalCodeByVipInfo',
                'special': {
                    'Mode': "ToFace",
                    'PlatformType': 3,
                    'paraTmp': that.element.paraTmp
                }
                
            },
            success: function (data) {
                //Jit.UI.Loading(false);
                if (data) {
                    if (data.IsSuccess && data.Data) {
                        if (callback) {
                            callback(data);
                        }


                    } else {
                        clearInterval(QrTimer);
                        Jit.UI.Dialog({
                            type: "Alert", content: data.Message,
                            CallBackOk: function (data) {
                                Jit.UI.Dialog("CLOSE");

                            }
                        });
                    }
                }

               
            }
        });
    }, //获取优惠卷信息及二维码信息及分享二维码
    GetSetoffToolDetail: function (id, SetoffToolID, callback) {
        var that = this;
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.VipGolden.GetSetoffToolDetail',
                'ToolType': "CTW",
                'ObjectId': id,
                'SetoffToolID':SetoffToolID,
                'PlatformType': 1,
                'VipDCode': 4
            },
            success: function (data) {
                if (data.IsSuccess && data.Data) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }


                } else {
                    Jit.UI.Dialog({
                        type: "Alert", content: data.Message,
                        CallBackOk: function (data) {
                            Jit.UI.Dialog("CLOSE");

                        }
                    });
                }
            }
        });

    },
    showSharePanel: function () {
        $('#shareMask').show();
    },
    hideSharePanel: function () {
        $('#shareMask').hide();
    }
    

});