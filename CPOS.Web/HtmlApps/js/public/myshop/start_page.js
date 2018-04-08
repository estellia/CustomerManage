Jit.AM.defindPage({
	name: 'StartPage',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		Jit.WX.OptionMenu(false);
		that.initData();
	},
	initData: function(){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Vip/VipGateway.ashx',
            data: {
                'action': 'GetVipInfo'
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode === 0){
					if(data.Data.isStore){
						Jit.AM.toPage('ShopIndex');
					}else{
						$('.startArea').show();
					}
				}else{
					alert(data.Message);
				}
            }
        });
	}
});