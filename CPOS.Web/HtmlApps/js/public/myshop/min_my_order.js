Jit.AM.defindPage({
	name: 'MinMyOrder',
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
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'GetEveryOneAmount',
				'isstore': 1 //是否为我的小店
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				var result = data.Data;
				$('#monthOrderCount').html(result.OrderCount);
				$('#totalOrderCount').html(result.OrderMonthCount);
				//GroupingType:1待付款 2待收货/提货 3完成
				$('#noPayCount').html(result.GroupingOrderCounts[0].OrderCount); //待付款，未支付
				$('#noReceivingCount').html(result.GroupingOrderCounts[1].OrderCount); //待收货，已支付未完成			
				$('#completeCount').html(result.GroupingOrderCounts[2].OrderCount); //已完成
				$('#cancelCount').html('0'); //已取消
               	
            }
        });
	},
	initEvent: function() {
		
	}
});