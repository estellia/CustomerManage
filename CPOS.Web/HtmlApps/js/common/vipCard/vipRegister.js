Jit.AM.defindPage({
    name: 'VipRegister',
	initWithParam: function(param){
		//前端配置显示层
		//if(param['showMiddleArea']=='false'){
			//$('.noticeList').hide();
		//}
	},
    onPageLoad: function () {
        this.initEvent();
    },
    initEvent:function(){
        var that = this,
			mobile = '',
			auth = '';
			_reMobile = /^1\d{10}$/;
		
		//获取验证码
		that.bindAuthCode();
		
		//联系方式
		$('#submitContactWay').on('click',function(){
			mobile = $('#mobileInput').val();
			auth = $('#authCardVal').val();
			if(!_reMobile.test(mobile)){
				alert('请输入正确的手机号码！');
				return false;
			}
			if(auth == ''){
				alert('请输入验证码！');
				return false;
			}
			that.setContactWay(mobile,auth);
			
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
	setContactWay:function(mobile,auth){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.AuthCodeLogin',
				'Mobile':mobile,
				'AuthCode':auth,
				'VipSource':3
            },
            success: function (data) {
				if (data.ResultCode == 0){
					//Jit.AM.toPage('MyCarAdd');
					//注册成功
					Jit.UI.Prompt({
							'title':'亲，注册成功！',
							'des':'你现在可以：',
							'html':'<a href=javascript:Jit.AM.toPage("GetVipCard"); class="btn01" style="width:40%;">去个人中心</a><a href=javascript:Jit.AM.toPage("GoodsList"); class="btn02" style="width:40%;">去购物</a>'});
					
					/*
					Jit.UI.Prompt({
							'title':'亲，验证成功！',
							'des':'会员卡号：'+'1353762424',
							'html':'<a href=javascript:Jit.AM.toPage("GetVipCard"); class="btn02" style="width:40%;">确定</a>'});
					*/
					
					/*
					Jit.UI.Prompt({
							'title':'亲，没有找到你的会员卡哦！',
							'des':'如有疑问请到门店咨询，谢谢！',
							'html':'<a href=javascript:Jit.AM.toPage("GetVipCard"); class="btn01" style="width:40%;">确定</a>'});
					*/
				}else{
					alert(data.Message);
				}
            }
		})
	}
})