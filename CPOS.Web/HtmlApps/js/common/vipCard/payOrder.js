Jit.AM.defindPage({
	isAddress: false,
	isUnit: false,
	isFirstClick: 0,//第一次点击到店自提
    name: 'GoodsOrder',
	initWithParam: function(param){
	},
	//优惠券积分模块
	Coupon:{
		vars:{
			totalAmount:0,
			score:0,     //积分
			cash:0,      //返现
			minScore:0,  //最小使用积分
			minCash:0,   //最小使用返现金额
			balance:0,    //余额
			discounts:0,  //会员折扣
			couponMoney:0,   //优惠券抵用的金额
			scoreMoney:0,    //积分抵用的金额
			cashMoney:0,     //返现抵用的金额
			orderMoney:0,    //订单金额
			passwordFlag:0,  //是否设置支付密码  1为设置  0为未设置
			lockFlag:0,      //是否账户锁定   1为锁定  0为未锁定 
			money:0,
			vipScore:0,    //可用积分
			skuIds:[{}]  //skuIds集合
		},  
		//接口的信息
		urls:{
			//前缀
			prefix:"/ApplicationInterface/Vip/VipGateway.ashx",
			//优惠券
			getCouponAction:"GetVipCoupon",
			//积分
			getScoreAction:"GetVipIntegral",
			//获取会员信息
			getVipInfoAction:"GetVipInfo",
			//余额设置密码
			setPayPassAction:"SetVipPayPassword",
			//检测支付密码
			checkPayPassAction:"CheckVipPayPassword",
		},
		//获得优惠券,积分数据
		getCouponData:function(obj){
			var me=this;
			//SkuIdAndQtyList   [{SkuId:"",Qty:""},{SkuId:"",Qty:""}]
			JitPage.ajax({
				url:me.urls.prefix,
				data:{
					"action":me.urls[obj.action],
					"SkuIdAndQtyList":obj.skuIds,
					"UsableRange":2,	//Int	是	适用范围(1=购物券；2=服务券)
					"ObjectID": Jit.AM.getUrlParam('UnitId'),   //String 否	发货门店ID
					'DiscountType': Jit.AM.getUrlParam('KJEventJoinId') ? 1 : 0, //（此字段建议后期优化掉，现在临时处理没有折扣的活动）砍价相关活动id活动id--DiscountType:1计算折扣
					"Type": 1,
					"PayAmount":me.vars.money||0
				},
				success:function(data){
					 if(data.ResultCode == 0){
                        //表示成功
                        if (obj.callback) {
                            obj.callback(data);
                        }
                     }else{
                        JitPage.alert(data.Message);
                     }
				}
			});
		},
		//获得验证码
		getAuthCode:function(mobile){
			Jit.UI.Loading(true);
			var me=this;
			JitPage.ajax({
	            url: '/ApplicationInterface/Gateway.ashx',
	            data: {
	                'action': 'VIP.Login.GetAuthCode',
	                'Mobile': mobile,
	                'VipSource':3
	            },
	            success: function (data) {
	                Jit.UI.Loading(false);
	                if(data.ResultCode==310){
	                	Jit.UI.Dialog({
			                'content': data.Message,
			                'type': 'Alert',
			                'CallBackOk': function () {
			                    Jit.UI.Dialog('CLOSE');
			                }
			            });
	                }else{
	                	//表示已经发送   1分钟后可以再次发送
	                	me.authCode=false;
	        			me.showTimer("#getCode");
	                }
	                //需要添加60秒倒计时代码
	            }
	        });
		},
		//设置支付密码
		setPayPass:function(obj){
			Jit.UI.Loading(true);
			var me=this;
			JitPage.ajax({
				url:me.urls.prefix,
				data:{
					"action":me.urls.setPayPassAction, //支付密码action
					"Mobile":obj.mobile,  //手机号
					"AuthCode":obj.authCode,  //验证码
					"Password":obj.password, //密码
					"PasswordAgain":obj.passAgain  //再次的密码
				},
				success:function(data){
					Jit.UI.Loading(false);
					if (data.ResultCode == 0) {
                        //表示成功
                        if (obj.callback) {
                            obj.callback(data);
                        }
                    }
                    else {
                        JitPage.alert(data.Message);
                    }
					
				}
			});
		},
		//检测支付密码
		checkPayPass:function(password,callback){
			Jit.UI.Loading(true);
			var me=this;
			JitPage.ajax({
				url:me.urls.prefix,
				data:{
					"action":me.urls.checkPayPassAction, //检测支付密码action
					"Password":password //密码
				},
				success:function(data){
					Jit.UI.Loading(false);
					 if (data.ResultCode == 0) {
                        //表示成功
                        if (callback) {
                            callback(data);
                        }
                    }
                    else {
                        JitPage.alert(data.Message);
                    }
					
				}
			});
		},
		//获得会员信息
		getVipInfo:function(callback){
			var me=this;
			JitPage.ajax({
				url:me.urls.prefix,
				data:{
					"action":me.urls.getVipInfoAction //获得VIP action
				},
				success:function(data){
					 if (data.ResultCode == 0) {
                        //表示成功
                        if (callback) {
                            callback(data);
                        }
                    }
				}
			});
		},
		elems:{
			couponDiv:$("#coupon"),    //优惠券层
			scoreDiv:$("#score"),      //积分层
			cashDiv:$('#returnCash'),   //返现层
			balanceDiv:$("#balance"),     //余额
			vipDiscounts:$(".vipDiscounts .textBox"), //会员折扣
			money:$("#money"),         //实际支付金额
			mask:$("#mask"),           //mask
			showCoupon:$("#showCoupon"),  //优惠券弹层
			showVip:$("#showVip"),      //显示是否已经是vip会员提示注册
			showPay:$("#showPay"),      //显示支付密码层
			showSetPass:$("#showSetPass"),  //显示设置密码
			useCoupon:$("#coupon .titWrap"), //是否使用优惠券
			useScore:$("#score .titWrap"),   //是否使用积分
			returnCash:$('#returnCash .titWrap'),//是否使用返现
			useBalance:$("#balance .titWrap") //是否使用余额
		},
		//初始化信息
		init:function(){
			this.initEvent();
		},
		//获得积分余额判断可用余额是否可以点击
		loadBanlanceData:function(callback){
			var me=this;
			//获取积分余额
			this.getCouponData({
				action:"getScoreAction",
				skuIds:me.vars.skuIds,
				callback:function(data){
					debugger;
					//积分
					me.vars.score=data.Data.Integral;
					//积分金额
					me.vars.scoreMoney=data.Data.IntegralAmount;
					//积分比例
					me.vars.scoreScale = me.vars.score/me.vars.scoreMoney;
					//最小使用积分
					me.vars.minScore=data.Data.PointsRedeemLowestLimit;
					//最小使用返现金额
					me.vars.minCash=data.Data.CashRedeemLowestLimit;
					//返现（0=不启用；1=启用）to:wLong
					me.vars.cash=data.Data.EnableRewardCash;
					//返现金额
					me.vars.cashMoney=data.Data.ReturnAmount;
					//会员折扣
					me.vars.discounts=data.Data.VipDiscount/10;
					//实际金额计算
					me.vars.orderMoney = parseFloat(Math.multiplied(me.vars.orderMoney,me.vars.discounts)).toFixed(2);
					//显示会员折扣
					me.elems.vipDiscounts.html(Math.subtr(me.vars.totalAmount,me.vars.orderMoney));
					//设置返现金额
					me.elems.cashDiv.find(".optionBox span").html("￥"+data.Data.ReturnAmount);
					//设置默认的返现的金额
					me.elems.cashDiv.attr("data-ammount",data.Data.ReturnAmount);
					//设置积分显示的内容
					me.elems.scoreDiv.find(".optionBox .integralNum").html(data.Data.Integral);
					me.elems.scoreDiv.find(".optionBox .integralAmount").html(data.Data.IntegralAmount);

					//设置默认的积分的金额
					me.elems.scoreDiv.attr("data-ammount",data.Data.IntegralAmount);

					if(data.Data.EnableIntegral!=1){
						me.elems.scoreDiv.hide();
					}

					//折扣率
					$('#vipDiscountsItem .optionBox span').text(me.vars.discounts*10);
					if(Number(data.Data.VipDiscount)<10){
						$('.vipDiscounts').children('.tix').children('span').html(data.Data.VipDiscount)
						$('#payOrder').children('.infoBox').find('p').eq(1).children('span').html(data.Data.VipDiscount);
					}else{
						$('#payOrder').children('.infoBox').find('p').eq(1).hide();
						$('.vipDiscounts').children('.tix').hide();
					}
					
					//设置实际支付的金额
					me.elems.money.html("￥"+me.vars.orderMoney);
					me.elems.money.attr("data-realmoney",me.vars.orderMoney);
					//是否启用层
					if(data.Data.EnableIntegral == 0){
						me.elems.scoreDiv.remove();//积分层
					}
					if(me.vars.cash == 0){
						me.elems.cashDiv.remove();//返现层
					}
					if(callback){
						callback();
					}
				}
			});
		},
		//根据选择的优惠券还有积分进行余额计算
		changeBalance:function(){
			var that = this,
				elems=this.elems, 
				couponMoney=0,   //优惠券对应的金额
				scoreMoney=0,    //积分对应的金额
				cashMoney=0,    //返现
				balance=0,      //余额
				orderMoney=0,
				shopMoney = Math.mul(that.vars.totalAmount,that.vars.discounts),//折扣后商品金额
				isCoupon = elems.useCoupon.hasClass("on"),
				isScore = elems.useScore.hasClass("on"),
				isCash = elems.returnCash.hasClass("on"),
				isBalance = elems.useBalance.hasClass("on"),
				discountMoney = Math.subtr(that.vars.totalAmount,shopMoney),
				$titWrap = $('.counteractArea .titWrap');
			//折扣金额
			var discountMoney = Number(discountMoney).toFixed(2);
			elems.vipDiscounts.html("￥"+discountMoney);

			//计算使用余额的金额
			//第一步判断是否选中了优惠券
			if(isCoupon){//如果使用则去查找对应的金额
				//选中的优惠券
				var itemDom=elems.showCoupon.find(".on");
				couponMoney=itemDom.attr('data-ammount');
			}
			//第二步判断是否使用了积分
			if(isScore){//使用积分兑换余额
				scoreMoney=elems.scoreDiv.attr('data-ammount');
				
				if(shopMoney<scoreMoney){
					//elems.scoreDiv.find(".optionBox .integralNum").html(scoreMoney);
					elems.scoreDiv.find(".optionBox .integralAmount").html(scoreMoney);
					elems.scoreDiv.attr("data-ammount",scoreMoney);
					//elems.scoreDiv.find(".optionBox .integralNum").html(Math.ceil(scoreMoney*this.vars.scoreScale));
				}
			}
			//第三步判断是否使用了返现
			
			if(isCash){//使用返现兑换余额
				cashMoney=elems.cashDiv.attr('data-ammount');
				if(shopMoney<cashMoney){
					elems.cashDiv.find(".optionBox span").html("￥"+shopMoney);
					elems.cashDiv.attr("data-ammount",shopMoney);
				}
			}
			//判断是否使用了余额
			if(isBalance){//使用返现兑换余额
				//balance=elems.balanceDiv.find(".tit").attr("data-balance");
				balance = elems.balanceDiv.find(".tit").attr("data-balanceCount");
				//2015/12/27 解决余额比实付金额大的时候，做的逻辑处理
				var minusMoney = Math.subtr(shopMoney,Math.add(couponMoney,scoreMoney));
					minusMoney=minusMoney>0?minusMoney:0;
				if(Number(minusMoney)<Number(balance)){
					minusMoney = Number(minusMoney).toFixed(2);
					balance = Number(minusMoney).toFixed(2);
					elems.balanceDiv.find(".optionBox span").html("￥"+minusMoney);
					elems.balanceDiv.find(".tit").attr("data-balance",minusMoney);
				}else{
					balance = Number(balance).toFixed(2);
					elems.balanceDiv.find(".optionBox span").html("￥"+balance);
					elems.balanceDiv.find(".tit").attr("data-balance",balance);
				}
			}


			//开始计算
			// if(!isScore){
			// 	//可用积分计算
			// 	//根据订单金额减掉 优惠券的金额  减去返现的金额,和余额进行比较  ： 积分的金额
			// 	var minusMoney3=Math.subtr(shopMoney,Math.add(Math.add(couponMoney,cashMoney),balance));
			// 		minusMoney3=minusMoney3>0?minusMoney3:0;
			// 	//获得小的金额
			// 	console.log(minusMoney3)
			// 	var usableScoreMoney=minusMoney3>this.vars.scoreMoney?this.vars.scoreMoney:minusMoney3;
			// 	//更改页面的积分显示内容
			// 	//elems.scoreDiv.find(".optionBox .integralNum").html(usableScoreMoney);
			// 	elems.scoreDiv.find(".optionBox .integralAmount").html(usableScoreMoney);
			// 	elems.scoreDiv.attr("data-ammount",usableScoreMoney);
				
			// 	if(!this.vars.scoreScale){
			// 		;
			// 		this.vars.scoreScale = 0;
			// 	}
			// 	elems.scoreDiv.find(".optionBox .integralNum").html(Math.ceil(usableScoreMoney*this.vars.scoreScale));
			// }
			if(!isCash){
				//可用返现计算
				//根据订单金额减掉 优惠券的金额  减去积分的金额,和余额进行比较  ： 返现的金额
				var minusMoney2=Math.subtr(shopMoney,Math.add(couponMoney,balance));
					minusMoney2=minusMoney2>0?minusMoney2:0;
				//获得小的金额
				var usableCashMoney=minusMoney2>this.vars.cashMoney?this.vars.cashMoney:minusMoney2;
				//更改页面的返现显示内容
				elems.cashDiv.find(".optionBox span").html("￥"+usableCashMoney);
				elems.cashDiv.attr("data-ammount",usableCashMoney);
			}
			if(!isBalance){
				//alert(this.vars.orderMoney);
				//第四步根据订单金额减掉 优惠券的金额  减去积分的金额,返现的金额和余额进行比较  ：   值小的则为要使用的余额
				var minusMoney1=Math.subtr(shopMoney,Math.add(couponMoney,scoreMoney));
					minusMoney1=minusMoney1>0?minusMoney1:0;
				//获得小的金额
				var usableBalance=minusMoney1>this.vars.balance?this.vars.balance:minusMoney1;
				//更改页面的余额显示内容
				usableBalance = Number(usableBalance).toFixed(2);
				elems.balanceDiv.find(".optionBox span").html("￥"+usableBalance);
				elems.balanceDiv.find(".tit").attr("data-balance",usableBalance);
			}
			//开始计算实付价格
			var minusMoney4=Math.subtr(shopMoney,Math.add(couponMoney,balance));
				minusMoney4=minusMoney4>0?minusMoney4:0;
			//获得要支付的实际金额
			//判断是否勾选了使用余额的操作
			// if(isScore){
			// 	realMoney=Math.subtr(minusMoney4,scoreMoney);
			// }else{
			// 	realMoney=minusMoney4;
			// }
			// if(elems.useBalance.hasClass("on")){
			// 	minusMoney4=Math.subtr(minusMoney,balance);
			// }else{
			// 	minusMoney4=minusMoney;
			// }
			minusMoney4=minusMoney4>0?minusMoney4:0;
			that.vars.money = minusMoney4;
			
			that.getCouponData({
				action:"getScoreAction",
				skuIds:that.vars.skuIds,
				callback:function(data){
					//积分
					if(data.Data.EnableIntegral!=1){
						that.elems.scoreDiv.hide();
					}
					//设置余额
					that.vars.score=data.Data.Integral;
					//积分金额
					that.vars.scoreMoney=data.Data.IntegralAmount;
					//积分比例
					that.vars.scoreScale = that.vars.score/that.vars.scoreMoney;
					//设置积分显示的内容
					that.elems.scoreDiv.find(".optionBox .integralNum").html(data.Data.Integral);
					that.elems.scoreDiv.find(".optionBox .integralAmount").html(data.Data.IntegralAmount);
					// $('#payOrder').children('.info').children('p').eq(0).children('span').html(data.Data.Integral);
					//设置默认的积分的金额

					that.elems.scoreDiv.attr("data-ammount",data.Data.IntegralAmount);

					
					var realMoney=0;
					var scoreMoney=data.Data.IntegralAmount;
					
					if(isScore){
						realMoney=Math.subtr(minusMoney4,scoreMoney);
					}else{
						realMoney=minusMoney4;
					}
					if(isBalance){//使用返现兑换余额
						//balance=elems.balanceDiv.find(".tit").attr("data-balance");
						balance = elems.balanceDiv.find(".tit").attr("data-balanceCount");
						//2015/12/27 解决余额比实付金额大的时候，做的逻辑处理
						var minusMoney = Math.subtr(shopMoney,Math.add(couponMoney,scoreMoney));
							minusMoney=minusMoney>0?minusMoney:0;
						if(Number(minusMoney)<Number(balance)){
							minusMoney = Number(minusMoney).toFixed(2);
							balance = Number(minusMoney).toFixed(2);
							elems.balanceDiv.find(".optionBox span").html("￥"+minusMoney);
							elems.balanceDiv.find(".tit").attr("data-balance",minusMoney);
						}else{
							balance = Number(balance).toFixed(2);
							elems.balanceDiv.find(".optionBox span").html("￥"+balance);
							elems.balanceDiv.find(".tit").attr("data-balance",balance);
						}
					}
					realMoney=realMoney>0?realMoney:0;
					//realMoney:抵扣后计算的价格

					//新的先折后抵方案
					var finalRealMoney = Number(realMoney).toFixed(2);//toFixed(2);Math.add(realMoney,0.0)
					elems.money.html("￥"+finalRealMoney);
					elems.money.attr("data-realmoney",finalRealMoney);
					Jit.UI.Loading(false);
				}
			});
				
			
			/*
			//老的先抵后折方案
			var discountPrice = Math.multiplied(realMoney,this.vars.discounts),
				discountMoney = parseFloat(Math.subtr(realMoney,discountPrice)).toFixed(2),
				finalRealMoney = 0;
			elems.vipDiscounts.html("￥"+discountMoney);
			//最终实际支付金额
			finalRealMoney = parseFloat(Math.subtr(Math.add(realMoney,JitPage.DeliveryAmount),discountMoney)).toFixed(2);
			elems.money.html("￥"+finalRealMoney);
			elems.money.attr("data-realmoney",finalRealMoney);
			*/
			//隐藏积分，优惠券，余额，返现的开关按钮
			// if(finalRealMoney==0){
			// 	$titWrap.hide();
			// 	$('.counteractArea .titWrap.on').show();
			// }else{
			// 	$titWrap.show();
			// }
		},
		//
		//设置支付密码的验证合法性
		validateData:function(){
			var flag=true;
			var phone=$("#phone").val();
			if(phone.length==0){
				JitPage.alert("手机号不能为空!");
				flag=false;
				return flag;
			}
			if(phone.length!=11){
				JitPage.alert("手机号长度为11位!");
				flag=false;
				return flag;
			}
		    if(!/^[0-9]*$/.test(phone)){
		    	JitPage.alert("手机号只能为数字!");
		        return false;
		    }
			var code=$("#code").val();
			if(code.length==0){
				JitPage.alert("验证码不能为空!");
				flag=false;
				return flag;
			}
			if(isNaN(parseInt(code))){
				JitPage.alert("验证码只能为数字!");
				flag=false;
				return flag;
			}
			var password=$("#pass").val();
			if(password.length==0){
				JitPage.alert("密码不能为空!");
				flag=false;
				return flag;
			}
			if(password.length<6&&password>0){
				JitPage.alert("密码长度最少为6位!");
				flag=false;
				return flag;
			}
			if(password.length>16){
				JitPage.alert("密码长度最多为16位!");
				flag=false;
				return flag;
			}
			var passagain=$("#passAgain").val();
			if(passagain.length==0){
				JitPage.alert("确认密码不能为空!");
				flag=false;
				return flag;
			}
			if(passagain.length<6&&passagain>0){
				JitPage.alert("确认密码长度最少为6位!");
				flag=false;
				return flag;
			}
			if(passagain.length>16){
				JitPage.alert("确认密码长度最多为16位!");
				flag=false;
				return flag;
			}
			if(passagain!=password){
				JitPage.alert("两次输入的密码不一致!");
				flag=false;
				return flag;
			}
			return true;
		},
		//倒计时显示
	    showTimer:function(id,flag){
	    	var count=60;
	    	var that=this;
	    	if(flag){//是否停止定时器
	    		$(id).html("发送验证码");
    			//表示已经发送   1分钟后可以再次发送
            	that.authCode=true;
	    		clearInterval(this.timerId);
	    	}else{
		    	this.timerId=setInterval(function(){
		    		if(count>0){
		    			--count;
		    			$(id).html("<font size='2'>"+count+"秒</font>后发送");
		    		}else{
		    			clearInterval(that.timerId);
		    			$(id).html("发送验证码");
		    			//表示已经发送   1分钟后可以再次发送
		            	that.authCode=true;
		    		}
		    	},1000);
		    }
	    },
		//优惠券相关事件
		initEvent:function(){
			var me=this;
			//获取href地址
			var strUrl = location.href;
			Jit.AM.setPageParam('setOrder',strUrl);
			//清空金额填写数据；
			$('#vipMoney input').val("");
			//选择优惠券事件
			this.elems.couponDiv.find(".titWrap").bind("click",function(){
				var titWrap=me.elems.couponDiv.find(".titWrap");
				var money = me.vars.totalAmount,
					acllMoney = me.elems.money.attr("data-realmoney");
					console.log(acllMoney)		
				if(titWrap.hasClass("on")){//已经选中  则取消选中状态  更改余额的价格
				titWrap.removeClass("on");
				me.elems.showCoupon.find(".on").removeClass("on");
				me.elems.mask.show();
				me.elems.showCoupon.show();
				//me.changeBalance();
				}else{
					//加载优惠券内容
					if(money!=0){
						me.elems.mask.show();
						//加载过 则只显示就行
						if(false){//me.elems.showCoupon.find(".item").length
							me.elems.showCoupon.show();
						}else{//获取优惠券
							debugger;
							
								me.elems.showCoupon.show();
								if(!$('.getStoreBtn').hasClass('on')){
									JitPage.carrierIdStr = '';
								}else{
									JitPage.carrierIdStr = JitPage.carrierId;
								}
								me.getCouponData({
									action:"getCouponAction",
									skuIds:me.vars.skuIds,
									carrierId:JitPage.carrierIdStr,		
									callback:function(data){
										var tpl=$('#Tpl_couponList').html();
										var html="";
										//优惠劵列表
										if(data.Data.CouponList&&data.Data.CouponList){
											for(var i=0;i<data.Data.CouponList.length;i++){
												var item=data.Data.CouponList[i];
												if(item.EnableFlag!=1){//表示可用
													item.FlagClass="off";
												}else{
													item.FlagClass="";
												}
												html+= Mustache.render(tpl,item);	
											}
											me.elems.showCoupon.find(".contentWrap").html(html);
										}else{
											me.elems.showCoupon.find(".contentWrap").html("没有优惠券信息");
										}
									}
								});
							
						}
						
					}else{
						debugger;
						JitPage.alert('请输入金额');
					}
				}
			});
			//优惠券确定选择的事件
			this.elems.showCoupon.find(".okBtn").bind("click",function(){
				//确定的时候把item上面为on的内容获得并填充
				var itemDom=me.elems.showCoupon.find(".on");
				if(itemDom.length){
					//设置选中的优惠券的名称
					$(me.elems.couponDiv.find(".optionBox>span")[0]).html('￥'+itemDom.data("ammount"));
					me.elems.couponDiv.find(".titWrap").html('已使用');
				}else{
					//设置选中的优惠券的名称
					$(me.elems.couponDiv.find(".optionBox")).html("抵<span>￥0</span>");
					me.elems.couponDiv.find(".titWrap").html('未使用');
				}
				me.elems.mask.hide();
				me.elems.showCoupon.hide();
				//计算余额
				Jit.UI.Loading(true);
				me.changeBalance();
			});
			//每个优惠券选择的事件
			this.elems.showCoupon.delegate(".item","click",function(){
				var $this=$(this);
				
				if($this.hasClass("off")){//表示优惠券不可用
					JitPage.alert("优惠券不可用!");
					return;
				}
				if(!$this.hasClass("on")){
					$this.addClass("on");
					$this.siblings().removeClass("on");
					me.elems.couponDiv.find(".titWrap").addClass("on");
				}else{
					$this.removeClass("on");
					me.elems.couponDiv.find(".titWrap").removeClass("on");
				}
					
			});
			//使用积分事件
			this.elems.useScore.bind("click",function(){
				//var ammount = me.elems.scoreDiv.data('ammount');
				console.log(me.vars.vipScore,me.vars.minScore,me.vars.score)
				if(me.vars.score<=0){//表示积分不能使用
					JitPage.alert("您暂时没有积分可用!");
				}else if(me.vars.vipScore<me.vars.minScore){
					JitPage.alert("满"+me.vars.minScore+"积分才能使用!");
				}else{
					//已经选中则进行取消
					if(me.elems.useScore.hasClass("on")){
						me.elems.useScore.removeClass("on");
					}else{
						me.elems.useScore.addClass("on");
					}
					Jit.UI.Loading(true);
					me.changeBalance();
				}
			});
			//使用返现的事件
			this.elems.returnCash.bind("click",function(){
				if(me.vars.cashMoney<=0){//表示积分不能使用
					JitPage.alert("您暂时没有返现可用!");
				}else if(me.vars.cashMoney<me.vars.minCash){
					JitPage.alert("返现满"+me.vars.minCash+"元才能使用!");
				}else{
					//已经选中则进行取消
					if(me.elems.returnCash.hasClass("on")){
						me.elems.returnCash.removeClass("on");
					}else{
						me.elems.returnCash.addClass("on");
					}
					me.changeBalance();
				}
			});
			//选中是否使用账户余额,选中的时候判断是否已经是会员,不是的话,则跳出领会员的层
			this.elems.useBalance.bind("click",function(){
				//是后使用余额   有On则表示已经选中  则取消
				if(me.elems.useBalance.hasClass("on")){
					me.elems.useBalance.removeClass("on");
					Jit.UI.Loading(true);
					me.changeBalance();
					return;
				}else{
					if(me.vars.balance>0){
						me.elems.useBalance.addClass("on");
						Jit.UI.Loading(true);
						me.changeBalance();
						return;
					}else{
						JitPage.alert("您的余额不足,不能使用");
					}
				}
				//判断是否有余额
				//if(me.vars.balance>0){
					//me.changeBalance();
					// if(me.vars.status!=undefined){
					// 	if(me.vars.status==1){//未领取会员卡
					// 		me.elems.mask.show();
					// 		me.elems.showVip.show();
					// 	}else if(me.vars.status==2){//已经领取
					// 		//判断账户是否激活
				 //       		if(me.vars.lockFlag==0){  //账户未锁定
				 //       			//弹出支付密码的层
				 //       			if(me.vars.passwordFlag==1){//已经设置支付密码
				 //       				me.elems.mask.show();
				 //       				me.elems.showPay.find("input").val("");
				 //       				me.elems.showPay.show();
				 //       			}else{//未设置支付密码   则弹出设置支付密码的层
				 //       				me.elems.mask.show();
				 //       				me.elems.showSetPass.show();
				 //       			}
				 //       		}else{
     //                            JitPage.alert("支付账户被冻结!");
				 //       		}
					// 	}else if(me.vars.status==0){
     //                        JitPage.alert("你的会员状态已取消，请联系管理员！");
     //                    }
					// }else{
					// 	//获得会员信息
					// 	me.getVipInfo(function(data){
					// 		me.vars.status=data.Data.Status;
					// 		if(data.Data.Status==1){//未领取会员卡
					// 			me.elems.mask.show();
					// 			me.elems.showVip.show();
					// 		}else if(data.Data.Status==2){//已经领取
					// 			//判断账户是否激活
					//        		if(me.vars.lockFlag==0){  //账户未锁定
					//        			//弹出支付密码的层
					//        			if(me.vars.passwordFlag==1){//已经设置支付密码
					//        				me.elems.mask.show();
					//        				me.elems.showPay.find("input").val("");
					//        				me.elems.showPay.show();
					//        			}else{//未设置支付密码   则弹出设置支付密码的层
					//        				me.elems.mask.show();
					//        				me.elems.showSetPass.show();
					//        			}
					//        		}else{
					//        			me.alert("支付账户被冻结!");
					//        		}
					// 		}
					// 	});
					//}
				//}else{
				//	JitPage.alert("您的余额不足,不能使用");
				//}
			});
			//隐藏会员领取的取消事件
			this.elems.showVip.find(".cancel").bind("click",function(){
				me.elems.mask.hide();
				me.elems.showVip.hide();
			});
			//确认支付
			this.elems.showPay.find(".surePay").bind("click",function(){
				var password=me.elems.showPay.find("input").val();
				if(password.length==0){
					JitPage.alert("支付密码不能为空");
					return;
				}
				if(password.length<6&&password>0){
					JitPage.alert("密码长度最少为6位!");
					return;
				}
				if(password.length>16){
					JitPage.alert("密码长度最多为16位!");
					return;
				}
				//检测支付密码
				me.checkPayPass(JitPage.MD5(password),function(data){
					Jit.UI.Loading(false);
					me.elems.balanceDiv.find(".titWrap").addClass("on");
					me.elems.mask.hide();
					me.elems.showPay.hide();
					me.changeBalance();
				});
			});
			//忘记密码
			this.elems.showPay.find(".forget").bind("click",function(){
				me.elems.showPay.hide();
				me.elems.showSetPass.show();
			});
			//关闭支付密码的层
			this.elems.showPay.find(".closeBtn").bind("click",function(){
				me.elems.showPay.hide();
				me.elems.mask.hide();
			});
			//设置密码的取消事件
			this.elems.showSetPass.find("#cancel").bind("click",function(){
				me.elems.mask.hide();
				me.elems.showSetPass.hide();
				//停止倒计时
    			me.showTimer("#getCode",true); //第二个参数为是否停止定时器
			});
			//设置密码的取消事件
			this.elems.showSetPass.find("#sureSet").bind("click",function(){
				//内容验证
				var flag=me.validateData();
				if(flag){//数据合法
					var phone=$("#phone").val();
					var passagain=$("#passAgain").val();
					var password=$("#pass").val();
					var code=$("#code").val();
					me.setPayPass({
						mobile:phone,
						password:JitPage.MD5(password),
						passAgain:JitPage.MD5(passagain),
						authCode:code,
						callback:function(data){
							Jit.UI.Loading(false);
							//设置支付密码状态成功
							me.vars.passwordFlag=1;
							me.elems.showSetPass.hide();
							me.elems.showPay.show();
							//me.elems.showSetPass.hide();
						}
					});
				}
			});
			//获取验证码
			this.elems.showSetPass.find("#getCode").bind("click",function(){
				if(me.authCode==undefined||me.authCode){
					var phone=$("#phone").val();
					if(phone.length==0){
						JitPage.alert("手机号不能为空!");
						return;
					}
					if(phone.length!=11){
						JitPage.alert("手机号长度为11位!");
						return;
					}
				    if(!/^[0-9]*$/.test(phone)){
				    	JitPage.alert("手机号只能为数字!");
				        return ;
				    }
	            	me.getAuthCode(phone);
	            }
			});
		}
	},
	//分销商扫码
	setOrderInfo:function(){
		//orderId=d3ed2b127bec478082958cd1a5ea4260
		var that = this,
			list = [{
	            'skuId': Jit.AM.getUrlParam('SkuId'),
	            'salesPrice': Jit.AM.getUrlParam('SkuPrice'),
	            'qty': Jit.AM.getUrlParam('SkuQty')
	        }];
        that.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            async: false,
            data: {
                'qty': list[0].qty,
                'totalAmount': list[0].qty * list[0].salesPrice,
                'action': 'setOrderInfo',
                'orderDetailList': list,
				'RetailTraderId':'',
				'SalesUser':Jit.AM.getUrlParam('SuperRetailTraderId') || '',
				'IsShared':Jit.AM.getUrlParam('isShared') || ''
            },
            success: function(data) {
                if (data.code == 200) {
                    //me.toPage('GoodsOrder', '&orderId=' + data.content.orderId + me.paramStr);
                	that.orderId = data.content.orderId;
                	Jit.AM.setPageHashParam('SUPERRETAILTRADERID',that.orderId);
                } else {
                    Jit.UI.Dialog({
                        'content': data.description,
                        'type': 'Alert',
                        'CallBackOk': function() {
                            Jit.UI.Dialog('CLOSE');
                        }
                    });
                }
				//TopMenuHandle.ReCartCount();
            }
        });
	},
    onPageLoad: function(){
		var me = this;
        me.orderInfo = null;
		me.retailTraderId = '';
		me.ownerVipId = '';
		//me.paramStr = '';
		if(Jit.AM.getUrlParam('channelId') == 6){
			$('body').prepend('<input id="channelIdSize" type="hidden" value="6">');
			$('#topNav').hide();
			$('.goods_wrap').css('margin-top','0px');
			me.ownerVipId = Jit.AM.getUrlParam('ownerVipId') || '';
		}else if(Jit.AM.getUrlParam('channelId') == 7){//7代表一起发码
			me.retailTraderId = Jit.AM.getUrlParam('RetailTraderId') || '';
			//me.paramStr = '&RetailTraderId='+me.retailTraderId+'&channelId=7';
			$('body').prepend('<input id="channelIdSize" type="hidden" value="7">');
		}else if(Jit.AM.getUrlParam('channelId') == 11){//分销商扫码
			$('body').prepend('<input id="channelIdSize" type="hidden" value="11">');
			me.Coupon.elems.scoreDiv.remove();//积分层
			me.Coupon.elems.cashDiv.remove();//返现层
			me.Coupon.elems.balanceDiv.remove();//余额
			$('.costInfoArea .vipDiscounts').hide();//会员折扣 
			me.orderId = Jit.AM.getPageHashParam('SUPERRETAILTRADERID') || '';
			if(!me.orderId){
				me.setOrderInfo();
			}
		}
        //人人销售的店员ID
		var salesUserId=Jit.AM.getPageParam("_salesUserId_"),
        	channelId=Jit.AM.getPageParam("_channelId_"),  //渠道ID
        	recommendVip=Jit.AM.getPageParam("_recommendVip_");  //推荐会员
        if(salesUserId&&channelId){
            var appVersion=Jit.AM.getAppVersion();
            appVersion.AJAX_PARAMS="openId,customerId,userId,locale,ChannelID";
            Jit.AM.setAppVersion(appVersion);
            Jit.AM.setPageParam("_salesUserId_",salesUserId);
            Jit.AM.setPageParam("_channelId_",channelId);
            //公用参数
            var baseInfo=Jit.AM.getBaseAjaxParam();
            baseInfo.ChannelID=channelId;
            Jit.AM.setBaseAjaxParam(baseInfo);
        }else{   //没有传递过来则把数据清空掉
            Jit.AM.setPageParam("_salesUserId_",null);
            Jit.AM.setPageParam("_channelId_",null);
        }
        me.initPageData();
        $('#vipMoney').delegate('.textBox input','keyup',function(){
        	var TotalAmount = $(this).val();
        	if(TotalAmount==''){
        		//me.alert('请输入正确的金额格式');
        		me.Coupon.vars.money=0;
        		me.Coupon.vars.totalAmount = 0;
        		me.Coupon.elems.balanceDiv.find('.optionBox').hide();
        		return false;
        	}
        	if(TotalAmount==0){
        		me.alert('请输入正确的金额格式');
        		me.Coupon.vars.money=0;
        		me.Coupon.vars.totalAmount = 0;
        		return false;
        	}
    		if(!TotalAmount.match(/^(([0-9]+)|([0-9]+\.[0-9]{0,2}))$/)){
        		me.alert('请输入正确的金额格式');
        		me.Coupon.vars.money=0;
        		me.Coupon.vars.totalAmount = 0;
        		return false;
        	}
        	Jit.UI.Loading(true);

        	me.Coupon.elems.balanceDiv.find('.optionBox').show();
        	me.Coupon.vars.totalAmount = TotalAmount;
        	me.Coupon.changeBalance();
   //      	me.Coupon.getCouponData({
			// 	action:"getScoreAction",
			// 	skuIds:me.Coupon.vars.skuIds,
			// 	callback:function(data){
			// 		if(data.Data.Integral){
			// 			if(!me.Coupon.elems.useScore.hasClass("on")){
			// 				me.Coupon.elems.useScore.trigger('click');
			// 			}
			// 		}else{
			// 			me.Coupon.elems.scoreDiv.find('.titWrap').removeClass('on');
			// 		}

			// 	}
			// });
        })
  //       //获取订单详情数据
		// me.ajax({
		// 	url : '/ApplicationInterface/Gateway.ashx',
		// 	data : {
		// 		'action' : 'Order.Order.GetOrderDetail',
		// 		'orderId' : me.getUrlParam('TimeStamp') || me.TimeStamp
		// 	},
		// 	success : function(data) {
		// 		if (data.IsSuccess) {
  //                   var order = data.Data.OrderListInfo,
  //                       tpl = $('#Tpl_goods_info').html(),
  //                       html = '',
  //                       totalprice = 0;
  //                   me.Coupon.vars.discounts = order.discount_rate/100;    
		// 			me.carrierId = order.StoreID;//CarrierID = 
  //                   me.initPageData(order);
  //                   $('#vipMoney').find('.textBox').html(order.TotalAmount);
  //                   for(var i = 0; i < order.OrderDetailInfo.length; i++) {
  //                       //totalprice += order.orderDetailList[i].salesPrice;
		// 				order.OrderDetailInfo[i]['image120'] = order.OrderDetailInfo[i].ImageInfo.length?Jit.UI.Image.getSize(order.OrderDetailInfo[i].ImageInfo[0].ImageUrl,'120'):"";
		// 				var gg="";
		// 				if(order.OrderDetailInfo[i].GG){
		// 					for(var j=0;j<5;j++){
		// 						var theName="PropDetailName"+(j+1);
		// 						var theName2="PropName"+(j+1);
		// 						if(order.OrderDetailInfo[i].GG[theName]&&order.OrderDetailInfo[i].GG[theName]!=""){
		// 							gg+=order.OrderDetailInfo[i].GG[theName2]+":"+order.OrderDetailInfo[i].GG[theName]+"; ";
		// 						}
		// 					}
		// 				}
		// 				order.OrderDetailInfo[i].GG=gg.substring(0,gg.length-2);
  //                       html += Mustache.render(tpl, order.OrderDetailInfo[i]);
  //                   }
  //                   $('#goods_list').append(html);
  //               } else {
  //                   Jit.UI.Dialog({
  //                       'content': data.Message,
  //                       'type': 'Alert',
  //                       'CallBackOk': function() {
  //                           Jit.UI.Dialog('CLOSE');
  //                       }
  //                   });
  //               }
		// 	}
		// });
        me.initEvent();
        //优惠券事件初始化
        me.Coupon.init();
    },
    
    initPageData: function(data) {
        var me = this;
        Jit.UI.Loading(true);
        // me.orderInfo = data;
        // me.isAllService = data.IsAllService;

        //是否为虚拟商品
        if(me.isAllService==3){
			$('.sendWayArea').remove();
			$('#fetchArea').remove();
			$('#orderLinksArea').remove();
		}
		$('.goodsOrderArea').show();
        /* 砍价*/
        me.isbargain = false;  //是否是砍价模块
        me.isbargain = Jit.AM.getUrlParam('KJEventJoinId') ? true : false;
        if (me.isbargain)
        {
            $("#score").remove();//积分层
            $("#returnCash").remove();//返现层
            $("#coupon").remove();//优惠卷层
            $("#balance").remove();//余额层
        }
        /* 砍价end*/
		//获取skuIds
		var array=[];
        
		//初始化优惠券数据
		me.Coupon.loadBanlanceData(function(){
			$('.submitBtn').attr('href','javascript:JitPage.submitOrder();');
		});
		//获取会员数据
		me.Coupon.getVipInfo(function(data){
			me.Coupon.vars.status=data.Data.Status;
			//是否设置了支付密码
			me.Coupon.vars.passwordFlag=data.Data.PasswordFlag;
			//是否账户锁定
			me.Coupon.vars.lockFlag=data.Data.LockFlag;
			me.Coupon.vars.vipScore = data.Data.EndIntegral;
			//设置余额
			//余额
			me.Coupon.vars.balance=data.Data.EndAmount;
			me.Coupon.elems.balanceDiv.find(".optionBox span").html("￥"+data.Data.EndAmount);
			me.Coupon.elems.balanceDiv.find(".tit").attr("data-balance",data.Data.EndAmount);
			me.Coupon.elems.balanceDiv.find(".tit").attr("data-balanceCount",data.Data.EndAmount);
			$('#payOrder').children('.info').children('p').eq(1).children('span').html(data.Data.EndAmount);
			$('#payOrder').children('.imgBox').find('img').attr('src',data.Data.HeadImgUrl);
			$('#payOrder').children('.infoBox').find('p').eq(0).html(data.Data.VipName);
			$('#payOrder').children('.info').children('p').eq(0).children('span').html(data.Data.EndIntegral);
			Jit.UI.Loading(false);
		});

		
    },
    initEvent: function() {
		var me = this;
		//监听配送方式的切换事件
		$('.sendWayArea .btnBox span').bind('click',function(){
			var $this = $(this),
				$orderLinksArea = $('#orderLinksArea'),
				$fetchArea = $('#fetchArea');
			if(!$this.hasClass('on')){
				$orderLinksArea.hide();
				$fetchArea.hide();
				$('.sendWayArea .btnBox span').removeClass('on');
				$this.addClass('on');
				$('.sendWayArea .addrBox').hide();
				$('.'+$this.data('item')).show();
				if($this.data('item')=='getHomeBox'){
					me.deliveryId = 1;
					//切换配送方式，控制运费
					$('.costInfoArea .freight').show();
					var params = {
						TotalAmount : JitPage.orderInfo.TotalAmount,
						DeliveryID : 1
					};
					me.GetDeliveryAmount(params,function(){
						me.Coupon.changeBalance();
					});
				}else{
					if(!me.isFirstClick){
						me.getPickingDate();
						me.isFirstClick++;
					}
					me.deliveryId = 2;
					$orderLinksArea.css({display: 'block'});
					$fetchArea.css({display:'block'});

					//取货日期；时间段 0-都不显示 1-显示日期 2-都显示
					if(me.isDisplay==0){
    					$fetchArea.hide();
    				}else if(me.isDisplay==1){
    					$('#fetchTime').hide();
    				}else if(me.isDisplay==2){
    					$('#fetchArea').show();
    				}

    				//切换配送方式，控制运费
    				$('.costInfoArea .freight').hide();
    				JitPage.DeliveryAmount = 0;
    				me.Coupon.changeBalance();
				}
			}
		});

		var orderId = me.getUrlParam('orderId') || me.orderId,
			channelId = me.getUrlParam('channelId'),
			channelIdStr = '';
		if(!!channelId){
			channelIdStr = '&channelId='+channelId;
		}
		$('.getHomeBox .modifyBtn').on('click',function(){
			me.toPage('SelectAddress', '&orderId=' + orderId + channelIdStr);
		});
		$('.getStoreBox .modifyBtn').on('click',function(){
			me.toPage('SelectStore', '&orderId=' + orderId + channelIdStr);
		});
    },
    //获取配送日期的起始日期
    getPickingDate: function(){
    	var me = this;
    	me.ajax({
    		url:'/ApplicationInterface/Gateway.ashx',
    		data:{
    			'action':'Order.Delivery.GetPickingDate'
    		},
    		success:function(data){
    			if(data.IsSuccess){
    				var result = data.Data;
    				me.beginDate = result.BeginDate;
    				me.endDate = result.EndDate;
    				me.isDisplay = result.IsDisplay;
    				//$('#fetchDate').val(me.beginDate);//默认显示日期
    				//0-都不显示 1-显示日期 2-都显示
    				if(me.isDisplay==0){
    					$('#fetchArea').hide();
    				}else if(me.isDisplay==1){
    					$('#fetchTime').hide();
    				}else if(me.isDisplay==2){
    					$('#fetchArea').show();
    					//me.getPickingQuantum();
    				}
    				me.setFetchTime();
    			}else{
    				me.alert(data.Message);
    			}
    		}
    	});
    },
    //获取配送时间段列表
    getPickingQuantum: function(){
    	var me = this;
		me.pickingDate = $('#fetchDate').val();
		me.ajax({
			url:'/ApplicationInterface/Gateway.ashx',
			data:{
				'action':'Order.Delivery.GetPickingQuantum',
				"BeginDate":me.beginDate,
				"PickingDate":me.pickingDate
			},
			success:function(data){
				if(data.IsSuccess){
					var result = data.Data.QuantumList || [],
						htmlStr = '<option>提货时间段</option>';
					if(!result.length){
						//me.alert('该日期没有配送时间段!');
					}else{
						for(var i=0;i<result.length;i++){
							htmlStr += '<option>'+result[i].Quantum+'</option>'
						}
						$('#fetchTime').html(htmlStr);
					}
				}else{
					me.alert(data.Message);
				}
			}
		});
    },
    //初始化日期插件
    setFetchTime: function(){
		var that = this,
			beginObj = that.beginDate.split('-'),
			endObj = that.endDate.split('-'),
			curr = new Date().getFullYear();
        var opt = {
            'date': {
                preset: 'date',
                //invalid: { daysOfWeek: [0, 6], daysOfMonth: ['5/1', '12/24', '12/25'] },
                minDate: new Date(beginObj[0],beginObj[1]-1,beginObj[2]),
                maxDate: new Date(endObj[0],endObj[1]-1,endObj[2])
            }
            /*
            'datetime': {
                preset: 'datetime',
                minDate: new Date(2012, 3, 10, 9, 22),
                maxDate: new Date(2014, 7, 30, 15, 44),
                stepMinute: 5
            },
            'time': {
                preset: 'time'
            },
            'select': {
                preset: 'select'
            },
            'select-opt': {
                preset: 'select',
                group: true,
                width: 50
            }
			*/
        };
        var demo = 'date';//$id
		//if (!demo.match(/select/i)) {
			//$('.demo-test-' + demo).val('');
		//}
		$('#fetchDate').scroller('destroy').scroller($.extend(opt[demo], {
			theme: 'default',
			mode: 'scroller',
			display: 'modal',
			animate: '',
			onSelect: function(){
				that.getPickingQuantum();
			}
		}));
    },
    //提交订单
    submitOrder: function() {
        var me = this,
		    _reMobile = /^1\d{10}$/,
			contactsText = $('#contactsText').val(),
			contactsPhoneText = $('#contactsPhoneText').val(),
			fetchDate = $('#fetchDate').val(),
			fetchTime = $('#fetchTime').val();
		// if(fetchTime=='提货时间段'){
		// 	fetchTime = '';
		// }

		// if(me.isAllService!=3){
		// 	if($('.getStoreBtn').hasClass('on')){
		// 		if (!me.isUnit) {
		// 			me.alert("请选择门店地址！");
		// 			return;
		// 		}
		// 	}else{
		// 		if (!me.isAddress) {
		// 			me.alert("请填写详细配送地址！");
		// 			return;
		// 		}
		// 	}
		// }
		//1-全部为实物商品 2-包含实物商品和虚拟商品 3-全部为虚拟商品
		// if(me.deliveryId==2 && me.isAllService!=3){
		// 	//0-都不显示 1-显示日期 2-都显示
		// 	if(me.isDisplay==1){
		// 		if(!fetchDate){
		// 			me.alert("请填写取货日期！");
		// 			return;
		// 		}
		// 	}else if(me.isDisplay==2){
		// 		if(!fetchDate){
		// 			me.alert("请选择取货日期！");
		// 			return;
		// 		}
		// 		if(fetchTime=='提货时间段' || !fetchTime){
		// 			me.alert("请选择取货时间段！");
		// 			return;
		// 		}
		// 	}
			
		// 	if(!contactsText){
		// 		me.alert("请填写联系人姓名！");
		// 		return;
		// 	}
		// 	if(!contactsPhoneText){
		// 		me.alert("请填写联系人电话！");
		// 		return;
		// 	}else if(!_reMobile.test(contactsPhoneText)){
		// 		me.alert("请填写正确联系人电话！");
		// 		return;
		// 	}
		// }

		
		// if(!me.orderInfo){
		// 	me.alert("订单信息丢失");
		// 	return;
		// }
		Jit.UI.Loading(true);
		//$('.submitBtn').attr('href','javascript:;');
		if(me.deliveryId==2){
			me.ajax({
				url:'/ApplicationInterface/Gateway.ashx',
				data:{
					'action':'Order.Delivery.UpdateOrderDeliveryInfo',
					'OrderID':Jit.AM.getUrlParam('TimeStamp') || me.orderId,
					'DeliveryTypeID':2,
					'StoreID':me.orderInfo.StoreID,//me.orderInfo.StoreID老的me.orderInfo.CarrierID
					'ReceiverAddress':$('.getStoreBox .contactInfo .cityName').text()+$('.getStoreBox .contactInfo .addrName').text(),
					//'Contacts':contactsText,//联系人
					//'ContactsPhone':contactsPhoneText//联系电话
					//'PickupUpDateRange':shopDate,
					'Mobile':contactsPhoneText,
					'ReceiverName':contactsText,
					'PickingDate':fetchDate,
					'PickingTime':fetchTime
				},
				success:function(data){
					if(data.IsSuccess){
						me.submit();
					}else{
						//$('.submitBtn').attr('href','javascript:JitPage.submitOrder();');
						Jit.UI.Loading(false);
						me.alert(data.Message);
					}
				}
			});
		}else{
			me.submit();
		}
    },
    submit:function(){
    	var me = this;
    	var money = $('#vipMoney').children('.textBox').children('input').val();
    	
    	if(money==""){
    		me.alert('请输入金额！');
    		Jit.UI.Loading(false);
    		return flase;
    	}
    	var elems=me.Coupon.elems;
    	//使用优惠券
    	var couponFlag=elems.useCoupon.hasClass("on")?1:0;
    	//优惠券ID
    	var couponId=elems.showCoupon.find(".on").data("id");
    	//是否使用积分
    	var integralFlag=elems.useScore.hasClass("on")?1:0;
		//是否使用返现
    	var returnAmountFlag=elems.returnCash.hasClass("on")?1:0;
    	//是否使用余额
    	var vipEndAmountFlag=elems.useBalance.hasClass("on")?1:0;
    	//余额
    	var vipEndAmount=elems.balanceDiv.find(".tit").data("balance");
		//var vipEndAmount=me.Coupon.vars.balance;
		//可使用积分（int）
		var integral=me.Coupon.vars.score;
		//可使用积分
		var integralAmount=elems.scoreDiv.data("ammount");
		//使用的返现金额（decimal）
		var returnAmount=elems.cashDiv.data("ammount");
		var vipDiscounts = parseFloat($('.vipDiscounts .textBox').text().substr(1));
		//实际金额
		var realMoney=elems.money.data("realmoney");
    	me.ajax({
            //url: '/OnlineShopping/data/Data.aspx',
            url:"/ApplicationInterface/Gateway.ashx",
            data: {
                'action': 'VIP.VipGolden.SetReceiveAmountOrder',
                'TimeStamp':me.getUrlParam('TimeStamp'),
                'UnitId':me.getUrlParam('UnitId'),
                'EmployeeID':me.getUrlParam('EmployeeID'),
                'CouponFlag':couponFlag,   //是否选用优惠券  1是 0 否
                'CouponId':couponId,       //优惠券ID
                'IntegralFlag':integralFlag,  //是否使用积分  1 是 0 否
                'VipEndAmountFlag':vipEndAmountFlag,  //是否使用余额  1是 0否
                'VipEndAmount':vipEndAmount,  //余额,
				'Integral':integral, 				//可使用积分（int）
				'IntegralAmount':integralAmount,		//可使用积分抵扣的金额（decimal）
				'ReturnAmountFlag':returnAmountFlag,  	//使用返现（0=不使用；1=使用）
				'ReturnAmount':returnAmount,		//使用的返现金额（decimal）
				'VipDiscount':vipDiscounts || 0, //折扣金额
				"TotalAmount":me.Coupon.vars.totalAmount
				// 'DeliveryAmount':JitPage.DeliveryAmount, //parseFloat($('.freight .textBox').text().substr(1)),//运费
				// 'EventId':me.getUrlParam('eventId') || '',//活动id
				// 'KJEventJoinId':me.getUrlParam('KJEventJoinId') || '',//砍价相关活动id活动id
				// 'RetailTraderId':me.retailTraderId,//分销商 老字段SalesUser
				// 'OwnerVipID':me.ownerVipId
            },
            success: function(data) {
                if (data.ResultCode ==0) {
                	//判断实际支付的金额是否是大于0   只要大于0
			       	var elems=me.Coupon.elems;
			       	//实际金额
			       	var realMoney=elems.money.data("realmoney");
			       	//不需要选择其他的支付方式通过余额优惠券直接支付
			       	if(realMoney==0){ //跳转到支付描述界面
			       		me.toPage('PaySuccess','useBalance=true');
			       	}else{
	                    //已下单
						var urlHtml = '';
						var orderId = data.Data.orderId;
						
						if(!!Jit.AM.getUrlParam('channelId')){
							urlHtml = '&channelId=' + Jit.AM.getUrlParam('channelId');
							me.toPage('OrderPay', '&orderId=' + (orderId || me.orderId) +'&isGoodsPage=1&realMoney='+realMoney + urlHtml);
						}else{
							me.toPage('OrderPay', '&orderId=' + (orderId || me.orderId)+'&isGoodsPage=1&realMoney='+realMoney+'&isP=1');
						}
	                    //TopMenuHandle.ReCartCount();
	                }
                }else{
					//$('.submitBtn').attr('href','javascript:JitPage.submitOrder();');
                	Jit.UI.Loading(false);
                	me.alert(data.Message);
                }
            }
        });
    },
    alert :function(text,callback){
		Jit.UI.Dialog({
			type:"Alert",
			content:text,
			CallBackOk:function(data){
				Jit.UI.Dialog("CLOSE");
				if(callback){
					callback();
				}
			}
		});
	},
	MD5 :function(string) {
		/*
		 *MD5 (Message-Digest Algorithm)
		 *http://www.webtoolkit.info/
		 */
		function RotateLeft(lValue, iShiftBits) {
			return (lValue<<iShiftBits) | (lValue>>>(32-iShiftBits));
		}
		function AddUnsigned(lX,lY) {
			var lX4,lY4,lX8,lY8,lResult;
			lX8 = (lX & 0x80000000);
			lY8 = (lY & 0x80000000);
			lX4 = (lX & 0x40000000);
			lY4 = (lY & 0x40000000);
			lResult = (lX & 0x3FFFFFFF)+(lY & 0x3FFFFFFF);
			if (lX4 & lY4) {
				return (lResult ^ 0x80000000 ^ lX8 ^ lY8);
			}
			if (lX4 | lY4) {
				if (lResult & 0x40000000) {
					return (lResult ^ 0xC0000000 ^ lX8 ^ lY8);
				} else {
					return (lResult ^ 0x40000000 ^ lX8 ^ lY8);
				}
			} else {
				return (lResult ^ lX8 ^ lY8);
			}
	 	}
	 	function F(x,y,z) { return (x & y) | ((~x) & z); }
	 	function G(x,y,z) { return (x & z) | (y & (~z)); }
	 	function H(x,y,z) { return (x ^ y ^ z); }
		function I(x,y,z) { return (y ^ (x | (~z))); }
		function FF(a,b,c,d,x,s,ac) {
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(F(b, c, d), x), ac));
			return AddUnsigned(RotateLeft(a, s), b);
		};
		function GG(a,b,c,d,x,s,ac) {
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(G(b, c, d), x), ac));
			return AddUnsigned(RotateLeft(a, s), b);
		};
		function HH(a,b,c,d,x,s,ac) {
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(H(b, c, d), x), ac));
			return AddUnsigned(RotateLeft(a, s), b);
		};
		function II(a,b,c,d,x,s,ac) {
			a = AddUnsigned(a, AddUnsigned(AddUnsigned(I(b, c, d), x), ac));
			return AddUnsigned(RotateLeft(a, s), b);
		};
		function ConvertToWordArray(string) {
			var lWordCount;
			var lMessageLength = string.length;
			var lNumberOfWords_temp1=lMessageLength + 8;
			var lNumberOfWords_temp2=(lNumberOfWords_temp1-(lNumberOfWords_temp1 % 64))/64;
			var lNumberOfWords = (lNumberOfWords_temp2+1)*16;
			var lWordArray=Array(lNumberOfWords-1);
			var lBytePosition = 0;
			var lByteCount = 0;
			while ( lByteCount < lMessageLength ) {
				lWordCount = (lByteCount-(lByteCount % 4))/4;
				lBytePosition = (lByteCount % 4)*8;
				lWordArray[lWordCount] = (lWordArray[lWordCount] | (string.charCodeAt(lByteCount)<<lBytePosition));
				lByteCount++;
			}
			lWordCount = (lByteCount-(lByteCount % 4))/4;
			lBytePosition = (lByteCount % 4)*8;
			lWordArray[lWordCount] = lWordArray[lWordCount] | (0x80<<lBytePosition);
			lWordArray[lNumberOfWords-2] = lMessageLength<<3;
			lWordArray[lNumberOfWords-1] = lMessageLength>>>29;
			return lWordArray;
		};
		function WordToHex(lValue) {
			var WordToHexValue="",WordToHexValue_temp="",lByte,lCount;
			for (lCount = 0;lCount<=3;lCount++) {
				lByte = (lValue>>>(lCount*8)) & 255;
				WordToHexValue_temp = "0" + lByte.toString(16);
				WordToHexValue = WordToHexValue + WordToHexValue_temp.substr(WordToHexValue_temp.length-2,2);
			}
			return WordToHexValue;
		};
		function Utf8Encode(string) {
			string = string.replace(/\r\n/g,"\n");
			var utftext = "";
			for (var n = 0; n < string.length; n++) {
				var c = string.charCodeAt(n);
				if (c < 128) {
					utftext += String.fromCharCode(c);
				}
				else if((c > 127) && (c < 2048)) {
					utftext += String.fromCharCode((c >> 6) | 192);
					utftext += String.fromCharCode((c & 63) | 128);
				}
				else {
					utftext += String.fromCharCode((c >> 12) | 224);
					utftext += String.fromCharCode(((c >> 6) & 63) | 128);
					utftext += String.fromCharCode((c & 63) | 128);
				}
			}
			return utftext;
		};
		var x=Array();
		var k,AA,BB,CC,DD,a,b,c,d;
		var S11=7, S12=12, S13=17, S14=22;
		var S21=5, S22=9 , S23=14, S24=20;
		var S31=4, S32=11, S33=16, S34=23;
		var S41=6, S42=10, S43=15, S44=21;
		string = Utf8Encode(string);
		x = ConvertToWordArray(string);
		a = 0x67452301; b = 0xEFCDAB89; c = 0x98BADCFE; d = 0x10325476;
		for (k=0;k<x.length;k+=16) {
			AA=a; BB=b; CC=c; DD=d;
			a=FF(a,b,c,d,x[k+0], S11,0xD76AA478);
			d=FF(d,a,b,c,x[k+1], S12,0xE8C7B756);
			c=FF(c,d,a,b,x[k+2], S13,0x242070DB);
			b=FF(b,c,d,a,x[k+3], S14,0xC1BDCEEE);
			a=FF(a,b,c,d,x[k+4], S11,0xF57C0FAF);
			d=FF(d,a,b,c,x[k+5], S12,0x4787C62A);
			c=FF(c,d,a,b,x[k+6], S13,0xA8304613);
			b=FF(b,c,d,a,x[k+7], S14,0xFD469501);
			a=FF(a,b,c,d,x[k+8], S11,0x698098D8);
			d=FF(d,a,b,c,x[k+9], S12,0x8B44F7AF);
			c=FF(c,d,a,b,x[k+10],S13,0xFFFF5BB1);
			b=FF(b,c,d,a,x[k+11],S14,0x895CD7BE);
			a=FF(a,b,c,d,x[k+12],S11,0x6B901122);
			d=FF(d,a,b,c,x[k+13],S12,0xFD987193);
			c=FF(c,d,a,b,x[k+14],S13,0xA679438E);
			b=FF(b,c,d,a,x[k+15],S14,0x49B40821);
			a=GG(a,b,c,d,x[k+1], S21,0xF61E2562);
			d=GG(d,a,b,c,x[k+6], S22,0xC040B340);
			c=GG(c,d,a,b,x[k+11],S23,0x265E5A51);
			b=GG(b,c,d,a,x[k+0], S24,0xE9B6C7AA);
			a=GG(a,b,c,d,x[k+5], S21,0xD62F105D);
			d=GG(d,a,b,c,x[k+10],S22,0x2441453);
			c=GG(c,d,a,b,x[k+15],S23,0xD8A1E681);
			b=GG(b,c,d,a,x[k+4], S24,0xE7D3FBC8);
			a=GG(a,b,c,d,x[k+9], S21,0x21E1CDE6);
			d=GG(d,a,b,c,x[k+14],S22,0xC33707D6);
			c=GG(c,d,a,b,x[k+3], S23,0xF4D50D87);
			b=GG(b,c,d,a,x[k+8], S24,0x455A14ED);
			a=GG(a,b,c,d,x[k+13],S21,0xA9E3E905);
			d=GG(d,a,b,c,x[k+2], S22,0xFCEFA3F8);
			c=GG(c,d,a,b,x[k+7], S23,0x676F02D9);
			b=GG(b,c,d,a,x[k+12],S24,0x8D2A4C8A);
			a=HH(a,b,c,d,x[k+5], S31,0xFFFA3942);
			d=HH(d,a,b,c,x[k+8], S32,0x8771F681);
			c=HH(c,d,a,b,x[k+11],S33,0x6D9D6122);
			b=HH(b,c,d,a,x[k+14],S34,0xFDE5380C);
			a=HH(a,b,c,d,x[k+1], S31,0xA4BEEA44);
			d=HH(d,a,b,c,x[k+4], S32,0x4BDECFA9);
			c=HH(c,d,a,b,x[k+7], S33,0xF6BB4B60);
			b=HH(b,c,d,a,x[k+10],S34,0xBEBFBC70);
			a=HH(a,b,c,d,x[k+13],S31,0x289B7EC6);
			d=HH(d,a,b,c,x[k+0], S32,0xEAA127FA);
			c=HH(c,d,a,b,x[k+3], S33,0xD4EF3085);
			b=HH(b,c,d,a,x[k+6], S34,0x4881D05);
			a=HH(a,b,c,d,x[k+9], S31,0xD9D4D039);
			d=HH(d,a,b,c,x[k+12],S32,0xE6DB99E5);
			c=HH(c,d,a,b,x[k+15],S33,0x1FA27CF8);
			b=HH(b,c,d,a,x[k+2], S34,0xC4AC5665);
			a=II(a,b,c,d,x[k+0], S41,0xF4292244);
			d=II(d,a,b,c,x[k+7], S42,0x432AFF97);
			c=II(c,d,a,b,x[k+14],S43,0xAB9423A7);
			b=II(b,c,d,a,x[k+5], S44,0xFC93A039);
			a=II(a,b,c,d,x[k+12],S41,0x655B59C3);
			d=II(d,a,b,c,x[k+3], S42,0x8F0CCC92);
			c=II(c,d,a,b,x[k+10],S43,0xFFEFF47D);
			b=II(b,c,d,a,x[k+1], S44,0x85845DD1);
			a=II(a,b,c,d,x[k+8], S41,0x6FA87E4F);
			d=II(d,a,b,c,x[k+15],S42,0xFE2CE6E0);
			c=II(c,d,a,b,x[k+6], S43,0xA3014314);
			b=II(b,c,d,a,x[k+13],S44,0x4E0811A1);
			a=II(a,b,c,d,x[k+4], S41,0xF7537E82);
			d=II(d,a,b,c,x[k+11],S42,0xBD3AF235);
			c=II(c,d,a,b,x[k+2], S43,0x2AD7D2BB);
			b=II(b,c,d,a,x[k+9], S44,0xEB86D391);
			a=AddUnsigned(a,AA);
			b=AddUnsigned(b,BB);
			c=AddUnsigned(c,CC);
			d=AddUnsigned(d,DD);
		}
		var temp = WordToHex(a)+WordToHex(b)+WordToHex(c)+WordToHex(d);
		return temp.toLowerCase();
	}

});