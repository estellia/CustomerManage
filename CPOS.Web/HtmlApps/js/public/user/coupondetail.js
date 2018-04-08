/*定义页面*/
Jit.AM.defindPage({
	name: 'CouponDetail',
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
		//self.elements.txtStatus = $('#txtStatus');
		self.elements.txtCouponDesc = $('#txtCouponDesc');
		//self.elements.txtVipName = $('#txtVipName');
		self.elements.txtBeginDate = $('#txtBeginDate');
		self.elements.txtEndDate = $('#txtEndDate');
		//self.elements.txtResidueDay = $('#txtResidueDay');
		self.elements.txtCouponCode = $('#txtCouponCode');
		self.elements.imgCouponCode = $('#imgCouponCode');
		//self.elements.txtDate = $('#txtDate');
		//self.elements.txtCountDown = $('.countDown');
		self.elements.txtCouponName=$('#txtCouponName');
		self.elements.couponLogo = $('.couponLogo');

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
                    self.elements.txtCouponDesc.html(data.couponDetail.CouponDesc);
                    //self.elements.txtVipName.html(data.couponDetail.VipName);
                    if (data.couponDetail.iseffective == 1) {//是否是永久有效
					
                    }else{
                        self.elements.txtBeginDate.html(data.couponDetail.BeginDate.substring(0, data.couponDetail.BeginDate.indexOf("T")));
                        self.elements.txtEndDate.html(data.couponDetail.EndDate.substring(0, data.couponDetail.EndDate.indexOf("T")));
                    }
                    if (data.couponDetail.isexpired == 1 || data.couponDetail.Status == 1 || data.couponDetail.isexpired == -1) {
                        if (data.couponDetail.isexpired == 1) {//已过期
                            $('.couponInfoTag').attr('class','couponInfoTag expireIcon');
							$('.onlineDeliveryBox').hide();
							//$('.couponInfoTag').attr('class','couponInfoTag usedIcon');//已使用
                        }
						if (data.couponDetail.isexpired == -1) {//未生效
							$('.onlineDeliveryBox').hide();	
                        }
						
                    }
					var couponName = data.couponDetail.CouponName,
						vipName = data.couponDetail.VipName,
						logoUrl = data.couponDetail.LogoUrl || '../../../images/public/user_default/logoPic.png?v1.0';
                    self.elements.txtCouponName.html(couponName);
                    //self.elements.txtResidueDay.html(data.couponDetail.diffDay);
                    self.elements.txtCouponCode.html(data.couponDetail.CouponCode);
                    self.elements.imgCouponCode.attr('src', data.couponDetail.QRUrl);
					
					self.elements.couponLogo.attr('src',logoUrl);
					
					self.sharePage(vipName,couponName,logoUrl);
                };
            }
        });
		
	},
	initEvent: function() {
		var that = this,
			$useRuleBox = $('#useRuleBox'),
			$deliveryBtn = $('.deliveryBtn');
		$useRuleBox.on('click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				$this.removeClass('on');
			}else{
				$this.addClass('on');
			}
		});	
		
		$deliveryBtn.on('click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				return;
			}else{
				that.toPage('CouponDelivery','couponId='+that.getUrlParam('couponId'));
			}
		});
		
		
		$('.g_share').bind('click',function(){
            $('#share-mask').show();
            $('#share-mask-img').show().attr('class','pullDownState');
        });
        
        $('#share-mask').bind('click',function(){
            var that = $(this);
            $('#share-mask-img').attr('class','pullUpState');
            setTimeout(function(){$('#share-mask-img').css({"display":"none"});that.css({"display":"none"});},500);
        });
		
	},
	//分享设置
	sharePage:function(vipName,couponName,logoUrl){
		var self = this,
			shareUrl = location.href.replace('coupondetail.html','couponDraw.html'),
			baseParam = Jit.AM.getBaseAjaxParam();
		var	shareInfo = {
			'title':'难得知己又送礼，还不快点拆！',
			'desc':vipName+'赠送您一张'+couponName+'的优惠券',
			'link':shareUrl+'&giverId='+baseParam.userId+'&couponId='+self.getUrlParam('couponId'),
			'imgUrl':'http://'+location.host+'/HtmlApps/images/public/user_default/couponDrawPic.png',//商户有logo显示logo没有显示默认的
			'isAuth':true//需要高级auth认证
		};
		Jit.AM.initShareEvent(shareInfo);
		// Jit.WX.OptionMenu(false);
	}
});