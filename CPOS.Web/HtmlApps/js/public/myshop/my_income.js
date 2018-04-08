Jit.AM.defindPage({
	name: 'MyIncome',
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
				var result = data.Data,
					monthAccount = result.MonthAccount,
					account =result.Account;
				$('#monthIncome').html(parseInt(monthAccount)==monthAccount?parseInt(monthAccount)+'.00':monthAccount);
				$('#totalIncome').html(parseInt(account)==monthAccount?parseInt(account)+'.00':account);
            }
        });
	},
	initEvent: function() {
		
	}
});