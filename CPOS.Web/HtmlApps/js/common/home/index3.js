Jit.AM.defindPage({
	name: 'Home',
	elements: {},
	onPageLoad: function() {
		//当页面加载完成时触发
		Jit.log('页面进入' + this.name);
		this.initLoad();
		this.initEvent();
	}, //加载数据
	initLoad: function() {
		var self = this;
		self.elements["homeWrapper"] = $('#homeWrapper');
		self.elements["indexNav"] = $('.indexNav');
		self.onBgSize();
		window.onresize = self.onBgSize;
        var param=this.pageParam;
		var maxWidth=document.body.offsetWidth>640 ? 640:document.body.offsetWidth;
		var w = maxWidth*0.2455;
		if(param.links){
			var length=param.menuLength||param.links.length,finalLength=length;

			var html="";
			var logoList=[
				"../../../images/common/home_default/images4/logoIcon.png",
				"../../../images/common/home_default/images4/zxdtIcon.png",
				"../../../images/common/home_default/images4/hdzqIcon.png",
				"../../../images/common/home_default/images4/xkzsIcon.png"
			];
			for(var i=0;i<length;i++){

				var item=param.links[i];

				if(!param.links[i].title){finalLength--;continue;}
				var style="'";
				html+="<li>";
				debugger;
				var str="rgba(0,0,0,0.6)"
				if(param.colorSchemes&&param.colorSchemes.bg) {
					str=  param.colorSchemes.bg.colorRgb();
				}
				if(param.colorSchemes&&param.colorSchemes.bg) {
					style+= "color:"+ param.colorSchemes.color+";";
				}
				if(item.backgroundImg){
					style+="background:"+str+" url("+item.backgroundImg+") no-repeat center 32px;"+"background-size:"+(w-5)+"px 188px";
				}else{
					var imgUrl=Math.floor(Math.random(3)*3);
					style+="background:"+str+" url("+logoList[imgUrl]+") no-repeat center 32px;"+"background-size:"+(w-5)+"px  188px";
				}
				style+="'";



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
				html+="<a href="+item.toUrl+" style="+style+"><span class='tit'>"+item.title+"</span></a></li>";
			}
			//每个li是74宽度

			$(".navList").css({"width":finalLength*(maxWidth/4+10)});
			$("#list").html(html);
			$(".navList li").css({"width":maxWidth/4});
		}
		//设置背景
		var homeBg = $('.bgImgWrap img'),
			bgList =[
				{
					imgUrl:'../../../images/special/europe/indexBG.jpg'
				},
				{
					imgUrl:'../../../images/special/europe/indexBg2.jpg'
				},
				{
					imgUrl:'../../../images/special/europe/indexBg1.jpg'
				}
			];
		//设置多个背景图
		if(param.backgroundImgArr&&param.backgroundImgArr[0].imgUrl){
			bgList=param.backgroundImgArr;
		}
		homeBg.each(function(i) {
			$(this).attr('src', bgList[i].imgUrl);
		});
		 self.onBgImgSlider();
		 
		self.onMenuSlider();
		
	},
	onBgSize: function() {
		var w = document.body.offsetWidth,
			self = this;
		self.elements.homeWrapper.find('li').css('width', w);
		$(self.elements.homeWrapper.find('li')[0]).addClass("tm2");
	},
	//背景图片滑动
	onBgImgSlider: function() {
		var myScroll = new iScroll('homeWrapper', {
			snap: true,
			momentum: false,
			hScrollbar: false
		});
        //myScroll. scrollToElement("li:nth-child(0)",100);
        var i=0;
        setInterval(function() {
            console.debug(i) ;
              i<$("#homeWrapper li").length?i++:i=0;
            var el="li:nth-child(" +i+")";
            myScroll. scrollToElement(el);
        },2000);

	},
	//菜单可以左右滑动
	onMenuSlider:function(){
		var myScroll = new iScroll('menuWrapper', {
			snap:"li",
			momentum: true,
			hScroll: true
		});
		this.elements.indexNav.find("a").addClass("tm");
	},
	//绑定事件
	initEvent: function() {
		var self = this;

	}
});