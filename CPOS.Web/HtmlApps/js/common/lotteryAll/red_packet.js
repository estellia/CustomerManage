Jit.AM.defindPage({
    name: 'RedPacket',
    isjustpop:false,
    eventId:'',
    element: {
        ruleBtn: $('.ruleBtn'),
        regular: $('.regular'),
        ruleArea: $('.ruleArea'),
        IsCTW:0,
        ctweventId:""
    },
	initWithParam: function(param) {
	},
	onPageLoad : function() {
		//当页面加载完成时触发
		Jit.log('进入'+this.name+'.....');
		this.ajaxSending = false;
		this.data = null;
		this.eventId = this.getUrlParam("eventId");
        JitPage.setHashParam('EventId',this.eventId);
		if(!this.eventId){
			self.alert("eventId为空，未获取到活动信息");
			return false;
		}
		var that = this;
		window.onhashchange = function () {
		    if (location.hash != "") {
		        that.element.ruleArea.show();
		    } else {

		        that.element.ruleArea.hide();
		    }

		}

		this.loadData();
		this.initEvent();
	},
	loadData:function(){
		var that = this;
    	var hasOAuth = Jit.AM.getAppParam('Launch','CheckOAuth');
    	if(hasOAuth == 'unAttention'){
            var cfg = Jit.AM.getAppVersion();
			that.alert('参与本活动需要先关注微信公众号：'+cfg.APP_NAME);
            return false;
        }
		//获得初始化数据
        that.getRedpackData();
		//设置红包背景样式
		that.screenReset();
		window.onresize = that.screenReset;
	},
	initEvent:function(){
		var that = this;
		$('.Redbg').bind('click', function () {

		    //Justpop活动start
		    var confirminfor = that.getUrlParam("confirminfor");
		    
		    if (confirminfor && confirminfor != null) {
		        that.isjustpop = true;
		        confirminfor = "领取此红包的代金券需要使用"+confirminfor+"积分，确定领取吗？";
		        if (!confirm(confirminfor)) {
		            return;
		        }
		    }
		    //Justpop活动end

			Jit.UI.Loading();
			that.getDrawLottery();
		});

		$(".startbtn").on('click', function () {
		    $(".cover").hide();
		    $(".coverRule").hide();
		    $(".gameRule").show();

		    
		});

       

	    //分享有奖
		$(".registerbtn").on("click", function () {
		    var eventid = $(this).data("eventid");
		    Jit.AM.toPage('UserRegister', "CTWEventId=" + eventid);
		});

	    //活动规则
		that.element.regular.on('click', function () {

		    window.location.href += "#a=1";
		});

		that.element.ruleArea.on('click', function () {
		    window.history.back(-1);
		});


	    //游戏规则
		that.element.ruleBtn.on('click', function () {
		    window.location.href += "#a=1";
		});

		
	},
	screenReset:function(){
		var that = this,
			$bg = $('#redBgImg');
			//w = window.screen.width+'px',
			//h = window.screen.height+'px';
			$bg.css({'width':'100%','height':'100%'});
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
				if(data.IsSuccess){
				}else{
					that.alert(data.Message);
				}
			}
		});
	},
    //获得初始化数据
    getRedpackData:function(){
        var that=this;
        var params = {
            'action': 'Event.Lottery.GetImage',
            'eventId': that.eventId,
            'RecommandId': Jit.AM.getUrlParam('sender') || ''  //推荐人的id
        };
        //加载中奖信息
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            async: false,//设置为同步请求
            data: params,
            success: function(data) {
                if (data.IsSuccess && data.Data) {
                    debugger;
                    var result = data.Data,
						info = Jit.AM.getBaseAjaxParam(),
                        sharetitle = '',    //分享标题
                        sharecontent = '',  //分享内容
                        shareimgUrl='',   //分享图片
                        shareUrl = location.href.replace('userId='+info.userId,''),
                        shareUrl = shareUrl.replace('openId=' + info.openId, ''),
						shareUrl = shareUrl.replace('?applicationId=' + Jit.AM.getUrlParam('applicationId'), ''),
                        shareUrl = shareUrl.replace('&applicationId=' + Jit.AM.getUrlParam('applicationId'), ''),
						shareUrl = shareUrl.replace('?rid=' + Jit.AM.getUrlParam('rid'), ''),
						shareUrl = shareUrl.replace('&rid=' + Jit.AM.getUrlParam('rid'), ''),
                        shareUrl = shareUrl.replace('?sender=' + Jit.AM.getUrlParam('sender'), ''),
                        shareUrl = shareUrl.replace('&sender=' + Jit.AM.getUrlParam('sender'), '');
                        shareUrl = shareUrl.replace('?BeSharedOpenId=' + Jit.AM.getUrlParam('BeSharedOpenId'), ''),
                        shareUrl = shareUrl.replace('&BeSharedOpenId=' + Jit.AM.getUrlParam('BeSharedOpenId'), '');
                        shareUrl = shareUrl.replace('?CTWEventId=' + Jit.AM.getUrlParam('CTWEventId'), '');
                        shareUrl = shareUrl.replace('&CTWEventId=' + Jit.AM.getUrlParam('CTWEventId'), '');
						//shareUrl = shareUrl.replace('customerId='+info.customerId,''),
						$redBgImg = $('#redBgImg'),//红包背景
	                	$LT_Rule = $('#LT_Rule'),//活动规则图片
						//$getRedBtn = $('.getRedBtn'), //领取红包按钮
						//$redpackNum = $('.redpackNum'), //红包多少元
						$redpackLogo = $('.redpackLogo img'); //红包logo
						//$redpackBackNum = $('.redpackBackNum span'), // 红包中奖数多少元
						//$redpackBackBox = $('#redpackBackBox');
					that.redPacketImgInfo = result;

					if(!!result.Receive){
						$('#redpackBackBox .backRedbg').attr('src',result.Receive);
					}else{
						$('#redpackBackBox .backRedbg').attr('src','/HtmlApps/images/common/lotteryAll/backRedbg.png');
					}

					if(!!result.NotReceive){
						$('.Redbg img').attr('src',result.NotReceive);
						$('.getRedBtn').hide();
					}else{
						$('.Redbg img').attr('src','/HtmlApps/images/common/lotteryAll/Redbg.png');
					}

					if(!!result.IsShare){
						//是分享活动
						shareUrl = shareUrl.replace('customerId='+info.customerId,'');
					}

					if (result.CTWEventId)
					{
					    that.element.ctweventId = result.CTWEventId;
					}

					if (result.EventId)
					{
					    that.eventId = result.EventId;
					}

					$redBgImg.attr('src', result.BeforeGround);
                    //分享赋值
					sharetitle = (result.EventTitle || '好友推荐');
					sharecontent = (result.EventContent || '大奖等你抢！');
					shareimgUrl=(result.Logo|| result.BeforeGround || JitCfg.shareIco);

                    //创意仓库
					if (result.IsCTW == 1) {
					    that.element.IsCTW = result.IsCTW;

					    $redBgImg.attr('src', result.BackGround);
					    if (result.Reg) {
					        $(".registerbtn").attr("style", "background-color:" + result.Reg.Color + ";color:#fff;    display: block;");
					        $(".registerbtn").html(result.Reg.Text);
					        $(".registerbtn").data("eventid", result.CTWEventId);
					    } else {
					        $(".registerbtn").hide();
					    }


					    if (result.Focus) {
					        $(".QRcode img").attr("src", result.Focus.LeadPageQRCodeImageUrl);
					        $(".sharebg").attr("src", result.Focus.BGImageUrl);
					        $(".tips").html(result.Focus.Text);
					    } else {
					        $(".tips").hide();
					    }

					    

					    if (result.Share) {
					        $(".sharebtn").attr("style", "background-color:" + result.Share.Color + ";color:#fff; display: block;");
					        $(".sharebtn").html(result.Share.Text);
					        shareUrl += '&CTWEventId=' + result.CTWEventId + '&EventId=' + that.eventId + '&BeSharedOpenId=' + info.openId + '&H5URL=' + result.OnLineRedirectUrl.replace("http://", "").replace(/\//g, "_Slash_").replace(/\?/g, "_questionmark_").replace(/=/g, "_equal_");
					        shareUrl += '&oldurl=' + "/common/lotteryAll/red_loading".replace(/\//g, "_Slash_") + '&newurl=' + "/public/shop/news_activity_list".replace(/\//g, "_Slash_");
					        shareUrl = shareUrl.replace("red_packet", "red_loading");
					        sharetitle = result.Share.Title;
					        sharecontent = result.Share.Summary;
					        shareimgUrl = result.Share.BGImageUrl;
					        $(".sharebtn").on("click", function () {
					            that.showSharePanel();
					        });
					    } else {
					        $(".sharebtn").hide();
					    }
					    that.CTWEventShareLog();
					}

					var	shareInfo = {
					        'title': sharetitle,
					        'desc': sharecontent,
							'link':shareUrl,
							'imgUrl': shareimgUrl,
						    'Authparam':  [{ "paramname": "ObjectID", "paramvalue": result.CTWEventId}]
						};
						
					if (result.RuleId==2) {
					    $LT_Rule.attr('src', result.Rule);
					    $(".RuleContent").hide();
					    $LT_Rule.show();
					} else {
					    $(".RuleContent").html(result.RuleContent);
					    $(".RuleContent").show();
					    $LT_Rule.hide();
					}
					$redpackLogo.attr('src',result.Logo);	
					var img = new Image();
					img.src = result.BackGround;
					that.data = result;


					if (result.CoverInfo) {

					    if (result.CoverInfo.IsShow == "1") {

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
					    }
					} else {
					    $(".cover").hide();
					    $(".coverRule").hide();
					    $(".gameRule").show();
					}

					

					/*
					if(!result.Qualification){
						that.isDrawLottery(result.PrizeName);
					}
					*/
                    //result.BootUrl=result.BootUrl?result.BootUrl:"";
                    //result.BootUrl = ( (result.BootUrl.indexOf('http://')==-1)?('http://'+result.BootUrl):result.BootUrl );
					that.initIsShareEvent(shareInfo);

                }else {
                   that.alert(data.Message);
                }
            }
        });

    },
    //初始化红包页面数据
    getDrawLottery:function(){
        var that = this,
			params = {
                'action': 'Event.Lottery.RedPacket',
                'EventId': that.eventId
                //'RecommandId': Jit.AM.getUrlParam('sender') || ''
            };
		that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			'async': false,   //设置为同步请求
			data: params,
			success: function(data) {
				if (data.IsSuccess && data.Data) {
					var result = data.Data;
					//that.lotteryData = result;
					//result.BootUrl=result.BootUrl?result.BootUrl:"";
					//result.BootUrl = ((result.BootUrl.indexOf('http://')==-1)?('http://'+result.BootUrl):result.BootUrl );
					if (result) {
					    if (result.PrizeId && result.PrizeId.length>1) {
					        that.isDrawLottery(result.PrizeName, result.ParValue, result.CouponTypeName);

					        if (that.element.IsCTW == 1) {
					        	//javascript:JitPage.showSharePanel();
					            // setTimeout(function () {
					            //     $(".Redbg").hide();
					            //     $(".share").show();
					            // }, 3000);
								$('#redpackBackWrap').on('click',function(){
									$('#redpackBackWrap').hide();
									$(".Redbg").hide();
									$(".share").show();
								});
					        }

					    } else {
					        that.dialog({
					            'title': '提示',
					            'content': result.PrizeName ? result.PrizeName : result.ResultMsg,
					            'labelOk': '返回',
					            'callback_ok': 'JitPage.closeDialog()'
					        });
					    }
					}
					
				}else {
					me.alert(data.Message);
				}
				Jit.UI.Loading(false);
			}
		});
		
		/*
		Jit.UI.Loading();
		this.dialog({
			'title':'提示',
			'content':'您已领过红包，再次领取需要使用'+me.lotteryData.content.EventPoints+'点积分。',
			'labelOk':'返回',
			'labelCancel':'继续',
			'callback_ok':'JitPage.closeDialog()'
			//'callback_cancel':'JitPage.usePointGetRedPacket()'
		});
		*/
    },
    //分享日记
    CTWEventShareLog: function () {
        if (Jit.AM.getUrlParam('CTWEventId') != null) {
            var that = this,
                params = {
                    'action': 'CreativityWarehouse.Log.CTWEventShareLog',
                    'CTWEventId': Jit.AM.getUrlParam('CTWEventId'),
                    'Sender': Jit.AM.getUrlParam('sender'),
                    'OpenId': Jit.AM.getUrlParam('openId'),
                    'BeSharedOpenId': Jit.AM.getUrlParam('BeSharedOpenId'),
                    'BEsharedUserId': Jit.AM.getUrlParam('userId'),
                    'ShareURL': location.pathname
                    //'RecommandId': Jit.AM.getUrlParam('sender') || ''
                };
            that.ajax({
                url: '/ApplicationInterface/Gateway.ashx',
                data: params,
                success: function (data) {
                    if (data.IsSuccess && data.Data) {
                        var result = data.Data;

                        if (result) {

                        }

                    } else {
                        alert(data.Message);
                    }
                    Jit.UI.Loading(false);
                }
            });
        }

    },
    isDrawLottery: function (prizeName, ParValue, CouponTypeName) {
		var that = this;
		if(!prizeName){
			prizeName = '未中奖';
		}
		if (ParValue > 0) {
		    $('.redpackBackNum .ParValue').text(ParValue);
		    $(".Parunit").show();
		    //$(".backRedbg").attr("src","/HtmlApps/images/common/lotteryAll/backRedbg1.png");
		} else {
		    var leng = that.getByteLen(prizeName);

		    if (leng < 9) {
		        $('.redpackBackNum span').text(prizeName);
		    } else {
		        var str = prizeName;
		        var strlen = that.getByteLen(str);
		        while (strlen > 9) {
		            str = str.substring(0, str.length - 1);
		            strlen = that.getByteLen(str);
		        }
		        $('.redpackBackNum .ParValue').text(str + "…");
		    }
		    //$(".backRedbg").attr("src", "/HtmlApps/images/common/lotteryAll/backRedbg.png");

		    $(".Parunit").hide();
		}



		$('.redpackBackText').html(prizeName);//"恭喜你获得" + 
		
		//$('#redBgImg').attr('src', that.data.BackGround);
		$('.Redbg').hide();
		$('.getRedBtn').hide();
		$('#redpackBackWrap').show();
		//$('#redpackBackBox').show();

        //Justpop活动start
		// if (that.isjustpop) {
		//     $(".shareFriendBtn").hide();
		// } else {
		//     $(".shareFriendBtn").show();
		// }
		$(".shareFriendBtn").hide();
		$('.redpackBackNum').hide();
        //Justpop活动end
	},

	initIsShareEvent:function(shareInfo){
		var that = this;
        var info = Jit.AM.getBaseAjaxParam();
		//是分享活动 + '&eventId=' + that.eventId
		shareInfo.link = shareInfo.link;
		shareInfo.isAuth=true;//需要高级auth认证

		Jit.WX.shareFriends(shareInfo);
		Jit.WX.shareTimeline(shareInfo);
        if(!!that.data.IsShare){
        	//分享给积分
			that.setShareAH();
        }

	},
    showSharePanel:function(){
    	$('#shareMask').show();
    },
    hideSharePanel:function(){
    	$('#shareMask').hide();
    },
	dialog:function(cfg){
    	var tpl = $('#tpl_dialog').html();
    	tpl = Mustache.render(tpl,cfg);
    	$(tpl).appendTo('body');
    },
    closeDialog:function(){
    	$('#dialog').remove();
    },
    alert:function(msg){
    	this.dialog({
    		'title':'提示',
			'content':msg,
			'labelOk':'确定',
			'callback_ok':'JitPage.closeDialog()'
    	});
    },
	getByteLen: function(val) {
		var len = 0;
		for (var i = 0; i < val.length; i++) {
			if (val[i].match(/[^\x00-\xff]/ig) != null) //全角
				len += 2;
			else
				len += 1;
		}
		return len;
	}
}); 