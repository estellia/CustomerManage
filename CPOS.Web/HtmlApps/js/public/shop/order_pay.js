Jit.AM.defindPage({

	name: 'OrderPay',

	onPageLoad: function() {
		Jit.UI.Loading(true);
		var channelId = Jit.AM.getUrlParam('channelId');
		if(!!channelId){
			if(channelId!=11){
				$('body').prepend('<input id="channelIdSize" type="hidden" value="'+channelId+'">');
			}
			$('#topNav').hide();
			//$('.goods_wrap').css('margin-top','0px');
			//$('.goods_wrap .op_area').css('margin-top','0px');
		}
		var me = this;
		//是否显示header
		var hasHeader=Jit.AM.getUrlParam("hasHeader");
		if(hasHeader==0){
			$("#topNav").hide();
			//$(".goods_wrap").css("margin-top","0");
			//$(".op_area").css("margin-top","0");
			
		}
		
		me.initEvent();
		
		me.LoadOrderInfo();
		
		TopMenuHandle.ReCartCount();
		
		var isSourceGoods = Jit.AM.getUrlParam('isGoodsPage');
		
		if (isSourceGoods) {
		
			$('#toPageBack').attr('href', "javascript:Jit.AM.toPage('GoodsList')");
		};
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
						htmlStr = '',
						getPayOrder = Jit.AM.getUrlParam('isP');
					for(var i=0;i<listData.length;i++){
						//if(listData[i].paymentTypeCode=='GetToPay'){
							//continue;
						//}
						htmlStr += bd.template('tpl_payWayList', listData[i]);
					}
					$('.op_pay_list').html(htmlStr);
					
					if(appPay!=1){
						$('.op_pay_list a[data-typecode="WXAPP"]').hide();
					}else if(appPay==1){
						$('.op_pay_list a[data-typecode="WXJS"]').hide();
					}
					if(getPayOrder=1){
						$('.op_pay_list a[data-typecode="GetToPay"]').hide();
					}
					//me.initPaymentType(data.content.paymentList);
				}
			},complete:function(){
                Jit.UI.Loading(false);
            }
		});
		
		
		
		me.ajax({
			url: '/OnlineShopping/data/Data.aspx',
			data: {
				'action': 'getOrderList',
				'orderId': me.getUrlParam('orderId'),
				'page': 1,
				'pageSize': 10000
			},beforeSend:function(){
                Jit.UI.Loading(true);
            },
			success: function(data) {
			
				if (data.code == 200 && data.content.orderList.length > 0) {
				
					var orderInfo = data.content.orderList[0];

					me.totalAmount = orderInfo.totalAmount;
					me.actualAmount=orderInfo.actualAmount;
					//应付总额
					var realMoney=me.getUrlParam('realMoney');
					//$('#orderTotal').html('￥' + orderInfo.totalAmount);
					if(realMoney){
						$('#orderTotal').html('￥' + realMoney);
					}else{
						$('#orderTotal').html('￥' + orderInfo.actualAmount);
					}
					Jit.UI.Loading(false);
				}else{
                    alert("获取订单失败");
                    Jit.UI.Loading(false);
                }
			},complete:function(){
                Jit.UI.Loading(false);
            }
		});
	},
	
	initPaymentType:function(typelist){
	
		var types = {};
		for(var i=0;i<typelist.length;i++){
		    
			types[typelist[i]['paymentTypeId']] = true;
			
		}
		$('table').each(function(i,dom){
			if(types[$($(dom).get(0)).attr('val')]){
				
				$(dom).show();
			}
		});
		//是否隐藏货到付款支付方式
		/*
		var isHideGetToPay=Jit.AM.getUrlParam("isHideGetToPay");
		if(isHideGetToPay==1){
			$("#getToPay").hide();
		}
		*/
		
	},
	initEvent: function() {
		var me = this;
	
		$('.op_pay_list').delegate('.items','click', function(event) {
			var $this = $(this);
			me.SubmitPay($this);
			event.preventDefault();
		});
		
	},
	SubmitPay: function($this) { //提交订单
		var me = this;
		/*
		if(me.hasSubmit){
			return;
		}
		me.hasSubmit = true;
		*/
		var payType = $this.attr('val'),
			phomeArea = $('#phoneArea'),
			phonenum = $('#phonenum');
		
		if (!payType) {
			Jit.UI.Dialog({
				'content': '不支持此支付方式',
				'type': 'Alert',
				'CallBackOk': function() {
					Jit.UI.Dialog('CLOSE');
				}
			});
			return false;
		};

		
		phomeArea.hide();
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
			console.log(phonenum.val());
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
						me.toPage('PaySuccess', '&orderId=' + JitPage.getUrlParam('orderId')+'&payType='+payType);
					}
				});
			}

		}else if(payType == 'DFD3E26D5C784BBC86B075090617F44E') { //微信APP支付
			location.href = 'AppPay://orderId='+me.getUrlParam("orderId")+'&amount='+me.actualAmount+'&paymentTypeId=DFD3E26D5C784BBC86B075090617F44E';
			return;
		}

		var baseInfo=me.getBaseInfo();
		// var toUrl="http://"+location.host+"/HtmlApps/auth.html?pageName=PaySuccess&eventId="+baseInfo.eventId+"&customerId="+baseInfo.customerId+"&openId="+baseInfo.openId+"&userId="+baseInfo.userId+"&orderId"+me.getUrlParam('orderId');

		var toUrl="http://"+location.host+"/HtmlApps/html/public/shop/pay_success.html?customerId="+me.getUrlParam('customerId'),
			toUrl2="http://"+location.host+"/HtmlApps/html/public/shop/order_detail.html?customerId="+me.getUrlParam('customerId')+"&orderId="+me.getUrlParam('orderId'),
			toUrl3="http://"+location.host+"/HtmlApps/html/public/shop/my_order.html?customerId="+me.getUrlParam('customerId')+"&groupingType=2";
			
		me.setParams('orderId_'+baseInfo.userId,me.getUrlParam('orderId'));

		//"orderPay",
		var hashdata = {
			action: 'setPayOrder',
			paymentId: payType,
			orderID: me.getUrlParam('orderId'),
			returnUrl: encodeURIComponent(toUrl3),
			mobileNo: payType == '7730ABEECF3048BE9E207D7E83C944AF' ? phonenum.val() : "", //为实现 语音支付
			amount:me.actualAmount,
			actualAmount:me.actualAmount,
			dataFromId:2
		};

		me.ajax({
			url: '/Interface/data/OrderData.aspx',
			data: hashdata,
            beforeSend:function(){
                Jit.UI.Loading(true);
            },
			success: function(data) {
				if (data.code == "200" && parseInt(payType) != 2) {
					
					if(payType == 'DFD3E26D5C784BBC86B075090617F44B'){
						
						var wxpackage = JSON.parse(data.content.WXPackage);
						//多公众号运营时，A号配置支付，B号无配置支付，关注了B号，在支付时，提示关注A号才能支付。
						if(!!wxpackage){
							WeixinJSBridge.invoke('getBrandWCPayRequest',wxpackage,function(res){
								if(res.err_msg == "get_brand_wcpay_request:ok" ) {
									//location.href = toUrl;
									Jit.AM.toPage('MyOrder','groupingType=2');
								}else if(res.err_msg=="get_brand_wcpay_request:cancel"){
									var getPayOrder = Jit.AM.getUrlParam('isP');
									var toUrl4 = Jit.AM.getPageParam('setOrder');
									if(getPayOrder=='1'){
										location.href = toUrl4;
									}else{
										location.href = toUrl2;
									}
								}else{
									alert("支付失败");
								}
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
			        	var paySrc = data.content.PayUrl,
			        		iframeHtml = '<iframe  name="iframePay" frameborder=1 border=none id="iframePayment" src="'+paySrc+'"></iframe>';
			        	$('body').append(iframeHtml);
			        	/*
			        	var OnMessage = function(e){
			        		e = e || window.event,
			        		e.data.indexOf('chainclouds.cn') > -1 && (window.location.href = e.data)
			        		alert(e.data);
			        	}
			        	if(window.addEventListener){// all browsers except IE before version 9
			        		window.addEventListener("message", OnMessage, false);
			        	}else{
			        		if(window.attachEvent){// IE before version 9
			        			window.attachEvent("onmessage", OnMessage);
			        		}
			        	}
			        	*/
			        	/*
						if(window != top){
							top.location.href = '';
						}
						*/
			        }
				}else{
                    alert(data.description);
                }
			},
            complete:function(){
                Jit.UI.Loading(false);
            }

		});


	}

});
