Jit.AM.defindPage({
    name: 'UserRegister',
	initWithParam: function(param){
		//前端配置显示层
		//if(param['showMiddleArea']=='false'){
			//$('.noticeList').hide();
		//}
		//注册之后填写表单，判断是否需要进入我要分销页面
		// if(Jit.AM.getUrlParam('type')=='distribution'){
		// 	Jit.AM.toPage('DistributionDown');
		// }else{
		// 	window.history.go(-1);
		// }
	},
    onPageLoad: function () {
		var that = this;
        that.initEvent();
    },
    initEvent:function(){
        var that = this;
		$('#mobileInput').trigger('click');
		//获取验证码
		that.bindAuthCode();
		//联系方式
		that.bindSubmitBtn();
		

		$('#submitVipInfo').on('click',function(){
			that.saveVipInfo();
		});

    },
    bindSubmitBtn:function(){
    	var that = this,
			mobile = '',
			auth = '';
			_reMobile = /^1\d{10}$/;
		//联系方式
		$('#submitContactWay').on('click',function(){
			var $this = $(this);
			mobile = $('#mobileInput').val();
			auth = $('#authCardVal').val();
			if(!_reMobile.test(mobile)){
				Jit.UI.Dialog({
					'content': '请输入正确的手机号码！',
					'type': 'Alert',
					'CallBackOk': function(){
						Jit.UI.Dialog('CLOSE');
					}
				});
				return false;
			}
			if(auth == ''){
				Jit.UI.Dialog({
					'content': '请输入验证码！',
					'type': 'Alert',
					'CallBackOk': function(){
						Jit.UI.Dialog('CLOSE');
					}
				});
				return false;
			}
			$this.text('提交中...');
			$this.unbind('click');
			that.setContactWay(mobile,auth);
		})
    },
	bindAuthCode:function(){
		var that = this;
		$('.getAuthCodeBtn').bind('click',function(){
			mobile = $('#mobileInput').val();
			if(!_reMobile.test(mobile)){
				Jit.UI.Dialog({
					'content': '请输入正确的手机号码！',
					'type': 'Alert',
					'CallBackOk': function(){
						Jit.UI.Dialog('CLOSE');
					}
				});
				return false;
			}
			that.getAuthCode(mobile);
		})
	},
	getAuthCode:function(mobile){
		var that = this,
			timeNum = 90,
			$getAuthCodeBtn = $('.getAuthCodeBtn'),
			timer;
		$('.getAuthCodeBtn').unbind('click');
		$getAuthCodeBtn.html('重新获取('+timeNum+')');
		timer = setInterval(function(){
			if(timeNum>=0){
				$getAuthCodeBtn.html('重新获取('+timeNum+')');
				timeNum--;
			}else{
				clearInterval(timer);
				$getAuthCodeBtn.html('获取验证码');
				that.bindAuthCode();
			}
		},1000);
			
		that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.GetAuthCode',
				'Mobile':mobile,
				'VipSource':3
            },
            success: function (data) {
				if (data.ResultCode == 0){
					
				}else{
					Jit.UI.Dialog({
						'content': data.Message,
						'type': 'Alert',
						'CallBackOk': function(){
							Jit.UI.Dialog('CLOSE');
						}
					});
				}
            }
        });
	},
	setContactWay:function(mobile,auth){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.AuthCodeLogin',
				'Mobile':mobile,
				'AuthCode':auth,
				'VipSource':3,
				'CTWEventId':Jit.AM.getUrlParam('CTWEventId') || '',
				'couponId':Jit.AM.getUrlParam('couponId') || '',
				'registerType':Jit.AM.getUrlParam('registerType') || '0'
            },
            success: function (data) {
            	var code = data.ResultCode,
            		isFalse = '';
            	switch(code){
	              case 330:
	              case 331:
	              case 332:
	              case 333:
	              case 312:
	              case 314:
              		isFalse = true;
              		break;
	              default:
	               	isFalse = false;
		        }	
            	that.formInfoData = data;
            	if(Jit.AM.getUrlParam('registerType')==2 || isFalse){
            		that.gotoHref(that.formInfoData);
            	}else{
            		that.getRegisterForm();
            	}
            	
            	/*
				if (data.ResultCode == 316){
					//表示有可绑卡，跳到绑卡列表，(不需要提示)
					Jit.AM.toPage('BindEntityCard','registerType=1');
				}else if(data.ResultCode == 317){
					//表示没有可绑卡，跳到会员中心首页
					that.hintTips(data.Message);
					setTimeout(function(){
						Jit.AM.toPage('GetVipCard');
					},1500);
				}else if(data.ResultCode == 318){
					//表示有积分的领取成功的提示，跳到会员中心首页
					that.hintTips(data.Message);
					setTimeout(function(){
						Jit.AM.toPage('GetVipCard');
					},1500);
				}else if(data.ResultCode == 319){
					//表示领取成功，提示领取成功 不跳转实体卡页面，跳到会员中心首页
					that.hintTips(data.Message);
					setTimeout(function(){
						Jit.AM.toPage('GetVipCard');
					},1500);
				}else if(data.ResultCode == 320){
					//表示有可绑卡，跳转绑卡列表页面(不需要提示)
					Jit.AM.toPage('BindEntityCard');
				}else{
					that.hintTips(data.Message);
					$('#submitContactWay').text('确定');
					that.bindSubmitBtn();
				}
				*/
            }
		})
	},
	//提示内容
    showTips:function(content){
    	Jit.UI.Dialog({
            'content':content+"",
            'type': 'Alert',
            'CallBackOk': function () {
                Jit.UI.Dialog('CLOSE');
            }
        });
    	return false;
    },
    hintTips:function(content){
        Jit.UI.Dialog({
            'content':content,
            'type': 'Dialog',
            'isDpi':2,
            'times': 1500
        });
    },





    getRegisterForm:function(){
		var that = this;
		Jit.UI.Loading(true);
		that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Register.GetRegisterFormItems',
                'EventCode':'OnLine005'
            },
            success: function (data){
                if(data.ResultCode == 0){
					var pagesObj = data.Data.Pages;
					if(pagesObj.length == 0){
						that.gotoHref(that.formInfoData);
						//window.history.go(-1);
					}else{
						//样式修改，层切换
						$('#userRegisterBox').hide();
						$('#vipUserInfoBox').show();
						that.PageFormData = data.Data.Pages[0].Blocks[0].PropertyDefineInfos;
                    	that.buildRegisterForm(that.PageFormData);
					}
                }
                /*
                else{
                	that.showTips("未获得注册表单!");
                	return false;
                }
                */
				Jit.UI.Loading(false);
            }
        });
	},
	buildRegisterForm:function(items){
        var that = this,
			htmlstr = '';
        for(var i in items){
			var item=items[i];
            htmlstr += template.render('tpl_block_item',items[i]);
        }

        $('#vipUserInfoBox').prepend(htmlstr);

    },
    //验证每个表单项
    myValidate:function(value,dataType,dataText){
        var that=this,
        	_reMobile = /^1\d{10}$/;
        if(value==""&&(dataType!=5)){
            that.showTips(dataText+"不能为空!");
            return true;
        }
        //数值类型验证
        if(dataType==2){
            if(!/^(\-?)(\d+)$/.test(value)){
                that.showTips(dataText+"只能输入数字!");
                return true;
            };
        }
        //手机号验证
        if(dataType==3){
            if(!_reMobile.test(value)){
                that.showTips("手机号不正确!");
                return true;
            };
        }
        //邮件验证
        if(dataType==4){
            if(!/^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/.test(value)){
                that.showTips("邮箱格式不正确!");
                return true;
            };
        }
        return false;
    },
    //提交的内容验证
    contentVal:function($dom){
    	var that=this;
    	var value=$dom.val();
    	//内容  如  姓名     
    	var dataText=$dom.attr("data-text"),
    		dataType=$dom.attr("data-type"),      
    		isMustDo=$dom.attr("data-ismustdo");//是否必填
        if(value!=""){   //不为空则进行验证
        	return that.myValidate(value,dataType,dataText);
        }else{
            if(isMustDo==1){  //为空的时候必填的必须要判断
                return that.myValidate(value,dataType,dataText);
            }else{
                return false;
            }
        }
    },
	//保存信息
    saveVipInfo:function(){
        var that = this;
        var vipinfo = [];
        var inputDom=$('[name=vipinfo]');
        inputDom.each(function(i,dom){
        	var $dom=$(dom);
        	var dataText=$dom.attr("data-text");
        	if(that.contentVal($dom)){  //进行每个验证
        		return false;
        	}else{
        		vipinfo.push({
	                'ID':$dom.attr('wid'),
	                'IsMustDo':false,
	                'Value':$dom.val()
            	});
        	}
            
        });
		if(inputDom&&(inputDom.length==vipinfo.length)){
	        Jit.UI.Loading(true);
	        that.ajax({
	            url: '/ApplicationInterface/Gateway.ashx',
	            data: {
	                'action': 'VIP.Register.SetRegisterFormItems',
	                'ItemList':vipinfo,
	                'VipSource':3
	            },
	            success: function (data) {
	                if(data.ResultCode == 0){
						//alert('个人信息填写完成!');
						//window.history.go(-1);
						//Jit.AM.toPage('GetVipCard');
						that.gotoHref(that.formInfoData);
	                }else{
	                	that.showTips(data.Message);
	                }
					Jit.UI.Loading(false);
	            }
	        });
	   }
    },
    gotoHref: function(data){
    	var that = this;
    	if (data.ResultCode == 316){
    		//表示有可绑卡，跳到绑卡列表，(不需要提示)
    		Jit.AM.toPage('BindEntityCard','registerType=1');
    	}else if(data.ResultCode == 317){
    		//表示没有可绑卡，跳到会员中心首页
    		that.hintTips(data.Message);
    		setTimeout(function(){
    			Jit.AM.toPage('GetVipCard');
    		},1500);
    	}else if(data.ResultCode == 318){
    		//表示有积分的领取成功的提示，跳到会员中心首页
    		that.hintTips(data.Message);
    		setTimeout(function(){
    			Jit.AM.toPage('GetVipCard');
    		},1500);
    	}else if(data.ResultCode == 319){
    		//表示领取成功，提示领取成功 不跳转实体卡页面，跳到会员中心首页
    		that.hintTips(data.Message);
    		setTimeout(function(){
    			Jit.AM.toPage('GetVipCard');
    		},1500);
    	}else if(data.ResultCode == 320){
    		//表示有可绑卡，跳转绑卡列表页面(不需要提示)
    		Jit.AM.toPage('BindEntityCard');
    	}else{
    		that.hintTips(data.Message);
    		$('#submitContactWay').text('确定');
    		that.bindSubmitBtn();
    	}
    }
})