Jit.AM.defindPage({
	name: 'ShopIndex',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		Jit.WX.OptionMenu(false);
		that.initData();
		that.initEvent();
	},
	initData: function(){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/VipStore/VipStoreGateway.ashx',
            data: {
                'action': 'VipStoreInfo'
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode === 0){
					var result = data.Data;
				   $('#goodsCount').html(result.StoreCount);
				   $('#newGoodsCount').html(result.RecentStoreCount);
				   $('#orderCount').html(result.OrderCount);
				   $('#monthIncomeCount').html(result.AmountCount);
				   $('#inviteCount').html(result.SetoffUserCount);
				   if(result.Ranking!=-1){
					 $('#rankingCount').html(result.Ranking);
				   }else{
					 $('#rankingCount').html('-');   
				   }
				}else{
				   alert(data.Message);	
				}
            }
        });
	},
	initEvent: function() {
		
	}
	
});