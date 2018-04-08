Jit.AM.defindPage({
    name: 'GroupGoodsDetail',
    initWithParam: function(param) {
        if(param.commentHide){
            $('#commentArea').hide();
        };
        if (param['gdeDisplay'] == false) {
            $('#ppTitle').hide();
            $('#ppBody').hide();
        }
    },
    onPageLoad: function() {
        var me = this;
        me.goodsId = me.getUrlParam('goodsId') || me.getParams('goodsId');
        me.eventId = me.getUrlParam('eventId') || me.getParams('eventId');

        me.setParams('eventId', me.eventId);
        me.setParams('goodsId', me.goodsId);

        Jit.log('商品ID: ' + me.goodsId);

        me.ajax({
            url: '/Interface/data/ItemData.aspx',
            data: {
                'action': 'getPanicbuyingItemDetail',
                'itemId': me.goodsId,
                'eventId':me.eventId
            },
            success: function(data) {
                if(data.code == '200') {
                    me.loadGoodsDetail(data.content);
                }else {
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

        $('#btn_delNum').bind('click', function() {
            var num = $('#goods_number').val();

            num--;

            if (num <= 1) {
                num = 1;
            }

            $('#goods_number').val(num);

            var pt = parseInt(me.data.Forpoints) * num;

            pt = (isNaN(pt) ? '0' : pt);

            $('#forpoint').html((isNaN(pt) ? '0' : pt));

            me.setForPonit(pt);
        });

        $('#btn_addNum').bind('click', function() {

            var num = $('#goods_number').val();

            num++;

            $('#goods_number').val(num);

            var pt = parseInt(me.data.Forpoints) * num;

            pt = (isNaN(pt) ? '0' : pt);

            $('#forpoint').html((isNaN(pt) ? '0' : pt));

            me.setForPonit(pt);
        });
    },
    SetSpareTime: function(){
        var self = this,
			element = $('#spareTime'),
			secound = 0,
			clearInt = null;
        function Adjust(){
            second = self.timeStr;
            if(second==0){
				clearInterval(clearInt);
                //self.onPageLoad();
            }
            _h = Math.floor(second / 3600);

            _m = Math.floor((second % 3600) / 60);

            _s = Math.floor(((second % 3600) % 60));

            //超过99小时全部显示99
            _m = _h > 99 ? 99 : _m;
            _s = _h > 99 ? 99 : _s;
            _h = _h > 99 ? 99 : _h;

			self.timeStr = --self.timeStr;
            //$(dom).attr('time-date', endtime - 1);
            //console.log(_h+' '+_m+' '+_s);

            var h1 = Math.floor(_h / 10) > 9 ? 9 : Math.floor(_h / 10),
				h2 = Math.floor(_h % 10),
				m1 = Math.floor(_m / 10),
				m2 = Math.floor(_m % 10),
				s1 = Math.floor(_s / 10),
				s2 = Math.floor(_s % 10),
				timeStr = h1.toString()+h2.toString()+':'+m1.toString()+m2.toString()+':'+s1.toString()+s2.toString();
			
            element.html(timeStr);
			if(timeStr=='00:00:00'){
				$('#submitOrderBtn').addClass("on").show();
				$('#submitOrderBtn').css({'background':'#ccc'}).attr('href','javascript:;').text('已结束');
				element.text('已结束');
			}
        }

        clearInt = setInterval(Adjust, 1000);
    },
    setForPonit: function(val) {

        if (val && val > 0) {

            $('#lab_forpoint').show();

            $('#forpoint').show().html(val);
        } else {

            $('#lab_forpoint').hide();

            $('#forpoint').html('').hide();
        }
    },
    initEvent: function() {
        /*
        $('[name=prop_option]').children().bind('click',function(evt){
			
        $('[name=prop_option]').children().removeClass('selected');
			
        JitPage.skuId = $(evt.target).addClass('selected').attr('skuId');
			
        });
        */
        $('[name=prop_option]').each(function(i, item) {

            $(item).bind('click', function(evt) {

                var skuid = $(evt.target).attr('skuId');

                if (skuid) {

                    $(evt.currentTarget).children().removeClass('selected');

                    var skuid = $(evt.target).addClass('selected').attr('skuid');

                    var idx = parseInt($(evt.currentTarget).attr('propidx'));

                    var propdetailid = $(evt.target).attr('prop_detail_id');
                    
                    JitPage.getSkuData(idx + 1, propdetailid, skuid);
                }
            });
        });

        // var heightKey='heightKey';
        // $('.gde_body').each(function(){
        //     var self=$(this);
        //     self.data(heightKey,self.height());
        // });

        $('.gde_title>a').bind('click', function() {

            var self = $(this),
                parentDiv = self.parent(),
                parentBody = parentDiv.next('.gde_body');
            if (self.hasClass('on')) {
                //显示
                self.removeClass('on');
                parentBody.show();
                // parentBody.animate({'height':0},400);
            } else {
                //隐藏
                self.addClass('on');
                parentBody.hide();
                // parentBody.animate({'height':curHeight},400);
            }

        });
		
		$('.g_share').bind('click', function () {
			$('#share-mask').show();
			$('#share-mask-img').show().attr('class', 'pullDownState');
		});
		$('#share-mask').bind('click', function () {
			var $this = $(this);
			$('#share-mask-img').attr('class', 'pullUpState').show();
			$('#share-mask-img').hide();
			$this.hide();
		});


    },
    loadGoodsDetail: function(data) {

        this.data = data;

        this.isGoing = data.beginLineSecond==0&&data.deadlineSecond>0;

		//var endtime = (new Date(data.endTime.replace(/-/gi,'/'))).getTime();
		var endtime = data.deadlineSecond;
		this.timeStr = endtime;
		this.SetSpareTime(this.timeStr);
		
        this.isKeep = data.isKeep;

        this.initPageInfo();

        this.initPropHtml();

        this.initEvent();
    },
    addToGoodsCart: function() {

        var me = this;

        if (!me.checkOAuth()) {

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

        var data = this.data,
			eventTypeId = data.eventTypeID,
			$submitOrderBtn = $('#submitOrderBtn');
        if(this.isGoing){
            $submitOrderBtn.addClass("on").show();
        }
		
		if(eventTypeId==1){//1=团购
			$submitOrderBtn.html('立即参团');
		}else if(eventTypeId==2){//2=抢购
			$submitOrderBtn.html('立即抢购');
		}else if(eventTypeId==3){//3=热销
			$submitOrderBtn.html('立即购买');
		}
		
		//未开始
		if(data.beginLineSecond > 0){
			$submitOrderBtn.addClass("on").show();
			$submitOrderBtn.css({'background':'#ccc'}).attr('href','javascript:;').text('未开始');
		}
		
		//判断商品是否被抢光了
		if(data.overQty == 0){
			$submitOrderBtn.css({'background':'#ccc'}).attr('href','javascript:;').text('已抢光');
		}
		

        $('[jitval=itemName]').html(data.itemName);
        $('#txtRebate').html(data.discountRate);
        $('#txtBuyCount').html(data.salesCount);

        // $('[jitval=itemImage]').attr('src', data.imageList[0].imageURL);
        if (data.imageList && data.imageList.length > 0) {
            var imgHtml = '',
                barHtml = '';
            for (var i = 0; i < data.imageList.length; i++) {
                imgHtml += "<li><img  src=\"" + Jit.UI.Image.getSize(data.imageList[i].imageUrl, '480') + "\" ></li>";
                barHtml += "<li class=\"" + (i == 0 ? "on" : "") + "\">" + (i + 1) + "</li>";
            };

            $('#goodsImgs').html(imgHtml);
            if (data.imageList.length > 1) {
                $('#goodsImgBar').html(barHtml);
                loaded();
            };
        };
        //分享设置
        var title="";
        if(data&&data.itemName){
            title=data.itemName||Jit.AM.getAppVersion().APP_WX_TITLE||"";  //商品名称
        }else{
            title=Jit.AM.getAppVersion().APP_WX_TITLE||document.title;  //商品名称
        }
        var imgUrl="";  //分享的图片地址
        if(data.imageList && data.imageList.length > 0) {
            imgUrl=data.imageList[0].imageUrl;
        }else{
            imgUrl=Jit.AM.getAppVersion().APP_WX_ICO;
        }
		var	shareInfo = {
			'title':(title|| document.title),
			'desc':'我发现一个不错的商品，赶紧来看看吧!',
			'link':location.href,
			'imgUrl':imgUrl,
			'isAuth':true//需要高级auth认证
		};
		Jit.AM.initShareEvent(shareInfo);
		
        var myScroll;
        //拖拽事件
        function loaded() {
            var goodsScroll = $('#groupGoodsScroll');
            // menuList = $('#goodsImgBar li');

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
            myScroll = new iScroll('groupGoodsScroll', {
                snap: true,
                momentum: true,
                hScrollbar: false,
                onScrollEnd: function() {
                    // if (this.currPageX > (menuList.size() - 1)) {
                    //     return false;
                    // };
                    // menuList.removeClass('on');
                    // menuList.eq(this.currPageX).addClass('on');
                },
				onBeforeScrollStart: function ( e ) {//可以滚动
					if ( this.absDistX > (this.absDistY + 5 ) ) {
						// user is scrolling the x axis, so prevent the browsers' native scrolling
						e.preventDefault();
					}
				}
            });
            // menuList.bind('click', function() {
            //     myScroll.scrollToPage(menuList.index(this));
            // });
            $(window).resize(function() {
                ReSize();
                myScroll.refresh();
            });
            goodsScroll.css('overflow', '');
        }

        this.setForPonit(parseInt(data.Forpoints));

        this.refreshKeepState();

        if (data.itemIntroduce) {
            $("#description").html(data.itemIntroduce);
        };

        var itemParaIntroduce = $('#itemParaIntroduce');
        if (data.itemParaIntroduce) {
            itemParaIntroduce.html(data.itemParaIntroduce);
        };
        if (data.remark) {
            $('#itemIntroduce').html(data.remark);

        };
        if (!data.itemParaIntroduce && !data.itemIntroduce) {
            itemParaIntroduce.html('无');
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
    initPropHtml: function() {

        var data = this.data,
            itemlists = this.data.itemList;


        if(data.singlePurchaseQty){
            $(".singlePurchaseQty dd").html("每人限购 <b style='color:red;'>"+data.singlePurchaseQty+"</b> 件");
        }

        var tpl = $('#prop_item').html(),
            prophtml = '';

        var doms = $('[name=propitem]');

        for (var i = 1; i <= 5; i++) {

            var hashtpl = tpl;

            var propName = data['prop' + i + 'Name'],
                propList = null;

            if (propName) {
                propList = data['prop' + i + 'List'];

                $($(doms.get(i - 1)).find('dt')).html(propName);

                if (propList) {

                    this.buildSkuItem(i, propList);

                    if (i == 1) {

                        this.getSkuData(2, propList[0]['prop' + i + 'DetailId'], propList[0]['skuId']);
                    }
                }

            } else {
                doms.get(i - 1).style.display = 'none';

                //如果此商品 prop1Name 都不存在说明 此商品无规格选择
                if (i == 1) {
					debugger;
                    this.getSkuData(1, '', data['prop1List'][0]['skuId']);
                }
            }
        }
    },
    getSkuData: function(idx, propId, skuId) {
        var me = this;
        debugger;
        if (!me.data['prop' + idx + 'Name']) {
            me.skuId = skuId;
            for (var i = 0; i < me.data.skuList.length; i++) {
                if (me.data.skuList[i]['skuId'] == skuId) {
                    me.scalePrice = me.data.skuList[i].salesPrice;
                    //debugger;
                    $('[jitval=itemPrice]').html('<span>￥'+me.data.skuList[i].salesPrice+'</span>');
                    //折扣计算
					debugger;
					if(me.data.skuList[i].salesPrice>=me.data.skuList[i].price){
						$('#txtRebateBox').hide();
						$('[jitval=oldPrice]').remove();
					}else{
						$('[jitval=oldPrice]').html('￥' + me.data.skuList[i].price);
						var discount = (me.data.skuList[i].salesPrice/me.data.skuList[i].price)*10;
						$('#txtRebate').html(discount.toString().substr(0,3)+'折');
					}
                    return;
                }
            }
            $('[jitval=itemPrice]').html('');
            return;
        }


        me.ajax({
            url: '/OnlineShopping/data/Data.aspx',
            data: {
                'action': 'getSkuProp2List',
                'propDetailId': propId,
                'itemId': me.goodsId,
                'EventId': Jit.AM.getUrlParam('eventId') || '',
                'Type': 2//2:活动商品 1:普通商品
            },
            success: function(data) {

                if (data.code == 200) {

                    for (var name in data.content) {

                        for (var i = 1; i <= 5; i++) {

                            if (('prop' + i + 'List') == name) {

                                me.buildSkuItem(i, data.content[name]);

                                me.getSkuData(i + 1, data.content[name][0]['prop' + i + 'DetailId'], data.content[name][0]['skuId']);

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
        var optionhtml = '';
        for (var p = 0; p < list.length; p++) {
            optionhtml += '<a class="' + ((p == 0) ? 'selected' : '') + '" skuid="' + list[p].skuId + '" prop_detail_id="' + list[p]['prop' + idx + 'DetailId'] + '" >' + list[p]['prop' + idx + 'DetailName'] + '</a>';
        }

        $($($('[name=propitem]').get(idx - 1)).find('dd')).html(optionhtml);
    },
    setItemKeep: function() {
        //添加收藏
        var me = this;
        if (!me.checkOAuth()) {
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
    submitOrder: function() {
        //提交订单
        var me = this;
        //还未开始
        if(!this.isGoing){
            return false;
        }
        if(this.data.singlePurchaseQty && $('#goods_number').val()>this.data.canBuyCount){
            Jit.UI.Dialog({
                'content': "限购"+this.data.singlePurchaseQty+"件，你还能购买"+this.data.canBuyCount+"件,请修改购买数量",
                'type': 'Alert',
                'CallBackOk': function(){
                    Jit.UI.Dialog('CLOSE');
                }
            });
            return;
        }
        if (!me.checkOAuth()) {

            return;
        }

        var me = this;

        if (!me.skuId) {

            return;
        }

        var list = [{
            'skuId': me.skuId,
            'salesPrice': me.scalePrice,
            'qty': parseInt($('#goods_number').val(), 10)
        }];

        me.ajax({
            url: '/Interface/data/ItemData.aspx',
            data: {
                'qty': list[0].qty,
                'totalAmount': list[0].qty * list[0].salesPrice,
                'action': 'setOrderInfo',
                'orderDetailList': list,
				'isGroupBy':1,
				'eventId':me.getUrlParam('eventId'),
				'reqBy':'3'
            },
            success: function(data) {

                if (data.code == 200) {

                    me.toPage('GoodsOrder', '&orderId=' + data.content.orderId + '&eventId=' + me.getUrlParam('eventId'));
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
    checkOAuth: function() {

        if (JitCfg.CheckOAuth == 'unAttention') {

            this.toPage('unAttention');

            return false;
        }

        return true;
    }
});