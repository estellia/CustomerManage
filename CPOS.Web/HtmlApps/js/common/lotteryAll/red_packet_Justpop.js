Jit.AM.defindPage({
    name: 'redpacketJustpop',
    eventId: "",
	initWithParam: function(param) {
	    
	},
	onPageLoad : function() {
		this.initPage();
	},
	initPage: function () {
		var that = this;


		that.initEvent();


	},
	initEvent: function(){
		var that = this;

		$(".FiveRMB").on('click', function () {
		    Jit.AM.toPage("RedPacket", "eventId=406de2b8-cc10-46cc-a6f8-0731a2a0f7a6&confirminfor=100");
		   
            
		});

		$(".TenRMB").on('click', function () {
		    
		    Jit.AM.toPage("RedPacket", "eventId=ac3d9abc-2d79-48c1-b861-636298ceda3e&confirminfor=200");
		    
		});

		$(".FifteenRMB").on('click', function () {
		    
		    Jit.AM.toPage("RedPacket", "eventId=e0a15632-db08-4769-9c1b-6aa44503e9fa&confirminfor=400");
		   
		});


	},
    sharePage: function (img) {//分享设置
        var that = this;
        var info = Jit.AM.getBaseAjaxParam(),
            shareUrl = location.href;
        var title = document.title;
        shareUrl = shareUrl.replace('?applicationId=' + Jit.AM.getUrlParam('applicationId'), '');
		shareUrl = shareUrl.replace('&applicationId=' + Jit.AM.getUrlParam('applicationId'), '');
		shareUrl = shareUrl.replace('?rid=' + Jit.AM.getUrlParam('rid'), '');
		shareUrl = shareUrl.replace('&rid=' + Jit.AM.getUrlParam('rid'), '');
        shareUrl = shareUrl.replace('?sender=' + Jit.AM.getUrlParam('sender'), '');
        shareUrl = shareUrl.replace('&sender=' + Jit.AM.getUrlParam('sender'), '');
                       


	    var shareInfo = {
	        'title': title,
	        'desc': '积分可以兑代金券哦，赶紧来看看吧!',
	        'link': shareUrl,
	        'isAuth': true ,//需要高级auth认证
	        'imgUrl': img
	    };
	    Jit.AM.initShareEvent(shareInfo);

	}

}); 