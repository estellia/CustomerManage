Jit.AM.defindPage({
	name: 'IncomeMonthCollect',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		Jit.WX.OptionMenu(false);
		that.initData();
	},
	initData: function() {
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/EveryoneSale/EveryoneGetway.ashx',
            data: {
                'action': 'GetAmountDetail'
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   var  htmlStr = '';
				   		listData = data.Data.AmountMonthList;
				   if(listData.length==0){
				   		$('.orderMonthCollectList ul').html("<div class='commonDataHint'>您还没有收入月汇总！</div>");
				   }else{
				   		for(var i=0;i<listData.length;i++){
							htmlStr += bd.template('tpl_incomeMonthCollect', listData[i]);  	
					   }
					   $('.orderMonthCollectList ul').html(htmlStr);
				   }
				}else{
				   alert(data.Message);
				}
            }
        });
	}
});