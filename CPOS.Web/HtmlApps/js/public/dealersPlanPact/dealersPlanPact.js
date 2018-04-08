Jit.AM.defindPage({
    name: 'DealersPlanPact',
    questionnaireID: "",
    eventId: "",
    iserror: false,
    ismiyuezhuan: false,
    IsShare: false,
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
	},
	initEvent: function(){
	    var that = this;
	    debugger;
	    //绑定是否同意协议事件
	    $('.agree').on('click', "img", function () {
	        var check = $(this).parents(".agree").data("check");
	        if (check == "0") {
	            $(this).parents(".agree").data("check", "1")
	            $(this).attr("src", "/HtmlApps/images/public/dealersPlanPact/checkon.png");
	        } else {
	            $(this).parents(".agree").data("check","0")
	            $(this).attr("src", "/HtmlApps/images/public/dealersPlanPact/check.png");
	        }
	    });

	    $('.agree').on('click', "span", function () {
	        $(".mask").show();
	        $(".layer").show();
	    });

	    $('.Backbtn').on('click', function () {
	        window.history.back(-1);
	    });

	    $('.closebtn').on('click', function () {
	        $(".mask").hide();
	        $(".layer").hide();
	    });

	    $('.submit').on('click', function () {
	        var check = $(".agree").data("check");

	        if (check=="1") {
	            that.SetVipDealer(function (data) {
	                if (data.Message != "OK") {
	                    that.GetVipDealerUpset(function (data1)
	                    {
	                        Jit.AM.toPage('VirtualGoodsList', 'cardPrice='+data1.Data.Prices);
	                    });
	                } else {
	                    Jit.AM.toPage("DealersPlanIndex");
	                }
	            });
	        } else {
	            alert("需先同意经销商协议！");
	        }
	    });

	},
   
	SetVipDealer: function (callback) {//根据问卷ID获取题目及选项列表
	    var that = this;
	    that.ajax({
	        url: "/ApplicationInterface/Gateway.ashx",
	        data: {
	            action: 'VIP.Dealer.SetVipDealer'
	        },
	        success: function (data) {
	            if (data.IsSuccess && data.ResultCode == 0) {
	                if (callback) {
	                    callback(data);
	                }
	                
	            } else {
	                alert(data.Message);
	            }
	        }
	    });

	}, GetVipDealerUpset: function (callback) {//根据问卷ID获取题目及选项列表
	    var that = this;
	    that.ajax({
	        url: "/ApplicationInterface/Gateway.ashx",
	        data: {
	            action: 'VIP.Dealer.GetVipDealerUpset'
	        },
	        success: function (data) {
	            if (data.IsSuccess && data.ResultCode == 0) {
	                if (callback) {
	                    callback(data);
	                }

	            } else {
	                alert(data.Message);
	            }
	        }
	    });

	}
  

}); 