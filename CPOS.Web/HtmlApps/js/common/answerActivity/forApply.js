Jit.AM.defindPage({
	name:'ForApply',
	initWithParam: function(param) {
	    
	},
	onPageLoad : function() {
		/*
		Jit.log('进入'+this.name+'.....');
		this.ajaxSending = false;
		this.data = null;
		this.eventId = this.getUrlParam("eventId");
        JitPage.setHashParam('EventId',this.eventId);
		if(!this.eventId){
			self.alert("eventId为空，未获取到活动信息");
			return false;
		}
		this.loadData();
		*/
		var me = this;
		me.loadData();
		
	},
	loadData:function(){
		var me = this;
		me.element={
			applyIntoPage : $('.applyIntoPage'),
			applyFormPage : $('.applyFormPage'),
			submitApplyBtn : $('.submitApplyBtn'),
			applyBtn : $('#applyBtn')
		};
		me.initEvent();
		me.isRegister();
	},
	initEvent:function(){
		var me = this;
		me.element.applyBtn.on('click',function(){
			//me.isRegister();
			me.isApply();
		});
		
		me.element.applyFormPage.delegate('.typeBox span','click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				$this.removeClass('on');
			}else{
				$this.addClass('on');
			}
		});
		
		me.element.submitApplyBtn.on('click',function(){
			var _reMobile = /^1\d{10}$/,
				_reInt = /^[1-9]+[0-9]*]*$/,//正整数
				nickName = $('#nickName').val(),
				territoryName = $('#territoryName').val(),
				ageNum = $('#ageNum').val(),
				phoneNum = $('#phoneNum').val(),
				likeType = '',
				$type = $('.typeBox .on',me.element.applyFormPage);
			if(nickName==''){
				alert('亲，请输入昵称！');
				return;
			}else if(territoryName==''){
				alert('亲，请输入地域！');
				return;
			}else if(ageNum==''){
				alert('亲，请输入年龄！');
				return;
			}else if(!_reInt.test(ageNum) || ageNum>120){
				alert('亲，请输入1~120的正整数！');
				return;
			}else if(phoneNum==''){
				alert('亲，请输入手机号！');
				return;
			}else if(!_reMobile.test(phoneNum)){
				alert('亲，请输入正确的手机号！');
				return;
			}
			
			for(var i=0;i<$type.length;i++){
				likeType += $type.eq(i).text()+',';
			}
			var objData = {
					'action': 'Extension.ActivityApply.SetActivityApply',
					'Nickname': nickName,
					'Territory': territoryName,
					'Age': ageNum,
					'Phone': phoneNum,
					'LikeTea': likeType
				};
			me.setApplyInfo(objData);
			
		});
	},
	setApplyInfo:function(obj){
		var me = this;
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: obj,
            success: function (data) {
                if(data.ResultCode == 0){
					Jit.UI.Dialog({
						'type':'Alert',
						'content':"欢迎加入鸣龙大家庭！",
						'CallBackOk':function(){
							//Jit.UI.Dialog('CLOSE');
							Jit.AM.toPage('BigDial');
						}
					})
				}else{
					me.alert(data.Message);
				}
			}
		})
	},
	isRegister:function(){
        var me = this;
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.GetMemberInfo',
                'VipSource':3
            },
            success: function (data) {
                if(data.ResultCode == 0){
					var result = data.Data.MemberInfo;
					if(result.Status==2){
						//me.isApply();
					}else{
						Jit.AM.toPage('UserRegister');
					}
				}else{
					me.alert(data.Message);
				}
			}
		})
	},
	isApply:function(){
		var me = this;
		me.ajax({
            url: '/applicationinterface/Gateway.ashx',
			data: {
				'action': 'Extension.ActivityApply.GetActivityApply'
			},
            success: function(data) {
                if (data.IsSuccess) {
                    if(!data.Data.IsApply){
						me.element.applyIntoPage.hide();
						me.element.applyFormPage.show();
					}else{
						alert('亲，您已报过名了!');
					}
					
                }else {
                    me.alert(data.Message);
                }
            }
        });
	}
}); 