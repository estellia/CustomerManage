Jit.AM.defindPage({
    name: 'poster',
    initWithParam: function (param) {
    },
    element: {
        ShareVipId:'', //分享人id
        ObjectId: '', //海报标识
        SetoffToolID: '', //工具id
        count:0,
        PlatformType:3, //二维码来源统计。默认3会员 4 超级分销商 1 员工  2是客服；
        paraTmp: '' //8位的随机数
    },
    share:{
        title: document.title ? document.title : "海报分享", //分享标题
        desc: "赶快来看看吧",    //分享描述
        imgUrl:""   //分享图片
    },
    onPageLoad: function () {
        this.initPage();
    },
    initPage: function () {
        var that = this;

       
        that.loadData();
       
        that.initEvent();
    },
    loadData:function()
    {
        var that = this;
        that.element.ShareVipId = Jit.AM.getUrlParam("ShareVipId");
        that.element.ObjectId = Jit.AM.getUrlParam("ObjectId");
        that.element.SetoffToolID = that.getUrlParam('SetoffToolID');

        that.GetSetoffToolDetail(function (_data) {
            $(".imageUrl").attr("src", _data.imageUrl);
            $(".BGImage").attr("src", _data.backImgUrl);

            that.share.imgUrl = _data.backImgUrl;

            that.element.paraTmp = _data.paraTemp;

            that.initIsShareEvent();
            if(that.getUrlParam('pushType')=='IsSuperRetail'){

                that.element.PlatformType=4;
            }
            //等待扫一扫
            QrTimer = setInterval(function () {
                that.getDimensionalCodeByVipInfo(function (_data1) {
                    if (_data1.ResultCode!= 0) {
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
    },
    //获取集客海报数据
    GetSetoffToolDetail: function (callback) {
        var that = this;
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.VipGolden.GetSetoffToolDetail',
                'ToolType': "SetoffPoster",
                'ShareVipId': that.element.ShareVipId,
                'SetoffToolID': that.element.SetoffToolID,
                'PlatformType': 1,
                'VipDCode': 6,
                'ObjectId': that.element.ObjectId
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
                    'ObjectID':that.element.ObjectId,
                    'Mode': "SetOffPoster",
                    'PlatformType': that.element.PlatformType,
                    'paraTmp': that.element.paraTmp,
                    'ShareVipId':Jit.AM.getUrlParam('ShareVipId') || ''
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
    }
    , initIsShareEvent: function () {
        var that = this;
        var info = Jit.AM.getBaseAjaxParam();
        var shareInfo = {
            'title': "快来成为我的集客小伙伴！",
            'desc': "打开后识别二维码，加入微信并注册会员，成为我的小伙伴，带你集客带你飞~",
            'link': location.href,
            'isAuth': true,//需要高级auth认证
            "imgUrl": location.origin + "/HtmlApps/images/common/GatheringClient/SetOffPosterImg.jpg",
            'Authparam': [{ "paramname": "ObjectID", "paramvalue": that.element.ObjectId }]
        };
        
        Jit.WX.shareFriends(shareInfo);
        Jit.WX.shareTimeline(shareInfo);
    },
    showSharePanel: function () {
        $('#shareMask').show();
    },
    hideSharePanel: function () {
        $('#shareMask').hide();
    }
    

});