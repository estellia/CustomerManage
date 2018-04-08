Jit.AM.defindPage({

    name: 'ListGoods',
	
	page:{
		index:1,
		size:10
	},
	
	initWithParam: function(param){
		
		if(param['bannerDisplay'] == true){
			//$('#goodsTitleUrl').show();

            //$('#goodsTitleUrl').attr('href',param.toUrl);

            //$('#goodsTitleUrl img').attr('src',param.imgUrl);

		}else{

            $('#goodsTitleUrl').remove();
        }

        if(param['hidePrice'] == 'true'){

        	this.hidePrice = true;
        }
	},
	
    onPageLoad: function () {

        //当页面加载完成时触发
        Jit.log('进入ListGoods.....');
        
		this.isSending = false;
		this.noMore = false;
		this.goodsType = JitPage.getUrlParam('goodsType');
        this.goodsItemName = JitPage.getUrlParam("itemName")?decodeURIComponent(JitPage.getUrlParam("itemName")):undefined;
		
		this.sortName = 'salesQty';
		this.sort = 'desc';
		
        this.loadData();
		this.initEvent();
    },
	initEvent:function(){
		var self = this;
		window.onscroll = function(){
			if(getScrollTop() + getWindowHeight() == getScrollHeight()){//console.log("you are in the bottom!");
				if(!self.noMore&&!self.isSending) {
                    self.page.index++;
                    self.loadData();
                }
            }
		};
		
		
		function pageScroll(){
			//把内容滚动指定的像素数（第一个参数是向右滚动的像素数，第二个参数是向下滚动的像素数）
			window.scrollBy(0,-100);
			//延时递归调用，模拟滚动向上效果
			scrolldelay = setTimeout(pageScroll,100);
			//获取scrollTop值，声明了DTD的标准网页取document.documentElement.scrollTop，否则取document.body.scrollTop；因为二者只有一个会生效，另一个就恒为0，所以取和值可以得到网页的真正的scrollTop值
			var sTop=document.documentElement.scrollTop+document.body.scrollTop;
			//判断当页面到达顶部，取消延时代码（否则页面滚动到顶部会无法再向下正常浏览页面）
			if(sTop<10) clearTimeout(scrolldelay);
		}
		
		$('.commonFunInto .goTop').bind('click',function(){
			pageScroll();
		})
		
		
		//itemName
		$('.searchBtn').on('click',function(){
			var searchVal =  $.trim($('.searchInput input').val());
			if(searchVal==''){
				Jit.UI.Dialog({
					'content': '亲，请填写搜索产品！',
					'type': 'Alert',
					'CallBackOk': function(){
						Jit.UI.Dialog('CLOSE');
					}
				});
				return false;
			}
			//self.goodsItemName = searchVal;
			
			self.goodsType = JitPage.getUrlParam('goodsType');
        	self.goodsItemName = searchVal || (JitPage.getUrlParam("itemName")?decodeURIComponent(JitPage.getUrlParam("itemName")):undefined);
			
			self.isSending = false;
			self.loadData();
		})
		
		$(document).on('keyup',function(){
			if(window.event.keyCode == 13){
				$('.searchBtn').trigger('click');
			}
		});
		
		$('.setTypeBtn').on('click',function(){
			var $this = $(this);
			if($this.hasClass('minStyles')){
				Jit.AM.toPage('GoodsListBig','goodsType='+self.goodsType+'&itemName='+self.goodsItemName);
			}else{
				Jit.AM.toPage('GoodsList','goodsType='+self.goodsType+'&itemName='+self.goodsItemName);
			}
			
		});
		
		
		$('.goodsSortArea a').on('click',function(){
			var $this = $(this),
				$span = $('span',$this),
				hasOn = $this.hasClass('on'),
				sortName = $this.data('tag'),
				hasPrice = $this.children('span').length;
			self.sortName = sortName;
			self.page.index = 1;
			if(hasOn){
				if(hasPrice){
					var tag = $span.attr('class');
					if(tag == 'desc'){
						$span.attr('class','asc');
						self.sort = 'asc';
					}else{
						$span.attr('class','desc');
						self.sort = 'desc';
					}
				}else{
					return false;
				}
			}else{
				$('.goodsSortArea a').removeClass('on');
				$this.addClass('on');
				self.sort = 'desc';
				if(hasPrice){
					$span.attr('class','desc');
				}
			}
			self.loadData();
		});
		

	},
    loadData: function () {

        var self = this;
		
		
		
		var _param = {
                'action': 'getItemList',
                'isExchange': 0,
				'sortName':self.sortName,//salesQty(销量)，create_time(上新)，salesPrice(价格)
				'sort':self.sort,//desc  asc
				'page':this.page.index,
				'pageSize':this.page.size
            };
			
		
		if(self.goodsType && self.goodsType!='undefined'){
			_param.itemTypeId = self.goodsType;
		}
        if(self.goodsItemName && self.goodsItemName!='undefined'){
            _param.itemName = self.goodsItemName;
        }
		
		if(!this.isSending){
			this.ajax({
	        	type:"GET",
	            url: '/OnlineShopping/data/Data.aspx',
	            data: _param,
	            beforeSend:function(){
            		Jit.UI.Loading(true);
	            	self.isSending = true;
	            },
	            success: function (data) {
	                self.renderPage(data.content);
	                if(data.content.itemList.length!=self.page.size){
	                	self.noMore = true;
	                }
	            },
	            complete:function(){
            		Jit.UI.Loading(false);
	            	self.isSending =false;
	            	$("img.lazy").lazyload({
						placeholder : "../../../images/common/opacityImg.png",
						effect : "fadeIn"//effect
						//threshold : 200,
						//event : "click",
						//failurelimit : 100
					});
	            }
	        });
		}
        

    },
    renderPage: function (data) {

        var me = this,
			itemlists = data.itemList,
			tpl = $('#Tpl_goods_list').html(), html = '';
		
		/*
		if(me.data.skuList[i].salesPrice>=me.data.skuList[i].price){
			$('.goods_txt_box .discount').remove();
			$('.oldPrice').remove();
		}else{
			$('[jitval=oldPrice]').html(me.data.skuList[i].price);
			var discount = (me.data.skuList[i].salesPrice/me.data.skuList[i].price)*10;
			$('.goods_txt_box .discount').html(discount.toString().substr(0,3)+'折');
		}
		*/
		

        for (var i = 0; i < itemlists.length; i++) {
            var itemdata = itemlists[i];
			if(itemdata.salesPrice>=itemdata.price){
				itemdata.oldprice = '';
				itemdata.discount = '';
				itemdata.hidePrice = 'hide';
			}else{
				itemdata.discount = (itemdata.salesPrice/itemdata.price*10).toString().substr(0,3)+'折';
				itemdata.oldprice = "¥" + itemdata.price;
			}
			
			
			/*if(this.hidePrice){
				itemdata.hidePrice = 'hide';
			}*/
			itemdata.imageUrl = Jit.UI.Image.getSize(itemdata.imageUrl,'240');
			itemdata.saled=Math.floor(Math.random() * (1000- 0) + 0);
            html += Mustache.render(tpl, itemdata);
        }
        //console.log("index:"+this.page.index)
		if(this.page.index==1){
			$('[tplpanel=goods_list]').html(html);
            if(itemlists.length==0){
                $('[tplpanel=goods_list]').html('<div style="width:320px;height:100px;text-align: center;position: absolute;left:50%;top:50%;margin:-50px 0 0 -160px;;">没有查找到您搜索的商品!</div>');
            }
			me.sharePage();
		}else{
			$('[tplpanel=goods_list]').append(html).siblings().remove();;
		}
        
    },
	sharePage:function(){//分享设置
		var	shareInfo = {
			'title':document.title,
			'desc':'我发现一个不错的商品，赶紧来看看吧!',
			'link':location.href,
			'imgUrl':$('img').eq(0).attr('src'),
			'isAuth':true//需要高级auth认证
		};
		Jit.AM.initShareEvent(shareInfo);  
	}
});

//滚动条在Y轴上的滚动距离
function getScrollTop(){
　　var scrollTop = 0, bodyScrollTop = 0, documentScrollTop = 0;
　　if(document.body){
　　　　bodyScrollTop = document.body.scrollTop;
　　}
　　if(document.documentElement){
　　　　documentScrollTop = document.documentElement.scrollTop;
　　}
　　scrollTop = (bodyScrollTop - documentScrollTop > 0) ? bodyScrollTop : documentScrollTop;
　　return scrollTop;
}

//文档的总高度
function getScrollHeight(){
　　var scrollHeight = 0, bodyScrollHeight = 0, documentScrollHeight = 0;
　　if(document.body){
　　　　bodyScrollHeight = document.body.scrollHeight;
　　}
　　if(document.documentElement){
　　　　documentScrollHeight = document.documentElement.scrollHeight;
　　}
　　scrollHeight = (bodyScrollHeight - documentScrollHeight > 0) ? bodyScrollHeight : documentScrollHeight;
　　return scrollHeight;
}

//浏览器视口的高度
function getWindowHeight(){
　　var windowHeight = 0;
　　if(document.compatMode == "CSS1Compat"){
　　　　windowHeight = document.documentElement.clientHeight;
　　}else{
　　　　windowHeight = document.body.clientHeight;
　　}
　　return windowHeight;
}
