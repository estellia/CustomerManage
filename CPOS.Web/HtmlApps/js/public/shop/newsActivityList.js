Jit.AM.defindPage({
    name: 'NewsActivityList',
    eventId: '',
    ele: {
        activityNav: $(".activityNav"),
        activityListBox: $(".activityListBox")
    },
    initWithParam: function(param) {
		
    },
    onPageLoad: function() {
        this.isFirst = true;
        this.eventTypeId = Jit.AM.getUrlParam("eventTypeId");
        this.ctwEventId = Jit.AM.getUrlParam("CTWEventId");
        if(!this.eventTypeId){
            alert("地址栏获不到eventTypeId");
            return false;
        }

        this.setShareAH();

        this.loadData();
        this.getKvPic();
        this.initEvent();
    },
    loadData:function(){
        var self = this,
            paramList = {
                'action':'GetPanicbuyingEvent',
                'eventTypeId': this.eventTypeId,//活动类型1=团购2=抢购3=热销
                'page': 1,
                'pageSize': 99,
                'IsCTW':1,  //Int 否   是否是创意活动（1,：是，空或者0：否）
                'CTWEventId':self.ctwEventId  //String  否   创意活动标识
            };
        self.ajax({
            url: '/ApplicationInterface/Event/EventGateway.ashx',
            data: paramList,
            beforeSend:function(){
                Jit.UI.AjaxTips.Loading(true);
            },
            success: function(data) {
                if (data.ResultCode == 0) {
                    var result = data.Data.PanicbuyingEventList || [],
                        paramsList = result.length;
                    self.shareInfo = data.Data.ShareInfo; 
                    if(paramsList){
                        var html = template.render("activityTimeList",{list:result});
                        self.ele.activityNav.html(html);
                        if(self.isFirst){
                            $('a',self.ele.activityNav).eq(0).trigger('click');
                            self.isFirst = false;
                        }
                    }else{
                        self.ele.activityList.html('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">活动已经结束，敬请期待下期！</div>');
                    }
                }else{
                    self.alert(data.Message);
                }

                var sharetitle = '',    //分享标题
                    sharecontent = '',  //分享内容
                    shareimgUrl = '',   //分享图片
                    shareUrl = location.href;

                var shareInfo = null;
                if (self.shareInfo) {
                    shareUrl = shareUrl.replace('?BeSharedOpenId=' + Jit.AM.getUrlParam('BeSharedOpenId'), '');
                    shareUrl = shareUrl.replace('&BeSharedOpenId=' + Jit.AM.getUrlParam('BeSharedOpenId'), '');
                    shareUrl = shareUrl.replace('CTWEventId=' + Jit.AM.getUrlParam('CTWEventId'), '');
                    shareUrl += '&CTWEventId=' + self.ctwEventId + '&BeSharedOpenId=' + Jit.AM.getBaseAjaxParam().openId + '&H5URL=' + self.shareInfo.OnLineRedirectUrl.replace("http://", "").replace(/\//g, "_Slash_").replace(/\?/g, "_questionmark_").replace(/=/g, "_equal_");
                    shareUrl += '&oldurl=' + "/common/lotteryAll/red_loading".replace(/\//g, "_Slash_") + '&newurl=' + "/public/shop/news_activity_list".replace(/\//g, "_Slash_");
                    shareUrl = shareUrl.replace("/public/shop/news_activity_list", "/common/lotteryAll/red_loading");
                    

                    shareInfo={
                        'title': self.shareInfo && self.shareInfo.Title || document.title,
                        'desc': self.shareInfo && self.shareInfo.Summary || '我发现一个不错的商品，赶紧来看看吧!',
                        'link': shareUrl?shareUrl:location.href,
                        'imgUrl': self.shareInfo && self.shareInfo.ImageUrl || $('img').eq(0).attr('src'),
                        'isAuth': true//需要高级auth认证
                    };
                }
                 
                self.sharePage(shareInfo);
            },
            complete:function(){
                Jit.UI.AjaxTips.Loading(false);
            }
        });
    },

    initEvent: function() {
        var self = this,
            timer = null;
        self.ele.activityNav.delegate("a","click",function(){
            var $this = $(this);
            self.eventId = $this.data('id');
            if($this.hasClass('on')){
                return false;
            }
            $('.activityNav a').removeClass('on');
            $this.addClass('on');
            if(!!timer){
                clearInterval(timer);
            }
            self.shopItemList(self.eventId,function(data){
                var second = data.Data.PanicbuyingEvent.DeadlineSecond; 
                $('.timeoutBox').attr('time-date',second);
                timer = setInterval(function(){
                    self.timeDown();
                },1000);
            });
        });

        self.ele.activityListBox.delegate(".itemBox","click",function(){
            var $this = $(this),
                itemId = $this.data('id');
                //eventId = Jit.AM.getUrlParam('eventId');
            Jit.AM.toPage('GroupGoodsDetail','goodsId='+itemId+'&eventId='+self.eventId);
        });
        
    },
    getDay: function(_unixTime) { //获取剩余天数
        var unixTime = parseInt(_unixTime) * 1000; //转换成JS时间
        var endTime = new Date(unixTime),
            seconds = 1000,
            min = 60 * seconds,
            hour = 60 * min,
            days = 24 * hour,
            now = new Date().getTime(),
            dateDiff = endTime - now,
            tipe;
        if (dateDiff <= 0)
            tipe = 1;
        else {
            tipe = parseInt(dateDiff / days);
        }
        return tipe || 1;
    },

    alert:function(text,callback){
        Jit.UI.Dialog({
            type : "Alert",
            content : text,
            CallBackOk : function() {
                Jit.UI.Dialog("CLOSE");
                if(callback){
                    callback();
                }
            }
        });
    },
  
    setShareAH: function () {
        var that = this,
			params = {
			    'action': 'Event.Lottery.Share',
			    'EventId': that.eventId,
			    'ShareUserId': Jit.AM.getUrlParam('sender') || '',
			    'TypeId': 'Share',
			};
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            //'async': false,
            data: params,
            success: function (data) {
                if (data.IsSuccess) {
                } else {
                    that.alert(data.Message);
                }
            }
        });
    },
    sharePage: function (shareInfo) {//分享设置
        var self = this;
        if (shareInfo==null) {
            shareInfo = {
                'title': self.shareInfo && self.shareInfo.Title || document.title,
                'desc': self.shareInfo && self.shareInfo.Summary || '我发现一个不错的商品，赶紧来看看吧!',
                'link': location.href,
                'imgUrl': self.shareInfo && self.shareInfo.ImageUrl || $('img').eq(0).attr('src'),
                'isAuth': true//需要高级auth认证
            };
        }
		Jit.AM.initShareEvent(shareInfo);
	},
    timeDown: function() {
        var self = this;
        var domlist = $('.timeoutBox'),
            endtime,
            second,
            _h,
            _m,
            _s;
        domlist.each(function (idx, dom) {
            endtime = $(dom).attr('time-date');
            second = parseInt(endtime);
            
            _h = Math.floor(second / 3600);
            _m = Math.floor((second % 3600) / 60);
            _s = Math.floor(((second % 3600) % 60));
            //超过99小时全部显示99
            _m = _h > 99 ? 99 : _m;
            _s = _h > 99 ? 99 : _s;
            _h = _h > 99 ? 99 : _h;
            if(second==0){
                //self.loadData();
                $(dom).attr('time-date', 0);
                $('.countdownTit p').text('抢购已结束');
            }else{
                $(dom).attr('time-date', endtime - 1);
            }
            //console.log(_h+' '+_m+' '+_s);
            $(dom).find('.time-h-max').html(Math.floor(_h / 10) > 9 ? 9 : Math.floor(_h / 10));
            $(dom).find('.time-h-min').html(Math.floor(_h % 10));
            $(dom).find('.time-m-max').html(Math.floor(_m / 10));
            $(dom).find('.time-m-min').html(Math.floor(_m % 10));
            $(dom).find('.time-s-max').html(Math.floor(_s / 10));
            $(dom).find('.time-s-min').html(Math.floor(_s % 10));
        });
    },
    shopItemList:function(id,callback){
        var self = this;
        self.ajax({
            url: '/ApplicationInterface/Event/EventGateway.ashx',
            data: {
                'action':'GetEventMerchandise',
                'EventId': id,//活动id
                'page': 1,
                'pageSize': 99
            },
            beforeSend:function(){
                Jit.UI.AjaxTips.Loading(true);
            },
            success: function(data) {
                if (data.ResultCode == 0) {
                    var result = data.Data.ItemList || [],
                        timeFlag = data.Data.TimeFlag,
                        flagText = '',
                        leng = result.length;
                    if(timeFlag=='end'){
                        flagText = '距离抢购结束还剩';
                    }else if (timeFlag=='begin') {
                        flagText = '距离抢购开始还剩';
                    }
                    $('.countdownTit p').text(flagText);    
                    if(leng){
                        var html = template.render("activityGoodsList",{list:result});
                        self.ele.activityListBox.html(html);

                        $("img.lazy").lazyload({
							placeholder : "../../../images/common/opacityImg.png",
							effect : "fadeIn"//effect
							//threshold : 200,
							//event : "click",
							//failurelimit : 100
						});
                    }else{
                        self.ele.activityListBox.html('<div style="width:100%;height:100px;padding-top: 50px;font-size:20px;text-align: center;">暂无商品，敬请期待下期！</div>');//活动已经结束，敬请期待下期！
                    }
                    if(callback){
                        callback(data);
                    }
                }else{
                    self.alert(data.Message);
                }
            },
            complete:function(){
                Jit.UI.AjaxTips.Loading(false);
            }
        });
    },
    getKvPic:function(){
        var self = this;
        self.ajax({
            url: '/ApplicationInterface/Event/EventGateway.ashx',
            data: {
                'action':'GetTCTWPanicbuyingEventKV',
                'CTWEventId': self.ctwEventId,
            },
            beforeSend:function(){
                Jit.UI.AjaxTips.Loading(true);
            },
            success: function(data) {
                if (data.ResultCode == 0) {
                    var result = data.Data.TCTWPanicbuyingEventKVInfo;
                    $('.kvPicBox img').attr('src',result.ImageURL);
                }else{
                    self.alert(data.Message);
                    Jit.UI.AjaxTips.Loading(false);
                }

               
            },
            complete:function(){
                Jit.UI.AjaxTips.Loading(false);
            }
        });
    }



});