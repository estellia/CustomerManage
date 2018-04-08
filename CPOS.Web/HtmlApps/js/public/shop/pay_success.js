Jit.AM.defindPage({

    name: 'PaySuccess',
    objects: {},
    onPageLoad: function() {
		if(Jit.AM.ChannelID == 6){
			$('#topNav').hide();
			$('.goods_wrap').css('margin-top','0px');
			$('#seeBill').attr('onclick',"Jit.AM.toPage('MinMyOrder')");
		}

        //当页面加载完成时触发
        Jit.log('进入paysuccess.....');


        this.LoadPayInfo();
    },
    initWithParam: function(param) {

        if (param.hideTop) {

            $('#topNav,.search_bar').hide();
            $('.goods_wrap,#payResult').css('margin-top', 0);
        }

        if (param.hideToOrder) {

            $('#payResult').next().hide();
        }
        this.objects.set = param;


    },
    ReCartCount: function() { //重新设置购物车数量
        var me = Jit.AM,
            menuCartCount = $('#menuCartCount');
        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getShoppingCartCount',
                'vipId':Jit.AM.getBaseAjaxParam().userId
            },
            success: function(data) {
                if (data.code == 200 && data.content.count && data.content.count > 0) {
                    ClientSession.set(KeyList.cartCount, data.content.count);
                    menuCartCount.html(data.content.count).show();
                } else {
                    ClientSession.set(KeyList.cartCount, 0);
                    menuCartCount.hide();
                }
            }
        });
    },
    initEvent: function() {

        // var me = this;

        // me.windowHeight = window.innerHeight;

        // me.windowWidth = window.innerWidth;

    }, //加载支付信息
    LoadPayInfo: function() {
        var me = this,
            payResult = $('#payResult');
        var orderId = me.getParams('orderId_' + me.getBaseInfo().userId);
        //是否是余额支付成功
        var flag = me.getUrlParam("useBalance");
        var sucStr = '<div class="paybar">' +
            '<img src="../../../images/public/shop_default/icon-success.png">' +
            '<p>支付成功!</p></div>';
        var errStr = '<div class="paybar">' +
            '<img src="../../../images/public/shop_default/icon-error.png">' +
            '<p>支付失败!</p></div>';
        if (flag) {
            payResult.html(sucStr);
            me.ReCartCount();
        } else {
			
            me.ajax({
                url: '/OnlineShopping/data/Data.aspx',
                beforeSend: function() {
                    payResult.html('数据正在加载,请稍后...');

                },
                data: {
                    'action': 'isOrderPaid',
                    'orderId': orderId
                },
                success: function(data) {
                    me.ReCartCount();
                    if (data.code == 200 && data.content.Status == 1) {

                        payResult.html(sucStr);

                        if (me.objects.set && me.objects.set.toPage) {
                            me.toPage('DonateSuccess', 'orderId=' + orderId)
                        };


                    } else {

                        payResult.html(sucStr);
                    }
                }
            });
        }

    }

});
