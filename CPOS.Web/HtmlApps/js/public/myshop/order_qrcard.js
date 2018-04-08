Jit.AM.defindPage({
	name: 'OrderQrcard',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		Jit.WX.OptionMenu(false);
		that.initData();
		that.initEvent();
	},
	initData: function(){
		var that = this;
		that.orderObj = JSON.parse(decodeURIComponent(Jit.AM.getUrlParam('orderObj')));
		//获取用户信息
		that.ajax({
            url: '/ApplicationInterface/Vip/VipGateway.ashx',
            data: {
                'action': 'GetVipInfo'
            },
            success: function (data) {
				if(data.ResultCode == 0){
				   var result = data.Data,
				   	   unitId= result.UnitId;
				   //请求二维码
				   that.getQrPic(unitId);
				}else{
				   alert(data.Message);
				}
            }
        });
	},
	getQrPic: function(unitId){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Stores/StoresGateway.ashx',
            data: {
                'action': 'getDimensionalCode',
				'unitId': unitId,
				'VipDCode': 2  //二维码类型(1=返利；2我的小店)
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   var imageUrl = data.Data.imageUrl,
				   	   paraTmp = data.Data.paraTmp;
				   $('.qrCardPic').attr('src',imageUrl);
				   //等待扫一扫
				   QrTimer = setInterval(function(){
				   		that.timerPush(paraTmp,unitId);
				   },1500)
				   
				}else{
				   alert(data.Message);
				}
            }
        });
	},
	timerPush: function(paraTmp,unitId){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Stores/StoresGateway.ashx',
            data: {
                'action': 'getDimensionalCodeByVipInfo',
				'special': {
					'unitId': unitId,
					'paraTmp': paraTmp
				}
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   if(data.Data.content.status == 1){
				   		clearInterval(QrTimer);
						var salesUser = JitPage.getBaseInfo().userId;
							userId = data.Data.content.userId,
							openId = data.Data.content.openId;
						//去下单，跳转订单页面
						that.submitOrder(that.orderObj,openId,userId,salesUser);
						
				        //that.promptHint('恭喜<br>关注成功',function(){});
				   }
				}else{
				   alert(data.Message);
				}
            }
        });
	},
	submitOrder: function(params,openId,userId,salesUser){
		var that = this;
		that.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
				'common':{
					'openId': openId,
					'userId': userId
				},
                'qty': params.qty,
				'salesUser': salesUser,
                'totalAmount': params.qty * params.salesPrice,
                'action': 'setOrderInfo',
				'dataFromId': 16,
                'orderDetailList': [params]
            },
            success: function(data) {
                if (data.code == 200) {
                    Jit.AM.toPage('GoodsOrder', '&orderId=' + data.content.orderId + '&openId=' + openId + '&userId=' + userId + '&channelId=6');
                } else {
                    Jit.UI.Dialog({
                        'content': data.description,
                        'type': 'Alert',
                        'CallBackOk': function() {
                            Jit.UI.Dialog('CLOSE');
                        }
                    });
                }
                //购物车数量提示
				//TopMenuHandle.ReCartCount();
            }
        });
	},
	substituteOrder: function(params){
		var that = this;
		that.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'qty': params.qty,
                'totalAmount': params.qty * params.salesPrice,
                'action': 'setOrderInfo',
                'orderDetailList': [params]
            },
            success: function(data) {
                if (data.code == 200) {
                    Jit.AM.toPage('GoodsOrder', '&orderId=' + data.content.orderId + '&channelId=6');
                } else {
                    Jit.UI.Dialog({
                        'content': data.description,
                        'type': 'Alert',
                        'CallBackOk': function() {
                            Jit.UI.Dialog('CLOSE');
                        }
                    });
                }
                //购物车数量提示
				//TopMenuHandle.ReCartCount();
            }
        });
	},
	initEvent: function() {
		var that = this;
		$('.replaceOrderBtn').on('click',function(){
			that.substituteOrder(that.orderObj);
		})
	}
});