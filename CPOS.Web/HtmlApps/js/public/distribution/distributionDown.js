Jit.AM.defindPage({
    name: 'DistributionDown',
	element:{
	},
	onPageLoad : function() {
		Jit.UI.Loading(true);
		Jit.WX.OptionMenu(true);
		var that = this;
		that.initEvent();
		that.loadPageData();
	},
    //加载页面的数据请求
	loadPageData: function () {
	    var that = this;
	    that.getSuperRetailTrader();
	},
	
	initEvent: function(){
	    var that = this;
	},
	showSharePanel: function () {
	    $('#shareMask').show();
	},
	hideSharePanel: function () {
	    $('#shareMask').hide();
	},
    getSuperRetailTrader:function(){
        var that = this,
        	userId = JitPage.getBaseInfo().userId;
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'SuperRetailTrader.Login.BeSuperRetailTrader',
                "BeRYType":"Vip",//User:掌柜App员工;Vip:微信会员
                "SuperRetailTraderFromId":userId,//当前要成为分销商的会员ID或员工ID(若为空则表示要注册)
                "HigheSuperRetailTraderID":""//上级超级分销商
            },
            success: function (data) {
                if(data.ResultCode == 0){
					var result = data.Data;
					//IsSuperRetailTrader: 0=未成为分销商(失败);1=成为分销商(成功);2=已成为分销商(已成为)
					$('#nameText').text(result.SuperRetailTraderLogin);
					$('#businessNum').text(result.CustomerCode);
					if(!!result.SuperRetailTraderPass){
						$('#passwordText').text(result.SuperRetailTraderPass || '');
					}else{
						$('.userInfoBox .itemBox').eq(2).html('');
					}
                }else{
                	Jit.UI.Dialog({
						'content': data.Message,
						'type': 'Alert',
						'CallBackOk': function(){
							Jit.UI.Dialog('CLOSE');
						}
					});
					return false;
                }
                Jit.UI.Loading(false);
            }
        });
    }





}); 