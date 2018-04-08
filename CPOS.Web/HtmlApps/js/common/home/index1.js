Jit.AM.defindPage({

    name: 'spaIndex',
    onPageLoad: function() {
        //当页面加载完成时触发
        Jit.log('页面进入' + this.name);
        this.initEvent();
        var param=this.pageParam;
        //初始化页面内容
		if(param) {

			//设置背景图
			if (param.backgroundImg) {
				$("#theBg").attr("src", param.backgroundImg);
			}
			//菜单项
			if (param.links) {
				var navs = $("#menu a").hide();
				var length = param.menuLength || param.links.length;
				for (var i = 0; i < length; i++) {
					var item = param.links[i];
					var doma = $(navs[i]).show();
					if (param.ColorSchemes && param.ColorSchemes.bg) {
						doma.css({
							"background": param.ColorSchemes.bg.colorRgb(),
							"color": param.ColorSchemes.color
							//"opacity":0.6
						});
					}
					var urlObj=JSON.parse(item.toUrl.replace(/&&/g,","));
					if(urlObj.typeid==3){
						if(urlObj.url.indexOf("http://")==-1&&urlObj.url.indexOf("https://")==-1){
							item.toUrl="http://"+urlObj.url
						} else{
							item.toUrl=urlObj.url
						}


					}else if(urlObj.typeid==2){
						item.toUrl="javascript:Jit.AM.toPage('GoodsDetail','goodsId="+urlObj.id+"')"
					}
					else if(urlObj.typeid==1){ //分类
						item.toUrl="javascript:Jit.AM.toPage('GoodsList','goodsType="+urlObj.id+"')";//商品分类id
					}else{
						item.toUrl="javascript:void(0)";
					}
					doma.html(item.title);
					doma.attr("href", item.toUrl);

				}
			}
		}
			$(".menu a").addClass("tm");
    }, //加载数据
    //绑定事件
    initEvent: function() {
       
    }
	
});
