Jit.AM.defindPage({
	name:'BigDial',
	initWithParam: function(param) {
	    
	},
	onPageLoad : function() {
		Jit.log('进入'+this.name+'.....');
		this.ajaxSending = false;
		this.data = null;
		this.eventId = this.getUrlParam("eventId");
        JitPage.setHashParam('EventId',this.eventId);
		//if(!this.eventId){
			//self.alert("eventId为空，未获取到活动信息");
			//return false;
		//}
		//this.loadData();
		this.initEvent();
	},
	loadData:function(){
		var me = this;
    	var hasOAuth = Jit.AM.getAppParam('Launch','CheckOAuth');
    	if(hasOAuth == 'unAttention'){
            var cfg = Jit.AM.getAppVersion();
			this.alert('参与本活动需要先关注微信公众号：'+cfg.APP_NAME);
            return false;
        }
		me.getInitData();
		
	},
    getInitData:function(){
        var me=this;
        var datas = {
            'action': 'Event.EventPrizes.GetPrizeEvent',
            'eventId': me.eventId,
            'RecommandId': Jit.AM.getUrlParam('sender') || ''  //推荐人的id
        };
        //加载中奖信息
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            //async: false,   //设置为同步请求
            data: datas,
            success: function(data) {
                if (data.IsSuccess && data.Data) {
                    var _data = data.Data;
                    var shareInfo = {
                        'title':(data.Data.ShareRemark||'好友推荐'),
                        'desc':(data.Data.OverRemark||'大奖等你抢！'),
                        'link':shareUrl,
                        'imgUrl':(data.Data.ShareLogoUrl||JitCfg.shareIco)
                    }
                    me.initIsShareEvent(shareInfo);
                }else {
                    me.alert(data.Message);
                }
            }
        });

    },
	initEvent: function(){
		var me = this,
			list = $('#content li'),
			len = list.length,
			index = 0,
			interval = null;
			
		$('#begin').on('click',function(){
			var $this = $(this);
			if(this.running)return;
			this.running = true;
			this.remain = 2000 + Math.random() * 3000;//3000与5000
			interval = setInterval(function(){
				if( begin.remain < 100 && (index==6 || index==3)){
					begin.running = false;
					Jit.UI.Dialog({
						"type":"Alert",
						"content":'你抽中了: '+list[index].innerText+'等奖',
						"CallBackOk":function(){
							Jit.UI.Dialog('CLOSE');
							//$this.unbind();
						}
					});
					clearInterval(interval);
				}else{
					list[index].className = "";
					list[(index+1) % len].className = "current";
					index = ++index % len;
					begin.remain -= 100;
				}
			},100);
		})
		
	},
	//抽奖扣除积分
	setVipResult:function(ind){
		var me = this;
		me.ajax({
            url: '/applicationinterface/Gateway.ashx',
			data: {
				'action': 'Extension.PointMark.SetVipPointMark',
				'Source':2, //点标来源（1=答题；2=兑换）
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