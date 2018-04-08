Jit.AM.defindPage({
	name: 'Binding',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		that.initEvent();
	},
	initEvent: function() {
		var that = this,
			$loginPage = $('#loginPage'),
			$registerPage = $('#registerPage'),
			$goBack = $('.goBack',$registerPage);
		$('.skipRegister').on('click',function(){
			$loginPage.hide();
			$registerPage.show();
		});
		$goBack.on('click',function(){
			$loginPage.show();
			$registerPage.hide();
		});
		
		var _re = /^1\d{10}$/,
			$phoneNum = $('#phoneNum'),
			$phoneNote = $('#phoneNote'),
			$getPasswordBtn = $('.getPasswordBtn'),
			$registerBtn = $('.registerBtn'),
			$loginPhoneNum = $('#loginPhoneNum'),
			$loginPassword = $('#loginPassword'),
			$loginBtn = $('.loginBtn'),
			$finishBtn = $('.finishBtn'),
			$fillPassword = $('#fillPassword'),
			$confirmPassword = $('#confirmPassword');
		
		$finishBtn.on('click',function(){
			if($fillPassword.val() == ''){
				return Jit.UI.Dialog({
					'content': '请输入密码！',
					'type': 'Alert',
					'CallBackOk': function() {
						Jit.UI.Dialog('CLOSE');
					}
				});
			}
			if($confirmPassword.val() == ''){
				return Jit.UI.Dialog({
					'content': '请输入确定密码！',
					'type': 'Alert',
					'CallBackOk': function() {
						Jit.UI.Dialog('CLOSE');
					}
				});
			}
			if($confirmPassword.val() != $fillPassword.val()){
				return Jit.UI.Dialog({
					'content': '两次输入的密码不一致！',
					'type': 'Alert',
					'CallBackOk': function() {
						Jit.UI.Dialog('CLOSE');
					}
				});
			}
			that.finishPassword($loginPhoneNum.val(),$confirmPassword.val());
			
		})	
			
		$loginBtn.on('click',function(){
			if(!_re.test($loginPhoneNum.val())){
				return Jit.UI.Dialog({
					'content': '请输入正确的手机号码！',
					'type': 'Alert',
					'CallBackOk': function() {
						Jit.UI.Dialog('CLOSE');
					}
				});
			}
			if($loginPassword.val()== ""){
				return Jit.UI.Dialog({
					'content': '请输入密码！',
					'type': 'Alert',
					'CallBackOk': function() {
						Jit.UI.Dialog('CLOSE');
					}
				});
			}
			
			that.login($loginPhoneNum.val(),$loginPassword.val());
		});	
			
		$registerBtn.on('click',function(){
			if(!_re.test($phoneNum.val())){
				return Jit.UI.Dialog({
					'content': '请输入正确的手机号码！',
					'type': 'Alert',
					'CallBackOk': function() {
						Jit.UI.Dialog('CLOSE');
					}
				});
			}
			if($phoneNote.val()== ""){
				return Jit.UI.Dialog({
					'content': '请输入短信验证码！',
					'type': 'Alert',
					'CallBackOk': function() {
						Jit.UI.Dialog('CLOSE');
					}
				});
			}
			that.register($phoneNum.val(),$phoneNote.val());
		});
		
		$getPasswordBtn.on('click',function(){
			
			if(!_re.test($phoneNum.val())){
				return Jit.UI.Dialog({
					'content': '请输入正确的手机号码！',
					'type': 'Alert',
					'CallBackOk': function() {
						Jit.UI.Dialog('CLOSE');
					}
				});
			}
			
			that.getAuthCode($phoneNum.val());
			
		});
		
		
		
	},
	getAuthCode: function(str){
		var that = this,
			opt = {
				"Parameters":{
					"Mobile":str,
					"IsReg":1
				}
			};
		opt = JSON.stringify(opt);
		$.ajax({
			url: "http://pmapi.chainclouds.com/Gateway.ashx?Action=QueryAuthCode&ReqContent="+opt,
			data: {},
			success: function(data) {
				var $getPasswordBtn = $('.getPasswordBtn'),
					timeNum = 90,
					timer;
				if (data.ResultCode == 200){
					$getPasswordBtn.html('重新获取('+timeNum+')');
					timer = setInterval(function(){
						if(timeNum>=0){
							$getPasswordBtn.html('重新获取('+timeNum+')');
							timeNum--;
						}else{
							clearInterval(timer);
							$getPasswordBtn.html('获取验证码');
						}
					},1000);
					
				}else{
					Jit.UI.Dialog({
						'content': data.Message,
						'type': 'Alert',
						'CallBackOk': function() {
							Jit.UI.Dialog('CLOSE');
						}
					});
				}
			}
		});
	},
	login: function(num,pass){
		var that = this,
			opt = {
				"Parameters":{
					"MemberNo":num,
					"Password":hex_md5(pass)
				}
			};
		opt = JSON.stringify(opt);
		$.ajax({
			url: "http://pmapi.chainclouds.com/Gateway.ashx?Action=MemberLogin&ReqContent="+opt,
			data: {},
			success: function(data) {
				if (data.ResultCode == 200) {
					//console.log(data);
					Jit.AM.setPageParam('_login_userInfo_',data.Data);
					//需要调整页面
					Jit.AM.toPage('GoodsDetail',"aldfrom="+Jit.AM.getPageParam("_aldfrom_"));
				}else{
					Jit.UI.Dialog({
						'content': data.Message,
						'type': 'Alert',
						'CallBackOk': function() {
							Jit.UI.Dialog('CLOSE');
						}
					});
				}
			}
		});
	},
	register: function(num,pass){
		var that = this,
			opt = {
				"Parameters":{
					"Mobile":num,
					//"MemberID":"",
					"AuthCode":pass
				}
			};
		opt = JSON.stringify(opt);
		$.ajax({
			url: "http://pmapi.chainclouds.com/Gateway.ashx?Action=AuthCodeLogin&ReqContent="+opt,
			data: {},
			success: function(data) {
				if (data.ResultCode == 200) {
					//console.log(data);
					//var info = Jit.AM.getBaseAjaxParam();获取公共信息
					//Jit.AM.setPageParam('_login_userInfo_',data.Data);
					//Jit.AM.toPage('GoodsOrder');
					//需要调整页面
					$('#authCodeLogin').hide();
					$('#changeMemberPWD').show();
				}else{
					Jit.UI.Dialog({
						'content': data.Message,
						'type': 'Alert',
						'CallBackOk': function() {
							Jit.UI.Dialog('CLOSE');
						}
					});
				}
			}
		});
	},
	finishPassword: function(num,pass){
		var that = this,
			opt = {
				"Parameters":{
					"MemberNo":num,
					"IsForget":1, //是否记住密码 1为记住
					"NewPWD":hex_md5(pass)
				}
			};
		opt = JSON.stringify(opt);
		$.ajax({
			url: "http://pmapi.chainclouds.com/Gateway.ashx?Action=ChangeMemberPWD&ReqContent="+opt,
			data: {},
			success: function(data) {
				if (data.ResultCode == 200) {
					//var info = Jit.AM.getBaseAjaxParam();获取公共信息
					Jit.AM.setPageParam('_login_userInfo_',data.Data);
					Jit.UI.Dialog({
						'content': '注册成功！',
						'type': 'Alert',
						'CallBackOk': function() {
							Jit.UI.Dialog('CLOSE');
							//需要调整页面
							Jit.AM.toPage('GoodsDetail',"aldfrom="+Jit.AM.getPageParam("_aldfrom_"));
						}
					});
					
				}else{
					Jit.UI.Dialog({
						'content': data.Message,
						'type': 'Alert',
						'CallBackOk': function() {
							Jit.UI.Dialog('CLOSE');
						}
					});
				}
			}
		});
	}
	
	
});