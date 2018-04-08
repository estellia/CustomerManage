Jit.AM.defindPage({
    name: 'GoodsDetail',
	hideMask:function(){
		$('#masklayer').hide();
	},
	initWithParam: function(param){
		/*
        if(param['imageBgColor']){
            $('.goods_img').css({
                'background-color':param['imageBgColor']
            })
        }
        */
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
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getItemDetail',
                'itemId': me.goodsId,
				'RetailTraderId':me.retailTraderId
            },
            success: function(data) {
				if(data.code == '200'){
					me.loadGoodsDetail(data.content);
				}else{
					Jit.UI.Dialog({
                        'content': data.description,
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
			//$('#lab_forpoint').show();
			//$('#forpoint').show().html(val);
		}else{
			$('#lab_forpoint').hide();
			//$('#forpoint').html('').hide();
		}
	},
    initEvent: function() {
		var that = this;
        /*
        $('[name=prop_option]').children().bind('click',function(evt){
			
        $('[name=prop_option]').children().removeClass('selected');
			
        JitPage.skuId = $(evt.target).addClass('selected').attr('skuId');
			
        });
        */
        $('[name=prop_option]').each(function(i, item) {
            $(item).bind('click', function(evt) {
                var $this = $(this),
					skuid = $(evt.target).attr('skuId');
				if($(evt.target).hasClass('unselected')){
					return false;
				}
                if(skuid){
                    $(evt.currentTarget).children().removeClass('selected');
                    var skuid = $(evt.target).addClass('selected').attr('skuid');
                    var idx = parseInt($(evt.currentTarget).attr('propidx'));

                    if(idx==2){
                        var propdetailid = $('[propidx="1"] a.selected').attr('prop_detail_id');
                        var propdetailid1 = $(evt.target).attr('prop_detail_id');
                        JitPage.getSkuData(idx + 1, propdetailid, skuid,propdetailid1);
                    }else{

                        JitPage.getSkuData(idx + 1, propdetailid, skuid);
                    }

                }
            });
        });
        // var heightKey='heightKey';
        // $('.gde_body').each(function(){
        //     var self=$(this);
        //     self.data(heightKey,self.height());
        // });
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
        this.initPageInfo();
        this.initPropHtml();
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
        me.itemNameText = data.itemName;    
        $('[jitval=itemName]').html(data.itemName);
		//$('.pinkageExplain').html(data.DeliveryStrategyDesc);
        // $('[jitval=itemImage]').attr('src', data.imageList[0].imageURL);
        if (data.imageList && data.imageList.length > 0) {
            var imgHtml = '',
                barHtml = '';
            for (var i = 0; i < data.imageList.length; i++) {
                imgHtml += "<li><img class=\"jsNativeView\" src=\"" + Jit.UI.Image.getSize(data.imageList[i].imageURL,'240') + "\" ></li>";
                barHtml += "<li class=\"" + (i == 0 ? "on" : "") + "\">" + (i + 1) + "</li>";
            };
            $('#goodsImgs').html(imgHtml);
            if (data.imageList.length > 1) {
                $('#goodsImgBar').html(barHtml);
                loaded();
            };
            Jit.UI.showPicture("jsNativeView");
        };
		//商品信息
		
		me.scalePrice = data.skuList[0].salesPrice;
		$('[jitval=itemPrice] span').html('￥' + data.skuList[0].salesPrice);//销售价
		$('.inventoryCount span').text(data.skuList[0].Stock);//库存数
		$('.salesCount span').text(data.salesQty);//销售量
		
		if(data.skuList[0].salesPrice>=data.skuList[0].price){
			$('.goods_txt_box .discount').remove();
			$('.oldPrice').remove();
		}else{
			$('[jitval=oldPrice]').html(data.skuList[0].price);
			var discount = (data.skuList[0].salesPrice/data.skuList[0].price)*10;
			$('.goods_txt_box .discount').html(discount.toString().substr(0,3)+'折');
		}
        //分享设置
        var title="";
        if(data&&data.itemName){
            title=data.itemName||Jit.AM.getAppVersion().APP_WX_TITLE||"";  //商品名称
        }else{
            title=Jit.AM.getAppVersion().APP_WX_TITLE||document.title;  //商品名称
        }
        var imgUrl="";  //分享的图片地址
        if(data.imageList && data.imageList.length > 0) {
            imgUrl=data.imageList[0].imageURL;
        }else{
            imgUrl=Jit.AM.getAppVersion().APP_WX_ICO;
        }
        var theLink="";
        var baseInfo=Jit.AM.getBaseAjaxParam();
        //用来做分润操作
        //theLink=location.href+"&RecommendVip="+baseInfo.userId;
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
        if (data.itemIntroduce) {
            $("#description").html(data.itemIntroduce);
        };
		//运费
		$('.postageBox p').html(data.DeliveryStrategyDesc);
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
        var data = this.data,
            itemlists = this.data.itemList;
        var tpl = $('#prop_item').html(),
            prophtml = '';
        var doms = $('[name=propitem]');
        for (var i = 1; i <= 5; i++) {
            var hashtpl = tpl;
            var propName = data['prop' + i + 'Name'],
                propList = null;
            if (propName) {
                $(doms.get(i - 1)).show();
                propList = data['prop' + i + 'List'];
                $($(doms.get(i - 1)).find('dt')).html(propName+'：');
                if (propList) {
                    this.buildSkuItem(i, propList);
                    if (i == 1) {
                        this.getSkuData(2, propList[0]['prop' + i + 'DetailId'], propList[0]['skuId']);
                    }
                }
            } else {
                //doms.get(i - 1).style.display = 'none';
				//如果此商品 prop1Name 都不存在说明 此商品无规格选择
				if(i==1){
					this.getSkuData(1,'',data['prop1List'][0]['skuId']);
				}
            }
        }
    },
    getSkuData: function(idx, propId,skuId,propId1) {
        var me = this;
        if (!me.data['prop' + idx + 'Name']) {//最后一个规格执行

            me.skuId = skuId;//判断当前层级是否存在规格
            for (var i = 0; i < me.data.skuList.length; i++) {
                if (me.data.skuList[i]['skuId'] == skuId) {
                    me.scalePrice = me.data.skuList[i].salesPrice;
                    $('[jitval=itemPrice] span').html('￥' + me.data.skuList[i].salesPrice);//销售价
					$('.inventoryCount span').text(me.data.skuList[i].Stock);//库存数
					$('.salesCount span').text(me.data.skuList[i].SalesCount);//销售量salesQty salesCount
					if(me.data.skuList[i].salesPrice>=me.data.skuList[i].price){
						$('.goods_txt_box .discount').remove();
						$('.oldPrice').remove();
					}else{
						$('[jitval=oldPrice]').html(me.data.skuList[i].price);
						var discount = (me.data.skuList[i].salesPrice/me.data.skuList[i].price)*10;
						$('.goods_txt_box .discount').html(discount.toString().substr(0,3)+'折');
					}
                    return;
                }
            }
            //$('[jitval=itemPrice]').html('');
            return;
        }
		//TODO:获取下个规格的数据
        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getSkuProp2List',
                'propDetailId': propId,
                'prop1DetailId': propId1,
                'itemId': me.goodsId,
                'EventId': '',
                'Type': 1//2:活动商品 1:普通商品
            },
            success: function(data) {
                if (data.code == 200) {
                    for (var name in data.content) {
                        for (var i = 1; i <= 5; i++) {
                            if (('prop' + i + 'List') == name) {
								if(data.content[name].length==0){
									return;
								}
                                me.buildSkuItem(idx, data.content[name]);
                                var skuId = (data.content[name][0]['Stock']<=0)? 0 : data.content[name][0]['skuId'];
                                me.getSkuData(idx+1,propId, skuId,data.content[name][0]['prop' + i + 'DetailId']);
                                if (!me.data['prop' + (i + 1) + 'Name']) {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        });
    },
    buildSkuItem: function(idx, list) {
        var optionhtml = '',
			$propitem = $($($('[name=propitem]').get(idx - 1)).find('dd')),
			isVal = $('.selected',$propitem).text(),
			stockQty,
			isFirst,
			isFalse;
        for (var p = 0; p < list.length; p++) {
			stockQty = (list[p].Stock < 0 ? 0 : list[p].Stock);
			isFirst = ((stockQty!=0 && p==0) ? 'selected' : 'select');
            if(idx>2){       //后台接口字段写死了。
                idx=2;
            }
			if(!isVal){
				//((p==0) ? 'selected' : '')
				optionhtml += '<a class="' + ((stockQty==0) ? 'unselected' : isFirst) + '" skuid="' + list[p].skuId + '" prop_detail_id="' + list[p]['prop' + idx + 'DetailId'] + '" >' + list[p]['prop' + idx + 'DetailName'] + '</a>';
			}else{
				//isFalse = (isVal == list[p]['prop' + idx + 'DetailName'])?true:false;
				//optionhtml += '<a class="' + (isFalse ? 'selected' : '') + '" skuid="' + list[p].skuId + '" prop_detail_id="' + list[p]['prop' + idx + 'DetailId'] + '" >' + list[p]['prop' + idx + 'DetailName'] + '</a>';
				optionhtml += '<a class="' + ((stockQty==0) ? 'unselected' : isFirst) + '" skuid="' + list[p].skuId + '" prop_detail_id="' + list[p]['prop' + idx + 'DetailId'] + '" >' + list[p]['prop' + idx + 'DetailName'] + '</a>';
			}
        }
        $propitem.html(optionhtml);
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
            stockNum = $('.inventoryCount span').text()-0;
		if(!me.checkOAuth()){
			return;
		}
        if (!me.skuId) {
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
            'qty': parseInt($('#goods_number').val(), 10)
        }];
        Jit.UI.Loading(true);
        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'qty': list[0].qty,
                'totalAmount': list[0].qty * list[0].salesPrice,
                'action': 'setOrderInfo',
                'orderDetailList': list,
				'RetailTraderId':me.retailTraderId
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
					}  else{
                        $('.g_myShopCart span').hide()
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
    }
});