Jit.AM.defindPage({

    name: 'IndexShop',
	 ele:{sortableDiv:$(".content")},
	initWithParam: function(param){
		console.log(param);
		if(param['showMiddleArea']=='false'){
			$('.noticeList').hide();
		}
	},
	
    onPageLoad: function () {
        Jit.UI.Loading(false);
		this.densityDpi();
        this.loadData();
        this.initEvent();
    },
	densityDpi:function(){
		if($('html').data('dpi')){
			var phoneWidth = parseInt(window.screen.width);
			var phoneScale = phoneWidth/640;
			var isAndroid = RegExp("Android").test(navigator.userAgent);
			if (isAndroid) {
				$('head').append('<meta name="viewport" content="width=640, user-scalable=no, minimum-scale = '+phoneScale+', maximum-scale = '+phoneScale+',initial-scale = '+phoneScale+ ',target-densitydpi=device-dpi ">');
			} else {
				$('head').append('<meta name="viewport" content="width=640, user-scalable=no, target-densitydpi=device-dpi">');
			}
		}
	},
    initEvent:function(){
        $("body").delegate("#searchBtn","click",function(){
			var searchVal = $.trim($("#searchContent").val());
			if(searchVal==''){
				alert('亲，请填写搜索产品！');
				return false;
			}
            Jit.AM.toPage('GoodsList','itemName='+ searchVal);
        });

		$(document).on('keyup',function(){
			if(window.event.keyCode == 13){
				$('#searchBtn').trigger('click');
			}
		});
    },

    loadData: function () {
        debugger;
        var self=this;
        Jit.UI.Loading(true);

        self.ajax({
            url: '/ApplicationInterface/Gateway.ashx',
            data: {
                'action': 'AppConfig.HomePageConfig.HomePageConfig',
                'homeId':Jit.AM.getUrlParam("homeId")
            },
            success: function (data) {

                Jit.UI.Loading(false);
                if(data.Data) {
                    if(!data.Data.Success){
                        alert(data.Data.ErrMsg);
                        return false;
                    }

                    //debugger;
                    var json=data.Data.sortActionJson||"[]"
                    self.sotrActionJson = JSON.parse(json);
                    if (data.Data.sortActionJson&&self.sotrActionJson.length>0) {
                        self.ele.sortableDiv.html("");
                    for(var i=0;i<self.sotrActionJson.length;i++){ var item = self.sotrActionJson[i];
                        self.ele.sortableDiv.append('<div data-type="'+item.type+'" class="action"></div>')
                        }
                        var ele = {
                            adList: $("[data-type='adList']"),  //幻灯片播放
                            entranceList: $("[data-type='categoryEntrance']"),//分类导航
                            //eventList: $("[data-type='eventList']"),//团购抢购营销,
                            originalityList: $("[data-type='originalityList']"),//创意组合
                            SearchDiv: $("[data-type='search']"), //搜索框
                            secondKill: $("[data-type='secondKill']"),// 团购 抢购 营销单个
                            hotBuy:$("[data-type='hotBuy']"),//  抢购
                            groupBuy:$("[data-type='groupBuy']"),//团购
                            bargain:$("[data-type='bargain']"),
                            navigation: $("[data-type='navList']"),//顶部导航
                            followInfo: $("[data-type='followInfo']"),//关注
                            productList: $("[data-type='productList']") //上班列表

                        };
                        $.extend(self.ele, ele);
                        var isEmpty=true;
                        if (data.Data.search&&data.Data.search.MHSearchAreaID&&self.ele.SearchDiv.length>0) { //搜索
                            isEmpty=false
                            self.renderSearchList(data.Data.search);
                        } else {
                            self.ele.SearchDiv.html('');
                            //self.ele.SearchDiv.remove();
                        }
                        if (data.Data.adAreaList&&data.Data.adAreaList.length>0&&self.ele.adList.length>0) {   //广告轮播左侧
                            isEmpty=false
                            self.initImageScrollView(data.Data.adAreaList);
                        } else {
                            self.ele.adList.html('');
                            //self.ele.adList.remove();
                        }

                        if (data.Data.categoryEntrance&&self.ele.entranceList.length>0) {    //分类导航左侧
                            isEmpty=false
                            self.renderEntranceList(data.Data.categoryEntrance);
                        } else {
                            self.ele.entranceList.html('');
                            //self.ele.entranceList.remove();
                        }
                        if (data.Data.eventList&&data.Data.eventList.length>0&&self.ele.eventList.length>0) {  //团购抢购组合框。
                            //debugger;
                            isEmpty=false
                            self.renderEventList(data.Data.eventList[0]);
                        } else {
                            self.ele.eventList.html('');
                            //self.ele.eventList.remove();
                        }
                        if (data.Data.originalityList&&data.Data.originalityList.length>0&&self.ele.originalityList.length>0){   //创意组合。
                            isEmpty=false
                            self.renderOriginalityList(data.Data.originalityList);
                        } else {
                            self.ele.originalityList.html('');
                            //self.ele.originalityList.remove();
                        }

                        if(data.Data.productList&&data.Data.productList.length>0&&self.ele.productList.length>0){   //商品列表
                            isEmpty=false
                            self.renderProductList(data.Data.productList);
                        } else {
                            self.ele.productList.html('');
                            //self.ele.productList.remove();
                        }

                        if (data.Data.follow&&data.Data.follow.FollowId&&self.ele.followInfo.length>0) {   //关注
                            isEmpty=false
                           if(data.Data.follow.Url) {
                               if (data.Data.follow.Url.indexOf("http:") == -1&& data.Data.follow.Url.indexOf("https:") == -1) {

                                   data.Data.follow.Url = "http://" + data.Data.follow.Url;
                               }

                           }  else{
                               data.Data.follow.Url="";
                           }
                            self.renderFollowInfo(data.Data.follow);

                        } else{
                            self.ele.followInfo.html('');
                            //self.ele.followInfo.remove();
                        }

                        if (data.Data.secondKill&&data.Data.secondKill.length>0&&self.ele.secondKill.length>0) {  //秒杀
                            isEmpty=false
                            self.renderSecondKillList(data.Data.secondKill);
                        } else {
                            self.ele.secondKill.html('');
                            //self.ele.secondKill.remove();
                        }
                        if (data.Data.groupBuy&&data.Data.groupBuy.length>0&&self.ele.groupBuy.length>0) {//团购
                            isEmpty=false
                            self.renderSecondKillList(data.Data.groupBuy);
                        } else {
                            self.ele.groupBuy.html('');
                            //self.ele.groupBuy.remove();
                        }
                        if (data.Data.hotBuy&&data.Data.hotBuy.length>0&&self.ele.hotBuy.length>0) {    //热销
                            isEmpty=false
                            self.renderSecondKillList(data.Data.hotBuy);
                        } else {
                            self.ele.hotBuy.html('');
                            //self.ele.hotBuy.remove();
                        }
                        if (data.Data.bargain&&data.Data.bargain.length>0&&self.ele.bargain.length>0) {    //热销
                            isEmpty=false
                            self.renderSecondKillList(data.Data.bargain);
                        } else{
                            self.ele.bargain.html('');
                            //self.ele.bargain.remove();
                        }
                        //debugger;
                        if (data.Data.navList&&self.ele.navigation.length>0) {//底部导航
                            isEmpty=false
                            self.initNavList(data.Data.navList);
                        } else {
                            self.ele.navigation.html('');
                            //self.ele.navigation.remove();
                        }
                        if(isEmpty) {
                            self.ele.sortableDiv.html("<p id='set' style='width: 100%; text-align:center; font-size: 16px; color: #000; line-height: 36px'>该首页模板无任何模块</p>");
                        }
                        /****倒计时***/
                        if( $(".timeList").length>0) {
                            $(".timeList").each(function () {
                                var me = $(this);
                                var maxTime = parseInt(me.data("time"));
                                var isStart=me.data("start");
                                var timer = setInterval(function () {
                                    if (maxTime >= 0) {
                                        /*var hour = Math.floor(maxTime / 60/60);
                                         var minutes = Math.floor(maxTime % (60*12));
                                         var seconds = Math.floor(maxTime % 60);*/
                                        var d = Math.floor(maxTime / 60 / 60 / 24);
                                        var h = Math.floor(maxTime / 60 / 60);
                                        var m = Math.floor(maxTime / 60 % 60);
                                        var s = Math.floor(maxTime % 60);
                                       if(h<10){
                                           h="0"+h;
                                       }
                                        if(m<10){
                                            m="0"+m;
                                        }
                                        if(s<10){
                                            s="0"+s;
                                        }

                                        var html = "<div class='textL'><span>距</span><span>开</span><span>始</span></div><em>" + h + "</em>:<em>" + m + "</em>:<em>" + s + "</em>";
                                       if(isStart==1){
                                            html = "<div class='textL'><span>距</span><span>结</span><span>束</span></div><em>" + h + "</em>:<em>" + m + "</em>:<em>" + s + "</em>";
                                       }
                                        me.html(html);

                                        --maxTime;

                                    }
                                    else {
                                        clearInterval(timer);
                                        me.html("活动已经结束").addClass("end");
                                    }
                                }, 1000);
                            });
                        }
                        if($(".commonIndexArea").length>0){
                            $(".commonIndexArea").each(function(){
                                  if($(this).find(".leftBox").length>0&&$(this).find(".rightBox").length>0){
                                      if($(this).find(".leftBox").height()>=$(this).find(".rightBox").height()){
                                          $(this).find(".leftBox").addClass("borderRBox")
                                      } else{
                                          $(this).find(".rightBox").addClass("borderLBox")
                                      }

                                  }
                            });
                        }
                    }
                    $(".Price").each(function(){
                          var value=$(this).html().trim();
                        $(".original").each(function(){
                            var value2=$(this).html().trim();
                            if(value==value2){
                                $(this).hide();
                            }
                        });
                    });



                    $("img.lazy").lazyload({
                        placeholder : "../../../images/common/opacityImg.png",
                        effect : "fadeIn"//effect
                        //threshold : 200,
                        //event : "click",
                        //failurelimit : 100
                    });
                }else{
                    //空白模板，无任何数据
                }

            }
        });
    },
    renderOriginalityList: function (data) {
        //debugger;

        var self = this;

        var list = data || [];
        if (list.length) {
            self.ele.originalityList.each(function () {
                var html = '', tpl;

                var index = $(this).index();
                $(this).addClass("originalityList");
                var idata = null;
                for (var i = 0; i < list.length; i++) {
                    if (list[i].displayIndex == index) {
                        idata = list[i];
                        break
                    }
                }

                html += self.addTitle(idata);
                if (idata['CategoryAreaList'].length == 3) {
                    //1+2模块
                    tpl = $('#Tpl_item_3').html();

                } else if (idata['CategoryAreaList'].length  == 1) {
                    //单张图片模块
                    tpl = $('#Tpl_item_1').html();
                } else if (idata['CategoryAreaList'].length == 2) {
                    //1+1模式
                    tpl = $('#Tpl_item_2').html();
                }

                var itemdata = self.buildItemData(idata);

                html += Mustache.render(tpl, itemdata);
                $(this).append(html)

            });


        }

    },
    renderFollowInfo:function(data){
        //debugger;
        var html="",tpl,me=this;
        tpl=$("#Tpl_follow").html();
        html+=Mustache.render(tpl, data);
        me.ele.followInfo.html(html);
        $("#followInfo").css({"top":"0","bottom":"auto"}) ;
        $("#followInfo").parent(".action").css({"margin":0});
        $(".content").css({"padding-top":"80px","padding-bottom":"0px"})
    },
    initNavList:function(data){
        //debugger;
        var html="",tpl,me=this;
        tpl=$("#navListItem").html();
        var dataList=me.buildItemData(data);
        html+=Mustache.render(tpl, dataList);

        me.ele.navigation.html(html);
        if (data.styleType == "s2") {
           $("#navigation").css({"top":"0","bottom":"auto"}) ;
            $(".content").css({"padding-top":"100px","padding-bottom":"0px"})
        } else {
            $("#navigation").css({"top":"auto","bottom":"0"}) ;
            $(".content").css({"padding-bottom":"105px"});
        }
    },
    renderEventList:function(data){
        var me = this;
        var tpl = '', html = '';
        var datas=[];
        if(data.arrayList.length>0){
            datas=data.arrayList
        }


        for(var i in datas){

            if(datas[i].TypeID == 1){
                //团购
                tpl = $('#Tpl_group_shop').html();

            }else if(datas[i].TypeID == 2){
                //抢购
                tpl = $('#Tpl_rush_shop').html();

            }else if(datas[i].TypeID == 3){
                //热购
                tpl = $('#Tpl_hot_shop').html();
            }

            datas[i].DiscountRate = datas[i].DiscountRate;

            html += Mustache.render(tpl, datas[i]);
        }

        me.ele.eventList.html('<div class="noticeList"><div class="line"></div>'+html+'</div>');
    },
    renderSecondKillList:function(dataList){

        var self = this;

        var list = dataList || [];
        if (list.length) {
			console.log(list);
            for (var i = 0; i < list.length; i++) {


                var idata = null;
                if(list[i].areaFlag=="hotBuy") {
                    self.ele.hotBuy.each(function () {

                        var index = $(this).index();
                        $(this).addClass("activity");

                        if (list[i].displayIndex == index) {
                            idata = list[i];
                        }
                          if(idata) {
                              var html = '', tpl;
                              html += self.addActivityTitle(idata);
                              if (idata['arrayList'].length == 3) {
                                  //1+2模块
                                  tpl = $('#Tpl_item_3').html();

                              } else if (idata['arrayList'].length == 1) {
                                  //单张图片模块
                                  tpl = $('#Tpl_item_1').html();
                              } else if (idata['arrayList'].length == 2) {
                                  //1+1模式
                                  tpl = $('#Tpl_item_2').html();
                              }

                              var itemdata = self.buildItemData(idata);

                              html += Mustache.render(tpl, itemdata);
                              $(this).html(html);
                              return false;
                          }

                    });
                }else if(list[i].areaFlag=="secondKill") {
                        self.ele.secondKill.each(function () {

                            var index = $(this).index();
                            $(this).addClass("activity");
                            if (list[i].displayIndex == index) {
                                idata = list[i];
                            }
                            //debugger;
                            if(idata) {
                                var html = '', tpl;
                                html += self.addActivityTitle(idata);
                                if (idata['arrayList'].length == 3) {
                                    //1+2模块
                                    tpl = $('#Tpl_item_3').html();

                                } else if (idata['arrayList'].length == 1) {
                                    //单张图片模块
                                    tpl = $('#Tpl_item_1').html();
									
                                } else if (idata['arrayList'].length == 2) {
                                    //1+1模式
                                    tpl = $('#Tpl_item_2').html();
                                }

                                var itemdata = self.buildItemData(idata);

                                html += Mustache.render(tpl, itemdata);
                                $(this).html(html);
                                return false;

                            }
                        });
                }else if(list[i].areaFlag=="groupBuy") {
                    self.ele.groupBuy.each(function () {

                        var index = $(this).index();
                        $(this).addClass("activity");
                        if (list[i].displayIndex == index) {
                            idata = list[i];
                        }
                        if(idata) {
                            var html = '', tpl;
                            html += self.addActivityTitle(idata);
                            if (idata['arrayList'].length == 3) {
                                //1+2模块
                                tpl = $('#Tpl_item_3').html();

                            } else if (idata['arrayList'].length == 1) {
                                //单张图片模块
                                tpl = $('#Tpl_item_1').html();
                            } else if (idata['arrayList'].length == 2) {
                                //1+1模式
                                tpl = $('#Tpl_item_2').html();
                            }
                            var itemdata = self.buildItemData(idata);
                            html += Mustache.render(tpl, itemdata);
                            $(this).html(html);
                            return false;
                        }
                    });
                } else if(list[i].areaFlag=="bargain") { //砍价
                    self.ele.bargain.each(function () {

                        var index = $(this).index();
                        $(this).addClass("activity");
                        if (list[i].displayIndex == index) {
                            idata = list[i];
                        }
                        if(idata) {
                            var html = '', tpl;
                            html += self.addActivityTitle(idata);
                            if (idata['arrayList'].length == 3) {
                                //1+2模块
                                tpl = $('#Tpl_item_3').html();

                            } else if (idata['arrayList'].length == 1) {
                                //单张图片模块
                                tpl = $('#Tpl_item_1').html();
                            } else if (idata['arrayList'].length == 2) {
                                //1+1模式
                                tpl = $('#Tpl_item_2').html();
                            }
                            var itemdata = self.buildItemData(idata);
                            html += Mustache.render(tpl, itemdata);
                            $(this).html(html);
                            return false;
                        }
                    });
                }
            }

        }
    },
    addActivityTitle:function(data){
        var html="",
            tpl = $('#activity_title').html();
        if(data) {
            data["timeIndex"] = 0;
            if(data.arrayList&&data.arrayList.length>0) {
                data["IsStart"] = data.arrayList[0].IsStart;
            }
            data["title"]="未知活动";
            if (data.arrayList.length > 0 ) {
                data["timeIndex"] = data.arrayList[0].RemainingSec>0 ?data.arrayList[0].RemainingSec :0 ;
                if(data.arrayList[0].areaFlag=="secondKill"){
                    data["title"]="限时抢购";
                } else if(data.arrayList[0].areaFlag=="groupBuy"){
                    data["title"]="疯狂团购";
                }else if(data.arrayList[0].areaFlag=="hotBuy"){
                    data["title"]="热销榜单";
                }  else if(data.arrayList[0].areaFlag=="bargain"){
                    data["title"]="砍价";
                }
            }

            html= Mustache.render(tpl, data);
        }

        return html;
    },
    renderProductList:function(data) {
        //debugger;

        var self = this;

        var list = data || [];
        if (list.length) {
            self.ele.productList.each(function () {
                var html = '', tpl;

                var index = $(this).index();
                $(this).addClass("productList");
                var idata = null;
                for (var j = 0; j < list.length; j++) {
                    if (list[j].displayIndex == index) {
                        idata = list[j];
                        break
                    }
                }
                if (idata) {
                    html += self.addTitle(idata);
                    var length = 0;
                    /*  showCount: 6 //商品个数
                     showDiscount: 1 显示商品折扣
                     showName: 1  显示商品名称
                     showPrice: 1 显示商品原价
                     showSalesPrice: 1  显示商品售价
                     showSalesQty: 1 显示销量 */
                    var itemList = [];
                    if (idata['styleType'] == "s1") {
                        tpl = $('#product_item_1').html();
                        //大图
                        for (var i = 0; i < idata['CategoryAreaList'].length; i++) {
                            itemList = [];
                            var itemDateInfo=idata['CategoryAreaList'][i];
                            itemList.push(idata['CategoryAreaList'][i]);
                            if(itemDateInfo.SalesPrice>=itemDateInfo.Price||itemDateInfo.DiscountRate=="10.0折"||itemDateInfo.DiscountRate==0){
                                itemList[0]["hideDiscount"]="hideDiscount"
                            }   //售价大于原价
                            html += Mustache.render(tpl, itemList);
                        }
                    } else if (idata['styleType'] == "s2") {
                        //小图
                        tpl = $('#product_item_2').html();
                        for (var i = 0; i < idata['CategoryAreaList'].length; i = i + 2) {
                            itemList = [];
                            var itemDateInfo=idata['CategoryAreaList'][i];
                            itemList.push(idata['CategoryAreaList'][i]);
                            if(itemDateInfo.SalesPrice>=itemDateInfo.Price||itemDateInfo.DiscountRate=="10.0折"||itemDateInfo.DiscountRate==0){
                                itemList[0]["hideDiscount"]="hideDiscount"
                            }   //售价大于原价

                            if (idata['CategoryAreaList'][i + 1]) {
                                itemList.push(idata['CategoryAreaList'][i + 1]);
                                html += Mustache.render(tpl, itemList);
                            } else {
                                tpl = $('#product_item_2_1').html();
                                html += Mustache.render(tpl, itemList);
                            }
                        }
                    } else if (idata['styleType'] == "s3") {
                        //一大两小
                        tpl = $('#product_item_3').html();
                        for (var i = 0; i < idata['CategoryAreaList'].length; i = i + 3) {
                            itemList = [];
                            var itemDateInfo=idata['CategoryAreaList'][i];
                            itemList.push(idata['CategoryAreaList'][i]);
                            if(itemDateInfo.SalesPrice>=itemDateInfo.Price||itemDateInfo.DiscountRate=="10.0折"||itemDateInfo.DiscountRate==0){
                                itemList[0]["hideDiscount"]="hideDiscount"
                            }   //售价大于原价

                            if (idata['CategoryAreaList'][i + 2] && idata['CategoryAreaList'][i + 1]) {
                                itemList.push(idata['CategoryAreaList'][i + 1]);
                                itemList.push(idata['CategoryAreaList'][i + 2]);
                            } else if (idata['CategoryAreaList'][i + 1]) {
                                itemList.push(idata['CategoryAreaList'][i + 1]);
                                tpl = $('#product_item_3_1').html();
                            } else {
                                tpl = $('#product_item_3_2').html();

                            }
                            html += Mustache.render(tpl, itemList);
                        }
                    } else if (idata['styleType'] == "s4") {
                        //详细列表
                        tpl = $('#product_item_4').html();
                        for (var i = 0; i < idata['CategoryAreaList'].length; i++) {
                            itemList = [];
                            var itemDateInfo=idata['CategoryAreaList'][i];
                            itemList.push(idata['CategoryAreaList'][i]);
                            if(itemDateInfo.SalesPrice>=itemDateInfo.Price||itemDateInfo.DiscountRate=="10.0折"||itemDateInfo.DiscountRate==0){
                                itemList[0]["hideDiscount"]="hideDiscount"
                            }   //售价大于原价
                            html += Mustache.render(tpl, itemList);
                        }
                    }

                    $(this).html("<div class='imagePanel'>" + html + "</div>");

                    /*  showDiscount: 1 显示商品折扣
                     showName: 1  显示商品名称
                     showPrice: 1 显示商品原价
                     showSalesPrice: 1  显示商品售价
                     showSalesQty: 1 显示销量 */
                    if (idata['showName'] == 0) {
                        $(this).find(".name").addClass("hide");  //名称
                    }
                    if (idata['showSalesPrice'] == 0) {
                        $(this).find(".Price").addClass("hide");  //售价
                    }
                    if (idata['showPrice'] == 0) {
                        $(this).find(".original").addClass("hide");//原价
                    }
                    if (idata['showDiscount'] == 0) {
                        $(this).find(".discount").addClass("hide");//折扣
                    }
                    if (idata['showSalesQty'] == 0) {
                        $(this).find(".sales").addClass("hide");//销量
                    }
                    $(".discount").each(function() {
                        if (!$(this).html()) {
                            $(this).addClass("hide");
                        }
                    });

                }
            });


        }

    },
    renderSearchList:function(data){
        
        this.ele.SearchDiv.html($('#tpl_search_item').html());
        if(!data){
            $("#seach").hide();return;
        }
        //debugger;
        if(data.styleType=="s2"){
            $("#search").find(".allClassify");
            $("#search").find(".commonSearchBox").css({"margin-left":"0px"});
        } else{
            $("#search").find(".allClassify").show();
            if(data.show=="logo"){
                var url="url("+data.imageUrl+")";
                $("#seach").find(".allClassify").css("background-image",url)
            }
        }
        $("#search").parent(".action").css({"margin":0});
    },
	initImageScrollView:function(datas){
		
		var me = this,htmls = [], bars = [], imglen = datas.length, href = '';
		  me.ele.adList.html($("#tpl_scroll_item").html());
		for(var i in datas){
		
			if(datas[i]['ObjectTypeID'] == 3){

				href = datas[i]['Url'];

				if(datas[i]['Url']&&datas[i]['Url'].indexOf('http://') == -1){

					href = 'http://'+href;
				}

			}else if(datas[i]['ObjectTypeID'] == 2) {

                href = "javascript:Jit.AM.toPage('GoodsDetail','goodsId=" + datas[i]['ObjectID'] + "')";
                //href = "javascript:Jit.AM.toPage('GoodsList','goodsType="+datas[i]['ObjectID']+"')"
            }else if(datas[i]['ObjectTypeID']==1||datas[i]['ObjectTypeID']==4){
                    //分类

                    href = "javascript:Jit.AM.toPage('GoodsList','goodsType="+datas[i]['ObjectID']+"')";


            }else {
                href = "javascript:void(0)";

            }


			
			htmls.push('<li><a href="'+href+'"><img style="max-width:100%;max-height:100%;" src="'+datas[i]['ImageUrl']+'"></a></li>');
			//htmls.push('<li><a><img src="/HtmlApps/images/common/misspic.png"></a></li>');
			
			if(i==0){
				
				bars.push('<em class="on"></em>');
				
			}else{
			
				bars.push('<em></em>');
			}
		}
		
		$('.picList').html(htmls.join(''));
		
		$('.dot').html(bars.join(''));
		
		var goodsScroll = $('#PicScroll'),
            menuList = $('.dot em');
		
        //重新設置大小
        ReSize();
			
		function ReSize() {
			
			goodsScroll.find('.picList').css({
			
				width: (goodsScroll.width()) * goodsScroll.find('.picList li').size() + 'px'
			});

			goodsScroll.find('.picList li').css({
			
				width: (goodsScroll.width()) + 'px'
			});
		}
		if(datas.length>1){


		me.ImgScroll = new iScroll('PicScroll', {
			snap: true,
			momentum: false,
			vScroll:false,
			hScrollbar: false,
			vScrollbar: false,
			onScrollEnd: function() {
				
				if (this.currPageX > (menuList.size() - 1)) {
				
					return false;
				};
				
                menuList.removeClass('on');
				
                menuList.eq(this.currPageX).addClass('on');
			}
		});
		
		$(window).resize(function(){
		
			ReSize();
			
			me.ImgScroll.refresh();
		});

		var scrollnum = 0;

		setInterval(function(){

			scrollnum++;

			me.ImgScroll.scrollToPage(scrollnum%imglen);

		},3000);
        }else{
            menuList.hide();
        }
	},
    addTitle:function(data){
        var html="",
			tpl = $('#title_1').html();
        if(data&&data.titleName) {
            if (data.titleStyle && data.titleStyle == "left") {
                html = Mustache.render(tpl, data);
            } else if (data.titleStyle && data.titleStyle == "center") {
                tpl = $('#title_2').html();
                html = Mustache.render(tpl, data);
            }
        }

        return html;
    },
    renderEntranceList:function(data){
        var me = this;

        var itemlists = data;

        var tpl = $('#Tpl_entrance').html();

        var itemdata = me.buildItemData(itemlists);

        var html= me.addTitle(data);

        for (var i = 0; i < itemdata.length; i++) {
            html += Mustache.render(tpl, itemdata[i]);
        }
       me.ele.entranceList.html(html).addClass("navPicArea");

    },

    loadCategoryList: function (data) {//加载c区模块
		
		var me = this;
		
        var itemlists = data;

        var html = '',tpl;

        for (var i = 0; i < itemlists.length; i++) {
              html+=me.addTitle(itemlists[i]);
        	if(itemlists[i]['ModelTypeId'] == 3){
        		//1+2模块
        		tpl = $('#Tpl_item_3').html();

        	}else if(itemlists[i]['ModelTypeId'] == 1){
        		//单张图片模块
        		tpl = $('#Tpl_item_1').html();
        	}else if(itemlists[i]['ModelTypeId']==2){
        		//1+1模式
        		tpl = $('#Tpl_item_2').html();
        	}
            
			var itemdata = me.buildItemData(itemlists[i]);

            html += Mustache.render(tpl, itemdata);
        }

        $('#shopList').append(html);
    },
	
	buildItemData:function(data){
		//单项排序
        var itemdata;
        if( data.CategoryAreaList) {
            itemdata = data.CategoryAreaList.sort(function (A, B) {

                if (A.DisplayIndex > B.DisplayIndex) {
                    return 1;

                } else {

                    return -1;
                }

            });
        }else if(data.arrayList){
            itemdata = data.arrayList.sort(function (A, B) {

                if (A.DisplayIndex > B.DisplayIndex) {
                    return 1;

                } else {

                    return -1;
                }

            });
        }
		for(var i=0;i<itemdata.length;i++){

			if(itemdata[i].TypeID==1){
				//分类


                if(itemdata[i]['ObjectID']){
                    itemdata[i]['href'] = "javascript:Jit.AM.toPage('GoodsList','goodsType="+itemdata[i]['ObjectID']+"')";
                }
                if(itemdata[i]['ItemID']){
                    //itemdata[i]['href'] = "javascript:Jit.AM.toPage('GoodsDetail','goodsId="+itemdata[i]['ItemID']+"')";
                    itemdata[i]['href'] = "javascript:Jit.AM.toPage('ActivityGoodsList','eventTypeId="+itemdata[i]['TypeID']+"&eventId="+itemdata[i]['ItemID']+"')";
                }
			}else if(itemdata[i].TypeID==2){
				//商品
                if(itemdata[i]['ObjectID']){
                    itemdata[i]['href'] = "javascript:Jit.AM.toPage('GoodsDetail','goodsId="+itemdata[i]['ObjectID']+"')";
                }
                if(itemdata[i]['ItemID']){
                    //itemdata[i]['href'] = "javascript:Jit.AM.toPage('GoodsDetail','goodsId="+itemdata[i]['ItemID']+"')";
					itemdata[i]['href'] = "javascript:Jit.AM.toPage('ActivityGoodsList','eventTypeId="+itemdata[i]['TypeID']+"&eventId="+itemdata[i]['ItemID']+"')";
                }


            }else if(itemdata[i].TypeID==3){
                if(itemdata[i].url) {
                    if (itemdata[i].url&&itemdata[i].url.indexOf("http://") == -1) {
                        itemdata[i]['href'] = "http://" + itemdata[i].url;
                    } else {
                        itemdata[i]['href'] = itemdata[i].url;
                    }
                }
                if(itemdata[i]['ItemID']){
                    //itemdata[i]['href'] = "javascript:Jit.AM.toPage('GoodsDetail','goodsId="+itemdata[i]['ItemID']+"')";
                    itemdata[i]['href'] = "javascript:Jit.AM.toPage('ActivityGoodsList','eventTypeId="+itemdata[i]['TypeID']+"&eventId="+itemdata[i]['ItemID']+"')";
                }


            }else if(itemdata[i].TypeID==4){
                if(itemdata[i].url) {
                    if (itemdata[i].url&&itemdata[i].url.indexOf("http://") == -1) {
                        itemdata[i]['href'] = "http://" + itemdata[i].url;
                    } else {
                        itemdata[i]['href'] = itemdata[i].url;
                    }
                }
                if(itemdata[i]['ItemID']){
                    //itemdata[i]['href'] = "javascript:Jit.AM.toPage('GoodsDetail','goodsId="+itemdata[i]['ItemID']+"')";
                    itemdata[i]['href'] = "javascript:Jit.AM.toPage('bargaingoodslist','eventTypeId="+itemdata[i]['TypeID']+"&eventId="+itemdata[i]['ItemID']+"')";
                }


            }  else if(itemdata[i].TypeID==31||itemdata[i].TypeID==32||itemdata[i].TypeID==33||itemdata[i].TypeID==37||itemdata[i].TypeID==34){

                itemdata[i]['href']="javascript:Jit.AM.toPage('"+itemdata[i].url+"')";
            }

            else if(itemdata[i].TypeID==8){
                // 全部分类
                itemdata[i]['href']="javascript:Jit.AM.toPage('Category')";
            }else if(itemdata[i].TypeID==99){
                // 全部分类
                itemdata[i]['href']="javascript:void(0)";
            }
		}

		return itemdata;
	}
});