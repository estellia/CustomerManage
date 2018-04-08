Jit.AM.defindPage({
    ele:{
        total:$("#total"),
        userImage:$("#userImage"),
        userVipName:$("#userVipName"),
        scoreList:$("#dataList"),
        money:null
    },
    page:{
        pageIndex:0,
        pageSize:10,
        allPage:2
    },
    onPageLoad: function () {
        this.initPage();
        this.initEvent();
    },
    initPage: function () {
    	//传递页面的内容
        var that = this;
        that.GetVipCard();
        that.LoadOrderInfo();
    },
    reachBottom:function(vars) {
	    var scrollTop = 0;
	    var clientHeight = 0;
	    var scrollHeight = 0;
	    if (document.documentElement && document.documentElement.scrollTop) {
	        scrollTop = document.documentElement.scrollTop;
	    } else if (document.body) {
	        scrollTop = document.body.scrollTop;
	    }
	    if (document.body.clientHeight && document.documentElement.clientHeight) {
	        clientHeight = (document.body.clientHeight < document.documentElement.clientHeight) ? document.body.clientHeight: document.documentElement.clientHeight;
	    } else {
	        clientHeight = (document.body.clientHeight > document.documentElement.clientHeight) ? document.body.clientHeight: document.documentElement.clientHeight;
	    }
	    scrollHeight = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
	    if (scrollTop + clientHeight>= scrollHeight-vars) {
	        return true;
	    } else {
	        return false;
	    }
	},
    LoadOrderInfo: function() {
        var me = this,
            appPay=Jit.AM.getUrlParam("appPay"),
            orderInfo;
            
        me.ajax({
            url: '/Interface/data/OrderData.aspx',
            data: {
                'action': 'GetPaymentListBycId'
                //'channelId':'2'
            },
            beforeSend:function(){
                Jit.UI.Loading(true);
            },
            success: function(data) {
                
                if(data.code == 200){
                    var listData = data.content.paymentList,
                        htmlStr = '';
                    for(var i=0;i<listData.length;i++){
                        //if(listData[i].paymentTypeCode=='GetToPay'){
                            //continue;
                        //}
                        htmlStr += template.render('tpl_payWayList', listData[i]);
                    }
                    var item = $('#levelCard').find('.title');
                    debugger;
                    item.each(function(i){
                        var $tt = $(this),
                            isPurch = $tt.attr('data-purch'),//购卡升级
                            isRech = $tt.attr('data-rechcharge'),
                            isBuy = $tt.attr('data-buy'),
                            Score =  $tt.attr('data-score'),//卡所需积分
                            Price =  $tt.attr('data-Price'),//卡所需价格
                            rechAmount = $tt.attr('data-RechAmount'),
                            $yy = $tt.children('.bugCard');
                        if(isPurch==1||isRech==1){
                            if(Price!=0){
                                $yy.append(htmlStr);
                            }else if(isRech=1&&rechAmount!=0){
                                $yy.append(htmlStr);
                            }
                            if(isPurch==1){
                                if(Score!=0){
                                    $yy.append("<option value='3'>积分支付</option>");
                                }
                            }
                        }
                    })                    
                    // if(appPay!=1){
                    //     $('.op_pay_list a[data-typecode="WXAPP"]').hide();
                    // }else if(appPay==1){
                    //     $('.op_pay_list a[data-typecode="WXJS"]').hide();
                    // }
                    //me.initPaymentType(data.content.paymentList);
                }
            },complete:function(){
                Jit.UI.Loading(false);
            }
        });
        
    },
    initEvent:function(){
    	var me=this;
    	// $(window).bind("scroll",function(){
    	// 	if(me.reachBottom(100)){
     //            if(!me.isSending){
    	// 		    me.page.pageIndex++;
    	// 		    me.GetVipCard();
     //            }
    	// 	}
    	// });
        $('#levelCard').delegate(".bugCard","click",function(){
            var $tt = $(this);   
            var ll = $tt.children('option').length;
            if(ll>1){
                $tt.change(function(){
                    var vals = $tt.find("option:checked").val();
                    
                    if(vals!='0'){
                        me.setUpGrade($tt);
                        $("#levelCard").find("option[value='0']").attr("selected",true);
                    }
                })

            }else{
                var buyAmount = $tt.parents('.title').attr('data-BuyAmount'),//累计消费
                    onceBuyAmount = $tt.parents('.title').attr('data-OnceBuyAmount'),//单次消费
                    total = me.ele.money;//我的金额,
                if(total==null){
                    total = 0;
                }
                var total =0;
                if(Number(total)<Number(buyAmount)&&buyAmount!=0&&onceBuyAmount!=0&&Number(total)<Number(onceBuyAmount)){
                    var price = Number(buyAmount)-Number(total);
                    var price2 = Number(onceBuyAmount)-Number(total);
                    Jit.UI.Dialog({
                        'content': "还需消费"+price+"元</br>或一次性消费"+price2+"元才能升级",
                        'type': 'Alert',
                        'CallBackOk': function() {
                            me.toPage('GetVipCard');
                        }
                    });
                }else if(Number(total)<Number(buyAmount)){
                    var price = Number(buyAmount)-Number(total);
                    var price2 = Number(onceBuyAmount)-Number(total);
                    Jit.UI.Dialog({
                        'content': "还需消费"+price+"元才能升级",
                        'type': 'Alert',
                        'CallBackOk': function() {
                            me.toPage('GetVipCard');
                        }
                    });
                }else if(Number(total)<Number(onceBuyAmount)){
                    var price = Number(onceBuyAmount)-Number(total);
                    Jit.UI.Dialog({
                        'content': "一次性消费"+price+"元才能升级",
                        'type': 'Alert',
                        'CallBackOk': function() {
                             me.toPage('GetVipCard');
                        }
                    });
                }
            }
        });
        
    },
    //获得升级
    GetVipCard:function(){
        var self = this;
        // if(self.page.pageIndex>self.page.allPage-1){
        //     return false;
        // }
        this.ajax({
            async: false,
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.VipGolden.GetVipCardTypeVirtualItem',
				'ApplicationType':1//1=微信，2=app
            },
            beforeSend:function(){
                Jit.UI.Loading(true);
                self.isSending = true;
            },
            success: function (data) {


                if (data && data.IsSuccess) {
                    // self.page.allPage = data.Data.TotalPageCount;
                    self.ele.total.html(data.Data.Integration);
                    self.ele.userImage.css({'backgroundImage':'url("'+data.Data.HeadImgUrl+'")','backgroundRepeat':'no-repeat','backgroundSize':'100%'});
                    self.ele.userVipName.children('p').html(data.Data.VipCardTypeName);
                    self.ele.money = data.Data.VipConsumptionAmount;
                    var list = data.Data.VipCardTypeItemList;
                    // var list = data.Data.VipCardTypeItemList.VipCardRule,
                    //     listOpen = data.Data.VipCardTypeItemList.VipCardUpgradeRule;
                    //     
                    // list = list.concat(listOpen);
                    // 
                    if(data.Data.VipCardTypeItemList){
                        var htmlList = template.render('tplList', {'list':data.Data.VipCardTypeItemList,level:data.Data.VipCardLevel}); 
                        $('#levelCard').html(htmlList); 
                    }else{
                        if(self.page.pageIndex==0){
                            self.ele.scoreList.html('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">没有查找到您的积分信息!</div>');
                        }
                    }
                }else{
                    self.ele.scoreList.html('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">'+data.Message+'</div>');
                }
            },
            complete:function(){
                self.isSending = false;
                Jit.UI.Loading(false);
            }
        });
    },
    //设置升级
    setUpGrade:function($tt){
        var me = this,
            setPayType = $tt.val(),//setPayType,1=微信；2=支付宝；3=积分
            payType = $tt.find("option:checked").attr('val'),
            cardId = $tt.parents('.title').attr('data-cardid'),//卡id
            isPurch = $tt.parents('.title').attr('data-purch'),//购卡升级条件=1
            isRech =  $tt.parents('.title').attr('data-rechcharge'),//充值升级条件=1
            isBuy = $tt.parents('.title').attr('data-buy'),//消费升级条件=1
            Price =  $tt.parents('.title').attr('data-Price'),//卡所需价格
            Score =  $tt.parents('.title').attr('data-score'),//卡所需积分
            isExtra = $tt.parents('.title').attr('data-extra'),//可补差价=1
            skuId = $tt.parents('.title').attr('data-SkuID'),//卡sku
            rechAmount = $tt.parents('.title').attr('data-RechAmount'),//充值所需金额
            BuyAmount = $tt.parents('.title').attr('data-BuyAmount'),//消费所需金额
            money =me.ele.money;//自己的金额
        
        if(setPayType!="3"){
            var salesPrice = Price;//卡的金额
            if(isPurch==1&&isExtra==1){//只有购卡升级才有可补差价
                if(Number(Price)-Number(money)>=0){
                    Price = Number(Price)-Number(money);
                }
            }else if(isRech ==1){
                Price = Number(rechAmount);
            }else if(isBuy ==1){
                Price = Number(BuyAmount);
            }
            var orderId = "";//定义订单参数；
            if(isRech == 1){//充值升级接口
                me.ajax({
                    url: '/ApplicationInterface/Gateway.ashx',
                    async: false,
                    data: {
                        'action': 'VIP.VipGolden.SetRechargeOrder',
                        "ActuallyPaid":Price,
                        "CardTypeId":cardId
                    },
                    beforeSend:function(){
                        Jit.UI.Loading(true);
                    },
                    success: function(data) {
                        
                        if(data&&data.IsSuccess){
                            orderId = data.Data.orderId;
                        }
                    }
                });
            }else{
                me.ajax({//购卡升级接口
                    url: '/OnlineShopping/data/Data.aspx',
                    async: false,
                    data: {
                        'action': 'setOrderInfo',
                        "qty":null,
                        "totalAmount":Price,
                        "action":"setOrderInfo",
                        "CardTypeId":cardId,
                        "CommodityType":1,
                        "orderDetailList":[{
                            'skuId':skuId,
                            'salesPrice':salesPrice,
                            'qty':1
                        }]
                    },
                    beforeSend:function(){
                        Jit.UI.Loading(true);
                    },
                    success: function(data) {
                        
                        if(data.code==200){
                            orderId = data.content.orderId;
                        }
                    }
                });
            }
            
            // if (!payType) {
            //     Jit.UI.Dialog({
            //         'content': '不支持此支付方式',
            //         'type': 'Alert',
            //         'CallBackOk': function() {
            //             Jit.UI.Dialog('CLOSE');
            //         }
            //     });
            //     return false;
            // };

            
            // phomeArea.hide();
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
            }else if (payType == '7730ABEECF3048BE9E207D7E83C944AF') { //银联语音支付
                
                phomeArea.show();
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
                }else if (!IsMobileNumber(phonenum.val()) ) {

                        Jit.UI.Dialog({
                            'content': '您输入的支付手机号码格式不正确',
                            'type': 'Alert',
                            'CallBackOk': function() {
                                Jit.UI.Dialog('CLOSE');
                                phonenum.focus();
                            }
                        });
                        return;
                }else {
                    Jit.UI.Dialog({
                        'content': '',
                        'type': 'Confirm',
                        'LabelOk': '取消',
                        'LabelCancel': '支付完成',
                        'CallBackOk': function() {
                            Jit.UI.Dialog("CLOSE");
                        },
                        'CallBackCancel': function() {
                            me.toPage('PaySuccess', '&orderId=' + orderId+'&payType='+payType);
                        }
                    });
                }

            }else if(payType == 'DFD3E26D5C784BBC86B075090617F44E') { //微信APP支付
                location.href = 'AppPay://orderId='+orderId+'&amount='+Price+'&paymentTypeId=DFD3E26D5C784BBC86B075090617F44E';
                return;
            }
            var baseInfo=me.getBaseInfo();
            // var toUrl="http://"+location.host+"/HtmlApps/auth.html?pageName=PaySuccess&eventId="+baseInfo.eventId+"&customerId="+baseInfo.customerId+"&openId="+baseInfo.openId+"&userId="+baseInfo.userId+"&orderId"+me.getUrlParam('orderId');

            var toUrl="http://"+location.host+"/HtmlApps/html/public/shop/pay_success.html?customerId="+me.getUrlParam('customerId'),
                toUrl2="http://"+location.host+"/HtmlApps/html/common/vipCard/getCard.html?customerId="+me.getUrlParam('customerId'),
                toUrl3="http://"+location.host+"/HtmlApps/html/common/vipCard/getCard.html?customerId="+me.getUrlParam('customerId');
            
            me.setParams('orderId_'+baseInfo.userId,orderId);

            //"orderPay",
            Price = Price;//金额后台数据缩小了100倍，前台需放大100倍；
            var hashdata = {
                action: 'setPayOrder',
                paymentId: payType,
                orderID: orderId,
                returnUrl: encodeURIComponent(toUrl3),
                mobileNo: payType == '7730ABEECF3048BE9E207D7E83C944AF' ? phonenum.val() : "", //为实现 语音支付
                amount:Price,
                actualAmount:Price,
                dataFromId:2
            };
                
            me.ajax({
                url: '/Interface/data/OrderData.aspx',
                data: hashdata,
                async: false,
                beforeSend:function(){
                    Jit.UI.Loading(true);
                },
                success: function(data) {
                    if (data.code == "200" && parseInt(payType) != 2) {
                        
                        if(payType == 'DFD3E26D5C784BBC86B075090617F44B'){
                            Jit.UI.Loading(false);
                            var wxpackage = JSON.parse(data.content.WXPackage);
                            //多公众号运营时，A号配置支付，B号无配置支付，关注了B号，在支付时，提示关注A号才能支付。
                            if(!!wxpackage){
                                WeixinJSBridge.invoke('getBrandWCPayRequest',wxpackage,function(res){
                                     if(res.err_msg == "get_brand_wcpay_request:ok" ) {
                                         location.href = toUrl2;
                                         //Jit.AM.toPage('getCard');
                                     }else if(res.err_msg=="get_brand_wcpay_request:cancel"){
                                         //Jit.AM.toPage('getCard');
                                         location.href = toUrl2;
                                     }else{
                                         alert("支付失败");
                                         Jit.AM.toPage('getCard');
                                     }
                                    location.href = toUrl2;
                                });
                            }else{
                                Jit.UI.Dialog({
                                    'content': data.content.PayNotice,
                                    'type': 'Alert',
                                    'CallBackOk': function() {
                                        Jit.UI.Dialog('CLOSE');
                                    }
                                });
                                return false;
                            }
                             
                             
                        }else if(payType=="257E95A658624C91AFCC8B6CE3DF8BFB"){
                            if(Jit.AM.getUrlParam('channelId') == 6){
                                Jit.UI.Dialog({
                                    'content': '订单已完成!',
                                    'type': 'Alert',
                                    'LabelOk': '我的小店',
                                    'CallBackOk': function() {
                                        me.toPage('ShopIndex');
                                    }
                                });
                            }else{
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
                            }
                            
                            
                        }else{
                            
                            $('#iframePayment').attr('src',data.content.PayUrl).show();
                            $('section').hide();
                            $('.myInfoArea').hide();
                            var OnMessage = function(e){
                                e = e || window.event,
                                e.data.indexOf('chainclouds.cn') > -1 && (window.location.href = e.data)
                                //alert(e.data);
                            }
                            if(window.addEventListener){// all browsers except IE before version 9
                                window.addEventListener("message", OnMessage, false);
                            }else{
                                if(window.attachEvent){// IE before version 9
                                    window.attachEvent("onmessage", OnMessage);
                                }
                            }
                            Jit.UI.Loading(false);
                            /*
                            if(window != top){
                                top.location.href = '';
                            }
                            */

                            //window.location = data.content.PayUrl;
                        }
                        //me.hasSubmit=false;
                    }else{
                        alert(data.description);
                    }
                },
                complete:function(){
                    Jit.UI.Loading(false);
                }
 
            }); 
        
        }else{ //积分升级
            var myScore = $('#total').html();
            if(Number(Score)>Number(myScore)){
                var val = Number(Score) - Number(myScore);
                me.alert("还需消耗"+val+"积分才能升级?");
                $("#levelCard").find("option[value='0']").attr("selected",true);
                Jit.UI.Loading(false);
            }else if(isPurch==1&&Number(myScore)!=0&&Number(Score)!=0){//只有购卡升级才能进行积分升级
                debugger;
                Jit.UI.Dialog({
                    'content':"是否消耗"+Score+"积分升级?",
                    'type': 'Confirm',
                    LabelOk: '是',
                    LabelCancel: '否',
                    'CallBackOk': function() {
                         isSuccess =true;
                         if(isSuccess){
                            me.ajax({
                                url: '/OnlineShopping/data/Data.aspx',
                                data: {
                                    'action': 'setOrderInfo',
                                    "qty":null,
                                    "totalAmount":Price,
                                    "action":"setOrderInfo",
                                    "CardTypeId":cardId,
                                    "CommodityType":1,
                                    'Integral':Score,
                                    "orderDetailList":[{
                                        'skuId':skuId,
                                        'salesPrice':null,
                                        'qty':null                                    

                                    }]
                                },
                                beforeSend:function(){
                                    Jit.UI.Loading(true);
                                },
                                success: function(data) {
                                    
                                    if(data.code==200){
                                        me.alert("升级成功");
                                        setTimeout(function () {
                                            Jit.AM.toPage('GetVipCard');
                                        }, 1500);
                                        
                                    }
                                }
                            });
                        }
                        Jit.UI.Dialog("CLOSE");
                     },
                     CallBackCancel:function(){
                        Jit.UI.Dialog("CLOSE");
                    }

                });
                

                
                
            }
        }
        
    },
    alert:function(text,callback){
        Jit.UI.Dialog({
            type : "Alert",
            content : text,
            CallBackOk : function() {
                Jit.UI.Dialog("CLOSE");
                if(callback){
                    callback();
                }
            }
        });
    }
});