Jit.AM.defindPage({
	name:'BigDial',
	initWithParam: function(param) {
	    
	},
	element:{
		bigDialContent : $('#bigDialContent'),
		beginBtn : $('#beginBtn'),
		ruleBtn : $('.ruleBtn'),
		ruleArea : $('.ruleArea')
	},
	onPageLoad : function() {
		this.initPage();
	},
	initPage:function(){
		/*
    	var hasOAuth = Jit.AM.getAppParam('Launch','CheckOAuth');
    	if(hasOAuth == 'unAttention'){
            var cfg = Jit.AM.getAppVersion();
			this.alert('参与本活动需要先关注微信公众号：'+cfg.APP_NAME);
            return false;
        }
		*/
		
		var that = this;
		//that.getBigDial();
		that.initEvent();
		
	},
	initEvent: function(){
		var that = this;
			
		$('#beginBtn').on('click',function(){
			var $this = $(this),
				totalPoint = Jit.AM.getUrlParam('totalPoint');
			if(totalPoint<that.element.point){
				alert('亲，您的积点还不够哦！');
				return;
			}
			
			that.initDraw(function(params){

				//flag 标志（1=中奖；2=未中奖；3=本周已抽奖；4=积不足；5=活动已结束）
				//prizesId 奖品ID
				//prizesName 奖品名称
				//displayIndex 1:世博会特供茶；2:定制旅行套餐；3:世博会纪念品；4:未中奖；
				
				
				var list = $('li',that.element.bigDialContent),
					len = list.length,
					index = 0,
					begin = that.element.beginBtn,
					selIndex = 0,
					hintText = '',
					displayIndex = params.displayIndex,
					prizesId = '',
					interval = null;
					
				begin.running = false;
				if(begin.running)return;
				begin.running = true;
				begin.remain = 2000 + Math.random() * 3000;//3000与5000
				
				if(params.flag==1){
					hintText = params.prizesName;
					//设置中奖的Id
					if(displayIndex === 1){//1:世博会特供茶
						prizesId = 'AEF542FA-70BA-4CDB-B4E9-54CD84B8B32B';
					}else if(displayIndex === 2){//2:定制旅行套餐；
						prizesId = 'A35B7CBB-5508-44C2-8304-D478360DF1E9';
					}else if(displayIndex === 3){//3:世博会纪念品；
						prizesId = 'AF17A95E-E967-414D-98DF-39F8BD9F1520';
					}
				}else if(params.flag==2){
					hintText = '亲，您未中奖！';
				}else{
					if(params.flag==3){
						hintText = '亲，您本周已抽奖！';
					}else if(params.flag==4){
						hintText = '亲，您的积点不足哦！';
					}else if(params.flag==5){
						hintText = '亲，活动已结束！';
					}
					Jit.UI.Dialog({
						"type":"Alert",
						"content":hintText,
						"CallBackOk":function(){
							Jit.UI.Dialog('CLOSE');
						}
					});
					return;
				}
				
				
				interval = setInterval(function(){
					if(displayIndex === 1){//1:世博会特供茶 
						selIndex = (index==11)?true:false; 
					}else if(displayIndex === 2){//2:定制旅行套餐；
						selIndex = (index==1 || index==5)?true:false;
					}else if(displayIndex === 3){//3:世博会纪念品；
						selIndex = (index==3 || index==6 || index==9)?true:false;
					}else if(displayIndex === 4){//4:未中奖；
						selIndex = (index==0 || index==4 || index==8)?true:false;
					}else{
						selIndex = (index==8)?true:false;
					}
					
					if( begin.remain < 100 && selIndex){
						begin.running = false;
						if(displayIndex === 4){
							Jit.UI.Dialog({
								"type":"Alert",
								"content":hintText,
								"CallBackOk":function(){
									Jit.UI.Dialog('CLOSE');
								}
							});
						}else{
							Jit.UI.Dialog({
								"type":"Confirm",
								"content":"恭喜您中"+hintText+"券。<br>确定要用积点兑换吗？",//list[index].innerText
								"LabelOk":"兑换",
								"LabelCancel":"不兑换",
								"CallBackOk":function(){
									that.setExchange(prizesId);
									$('#jit_btn_ok').unbind();
								},
								"CallBackCancel":function(){
									Jit.UI.Dialog('CLOSE');
								}
							});
						}
						
						clearInterval(interval);
					}else{
						list[index].className = "";
						list[(index+1) % len].className = "current";
						index = ++index % len;
						begin.remain -= 100;
					}
				},100);
			})
			
		})
		
		//活动规则
		that.element.ruleBtn.on('click',function(){
			that.element.ruleArea.show();
		});
		
		that.element.ruleArea.on('click',function(){
			that.element.ruleArea.hide();
		})
		
	},
	getBigDial:function(){
		var that = this;
		that.ajax({
            url: '/applicationinterface/Gateway.ashx',
			data: {
				'action': 'GetActivityPrizes'
			},
            success: function(data) {
                if (data.ResultCode == 0) {
                    var result = data.Data,
						prizesList = result.ActivityPrizesInfo,
						htmlStr = '';
					for(var i=0;i<prizesList.length;i++){
						htmlStr += bd.template('tpl_bigDialList', listData[i]);
					}
					that.element.bigDialContent.html(htmlStr);
					that.element.point = result.LowestPointLimit;
					//element.beginBtn.css({'background':'url('+result.img+') no-repeat center center'})
                }else {
                    alert(data.Message);
                }
            }
        });
	},
	//开始抽奖
	initDraw:function(callback){
		var that = this;
		that.ajax({
            url: '/applicationinterface/Gateway.ashx',
			data: {
				'action': 'Extension.LuckDraw.LuckDraw'
			},
            success: function(data) {
                if (data.IsSuccess) {
                    var result = data.Data,
						obj = {
							flag : result.Flag,//标志（1=中奖；2=未中奖；3=本周已抽奖；4=积不足）
							prizesId : result.PrizesID, //奖品ID
							prizesName : result.PrizesName, //奖品名称
							displayIndex : result.DisplayIndex	//奖品显示序号
						};
					if(result.Flag == 6){
						Jit.UI.Dialog({
							"type":"Confirm",
							"content":"亲，只有龙粉才可以抽奖哦！",//list[index].innerText
							"LabelOk":"确定",
							"LabelCancel":"取消",
							"CallBackOk":function(){
								Jit.AM.toPage('ForApply');
							},
							"CallBackCancel":function(){
								Jit.UI.Dialog('CLOSE');
							}
						});
						
					}else{
						if(!!callback){
							callback(obj);
						}
					}
					
						
                }else {
                    alert(data.Message);
                }
            }
        });
	},
	//兑换
	setExchange:function(id){
		var that = this;
		that.ajax({
            url: '/applicationinterface/Gateway.ashx',
			data: {
				'action': 'Extension.LuckDraw.ExchangeCoupon',
				'PrizesID': id
			},
            success: function(data) {
                if(data.IsSuccess) {
                    Jit.UI.Dialog('CLOSE');
					alert('兑换成功，请去个人中心查看兑换劵！');
                }else{
                    alert(data.Message);
                }
            }
        });
	},
	
	initIsShareEvent:function(shareInfo){
		var that = this,
			data = that.data;
        var info = Jit.AM.getBaseAjaxParam();
        if(!data.IsShare){
        	//不是分享活动
            Jit.WX.shareFriends(shareInfo);
            return ;
        }else{
        	//是分享活动
            shareInfo.link = shareInfo.link + '&sender=' + info.userId + '&eventId=' + that.eventId;
            shareInfo.isAuth=true;   //需要高级auth认证
            //高级auth 认证
			Jit.WX.shareFriends(shareInfo);
        }
	}
}); 