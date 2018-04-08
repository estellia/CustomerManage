Jit.AM.defindPage({
	name:'ChannelAgent',
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
			productTryPage : $('.productTryPage'),
			productTryBtn : $('.productTryBtn')
		};
		me.initEvent();
	},
	initEvent:function(){
		var me = this;
		
		me.element.productTryBtn.on('click',function(){
			var _reMobile = /^1\d{10}$/,
				_reInt = /^[1-9]+[0-9]*]*$/,//正整数
				_reMail = /^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$/,
				nameText = $('.nameText input').val(),
				telText = $('.telText input').val(),
				mailText = $('.mailText input').val(),
				postText = $('.postText input').val(),
				companyName = $('.companyName input').val(),
				companyScale = $('.companyScale option:selected').val() || '',
				storeCount = $('.storeCount input').val() || '';
				
			if(nameText==''){
				alert('亲，请输入姓名！');
				return;
			}else if(telText==''){
				alert('亲，请输入手机号！');
				return;
			}else if(!_reMobile.test(telText)){
				alert('亲，请输入正确的手机号！');
				return;
			}else if(mailText==''){
				alert('亲，请输入邮箱！');
				return;
			}else if(!_reMail.test(mailText)){
				alert('亲，请输入正确的邮箱！');
				return;
			}else if(companyName==''){
				alert('亲，请输入公司名称！');
				return;
			}
			
			
			var objData = {
					'action': 'SaveAgentCustomer',
					'AgentCustomerInfo': {
						"AgentID":"",
						"AgentName":nameText,
						"AgentPhone":telText,
						"AgentMail":mailText,
						"AgentPost":postText,
						"CompanyName":companyName,
						"CompanyScale":companyScale,
						"StoreNumber":storeCount,
						"TryOrAgent":2 //产品试用：1 ， 代理商：2
					}
				};
			me.setApplyInfo(objData);
			
		});
	},
	setApplyInfo:function(obj){
		var me = this;
        me.ajax({
            url: '/ApplicationInterface/Module/NewAppContent/NewWXGateway.ashx',
            data: obj,
            success: function (data) {
                if(data.ResultCode == 0){
					alert('恭喜您，申请成功！');
					$('input').val('');
					/*
					Jit.UI.Dialog({
						'type':'Alert',
						'content':"",
						'CallBackOk':function(){
							//Jit.UI.Dialog('CLOSE');
							//Jit.AM.toPage('BigDial');
						}
					})
					*/
				}else{
					alert(data.Message);
				}
			}
		})
	}
	
}); 