Jit.AM.defindPage({
	name: 'ShareMinShop',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this,
			sender = Jit.AM.getUrlParam('sender') || '';
		//Jit.WX.OptionMenu(false);
		if(sender){
			Jit.AM.setPageParam('shareMinShopSender',sender);
		}else{
			alert('没有senderId');
			return false;
		}
		that.initData();
		that.initEvent();
	},
	initData: function(){
		var that = this;
		that.shopList('','您还没有添加商品');
		//添加分类列表
		that.classifyList();
		that.getMemberInfo();
	},
	shopList: function(typeId,hint){
		var that = this;
		that.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getItemList',
				'isstore': 1,
				'itemTypeId': typeId || '',
				'OwnerVipID': Jit.AM.getPageParam('shareMinShopSender')
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.code == 200){
				   var  htmlStr = '';
				   		listData = data.content.itemList;
				   if(listData.length==0){
				   		$('.goodsList').html("<div class='commonDataHint'>"+hint+"！");
				   }else{
				   	   for(var i=0;i<listData.length;i++){
							htmlStr += bd.template('tpl_myMinShop', listData[i]);  	
					   }
					   $('.goodsList').html(htmlStr);
					   $('.storeBaby span').text(listData.length);
					   //that.sharePage();
				   }
				}else{
				   alert(data.description);
				}
            }
        });
	},
	stopEventBubble: function(event){
		var e=event || window.event;
		if (e && e.stopPropagation){
			e.stopPropagation();    
		}else{
			e.cancelBubble=true;
		}
	},
	initEvent: function() {
		var that = this,
			$goodsList = $('.goodsList');
			
		$('.generalizeBox').bind('click',function(){
            $('#share-mask').show();
            $('#share-mask-img').show().attr('class','pullDownState');
        });
        
        $('#share-mask').bind('click',function(){
            var that = $(this);
            $('#share-mask-img').attr('class','pullUpState');
            setTimeout(function(){$('#share-mask-img').css({"display":"none"});that.css({"display":"none"});},500);
        });
		
		$goodsList.delegate('.goodsList-item','click',function(){
			var $this = $(this),
				senderId = Jit.AM.getUrlParam('sender');//shareMinShopSender
			Jit.AM.toPage('GoodsDetail','&goodsId='+$this.data('itemid')+'&sender='+senderId+'&channelId=6');//+'&channelId=6'
		});
		
		$('.myMinShopHome-nav').delegate('a','click',function(){
			var $this = $(this);
			if(!$this.hasClass('action')){
				$('.myMinShopHome-nav a').removeClass('action');
				$this.addClass('action');
			}
			if($this.hasClass('nav-classify')){
				$('.sortList').show();
			}else{
				$('.sortList').hide();
			}
		})
		
		//绑定分类列表
		$('.sortList').delegate('em','click',function(){
			that.stopEventBubble();
			$('.sortList').hide();
			var $this = $(this);
			that.shopList($this.data('categoryid'),'您还没有此分类商品');
		});
		
		
		$('body').delegate('.cancel-btn','click',function(){
			var $this = $(this);
			$this.parents('.ui-mask').remove();
		});
		
		
	},
	classifyList: function(){
		var that = this;
		that.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'GetItemCategoryList'
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   var $sortList = $('.sortList'),
				   	   strHtml = '<em>全部</em>',
				   	   result = data.Data.GetItemCategoryList;
				   for(var i=0;i<result.length;i++){
				   	  strHtml = strHtml + '<em class="t-overflow" data-categoryid="'+result[i].ItemCategoryId+'">'+result[i].ItemCategoryName+'</em>';
				   }
				   $sortList.html(strHtml).css('top','-'+($sortList.height()+20)+'px');
				}else{
				   alert(data.Message);
				}
            }
        });
	},
	promptHint: function(str){
		var that = this;
			htmlStr = '<div class="ui-mask" style="display:block;">\
			<div class="ui-dialogs ui-dialogs-prompt">\
				<p class="dialogs-text">'+str+'</p>\
				<div class="clearfix">\
					<a class="dialogs-btn cancel-btn" href="javascript:;">取消</a>\
					<a class="dialogs-btn del-btn" href="javascript:;">确定</a>\
				</div>\
			</div>\
			</div>';
		$('body').append(htmlStr);
	},
	//分享设置
	sharePage:function(){
		var that = this,
			baseParam=Jit.AM.getBaseAjaxParam(),
			title=Jit.AM.getAppVersion().APP_WX_TITLE || document.title,
			desc=Jit.AM.getAppVersion().APP_WX_DES || document.title,
			imgUrl=Jit.AM.getAppVersion().APP_WX_ICO || $('.picBox img').eq(0).attr('src');
        //var theLink=location.href+"&RecommendVip="+baseInfo.userId;//用来做分润操作
		var	shareInfo = {
			'title': title,
			'desc':desc,
			'link':location.href+'&sendOpenId='+baseParam.openId,
			'imgUrl':imgUrl,
			'isAuth':true//需要高级auth认证
		};
		Jit.AM.initShareEvent(shareInfo);  
	},
	getMemberInfo:function(){
        var that = this;
        //Jit.UI.Loading(true);
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'VIP.Login.GetMemberInfo',
                'VipSource':3,
				'OwnerVipID':Jit.AM.getPageParam('shareMinShopSender')
            },
            success: function (data) {
                if(data.ResultCode == 0){//data.Data.MemberInfo.Status==2
					var result = data.Data.MemberInfo,
						imageUrl = result.ImageUrl,
						vipName = result.VipName;
					$('.homeHeader-info img').attr('src',imageUrl);
					$('.storeName span').text(vipName);
                }
            }
        });
    }
});