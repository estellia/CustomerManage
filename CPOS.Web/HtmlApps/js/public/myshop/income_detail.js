Jit.AM.defindPage({
	name: 'IncomeDetail',
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
		that.shopList();
	},
	shopList: function(typeId){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Order.GetOrders',
				'GroupingType': 3,
				'PageIndex':0,
				'PageSize':100
            },
            success: function (data) {
                //Jit.UI.Loading(false);
                if(data.ResultCode == 0){
				   var  htmlStr = '',
				   		listData = data.Data.Orders;
				   if(!listData){
				   		return $('.incomeDetailList').html("<div class='commonDataHint'>抱歉，您暂时没有收入明细！<a href='javascript:history.go(-1)';>返回</a></div>");
				   }else{
				   		for(var i=0;i<listData.length;i++){
							htmlStr += bd.template('tpl_incomeDetailList', listData[i]);  	
					   }
					   $('.incomeDetailList ul').html(htmlStr);
				   }
				}else{
				   alert(data.Message);
				}
            }
        });
	}
});