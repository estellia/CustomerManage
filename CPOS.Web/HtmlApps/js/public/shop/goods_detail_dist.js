Jit.AM.defindPage({
    name: 'GoodsDetailDist',
	hideMask:function(){
		$('#masklayer').hide();
	},
	initWithParam: function(param){
        if(param['imageBgColor']){
            $('.goods_img').css({
                'background-color':param['imageBgColor']
            })
        }
	},
    onPageLoad: function() {
        var me = this,
			mode = Jit.AM.getUrlParam('mode'),
            //商品预览标示
            isPreview  = Jit.AM.getUrlParam('isPreview') || '';
		me.pageSize = 5;
		me.pageIndex = 0;
		me.retailTraderId = '';
		me.ownerVipId = '';
		me.paramStr = '';
        //6:代表我的小店;7:代表一起发码
		if(Jit.AM.getUrlParam('channelId') == 6){
			me.minGoodsDetail();
		}else if(Jit.AM.getUrlParam('channelId') == 7){
			me.retailTraderId = Jit.AM.getUrlParam('RetailTraderId') || '';
			me.paramStr = '&RetailTraderId='+me.retailTraderId+'&channelId=7';
			$('body').prepend('<input id="channelIdSize" type="hidden" value="7">');
		}else if(Jit.AM.getUrlParam('channelId') == 11){
            me.paramStr = '&channelId=11';
            $('body').prepend('<input id="channelIdSize" type="hidden" value="11">');
        }
		me.goodsId = me.getUrlParam('goodsId')||me.getParams('goodsId');
        if(mode=='noAction'){
			$('.detailFooterBox').hide();
			$('.customerServiceBox').hide();
			$('.goodsDetailArea').css({'paddingBottom':'0px'});
		}
		me.setParams('goodsId',me.goodsId);
		//从APP转发到微信端 跳转过来的id
        var salesUserId=me.getUrlParam("salesUserId"),      //人人销售的店员ID
            channelId=me.getUrlParam("channelId"),          //人人销售的渠道ID
            recommendVip=me.getUrlParam("RecommendVip");   //会员推荐
        var baseInfo=Jit.AM.getBaseAjaxParam();
        //存储起来以便别的页面使用
        if(salesUserId&&channelId){
            var appVersion=Jit.AM.getAppVersion();
            appVersion.AJAX_PARAMS="openId,customerId,userId,locale,ChannelID";
            Jit.AM.setAppVersion(appVersion);
            Jit.AM.setPageParam("_salesUserId_",salesUserId);
            Jit.AM.setPageParam("_channelId_",channelId);
            //公用参数
            baseInfo.ChannelID=channelId;
            Jit.AM.setBaseAjaxParam(baseInfo);
        }else{//没有传递过来则把数据清空掉
            Jit.AM.setPageParam("_salesUserId_",null);
            Jit.AM.setPageParam("_channelId_",null);
        }
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'SuperRetailTrader.Item.GetSuperRetailTraderItemDetail',
                'ItemId': me.goodsId
                //'RetailTraderId':me.retailTraderId
            },
            success: function(data) {
                if(data.ResultCode == 0){
                    me.loadGoodsDetail(data.Data);
                }else{
                    Jit.UI.Dialog({
                        'content': data.Message,
                        'type': 'Alert',
                        'CallBackOk': function() {
                            Jit.UI.Dialog('CLOSE');
                        }
                    });
                }
            },
            complete:function(){
                me.hideMask();
            }
        });

        $('#btn_delNum').bind('click', function() {
            var num = $('#goods_number').val();
            num--;
            if (num <= 1) {
                num = 1;
            }
            $('#goods_number').val(num);
            var pt = parseInt(me.data.Forpoints) * num;
            pt = (isNaN(pt) ? '0' : pt);
            //$('#forpoint').html((isNaN(pt) ? '0' : pt));
			me.setForPonit(pt);
        });
        $('#btn_addNum').bind('click', function() {
            var stockNum = $('.inventoryCount span').text()-0,
				num = $('#goods_number').val();
            num++;
			if(stockNum<num){
				//待开发
				Jit.UI.Dialog({
					'content': '亲，购买数量不能大于库存数量！',
					'type': 'Alert',
					'CallBackOk': function(){
						Jit.UI.Dialog('CLOSE');
						$('#goods_number').val(stockNum);
					}
				});
				return false;
			}
            $('#goods_number').val(num);
			var pt = parseInt(me.data.Forpoints) * num;
			pt = (isNaN(pt) ? '0' : pt);
            //$('#forpoint').html((isNaN(pt) ? '0' : pt));
			me.setForPonit(pt);
        });
		me.evaluateList();
        //购物车数量
		me.getShoppingCart();
        //预览功能
        if(isPreview==1){
            me.setPreview();
        }
    },
    setPreview: function(){
        var that = this,
            $goodsDetailArea = $('.goodsDetailArea'),
            $detailFooterBox = $('.detailFooterBox'),
            $btn_cart = $('.btn_cart'),
            $btn_buy = $('.btn_buy'),
            $g_myShopCart = $('.g_myShopCart'),
            $btn_keep = $('#btn_keep'),
            $comBtn = $('.btn_cart,.btn_buy,.g_myShopCart,#btn_keep,.customerServiceBox');
        $goodsDetailArea.css({'width':'320px','margin':'0 auto'});
        $detailFooterBox.css({'width':'320px','left':'50%','marginLeft':'-160px'});
        $comBtn.attr('href','javascript:JitPage.alert("预览，暂不支持操作！")');
        $('.qr-goods-box').show();
        that.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'Item.QRCode.GetQRCode',
                "ItemId":Jit.AM.getUrlParam('goodsId') || '',
                "ItemName":that.itemNameText
            },
            success: function (data){
                if (data.ResultCode == 0){
                    $('.qr-box img').attr('src',data.Data.QRCodeURL);
                   
                }else{
                    alert(data.Message);
                }
            }
        })
    },
	setForPonit: function(val){
		if(val && val > 0){
		}else{
			$('#lab_forpoint').hide();
		}
	},
    initEvent: function() {
		var that = this;
        $('[name=prop_option]').delegate('a','click',function(evt) {
            var $this = $(this);
			if($this.hasClass('unselected') || $this.hasClass('selected')){
				return false;
			}else{
                $this.siblings().removeClass('selected');
                var idx = $this.addClass('selected').attr('idxVal'),
                    ary = that.skuListObj,
                    obj = {};
                
                if(that.level==1){
                    obj = ary[idx];
                }else if(that.level==2){
                    //两级且点击一级的时候
                    if(idx!==undefined){
                        obj = ary[idx];
                        JitPage.getSkuData(idx);
                    }else{
                        var index = $this.addClass('selected').attr('idx');
                        obj = ary[index];
                    }
                }
                $('[jitval=itemPrice] span').html('￥' + obj.DistributerPrice);//销售价
                $('.inventoryCount span').text(obj.DistributerStock);//库存数
                $('.salesCount span').text(obj.SoldQty);//销售量
                if(obj.DistributerPrice>=that.scalePrice){
                    $('.goods_txt_box .discount').hide();
                    $('.oldPrice').hide();
                }else{
                    $('.goods_txt_box .discount').show();
                    $('.oldPrice').show();
                    $('[jitval=oldPrice]').html(that.scalePrice);
                    var discount = Math.mul(Math.div(obj.DistributerPrice,that.scalePrice),10);
                    $('.goods_txt_box .discount').html(discount.toFixed(1)+'折');
                }
            }
        });
        $('.gde_title>a').bind('click', function(e) {
            var self = $(this),
                parentDiv = self.parent(),
                parentBody = parentDiv.next('.gde_body');
            if (self.hasClass('on')) {
                //显示
                parentBody.show();
                self.removeClass('on');
                // parentBody.animate({'height':0},400);
            } else {
                //隐藏
                parentBody.hide();
                self.addClass('on');
                // parentBody.animate({'height':curHeight},400);
            }
        });
   		$('.g_share').bind('click',function(){
			var $this = $(this);
			if($this.hasClass('generalizeBtn')){
				$('#share-mask-img').attr('src','../../../images/public/my_shop/wxShare-tip.png').css({'display':'none','width':'287px','height':'110px'});
			}
            $('#share-mask').show();
            $('#share-mask-img').show().attr('class','pullDownState');
        });
        $('#share-mask').bind('click',function(){
            var that = $(this);
            $('#share-mask-img').attr('class','pullUpState');
            setTimeout(function(){$('#share-mask-img').css({"display":"none"});that.css({"display":"none"});},500);
        });
		$('.gde_loadbottom').on('click',function(){
			if(!that.totalCount){
				$('.gde_loadbottom').unbind();
				return false;
			}
			if(that.pageIndex<that.totalPageCount){
				that.seeMoreEvaluate();
			}else{
				$('.gde_loadbottom').unbind();
			}
		})
    },
	//评价列表
	evaluateList: function(){
		var that = this;
		that.ajax({
			url: '/ApplicationInterface/Gateway.ashx',
			data: {
				'action': 'VIP.Evaluation.GetEvaluationList',
				'ObjectID': Jit.AM.getUrlParam('goodsId') || '',//商品，门店id 
				"PageSize":that.pageSize,
				"PageIndex":that.pageIndex
			},
			success: function (data){
				if (data.ResultCode == 0){
					var htmlStr = '',
						evaluateData = data.Data.EvaluationList;
					for(var i=0;i<evaluateData.length;i++){
						htmlStr += bd.template('tpl_evaluateList', evaluateData[i]);
					}
					$('.goodsReviewList .gder_list').append(htmlStr);
					if(!that.pageIndex){
						that.pageSize = 10;
						that.totalPageCount = data.Data.TotalPageCount;
						that.totalCount = data.Data.Count;
						$('.commentNum span').html(data.Data.Count);
						$('.goodPraise span').html(data.Data.GoodPer);
					}
					that.pageIndex = ++that.pageIndex;
					if(!that.totalCount){
						$('.gde_loadbottom').text('暂时，还没有评论信息！');
					}else{
						if(that.pageIndex>=that.totalPageCount){
							$('.gde_loadbottom').text('评论数据加载完毕！');
						}
					}
				}else{
					alert(data.Message);
				}
			}
		})
	},
	seeMoreEvaluate: function(){
		var that = this;
		that.evaluateList();
	},
    loadGoodsDetail: function(data) {
        this.data = data;
        this.isKeep = data.isKeep;
        this.isPraise = data.isPraise;
        this.level = 0;
        this.initPageInfo();
        this.initPropHtml();//规格
        this.initEvent();
    },
    addToGoodsCart: function() {
        var me = this;
		if(!me.checkOAuth()){
			return;
		}
        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'setShoppingCart',
                'skuId': me.skuId,
                'qty': $('#goods_number').val()
            },
            success: function(data) {
                if (data.code == '200') {
                    TopMenuHandle.ReCartCount();
                    Jit.UI.Dialog({
                        'content': '添加成功!',
                        'type': 'Confirm',
                        'LabelOk': '再逛逛',
                        'LabelCancel': '去购物车结算',
                        'CallBackOk': function() {
                            me.toPage('GoodsList');
                        },
                        'CallBackCancel': function() {
                            me.toPage('GoodsCart');
                        }
                    });
                } else {
                    Jit.UI.Dialog({
                        'content': data.description,
                        'type': 'Alert',
                        'CallBackOk': function() {
                            Jit.UI.Dialog('CLOSE');
                        }
                    });
                }
            }
        });
    },
    initPageInfo: function() {
        var me = this,
			data = this.data;
        me.itemNameText = data.ItemName;    
        $('[jitval=itemName]').html(data.ItemName);
        if (data.ImageList && data.ImageList.length > 0) {
            var imgHtml = '',
                barHtml = '';
            for (var i = 0; i < data.ImageList.length; i++) {
                imgHtml += "<li><img class=\"jsNativeView\" src=\"" + Jit.UI.Image.getSize(data.ImageList[i].imageURL,'240') + "\" ></li>";
                barHtml += "<li class=\"" + (i == 0 ? "on" : "") + "\">" + (i + 1) + "</li>";
            };
            $('#goodsImgs').html(imgHtml);
            if (data.ImageList.length > 1) {
                $('#goodsImgBar').html(barHtml);
                loaded();
            };
            Jit.UI.showPicture("jsNativeView");
        };
		//商品信息
		
        me.scalePrice = data.Price;
        me.discount = data.Discount;
        if(!!data.SkuList.length){
            var distributerPrice = data.SkuList[0].DistributerPrice,
                distributerStock = data.SkuList[0].DistributerStock,
                soldQty = data.SkuList[0].SoldQty;
            $('[jitval=itemPrice] span').html('￥' + distributerPrice);//销售价
            $('.inventoryCount span').text(distributerStock);//库存数
            $('.salesCount span').text(soldQty);//销售量
            
            if(distributerPrice>=data.Price){
                $('.goods_txt_box .discount').hide();
                $('.oldPrice').hide();
            }else{
                $('[jitval=oldPrice]').html(data.Price);
                var discount = Math.mul(Math.div(distributerPrice,data.Price),10);
                $('.goods_txt_box .discount').html(discount.toFixed(1)+'折');
            }
        }else{
            $('[jitval=itemPrice] span').html('￥' + data.DistributerPrice);//销售价
            $('[jitval=oldPrice]').html(data.Price);
            $('.goods_txt_box .discount').html(Math.mul(data.Discount,10).toFixed(1)+'折');
            $('.inventoryCount').hide();
            $('.salesCount').hide();
            //$('.inventoryCount span').text(distributerStock);//库存数
            //$('.salesCount span').text(soldQty);//销售量
        }
		
        //分享设置
        var title="";
        if(data&&data.ItemName){
            title=data.ItemName||Jit.AM.getAppVersion().APP_WX_TITLE||"";  //商品名称
        }else{
            title=Jit.AM.getAppVersion().APP_WX_TITLE||document.title;  //商品名称
        }
        var imgUrl="";  //分享的图片地址
        if(data.ImageList && data.ImageList.length > 0) {
            imgUrl=data.ImageList[0].imageURL;
        }else{
            imgUrl=Jit.AM.getAppVersion().APP_WX_ICO;
        }
        var theLink="";
        var baseInfo=Jit.AM.getBaseAjaxParam();

		var	shareInfo = {
			'title':(title|| document.title),
			'desc':'我发现一个不错的商品，赶紧来看看吧!',
			'link':location.href,
			'imgUrl':imgUrl,
            'Authparam':  [{ "paramname": "ObjectID", "paramvalue": Jit.AM.getUrlParam('goodsId')}],
			'isAuth':true//需要高级auth认证
		};
		Jit.AM.initShareEvent(shareInfo);
        var myScroll;
        //拖拽事件
        function loaded() {
            var goodsScroll = $('#goodsScroll'),
                menuList = $('#goodsImgBar li');
            //重新設置大小
            ReSize();
            function ReSize() {
                goodsScroll.find('.goods_img_list').css({
                    width: (goodsScroll.width()) * goodsScroll.find('.goods_img_list li').size()
                });
                goodsScroll.find('.goods_img_list li').css({
                    width: (goodsScroll.width())
                });
            }
            //綁定滾動事件
            myScroll = new iScroll('goodsScroll', {
                snap: true,
                momentum: true,
                hScrollbar: false,
                onScrollEnd: function() {
                    if (this.currPageX > (menuList.size() - 1)) {
                        return false;
                    };
                    menuList.removeClass('on');
                    menuList.eq(this.currPageX).addClass('on');
                },
				onBeforeScrollStart: function ( e ) {//可以滚动
					if ( this.absDistX > (this.absDistY + 5 ) ) {
						//防止浏览器的原生滚动
						e.preventDefault();
					}
				}
            });
            menuList.bind('click', function() {
                myScroll.scrollToPage(menuList.index(this));
            });
            $(window).resize(function(){
                ReSize();
                myScroll.refresh();
            });
            goodsScroll.css('overflow', '');
        }
		this.setForPonit(parseInt(data.Forpoints));
        this.refreshKeepState();
		this.refreshPraiseState();
        //详情内容
        if(data.ItemIntroduce){
            $("#description").html(data.ItemIntroduce);
        };
		//运费
		$('.postageBox p').html(data.DeliveryDesc);
        /*
        if (data.itemParaIntroduce) {
            var tempHtml = '',list;
            //data.itemParaIntroduce = '[{ "Name": "大小", "Value":"15寸"}, {"Name": "颜色","Value":"白色" }]';
            try{
                list = JSON.parse(data.itemParaIntroduce);
                for(var i=0;i<list.length;i++){
                    tempHtml += '<div><label style="width:72px;display: inline-block;">'+list[i]['Name']+'：</label><span>'+list[i]['Value']+'</span></div>';
                }
            }catch(e){
                tempHtml = data.itemParaIntroduce;
            }
            $('#itemParaIntroduce').html(tempHtml);
        };
        */
    },
    refreshKeepState: function() {
        var btnKeep = $('#btn_keep');
        if (this.isKeep) {
            btnKeep.addClass('g_favon').removeClass('g_fav');
        } else {
            btnKeep.addClass('g_fav').removeClass('g_favon');
        }
    },
    refreshPraiseState: function() {
        var btnPraise = $('#btn_praise');
        if (this.isPraise) {
            btnPraise.addClass('g_praon').removeClass('g_pra');
        } else {
            btnPraise.addClass('g_pra').removeClass('g_praon');
        }
    },
    initPropHtml: function() {
        var that = this,
            data = that.data,
            tpl = $('#prop_item').html(),
            prophtml = '',
            doms = $('[name=propitem]'),
            skuList = data.SkuList;
        that.skuListObj = skuList;
        for (var i = 1; i <= 5; i++) {
            var hashtpl = tpl,
                propName = data['Prop' + i];
            if (propName) {
                ++(that.level);
                $(doms.get(i - 1)).show();
                $($(doms.get(i - 1)).find('dt')).html(propName+'：');
            }
        }  
        that.setSkuListObj(skuList);
    },
    setSkuListObj:function(params){
        var that = this,
            skuList = params,
            leng = skuList.length,
            keysArray = [],
            skuArray = [],
            skuListObj = {};
        debugger;
        for(var i=0;i<leng;i++){
            if(skuList[i].DistributerStock==0){
                break;
            }
            var ind = skuArray.length,
                ishave = false;
            if(!ind){
                skuArray.push(skuList[i].PropName1);
                keysArray.push([]);
                keysArray[0].push(0);
            }else{
                for(var j=0;j<ind;j++){
                    if(skuArray[j] == skuList[i].PropName1){
                        ishave = true;
                        keysArray[j].push(i);
                    }
                }
                if(!ishave){
                    keysArray.push([]);
                    keysArray[ind].push(i);
                    skuArray.push(skuList[i].PropName1);
                }
            }
        }
        that.keysArray = keysArray;
        that.skuArray = skuArray;
        that.getSkuArray(keysArray);
    },
    getSkuArray:function(list){
        var that = this,
            optionhtml = '',
            skuListObj = that.skuListObj,
            $propitem = $($($('[name=propitem]').get(0)).find('dd'));
        for (var p = 0; p < list.length; p++) {
            var classVal = (p==0?'selected':'select'),
                item = skuListObj[list[p][0]];
            optionhtml += '<a class="'+classVal+'" skuid="'+ item.SkuID +'" idxVal="'+p+'">'+ item['PropName1'] +'</a>';
        }
        $propitem.html(optionhtml);
        if(that.level==2){
            that.getSkuData(0);
        }
    },
    getSkuData: function(idx) {
        var me = this,
            optionhtml = '',
            skuListObj = me.skuListObj,
            $propitem = $($($('[name=propitem]').get(1)).find('dd')),
            keysArray = me.keysArray[idx];
        if (!!keysArray) {
            for(var i = 0; i < keysArray.length; i++) {
                var key = keysArray[i],
                    params = skuListObj[key],
                    classVal = (i==0?'selected':'select');
                optionhtml += '<a class="'+classVal+'" skuid="'+ params.SkuID +'" idx="'+key+'" >' + params.PropName2 + '</a>';  
            }
            $propitem.html(optionhtml);
            /*
            $('[jitval=itemPrice] span').html('￥' + keysArray.DistributerPrice);//销售价
            $('.inventoryCount span').text(keysArray.DistributerStock);//库存数
            $('.salesCount span').text(keysArray.SoldQty);//销售量
            if(keysArray.DistributerPrice>=me.scalePrice){
                $('.goods_txt_box .discount').remove();
                $('.oldPrice').remove();
            }else{
                $('[jitval=oldPrice]').html(me.scalePrice);
                $('.goods_txt_box .discount').html(me.discount*10+'折');
            }
            */
        }
    },
    setItemKeep: function() {
		//添加收藏
		var me = this;
		if(!me.checkOAuth()){
			return;
		}
        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'setItemKeep',
                'itemId': me.goodsId,
                'keepStatus': (this.isKeep ? '0' : '1')
            },
            success: function(data) {
                if (data.code == 200) {
                    me.isKeep = !me.isKeep;
                    me.refreshKeepState();
                }
            }
        });
    },
    setItemPraise: function() {
		var me = this;
		if(!me.checkOAuth()){
			return;
		}
        me.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
            	'type':"Product",
                'action': 'UnitAndItem.Item.PraiseItem',
                'ItemId': me.goodsId
            },
            success: function(data) {
                if (data.IsSuccess) {
                    me.isPraise = !me.isPraise;
                    me.refreshPraiseState();
                }
            }
        });
    },
    submitOrder: function() {
		//提交订单
		var me = this,
            stockNum = $('.inventoryCount span').text()-0,
            propidx1 = $('[propidx=1] a.selected').index(),
            propidx2 = $('[propidx=2] a.selected').index();
        //JitPage.skuListObj  
        //JitPage.keysArray
        if(propidx1==-1){
            me.skuId = JitPage.skuListObj[0].SkuID;
        }else{
            if(propidx2==-1){
                var key = JitPage.keysArray[propidx1];
                me.skuId = JitPage.skuListObj[key].SkuID;
            }else{
                var key = JitPage.keysArray[propidx1][propidx2];
                me.skuId = JitPage.skuListObj[key].SkuID;
            }
        }
		if(!me.checkOAuth()){
			return;
		}
        if(!me.skuId) {
			Jit.UI.Dialog({
				'content': '亲，请选择商品的规格！',
				'type': 'Alert',
				'CallBackOk': function() {
					Jit.UI.Dialog('CLOSE');
				}
			});
            return;
        }
        if(stockNum==0 || stockNum<0){
            Jit.UI.Dialog({
                'content': '亲，此商品库存不足哦！',
                'type': 'Alert',
                'CallBackOk': function() {
                    Jit.UI.Dialog('CLOSE');
                }
            });
            return;
        }
        var list = [{
            'skuId': me.skuId,
            'salesPrice': me.scalePrice,
            'qty': parseInt($('#goods_number').val(),10)
        }];
        Jit.UI.Loading(true);
        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'qty': list[0].qty,
                'totalAmount': list[0].qty * list[0].salesPrice,
                'action': 'setOrderInfo',
                'orderDetailList': list,
				'RetailTraderId':me.retailTraderId,
                'SalesUser':Jit.AM.getUrlParam('salesUserId')||'',
                'IsShared':Jit.AM.getUrlParam('isShared')||''
            },
            success: function(data) {
                if (data.code == 200) {
                    me.toPage('GoodsOrder', '&orderId=' + data.content.orderId + me.paramStr);
                } else {
                    Jit.UI.Dialog({
                        'content': data.description,
                        'type': 'Alert',
                        'CallBackOk': function() {
                            Jit.UI.Dialog('CLOSE');
                        }
                    });
                }
				TopMenuHandle.ReCartCount();
            }
        });
    },
    getShoppingCart:function(){
        var me = this;
        me.ajax({
            url:'/OnlineShopping/data/Data.aspx',
            data:{
                'action':'getShoppingCart',
                'page':1,
                'pageSize':99
            },
            success:function(data){
                if(data.code == 200){
                    var result = data.content,
                        totalQty = result.totalQty;
                    if(!!totalQty){
                        $('.g_myShopCart span').show().text(totalQty);
                    }
                }
            },
            complete:function(){
                Jit.UI.Loading(false);
            }
        });
    },
    alert :function(text){
        Jit.UI.Dialog({
            type:"Dialog",
            content:text,
            times:1500
        });
        $('#dialog_div .popup').css({'width':'200px'});
    },
	checkOAuth:function(){
		if(JitCfg.CheckOAuth == 'unAttention'){
			this.toPage('unAttention');
			return false;
		}
		return true;
	},
	minGoodsDetail: function(){	
		var me = this,
			senderId = Jit.AM.getUrlParam('sender') || '';
		$('body').prepend('<input id="channelIdSize" type="hidden" value="6">');
		if(senderId){
			//我的小店分享之后点击商品的详情的情况
			me.ownerVipId = senderId;
			me.paramStr = '&ownerVipId='+me.ownerVipId+'&channelId=6';
		}else{
			$('#topNav').hide();
			$('.goods_wrap').css('margin-top','0px');
			$('.goods_btn .btnBox a').hide();
			$('#btn_keep').hide();//收藏按钮
			$('.createQrBtn').show();
			$('.generalizeBtn').show();//推广
			$('.g_myShopCart').hide();
			
			$('.createQrBtn').on('click',function(){
				if(!me.checkOAuth()){
					return;
				}
				if (!me.skuId){
					return;
				}
                var stockNum = $('.inventoryCount span').text()-0;
                if(stockNum==0 || stockNum<0){
                    Jit.UI.Dialog({
                        'content': '亲，此商品库存不足哦！',
                        'type': 'Alert',
                        'CallBackOk': function() {
                            Jit.UI.Dialog('CLOSE');
                        }
                    });
                    return;
                }
				var obj = {
					'skuId': me.skuId,
					'salesPrice': me.scalePrice,
					'qty': parseInt($('#goods_number').val(), 10)
				};
				obj = JSON.stringify(obj);
				Jit.AM.toPage('OrderQrcard','&orderObj='+obj);
			});
		}
	}
});