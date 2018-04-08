Jit.AM.defindPage({
	name:'AnswerActivity',
	rightCount:0,
	allCount:0,
	initWithParam: function(param) {
	    
	},
	onPageLoad : function() {
		/*
		Jit.log('进入'+this.name+'.....');
		this.ajaxSending = false;
		this.data = null;
		this.eventId = this.getUrlParam("eventId");
        JitPage.setHashParam('EventId',this.eventId);
		if(!this.eventId){
			self.alert("eventId为空，未获取到活动信息");
			return false;
		}
		this.loadData();
		*/
		var me = this;
		me.initEvent();
		me.getVipMark(function(data){
			me.isWeek = data.WeekCount; //本周是否答过题目
			me.totalCount = me.allCount = data.Count;
		});
		if(me.getUrlParam("isMarkPage")==1){
			$('.myMarkerBtn').trigger('click');
		}
		
	},
	loadData:function(){
		var me = this;
    	var hasOAuth = Jit.AM.getAppParam('Launch','CheckOAuth');
    	if(hasOAuth == 'unAttention'){
            var cfg = Jit.AM.getAppVersion();
			this.alert('参与本活动需要先关注微信公众号：'+cfg.APP_NAME);
            return false;
        }
		
		//进入页面判断本周是否答过题目
		me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            //async: false,   //设置为同步请求
            data: datas,
            success: function(data) {
                if (data.IsSuccess) {
                    var result = data.Data;
					if(!result.is){
						//题目一
						$('.answerFirstPage .titlePic').attr('src','');
						$('.answerFirstPage .topicA img').attr('src','');
						$('.answerFirstPage .topicB img').attr('src','');
						//题目二
						$('.answerSecondPage .titlePic').attr('src','');
						$('.answerSecondPage .topicA img').attr('src','');
						$('.answerSecondPage .topicB img').attr('src','');
					}else{
						
					}
					/*
					var _data = data.Data;
                    var shareInfo = {
                        'title':(data.Data.ShareRemark||'好友推荐'),
                        'desc':(data.Data.OverRemark||'大奖等你抢！'),
                        'link':shareUrl,
                        'imgUrl':(data.Data.ShareLogoUrl||JitCfg.shareIco)
                    }
                    me.initIsShareEvent(shareInfo);
					*/
                }else {
                    me.alert(data.Message);
                }
            }
        });
		
	},
	getTopic:function(){
		var me = this;
		me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
			data: {
                'action': 'Extension.Question.GetQuestionList',
				'PageIndex':0,
				'PageSize':10
            },
            success: function(data) {
                if (data.IsSuccess) {
                    var result = data.Data,
						questionData = result.QuestionList;
						
					//题目一
					$('.answerFirstPage .titlePic').attr('src',questionData[0].NameUrl);
					$('.answerFirstPage .topicA img').attr('src',questionData[0].Option1ImageUrl);
					$('.answerFirstPage .topicB img').attr('src',questionData[0].Option2ImageUrl);
					if(questionData[0].Answer==1){
						$('.answerFirstPage .topicA').attr('data-bool',1);
					}else{
						$('.answerFirstPage .topicB').attr('data-bool',1);
					}
					
					//题目二
					$('.answerSecondPage .titlePic').attr('src',questionData[1].NameUrl);
					$('.answerSecondPage .topicA img').attr('src',questionData[1].Option1ImageUrl);
					$('.answerSecondPage .topicB img').attr('src',questionData[1].Option2ImageUrl);
					if(questionData[1].Answer==1){
						$('.answerSecondPage .topicA').attr('data-bool',1);
					}else{
						$('.answerSecondPage .topicB').attr('data-bool',1);
					}
                }else {
                    me.alert(data.Message);
                }
            }
        });
		
	},
	setVipResult:function(ind){
		var me = this;
		me.ajax({
            url: '/applicationinterface/Gateway.ashx',
			data: {
				'action': 'Extension.PointMark.SetVipPointMark',
				'Source':1, //点标来源（1=答题；2=兑换）
				'Count':ind //收入/获支出个数(收入为正数，支出为负数)
			},
            success: function(data) {
                if (data.IsSuccess) {
                    var result = data.Data;
					
                }else {
                    me.alert(data.Message);
                }
            }
        });
	},
	getVipMark:function(callback){
		var me = this;
		me.ajax({
            url: '/applicationinterface/Gateway.ashx',
			data: {
				'action': 'Extension.PointMark.GetVipPointMark'
			},
            success: function(data) {
                if (data.IsSuccess) {
                    var result = data.Data;
					if(callback){
						callback(result);
					}
					
                }else {
                    me.alert(data.Message);
                }
            }
        });
	},
	deleteMark:function(){
		var me = this;
		me.ajax({
            url: '/applicationinterface/Gateway.ashx',
			data: {
				'action': 'Extension.PointMark.DelVipPointMark'
			},
            success: function(data) {
                if (data.IsSuccess) {
                    var result = data.Data;
					$('body>section').hide();
					$('.answerFirstPage').show();
					
					me.getVipMark(function(data){
						me.isWeek = data.WeekCount; //本周是否答过题目
						me.totalCount = data.Count;
					});
                }else {
                    me.alert(data.Message);
                }
            }
        });
	},
	initEvent: function(){
		var me = this;
		//开始答题
		$('#goAnswerBtn').on('click',function(){
			var $this = $(this);
			//获取题目
			if(me.isWeek>0){
				alert('亲，一周只能答题一次哦！');
			}else{
				me.getTopic();
				$('body>section').hide();
				//$this.parents('section').hide();
				$('.answerFirstPage').show();
			}
		});
		
		
		//答题一
		$('.answerFirstPage a').on('click',function(){
			var $this = $(this);
			$('body>section').hide();
			if($this.data('bool')){
				me.rightCount = ++me.rightCount;
			}
			$('.answerSecondPage').show();
		});
		
		//答题二
		$('.answerSecondPage a').on('click',function(){
			var $this = $(this);
			me.allCount = 0;
			$('body>section').hide();
			if($this.data('bool')){
				me.rightCount = ++me.rightCount;
			}
			if(me.rightCount == 2){
				$('.answerSuccessPage').show();
			}else{
				$('.answerFailurePage .correctCount').text(me.rightCount);
				$('.answerFailurePage').show();
			}
			if(me.rightCount == 1 || me.rightCount == 2){
				me.isWeek = 1;
				me.setVipResult(me.rightCount);
			}
			
			
			me.allCount = me.totalCount+me.rightCount;
			if(me.allCount>4){
				$('.answerSuccessPage .numBgMask').html('亲，您可以抽奖了哦~');
			}else{
				var remain = 5-me.allCount;
				$('.answerSuccessPage .numBgMask').html('还差'+remain+'个积点<br>下周还有抽奖哦~');
			}
			
		});
		
		//再来一次
		$('.encoreBtn').on('click',function(){
			var $this = $(this);
			me.rightCount = 0;
			me.deleteMark();
		});
		
		
		function myMark(){
			$('body>section').hide();
			$('.answerIntegralPage').show();
			
			/*
			me.getVipMark(function(data){
				me.totalCount = data.TotalCount;
				$('.progressLine').css({"width":7*me.totalCount+"%"});
				$('.totaleCountArea span').text(me.totalCount);
			});
			*/
			me.totalCount = me.allCount;
			var progressNum = (7*me.totalCount)>100 ? 100 : 7*me.totalCount;
			$('.progressLine').css({"width":progressNum+"%"});
			$('.totaleCountArea span').text(me.totalCount);
			
			if(me.totalCount>=5 && me.totalCount<10){
				$('.progressBox .dot5').addClass('on');
			}else if(me.totalCount>=10 && me.totalCount<15){
				$('.progressBox .dot5').addClass('on');
				$('.progressBox .dot10').addClass('on');
			}else if(me.totalCount>=15){
				$('.progressBox .dot5').addClass('on');
				$('.progressBox .dot10').addClass('on');
				$('.progressBox .dot15').addClass('on');
			}
		}
		
		//点击我的点标
		$('.myMarkerBtn').on('click',function(){
			var $this = $(this);
			myMark();
		});
		
		$('.linkMyMark').on('click',function(){
			myMark();
		})
		
		
		//继续收集积点
		$('.skipAnswerBtn').on('click',function(){
			if(me.isWeek>0){
				alert('亲，一周只能答题一次哦！');
			}else{
				me.getTopic();
				$('body>section').hide();
				$('.answerFirstPage').show();
			}
		});
		
		
		//拼手气  抓大奖咯
		$('.skipDrawBtn').on('click',function(){
			//$('#hintNoAccess').show();
			Jit.AM.toPage('BigDial');
		});
		
		
		$('.goDrawBtn').on('click',function(){
			Jit.AM.toPage('BigDial');
			//$('#hintNoAccess').show();
			/*
			if(me.totalCount<5){
				alert('亲，您的积分还不够哦！');
			}else{
				Jit.AM.toPage('BigDial');
				//alert('一周只能抽一次大奖哦！');
			}
			*/
		});
		
		
		//关闭提示层
		$('#hintNoAccess').delegate('.closeBtn','click',function(){
			$('#hintNoAccess').hide();
		});
		
		
		
		//关闭提示抽奖时间
		/*
		$('body').delegate('.closeBtnTwo','click',function(){
			$('#tipActionDate').remove();
		})
		*/
		
		
		
	},
	initIsShareEvent:function(shareInfo){
		var me = this,
			data = me.data;
        var info = Jit.AM.getBaseAjaxParam();
        if(!data.IsShare){
        	//不是分享活动
            Jit.WX.shareFriends(shareInfo);
            return ;
        }else{
        	//是分享活动
            shareInfo.link = shareInfo.link + '&sender=' + info.userId + '&eventId=' + me.eventId;
            shareInfo.isAuth=true;   //需要高级auth认证
            //高级auth 认证
			Jit.WX.shareFriends(shareInfo);
        }
	}
}); 