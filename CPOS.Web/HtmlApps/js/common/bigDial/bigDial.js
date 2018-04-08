Jit.AM.defindPage({
    name: 'BigDial',
    eventId: "",
    back: window.history.back,
    interval: null, //大转盘定时器
    IsShare: false,
    element: {
        ctweventId: ""
    },
    result: {
        IsOk: false,
        Location: 0,
        alertName: "",
        IsData: false,
        PrizeId: null
    },//抽奖结果
	initWithParam: function(param) {
	    
	},
	element:{
		bigDialContent : $('#bigDialContent'),
		beginBtn : $('#beginBtn'),
		ruleBtn: $('.ruleBtn'),
		regular: $('.regular'),
		ruleArea: $('.ruleArea'),
		Drawstatus: true, //抽奖状态
	    scrollDiv:null
	},
	onPageLoad : function() {
		this.initPage();
	},
	initPage: function () {
		var that = this;
		window.onhashchange = function ()
		{
		    if (location.hash != "") {
		        that.element.ruleArea.show();
		    } else {

		        that.element.ruleArea.hide();
		    }
		}


	    debugger;
	    this.eventId = this.getUrlParam("eventId");


		
	    that.getRedpackData();

	    if (that.IsShare) {
	        that.setShareAH();
	    }

	    //加载中奖名单

		that.CycleloadPrize();

		that.initEvent();
		
	},
	initEvent: function(){
		var that = this;
		$('#beginBtn').on('click', function () {
		    var $this = $(this),
				totalPoint = Jit.AM.getUrlParam('totalPoint');
		    if (totalPoint < that.element.point) {
		        alert('亲，您的积点还不够哦！');
		        return;
		    }


		    that.initDraw();


		});

		$(".startbtn").on('click', function () {
		    $(".cover").hide();
		    $(".coverRule").hide();
		    $(".gameRule").show();
		});

	    //活动规则
		that.element.regular.on('click', function () {

		    window.location.href += "#a=1";
		});
		
		//游戏规则
		that.element.ruleBtn.on('click', function () {
		    window.location.href += "#a=1";
		});
		
		that.element.ruleArea.on('click', function () {
		    window.history.back(-1);
		})

	   
	},
	StopDisc: function (isClearChoose) {
	    var that = this;
	    clearInterval(that.interval);
	    that.element.Drawstatus = true;
	    if (isClearChoose)
	        $("#bigDialContent li").removeClass("current");
	}

    ,
	RunDisc: function () {
	    var that = this;
	    var list = $('li', that.element.bigDialContent),
                       len = list.length,
                       index = 1,
                       begin = that.element.beginBtn,
                       selIndex = false,
                       hintText = '',
                       prizesId = '',
                       result = that.result;
	    $("#bigDialContent li").removeClass("current");
	    begin.running = false;
	    if (begin.running) return;
	    begin.running = true;
	    begin.remain = 5000 + Math.random() * 1000;//5000与6000

	    that.interval = setInterval(function () {

	        if (begin.remain < 100 && selIndex) {
	            begin.running = false;
	            if (result.PrizeId == "0") {
	                that.alertDialog(result.alertName);
	            } else {
	                that.alertDialog("恭喜您中了“" + result.alertName + "”!");
	            }
	            that.StopDisc(false);

	        } else {
	            list[index - 1].className = "";
	            list[(index) % len].className = "current";
	            index = index % len;
	            index++;
	            begin.remain -= 100;
	        }

	        if (result.IsData) {
	            if (result.IsOk) {
	                if (result.Location == index) {
	                    selIndex = true;
	                } else {
	                    selIndex = false;
	                }
	            } else {
	                that.StopDisc(true);
	                that.alertDialog(result.alertName);
	            }
	        } else {
	            selIndex = false;
	        }

	       
	    }, 100);



	},
	alertDialog: function (message) {
	    Jit.UI.Dialog({
	        "type": "Alert",
	        "content": message,
	        "CallBackOk": function () {
	            Jit.UI.Dialog('CLOSE');
	        }
	    });
	}
	,
    //开始抽奖
	initDraw: function (callback) {
	    var that = this,
			params = {
			    'action': 'Event.Lottery.Roulette',
			    'EventId': that.element.ctweventId
			};

	    

	    if (that.element.Drawstatus) {

	        that.result.IsData = false;//抽奖结果初始化
	        that.result.IsOk = false;
	        that.result.alertName = "";
	        that.result.Location = 0;

	        that.element.Drawstatus = false;
	        //运行转盘
	        that.RunDisc();

	        that.ajax({
	            url: '/ApplicationInterface/Gateway.ashx',
	            //'async': false,   //设置为同步请求
	            data: params,
	            success: function (data) {
	                var result = data.Data;
	                if (data.IsSuccess && data.Data) {
	                    that.result.alertName = result.PrizeName ? result.PrizeName : result.ResultMsg;
	                    that.result.Location = result.Location;
	                    if (result.PrizeId != null && result.Location > 0 && result.Location < 13) {
	                        that.result.IsOk = true;
	                        that.result.PrizeId = result.PrizeId;
	                    }

	                } else {
	                    that.result.alertName = "错误信息：" + data.Message;
	                }
	                that.result.IsData = true;

	                Jit.UI.Loading(false);
	            },
	            error: function () {
	                that.result.alertName = "网络超时！";
	                that.result.IsData = true;

	            }
	        })
	        debugger;
	    } else {
	        that.alertDialog("已经在抽奖，请耐心等待！");
	    }
	},


    //获得初始化数据
	getRedpackData: function () {
	    var that = this;
	    var params = {
	        'action': 'Event.Lottery.GetImage',
	        'eventId': that.eventId,
	        'RecommandId': Jit.AM.getUrlParam('sender') || ''  //推荐人的id
	    };
	    that.ajax({
	        url: '/ApplicationInterface/Gateway.ashx',
	        async: false,   //设置为同步请求
	        data: params,
	        success: function (data) {
	            if (data.IsSuccess && data.Data) {
	                debugger;
	                var result = data.Data,
						info = Jit.AM.getBaseAjaxParam(),
                        shareUrl = location.href.replace('userId=' + info.userId, ''),
                        shareUrl = shareUrl.replace('openId=' + info.openId, ''),
	                	$LT_kvPic = $('#LT_kvPic'),//KV背景图片
	                	$LT_bgpic1 = $('#LT_bgpic1'),//背景图片1
	                	$LT_bgpic2 = $('#LT_bgpic2'),//背景图片2
	                	$LT_Rule = $('#LT_Rule'),//活动规则图片
	                	$LT_regularpic = $('.LT_regularpic');//试一试手气图片
	                that.IsShare = result.IsShare;

	              
	                $LT_kvPic.attr('src', result.LT_kvPic);
	                $LT_bgpic1.attr('src', result.LT_bgpic1);
	                $LT_bgpic2.attr('src', result.LT_bgpic2);
	                $LT_Rule.attr('src', result.LT_Rule);
	                $LT_regularpic.attr('src', result.LT_regularpic);

	                $(".bigDialArea").height($(".bigDialArea").width());


	                if (result.EventId) {
	                    that.element.ctweventId = result.EventId;
	                }

	                if (result.CoverInfo) {
	                    //封面背景图片初始化
	                    $("._BGImageSrc").attr("src", result.CoverInfo.CoverImageUrl);
	                    //封面按钮背景和字体颜色、规则颜色start
	                    $(".startbtn").attr("style", "background-color:" + result.CoverInfo.ButtonColor + ";color:" + result.CoverInfo.ButtonFontColor + ";");
	                    $(".regular").attr("style", "color:" + result.CoverInfo.ButtonColor + ";");
	                    //封面按钮背景和字体颜色、规则颜色end
	                    $(".startbtn").html(result.CoverInfo.ButtonText);
	                    if (result.CoverInfo.RuleType == 1) {
	                        $('#LT_Rule1').attr('src', result.CoverInfo.RuleImageUrl);
	                        $(".RuleContent1").hide();
	                        $('#LT_Rule1').show();
	                    } else {
	                        $(".RuleContent1").html(result.CoverInfo.RuleText);
	                        $(".RuleContent1").show();
	                        $('#LT_Rule1').hide();
	                    }
	                } else {
	                    $(".cover").hide();
	                    $(".coverRule").hide();
	                    $(".gameRule").show();
	                }




	                that.GetPrizeLocationList();
	            } else {
	                alert(data.Message);
	            }
	        }
	    });

	},
    
	GetPrizeLocationList: function () {//根据EventID获取奖品位置列表

	    var that = this;
	    that.ajax({
	        url: "/ApplicationInterface/Gateway.ashx",
	        data: {
	            action: 'Event.Lottery.GetPrizeLocationList',
	            EventID: that.element.ctweventId
	        },
	        success: function (data) {
	            if (data.IsSuccess && data.ResultCode == 0) {
	                debugger;
	                var data = data.Data;
	                var imgs = $("#bigDialContent img");
	                if (data.PrizeLocationList) {
	                    for (i = 0; i < data.PrizeLocationList.length; i++) {
	                        $("#Img" + data.PrizeLocationList[i].Location).data("index", data.PrizeLocationList[i].Location);
	                        $("#Img" + data.PrizeLocationList[i].Location).data("prizesid", data.PrizeLocationList[i].PrizeID);
	                        $("#Img" + data.PrizeLocationList[i].Location).data("prizesname", data.PrizeLocationList[i].PrizeName);
	                        $("#Img" + data.PrizeLocationList[i].Location).data("prizeLocationid", data.PrizeLocationList[i].PrizeLocationID);
	                        $("#Img" + data.PrizeLocationList[i].Location).addClass("_selected");
	                        if (data.PrizeLocationList[i].PrizeID == "")
	                            //$("#Img" + data.PrizeLocationList[i].Location).attr("src", "../../../images/common/bigDial/THX.png");
	                            $("#Img" + data.PrizeLocationList[i].Location).attr("src", "../../../images/common/bigDial/" + data.PrizeLocationList[i].ImageUrl.replace("images/", ""));
	                        else
	                            $("#Img" + data.PrizeLocationList[i].Location).attr("src", data.PrizeLocationList[i].ImageUrl);

	                    }
	                }

	                that.sharePage();
	            } else {
	                alert(data.Message);
	            }
	        }
	    });

	},
	CycleloadPrize: function ()//循环加载奖品名单
	{
	    var that = this;
	    if (that.element.Drawstatus) {
	        that.GetPrizeWinnerList();
	    }
	    setTimeout(function ()
	    {
	        that.CycleloadPrize();
	    }, 20000);
	},
	GetPrizeWinnerList: function () {//根据EventID获取奖品列表
	    var that = this;
	    that.ajax({
	        url: "/ApplicationInterface/Gateway.ashx",
	        data: {
	            action: 'Event.Lottery.GetPrizeWinnerList',
	            EventID: that.element.ctweventId
	        },
	        success: function (data) {
	            if (data.IsSuccess && data.ResultCode == 0) {
	                var data = data.Data;
	                var PrizeListstr = "";
	                if (data.WinnerList) {
	                    for (i = 0; i < data.WinnerList.length; i++) {
	                        PrizeListstr += "<li><span class='span1'>" + (data.WinnerList[i].VipName.length > 7 ? data.WinnerList[i].VipName.substring(0, 6) + "..." : data.WinnerList[i].VipName) + "</span><span  class='span2'>" + data.WinnerList[i].PrizeLevel + "</span>";

	                        var html = ""
	                        var long = 7;
	                        var value = data.WinnerList[i].PrizeName;
                            if (value && value.length > long) {
                                html = "<span  class='span3'>" + value.substring(0, long-1) + "...</span>";
                            } else {
                                html = "<span  class='span3'>"+ value + "</span>"
                            }

	                        PrizeListstr += html + "</li>";
	                    }
	                    $(".PrizeList").html(PrizeListstr);
	                    debugger;
	                    window.clearInterval(that.element.scrollDiv);
	                     $("#scrollDiv").textSlider({ line: 1, speed: 1000, timer: 100 });
	                     that.element.scrollDiv = $("#scrollDiv").textSlider.timerID;

	                }
	            } else {
	                alert(data.Message);
	            }
	        }
	    });

	    
	},
    setShareAH:function(){
        var that = this,
			params = {
			    'action': 'Event.Lottery.Share',
			    'EventId': that.eventId,
			    'ShareUserId': Jit.AM.getUrlParam('sender') || '' ,
			    'TypeId':'',
			};
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            //'async': false,
            data: params,
            success: function(data) {
                if (data.IsSuccess) {
                }else {
                    that.alert(data.Message);
                }
            }
        });
    },
    sharePage: function () {//分享设置
        var info = Jit.AM.getBaseAjaxParam(),
           shareUrl = location.href;
                    shareUrl = shareUrl.replace('?applicationId=' + Jit.AM.getUrlParam('applicationId'), '');
                    shareUrl = shareUrl.replace('&applicationId=' + Jit.AM.getUrlParam('applicationId'), '');
                    shareUrl = shareUrl.replace('?rid=' + Jit.AM.getUrlParam('rid'), '');
                    shareUrl = shareUrl.replace('&rid=' + Jit.AM.getUrlParam('rid'), '');
                    shareUrl = shareUrl.replace('?sender=' + Jit.AM.getUrlParam('sender'), '');
                    shareUrl = shareUrl.replace('&sender=' + Jit.AM.getUrlParam('sender'), '');
	    var shareInfo = {
	        'title': document.title,
	        'desc': '大奖抽不停，赶紧来看看吧!',
	        'link': shareUrl ,
	        'imgUrl': $('#LT_kvPic').attr('src'),
	        'isAuth': true//需要高级auth认证
	    };
	    Jit.AM.initShareEvent(shareInfo);

	}

}); 