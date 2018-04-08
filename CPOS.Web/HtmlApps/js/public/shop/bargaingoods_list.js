Jit.AM.defindPage({

    name: 'bargaingoodslist',
	
    element: {
        EventId: "",           //活动主标识
        ItemId: "",            //商品Id
        PageSize: 5,
        PageIndex: 0
    },
	
	initWithParam: function(param){
		
        if(param['hidePrice'] == 'true'){

        	this.hidePrice = true;
        }
	},
	
    onPageLoad: function () {

        var that = this;
        //当页面加载完成时触发
        Jit.log('进入ListGoods.....');

        that.element.EventId = that.getUrlParam('eventId');
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

        var that = this;
		
        that.GetItemList(function (_data) {
			_data.EventId=that.element.EventId
            $(".goods_list").html(template.render("Tpl_goods_list", _data));
        });
		
		

    },
    GetItemList: function (callback) {//砍价商品列表
        var that = this;
        that.ajax({
            url: "/ApplicationInterface/Gateway.ashx",
            data: {
                'action': 'WEvent.Bargain.GetItemList',
                'EventId': that.element.EventId
            },
            success: function (data) {
                if (data.IsSuccess && data.ResultCode == 0) {
                    var data = data.Data;
                    if (callback) {
                        callback(data);
                    }

                } else {
                    alert(data.Message);
                }
            }
        });

    },
    renderPage: function (data) {

        var me = this,
			itemlists = data.itemList,
			tpl = $('#Tpl_goods_list').html(), html = '';
		
		

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
