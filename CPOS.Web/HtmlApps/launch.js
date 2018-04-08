document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
	WeixinJSBridge.call('hideOptionMenu');
	WeixinJSBridge.call('hideToolbar');
});

//js库的路径配置
require.config({
	baseUrl: "../../../",
	paths: {
	　　'jquery':'lib/jquery-1.8.3.min',
		'zepto':'lib/zepto.min',
		'weixin':'lib/WeiXin',
		'jit':'lib/jit-lib'
　　}
});

(function(){
	function getUrlParam(key) {
		var value = "",
			itemarr = [],
			urlstr = window.location.href.split("?");
		if (urlstr[1]) {
			var item = urlstr[1].split("&");
			for (i = 0; i < item.length; i++) {
				itemarr = item[i].split("=");
				if (key == itemarr[0]) {
					value = itemarr[1];
				}
			}
		}
		return value;
	}
	
	function ajax(url){
		url = encodeURI(url);
		var xhr=new XMLHttpRequest();
		if(xhr!=null){
			xhr.open('GET',url,false);
			xhr.send(null);
			if(xhr.status==200 || xhr.status==0){
				return eval( '(' + xhr.responseText + ')' );
			}
		}
	}
	
	var isGloble = getUrlParam('isGloble'),
		CID = (isGloble?'_globle':getUrlParam('customerId')),
		cfg = ajax('/HtmlApps/version/'+CID+'.js?v'+Math.random()),
		jslib = (cfg.APP_JSLIB?cfg.APP_JSLIB:'zepto'),
		version = (cfg.APP_CACHE?cfg.APP_VERSION:((new Date()).getTime()));
		//version = cfg.APP_VERSION;
	require([jslib,'../../../lib/jit-lib.js?v'+version], function (){
		if(typeof Zepto != 'undefined'){
			$ = Zepto;
		}
		if(typeof jQuery != 'undefined'){
			$ = jQuery;
		}
		
		var oldcfg = Jit.AM.getAppVersion(),
			_needReLoad = false;
		//判断版本号是否有更新，或者缓存未开始（调试状态）来决定是否开启文件重新装载
		if((oldcfg && oldcfg.APP_VERSION != cfg.APP_VERSION) || cfg.APP_CACHE === false){
			_needReLoad = true;
		}
		//记录此版本
		Jit.AM.setAppVersion(cfg);
		
		if(isGloble){
			Jit.AM.setAppParam('isGloble','true');
		}else{
			Jit.AM.setAppParam('isGloble','false');
		}
		
		//true:重新装载，false:不重新装载（config.js）
		Jit.AM.checkAppPageConfig(_needReLoad);
		
		require(['/HtmlApps/main.js?v'+version]);
		
		require(['/HtmlApps/weixin.js?v='+version], function (res) {
            WeiXin = res.obj;
        });
	});
	
})()
