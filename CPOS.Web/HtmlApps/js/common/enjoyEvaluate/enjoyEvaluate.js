Jit.AM.defindPage({
    name: 'EnjoyEvaluate',
	initWithParam: function(param){
		//前端配置显示层
		//if(param['showMiddleArea']=='false'){
			//$('.noticeList').hide();
		//}
	},
    onPageLoad: function () {
		var that = this;
		that.ele = {
			uiMask : $('.ui-mask'),
			evaluatePage : $('#evaluatePage'),
			enjoyPage : $('#enjoyPage'),
			evaluatePrompt : $('#evaluatePrompt'),
			enjoyPrompt : $('#enjoyPrompt'),
			newPromptBox : $('.newPromptBox'),
			evaluateBox : $('.evaluateBox'),
			already : $('.already em'),
			staffName : $('.staffName'),
			starBox : $('.starBox em'),
			headPic : $('.headPic img')
		};
		window.onhashchange=function(){
			var hashStr = location.hash.replace("#a=","");
			$('.panlPage').hide();
			that.ele.evaluateBox.removeClass('isEvaluate');
			switch(hashStr){
				case '1':
					that.ele.enjoyPage.show();
				    break;
				case '2':
					that.ele.evaluatePage.show();
					that.ele.evaluateBox.addClass('isEvaluate');
				  	break;
				case '0':
					that.ele.evaluatePage.show();  
				default:
			 		that.ele.evaluatePage.show();
			}
		}
		that.getConfig();
        that.initEvent();
		Jit.WX.OptionMenu(false);
    },
    initEvent:function(){
        var that = this,
			r = /^\d+(\.\d+)?$/;
		//进入页面执行一次hashchange事件
		//$('body').trigger('hashchange');
		$('.moneyList').delegate('a','click',function(){
			Jit.UI.Loading(true);
			var $this = $(this),
				amount = parseFloat($this.data('amount'));
			that.setRewardOrder(amount);
		});
		
		//监听输入打赏金额的提交事件
		that.ele.enjoyPrompt.delegate('.submitEnjoyBtn','click',function(){
			var $enjoyInputBox = $('#enjoyInputBox'),
				$hintBox = $('.hintBox',that.ele.enjoyPrompt),
				amount = parseFloat($enjoyInputBox.val());
			if(!r.test(amount) || amount>300 || amount<1){
				$hintBox.show();
				setTimeout(function(){
					$hintBox.hide();
				},1000);
				return false;
			}
			that.setRewardOrder(amount);
		});
		
		$('.heartBox a').bind('click',function(){
			var $this = $(this),
				$thisPar = $this.parents('.heartBox'),
				index = $this.index();
			if(!$this.hasClass('on')){
				index = index+1;
			}
			$('a',$thisPar).removeClass('on');
			for(var i=0;i<index;i++){
				$('a',$thisPar).eq(i).addClass('on');
			}
			$thisPar.attr('data-val',index);
			//提交评价接口
			that.submitEvaluate();
		});

		$('.intoEnjoyBtn').on('click',function(){
			location.hash = '#a=1';
			that.ele.evaluatePage.hide();
			that.ele.enjoyPage.show();
		});
	
		
		$('.enjoyEvaluatePage').delegate('.otherMoney','click',function(){
			that.ele.uiMask.show();
			that.ele.enjoyPrompt.show();
			$('#enjoyInputBox').trigger('focus');
		});
		
		that.ele.uiMask.on('click',function(){
			that.ele.uiMask.hide();
			that.ele.newPromptBox.hide();
		});
		
    },
	getConfig: function(){
		Jit.UI.Loading(true);
		var that = this;
		that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: {
				'action': 'VIP.Reward.GetConfig',
				'EmployeeID': Jit.AM.getUrlParam('employeeId') || ''
			},
			success: function (data){
				if(data.ResultCode == 0){
					Jit.UI.Loading(false);
					var result = data.Data,
						type = result.Type,//0=两者，1=打赏，2=评分
						userInfo = result.UserInfo[0],//员工信息
						amountList = result.AmountList;//打赏金额列表
					that.type = type;//设置全局type
					that.getEnjoyList(amountList);//遍历打赏金额列表
					if(type==1){
						that.ele.enjoyPage.show();
					}else if(type==2){
						that.ele.evaluatePage.show();
						//that.ele.evaluateBox.addClass('isEvaluate');
					}else if(type==0){
						//location.hash = '#a=0';
						that.ele.enjoyPage.show();
						that.ele.evaluatePage.show();
					}
					if(!!userInfo){
						that.ele.already.text(userInfo.RewardCount);//被打赏人数
						that.ele.staffName.text(userInfo.UserName);//员工姓名
						that.ele.headPic.attr('src',userInfo.UserPhoto || '../../../images/common/enjoyEvaluate/headPic.png');//员工头像
						for(var i=0;i<userInfo.StarLevel;i++){//星级
							that.ele.starBox.eq(i).addClass('on');
						}
					}
				}else{
					alert(data.Message);
				}
			}
		})
	},
	getEnjoyList:function(amountList){
		var that = this,
			htmlStr = '',
			$moneyList = $('.moneyList');
		for(var i=0;i<amountList.length;i++){
			var amount = amountList[i].Amount.toString();
			
			htmlStr += '<a href="javascript:;" data-amount="'+amount+'"><span>￥</span>'+amount+'</a>';
			/*
			if(amount.length==2){
				htmlStr += '<a class="indent45" href="javascript:;"><span>￥</span>'+amount+'</a>';
			}else if(amount.length==3){
				htmlStr += '<a class="indent68" href="javascript:;"><span>￥</span>'+amount+'</a>';
			}else{
				htmlStr += '<a href="javascript:;"><span>￥</span>'+amount+'</a>';
			}
			*/
		}	
		$moneyList.html(htmlStr);
	},
	setRewardOrder: function(amount){
		var that = this;
		that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: {
				'action': 'VIP.Reward.SetRewardOrder',
				'RewardAmount':amount,	//打赏金额
				'EmployeeID':Jit.AM.getUrlParam('employeeId')	//员工ID
			},
			success: function (data){
				if(data.ResultCode == 0){
					var result = data.Data,
						rewardOrderId = result.RewardOrderID,//打赏单号（格式: REWARD|*********）
						paymentId = result.paymentId;//付款方式ID
					//隐藏弹层
					that.ele.uiMask.hide();
					that.ele.enjoyPrompt.hide();	
					//微信支付
					that.setPayOrder(rewardOrderId,paymentId,amount);
				}else{
					alert(data.Message);
				}
			}
		})
	},
	setPayOrder: function(orderId,paymentId,amount){
		var that = this;
		var toUrl="http://"+location.host+"/HtmlApps/html/public/shop/pay_success.html?customerId="+that.getUrlParam('customerId');
		//that.setParams('orderId_'+baseInfo.userId,me.getUrlParam('orderId'));
		
		var hashdata = {
			action: 'setPayOrder',
			paymentId: paymentId,
			orderID: orderId,
			returnUrl: toUrl,
			mobileNo: "", //为实现 语音支付
			amount:amount,
			actualAmount:amount,
			dataFromId:2
		};
		
		that.ajax({
			url: '/Interface/data/OrderData.aspx',
			data: hashdata,
			success: function(data) {
				if (data.code == "200") {
					Jit.UI.Loading(false);
					if(paymentId == 'DFD3E26D5C784BBC86B075090617F44B'){
						var wxpackage = JSON.parse(data.content.WXPackage);
						//多公众号运营时，A号配置支付，B号无配置支付，关注了B号，在支付时，提示关注A号才能支付。
						if(!!wxpackage){
							WeixinJSBridge.invoke('getBrandWCPayRequest',wxpackage,function(res){
								if(res.err_msg == "get_brand_wcpay_request:ok" ) {
									//location.href = toUrl;
								}else if(res.err_msg=="get_brand_wcpay_request:cancel"){
									//location.href = toUrl2;
									location.reload();
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
			        }else{
			        	window.location = data.content.PayUrl;
			        }
			        //me.hasSubmit=false;
				}else{
                    alert(data.description);
                }
			}
		});
	},
	submitEvaluate: function(){
		var that = this,
			$evaluateList = $('.goodsCommentList .itemBox'),
			evaluationInfo = [],
			starLevel = parseInt($('#describeSame').attr('data-val'));
		if(!starLevel){
			alert('亲，您还没有评分哦！');
			return false;
		}
		//evaluationInfo[i]['StarLevel'] = $('.commentBox01.on',$evaluateList.eq(i)).data('level');
		
		that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: {
				'action': 'VIP.Evaluation.SetEvaluation',
				'ObjectID': Jit.AM.getUrlParam('employeeId'),//员工ID
				'Type': 4, //1-商品，2-门店，3-订单，4-员工 必须
				'StarLevel': starLevel
			},
			success: function (data){
				if (data.ResultCode == 0){
					$('.heartBox a').unbind();
					that.ele.uiMask.show();
					that.ele.evaluatePrompt.show();
					/*
					Jit.UI.Prompt({
						'title':'亲，您给你好评！',
						'des':'您现在可以去：',
						'html':'<a href=javascript:Jit.AM.toPage("MyCenter"); class="btn02" style="width:40%;">个人中心</a>'
					})
					*/
					//window.close();
				}else{
					alert(data.Message);
				}
			}
		})
	},
	closeEvaluatePrompt:function(){
		var that = this;
		that.ele.uiMask.hide();
		that.ele.evaluatePrompt.hide();
	}
})