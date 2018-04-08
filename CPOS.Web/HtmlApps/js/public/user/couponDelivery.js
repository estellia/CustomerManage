/*定义页面*/
Jit.AM.defindPage({
	name: 'CouponDelivery',
	elements: {},
	onPageLoad: function() {
		//当页面加载完成时触发
		Jit.log('进入Coupon.....');
		this.initData();
		this.initEvent();
	},
	initData: function() {
		var self = this,
			urlCouponId = self.getUrlParam('couponId');
		self.elements.txtCouponCode = $('#txtCouponCode');
		self.elements.txtCouponName=$('#txtCouponName');
        Jit.UI.Loading(true);
		//获取列表类型
        self.ajax({
            interfaceMode: 'V2.0',
            url: '/ApplicationInterface/Module/Coupon/CouponHandler.ashx',
            data: {
                'action': 'getCouponDetail',
                'cuponID': urlCouponId
            },
            success: function (data) {
                Jit.UI.Loading(false);
                if (data.ResultCode && data.ResultCode == 200 && data.couponDetail) {
                    self.elements.txtCouponName.html(data.couponDetail.CouponName);
                    self.elements.txtCouponCode.html(data.couponDetail.CouponCode);
                }else{
					alert(data.Message);
				}
            }
        });
		self.getAddressList();
		Jit.WX.OptionMenu(true);
	},
	getAddressList: function(){
		var that = this,
			$contactInfo = $('.contactInfo'),
			$linkName = $('.linkName',$contactInfo),
			$tel = $('.tel',$contactInfo),
			$cityName = $('.cityName',$contactInfo),
			$addrName = $('.addrName',$contactInfo);
		that.ajax({
            //interfaceMode: 'V2.0',
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getVipAddressList'
            },
            success: function (data) {
                if(data.code == 200) {
				   if(!data.content.itemList || data.content.itemList.length==0){
				   	 $contactInfo.html('请选择地址');
					 $('.modifyBtn').text('选择');
				   }else{
				   	   var result = data.content.itemList[0];
					   $linkName.text(result.linkMan);
					   $tel.text(result.linkTel);
					   $cityName.text(result.cityName);
					   $addrName.text(result.address);
				   }
                }else{
					alert(data.description);
				}
            }
        });
	},
	onlineDeliveryBtn:function(){
	 	var self = this,
			$contactInfo = $('.contactInfo'),
			$linkName = $('.linkName',$contactInfo),
			$tel = $('.tel',$contactInfo),
			$cityName = $('.cityName',$contactInfo),
			$addrName = $('.addrName',$contactInfo);
	 	//获取列表类型
        self.ajax({
            interfaceMode: 'V2.0',
            url: '/ApplicationInterface/Vip/VipGateway.ashx',
            data: {
                'action': 'VoucherCouponOrder',
                'CouponId': self.getUrlParam('couponId'),
				'Remark': $('.remarksContent').val(),
				'linkMan':$linkName.text(),
				'linkTel':$tel.text(),
				'address':$addrName.text()
            },
            success: function (data) {
                if (data.ResultCode == 0) {
                   $('#onlineDelivery-prompt').show();
                }else{
					alert(data.Message);
				}
            }
        });
	},
	initEvent: function() {
		var self = this,
			$modifyBtn = $('.modifyBtn'),
			$onlineDeliveryBtn = $('.onlineDeliveryBtn'),
			$onlineDeliveryPrompt = $('#onlineDelivery-prompt');
		$modifyBtn.on('click',function(){
			var $this = $(this);
			self.toPage('SelectAddress','couponId='+self.getUrlParam('couponId'));
		});
		
		$onlineDeliveryBtn.on('click',function(){
			self.onlineDeliveryBtn();
		});
		
		$('.confirmBtn',$onlineDeliveryPrompt).on('click',function(){
			 self.toPage('CouponList');//'couponId='+self.getUrlParam('couponId')
		})
		
	}
});