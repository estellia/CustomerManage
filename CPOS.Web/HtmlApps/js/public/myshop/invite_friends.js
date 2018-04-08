Jit.AM.defindPage({
	name: 'InviteFriends',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		//Jit.WX.OptionMenu(false);
		that.initData();
		that.initEvent();
		that.inviteFriends();
	},
	initEvent: function() {
		var that = this;
		$('.friendBtn').on('click',function(){
			that.shareHint();
		});
		$('.circleBtn').on('click',function(){
			that.shareHint();
		});
	},
	initData: function(){
		var that = this;
		//获取用户信息
		that.ajax({
            url: '/ApplicationInterface/Vip/VipGateway.ashx',
            data: {
                'action': 'GetVipInfo'
            },
            success: function (data) {
				if(data.ResultCode == 0){
				   var result = data.Data,
				   	   unitId= result.UnitId;
				   $('.headPic img').attr('src',result.HeadImgUrl);//头像
				   $('.nickname').html(result.VipName);//用户名
				   $('.integralCount').html(result.EndIntegral);//积分
				   $('.inviteCount').html(result.inviteCount);//邀请人数
				   //请求二维码
				   that.getQrPic(unitId);
				}else{
				   alert(data.Message);
				}
            }
        });
		
	},
	getQrPic: function(unitId){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Stores/StoresGateway.ashx',
            data: {
                'action': 'getDimensionalCode',
				'unitId': unitId,
				'VipDCode': 2  //二维码类型(1=返利；2我的小店)
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   var imageUrl = data.Data.imageUrl,
				   	   paraTmp = data.Data.paraTmp;
				   $('.qrCardPic').attr('src',imageUrl);
				   //等待扫一扫
				   QrTimer = setInterval(function(){
				   		that.timerPush(paraTmp,unitId);
				   },1500)
				   
				}else{
				   alert(data.Message);
				}
            }
        });
	},
	timerPush: function(paraTmp,unitId){
		var that = this;
		that.ajax({
            url: '/ApplicationInterface/Stores/StoresGateway.ashx',
            data: {
                'action': 'getDimensionalCodeByVipInfo',
				'special': {
					'unitId': unitId,
					'paraTmp': paraTmp
				}
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   if(data.Data.content.status == 1){
				   		clearInterval(QrTimer);
				        that.promptHint('恭喜<br>关注成功',function(){
							//刷新数据
							that.ajax({
								url: '/ApplicationInterface/Vip/VipGateway.ashx',
								data: {
									'action': 'GetVipInfo'
								},
								success: function (data) {
									if(data.ResultCode == 0){
									   var result = data.Data;
									   $('.integralCount').html(result.EndIntegral);//积分
									   $('.inviteCount').html(result.inviteCount);//邀请人数
									}else{
									   alert(data.Message);
									}
								}
							});
						});
				   }
				}else{
				   alert(data.Message);
				}
            }
        });
	},
	shareHint: function(){
		$('body').append('<div id="share-mask"><img id="share-mask-img" src="../../../images/public/my_shop/share-hint.png" class="pullDownState"></div>');
		//$('#share-mask').show();
		//$('#share-mask-img').show().attr('class', 'pullDownState');
		$('#share-mask').bind('click', function () {
			var that = $(this);
			$('#share-mask-img').attr('class', 'pullUpState').show();
			setTimeout(function () {that.remove(); }, 500);
		})
	},
	promptHint: function(str,callback){
		var htmlStr = '<div class="ui-mask" style="display:block;">\
		<div class="ui-dialogs ui-dialogs-prompt">\
			<p class="dialogs-text">'+str+'</p>\
			<div class="clearfix">\
				<a class="dialogs-btn" style="width:100%;display:block;" href="javascript:;">确定</a>\
			</div>\
		</div>\
		</div>';
		$('body').append(htmlStr);
		$('body').delegate('.dialogs-btn','click',function(){
			var $this = $(this);
			$this.parents('.ui-mask').remove();
			if(typeof callback==="function"){
				callback();
			}
		});
	},
	inviteFriends: function(){
		var that = this,
			title = '我的小店真不错！赚了,嘿嘿',
		    des = '快来加入我的小店吧,我快赚翻啦！',
			url = '',
			imgUrl = location.href.substring(0,location.href.indexOf("/html/")+1)+'images/public/my_shop/shareImg.jpg';
		that.ajax({
            url: '/ApplicationInterface/VipStore/VipStoreGateway.ashx',
            data: {
                'action': 'GetWeixinShareUrl',
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   //url =  data.Message;
				   // 发送给好友
				   //Jit.WX.shareFriends(title,des,url,imgUrl);
				   // 分享到朋友圈
				   //Jit.WX.shareTimeline(title,des,url,imgUrl);
				}else{
				   alert(data.Message);
				}
            }
        });
		
		
	}
});