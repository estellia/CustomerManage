(function($){
	var alading={
		options : {
			searchItem : "", //搜索关键字
			pageIndex : 0,
			pageSize : 10,
			isAppend : false, //是否是追加
			pageCount : 1 //总页数
		},
		onPageLoad : function() {
			this.initPage();
			this.initEvent();
		},
		initEvent : function() {
			var sUserAgent= navigator.userAgent.toLowerCase();  
			var bIsIpad= sUserAgent.match(/ipad/i) == "ipad";  
			var bIsIphoneOs= sUserAgent.match(/iphone os/i) == "iphone os";  
			var bIsAndroid= sUserAgent.match(/android/i) == "android";  
			var me = this;
			$(window).scroll(function() {
				if (me.reachBottom(50)) {
					me.options.isAppend=true;
					me.scrollLoad();
				}
			});
			//搜索事件
			$("#search").bind("tap", function() {
				//搜索的内容
				var searchText=$("#searchText").val();
				if(me.options.searchItem!=searchText){
					me.options.pageIndex=0;
					me.options.isAppend=false;
					me.options.searchItem =searchText ;
					me.getData();
				}
				
			});
			//安卓和iphone下载
			$("#list").delegate("a", "tap", function(event) {
				var $this = $(this);
				var url = $this.attr("data-downurl");
				if(!$this.hasClass("gray")){
					if(bIsAndroid){
						if($this.hasClass("iphoneBtn")){
							alert("该平台下不支持下载iphone应用");
							return false;
						}else{
							location.href = url;
						}
					}
					location.href = url;
				}else{
					return false;
				}
				
			});

		},
		//滚动加载更多
		scrollLoad : function() {
			//加载更多
			this.options.isAppend = true;
			this.options.pageIndex++;
			if(this.options.pageIndex<this.options.pageCount){
				this.getData();
			}
		},
		//搜索
		search : function() {
			var searchText = $("#searchText").val();
			if (searchText) {
				this.options.searchItem = searchText;
				this.getData();
			}
		},
		//是否到底了
		reachBottom : function(height) {
			if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - (height || 0)) {
				return true;
			}
			return false;
		},
		getData : function() { 
			var me = this;
			var callback = ("callback" + (new Date().getTime()));
			var data = {
				"UserID" : "",
				"Locale" : "",
				"BusinessZoneID" : "",
				"Token" : "",
				"CallBack" : callback,
				"Parameters" : {
					"SearchItem" : me.options.searchItem ? me.options.searchItem : "",
					"PageIndex" : me.options.pageIndex,
					"PageSize" : me.options.pageSize
				}
			}

			$.ajax({
				url : 'http://121.199.42.125:5001/Gateway.ashx?action=GetAppList',
				dataType : "jsonp",
				type : "GET",
				jsonp : "CallBack",  //回调的名称
				data : {
					CallBack:callback,
					ReqContent : JSON.stringify(data)
				},
				jsonpCallback:callback,
				success : function(data) {
					console.log(data);
					if(data.ResultCode=="200"){
						if(data.Data.AppList&&data.Data.AppList.length){
							me.options.pageCount=data.Data.PageCount;
							var html=bd.template("listTpl",data.Data);
							if(me.options.isAppend){
								$("#list").append(html);
							}else{
								$("#list").html(html);
							}
							$("#noresult").hide();
						}else{
							$("#list").html("");
							$("#noresult").show();
						}
					}
				},
				error : function(XMLHttpRequest, textStatus, errorThrown) {
				}
			});
		},
		initPage : function() {
			this.getData();
		}
	}
	alading.onPageLoad();
})(Zepto); 