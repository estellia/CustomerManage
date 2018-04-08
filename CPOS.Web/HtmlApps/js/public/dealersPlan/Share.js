Jit.AM.defindPage({
    name: 'DealersShare',
    questionnaireID: "",
    eventId: "",
    iserror: false,
    ismiyuezhuan: false,
    IsShare: false,
    ModelType: "1",
    back: window.history.back,
    element: {
    },
    onPageLoad: function () {
        this.initPage();
    },
    initPage: function () {
        var that = this;
        that.initEvent();
        //var ShareUserId = Jit.AM.getUrlParam('shareuserid') || '';
        that.getDimensionalCode(function (data) {
            $(".QRcodeImg img").attr("src", data.imageUrl);
           
        });
		that.sharePage("http://"+ location.host + "/HtmlApps/images/public/dealersPlanPact/ShareBGdefault.png");
    },
    initEvent: function () {
        var that = this;

    },
    getDimensionalCode: function (callback) {
        var that = this;
        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
				action: 'VIP.Dealer.GetVipDealerSpreadCode',
				//shareuserid: ShareUserId
				QRCodeId: Jit.AM.getUrlParam('qrCodeId') || ''
            },
            success: function (data) {
                if (data.IsSuccess && data.ResultCode == 0) {
                    var data = data.Data;
                    if(callback){
                        callback(data);
                    }

                } else {
                    alert(data.Message);
                }
            }
        });
    },
	sharePage: function (img) {//分享设置
	    var that = this;
	    var desc = "学习财商，创造财富！轻松分享，获得你的第一桶金!";
	    var info = Jit.AM.getBaseAjaxParam(),
            shareUrl = location.href;//+ "&shareuserid=" + info.userId
	    var title = '分享是金';

	    shareUrl = shareUrl.replace('&applicationId=' + Jit.AM.getUrlParam('applicationId'), '');
	    shareUrl = shareUrl.replace('&rid=' + Jit.AM.getUrlParam('rid'), '');
	    //shareUrl = shareUrl.replace("dealersPlanIndex.html", "Share.html");



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