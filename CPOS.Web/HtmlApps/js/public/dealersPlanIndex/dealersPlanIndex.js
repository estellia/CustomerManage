Jit.AM.defindPage({
    name: 'DealersPlanIndex',
    questionnaireID: "",
    eventId: "",
    iserror: false,
    ismiyuezhuan: false,
    IsShare: false,
    QrTimer:null,
    ModelType:"1",
    back: window.history.back,
	element:{
	},
	onPageLoad : function() {
		this.initPage();
	},
	initPage: function () {
		var that = this;
		that.initEvent();


		that.loadPageData();

		

	},
    //加载页面的数据请求
	loadPageData: function () {
	    var that = this;
	    
	    that.GetVipDealerAccountDetail(function (data) {
	        $(".DayIncome").html(data.DayIncome);//当天分润收入
	        $(".DayFans").html(data.DayFans);//当天收录粉丝
	        $(".MounthIncome").html(data.MounthIncome);//月分润收入
	        $(".SumIncome").html(data.SumIncome);//分润总收入
	        $(".MounthFans").html(data.MounthFans);//月累计粉丝
	        $(".SumFans").html(data.SumFans);//累计粉丝
	        $(".WithdrawSumMoney").html(data.WithdrawSumMoney);//账户佣金(可提现金额)
	    });
		
		that.getDimensionalCode(function (data1) {
			$(".QRcodeImg img").attr("src", data1.imageUrl);
			$(".QRhead img").attr("src", data1.HeadImgUrl);
			$(".QRhead span").html(data1.VipName);
			
			that.sharePage("http://"+ location.host + "/HtmlApps/images/public/dealersPlanPact/ShareBGdefault.png");
		});
	},

	initEvent: function(){
	    var that = this;
	    debugger;
	   
	    $('.colsebtn').on('click', function () {
	        window.history.back(-1);
	    });
	   
	    $(".detailed").on("click", function () {
	        Jit.AM.toPage("Fanslist");
	    })

	    $('.submit').on('click', function () {
	        window.location.href += "#a=1";
	    });

	    

	    $('.sharebtn').on('click', function () {
	        

	    });

	    $(".Withdrawals").on('click', function () {

	        Jit.AM.toPage("WithdrawDeposit");

	    });

	    window.onhashchange = function () {
	        if (location.hash != "") {
	            $(".QRcode").show();
	        } else {
	            that.loadPageData();
	            $(".QRcode").hide();
	        }
	    }


	},
	showSharePanel: function () {
	    $('#shareMask').show();
	},
	hideSharePanel: function () {
	    $('#shareMask').hide();
	},
	timerPush: function (paraTmp) {
	    var that = this;
	    that.ajax({
	        url: '/ApplicationInterface/Stores/StoresGateway.ashx',
	        data: {
	            'action': 'getDimensionalCodeByVipInfo',
	            'special': { 'paraTmp': paraTmp }
	        },
	        success: function (data) {
	            //Jit.UI.Loading(false);
	            if (data.ResultCode == 0) {
	                if (data.Data.content.status == 1) {
	                    clearInterval(QrTimer);
	                    alert("恭喜你发展了一个新粉丝！");
	                }
	            } else {
	                alert(data.Message);
	            }
	        }
	    });
	},
   
	GetVipDealerAccountDetail: function (callback) {//根据问卷ID获取题目及选项列表
	    var that = this;
	   
	    that.ajax({
	        url: "/ApplicationInterface/Gateway.ashx",
	        data: {
	            action: 'VIP.Dealer.GetVipDealerAccountDetail'
	        },
	        success: function (data) {
	            if (data.IsSuccess && data.ResultCode == 0) {
	                var data = data.Data;
	                if (callback) {
	                    callback(data);
	                }
	                
	            } else {
	                alert(data.Message);
	            }
	        }
	    });

	},

	getDimensionalCode: function (callback) {//根据问卷ID获取题目及选项列表
	    var that = this;

	    that.ajax({
	        url: "/ApplicationInterface/Gateway.ashx",
	        data: {
	            action: 'VIP.Dealer.GetVipDealerSpreadCode',
				//shareuserid: Jit.AM.getUrlParam('shareuserid') || '',
				QRCodeId: ''
	        },
	        success: function (data) {
	            if (data.IsSuccess && data.ResultCode == 0) {
	                var data = data.Data;
					that.QRCodeId = data.QRCodeId || '';
	                if (callback) {
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