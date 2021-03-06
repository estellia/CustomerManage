﻿Jit.AM.defindPage({

    name: 'Pay',
    tickedInfo: '',

    onPageLoad: function() {

        var me = this;
        me.initEvent();
        me.LoadOrderInfo();

        var isSourceGoods = Jit.AM.getUrlParam('isGoodsPage');
        if (isSourceGoods) {
            $('#toPageBack').attr('href', "javascript:Jit.AM.toPage('GoodsList')");
        };
    },
    LoadOrderInfo: function() {
        var me = this;
        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'GetTickedInfo',
                'orderId': me.getUrlParam('tickedId'),
                'page': 1,
                'pageSize': 10000
            },
            success: function(data) {
                if (data.code == 200 && data.content.TickedInfo.length > 0) {
                    me.tickedInfo=data.content.TickedInfo;
                    $('#orderTotal').html('￥' + me.tickedInfo.totalAmount);
                }
            }
        });

    },
    initEvent: function() {
        var me = this,
            phomeArea = $('#phoneArea');
        //绑定选择支付类型事件
        var iList = $('.op_pay_list .items');
        iList.bind('click', function() {
            var self = $(this);
            iList.find('i').removeClass('on');
            self.find('i').addClass('on');
            var payType = self.find('i').attr('val');
            if (payType == 2) {
                phomeArea.show();
            } else {
                phomeArea.hide();
            }
        });
        //submitPay
        $('#submitPay').bind('click', function() {


            me.SubmitPay();
        });

    },
    SubmitPay: function() { //提交订单
        var me = this;

        var payType = $('.op_pay_list .items i.on').attr('val'),
            phomeArea = $('#phoneArea'),
            phonenum = $('#phonenum');

        if (!payType) {
            Jit.UI.Dialog({
                'content': '请选择支付方式',
                'type': 'Alert',
                'CallBackOk': function() {
                    Jit.UI.Dialog('CLOSE');
                }
            });
            return false;
        };

        Jit.UI.Dialog({
            'content': '正在开发中',
            'type': 'Alert',
            'CallBackOk': function() {
                Jit.UI.Dialog('CLOSE');
            }
        });
        return false;



        if (parseInt(payType) == 0) { //货到付款
            Jit.UI.Dialog({
                'content': '订单已完成!',
                'type': 'Confirm',
                'LabelOk': '去逛逛',
                'LabelCancel': '我的订单',
                'CallBackOk': function() {
                    me.toPage('GoodsList');
                },
                'CallBackCancel': function() {
                    me.toPage('MyOrder');
                }
            });
            return;
        } else if (parseInt(payType) == 2) { //银联语音支付
            if (phonenum.val() == "") {
                Jit.UI.Dialog({
                    'content': '请填写您的支付手机号码',
                    'type': 'Alert',
                    'CallBackOk': function() {
                        Jit.UI.Dialog('CLOSE');
                        phonenum.focus();
                    }
                });

                return;
            } else if (!IsMobileNumber(phonenum.val())) {

                Jit.UI.Dialog({
                    'content': '您输入的支付手机号码格式不正确',
                    'type': 'Alert',
                    'CallBackOk': function() {
                        Jit.UI.Dialog('CLOSE');
                        phonenum.focus();
                    }
                });
                return;
            } else {
                Jit.UI.Dialog({
                    'content': '',
                    'type': 'Confirm',
                    'LabelOk': '取消',
                    'LabelCancel': '支付完成',
                    'CallBackOk': function() {
                        Jit.UI.Dialog("CLOSE");
                    },
                    'CallBackCancel': function() {
                        me.toPage('PaySuccess', '&orderId=' + JitPage.getUrlParam('orderId') + '&payType=' + payType);
                    }
                });
            }

        }

        var baseInfo = me.getBaseInfo();
        // var toUrl="http://"+location.host+"/HtmlApps/auth.html?pageName=PaySuccess&eventId="+baseInfo.eventId+"&customerId="+baseInfo.customerId+"&openId="+baseInfo.openId+"&userId="+baseInfo.userId+"&orderId"+me.getUrlParam('orderId');

        var toUrl = "http://" + location.host + "/HtmlApps/html/public/shop/pay_success.html?customerId=" + me.getUrlParam('customerId');

        me.setParams('orderId_' + baseInfo.userId, me.getUrlParam('orderId'));


        var hashdata = {
            action: "orderPay",
            payChannelID: payType,
            orderID: me.getUrlParam('orderId'),
            returnUrl: toUrl,
            mobileNO: parseInt(payType) == 2 ? phonenum.val() : "" //为实现 语音支付
        };

        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: hashdata,
            success: function(data) {
                if (data.code == "200" && parseInt(payType) != 2) {
                    window.location = data.content.PayUrl;
                }
            }
        });


    }

});