Jit.AM.defindPage({
    name: 'MyDistributionInit',
	element:{
	},
	onPageLoad : function() {
		Jit.WX.OptionMenu(false);
		var that = this;
		that.initEvent();
		//that.loadPageData();
	},
    //加载页面的数据请求
	loadPageData: function () {
	    var that = this;
	},

	initEvent: function(){
	    var that = this;
	    $('.distributorInitBtn').on('click',function(){
	    	that.checkIsRegister();
	    });
	},
	showSharePanel: function () {
	    $('#shareMask').show();
	},
	hideSharePanel: function () {
	    $('#shareMask').hide();
	},
    checkIsRegister:function(){
        var that = this;
        //Jit.UI.Loading(true);
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.GetMemberInfo',
                'VipSource':3
            },
            success: function (data) {
                if(data.ResultCode == 330 || (data.Data && data.Data.MemberInfo && data.Data.MemberInfo.Status==1)){
					//用户未注册
					Jit.AM.toPage('UserRegister','type=distribution');
                }else if(data.ResultCode == 0 && data.Data && data.Data.MemberInfo.Status==2){
					//用户已注册
					Jit.AM.toPage('DistributionDown');
                }
            }
        });
    },
    //分享设置
	sharePage: function (img) {
	    var that = this;
	    var desc = "";
	    var info = Jit.AM.getBaseAjaxParam(),
            shareUrl = location.href + "&qrCodeId=" + that.QRCodeId;//"&shareuserid=" + info.userId +
	    var title = document.title;

	    shareUrl = shareUrl.replace('&applicationId=' + Jit.AM.getUrlParam('applicationId'), '');
	    shareUrl = shareUrl.replace('&rid=' + Jit.AM.getUrlParam('rid'), '');
	    shareUrl = shareUrl.replace("dealersPlanIndex.html", "Share.html");
	    var shareInfo = {
	        'title': title,
	        'desc': desc,
	        'link': shareUrl,
	        'isAuth': true,//需要高级auth认证
	        'imgUrl': img
	    };
	    Jit.AM.initShareEvent(shareInfo);
	}
  

}); 