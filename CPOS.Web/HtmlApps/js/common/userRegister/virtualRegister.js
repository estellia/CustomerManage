Jit.AM.defindPage({
    name: 'UserRegister',
	initWithParam: function(param){
		//前端配置显示层
		//if(param['showMiddleArea']=='false'){
			//$('.noticeList').hide();
		//}
	},
    onPageLoad: function () {
		var that = this;
        that.initEvent();
    },
    initEvent:function(){
        var that = this,
			mobile = '',
			auth = '',
			nameVal = '';
			_reMobile = /^1\d{10}$/;
		
		//获取验证码
		that.bindAuthCode();
		
		//联系方式
		$('#submitContactWay').on('click',function(){
			var $this = $(this);
			mobile = $('#mobileInput').val();
			auth = $('#authCardVal').val();
			nameVal = $('#nameVal').val();
			if(!_reMobile.test(mobile)){
				alert('请输入正确的手机号码！');
				return false;
			}
			if(auth == ''){
				alert('请输入验证码！');
				return false;
			}
			if(nameVal == ''){
				alert('请输入姓名！');
				return false;
			}
			$this.text('提交中...');
			$this.unbind('click');
			that.setContactWay(mobile,auth,nameVal);
			
		})
		
		$('#submitVipInfo').on('click',function(){
			
			that.saveVipInfo();
			
		})
		
    },
	bindAuthCode:function(){
		var that = this;
		$('.getAuthCodeBtn').bind('click',function(){
			mobile = $('#mobileInput').val();
			if(!_reMobile.test(mobile)){
				alert('请输入正确的手机号码！');
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
					alert(data.Message);
				}
            }
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
						window.history.go(-1);
					}else{
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
	setContactWay:function(mobile,auth,nameVal){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.AuthCodeLogin',
				'Mobile':mobile,
				'AuthCode':auth,
				'VipRealName':nameVal,
				'VipSource':3
            },
            success: function (data) {
				if (data.ResultCode == 0){
					//alert('恭喜您，已注册成为会员！请完善资料哦...');
					//that.getRegisterForm();
					//$('#userRegisterBox').hide();
					//$('#vipUserInfoBox').show();
					//document.title='填写资料';
					//Jit.AM.toPage('MyCarAdd');
					//注册成功
					/*
					Jit.UI.Prompt({
							'title':'亲，注册成功！',
							'des':'你现在可以：',
							'html':'<a href=javascript:Jit.AM.toPage("GetVipCard"); class="btn01" style="width:40%;">去个人中心</a><a href=javascript:Jit.AM.toPage("GoodsList"); class="btn02" style="width:40%;">去购物</a>'});
					*/
					Jit.AM.setPageParam('isUpVipCardStatus','2');
					window.history.go(-1);
				}else{
					alert(data.Message);
					$('#submitContactWay').text('确定');
					that.initEvent();
				}
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
    //验证每个表单项
    myValidate:function(value,dataType,dataText){
        var that=this;
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
            if(!(/^13\d{9}$/g.test(value)||(/^15\d{9}$/g.test(value))||(/^18[8,9]\d{8}$/g.test(value)))){
                that.showTips("手机号格式不正确!");
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
						Jit.AM.toPage('GetVipCard');
	                }else{
	                	that.showTips(data.Message);
	                }
					Jit.UI.Loading(false);
	            }
	        });
	   }
    }
})