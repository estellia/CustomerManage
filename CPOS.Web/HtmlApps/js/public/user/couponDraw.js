/*定义页面*/
Jit.AM.defindPage({
	name: 'CouponDraw',
	elements: {},
	onPageLoad: function() {
		//当页面加载完成时触发
		Jit.log('进入CouponDraw.....');
		this.initData();
	},
	initData: function() {
		var self = this;
        Jit.UI.Loading(true);
		//获取列表类型
        self.ajax({
            interfaceMode: 'V2.0',
            url: '/applicationInterface/Gateway.ashx',
            data: {
                'action': 'Coupon.Grant.GrantCoupon',
				'Giver': self.getUrlParam('giverId') || '',
                'CouponId': self.getUrlParam('couponId') || ''
            },
            success: function (data) {
                Jit.UI.Loading(false);
				debugger;
				var url = data.Data.FollowUrl,
					guideQRCode = data.Data.GuideQRCode,
					customerName = data.Data.CustomerName,
					messageText = data.Data.Message,
					isAccept = data.Data.IsAccept;//1为被领了
				if(isAccept == 1){
					$('.tipBox').hide();
				}
				$('.couponText').text(messageText);
				//$('.attentionBtn').attr('href',url);
				if(guideQRCode){
					$('.qrAttentionBtn img').attr('src',guideQRCode);
				}
				//$('.tipBox span').text(customerName);
            }
        });
		Jit.WX.OptionMenu(false);
	}
});