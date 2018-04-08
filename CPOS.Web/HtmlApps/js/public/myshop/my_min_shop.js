Jit.AM.defindPage({
	
	name: 'MyMinShop',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		that.initData();
		that.initEvent();
	},
	initData: function(){
		var that = this;
		that.shopList('','您还没有添加商品');
		//添加分类列表
		that.classifyList();
	},
	shopList: function(typeId,hint){
		var that = this;
		that.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getItemList',
				'isstore': 1,
				'itemTypeId': typeId || '',
				'page':1,
				'pageSize':100
            },
            success: function (data) {
                //Jit.UI.Loading(false);
				if(data.code == 200){
				   var  htmlStr = '';
				   		listData = data.content.itemList;
				   if(listData.length==0){
				   		$('.goodsList').html("<div class='commonDataHint'>"+hint+"！<a href=javascript:Jit.AM.toPage('AddGoods')>去添加商品</a></div>");
				   }else{
				   		for(var i=0;i<listData.length;i++){
							htmlStr += bd.template('tpl_myMinShop', listData[i]);  	
					   }
					   $('.goodsList').html(htmlStr);
					   that.sharePage();
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
		}
		else{
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
		

		$('.toolsBox').delegate('.editBox','click',function(){
			var $this = $(this);
			if($goodsList.hasClass('on')){
				$this.text('编辑');
				$goodsList.removeClass('on');
				$('.sortBox').show();
			}else{
				$this.text('取消');
				$goodsList.addClass('on');
				$('.sortBox').hide();
			}
			//$goodsList.animate({left:"50px"},500,"linear");
		})
		
		$goodsList.delegate('.goodsList-item','click',function(){
			var $this = $(this);
			Jit.AM.toPage('GoodsDetail','&goodsId='+$this.data('itemid')+'&channelId=6');
		})
		
		$goodsList.delegate('.delBox','click',function(evt){
			that.stopEventBubble(evt);
			$$this = $(this);
			that.promptHint('删除商品');
		})
		
		$('.toolsBox').delegate('.sortBox','click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				$this.removeClass('on');
			}else{
				$this.addClass('on');
			}
		})
		
		//绑定分类列表
		$('.sortList').delegate('a','click',function(){
			var $this = $(this);
			that.shopList($this.data('categoryid'),'您还没有此分类商品');
		});
		
		//删除商品
		$('body').delegate('.del-btn','click',function(){
			var $this = $(this);
			$this.parents('.ui-mask').remove();
			that.removeShop($$this);
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
				   var strHtml = '<a href="javascript:;">全部</a>',
				   	   result = data.Data.GetItemCategoryList;
				   for(var i=0;i<result.length;i++){
				   	  strHtml = strHtml + '<a href="javascript:;" class="t-overflow" data-categoryid="'+result[i].ItemCategoryId+'">'+result[i].ItemCategoryName+'</a>';
				   }
				   $('.sortList').html(strHtml); 
				   
				}else{
				   alert(data.Message);
				}
            }
        });
	},
	removeShop: function($dom){
		var that = this;
		that.ajax({
			url: '/ApplicationInterface/VipStore/VipStoreGateway.ashx',
			data: {
				'action': 'VipStoreDelItem',
				'ItemID':$dom.parents('.goodsList-item').data('itemid')
			},
			success: function (data) {
				//Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   $dom.parent('.goodsList-item').animate({left:'640px'},'500','linear',function(){$dom.parent('.goodsList-item').remove()});
				   
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
		//
		var that = this,
			baseParam=Jit.AM.getBaseAjaxParam(),
			shareUrl = location.href.replace('myMinShop.html','shareMinShop.html'),
			title=Jit.AM.getAppVersion().APP_WX_TITLE || document.title,
			desc=Jit.AM.getAppVersion().APP_WX_DES || document.title,
			imgUrl=Jit.AM.getAppVersion().APP_WX_ICO || $('.picBox img').eq(0).attr('src');
        //var theLink=location.href+"&RecommendVip="+baseInfo.userId;//用来做分润操作
		var	shareInfo = {
			'title': title,
			'desc':desc,
			'link':shareUrl+'&sendOpenId='+baseParam.openId,
			'imgUrl':imgUrl,
			'isAuth':true//需要高级auth认证
		};
		Jit.AM.initShareEvent(shareInfo);  
	}
});