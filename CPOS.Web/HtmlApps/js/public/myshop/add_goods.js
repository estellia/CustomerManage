Jit.AM.defindPage({
	name: 'AddGoods',
	onPageLoad: function() {
		this.loadPageData();
	},
	loadPageData: function() {
		var that = this;
		Jit.WX.OptionMenu(false);
		that.initData();
		that.initEvent();
	},
	initData: function(){
		var that = this;
		that.shopList();
		//添加分类列表
		that.classifyList();
	},
	shopList: function(typeId){
		var that = this;
		that.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getItemList',
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
				   		return $('.goodsList').html("<div class='commonDataHint'>抱歉，还没有可以添加的商品！</div>");
				   }else{
				   		for(var i=0;i<listData.length;i++){
							htmlStr += bd.template('tpl_addShopList', listData[i]);  	
					   }
					   $('.goodsList').html(htmlStr);
				   }
				}else{
				   alert(data.description);
				}
            }
        });
	},
	addShop: function(itemidStr){
		var that = this,
			$goodsListItem = $('.goodsList-item.checked'),
			goodsNum = $goodsListItem.length;
		that.ajax({
			url: '/ApplicationInterface/VipStore/VipStoreGateway.ashx',
			data: {
				'action': 'VipStoreAddItem',//这里有问题,是否可以一次添加多个商品，添加之后是否有标示已被添加
				'ItemID': itemidStr
			},
			success: function (data) {
				//Jit.UI.Loading(false);
				if(data.ResultCode == 0){
				   that.promptAdd(goodsNum,function(){
					  $goodsListItem.addClass('added').removeClass('checked');
				   });
				}else{
				   alert(data.Message);
				}
			}
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
	initEvent: function() {
		var that = this,
			$goodsList = $('.goodsList');
		$('.toolsBox').delegate('.sortBox','click',function(){
			var $this = $(this);
			if($this.hasClass('on')){
				$this.removeClass('on');
			}else{
				$this.addClass('on');
			}
		})
		
		//点击选中商品
		$goodsList.delegate('.goodsList-item','click',function(){
			var $this = $(this);
			if($this.hasClass('added')){
				that.promptHint('此商品已被添加！');
			}else{
				if($this.hasClass('checked')){
					$this.removeClass('checked');
				}else{
					$this.addClass('checked');
				}
			}
		})
		
		//添加商品
		$('.addShopBtn').on('click',function(){
			var itemidStr = '',
				$checked = $('.goodsList-item.checked'),
				goodsNum = $checked.length;
			if(goodsNum<=0){
				that.promptHint('请选择添加商品！');
			}else{
				for(var i=0;i<goodsNum;i++){
					itemidStr = itemidStr+$checked.eq(i).data('itemid')+',';
				}
				that.addShop(itemidStr);
			}
		})
		
		//绑定分类列表
		$('.sortList').delegate('a','click',function(){
			var $this = $(this);
			that.shopList($this.data('categoryid'));
		});
		
	},
	promptHint: function(str,callback){
		var htmlStr = '<div class="ui-mask" style="display:block;">\
		<div class="ui-dialogs ui-dialogs-prompt">\
			<p class="dialogs-text">'+str+'</p>\
			<div class="clearfix">\
				<a class="dialogs-btn" style="width:100%;display:block;" href="javascript:;">确定</a>\
			</div>\
		</div>\
		</div>';
		$('body').append(htmlStr);
		$('body').delegate('.dialogs-btn','click',function(){
			var $this = $(this);
			$this.parents('.ui-mask').remove();
			if(typeof callback==="function"){
				callback();
			}
		});
		
		
	},
	promptAdd: function(num,callback){
		var htmlStr = '<div class="ui-mask" style="display:block;">\
		<div class="ui-dialogs ui-dialogs-prompt">\
			<p class="dialogs-text">成功添加了<span>'+num+'</span>个宝贝！</p>\
			<div class="clearfix">\
				<a class="dialogs-btn fr" href="javascript:;">进入小店</a>\
				<a class="dialogs-btn fl" href="javascript:;">继续添加</a>\
			</div>\
		</div>\
		</div>';
		$('body').append(htmlStr);
		$('body').delegate('.dialogs-btn','click',function(){
			var $this = $(this);
			$this.parents('.ui-mask').remove();
			if($this.hasClass('fr')){
				location.href = 'myMinShop.html';
				Jit.AM.toPage('MyMinShop');
			}else{
				if(typeof callback==="function"){
					callback();
				}
			}
			
		});
	}
});